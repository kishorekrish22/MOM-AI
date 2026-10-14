CREATE OR REPLACE FUNCTION "MailSender"(
	pvar_mailfor character varying,
	pvar_entityid character varying,
	pvar_created_by_id character varying)
    RETURNS TABLE(toname character varying, username character varying, userpassword character varying, receiveremail character varying, mobilenumber character varying, usersid Bigint, userrole character varying) 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$
BEGIN
   IF(pvar_mailfor = 'forgotpassword') 
THEN
RETURN QUERY SELECT
      CAST(users.firstname || ' ' || COALESCE(users.lastname, '') AS Varchar) as toname,
      users.username,
      users.userpassword,
      COALESCE(users.emailid, '') as receiveremail,
      users.mobilenumber,
      users.id as usersid, 
      users.userrole
	  FROM
         users 
      WHERE
         CAST (users.id AS character varying) = pvar_entityid;

END
IF;
END;
$BODY$;


