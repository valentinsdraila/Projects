using AForge.Imaging.Filters;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Xml;
using Tesseract;
using Point = OpenCvSharp.Point;

namespace SherlockUtil
{
  public class TestObject
  {
    public string Category { get; set; }
    public string Name { get; set; }
    public string InputFolder { get; set; }
    public string OutputFolder { get; set; }
    public string Responsible { get; set; }
    public string Status { get; set; }
    public string HostName { get; set; }

    public TestObject() { }
    public TestObject(XmlNode testObject)
    {
      string[] categoryTest = testObject.Attributes.GetNamedItem("Name").Value.Split('\\');
      this.Category = categoryTest[0];
      this.Name = categoryTest[1];
      this.InputFolder = testObject.SelectSingleNode("InputFolder").InnerText;
      this.OutputFolder = testObject.SelectSingleNode("OutputFolder").InnerText;
      this.Responsible = Directory.GetParent(InputFolder).FullName;
      this.Status = testObject.SelectSingleNode("Status").InnerText;
      ReadWriteINIfile iniUtil = new ReadWriteINIfile(Responsible + "\\Adm" + "\\properties.txt");
      Responsible = iniUtil.ReadINI("general", "Responsible")[0];
    }

    public string ExtractError(List<OutputFile> outputFiles)
    {
      foreach (var outputFile in outputFiles)
      {
        string fileToSearch = outputFile.Path;
        if (File.Exists(fileToSearch))
        {
          FileInfo outputInfo = new FileInfo(fileToSearch);
          if (outputInfo.Length > 0)
          {
            string extractedText = outputFile.ExtractError();
            if (extractedText.Length > 0)
              return extractedText;
          }
        }
      }
      return "";
    }

    public string ExtractImage()
    {
      string extracted = "";
      if (File.Exists(OutputFolder + @"\ATFSystem\1.jpg"))
      {
        Mat img = Cv2.ImRead(OutputFolder + "\\ATFSystem\\1.jpg");
        Mat img_gray = new Mat();
        Cv2.CvtColor(img, img_gray, ColorConversionCodes.BGR2GRAY);
        Mat img_blur = new Mat();
        Cv2.GaussianBlur(img_gray, img_blur, new OpenCvSharp.Size(5, 5), 1);
        Mat edges = new Mat();
        Cv2.Canny(img_blur, edges, 100, 200, 3, false);
        Point[][] contours;
        HierarchyIndex[] hierarchy;
        Cv2.FindContours(edges, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
        Mat result = new Mat();
        OpenCvSharp.Rect roi = new OpenCvSharp.Rect(img.Width / 2 - 100, img.Height / 2 - 25, 200, 50);
        Rectangle OCRroi = new Rectangle(0, 0, img.Width, img.Height);
        foreach (var contour in contours)
        {
          var boundingRect = Cv2.BoundingRect(contour);
          double aspectRatio = (double)boundingRect.Width / boundingRect.Height;

          if (boundingRect.Contains(roi) && OCRroi.Width * OCRroi.Height > boundingRect.Width * boundingRect.Height)
          {
            Point[][] rectcontour = new Point[1][];
            rectcontour[0] = contour;
            OCRroi = new Rectangle(boundingRect.X, boundingRect.Y, boundingRect.Width, boundingRect.Height);
          }
        }
        if (OCRroi.Width > img.Width - 100 && OCRroi.Height > img.Height - 100)
          return extracted;
        OCRroi.Y = OCRroi.Y + 20;
        OCRroi.Height = OCRroi.Height - 50;
        var image = new Bitmap(OutputFolder + "\\ATFSystem\\1.jpg");
        Crop crop = new Crop(OCRroi);
        var croppedImage = crop.Apply(image);
        var upscaledImage = new Bitmap(croppedImage.Width * 2, croppedImage.Height * 2);
        Graphics graphics = Graphics.FromImage(upscaledImage);
        graphics.DrawImage(croppedImage, new Rectangle(0, 0, upscaledImage.Width, upscaledImage.Height));
        graphics.Dispose();
        ContrastCorrection contrast = new ContrastCorrection();
        var finalImage = contrast.Apply(upscaledImage);
        string path = "..\\..\\..\\SherlockUtil\\tessdata";
        var ocr = new TesseractEngine(path, "eng");
        using (Page page = ocr.Process(PixConverter.ToPix(finalImage)))
        {
          extracted = page.GetText();
          return extracted;
        }
      }
      return extracted;
    }

    public List<OutputFile> GetOutputFiles(List<KeyValuePair<string, string>> outputFiles, string iniPath, IMessage message)
    {
      List<OutputFile> testOutputFiles = new List<OutputFile>();
      try
      {
        DirectorySecurity ds = Directory.GetAccessControl(OutputFolder);
      }
      catch (Exception)
      {
        message.Write("The output folder cannot be accessed:" + OutputFolder, false);
        
        return testOutputFiles;
      }
      foreach (var item in outputFiles)
      {
        OutputFile outputFile = new OutputFile(item.Key, iniPath, OutputFolder + "\\" + item.Value);
        if (outputFile.Path.Contains('*'))
        {
          try
          {
            string[] files = Directory.GetFiles(Path.GetDirectoryName(outputFile.Path), Path.GetFileName(outputFile.Path));
            if (files.Length > 0)
              foreach (string file in files)
              {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Length > 0)
                {
                  OutputFile wildcardFile = new OutputFile(item.Key, iniPath, file);
                  testOutputFiles.Add(wildcardFile);
                }
              }
          }
          catch (Exception e)
          {
            Console.WriteLine(e.Message);
            Console.WriteLine("Could not add wildcard files for test {0}", Name);
          }
          continue;
        }
        testOutputFiles.Add(outputFile);
      }
      return testOutputFiles;
    }
  }
}
