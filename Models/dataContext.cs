using Microsoft.EntityFrameworkCore;

namespace Project_sem3.Models
{
    public class dataContext : DbContext
    {
        public dataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Brand> Brands { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Discount> Discounts { get; set; }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Flash_Sale> Flash_Sales { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Order_Detail> Order_Details { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Properties> Properties { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Question_Reply> Question_Replies { get; set; }

        public DbSet<Rate> Rates { get; set; }

        public DbSet<Rate_Reply> Rate_Replies { get; set; }

        public DbSet<Segment> Segments { get; set; }
        public DbSet<Shipping> Shippings { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<Subcategory> subcategories { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Voucher> Vouchers { get; set; }

        public DbSet<VoucherUser> VoucherUsers { get; set; }

        public DbSet<Goods> Goods { get; set; }

        public DbSet<Product_Properties> Product_Properties { get; set; }

        public DbSet<Permissions> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Store>().ToTable(tb => tb.UseSqlOutputClause(false));

            modelBuilder.Entity<Cart>(c =>
            {

                c.HasOne(c => c.User).WithMany(c => c.Carts).HasForeignKey(c => c.UserId);
                c.HasOne(c => c.Properties).WithMany(c => c.Carts).HasForeignKey(c => c.PropertiesId);
            });

            modelBuilder.Entity<Admin>(c =>
            {
                c.HasOne(c => c.Store).WithMany(c => c.Admins).HasForeignKey(c => c.StoreId);
                c.HasData(new Admin[]
                {
                    new Admin { Id = 1,FullName = "Tran Quoc Viet", Email = "viet@gmail.com",Password = "$2a$12$YD4Zp42VAapdLtQGgNbmGOG5yzI7yJvQhwSlU63vgJmTpTkk77WDy",Phone = "0934522407",Role = "SAdmin",Create_at = DateTime.Now, Update_at = null , Status = true , Image = "null.jpg" , StoreId = null ,IsOnline = false }
                });
            });
            modelBuilder.Entity<Permissions>(c =>
            {
                c.HasOne(o => o.Admin).WithOne(o => o.Permissions).HasForeignKey<Permissions>(e => e.AdminId);
               
            });
            modelBuilder.Entity<Order>(o =>
            {
                o.HasKey(e => e.IdOrder);
                o.HasOne(o => o.User).WithMany(o => o.Orders).HasForeignKey(o => o.UserID);
              
                o.HasOne(o => o.Store).WithMany(o => o.Orders).HasForeignKey(o => o.StoreId);
              
                o.HasOne(o => o.Payment).WithOne(o => o.Order).HasForeignKey<Payment>(e=>e.OrderId);
            });
            modelBuilder.Entity<Order_Detail>(od =>
            {
              
                od.HasOne(od => od.Orders).WithMany(od => od.OrderDetails).HasForeignKey(o => o.OrederId).OnDelete(DeleteBehavior.Restrict);
                od.HasOne(od => od.Properties).WithMany(od => od.OrderDetails).HasForeignKey(o => o.PropertiesId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Product>(p =>
            {
                p.HasKey(p => p.ProductId);
                p.HasOne(p => p.Category).WithMany(p => p.Products).HasForeignKey(p => p.CategoryID);
                p.HasOne(p => p.Subcategory).WithMany(p => p.Products).HasForeignKey(p => p.SubCategoryId);
                p.HasOne(p => p.Segment).WithMany(p => p.Products).HasForeignKey(p => p.SegmentId);
                p.HasOne(p => p.Brand).WithMany(p => p.Products).HasForeignKey(p => p.BrandId);
                p.ToTable(tb => tb.UseSqlOutputClause(false));
            });

            modelBuilder.Entity<Properties>(p =>
            {
                p.HasOne(p => p.Product).WithMany(p => p.Properties).HasForeignKey(p => p.ProductId);
                p.HasOne(p => p.Store).WithMany(p => p.Properties).HasForeignKey(p => p.StoreId);
                p.HasOne(p => p.Discount).WithMany(p => p.Properties).HasForeignKey(p => p.DiscountId);
                p.HasOne(p => p.Flash_Sale).WithMany(p => p.Properties).HasForeignKey(p => p.FlashSaleId);
                p.ToTable(tb => tb.UseSqlOutputClause(false));
            });
            modelBuilder.Entity<Question>(q =>
            {
                q.HasOne(q => q.User).WithMany(q => q.Questions).HasForeignKey(q => q.UserId);
                q.HasOne(q => q.Products).WithMany(q => q.Questions).HasForeignKey(q => q.ProductId);
                q.ToTable(tb => tb.UseSqlOutputClause(false));
            });
            modelBuilder.Entity<Question_Reply>(qr =>
            {
                qr.HasOne(q => q.Admin).WithMany(q=>q.Question_Reply).HasForeignKey(q => q.AdminId).OnDelete(DeleteBehavior.Restrict);
                qr.HasOne(q => q.Question).WithOne(q => q.Question_Replies).HasForeignKey<Question_Reply>(q => q.QuestionId).OnDelete(DeleteBehavior.Restrict);
                qr.ToTable(tb => tb.UseSqlOutputClause(false));
            });
            modelBuilder.Entity<Discount>(q =>
            {
                q.ToTable(tb => tb.UseSqlOutputClause(false));
            });
            modelBuilder.Entity<Flash_Sale>(q =>
            {
                q.ToTable(tb => tb.UseSqlOutputClause(false));
            });
            modelBuilder.Entity<Voucher>(q =>
            {
                q.ToTable(tb => tb.UseSqlOutputClause(false));
            });

            modelBuilder.Entity<Rate>(r =>
            {
                r.HasOne(q => q.User).WithMany(q => q.Rates).HasForeignKey(q => q.UserId);
                r.HasOne(q => q.Products).WithMany(q => q.Rates).HasForeignKey(q => q.ProductId);
                r.HasOne(q => q.Order_Detail).WithOne(q => q.Rate).HasForeignKey<Rate>(e=>e.Order_detailId);
                r.ToTable(tb => tb.UseSqlOutputClause(false));
            });

            modelBuilder.Entity<Rate_Reply>(rp =>
            {
                rp.HasOne(q => q.Admin).WithMany(q=>q.Rate_Reply).HasForeignKey(q => q.AdminId).OnDelete(DeleteBehavior.Restrict);
                rp.HasOne(q => q.Rate).WithOne(q => q.Rate_Replies).HasForeignKey<Rate_Reply>(e=>e.RateId);
                rp.ToTable(tb => tb.UseSqlOutputClause(false));
            });
            modelBuilder.Entity<Segment>(s =>
            {
                s.HasOne(s => s.Subcategory).WithMany(s => s.Segments).HasForeignKey(s => s.SubCategoryId);
            });
            modelBuilder.Entity<Subcategory>(s =>
            {
                s.HasOne(s=>s.Category).WithMany(s=>s.Subcategories).HasForeignKey(s => s.CategoryId);
            });
            modelBuilder.Entity<VoucherUser>(v =>
            {
               
                v.HasOne(v=>v.Voucher).WithMany(v=>v.VoucherUsers).HasForeignKey(v=>v.VoucherId);
                v.HasOne(v => v.User).WithMany(v => v.VoucherUsers).HasForeignKey(v => v.UserId);
                v.ToTable(tb => tb.UseSqlOutputClause(false));
            });
            modelBuilder.Entity<Goods>(g =>
            {
                g.HasOne(v => v.Properties).WithMany(v => v.Goods).HasForeignKey(v => v.PropertiesId);
            });
            modelBuilder.Entity<Product_Properties>(g =>
            {
                g.HasOne(v => v.Product).WithMany(v => v.Product_Properties).HasForeignKey(v => v.ProductId);
            });
         
        }
    }
}
