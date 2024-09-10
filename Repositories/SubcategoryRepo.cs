using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Repositories
{
    public class SubcategoryRepo : ISubcategory
    {
        private readonly dataContext _dataContext;
        public SubcategoryRepo(dataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<CustomResult> ChangeStatus(int id)
        {
            try {
                var data = await _dataContext.subcategories.SingleOrDefaultAsync(x => x.Id == id);
                if (data != null)
                {
                    data.Status = ! data.Status;
                    data.Update_at = DateTime.Now;
                    _dataContext.subcategories.Update(data);
                    await _dataContext.SaveChangesAsync();
                    return new CustomResult()
                    {
                        Status = 200,
                        Message="Change Status Subcategory Success!",
                        data = data
                        
                    };
                }
                else {
                    return new CustomResult()
                    {
                         
                              Status = 205,
                              Message = "Subcategory Not Found",
                                                    
                };
                }
            }

            catch (Exception ex) {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error " + ex.Message,
                };
            }
        }

        public async Task<CustomResult> CreateCategory(Subcategory subcategory)
        {
            try { 
                var checkSubCategorycode = await _dataContext.subcategories.SingleOrDefaultAsync(e=>e.Name.ToLower() == subcategory.Name.ToLower() && e.CategoryId ==subcategory.CategoryId);
                if(checkSubCategorycode != null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Duplicate Subcategory Name !",
                        data = subcategory
                    };
                }
                subcategory.Create_at = DateTime.Now;
                subcategory.Status = false;
                _dataContext.subcategories.Add(subcategory);
                await _dataContext.SaveChangesAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Create Subcategory Success!",
                    data = subcategory
                };
            }
            catch (Exception ex) {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error " + ex.Message,
                };
            }
        }

        public async Task<CustomResult> GetAllSubcategoryTrue()
        {
            try {
                var list = await _dataContext.subcategories.Include("Category").Where(e=>e.Status==true).ToListAsync();
               
                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Get Subcategory Success!",
                        data = list
                    };
            }
            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error " + ex.Message,
                };
            }
        }

        public async Task<CustomResult> GetAllSubcategory()
        {
            try
            {
                var list = await _dataContext.subcategories.Include("Category").ToListAsync();

                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get Subcategory Success!",
                    data = list
                };
            }
            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error " + ex.Message,
                };
            }
        }

        public async Task<CustomResult> GetById(int id)
        {
            try { 
                var data = await _dataContext.subcategories.Include("Category").SingleOrDefaultAsync(e=>e.Id == id);
                if (data == null)
                {
                    return new CustomResult() { Status = 205,Message = "Subcategory Not Found!" };

                }
                return new CustomResult() { Status = 200 , Message = "Get Subcategory Success!",data = data};
            }
            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error " + ex.Message,
                };
            }
        }

       

       

        public async Task<CustomResult> UpdateSubCategory(Subcategory subcategory)
        {
            try {
                
                var dataOld = await _dataContext.subcategories.SingleOrDefaultAsync(e=>e.Id==subcategory.Id);
               
                if(dataOld != null)
                {
                    if (dataOld.Name.ToLower() != subcategory.Name.ToLower()) { 
                        var data = await _dataContext.subcategories.SingleOrDefaultAsync(e=>e.Name.ToLower()==subcategory.Name.ToLower()&& e.CategoryId==subcategory.CategoryId);
                        if (data != null)
                        {
                            return new CustomResult()
                            {
                                Status = 205,
                                Message = "Duplicate Subcategory Name !",
                                data = subcategory
                            };
                        }
                    }
                    dataOld.Update_at= DateTime.Now;    
                  
                    dataOld.CategoryId = subcategory.CategoryId;
                    dataOld.Name = subcategory.Name;
                    _dataContext.subcategories.Update(dataOld);
                    await _dataContext.SaveChangesAsync();
                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Update Subcategory Success!",
                        data = dataOld
                    };
                }
                else
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Update Fail Subcategory Not Found!",
                        data = subcategory
                    };
                }
            }
            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error " + ex.Message,
                };
            }
        }

        public async Task<CustomResult> Search(string? name, bool? status , int? categoryId)
        {
            try
            {
                var query = _dataContext.subcategories.AsQueryable();

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(s => s.Name.Contains(name));

                if (categoryId>0)
                    query = query.Where(s => s.CategoryId == categoryId);



                if (status.HasValue)
                    query = query.Where(s => s.Status == status.Value);

                var result = await query.Include("Category").ToListAsync();
               

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
    }
}
