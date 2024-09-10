using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IVoucherFE
    {
        Task<IEnumerable<Voucher>> GetVouchers();
        Task<IEnumerable<Voucher>> GetVoucher(string email);
        Task<IEnumerable<VoucherUser>> GetVouchersCustomer(string email);

        Task<int> CollectVoucher(string email, int id);
    }
}
