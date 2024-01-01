/*Цей контролер призначений для роботи з замовленнями користувача, і дія UserOrders очікує відображення 
 * списку замовлень на відповідній сторінці, при умові, що користувач аутентифікований (авторизований).*/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingCartMvcUI.Controllers
{
    //Цей атрибут зазначає, що доступ до всіх дій (actions) в цьому контролері має бути авторизованим
    [Authorize]
    //Визначаєм клас контролера, який успадковує базовий клас Controller.
    public class UserOrderController : Controller
    {
        /*Оголошуєм приватне поле _userOrderRepo типу IUserOrderRepository, яке представляє собою 
         * інтерфейс для роботи з замовленнями користувача.*/
        private readonly IUserOrderRepository _userOrderRepo;

        /*Конструктор контролера, який отримує параметром екземпляр сервісу IUserOrderRepository і 
         * ініціалізує поле _userOrderRepo. Це використовується для внедрення залежностей (Dependency 
         * Injection) та робить сервіс доступним у контролері.*/
        public UserOrderController(IUserOrderRepository userOrderRepo)
        {
            _userOrderRepo = userOrderRepo;
        }

        //Ця дія (action) обробляє запит для відображення замовлень користувача.
        public async Task<IActionResult> UserOrders()
        {
            //Виклик методу UserOrders() із _userOrderRepo для отримання замовлень користувача.
            var orders = await _userOrderRepo.UserOrders();
            //Повертаєм представлення (View) з отриманими замовленнями. 
            return View(orders);
        }
    }
}