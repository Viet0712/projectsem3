using Microsoft.EntityFrameworkCore;
using Project_sem3.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Project_sem3.SqlTableDependencies
{
    public class StoreHubRepo
    {
      
        private readonly IServiceProvider _serviceProvider;
        public StoreHubRepo(IServiceProvider serviceProvider)
        {
           
            _serviceProvider = serviceProvider;
        }
        //public List<Store> GetStores()
        //{
        //    //List<Store> result = new List<Store>();
        //    //var data = GetData();
        //    //foreach (DataRow item in data.Rows)
        //    //{
              
        //    //    var store = new Store()
        //    //    {
                    
        //    //        Id = int.Parse(item["Id"]?.ToString()!),
        //    //        Address = item["Address"]?.ToString() ?? string.Empty,
        //    //        City = item["City"]?.ToString() ?? string.Empty,
        //    //        District = item["District"]?.ToString() ?? string.Empty,
        //    //        Status = bool.Parse(item["Status"]?.ToString()!),
        //    //        Image =item["Image"]?.ToString(),

        //    //        //Admins = new List<Admin>()

        //    //    };
        //    //    //var adminIds = int.Parse(item["AdminId"]?.ToString()!);
        //    //    //if (adminIds != 0)
        //    //    //{

        //    //    //    var admin = new Admin()
        //    //    //    {
        //    //    //        Id = int.Parse(item["AdminId"]?.ToString()!),
        //    //    //        FullName = item["FullName"]?.ToString() ?? string.Empty,
        //    //    //        Email = item["Email"]?.ToString() ?? string.Empty,
        //    //    //        Phone = item["Phone"]?.ToString() ?? string.Empty,
        //    //    //        Role = item["Role"]?.ToString() ?? string.Empty,
        //    //    //        Status = bool.Parse(item["Status"]?.ToString()!),
        //    //    //    };
        //    //    //        store.Admins.Add(admin);
                    
        //    //    //}

        //    //    result.Add(store);
        //    //}
            

        //    //return result;

        //}
        public async Task<IEnumerable<Discount>> GetData()
        {
            //var query = "SELECT Stores.Id AS StoreId,Admins.Id AS AdminId, * FROM Stores INNER JOIN Admins ON Stores.Id = Admins.StoreId;";
            //var query = "Select * from Stores";
            //DataTable dataTable = new DataTable();
            //using(SqlConnection connection = new SqlConnection(_connectionString))
            //{
            //    try
            //    {
            //        connection.Open();
            //        using(SqlCommand command = new SqlCommand(query, connection))
            //        {
            //            using(SqlDataReader reader = command.ExecuteReader())
            //            {
            //                dataTable.Load(reader);
            //            }
            //        }
            //        return dataTable;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //    finally
            //    {
            //        connection.Close();

            //    }
            //}
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<dataContext>();
                var list = await dbContext.Discounts.ToListAsync();
                return list;
            }
        }
    }
}
