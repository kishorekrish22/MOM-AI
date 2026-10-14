namespace Admin.Controllers
			{
				using System;
				using System.Data;
				using System.Linq;
				using Microsoft.AspNetCore.Mvc;
				using Newtonsoft.Json;
				using System.Net.Http;
				using System.Net.Http.Formatting;
				using System.Threading.Tasks;
				using System.Net.Http.Headers;
				using Microsoft.Extensions.Options;
				using Microsoft.AspNetCore.Http;
				using System.Collections.Generic;
				using System.IO;
				using Microsoft.AspNetCore.Hosting;
				using System.Net;
				using FluentValidation.Results;
				using MahatProjects.Models;
				using Microsoft.AspNetCore.Mvc.Infrastructure;
				using Microsoft.AspNetCore.HttpOverrides;
                using Microsoft.Extensions.Configuration;
                using Microsoft.Extensions.Logging;
                using System.Threading;
	            using System.Globalization;
                using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;using System.IdentityModel.Tokens.Jwt;
                
                
                
				//This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:23
				
                
                
                
                
				public class usersController : BaseController
				{	 
					private IWebHostEnvironment hostingEnv;
					private IOptions<ApiSettings> _balSettings;
                    private IOptions<MailSettings> _mailSettings;
					private string  url = "";
					private string  baseUrl = "";
                    private string  adminUrl = "";
                    private string  clientUrl = "";
                    private string  accesskey = "";
					private IHttpContextAccessor _accessor;
                    public IConfiguration Configuration { get; }
                    private readonly ILogger<usersController> _logger;
                    private static string authProvider = "";
                    
                    
					public usersController(IConfiguration configuration,IHttpContextAccessor accessor,IOptions<ApiSettings> ApiSettings, IOptions<MailSettings> MailSettings, IWebHostEnvironment env, ILogger<usersController> logger):base( configuration)
					{
                        _logger = logger;
						this.hostingEnv = env;
						_balSettings = ApiSettings;
                        _mailSettings = MailSettings;
						url = _balSettings.Value.apiURL;
						baseUrl = _balSettings.Value.baseURL;
                        adminUrl = _balSettings.Value.adminURL;
                        clientUrl = _balSettings.Value.clientURL;
                        accesskey = _balSettings.Value.accesskey;
 
						_accessor = accessor;
                        Configuration = configuration;
                        
					}
					 public virtual IActionResult detail()
			         {
					        return View();
			         }
                     public virtual IActionResult audit()
			         {
					        return View();
			         }
	

					
				
			  public virtual async Task<string> getById_alloweddevices(string id)
			  {
					return await ApiClient.Get_ApiValues(getHttpClient(), "api/users/getById_alloweddevices?id="+id+"&loginUserID="+HttpContext.Session.GetString("MahatProjectsloginUserID"));
					 
			  }


			  public virtual IActionResult CreatePlatform()
			  {
					return View();
			  }	
			  [HttpPost()]
			public virtual async Task<string> CreatePlatform(usersModel model, IFormCollection collection)
			{
				string strReturnMessage = "";
				
				try
				{
					ModelState.Remove("id");
					ModelState.Remove("created_by_id");
                    ModelState.Remove("craftmyapp_actionmethodname");
                    model.craftmyapp_actionmethodname="CreatePlatform";
					
					ModelState.Remove("profilepicture");

					
			 	 
					 if (ModelState.IsValid)
					 {
							 usersModelValidator validator = new usersModelValidator();
							 ValidationResult results = validator.Validate(model);
							 if (!results.IsValid)
							 {
								 var errorCollection = string.Join(" | ", results.Errors.Select(e => e.ErrorMessage.Replace("{propertyName}",e.PropertyName)));
								 strReturnMessage = errorCollection.ToString();
								 foreach (var failure in results.Errors)
								 {
									ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
								 }
							 }
							 else
							 {
								 
								 var files = Request.Form.Files;
foreach (var file in files) 
{
var filename = ContentDispositionHeaderValue
.Parse(file.ContentDisposition)
.FileName
.Trim('"'); 
string fileExtention = "." + filename.Split('.').Last(); 
Random rnd = new Random();
string uploadFileName = System.Text.RegularExpressions.Regex.Replace(filename.Split('.').First(), @"[^0-9a-zA-Z_.]+", "").Replace(" ", String.Empty)+"_"+"users_" +rnd.Next(1, 10000).ToString() + DateTime.Now.ToString("ddMMyyHHmmss")+ fileExtention;
if (fileExtention != ".")
{
var filanameFolder = hostingEnv.WebRootPath +  "/uploads/"; 
if (!Directory.Exists(filanameFolder))
{
Directory.CreateDirectory(filanameFolder);
}
filename  = filanameFolder + uploadFileName;
using (FileStream fs = System.IO.File.Create(filename))
{
file.CopyTo(fs);
fs.Flush();
}
if (file.Name == "profilepicture")
{
uploadFileName = baseUrl + "/uploads/" + uploadFileName;
model.profilepicture = uploadFileName;
}
}
}

                                 response_model obj_response_model = await ApiClient.Post_ApiValuesGetResponseModel(getHttpClient(),"api/users/Register_Profile", model);
                                                                                                strReturnMessage=obj_response_model.message;
                                 model.id=Convert.ToInt64(obj_response_model.id);   
								 
							 }
					 }
					 else
					 {
							   var errorMessages = ModelState.Where(entry => entry.Value.Errors.Any()).SelectMany(entry => entry.Value.Errors.Select(error => $"{entry.Key}: {error.ErrorMessage}"));
                               var errorCollection = string.Join(" | ", errorMessages);
                               strReturnMessage = errorCollection;
					 }
			 }
			 catch (Exception ex)
			 {
                 
                 _logger.LogError(ex,"An exception occurred in - users / CreatePlatform, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
               
				 strReturnMessage = ex.Message;
			 }
		     ViewData["message"] = strReturnMessage;
			 if(strReturnMessage.Replace("\"", "").Contains("201.1")){
				 TempData["message"] = "Success";
				 
                 
				return "Success";
			 }else{
				  if(strReturnMessage=="401.1")
				  	 	 strReturnMessage = "Authorization Failed";

				  return strReturnMessage;
			 }
 
		   }

			  public virtual IActionResult Register_Profile()
			  {
					return View();
			  }	
			  [HttpPost()]
			public virtual async Task<string> Register_Profile(usersModel model, IFormCollection collection)
			{
				string strReturnMessage = "";
				
				try
				{
					ModelState.Remove("id");
					ModelState.Remove("created_by_id");
                    ModelState.Remove("craftmyapp_actionmethodname");
                    model.craftmyapp_actionmethodname="Register_Profile";
					
					ModelState.Remove("profilepicture");

					
			 	 
					 if (ModelState.IsValid)
					 {
							 usersModelValidator validator = new usersModelValidator();
							 ValidationResult results = validator.Validate(model);
							 if (!results.IsValid)
							 {
								 var errorCollection = string.Join(" | ", results.Errors.Select(e => e.ErrorMessage.Replace("{propertyName}",e.PropertyName)));
								 strReturnMessage = errorCollection.ToString();
								 foreach (var failure in results.Errors)
								 {
									ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
								 }
							 }
							 else
							 {
								 
								 var files = Request.Form.Files;
foreach (var file in files) 
{
var filename = ContentDispositionHeaderValue
.Parse(file.ContentDisposition)
.FileName
.Trim('"'); 
string fileExtention = "." + filename.Split('.').Last(); 
Random rnd = new Random();
string uploadFileName = System.Text.RegularExpressions.Regex.Replace(filename.Split('.').First(), @"[^0-9a-zA-Z_.]+", "").Replace(" ", String.Empty)+"_"+"users_" +rnd.Next(1, 10000).ToString() + DateTime.Now.ToString("ddMMyyHHmmss")+ fileExtention;
if (fileExtention != ".")
{
var filanameFolder = hostingEnv.WebRootPath +  "/uploads/"; 
if (!Directory.Exists(filanameFolder))
{
Directory.CreateDirectory(filanameFolder);
}
filename  = filanameFolder + uploadFileName;
using (FileStream fs = System.IO.File.Create(filename))
{
file.CopyTo(fs);
fs.Flush();
}
if (file.Name == "profilepicture")
{
uploadFileName = baseUrl + "/uploads/" + uploadFileName;
model.profilepicture = uploadFileName;
}
}
}

                                 response_model obj_response_model = await ApiClient.Post_ApiValuesGetResponseModel(getHttpClient(),"api/users/Register_Profile", model);
                                                                                                strReturnMessage=obj_response_model.message;
                                 model.id=Convert.ToInt64(obj_response_model.id);   
								 
							 }
					 }
					 else
					 {
							   var errorMessages = ModelState.Where(entry => entry.Value.Errors.Any()).SelectMany(entry => entry.Value.Errors.Select(error => $"{entry.Key}: {error.ErrorMessage}"));
                               var errorCollection = string.Join(" | ", errorMessages);
                               strReturnMessage = errorCollection;
					 }
			 }
			 catch (Exception ex)
			 {
                 
                 _logger.LogError(ex,"An exception occurred in - users / Register_Profile, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
               
				 strReturnMessage = ex.Message;
			 }
		     ViewData["message"] = strReturnMessage;
			 if(strReturnMessage.Replace("\"", "").Contains("201.1")){
				 TempData["message"] = "Success";
				 
                 
				return "Success";
			 }else{
				  if(strReturnMessage=="401.1")
				  	 	 strReturnMessage = "Authorization Failed";

				  return strReturnMessage;
			 }
 
		   }

				
			  public virtual async Task<IActionResult> EditPlatform(string id)
			  {

                    string redirectTo="";
                    if(HttpContext.Session.GetString("MahatProjectsrole_JSON") != null){
                            DataTable MahatProjectsrole_JSON =HttpContext.Session.GetSession<DataTable>("MahatProjectsroles");
                            DataView dv = new DataView(MahatProjectsrole_JSON);
                            dv.RowFilter = "controllername='users' AND viewname='list'";

                            if(dv.Count  >0){
                                redirectTo = dv[0]["actionmethodname"] as string;
							 
                            }

                            try{
                                     var jsonObjusers = await ApiClient.Get_ApiValues(getHttpClient(), "api/users/getById_users?id="+id+"&loginUserID="+HttpContext.Session.GetString("MahatProjectsloginUserID"));
                                if(jsonObjusers.Length > 2)
                                {
                                  
                                    var model = JsonConvert.DeserializeObject<usersModel>(jsonObjusers);


                
                                     
                                    return View(model);
                                }
                                else
                                {
                    
                                    TempData["message"] = "Data Not Found - Contact Administrator";
                                    return RedirectToAction(redirectTo);
						 
                                }

                            }catch(Exception ex){
                               _logger.LogError(ex,"An exception occurred in - users / EditPlatform, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
              
                                TempData["errMessage"] = "Error while fetching data - Contact Administrator";
                                return RedirectToAction(redirectTo);
                            }

                    }
                    TempData["errMessage"] = "Session Expired";
                    return RedirectToAction("Logout", "users");
                }	
			  [HttpPost()]
				public virtual async Task<string> EditPlatform(usersModel model, IFormCollection collection)
				{
					string strReturnMessage = "";
					try
					{
							ModelState.Remove("id");
                            ModelState.Remove("craftmyapp_actionmethodname");
                             model.craftmyapp_actionmethodname="EditPlatform";
							
							model.userrole =collection["userrole"];
							if(HttpContext.Session.GetString("MahatProjectsloginUserID") != null)
					model.updated_by_id =Convert.ToInt64 (HttpContext.Session.GetString("MahatProjectsloginUserID"));
					else
					return "Session Expired";
							

                            
							if (ModelState.IsValid)
							{
									usersModelValidator validator = new usersModelValidator();
									ValidationResult results = validator.Validate(model);
									if (!results.IsValid)
									{
										var errorCollection = string.Join(" | ", results.Errors.Select(e => e.ErrorMessage.Replace("{propertyName}",e.PropertyName)));
										strReturnMessage = errorCollection.ToString();
										foreach (var failure in results.Errors)
										{
											ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
										}
									}
									else
									{
                                        model.profilepicture = collection["profilepicture_existing"];

										var files = Request.Form.Files;
foreach (var file in files) 
{
var filename = ContentDispositionHeaderValue
.Parse(file.ContentDisposition)
.FileName
.Trim('"'); 
string fileExtention = "." + filename.Split('.').Last(); 
Random rnd = new Random();
string uploadFileName = System.Text.RegularExpressions.Regex.Replace(filename.Split('.').First(), @"[^0-9a-zA-Z_.]+", "").Replace(" ", String.Empty)+"_"+"users_" +rnd.Next(1, 10000).ToString() + DateTime.Now.ToString("ddMMyyHHmmss")+ fileExtention;
if (fileExtention != ".")
{
var filanameFolder = hostingEnv.WebRootPath +  "/uploads/"; 
if (!Directory.Exists(filanameFolder))
{
Directory.CreateDirectory(filanameFolder);
}
filename  = filanameFolder + uploadFileName;
using (FileStream fs = System.IO.File.Create(filename))
{
file.CopyTo(fs);
fs.Flush();
}
if (file.Name == "profilepicture")
{
uploadFileName = baseUrl + "/uploads/" + uploadFileName;
model.profilepicture = uploadFileName;
}
}
}

                                        
                                        
                                        strReturnMessage = await ApiClient.Post_ApiValuesGetString(getHttpClient(),"api/users/Update_Profile", model);
 									}
							}
							else
							{
									var errorMessages = ModelState.Where(entry => entry.Value.Errors.Any()).SelectMany(entry => entry.Value.Errors.Select(error => $"{entry.Key}: {error.ErrorMessage}"));
                               var errorCollection = string.Join(" | ", errorMessages);
                               strReturnMessage = errorCollection;
							}
					}
					catch (Exception ex)
					{
                      _logger.LogError(ex,"An exception occurred in - users / EditPlatform, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
              
						strReturnMessage = ex.Message;
					}
					ViewData["message"] = strReturnMessage;
					    if(strReturnMessage.Replace("\"", "")=="201.1"){
							TempData["message"] = "Success";
							
							return "Success";
						}else{
							if(strReturnMessage=="401.1")
									strReturnMessage = "Authorization Failed";

							return strReturnMessage;
						}
		
				}

				
			  public virtual async Task<IActionResult> UpdateProfile(string id)
			  {

                    string redirectTo="";
                    if(HttpContext.Session.GetString("MahatProjectsrole_JSON") != null){
                            DataTable MahatProjectsrole_JSON =HttpContext.Session.GetSession<DataTable>("MahatProjectsroles");
                            DataView dv = new DataView(MahatProjectsrole_JSON);
                            dv.RowFilter = "controllername='users' AND viewname='list'";

                            if(dv.Count  >0){
                                redirectTo = dv[0]["actionmethodname"] as string;
							 
                            }

                            try{
                                     var jsonObjusers = await ApiClient.Get_ApiValues(getHttpClient(), "api/users/getById_users?id="+id+"&loginUserID="+HttpContext.Session.GetString("MahatProjectsloginUserID"));
                                if(jsonObjusers.Length > 2)
                                {
                                  
                                    var model = JsonConvert.DeserializeObject<usersModel>(jsonObjusers);


                
                                     
                                    return View(model);
                                }
                                else
                                {
                    
                                    TempData["message"] = "Data Not Found - Contact Administrator";
                                    return RedirectToAction(redirectTo);
						 
                                }

                            }catch(Exception ex){
                               _logger.LogError(ex,"An exception occurred in - users / UpdateProfile, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
              
                                TempData["errMessage"] = "Error while fetching data - Contact Administrator";
                                return RedirectToAction(redirectTo);
                            }

                    }
                    TempData["errMessage"] = "Session Expired";
                    return RedirectToAction("Logout", "users");
                }	
			  [HttpPost()]
				public virtual async Task<string> UpdateProfile(usersModel model, IFormCollection collection)
				{
					string strReturnMessage = "";
					try
					{
							ModelState.Remove("id");
                            ModelState.Remove("craftmyapp_actionmethodname");
                             model.craftmyapp_actionmethodname="UpdateProfile";
							
							model.userrole =collection["userrole"];
							if(HttpContext.Session.GetString("MahatProjectsloginUserID") != null)
					model.updated_by_id =Convert.ToInt64 (HttpContext.Session.GetString("MahatProjectsloginUserID"));
					else
					return "Session Expired";
							

                            
							if (ModelState.IsValid)
							{
									usersModelValidator validator = new usersModelValidator();
									ValidationResult results = validator.Validate(model);
									if (!results.IsValid)
									{
										var errorCollection = string.Join(" | ", results.Errors.Select(e => e.ErrorMessage.Replace("{propertyName}",e.PropertyName)));
										strReturnMessage = errorCollection.ToString();
										foreach (var failure in results.Errors)
										{
											ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
										}
									}
									else
									{
                                        model.profilepicture = collection["profilepicture_existing"];

										var files = Request.Form.Files;
foreach (var file in files) 
{
var filename = ContentDispositionHeaderValue
.Parse(file.ContentDisposition)
.FileName
.Trim('"'); 
string fileExtention = "." + filename.Split('.').Last(); 
Random rnd = new Random();
string uploadFileName = System.Text.RegularExpressions.Regex.Replace(filename.Split('.').First(), @"[^0-9a-zA-Z_.]+", "").Replace(" ", String.Empty)+"_"+"users_" +rnd.Next(1, 10000).ToString() + DateTime.Now.ToString("ddMMyyHHmmss")+ fileExtention;
if (fileExtention != ".")
{
var filanameFolder = hostingEnv.WebRootPath +  "/uploads/"; 
if (!Directory.Exists(filanameFolder))
{
Directory.CreateDirectory(filanameFolder);
}
filename  = filanameFolder + uploadFileName;
using (FileStream fs = System.IO.File.Create(filename))
{
file.CopyTo(fs);
fs.Flush();
}
if (file.Name == "profilepicture")
{
uploadFileName = baseUrl + "/uploads/" + uploadFileName;
model.profilepicture = uploadFileName;
}
}
}

                                        
                                        if(model.profilepicture !=  null && model.profilepicture!="")
                                                    HttpContext.Session.SetString("MahatProjectsprofilepicture", model.profilepicture.ToString());
                                        strReturnMessage = await ApiClient.Post_ApiValuesGetString(getHttpClient(),"api/users/Update_Profile", model);
 									}
							}
							else
							{
									var errorMessages = ModelState.Where(entry => entry.Value.Errors.Any()).SelectMany(entry => entry.Value.Errors.Select(error => $"{entry.Key}: {error.ErrorMessage}"));
                               var errorCollection = string.Join(" | ", errorMessages);
                               strReturnMessage = errorCollection;
							}
					}
					catch (Exception ex)
					{
                      _logger.LogError(ex,"An exception occurred in - users / UpdateProfile, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
              
						strReturnMessage = ex.Message;
					}
					ViewData["message"] = strReturnMessage;
					    if(strReturnMessage.Replace("\"", "")=="201.1"){
							TempData["message"] = "Success";
							
							return "Success";
						}else{
							if(strReturnMessage=="401.1")
									strReturnMessage = "Authorization Failed";

							return strReturnMessage;
						}
		
				}

				
			  public virtual async Task<IActionResult> Update_Profile(string id)
			  {

                    string redirectTo="";
                    if(HttpContext.Session.GetString("MahatProjectsrole_JSON") != null){
                            DataTable MahatProjectsrole_JSON =HttpContext.Session.GetSession<DataTable>("MahatProjectsroles");
                            DataView dv = new DataView(MahatProjectsrole_JSON);
                            dv.RowFilter = "controllername='users' AND viewname='list'";

                            if(dv.Count  >0){
                                redirectTo = dv[0]["actionmethodname"] as string;
							 
                            }

                            try{
                                     var jsonObjusers = await ApiClient.Get_ApiValues(getHttpClient(), "api/users/getById_users?id="+id+"&loginUserID="+HttpContext.Session.GetString("MahatProjectsloginUserID"));
                                if(jsonObjusers.Length > 2)
                                {
                                  
                                    var model = JsonConvert.DeserializeObject<usersModel>(jsonObjusers);


                
                                     
                                    return View(model);
                                }
                                else
                                {
                    
                                    TempData["message"] = "Data Not Found - Contact Administrator";
                                    return RedirectToAction(redirectTo);
						 
                                }

                            }catch(Exception ex){
                               _logger.LogError(ex,"An exception occurred in - users / Update_Profile, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
              
                                TempData["errMessage"] = "Error while fetching data - Contact Administrator";
                                return RedirectToAction(redirectTo);
                            }

                    }
                    TempData["errMessage"] = "Session Expired";
                    return RedirectToAction("Logout", "users");
                }	
			  [HttpPost()]
				public virtual async Task<string> Update_Profile(usersModel model, IFormCollection collection)
				{
					string strReturnMessage = "";
					try
					{
							ModelState.Remove("id");
                            ModelState.Remove("craftmyapp_actionmethodname");
                             model.craftmyapp_actionmethodname="Update_Profile";
							
							model.userrole =collection["userrole"];
							if(HttpContext.Session.GetString("MahatProjectsloginUserID") != null)
					model.updated_by_id =Convert.ToInt64 (HttpContext.Session.GetString("MahatProjectsloginUserID"));
					else
					return "Session Expired";
							

                            
							if (ModelState.IsValid)
							{
									usersModelValidator validator = new usersModelValidator();
									ValidationResult results = validator.Validate(model);
									if (!results.IsValid)
									{
										var errorCollection = string.Join(" | ", results.Errors.Select(e => e.ErrorMessage.Replace("{propertyName}",e.PropertyName)));
										strReturnMessage = errorCollection.ToString();
										foreach (var failure in results.Errors)
										{
											ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
										}
									}
									else
									{
                                        model.profilepicture = collection["profilepicture_existing"];

										var files = Request.Form.Files;
foreach (var file in files) 
{
var filename = ContentDispositionHeaderValue
.Parse(file.ContentDisposition)
.FileName
.Trim('"'); 
string fileExtention = "." + filename.Split('.').Last(); 
Random rnd = new Random();
string uploadFileName = System.Text.RegularExpressions.Regex.Replace(filename.Split('.').First(), @"[^0-9a-zA-Z_.]+", "").Replace(" ", String.Empty)+"_"+"users_" +rnd.Next(1, 10000).ToString() + DateTime.Now.ToString("ddMMyyHHmmss")+ fileExtention;
if (fileExtention != ".")
{
var filanameFolder = hostingEnv.WebRootPath +  "/uploads/"; 
if (!Directory.Exists(filanameFolder))
{
Directory.CreateDirectory(filanameFolder);
}
filename  = filanameFolder + uploadFileName;
using (FileStream fs = System.IO.File.Create(filename))
{
file.CopyTo(fs);
fs.Flush();
}
if (file.Name == "profilepicture")
{
uploadFileName = baseUrl + "/uploads/" + uploadFileName;
model.profilepicture = uploadFileName;
}
}
}

                                        
                                        
                                        strReturnMessage = await ApiClient.Post_ApiValuesGetString(getHttpClient(),"api/users/Update_Profile", model);
 									}
							}
							else
							{
									var errorMessages = ModelState.Where(entry => entry.Value.Errors.Any()).SelectMany(entry => entry.Value.Errors.Select(error => $"{entry.Key}: {error.ErrorMessage}"));
                               var errorCollection = string.Join(" | ", errorMessages);
                               strReturnMessage = errorCollection;
							}
					}
					catch (Exception ex)
					{
                      _logger.LogError(ex,"An exception occurred in - users / Update_Profile, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
              
						strReturnMessage = ex.Message;
					}
					ViewData["message"] = strReturnMessage;
					    if(strReturnMessage.Replace("\"", "")=="201.1"){
							TempData["message"] = "Success";
							
							return "Success";
						}else{
							if(strReturnMessage=="401.1")
									strReturnMessage = "Authorization Failed";

							return strReturnMessage;
						}
		
				}
public virtual async Task<IActionResult> Suspend_Profile(string id)
			{
				string message = "";
				try
				{
						message = await ApiClient.Get_ApiValues(getHttpClient(), "api/users/Suspend_Profile?id="+id+"&loginUserID="+HttpContext.Session.GetString("MahatProjectsloginUserID"));
						 if(message.Replace("\"","")=="201.1")
						{
							TempData["message"] = "Success";

						}else{
							TempData["errMessage"] = message.Replace("\"","");
						}
						
				
				
				}
				catch (Exception ex)
				{
                     _logger.LogError(ex,"An exception occurred in - users / Suspend_Profile, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
              
                
					 TempData["errMessage"] = ex.Message;
					 message = ex.Message;
				}

				string redirectTo="";
						if(HttpContext.Session.GetString("MahatProjectsrole_JSON") != null){
					DataTable MahatProjectsrole_JSON =HttpContext.Session.GetSession<DataTable>("MahatProjectsroles");
						 DataView dv = new DataView(MahatProjectsrole_JSON);
						 dv.RowFilter = "controllername='users' AND viewname='list'";

						if(dv.Count  >0){
						    redirectTo = dv[0]["actionmethodname"] as string;
							 
						}

					}
				
				return RedirectToAction(redirectTo);
			}
public virtual async Task<IActionResult> DeletePlatform(string id)
			{
				string message = "";
				try
				{
						message = await ApiClient.Get_ApiValues(getHttpClient(), "api/users/Suspend_Profile?id="+id+"&loginUserID="+HttpContext.Session.GetString("MahatProjectsloginUserID"));
				}
				catch (Exception ex)
				{
                 _logger.LogError(ex,"An exception occurred in - users / DeletePlatform, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
              
               
				message = ex.Message;
				}
				TempData["message"] = message;
				return RedirectToAction("IndexPlatform");
			}

		  public async Task<string> get_Transcript(string input)
          {
            try
            {
                string targetLanguage = HttpContext.Request.Cookies["MahatProjectsCurrentCulture"];
                     string[] parts = targetLanguage.Split('-');
                    string targetLanguage1 = parts[0]; // This will split target LAguage 

                string apiUrl = $"https://inputtools.google.com/request?text={Uri.EscapeDataString(input)}&ime=transliteration_en_{targetLanguage1}&num=1&cp=0&cs=1&ie=utf-8&oe=utf-8&app=demopage";
                
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Referer", "https://www.google.com/");

                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    var jsonResult = JsonDocument.Parse(responseBody);
                    var root = jsonResult.RootElement;
                   
                        var translations = root[1][0][1];

                        foreach (var translation in translations.EnumerateArray())
                        {
                            var transliteratedText = translation.GetString();
                            Console.WriteLine("Transliterated Text: " + transliteratedText);
                        }

                        // Return the first transliterated text
                        return translations[0].GetString();
                   
                }
            }
            catch(Exception ex)
            {
                return input;

            }
        }
		
			
        public virtual IActionResult SessionExpired()
        {
            TempData["messagereferer"] = "donotrefer";
	        TempData["message"] = "Session Expired , Please login again !";
	        return RedirectToAction("Login");
        }
        public virtual IActionResult AuthorizationFailed()
        {
            TempData["messagereferer"] = "donotrefer";
	        TempData["message"] = "API Authorization failed ,Please login again !";
	        return RedirectToAction("Login");
        }
        public virtual IActionResult RoleAuthorizationFailed()
        {
            TempData["messagereferer"] = "donotrefer";
	        TempData["message"] = "Not authorized to view this page !";
	        return RedirectToAction("Login");
        }
		


			
                        [HttpGet()]
                        public virtual async Task<string> get_roles(string parentname)
                        {
				 
		                        return await ApiClient.Get_ApiValues(getHttpClient(), "api/users/get_roles?roles="+HttpContext.Session.GetString("MahatProjectsuserrole") +"&parentname="+ parentname);
				 
			
                        }
                        public async Task<ActionResult> ChangeCurrentCulture(string culture)
                        {

                        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
                        Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
                        HttpResponseMessage response_menu = await ApiClient.GET_ApiValuesGetRespnse(getHttpClient(), "api/users/get_project_Menu?viewactionroles="+ HttpContext.Session.GetString("MahatProjectsuserrole") + "&SubSystem=Admin");
            
                        if (response_menu.IsSuccessStatusCode)
                        {

                                string menu_JSON = await response_menu.Content.ReadAsStringAsync();


                            var assembly = typeof(usersController).Assembly;
                            System.Resources.ResourceManager _resourceManager = new System.Resources.ResourceManager("Admin.Resource", assembly);
 
                                List<dynamic> menuItems = JsonConvert.DeserializeObject<List<dynamic>>(menu_JSON);

                            foreach (dynamic menuItem in menuItems)
                            { 
		                        string projectElementName = System.Text.RegularExpressions.Regex.Replace(menuItem.projectelementname.ToString(), @"\s+", "").ToLower();
		                        string translatedProjectElementName = _resourceManager.GetString(projectElementName);
		                        string parentnode = System.Text.RegularExpressions.Regex.Replace(menuItem.parentnode.ToString(), @"\s+", "").ToLower();
		                        string translatedparentnode = _resourceManager.GetString(parentnode);
		                        menuItem.projectelementname = translatedProjectElementName;
		                        menuItem.parentnode = translatedparentnode;
                            }
	                        menu_JSON = JsonConvert.SerializeObject(menuItems);
	                        HttpContext.Session.SetString("MahatProjectsmenu_JSON", menu_JSON);

				 


                        }



	                        Response.Cookies.Append("MahatProjectsCurrentCulture", culture, new CookieOptions
                        {
                            Expires = DateTimeOffset.UtcNow.AddDays(10), // Cookie expires in 1 hour
                            HttpOnly = true // Makes the cookie accessible only through the HTTP protocol, not JavaScript
                        });
                        string referrerUrl = HttpContext.Request.Headers["Referer"];
                        if (referrerUrl != null)
                        {
	                        return Redirect(referrerUrl);
                        }
                        else
                        {
	                        return RedirectToAction("Home", "users");
                        }
                        }
[AllowAnonymous]
            public virtual IActionResult Home()
			{
                HttpContext.Session.SetString("ReferrerUrl", "");
                TempData["messagereferer"] = "donotrefer";

                return RedirectToAction("Login");
            } 
[AllowAnonymous]
            public virtual IActionResult Login()
			{
                HttpContext.Session.SetString("ReferrerUrl", "");
                string referrerUrl = HttpContext.Request.Headers["Referer"];
                if (referrerUrl != null && TempData["messagereferer"] ==null)
                {
                       HttpContext.Session.SetString("ReferrerUrl", referrerUrl);

                }
                return View();
			}
            public virtual IActionResult Logout()
			{
				HttpContext.Session.Clear();
                TempData["messagereferer"] = "donotrefer";
				return RedirectToAction("Login");
			}
            [HttpPost()]
[AllowAnonymous]
			public virtual async Task<IActionResult> Login(userloginModel model)
			{
				string message = "";
				string strReturnMessage = "";
				try
				{


						userlockoutModel objuserlockoutModel=new userlockoutModel();
						objuserlockoutModel.username=model.username;
						objuserlockoutModel.latlan=model.latlan;
						string  remoteipaddress=_accessor.HttpContext.Connection.RemoteIpAddress.ToString();
						objuserlockoutModel.remoteipaddress=remoteipaddress;
						objuserlockoutModel.clientipaddress=model.clientipaddress;
						
						strReturnMessage = await ApiClient.Post_ApiValuesGetString(getHttpClient(),"api/userlockout/verify_userlockout", objuserlockoutModel);
						if(strReturnMessage !="Allow"){
						ViewData["message"] = strReturnMessage;
						return View(model);
						}
                        model.source="Internal";
						HttpResponseMessage response = await ApiClient.Post_ApiValuesGetRespnse(client,"api/users/CheckAuthentication", model);
						
						
						if (response.IsSuccessStatusCode)
						{
						DataTable dt = await response.Content.ReadAsAsync<DataTable>();
						if (dt.Rows.Count >0)
						{
							HttpContext.Session.SetString("MahatProjectsusername", dt.Rows[0]["username"].ToString());
							HttpContext.Session.SetString("MahatProjectsloginUserID", dt.Rows[0]["id"].ToString());
							
							HttpContext.Session.SetString("MahatProjectstoken", dt.Rows[0]["token"].ToString());
							HttpContext.Session.SetString("MahatProjectsuserrole", dt.Rows[0]["userrole"].ToString());
                            if(dt.Rows[0]["profilepicture"].ToString()!=null && dt.Rows[0]["profilepicture"].ToString()!="")
                            HttpContext.Session.SetString("MahatProjectsprofilepicture", dt.Rows[0]["profilepicture"].ToString());
                            else
                            HttpContext.Session.SetString("MahatProjectsprofilepicture", baseUrl+"/Icon_set/User.png");

                            HttpContext.Session.SetString("MahatProjectstenantlogo", baseUrl+"/images/logo.png");

							
							
							message ="Login Success";

							
HttpContext.Session.SetString("MahatProjectsshowtenant", "N");
                        if (dt.Rows[0]["viewertenantids"].ToString().Contains(","))
                        {
                            HttpContext.Session.SetString("MahatProjectsshowtenant", "Y");
                        }

                                                                               HttpContext.Session.SetString("MahatProjectschoosedtenantname", dt.Rows[0]["tenantname"].ToString());

                                                                         
																			HttpContext.Session.SetString("MahatProjectschoosedtenantid", dt.Rows[0]["tenantid"].ToString());
                                                                             HttpContext.Session.SetString("natureofbusiness", dt.Rows[0]["natureofbusiness"].ToString());
                                                                    
																			if(dt.Rows[0]["tenantid"].ToString() !="0")
																			{
                                                                                     HttpContext.Session.SetString("partyname",  dt.Rows[0]["natureofbusiness"].ToString());
																					HttpContext.Session.SetString("module", "client");

																			} else
						                                                    {
                                                                                HttpContext.Session.SetString("partyname",  dt.Rows[0]["natureofbusiness"].ToString());
                                                                                 HttpContext.Session.SetString("MahatProjectsshowtenant", "Y");
                                                                                                            HttpContext.Session.SetString("module", "admin");

                                                                                HttpContext.Session.SetString("MahatProjectschoosedtenantid", "");
                                                                             }




                                            

							//ROLE SECTION



							HttpResponseMessage response_roles = await ApiClient.GET_ApiValuesGetRespnse(client,"api/users/get_roleAuthorizations?viewactionroles="+dt.Rows[0]["userrole"].ToString());

							
							if (response_roles.IsSuccessStatusCode)
							{
								
								DataTable dt_roles = await response_roles.Content.ReadAsAsync<DataTable>();
								string role_JSON = await response_roles.Content.ReadAsStringAsync();
                                if(dt_roles.Rows.Count ==0){
                                        ViewData["message"]="You are not authorized to access MahatProjects , Please contact administrator";
                                        return View(model); 
                                }
            
								HttpContext.Session.SetSession("MahatProjectsroles", dt_roles);
								HttpContext.Session.SetString("MahatProjectsrole_JSON", role_JSON);
								

								
							} 
												


							//ROLE SECTION



							//MENU SECTION



							HttpResponseMessage response_menu = await ApiClient.GET_ApiValuesGetRespnse(client,"api/users/get_project_Menu?viewactionroles="+dt.Rows[0]["userrole"].ToString()+"&SubSystem=Admin");
							
							if (response_menu.IsSuccessStatusCode)
							{
								
								DataTable dt_menu = await response_menu.Content.ReadAsAsync<DataTable>();
								string menu_JSON = await response_menu.Content.ReadAsStringAsync();
								HttpContext.Session.SetSession("MahatProjectsmenu", dt_menu);
								 var assembly = typeof(usersController).Assembly;
                                    System.Resources.ResourceManager _resourceManager = new System.Resources.ResourceManager("Admin.Resource", assembly);
 
                                     List<dynamic> menuItems = JsonConvert.DeserializeObject<List<dynamic>>(menu_JSON);

                                    foreach (dynamic menuItem in menuItems)
                                    { 
					                    string projectElementName = System.Text.RegularExpressions.Regex.Replace(menuItem.projectelementname.ToString(), @"\s+", "").ToLower();
					                    string translatedProjectElementName = _resourceManager.GetString(projectElementName);
					                    string parentnode = System.Text.RegularExpressions.Regex.Replace(menuItem.parentnode.ToString(), @"\s+", "").ToLower();
					                    string translatedparentnode = _resourceManager.GetString(parentnode);
					                    menuItem.projectelementname = translatedProjectElementName;
					                    menuItem.parentnode = translatedparentnode;
                                    }
				                    menu_JSON = JsonConvert.SerializeObject(menuItems);
			                        HttpContext.Session.SetString("MahatProjectsmenu_JSON", menu_JSON);
								

								
							} 
												


							//MENU SECTION
					
						}
						else
						{
							 message = await ApiClient.Post_ApiValuesGetString(client,"api/userlockout/ins_userlockout", objuserlockoutModel);
						
							 
						}
						}
						else
						{
							message ="Response Failed";
						}
				}
				catch (Exception ex)
				{
                      _logger.LogError(ex,"An exception occurred in - users / Login, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
                   
					message = ex.Message;
				}
				ViewData["message"] = message;
				if(message=="Login Success"){


                    
					return RedirectToAction("UpdateProfile","users", new {@id=HttpContext.Session.GetString("MahatProjectsloginUserID")});
					 
				}else{
				   
					return View(model);
				}

			}//Register_user
       [HttpPost]
        public virtual async Task<String> appregister([FromBody] userloginModel model)
        {
            string message = "";
            string redirecturl = "";

            try
            {
				//IOS , ANDROID and WEB etc
                if (model.source == null || model.source == "")
                {

                    message = "|source is required|";

                }
				//Anything
                if (model.username == null || model.username == "")
                {

                    message += "|username is required|";

                }
				 
				//UUID Of android or IOS 
                if (model.deviceid == null || model.deviceid == "")
                {

                    message += "|deviceid is required|";

                }

                if (model.notificationid == null || model.notificationid == "")
                {

                    message += "|notificationid is required|";

                }


                if (model.accesskey == null || model.accesskey == "")
                {
                    message += "|accesskey is Required|";
                }

                else if (model.accesskey != accesskey)
                {
                    message += "|Invalid accesskey|";
                }
                 
                if (message == "")
                {
                     HttpResponseMessage response = await ApiClient.Post_ApiValuesGetRespnse(getHttpClient(),"api/users/CheckAuthentication", model);
						
                    if (response.IsSuccessStatusCode)
                    {
                        DataTable dt = await response.Content.ReadAsAsync<DataTable>();
                        if (dt.Rows.Count > 0)
                        {

                            message = "Login Success";
                            redirecturl = adminUrl + "/" + "users/toapp?usersid=" + dt.Rows[0]["id"].ToString();
                            

                            
HttpContext.Session.SetString("MahatProjectsshowtenant", "N");
                        if (dt.Rows[0]["viewertenantids"].ToString().Contains(","))
                        {
                            HttpContext.Session.SetString("MahatProjectsshowtenant", "Y");
                        }

                                                                               HttpContext.Session.SetString("MahatProjectschoosedtenantname", dt.Rows[0]["tenantname"].ToString());

                                                                         
																			HttpContext.Session.SetString("MahatProjectschoosedtenantid", dt.Rows[0]["tenantid"].ToString());
                                                                             HttpContext.Session.SetString("natureofbusiness", dt.Rows[0]["natureofbusiness"].ToString());
                                                                    
																			if(dt.Rows[0]["tenantid"].ToString() !="0")
																			{
                                                                                     HttpContext.Session.SetString("partyname",  dt.Rows[0]["natureofbusiness"].ToString());
																					HttpContext.Session.SetString("module", "client");

																			} else
						                                                    {
                                                                                HttpContext.Session.SetString("partyname",  dt.Rows[0]["natureofbusiness"].ToString());
                                                                                 HttpContext.Session.SetString("MahatProjectsshowtenant", "Y");
                                                                                                            HttpContext.Session.SetString("module", "admin");

                                                                                HttpContext.Session.SetString("MahatProjectschoosedtenantid", "");
                                                                             }




                                            


                            //ROLE SECTION
                            HttpResponseMessage response_roles = await ApiClient.GET_ApiValuesGetRespnse(client,"api/users/get_roleAuthorizations?viewactionroles="+dt.Rows[0]["userrole"].ToString());

							
							if (response_roles.IsSuccessStatusCode)
							{
								
								DataTable dt_roles = await response_roles.Content.ReadAsAsync<DataTable>();
								if(dt_roles.Rows.Count ==0){
                                        message="You are not authorized to access MahatProjects , Please contact administrator";
                                         
                                }
            
								 
							}


 

                        }
                        else
                        {
                            message = "user not available";

                        }
                    }
                    else
                    {
                        message = "MahatProjects's API Response Failed";
                    }
                }
                else
                {

                    message = "Validation Failed : " + message;
                }




            }
            catch (Exception ex)
            {
                
                  _logger.LogError(ex,"An exception occurred in - users / appregister, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
                  
                message = ex.Message;
            }

        

            if (message == "Login Success")
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "success",
                    message = "success",
                    redirecturl = redirecturl
                });

            }
            else
            {

                return JsonConvert.SerializeObject(new
                {
                    status = "failed",
                    message = message,
                    redirecturl = ""
                });
            }
        }

		[AllowAnonymous]
        public virtual IActionResult applogin()
        {
            return RedirectToAction("Login");
        }

        //Mobile Or UUID FROM Mobile APP
		//uuid or mobile number is username
[AllowAnonymous]
        [HttpPost]
        public virtual async Task<String> applogin(userloginModel model)
        {
            string message = "";
            string redirecturl = "";

            try
            {

                if (model.source == null || model.source == "")
                {

                    message = "|source required|";

                }

                if (model.username == null || model.username == "")
                {

                    message += "|username required|";

                }

                if (model.accesskey == null || model.accesskey == "")
                {
                    message += "|accesskey Required|";
                }

                else if (model.accesskey != accesskey)
                {

                    Console.WriteLine(model.accesskey);
                    Console.WriteLine(accesskey);

                    message += "|Invalid accesskey|";
                }

           

                if (message == "")
                {
  				 HttpResponseMessage response = await ApiClient.Post_ApiValuesGetRespnse(getHttpClient(),"api/users/CheckAuthentication", model);
				

                    if (response.IsSuccessStatusCode)
                    {
                        DataTable dt = await response.Content.ReadAsAsync<DataTable>();
                        if (dt.Rows.Count > 0)
                        {
                            message = "Login Success";
    
                            redirecturl = adminUrl + "/" + "users/toapp?usersid=" + dt.Rows[0]["id"].ToString();
                            

                            
HttpContext.Session.SetString("MahatProjectsshowtenant", "N");
                        if (dt.Rows[0]["viewertenantids"].ToString().Contains(","))
                        {
                            HttpContext.Session.SetString("MahatProjectsshowtenant", "Y");
                        }

                                                                               HttpContext.Session.SetString("MahatProjectschoosedtenantname", dt.Rows[0]["tenantname"].ToString());

                                                                         
																			HttpContext.Session.SetString("MahatProjectschoosedtenantid", dt.Rows[0]["tenantid"].ToString());
                                                                             HttpContext.Session.SetString("natureofbusiness", dt.Rows[0]["natureofbusiness"].ToString());
                                                                    
																			if(dt.Rows[0]["tenantid"].ToString() !="0")
																			{
                                                                                     HttpContext.Session.SetString("partyname",  dt.Rows[0]["natureofbusiness"].ToString());
																					HttpContext.Session.SetString("module", "client");

																			} else
						                                                    {
                                                                                HttpContext.Session.SetString("partyname",  dt.Rows[0]["natureofbusiness"].ToString());
                                                                                 HttpContext.Session.SetString("MahatProjectsshowtenant", "Y");
                                                                                                            HttpContext.Session.SetString("module", "admin");

                                                                                HttpContext.Session.SetString("MahatProjectschoosedtenantid", "");
                                                                             }




                                            
 
                            HttpResponseMessage response_roles = await ApiClient.GET_ApiValuesGetRespnse(client,"api/users/get_roleAuthorizations?viewactionroles="+dt.Rows[0]["userrole"].ToString());

							
							if (response_roles.IsSuccessStatusCode)
							{
								
								DataTable dt_roles = await response_roles.Content.ReadAsAsync<DataTable>();
								if(dt_roles.Rows.Count ==0){
                                        message="You are not authorized to access MahatProjects , Please contact administrator";
                                         
                                }
            
								 
							}

                         }
                        else
                        {
                            message = "user not available";
  

                        }
                    }
                    else
                    {
                        message = "MahatProjects's API Response Failed";
                    }
                }
                else
                {

                    message = "Validation Failed : " + message;
                }




            }
            catch (Exception ex)
            {
                
                 _logger.LogError(ex,"An exception occurred in - users / applogin, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
                  
                message = ex.Message;
            }

            

            if (message == "Login Success")
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "success",
                    message = "success",
                    redirecturl = redirecturl
                });

            }
            else
            {

                return JsonConvert.SerializeObject(new
                {
                    status = "failed",
                    message = message,
                    redirecturl = ""
                });
            }
        }
 //Go_to_app_with_user_id
        public virtual async Task<IActionResult> toapp(string usersid)
        {
            string message = "";

            userloginModel model = new userloginModel();
            try
            {
                model.username = usersid;
                model.source = "allowbyid";
                HttpResponseMessage response = await ApiClient.Post_ApiValuesGetRespnse(getHttpClient(),"api/users/CheckAuthentication", model);
					
               

                if (response.IsSuccessStatusCode)
                {
                    DataTable dt = await response.Content.ReadAsAsync<DataTable>();
                    if (dt.Rows.Count > 0)
                    {
                            HttpContext.Session.SetString("MahatProjectsusername", dt.Rows[0]["username"].ToString());
							HttpContext.Session.SetString("MahatProjectsloginUserID", dt.Rows[0]["id"].ToString());
							
							HttpContext.Session.SetString("MahatProjectstoken", dt.Rows[0]["token"].ToString());
							HttpContext.Session.SetString("MahatProjectsuserrole", dt.Rows[0]["userrole"].ToString());
                            if(dt.Rows[0]["profilepicture"].ToString()!=null && dt.Rows[0]["profilepicture"].ToString()!="")
                            HttpContext.Session.SetString("MahatProjectsprofilepicture", dt.Rows[0]["profilepicture"].ToString());
                            else
                            HttpContext.Session.SetString("MahatProjectsprofilepicture", baseUrl+"/Icon_set/User.png");

                            HttpContext.Session.SetString("MahatProjectstenantlogo", baseUrl+"/images/logo.png");
                            message = "Login Success";

                         

							
HttpContext.Session.SetString("MahatProjectsshowtenant", "N");
                        if (dt.Rows[0]["viewertenantids"].ToString().Contains(","))
                        {
                            HttpContext.Session.SetString("MahatProjectsshowtenant", "Y");
                        }

                                                                               HttpContext.Session.SetString("MahatProjectschoosedtenantname", dt.Rows[0]["tenantname"].ToString());

                                                                         
																			HttpContext.Session.SetString("MahatProjectschoosedtenantid", dt.Rows[0]["tenantid"].ToString());
                                                                             HttpContext.Session.SetString("natureofbusiness", dt.Rows[0]["natureofbusiness"].ToString());
                                                                    
																			if(dt.Rows[0]["tenantid"].ToString() !="0")
																			{
                                                                                     HttpContext.Session.SetString("partyname",  dt.Rows[0]["natureofbusiness"].ToString());
																					HttpContext.Session.SetString("module", "client");

																			} else
						                                                    {
                                                                                HttpContext.Session.SetString("partyname",  dt.Rows[0]["natureofbusiness"].ToString());
                                                                                 HttpContext.Session.SetString("MahatProjectsshowtenant", "Y");
                                                                                                            HttpContext.Session.SetString("module", "admin");

                                                                                HttpContext.Session.SetString("MahatProjectschoosedtenantid", "");
                                                                             }




                                            

							//ROLE SECTION



							HttpResponseMessage response_roles = await ApiClient.GET_ApiValuesGetRespnse(client,"api/users/get_roleAuthorizations?viewactionroles="+dt.Rows[0]["userrole"].ToString());

							
							if (response_roles.IsSuccessStatusCode)
							{
								
								DataTable dt_roles = await response_roles.Content.ReadAsAsync<DataTable>();
								string role_JSON = await response_roles.Content.ReadAsStringAsync();
                                if(dt_roles.Rows.Count ==0){
                                        ViewData["message"]="You are not authorized to access MahatProjects , Please contact administrator";
                                        return View(model); 
                                }
            
								HttpContext.Session.SetSession("MahatProjectsroles", dt_roles);
								HttpContext.Session.SetString("MahatProjectsrole_JSON", role_JSON);
								

								
							} 
												


							//ROLE SECTION



							//MENU SECTION



							HttpResponseMessage response_menu = await ApiClient.GET_ApiValuesGetRespnse(client,"api/users/get_project_Menu?viewactionroles="+dt.Rows[0]["userrole"].ToString()+"&SubSystem=Admin");
							
							if (response_menu.IsSuccessStatusCode)
							{
								
								DataTable dt_menu = await response_menu.Content.ReadAsAsync<DataTable>();
								string menu_JSON = await response_menu.Content.ReadAsStringAsync();
								HttpContext.Session.SetSession("MahatProjectsmenu", dt_menu);
								 var assembly = typeof(usersController).Assembly;
                                    System.Resources.ResourceManager _resourceManager = new System.Resources.ResourceManager("Admin.Resource", assembly);
 
                                     List<dynamic> menuItems = JsonConvert.DeserializeObject<List<dynamic>>(menu_JSON);

                                    foreach (dynamic menuItem in menuItems)
                                    { 
					                    string projectElementName = System.Text.RegularExpressions.Regex.Replace(menuItem.projectelementname.ToString(), @"\s+", "").ToLower();
					                    string translatedProjectElementName = _resourceManager.GetString(projectElementName);
					                    string parentnode = System.Text.RegularExpressions.Regex.Replace(menuItem.parentnode.ToString(), @"\s+", "").ToLower();
					                    string translatedparentnode = _resourceManager.GetString(parentnode);
					                    menuItem.projectelementname = translatedProjectElementName;
					                    menuItem.parentnode = translatedparentnode;
                                    }
				                    menu_JSON = JsonConvert.SerializeObject(menuItems);
			                        HttpContext.Session.SetString("MahatProjectsmenu_JSON", menu_JSON);
								

								
							} 
												


							//MENU SECTION
					


                    }
                    else
                    {
                         message = "user not exists";


                    }
                }
                else
                {
                    message = "Response Failed";
                }
            }
            catch (Exception ex)
            {
               
                _logger.LogError(ex,"An exception occurred in - users / toapp, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
                 
                message = ex.Message;
            }
            TempData["message"] = message;
            if (message == "Login Success")
            {
                 
                 return RedirectToAction("UpdateProfile","users", new {@id=HttpContext.Session.GetString("MahatProjectsloginUserID")});
					
            }
            else
            {

                return RedirectToAction("Login");
            }
        }
        public virtual IActionResult forgotpassword()
        {
            HttpContext.Session.SetString("ReferrerUrl", "");
            TempData["messagereferer"] = "donotrefer";
            return View();
        }
        [HttpPost()]
        public virtual async Task<IActionResult> forgotpassword(userloginModel loginmodel)
        {
            string message = "";
            string pwd = "";
            usersModel model = new usersModel();
            try
            {
                loginmodel.source = "Internal";
                HttpResponseMessage response_Password = await ApiClient.Post_ApiValuesGetRespnse(getHttpClient(), "api/users/get_decryptedPassword", loginmodel);
                if (response_Password.IsSuccessStatusCode)
                {
                    message = await response_Password.Content.ReadAsAsync<string>();

                    loginmodel.userpassword = message;
                    pwd = message;
                }
                HttpResponseMessage response = await ApiClient.Post_ApiValuesGetRespnse(client, "api/users/CheckAuthentication", loginmodel);

                if (response.IsSuccessStatusCode)
                {
                    var jsonObj = await response.Content.ReadAsStringAsync();
                    if (jsonObj.Length > 2)
                    {
                        jsonObj = jsonObj.Substring(1, jsonObj.Length - 2);
                        model = JsonConvert.DeserializeObject<usersModel>(jsonObj);
                        message = "Login Success";
                        model.userpassword = pwd;
                    }
                    else
                    {
                        message = "Login Failed";
                    }
                }
                else
                {
                    message = "Response Failed";
                }
            }
            catch (Exception ex)
            {
               
                 _logger.LogError(ex,"An exception occurred in - users / forgotpassword, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
               
                message = ex.Message;
            }
            ViewData["message"] = "User Info not available - Please contact Admin ";
            if (message == "Login Success")
            {

                if (model.emailid != null && model.emailid != "")
                {

                    MailSender maillog = new MailSender();
                    bool mailsent = await maillog.sendMail("users"
                        , "forgotpassword"
                        , model.id.ToString()
                        , "Forgot Password"
                        , _mailSettings
                        , "1"
                        , client, model.username, model.userpassword);

                    if (mailsent)
                    {
                        ViewData["message"] = "User Info Sent to your registered email";
                    }
                    else
                    {
                        ViewData["message"] = "User Info found - Mail Sending Failed - Please contact Admin ";
                    }


                }
                else
                {
                    ViewData["message"] = "User Info found - Mail Sending Failed - Please contact Admin ";
                }



                return View(loginmodel);
            }
            else
            {
                return View(loginmodel);
            }
;
        }

        public virtual async Task<string> get_Dashboard_Items(string viewactionroles)
		{
			string dashmenu = await ApiClient.Get_ApiValues(getHttpClient(), "api/users/get_Dashboard_Items?viewactionroles=" + HttpContext.Session.GetString("MahatProjectsuserrole"));
			if (dashmenu != "")
			{
				var assembly = typeof(usersController).Assembly;
				System.Resources.ResourceManager _resourceManager = new System.Resources.ResourceManager("Admin.Resource", assembly);

				List<dynamic> menuItems = JsonConvert.DeserializeObject<List<dynamic>>(dashmenu);

				foreach (dynamic menuItem in menuItems)
				{
					string actiondisplayname = System.Text.RegularExpressions.Regex.Replace(menuItem.actiondisplayname.ToString(), @"\s+", "").ToLower();
					string translatedactiondisplayname = _resourceManager.GetString(actiondisplayname);
					menuItem.actiondisplayname = translatedactiondisplayname;
				}
				dashmenu = JsonConvert.SerializeObject(menuItems);
				
			}
			return dashmenu;

		}

[AllowAnonymous]
                public IActionResult OpenidLoginNav(string provider, string returnUrl = "/")
		        {
			        authProvider = provider;
			        HttpContext.Session.SetString("MahatProjectsAuthProvider", provider);

			        Console.WriteLine(provider + " provider ");
			        try
			        {
				        var authenticationProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
				        {
					        RedirectUri = Url.Action("OpenidLogin", "users", new { returnUrl })
				        };
				            return Challenge(authenticationProperties, authProvider);
			        }
			        catch (Exception ex)
			        {
				        Console.WriteLine("came inside Exception" + ex.Message);
			        }
			        return null;
		        }
[AllowAnonymous]
                public async Task<IActionResult> OpenidLogin()
		        {
			        userClaims model = new userClaims();
			        string message = "";
			        try
			        {

				        HttpContext.Session.SetString("ReferrerUrl", "");
				        string referrerUrl = HttpContext.Request.Headers["Referer"];
				        if (referrerUrl != null && TempData["messagereferer"] == null)
				        {
					        HttpContext.Session.SetString("ReferrerUrl", referrerUrl);
				        }
                        
                        
                    if(HttpContext.Session.GetString("MahatProjectsAuthProvider") != "oidc"){
                        model.token = await HttpContext.GetTokenAsync("id_token");
                        model.RefreshToken = await HttpContext.GetTokenAsync("refresh_token");

                        Console.WriteLine(model.ExpiryTime + "  expiry time");

                        var tokenHandler = new JwtSecurityTokenHandler();
                        var jwtToken = tokenHandler.ReadJwtToken(model.token);
                        Console.WriteLine(model.token);
                        var claims = jwtToken.Claims;
                        var exp = claims.FirstOrDefault(c => c.Type == "exp")?.Value;
                        var issuer = claims.FirstOrDefault(c => c.Type == "iss")?.Value;
                        Console.WriteLine(issuer);

                        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                        DateTime convertedTime = unixEpoch.AddSeconds(Double.Parse(exp));

                        DateTime newTime = convertedTime.AddHours(5).AddMinutes(30);


                        model.ExpiryTime = newTime;
                        if (issuer.Contains("google"))
                        {
                            model.AuthProvider = "GoogleOpenIdConnect";
                            authProvider = model.AuthProvider;
                        }

                        else if (issuer.Contains("sts.windows"))
                        {
                            authProvider = model.AuthProvider;

                            model.AuthProvider = "AzureADJwtBearer";
					

                        }
                        model.email = claims.FirstOrDefault(c => c.Type == "upn")?.Value;
                        string role = claims.FirstOrDefault(c => c.Type == "userrole")?.Value;

                        if (model.email == null)
                        {
                            model.email = claims.FirstOrDefault(c => c.Type == "email")?.Value;
                            Console.WriteLine(model.email + " email 2");
                        }
                        if (!string.IsNullOrEmpty(role))
                        {
                            model.role = role;
                        }
                        else
                        {
                            model.role = "Client Admin";
                        }
model.first_name = model.email?.Split('@')[0];
                    }
                    
				        model.AccessToken = await HttpContext.GetTokenAsync("access_token");

				        model.source = model.AuthProvider;

				        Console.WriteLine("id_token " + model.token);
				        Console.WriteLine("access token " + model.AccessToken);
				        Console.WriteLine("refresh token " + model.RefreshToken);

				        HttpResponseMessage response = await ApiClient.Post_ApiValuesGetRespnse(getHttpClient(),"api/users/CheckOAuthAuthentication", model);
						
						if (response.IsSuccessStatusCode)
						{
						    DataTable dt = await response.Content.ReadAsAsync<DataTable>();
						    if (dt.Rows.Count >0)
						    {
							    HttpContext.Session.SetString("MahatProjectsusername", dt.Rows[0]["username"].ToString());
							    HttpContext.Session.SetString("MahatProjectsloginUserID", dt.Rows[0]["id"].ToString());
							
							    HttpContext.Session.SetString("MahatProjectstoken", model.token);

						        //HttpContext.Session.SetString("MahatProjectstokenExpiry", model.ExpiryTime.ToString()?? "");

						        HttpContext.Session.SetString("MahatProjectsAccesstoken", model.AccessToken);
						        HttpContext.Session.SetString("MahatProjectsAuthProvider", model.AuthProvider);
						        authProvider = model.AuthProvider;
						        HttpContext.Session.SetString("MahatProjectsRefreshtoken", model.RefreshToken?? "");
							    HttpContext.Session.SetString("MahatProjectsuserrole", dt.Rows[0]["userrole"].ToString());
                                if(dt.Rows[0]["profilepicture"].ToString()!=null && dt.Rows[0]["profilepicture"].ToString()!="")
                                HttpContext.Session.SetString("MahatProjectsprofilepicture", dt.Rows[0]["profilepicture"].ToString());
                                else
                                HttpContext.Session.SetString("MahatProjectsprofilepicture", baseUrl+"/Icon_set/User.png");

                                HttpContext.Session.SetString("MahatProjectstenantlogo", baseUrl+"/images/logo.png");
HttpContext.Session.SetString("MahatProjectsshowtenant", "N");
                        if (dt.Rows[0]["viewertenantids"].ToString().Contains(","))
                        {
                            HttpContext.Session.SetString("MahatProjectsshowtenant", "Y");
                        }

                                                                               HttpContext.Session.SetString("MahatProjectschoosedtenantname", dt.Rows[0]["tenantname"].ToString());

                                                                         
																			HttpContext.Session.SetString("MahatProjectschoosedtenantid", dt.Rows[0]["tenantid"].ToString());
                                                                             HttpContext.Session.SetString("natureofbusiness", dt.Rows[0]["natureofbusiness"].ToString());
                                                                    
																			if(dt.Rows[0]["tenantid"].ToString() !="0")
																			{
                                                                                     HttpContext.Session.SetString("partyname",  dt.Rows[0]["natureofbusiness"].ToString());
																					HttpContext.Session.SetString("module", "client");

																			} else
						                                                    {
                                                                                HttpContext.Session.SetString("partyname",  dt.Rows[0]["natureofbusiness"].ToString());
                                                                                 HttpContext.Session.SetString("MahatProjectsshowtenant", "Y");
                                                                                                            HttpContext.Session.SetString("module", "admin");

                                                                                HttpContext.Session.SetString("MahatProjectschoosedtenantid", "");
                                                                             }

							    
							
							    message ="Login Success";

							    

							    //ROLE SECTION



							    HttpResponseMessage response_roles = await ApiClient.GET_ApiValuesGetRespnse(client,"api/users/get_roleAuthorizations?viewactionroles="+dt.Rows[0]["userrole"].ToString());

							
							    if (response_roles.IsSuccessStatusCode)
							    {
								
								    DataTable dt_roles = await response_roles.Content.ReadAsAsync<DataTable>();
								    string role_JSON = await response_roles.Content.ReadAsStringAsync();
                                    if(dt_roles.Rows.Count ==0){
                                            ViewData["message"]="You are not authorized to access MahatProjects , Please contact administrator";
                                            return RedirectToAction("Login", "users");
                                    }
            
								    HttpContext.Session.SetSession("MahatProjectsroles", dt_roles);
								    HttpContext.Session.SetString("MahatProjectsrole_JSON", role_JSON);
								

								
							    } 
												


							    //ROLE SECTION



							    //MENU SECTION



							    HttpResponseMessage response_menu = await ApiClient.GET_ApiValuesGetRespnse(client,"api/users/get_project_Menu?viewactionroles="+dt.Rows[0]["userrole"].ToString()+"&SubSystem=Admin");
							
							    if (response_menu.IsSuccessStatusCode)
							    {
								
								    DataTable dt_menu = await response_menu.Content.ReadAsAsync<DataTable>();
								    string menu_JSON = await response_menu.Content.ReadAsStringAsync();
								    HttpContext.Session.SetSession("MahatProjectsmenu", dt_menu);
								        var assembly = typeof(usersController).Assembly;
                                        System.Resources.ResourceManager _resourceManager = new System.Resources.ResourceManager("Admin.Resource", assembly);
 
                                            List<dynamic> menuItems = JsonConvert.DeserializeObject<List<dynamic>>(menu_JSON);

                                        foreach (dynamic menuItem in menuItems)
                                        { 
					                        string projectElementName = System.Text.RegularExpressions.Regex.Replace(menuItem.projectelementname.ToString(), @"\s+", "").ToLower();
					                        string translatedProjectElementName = _resourceManager.GetString(projectElementName);
					                        string parentnode = System.Text.RegularExpressions.Regex.Replace(menuItem.parentnode.ToString(), @"\s+", "").ToLower();
					                        string translatedparentnode = _resourceManager.GetString(parentnode);
					                        menuItem.projectelementname = translatedProjectElementName;
					                        menuItem.parentnode = translatedparentnode;
                                        }
				                        menu_JSON = JsonConvert.SerializeObject(menuItems);
			                            HttpContext.Session.SetString("MahatProjectsmenu_JSON", menu_JSON);
								

								
							    } 
												


							    //MENU SECTION
					
						    }
						 }
						else
						{
							message ="Response Failed";
						}
				    }
				    catch (Exception ex)
				    {
                            _logger.LogError(ex,"An exception occurred in - users / Login, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
                   
					    message = ex.Message;
				    }
				    ViewData["message"] = message;
				    if(message=="Login Success"){

                            string redirecturl = HttpContext.Session.GetString("ReferrerUrl");
                    HttpContext.Session.SetString("ReferrerUrl", "");
                    //if (!String.IsNullOrEmpty(redirecturl))// && Url.IsLocalUrl(redirecturl))
                    //    return Redirect(redirecturl);

                        
					    return RedirectToAction("UpdateProfile","users", new {@id=HttpContext.Session.GetString("MahatProjectsloginUserID")});
					 
				    }else{
				   
					    return RedirectToAction("Login", "users");
				    }

			    }

			public virtual IActionResult List_of_User_Profiles()
			{
				return View();
			}
			public virtual IActionResult IndexPlatform()
					{
						 HttpContext.Session.SetString("MahatProjectschoosedtenantid","");
						return View();
					}	
			[HttpGet()]
			public virtual async Task<string> get_List_of_User_Profiles(string tenantid
)
			{
				
				return await ApiClient.Get_ApiValues(getHttpClient(), "api/users/List_of_User_Profiles?tenantid="+tenantid+"&loginUserID="+HttpContext.Session.GetString("MahatProjectsloginUserID")
);
			}
			 

				
			  public virtual async Task<IActionResult> ChangePassword(string id)
			  {
                    string redirectTo="";
            if(HttpContext.Session.GetString("MahatProjectsrole_JSON") != null){
                
			DataTable MahatProjectsrole_JSON =HttpContext.Session.GetSession<DataTable>("MahatProjectsroles");
			DataView dv = new DataView(MahatProjectsrole_JSON);
			dv.RowFilter = "controllername='users' AND viewname='list'";

			if(dv.Count  >0){
				redirectTo = dv[0]["actionmethodname"] as string;
							 
			}	 
                    try
                        {
                            var jsonObjusers = await ApiClient.Get_ApiValues(getHttpClient(), "api/users/getById_users?id="+id+"&loginUserID="+HttpContext.Session.GetString("MahatProjectsloginUserID"));
                                 
						         var model = JsonConvert.DeserializeObject<usersModel>(jsonObjusers);


                
					      
                     
						   
						     

                            usersChangePasswordModel objModel=new usersChangePasswordModel();     
                            objModel.usersid=model.id;
						    return View(objModel);

                        }catch(Exception ex){
                            _logger.LogError(ex,"An exception occurred in - users / ChangePassword, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
              
                            TempData["errMessage"] = "Error while fetching data - Contact Administrator";
                            return RedirectToAction(redirectTo);
                        }
					 

                }
                TempData["errMessage"] = "Session Expired";
                return RedirectToAction("Logout","users");

			  }	
			  [HttpPost()]
				public virtual async Task<string> ChangePassword(usersChangePasswordModel model, IFormCollection collection)
				{
					string strReturnMessage = "";
					try
					{
							ModelState.Remove("id");
							 
							if(HttpContext.Session.GetString("MahatProjectsloginUserID") != null)
					model.updated_by_id =Convert.ToInt64 (HttpContext.Session.GetString("MahatProjectsloginUserID"));
					else
					return "Session Expired";
							 
							if (ModelState.IsValid)
							{
									    strReturnMessage = await ApiClient.Post_ApiValuesGetString(getHttpClient(),"api/users/ChangePassword", model);
									  
							}
							else
							{
									var errorMessages = ModelState.Where(entry => entry.Value.Errors.Any()).SelectMany(entry => entry.Value.Errors.Select(error => $"{entry.Key}: {error.ErrorMessage}"));
                               var errorCollection = string.Join(" | ", errorMessages);
                               strReturnMessage = errorCollection;
							}
					}
					catch (Exception ex)
					{
                        _logger.LogError(ex,"An exception occurred in - users / ChangePassword, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
              
						strReturnMessage = ex.Message;
					}
					ViewData["message"] = strReturnMessage;
					    if(strReturnMessage.Replace("\"", "")=="201.1"){
							TempData["message"] = "Success";
							
							return "Success";
						}else{
							if(strReturnMessage=="401.1")
									strReturnMessage = "Authorization Failed";

							return strReturnMessage;
						}
		
				}

				
			  public virtual async Task<string> getById_allinfo_users(string id)
			  {
					return await ApiClient.Get_ApiValues(getHttpClient(), "api/users/getById_allinfo_users?id="+id);
					 
			  }






                    
                     

				}


			}
