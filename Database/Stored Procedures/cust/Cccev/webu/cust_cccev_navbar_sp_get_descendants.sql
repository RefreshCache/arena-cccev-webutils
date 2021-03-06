/****** Object:  StoredProcedure [dbo].[cust_cccev_navbar_sp_get_descendants]    Script Date: 03/03/2009 09:28:59 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[cust_cccev_navbar_sp_get_descendants]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [cust_cccev_navbar_sp_get_descendants]
GO


CREATE     Proc [dbo].[cust_cccev_navbar_sp_get_descendants]
@CurrentPageID int
,@PersonID int
AS
/**********************************************************************
* Description: Procedure to find heirachy for a given root page. Based
*              on "How to show expanding hierarchies by using SQL Server"
*              found here: http://support.microsoft.com/kb/248915
* Created By:   Nick Airdo @ Central Christian Church of the East Valley
* Date Created:	11/21/2007 08:25:00
*
* $Workfile: cust_cccev_navbar_sp_get_descendants.sql $
* $Revision: 1 $ 
* $Header: /trunk/Database/Stored Procedures/cust/Cccev/webu/cust_cccev_navbar_sp_get_descendants.sql   1   2009-03-03 09:44:34-07:00   nicka $
* 
* $Log: /trunk/Database/Stored Procedures/cust/Cccev/webu/cust_cccev_navbar_sp_get_descendants.sql $
*  
*  Revision: 1   Date: 2009-03-03 16:44:34Z   User: nicka 
**********************************************************************/
SET NOCOUNT ON

DECLARE @lvl int, @line char(20)
DECLARE @RootID int
SET @RootID = @CurrentPageID

DECLARE @AllPages TABLE (
	page_id int,
	parent_page_id int,
	page_name varchar(100),
	url varchar(100)
)

DECLARE @Stack TABLE (
	page_id int,
	lvl int
)

-- insert root page into stack as level 1
--INSERT INTO @AllPages VALUES (@CurrentPageID, NULL, 'root')
INSERT INTO @Stack VALUES (@CurrentPageID, 1)

SELECT @lvl = 1				
WHILE @lvl > 0					--From the top level going down.
	BEGIN
	    IF EXISTS (SELECT * FROM @Stack WHERE lvl = @lvl)
	        BEGIN
	            SELECT @CurrentPageID = page_id	--Find the first node that matches current node's id.
	            FROM @Stack
	            WHERE lvl = @lvl

	            SELECT @line = space(@lvl - 1) + @CurrentPageID	--@lvl - 1 s spaces before the node name.
	            PRINT @line					--Print it.

	            DELETE FROM @Stack
	            WHERE lvl = @lvl
	                AND page_id = @CurrentPageID	--Remove the current node from the stack.

	            INSERT @Stack		--Insert the childnodes of the current node into the stack.
					SELECT page_id, @lvl + 1
					FROM port_portal_page p
					WHERE (parent_page_id = @CurrentPageID) AND (display_in_nav = 1) AND
					EXISTS ( SELECT permission_id FROM secu_permission sp WHERE p.page_id = sp.object_key AND sp.object_type = 1 AND sp.operation_type = 0 AND sp.subject_type = 0 AND ( sp.subject_key = 1 OR sp.subject_key IN (SELECT role_id FROM secu_person_role WHERE person_id = @PersonID ) ) )
					ORDER BY page_order

				IF  @RootID = @CurrentPageID
					INSERT @AllPages		--Insert the ROOT childnodes of the current node into the stack.
						SELECT p.page_id, NULL, p.page_name, '~/default.aspx?page=' + CAST(p.page_id AS varchar)
						FROM port_portal_page p
						WHERE (parent_page_id = @CurrentPageID) AND (display_in_nav = 1) AND
						EXISTS ( SELECT permission_id FROM secu_permission sp WHERE p.page_id = sp.object_key AND sp.object_type = 1 AND sp.operation_type = 0 AND sp.subject_type = 0 AND ( sp.subject_key = 1 OR sp.subject_key IN (SELECT role_id FROM secu_person_role WHERE person_id = @PersonID ) ) )
						ORDER BY page_order

				ELSE
					INSERT @AllPages		--Insert the childnodes of the current node into the stack.
						SELECT p.page_id, p.parent_page_id, p.page_name, '~/default.aspx?page=' + CAST(p.page_id AS varchar)
						FROM port_portal_page p
						WHERE (parent_page_id = @CurrentPageID) AND (display_in_nav = 1) AND
						EXISTS ( SELECT permission_id FROM secu_permission sp WHERE p.page_id = sp.object_key AND sp.object_type = 1 AND sp.operation_type = 0 AND sp.subject_type = 0 AND ( sp.subject_key = 1 OR sp.subject_key IN (SELECT role_id FROM secu_person_role WHERE person_id = @PersonID ) ) )
						ORDER BY page_order

	            IF @@ROWCOUNT > 0		--If the previous statement added one or more nodes, go down for its first child.
                        SELECT @lvl = @lvl + 1	--If no nodes are added, check its brother nodes.
		END
    	    ELSE
	      	SELECT @lvl = @lvl - 1		--Back to the level immediately above.
       	
END

SELECT * FROM @AllPages