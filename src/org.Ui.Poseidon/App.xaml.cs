using org.DataAccess;
using org.Ui.MultiLanguage;
using org.Utils;
using org.Utils.Global;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Runtime;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace org.Ui.Poseidon
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppContext.SetData("System.GC.ConserveMemory", 9);
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;

            LogFactory.Initialize();

            InitializeGlobalExceptionsCatching();

            ConfigContext.Initialize();
            ModuleContext.Initialize();
            DbContext.Initialize();
            LoginAccountContext.Initialize();
            MultiLang.Initialize();
            EmsSolutionContext.Reinitialize();
            ExcelHelper.Initialize();

            var mainWin = new MainWindow();
            mainWin.ShowDialog();
        }

        private void InitializeGlobalExceptionsCatching()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            LogFactory.Error(
                e?.Exception as Exception,
                $"TaskScheduler_UnobservedTaskException{Environment.NewLine}{0}{Environment.NewLine}Observed {1}",
                "",
                e.Observed);

            e.SetObserved();

            MessageBox.Show("TaskScheduler Exception Thrown!");
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogFactory.Error(
                e?.ExceptionObject as Exception,
                $"CurrentDomain_UnhandledException{Environment.NewLine}{{str}}{Environment.NewLine}Terminating {{IsTerminating}}",
                JsonConvert.SerializeObject(sender),
                e?.IsTerminating);

            if (e?.IsTerminating == true)
            {
                SnapShotAsync();
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogFactory.Error(
                e?.Exception,
                $"App_DispatcherUnhandledException{Environment.NewLine}{0}{Environment.NewLine}{1}",
                JsonConvert.SerializeObject(sender),
                e.Dispatcher?.Thread?.Name);

            e.Handled = true;
        }

        private void SnapShotAsync()
        {
            Task.Factory.StartNew(() =>
            {
                //创建与屏幕大小相同的位图对象
                var bmpScreen = new Bitmap(
                    (int)SystemParameters.VirtualScreenWidth,
                    (int)SystemParameters.VirtualScreenHeight,
                    System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                //使用位图对象来创建Graphics的对象
                using (Graphics g = Graphics.FromImage(bmpScreen))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;   //设置平滑模式，抗锯齿
                    g.CompositingQuality = CompositingQuality.HighQuality;  //设置合成质量
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;     //设置插值模式
                    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;   //设置文本呈现的质量
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;    //设置呈现期间，像素偏移的方式

                    //利用CopyFromScreen将当前屏幕截图并将内容存储在bmpScreen的位图中
                    g.CopyFromScreen(0, 0, 0, 0, bmpScreen.Size, CopyPixelOperation.SourceCopy);
                }

                bmpScreen.Save(
                    Path.Combine(
                        AppContext.BaseDirectory,
                        "Logs",
                        "SnapShots",
                        $"{DateTime.Now:yyyyMMdd-HHmmss}.bmp"),
                    ImageFormat.Bmp);
            });
        }
    }

}
