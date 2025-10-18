using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class ModalidadesController : Controller
    {
        private readonly MvcMovieContext _context;

        public ModalidadesController(MvcMovieContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _context.Modalidade
                .Include(m => m.Turma)
                .ToListAsync();
            return View(lista);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var modalidade = await _context.Modalidade
                .Include(m => m.Turma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modalidade == null) return NotFound();
            return View(modalidade);
        }

        public IActionResult Create()
        {
            ViewData["TurmaId"] = new SelectList(_context.Set<Turma>(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,TurmaId")] Modalidade modalidade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modalidade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TurmaId"] = new SelectList(_context.Set<Turma>(), "Id", "Nome", modalidade.TurmaId);
            return View(modalidade);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var modalidade = await _context.Set<Modalidade>().FindAsync(id);
            if (modalidade == null) return NotFound();
            ViewData["TurmaId"] = new SelectList(_context.Set<Turma>(), "Id", "Nome", modalidade.TurmaId);
            return View(modalidade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,TurmaId")] Modalidade modalidade)
        {
            if (id != modalidade.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modalidade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Set<Modalidade>().AnyAsync(e => e.Id == modalidade.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TurmaId"] = new SelectList(_context.Set<Turma>(), "Id", "Nome", modalidade.TurmaId);
            return View(modalidade);
        }

        // Remova o Delete se não quiser implementar agora
    }
}
