using Tesseract;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using Patagames.Pdf.Net;
using System.Drawing;
using System.Collections.Concurrent;

sealed class Program
{
    static void Main(string[] args)
    {
        string tessdataDir = "./tessdata/"; // Ensure the Tesseract data files are here
        string pdfDir = "./pdfs/"; // Directory containing PDF files

        try
        {
            // Ensure the PDF directory exists
            if (!Directory.Exists(pdfDir))
            {
                Console.WriteLine($"PDF directory not found: {pdfDir}");
                return;
            }

            // Initialize PDF SDK
            PdfCommon.Initialize();

            Parallel.ForEach(Directory.GetFiles(pdfDir, "*.pdf"), pdfFilePath =>
            {
                Console.WriteLine($"Processing PDF: {pdfFilePath}");

                try
                {
                    // Open the PDF with Patagames.Pdf.Net
                    using (var pdfDoc = PdfDocument.Load(pdfFilePath))
                    {
                        int pageCount = pdfDoc.Pages.Count;

                        // Create a concurrent dictionary to store page results
                        ConcurrentDictionary<int, string> pageResults = new ConcurrentDictionary<int, string>();

                        Parallel.For(0, pageCount, i =>
                        {
                            Console.WriteLine($"Processing PDF Pg {i+1}/{pageCount}: {pdfFilePath}");
                            // Initialize a separate TesseractEngine instance for each page
                            using var engine = new TesseractEngine(tessdataDir, "eng", EngineMode.Default);

                            // Render PDF page to an in-memory image
                            var pdfPage = pdfDoc.Pages[i];
                            using var imageStream = RenderPageToMemory(pdfPage);

                            // Use Tesseract to do OCR on the image
                            using var img = Pix.LoadTiffFromMemory(imageStream.ToArray());
                            using var ocrPage = engine.Process(img);
                            string text = ocrPage.GetText();

                            // Store the result in the concurrent dictionary with the page number
                            pageResults[i] = text;
                            Console.WriteLine($"Done Processing PDF Pg {i+1}/{pageCount}: {pdfFilePath}");
                        });

                        // Sort the results by page number
                        var sortedResults = pageResults.OrderBy(kv => kv.Key).Select(kv => kv.Value);

                        // Create a text file with the sorted accumulated text content and overwrite the existing file
                        string pdfFileName = Path.GetFileNameWithoutExtension(pdfFilePath);
                        string textFilePath = Path.Combine(pdfDir, $"{pdfFileName}.txt");
                        File.WriteAllText(textFilePath, string.Join(Environment.NewLine, sortedResults));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error processing {pdfFilePath}: {e.Message}");
                }
            });
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }

    private static MemoryStream RenderPageToMemory(PdfPage page)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new PlatformNotSupportedException("This application is only supported on Windows.");
        }

        int dpi = 720;
        int width = (int)(page.Width * dpi / 72.0);
        int height = (int)(page.Height * dpi / 72.0);

        using (var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb))
        using (var g = Graphics.FromImage(bitmap))
        {
            IntPtr hdc = g.GetHdc();
            try
            {
                page.Render(hdc, 0, 0, width, height, Patagames.Pdf.Enums.PageRotate.Normal, Patagames.Pdf.Enums.RenderFlags.FPDF_NONE);
            }
            finally
            {
                g.ReleaseHdc(hdc);
            }

            // Create a memory stream and save the image data to it
            MemoryStream imageStream = new MemoryStream();
            bitmap.Save(imageStream, System.Drawing.Imaging.ImageFormat.Tiff);

            return imageStream;
        }
    }
}
