using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupermarketProjectMVC.Data;
using SupermarketProjectMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SupermarketProjectMVC.Controllers
{
    [AllowAnonymous]
    public class StoreController : Controller

    {
        private readonly ApplicationDbContext _context;

        public StoreController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Products
        public async Task<IActionResult> Index()
        {

            return View(await _context.Category.ToListAsync());
        }

        public IActionResult browse(string category)
        {
            var categoryModel = _context.Category.Include("Items")
                .Single(c => c.Name == category);
            return (View(categoryModel));
        }

        // GET: StoreController/Details/5
        public async Task<ActionResult> Details(int id)
        {

            var item = await _context.Item
          .Include(i => i.Category)
          .Include(i => i.Producer)
          .FirstOrDefaultAsync(m => m.ItemId == id);

            return View(item);
        }

        // GET: StoreController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StoreController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StoreController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StoreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public async Task<IActionResult> SearchFct(string searchString)
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
                items = items.Where(s => s.Title.Contains(searchString));

            }



            return View(await items.Include(i => i.Category).Include(i => i.Producer).ToListAsync());
            //  return View(await applicationDbContext.ToListAsync());
        }
        // GET: StoreController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StoreController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}