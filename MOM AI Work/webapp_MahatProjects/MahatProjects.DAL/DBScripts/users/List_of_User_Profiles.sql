
			  CREATE OR REPLACE FUNCTION  "List_of_User_Profiles"
              (pvar_tenantid Varchar
)
			  RETURNS TABLE(id Bigint
,firstname Varchar,lastname Varchar,profilepicture Varchar,username Varchar,userpassword Varchar,passwordkey Varchar,emailid Varchar,mobilenumber Varchar,userrole Varchar,"automaton_users_alloweddevices" json,created_by_id Bigint
,created_at Timestamp(5)
,updated_by_id Bigint
,updated_at Timestamp(5)
)
			  AS $BODY$
              
              
			  BEGIN
			  /*This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:22*/
			  		
              
                RETURN QUERY
			  SELECT  
				 users.id
,users.firstname
,users.lastname
,users.profilepicture
,users.username
,users.userpassword
,users.passwordkey
,users.emailid
,users.mobilenumber
,users.userrole

				 ,
                (SELECT json_agg(J) FROM (SELECT   
				 users_alloweddevices.devicename as "Device Name"
,users_alloweddevices.deviceid as "Device ID"
,users_alloweddevices.notificationid as "Notification ID"

		 	   FROM  users_alloweddevices 

			  WHERE users.id =users_alloweddevices.usersid
AND COALESCE(users_alloweddevices.isdeleted,false) = false 
) J)
			    as automaton_users_alloweddevices

				 ,users.created_by_id,users.created_at,users.updated_by_id,users.updated_at
			  FROM  users 

			  
                ORDER BY users.created_at DESC;
			  
					 	
			  END
              $BODY$
              LANGUAGE plpgsql;

