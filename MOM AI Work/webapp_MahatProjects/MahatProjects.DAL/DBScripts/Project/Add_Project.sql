
			  CREATE OR REPLACE FUNCTION  "Add_Project"
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
 
				    ,pvar_created_by_id  Bigint 				  
                    ,OUT pvar_returnid Bigint
                    ,OUT pvar_returnMessage Varchar(4000)
			  )
              AS $BODY$  
              DECLARE lv_viewactionroles Varchar(128);
              BEGIN

				/*This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:31:56*/
		

			   
              
			  
			  IF "Check_Authorization"(pvar_created_by_id, 'Project', 'create') THEN
			  pvar_returnMessage:='';
			  
                
			  if(pvar_returnMessage='')
			  THEN

			  

			  INSERT INTO Project(
				 project_name
,description
,handled_by
,interaction_date
,video_upload
,transcript

				 ,created_by_id
				  
				 
                 
			  )
			  VALUES (
 				 pvar_project_name
,pvar_description
,pvar_handled_by
,pvar_interaction_date
,pvar_video_upload
,pvar_transcript

				 ,pvar_created_by_id
				 
				 
                    
			  );
			   
               SELECT currval(pg_get_serial_sequence('Project','id')) INTO pvar_returnid;
                SELECT currval(pg_get_serial_sequence('Project','id')) INTO pvar_id;

              
			  

               
			  
					 
			  pvar_returnMessage := '201.1';
              END IF;
			   

			  
																ELSE
															    INSERT INTO system_logging
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
																,'Add_Project'
																,'Authorization Failed Add_Project'
																,pvar_created_by_id
																);
																pvar_returnMessage := '401.1';
															    END IF;
			  /*EXCEPTION WHEN OTHERS THEN
			 
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
						 
						,'Store Proc Exception'
						,NOW()
						,'16'
						,'Add_Project'
						,'insert failed'
						);
                        pvar_returnMessage := 'Add_Project - Insert failed';*/
			  	
			  END
              $BODY$
              LANGUAGE plpgsql;

