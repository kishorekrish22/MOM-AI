namespace MahatProjects.Models{
			using System;
			using System.ComponentModel.DataAnnotations;
			using Microsoft.AspNetCore.Mvc;
			using System.Collections.Generic;
			using FluentValidation;
			using System.Linq;
			//This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:25
			public class AlertTemplatesModel
			{

			 public System.Int64 ? id { get; set; }

[xssFilter]
public string entityname{ get; set; }

[xssFilter]
public string? entityaction{ get; set; }

public bool sendasbatch	{ get; set; }

[xssFilter]
public string? alerttype{ get; set; }

[xssFilter]
public string? alertsubject{ get; set; }

[xssFilter]
public string? alertcopyto{ get; set; }

[xssFilter]
public string? alertcarboncopyto{ get; set; }

[xssFilter]
public string? alertcontent{ get; set; }
public System.Int64 created_by_id	{ get; set; }
[DataType(DataType.Date)]
[ModelBinder(BinderType = typeof(DateTimeModelBinder))]
[DisplayFormat(DataFormatString="{0:dd/MM/yyyy}", ApplyFormatInEditMode=true)]
public System.DateTime ?created_at	{ get; set; }
public System.Int64? updated_by_id	{ get; set; }
[DataType(DataType.Date)]
[ModelBinder(BinderType = typeof(DateTimeModelBinder))]
[DisplayFormat(DataFormatString="{0:dd/MM/yyyy}", ApplyFormatInEditMode=true)]
public System.DateTime ?updated_at	{ get; set; }
public bool isdeleted	{ get; set; }
[xssFilter]
                        [Required(ErrorMessage = "craftmyapp_actionmethodname is required,please pass current action name")]
                        public String craftmyapp_actionmethodname{ get; set; }



			}
			

			public class AlertTemplatesModelValidator: AbstractValidator<AlertTemplatesModel>
			{
					 
					public AlertTemplatesModelValidator()
					{

						 When(model => model.craftmyapp_actionmethodname == "Create_Alert_Templates", () =>
                                    {
                                        {RuleFor(m => m.entityname)
.NotEmpty().WithMessage("Entity Name is required")
.MaximumLength(128).WithMessage("The allowed length of Entity Name is 128 characters or fewer")
;




RuleFor(m => m.alertcopyto)
.MaximumLength(50).WithMessage("The allowed length of Alert Copy To is 50 characters or fewer")
.EmailAddress()

;
RuleFor(m => m.alertcarboncopyto)
.MaximumLength(50).WithMessage("The allowed length of Alert Carbon Copy To  is 50 characters or fewer")
.EmailAddress()

;

}

                                    });
When(model => model.craftmyapp_actionmethodname == "Update_Alert_Templates", () =>
                                    {
                                        {RuleFor(m => m.entityname)
.NotEmpty().WithMessage("Entity Name is required")
.MaximumLength(128).WithMessage("The allowed length of Entity Name is 128 characters or fewer")
;




RuleFor(m => m.alertcopyto)
.MaximumLength(50).WithMessage("The allowed length of Alert Copy To is 50 characters or fewer")
.EmailAddress()

;
RuleFor(m => m.alertcarboncopyto)
.MaximumLength(50).WithMessage("The allowed length of Alert Carbon Copy To  is 50 characters or fewer")
.EmailAddress()

;

}

                                    });

						 
						
					}

			}

                

                

                
 

                

                
			}
