using Livet;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PutPhoto4A4.ViewModels
{
    public class GooglePhotoViewerViewModel : ViewModel
    {
        #region プロパティ
        public List<Mat> MatList { get; set; }
        #endregion
    }
}
