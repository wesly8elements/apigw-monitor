using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using apigw_monitor.Data;
using apigw_monitor.Model;

namespace apigw_monitor.Controllers
{
    public class MTsController : Controller
    {
        private readonly DelapanElementApiGWDBContext _context;

        public MTsController(DelapanElementApiGWDBContext context)
        {
            _context = context;
        }

        // GET: MTs
        public async Task<IActionResult> Index()
        {
            return View(await _context.MT.ToListAsync());
        }

        // GET: MTs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mT = await _context.MT
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mT == null)
            {
                return NotFound();
            }

            return View(mT);
        }

        // GET: MTs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MTs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Created,PortalId,CarrierId,Shortcode,Msisdn,Message,SessionId,RequestStatus,DeliveryStatus,Remarks")] MT mT)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mT);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mT);
        }

        // GET: MTs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mT = await _context.MT.FindAsync(id);
            if (mT == null)
            {
                return NotFound();
            }
            return View(mT);
        }

        // POST: MTs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Created,PortalId,CarrierId,Shortcode,Msisdn,Message,SessionId,RequestStatus,DeliveryStatus,Remarks")] MT mT)
        {
            if (id != mT.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mT);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MTExists(mT.Id))
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
            return View(mT);
        }

        // GET: MTs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mT = await _context.MT
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mT == null)
            {
                return NotFound();
            }

            return View(mT);
        }

        // POST: MTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mT = await _context.MT.FindAsync(id);
            if (mT != null)
            {
                _context.MT.Remove(mT);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MTExists(int id)
        {
            return _context.MT.Any(e => e.Id == id);
        }
    }
}
