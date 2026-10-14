
			  CREATE OR REPLACE FUNCTION  "Update_MailLog"
			  (
				  pvar_id Bigint
,
pvar_entityname Varchar(128)
,
pvar_entityid Varchar(128)
,
pvar_mailfor Varchar(256)
,
pvar_mailsubject Varchar(256)
,
pvar_mailto Varchar(4096)
,
pvar_mailbody text
,
pvar_issent Boolean

				  ,pvar_updated_by_id  Bigint
				  ,OUT pvar_returnMessage Varchar(4000)
			  )
			  RETURNS Varchar(4000) 
              AS $BODY$  
              DECLARE lv_viewactionroles Varchar(128);
              BEGIN

			  /*This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:26*/
			  IF "Check_Authorization"(pvar_updated_by_id, 'MailLogs', 'edit') THEN


			  pvar_returnMessage:='';

			  
                
			  IF(pvar_returnMessage='')
			  THEN

                    INSERT INTO MailLogs_history
SELECT *,NOW(),pvar_updated_by_id:: Bigint,'Modified' FROM MailLogs
 WHERE id= pvar_id;

                    
                    UPDATE MailLogs SET
                    entityname=pvar_entityname
,entityid=pvar_entityid
,mailfor=pvar_mailfor
,mailsubject=pvar_mailsubject
,mailto=pvar_mailto
,mailbody=pvar_mailbody
,issent=pvar_issent

                    
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
																,'Update_MailLog'
																,'Authorization Failed Update_MailLog'
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
						,'Update_MailLog'
						,'update failed'
						);
                        pvar_returnMessage := 'Update_MailLog - update failed';*/
			  	
			  END
              $BODY$
              LANGUAGE plpgsql;

