namespace BookShoppingCartMvcUI
{
    public interface IHomeRepository
    {
        /*Цей метод визначає, що клас, який реалізує інтерфейс, повинен мати функціональність для о
         * тримання списку книг. Параметри sTerm та genreId дозволяють задавати пошуковий термін і 
         * ідентифікатор жанру для отримання відфільтрованого списку книг.*/
        Task<IEnumerable<Book>> GetBooks(string sTerm = "", int genreId = 0);
        //Цей метод визначає функціональність для отримання списку жанрів.
        Task<IEnumerable<Genre>> Genres();
    }
}