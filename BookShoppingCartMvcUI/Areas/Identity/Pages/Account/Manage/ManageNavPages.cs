// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//ManageNavPages.cs використовується для визначення статичних рядків для сторінок у розділі
//"Account\Manage" та надає методи для визначення класів активності для навігаційного меню. 

/*це директива препроцесора для вимкнення опції Nullable Reference Types у проекті C#. Ця опція 
 *  дозволяє розробникам визначати, які типи можуть мати 
 * значення null і які повинні бути завжди ненульовими.*/
#nullable disable

using Microsoft.AspNetCore.Mvc.Rendering;

namespace  BookShoppingCartMvcUI.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public static class ManageNavPages
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        
        /* Визначає статичний рядок для сторінки*/
        public static string Index => "Index";

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        
        /* Визначає статичний рядок для сторінки */
        public static string Email => "Email";

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /* Визначає статичний рядок для сторінки */
        public static string ChangePassword => "ChangePassword";

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /* Визначає статичний рядок для сторінки */
        public static string DownloadPersonalData => "DownloadPersonalData";

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /* Визначає статичний рядок для сторінки */
        public static string DeletePersonalData => "DeletePersonalData";

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /* Визначає статичний рядок для сторінки */
        public static string ExternalLogins => "ExternalLogins";

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /* Визначає статичний рядок для сторінки */
        public static string PersonalData => "PersonalData";

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /* Визначає статичний рядок для сторінки */
        public static string TwoFactorAuthentication => "TwoFactorAuthentication";

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /*визначає клас активності для навігаційного пункту сторінки в залежності від активності 
         * цієї сторінки в поточному контексті перегляду (ViewContext). Він використовує метод 
         * PageNavClass, який визначений у цьому ж класі, для визначення класу активності.*/
        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /*визначає клас активності для навігаційного пункту сторінки в залежності від активності 
         * цієї сторінки в поточному контексті перегляду (ViewContext). Він використовує метод 
         * PageNavClass, який визначений у цьому ж класі, для визначення класу активності.*/
        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /*визначає клас активності для навігаційного пункту сторінки в залежності від активності 
         * цієї сторінки в поточному контексті перегляду (ViewContext). Він використовує метод 
         * PageNavClass, який визначений у цьому ж класі, для визначення класу активності.*/
        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /*визначає клас активності для навігаційного пункту сторінки в залежності від активності 
         * цієї сторінки в поточному контексті перегляду (ViewContext). Він використовує метод 
         * PageNavClass, який визначений у цьому ж класі, для визначення класу активності.*/
        public static string DownloadPersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DownloadPersonalData);

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /*визначає клас активності для навігаційного пункту сторінки в залежності від активності 
         * цієї сторінки в поточному контексті перегляду (ViewContext). Він використовує метод 
         * PageNavClass, який визначений у цьому ж класі, для визначення класу активності.*/
        public static string DeletePersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeletePersonalData);

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /*визначає клас активності для навігаційного пункту сторінки в залежності від активності 
         * цієї сторінки в поточному контексті перегляду (ViewContext). Він використовує метод 
         * PageNavClass, який визначений у цьому ж класі, для визначення класу активності.*/
        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /*визначає клас активності для навігаційного пункту сторінки в залежності від активності 
         * цієї сторінки в поточному контексті перегляду (ViewContext). Він використовує метод 
         * PageNavClass, який визначений у цьому ж класі, для визначення класу активності.*/
        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /*визначає клас активності для навігаційного пункту сторінки в залежності від активності 
         * цієї сторінки в поточному контексті перегляду (ViewContext). Він використовує метод 
         * PageNavClass, який визначений у цьому ж класі, для визначення класу активності.*/
        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        //Цей метод PageNavClass визначає клас для активного навігаційного
        //пункту на основі інформації з ViewContext
        public static string PageNavClass(ViewContext viewContext, string page)
        {
            /*Цей рядок отримує активну сторінку з контексту перегляду (ViewContext). Він перевіряє, 
             * чи у ViewData існує значення з ключем "ActivePage" і, якщо так, використовує його як 
             * активну сторінку. Якщо такого значення немає або воно не є рядком, то використовується 
             * ім'я дії (Action) з ActionDescriptor (вказує на поточну дію контролера), і видаляється 
             * розширення файлу.*/
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);

            /*Цей вираз порівнює активну сторінку (activePage) з переданою сторінкою (page) без 
             * врахування регістру за допомогою параметру StringComparison.OrdinalIgnoreCase. 
             * Якщо вони ідентичні, вираз повертає true, інакше - false.
             * ? "active" : null;: Це тернарний оператор, який повертає рядок "active", 
             * якщо обидві сторінки ідентичні (вираз в пункті 2 повернув true), 
             * інакше повертає null.*/
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}