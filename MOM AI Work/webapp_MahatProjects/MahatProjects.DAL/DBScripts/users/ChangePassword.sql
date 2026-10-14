  
                    CREATE OR REPLACE FUNCTION "ChangePassword"
                    (
                    pvar_usersid Varchar(50)
                    ,pvar_userpassword Varchar(128)  
                    ,pvar_passwordkey Varchar(150)  
                    ,pvar_updated_by_id  Bigint 
                    ,OUT pvar_returnMessage Varchar(4000) 
                    )  
                    RETURNS Varchar(4000) 
                    AS $BODY$  
                    BEGIN
                    /*This code generated from staging Powered by Mahat, Source Machine : stg , Build Number : #2024-07-004 (Updated on 07/07/2024 22:07) on 12/26/2024 10:27:22*/

 
                            UPDATE users SET
                            userpassword = pvar_userpassword
                            ,passwordkey = pvar_passwordkey
                            ,updated_by_id=pvar_updated_by_id
                            WHERE id = pvar_usersid;

                            pvar_returnMessage := '201.1';


                               /* EXCEPTION WHEN OTHERS THEN
			 
                                INSERT INTO system_logging
                                (
                                Log_code
                                
                                ,log_application
                                ,log_date
                                ,log_level
                                ,log_logger
                                ,log_message
                                )
                                VALUES
                                ('16'
                                
                                ,'Postgre Function Exception'
                                ,NOW()
                                ,'16'
                                ,'{ActionMethodName}'
                                ,'update failed'
                                );*/
			  
                    END
                    $BODY$
                    LANGUAGE plpgsql;
                    

