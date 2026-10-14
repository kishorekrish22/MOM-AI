CREATE OR REPLACE FUNCTION "Check_Authorization"(
	pvar_user_id Bigint,
	pvar_controller_name character varying,
	pvar_view_name character varying)
    RETURNS boolean
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
DECLARE
    lv_view_action_roles varchar(128);
BEGIN
     /*This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:22*/

    SELECT  userrole INTO lv_view_action_roles from users where CAST(id AS VARCHAR)=CAST(pvar_user_id AS VARCHAR) OR CAST(id AS VARCHAR)=CAST(pvar_user_id AS VARCHAR);
    RETURN EXISTS (
        SELECT 1
        FROM RoleAuthorization
        WHERE regexp_split_to_array(lower(viewactionroles), ',') @> regexp_split_to_array(lower(lv_view_action_roles), ',')
        AND controllername = pvar_controller_name
        AND viewname = pvar_view_name
    );
END
$BODY$;

