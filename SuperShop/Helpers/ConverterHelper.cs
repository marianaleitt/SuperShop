using SuperShop.Data.Entities;
using SuperShop.Models;
using System.IO;

namespace SuperShop.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        Product IConverterHelper.ToProduct(ProductViewModel model, string path,  bool isNew)
        {
            return new Product
            {
                Id = isNew ? 0 : model.Id,
                ImageUrl = path,
                IsAvailable = model.IsAvailable,
                LastPurchase = model.LastPurchase,
                LastSale = model.LastSale,
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
                User = model.User,
            };
        }

        ProductViewModel IConverterHelper.ToProductViewModel(Product product)
        {
            throw new System.NotImplementedException();
        }
    }
}
