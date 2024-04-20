namespace tdl界面
{
    public class ViewModel : ViewModelBase
    {
        public ViewModel()
        {
        }
        private AsyncObservableCollection<string> output = [];
        public AsyncObservableCollection<string> Output
        {
            get => output;
            set
            {
                output = value; OnPropertyChanged();
            }
        }
        private AsyncObservableCollection<string> logList = [];
        public AsyncObservableCollection<string> LogList
        {
            get => logList;
            set
            {
                logList = value; OnPropertyChanged();
            }
        }
        private AsyncObservableCollection<string> urlList = [];
        public AsyncObservableCollection<string> UrlList
        {
            get => urlList;
            set
            {
                urlList = value; OnPropertyChanged();
            }
        }
    }
}
