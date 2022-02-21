using System;
using System.Collections.ObjectModel;
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
        
        private string _token;

        public ObservableCollection<Log> LogCollection => _logger?.LogCollection;

        public bool IsConnected => _binaryWebService?.IsConnected ?? false;

        public MainWindowViewModel()
        {
            _logger = Logger.GetInstance();
            _binaryWebService = BinaryWebService.GetInstance();
            _binaryWebService.InitializeViewModel(this);

            ConnectCommand = new RelayCommand(ConnectCanExecute, Connect);
                
            InitializeEnvironmentVariables();
        }

        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }

        private bool ConnectCanExecute(object obj)
        {
            return !string.IsNullOrEmpty(_token);
        }

        private void Connect(object obj)
        {
            if (IsConnected)
            {
                _logger.AddLog("Disconnecting...");
                _binaryWebService.IsConnected = false;
                OnPropertyChanged(nameof(IsConnected));
            }
            else
            {
                _logger.AddLog("Connecting...");
                _binaryWebService.IsConnected = true;
                OnPropertyChanged(nameof(IsConnected));    
            }
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