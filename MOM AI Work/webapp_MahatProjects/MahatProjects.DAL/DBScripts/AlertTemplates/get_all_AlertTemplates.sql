 
			  
			  CREATE OR REPLACE FUNCTION  "get_all_AlertTemplates"
              (
			  pvar_tenantid Varchar=null
              )
			  RETURNS TABLE(
                entityname Varchar
,entityaction Varchar
,sendasbatch Boolean
,alerttype Varchar
,alertsubject Varchar
,alertcopyto Varchar
,alertcarboncopyto Varchar
,alertcontent text
,created_by_id Bigint
,created_at Timestamp(5)
,updated_by_id Bigint
,updated_at Timestamp(5)

                ,"id" Bigint
               )
               AS $BODY$
               BEGIN

			  /*This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:25*/

			 
                RETURN QUERY
                SELECT 
                AlertTemplates.entityname
,AlertTemplates.entityaction
,COALESCE(AlertTemplates.sendasbatch,true) as sendasbatch
,AlertTemplates.alerttype
,AlertTemplates.alertsubject
,AlertTemplates.alertcopyto
,AlertTemplates.alertcarboncopyto
,AlertTemplates.alertcontent

                ,AlertTemplates.created_by_id,AlertTemplates.created_at,AlertTemplates.updated_by_id,AlertTemplates.updated_at
                ,AlertTemplates.id
                FROM AlertTemplates
			    
                 WHERE AlertTemplates.isdeleted=false
                ;
			 

					 	
			  END
              $BODY$
              LANGUAGE plpgsql;

