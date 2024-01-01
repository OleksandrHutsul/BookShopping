using BookShoppingCartMvcUI.Constants;
using Microsoft.AspNetCore.Identity;

namespace BookShoppingCartMvcUI.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            /*У методі SeedDefaultData отримуються екземпляри UserManager<IdentityUser> та 
             * RoleManager<IdentityRole> через IServiceProvider. Це дозволяє отримати доступ до 
             * менеджерів користувачів та ролей для взаємодії з ідентифікацією та автентифікацією.*/
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();

            /*Тут створюються дві ролі: "Admin" і "User", і вони додаються до бази даних використовуючи
             * CreateAsync метод RoleManager.*/
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

            /*Створюємо об'єкт IdentityUser для адміністратора з ім'ям користувача,
            електронною поштою та підтвердженою електронною адресою.*/
            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            };

            //Перевіряємо, чи такий користувач вже існує в базі даних за допомогою FindByEmailAsync.
            var userInDb = await userMgr.FindByEmailAsync(admin.Email);

            /*Якщо користувача не знайдено, він створюється за допомогою CreateAsync, і йому 
             * присвоюється роль "Admin" за допомогою AddToRoleAsync.*/
            if (userInDb is null)
            {
                await userMgr.CreateAsync(admin, "Admin@123");
                await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
        }
    }
}