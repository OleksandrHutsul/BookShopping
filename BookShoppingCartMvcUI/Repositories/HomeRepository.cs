//цей клас відповідає за взаємодію з базою даних для отримання інформації про жанри та книги,
//які будуть використовуватися на домашній сторінці веб-додатка.

using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            //екземпляр ApplicationDbContext, який представляє з'єднання з базою даних.
            _db = db;
        }

        //Цей метод отримує всі жанри книг з бази даних і повертає їх у вигляді списку.
        //Використовується асинхронний підхід для отримання даних.
        public async Task<IEnumerable<Genre>> Genres()
        {
            return await _db.Genres.ToListAsync();
        }

        /*Цей метод взаємодіє з базою даних для отримання списку книг з можливістю фільтрації за 
         * пошуковим терміном та ідентифікатором жанру.*/
        public async Task<IEnumerable<Book>> GetBooks(string sTerm = "", int genreId = 0)
        {
            //Це забезпечує реєстронезалежний пошук, оскільки порівнювання буде
            //виконуватися в нижньому регістрі.
            sTerm = sTerm.ToLower();
            /*Використовується LINQ-запит для об'єднання таблиць Books та Genres за умовою 
             * book.GenreId equals genre.Id. Застосовується фільтрація на основі пошукового 
             * терміну (sTerm). Якщо sTerm порожній або містить пробіли, фільтрація не використовується.
             * В іншому випадку використовується умова, що ім'я книги (book.BookName) має починатися з 
             * введеного терміну в нижньому регістрі. Результат LINQ-запиту представляється у 
             * вигляді об'єктів класу Book.*/
            IEnumerable<Book> books = await (from book in _db.Books
                                             join genre in _db.Genres
                                             on book.GenreId equals genre.Id
                                             where string.IsNullOrWhiteSpace(sTerm) || 
                                             (book != null && book.BookName.ToLower().StartsWith(sTerm))
                                             select new Book
                                             {
                                                 Id = book.Id,
                                                 Image = book.Image,
                                                 AuthorName = book.AuthorName,
                                                 BookName = book.BookName,
                                                 GenreId = book.GenreId,
                                                 Price = book.Price,
                                                 GenreName = genre.GenreName
                                             }
                         ).ToListAsync();
            /*Якщо genreId більше 0, виконується фільтрація списку книг за ідентифікатором жанру.
            В результаті використовується Where для відфільтрування книг, які мають ідентифікатор 
            жанру, який відповідає вказаному genreId. Результат фільтрації конвертується в список 
            за допомогою ToList().*/
            if (genreId > 0)
            {
                books = books.Where(a => a.GenreId == genreId).ToList();
            }
            //Метод повертає відфільтрований список книг.
            return books;
        }
    }
}