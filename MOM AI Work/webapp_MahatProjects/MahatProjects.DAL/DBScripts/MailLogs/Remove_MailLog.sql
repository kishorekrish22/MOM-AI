
			  CREATE OR REPLACE FUNCTION  "Remove_MailLog"
			  (
				  pvar_id Varchar(50)
				  ,pvar_updated_by_id  Varchar(50)
				  ,OUT pvar_returnMessage Varchar(4000)
			  )
			  RETURNS Varchar(4000) 
              AS $BODY$  
              DECLARE lv_viewactionroles Varchar(128);
              BEGIN
              /*This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:26*/
			  IF "Check_Authorization"(pvar_updated_by_id::Bigint, 'MailLogs', 'delete') THEN
			  
            INSERT INTO MailLogs_history
SELECT *,NOW(),pvar_updated_by_id:: Bigint,'Deleted' FROM MailLogs
 WHERE CAST(id AS VARCHAR)= pvar_id;

			 
			 UPDATE MailLogs SET
													isdeleted=true ,updated_by_id=CAST(pvar_updated_by_id AS Bigint),updated_at=NOW()
													WHERE CAST(MailLogs.id AS VARCHAR)=pvar_id;
					 
				  pvar_returnMessage := '201.1';
					 
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
																,'Remove_MailLog'
																,'Authorization Failed Remove_MailLog'
																,pvar_updated_by_id
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
						
						,'Postgre Function Exception'
						,NOW()
						,'16'
						,'Remove_MailLog'
						,'user delete failed'
						);				 
					    pvar_returnMessage := 'user delete failed';*/
				  
			  
 			  END
              $BODY$
              LANGUAGE plpgsql;

