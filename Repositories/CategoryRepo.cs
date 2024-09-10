using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;
using System.Collections.Generic;

namespace Project_sem3.Repositories
{
    public class CategoryRepo : ICategory
    {
        private readonly dataContext _datacontext;

        public CategoryRepo(dataContext dataContext)
        {
            _datacontext = dataContext;
        }
        public async Task<CustomResult> ChangeStatus(int id)
        {
            try
            {
                var data = await _datacontext.Categories.SingleOrDefaultAsync(e => e.Id == id);
                if (data == null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message="Record Not Found!",
                        data = null

                    };
                }
                else
                {
                    data.Status = !data.Status;
                    data.Update_at = DateTime.Now;
                    _datacontext.Categories.Update(data);
                    await _datacontext.SaveChangesAsync();
                    return new CustomResult()
                    {
                        Status = 200,
                        Message="Change Status Success!",
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

        public async Task<CustomResult> CreateCategory(Category category)
        {
           try
            {
                var dataCode = await _datacontext.Categories.SingleOrDefaultAsync(e => e.CodeCategory == category.CodeCategory);
                var dataName = await _datacontext.Categories.SingleOrDefaultAsync(e=>e.Name.ToLower() == category.Name.ToLower());
                if (dataCode != null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Create Category Fail , Duplicate Category Code!",
                        data = category,
                    };
                }
                if (dataName != null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Create Category Fail , Duplicate Category Name!",
                        data = category,
                    };
                }
                category.Create_at = DateTime.Now;
                category.Status = false;
                _datacontext.Categories.Add(category);
                await _datacontext.SaveChangesAsync();
               
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Create Category Success!",
                    data = category,
                }; 
            }
            catch (Exception ex)
            {
                
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " +ex.Message,

                }; 
            }
        }

        public async Task<CustomResult> GetAllCategoryTrue()
        {
            try
            {
                var list = await _datacontext.Categories.Where(e=>e.Status==true).ToListAsync();
              
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get All Category Success!",
                    data = list
                };
            }
            catch (Exception ex)
            {
               
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " +ex.Message,

                };
            }
        }
        public async Task<CustomResult> GetAllCategory()
        {
            try
            {
                var list = await _datacontext.Categories.ToListAsync();

                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get All Category Success!",
                    data = list
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

        public async Task<CustomResult> GetById(int id)
        {
            try
            {
                var data = await _datacontext.Categories.SingleOrDefaultAsync(e=>e.Id == id);
                if (data == null)
                {
                    return new CustomResult() { 
                        Status = 205,
                        Message = "Category Not Found"
                    };
                }
                return new CustomResult() { 
                    Status = 200,
                    Message = "Get Category Success!",
                    data = data
                    
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

        public async Task<CustomResult> Search(string? name, bool? status)
        {
            try
            {
                var query = _datacontext.Categories.AsQueryable();

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(s => s.Name.Contains(name));

                
                if (status.HasValue)
                    query = query.Where(s => s.Status == status.Value);

                var result = await query.ToListAsync();


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

        public async Task<CustomResult> UpdateCategory(Category category)
        {
            try
            {
                var dataOld = await _datacontext.Categories.SingleOrDefaultAsync(e=>e.Id == category.Id);
                if (dataOld != null)
                {
                    if (dataOld.Name.ToLower() != category.Name.ToLower())
                    {
                        var data = await _datacontext.Categories.SingleOrDefaultAsync(e=>e.Name.ToLower() == category.Name.ToLower());
                        if (data != null)
                        {
                            return new CustomResult()
                            {
                                Status = 205,
                                Message = "Duplicate Category Name "


                            };
                        }
                    }
                    dataOld.Update_at = DateTime.Now;
                    dataOld.Name = category.Name;
                    _datacontext.Categories.Update(dataOld);
                    await _datacontext.SaveChangesAsync();
                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Update Category Success!",
                        data = dataOld
                    };
                }
                else
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Update Fail Category Not Found!",
                        data = category
                        
                        
                    };
                }
          
            }
            catch(Exception ex)
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
