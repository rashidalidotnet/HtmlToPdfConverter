
namespace HtmlToPdfConverter.DTO
{
    public class PdfPageProps
    {
        public string Html { get; set; }
        public bool IsPrintPageNumber { get; set; } = true;

        public string PageBottomLeftCornerText { get; set; } = string.Empty;

        public int PageBottomLeftCornerTextFontSize { get; set; } = 8;

        public int CountOfAdditionalPages { get; set; } = 0;

        public string FooterCenterText { get; set; }

        public PdfPageSize PageSize { get; set; } = new PdfPageSize() { Width = 595, Height = 792 };

        public PdfPageMargins PageMargins { get; set; } = new PdfPageMargins() { Left = 35f, Right = 35f, Top = 30f, Bottom = 60f };
        public int CountOfHeaderRows { get; set; } = 0;
    }
}
