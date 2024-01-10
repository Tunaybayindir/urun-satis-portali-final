using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UrunSatisPortali.Models;
using UrunSatisPortali.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace UrunSatisPortali.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AdminController(AppDbContext context,UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            var degerler = _userManager.Users.ToList();
            return View(degerler);
        }
        //Kategoriler
        public IActionResult Kategori()
        {
            var degerler = _context.kategoris.ToList();
            return View(degerler);
        }
        [HttpGet]
        public IActionResult KategoriYeni()
        {
            return View();
        }
        [HttpPost]
        public IActionResult KategoriYeni(Kategori d)
        {
            _context.Add(d);
            _context.SaveChanges();
            return RedirectToAction("Kategori");
        }
        public IActionResult KategoriSil(int id)
        {
            var dep = _context.kategoris.Find(id);
            _context.kategoris.Remove(dep);
            _context.SaveChanges();
            return RedirectToAction("Kategori");
        }
        public IActionResult KategoriGetir(int id)
        {
            var dep = _context.kategoris.Find(id);
            return View("KategoriGetir", dep);
        }
       
        public IActionResult KategoriDetay(int id)
        {
            var degerler = _context.uruns.Where(x => x.kategori.kategoriID == id).ToList();
            return View(degerler);
        }
        public IActionResult KategoriGuncelle(Kategori d)
        {
            var dep = _context.kategoris.Find(d.kategoriID);
            dep.kategoriAD = d.kategoriAD;
            _context.SaveChanges();
            return RedirectToAction("Kategori");
        }
        //URUNLER 
        public IActionResult Urun()
        {
            var degerler = _context.uruns.Include(x => x.kategori).ToList();
            return View(degerler);
        }
        [HttpGet]
        public IActionResult UrunEkle()
        {
            List<SelectListItem> degerler = (from x in _context.kategoris.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = x.kategoriAD,
                                                 Value = x.kategoriID.ToString()
                                             }).ToList();
            ViewBag.dgr = degerler;
            return View();
        }
        [HttpPost]
        public IActionResult UrunEkle(urunEkle d)
        {
            Urun f = new Urun();
            var per = _context.kategoris.Where(x => x.kategoriID == d.kategoriID).FirstOrDefault();
            if (d.resim != null)
            {
                string imageExtension = Path.GetExtension(d.resim.FileName);

                string imageName = Guid.NewGuid() + imageExtension;

                string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/img/profile/{imageName}");

                var stream = new FileStream(path, FileMode.Create);

                d.resim.CopyTo(stream);
                f.resim = imageName;

            }
            f.urunAD = d.urunAD;
            f.urunSTOCK = d.urunSTOCK;
            f.Aciklama = d.Aciklama;
            f.kategoriFiyat = d.kategoriFiyat;
            f.urunETIKETNO = d.urunETIKETNO;
            f.kategoriID = d.kategoriID;
            _context.uruns.Add(f);
            _context.SaveChanges();
            return RedirectToAction("Urun");
        }
        public IActionResult urunSil(int id)
        {
            var dep = _context.uruns.Find(id);
            _context.uruns.Remove(dep);
            _context.SaveChanges();
            return RedirectToAction("Urun");
        }
        public IActionResult UrunGetir(int id)
        {
            var dep = _context.uruns.Find(id);
            return View("UrunGetir", dep);
        }

        public IActionResult AccessDenied()
        {
            return View();
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

        public IActionResult RoleAdd()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleAdd(AppRole model)
        {
            var role = await _roleManager.FindByNameAsync(model.Name);
            if (role == null)
            {

                var newrole = new AppRole();
                newrole.Name = model.Name; ;
                await _roleManager.CreateAsync(newrole);
            }
            return RedirectToAction("GetRoleList");
        }
    }
}
