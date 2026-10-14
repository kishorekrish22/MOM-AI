 
			  
			  CREATE OR REPLACE FUNCTION  "getById_sp_users"
			  (
				  pvar_id Varchar
			  )
			  RETURNS TABLE(
                firstname Varchar
,lastname Varchar
,profilepicture Varchar
,username Varchar
,userpassword Varchar
,passwordkey Varchar
,emailid Varchar
,mobilenumber Varchar
,userrole Varchar
,created_by_id Bigint
,created_at Timestamp(5)
,updated_by_id Bigint
,updated_at Timestamp(5)
,tenantid Bigint

                ,id Bigint
                ,alloweddevices JSON
            )
            AS $BODY$
            BEGIN
			  /*This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:22*/
               
              RETURN QUERY
			  SELECT 
				 users.firstname
,users.lastname
,users.profilepicture
,users.username
,users.userpassword
,users.passwordkey
,users.emailid
,users.mobilenumber
,users.userrole

				 ,users.created_by_id,users.created_at,users.updated_by_id,users.updated_at
				 ,users.tenantid
                 ,users.id
                 ,(SELECT json_agg(J) FROM (
											 SELECT 
											 users_alloweddevices.id
                                             ,users_alloweddevices.usersid   
											 ,users_alloweddevices.devicename
,users_alloweddevices.deviceid
,users_alloweddevices.notificationid
 
											 FROM users_alloweddevices
											 WHERE 
											 users_alloweddevices.usersid=users.id
                                             AND COALESCE(users_alloweddevices.isdeleted,false) = false 
                                             ORDER BY record_order ASC
											) J) as alloweddevices
   
			  FROM users
			  WHERE CAST(users.id AS Varchar)=pvar_id
                       ;

					 	
			  END
              $BODY$
              LANGUAGE plpgsql;

