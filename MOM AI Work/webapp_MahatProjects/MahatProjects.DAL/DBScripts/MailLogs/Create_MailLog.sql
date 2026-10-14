
			  CREATE OR REPLACE FUNCTION  "Create_MailLog"
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
 
				    ,pvar_created_by_id  Bigint 				  
                    ,OUT pvar_returnid Bigint
                    ,OUT pvar_returnMessage Varchar(4000)
			  )
              AS $BODY$  
              
              BEGIN

				/*This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:26*/
		

			   
              
			  
			  
			  pvar_returnMessage:='';
			  
                
			  if(pvar_returnMessage='')
			  THEN

			  

			  INSERT INTO MailLogs(
				 entityname
,entityid
,mailfor
,mailsubject
,mailto
,mailbody
,issent

				 ,created_by_id
				  
				 
                 
			  )
			  VALUES (
 				 pvar_entityname
,pvar_entityid
,pvar_mailfor
,pvar_mailsubject
,pvar_mailto
,pvar_mailbody
,pvar_issent

				 ,pvar_created_by_id
				 
				 
                    
			  );
			   
               SELECT currval(pg_get_serial_sequence('MailLogs','id')) INTO pvar_returnid;
                SELECT currval(pg_get_serial_sequence('MailLogs','id')) INTO pvar_id;

              
			  

               
			  
					 
			  pvar_returnMessage := '201.1';
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
						,'Create_MailLog'
						,'insert failed'
						);
                        pvar_returnMessage := 'Create_MailLog - Insert failed';*/
			  	
			  END
              $BODY$
              LANGUAGE plpgsql;

