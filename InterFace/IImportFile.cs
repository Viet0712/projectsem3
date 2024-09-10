using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IImportFile
    {
        Task<CustomResult> ImportFileProduct(IFormFile file);
    }
}
