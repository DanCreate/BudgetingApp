using BudgetingApp.Models;
using BudgetingApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var usersautenticatedpolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddControllersWithViews(options =>

{
    options.Filters.Add(new AuthorizeFilter(usersautenticatedpolicy));

});
builder.Services.AddTransient<IAccountTypeRepository, AccountTypeRepository>();
builder.Services.AddTransient<IServiceUsers, ServiceUsers>();
builder.Services.AddTransient<IAccountsRepository, AccountsRepository>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<ICategoryRepository, CategoriesRepository>();
builder.Services.AddTransient<ITransactionsRepository, TransactionsRepository>();
builder.Services.AddTransient<IReportsService, ReportsService>();
builder.Services.AddHttpContextAccessor();  
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddTransient<IUserStore<User>, StorageUsers>();
builder.Services.AddTransient<SignInManager<User>>();
builder.Services.AddIdentityCore<User>(options =>

{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;


}).AddCookie(IdentityConstants.ApplicationScheme, options =>

{ 
options.LoginPath = "/Users/Login";
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Transaction}/{action=Index}/{id?}");

app.Run();
