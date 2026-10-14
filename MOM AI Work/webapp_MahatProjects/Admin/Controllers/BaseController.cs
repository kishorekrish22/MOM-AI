
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Policy;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IdentityModel;

namespace Admin.Controllers
{
	public abstract class BaseController : Controller
	{
		public HttpClient client = new HttpClient();
		public string apiURL = "";
		public BaseController(IConfiguration configuration)
		{
			apiURL = configuration["ApiSettings:apiURL"];

		}
		public virtual HttpClient getHttpClient()
		{
			client.BaseAddress = new Uri(apiURL);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("MahatProjectstoken"));
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			client.DefaultRequestHeaders.Add("AuthProvider", HttpContext.Session.GetString("MahatProjectsAuthProvider"));

			return client;
		}

	}
}

