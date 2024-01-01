using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingCartMvcUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPanelController : Controller
    {
        public IActionResult Index()
        {
            var model = new Book(); 
            return View(model);
        }

        private readonly ApplicationDbContext _context;

        public AdminPanelController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddBook(Book model)
        {
            ModelState.Remove("Genre");
            ModelState.Remove("OrderDetail");
            ModelState.Remove("CartDetail");
            ModelState.Remove("GenreName");

            if (ModelState.IsValid)
            {
                if (model.Image == null)
                {
                    model.Image = "noimage.png";
                }

                var book = new Book
                {
                    BookName = model.BookName,
                    Price = model.Price,
                    Image = model.Image,
                    AuthorName = model.AuthorName,
                    GenreId = model.GenreId,
                };

                _context.Books.Add(book);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            return View(model);
        }
    }
}
