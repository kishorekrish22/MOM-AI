
			  CREATE OR REPLACE FUNCTION  "getById_sp_all_MailLogs"
              (
			  pvar_id Varchar
			  )
              RETURNS TABLE(
                "id" Bigint
,entityname Varchar
,entityid Varchar
,mailfor Varchar
,mailsubject Varchar
,mailto Varchar
,mailbody text
,issent Varchar
,created_by_id Bigint
,created_at Timestamp(5)
,updated_by_id Bigint
,updated_at Timestamp(5)

                
                
                
                )

              AS $BODY$
                BEGIN
			   
			  /*This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:26*/
			  		 
            RETURN QUERY
			  SELECT  
				 MailLogs.id
,MailLogs.entityname
,MailLogs.entityid
,MailLogs.mailfor
,MailLogs.mailsubject
,MailLogs.mailto
,MailLogs.mailbody
,CAST(case when MailLogs.issent=true then 'Yes' else 'No' End AS Varchar) as issent

				 ,MailLogs.created_by_id,MailLogs.created_at,MailLogs.updated_by_id,MailLogs.updated_at
                 				 				 
                    
			  FROM  MailLogs 

			  WHERE CAST(MailLogs.id AS Varchar)=pvar_id ;
			  
					 	
			  END
              $BODY$
              LANGUAGE plpgsql;

