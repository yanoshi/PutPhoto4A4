﻿using System;
using System.Windows;

namespace PutPhoto4A4.Models
{
    public sealed class DragAcceptDescription
    {
        public event Action<DragEventArgs> DragOver;

        public void OnDragOver(DragEventArgs dragEventArgs)
        {
            var handler = DragOver;
            if (handler != null)
            {
                handler(dragEventArgs);
            }
        }

        public event Action<DragEventArgs> DragDrop;

        public void OnDrop(DragEventArgs dragEventArgs)
        {
            var handler = DragDrop;
            if (handler != null)
            {
                handler(dragEventArgs);
            }
        }
    }
}
