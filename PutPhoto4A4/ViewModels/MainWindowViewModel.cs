using Livet;
using OpenCvSharp.CPlusPlus;
using PutPhoto4A4.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PutPhoto4A4.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region コンストラクタ
        public MainWindowViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                //VSデザイナ向けの処理
                Title = "これはテストです";

                InputPhotoList = new ObservableCollection<Photo>(new Photo[]
                {
                    new Photo(@"C:\Windows\Web\Screen\img100.jpg"),
                    new Photo(@"C:\Windows\Web\Screen\img101.jpg"),
                    new Photo(@"C:\Windows\Web\Screen\img102.jpg")
                });
            }
        }
        #endregion

        #region プロパティ

        #region Title
        private string _Title;
        /// <summary>
        /// 印刷物上部に表示するタイトル
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set
            {
                if (_Title != value)
                {
                    _Title = value;
                    System.Diagnostics.Debug.WriteLine(value.ToString());
                    RaisePropertyChanged();
                }
            }
        }
        #endregion


        #region InputPhotoList
        private ObservableCollection<Photo> _InputPhotoList;
        public ObservableCollection<Photo> InputPhotoList
        {
            get { return _InputPhotoList; }
            set
            {
                if(_InputPhotoList!=value)
                {
                    _InputPhotoList = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion
        #endregion
    }
}