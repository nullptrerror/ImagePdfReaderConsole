# ImagePdfReaderConsole

This GitHub project provides a command-line utility for converting PDF documents into text files using Optical Character Recognition (OCR) technology. The program leverages the Patagames.Pdf.Net library to extract text from PDF pages and the Tesseract OCR engine for accurate text recognition.

## Features

- Batch processing: The program processes all PDF files in a specified directory.
- Parallel processing: PDF pages are processed concurrently for improved performance.
- OCR capabilities: Utilizes Tesseract OCR to extract text content from PDF pages.
- Page sorting: The extracted text is sorted by page number and saved to a text file.
- Windows support: Currently supported on Windows operating systems.

## Prerequisites

Before you can use this tool, you need to set up the required tools and clone the project.

1. Install Git:F
If you don't have Git installed, you can download and install it from <https://git-scm.com/>.

2. Install .NET SDK:
You'll need the .NET SDK to build and run the project. You can download and install it from <https://dotnet.microsoft.com/download>.

3. Install Visual Studio Code (Optional):
Visual Studio Code is an integrated development environment (IDE) that you can use to work with .NET projects. You can download and install it from <https://code.visualstudio.com/>.

## Clone the Project

To get the PDF to Text OCR Converter project, follow these steps:

1. Open your command-line terminal (e.g., Command Prompt, Git Bash, or PowerShell).

2. Navigate to the directory where you want to clone the project.

3. Run the following command to clone the project repository:

``` bash
git clone https://github.com/nullptrerror/ImagePdfReaderConsole.git

```

## Usage

1. Place your PDF files in the designated "pdfs" directory within the cloned project folder.

2. Ensure that the Tesseract data files are located in the "tessdata" directory within the project folder.

3. Build and run the program as follows

``` bash
cd ImagePdfReaderConsole
dotnet run
```

The program will process each PDF file, extract the text content, and save it in sorted order to text files.

## Dependencies

- Patagames.Pdf.Net: A PDF processing library for rendering and manipulating PDF documents.
- Tesseract OCR: An OCR engine for extracting text from images and documents.

## Example Output

Processed PDFs will have their text content extracted, sorted by page number, and saved as ".txt" files in the "pdfs" directory.

Feel free to contribute, report issues, or provide enhancements to this PDF to Text OCR Converter tool.
