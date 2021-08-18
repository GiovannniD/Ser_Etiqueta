using Microsoft.AspNetCore.Mvc;
using Ser_Etiqueta.Data;
using Ser_Etiqueta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ser_Etiqueta.Controllers
{
    public class LibrosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LibrosController(ApplicationDbContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Libro> listLibros = _context.Libro;
            return View(listLibros);
        }

        public IActionResult Create()
        {
           
            return View();
        }

        [HttpPost]
        public IActionResult Create(Libro libro)
        {
            if (ModelState.IsValid) 
            {
                _context.Libro.Add(libro);
                _context.SaveChanges();

                TempData["Mensaje"] = "El libro se ha creado correctamente";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Libro libro)
        {
            if (ModelState.IsValid)
            {
                _context.Libro.Update(libro);
                _context.SaveChanges();

                TempData["Mensaje"] = "El libro se ha editado correctamente";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id==0) {
                return NotFound();
            }
            //obtener libro
            var libro = _context.Libro.Find(id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        [HttpGet]
        public object getData(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //obtener libro
            var libro = _context.Libro.Find(id);
            if (libro == null)
            {
                return NotFound();
            }

            return libro;
        }
    }
}
