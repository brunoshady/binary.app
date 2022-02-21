using BinaryApp.ViewModel;

namespace BinaryApp.Services;

public class BinaryWebService
{
    private const string Url = "wss://ws.binaryws.com/websockets/v3?app_id=18951";
    private static BinaryWebService _instance;
    private MainWindowViewModel _mainWindowViewModel;

    public bool IsConnected { get; set; }
    
    private BinaryWebService() 
    { }

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
}