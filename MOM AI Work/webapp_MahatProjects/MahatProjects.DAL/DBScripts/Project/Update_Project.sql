
			  CREATE OR REPLACE FUNCTION  "Update_Project"
			  (
				  pvar_id Bigint
,
pvar_project_name Varchar(128)
,
pvar_description Varchar(256)
,
pvar_handled_by Varchar(128)
,
pvar_interaction_date date
,
pvar_video_upload Varchar(256)
,
pvar_transcript Varchar(6000)

				  ,pvar_updated_by_id  Bigint
				  ,OUT pvar_returnMessage Varchar(4000)
			  )
			  RETURNS Varchar(4000) 
              AS $BODY$  
              DECLARE lv_viewactionroles Varchar(128);
              BEGIN

			  /*This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:31:56*/
			  IF "Check_Authorization"(pvar_updated_by_id, 'Project', 'edit') THEN


			  pvar_returnMessage:='';

			  
                
			  IF(pvar_returnMessage='')
			  THEN

                    INSERT INTO Project_history
SELECT *,NOW(),pvar_updated_by_id:: Bigint,'Modified' FROM Project
 WHERE id= pvar_id;

                    
                    UPDATE Project SET
                    project_name=pvar_project_name
,description=pvar_description
,handled_by=pvar_handled_by
,interaction_date=pvar_interaction_date
,video_upload=pvar_video_upload
,transcript=pvar_transcript

                    
                    ,updated_by_id=pvar_updated_by_id,updated_at=NOW()
                    WHERE id=pvar_id;

                    

                    


					
							
					pvar_returnMessage := '201.1';
			
			  END IF;

			  
																ELSE
															 
																INSERT INTO System_logging
																(
																Log_code
																 
																,log_application
																,log_date
																,log_level
																,log_logger
																,log_message
																,log_user_name
																)
																VALUES
																('401.1'
																 
																,'Store Proc Authorization Check'
																,NOW()
																,'Critical'
																,'Update_Project'
																,'Authorization Failed Update_Project'
																,pvar_updated_by_id
																);
																pvar_returnMessage = '401.1';
																
																END IF;

			  			 /* EXCEPTION WHEN OTHERS THEN
			 
						INSERT INTO system_logging
						(
						Log_code
						
						,log_application
						,log_date
						,log_level
						,log_logger
						,log_message
						)
						VALUES
						('16'
						
						,'Postgre Function Exception'
						,NOW()
						,'16'
						,'Update_Project'
						,'update failed'
						);
                        pvar_returnMessage := 'Update_Project - update failed';*/
			  	
			  END
              $BODY$
              LANGUAGE plpgsql;

