using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2013.Word;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;
using System.Data;

namespace Project_sem3.Repositories
{
    public class ReportRepo : IReport
    {
        private readonly dataContext _dataContext;
        public ReportRepo(dataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<FileResult> ExportDaily(DateTime date)
        {
            var order = await _dataContext.Orders.Include(e => e.User).Include(e => e.Payment).Include(e => e.Store).Where(e => EF.Functions.DateDiffDay(date, e.Create_at) == 0).ToListAsync();

            return GenerateExcelDaily(order);

        }

        public async Task<FileResult> ExportMonthly(DateTime date)
        {
            var order = await _dataContext.Orders.Include(e => e.User).Include(e => e.Payment).Include(e => e.Store).Where(e => EF.Functions.DateDiffMonth(date, e.Create_at) == 0).ToListAsync();

            return GenerateExcelDaily(order);
        }



        private FileResult GenerateExcelDaily(IEnumerable<Order> orders)
        {
            DataTable dataTable = new DataTable("DailyReport");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("IdOrder"),
                new DataColumn("User"),
                new DataColumn("Store"),
                new DataColumn("Type Voucher"),
                new DataColumn("Volume Voucher"),
                new DataColumn("Address"),
                new DataColumn("Phone"),
                new DataColumn("Recipient"),
                new DataColumn("Total"),
                new DataColumn("Revenue"),
                new DataColumn("Status"),
                new DataColumn("Create at"),
            });

            foreach (var order in orders)
            {
                DataRow row = dataTable.NewRow();

                // Gán giá trị cho từng cột cụ thể
                row["IdOrder"] = order.IdOrder;
                row["User"] = order.User.FullName;
                row["Store"] = $"{order.Store.Address}, {order.Store.District}, {order.Store.City}";
                row["Type Voucher"] = order.TypeVoucher;
                row["Volume Voucher"] = order.VolumeVoucher;
                row["Address"] = order.Address;
                row["Phone"] = order.Phone;
                row["Recipient"] = order.Name;
                row["Total"] = order.Total;
                row["Revenue"] = order.Payment.Revenue;
                row["Status"] = order.Status;
                row["Create at"] = order.Create_at;

                // Thêm hàng vào DataTable
                dataTable.Rows.Add(row);
            }


            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    var content = stream.ToArray();
                    return new FileContentResult(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = "DailyReport.xlsx"
                    };
                }
            }

        }

        //private DataTable CreateStockDataTable(IEnumerable<Order> orders, string tableName)
        //{
        //    DataTable dataTable = new DataTable(tableName);
        //    dataTable.Columns.AddRange(new DataColumn[]
        //    {
        //        new DataColumn("IdOrder"),
        //        new DataColumn("User"),
        //        new DataColumn("Store"),
        //        new DataColumn("Type Voucher"),
        //        new DataColumn("Volume Voucher"),
        //        new DataColumn("Address"),
        //        new DataColumn("Phone"),
        //        new DataColumn("Recipient"),
        //        new DataColumn("Total"),
        //        new DataColumn("Revenue"),
        //        new DataColumn("Status"),
        //        new DataColumn("Create at"),
        //    });

        //    foreach (var order in orders)
        //    {
        //        DataRow row = dataTable.NewRow();

        //        row["IdOrder"] = order.IdOrder;
        //        row["User"] = order.User.FullName;
        //        row["Store"] = $"{order.Store.Address}, {order.Store.District}, {order.Store.City}";
        //        row["Type Voucher"] = order.TypeVoucher;
        //        row["Volume Voucher"] = order.VolumeVoucher;
        //        row["Address"] = order.Address;
        //        row["Phone"] = order.Phone;
        //        row["Recipient"] = order.Name;
        //        row["Total"] = order.Total;
        //        row["Revenue"] = order.Payment.Revenue;
        //        row["Status"] = order.Status;
        //        row["Create at"] = order.Create_at;

        //        dataTable.Rows.Add(row);
        //    }

        //    return dataTable;
        //}


        public async Task<FileResult> GetStockByStore(int StoreId)
        {
           
                var data = await _dataContext.Goods.Include(e=>e.Properties).ThenInclude(e=>e.Product).ThenInclude(e=>e.Brand).Where(e=>e.Properties.StoreId == StoreId).ToListAsync();
                return GenerateExcelStockByStore(data);


        }
        private FileResult GenerateExcelStockByStore(IEnumerable<Goods> datas)
        {
            DataTable dataTable = new DataTable("DailyReport");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Product"),
                new DataColumn("Type"),
                new DataColumn("Brand"),
                new DataColumn("Cost"),
                new DataColumn("Price"),
                new DataColumn("Stock"),
                new DataColumn("Entry Date"),
                new DataColumn("Expiried Date"),
                new DataColumn("Status"),
               
            });

            foreach (var item in datas)
            {
                DataRow row = dataTable.NewRow();

                // Gán giá trị cho từng cột cụ thể
                row["Product"] = item.Properties.Product.Name;
                row["Type"] = item.Properties.Name;
                row["Brand"] = item.Properties.Product.Brand.Name;
                row["Cost"] = item.Properties.CostPrice;
                row["Price"] = item.Properties.Price;
                row["Stock"] = item.Stock;
                row["Entry Date"] =item.Arrival_date;
                row["Expiried Date"] = item.Expiry_date;
                row["Status"] = item.Status;
               

                // Thêm hàng vào DataTable
                dataTable.Rows.Add(row);
            }


            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    var content = stream.ToArray();
                    return new FileContentResult(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = "Stock.xlsx"
                    };
                }
            }

        }

        private FileResult GenerateExcelStock(IEnumerable<Goods> datas)
        {
            DataTable dataTable = new DataTable("DailyReport");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Store"),
                new DataColumn("Product"),
                new DataColumn("Type"),
                new DataColumn("Brand"),
                new DataColumn("Cost"),
                new DataColumn("Price"),
                new DataColumn("Stock"),
                new DataColumn("Entry Date"),
                new DataColumn("Expiried Date"),
                new DataColumn("Status"),

            });

            foreach (var item in datas)
            {
                DataRow row = dataTable.NewRow();

                // Gán giá trị cho từng cột cụ thể
                row["Store"] = item.Properties.Store.Address + ", " + item.Properties.Store.District +", " + item.Properties.Store.City;
                row["Product"] = item.Properties.Product.Name;
                row["Type"] = item.Properties.Name;
                row["Brand"] = item.Properties.Product.Brand.Name;
                row["Cost"] = item.Properties.CostPrice;
                row["Price"] = item.Properties.Price;
                row["Stock"] = item.Stock;
                row["Entry Date"] = item.Arrival_date;
                row["Expiried Date"] = item.Expiry_date;
                row["Status"] = item.Status;


                // Thêm hàng vào DataTable
                dataTable.Rows.Add(row);
            }


            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    var content = stream.ToArray();
                    return new FileContentResult(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = "Stock.xlsx"
                    };
                }
            }

        }

        public async Task<FileResult> GetStock()
        {
            var data = await _dataContext.Goods.Include(e => e.Properties).ThenInclude(e => e.Product).ThenInclude(e => e.Brand).Include(e=>e.Properties).ThenInclude(e=>e.Store).ToListAsync();
            return GenerateExcelStock(data);
        }
    }
}
