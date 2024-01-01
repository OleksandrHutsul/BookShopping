using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            //екземпляр ApplicationDbContext, який представляє з'єднання з базою даних.
            _db = db;
            //екземпляр UserManager<IdentityUser>, який використовується для управління користувачами,
            //такими як отримання ідентифікатора користувача.
            _userManager = userManager;
            //Це інтерфейс, який надає доступ до поточного HTTP - контексту
            _httpContextAccessor = httpContextAccessor;
        }
        //Цей метод призначений для додавання одного елемента в корзину покупок користувача.
        public async Task<int> AddItem(int bookId, int qty)
        {
            //Отримуєм ідентифікатора користувача
            string userId = GetUserId();
            /*Використовуєм BeginTransaction(), щоб створити транзакцію бази даних. Це гарантує 
             * атомарність операцій, тобто усі операції виконуються або всі успішно, або жодна з них 
             * не виконується.*/
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    //Якщо ідентифікатор користувача порожній (не залогований користувач), викидається виняток.
                    if (string.IsNullOrEmpty(userId))
                        throw new Exception("user is not logged-in");
                    ////Отримуєм корзину покупок для користувача
                    var cart = await GetCart(userId);
                    //Якщо корзина покупок не існує, створює новий об'єкт корзини покупок та додає
                    //його до бази даних.
                    if (cart is null)
                    {
                        cart = new ShoppingCart
                        {
                            UserId = userId
                        };
                        _db.ShoppingCarts.Add(cart);
                    }
                    //Зберігаєм зміни в базі даних, зокрема, новостворену корзину покупок.
                    _db.SaveChanges();
                    //Отримуєм деталь корзини покупок для вказаного bookId та ідентифікатора корзини
                    //покупок користувача.
                    var cartItem = _db.CartDetails
                                      .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                    //Якщо елемент уже існує в корзині, збільшує його кількість на вказану кількість 
                    if (cartItem is not null)
                    {
                        cartItem.Quantity += qty;
                    }
                    else //Якщо елемент не існує в корзині, створює новий об'єкт деталі корзини покупок
                         //та додає його до бази даних.
                    {
                        var book = _db.Books.Find(bookId);

                        cartItem = new CartDetail
                        {
                            BookId = bookId,
                            ShoppingCartId = cart.Id,
                            Quantity = qty,
                            UnitPrice = book.Price
                        };
                        _db.CartDetails.Add(cartItem);
                    }
                    //Зберігаєм зміни в базі даних, зокрема, новостворену корзину покупок.
                    _db.SaveChanges();
                    //Завершуєм транзакцію, виконуючи фіксацію всіх змін у базі даних.
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                }
            }
            //Отримує кількість елементів у корзині покупок
            var cartItemCount = await GetCartItemCount(userId);
            //Повертає кількість елементів у корзині покупок після додавання нового елемента.
            return cartItemCount;
        }

        //Цей метод призначений для видалення одного елемента з корзини покупок користувача.
        public async Task<int> RemoveItem(int bookId)
        {
            //Отримуєм ідентифікатора користувача
            string userId = GetUserId();
            try
            {
                //Якщо ідентифікатор користувача порожній (не залогований користувач), викидається виняток.
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                //Отримуєм корзину покупок для користувача
                var cart = await GetCart(userId);
                //Перевіряєм, чи корзина покупок не є порожньою. Якщо порожня, генерує виняток.
                if (cart is null)
                    throw new Exception("Invalid cart");
                /*Отримуєм деталь корзини покупок для вказаного bookId та ідентифікатора 
                 * корзини покупок користувача.*/
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                //Перевіряєм, чи елемент існує у корзині. Якщо ні, генерує виняток.
                if (cartItem is null)
                    throw new Exception("Not items in cart");
                //Якщо кількість елементів у корзині більше 1, зменшує кількість на 1.
                else if (cartItem.Quantity == 1)
                    _db.CartDetails.Remove(cartItem);
                else //Якщо кількість елементів у корзині більше 1, зменшує кількість на 1.
                    cartItem.Quantity--;
                //Зберігаєм зміни в базі даних.
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error");
            }
            //Отримуєм кількість елементів у корзині покупок
            var cartItemCount = await GetCartItemCount(userId);
            //Повертаєм кількість елементів у корзині покупок після видалення одного елемента.
            return cartItemCount;
        }

        //Оголошуєм метод, який повертає Task<ShoppingCart>. Це асинхронний метод,
        //який отримує об'єкт корзини покупок для поточного користувача.
        public async Task<ShoppingCart> GetUserCart()
        {
            //Отримуєм ідентифікатора користувача за допомогою методу GetUserId.
            var userId = GetUserId();
            //Якщо ідентифікатор користувача порожній (не залогований користувач), викидається виняток.
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "Invalid userid");
            //Викликаєм асинхронний метод FirstOrDefaultAsync, щоб отримати перший об'єкт
            //корзини покупок для вказаного користувача.
            var shoppingCart = await _db.ShoppingCarts
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Book)
                                  .ThenInclude(a => a.Genre)
                                  .Where(a => a.UserId == userId)
                                  .FirstOrDefaultAsync();/*фільтруєм результати, обираючи лише 
                                                          * ті корзини покупок, які належать 
                                                          * поточному користувачеві.*/
            //Повертаєм отриманий об'єкт корзини покупок.
            return shoppingCart;

        }

        /*Оголошуєм метод GetCart, який повертає Task<ShoppingCart>. Це асинхронний метод, який 
         * отримує об'єкт корзини покупок для вказаного користувача за його ідентифікатором userId.*/
        public async Task<ShoppingCart> GetCart(string userId)
        {
            /*Викликаєм асинхронний метод FirstOrDefaultAsync, щоб отримати перший елемент,
            який задовольняє умову в лямбда-виразі( ця умова порівнює поле UserId 
            об'єкта корзини покупок з переданим значенням userId.)*/
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);

            /*Повертаєм отриманий об'єкт корзини покупок. Якщо корзина для вказаного користувача 
             * не знайдена (або користувач не ідентифікований), повертається null.*/
            return cart;
        }

        /*Оголошуєм метод, який повертає Task<int>. Це асинхронний метод, що повертає кількість 
         * елементів у корзині покупок для вказаного користувача або загальну кількість, якщо 
         * userId не вказаний.*/
        public async Task<int> GetCartItemCount(string userId = "")
        {
            //Перевіряєм, чи userId не є порожнім або null. Якщо userId порожній, викликає метод
            //GetUserId() для отримання ідентифікатора поточного користувача.
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            //Створюєм LINQ-запит для отримання даних з корзин покупок та деталей корзин покупок.
            //використовуєм метод ToListAsync() для отримання списку результатів.
            var data = await (from cart in _db.ShoppingCarts
                              join cartDetail in _db.CartDetails //Здійснюєм з'єднання двох таблиць
                                                                 //(ShoppingCarts та CartDetails) за
                                                                 //умовою, що Id корзини покупок
                                                                 //дорівнює ShoppingCartId в деталях
                                                                 //корзини покупок.
                              on cart.Id equals cartDetail.ShoppingCartId
                              select new { cartDetail.Id } //Вибираєм ідентифікатори деталей корзини
                                                           //покупок для кожної пари (ShoppingCart,
                                                           //CartDetail) та утворює анонімний об'єкт
                                                           //з вибраними ідентифікаторами.
                        ).ToListAsync();
            //Повертаєм кількість отриманих елементів (ідентифікаторів деталей корзини покупок), які
            //були підраховані з вищезазначеного LINQ-запиту.
            return data.Count;
        }

        public async Task<bool> DoCheckout()
        {
            /*Використовуєм BeginTransaction(), щоб створити транзакцію бази даних. Це гарантує 
             * атомарність операцій, тобто усі операції виконуються або всі успішно, або жодна з них 
             * не виконується.*/
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    //Отримуєм ідентифікатора користувача за допомогою методу GetUserId.
                    var userId = GetUserId();
                    //Якщо ідентифікатор користувача порожній (не залогований користувач), викидається виняток.
                    if (string.IsNullOrEmpty(userId))
                        throw new Exception("User is not logged-in");
                    //Отримуєм корзину покупок для користувача за допомогою методу GetCart.
                    var cart = await GetCart(userId);
                    //Перевіряєм, чи корзина покупок не є порожньою. Якщо порожня, генерує виняток.
                    if (cart is null)
                        throw new Exception("Invalid cart");
                    //Отримуєм деталі корзини покупок для відповідної корзини.
                    var cartDetail = _db.CartDetails
                                        .Where(a => a.ShoppingCartId == cart.Id).ToList();
                    //Перевіряєм, чи деталі корзини покупок не є порожніми. Якщо порожні, генерує виняток.
                    if (cartDetail is null)
                        throw new Exception("Cart is empty");
                    //Створюєм новий об'єкт Order для створення нового замовлення.
                    var order = new Order
                    {
                        UserId = userId,
                        CreateDate = DateTime.UtcNow,
                        OrderStatusId = 1//pending
                    };
                    //Додає замовлення до бази даних і зберігає зміни.
                    _db.Orders.Add(order);
                    _db.SaveChanges();
                    //Переносим дані з деталей корзини покупок до деталей замовлення.
                    foreach (var item in cartDetail)
                    {
                        var orderDetail = new OrderDetail
                        {
                            BookId = item.BookId,
                            OrderId = order.Id,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice
                        };
                        _db.OrderDetails.Add(orderDetail);
                    }
                    //Зберігаєм зміни після додавання деталей замовлення.
                    _db.SaveChanges();
                    //Видаляєм деталі корзини покупок з бази даних.
                    _db.CartDetails.RemoveRange(cartDetail);
                    _db.SaveChanges();
                    //Завершуєм транзакцію, виконуючи фіксацію всіх змін у базі даних.
                    transaction.Commit();
                    return true;
                }
                //Обробляєм будь-який виняток, який може виникнути під час виконання методу.
                catch (Exception)
                {
                    // У випадку помилки, відміна транзакції і повернення значення false
                    transaction.Rollback();
                    return false;
                }
            }
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