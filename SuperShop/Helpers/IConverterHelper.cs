using System;
using SuperShop.Data.Entities;
using SuperShop.Models;


namespace SuperShop.Helpers
{
    public interface IConverterHelper
    {
        Product ToProduct(ProductViewModel model, Guid imadeId, bool isNew);

        ProductViewModel ToProductViewModel(Product product);
    }
}
