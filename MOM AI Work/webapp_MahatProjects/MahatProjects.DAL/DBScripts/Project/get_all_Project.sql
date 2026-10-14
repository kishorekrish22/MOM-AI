 
			  
			  CREATE OR REPLACE FUNCTION  "get_all_Project"
              (
			    pvar_tenantid Varchar=null,
                pvar_searchterm character varying='',
                pvar_pagesize integer=50,
                pvar_pagenumber integer=0
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

                ,"id" Bigint
               )
               AS $BODY$
               BEGIN

			  /*This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:31:56*/

                if(pvar_searchterm is not null and LENGTH(CAST(pvar_searchterm as Varchar)) > 0)
                then
                pvar_searchterm := '%' || pvar_searchterm || '%';
                else
                pvar_searchterm := null;
                end if;

			 
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
			    
                 WHERE Project.isdeleted=false
                
                 AND (((pvar_searchterm is null) or COALESCE(Project.id::varchar,'') ilike pvar_searchterm) OR ((pvar_searchterm is null) or COALESCE(Project.project_name,'') ilike pvar_searchterm) OR ((pvar_searchterm is null) or COALESCE(Project.description,'') ilike pvar_searchterm) OR ((pvar_searchterm is null) or COALESCE(Project.handled_by,'') ilike pvar_searchterm) OR ((pvar_searchterm is null) or COALESCE(Project.transcript,'') ilike pvar_searchterm)) 
                limit pvar_pagesize offset pvar_pagenumber * pvar_pagesize;
			 

					 	
			  END
              $BODY$
              LANGUAGE plpgsql;

