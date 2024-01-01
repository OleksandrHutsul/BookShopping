/*Створення DTOs допомагає вам розділити логіку представлення та бізнес-логіку, і також робить код 
 * більш зрозумілим та легше підтримуваним. DTOs використовується для передачі об'єктів між 
 * різними частинами додатку та забезпечення незалежності від конкретної структури бази даних або 
 * моделі представлення.*/

namespace BookShoppingCartMvcUI.Models.DTOs
{
    public class BookDisplayModel
    {
        //Це властивість, яка містить колекцію об'єктів класу Book.
        public IEnumerable<Book> Books { get; set; }
        //Це властивість, яка містить колекцію об'єктів класу Genre.
        public IEnumerable<Genre> Genres { get; set; }
        public string STerm { get; set; } = "";
        public int GenreId { get; set; } = 0;
    }
}