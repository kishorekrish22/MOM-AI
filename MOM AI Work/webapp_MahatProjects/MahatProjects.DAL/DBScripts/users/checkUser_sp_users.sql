
				CREATE OR REPLACE FUNCTION  "checkUser_sp_users"
				(
                        pvar_username Varchar(256),
                        pvar_userpassword Varchar(256),
                        pvar_source Varchar(256),
                        pvar_devicename Varchar(256),
                        pvar_deviceid Varchar(256),
                        pvar_notificationid Varchar(256),
                        pvar_mobilenumber Varchar(256)
				)
                RETURNS TABLE(
                firstname Varchar
,lastname Varchar
,profilepicture Varchar
,username Varchar
,userpassword Varchar
,emailid Varchar
,mobilenumber Varchar
,userrole Varchar
,created_by_id Bigint
,created_at Timestamp(5)
,updated_by_id Bigint
,updated_at Timestamp(5)

                ,tenantid Bigint
                ,id Bigint
                ,tenantname Varchar(150)
                ,natureofbusiness character varying
                ,viewertenantids character varying
                )
				AS $BODY$
				DECLARE v_usersid Bigint;
                BEGIN  

                IF(pvar_source='Internal') THEN
		            RETURN QUERY
					SELECT 
					users.firstname
,users.lastname
,users.profilepicture
,users.username
,users.userpassword
,users.emailid
,users.mobilenumber
,users.userrole

					,users.created_by_id,users.created_at,users.updated_by_id,users.updated_at
					,0::bigint as tenantid
                    ,users.id
                    ,''::varchar as tenantname
                    ,''::varchar as natureofbusiness
                    ,coalesce(users.viewertenantids,'') as viewertenantids
					FROM users
					WHERE 
                    (upper(users.username)= upper(pvar_username)
		            OR  users.mobilenumber=pvar_username
                    OR users.emailid=pvar_username)
                    AND users.userpassword=pvar_userpassword
					 AND users.isdeleted=false;
                ELSE IF(pvar_source='allowbyid') THEN
                     RETURN QUERY
					SELECT 
					users.firstname
,users.lastname
,users.profilepicture
,users.username
,users.userpassword
,users.emailid
,users.mobilenumber
,users.userrole

					,users.created_by_id,users.created_at,users.updated_by_id,users.updated_at
					,0::bigint as tenantid
                    ,users.id
                    ,''::varchar as tenantname
                    ,''::varchar as natureofbusiness
                    ,coalesce(users.viewertenantids,'') as viewertenantids
                     
					FROM users
					WHERE  CAST(users.id AS Varchar)=pvar_username 
                      AND users.isdeleted=false;
                ELSE


                             SELECT users.id 
                                    INTO v_usersid 
                                    FROM   users
                                    WHERE  (upper(users.username)= upper(pvar_username)
                                    OR     users.mobilenumber=pvar_username
                                    OR     users.emailid=pvar_username) LIMIT 1; 

                         IF(pvar_mobilenumber IS NOT NULL) THEN
                            SELECT 
					        users.id INTO v_usersid 
					        FROM users
					        WHERE 
                            (upper(users.username)= upper(pvar_username)
		                    OR  users.mobilenumber=pvar_username
                            OR  users.mobilenumber=pvar_mobilenumber
                            OR users.emailid=pvar_username)
                            --AND users.userpassword=pvar_userpassword
					         AND users.isdeleted=false;
                        END IF;
             

                        IF NOT EXISTS
                            (SELECT *
                            FROM   users_alloweddevices
                            WHERE  deviceid=pvar_deviceid OR notificationid=pvar_notificationid) THEN

                               
                                    INSERT INTO users_alloweddevices 
                                    (
										usersid,
										devicename,
										deviceid,notificationid
                                            
									)
                                    VALUES
                                    (
                                    
                                    v_usersid,
                                    pvar_devicename,
                                    pvar_deviceid,pvar_notificationid
                                    );
                        END IF;
                                RETURN QUERY 	
                                    SELECT 
					                users.firstname
,users.lastname
,users.profilepicture
,users.username
,users.userpassword
,users.emailid
,users.mobilenumber
,users.userrole

					                ,users.created_by_id,users.created_at,users.updated_by_id,users.updated_at
					                ,0::bigint as tenantid
                                    ,users.id
					                 ,''::varchar as tenantname
                                    ,''::varchar as natureofbusiness
                                    ,coalesce(users.viewertenantids,'') as viewertenantids
					                FROM users
					                 WHERE  users.id=v_usersid 
                                 AND users.isdeleted=false;

                        END IF;

                END IF;
                 




					
				END
                $BODY$
                LANGUAGE plpgsql;
CREATE OR REPLACE FUNCTION "checkoauthauthentication_sp_users"(
pvar_userdata json)
    RETURNS TABLE(firstname character varying, lastname character varying, profilepicture character varying, username character varying, userpassword character varying, emailid character varying, mobilenumber character varying, userrole character varying, createduser uuid, createddate timestamp without time zone, modifieduser uuid, modifieddate timestamp without time zone, tenantid uuid,tenantname character varying, natureofbusiness character varying,viewertenantids character varying, usersid uuid) 


AS $BODY$

BEGIN  

IF (pvar_userdata->>'source' = 'AzureADJwtBearer' OR pvar_userdata->>'source' = 'GoogleOpenIdConnect' OR pvar_userdata->>'source' = 'oidc') THEN
 
	IF NOT EXISTS(SELECT 1 FROM users WHERE UPPER(users.username) = UPPER(pvar_userdata->>'first_name') and users.emailid = pvar_userdata->>'email'
	AND users.isdeleted = false
	)
		THEN -- Insert new user if not found
		INSERT INTO users(firstname, lastname, profilepicture, username, userpassword, emailid, mobilenumber, userrole, passwordkey, createduser, usersid, tenantid) 
		VALUES (
                pvar_userdata->>'first_name', '', NULL, pvar_userdata->>'first_name', 
                'xxxxx', pvar_userdata->>'email', 'xxxxxxxxxx', pvar_userdata->>'role', 
                NULL, NULL, gen_random_uuid(), NULL
            );
	
	END IF;
	-- Return the user details after insertion or if the user already exists
	RETURN QUERY
		SELECT 
		users.firstname,
		users.lastname,
		users.profilepicture,
		users.username,
		users.userpassword,
		users.emailid,
		users.mobilenumber,
		users.userrole,
		users.createduser,
		users.createddate,
		users.modifieduser,
		users.modifieddate,
		COALESCE(users.tenantid, '00000000-0000-0000-0000-000000000000') AS tenantid,
coalesce((SELECT tenant.businessname from tenant where tenant.tenantid=users.tenantid),'') as tenantname,
coalesce((SELECT tenant.natureofbusiness from tenant where tenant.tenantid=users.tenantid),'') as natureofbusiness,
coalesce(users.viewertenantids,'') as viewertenantids,
		users.usersid
		FROM users
		WHERE UPPER(users.username) = UPPER(pvar_userdata->>'first_name')
		and users.emailid = pvar_userdata->>'email'
		and users.userrole = pvar_userdata->>'role'
		AND users.isdeleted = false;
	
END IF;

END

$BODY$
LANGUAGE plpgsql;


