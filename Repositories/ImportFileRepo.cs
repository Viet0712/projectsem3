using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Project_sem3.InterFace;
using Project_sem3.Models;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace Project_sem3.Repositories
{
    public class ImportFileRepo : IImportFile
    {
        private readonly dataContext _dataContext;
        public ImportFileRepo(dataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<CustomResult> ImportFileProduct(IFormFile file)
        {
            try {
                if (file == null || file.Length == 0)
                {
                    return new CustomResult() { 
                        Status = 205,
                        Message ="File Empty"
                    };
                }
                var _product = new List<Product>();
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var workbook = new XLWorkbook(stream))
                    {
                        var worksheet = workbook.Worksheet(1);
                        var rows = worksheet.RowsUsed();
                        var headerRow = worksheet.Row(1); // Giả sử hàng đầu tiên là header

                        // Tạo dictionary để lưu vị trí của các cột dựa trên tên cột
                        var columnMapping = new Dictionary<string, int>();

                        foreach (var cell in headerRow.Cells())
                        {
                            columnMapping[cell.GetString()] = cell.Address.ColumnNumber;
                        }

                        // Bắt đầu từ hàng thứ hai để bỏ qua header
                        foreach (var row in rows.Skip(1))
                        {
                            var entity = new Product
                            {
                             ProductId = row.Cell(columnMapping["ProductId"]).GetString(),

                             CategoryID = row.Cell(columnMapping["CategoryID"]).GetValue<int>(),
                             SubCategoryId = row.Cell(columnMapping["SubCategoryId"]).GetValue<int>(),
                             SegmentId= row.Cell(columnMapping["SegmentId"]).GetValue<int>(),
                             BrandId = row.Cell(columnMapping["BrandId"]).GetValue<int>(),
                             Status = false,
                             Name = row.Cell(columnMapping["Name"]).GetString(),
                             Description= row.Cell(columnMapping["Description"]).GetString(),
                             MadeIn = row.Cell(columnMapping["MadeIn"]).GetString(),
                             Weight = row.Cell(columnMapping["Weight"]).GetString(),
                             Volume = row.Cell(columnMapping["Volume"]).GetString(),
                             Shelf_life = row.Cell(columnMapping["Shelf_life"]).GetString(),
                             Expiry_date = row.Cell(columnMapping["PackingType"]).GetString(),
                             Image = row.Cell(columnMapping["Image"]).GetString(),
                            };
                            _product.Add(entity);
                        }
                    }
                }

                _dataContext.Products.AddRange(_product);
                await _dataContext.SaveChangesAsync();
                return new CustomResult() {
                    Status = 200,
                    Message="Import File Success!"
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
