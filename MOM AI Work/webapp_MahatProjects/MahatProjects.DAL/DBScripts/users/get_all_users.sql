 
			  
			  CREATE OR REPLACE FUNCTION  "get_all_users"
              (
			  pvar_tenantid Varchar=null
              )
			 RETURNS TABLE(
                firstname Varchar
,lastname Varchar
,profilepicture Varchar
,username Varchar
,emailid Varchar
,mobilenumber Varchar
,userrole Varchar
,created_by_id Bigint
,created_at Timestamp(5)
,updated_by_id Bigint
,updated_at Timestamp(5)
,tenantid Bigint

                ,"id" Bigint
                 
               )
               AS $BODY$
                declare lvar_tenantid varchar[];
                declare lstr_usersid varchar;
               BEGIN
              /*This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:22*/
              
                SELECT  SPLIT_PART(pvar_tenantid, '|', 1),SPLIT_PART(pvar_tenantid, '|', 2) into lstr_usersid,pvar_tenantid;
		        
                if(pvar_tenantid is null or pvar_tenantid='' or pvar_tenantid='0')	
				then
                    SELECT STRING_TO_ARRAY(viewertenantids, ',') into lvar_tenantid
				    FROM users where users.id::varchar=lstr_usersid;	
                    if(lvar_tenantid is NULL)
					then 
						SELECT array_agg(tenant.id) INTO lvar_tenantid FROM tenant;
               
					end if;
                else 
				  lvar_tenantid=ARRAY[pvar_tenantid];
                end if;
                lvar_tenantid := lvar_tenantid || ARRAY[''::character varying] || ARRAY['0'::character varying];
				
			    
                RETURN QUERY
			  SELECT 

				 users.firstname
,users.lastname
,users.profilepicture
,users.username
,users.emailid
,users.mobilenumber
,users.userrole

				 ,users.created_by_id,users.created_at,users.updated_by_id,users.updated_at
				,users.tenantid 
                ,users.id
				 
			  FROM users
			  WHERE
               (lvar_tenantid is null or COALESCE(cast(users.tenantid as varchar),'') = Any(lvar_tenantid))
                 
			  
			  ;
			 

					 	
			  END
              $BODY$
              LANGUAGE plpgsql;

