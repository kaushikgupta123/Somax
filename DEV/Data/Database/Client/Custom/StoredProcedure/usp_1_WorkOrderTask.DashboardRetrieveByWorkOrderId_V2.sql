IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[usp_WorkOrderTask_DashboardRetrieveByWorkOrderId_V2]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[usp_WorkOrderTask_DashboardRetrieveByWorkOrderId_V2] 
GO

CREATE PROCEDURE [dbo].[usp_WorkOrderTask_DashboardRetrieveByWorkOrderId_V2]  
	 @CallerUserInfoId bigint,  
	@CallerUserName nvarchar(256) = NULL,  
	@ClientId bigint,  
	@WorkOrderId bigint,
	@orderbyColumn varchar(100) = '1',
	@orderBy varchar(10) = 'ASC',
	@offset1 int = 0,
	@nextrow int = 10 
	 
AS  
SET ANSI_NULLS ON   
SET NOCOUNT ON  
BEGIN  
  DECLARE @err integer  
  DECLARE @count integer  
     Declare @WorkOrderTaskTemp TABLE
  ( 
		[ClientId] [bigint] NOT NULL, 
		[WorkOrderTaskId] [bigint] NOT NULL, 
		[WorkOrderId] [bigint] NOT NULL, 
		[Description][nvarchar](max)NOT NULL,
		[Status][nvarchar](15)NOT NULL,
		[TaskNumber][nvarchar](7)NOT NULL,	
		[UpdateIndex][int]NOT NULL,	 	
		[ChargeToClientLookupId] [nvarchar](15) NULL	  
  )

   INSERT INTO @WorkOrderTaskTemp
  SELECT   
      [WorkOrderTask].[ClientId],  
      [WorkOrderTask].[WorkOrderTaskId],  
      [WorkOrderTask].[WorkOrderId],    
		[WorkOrderTask].[Description],  
      [WorkOrderTask].[Status],  
      [WorkOrderTask].[TaskNumber], 
    [WorkOrderTask].[UpdateIndex], 
     (SELECT  
      (CASE   
       WHEN [WorkOrderTask].[ChargeType] = 'Equipment' THEN   
         (SELECT ClientLookupId   
           FROM   [dbo].[Equipment] WITH (NOLOCK)  
              WHERE  [EquipmentId] = [WorkOrderTask].[ChargeToId]  
              AND    [ClientId] = @ClientId)  
       WHEN [WorkOrderTask].[ChargeType] = 'Location' THEN  
        (SELECT ClientLookupId  
          FROM   [dbo].[Location] WITH (NOLOCK)  
          WHERE  [LocationId] = [WorkOrderTask].[ChargeToId]  
          AND    [ClientId] = @ClientId)  
       WHEN [WorkOrderTask].[ChargeType] = 'Account' THEN  
        (SELECT ClientLookupId  
          FROM   [dbo].[Account] WITH (NOLOCK)  
          WHERE  [AccountId] = [WorkOrderTask].[ChargeToId]  
          AND    [ClientId] = @ClientId)  

       ELSE ''  
        END)) AS ChargeToClientLookupId
    
  
  FROM [dbo].[WorkOrderTask] WITH (NOLOCK)  
  WHERE ([WorkOrderTask].[ClientId] = @ClientId)  
  AND ([WorkOrderTask].[WorkOrderId] = @WorkOrderId)  
  ORDER BY [WorkOrderTask].[TaskNumber]   
  

     SELECT *,COUNT(X.ClientId) OVER() TotalCount 
INTO #T2 
FROM 
 (SELECT
      [WorkOrderTask].[ClientId],  
      [WorkOrderTask].[WorkOrderTaskId],  
      [WorkOrderTask].[WorkOrderId],     
      [WorkOrderTask].[Description],  
      [WorkOrderTask].[Status],  
      [WorkOrderTask].[TaskNumber],  
   [WorkOrderTask].[UpdateIndex],  
	  [WorkOrderTask].[ChargeToClientLookupId]
	FROM @WorkOrderTaskTemp WorkOrderTask) AS X
	Where (@ClientId IS NULL OR @ClientId = 0 or([ClientId] =@ClientId))
	  Order by	WorkOrderTaskId

	  SELECT * 
	FROM #T2 
	ORDER BY 
	CASE WHEN @orderbyColumn = '1' AND @orderBy = 'asc' THEN [TaskNumber] END ASC,
	CASE WHEN @orderbyColumn = '2' AND @orderBy = 'asc' THEN [Description] END ASC,
    CASE WHEN @orderbyColumn = '3' AND @orderBy = 'asc' THEN [Status] END ASC,
	CASE WHEN @orderbyColumn = '4' AND @orderBy = 'asc' THEN [ChargeToClientLookupId] END ASC,
	CASE WHEN @orderbyColumn = '1' AND @orderBy = 'desc' THEN [TaskNumber] END DESC,
	CASE WHEN @orderbyColumn = '2' AND @orderBy = 'desc' THEN [Description] END DESC,
    CASE WHEN @orderbyColumn = '3' AND @orderBy = 'desc' THEN [Status]  END DESC,
	CASE WHEN @orderbyColumn = '4' AND @orderBy = 'desc' THEN [ChargeToClientLookupId]  END DESC

	OFFSET @offset1 ROWS 
	FETCH NEXT @nextrow ROWS ONLY
	 
   IF OBJECT_ID('tempdb..#T2')  IS NOT NULL DROP TABLE #T2
   
  --  
  -- Handle error  
  --  
  SELECT @err = @@ERROR, @count=@@ROWCOUNT  
  IF (@err <> 0)  
  BEGIN  
    RETURN @err  
  END  
  
  --  
  -- Return to caller  
  --  
  RETURN 0  
END  
  
SET NOCOUNT OFF  
SET QUOTED_IDENTIFIER OFF   
SET ANSI_NULLS ON
GO


