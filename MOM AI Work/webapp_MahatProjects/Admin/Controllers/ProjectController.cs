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



    //This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:31:56
				
                
                
                
                
				public class ProjectController : BaseController
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
                    private readonly ILogger<ProjectController> _logger;
                    private static string authProvider = "";

		private string apikey = "";
		private string endpoint = "";

              private string audiofilepath = "";
        public ProjectController(IConfiguration configuration,IHttpContextAccessor accessor,IOptions<ApiSettings> ApiSettings, IOptions<MailSettings> MailSettings, IWebHostEnvironment env, ILogger<ProjectController> logger):base( configuration)
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
            apikey = _balSettings.Value.apikey;
            endpoint = _balSettings.Value.endpoint;
            audiofilepath = _balSettings.Value.audiofilepath;
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
	

					
			  public virtual IActionResult Add_Project()
			  {
					return View();
			  }	
			  [HttpPost()]
			public virtual async Task<string> Add_Project(ProjectModel model, IFormCollection collection)
			{
				string strReturnMessage = "";
				
				try
				{
					ModelState.Remove("id");
					ModelState.Remove("created_by_id");
                    ModelState.Remove("craftmyapp_actionmethodname");
                    model.craftmyapp_actionmethodname="Add_Project";
					if(HttpContext.Session.GetString("MahatProjectsloginUserID") != null)
								model.created_by_id =Convert.ToInt64(HttpContext.Session.GetString("MahatProjectsloginUserID"));
								else
								return "Session Expired";
					ModelState.Remove("video_upload");

					
			 	 
					 if (ModelState.IsValid)
					 {
							 ProjectModelValidator validator = new ProjectModelValidator();
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
string uploadFileName = System.Text.RegularExpressions.Regex.Replace(filename.Split('.').First(), @"[^0-9a-zA-Z_.]+", "").Replace(" ", String.Empty)+"_"+"Project_" +rnd.Next(1, 10000).ToString() + DateTime.Now.ToString("ddMMyyHHmmss")+ fileExtention;
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
if (file.Name == "video_upload")
{
uploadFileName = baseUrl + "/uploads/" + uploadFileName;
model.video_upload += "|" + uploadFileName +"|";
}
}
}

                                 response_model obj_response_model = await ApiClient.Post_ApiValuesGetResponseModel(getHttpClient(),"api/Project/Add_Project", model);
                                                                                                strReturnMessage=obj_response_model.message;
                                 model.id=Convert.ToInt64(obj_response_model.id);


						string transcript = await GenerateMinutesFromAudio(audiofilepath, apikey, endpoint);


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
                 
                 _logger.LogError(ex,"An exception occurred in - Project / Add_Project, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
               
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

		public static async Task<TranscriptionResponse> TranscribeAudioAsync(string apiKey, string apiEndpoint, string audioFilePath)
		{
			using (var client = new HttpClient())
			{
				// Set up the Authorization header
				client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

				// Create the form content with the audio file
				using (var form = new MultipartFormDataContent())
				{
					var audioContent = new ByteArrayContent(await File.ReadAllBytesAsync(audioFilePath));
					audioContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg"); // Or "audio/wav" depending on your file format
					form.Add(audioContent, "file", Path.GetFileName(audioFilePath));

					// Set the model and other parameters
					form.Add(new StringContent("whisper-1"), "model");
					form.Add(new StringContent("verbose_json"), "response_format");
					form.Add(new StringContent("word"), "timestamp_granularities");

					// Send the POST request
					var response = await client.PostAsync(apiEndpoint, form);

					// Check for success
					if (response.IsSuccessStatusCode)
					{
						string responseContent = await response.Content.ReadAsStringAsync();
						var transcription = JsonConvert.DeserializeObject<TranscriptionResponse>(responseContent);
						return transcription;
					}
					else
					{
						Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
						return null;
					}
				}
			}
		}
            

            [HttpPost]
        [Route("generate-minutes")]
        public async Task<IActionResult> GenerateMinutesFromAudioAction([FromForm] string audioFilePath, [FromForm] string apiKey, [FromForm] string apiEndpoint)
        {
            try
            {
                // Call the GenerateMinutesFromAudio method and store the result (transcript) in a variable
                string transcript = await GenerateMinutesFromAudio(audioFilePath, apiKey, apiEndpoint);

                // If the transcript is not available, return an error response
                if (string.IsNullOrEmpty(transcript))
                {
                    return BadRequest("Failed to generate minutes from the audio.");
                }

                // Return the transcript as plain text
                return Ok(new { Transcript = transcript });
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

		// Helper method that interacts with the external API and processes the audio file
		private async Task<string> GenerateMinutesFromAudio(string audioFilePath, string apiKey, string apiEndpoint)
		{
			using (var client = new System.Net.Http.HttpClient())
			{
				// Set authorization header
				client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

				try
				{
					using (var form = new System.Net.Http.MultipartFormDataContent())
					{
						// Attach the audio file to the form
						var fileContent = new System.Net.Http.ByteArrayContent(await System.IO.File.ReadAllBytesAsync(audioFilePath));
						fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/wav");
						form.Add(fileContent, "file", Path.GetFileName(audioFilePath));

						// Send the request
						System.Net.Http.HttpResponseMessage response = await client.PostAsync(apiEndpoint, form);

						// Check the response
						if (response.IsSuccessStatusCode)
						{
							string responseContent = await response.Content.ReadAsStringAsync();
							// Assuming the API returns JSON with a "minutes" field
							var responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseContent);
							return responseJson.minutes;
						}
						else
						{
							return null; // API call failed
						}
					}
				}
				catch (Exception)
				{
					return null; // Exception during API call
				}
			}
		}
        public virtual async Task<IActionResult> Update_Project(string id)
			  {

                    string redirectTo="";
                    if(HttpContext.Session.GetString("MahatProjectsrole_JSON") != null){
                            DataTable MahatProjectsrole_JSON =HttpContext.Session.GetSession<DataTable>("MahatProjectsroles");
                            DataView dv = new DataView(MahatProjectsrole_JSON);
                            dv.RowFilter = "controllername='Project' AND viewname='list'";

                            if(dv.Count  >0){
                                redirectTo = dv[0]["actionmethodname"] as string;
							 
                            }

                            try{
                                     var jsonObjProject = await ApiClient.Get_ApiValues(getHttpClient(), "api/Project/getById_Project?id="+id+"&loginUserID="+HttpContext.Session.GetString("MahatProjectsloginUserID"));
                                if(jsonObjProject.Length > 2)
                                {
                                  
                                    var model = JsonConvert.DeserializeObject<ProjectModel>(jsonObjProject);


                
                                     
                                    return View("Add_Project", model);
                                }
                                else
                                {
                    
                                    TempData["message"] = "Data Not Found - Contact Administrator";
                                    return RedirectToAction(redirectTo);
						 
                                }

                            }catch(Exception ex){
                               _logger.LogError(ex,"An exception occurred in - Project / Update_Project, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
              
                                TempData["errMessage"] = "Error while fetching data - Contact Administrator";
                                return RedirectToAction(redirectTo);
                            }

                    }
                    TempData["errMessage"] = "Session Expired";
                    return RedirectToAction("Logout", "users");
                }	
			  [HttpPost()]
				public virtual async Task<string> Update_Project(ProjectModel model, IFormCollection collection)
				{
					string strReturnMessage = "";
					try
					{
							ModelState.Remove("id");
                            ModelState.Remove("craftmyapp_actionmethodname");
                             model.craftmyapp_actionmethodname="Update_Project";
							
							
							if(HttpContext.Session.GetString("MahatProjectsloginUserID") != null)
					model.updated_by_id =Convert.ToInt64 (HttpContext.Session.GetString("MahatProjectsloginUserID"));
					else
					return "Session Expired";
							

                            
							if (ModelState.IsValid)
							{
									ProjectModelValidator validator = new ProjectModelValidator();
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
                                        model.video_upload = collection["video_upload_existing"];

										var files = Request.Form.Files;
foreach (var file in files) 
{
var filename = ContentDispositionHeaderValue
.Parse(file.ContentDisposition)
.FileName
.Trim('"'); 
string fileExtention = "." + filename.Split('.').Last(); 
Random rnd = new Random();
string uploadFileName = System.Text.RegularExpressions.Regex.Replace(filename.Split('.').First(), @"[^0-9a-zA-Z_.]+", "").Replace(" ", String.Empty)+"_"+"Project_" +rnd.Next(1, 10000).ToString() + DateTime.Now.ToString("ddMMyyHHmmss")+ fileExtention;
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
if (file.Name == "video_upload")
{
uploadFileName = baseUrl + "/uploads/" + uploadFileName;
model.video_upload += "|" + uploadFileName +"|";
}
}
}

                                        
                                        
                                        strReturnMessage = await ApiClient.Post_ApiValuesGetString(getHttpClient(),"api/Project/Update_Project", model);
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
                      _logger.LogError(ex,"An exception occurred in - Project / Update_Project, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
              
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
public virtual async Task<IActionResult> Remove_Project(string id)
			{
				string message = "";
				try
				{
						message = await ApiClient.Get_ApiValues(getHttpClient(), "api/Project/Remove_Project?id="+id+"&loginUserID="+HttpContext.Session.GetString("MahatProjectsloginUserID"));
						 if(message.Replace("\"","")=="201.1")
						{
							TempData["message"] = "Success";

						}else{
							TempData["errMessage"] = message.Replace("\"","");
						}
						
				
				
				}
				catch (Exception ex)
				{
                     _logger.LogError(ex,"An exception occurred in - Project / Remove_Project, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
              
                
					 TempData["errMessage"] = ex.Message;
					 message = ex.Message;
				}

				string redirectTo="";
						if(HttpContext.Session.GetString("MahatProjectsrole_JSON") != null){
					DataTable MahatProjectsrole_JSON =HttpContext.Session.GetSession<DataTable>("MahatProjectsroles");
						 DataView dv = new DataView(MahatProjectsrole_JSON);
						 dv.RowFilter = "controllername='Project' AND viewname='list'";

						if(dv.Count  >0){
						    redirectTo = dv[0]["actionmethodname"] as string;
							 
						}

					}
				
				return RedirectToAction(redirectTo);
			}

			        public virtual IActionResult Project_List()
			        {
				        return View();
			        }
			        	
			        [HttpGet()]
			        public virtual async Task<string> get_Project_List( int? pagesize=100 , int? pagenumber=0,string searchterm="",string sortFieldsJson="")
			        {
				        
				        return await ApiClient.Get_ApiValues(getHttpClient(), "api/Project/Project_List?loginUserID="+HttpContext.Session.GetString("MahatProjectsloginUserID")
+ "&pagesize=" + pagesize + "&pagenumber="+ pagenumber + "&searchterm="+ searchterm + "&sort_fields=" + sortFieldsJson);
			        }
			         

				
			  public virtual async Task<string> getById_allinfo_Project(string id)
			  {
					return await ApiClient.Get_ApiValues(getHttpClient(), "api/Project/getById_allinfo_Project?id="+id);
					 
			  }






                    
                     

				}


			}
