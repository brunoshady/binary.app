using BinaryApp.Services;
using BinaryApp.ViewModel;

namespace BinaryApp.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            var logger = Logger.GetInstance();
            
            logger.AddLog("Setting DataContext...", LogType.Debug);
            DataContext = new MainWindowViewModel();
            logger.AddLog("Initializing...");
            InitializeComponent();
        }
    }
}