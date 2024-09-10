using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReport _reportRepo;
        public ReportController(IReport IReport)
        {
            _reportRepo = IReport;
        }

        [HttpGet("Daily")]
        [Authorize(Roles = "SAdmin")]
        public async Task<FileResult> ExportOrder([FromQuery] DailyReportParams reportParams)
        {

            try
            {
                var result = await _reportRepo.ExportDaily(reportParams.date);

                return result;
            }
           
           catch (Exception ex)
            {
                throw ex.InnerException;
            }
           
        }
        [HttpGet("Monthly")]
        [Authorize(Roles = "SAdmin")]
        public async Task<FileResult> ExportMonthly([FromQuery] DailyReportParams reportParams)
        {

            try
            {
                var result = await _reportRepo.ExportMonthly(reportParams.date);

                return result;
            }

            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }
        [HttpGet("StockByStore/{StoreId}")]
        [Authorize(Roles = "Admin")]
        public async Task<FileResult> ExportStockByStore(int StoreId)
        {

            try
            {
                var result = await _reportRepo.GetStockByStore(StoreId);

                return result;
            }

            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }

        [HttpGet("Stock")]
        [Authorize(Roles = "SAdmin")]
        public async Task<FileResult> ExportStock()
        {

            try
            {
                var result = await _reportRepo.GetStock();

                return result;
            }

            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }
        public class DailyReportParams
        {

            public DateTime date { get; set; }
        }
    }
}
