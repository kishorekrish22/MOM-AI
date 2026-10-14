namespace MahatProjects.DAL{
					using System;
					using System.Text;
					using System.Data;
					using System.Data.Common;
					using MahatProjects.Models;
					using EncrypDecrypt;
					using Npgsql;
					using NpgsqlTypes;
					//This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:17
					public class userlockoutDAL
					{
					 
					public userlockoutDAL(string connectionString)
					{
						db_connectionstring=connectionString;
					}
				 
					private string _connectionstring;
					public virtual string db_connectionstring
					{
						get
						{
							return _connectionstring;
						}
						set
						{
							_connectionstring=value;
						}
					}



					public virtual string  ins_userlockout(userlockoutModel model)
					{ 
						string ResponseMessage="";
						try{

							 		
							 		 using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
									{
										npsql.Open();
										using (var dbCommand = new NpgsqlCommand("\"ins_sp_userlockout\"", npsql))
										{
												dbCommand.CommandType = CommandType.StoredProcedure;
						            			dbCommand.Parameters.AddWithValue("pvar_username",model.username);
												dbCommand.Parameters.AddWithValue("pvar_latlan",(object)model.latlan??DBNull.Value);
												dbCommand.Parameters.AddWithValue("pvar_remoteipaddress",(object)model.remoteipaddress??DBNull.Value);
												dbCommand.Parameters.AddWithValue("pvar_clientipaddress",(object)model.clientipaddress??DBNull.Value);
												NpgsqlParameter outParm = new NpgsqlParameter("pvar_returnMessage", NpgsqlDbType.Varchar)
												{
													 Direction = ParameterDirection.Output
												};
												dbCommand.Parameters.Add(outParm);;

												dbCommand.ExecuteNonQuery();
												ResponseMessage = outParm.Value.ToString();
												if (dbCommand.Connection.State != ConnectionState.Closed)
                    							{
														 dbCommand.Connection.Dispose();
												}

										}
										npsql.Close();
									}
							 
 		 
								
						 }
						 catch(Exception ex)
						 {
								ResponseMessage=ex.Message;
								
						 }
							 

						return ResponseMessage;

					}



					public virtual string  verify_userlockout(userlockoutModel model)
					{ 

							string ResponseMessage="";
							
							try{
								    using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
									{
										npsql.Open();
										using (var dbCommand = new NpgsqlCommand("\"verify_sp_userlockout\"", npsql))
										{
												dbCommand.CommandType = CommandType.StoredProcedure;
						            			dbCommand.Parameters.AddWithValue("pvar_username",model.username); 
												NpgsqlParameter outParm = new NpgsqlParameter("pvar_returnMessage", NpgsqlDbType.Varchar)
												{
													 Direction = ParameterDirection.Output
												};
												dbCommand.Parameters.Add(outParm);;

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

					public virtual string  upd_userlockout(userlockoutModel model)
					{ 
						string ResponseMessage="";
						try{

							 		 using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
					        {
						        npsql.Open();
						        using (var dbCommand = new NpgsqlCommand("\"upd_sp_userlockout\"", npsql))
						        {
                                        dbCommand.CommandType = CommandType.StoredProcedure;
						            	dbCommand.Parameters.AddWithValue("pvar_lockoutid",model.lockoutid);
										dbCommand.Parameters.AddWithValue("pvar_loginUser",model.loginUser);
                                        NpgsqlParameter outParm = new NpgsqlParameter("@returnMessage", NpgsqlDbType.Varchar)
                                        {
                                             Direction = ParameterDirection.Output
                                        };
                                        dbCommand.Parameters.Add(outParm);;

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

					public virtual System.Data.DataTable get_userlockout()
					{

									DataSet dataSet = new DataSet();
									 DataTable dataTable=new DataTable();
									try{
										  using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
					                                    {
						                                    npsql.Open();
						                                    using (var dbCommand = new NpgsqlCommand("\"get_sp_userlockout\"", npsql))
						                                    {
                                            
                                                                                                          
                                                                dbCommand.CommandType = CommandType.StoredProcedure;
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
					
