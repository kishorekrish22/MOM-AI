namespace MahatProjects.DAL{
					using System;
					using System.Text;
					using System.Data;
					using System.Data.Common;
					using MahatProjects.Models;
					using EncrypDecrypt;
					using Npgsql;
					using NpgsqlTypes;
					using System.IdentityModel.Tokens.Jwt;
					using System.Linq;
					//This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:24

					public class External_System_DAL
					{
						public External_System_DAL(string connectionString)
						{
							db_connectionstring = connectionString;
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
						_connectionstring = value;
						}
						}
 
					public virtual string Create_access_logs(access_logsModel model)
					{
						String ResponseMessage = "";

						try
						{

						using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
						{
							npsql.Open();
							using (var dbCommand = new NpgsqlCommand("\"Create_access_logs\"", npsql))
							{
								dbCommand.CommandType = CommandType.StoredProcedure;

								dbCommand.Parameters.AddWithValue("pvar_access_logsid", NpgsqlDbType.Bigint, (object)model.access_logsid ?? DBNull.Value);

								dbCommand.Parameters.AddWithValue("pvar_users_id", NpgsqlDbType.Varchar, (object)model.users_id ?? DBNull.Value);

								dbCommand.Parameters.AddWithValue("pvar_logged_date", NpgsqlDbType.Timestamp, (object)model.logged_date ?? DBNull.Value);

								dbCommand.Parameters.AddWithValue("pvar_expiry_date", NpgsqlDbType.Timestamp, (object)model.expiry_date ?? DBNull.Value);

								dbCommand.Parameters.AddWithValue("pvar_user_token", NpgsqlDbType.Varchar, (object)model.user_token ?? DBNull.Value);


								dbCommand.Parameters.AddWithValue("pvar_latlan", NpgsqlDbType.Varchar, (object)model.latlan ?? DBNull.Value);

								dbCommand.Parameters.AddWithValue("pvar_clientipaddress", NpgsqlDbType.Varchar, (object)model.clientipaddress ?? DBNull.Value);

								dbCommand.Parameters.AddWithValue("pvar_devicename", NpgsqlDbType.Varchar, (object)model.devicename ?? DBNull.Value);

								dbCommand.Parameters.AddWithValue("pvar_browsername", NpgsqlDbType.Varchar, (object)model.browsername ?? DBNull.Value);


								dbCommand.Parameters.AddWithValue("pvar_external_entity_name", NpgsqlDbType.Varchar, (object)model.external_entity_name ?? DBNull.Value);

								dbCommand.Parameters.AddWithValue("pvar_external_users_id", NpgsqlDbType.Varchar, (object)model.external_users_id ?? DBNull.Value);

								dbCommand.Parameters.AddWithValue("pvar_created_by_id", NpgsqlDbType.Bigint, (object)model.created_by_id ?? DBNull.Value);

							    dbCommand.Parameters.AddWithValue("pvar_decoded_email", NpgsqlDbType.Varchar, (object)model.decoded_email ?? DBNull.Value);
								dbCommand.Parameters.AddWithValue("pvar_decoded_users_id", NpgsqlDbType.Varchar, (object)model.decoded_users_id ?? DBNull.Value);
								dbCommand.Parameters.AddWithValue("pvar_decoded_role", NpgsqlDbType.Varchar, (object)model.decoded_role ?? DBNull.Value);

								
								NpgsqlParameter outParm = new NpgsqlParameter("pvar_returnMessage", NpgsqlDbType.Varchar)
								{
									Direction = ParameterDirection.Output
								};
								dbCommand.Parameters.Add(outParm); ;

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
						catch (Exception ex)
						{
						ResponseMessage = ex.Message;
						}

						return ResponseMessage;

					}
public virtual string create_access_logs_details(access_logsdetailsModel model)
					{
						String ResponseMessage = "";

						try
						{

						using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
						{
							npsql.Open();
							using (var dbCommand = new NpgsqlCommand("\"create_access_logs_details\"", npsql))
							{
								dbCommand.CommandType = CommandType.StoredProcedure;

								dbCommand.Parameters.AddWithValue("pvar_access_logsid", NpgsqlDbType.Bigint, (object)model.access_logsid ?? DBNull.Value);

								dbCommand.Parameters.AddWithValue("pvar_action_method_name", NpgsqlDbType.Varchar, (object)model.action_method_name ?? DBNull.Value);

								dbCommand.Parameters.AddWithValue("pvar_api_response", NpgsqlDbType.Varchar, (object)model.api_response ?? DBNull.Value);
 
								NpgsqlParameter outParm = new NpgsqlParameter("pvar_returnMessage", NpgsqlDbType.Varchar)
								{
									Direction = ParameterDirection.Output
								};
								dbCommand.Parameters.Add(outParm); ;

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
						catch (Exception ex)
						{
						ResponseMessage = ex.Message;
						}

						return ResponseMessage;

					}

					public virtual string Create_api_access_logs(access_logsModel model)
					{
						String ResponseMessage = "";

						try
						{

						using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
						{
							npsql.Open();
							using (var dbCommand = new NpgsqlCommand("\"Create_api_access_logs\"", npsql))
							{
								dbCommand.CommandType = CommandType.StoredProcedure;

								dbCommand.Parameters.AddWithValue("pvar_api_access_logsid", NpgsqlDbType.Bigint, (object)model.access_logsid ?? DBNull.Value);

								dbCommand.Parameters.AddWithValue("pvar_users_id", NpgsqlDbType.Varchar, (object)model.users_id ?? DBNull.Value);

								dbCommand.Parameters.AddWithValue("pvar_logged_date", NpgsqlDbType.Timestamp, (object)model.logged_date ?? DBNull.Value);

								dbCommand.Parameters.AddWithValue("pvar_user_token", NpgsqlDbType.Varchar, (object)model.user_token ?? DBNull.Value);

								dbCommand.Parameters.AddWithValue("pvar_request_type", NpgsqlDbType.Varchar, (object)model.request_type ?? DBNull.Value);

								dbCommand.Parameters.AddWithValue("pvar_api_url", NpgsqlDbType.Varchar, (object)model.api_url ?? DBNull.Value);


								if (model.request_json != "")
									dbCommand.Parameters.AddWithValue("pvar_request_json", NpgsqlDbType.Json, (object)model.request_json ?? DBNull.Value);
								else
									dbCommand.Parameters.AddWithValue("pvar_request_json", NpgsqlDbType.Json, DBNull.Value);


								if (model.response_json !="")
									dbCommand.Parameters.AddWithValue("pvar_response_json", NpgsqlDbType.Json, (object)model.response_json ?? DBNull.Value);
								else
									dbCommand.Parameters.AddWithValue("pvar_response_json", NpgsqlDbType.Json, DBNull.Value);

								dbCommand.Parameters.AddWithValue("pvar_created_by_id", NpgsqlDbType.Bigint, (object)model.created_by_id ?? DBNull.Value);
								NpgsqlParameter outParm = new NpgsqlParameter("pvar_returnMessage", NpgsqlDbType.Varchar)
								{
									Direction = ParameterDirection.Output
								};
								dbCommand.Parameters.Add(outParm); ;

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
						catch (Exception ex)
						{
						ResponseMessage = ex.Message;
						}

						return ResponseMessage;

					}


					public virtual bool get_active_token(string pvar_users_id,string pvar_external_entity_name, ref string pvar_user_token,ref string pvar_external_users_id)
					{
						bool is_token_active = false;

						DataTable dataTable = new DataTable();
						DataSet dataSet = new DataSet();

						try
						{

						using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
						{
							npsql.Open();
							using (var dbCommand = new NpgsqlCommand("\"get_active_token\"", npsql))
							{
								dbCommand.CommandType = CommandType.StoredProcedure;
 
								dbCommand.Parameters.AddWithValue("pvar_users_id", NpgsqlDbType.Varchar, (object)pvar_users_id ?? DBNull.Value);
								dbCommand.Parameters.AddWithValue("pvar_external_entity_name", NpgsqlDbType.Varchar, (object)pvar_external_entity_name ?? DBNull.Value);


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
						catch(Exception ex)
						{
						throw;
						}
						if(dataTable.Rows.Count > 0)
						{
						is_token_active = true;
						pvar_user_token = dataTable.Rows[0]["user_token"].ToString();
						pvar_external_users_id= dataTable.Rows[0]["external_users_id"].ToString();
						}

						return is_token_active;


						}

					   public virtual string get_users_info_by_token(string user_token)
					{
            
						// Create a JwtSecurityTokenHandler to validate and read the JWT
						var tokenHandler = new JwtSecurityTokenHandler();

						// Read and parse the JWT string into a JwtSecurityToken
						var jwtToken = tokenHandler.ReadJwtToken(user_token);

						// Access the claims from the JwtSecurityToken's Claims property
						var claims = jwtToken.Claims;

						// Retrieve custom claim values by claim type
						var usersid = claims.FirstOrDefault(c => c.Type == "usersid")?.Value;
 

						return usersid;


					}
						
						public virtual String[] get_users_by_token(string user_token)
        {
            String[] userdetails = new String[2];

                        DataTable dataTable = new DataTable();
						DataSet dataSet = new DataSet();

           
            try
            {

					using (NpgsqlConnection npsql = new NpgsqlConnection(db_connectionstring))
						{
							npsql.Open();
							using (var dbCommand = new NpgsqlCommand("\"get_users_by_token\"", npsql))
							{
								dbCommand.CommandType = CommandType.StoredProcedure;
 
								dbCommand.Parameters.AddWithValue("pvar_user_token", NpgsqlDbType.Varchar, (object)user_token ?? DBNull.Value);
							 
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
            catch (Exception ex)
            {
                throw;
            }
            if (dataTable.Rows.Count > 0)
            {
                userdetails[0] = dataTable.Rows[0]["users_id"].ToString();
				userdetails[1] = dataTable.Rows[0]["access_logsid"].ToString();                

            }

            return userdetails;


        }


					}
 
					}
					
