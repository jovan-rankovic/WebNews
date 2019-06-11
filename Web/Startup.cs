using Application.Commands.Article;
using Application.Commands.Category;
using Application.Commands.Comment;
using Application.Commands.Hashtag;
using EfCommands.Article;
using EfCommands.Category;
using EfCommands.Comment;
using EfCommands.Hashtag;
using EfDataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<WebNewsContext>();

            services.AddTransient<IGetArticlesCommand, EfGetArticlesCommand>();
            services.AddTransient<IGetArticleCommand, EfGetArticleCommand>();
            services.AddTransient<ICreateArticleCommand, EfCreateArticleCommand>();
            services.AddTransient<IEditArticleCommand, EfEditArticleCommand>();
            services.AddTransient<IDeleteArticleCommand, EfDeleteArticleCommand>();

            services.AddTransient<IGetCategoriesCommand, EfGetCategoriesCommand>();
            services.AddTransient<IGetCategoryCommand, EfGetCategoryCommand>();
            services.AddTransient<ICreateCategoryCommand, EfCreateCategoryCommand>();
            services.AddTransient<IEditCategoryCommand, EfEditCategoryCommand>();
            services.AddTransient<IDeleteCategoryCommand, EfDeleteCategoryCommand>();

            services.AddTransient<IGetCommentsCommand, EfGetCommentsCommand>();
            services.AddTransient<IGetCommentCommand, EfGetCommentCommand>();
            services.AddTransient<ICreateCommentCommand, EfCreateCommentCommand>();
            services.AddTransient<IEditCommentCommand, EfEditCommentCommand>();
            services.AddTransient<IDeleteCommentCommand, EfDeleteCommentCommand>();

            services.AddTransient<IGetHashtagsCommand, EfGetHashtagsCommand>();
            services.AddTransient<IGetHashtagCommand, EfGetHashtagCommand>();
            services.AddTransient<ICreateHashtagCommand, EfCreateHashtagCommand>();
            services.AddTransient<IEditHashtagCommand, EfEditHashtagCommand>();
            services.AddTransient<IDeleteHashtagCommand, EfDeleteHashtagCommand>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}