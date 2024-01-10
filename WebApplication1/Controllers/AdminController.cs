using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WebApplication1.Models;
using WebApplication1.ModelViews;

namespace WebApplication1.Controllers
{
    [Authorize(Roles ="Admin,Calisan")]
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly INotyfService _notyfService;
        private readonly AppDbContext _appDbContext;
        private readonly IFileProvider _fileProvider;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AdminController(IConfiguration configuration, INotyfService notyfService, AppDbContext appDbContext, IFileProvider fileProvider, UserManager<AppUser> userManager = null, RoleManager<AppRole> roleManager = null, SignInManager<AppUser> signInManager = null)
        {
            _configuration = configuration;
            _notyfService = notyfService;
            _appDbContext = appDbContext;
            _fileProvider = fileProvider;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Haber()
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

        [HttpGet]
        public IActionResult HaberEkle()
        {
            List<SelectListItem> kategorigetir = (from x in _appDbContext.kategori.ToList()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.KategoriBaslik,
                                                      Value = x.Id.ToString()
                                                  }).ToList();
            ViewBag.Kategori = kategorigetir;
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> HaberEkle(HaberViewModels models)
        {
            var rootfolder = _fileProvider.GetDirectoryContents("wwwroot");
            var photoUrl = "-";
            if (models.HaberResim.Length > 0 && models.HaberResim != null)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(models.HaberResim.FileName);
                var photoPath = Path.Combine(rootfolder.First(x => x.Name == "Haber").PhysicalPath, filename);
                using var stream = new FileStream(photoPath, FileMode.Create);
                models.HaberResim.CopyTo(stream);
                photoUrl = filename;
            }
            var haberekle = new Haber();
            haberekle.HaberBaslik = models.HaberBaslik;
            haberekle.HaberDetay=models.HaberDetay;
            haberekle.HaberResim = photoUrl;
            haberekle.KategoriId = models.KategoriId;
            await _appDbContext.habers.AddAsync(haberekle);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Haber","Admin");
        }
        [HttpGet]
        public IActionResult HaberGuncelle(int id)
        {
            List<SelectListItem> kategorigetir = (from x in _appDbContext.kategori.ToList()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.KategoriBaslik,
                                                      Value = x.Id.ToString()
                                                  }).ToList();
            ViewBag.Kategori = kategorigetir;

            var haberguncelle = _appDbContext.habers.Where(x => x.Id == id).Select(y => new HaberViewModels()
            {
                HaberBaslik = y.HaberBaslik,
                HaberDetay = y.HaberDetay,
                HaberResimData = y.HaberResim,
                KategoriId = y.KategoriId,
            }).FirstOrDefault();
            return View(haberguncelle);
        }
        [HttpPost]
        public async Task<IActionResult> HaberGuncelle(HaberViewModels models)
        {
            var haberguncelle = _appDbContext.habers.Where(x => x.Id == models.Id).FirstOrDefault();
            haberguncelle.HaberResim = models.HaberResimData;
            haberguncelle.HaberDetay= models.HaberDetay;
            haberguncelle.HaberBaslik= models.HaberBaslik;
            haberguncelle.KategoriId = models.KategoriId;
            await _appDbContext.SaveChangesAsync();
            _notyfService.Success("Güncelleme işlemi Başarılı");
            return RedirectToAction("Haber", "Admin");
        }

        public IActionResult Kategori()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> KategoriGetir(KategoriViewModel model)
        {

            var kategori = _appDbContext.kategori.Select(x => new KategoriViewModel()
            {
                Id = x.Id,
                KategoriBaslik = x.KategoriBaslik
            }).ToList();
            return Json(kategori);         
        }
        [HttpPost]
        public async Task<IActionResult> KategoriEkle(KategoriViewModel model)
        {
            var kategori = new Kategori();
            kategori.KategoriBaslik = model.KategoriBaslik;
            _appDbContext.kategori.Add(kategori);
            await _appDbContext.SaveChangesAsync();
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> KategoriGuncelle(KategoriViewModel model)
        {
         
            var kategori = _appDbContext.kategori.SingleOrDefault(x => x.Id == model.Id);
            kategori.KategoriBaslik = model.KategoriBaslik;
            _appDbContext.kategori.Update(kategori);
            await _appDbContext.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> KategoriSil(KategoriViewModel model)
        {
    
            var kategorisil = _appDbContext.kategori.FirstOrDefault(x => x.Id == model.Id);
            _appDbContext.kategori.Remove(kategorisil);
            await _appDbContext.SaveChangesAsync();
            return Json(new { success = true });

        }

        public async Task<IActionResult> GetUserList()
        {
            var userModels = await _userManager.Users.Select(x => new UserModel()
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                UserName = x.UserName,
                City = x.City
            }).ToListAsync();
            return View(userModels);
        }
        public async Task<IActionResult> GetRoleList()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        [HttpGet]
        public IActionResult Iletisim()
        {
            var iletisim = _appDbContext.ıletisims.Select(x=>new İletisimViewModel()
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                Surname = x.Surname,
                Message = x.Message,

            }).ToList();
            return View(iletisim);
        }
       
        public IActionResult IletisimDelete(int ıd)
        {
            var iletisimdelete = _appDbContext.ıletisims.Where(x => x.Id == ıd).FirstOrDefault();
            _appDbContext.ıletisims.Remove(iletisimdelete);
            _appDbContext.SaveChanges();
            return RedirectToAction("Iletisim","Admin");
        }




        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Login");
        }


    }
}
