using BookShoppingCartMvcUI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

/*Створюється екземпляр WebApplicationBuilder, який використовується для конфігурації та
налаштування веб-додатку.*/
var builder = WebApplication.CreateBuilder(args);

/*Отримується рядок підключення до бази даних з конфігурації додатку. Якщо рядок підключення не
знайдено, викидається виключення*/
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

/*Додається служба для роботи з базою даних за допомогою Entity Framework Core.
Використовується SQL Server як провайдер бази даних.*/
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//Додається фільтр для виведення винятків на сторінку розробника бази даних. 
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

/*Додається служба для автентифікації та авторизації користувачів за допомогою Identity Framework.
Вимагається підтвердження облікового запису користувача. Налаштовується Identity Framework для 
зберігання користувачів та ролей у базі даних. Додаються стандартні інтерфейси для користувацького 
інтерфейсу та генерації токенів.*/
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultUI().AddDefaultTokenProviders();

//Додається підтримка контролерів та видів для обробки HTTP-запитів та відображення сторінок.
builder.Services.AddControllersWithViews();

/*Додається транзитна служба (Transient), яка визначає, що для кожного виклику служби буде
створено новий об'єкт. Це використовується для ін'єкції залежностей для репозиторію*/
builder.Services.AddTransient<IHomeRepository, HomeRepository>();
builder.Services.AddTransient<ICartRepository, CartRepository>();
builder.Services.AddTransient<IUserOrderRepository, UserOrderRepository>();

//Завершується конфігурація веб-додатку та створюється екземпляр WebApplication
var app = builder.Build();

/*Цей код використовується для створення області (scope), в межах якої буде виконана ініціалізація
бази даних (seed data). 
app.Services представляє колекцію служб, які були зареєстровані в додатку.
CreateScope() створює новий область (scope) для здійснення областевої змінної scope.
using гарантує, що область буде коректно видалена після завершення виконання блоку коду. 
Область дозволяє керувати життєвим циклом служб та інших ресурсів. DbSeeder - це клас, який відповідає 
за ініціалізацію бази даних початковими даними (seed data). scope.ServiceProvider надає доступ 
до служб, які були зареєстровані в поточній області. Його використовують для ін'єкції залежностей 
у клас, який відповідає за заповнення бази даних даними.*/
//виконати цей код тільки при першому запуску програми
//using (var scope = app.Services.CreateScope())
//{
//    await DbSeeder.SeedDefaultData(scope.ServiceProvider);
//}

/*Цей код визначає, які middleware(проміжні компоненти) повинні бути використані в ASP.NET Core додатку 
 в залежності від середовища виконання (environment). app.Environment представляє об'єкт, який 
містить інформацію про поточне середовище виконання додатку. IsDevelopment() перевіряє, чи 
додаток працює в середовищі розробки.*/
if (app.Environment.IsDevelopment())
{
    /*Якщо додаток працює в середовищі розробки, то використовується middleware UseMigrationsEndPoint().
     * Це middleware, яке дозволяє використовувати сторінку для виконання міграцій бази 
     * даних безпосередньо з веб-інтерфейсу.*/
    app.UseMigrationsEndPoint();
}
else
{
    /*В іншому випадку використовується middleware UseExceptionHandler, яке налаштовано так, щоб 
     * при виникненні винятку перенаправляло на сторінку /Home/Error. Це допомагає обробляти 
     * помилки під час роботи додатку в середовищі виробництва або на інших серверах.*/
    app.UseExceptionHandler("/Home/Error");
    /*UseHsts включає HTTP Strict Transport Security (HSTS). Це захисний механізм, який змушує 
     * браузери використовувати HTTPS для всіх запитів до сервера. Це підвищує безпеку, упевнюючись, 
     * що всі з'єднання з сервером відбуваються через зашифрований протокол.*/
    app.UseHsts();
}
/*UseHttpsRedirection встановлює middleware для автоматичного перенаправлення HTTP-запитів на HTTPS. 
 * Це важливо для забезпечення безпеки комунікації між клієнтом і сервером.*/
app.UseHttpsRedirection();

/*UseStaticFiles додає middleware для обробки статичних файлів, таких як зображення, CSS, JavaScript 
 * тощо. Він дозволяє серверу повертати статичні ресурси без обробки їх ASP.NET Core.*/
app.UseStaticFiles();

/*UseRouting встановлює middleware для маршрутизації запитів. Він визначає, які обробники контролерів 
 * повинні викликатися для конкретного HTTP-запиту на основі його маршруту.*/
app.UseRouting();

//UseAuthorization додає middleware для обробки авторизації в додатку.
app.UseAuthorization();

/*MapControllerRoute визначає маршрутизацію для контролерів. В даному випадку, якщо шлях URL не 
 * визначено, то за замовчуванням викликається метод Index контролера Home. Це встановлює 
 * стандартний маршрут для контролерів. MapRazorPages додає middleware для обробки сторінок Razor Pages.
 * Razor Pages - це архітектурний підхід в ASP.NET Core для створення статичних і динамічних 
 * веб-сторінок.*/

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

/*Run завершує конфігурацію middleware. Цей метод позначає кінець конфігурації і
початок обробки HTTP-запитів.*/
app.Run();