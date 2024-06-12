
using Spire.Presentation;
using Spire.Doc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
namespace LXP.Common.Utils
{
    public static class FileConversion
    {
        public static string Conversion(string materialType,string FilePath, IWebHostEnvironment environment, IHttpContextAccessor contextAccessor)
        {
            // Assuming 'pptFilePath' is a relative path like "CourseMaterial/file.pptx"
            string webRootPath = environment.WebRootPath;

            // Correct the file path by removing the URL part and ensuring proper separators
            string correctedFilePath = FilePath.Replace("http://localhost:5199/wwwroot/", "").Replace("/", "\\");
            string fullFilePath = Path.Combine(webRootPath, correctedFilePath);

            // Check if the file exists
            if (!File.Exists(fullFilePath))
            {
                throw new FileNotFoundException($"The file was not found: {fullFilePath}");
            }
            if (materialType== "PPT")
            {
                Presentation presentation = new Presentation();

                // Load the PPT file
                presentation.LoadFromFile(fullFilePath);

                // Define the output PDF file path
                string pdfFilePath = Path.ChangeExtension(fullFilePath, ".pdf");


                // Save the presentation as a PDF
                presentation.SaveToFile(pdfFilePath, Spire.Presentation.FileFormat.PDF);
                string relativePptPdfPath = pdfFilePath.Replace(webRootPath, "").Replace("\\", "/");
                return $"{contextAccessor.HttpContext.Request.Scheme}://{contextAccessor.HttpContext.Request.Host}{contextAccessor.HttpContext.Request.PathBase}/wwwroot{relativePptPdfPath}";


            }
            else if (materialType == "TEXT")
            {
                Document document = new Document();

                // Load the DOCX file
                document.LoadFromFile(fullFilePath);

                // Define the output PDF file path
                string docFilePath = Path.ChangeExtension(fullFilePath, ".pdf");
                // Save the document as a PDF
                document.SaveToFile(docFilePath, Spire.Doc.FileFormat.PDF);
                string relativeDocPdfPath = docFilePath.Replace(webRootPath, "").Replace("\\", "/");
                return $"{contextAccessor.HttpContext.Request.Scheme}://{contextAccessor.HttpContext.Request.Host}{contextAccessor.HttpContext.Request.PathBase}/wwwroot{relativeDocPdfPath}";

            }
            string relativePath = fullFilePath.Replace(webRootPath, "").Replace("\\", "/");
            return $"{contextAccessor.HttpContext.Request.Scheme}://{contextAccessor.HttpContext.Request.Host}{contextAccessor.HttpContext.Request.PathBase}/wwwroot{relativePath}";
            //return fullFilePath.Split(".")[1];
              }


       

    }
}
