 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Commons;
using Business.DTO.Product;
using DAL.Data;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/{controller}")]
    [ApiController, Authorize]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _context.Products.Where(n=>!n.IsDeleted).ToListAsync();
            if (data is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { code = 4150, message = "Id is Null" });

            }
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            var data = await _context.Products.Where(n=>n.Id == id && !n.IsDeleted).FirstOrDefaultAsync();
            if (data is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new{code = 4150, message= "Id is Null" });
            }
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create( ProductPostDto productDto)
        {
            if (productDto is null)
            {
               return StatusCode(StatusCodes.Status404NotFound, new { code = 4160, message = "ProductDTO is Null" });
            }

            Product product = new Product();
            product.Title = productDto.Title;
            product.Price = productDto.Price;
            product.Rate = productDto.Price;
            product.CreateDate = DateTime.UtcNow.AddHours(4);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync(); 
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update(int id, ProductPutDto producttDto)
        {
            var dbdata = await _context.Products.Where(n => n.Id == id).FirstOrDefaultAsync();
            if (dbdata is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response(4456,"Id is Invalid"));
            }
            dbdata.Title = producttDto.Title ?? dbdata.Title;
            dbdata.Price = producttDto.Price == 0 ? dbdata.Price : producttDto.Price;
            dbdata.Rate = producttDto.Rate == 0 ? dbdata.Rate : producttDto.Rate;
            dbdata.UpdatedDate = DateTime.UtcNow.AddHours(4);
            _context.Products.Update(dbdata);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dbdata = await _context.Products.Where(n => n.Id == id).FirstOrDefaultAsync();
            if (dbdata is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response(4456, "Id is Invalid"));
            }
            dbdata.IsDeleted = true;
            _context.Products.Update(dbdata);
            await _context.SaveChangesAsync();
            return Ok();
        }
    } 
}

