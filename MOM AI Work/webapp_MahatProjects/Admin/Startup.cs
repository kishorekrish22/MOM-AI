using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using  Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
namespace Admin
{
		public class Startup
		{
			public Startup(IConfiguration configuration, IWebHostEnvironment env, IServiceProvider serviceProvider)
			{
				Configuration = configuration;
				var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
				//var mvcBuilder = serviceProvider.GetService<IMvcBuilder>();
				//new MvcConfiguration().ConfigureMvc(mvcBuilder);
				Configuration = builder.Build();
		}
			public IConfiguration Configuration { get; }
			public void ConfigureServices(IServiceCollection services)
			{
//Please remove comment from below line if you wish to see the changes of razor files immediately
//services.AddRazorPages().AddRazorRuntimeCompilation();
				services.Configure<ApiSettings>(Configuration.GetSection("ApiSettings"));
				services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
				services.Configure<FormOptions>(options => { options.ValueCountLimit = int.MaxValue; options.ValueLengthLimit = int.MaxValue; options.MultipartBodyLengthLimit = int.MaxValue; });

				services.AddDistributedMemoryCache();
				services.AddSession(options =>
				{
					options.IdleTimeout = TimeSpan.FromMinutes(60);
				});
				services.Configure<IISOptions>(options =>
				{
				});
				services.AddMvc().AddFluentValidation()
				.AddNewtonsoftJson(options =>
				{
				options.SerializerSettings.ContractResolver = new DefaultContractResolver();
				});
services.AddAuthentication(options =>
		            {
			            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

		            }).AddCookie();
services.AddLocalization(options => options.ResourcesPath = "Resource");


                                        services.AddControllersWithViews(options =>
                                        {
                                            options.Filters.Add(typeof(LanguageActionFilter));
                                        })
		                               .AddViewLocalization()
		                               .AddDataAnnotationsLocalization();

			                            services.Configure<RazorViewEngineOptions>(options =>
			                            {
				                            options.ViewLocationExpanders.Add(new LanguageViewLocationExpander());
			                            });
		}
			public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
			{
				app.UseStaticFiles();
				if (env.IsDevelopment())
				{
					app.UseDeveloperExceptionPage();
				}
				else
				{
					app.UseExceptionHandler("/Home/Error");
					app.UseForwardedHeaders();
				}
				app.UseSession();
			app.UseMiddleware<ErrorLoggingMiddleware>();
if (env.IsProduction())
{
app.Use((context, next) =>
{
context.Request.PathBase = Configuration.GetValue<string>("envSettings:basePath");
return next();
});
}
			app.Use(async (context, next) =>
				{
					context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
					context.Response.Headers.Add("X-Xss-Protection", "1");
					context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
					await next();
				});
			app.UseForwardedHeaders(new ForwardedHeadersOptions
				{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor |
				ForwardedHeaders.XForwardedProto
				});

app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.UseEndpoints(endpoints =>
{
endpoints.MapControllerRoute("area", "{area:exists}/{controller=users}/{action=Home}/{id?}");
endpoints.MapControllerRoute(
name: "default",
pattern: "{controller=users}/{action=Login}/{id?}");
});
			}
		}
}

