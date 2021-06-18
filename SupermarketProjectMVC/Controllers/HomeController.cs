using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SupermarketProjectMVC.Data;
using SupermarketProjectMVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SupermarketProjectMVC.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;

        }


        public async Task<IActionResult> Index( string itemCategory, string searchString)
        {
                //  var applicationDbContext = _context.Item.Include(i => i.Category).Include(i => i.Producer);
                IQueryable<string> categoryQuery = from m in _context.Item orderby m.Category.Name select m.Category.Name;

                var items = from m in _context.Item select m;

                //var items = _context.Item.Include(n => n.Category);
                //var items = _context.Item
                //    .Include(i => i.Category)
                //    .Include(i => i.Producer)
                //    .FirstOrDefaultAsync(from m in _context.Item select m);

                if (!string.IsNullOrEmpty(searchString))
                {
                    items = items.Where(s => s.Title.Contains(searchString) || s.Producer.Name.Contains(searchString));

                }

                if (!string.IsNullOrEmpty(itemCategory))
                {
                    items = items.Where(x => x.Category.Name == itemCategory);
                }

                var CategoryListM = new CategoryListModel
                {
                    Category = new SelectList(await categoryQuery.Distinct().ToListAsync()),
                    Item = await items.Include(i => i.Category).ToListAsync()
    ,
                };
                var applicationDbContext = _context.Item.Include(i => i.Category).Include(i => i.Producer);

            return View(CategoryListM);
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
        public async Task<IActionResult> Search(string itemCategory, string searchString)
        {
            //  var applicationDbContext = _context.Item.Include(i => i.Category).Include(i => i.Producer);
            IQueryable<string> categoryQuery = from m in _context.Item orderby m.Category.Name select m.Category.Name;

            var items = from m in _context.Item select m;

            //var items = _context.Item.Include(n => n.Category);
            //var items = _context.Item
            //    .Include(i => i.Category)
            //    .Include(i => i.Producer)
            //    .FirstOrDefaultAsync(from m in _context.Item select m);

            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Title.Contains(searchString) || s.Producer.Name.Contains(searchString));

            }

            if (!string.IsNullOrEmpty(itemCategory))
            {
                items = items.Where(x => x.Category.Name == itemCategory );
            }

            var CategoryListM = new CategoryListModel
            {
                Category = new SelectList(await categoryQuery.Distinct().ToListAsync()),
                Item = await items.Include(i => i.Category).ToListAsync()
,
            };
            return View("~/Views/StoreManager/Index.cshtml", CategoryListM);
            //return View(CategoryListM);
            //  return View(await applicationDbContext.ToListAsync());
        }
    }
}
