using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newfactjo.Models;
using Newfactjo.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Localization;
using Newfactjo.ViewModels;


namespace Newfactjo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IViewLocalizer _localizer;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IViewLocalizer localizer)
        {
            _logger = logger;
            _context = context;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            ViewData["FullWidth"] = "container-fluid";

            var latestNews = _context.NewsItems
                .Where(n => n.IsPublished) // ✅ عرض الأخبار المفعّلة فقط
                .OrderByDescending(n => n.PublishedDate)
                .Take(5)
                .ToList();


            var latestArticles = _context.Articles
                .OrderByDescending(a => a.PublishedDate)
                .Take(5)
                .ToList();


            var tickerNews = _context.NewsItems
                .Where(n => n.IsPublished && n.CategoryId == 11) // ✅ مفعّلة ومن تصنيف شريط الأعلى
                .OrderByDescending(n => n.PublishedDate)
                .Take(10)
                .ToList();

            var categories = _context.Categories.ToList();

           


            // احضر ID تصنيف "وسط البلد"
            var downtownCategory = _context.Categories.FirstOrDefault(c => c.Name == "وسط البلد");

            List<News> downtownNews = new();

            if (downtownCategory != null)
            {
                downtownNews = _context.NewsItems
                    .Where(n => n.CategoryId == downtownCategory.Id && n.IsPublished)
                    .OrderByDescending(n => n.PublishedDate)
                    .Take(7) // نأخذ 7 أخبار فقط لتوزيعها على الأعمدة
                    .ToList();
            }

            ViewBag.DowntownNews = downtownNews;


            // ✅ أخبار بانوراما (CategoryId = 13)
            var panoramaNews = _context.NewsItems
                .Where(n => n.IsPublished && n.CategoryId == 13)
                .OrderByDescending(n => n.PublishedDate)
                .Take(8) // عدد كافٍ لتقسيم الأعمدة الثلاثة
                .ToList();

            ViewBag.PanoramaNews = panoramaNews;


            var specialCategoriesIds = new List<int> { 6, 4, 3 };  // شريط نافذة الحقيقة و مال واعمال و وجهة نظر

            var specialCategories = _context.Categories
                .Where(c => specialCategoriesIds.Contains(c.Id))
                .Select(cat => new CategoryWithNewsViewModel
                {
                    CategoryId = cat.Id,
                    CategoryName = cat.Name,
                    NewsItems = _context.NewsItems
                        .Where(n => n.IsPublished && n.CategoryId == cat.Id)
                        .OrderByDescending(n => n.PublishedDate)
                        .Take(4)
                        .ToList()
                }).ToList();
            var hiddenCategoryIds = _context.HiddenCategories.Select(h => h.CategoryId).ToList();


            var categoriesWithNews = categories
                .Where(cat => !hiddenCategoryIds.Contains(cat.Id)) // ✅ استثناء التصنيفات المخفية
                .Select(cat => new CategoryWithNewsViewModel
                {
                    CategoryId = cat.Id,
                    CategoryName = cat.Name,
                    NewsItems = _context.NewsItems
                        .Where(n => n.IsPublished && n.CategoryId == cat.Id)
                        .OrderByDescending(n => n.PublishedDate)
                        .Take(3)
                        .ToList()
                }).ToList();

            var advertisements = _context.Advertisements
    .Where(ad => ad.IsActive)
    .OrderByDescending(ad => ad.CreatedAt)
    .ToList();




            var viewModel = new HomeIndexViewModel
            {
                LatestNews = latestNews,
                LatestArticles = latestArticles,
                TickerNews = tickerNews,
                CategoriesWithNews = categoriesWithNews
         .Where(c => !specialCategoriesIds.Contains(c.CategoryId))
         .ToList(),
                SpecialThreeColumns = specialCategories,
                Advertisements = advertisements
            };


            return View(viewModel);
        }


        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Content("Test page works!");
        }
        [HttpGet]
        public IActionResult Subscribe()
        {
            return View();
        }
        public IActionResult FactjoSocial()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Subscribe(string Name, string Email, string Phone)
        {
            ViewBag.Message = $"شكراً لك {Name} على الاشتراك! سنراسلك على البريد {Email} قريبًا.";
            return View(); // سيعرض View: Views/Home/Subscribe.cshtml
        }





        [HttpGet]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true
                });

            return LocalRedirect(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Live()
        {
            return View();
        }

    }
}
