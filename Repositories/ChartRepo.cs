using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Project_sem3.Repositories
{
    public class ChartRepo : IChart
    {
        private readonly dataContext _dataContext;
        public ChartRepo(dataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<CustomResult> ChartStorePerDay(int? id, DateTime date)
        {
            try
            {
                var HourInDay = Enumerable.Range(0, 24).ToList();
                var query = _dataContext.Payments.Include(e => e.Order).Where(e => EF.Functions.DateDiffDay(date, e.Create_at) == 0).AsQueryable();
                if (id > 0)
                {
                    query = query.Where(e => e.Order.StoreId == id);
                }
                var list = await query.GroupBy(e => e.Create_at.HasValue ? e.Create_at.Value.Hour : 0).Select(g => new
                {
                    Hour = g.Key,
                    RecordCount = g.Count()
                })
                   .ToListAsync();
                var result1 = HourInDay.GroupJoin(list, hour => hour, RecordCount => RecordCount.Hour, (hour, RecordCount) => new {
                    Hour = hour,
                    RecordCount = RecordCount.FirstOrDefault()?.RecordCount ?? 0
                }).ToList();
                return new CustomResult() { Status = 200, Message = "Success!", data = result1 };

            }

            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message,


                };
            }
        }

      

        public async Task<CustomResult> CompareStorePerDay(int? id, DateTime date1, DateTime date2, int? BrandId)
        {
            try
            {
                var HourInDay = Enumerable.Range(0, 24).ToList();
                var query = _dataContext.Order_Details.Include(e => e.Properties).ThenInclude(e => e.Product).Include(e => e.Orders).ThenInclude(e => e.Payment).AsQueryable();

                if (BrandId > 0)
                {
                    query = query.Where(e => e.Properties.Product.BrandId == BrandId);
                }
                if (id > 0)
                {
                    query = query.Where(e => e.Properties.StoreId == id);
                }
                var list1 = await query.Where(e =>EF.Functions.DateDiffDay(date1, e.Create_at) == 0).GroupBy(e => e.Create_at.HasValue ? e.Create_at.Value.Hour : 0)
                   .Select(g => new { Hour = g.Key, Sum = g.Sum(e => e.Orders.Payment.Revenue) })
                   .ToListAsync();
                var result1 = HourInDay.GroupJoin(list1, hour => hour, revenue => revenue.Hour, (hour, revenue) => new {
                    Hour = hour,
                    Sum = revenue.FirstOrDefault()?.Sum ?? 0
                }).ToList();

               

                var list2 = await query.Where(e =>EF.Functions.DateDiffDay(date2, e.Create_at) == 0).GroupBy(e => e.Create_at.HasValue ? e.Create_at.Value.Hour : 0)
                   .Select(g => new{Hour = g.Key,Sum = g.Sum(e => e.Orders.Payment.Revenue)})
                   .ToListAsync();
                var result2 = HourInDay.GroupJoin(list2, hour => hour, revenue => revenue.Hour, (hour, revenue) => new {
                    Hour = hour,
                    Sum = revenue.FirstOrDefault()?.Sum ?? 0
                }).ToList();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Success!",
                    data = new Dictionary<string, object>  {
                     { "list1", result1 },
                     { "list2", result2 }
                }
                };

            }

            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message,


                };
            }
        }

        public async Task<CustomResult> CompareStorePerMonth(int? id, DateTime date1, DateTime date2, int? BrandId)
        {
            try
            {
                var daysInMonth = Enumerable.Range(1, 31).ToList();
                var query = _dataContext.Order_Details.Include(e => e.Properties).ThenInclude(e => e.Product).Include(e => e.Orders).ThenInclude(e => e.Payment).AsQueryable();


                if (BrandId > 0)
                {
                    query = query.Where(e => e.Properties.Product.BrandId == BrandId);
                }
                if (id > 0)
                {
                    query = query.Where(e => e.Properties.StoreId == id);
                }

                var list1 = await query.Where(e =>EF.Functions.DateDiffMonth(date1, e.Create_at) == 0).GroupBy(e => e.Create_at.HasValue ? e.Create_at.Value.Day : 0)
                    .Select(g => new { Day = g.Key, Sum = g.Sum(e => e.Orders.Payment.Revenue) })
                    .ToListAsync();
                var result1 = daysInMonth.GroupJoin(list1, day => day,revenue => revenue.Day,(day,revenue)=> new  {Day = day,Sum = revenue.FirstOrDefault()?.Sum ?? 0}).ToList();

                var list2 = await query.Where(e =>EF.Functions.DateDiffMonth(date2, e.Create_at) == 0).GroupBy(e => e.Create_at.HasValue ? e.Create_at.Value.Day : 0)
                    .Select(g => new{Day = g.Key,Sum = g.Sum(e => e.Orders.Payment.Revenue)})
                    .ToListAsync();

                var result2 = daysInMonth.GroupJoin(list2, day => day, revenue => revenue.Day, (day, revenue) => new { Day = day, Sum = revenue.FirstOrDefault()?.Sum ?? 0 }).ToList();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Success!",
                    data = new Dictionary<string, object>  {
                     { "list1", result1 },
                     { "list2", result2 }
                }
                };

            }

            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message,


                };
            }
        }

        public async Task<CustomResult> CompareStoreBrandPerDay(int? id, DateTime date, int BrandId1, int BrandId2)
        {
            try
            {
                var HourInDay = Enumerable.Range(0, 24).ToList();
                var query = _dataContext.Order_Details.Include(e => e.Properties).ThenInclude(e => e.Product).Include(e => e.Orders).ThenInclude(e => e.Payment).AsQueryable();

                if (id > 0)
                {
                    query = query.Where(e => e.Properties.StoreId == id);
                }
                var list1 = await query.Where(e =>EF.Functions.DateDiffDay(date, e.Create_at) == 0 && e.Properties.Product.BrandId == BrandId1).GroupBy(e => e.Create_at.HasValue ? e.Create_at.Value.Hour : 0)
                   .Select(g => new { Hour = g.Key, Sum = g.Sum(e => e.Orders.Payment.Revenue) })
                   .ToListAsync();
                var result1 = HourInDay.GroupJoin(list1, hour => hour, revenue => revenue.Hour, (hour, revenue) => new {
                    Hour = hour,
                    Sum = revenue.FirstOrDefault()?.Sum ?? 0
                }).ToList();



                var list2 = await query.Where(e =>EF.Functions.DateDiffDay(date, e.Create_at) == 0 && e.Properties.Product.BrandId == BrandId2).GroupBy(e => e.Create_at.HasValue ? e.Create_at.Value.Hour : 0)
                  .Select(g => new { Hour = g.Key, Sum = g.Sum(e => e.Orders.Payment.Revenue) })
                  .ToListAsync();
                var result2 = HourInDay.GroupJoin(list2, hour => hour, revenue => revenue.Hour, (hour, revenue) => new {
                    Hour = hour,
                    Sum = revenue.FirstOrDefault()?.Sum ?? 0
                }).ToList();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Success!",
                    data = new Dictionary<string, object>  {
                     { "list1", result1 },
                     { "list2", result2 }
                }
                };

            }

            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message,


                };
            }
        }

        public async Task<CustomResult> CompareStoreBrandPerMonth(int? id, DateTime date, int BrandId1, int BrandId2)
        {
            try
            {
                var daysInMonth = Enumerable.Range(1, 31).ToList();
                var query = _dataContext.Order_Details.Include(e => e.Properties).ThenInclude(e => e.Product).Include(e => e.Orders).ThenInclude(e => e.Payment).AsQueryable();

                if (id > 0)
                {
                    query = query.Where(e => e.Properties.StoreId == id);
                }
                var list1 = await query.Where(e =>EF.Functions.DateDiffMonth(date, e.Create_at) == 0 && e.Properties.Product.BrandId == BrandId1).GroupBy(e => e.Create_at.HasValue ? e.Create_at.Value.Day : 0)
                   .Select(g => new { Day = g.Key, Sum = g.Sum(e => e.Orders.Payment.Revenue) })
                   .ToListAsync();
                var result1 = daysInMonth.GroupJoin(list1, day => day, revenue => revenue.Day, (day, revenue) => new {
                    Day = day,
                    Sum = revenue.FirstOrDefault()?.Sum ?? 0
                }).ToList();



                var list2 = await query.Where(e =>EF.Functions.DateDiffMonth(date, e.Create_at) == 0 && e.Properties.Product.BrandId == BrandId2).GroupBy(e => e.Create_at.HasValue ? e.Create_at.Value.Day : 0)
                  .Select(g => new { Day = g.Key, Sum = g.Sum(e => e.Orders.Payment.Revenue) })
                  .ToListAsync();
                var result2 = daysInMonth.GroupJoin(list2, day => day, revenue => revenue.Day, (day, revenue) => new {
                    Day = day,
                    Sum = revenue.FirstOrDefault()?.Sum ?? 0
                }).ToList();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Success!",
                    data = new Dictionary<string, object>  {
                     { "list1", result1 },
                     { "list2", result2 }
                }
                };

            }

            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message,


                };
            }
        }

        public async Task<CustomResult> CompareStore_StorePerDay(DateTime date, int Store1, int Store2)
        {
            try
            {
                var HourInDay = Enumerable.Range(0, 24).ToList();
                var query = _dataContext.Order_Details.Include(e => e.Properties).ThenInclude(e => e.Product).Include(e => e.Orders).ThenInclude(e => e.Payment).AsQueryable();

               
                var list1 = await query.Where(e => EF.Functions.DateDiffDay(date, e.Create_at) == 0 && e.Properties.StoreId == Store1).GroupBy(e => e.Create_at.HasValue ? e.Create_at.Value.Hour : 0)
                   .Select(g => new { Hour = g.Key, Sum = g.Sum(e => e.Orders.Payment.Revenue) })
                   .ToListAsync();
                var result1 = HourInDay.GroupJoin(list1, hour => hour, revenue => revenue.Hour, (hour, revenue) => new {
                    Hour = hour,
                    Sum = revenue.FirstOrDefault()?.Sum ?? 0
                }).ToList();



                var list2 = await query.Where(e => EF.Functions.DateDiffDay(date, e.Create_at) == 0 && e.Properties.StoreId == Store2).GroupBy(e => e.Create_at.HasValue ? e.Create_at.Value.Hour : 0)
                  .Select(g => new { Hour = g.Key, Sum = g.Sum(e => e.Orders.Payment.Revenue) })
                  .ToListAsync();
                var result2 = HourInDay.GroupJoin(list2, hour => hour, revenue => revenue.Hour, (hour, revenue) => new {
                    Hour = hour,
                    Sum = revenue.FirstOrDefault()?.Sum ?? 0
                }).ToList();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Success!",
                    data = new Dictionary<string, object>  {
                     { "list1", result1 },
                     { "list2", result2 }
                }
                };

            }

            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message,


                };
            }
        }

        public async Task<CustomResult> CompareStore_StorePerMonth(DateTime date, int Store1, int Store2)
        {
            try
            {
                var daysInMonth = Enumerable.Range(1, 31).ToList();
                var query = _dataContext.Order_Details.Include(e => e.Properties).ThenInclude(e => e.Product).Include(e => e.Orders).ThenInclude(e => e.Payment).AsQueryable();

               
                var list1 = await query.Where(e => EF.Functions.DateDiffMonth(date, e.Create_at) == 0 && e.Properties.StoreId == Store1).GroupBy(e => e.Create_at.HasValue ? e.Create_at.Value.Day : 0)
                   .Select(g => new { Day = g.Key, Sum = g.Sum(e => e.Orders.Payment.Revenue) })
                   .ToListAsync();
                var result1 = daysInMonth.GroupJoin(list1, day => day, revenue => revenue.Day, (day, revenue) => new {
                    Day = day,
                    Sum = revenue.FirstOrDefault()?.Sum ?? 0
                }).ToList();



                var list2 = await query.Where(e => EF.Functions.DateDiffMonth(date, e.Create_at) == 0 && e.Properties.StoreId == Store2).GroupBy(e => e.Create_at.HasValue ? e.Create_at.Value.Day : 0)
                  .Select(g => new { Day = g.Key, Sum = g.Sum(e => e.Orders.Payment.Revenue) })
                  .ToListAsync();
                var result2 = daysInMonth.GroupJoin(list2, day => day, revenue => revenue.Day, (day, revenue) => new {
                    Day = day,
                    Sum = revenue.FirstOrDefault()?.Sum ?? 0
                }).ToList();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Success!",
                    data = new Dictionary<string, object>  {
                     { "list1", result1 },
                     { "list2", result2 }
                }
                };

            }

            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message,


                };
            }
        }
    }
}
