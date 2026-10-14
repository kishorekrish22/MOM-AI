
			  CREATE OR REPLACE FUNCTION  "Project_List"
              (pvar_pagesize integer
,pvar_pagenumber integer
,pvar_searchterm varchar
,pvar_sort_fields json



                )
			  RETURNS json
			  AS $BODY$
               declare local_sortcolumn_array text[] = (
	                select array_agg(col) from json_to_recordset(pvar_sort_fields) as x(col text, dir text)
                );
                declare local_sortorder_array text[] = (
	                select array_agg(dir) from json_to_recordset(pvar_sort_fields) as x(col text, dir text)	
                );          
                
               
          	  BEGIN
			  /*This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:31:56*/
			  		

                    if(pvar_searchterm is not null and LENGTH(CAST(pvar_searchterm as Varchar)) > 0)
                    then
                    pvar_searchterm := '%' || pvar_searchterm || '%';
                    else
                    pvar_searchterm := null;
                    end if;
              
                    RETURN json_build_object(
                    'count'
                    ,(SELECT  
                    COUNT(*)
                    FROM  Project 

                    WHERE Project.isdeleted=false 
 AND ( ((pvar_searchterm is null) or CAST(Project.project_name AS VARCHAR) ilike pvar_searchterm)
 OR ((pvar_searchterm is null) or CAST(Project.description AS VARCHAR) ilike pvar_searchterm)
 OR ((pvar_searchterm is null) or CAST(Project.handled_by AS VARCHAR) ilike pvar_searchterm)
 OR ((pvar_searchterm is null) or CAST(COALESCE(to_char(Project.interaction_date,'dd/MM/yyyy'),'') AS Varchar) ilike pvar_searchterm)
 OR ((pvar_searchterm is null) or CAST(Project.video_upload AS VARCHAR) ilike pvar_searchterm)
 OR ((pvar_searchterm is null) or CAST(Project.transcript AS VARCHAR) ilike pvar_searchterm)
))                   
                    ,'detail'
                    ,(SELECT json_agg(row_to_json(d)) FROM (
                    SELECT  
                    Project.id
,Project.project_name
,Project.description
,Project.handled_by
,CAST(COALESCE(to_char(Project.interaction_date,'dd/MM/yyyy'),'') AS Varchar) as interaction_date
,Project.video_upload
,Project.transcript

                    
                    ,Project.created_by_id,Project.created_at,Project.updated_by_id,Project.updated_at
                    FROM  Project 

                    WHERE Project.isdeleted=false 

                     AND ( ((pvar_searchterm is null) or CAST(Project.project_name AS VARCHAR) ilike pvar_searchterm)
 OR ((pvar_searchterm is null) or CAST(Project.description AS VARCHAR) ilike pvar_searchterm)
 OR ((pvar_searchterm is null) or CAST(Project.handled_by AS VARCHAR) ilike pvar_searchterm)
 OR ((pvar_searchterm is null) or CAST(COALESCE(to_char(Project.interaction_date,'dd/MM/yyyy'),'') AS Varchar) ilike pvar_searchterm)
 OR ((pvar_searchterm is null) or CAST(Project.video_upload AS VARCHAR) ilike pvar_searchterm)
 OR ((pvar_searchterm is null) or CAST(Project.transcript AS VARCHAR) ilike pvar_searchterm)
) 
                    ORDER BY 
                    CASE WHEN local_sortorder_array[1] = 'asc' THEN
                    CASE local_sortcolumn_array[1]
                      WHEN 'project_name' THEN Project.project_name::TEXT
WHEN 'description' THEN Project.description::TEXT
WHEN 'handled_by' THEN Project.handled_by::TEXT
WHEN 'video_upload' THEN Project.video_upload::TEXT
WHEN 'video_upload' THEN Project.video_upload::TEXT
WHEN 'transcript' THEN Project.transcript::TEXT
		
                    ELSE NULL
                    END
                    ELSE
                    NULL
                    END ASC
,
                    CASE WHEN local_sortorder_array[1] = 'asc' THEN
                    CASE local_sortcolumn_array[1]
                      WHEN 'interaction_date' THEN Project.interaction_date
		
                    ELSE NULL
                    END
                    ELSE
                    NULL
                    END ASC,
                    CASE WHEN local_sortorder_array[1] = 'desc' THEN
                    CASE local_sortcolumn_array[1]
                      WHEN 'project_name' THEN Project.project_name::TEXT
WHEN 'description' THEN Project.description::TEXT
WHEN 'handled_by' THEN Project.handled_by::TEXT
WHEN 'video_upload' THEN Project.video_upload::TEXT
WHEN 'video_upload' THEN Project.video_upload::TEXT
WHEN 'transcript' THEN Project.transcript::TEXT
			
                    ELSE NULL
                    END
                    ELSE
                    NULL
                    END DESC
,
                    CASE WHEN local_sortorder_array[1] = 'desc' THEN
                    CASE local_sortcolumn_array[1]
                      WHEN 'interaction_date' THEN Project.interaction_date
			
                    ELSE NULL
                    END
                    ELSE
                    NULL
                    END DESC

                    limit pvar_pagesize
                    offset pvar_pagenumber * pvar_pagesize			 	
			 	
                    ) d));
	
			  
					 	
			  END
              $BODY$
              LANGUAGE plpgsql;

