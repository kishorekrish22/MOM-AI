CREATE OR REPLACE FUNCTION "getById_sp_users_alloweddevices"(
												 pvar_id Varchar(50)
											 )
                                             RETURNS TABLE("id" Bigint,"usersid" Bigint ,devicename Varchar
,deviceid Varchar
,notificationid Varchar
)
											 AS $BODY$
											 BEGIN
											 
											 RETURN QUERY
											 SELECT 
											 users_alloweddevices.id
                                             ,users_alloweddevices.usersid   
											 ,users_alloweddevices.devicename
,users_alloweddevices.deviceid
,users_alloweddevices.notificationid
 
											 FROM users_alloweddevices
											 WHERE 
											 CAST(users_alloweddevices.usersid AS VARCHAR)=pvar_id
                                             AND COALESCE(users_alloweddevices.isdeleted,false) = false   
                                             ORDER BY record_order ASC;
											
											 END
                                             $BODY$
                                             LANGUAGE plpgsql;

