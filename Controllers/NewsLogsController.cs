using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newfactjo.Data;
using Newfactjo.Models;
using System.Threading.Tasks;

namespace Newfactjo.Controllers
{
    public class NewsLogsController : Controller
    {
        private readonly AppDbContext _context;

        public NewsLogsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: NewsLogs
        public async Task<IActionResult> Index()
        {
            var logs = await _context.NewsLogs
                .Include(nl => nl.News)
                .Include(nl => nl.AdminUser)
                .OrderByDescending(nl => nl.ActionDate)
                .ToListAsync();

            return View(logs);
        }
    }
}
