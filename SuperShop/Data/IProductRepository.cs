using SuperShop.Data.Entities;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task DeleteAsync(Product product);
    }
}
