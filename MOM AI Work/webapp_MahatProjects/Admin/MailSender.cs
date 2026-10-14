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
using System.Text;
namespace Admin.Controllers
{
    public class MailSender
    {

		public async Task<bool> sendMail(
			 string entityname
			, string entityactionname
			, string entityid
			, string mailfor
			 
			, IOptions<MailSettings> _mailSettings
			, string loginUserID
			, HttpClient client
			, string optionaldataOne=""
			,string optionaldataTwo="")
		{




			DataTable mailData = new DataTable();

			bool mailSent = false;
			HttpResponseMessage response_alert_maildata = await ApiClient.GET_ApiValuesGetRespnse(client, "api/MailLogs/MailSender?mailfor=" + entityactionname + "&entityid=" + entityid + "&created_by_id=" + loginUserID);


			if (response_alert_maildata.IsSuccessStatusCode)
			{
				mailData = await response_alert_maildata.Content.ReadAsAsync<DataTable>();

				if (mailData.Rows.Count > 0)
				{

					HttpResponseMessage response_alert_templates = await ApiClient.GET_ApiValuesGetRespnse(client, "api/AlertTemplates/Alert_Templates_List?entityname=" + entityname + "&entityaction=" + entityactionname + "");


					if (response_alert_templates.IsSuccessStatusCode)
					{
						DataTable dt_alert_template = await response_alert_templates.Content.ReadAsAsync<DataTable>();
						if (dt_alert_template.Rows.Count > 0)
						{
							if (dt_alert_template.Rows[0]["alerttype"].ToString() == "Email")
							{
								string alertcontent = dt_alert_template.Rows[0]["alertcontent"].ToString();
								string alertsubject = "";
								switch (entityactionname)
								{
									 
									case "forgotpassword":
										alertcontent = alertcontent.Replace("{toname}", mailData.Rows[0]["toname"].ToString())
											.Replace("{username}", mailData.Rows[0]["username"].ToString())
											.Replace("{userpassword}", optionaldataTwo)
											.Replace("{userrole}", mailData.Rows[0]["userrole"].ToString())
											.Replace("{clienturl}", optionaldataTwo);

										alertsubject = dt_alert_template.Rows[0]["alertsubject"].ToString();

										break;

									default:
										break;

								}


								Mailer objmail = new Mailer(_mailSettings);

								string tomail = mailData.Rows[0]["receiveremail"].ToString();
								if (tomail != "")
								{
									mailSent = objmail.SendMail_TLS(tomail, alertsubject, alertcontent, true, null, true);


									MailLogsModel objMailLogsModel = new MailLogsModel();
									objMailLogsModel.entityname = entityname;
									objMailLogsModel.entityid = entityid;
									objMailLogsModel.mailfor = mailfor;
									objMailLogsModel.mailto = tomail;
									objMailLogsModel.mailsubject = alertsubject;
									objMailLogsModel.mailbody = alertcontent;
									objMailLogsModel.issent = mailSent;
									objMailLogsModel.created_by_id =Convert.ToInt64(loginUserID);
									objMailLogsModel.craftmyapp_actionmethodname = "Create_MailLog";
									string strMailLogsReturnMessage = await ApiClient.Post_ApiValuesGetString(client, "api/MailLogs/Create_MailLog", objMailLogsModel);
								}

							}
						}
					}

				}

		}

			return mailSent;
		}
                public async Task<bool> sendNotification(
             string entityname
            , string entityactionname
            , string entityid
            , IOptions<MailSettings> _mailSettings
            , string loginUserID
            , HttpClient client)
        {
            bool mailSent = false;
            try
            {





                mailmodel mailData = new mailmodel();
               
                HttpResponseMessage response_alert_maildata = await ApiClient.GET_ApiValuesGetRespnse(client, "api/MailLogs/Mailer?entityname=" + entityname + "&entityactionname=" + entityactionname + "&entityid=" + entityid);

                 

                if (response_alert_maildata.IsSuccessStatusCode)
                {
                    mailData = await response_alert_maildata.Content.ReadAsAsync<mailmodel>();



                    if (mailData.mailsubject != "")
                    {


                Mailer objmail = new Mailer(_mailSettings);


                if (mailData.mailto != "")
                        {
                    mailSent = objmail.SendMail_TLS(mailData.mailto, mailData.mailsubject, mailData.mailbody, true, null, true);
                }
            }
        }



                MailLogsModel objMailLogsModel = new MailLogsModel();
                objMailLogsModel.entityname = entityname;
                objMailLogsModel.entityid = entityid;
                objMailLogsModel.mailfor = entityname+" / " +entityactionname;
                objMailLogsModel.mailto = mailData.mailto;
                objMailLogsModel.mailsubject = mailData.mailsubject;
                objMailLogsModel.mailbody = mailData.mailbody;
                objMailLogsModel.issent = mailSent;
                objMailLogsModel.created_by_id =Convert.ToInt64(loginUserID);
                objMailLogsModel.craftmyapp_actionmethodname = "Create_MailLog";


                string strMailLogsReturnMessage = await ApiClient.Post_ApiValuesGetString(client, "api/MailLogs/Create_MailLog", objMailLogsModel);
    }
            catch (Exception ex)
            {

            }

return mailSent;
        }
         public async Task<bool> sendWhatsAppNotification(
      string entityname
     , string entityactionname
     , string entityid
     , IOptions<MailSettings> _mailSettings
     , string loginUserID
     , HttpClient client)
        {
            bool mailSent = false;
            try
            {





                mailmodel mailData = new mailmodel();

                HttpResponseMessage response_alert_maildata = await ApiClient.GET_ApiValuesGetRespnse(client, "api/MailLogs/WhatsApp?entityname=" + entityname + "&entityactionname=" + entityactionname + "&entityid=" + entityid);



                if (response_alert_maildata.IsSuccessStatusCode)
                {
                    mailData = await response_alert_maildata.Content.ReadAsAsync<mailmodel>();



                    if (mailData.mailsubject != "")
                    {


                        Mailer objmail = new Mailer(_mailSettings);


                        if (mailData.mailto != "")
                        {


                            bool retValue = false;
                            try
                            {
                                string apiUrl = "http://103.212.121.131/api/v1/postmessage.ashx";
                                string bearerToken = "j30qfoeyrddqh19m1w90ryu7ttnax4jwfkjccses";
                                using (HttpClient wappclient = new HttpClient())
                                {
                                    string[] destinationNumbers = mailData.mailto.Split(',');

                                    
                                    foreach (string destNumber in destinationNumbers)
                                    {
                                       
                                        string jsonBody = $@"
                                        {{
                                            ""Destination"": ""{destNumber}"",
                                            ""body"": ""{mailData.mailbody}"",
                                            ""datetosend"": ""{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}"" 
                                        }}";

                                        // Set the authorization header with the bearer token
                                        wappclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                                        // Set the content type
                                        HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                                        // Send the POST request
                                        HttpResponseMessage response = await wappclient.PostAsync(apiUrl, content);

                                        // Check if the request was successful
                                        if (response.IsSuccessStatusCode)
                                        {
                                            // Read the response content if needed
                                            string responseBody = await response.Content.ReadAsStringAsync();
                                            LogginLib.WriteLog("SendMail Exception Message: " + responseBody);

                                            retValue = true;
                                        }
                                        else
                                        {
                                            LogginLib.WriteLog("SendMail Exception Message: " + response.StatusCode);

                                        }
                                    }
                                }


                            }
                            catch (Exception ex)
                            {
                                // LogginLib.WriteLog(ex);

                                LogginLib.WriteLog("SendMail Exception Message: " + ex.Message);
                                if (ex.InnerException != null)
                                    LogginLib.WriteLog(" SendMail Exception Inner:   " + ex.InnerException);


                            }
                            LogginLib.WriteLog("---mailer code worked--");
                            return retValue;
                        }


                    }
                }



                MailLogsModel objMailLogsModel = new MailLogsModel();
                objMailLogsModel.entityname = entityname;
                objMailLogsModel.entityid = entityid;
                objMailLogsModel.mailfor = entityname + " / " + entityactionname;
                objMailLogsModel.mailto = mailData.mailto;
                objMailLogsModel.mailsubject = mailData.mailsubject;
                objMailLogsModel.mailbody = mailData.mailbody;
                objMailLogsModel.issent = mailSent;
                 objMailLogsModel.created_by_id =Convert.ToInt64(loginUserID);
                objMailLogsModel.craftmyapp_actionmethodname = "Create_MailLog";


                string strMailLogsReturnMessage = await ApiClient.Post_ApiValuesGetString(client, "api/MailLogs/Create_MailLog", objMailLogsModel);
            }
            catch (Exception ex)
            {

            }

            return mailSent;
        }
	}
}


