using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.IO.Compression;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;
using System.Drawing;
using PutPhoto4A4.ViewModels;

namespace PutPhoto4A4.Views
{
    /// <summary>
    /// GooglePhotoViewer.xaml の相互作用ロジック
    /// </summary>
    public partial class GooglePhotoViewer : System.Windows.Window
    {
        Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(FEATURE_BROWSER_EMULATION);
        const string FEATURE_BROWSER_EMULATION = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
        string process_name = System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe";
        string process_dbg_name = System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".vshost.exe";

        public GooglePhotoViewer()
        {
            regkey.SetValue(process_name, 11001, Microsoft.Win32.RegistryValueKind.DWord);
            regkey.SetValue(process_dbg_name, 11001, Microsoft.Win32.RegistryValueKind.DWord);

            InitializeComponent();
        }

        ~GooglePhotoViewer()
        {
            regkey.DeleteValue(process_name);
            regkey.DeleteValue(process_dbg_name);
            regkey.Close();
        }

        /// <summary>
        /// ローディング開始時のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowser_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            //良い条件が思いつかなかった(ホントはMIMEで指定したかったが…)
            if (e.Uri.ToString().IndexOf("video.googleusercontent.com") != -1)
            {
                e.Cancel = true;

                WebClient client = new WebClient();

                client.DownloadDataCompleted += new DownloadDataCompletedEventHandler((s2,e2)=> 
                {
                    var matList = new List<Mat>();
                    using (var ms = new System.IO.MemoryStream(e2.Result))
                    {
                        using (var zipArchive = new ZipArchive(ms, ZipArchiveMode.Read))
                        {
                            foreach(var item in zipArchive.Entries)
                            {
                                matList.Add((new Bitmap(item.Open())).ToMat());
                            }
                        }
                    }
                    ((GooglePhotoViewerViewModel)this.DataContext).MatList = matList;

                    this.Close();
                });

                client.DownloadDataAsync(e.Uri);
            }
        }
    }
}
