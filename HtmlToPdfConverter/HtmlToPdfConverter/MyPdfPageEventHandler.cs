using System;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HtmlToPdfConverter
{
    public class MyPdfPageEventHandler : PdfPageEventHelper
    {
        public bool IsPrintPageNumber { get; set; }

        public string PageBottomLeftCornerText { get; set; }

        public int PageBottomLeftCornerTextFontSize { get; set; }

        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public int CountOfAdditionalPages { get; set; }

        public string FooterCenterText { get; set; }

        PdfContentByte cb;

        PdfTemplate template;

        BaseFont bf = null;

        DateTime PrintTime = DateTime.Now;

        #region Properties
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        private string _HeaderLeft;
        public string HeaderLeft
        {
            get { return _HeaderLeft; }
            set { _HeaderLeft = value; }
        }
        private string _HeaderRight;
        public string HeaderRight
        {
            get { return _HeaderRight; }
            set { _HeaderRight = value; }
        }
        private Font _HeaderFont;
        public Font HeaderFont
        {
            get { return _HeaderFont; }
            set { _HeaderFont = value; }
        }
        private Font _FooterFont;
        public Font FooterFont
        {
            get { return _FooterFont; }
            set { _FooterFont = value; }
        }
        #endregion

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                template = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
            }
            catch (System.IO.IOException ioe)
            {
            }
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            PageNumber = writer.PageNumber;

            String text = "Page " + PageNumber + " of  ";

            float len = bf.GetWidthPoint(text, 8);

            Rectangle pageSize = document.PageSize;

            cb.SetRGBColorFill(0, 0, 0);

            if (IsPrintPageNumber)
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(pageSize.GetRight(100), pageSize.GetBottom(30));

                cb.ShowText(text);
                cb.EndText();

                cb.AddTemplate(template, pageSize.GetRight(100) + len, pageSize.GetBottom(30));
            }

            cb.BeginText();
            cb.SetFontAndSize(bf, PageBottomLeftCornerTextFontSize);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, PageBottomLeftCornerText, pageSize.GetLeft(130), pageSize.GetBottom(30), 0);
            cb.EndText();

            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.HELVETICA_BOLD).BaseFont, 8);
            ColumnText ct = new ColumnText(writer.DirectContent);
            ct.SetSimpleColumn(new Rectangle(160, 0, (pageSize.Width - 160), 50));

            if (!string.IsNullOrEmpty(FooterCenterText))
            {
                Font f = new Font(FontFactory.GetFont(FontFactory.HELVETICA_BOLD).BaseFont, 8, Font.NORMAL, BaseColor.BLACK);
                Paragraph p = new Paragraph(FooterCenterText, f);
                p.Alignment = Element.ALIGN_CENTER;
                ct.AddElement(p);
                ct.Go();
            }
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            TotalPages = (writer.PageNumber - 1) + CountOfAdditionalPages;

            base.OnCloseDocument(writer, document);
            template.BeginText();
            template.SetFontAndSize(bf, 8);
            template.SetTextMatrix(0, 0);
            template.ShowText("" + (TotalPages));
            template.EndText();
        }
    }
}