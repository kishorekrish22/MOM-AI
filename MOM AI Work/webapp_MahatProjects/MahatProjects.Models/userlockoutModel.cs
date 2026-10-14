namespace MahatProjects.Models
								{
									using System;
									using System.ComponentModel.DataAnnotations;
									using Microsoft.AspNetCore.Mvc;
									//This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:17
									
									public class userloginModel
									{
										[Required(ErrorMessage="UserName is Required")]	
										[xssFilter]	
										public String username {get; set; }

										[Required(ErrorMessage="Password is Required")]
										[xssFilter]	
										public String userpassword{ get; set; }

										[xssFilter]	
										public String latlan{ get; set; }

										[xssFilter]	
										public String clientipaddress{ get; set; }
										
										[xssFilter]
										public String devicename { get; set; }

										[xssFilter]
										public String browsername { get; set; }
  
										[xssFilter]
										public String deviceid { get; set; }

										[xssFilter]
										public String source { get; set; }
										
										[xssFilter]
										public String accesskey { get; set; }

										[xssFilter]
										public String notificationid { get; set; }
										
										[xssFilter]
										public String mobilenumber {get; set; }
									
									}
									public class userlockoutModel
									{
										public String lockoutid{get;set;}

										[xssFilter]	
										public String username {get; set; }
										public int numberOfFailedTry {get; set; }
										[DataType(DataType.Date)]
										[ModelBinder(BinderType = typeof(DateTimeModelBinder))]
										[DisplayFormat(DataFormatString="{0:dd/MM/yyyy}", ApplyFormatInEditMode=true)]
										public System.DateTime ? lastupdateddate {get; set; }
										public String lockremovedby {get; set; }
										public String latlan {get; set; }
										public String remoteipaddress {get; set; }
										public String clientipaddress {get; set; }
										public String loginUser {get; set; }
									}
									public class response_model
									{
										public String message{get;set;}
										public String id{get;set;}
									}
								}
