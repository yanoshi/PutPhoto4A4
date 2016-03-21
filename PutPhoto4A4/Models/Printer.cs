using PutPhoto4A4.ViewModels;
using PutPhoto4A4.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Xps;

namespace PutPhoto4A4.Models
{
    public class Printer
    {
        public static void Print(MainWindowViewModel viewModel)
        {
            //Set up the WPF Control to be printed

            var fixedDoc = new FixedDocument();

            var objReportToPrint = new A4Paper();

            var pageSize = new Size(objReportToPrint.Width, objReportToPrint.Height);

            var reportToPrint = objReportToPrint as UserControl;
            reportToPrint.DataContext = viewModel;

            var pageContent = new PageContent();
            var fixedPage = new FixedPage();

            //Create first page of document
            fixedPage.Children.Add(reportToPrint);
            fixedPage.Measure(pageSize);
            //fixedPage.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));
            fixedPage.Arrange(new Rect(15, 15, pageSize.Height, pageSize.Width));
            fixedPage.UpdateLayout();
            fixedPage.Width = 11.69 * 96;
            fixedPage.Height = 8.27 * 96;

            ((IAddChild)pageContent).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);
            

            SendFixedDocumentToPrinter(fixedDoc);
        }

        private static void SendFixedDocumentToPrinter(FixedDocument fixedDocument)
        {
            var printDialog = new PrintDialog();
            var mediaSize = new PageMediaSize(PageMediaSizeName.ISOA4Rotated);//とりあえず固定値

            printDialog.PrintTicket.PageMediaSize = mediaSize;

            if (printDialog.ShowDialog() == true)
            {
                var pq = printDialog.PrintQueue;

                printDialog.PrintTicket.PageOrientation = PageOrientation.Landscape;
                //printTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4Rotated);
                //printTicket.PageOrientation = PageOrientation.Portrait;

                printDialog.PrintDocument(fixedDocument.DocumentPaginator, "A4");
            }


            
        }
    }
}

