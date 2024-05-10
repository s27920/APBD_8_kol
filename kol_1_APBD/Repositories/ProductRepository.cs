using kol_1_APBD.Models;
using Microsoft.Data.SqlClient;

namespace kol_1_APBD.Repositories;

public interface IProductRepository
{
    public Task<Order> getOrderByIdAsync(int id);
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
            reader.Read();
            string orderName = reader.GetString(productNameOrdinal);
            string orderDescription = reader.GetString(orderDescritionOrdinal);
            DateTime orderCreationDate = reader.GetDateTime(orderDateOrdinal);
            int orderIdClient = reader.GetInt32(orderClientIdOrdinal);
            string productName = reader.GetString(productNameOrdinal);
            float productPrice = reader.GetFloat(productPriceOrdinal);
            int ProductIdProduct = reader.GetInt32(productIdOrdinal);
        }
    }
}