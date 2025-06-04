using API.RequestPagination;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductsController (IGenericRepository<Product> repo):BaseAPIController
     
    {
       

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(
            [FromQuery] ProductSpecParameters specParams)
        {
            //return await _Context.products.ToListAsync();
            var spec = new ProductSpecification(specParams);
            // var products = await repo.ListAsync(spec);
            // var count =await repo.CountAsync(spec);
            // var pagination = new Pagination<Product>(specParams.PageIndex, specParams.PageSize,
            //                 count, products);
            // return Ok(await repo.ListAllAsync());
            return await CreatePagedResult(repo, spec, specParams.PageIndex, specParams.PageSize);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            // var product = await _Context.products.FindAsync(id);
            var product = await repo.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            return product;
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();
            return Ok(await repo.ListAsync(spec));
           // return Ok(await repo.GetBrandsAsync());
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();

            return Ok(await repo.ListAsync(spec));
            //return Ok(await repo.GetTypes());
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
             // _Context.products.Add(product);
            // await _Context.SaveChangesAsync();
            // return product;
            repo.Add(product);
            if (await repo.SaveAllAsync())
            {
                return Ok();
                // return CreatedAtAction("Product Created", new { id = product.Id }, product);
            }
            return BadRequest("Problem Creating product");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExist(id))
                return BadRequest("Cannot update this product");
            repo.Update(product);
            if (await repo.SaveAllAsync())
            {
                return NoContent();
            }
            // _Context.Entry(product).State = EntityState.Modified;
                // await _Context.SaveChangesAsync();
                return BadRequest("Problem updating product");

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            // var product = await _Context.products.FindAsync(id);
            var product = await repo.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            // _Context.products.Remove(product);
            // await _Context.SaveChangesAsync();
            // return NoContent();
            repo.Delete(product);
             if (await repo.SaveAllAsync())
            {
                return NoContent();
            }
           return BadRequest("Problem deleting product");

        }

        private bool ProductExist(int id)
        {
            // return _Context.products.Any(x => x.Id == id);
            return repo.Exist(id);
        }
    }
}
