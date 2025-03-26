using _0_Framework.Application;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Domain.ProductAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class ProductRepository : RepositoryBase<long, Product>, IProductRepository
    {
        private readonly ShopContext _shopContext;

        public ProductRepository(ShopContext shopContext) : base(shopContext)
        {
            _shopContext = shopContext;
        }

        public EditProduct GetDetails(long id)
        {
            return _shopContext.Products.Select(x => new EditProduct
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                UnitPrice = x.UnitPrice,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                KeyWords = x.KeyWords,
                MetaDescription = x.MetaDescription,
                Slug = x.Slug,
                CategoryId = x.CategoryId,
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<ProductViewModel> GetProducts()
        {
            return _shopContext.Products.Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
        }

        public List<ProductViewModel> Search(ProductSearchModel searchModel)
        {
            var query = _shopContext.Products.Include(x => x.Category).Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                UnitPrice = x.UnitPrice,
                Picture = x.Picture,
                Category = x.Category.Name,
                CategoryId = x.CategoryId,
                CreationDate = x.CreationDate.ToFarsi(),
                IsInStock = x.InStock,

            });


            if (searchModel.CategoryId != 0)
                query = query.Where(x => x.CategoryId == searchModel.CategoryId);

            if (!string.IsNullOrWhiteSpace(searchModel.Code))
                query = query.Where(x => x.Code.Contains(searchModel.Code));

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name.Contains(searchModel.Name));

            return query.OrderByDescending(x => x.Id).ToList();

        }
    }
}
