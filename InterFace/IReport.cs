using Microsoft.AspNetCore.Mvc;

namespace Project_sem3.InterFace
{
    public interface IReport
    {
        Task<FileResult> ExportDaily(DateTime date);

        Task<FileResult> ExportMonthly(DateTime date);

        Task<FileResult> GetStockByStore(int StoreId);

        Task<FileResult> GetStock();
    }
}
