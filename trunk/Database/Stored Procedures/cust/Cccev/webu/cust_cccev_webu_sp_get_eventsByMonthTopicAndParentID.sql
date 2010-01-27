 /**********************************************************************
* Description:	TBD
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created:	TBD
*
* $Workfile: cust_cccev_webu_sp_get_eventsByMonthTopicAndParentID.sql $
* $Revision: 8 $ 
* $Header: /trunk/Database/Stored Procedures/cust/Cccev/webu/cust_cccev_webu_sp_get_eventsByMonthTopicAndParentID.sql   8   2009-06-03 09:07:52-07:00   JasonO $
* 
* $Log: /trunk/Database/Stored Procedures/cust/Cccev/webu/cust_cccev_webu_sp_get_eventsByMonthTopicAndParentID.sql $
*  
*  Revision: 8   Date: 2009-06-03 16:07:52Z   User: JasonO 
*  cleaning up formatting 
*  
*  Revision: 7   Date: 2009-06-03 01:03:51Z   User: JasonO 
*  Moving occurrence querying outside of common table expression so 
*  occurrences aren't required on "non-leaf" nodes. 
*  
*  Revision: 6   Date: 2009-03-19 01:20:23Z   User: JasonO 
*  
*  Revision: 5   Date: 2009-03-17 00:11:15Z   User: JasonO 
*  
*  Revision: 4   Date: 2009-03-11 20:01:45Z   User: JasonO 
*  
*  Revision: 3   Date: 2009-03-11 15:27:22Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-03-11 15:26:03Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-03-10 17:29:55Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-03-10 17:24:49Z   User: JasonO 
**********************************************************************/

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[cust_cccev_webu_sp_get_eventsByMonthTopicAndParentID]

@ParentID INT,
@StartDate DATETIME,
@EndDate DATETIME,
@TopicAreas VARCHAR(1000)

AS

WITH cte_profiles (profile_id, location_id, profile_name, profile_desc, approved_date, active, visibility_type_luid, topic_area_luid, display_order)
AS
(
	SELECT	e.profile_id,
			e.location_id,
			p.profile_name,
			p.profile_desc,
			e.approved_date,
			p.active,
			e.visibility_type_luid,
			e.topic_area_luid,
			p.display_order
	FROM evnt_event_profile e
	INNER JOIN core_profile p
		ON e.profile_id = p.profile_id
	WHERE p.parent_profile_id = @ParentID

	UNION ALL

	SELECT	e.profile_id,
			e.location_id,
			p.profile_name,
			p.profile_desc,
			e.approved_date,
			p.active,
			e.visibility_type_luid,
			e.topic_area_luid,
			p.display_order
	FROM evnt_event_profile e
	INNER JOIN core_profile p
		ON e.profile_id = p.profile_id
	INNER JOIN cte_profiles cte
		ON p.parent_profile_id = cte.profile_id
)

SELECT DISTINCT	
	cte.*,
	o.occurrence_id,
	o.occurrence_start_time AS start,
	o.occurrence_end_time AS [end]
FROM cte_profiles cte
LEFT OUTER JOIN core_profile_occurrence po
	ON cte.profile_id = po.profile_id
LEFT OUTER JOIN core_occurrence o
	ON po.occurrence_id = o.occurrence_id
WHERE cte.visibility_type_luid <> 763	-- Visibility Type: Private
AND (
		@TopicAreas = '' OR
		cte.topic_area_luid IN	(
									SELECT [item] 
									FROM [dbo].[fnSplit](@TopicAreas)
								)
	)
AND cte.active = 1
AND cte.approved_date <> '1/1/1900'
AND (
		o.occurrence_start_time BETWEEN @StartDate AND @EndDate
	)