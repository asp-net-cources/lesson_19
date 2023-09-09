using System.Threading.Tasks;
using Lesson19.Data.Models;

namespace Lesson19.Data;

public interface IDataContext
{
    public Task<Product[]> SelectProducts();
    public Task InsertProduct(Product product);
    public Task UpdateProduct(Product product);
    public Task DeleteProduct(int id);

    public Task<Customer[]> SelectCustomers();
    public Task InsertCustomer(Customer customer);
    public Task UpdateCustomer(Customer customer);
    public Task DeleteCustomer(int id);

    // public Task<Order[]> SelectOrders();
    // public Task InsertOrder(Order order);
    // public Task UpdateOrder(Order order);
    // public Task DeleteOrder(int id);
}
