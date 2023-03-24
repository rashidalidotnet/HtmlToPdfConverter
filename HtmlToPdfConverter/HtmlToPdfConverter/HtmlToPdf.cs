using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using iTextSharp.text.html.simpleparser;
using HtmlToPdfConverter.DTO;

namespace HtmlToPdfConverter
{
    public class HtmlToPdf
    {
        public PdfPageOutput WriteToPdf(PdfPageProps pageProps, string targetFilePhysicalPath)
        {
            PdfPageOutput pdfPageOutput = new PdfPageOutput();

            Byte[] bytes = null;

            StringBuilder sb = new StringBuilder();
            sb.Append(pageProps.Html);

            StringReader sr = new StringReader(sb.ToString());

            Rectangle ps = new Rectangle(pageProps.PageSize.Width, pageProps.PageSize.Height);

            Document pdfDoc = new Document(ps, pageProps.PageMargins.Left, pageProps.PageMargins.Right, pageProps.PageMargins.Top, pageProps.PageMargins.Bottom);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                writer.PageEvent = new MyPdfPageEventHandler(pageProps);

                pdfDoc.Open();

                var pages = HTMLWorker.ParseToList(sr, new StyleSheet());

                foreach (var page in pages)
                {
                    if (page is PdfPTable)
                    {
                        (page as PdfPTable).SplitLate = false;

                        if (pageProps.CountOfHeaderRows > 0)
                            (page as PdfPTable).HeaderRows = pageProps.CountOfHeaderRows;
                    }
                    pdfDoc.Add(page as IElement);
                }

                //htmlparser.Parse(sr);

                pdfDoc.Close();

                pdfPageOutput.LastPageNumber = (writer.PageEvent as MyPdfPageEventHandler).PageNumber;
                pdfPageOutput.TotalPages = (writer.PageEvent as MyPdfPageEventHandler).TotalPages;

                bytes = memoryStream.ToArray();
                memoryStream.Close();
            }

            System.IO.File.WriteAllBytes(targetFilePhysicalPath, bytes);

            return pdfPageOutput;
        }
    }
}