using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UploadThree.Models;
using static System.Net.WebRequestMethods;

namespace UploadThree.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet("")]
        public IActionResult Index()
        {
            StringComparison comp = StringComparison.OrdinalIgnoreCase;
            List<string> type = new List<string>(); 
            List<string> files = new List<string>(); 
            string path = "./files/";
            DirectoryInfo d = new DirectoryInfo(path);
            foreach(FileInfo file in d.GetFiles()){
                files.Add(file.Name);
                Console.WriteLine(file.Name);
                String FileNameBoi = file.Name;
                if(FileNameBoi.Contains(".JPG", comp) || FileNameBoi.Contains(".BMP", comp) || FileNameBoi.Contains(".GIF", comp) || FileNameBoi.Contains(".PNG", comp)){
                    type.Add("IMG");
                }
                else{
                    type.Add("Other");
                }
            }
            ViewBag.TotalFiles = files;
            ViewBag.FileTypes = type;
            ViewBag.progress = 0;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Index(IFormFile file) {
            Startup.Progress = 0;
            ViewBag.progress = Startup.Progress;

            long totalBytes = file.Length;

            byte[] buffer = new byte[16 * 1024];

            using (FileStream output = System.IO.File.Create("./files/" + file.FileName))
            {
                using (Stream input = file.OpenReadStream())
                {
                    long totalReadBytes = 0;
                    int readBytes;

                    while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0){
                        await output.WriteAsync(buffer, 0, readBytes);
                        totalReadBytes += readBytes;
                        Startup.Progress = (int)((float)totalReadBytes / (float)totalBytes * 100.0);
                        ViewBag.progress = Startup.Progress;
                        Console.WriteLine(Startup.Progress);
                        ViewBag.Message = "File successfully uploaded";
                    }
                }
            }
        //     if (file.Length > 0)
        //     {
        //         // full path to file in temp location
        //         var filePath = "./files/" + file.FileName; //we are using Temp file name just for the example. Add your own file path.
        //         using (var stream = new FileStream(filePath, FileMode.Create))
        //         {
        //             await file.CopyToAsync(stream);
        //         }
        // }
        StringComparison comp = StringComparison.OrdinalIgnoreCase;
        List<string> files = new List<string>(); 
        List<string> type = new List<string>(); 
            string path = "./files/";
            DirectoryInfo d = new DirectoryInfo(path);
            foreach(FileInfo fileInFolder in d.GetFiles()){
                files.Add(fileInFolder.Name);
                Console.WriteLine(fileInFolder.Name);
                String FileNameBoi = fileInFolder.Name;
                if(FileNameBoi.Contains(".JPG", comp) || FileNameBoi.Contains(".BMP", comp) || FileNameBoi.Contains(".GIF", comp) || FileNameBoi.Contains(".PNG", comp)){
                    type.Add("IMG");
                }
                else{
                    type.Add("Other");
                }
            }
            ViewBag.TotalFiles = files;
            ViewBag.FileTypes = type;
        return View();
        }

        [HttpGet("{file}")]
        public FileStreamResult Download(string file) {
            var path = "./files/" + file;
            FileStream stream = System.IO.File.OpenRead(path);
            return new FileStreamResult(stream, "application/octet-stream");
    }  

    }
}
