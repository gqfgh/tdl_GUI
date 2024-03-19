using System.Diagnostics;

namespace tdl_GUI
{
    internal class Lib
    {
        /// <summary>
        /// 执行命令，等待退出
        /// </summary>
        public static void RunCmd(string command)
        {
            // 创建进程对象
            Process process = new();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/c {command}"; // /c 参数表示执行完命令后关闭命令行窗口
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.CreateNoWindow = false;
            // 启动进程并等待执行完成
            process.Start();
            process.WaitForExit();
        }
    }
}
