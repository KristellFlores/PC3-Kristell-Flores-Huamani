using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pc3.Service;
using petclinic.DTO;
using petclinic.Integrations;

namespace petclinic.Controllers.UI
{
    public class TodoController : Controller
    {
        private readonly PostService _postService;

        public TodoController(PostService postService)
        {
            _postService = postService;
        }

   
        public async Task<IActionResult> Index()
        {
            var productos = await _postService.GetAll();
            return productos != null ?
                          View(productos) :
                          Problem("Entity set 'ApplicationDbContext.DataPost'  is null.");
        }

       
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _postService.FirstOrDefault(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

   
        public IActionResult Create()
        {
            return View();
        }


      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,userId,title,Body")] TodoDTO todoDTO)
        {
            if (ModelState.IsValid)
            {
                await _postService.CreateOrUpdate(todoDTO);
                return RedirectToAction(nameof(Index));
            }
            return View(todoDTO);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            var producto = await _postService.Get(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,userId,title,Body")] TodoDTO todoDTO)
        {
            if (id != todoDTO.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _postService.CreateOrUpdate(todoDTO);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_postService.ProductExists(todoDTO.id))
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
            return View(todoDTO);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            var producto = await _postService.Get(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _postService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}