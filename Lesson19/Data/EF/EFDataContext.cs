using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Lesson19.Data.Models;

namespace Lesson19.Data.EF;

public class EfDataContext : DbContext, IDataContext {
    public DbSet<Customer> Customers { get; set; }
    // public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }

    public EfDataContext() {
        // Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public Task<Customer[]> SelectCustomers()
    {
        return Customers.ToArrayAsync();
    }
    public async Task InsertCustomer(Customer customer) {
        Customers.Add(customer);
        await SaveChangesAsync();
    }

    public async Task UpdateCustomer(Customer customer) {
        var foundCustomer = await Customers.FindAsync(customer.Id);

        if (foundCustomer == null)
            throw new ArgumentException($"Customer with id={customer.Id} not found");

        foundCustomer.FirstName = customer.FirstName;
        foundCustomer.LastName = customer.LastName;
        foundCustomer.Age = customer.Age;
        foundCustomer.Country = customer.Country;

        await SaveChangesAsync();
    }

    public async Task DeleteCustomer(int id) {
        var foundCustomer = Customers.AsNoTracking().FirstOrDefault(row => row.Id == id);
        
        if (foundCustomer == null)
            throw new ArgumentException($"Customer with id={id} not found");
        
        Customers.Remove(foundCustomer);
        await SaveChangesAsync();
    }

    // public IList<Order?> SelectOrders() => throw new NotImplementedException();
    // public async Task InsertOrder(Order order) => throw new NotImplementedException();
    // public async Task UpdateOrder(Order order) => throw new NotImplementedException();
    // public async Task DeleteOrder(int id) => throw new NotImplementedException();

    public Task<Product[]> SelectProducts() => Products.ToArrayAsync();

    public async Task InsertProduct(Product product)
    {
        Products.Add(product);
        await SaveChangesAsync();
    }
    
    public async Task UpdateProduct(Product product) {
        var foundProduct = Products.AsNoTracking().FirstOrDefault(row => row.Id == product.Id);

        if (foundProduct == null)
            throw new ArgumentException($"Product with id={product.Id} not found");
        
        Products.Remove(foundProduct);
        await SaveChangesAsync();
    }
    
    public async Task DeleteProduct(int id) {
        var foundProduct = Products.AsNoTracking().FirstOrDefault(row => row.Id == id);

        if (foundProduct == null)
            throw new ArgumentException($"Product with id={id} not found");
        
        Products.Remove(foundProduct);
        await SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Product>()
            .Property(product => product.ProductType)
            .HasConversion(
                productType => productType.ToString(),
                productType => (ProductType)Enum.Parse(typeof(ProductType), productType)
            );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseMySQL("Datasource=localhost;Database=shop;User=root;Password=root123;");
    }
}
