using DosyaIslemleri.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DosyaIslemleri.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult DosyaIslemleri()
        {
            var rootPath = _webHostEnvironment.ContentRootPath;
            var filePath = Path.Combine(rootPath, "wwwroot/Files/ogrenciListesi.txt");

            var allLines = System.IO.File.ReadAllLines(filePath);
            var ogrenciListesi = new List<Ogrenci>();
            if (allLines != null) 
            {
                foreach (var line in allLines) 
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        var parcalar = line.Split('|');
                        var id = Convert.ToInt32(parcalar[0]);
                        if (ogrenciListesi.Exists(o => o.Id == id))
                        {
                            continue;
                        }
                        Ogrenci ogrenci = new Ogrenci();
                        ogrenci.Id = id;
                        ogrenci.AdSoyad = parcalar[1];
                        ogrenci.Sinif=parcalar[2];
                        ogrenci.Yas = Convert.ToInt32(parcalar[3]);

                        ogrenciListesi.Add(ogrenci);
                    }
                }
                var yeniOgrenciYolu = Path.Combine(rootPath, "wwwroot/files/yeniOgrenciListesi.txt");
                var content = ogrenciListesi.Select(o => $"{o.Id}|{o.AdSoyad}|{o.Sinif}|{o.Yas}").ToList();
                System.IO.File.WriteAllLines(yeniOgrenciYolu,content);
            }

            return View();
        }
    }
}
