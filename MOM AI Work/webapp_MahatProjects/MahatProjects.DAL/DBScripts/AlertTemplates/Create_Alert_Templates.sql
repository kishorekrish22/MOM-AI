
			  CREATE OR REPLACE FUNCTION  "Create_Alert_Templates"
			  (
				    pvar_id Bigint
,
pvar_entityname Varchar(128)
,
pvar_entityaction Varchar(128)
,
pvar_sendasbatch Boolean
,
pvar_alerttype  Varchar(1024)
,
pvar_alertsubject Varchar(256)
,
pvar_alertcopyto Varchar(50)
,
pvar_alertcarboncopyto Varchar(50)
,
pvar_alertcontent text
 
				    ,pvar_created_by_id  Bigint 				  
                    ,OUT pvar_returnid Bigint
                    ,OUT pvar_returnMessage Varchar(4000)
			  )
              AS $BODY$  
              DECLARE lv_viewactionroles Varchar(128);
              BEGIN

				/*This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:25*/
		

			   
              
			  
			  IF "Check_Authorization"(pvar_created_by_id, 'AlertTemplates', 'create') THEN
			  pvar_returnMessage:='';
			  
              IF(pvar_alerttype is not null AND pvar_alerttype!='0' AND LENGTH(pvar_alerttype)>0)
                                                            THEN                        
                                                                 if(CAST((SELECT Count(T1.T1) 
                                                                FROM regexp_split_to_table(pvar_alerttype, ',') AS T1
                                                                    INNER JOIN regexp_split_to_table((Select  fielddesc 
                                                                from lookups  where fieldname='alerttype'
                                                                and entityname='AlertTemplates' LIMIT 1), ',') AS T2 on T1.T1 = T2.T2) AS int) <> CAST((SELECT Count(T1.T1)
                                                                FROM regexp_split_to_table(pvar_alerttype, ',')  AS T1) AS int))
                                                                THEN
                                                                        pvar_returnMessage := pvar_returnMessage || 'alerttype value is invalid';


                                                                END IF;
                                                            END IF;
  
			  if(pvar_returnMessage='')
			  THEN

			  

			  INSERT INTO AlertTemplates(
				 entityname
,entityaction
,sendasbatch
,alerttype
,alertsubject
,alertcopyto
,alertcarboncopyto
,alertcontent

				 ,created_by_id
				  
				 
                 
			  )
			  VALUES (
 				 pvar_entityname
,pvar_entityaction
,pvar_sendasbatch
,pvar_alerttype
,pvar_alertsubject
,pvar_alertcopyto
,pvar_alertcarboncopyto
,pvar_alertcontent

				 ,pvar_created_by_id
				 
				 
                    
			  );
			   
               SELECT currval(pg_get_serial_sequence('AlertTemplates','id')) INTO pvar_returnid;
                SELECT currval(pg_get_serial_sequence('AlertTemplates','id')) INTO pvar_id;

              
			  

               
			  
					 
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
																,'Create_Alert_Templates'
																,'Authorization Failed Create_Alert_Templates'
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
						,'Create_Alert_Templates'
						,'insert failed'
						);
                        pvar_returnMessage := 'Create_Alert_Templates - Insert failed';*/
			  	
			  END
              $BODY$
              LANGUAGE plpgsql;

