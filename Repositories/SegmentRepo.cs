using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Repositories
{
    public class SegmentRepo : ISegment
    {
        private readonly dataContext _datacontext;
        public SegmentRepo(dataContext dataContext)
        {
            _datacontext = dataContext;
        }
        public async Task<CustomResult> ChangeStatus(int id)
        {
            try {
                var data = await _datacontext.Segments.SingleOrDefaultAsync(e=>e.Id == id);
                if (data == null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Segment Not Found!"
                    };
                }
                else
                {
                    data.Status = !data.Status;
                    _datacontext.Segments.Update(data);
                    await _datacontext.SaveChangesAsync();
                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Change Status Success!",
                        data = data
                    };
                }
            }
            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message
                };
            }
        }

        public async Task<CustomResult> CreateSegment(Segment segment)
        {
            try {
                var data = await _datacontext.Segments.SingleOrDefaultAsync(e=>e.Name.ToLower() == segment.Name.ToLower() && e.SubCategoryId==segment.SubCategoryId);
                if (data != null)
                {
                    return new CustomResult() { Status = 205 , Message = "Duplicate Name"};
                }
                segment.Create_at = DateTime.Now;
                segment.Status = false;
                _datacontext.Segments.Add(segment);
                await _datacontext.SaveChangesAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Create Segment Success!",
                    data = segment
                };
            }
            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message
                };
            }
        }

        public async Task<CustomResult> GetAllSegmentTrue()
        {
            try {
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get Segment  Success!",
                    data = await _datacontext.Segments.Include(e=>e.Subcategory).ThenInclude(e=>e.Category).Where(e=>e.Status==true).ToListAsync()

                };
            }
            catch (Exception ex) {

                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message
                };
            }
        }

        public async Task<CustomResult> GetAllSegment()
        {
            try
            {
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get Segment  Success!",
                    data = await _datacontext.Segments.Include(e => e.Subcategory).ThenInclude(e => e.Category).ToListAsync()

                };
            }
            catch (Exception ex)
            {

                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message
                };
            }
        }

        public async Task<CustomResult> GetById(int id)
        {
            try {
                var data = await _datacontext.Segments.Include(e=>e.Subcategory).ThenInclude(e=>e.Category).SingleOrDefaultAsync(e => e.Id == id);
                if (data == null)
                {
                    return new CustomResult() {Status = 205, Message = "Segment Not Found!" };
                }
                return new CustomResult() { Status = 200, Message = "Get Segment Success!", data = data };
            }
            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message
                };
            }
        }

        public async Task<CustomResult> Search(string? name, bool? status, int? categoryid, int? subcategoryid)
        {
            try
            {
                var query = _datacontext.Segments.AsQueryable();

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(s => s.Name.Contains(name));

                if (subcategoryid > 0)
                    query = query.Where(s => s.SubCategoryId == subcategoryid);



                if (status.HasValue)
                    query = query.Where(s => s.Status == status.Value);

                var result = await query.Include(e=>e.Subcategory).ThenInclude(e=>e.Category).ToListAsync();

                if (categoryid > 0 && subcategoryid == null)
                {
                    List<Segment> resultFilter = new List<Segment>();
                   foreach(var item in result)
                    {
                        if(item.Subcategory.Category.Id == categoryid)
                            resultFilter.Add(item);
                    }
                    result = resultFilter;
                }

                return new CustomResult()
                {
                    Status = 200,
                    Message = "Search Success!",
                    data = result
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

        public async Task<CustomResult> UpdateSegment(Segment segment)
        {
            try {
                var dataOld = await _datacontext.Segments.SingleOrDefaultAsync(e => e.Id == segment.Id);
                if (dataOld == null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Segment Update Fail , Not Found!",
                        data = segment
                    };
                }
                else
                {
                    if (dataOld.Name.ToLower() != segment.Name.ToLower())
                    {
                        var data = await _datacontext.Segments.SingleOrDefaultAsync(e => e.Name.ToLower() == segment.Name.ToLower()&& e.SubCategoryId == segment.SubCategoryId);
                        if (data != null)
                        {
                            return new CustomResult() { Status = 205, Message = "Duplicate Name" };
                        }
                    }
                    dataOld.Update_at = DateTime.Now;
                  
                  
                    dataOld.Name = segment.Name;
                    _datacontext.Segments.Update(dataOld);
                    await _datacontext.SaveChangesAsync();
                    return new CustomResult() {

                        Status = 200,
                        Message = "Update Segment Success!",
                        data = dataOld
                    };
                }
            }
            catch (Exception ex) {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message
                };
            }

        }
    }
}
