namespace MahatProjectsWebApi.Controllers
                {
                    using System;
                    using System.Data;
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
                    using System.Reflection;
                    using System.Collections;
                    using System.Linq;

                            public class ExternalSystemUtility
                            {
                                public ExternalSystemUtility(IOptions<ConnectionSettings> connectionSettings,IConfiguration configuration)
                                {
                                    _configuration = configuration;
                                    _connectionSettings = connectionSettings;
                                    obj_External_System_DAL = new External_System_DAL(_connectionSettings.Value.ConnectionString)
                                    ;
                                }
                                private External_System_DAL obj_External_System_DAL;
                                private IOptions<ConnectionSettings> _connectionSettings;
                                private IConfiguration _configuration;
         
                                public virtual bool Insert_external_tokens(string usersid,userloginModel model,string identityTokenIfAny="")
                                {

                                    try
                                    {
                                        return false;
                                    }
                                    catch
                                    {
                                        return false;
                                    }
            

                                    return true;
                                }

                                public virtual string Get_post_response<T>(string api_url, string created_by_id,string authorizationcheckby,string externalSystem, T model)
                                {

                                    string message = "";
                                    //Vessel_System
                                    try
                                    {
                 
                                        var client_R = new RestSharp.RestClient(api_url);
                                        var request = new RestSharp.RestRequest(RestSharp.Method.POST);

                                        string user_token = "";
                                        string external_user = "";
                                        External_System_DAL objExternal_System_DAL = new External_System_DAL(_connectionSettings.Value.ConnectionString);
                                        string received_user_token = "";
                                        if (objExternal_System_DAL.get_active_token(created_by_id, externalSystem, ref user_token, ref external_user))
                                        {
                     


                                            Type type = model.GetType();
                                            PropertyInfo property = type.GetProperty(authorizationcheckby);
                                            property.SetValue(model, Convert.ToInt64(external_user), null);

                                            received_user_token = user_token;
                                            request.AddHeader("Authorization", string.Format("Bearer {0}", received_user_token));
                                        }
                                        var model_message = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                                        request.AddHeader("Content-Type", "application/json");
                                        request.AddParameter("application/json", model_message, RestSharp.ParameterType.RequestBody);
                                        RestSharp.IRestResponse response_ = client_R.Execute(request);
                                        message = response_.Content.ToString();


                                        if (_configuration.GetSection("api_access_get_logs").Value.ToString() == "YES")
                                        {

                                            access_logsModel objExternalapilogsModel = new access_logsModel();
                                            objExternalapilogsModel.logged_date = DateTime.Now;
                                            objExternalapilogsModel.expiry_date = DateTime.Now.AddMinutes(60);
                                            objExternalapilogsModel.user_token = received_user_token;
                                            objExternalapilogsModel.users_id = created_by_id;
                                            objExternalapilogsModel.request_type = "POST";
                                            objExternalapilogsModel.api_url = api_url;
                                            objExternalapilogsModel.request_json = model_message;
                                            objExternalapilogsModel.response_json = message;
                                            objExternalapilogsModel.created_by_id = Convert.ToInt64(created_by_id);
                                            obj_External_System_DAL.Create_api_access_logs(objExternalapilogsModel);

                                        }

                

                                    }
                                    catch(Exception ex)
                                    {
                                        message="External Exception | " +ex.Message;
                                    }


                                    return message;
                                }

                                public virtual string Get_response(string api_url, string created_by_id, string externalSystem, System.Collections.Generic.IDictionary<string,string> queryParameters)
                                {

                                    string message = "";
                                    //Vessel_System
                                    try
                                    {
                                         
                                        var client_R = new RestSharp.RestClient(api_url);
                                        var request = new RestSharp.RestRequest(RestSharp.Method.GET);

                                        string user_token = "";
                                        string external_user = "";
                                        string received_user_token = "";
                                        External_System_DAL objExternal_System_DAL = new External_System_DAL(_connectionSettings.Value.ConnectionString);
                                        if (objExternal_System_DAL.get_active_token(created_by_id, externalSystem, ref user_token, ref external_user))
                                        {
                                            received_user_token = user_token;
                                            request.AddHeader("Authorization", string.Format("Bearer {0}", received_user_token));
                                            request.AddHeader("apikey", external_user);
                                            request.AddHeader("x-api-key", external_user);
                                        }
                 


                
                                        foreach (KeyValuePair<string, string> kvp in queryParameters)
                                        {
                                            request.AddQueryParameter(kvp.Key.ToString(), kvp.Value != null ? kvp.Value.ToString() : "");
                                        }

                                        var entries = queryParameters.Select(d =>
                                        string.Format("\"{0}\": \"{1}\"", d.Key, string.Join(",", d.Value)));
                                        string RequestJSON= "{" + string.Join(",", entries) + "}";

                                        request.AddHeader("Content-Type", "application/json");
                                        RestSharp.IRestResponse response_ = client_R.Execute(request);
                                        message = response_.Content.ToString();
                                        if (_configuration.GetSection("api_access_get_logs").Value.ToString() == "YES")
                                        {

                                            access_logsModel objExternalapilogsModel = new access_logsModel();
                                            objExternalapilogsModel.logged_date = DateTime.Now;
                                            objExternalapilogsModel.expiry_date = DateTime.Now.AddMinutes(60);
                                            objExternalapilogsModel.user_token = received_user_token;
                                            objExternalapilogsModel.users_id = created_by_id;
                                            objExternalapilogsModel.request_type = "GET";
                                            objExternalapilogsModel.api_url = api_url;
                                            objExternalapilogsModel.request_json = RequestJSON;
                                            objExternalapilogsModel.response_json = message;
                                            objExternalapilogsModel.created_by_id = Convert.ToInt64(created_by_id);
                                            obj_External_System_DAL.Create_api_access_logs(objExternalapilogsModel);

                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        message = "External Exception | " + ex.Message;
                                    }


                                    return message;
                                }

                            }
                     
                }

                
