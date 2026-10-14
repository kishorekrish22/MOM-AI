
			  CREATE OR REPLACE FUNCTION  "getById_sp_all_AlertTemplates"
              (
			  pvar_id Varchar
			  )
              RETURNS TABLE(
                "id" Bigint
,entityname Varchar
,entityaction Varchar
,sendasbatch Varchar
,alerttype Varchar
,alertsubject Varchar
,alertcopyto Varchar
,alertcarboncopyto Varchar
,alertcontent text
,created_by_id Bigint
,created_at Timestamp(5)
,updated_by_id Bigint
,updated_at Timestamp(5)

                
                
                
                )

              AS $BODY$
                BEGIN
			   
			  /*This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:25*/
			  		 
            RETURN QUERY
			  SELECT  
				 AlertTemplates.id
,AlertTemplates.entityname
,AlertTemplates.entityaction
,CAST(case when AlertTemplates.sendasbatch=true then 'Yes' else 'No' End AS Varchar) as sendasbatch
,AlertTemplates.alerttype
,AlertTemplates.alertsubject
,AlertTemplates.alertcopyto
,AlertTemplates.alertcarboncopyto
,AlertTemplates.alertcontent

				 ,AlertTemplates.created_by_id,AlertTemplates.created_at,AlertTemplates.updated_by_id,AlertTemplates.updated_at
                 				 				 
                    
			  FROM  AlertTemplates 

			  WHERE CAST(AlertTemplates.id AS Varchar)=pvar_id ;
			  
					 	
			  END
              $BODY$
              LANGUAGE plpgsql;

