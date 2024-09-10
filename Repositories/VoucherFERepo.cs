using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Repositories
{
    public class VoucherFERepo : IVoucherFE
    {
        private readonly dataContext db;

        public VoucherFERepo(dataContext _dataContext)
        {
            this.db = _dataContext;
        }
        public async Task<IEnumerable<Voucher>> GetVouchers()
        {
            var currentDate = DateTime.Now;
            try
            {
                var list = await db.Vouchers
                    .Where(p=>p.Status == true && p.Quantity >0 &&
                    p.Start_at <= currentDate &&
                    p.Expiry_date >= currentDate)
                    .ToListAsync();
                return list;
            }catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Voucher>> GetVoucher(string email)
        {
            try
            {
                var currentDate = DateTime.Now;
                var user = await db.Users.SingleOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return null;
                }
                var userId = user.Id;
                var list = await db.Vouchers
                    .Where(v => !db.VoucherUsers.Any(vu => vu.VoucherId == v.Id && vu.UserId == userId) &&
                    v.Status == true &&
                     v.Quantity > 0 &&
                    v.Start_at <= currentDate &&
                    v.Expiry_date >= currentDate)
                    .ToListAsync();

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<VoucherUser>> GetVouchersCustomer(string email)
        {
            try
            {
                var currentDate = DateTime.Now;
                var user = await db.Users.SingleOrDefaultAsync(u=>u.Email == email);
                if (user == null)
                {
                    return null;
                }
                var list = await db.VoucherUsers
                        .Include(v=>v.Voucher)
                        .Where(p => p.UserId == user.Id && p.Voucher.Status == true && p.Status == true && p.Voucher.Start_at <= currentDate &&
                    p.Voucher.Expiry_date >= currentDate)
                        .ToListAsync();
                return list;
            }catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> CollectVoucher(string email, int id)
        {
            try
            {
                var user = await db.Users.SingleOrDefaultAsync(u=>u.Email == email);
                if(user == null)
                {
                    return 202;
                }
                var voucher = await db.Vouchers.SingleOrDefaultAsync(v=>v.Id == id);
                if(voucher == null)
                {
                    return 201;
                }
                if(voucher.Status != true || voucher.Quantity<= 0) {
                    return 203;
                }
                var voucheruser = new VoucherUser
                {
                    VoucherId = voucher.Id,
                    UserId = user.Id,
                    Status = true,
                    Create_at = DateTime.Now,
                    Update_at = DateTime.Now

                };
                db.VoucherUsers.Add(voucheruser);
              var rs=  await db.SaveChangesAsync();
                if (rs > 0)
                {
                    voucher.Quantity = voucher.Quantity - 1;
                    await db.SaveChangesAsync();
                    return 200;     
                }
                return 201;
            }catch (Exception ex)
            {
                return 500;
            }
        }
    }
}
