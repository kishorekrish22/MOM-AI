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
				//This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:31:56
				public class ProjectController : BaseController
				{
				    public ProjectController(IHttpContextAccessor httpContextAccessor,IOptions<ConnectionSettings> connectionSettings, ILoggerFactory loggerFactory, IConfiguration configuration,IWebHostEnvironment hostingEnvironment)
				    {
					     _configuration = configuration;
					     _logger = loggerFactory.CreateLogger<ProjectController>();
					     _connectionSettings = connectionSettings;
					     objProjectDAL = new ProjectDAL(_connectionSettings.Value.ConnectionString);
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
				private ProjectDAL objProjectDAL;
                private External_System_DAL obj_External_System_DAL;
				private IOptions<ConnectionSettings> _connectionSettings;
				private ILogger _logger;
				private IConfiguration _configuration;
				private IWebHostEnvironment hostingEnv;
                private ExternalSystemUtility objExternalSystemUtitlity;
                private string pvar_tenantid="|";
                private string pvar_usersid="";

			    
            [HttpPost()]
            [ActionName("Add_Project")]
            public virtual IActionResult Add_Project([FromBody]ProjectModel model)
            { 
              string message = "";
                response_model obj_response_model = new response_model();
                access_logsdetailsModel obj_access_logsdetailsModel = new access_logsdetailsModel();
                   obj_access_logsdetailsModel.action_method_name="Add_Project";
            try{

            if (ModelState.IsValid)
            {

            	ProjectModelValidator validator = new ProjectModelValidator();
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
                

                                
            		                
                                     obj_response_model = objProjectDAL.Add_Project(model);
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

            	_logger.LogError("ProjectModel - Add_Project , Validation Error :" + message);
            	message = ("Validation Error : " + message);
            }






            }catch(Exception ex){
               obj_response_model.message= ex.Message;
               _logger.LogError(ex,"An exception occurred in - Add_Project, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
             
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
			  [ActionName("getById_Project")]
			  public virtual ProjectModel getById_Project(string id,string loginUserID="")
			  { 
				    ProjectModel objProject = new ProjectModel();
					try
					{
						  objProject = objProjectDAL.getById_Project(id);
					}
					catch (Exception ex)
					{
                         _logger.LogError(ex,"An exception occurred in - getById_Project, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
            
					}
					return objProject;

			  }
			  [HttpPost()]
			  [ActionName("Update_Project")]
			  public virtual IActionResult Update_Project([FromBody]ProjectModel model)
			  { 
				    string message = "";
                   access_logsdetailsModel obj_access_logsdetailsModel = new access_logsdetailsModel();
                   obj_access_logsdetailsModel.action_method_name="Update_Project";

					try{

					if (ModelState.IsValid)
					{

						ProjectModelValidator validator = new ProjectModelValidator();
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
                                
            
		       
                                	
							    message = objProjectDAL.Update_Project(model);	
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

						_logger.LogError("ProjectModel - Update_Project, Validation Error :" + message);
					
						//return BadRequest("Validation Error : " + message);
					}






					}catch(Exception ex){
                        
						message=ex.Message;
					    _logger.LogError(ex,"An exception occurred in - Update_Project, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
           
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
            public virtual string Remove_Project(string id,string loginUserID="")
			{
					string message ="";
                    access_logsdetailsModel obj_access_logsdetailsModel = new access_logsdetailsModel();
                   obj_access_logsdetailsModel.action_method_name="Remove_Project";

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
                                 
                                
            
                        	 message = objProjectDAL.Remove_Project(id,loginUserID);
						    }
	                        else{
		                        message = "Invalid Token";
		                       
	                        }
					 

					}catch(Exception ex){
						message=ex.Message;
                         _logger.LogError(ex,"An exception occurred in - Remove_Project, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
                         
					}
                    if(obj_access_logsdetailsModel.access_logsid !=null)
                    {
                         obj_access_logsdetailsModel.api_response=message.Replace("\"",""); 
                         obj_External_System_DAL.create_access_logs_details(obj_access_logsdetailsModel);
                    }
                 
					return message;

			}
[HttpGet()]
			
			[ActionName("Project_List")]
			public virtual JObject Project_List(string loginUserID="",int? pagesize=100 , int? pagenumber=0,string searchterm="",string  sort_fields = "")
			{
					 
				  	JObject jsonProject = null;
					try
					{
                        
						jsonProject = objProjectDAL.Project_List(pagesize,pagenumber,searchterm, sort_fields);
					}
					catch (Exception ex)
					{
                             _logger.LogError(ex,"An exception occurred in - Project_List, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
                        
					}
					return jsonProject;

			   }
			   
[HttpGet()]
			
			[ActionName("get_all_Project")]
			public virtual System.Data.DataTable get_all_Project(string tenantid,string searchterm="",string pagesize = "50", string pagenumber = "1",string loginUserID="")
			{
					  int? pageSize = null; 
                     if (!string.IsNullOrEmpty(pagesize))
                     {                 
                     pageSize = int.Parse(pagesize);
                     }
                     int? pageNumber = null;
                     if (!string.IsNullOrEmpty(pagenumber))
                     {
 
                     pageNumber = int.Parse(pagenumber);
                     pageNumber--;//0 based index for page numbers
                     }
				  	DataTable dtProject = new DataTable();
					try
					{
                        tenantid=pvar_tenantid;
						dtProject = objProjectDAL.get_all_Project(tenantid,searchterm,pageSize,pageNumber);
					}
					catch (Exception ex)
					{
                          _logger.LogError(ex,"An exception occurred in - get_all_Project, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
             
					    
					}
					return dtProject;

			   }
[HttpGet()]
			  [ActionName("getById_allinfo_Project")]
			  public virtual System.Data.DataTable getById_allinfo_Project(string id)
			  { 
				    DataTable dtProject = new DataTable();
					try
					{
						  dtProject = objProjectDAL.getById_allinfo_Project(id);
					}
					catch (Exception ex)
					{
                         _logger.LogError(ex,"An exception occurred in - getById_allinfo_Project, Error Message : " + (ex.StackTrace != null ? $", Stack Trace: {ex.StackTrace.ToString()}" :ex.Message));
   
					}
					return dtProject;

			  }






				}


			}
