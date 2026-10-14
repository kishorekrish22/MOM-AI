namespace MahatProjects.Models{
			using System;
			using System.ComponentModel.DataAnnotations;
			using Microsoft.AspNetCore.Mvc;
			using System.Collections.Generic;
			using FluentValidation;
			using System.Linq;
			//This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:31:56
			public class ProjectModel
			{

			 public System.Int64 ? id { get; set; }

[xssFilter]
public string? project_name{ get; set; }

[xssFilter]
public string? description{ get; set; }

[xssFilter]
public string? handled_by{ get; set; }

[DataType(DataType.Date)][ModelBinder(BinderType = typeof(DateTimeModelBinder))][DisplayFormat(DataFormatString="{0:dd/MM/yyyy}", ApplyFormatInEditMode=true)]public DateTime? interaction_date	{ get; set; }

[xssFilter]
public string? video_upload{ get; set; }

[xssFilter]
public string? transcript{ get; set; }
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
			

			public class ProjectModelValidator: AbstractValidator<ProjectModel>
			{
					 
					public ProjectModelValidator()
					{

						 When(model => model.craftmyapp_actionmethodname == "Add_Project", () =>
                                    {
                                        {


RuleFor(m => m.interaction_date)


;


}

                                    });
When(model => model.craftmyapp_actionmethodname == "Update_Project", () =>
                                    {
                                        {


RuleFor(m => m.interaction_date)


;


}

                                    });

						 
						
					}

			}

                

                

                
 

                

                
			}
