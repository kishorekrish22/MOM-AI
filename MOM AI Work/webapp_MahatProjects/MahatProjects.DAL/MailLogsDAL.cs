namespace MahatProjects.DAL{
			    using System;
			    using System.Text;
			    using System.Data;
			    using System.Data.Common;
			    using MahatProjects.Models;
			    using EncrypDecrypt;
			    using Newtonsoft.Json;
				using Newtonsoft.Json.Linq;
                using Npgsql;
				using NpgsqlTypes;
				using System.Text.RegularExpressions;

			    //This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:27
			    public class MailLogsDAL
			    {
					public virtual string db_connectionstring{get;set;}
					public virtual string str_approvalurl { get; set; }
			 	    public MailLogsDAL(string connectionString,string approvalurl)
				    {
						 str_approvalurl = approvalurl;
					    db_connectionstring=connectionString;
				    }
				  
			        public virtual System.Data.DataTable MailSender(string mailfor,string entityid,string created_by_id){
										   DataTable dataTable=new DataTable();
											DataSet dataSet=new DataSet();
											try
											{
							  
                            
													 using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
													{
														npsql.Open();
														using (var dbCommand = new NpgsqlCommand("\"MailSender\"", npsql))
														{
															dbCommand.CommandType = CommandType.StoredProcedure;
															dbCommand.Parameters.AddWithValue("pvar_mailfor",(object)mailfor??DBNull.Value);
															dbCommand.Parameters.AddWithValue("pvar_entityid",(object)entityid??DBNull.Value);
															dbCommand.Parameters.AddWithValue("pvar_created_by_id",(object)created_by_id??DBNull.Value);
															using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(dbCommand))
															{
																dataSet.Reset();
																dataAdapter.Fill(dataSet);
																dataTable = dataSet.Tables[0];
																if (dbCommand.Connection.State != ConnectionState.Closed)
																{
																	dbCommand.Connection.Dispose();
																}
															}
														}
														npsql.Close();
													}

						
											}
											catch{
												throw;
											}
											return dataTable;	
										}
										 public virtual mailmodel Mailer(string entityname, string entityactionname, string entityid)
										{

											string alertcontent = "";
											string alertsubject = "";

											mailmodel objmailmodel = new mailmodel();
											DataTable dataTable = new DataTable();
											DataSet dataSet = new DataSet();
											try
											{
												using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
												{
													npsql.Open();
													using (var dbCommand = new NpgsqlCommand("\"Alert_Templates_List\"", npsql))
													{
														dbCommand.CommandType = CommandType.StoredProcedure;
														dbCommand.Parameters.AddWithValue("pvar_entityname", (object)entityname ?? DBNull.Value);
														dbCommand.Parameters.AddWithValue("pvar_entityaction", (object)entityactionname ?? DBNull.Value);
														dbCommand.Parameters.AddWithValue("pvar_alerttype", "Email");

														using (var reader = dbCommand.ExecuteReader())
														{
															if (reader.Read())
															{
																// Retrieve the values from the reader
																alertsubject = reader["alertsubject"].ToString(); 
																alertcontent = reader["alertcontent"].ToString(); 

															}
															else
															{
																// Handle the case where no data is returned from the stored procedure
															}
														}
													}
													npsql.Close();
												}

												// Define a regular expression pattern to match text inside curly braces
												string pattern = @"\{([^}]*)\}";

												// Use Regex.Matches to find all matches in the input string
												MatchCollection matches = Regex.Matches(alertcontent, pattern);

												// Create an array to store the matched values
												string[] extractedValues = new string[matches.Count];

												// Extract and store the matched values in the array
												for (int i = 0; i < matches.Count; i++)
												{
													extractedValues[i] = matches[i].Groups[1].Value;
												}




												using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
												{
													npsql.Open();
													using (var dbCommand = new NpgsqlCommand("\"getById_sp_all_"+entityname+"\"", npsql))
													{
														dbCommand.CommandType = CommandType.StoredProcedure;
														dbCommand.Parameters.AddWithValue("pvar_"+entityname.ToLower()+"id", (object)entityid ?? DBNull.Value);
														using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(dbCommand))
														{
															dataSet.Reset();
															dataAdapter.Fill(dataSet);
															dataTable = dataSet.Tables[0];


															if (dbCommand.Connection.State != ConnectionState.Closed)
															{
																dbCommand.Connection.Dispose();
															}
														}
													}
													npsql.Close();
												}

												if (entityactionname == "ReadyForReview")
												{
													alertcontent = alertcontent.Replace("~clickhere~","<a href='"+ str_approvalurl + "/" + entityname + "/detail?id=" + entityid +"'>Click Here</a>");
												}

												foreach (string value in extractedValues)
												{
													alertsubject = alertsubject.Replace("{" + value + "}", dataTable.Rows[0][value].ToString());
													alertcontent = alertcontent.Replace("{" + value + "}", dataTable.Rows[0][value].ToString());
												}
												objmailmodel.mailbody = alertcontent;
												objmailmodel.mailsubject = alertsubject;
												objmailmodel.mailto = dataTable.Rows[0]["authorized_users"].ToString();

											}
											catch
											{
												throw;
											}
											return objmailmodel;
										}
										 public virtual mailmodel WhatsApp(string entityname, string entityactionname, string entityid)
										{

											string alertcontent = "";
											string alertsubject = "";

											mailmodel objmailmodel = new mailmodel();
											DataTable dataTable = new DataTable();
											DataSet dataSet = new DataSet();
											try
											{
												using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
												{
													npsql.Open();
													using (var dbCommand = new NpgsqlCommand("\"Alert_Templates_List\"", npsql))
													{
														dbCommand.CommandType = CommandType.StoredProcedure;
														dbCommand.Parameters.AddWithValue("pvar_entityname", (object)entityname ?? DBNull.Value);
														dbCommand.Parameters.AddWithValue("pvar_entityaction", (object)entityactionname ?? DBNull.Value);
														dbCommand.Parameters.AddWithValue("pvar_alerttype", "WhatsApp");
														using (var reader = dbCommand.ExecuteReader())
														{
															if (reader.Read())
															{
																// Retrieve the values from the reader
																alertsubject = reader["alertsubject"].ToString(); 
																alertcontent = reader["alertcontent"].ToString(); 

															}
															else
															{
																// Handle the case where no data is returned from the stored procedure
															}
														}
													}
													npsql.Close();
												}

												// Define a regular expression pattern to match text inside curly braces
												string pattern = @"\{([^}]*)\}";

												// Use Regex.Matches to find all matches in the input string
												MatchCollection matches = Regex.Matches(alertcontent, pattern);

												// Create an array to store the matched values
												string[] extractedValues = new string[matches.Count];

												// Extract and store the matched values in the array
												for (int i = 0; i < matches.Count; i++)
												{
													extractedValues[i] = matches[i].Groups[1].Value;
												}




												using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
												{
													npsql.Open();
													using (var dbCommand = new NpgsqlCommand("\"getById_sp_all_"+entityname+"\"", npsql))
													{
														dbCommand.CommandType = CommandType.StoredProcedure;
														dbCommand.Parameters.AddWithValue("pvar_"+entityname.ToLower()+"id", (object)entityid ?? DBNull.Value);
														using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(dbCommand))
														{
															dataSet.Reset();
															dataAdapter.Fill(dataSet);
															dataTable = dataSet.Tables[0];


															if (dbCommand.Connection.State != ConnectionState.Closed)
															{
																dbCommand.Connection.Dispose();
															}
														}
													}
													npsql.Close();
												}

												if (entityactionname == "ReadyForReview")
												{
													alertcontent = alertcontent.Replace("~clickhere~"," Click Here " + str_approvalurl + "/" + entityname + "/detail?id=" + entityid);
												}

												foreach (string value in extractedValues)
												{
													alertsubject = alertsubject.Replace("{" + value + "}", dataTable.Rows[0][value].ToString());
													alertcontent = alertcontent.Replace("{" + value + "}", dataTable.Rows[0][value].ToString());
												}
												objmailmodel.mailbody = alertcontent;
												objmailmodel.mailsubject = alertsubject;
												objmailmodel.mailto = dataTable.Rows[0]["authorized_users_mobile"].ToString();

											}
											catch
											{
												throw;
											}
											return objmailmodel;
										}

								

              public virtual response_model Create_MailLog(MailLogsModel model)
			  { 
				  response_model obj_response_model=new response_model();
						 
					try{
							 
                            using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
					        {
						        npsql.Open();
						        using (var dbCommand = new NpgsqlCommand("\"Create_MailLog\"", npsql))
						        {
                                        dbCommand.CommandType = CommandType.StoredProcedure;
						            	
								        					dbCommand.Parameters.AddWithValue("pvar_id",NpgsqlDbType.Bigint,(object)model.id??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_entityname",NpgsqlDbType.Varchar,(object)model.entityname??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_entityid",NpgsqlDbType.Varchar,(object)model.entityid??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_mailfor",NpgsqlDbType.Varchar,(object)model.mailfor??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_mailsubject",NpgsqlDbType.Varchar,(object)model.mailsubject??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_mailto",NpgsqlDbType.Varchar,(object)model.mailto??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_mailbody",NpgsqlDbType.Varchar,(object)model.mailbody??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_issent",NpgsqlDbType.Boolean,(object)model.issent??DBNull.Value);
dbCommand.Parameters.AddWithValue("pvar_created_by_id",NpgsqlDbType.Bigint,(object)model.created_by_id??DBNull.Value);	
                                        NpgsqlParameter outParm = new NpgsqlParameter("pvar_returnMessage", NpgsqlDbType.Varchar)
                                        {
                                             Direction = ParameterDirection.Output
                                        };
                                        NpgsqlParameter outParmid = new NpgsqlParameter("pvar_returnind", NpgsqlDbType.Integer)
                                        {
                                             Direction = ParameterDirection.Output
                                        };
										dbCommand.Parameters.Add(outParmid);
                                        dbCommand.Parameters.Add(outParm);
										
                                        dbCommand.ExecuteNonQuery();
										obj_response_model.id =outParmid.Value.ToString();
										obj_response_model.message=outParm.Value.ToString();
										 
								        if (dbCommand.Connection.State != ConnectionState.Closed)
                    			        {
										         dbCommand.Connection.Dispose();
								        }

						        }
						        npsql.Close();
					        }
 

					}catch(Exception ex){
						obj_response_model.message=ex.Message;
					} 
					
					return obj_response_model;

			   }
public virtual MailLogsModel getById_MailLogs(string id)
									 {
										DataTable dataTable = new DataTable();
										DataSet dataSet = new DataSet();
										try{
												 
												using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
												{
													npsql.Open();
													using (var dbCommand = new NpgsqlCommand("\"getById_sp_MailLogs\"", npsql))
													{
														dbCommand.CommandType = CommandType.StoredProcedure;
														dbCommand.Parameters.AddWithValue("pvar_id",(object)id??DBNull.Value);
														using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(dbCommand))
														{
															dataSet.Reset();
															dataAdapter.Fill(dataSet);
															dataTable = dataSet.Tables[0];
															if (dbCommand.Connection.State != ConnectionState.Closed)
															{
																dbCommand.Connection.Dispose();
															}
														}
													}
													npsql.Close();
												}
					 
										}catch{
												throw;
										}
										if (dataTable.Rows.Count > 0)
										{
											DataRow row = dataTable.Rows[0];
											return ModelConverter.ConvertDataRowToModel<MailLogsModel>(row);
										}
										else
										{
											return null;
										}
									 }
			 public virtual string  Update_MailLog(MailLogsModel model)
			 { 
				 String ResponseMessage="";
					try{
						 	 
							using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
							{
								npsql.Open();
								using (var dbCommand = new NpgsqlCommand("\"Update_MailLog\"", npsql))
								{
										dbCommand.CommandType = CommandType.StoredProcedure;
															dbCommand.Parameters.AddWithValue("pvar_id",NpgsqlDbType.Bigint,(object)model.id??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_entityname",NpgsqlDbType.Varchar,(object)model.entityname??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_entityid",NpgsqlDbType.Varchar,(object)model.entityid??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_mailfor",NpgsqlDbType.Varchar,(object)model.mailfor??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_mailsubject",NpgsqlDbType.Varchar,(object)model.mailsubject??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_mailto",NpgsqlDbType.Varchar,(object)model.mailto??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_mailbody",NpgsqlDbType.Varchar,(object)model.mailbody??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_issent",NpgsqlDbType.Boolean,(object)model.issent??DBNull.Value);
dbCommand.Parameters.AddWithValue("pvar_updated_by_id",NpgsqlDbType.Bigint,model.updated_by_id);	
										NpgsqlParameter outParm = new NpgsqlParameter("@returnMessage", NpgsqlDbType.Varchar)
										{
											 Direction = ParameterDirection.Output
										};
										dbCommand.Parameters.Add(outParm);

										dbCommand.ExecuteNonQuery();
										ResponseMessage = outParm.Value.ToString();
										if (dbCommand.Connection.State != ConnectionState.Closed)
										{
												 dbCommand.Connection.Dispose();
										}

								}
								npsql.Close();
							}		 

					}catch(Exception ex){
						ResponseMessage=ex.Message;
					}
					
					return ResponseMessage;

			   }
public virtual string  Remove_MailLog(string id,string loginUserID)
			  { 
				  String ResponseMessage="";
					try{ 
							using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
							{
								npsql.Open();
								using (var dbCommand = new NpgsqlCommand("\"Remove_MailLog\"", npsql))
								{
										dbCommand.CommandType = CommandType.StoredProcedure;
										dbCommand.Parameters.AddWithValue("pvar_id",(object)id??DBNull.Value);
										dbCommand.Parameters.AddWithValue("pvar_updated_by_id",(object)loginUserID??DBNull.Value);
										NpgsqlParameter outParm = new NpgsqlParameter("@returnMessage", NpgsqlDbType.Varchar)
										{
											 Direction = ParameterDirection.Output
										};
										dbCommand.Parameters.Add(outParm);

										dbCommand.ExecuteNonQuery();
										ResponseMessage = outParm.Value.ToString();
										if (dbCommand.Connection.State != ConnectionState.Closed)
										{
												 dbCommand.Connection.Dispose();
										}

								}
								npsql.Close();
							}	 

					}catch(Exception ex){
						ResponseMessage=ex.Message;
					}
					
					return ResponseMessage;

			   }
public virtual JObject MailLogs_List(string entityname
,string mailfor
, int? pagesize=1000 , int? pagenumber=0,string searchterm="",string  sort_fields="")
			  { 
				  object dalResponse = null;
			
					try{
 
							using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
							{
								
								using (var dbCommand = new NpgsqlCommand("\"MailLogs_List\"", npsql))
								{
									dbCommand.CommandType = CommandType.StoredProcedure;
									dbCommand.Parameters.AddWithValue("pvar_entityname",(object)entityname??DBNull.Value);
dbCommand.Parameters.AddWithValue("pvar_mailfor",(object)mailfor??DBNull.Value);

									dbCommand.Parameters.AddWithValue("pvar_pagesize",(object)pagesize??DBNull.Value);
									dbCommand.Parameters.AddWithValue("pvar_pagenumber",(object)pagenumber??DBNull.Value);

									dbCommand.Parameters.AddWithValue("pvar_searchterm",(object)searchterm??DBNull.Value);
						if (sort_fields != null && sort_fields.Length > 2)
                            dbCommand.Parameters.AddWithValue("pvar_sort_fields", NpgsqlDbType.Json, sort_fields);
						else
                            dbCommand.Parameters.AddWithValue("pvar_sort_fields", NpgsqlDbType.Json, DBNull.Value);


									npsql.Open();
									dalResponse = dbCommand.ExecuteScalar();
									npsql.Close();
									
								}
								
							}

						 

					}catch{
						throw;
					}


					return JObject.Parse(dalResponse.ToString());	


					 

			   }
			   
			 
public virtual System.Data.DataTable get_all_MailLogs(string tenantid,string searchterm="", int? pagesize=1000, int? pagenumber=0)
			  { 

				    DataTable dataTable = new DataTable();
					DataSet dataSet = new DataSet();

					try{
 
							using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
							{
								npsql.Open();
								using (var dbCommand = new NpgsqlCommand("\"get_all_MailLogs\"", npsql))
								{
									dbCommand.CommandType = CommandType.StoredProcedure;
									dbCommand.Parameters.AddWithValue("pvar_tenantid",(object)tenantid??DBNull.Value);
									dbCommand.Parameters.AddWithValue("pvar_searchterm", (object)searchterm?? DBNull.Value);
                dbCommand.Parameters.AddWithValue("pvar_pagesize", (object)pagesize ?? DBNull.Value);
                dbCommand.Parameters.AddWithValue("pvar_pagenumber", (object)pagenumber ?? DBNull.Value);
									using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(dbCommand))
									{
										dataSet.Reset();
										dataAdapter.Fill(dataSet);
										dataTable = dataSet.Tables[0];
										if (dbCommand.Connection.State != ConnectionState.Closed)
										{
											dbCommand.Connection.Dispose();
										}
									}
								}
								npsql.Close();
							}
						

					}catch{
						throw;
					}
					return dataTable;	


					 

			   }
public virtual System.Data.DataTable getById_allinfo_MailLogs(string id)
			 {
				DataSet dataSet=new DataSet();
				DataTable dataTable = new DataTable();
				try{
					     
						using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
						{
							npsql.Open();
							using (var dbCommand = new NpgsqlCommand("\"getById_sp_all_MailLogs\"", npsql))
							{
								dbCommand.CommandType = CommandType.StoredProcedure;
								dbCommand.Parameters.AddWithValue("pvar_id",(object)id??DBNull.Value);
								using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(dbCommand))
								{
									dataSet.Reset();
									dataAdapter.Fill(dataSet);
									dataTable = dataSet.Tables[0];
									if (dbCommand.Connection.State != ConnectionState.Closed)
									{
										dbCommand.Connection.Dispose();
									}
								}
							}
							npsql.Close();
						}
					 
				}catch{
						throw;
				}
				return dataTable;
			 }
			  







			    }


			    }
