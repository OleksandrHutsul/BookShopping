namespace BookShoppingCartMvcUI.Repositories
{
    public interface IUserOrderRepository
    {
        //Цей метод повинен повертати об'єкт Task<IEnumerable<Order>> і використовується
        //для отримання замовлень користувача.
        Task<IEnumerable<Order>> UserOrders();
    }
}