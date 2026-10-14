 
			  
			  CREATE OR REPLACE FUNCTION  "getById_sp_Project"
			  (
				  pvar_id Varchar
			  )
			  RETURNS TABLE(
                project_name Varchar
,description Varchar
,handled_by Varchar
,interaction_date date
,video_upload Varchar
,transcript Varchar
,created_by_id Bigint
,created_at Timestamp(5)
,updated_by_id Bigint
,updated_at Timestamp(5)

                ,id Bigint
                
            )
            AS $BODY$
            BEGIN
			  /*This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:31:56*/
               
              RETURN QUERY
			  SELECT 
				 Project.project_name
,Project.description
,Project.handled_by
,Project.interaction_date
,Project.video_upload
,Project.transcript

				 ,Project.created_by_id,Project.created_at,Project.updated_by_id,Project.updated_at
				 
                 ,Project.id
                    
			  FROM Project
			  WHERE CAST(Project.id AS Varchar)=pvar_id
                       ;

					 	
			  END
              $BODY$
              LANGUAGE plpgsql;

