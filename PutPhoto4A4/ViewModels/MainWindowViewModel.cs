using Livet;
using System;
using System.Collections.Generic;
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

        #endregion
    }
}