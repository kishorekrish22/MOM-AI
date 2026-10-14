namespace MahatProjects.Models
								{
									using System.ComponentModel.DataAnnotations;
									using Microsoft.AspNetCore.Mvc;
									//This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:17
									public class lookupsModel
									{
										[Required(ErrorMessage="lookupid is Required")]
										public System.Int64 lookupid	{ get; set; }
										public string entityname	{ get; set; }
										public string attributetype	{ get; set; }
										public string fieldname	{ get; set; }
										public string fielddesc	{ get; set; }
										public System.Int64 created_by_id	{ get; set; }
										[DataType(DataType.Date)]
										[ModelBinder(BinderType = typeof(DateTimeModelBinder))]
										[DisplayFormat(DataFormatString="{0:dd/MM/yyyy}", ApplyFormatInEditMode=true)]
										public System.DateTime ?created_at	{ get; set; }
										public System.Int64 ?updated_by_id	{ get; set; }
										[DataType(DataType.Date)]
										[ModelBinder(BinderType = typeof(DateTimeModelBinder))]
										[DisplayFormat(DataFormatString="{0:dd/MM/yyyy}", ApplyFormatInEditMode=true)]
										public System.DateTime ?updated_at	{ get; set; }
										public bool isdeleted	{ get; set; }
									}
								}
