﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;
using Livet;

namespace PutPhoto4A4.ViewModels
{
    public class Photo : ViewModel
    {
        #region コンストラクタ
        public Photo(string uri)
        {
            OriginalMat = new Mat(uri);
        }

        public Photo(Mat input)
        {
            OriginalMat = input;
        }
        #endregion


        #region プロパティ
        #region Title
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                if(_Title!=value)
                {
                    _Title = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged("OutputMat");
                    RaisePropertyChanged("OutputImageSource");
                }
            }
        }
        #endregion


        #region IsCrop
        private bool _IsCrop=false;
        public bool IsCrop
        {
            get { return _IsCrop; }
            set
            {
                if (_IsCrop != value)
                {
                    _IsCrop = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged("OutputMat");
                    RaisePropertyChanged("OutputImageSource");
                }
            }
        }
        #endregion


        #region VerticalAlign
        private VerticalState _VerticalAlign;

        public VerticalState VerticalAlign
        {
            get { return _VerticalAlign; }
            set
            {
                if(_VerticalAlign!=value)
                {
                    _VerticalAlign = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged("OutputMat");
                    RaisePropertyChanged("OutputImageSource");
                }
            }
        }
        
        public enum VerticalState { Top,Center,Bottom }
        #endregion


        #region HorizontalAlign
        private HorizontalState _HorizontalAlign;
        public HorizontalState HorizontalAlign
        {
            get { return _HorizontalAlign; }
            set
            {
                if(_HorizontalAlign!=value)
                {
                    _HorizontalAlign = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged("OutputMat");
                    RaisePropertyChanged("OutputImageSource");
                }
            }
        }

        public enum HorizontalState { Left,Center,Right}
        #endregion


        #region OriginalMat
        public Mat OriginalMat { get; }
        #endregion


        #region OutputMat
        public Mat OutputMat
        {
            get
            {
                if (!IsCrop)
                    return OriginalMat;

                Rect rectForCrop;

                if (OriginalMat.Width > OriginalMat.Height)
                {
                    //横が長辺
                    
                    switch(HorizontalAlign)
                    {
                        case HorizontalState.Left:
                            rectForCrop = new Rect(0, 0, OriginalMat.Height, OriginalMat.Height);
                            break;
                        case HorizontalState.Right:
                            rectForCrop = new Rect(OriginalMat.Width - OriginalMat.Height, 0, OriginalMat.Height, OriginalMat.Height);
                            break;
                        default:
                            rectForCrop = new Rect((OriginalMat.Width - OriginalMat.Height) / 2, 0, OriginalMat.Height, OriginalMat.Height);
                            break;
                    }
                }
                else if (OriginalMat.Width < OriginalMat.Height)
                {
                    //縦が長辺

                    switch (VerticalAlign)
                    {
                        case VerticalState.Top:
                            rectForCrop = new Rect(0, 0, OriginalMat.Width, OriginalMat.Width);
                            break;
                        case VerticalState.Bottom:
                            rectForCrop = new Rect(0, OriginalMat.Height - OriginalMat.Width, OriginalMat.Width, OriginalMat.Width);
                            break;
                        default:
                            rectForCrop = new Rect(0, (OriginalMat.Height - OriginalMat.Width) / 2, OriginalMat.Width, OriginalMat.Width);
                            break;
                    }
                }
                else
                {
                    //正方形
                    return OriginalMat;
                }

                return OriginalMat[rectForCrop];
            }
        }
        #endregion


        #region OutputImageSource
        public object OutputImageSource
        {
            get
            {
                return OutputMat.ToBitmapSource();
            }
        }
        #endregion


        #region Width
        public double Width
        {
            get
            {
                var tempMat = OutputMat;

                double longSideLen = ScaleToDouble(Scale);

                if (tempMat.Width > tempMat.Height)
                    return longSideLen;
                else
                    return (longSideLen / (double)tempMat.Height) * (double) tempMat.Width;
            }
        }
        #endregion


        #region Height
        public double Height
        {
            get
            {
                var tempMat = OutputMat;

                double longSideLen = ScaleToDouble(Scale);

                if (tempMat.Width < tempMat.Height)
                    return longSideLen;
                else
                    return (longSideLen /(double)tempMat.Width) * (double)tempMat.Height;
            }
        }
        #endregion


        #region Scale
        private int _Scale = 3;
        /// <summary>
        /// スケールを示す
        /// 1: 2.5cm
        /// 2: 3.0cm
        /// 3: 4.0cm
        /// 4: 5.0cm
        /// 5: 7.0cm
        /// </summary>
        public int Scale
        {
            get { return _Scale; }
            set
            {
                if(_Scale != value)
                {
                    _Scale = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged("Width");
                    RaisePropertyChanged("Height");
                }
            }
        }
        #endregion
        #endregion


        #region メソッド
        public double ScaleToDouble(int s)
        {
            return (double)(new System.Windows.LengthConverter()).ConvertFrom(new string[]
                {
                    "2.5cm",
                    "3.0cm",
                    "4.0cm",
                    "5.0cm",
                    "7.0cm"
                }[s]);
        }
        #endregion
    }
}