using Microsoft.EntityFrameworkCore;
using Project_sem3.Hubs;
using Project_sem3.Models;
using System.Data.SqlClient;
using System.Diagnostics;
using TableDependency.SqlClient;

namespace Project_sem3.SqlTableDependencies
{
   
    public class demoDependencies
    {
        private readonly DemoHubs _demohubs;
        SqlTableDependency<Discount> StoreDependen;
        private readonly IConfiguration _configuration;
      
        private string _connectionString;
        

        public demoDependencies(DemoHubs demoHubs, IConfiguration configuration )
        {
            _demohubs = demoHubs;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("MyConnection");
            

        }

        //public void Listen()
        //{
        //    try
        //    {
        //        StoreDependen = new SqlTableDependency<Discount>(_connectionString, "Discounts");
        //        StoreDependen.OnChanged += StoreDependen_OnChanged;
        //        StoreDependen.Start();
        //    }


        //    catch (SqlException ex)
        //    {
        //        Debug.WriteLine($"SQL Error: {ex}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("BBBBBBBBBBBBBBBBBBBBB " + ex.ToString());
        //    }


        //}

        //private async void StoreDependen_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Discount> e)
        //{

        //    if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Update || e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Insert)
        //    {

        //        await _demohubs.OnConnectedAsync();


        //    }
        //}

    }
}
