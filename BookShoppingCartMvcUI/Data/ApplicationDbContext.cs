/*Цей код представляє клас ApplicationDbContext, який є підкласом IdentityDbContext. 
 * IdentityDbContext визначає структуру бази даних для системи ідентифікації та автентифікації 
 * ASP.NET Core Identity, а ApplicationDbContext додає деякі додаткові таблиці */

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        /*Цей конструктор приймає параметри DbContextOptions<ApplicationDbContext> options та 
         * передає їх базовому класу. Ці параметри надають конфігурацію для підключення до бази 
         * даних, так що ASP.NET Core знає, яку базу даних використовувати та як з нею взаємодіяти.*/
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Кожна властивість DbSet<> визначає таблицю в базі даних для конкретної сутності. 
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> orderStatuses { get; set; }
    }
}