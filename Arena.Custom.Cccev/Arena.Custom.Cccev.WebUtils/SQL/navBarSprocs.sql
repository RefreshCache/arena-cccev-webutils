/****** Object:  StoredProcedure [dbo].[cust_cccev_navbar_sp_get_descendants]    Script Date: 11/21/2007 08:28:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cust_cccev_navbar_sp_get_descendants]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cust_cccev_navbar_sp_get_descendants]
GO

/****** Object:  StoredProcedure [dbo].[cust_cccev_navbar_sp_get_descendants]    Script Date: 11/21/2007 08:51:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE     Proc [dbo].[cust_cccev_navbar_sp_get_descendants]
@CurrentPageID int
AS
/**********************************************************************
* Description: Procedure to find heirachy for a given root page. Based
*              on "How to show expanding hierarchies by using SQL Server"
*              found here: http://support.microsoft.com/kb/248915
* Created By:   Nick Airdo @ Central Christian Church of the East Valley
* Date Created:	11/21/2007 08:25:00
*
* $Workfile: navBarSprocs.sql $
* $Revision: 1 $ 
* $Header: /Arena/Arena.Custom.Cccev.WebUtils/SQL/navBarSprocs.sql   1   2007-12-19 10:19:52-07:00   nicka $
* 
* $Log: /Arena/Arena.Custom.Cccev.WebUtils/SQL/navBarSprocs.sql $
*  
*  Revision: 1   Date: 2007-12-19 17:19:52Z   User: nicka 
*  first semi-working draft w/o security 
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
	                FROM port_portal_page
	                WHERE parent_page_id = @CurrentPageID
					AND display_in_nav = 1
					ORDER BY page_order

				IF  @RootID = @CurrentPageID
					INSERT @AllPages		--Insert the ROOT childnodes of the current node into the stack.
						SELECT page_id, NULL, page_name, '~/default.aspx?page=' + CAST(page_id AS varchar)
						FROM port_portal_page
						WHERE parent_page_id = @CurrentPageID
						AND display_in_nav = 1
						ORDER BY page_order
				ELSE
					INSERT @AllPages		--Insert the childnodes of the current node into the stack.
						SELECT page_id, parent_page_id, page_name, '~/default.aspx?page=' + CAST(page_id AS varchar)
						FROM port_portal_page
						WHERE parent_page_id = @CurrentPageID
						AND display_in_nav = 1
						ORDER BY page_order

	            IF @@ROWCOUNT > 0		--If the previous statement added one or more nodes, go down for its first child.
                        SELECT @lvl = @lvl + 1	--If no nodes are added, check its brother nodes.
		END
    	    ELSE
	      	SELECT @lvl = @lvl - 1		--Back to the level immediately above.
       	
END

SELECT * FROM @AllPages