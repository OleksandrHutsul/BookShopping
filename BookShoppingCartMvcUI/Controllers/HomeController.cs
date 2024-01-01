using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookShoppingCartMvcUI.Controllers
{
    //Визначаєм клас контролера, який успадковує базовий клас Controller.
    public class HomeController : Controller
    {
        //використовується для ведення журналу
        private readonly ILogger<HomeController> _logger;
        //інтерфейс для отримання даних, пов'язаних із домашньою сторінкою.
        private readonly IHomeRepository _homeRepository;

        /*Конструктор класу HomeController, який отримує екземпляри ILogger<HomeController> і 
         * IHomeRepository через Dependency Injection. Це дозволяє внедрити залежності в контролер.*/
        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
            _logger = logger;
        }

        /* Дія (action) Index, яка обробляє запит для відображення домашньої сторінки. Отримує два 
         * параметри зі значеннями за замовчуванням: sterm (пошуковий термін) та genreId 
         * (ідентифікатор жанру).*/
        public async Task<IActionResult> Index(string sterm = "", int genreId = 0)
        {
            //Викликаєм метод _homeRepository.GetBooks(sterm, genreId) для отримання списку книг. 
            IEnumerable<Book> books = await _homeRepository.GetBooks(sterm, genreId);
            //Викликаєм метод _homeRepository.Genres() для отримання списку жанрів.
            IEnumerable<Genre> genres = await _homeRepository.Genres();
            /* Створюєм об'єкт BookDisplayModel та ініціалізує його властивості з отриманими 
             * списками книг та жанрів, а також переданими параметрами sterm та genreId.*/
            BookDisplayModel bookModel = new BookDisplayModel
            {
                Books = books,
                Genres = genres,
                STerm = sterm,
                GenreId = genreId
            };


            //Повертаєм представлення (View) з отриманим об'єктом bookModel. 
            return View(bookModel);
        }

        //Дія, яка повертає сторінку "Privacy".
        public IActionResult Privacy()
        {
            return View();
        }

        /*Це дія для відображення сторінки помилки. Атрибут [ResponseCache] дозволяє налаштувати 
         * кешування для цієї дії. Повертає представлення з об'єктом ErrorViewModel, яке містить 
         * ідентифікатор запиту та інші відомості про помилку.*/
        /*Атрибут [ResponseCache] використовується для кешування результатів.
         * Duration = 0: Це встановлює тривалість кешування в 0 секунд, 
         * що означає, що результат дії не буде кешуватися.
         * Location = ResponseCacheLocation.None: Це вказує на те, що кеш не повинен 
         * зберігатися в жодному місці. ResponseCacheLocation.None означає, що результат 
         * не зберігається ні в кеші клієнта, ні на проксі-серверах, ні в кеші сервера.
         * NoStore = true: Цей параметр встановлює, що результат не повинен зберігатися ні в 
         * кеші клієнта, ні на проксі-серверах, ні в кеші сервера.*/
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}