using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RelatedProductsApi.Configurations;
using RelatedProductsApi.Data;
using RelatedProductsApi.DataProviders;
using RelatedProductsApi.DataProviders.Abstractions;
using RelatedProductsApi.Services;
using RelatedProductsApi.Services.Abstractions;

namespace RelatedProductsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            AppConfiguration = builder.Build();
        }

        public IConfiguration AppConfiguration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();

            // accepts any access token issued by identity server
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "http://192.168.1.120:5000";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            // adds an authorization policy to make sure the token is for scope 'api1'
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "relatedproductsapi.relatedproductsapi");
                });
                options.AddPolicy("ApiScopeBff", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "relatedproductsapi.relatedproductsapibff");
                });
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RelatedProductsApi", Version = "v1" });
            });
            services.Configure<Config>(AppConfiguration);
            var connectionString = AppConfiguration["RelatedProductsApi:ConnectionString"];
            services.AddDbContextFactory<RelatedProductsDbContext>(
                opts => opts.UseNpgsql(connectionString));
            services.AddTransient<IRelatedProductProvider, RelatedProductProvider>();
            services.AddTransient<IRelatedProductService, RelatedProductService>();
            services.AddScoped<IDbContextWrapper<RelatedProductsDbContext>, DbContextWrapper<RelatedProductsDbContext>>();
            services.AddTransient<IHttpClientService, HttpClientService>();
            services.AddTransient<IRateLimitService, RateLimitService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RelatedProductsApi v1"));
            }

            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict });
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
