using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;

namespace PutPhoto4A4.Models
{
    public class Photo
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
        public string Title { get; set; }
        #endregion


        #region IsCrop
        public bool IsCrop { get; set; } = false;
        #endregion

        #region VerticalAlign
        public VerticalState VerticalAlign { get; set; }
        
        public enum VerticalState { Top,Center,Bottom }
        #endregion

        
        #region HorizontalAlign
        public HorizontalState HorizontalAlign { get; set; }

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

        #endregion
    }
}
