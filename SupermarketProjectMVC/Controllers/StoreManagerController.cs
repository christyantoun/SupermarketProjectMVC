using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupermarketProjectMVC.Areas.Identity.Data;
using SupermarketProjectMVC.Data;
using SupermarketProjectMVC.Models;

namespace SupermarketProjectMVC.Controllers
{
   // [Authorize(UserStore = "Ali@gmail.com,Ahmed@gmail.com")]
   [Authorize]
    public class StoreManagerController : Controller
    {
       
        private readonly ApplicationDbContext _context;

        public StoreManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StoreManager
        public async Task<IActionResult> Index(string itemCategory, string searchString)
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
       
            return View(CategoryListM);
          //  return View(await applicationDbContext.ToListAsync());
        }

        // GET: StoreManager/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var item = await _context.Item
                .Include(i => i.Category)
                .Include(i => i.Producer)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: StoreManager/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name");
            ViewData["ProducerId"] = new SelectList(_context.Producer, "ProducerId", "Name");
            return View();
        }

        // POST: StoreManager/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,CategoryId,ProducerId,Title,Price,ItemArtUrl")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name", item.CategoryId);
            ViewData["ProducerId"] = new SelectList(_context.Producer, "ProducerId", "Name", item.ProducerId);
            return View(item);
        }

        // GET: StoreManager/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name", item.CategoryId);
            ViewData["ProducerId"] = new SelectList(_context.Producer, "ProducerId", "Name", item.ProducerId);
            return View(item);
        }

        // POST: StoreManager/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,CategoryId,ProducerId,Title,Price,ItemArtUrl")] Item item)
        {
            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name", item.CategoryId);
            ViewData["ProducerId"] = new SelectList(_context.Producer, "ProducerId", "Name", item.ProducerId);
            return View(item);
        }

        // GET: StoreManager/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(i => i.Category)
                .Include(i => i.Producer)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: StoreManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Item.FindAsync(id);
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.ItemId == id);
        }
    }
}
