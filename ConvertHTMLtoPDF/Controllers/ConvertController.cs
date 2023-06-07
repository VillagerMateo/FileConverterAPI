using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using System.Diagnostics;
using System.IO;
using ConvertHTMLtoPDF.Models;

namespace ConvertHTMLtoPDF.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ConvertController : ControllerBase
    {
        [HttpPost("Convert")]
        public async Task<ActionResult> HtmlToPdf([FromBody] LocalPathModel htmlFilePath)
        {
            byte[] byteArr;
            if (htmlFilePath.LocalPath.Substring(htmlFilePath.LocalPath.Length - 4) == "html")
            {
                Console.WriteLine(htmlFilePath.LocalPath);

                var pdfFilePath = htmlFilePath.LocalPath.Replace(".html", ".pdf");

                var html = System.IO.File.ReadAllText(htmlFilePath.LocalPath);

                var pdfOptions = new PuppeteerSharp.PdfOptions();
                pdfOptions.Format = PuppeteerSharp.Media.PaperFormat.A4;

                using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true
                }))
                {
                    using (var page = await browser.NewPageAsync())
                    {
                        await page.SetContentAsync(html);
                        //await page.PdfAsyn(pdfFilePath, pdfOptions);
                        byteArr = await page.PdfDataAsync(pdfOptions);
                    }
                }

                return File(byteArr, "application/pdf", "pdf123.pdf");
            }
            else
                return BadRequest("Link must end with 'html'.");
        }



    }
}
