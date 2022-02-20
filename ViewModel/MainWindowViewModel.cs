namespace BinaryApp.ViewModel
{
    public class MainWindowViewModel : SimpleViewModel
    {
        private string _test;
        private string _test2;

        public string Test
        {
            get => _test;
            set
            {
                SetProperty(ref _test, value);
                Test2 = Test;
            }
        }

        public string Test2
        {
            get => _test2;
            set => SetProperty(ref _test2, value);
        }
    }
}