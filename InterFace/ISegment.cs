using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface ISegment
    {
        Task<CustomResult> GetAllSegment();
        Task<CustomResult> GetAllSegmentTrue();
        Task<CustomResult> CreateSegment(Segment segment);

        Task<CustomResult> UpdateSegment(Segment segment);

        Task<CustomResult> GetById(int id);

        Task<CustomResult> Search(string? name,bool? status , int? categoryid , int? subcategoryid );

        Task<CustomResult> ChangeStatus(int id);
    }
}
