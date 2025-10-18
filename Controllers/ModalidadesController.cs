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
                .Include(m => m.ModalidadesTurmas)
                    .ThenInclude(mt => mt.Turma)
                .ToListAsync();
            return View(lista);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var modalidade = await _context.Modalidade
                .Include(m => m.ModalidadesTurmas)
                    .ThenInclude(mt => mt.Turma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modalidade == null) return NotFound();
            return View(modalidade);
        }

        public IActionResult Create()
        {
            ViewBag.Turmas = new MultiSelectList(_context.Set<Turma>(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Modalidade modalidade, int[] selectedTurmas)
        {
            if (ModelState.IsValid)
            {
                if (selectedTurmas != null)
                {
                    foreach (var turmaId in selectedTurmas)
                    {
                        modalidade.ModalidadesTurmas.Add(new ModalidadeTurma
                        {
                            ModalidadeId = modalidade.Id,
                            TurmaId = turmaId
                        });
                    }
                }
                
                _context.Add(modalidade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Turmas = new MultiSelectList(_context.Set<Turma>(), "Id", "Nome", selectedTurmas);
            return View(modalidade);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var modalidade = await _context.Modalidade
                .Include(m => m.ModalidadesTurmas)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modalidade == null) return NotFound();
            
            var selectedTurmas = modalidade.ModalidadesTurmas.Select(mt => mt.TurmaId).ToArray();
            ViewBag.Turmas = new MultiSelectList(_context.Set<Turma>(), "Id", "Nome", selectedTurmas);
            return View(modalidade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Modalidade modalidade, int[] selectedTurmas)
        {
            if (id != modalidade.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    var modalidadeToUpdate = await _context.Modalidade
                        .Include(m => m.ModalidadesTurmas)
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (modalidadeToUpdate == null) return NotFound();

                    modalidadeToUpdate.Nome = modalidade.Nome;
                    modalidadeToUpdate.ModalidadesTurmas.Clear();

                    if (selectedTurmas != null)
                    {
                        foreach (var turmaId in selectedTurmas)
                        {
                            modalidadeToUpdate.ModalidadesTurmas.Add(new ModalidadeTurma
                            {
                                ModalidadeId = modalidade.Id,
                                TurmaId = turmaId
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Set<Modalidade>().AnyAsync(e => e.Id == modalidade.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Turmas = new MultiSelectList(_context.Set<Turma>(), "Id", "Nome", selectedTurmas);
            return View(modalidade);
        }

        // Remova o Delete se não quiser implementar agora
    }
}
