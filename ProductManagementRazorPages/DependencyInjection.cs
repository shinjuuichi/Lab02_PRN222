﻿using DataAccessObjects;
using ProductManagementRazorPages.SignalR;
using Repositories;
using Services;

namespace ProductManagementRazorPages
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add DbContext
            services.AddSqlServer<MyStoreContext>(configuration.GetConnectionString("MyStockDB"));

            //Add SignalR
            services.AddSignalR();

            // Register DAOs
            services.AddScoped<CategoryDAO>();
            services.AddScoped<ProductDAO>();
            services.AddScoped<AccountDAO>();

            // Register Repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            // Register Services
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAccountService, AccountService>();

            return services;
        }

        public static WebApplication UseApplicationHubs(this WebApplication app)
        {
            app.MapHub<SignalRServer>("/signalRServer");

            return app;
        }
    }
}
