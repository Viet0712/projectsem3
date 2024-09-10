using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_sem3.Repositories
{
  
    public class UserFERepo : IUserFE
    {
        private readonly dataContext db;
        private readonly PasswordHasher<User> passwordHasher;
        private IWebHostEnvironment _env;

        public UserFERepo(dataContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.passwordHasher = new PasswordHasher<User>();
            _env = env;
        }

        public async Task<int> UpdateCard(User user)
        {
            try
            {
                var acc = await db.Users.SingleOrDefaultAsync(u => u.Email == user.Email);
                if (acc != null)
                {
                    acc.Credit_card_number = user.Credit_card_number;
                   
                    acc.Credit_card_expiry = user.Credit_card_expiry;
                    var rs = await db.SaveChangesAsync(); 
                    if (rs > 0)
                    {
                        return 200;
                    }
                    return 201;
                }
                return 202;
            }
            catch (Exception ex) {
                return 500;
            }
        }

        public async Task<int> UpdateInfo(User user)
        {

            try
            {
                var acc = await db.Users.SingleOrDefaultAsync(u => u.Email == user.Email);
                if (acc != null)
                {
                    acc.Address = user.Address;
                    acc.FullName = user.FullName;
                    acc.Phone = user.Phone;
                    if (user.UploadImage != null)
                    {
                        var filename = GetUniqueFilename(user.UploadImage.FileName);
                        var upload = Path.Combine(_env.WebRootPath, "UserImage");
                        var filePath = Path.Combine(upload, filename);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await user.UploadImage.CopyToAsync(stream);
                        }


                        acc.Image = filename;


                    }
                  
                    var rs = await db.SaveChangesAsync();
                    if (rs > 0)
                    {
                        return 200;
                    }
                    return 201;
                }
                return 202;
            }
            catch (Exception ex)
            {
                return 500;
            }
        }

        public string GetUniqueFilename(string file)
        {
            file = Path.GetFileName(file);
            return Path.GetFileNameWithoutExtension(file) + "_" + DateTime.Now.Ticks + Path.GetExtension(file);
        }

        public async Task<int> UpdatePassword(User user)
        {
            try
            {
                var acc = await db.Users.SingleOrDefaultAsync(u => u.Email == user.Email);
                if (acc != null)
                {
                    if(acc.Password == "EMAIL")
                    {
                        return 203;
                    }
                    var result = passwordHasher.VerifyHashedPassword(acc, acc.Password, user.Password);

                    if (result == PasswordVerificationResult.Success)
                    {
                        acc.Password = passwordHasher.HashPassword(acc, user.Address);
                        await db.SaveChangesAsync();
                        return 200;
                    }

                    return 201;
                }
                return 202;
            }
            catch (Exception ex)
            {
                return 500;
            }
        }
    }
}
