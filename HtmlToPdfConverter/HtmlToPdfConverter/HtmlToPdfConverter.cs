using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using iTextSharp.text.html.simpleparser;

namespace HtmlToPdfConverter
{
    public class HtmlToPdf
    {
        public PdfPageSize PageSize { get; set; }

        public PdfPageMargins PageMargins { get; set; }

        public bool IsPrintPageNumber { get; set; }

        public string PageBottomLeftCornerText { get; set; }

        public int PageBottomLeftCornerTextFontSize { get; set; }

        public int LastPageNumber { get; set; }

        public int TotalPages { get; set; }

        public int CountOfAdditionalPages { get; set; }

        public string FooterCenterText { get; set; }

        public HtmlToPdf()
        {
            this.PageSize = new PdfPageSize() { Width = 595, Height = 792 };

            this.PageMargins = new PdfPageMargins() { Left = 35f, Right = 35f, Top = 30f, Bottom = 60f };

            this.IsPrintPageNumber = true;

            this.PageBottomLeftCornerText = "";

            this.PageBottomLeftCornerTextFontSize = 8;

            this.CountOfAdditionalPages = 0;
        }

        public HtmlToPdf(string pageBottomLeftCornerText, int pageBottomLeftCornerTextFontSize, int countOfAdditionalPages)
        {
            this.PageSize = new PdfPageSize() { Width = 595, Height = 792 };

            this.PageMargins = new PdfPageMargins() { Left = 35f, Right = 35f, Top = 30f, Bottom = 60f };

            this.IsPrintPageNumber = true;

            this.PageBottomLeftCornerText = pageBottomLeftCornerText;

            this.PageBottomLeftCornerTextFontSize = pageBottomLeftCornerTextFontSize;

            this.CountOfAdditionalPages = countOfAdditionalPages;
        }

        public HtmlToPdf(PdfPageSize pageSize, PdfPageMargins pageMargins, bool isPrintPageNumber, string pageBottomLeftCornerText, int pageBottomLeftCornerTextFontSize,
            int countOfAdditionalPages)
        {
            this.PageSize = pageSize;

            this.PageMargins = pageMargins;

            this.IsPrintPageNumber = isPrintPageNumber;

            this.PageBottomLeftCornerText = pageBottomLeftCornerText;

            this.PageBottomLeftCornerTextFontSize = pageBottomLeftCornerTextFontSize;

            this.CountOfAdditionalPages = countOfAdditionalPages;
        }

        public void WriteToPdf(string html, string targetFilePhysicalPath, int countOfHeaderRows = 0)
        {
            Byte[] bytes = null;

            StringBuilder sb = new StringBuilder();
            sb.Append(html);

            StringReader sr = new StringReader(sb.ToString());

            Rectangle ps = new Rectangle(this.PageSize.Width, this.PageSize.Height);

            Document pdfDoc = new Document(ps, this.PageMargins.Left, this.PageMargins.Right, this.PageMargins.Top, this.PageMargins.Bottom);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                writer.PageEvent = new MyPdfPageEventHandler();

                (writer.PageEvent as MyPdfPageEventHandler).IsPrintPageNumber = this.IsPrintPageNumber;
                (writer.PageEvent as MyPdfPageEventHandler).PageBottomLeftCornerText = this.PageBottomLeftCornerText;
                (writer.PageEvent as MyPdfPageEventHandler).PageBottomLeftCornerTextFontSize = this.PageBottomLeftCornerTextFontSize;
                (writer.PageEvent as MyPdfPageEventHandler).CountOfAdditionalPages = this.CountOfAdditionalPages;
                (writer.PageEvent as MyPdfPageEventHandler).FooterCenterText = this.FooterCenterText;

                pdfDoc.Open();

                var pages = HTMLWorker.ParseToList(sr, new StyleSheet());

                foreach (var page in pages)
                {
                    if (page is PdfPTable)
                    {
                        (page as PdfPTable).SplitLate = false;

                        if (countOfHeaderRows > 0)
                            (page as PdfPTable).HeaderRows = countOfHeaderRows;
                    }
                    pdfDoc.Add(page as IElement);
                }

                //htmlparser.Parse(sr);

                pdfDoc.Close();

                this.LastPageNumber = (writer.PageEvent as MyPdfPageEventHandler).PageNumber;
                this.TotalPages = (writer.PageEvent as MyPdfPageEventHandler).TotalPages;

                bytes = memoryStream.ToArray();
                memoryStream.Close();
            }

            System.IO.File.WriteAllBytes(targetFilePhysicalPath, bytes);
        }
    }
}