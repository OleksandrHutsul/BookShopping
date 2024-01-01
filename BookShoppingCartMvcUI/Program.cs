using BookShoppingCartMvcUI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

/*����������� ��������� WebApplicationBuilder, ���� ��������������� ��� ������������ ��
������������ ���-�������.*/
var builder = WebApplication.CreateBuilder(args);

/*���������� ����� ���������� �� ���� ����� � ������������ �������. ���� ����� ���������� ��
��������, ���������� ����������*/
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

/*�������� ������ ��� ������ � ����� ����� �� ��������� Entity Framework Core.
��������������� SQL Server �� ��������� ���� �����.*/
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//�������� ������ ��� ��������� ������� �� ������� ���������� ���� �����. 
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

/*�������� ������ ��� �������������� �� ����������� ������������ �� ��������� Identity Framework.
���������� ������������ ��������� ������ �����������. ������������� Identity Framework ��� 
��������� ������������ �� ����� � ��� �����. ��������� ��������� ���������� ��� ��������������� 
���������� �� ��������� ������.*/
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultUI().AddDefaultTokenProviders();

//�������� �������� ���������� �� ���� ��� ������� HTTP-������ �� ����������� �������.
builder.Services.AddControllersWithViews();

/*�������� ��������� ������ (Transient), ��� �������, �� ��� ������� ������� ������ ����
�������� ����� ��'���. �� ��������������� ��� ��'����� ����������� ��� ����������*/
builder.Services.AddTransient<IHomeRepository, HomeRepository>();
builder.Services.AddTransient<ICartRepository, CartRepository>();
builder.Services.AddTransient<IUserOrderRepository, UserOrderRepository>();

//����������� ������������ ���-������� �� ����������� ��������� WebApplication
var app = builder.Build();

/*��� ��� ��������������� ��� ��������� ������ (scope), � ����� ��� ���� �������� �����������
���� ����� (seed data). 
app.Services ����������� �������� �����, �� ���� ����������� � �������.
CreateScope() ������� ����� ������� (scope) ��� ��������� ��������� ����� scope.
using �������, �� ������� ���� �������� �������� ���� ���������� ��������� ����� ����. 
������� �������� �������� ������� ������ ����� �� ����� �������. DbSeeder - �� ����, ���� ������� 
�� ����������� ���� ����� ����������� ������ (seed data). scope.ServiceProvider ���� ������ 
�� �����, �� ���� ����������� � ������� ������. ���� �������������� ��� ��'����� ����������� 
� ����, ���� ������� �� ���������� ���� ����� ������.*/
//�������� ��� ��� ����� ��� ������� ������� ��������
//using (var scope = app.Services.CreateScope())
//{
//    await DbSeeder.SeedDefaultData(scope.ServiceProvider);
//}

/*��� ��� �������, �� middleware(������ ����������) ������ ���� ���������� � ASP.NET Core ������� 
 � ��������� �� ���������� ��������� (environment). app.Environment ����������� ��'���, ���� 
������ ���������� ��� ������� ���������� ��������� �������. IsDevelopment() ��������, �� 
������� ������ � ���������� ��������.*/
if (app.Environment.IsDevelopment())
{
    /*���� ������� ������ � ���������� ��������, �� ��������������� middleware UseMigrationsEndPoint().
     * �� middleware, ��� �������� ��������������� ������� ��� ��������� ������� ���� 
     * ����� ������������� � ���-����������.*/
    app.UseMigrationsEndPoint();
}
else
{
    /*� ������ ������� ��������������� middleware UseExceptionHandler, ��� ����������� ���, ��� 
     * ��� ��������� ������� �������������� �� ������� /Home/Error. �� �������� ��������� 
     * ������� �� ��� ������ ������� � ���������� ����������� ��� �� ����� ��������.*/
    app.UseExceptionHandler("/Home/Error");
    /*UseHsts ������ HTTP Strict Transport Security (HSTS). �� �������� �������, ���� ����� 
     * �������� ��������������� HTTPS ��� ��� ������ �� �������. �� ������ �������, �����������, 
     * �� �� �'������� � �������� ����������� ����� ������������ ��������.*/
    app.UseHsts();
}
/*UseHttpsRedirection ���������� middleware ��� ������������� ��������������� HTTP-������ �� HTTPS. 
 * �� ������� ��� ������������ ������� ���������� �� �볺���� � ��������.*/
app.UseHttpsRedirection();

/*UseStaticFiles ���� middleware ��� ������� ��������� �����, ����� �� ����������, CSS, JavaScript 
 * ����. ³� �������� ������� ��������� ������� ������� ��� ������� �� ASP.NET Core.*/
app.UseStaticFiles();

/*UseRouting ���������� middleware ��� ������������� ������. ³� �������, �� ��������� ���������� 
 * ������ ����������� ��� ����������� HTTP-������ �� ����� ���� ��������.*/
app.UseRouting();

//UseAuthorization ���� middleware ��� ������� ����������� � �������.
app.UseAuthorization();

/*MapControllerRoute ������� ������������� ��� ����������. � ������ �������, ���� ���� URL �� 
 * ���������, �� �� ������������� ����������� ����� Index ���������� Home. �� ���������� 
 * ����������� ������� ��� ����������. MapRazorPages ���� middleware ��� ������� ������� Razor Pages.
 * Razor Pages - �� ������������� ����� � ASP.NET Core ��� ��������� ��������� � ��������� 
 * ���-�������.*/

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

/*Run ������� ������������ middleware. ��� ����� ������� ����� ������������ �
������� ������� HTTP-������.*/
app.Run();