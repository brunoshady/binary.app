using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BinaryApp.Model;
using BinaryApp.Utils;
using BinaryApp.ViewModel;

namespace BinaryApp.Services;

public class BinaryWebService
{
    private const string Url = "wss://ws.binaryws.com/websockets/v3?app_id=18951";
    private static BinaryWebService _instance;

    private readonly Logger _logger;
    
    private MainWindowViewModel _mainWindowViewModel;

    private ClientWebSocket _webSocket = new();
    
    public bool IsConnected => _webSocket.State is not (WebSocketState.Closed or WebSocketState.Aborted or WebSocketState.None);
    public bool IsConnecting => _webSocket.State == WebSocketState.Connecting; 

    private BinaryWebService()
    {
        _logger = Logger.GetInstance();
    }

    public static BinaryWebService GetInstance()
    {
        if (_instance == null)
            _instance = new BinaryWebService();

        return _instance;
    }

    public void InitializeViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
    }

    private void UpdateIsConnected()
    {
        _mainWindowViewModel.OnPropertyChanged(nameof(IsConnected));
        _mainWindowViewModel.OnPropertyChanged(nameof(IsConnecting));
    }

    private async Task SendRequestAsync(string data, CancellationToken cancellationToken)
    {
        while (_webSocket.State == WebSocketState.Connecting)
        {
            await Task.Delay(100, cancellationToken).ConfigureAwait(false);
        }

        if (_webSocket.State != WebSocketState.Open)
            throw new Exception("Connection is not open.");

        UpdateIsConnected();
        
        var reqAsBytes = Encoding.UTF8.GetBytes(data);
        var ticksRequest = new ArraySegment<byte>(reqAsBytes);

        await _webSocket
            .SendAsync(ticksRequest, WebSocketMessageType.Text, true, cancellationToken)
            .ConfigureAwait(false);

        _logger.AddLog($"Request sent! {data}", LogType.Debug);
    }

    private async Task StartListenAsync(CancellationToken cancellationToken)
    {
        while (_webSocket.State == WebSocketState.Open)
        {
            var buffer = new ArraySegment<byte>(new byte[1024]);

            WebSocketReceiveResult result;
            do
            {
                result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer.Array!), cancellationToken);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _logger.AddLog("Connection Closed!", LogType.Error);
                    UpdateIsConnected();
                    break;
                }

                var str = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                _logger.AddLog($"Received {str}", LogType.Debug);


                var response = JsonHandler.Deserialize(str);

            } while (!result.EndOfMessage);
        }
    }

    private async Task ConnectAsync(CancellationToken cancellationToken)
    {
        _logger.AddLog($"Connecting to {Url}...");
        	
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
        await _webSocket.ConnectAsync(new Uri(Url), cancellationToken);
        
        _logger.AddLog($"Connection established!");
        UpdateIsConnected();
    }


    private async Task Authorize(CancellationToken cancellationToken)
    {
        var authorize = new {authorize = _mainWindowViewModel.Token};
        var authorizeJson = JsonHandler.Serialize(authorize);

        await SendRequestAsync(authorizeJson, cancellationToken).ConfigureAwait(false);
    }
    
    
    
    
    public void Connect()
    {
        Task.Run(async () =>
        {
            var cancellationToken = new CancellationToken();
            string data = "{\"ticks\":\"1HZ100V\"}";

            _webSocket = new ClientWebSocket();

            await ConnectAsync(cancellationToken).ConfigureAwait(false);
            
            await Authorize(cancellationToken).ConfigureAwait(false);
            
            await SendRequestAsync(data, cancellationToken).ConfigureAwait(false);
            await StartListenAsync(cancellationToken).ConfigureAwait(false);
        });
    }

    public void Disconnect()
    {
        _webSocket.Abort();
        UpdateIsConnected();
    }
}