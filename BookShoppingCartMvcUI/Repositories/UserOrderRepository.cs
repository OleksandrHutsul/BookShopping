//Цей код використовується для отримання та відображення замовлень конкретного користувача на сайті

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public UserOrderRepository(ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
             IHttpContextAccessor httpContextAccessor)
        {
            //екземпляр ApplicationDbContext, який представляє з'єднання з базою даних.
            _db = db;
            //Це інтерфейс, який надає доступ до поточного HTTP - контексту
            _httpContextAccessor = httpContextAccessor;
            //екземпляр UserManager<IdentityUser>, який використовується для управління користувачами,
            //такими як отримання ідентифікатора користувача.
            _userManager = userManager;
        }

        public async Task<IEnumerable<Order>> UserOrders()
        {
            //Отримання ідентифікатора користувача за допомогою методу GetUserId.
            var userId = GetUserId();
            //Якщо ідентифікатор користувача порожній (не залогований користувач), викидається виняток.
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");
            /*Запит до бази даних для отримання замовлень користувача за його ідентифікатором.
            Користуючись Include, здійснюється загрузка пов'язаних об'єктів (OrderStatus,
            OrderDetail, Book, Genre) для кожного замовлення.*/
            var orders = await _db.Orders
                            .Include(x => x.OrderStatus)
                            .Include(x => x.OrderDetail)
                            .ThenInclude(x => x.Book)
                            .ThenInclude(x => x.Genre)
                            .Where(a => a.UserId == userId)
                            .ToListAsync();
            //Повертається список замовлень користувача.
            return orders;
        }

        private string GetUserId()
        {
            //Отримання об'єкта ClaimsPrincipal з поточного HTTP-контексту.
            var principal = _httpContextAccessor.HttpContext.User;
            //Використання UserManager для отримання ідентифікатора користувача з принципала.
            string userId = _userManager.GetUserId(principal);
            //Значення ідентифікатора користувача повертається з методу.
            return userId;
        }
    }
}