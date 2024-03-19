using System.IO;

namespace tdl_GUI
{
    public class MainViewModel : ViewModelBase
    {
        public string CmdPrefixFile = "CmdPrefix.txt";
        public MainViewModel()
        {
            // 变量初始化
            if (File.Exists(CmdPrefixFile))
            {
                CmdPrefix = File.ReadAllText(CmdPrefixFile);
            }
            else
            {
                Log += $"{CmdPrefixFile} 不存在，请创建";
            }
        }
        // 变量声明
        private string urls = string.Empty;
        public string Urls
        {
            get => urls;
            set
            {
                urls = value; OnPropertyChanged();//不用填写字段
            }
        }
        private string cmdPrefix;
        public string CmdPrefix
        {
            get => cmdPrefix;
            set
            {
                cmdPrefix = value;
                OnPropertyChanged();
                File.WriteAllText(CmdPrefixFile, value);
            }
        }
        private string log;
        public string Log
        {
            get => log;
            set
            {
                log = value; OnPropertyChanged();
            }
        }
    }
}
