using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using BinaryApp.Command;
using BinaryApp.Services;

namespace BinaryApp.ViewModel
{
    public class MainWindowViewModel : SimpleViewModel
    {
        private readonly Logger _logger;
        private readonly BinaryWebService _binaryWebService;
        
        public ICommand ConnectCommand { get; }

        public ICollectionView CollectionLog => CollectionViewSource.GetDefaultView(_logger?.LogCollection ?? new ObservableCollection<Log>());

        public bool IsConnected => _binaryWebService?.IsConnected ?? false;
        public bool IsConnecting => _binaryWebService?.IsConnecting ?? false;

        public MainWindowViewModel()
        {
            _logger = Logger.GetInstance();
            _binaryWebService = BinaryWebService.GetInstance();
            _binaryWebService.InitializeViewModel(this);

            ConnectCommand = new RelayCommand(ConnectCanExecute, Connect);
                
            InitializeEnvironmentVariables();

            ShowDebug = true;
        }

        private bool _showDebug;
        public bool ShowDebug
        {
            get => _showDebug;
            set
            {
                _showDebug = value;
                ShowDebugChanged();
            }
        }

        private string _token;
        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }

        private bool ConnectCanExecute(object obj)
        {
            if (string.IsNullOrEmpty(_token))
                return false;

            if (IsConnecting)
                return false;

            return true;
        }

        private void Connect(object obj)
        {
            if (IsConnected)
            {
                _logger.AddLog("Disconnecting...");
                _binaryWebService.Disconnect();
            }
            else
            {
                _logger.AddLog("Connecting...");
                _binaryWebService.Connect();
            }
        }

        private void ShowDebugChanged()
        {
            if (ShowDebug)
                CollectionLog.Filter = null; 
            else
                CollectionLog.Filter = x => x is Log log && log.Type != LogType.Debug; 

            CollectionLog.Refresh();
        }

        private void InitializeEnvironmentVariables()
        {
            _logger.AddLog("Searching for token in System Environment...", LogType.Debug);
            var token = Environment.GetEnvironmentVariable("Token");

            if (token == null) 
                return;

            _logger.AddLog($"Token '{token}' found.", LogType.Debug);
            Token = token;
        }
    }
}