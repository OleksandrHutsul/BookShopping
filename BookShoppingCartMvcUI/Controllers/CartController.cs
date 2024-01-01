using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingCartMvcUI.Controllers
{
    //Цей атрибут зазначає, що доступ до всіх дій (actions) в цьому контролері має бути авторизованим
    [Authorize]
    //Визначаєм клас контролера, який успадковує базовий клас Controller.
    public class CartController : Controller
    {
        /*Оголошуєм приватне поле _cartRepo типу ICartRepository. Це використовується для 
         * створення екземпляра репозиторію корзини, який, використовується для взаємодії 
         * з даними корзини.*/
        private readonly ICartRepository _cartRepo;

        //Конструктор класу CartController, який отримує екземпляр репозиторію корзини
        //через Dependency Injection.
        public CartController(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        /*Дія (action) AddItem, яка додає елемент до корзини. Отримує параметри 
         * bookId (ідентифікатор книги), qty (кількість екземплярів) та 
         * redirect (значення для перенаправлення).*/
        public async Task<IActionResult> AddItem(int bookId, int qty = 1, int redirect = 0)
        {
            //Викликає метод _cartRepo.AddItem(bookId, qty) для додавання елемента до
            //корзини та отримання кількості елементів у корзині.
            var cartCount = await _cartRepo.AddItem(bookId, qty);
            //Якщо redirect дорівнює 0, повертає об'єкт Ok(cartCount) з кількістю
            //елементів у корзині у відповіді.
            if (redirect == 0)
                return Ok(cartCount);
            /*Якщо redirect не дорівнює 0, перенаправляє користувача на дію GetUserCart, 
             * щоб показати вміст корзини.*/
            return RedirectToAction("GetUserCart");
        }

        //Дія (action) RemoveItem, яка видаляє елемент з корзини за допомогою ідентифікатора книги bookId
        public async Task<IActionResult> RemoveItem(int bookId)
        {
            /*Викликає метод _cartRepo.RemoveItem(bookId) для видалення елемента з 
             * корзини та отримання кількості елементів у корзині.*/
            var cartCount = await _cartRepo.RemoveItem(bookId);
            /*Перенаправляє користувача на дію GetUserCart, щоб показати оновлений вміст корзини.*/
            return RedirectToAction("GetUserCart");
        }

        /*Дія (action) GetUserCart, яка отримує вміст корзини користувача та відображає його 
         * за допомогою відповідного представлення.*/
        public async Task<IActionResult> GetUserCart()
        {
            //Цей рядок коду викликає метод _cartRepo.GetUserCart(), який повертає об'єкт,
            //що представляє вміст корзини користувача.
            var cart = await _cartRepo.GetUserCart();
            //Повертає представлення з моделлю cart, яка представляє вміст корзини.
            return View(cart);
        }

        /*Дія (action) GetTotalItemInCart, яка отримує загальну кількість елементів у корзині користувача.*/
        public async Task<IActionResult> GetTotalItemInCart()
        {
            //Цей рядок коду викликає метод _cartRepo.GetCartItemCount(), який повертає кількість
            //елементів у корзині користувача.
            int cartItem = await _cartRepo.GetCartItemCount();
            //Повертає об'єкт Ok(cartItem) з кількістю елементів у корзині у відповіді.
            return Ok(cartItem);
        }

        //Дія (action) Checkout, яка виконує операцію оформлення замовлення (checkout).
        public async Task<IActionResult> Checkout()
        {
            //Викликає метод _cartRepo.DoCheckout() для виконання операції оформлення замовлення.
            bool isCheckedOut = await _cartRepo.DoCheckout();
            //Якщо операція оформлення замовлення не вдалася (зазвичай це може бути
            //пов'язано з помилкою на сервері), викидає виняток.
            if (!isCheckedOut)
                throw new Exception("Something happen in server side");
            //Перенаправляє користувача на дію Index контролера Home
            //після успішного оформлення замовлення.
            return RedirectToAction("Index", "Home");
        }
    }
}