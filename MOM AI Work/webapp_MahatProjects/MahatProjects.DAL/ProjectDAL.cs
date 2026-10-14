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

			    //This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:31:56
			    public class ProjectDAL
			    {
					public virtual string db_connectionstring{get;set;}
					
			 	    public ProjectDAL(string connectionString)
				    {
						
					    db_connectionstring=connectionString;
				    }
				  
			        
              public virtual response_model Add_Project(ProjectModel model)
			  { 
				  response_model obj_response_model=new response_model();
						 
					try{
							 
                            using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
					        {
						        npsql.Open();
						        using (var dbCommand = new NpgsqlCommand("\"Add_Project\"", npsql))
						        {
                                        dbCommand.CommandType = CommandType.StoredProcedure;
						            	
								        					dbCommand.Parameters.AddWithValue("pvar_id",NpgsqlDbType.Bigint,(object)model.id??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_project_name",NpgsqlDbType.Varchar,(object)model.project_name??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_description",NpgsqlDbType.Varchar,(object)model.description??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_handled_by",NpgsqlDbType.Varchar,(object)model.handled_by??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_interaction_date",NpgsqlDbType.Date,(object)model.interaction_date??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_video_upload",NpgsqlDbType.Varchar,(object)model.video_upload??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_transcript",NpgsqlDbType.Varchar,(object)model.transcript??DBNull.Value);
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
public virtual ProjectModel getById_Project(string id)
									 {
										DataTable dataTable = new DataTable();
										DataSet dataSet = new DataSet();
										try{
												 
												using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
												{
													npsql.Open();
													using (var dbCommand = new NpgsqlCommand("\"getById_sp_Project\"", npsql))
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
											return ModelConverter.ConvertDataRowToModel<ProjectModel>(row);
										}
										else
										{
											return null;
										}
									 }
			 public virtual string  Update_Project(ProjectModel model)
			 { 
				 String ResponseMessage="";
					try{
						 	 
							using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
							{
								npsql.Open();
								using (var dbCommand = new NpgsqlCommand("\"Update_Project\"", npsql))
								{
										dbCommand.CommandType = CommandType.StoredProcedure;
															dbCommand.Parameters.AddWithValue("pvar_id",NpgsqlDbType.Bigint,(object)model.id??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_project_name",NpgsqlDbType.Varchar,(object)model.project_name??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_description",NpgsqlDbType.Varchar,(object)model.description??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_handled_by",NpgsqlDbType.Varchar,(object)model.handled_by??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_interaction_date",NpgsqlDbType.Date,(object)model.interaction_date??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_video_upload",NpgsqlDbType.Varchar,(object)model.video_upload??DBNull.Value);

dbCommand.Parameters.AddWithValue("pvar_transcript",NpgsqlDbType.Varchar,(object)model.transcript??DBNull.Value);
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
public virtual string  Remove_Project(string id,string loginUserID)
			  { 
				  String ResponseMessage="";
					try{ 
							using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
							{
								npsql.Open();
								using (var dbCommand = new NpgsqlCommand("\"Remove_Project\"", npsql))
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
public virtual JObject Project_List( int? pagesize=1000 , int? pagenumber=0,string searchterm="",string  sort_fields="")
			  { 
				  object dalResponse = null;
			
					try{
 
							using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
							{
								
								using (var dbCommand = new NpgsqlCommand("\"Project_List\"", npsql))
								{
									dbCommand.CommandType = CommandType.StoredProcedure;
									
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
			   
			 
public virtual System.Data.DataTable get_all_Project(string tenantid,string searchterm="", int? pagesize=1000, int? pagenumber=0)
			  { 

				    DataTable dataTable = new DataTable();
					DataSet dataSet = new DataSet();

					try{
 
							using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
							{
								npsql.Open();
								using (var dbCommand = new NpgsqlCommand("\"get_all_Project\"", npsql))
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
public virtual System.Data.DataTable getById_allinfo_Project(string id)
			 {
				DataSet dataSet=new DataSet();
				DataTable dataTable = new DataTable();
				try{
					     
						using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
						{
							npsql.Open();
							using (var dbCommand = new NpgsqlCommand("\"getById_sp_all_Project\"", npsql))
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
