using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace tdl界面
{
    public partial class MainWindow : Window
    {
        const string URL_PREFIX = "https://t.me/";
        public ViewModel vm = new();
        public string TdlConfigs = File.ReadAllText("TdlConfigs.txt");

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
                Thread.Sleep(1000);
                if (vm.UrlList.Count == 0)
                {
                    continue;
                }
                string url = vm.UrlList.First();
                if (!url.StartsWith(URL_PREFIX))
                {
                    Log($"错误！「{url}」 不是 TG 链接");
                    vm.UrlList.RemoveAt(0);
                    continue;
                }
                string name = url.Replace(URL_PREFIX, "");
                Log($"下载 {name}");
                RunTdl($"{TdlConfigs} --url {url}");
                vm.UrlList.RemoveAt(0);
                Log($"{name} 下载完成");
            }
        }
        private void Log(string message)
        {
            vm.LogList.Add(message);
            if (vm.LogList.Count == 20) { vm.LogList.RemoveAt(0); }
        }
        private void Paste(object sender, RoutedEventArgs e)
        {
            // 先 Trim，保证最后没有换行，然后加上
            string content = Clipboard.GetText().Trim();
            vm.UrlList.Add(content);
        }
        /// <summary>
        /// 启动 tdl
        /// </summary>
        public void RunTdl(string command)
        {
            ProcessStartInfo startInfo = new()
            {
                FileName = @"D:\tdl\tdl.exe",
                Arguments = "download " + command,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
            };
            Process process = new() { StartInfo = startInfo };
            process.OutputDataReceived += ProcessOutputToUI;
            process.ErrorDataReceived += ProcessOutputToUI;
            vm.Output.Clear();
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }
        private void ProcessOutputToUI(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                string output = e.Data.Replace("[A\u001b[K\u001b[A\u001b[K\u001b[A\u001b[K", "").Replace("##", "#");
                vm.Output.Add(output);
                if (vm.Output.Count == 17) { vm.Output.RemoveAt(0); }
            }
        }
        /// <summary>
        /// 窗口关闭时，结束 tdl 进程
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new()
            {
                FileName = "powershell",
                Arguments = "stop-process -name tdl",
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            Process process = new() { StartInfo = startInfo };
            process.Start();
            process.WaitForExit();
        }

        private void ConfigTdl(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo startInfo = new()
            {
                FileName = "notepad",
                Arguments = "TdlConfigs.txt"
            };
            Process process = new() { StartInfo = startInfo };
            process.Start();
            process.WaitForExit();
            TdlConfigs = File.ReadAllText("TdlConfigs.txt");
        }
    }
}