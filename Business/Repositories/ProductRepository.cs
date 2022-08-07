using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Services;
using DAL.Data;
using DAL.Entities;

namespace Business.Repositories
{
    public class ProductRepository : IProductService
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<Product> Get(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Create(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(int id, Product entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}

