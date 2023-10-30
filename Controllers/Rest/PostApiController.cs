using Microsoft.AspNetCore.Mvc;
using Pc3.Service;
using petclinic.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using petclinic.Data;
using petclinic.Models;


namespace Pc3.Controllers.Rest
{
    [ApiController]
    [Route("api/producto")]
    public class PostApiController :ControllerBase
    {
        private readonly PostService _postService;

        public PostApiController(PostService postService)
        {
            _postService = postService;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<TodoDTO>>> List()
        {
            var productos = await _postService.GetAll();
            if (productos == null)
                return NotFound();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoDTO>> GetProducto(int? id)
        {
            var producto = await _postService.Get(id);
            if (producto == null)
                return NotFound();
            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<TodoDTO>> CreateProducto(TodoDTO producto)
        {
            if (producto == null)
            {
                return BadRequest();
            }
            await _postService.CreateOrUpdate(producto);
            return Ok(producto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProducto(int? id)
        {
            await _postService.Delete(id);
            //await _postService.CreateOrUpdate(id);
            return Ok();
        }
    }
}
