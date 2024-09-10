using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IChart
    {
        Task<CustomResult> ChartStorePerDay(int? id,DateTime date);

        Task<CustomResult> CompareStorePerDay(int? id, DateTime date1 , DateTime date2 ,int? BrandId);

        Task<CustomResult> CompareStorePerMonth(int? id, DateTime date1, DateTime date2, int? BrandId);

        Task<CustomResult> CompareStoreBrandPerDay(int? id, DateTime date, int BrandId1 , int BrandId2);

        Task<CustomResult> CompareStoreBrandPerMonth(int? id, DateTime date, int BrandId1, int BrandId2);

        Task<CustomResult> CompareStore_StorePerDay(DateTime date, int Store1, int Store2);

        Task<CustomResult> CompareStore_StorePerMonth(DateTime date, int Store1, int Store2);
    }
}
