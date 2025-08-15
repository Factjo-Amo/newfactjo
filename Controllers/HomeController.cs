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
using Microsoft.EntityFrameworkCore;

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
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.PublishedDate)
                .Take(5)
                .ToList();

            // ✅ الخبر الرئيسي (كبير)
            var mainTopNews = _context.NewsItems
                .Where(n => n.IsPublished && n.Placement == NewsPlacement.MainTop)
                .OrderByDescending(n => n.PublishedDate)
                .FirstOrDefault();

            // ✅ نحاول جلب 6 أخبار صغيرة
            var mainSmallNews = _context.NewsItems
                .Where(n => n.IsPublished && n.Placement == NewsPlacement.Main)
                .OrderByDescending(n => n.PublishedDate)
                .Take(6)
                .ToList();

            // ✅ لو أقل من 6، نكمّل تلقائياً من أحدث الأخبار المنشورة
            //    مع استثناء الخبر الكبير ومنع التكرار
            if (mainSmallNews.Count < 6)
            {
                var excludedIds = new HashSet<int>(mainSmallNews.Select(n => n.Id));
                if (mainTopNews != null) excludedIds.Add(mainTopNews.Id);

                int needed = 6 - mainSmallNews.Count;

                var filler = _context.NewsItems
                    .Where(n => n.IsPublished
                                && n.Placement != NewsPlacement.MainTop   // لا نكرر الكبير
                                && !excludedIds.Contains(n.Id))           // ولا نكرر المختار
                    .OrderByDescending(n => n.PublishedDate)
                    .Take(needed)
                    .ToList();

                mainSmallNews.AddRange(filler);
            }

            // ✅ أخبار الشريط العلوي (TopBar)
            var topBarNews = _context.NewsItems
                .Where(n => n.IsPublished && n.Placement == NewsPlacement.TopBar)
                .OrderByDescending(n => n.PublishedDate)
                .Take(10)
                .ToList();

            var latestArticles = _context.Articles
                .OrderByDescending(a => a.PublishedDate)
                .Take(5)
                .ToList();

            var tickerNews = _context.NewsItems
                .Where(n => n.IsPublished && n.CategoryId == 11) // شريط الأعلى
                .OrderByDescending(n => n.PublishedDate)
                .Take(10)
                .ToList();

            var categories = _context.Categories.ToList();

            // ✅ وسط البلد — robust + fallback
            const int DowntownCategoryIdFallback = 1; // إذا كان ID=1 ثابت لديك (كما يبدو من الكود)
            var downtownCategoryId =
                _context.Categories
                    .Where(c => c.Name.Trim() == "وسط البلد")
                    .Select(c => (int?)c.Id)
                    .FirstOrDefault()
                ?? DowntownCategoryIdFallback;

            // اجلب آخر 4 أخبار منشورة للتصنيف (مع استثناء TopBar إن رغبت)
            var downtownNews = _context.NewsItems
                .Where(n => n.IsPublished
                            && n.CategoryId == downtownCategoryId
                            && n.Placement != NewsPlacement.TopBar)
                .OrderByDescending(n => n.Id)   // ترتيب آمن وبسيط
                .Take(4)
                .ToList();

            // 🛟 Fallback: لو ما في عناصر للتصنيف، اعرض آخر 4 منشورة من أي تصنيف (حتى لا يختفي القسم كله)
            if (downtownNews.Count == 0)
            {
                downtownNews = _context.NewsItems
                    .Where(n => n.IsPublished)
                    .OrderByDescending(n => n.Id)
                    .Take(4)
                    .ToList();
            }

            ViewBag.DowntownNews = downtownNews;


            // ✅ بانوراما (CategoryId = 13)
            var panoramaNews = _context.NewsItems
                .Where(n => n.IsPublished && n.CategoryId == 13)
                .OrderByDescending(n => n.PublishedDate)
                .Take(8)
                .ToList();
            ViewBag.PanoramaNews = panoramaNews;

            var specialCategoriesIds = new List<int> { 6, 4, 3 };  // نافذة الحقيقة / مال وأعمال / وجهة نظر

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
                .Where(cat => !hiddenCategoryIds.Contains(cat.Id))
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
                Advertisements = advertisements,

                // 🎯 القيم الجديدة
                MainTopNews = mainTopNews,
                MainSmallNews = mainSmallNews,
                TopBarNews = topBarNews
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
            var messages = _context.ChatMessages
                .OrderBy(m => m.Timestamp)
                .ToList();

            ViewBag.ChatMessages = messages;
            return View();
        }

        public IActionResult OurPrograms()
        {
            return View();
        }

      
    }
}
