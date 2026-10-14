 
			  
			  CREATE OR REPLACE FUNCTION  "get_all_MailLogs"
              (
			    pvar_tenantid Varchar=null,
                pvar_searchterm character varying='',
                pvar_pagesize integer=50,
                pvar_pagenumber integer=0
              )
			  RETURNS TABLE(
                entityname Varchar
,entityid Varchar
,mailfor Varchar
,mailsubject Varchar
,mailto Varchar
,issent Boolean
,created_by_id Bigint
,created_at Timestamp(5)
,updated_by_id Bigint
,updated_at Timestamp(5)

                ,"id" Bigint
               )
               AS $BODY$
               BEGIN

			  /*This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:26*/

                if(pvar_searchterm is not null and LENGTH(CAST(pvar_searchterm as Varchar)) > 0)
                then
                pvar_searchterm := '%' || pvar_searchterm || '%';
                else
                pvar_searchterm := null;
                end if;

			 
                RETURN QUERY
                SELECT 
                MailLogs.entityname
,MailLogs.entityid
,MailLogs.mailfor
,MailLogs.mailsubject
,MailLogs.mailto
,COALESCE(MailLogs.issent,true) as issent

                ,MailLogs.created_by_id,MailLogs.created_at,MailLogs.updated_by_id,MailLogs.updated_at
                ,MailLogs.id
                FROM MailLogs
			    
                 WHERE MailLogs.isdeleted=false
                
                 AND (((pvar_searchterm is null) or COALESCE(MailLogs.id::varchar,'') ilike pvar_searchterm) OR ((pvar_searchterm is null) or COALESCE(MailLogs.entityname,'') ilike pvar_searchterm) OR ((pvar_searchterm is null) or COALESCE(MailLogs.entityid,'') ilike pvar_searchterm) OR ((pvar_searchterm is null) or COALESCE(MailLogs.mailfor,'') ilike pvar_searchterm) OR ((pvar_searchterm is null) or COALESCE(MailLogs.mailsubject,'') ilike pvar_searchterm) OR ((pvar_searchterm is null) or COALESCE(MailLogs.mailto,'') ilike pvar_searchterm) OR ((pvar_searchterm is null) or COALESCE(MailLogs.mailbody,'') ilike pvar_searchterm)) 
                limit pvar_pagesize offset pvar_pagenumber * pvar_pagesize;
			 

					 	
			  END
              $BODY$
              LANGUAGE plpgsql;

