# HTML to PDF converter
A .NET class library to create a PDF file from HTML content using iTextSharp and saves to the given path.

## How to use
The project is a simple class library that can be added to a .NET solution and referenced to the web project.  
The main class is located on the root of the project (HtmlToPdf) with a method (WriteToPdf) that can be called from the web project, it expects an object of PageProps class with properties as per the PDF page requirement including page size, page margins, page footer note, page header text, font size and the HTML that needs to be converted to a PDF file additional to the parameter to pass in the full target path where newly created PDF file needs to be saved including the file name and extension.
Only HTML property is a must to set, others properties have default values defined if values are not supplied.

## Ability
Additional to page size, page margins, page footer note, page header text, font size, the tool have the ability to add page number and time stamp to each page as an optional feature that can turn on/off with flag(s) in the PageProps parameter.
The WriteToPdf method's out have the count of pages created and the number of last page, last page number can be useful when multiple PDF files are needed to be created and then needs to be combined with correct page number sequence.
