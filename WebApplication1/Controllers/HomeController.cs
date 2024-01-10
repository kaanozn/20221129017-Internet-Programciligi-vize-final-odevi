using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication1.Models;
using WebApplication1.ModelViews;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            var haber = _appDbContext.habers.Select(x => new HaberViewModels
            {
                Id = x.Id,
                HaberBaslik = x.HaberBaslik,
                HaberDetay = x.HaberDetay,
                HaberResimData = x.HaberResim
            }).ToList();
            return View(haber);
        }

        public IActionResult HaberDetay(int ıd)
        {
            var habergetir = _appDbContext.habers.Where(x => x.Id == ıd).Select(y => new HaberViewModels()
            {
                Id = y.Id,
                HaberBaslik = y.HaberBaslik,
                HaberDetay = y.HaberDetay,
                HaberResimData = y.HaberResim
            }).ToList();
            return View(habergetir);
        }
        public IActionResult Iletisim()
        {
            return View();
        }

        public IActionResult Spor()
        {
            string yazı = "Spor";
            var spor = _appDbContext.habers.Include(m=>m.Kategori).Where(x => x.Kategori.KategoriBaslik == yazı).Select(m => new HaberViewModels()
            {
                Id = m.Id,
                HaberBaslik = m.HaberBaslik,
                HaberDetay = m.HaberDetay,
                HaberResimData = m.HaberResim
            }).ToList();

            return View(spor);
        }
        public IActionResult Saglık()
        {
            string yazı = "Sağlık";
            var saglık = _appDbContext.habers.Include(y=>y.Kategori).Where(x => x.Kategori.KategoriBaslik == yazı).Select(m => new HaberViewModels()
            {
                Id = m.Id,
                HaberBaslik = m.HaberBaslik,
                HaberDetay = m.HaberDetay,
                HaberResimData = m.HaberResim
            }).ToList();

            return View(saglık);
        }
        public IActionResult Siyaset()
        {
            string yazı = "Siyaset";
            var Siyaset = _appDbContext.habers.Include(br=>br.Kategori).Where(x => x.Kategori.KategoriBaslik == yazı).Select(m => new HaberViewModels()
            {
                Id = m.Id,
                HaberBaslik = m.HaberBaslik,
                HaberDetay = m.HaberDetay,
                HaberResimData = m.HaberResim
            }).ToList();

            return View(Siyaset);
        }


        [HttpPost]
        public async Task<IActionResult> MailGonder(İletisimViewModel model)
        {
            var kaydet = new Iletisim();
            kaydet.Name = model.Name;
            kaydet.Email = model.Email;
            kaydet.Surname =model.Surname;
            kaydet.Message = model.Message;
            _appDbContext.ıletisims.Add(kaydet);
            await _appDbContext.SaveChangesAsync();
            return Json(new { success = true });
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
    }
}