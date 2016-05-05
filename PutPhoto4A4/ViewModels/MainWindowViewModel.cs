using Livet;
using Livet.Commands;
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
using System.Windows.Controls;

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
                    new Photo(@"C:\Windows\Web\Screen\img101.png"),
                    new Photo(@"C:\Windows\Web\Screen\img102.jpg")
                });
            }

            this.Description = new DragAcceptDescription();
            this.Description.DragOver += this.OnDragOver;
            this.Description.DragDrop += this.OnDragDrop;
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


        #region PaperTopPhotoList
        public ObservableCollection<Photo> PaperTopPhotoList { get; } = new ObservableCollection<Photo>();
        #endregion


        #region PaperBottomPhotoList
        public ObservableCollection<Photo> PaperBottomPhotoList { get; } = new ObservableCollection<Photo>();
        #endregion


        #region Description
        public DragAcceptDescription Description { get; }
        #endregion


        #region TopRegionHeight
        private double _TopRegionHeight = (double)(new System.Windows.LengthConverter()).ConvertFrom("13.2cm");
        public double TopRegionHeight
        {
            get { return _TopRegionHeight; }
            set
            {
                if(_TopRegionHeight !=value)
                {
                    _TopRegionHeight = value;
                    _BottomRegionHeight = (double)(new System.Windows.LengthConverter()).ConvertFrom("19.75cm") - value;
                    RaisePropertyChanged();
                    RaisePropertyChanged("BottomRegionHeight");
                }
            }
        }
        #endregion


        #region BottomRegionHeight
        private double _BottomRegionHeight = (double)(new System.Windows.LengthConverter()).ConvertFrom("6.58cm");
        public double BottomRegionHeight
        {
            get { return _BottomRegionHeight; }
            set
            {
                if (_BottomRegionHeight!= value)
                {
                    _BottomRegionHeight = value;
                    _TopRegionHeight = (double)(new System.Windows.LengthConverter()).ConvertFrom("19.75cm") - value;
                    RaisePropertyChanged();
                    RaisePropertyChanged("TopRegionHeight");
                }
            }
        }
        #endregion


        #region PaperScale
        private double _PaperScale = 1.0;
        public double PaperScale
        {
            get { return _PaperScale; }
            set
            {
                if(_PaperScale!=value)
                {
                    _PaperScale = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion
        #endregion


        #region コマンド
        #region ChangeImageSizeCommand
        private void ChangeImageSize(Photo obj)
        {
            obj.SizeStateIndex++;
        }
        private ListenerCommand<Photo> _ChangeImageSizeCommand;
        public ListenerCommand<Photo> ChangeImageSizeCommand
        {
            get
            {
                if (_ChangeImageSizeCommand == null)
                    _ChangeImageSizeCommand = new ListenerCommand<Photo>(ChangeImageSize);
                return _ChangeImageSizeCommand;
            }
        }
        #endregion


        #region ChangePlacementCommand
        private int placementStateNum = 1;

        private void ChangePlacement(Photo sender)
        {
            placementStateNum++;
            if (placementStateNum > 2)
                placementStateNum = 0;

            sender.HorizontalAlign = (HorizontalState)Enum.ToObject(typeof(HorizontalState), placementStateNum);
            sender.VerticalAlign = (VerticalState)Enum.ToObject(typeof(VerticalState), placementStateNum);
        }

        private ListenerCommand<Photo> _ChangePlacementCommand;
        public ListenerCommand<Photo> ChangePlacementCommand
        {
            get
            {
                if (_ChangePlacementCommand == null)
                    _ChangePlacementCommand = new ListenerCommand<Photo>(ChangePlacement);
                return _ChangePlacementCommand;
            }
        }
        #endregion


        #region ChangeRotateCommand
        private void ChangeRotate(Photo sender)
        {
            sender.Rotate = (sender.Rotate + 90) % 360;
        }
        private ListenerCommand<Photo> _ChangeRotateCommand;
        public  ListenerCommand<Photo> ChangeRotateCommand
        {
            get
            {
                if (_ChangeRotateCommand == null)
                    _ChangeRotateCommand = new ListenerCommand<Photo>(ChangeRotate);
                return _ChangeRotateCommand;
            }
        }
        #endregion


        #region PrintCommand
        private void Print()
        {
            Printer.Print(this);
        }
        private ViewModelCommand _PrintCommand;
        public ViewModelCommand PrintCommand
        {
            get{ return _PrintCommand ?? (_PrintCommand = new ViewModelCommand(Print)); }
        }
        #endregion
        #endregion


        #region メソッド
        private void OnDragOver(DragEventArgs args)
        {
            if (args.AllowedEffects.HasFlag(DragDropEffects.Move) &&
                args.Data.GetDataPresent(typeof(Photo)))
            {
                args.Effects = DragDropEffects.Move;
            }
        }

        private void OnDragDrop(DragEventArgs args)
        {
            // check and get data
            if (!args.Data.GetDataPresent(typeof(Photo))) return;
            var data = args.Data.GetData(typeof(Photo)) as Photo;
            if (data == null) return;
            var fe = args.OriginalSource as FrameworkElement;
            if (fe == null) return;
            var target = fe.DataContext as Photo;
            if (target == null)
            {
                var tempItemControl = args.Source as ItemsControl;
                if (tempItemControl == null)
                    return;

                /*if (((args.Source as ItemsControl).ItemsSource as ObservableCollection<Photo>).Count != 0)
                    return;*/

                if (PaperBottomPhotoList.Contains(data))
                    PaperBottomPhotoList.Remove(data);
                else if (PaperTopPhotoList.Contains(data))
                    PaperTopPhotoList.Remove(data);
                else
                    InputPhotoList.Remove(data);

                ((args.Source as ItemsControl).ItemsSource as ObservableCollection<Photo>).Add(data);
                return;
            }

            var receiveList = (args.Source as ItemsControl).ItemsSource as ObservableCollection<Photo>;

            if (receiveList.Contains(data))
            {
                //同じリスト内の移動の場合
                var sendDataIndex = receiveList.IndexOf(data);
                var insertIndex = receiveList.IndexOf(target);
                receiveList.Move(sendDataIndex, insertIndex);
            }
            else
            {
                if (PaperBottomPhotoList.Contains(data))
                    PaperBottomPhotoList.Remove(data);
                else if (PaperTopPhotoList.Contains(data))
                    PaperTopPhotoList.Remove(data);
                else
                    InputPhotoList.Remove(data);


                var insertIndex = receiveList.IndexOf(target);
                receiveList.Insert(insertIndex, data);
            }
        }
        #endregion
    }
}