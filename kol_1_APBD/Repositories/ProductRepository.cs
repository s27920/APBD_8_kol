using kol_1_APBD.Exceptions;
using kol_1_APBD.Models;
using Microsoft.Data.SqlClient;

namespace kol_1_APBD.Repositories;

public interface IProductRepository
{
    public Task<Order> getOrderByIdAsync(int id);
    public Task<bool> deleteByIdAsync(int id);
}

public class ProductRepository : IProductRepository
{
    private IConfiguration _configuration;

    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Order> getOrderByIdAsync(int id)
    {
        await using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await connection.OpenAsync();
        var query =
            "SELECT * FROM \"Order\" " +
            "INNER JOIN Order_Product ON Order_Product.IdOrder = \"Order\".IdOrder " +
            "INNER JOIN Product ON Order_Product.IdProduct = Product.IdProduct " +
            "WHERE IdOrder = @IdOrder;";
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@IdOrder", id);
        
        await using var reader = await command.ExecuteReaderAsync();
        
        var orderNameOrdinal = reader.GetOrdinal("\"Order\".Name");
        var orderDescritionOrdinal = reader.GetOrdinal("Description");
        var orderDateOrdinal = reader.GetOrdinal("CreationDate");
        var orderClientIdOrdinal = reader.GetOrdinal("\"Order\".IdClient");
        
        var productNameOrdinal = reader.GetOrdinal("Product.Name");
        var productPriceOrdinal = reader.GetOrdinal("Price");
        var productIdOrdinal = reader.GetOrdinal("Product.IdProduct");

        if (reader.HasRows)
        {
            await reader.ReadAsync();
            string orderName = reader.GetString(orderNameOrdinal);
            string orderDescription = reader.GetString(orderDescritionOrdinal);
            DateTime orderCreationDate = reader.GetDateTime(orderDateOrdinal);
            int orderIdClient = reader.GetInt32(orderClientIdOrdinal);
            string productName = reader.GetString(productNameOrdinal);
            float productPrice = reader.GetFloat(productPriceOrdinal);
            int ProductIdProduct = reader.GetInt32(productIdOrdinal);
            List<Product> productList = new List<Product>();
            Product product = new Product { Name = productName, Price = productPrice, IdProduct = ProductIdProduct };
            productList.Add(product);
            while(await reader.ReadAsync())
            {
                
                productName = reader.GetString(productNameOrdinal);
                productPrice = reader.GetFloat(productPriceOrdinal);
                ProductIdProduct = reader.GetInt32(productIdOrdinal);
                product = new Product { Name = productName, Price = productPrice, IdProduct = ProductIdProduct };
                productList.Add(product);
                
            }
            return new Order {IdOrder = id, Name = orderName, Description = orderDescription, Products = productList, CreationDate = orderCreationDate, IdClient = orderIdClient};
        }

        return null;
    }

    public async Task<bool> deleteByIdAsync(int id)
    {
        await using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();
        try
        {
            var query = "DELETE FROM \"ORDER\" WHERE IdOrder = @IdOrder1; DELETE FROM Order_ProducT WHERE IdOrder = @IdOrder2;";
            await using var command = new SqlCommand(query, connection);
            
            command.Transaction = (SqlTransaction)transaction;
            var rowsAffected = await command.ExecuteNonQueryAsync();
            await transaction.CommitAsync();
            if (rowsAffected > 0)
            {
                return true;
            }
        }
        catch
        {
            await transaction.RollbackAsync();
            throw new ConflictException("transaction interrupted. Rollback initiated");
        }

        return false;
    }
}