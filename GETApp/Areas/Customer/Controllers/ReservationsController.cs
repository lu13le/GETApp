using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GETApp.Data;
using GETApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace GETApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<SignalrServer> _signalHub;

        public ReservationsController(ApplicationDbContext context, IHubContext<SignalrServer> signalHub)
        {
            _context = context;
            _signalHub = signalHub;
        }

        // GET: Customer/Reservations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reservation.ToListAsync());
        }
        [HttpGet]
        public IActionResult GetReservations()
        {
            var res = _context.Reservation.ToList();
            return Ok(res);
        }
        // GET: Customer/Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.ResId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Customer/Reservations/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Success()
        {
            return Content("<html><h1>Uspesno ste dodali <u>rezervaciju!</u></h1></html>", "text/html");
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResId,Name,PhoneNumber,DateTime,Table,AdditionalComment")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                await _signalHub.Clients.All.SendAsync("LoadReservations");
                return RedirectToAction(nameof(Success));
            }
            return View(reservation);
        }

        // GET: Customer/Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int ResId, [Bind("ResId,Name,PhoneNumber,DateTime,Table,AdditionalComment")] Reservation reservation)
        {
            if (ResId != reservation.ResId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                    await _signalHub.Clients.All.SendAsync("LoadReservations");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ResId))
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
            return View(reservation);
        }

        // GET: Customer/Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.ResId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Customer/Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ResId)
        {
            var reservation = await _context.Reservation.FindAsync(ResId);
            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();
            await _signalHub.Clients.All.SendAsync("LoadReservations");
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.ResId == id);
        }
    }
}
