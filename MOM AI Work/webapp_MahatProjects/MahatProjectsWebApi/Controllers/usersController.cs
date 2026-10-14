namespace MahatProjectsWebApi.Controllers
			{
				using System;
				using System.Data;
				using System.Linq;
                using Newtonsoft.Json.Linq;
				using Microsoft.AspNetCore.Mvc;
				using System.Collections.Generic;
				using Microsoft.Extensions.Options;
				using Microsoft.Extensions.Logging;
				using Microsoft.AspNetCore.Authorization;
				using Microsoft.Extensions.Configuration;
				using System.IdentityModel.Tokens.Jwt;
				using System.Security.Claims;
				using System.Text;
				using Microsoft.IdentityModel.Tokens;
				using MahatProjects.Models;
				using MahatProjects.DAL;
				using FluentValidation.Results;

				using Microsoft.AspNetCore.Hosting;
				using System.IO;
				using System.Net.Http.Headers;
                using Microsoft.AspNetCore.Http;
                using Newtonsoft.Json;
				[Route("api/[controller]/[action]")]
				//This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:22
				public class usersController : BaseController
				{
				    public usersController(IHttpContextAccessor httpContextAccessor,IOptions<ConnectionSettings> connectionSettings, ILoggerFactory loggerFactory, IConfiguration configuration,IWebHostEnvironment hostingEnvironment)
				    {
					     _configuration = configuration;
					     _logger = loggerFactory.CreateLogger<usersController>();
					     _connectionSettings = connectionSettings;
					     objusersDAL = new usersDAL(_connectionSettings.Value.ConnectionString);
                         obj_External_System_DAL =new External_System_DAL(_connectionSettings.Value.ConnectionString);
                         objExternalSystemUtitlity = new ExternalSystemUtility(_connectionSettings, _configuration);
					     hostingEnv = hostingEnvironment;

                            var authHeader = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

                            if (authHeader.StartsWith("Bearer "))
                            {
                                   var token = authHeader.Substring("Bearer ".Length);
                                   string usersid = obj_External_System_DAL.get_users_info_by_token(token);
                                   string tenantid = string.IsNullOrEmpty(httpContextAccessor.HttpContext.Request.Query["tenantid"]) ? "0": httpContextAccessor.HttpContext.Request.Query["tenantid"].ToString();
                                   pvar_tenantid = usersid + "|" + tenantid;//usersid+tenantid
                                   pvar_usersid=usersid;

                            }
				    }
				private usersDAL objusersDAL;
                private External_System_DAL obj_External_System_DAL;
				private IOptions<ConnectionSettings> _connectionSettings;
				private ILogger _logger;
				private IConfiguration _configuration;
				private IWebHostEnvironment hostingEnv;
                private ExternalSystemUtility objExternalSystemUtitlity;
                private string pvar_tenantid="|";
                private string pvar_usersid="";

			    [HttpGet()]
			  [ActionName("getById_alloweddevices")]
			  public virtual System.Data.DataTable getById_alloweddevices(string id)
			  {
					DataTable dtusers_alloweddevices = new DataTable();
					try
					{
						dtusers_alloweddevices = objusersDAL.getById_alloweddevices(id);
					}
					catch (Exception ex)
					{
                         _logger.LogError(ex,"An exception occurred in - getById_alloweddevices, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
    
					}
					return dtusers_alloweddevices;


			  }

[AllowAnonymous()]
            [HttpPost()]
            [ActionName("Register_Profile")]
            public virtual IActionResult Register_Profile([FromBody]usersModel model)
            { 
              string message = "";
                response_model obj_response_model = new response_model();
                access_logsdetailsModel obj_access_logsdetailsModel = new access_logsdetailsModel();
                   obj_access_logsdetailsModel.action_method_name="Register_Profile";
            try{

            if (ModelState.IsValid)
            {

            	usersModelValidator validator = new usersModelValidator();
            	ValidationResult results = validator.Validate(model);
            	if (!results.IsValid)
            	{
            		var errorCollection = string.Join(" | ", results.Errors.Select(e => e.ErrorMessage.Replace("{propertyName}",e.PropertyName)));
             		message = ("Validation Error : " + errorCollection);


            	}else{

                                   var authHeader = HttpContext.Request.Headers["Authorization"][0];
                                if (authHeader.StartsWith("Bearer "))
                                {
                                    var token = authHeader.Substring("Bearer ".Length);
                 String[] userdetails=obj_External_System_DAL.get_users_by_token(token);
                if (userdetails != null && userdetails.Length > 0 && userdetails[0] != null)
                {
                    model.created_by_id = Convert.ToInt64 (userdetails[0].ToString());
                    obj_access_logsdetailsModel.access_logsid = Convert.ToInt64(userdetails[1].ToString());
                }
                else
                {

                    obj_response_model.message = "Invalid Token";
                    return BadRequest(obj_response_model);
                }
                

                                
            		                
                                     obj_response_model = objusersDAL.Register_Profile(model);
                               }
                                else{
                                  message = "Invalid Token";
                                 }

            	}


            }
            else
            {
            	var errorCollection = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            	message = errorCollection.ToString();

            	_logger.LogError("usersModel - Register_Profile , Validation Error :" + message);
            	message = ("Validation Error : " + message);
            }






            }catch(Exception ex){
               obj_response_model.message= ex.Message;
               _logger.LogError(ex,"An exception occurred in - Register_Profile, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
             
            }
            if(obj_access_logsdetailsModel.access_logsid !=null)
            {
                 obj_access_logsdetailsModel.api_response=message.Replace("\"",""); 
                 obj_External_System_DAL.create_access_logs_details(obj_access_logsdetailsModel);
            }

            if(obj_response_model.message.Replace("\"","").Contains("201.1"))
					return Ok(obj_response_model);
					else if(obj_response_model.message.Replace("\"","")=="401.1")
					return Unauthorized(obj_response_model);
					else{
					  return BadRequest(obj_response_model);
                    }


             }
[HttpGet()]
			  [ActionName("getById_users")]
			  public virtual usersModel getById_users(string id,string loginUserID="")
			  { 
				    usersModel objusers = new usersModel();
					try
					{
						  objusers = objusersDAL.getById_users(id);
					}
					catch (Exception ex)
					{
                         _logger.LogError(ex,"An exception occurred in - getById_users, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
            
					}
					return objusers;

			  }
			  [HttpPost()]
			  [ActionName("Update_Profile")]
			  public virtual IActionResult Update_Profile([FromBody]usersModel model)
			  { 
				    string message = "";
                   access_logsdetailsModel obj_access_logsdetailsModel = new access_logsdetailsModel();
                   obj_access_logsdetailsModel.action_method_name="Update_Profile";

					try{

					if (ModelState.IsValid)
					{

						usersModelValidator validator = new usersModelValidator();
						ValidationResult results = validator.Validate(model);
						if (!results.IsValid)
						{
							var errorCollection = string.Join(" | ", results.Errors.Select(e => e.ErrorMessage.Replace("{propertyName}",e.PropertyName)));
							message = errorCollection.ToString();
							//return BadRequest("Validation Error : " + message);

						}else{
                            var authHeader = HttpContext.Request.Headers["Authorization"][0];
	                        if (authHeader.StartsWith("Bearer "))
	                        {
		                       
                                var token = authHeader.Substring("Bearer ".Length);
		                         String[] userdetails=obj_External_System_DAL.get_users_by_token(token);
                                    if(userdetails !=null && userdetails.Length >0 && userdetails[0] !=null)
                                    {
                                     model.updated_by_id=Convert.ToInt64(userdetails[0].ToString());
                                obj_access_logsdetailsModel.access_logsid=Convert.ToInt64(userdetails[1].ToString());
                                 }else{
                                        
                                          return BadRequest("Invalid Token");
                                    }
                                
            
		       
                                	
							    message = objusersDAL.Update_Profile(model);	
                            }
                            else{
                                message = "Invalid Token";
                                 
                            }
							
						}


					}
					else
					{
						var errorCollection = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
						message = errorCollection.ToString();

						_logger.LogError("usersModel - Update_Profile, Validation Error :" + message);
					
						//return BadRequest("Validation Error : " + message);
					}






					}catch(Exception ex){
                        
						message=ex.Message;
					    _logger.LogError(ex,"An exception occurred in - Update_Profile, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
           
					}

                    if(obj_access_logsdetailsModel.access_logsid !=null)
                    {
                            obj_access_logsdetailsModel.api_response=message.Replace("\"",""); 
                            obj_External_System_DAL.create_access_logs_details(obj_access_logsdetailsModel);
                    }

					if(message.Replace("\"","")=="201.1")
					return Ok(message);
					else if(message.Replace("\"","")=="401.1")
					return Unauthorized(message);
					else
					return BadRequest(message);

					


			   }
[HttpGet()]
            public virtual string Suspend_Profile(string id,string loginUserID="")
			{
					string message ="";
                    access_logsdetailsModel obj_access_logsdetailsModel = new access_logsdetailsModel();
                   obj_access_logsdetailsModel.action_method_name="Suspend_Profile";

					try{
						
						  var authHeader = HttpContext.Request.Headers["Authorization"][0];
	                        if (authHeader.StartsWith("Bearer "))
	                        {
		                        
		                      
		                      
		                        var token = authHeader.Substring("Bearer ".Length);
		                         
		                         String[] userdetails=obj_External_System_DAL.get_users_by_token(token);
                                 if(userdetails !=null && userdetails.Length >0 && userdetails[0] !=null)
                                    {
		                          loginUserID=userdetails[0].ToString();
                                  obj_access_logsdetailsModel.access_logsid=Convert.ToInt64(userdetails[1].ToString());
                                  }
                                  else{
                                        
                                          return "Invalid Token";
                                    }
                                 
                                
            
                        	 message = objusersDAL.Suspend_Profile(id,loginUserID);
						    }
	                        else{
		                        message = "Invalid Token";
		                       
	                        }
					 

					}catch(Exception ex){
						message=ex.Message;
                         _logger.LogError(ex,"An exception occurred in - Suspend_Profile, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
                         
					}
                    if(obj_access_logsdetailsModel.access_logsid !=null)
                    {
                         obj_access_logsdetailsModel.api_response=message.Replace("\"",""); 
                         obj_External_System_DAL.create_access_logs_details(obj_access_logsdetailsModel);
                    }
                 
					return message;

			}

            [AllowAnonymous()]
		[HttpPost()]
		[ActionName("get_decryptedPassword")]
		public virtual string get_decryptedPassword([FromBody] userloginModel model)
		{
			string decryptedPassword = objusersDAL.get_decryptedPassword(model.username);

			return decryptedPassword;
		}
			[AllowAnonymous()]
[HttpGet()]
[ActionName("get_roleAuthorizations")]
public virtual System.Data.DataTable get_roleAuthorizations(string viewactionroles)
{
	 
	DataTable dtusers = new DataTable();
	try
	{
		dtusers = objusersDAL.get_roleAuthorizations(viewactionroles);
	}
	catch (Exception ex)
	{
         _logger.LogError(ex,"An exception occurred in - get_roleAuthorizations, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
               
	}
	return dtusers;

}
[AllowAnonymous()]
[HttpGet()]
[ActionName("get_project_Menu")]
public virtual System.Data.DataTable get_project_Menu(string viewactionroles,string SubSystem)
{
	 
	DataTable dtusers = new DataTable();
	try
	{
		dtusers = objusersDAL.get_project_Menu(viewactionroles,SubSystem);
	}
	catch (Exception ex)
	{
         _logger.LogError(ex,"An exception occurred in - get_project_Menu, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
  
	}
	return dtusers;

}
			[AllowAnonymous()]
        [HttpGet()]
		[ActionName("get_roles")]
		public virtual System.Data.DataTable get_roles(string roles,string parentname)
		{
			    DataTable dt= objusersDAL.get_roles(roles,parentname);
				return dt;
		}
			
			
			[AllowAnonymous()]
		[HttpPost()]
		[ActionName("CheckAuthentication")]
		public virtual System.Data.DataTable CheckAuthentication([FromBody]userloginModel model)
		{
			    DataTable dt= objusersDAL.CheckAuthentication(model);
				if (dt.Rows.Count > 0)
				{
				dt.Columns.Add("token");


     			 var userClaims = new Dictionary<string, object>
                {
                    { "username", model.username },
                    { "usersid",  dt.Rows[0]["id"].ToString() },
                    { "tenantid",  dt.Rows[0]["tenantid"].ToString() }
                };
                
                var claims = new List<Claim>();
                foreach (var claim in userClaims)
                {
                    claims.Add(new Claim(claim.Key, claim.Value.ToString()));
                }


				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
				var token = new JwtSecurityToken(
				issuer: _configuration["ValidIssuer"],
				audience: "yourdomain.com",
				claims: claims,
				expires: DateTime.Now.AddMinutes(3000),
				signingCredentials: creds);
				string token_ = new JwtSecurityTokenHandler().WriteToken(token);
				dt.Rows[0]["token"] = token_;


                         
                        access_logsModel objExternalapilogsModel = new access_logsModel();
                        objExternalapilogsModel.logged_date = DateTime.Now;
                        objExternalapilogsModel.expiry_date = DateTime.Now.AddMinutes(60);
                        objExternalapilogsModel.external_entity_name = "";
                        objExternalapilogsModel.user_token = token_;
                        objExternalapilogsModel.users_id =dt.Rows[0]["id"].ToString();
                        objExternalapilogsModel.latlan = "";
                        objExternalapilogsModel.clientipaddress = "";
                        objExternalapilogsModel.devicename = "";
                        objExternalapilogsModel.browsername = "";
                        objExternalapilogsModel.external_users_id = "";
                        objExternalapilogsModel.created_by_id = Convert.ToInt64(dt.Rows[0]["id"].ToString());
                        obj_External_System_DAL.Create_access_logs(objExternalapilogsModel);

                        ExternalSystemUtility objExternalSystemUtitlity = new ExternalSystemUtility(_connectionSettings, _configuration);
				        objExternalSystemUtitlity.Insert_external_tokens(dt.Rows[0]["id"].ToString(),model);

                }
				return dt
				;
		}
        [HttpGet()]
		[ActionName("get_Dashboard_Items")]
		public virtual System.Data.DataTable get_Dashboard_Items(string viewactionroles)
		{
			DataTable dtusers = new DataTable();
			try
			{
				dtusers = objusersDAL.get_Dashboard_Items(viewactionroles);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An exception occurred in - get_Dashboard_Items, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" : ex.Message));

			}
			return dtusers;

		}
                [AllowAnonymous()]
		        [HttpPost()]
		        [ActionName("CheckOAuthAuthentication")]
		        public virtual System.Data.DataTable CheckOAuthAuthentication([FromBody] userClaims model)
		        {
					DataTable dtOauth = new DataTable();
					try
					{
						dtOauth = objusersDAL.CheckOAuthAuthentication(model);
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "An exception occurred in - List_of_User_Profiles, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" : ex.Message));

					}
			        
			        return dtOauth;
		        }
[HttpGet()]
			
			[ActionName("List_of_User_Profiles")]
			public virtual System.Data.DataTable List_of_User_Profiles(string tenantid
)
			{
					 
				  	DataTable dtusers = new DataTable();
					try
					{
                        tenantid=pvar_tenantid;
						dtusers = objusersDAL.List_of_User_Profiles( tenantid
);
					}
					catch (Exception ex)
					{
                         _logger.LogError(ex,"An exception occurred in - List_of_User_Profiles, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
                       
					}
					return dtusers;

			   }
			   
[HttpGet()]
			
			[ActionName("get_all_users")]
			public virtual System.Data.DataTable get_all_users(string tenantid,string loginUserID="")
			{
					 
				  	DataTable dtusers = new DataTable();
					try
					{
                        tenantid=pvar_tenantid;
						dtusers = objusersDAL.get_all_users(tenantid);
					}
					catch (Exception ex)
					{
                        _logger.LogError(ex,"An exception occurred in - get_all_users, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
             
					   
					}
					return dtusers;

			   }
 
			  [HttpPost()]
			  [ActionName("ChangePassword")]
			  public virtual string ChangePassword([FromBody]usersChangePasswordModel model)
			  { 
				  string message = "";
					try{
						
						  message = objusersDAL.ChangePassword(model);	
						 

					}catch(Exception ex){
						message=ex.Message;
                          _logger.LogError(ex,"An exception occurred in - ChangePassword, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
             
					}
					
					return message;

			   }
[HttpGet()]
			  [ActionName("getById_allinfo_users")]
			  public virtual System.Data.DataTable getById_allinfo_users(string id)
			  { 
				    DataTable dtusers = new DataTable();
					try
					{
						  dtusers = objusersDAL.getById_allinfo_users(id);
					}
					catch (Exception ex)
					{
                         _logger.LogError(ex,"An exception occurred in - getById_allinfo_users, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
   
					}
					return dtusers;

			  }






				}


			}
