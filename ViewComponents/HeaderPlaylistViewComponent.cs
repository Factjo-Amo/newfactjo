using Microsoft.AspNetCore.Mvc;
using Newfactjo.Data;
using System.Linq;

namespace Newfactjo.ViewComponents
{
    public class HeaderPlaylistViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public HeaderPlaylistViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var items = _context.HeaderPlaylists
                .Where(p => p.IsActive)
                .OrderBy(p => p.DisplayOrder)
                .ToList();

            return View(items);
        }
    }
}
