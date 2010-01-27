
/****** Object:  StoredProcedure [dbo].[cust_cccev_breadcrumb_sp_get_parents]    Script Date: 11/21/2007 08:28:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cust_cccev_breadcrumb_sp_get_parents]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cust_cccev_breadcrumb_sp_get_parents]
GO

/****** Object:  StoredProcedure [dbo].[cust_cccev_breadcrumb_sp_get_parents]    Script Date: 11/21/2007 08:51:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE     Proc [dbo].[cust_cccev_breadcrumb_sp_get_parents]
@CurrentPageID int,
@StopParentPageID int
AS
/**********************************************************************
* Description: Procedure to find parent heirachy for a given child page.
* Created By:   Nick Airdo @ Central Christian Church of the East Valley
* Date Created:	11/21/2007 08:25:00
*
* $Workfile: breadcrumbSproc.sql $
* $Revision: 2 $ 
* $Header: /Arena/Arena.Custom.Cccev.WebUtils/SQL/breadcrumbSproc.sql   2   2007-12-18 14:49:27-07:00   nicka $
* 
* $Log: /Arena/Arena.Custom.Cccev.WebUtils/SQL/breadcrumbSproc.sql $
*  
*  Revision: 2   Date: 2007-12-18 21:49:27Z   User: nicka 
*  only select pages that are set to 'Display in nav' 
*  
*  Revision: 1   Date: 2007-12-18 21:44:57Z   User: nicka 
*  first version 
**********************************************************************/
SET NOCOUNT ON

declare @ParentPageID int
set @ParentPageID = @CurrentPageID

DECLARE @Breadcrumbs TABLE (
page_id int,
page_name varchar(100) )

	WHILE @ParentPageID IS NOT NULL AND @ParentPageID <> @StopParentPageID AND @ParentPageID <> -1 
	BEGIN
		INSERT INTO @Breadcrumbs (page_id, page_name)
		SELECT page_id, page_name FROM port_portal_page WHERE page_id = @ParentPageID AND display_in_nav = 1

		SET @ParentPageID = (SELECT parent_page_id FROM port_portal_page
		WHERE page_id = @ParentPageID)
	END

SELECT * FROM @Breadcrumbs