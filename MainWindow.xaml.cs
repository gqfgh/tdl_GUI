using System.Windows;
using System.Windows.Controls;

namespace tdl_GUI
{

    public partial class MainWindow : Window
    {
        const string URL_PREFIX = "https://t.me/";
        const string WIN_NEWLINE = "\r\n";
        public MainViewModel vm = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
        }
        private async void AutoDL(object sender, RoutedEventArgs e)
        {
            await Task.Run(StartAutoDL);
        }
        private void StartAutoDL()
        {
            Log("-----自动下载开启-----");
            while (true)
            {
                if (vm.Urls != string.Empty)
                {
                    foreach (var url in vm.Urls.Trim().Split(WIN_NEWLINE))
                    {
                        if (!url.StartsWith(URL_PREFIX))
                        {
                            Log($"错误！「{url}」 不是 TG 链接");
                            vm.Urls = vm.Urls.Replace(url, "").Trim();
                            continue;
                        }
                        string name = url.Replace(URL_PREFIX, "");
                        Log($"下载 {name}");
                        Lib.RunCmd($"{vm.CmdPrefix} --url {url}");
                        vm.Urls = vm.Urls.Replace(url, "").Trim();
                        Log($"{name} 下载完成");
                    }
                }
                Thread.Sleep(1000);
            }
        }
        private void Log(string message)
        {
            vm.Log += message + WIN_NEWLINE;
        }
        private void Paste(object sender, RoutedEventArgs e)
        {
            // 必须用 TrimEnd，不然会把上一行末尾的换行去掉
            vm.Urls += Clipboard.GetText().TrimEnd() + WIN_NEWLINE;
        }
        /// <summary>
        /// TextBox 一直显示最后一行
        /// </summary>
        private void Scroll2End(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.ScrollToEnd();
        }
    }
}