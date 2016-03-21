using PutPhoto4A4.Models;
using PutPhoto4A4.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PutPhoto4A4.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool assemblyLoaded { get; set; }

        public MainWindow()
        {
            assemblyLoaded = typeof(System.Windows.Interactivity.Interaction) != null;
            InitializeComponent();
        }

        /// <summary>
        /// ロード完了時のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var googlePhotos = new GooglePhotoViewer();
            var vm = new GooglePhotoViewerViewModel();
            googlePhotos.DataContext = vm;
            googlePhotos.ShowDialog();

            if (vm.MatList == null)
            {
                this.Close();
                return;
            }

            var tempPhotoList = new ObservableCollection<Photo>();
            foreach (var item in vm.MatList)
                tempPhotoList.Add(new Photo(item));

            ((MainWindowViewModel)this.DataContext).InputPhotoList = tempPhotoList;
;
        }
    }
}
