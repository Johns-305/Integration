if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddQuery]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddQuery]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddQueryColumn]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddQueryColumn]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddQueryCondition]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddQueryCondition]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddQuerySortOrder]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddQuerySortOrder]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Allthing]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Allthing]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CustOrderHist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CustOrderHist]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CustOrdersDetail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CustOrdersDetail]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CustOrdersOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CustOrdersOrders]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteQuery]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteQuery]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteQueryColumns]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteQueryColumns]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteQueryConditions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteQueryConditions]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteQuerySortOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteQuerySortOrders]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EmployeeSalesbyCountry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EmployeeSalesbyCountry]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetQueryByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetQueryByID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ListAllQuery]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ListAllQuery]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ListByRawSQL]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ListByRawSQL]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ListQuerysByDataSource]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ListQuerysByDataSource]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PassBinary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PassBinary]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PassImage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PassImage]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PassVarBin]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PassVarBin]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_SelectCustomers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_SelectCustomers]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sales by Year]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sales by Year]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SalesByCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SalesByCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Ten Most Expensive Products]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Ten Most Expensive Products]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateQuery]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateQuery]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_InsertDynamicStateInfo_BizTalkServerApplication]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[bts_InsertDynamicStateInfo_BizTalkServerApplication]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AllThings]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[AllThings]
GO

CREATE TABLE [dbo].[AllThings] (
	[bigint] [bigint] NULL ,
	[binary] [binary] (50) NULL ,
	[bitf] [bit] NULL ,
	[char] [char] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[dt] [datetime] NULL ,
	[dec] [decimal](18, 0) NULL ,
	[flt] [float] NULL ,
	[img] [image] NULL ,
	[in] [int] NULL ,
	[mnry] [money] NULL ,
	[nchar] [nchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ntext] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[num] [numeric](18, 0) NULL ,
	[nvarchar] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[real] [real] NULL ,
	[smdt] [smalldatetime] NULL ,
	[smallint] [smallint] NULL ,
	[smallmon] [smallmoney] NULL ,
	[sqlVariant] [sql_variant] NULL ,
	[txt] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[timestamp] [timestamp] NULL ,
	[tinyint] [tinyint] NULL ,
	[unqid] [uniqueidentifier] NULL ,
	[varbinary] [varbinary] (50) NULL ,
	[varchar] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [AddQuery]
	@ID			int OUTPUT,
	@Title			varchar(255),
	@DataSource		varchar(128)
AS
	SET NOCOUNT ON
	SET ANSI_WARNINGS OFF

	INSERT INTO [Querys] ([Title], [DataSource])
	VALUES(@Title, @DataSource)

	SET @ID = @@identity

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [AddQueryColumn]
	@Column			varchar(128),
	@QueryID		int
AS
	SET NOCOUNT ON
	SET ANSI_WARNINGS OFF

	DECLARE @OrderNo int

	SET @OrderNo = isnull((SELECT MAX([OrderNo]) FROM [QueryColumns] WHERE [QueryID] = @QueryID), 0) + 1


	INSERT INTO [QueryColumns] ([OrderNo], [Column], [QueryID])
	VALUES(@OrderNo, @Column, @QueryID)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [AddQueryCondition]
	@Column			varchar(128),
	@Operator		varchar(64),
	@Value			varchar(255),
	@Type			smallint,
	@QueryID		int
AS
	SET NOCOUNT ON
	SET ANSI_WARNINGS OFF

	DECLARE @OrderNo int

	SET @OrderNo = isnull((SELECT MAX([OrderNo]) FROM [QueryConditions] WHERE [QueryID] = @QueryID), 0) + 1
	
	INSERT INTO [QueryConditions] ([OrderNo], [Column], [Operator], [Value], [Type], [QueryID])
	VALUES(@OrderNo, @Column, @Operator, @Value, @Type, @QueryID)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [AddQuerySortOrder]
	@Column			varchar(128),
	@Type			smallint,
	@QueryID		int
AS
	SET NOCOUNT ON
	SET ANSI_WARNINGS OFF

	DECLARE @OrderNo int

	SET @OrderNo = isnull((SELECT MAX([OrderNo]) FROM [QuerySortOrders] WHERE [QueryID] = @QueryID), 0) + 1

	INSERT INTO [QuerySortOrders] ([OrderNo], [Column], [Type], [QueryID])
	VALUES(@OrderNo, @Column, @Type, @QueryID)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE Allthing @bigint bigint,
@binary binary(50),
@bitf bit,
@char char(10),
@dt datetime,
@dec decimal(18, 0),
@flt float,
@img image,
@in int,
@mnry money,
@nchar nchar(10),
@ntext ntext,
@num numeric(18, 0),
@nvarchar nvarchar(50),
@real real,
@smdt smalldatetime,
@smallint smallint,
@smallmon smallmoney,
@sqlVariant sql_variant,
@txt text,
@timestamp timestamp,
@tinyint tinyint,
@unqid uniqueidentifier,
@varbinary varbinary(50),
@varchar varchar(50)
AS
SELECT * FROM AllThings
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE CustOrderHist @CustomerID nchar(5)
AS
SELECT ProductName, Total=SUM(Quantity)
FROM Products P, [Order Details] OD, Orders O, Customers C
WHERE C.CustomerID = @CustomerID
AND C.CustomerID = O.CustomerID AND O.OrderID = OD.OrderID AND OD.ProductID = P.ProductID
GROUP BY ProductName

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE CustOrdersDetail @OrderID int
AS
SELECT ProductName,
    UnitPrice=ROUND(Od.UnitPrice, 2),
    Quantity,
    Discount=CONVERT(int, Discount * 100), 
    ExtendedPrice=ROUND(CONVERT(money, Quantity * (1 - Discount) * Od.UnitPrice), 2)
FROM Products P, [Order Details] Od
WHERE Od.ProductID = P.ProductID and Od.OrderID = @OrderID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE CustOrdersOrders @CustomerID nchar(5)
AS
SELECT OrderID, 
	OrderDate,
	RequiredDate,
	ShippedDate
FROM Orders
WHERE CustomerID = @CustomerID
ORDER BY OrderID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [DeleteQuery]
	@ID			int
AS
	SET NOCOUNT ON
	SET ANSI_WARNINGS OFF

	DELETE [Querys]
	WHERE
		[ID] = @ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [DeleteQueryColumns]
	@ID			int
AS
	SET NOCOUNT ON
	SET ANSI_WARNINGS OFF

	DELETE [QueryColumns]
	WHERE
		[QueryID] = @ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [DeleteQueryConditions]
	@ID			int
AS
	SET NOCOUNT ON
	SET ANSI_WARNINGS OFF

	DELETE [QueryConditions]
	WHERE
		[QueryID] = @ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [DeleteQuerySortOrders]
	@ID			int
AS
	SET NOCOUNT ON
	SET ANSI_WARNINGS OFF

	DELETE [QuerySortOrders]
	WHERE
		[QueryID] = @ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

create procedure "Employee Sales by Country" 
@Beginning_Date DateTime, @Ending_Date DateTime AS
SELECT Employees.Country, Employees.LastName, Employees.FirstName, Orders.ShippedDate, Orders.OrderID, "Order Subtotals".Subtotal AS SaleAmount
FROM Employees INNER JOIN 
	(Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID) 
	ON Employees.EmployeeID = Orders.EmployeeID
WHERE Orders.ShippedDate Between @Beginning_Date And @Ending_Date

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [GetQueryByID]
	@ID			int
AS
	SET NOCOUNT ON
	SET ANSI_WARNINGS OFF

	SELECT [ID], [Title], [DataSource]
	FROM [Querys]
	WHERE
		[ID] = @ID

	SELECT [OrderNo], [Column], [QueryID]
	FROM [QueryColumns]
	WHERE
		[QueryID] = @ID
	ORDER BY [OrderNo] ASC 

	SELECT [OrderNo], [Column], [Operator], [Value], [Type], [QueryID]
	FROM [QueryConditions]
	WHERE
		[QueryID] = @ID
	ORDER BY [OrderNo] ASC

	SELECT [OrderNo], [Column], [Type], [QueryID]
	FROM [QuerySortOrders]
	WHERE
		[QueryID] = @ID
	ORDER BY [OrderNo] ASC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [ListAllQuery]
AS
	SET NOCOUNT ON
	SET ANSI_WARNINGS OFF

	SELECT [ID], [Title], [DataSource]
	FROM [Querys]
	ORDER BY [Title] ASC

	SELECT [OrderNo], [Column], [QueryID]
	FROM [QueryColumns]
	JOIN [Querys]
	ON [QueryColumns].[QueryID] = [Querys].[ID]
	ORDER BY [Querys].[Title] ASC, [QueryColumns].[OrderNo] ASC 

	SELECT [OrderNo], [Column], [Operator], [Value], [Type], [QueryID]
	FROM [QueryConditions]
	JOIN [Querys]
	ON [QueryConditions].[QueryID] = [Querys].[ID]
	ORDER BY [Querys].[Title] ASC, [QueryConditions].[OrderNo] ASC 

	SELECT [OrderNo], [Column], [Type], [QueryID]
	FROM [QuerySortOrders]
	JOIN [Querys]
	ON [QuerySortOrders].[QueryID] = [Querys].[ID]
	ORDER BY [Querys].[Title] ASC, [QuerySortOrders].[OrderNo] ASC 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [ListByRawSQL]
	@SQL		nvarchar(4000)
AS
	SET NOCOUNT ON
	SET ANSI_WARNINGS OFF

	EXECUTE sp_executesql @SQL

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [ListQuerysByDataSource]
	@DataSource		varchar(128)
AS
	SET NOCOUNT ON
	SET ANSI_WARNINGS OFF

	SELECT [ID], [Title], [DataSource]
	INTO #QueryTemp
	FROM [Querys]
	WHERE
		[DataSource] = @DataSource

	SELECT *
	FROM #QueryTemp
	ORDER BY [Title] ASC

	SELECT [OrderNo], [Column], [QueryID]
	FROM [QueryColumns]
	JOIN #QueryTemp
	ON [QueryColumns].[QueryID] = #QueryTemp.[ID]
	ORDER BY #QueryTemp.[Title] ASC, [QueryColumns].[OrderNo] ASC 

	SELECT [OrderNo], [Column], [Operator], [Value], [Type], [QueryID]
	FROM [QueryConditions]
	JOIN #QueryTemp
	ON [QueryConditions].[QueryID] = #QueryTemp.[ID]
	ORDER BY #QueryTemp.[Title] ASC, [QueryConditions].[OrderNo] ASC 

	SELECT [OrderNo], [Column], [Type], [QueryID]
	FROM [QuerySortOrders]
	JOIN #QueryTemp
	ON [QuerySortOrders].[QueryID] = #QueryTemp.[ID]
	ORDER BY #QueryTemp.[Title] ASC, [QuerySortOrders].[OrderNo] ASC 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE PassBinary
@binary binary(50)
AS
SELECT * FROM AllThings
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE PassImage 
@img image
AS
SELECT * FROM AllThings
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE PassVarBin
@varbinary varbinary(50)
AS
SELECT * FROM AllThings
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE SP_SelectCustomers
	@Title 	nvarchar(30),
	@Country 	nvarchar(15)
AS
	select * 
	from Customers
	where ContactTitle = @Title and Country = @Country
	for xml auto
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

create procedure "Sales by Year" 
	@Beginning_Date DateTime, @Ending_Date DateTime AS
SELECT Orders.ShippedDate, Orders.OrderID, "Order Subtotals".Subtotal, DATENAME(yy,ShippedDate) AS Year
FROM Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID
WHERE Orders.ShippedDate Between @Beginning_Date And @Ending_Date

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE SalesByCategory
    @CategoryName nvarchar(15), @OrdYear nvarchar(4) = '1998'
AS
IF @OrdYear != '1996' AND @OrdYear != '1997' AND @OrdYear != '1998' 
BEGIN
	SELECT @OrdYear = '1998'
END

SELECT ProductName,
	TotalPurchase=ROUND(SUM(CONVERT(decimal(14,2), OD.Quantity * (1-OD.Discount) * OD.UnitPrice)), 0)
FROM [Order Details] OD, Orders O, Products P, Categories C
WHERE OD.OrderID = O.OrderID 
	AND OD.ProductID = P.ProductID 
	AND P.CategoryID = C.CategoryID
	AND C.CategoryName = @CategoryName
	AND SUBSTRING(CONVERT(nvarchar(22), O.OrderDate, 111), 1, 4) = @OrdYear
GROUP BY ProductName
ORDER BY ProductName

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

create procedure "Ten Most Expensive Products" AS
SET ROWCOUNT 10
SELECT Products.ProductName AS TenMostExpensiveProducts, Products.UnitPrice
FROM Products
ORDER BY Products.UnitPrice DESC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [UpdateQuery]
	@ID			int,
	@Title			varchar(255),
	@DataSource		varchar(128)
AS
	SET NOCOUNT ON
	SET ANSI_WARNINGS OFF

	UPDATE 	[Querys]
	SET	[Title] = @Title,
		[DataSource] = @DataSource
	WHERE
		[ID] = @ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE [dbo].[bts_InsertDynamicStateInfo_BizTalkServerApplication]
@uidClassID uniqueidentifier,
@uidServiceID uniqueidentifier,
@uidInstanceID uniqueidentifier,
@uidActivationID uniqueidentifier,
@uidInstanceStateID uniqueidentifier,
@imgData image,
@nvcUserState nvarchar(256),
@fSuccess int OUTPUT
AS
set transaction isolation level read committed
set nocount on
declare @dtTimeStamp datetime,
		@uidProcessID uniqueidentifier,
		@nCount int
		
set @dtTimeStamp = GetUTCDate()
set @fSuccess = 0 
if ( @uidInstanceID IS NOT NULL AND @uidActivationID IS NOT NULL)
begin
	set @uidProcessID = APP_NAME()
	SELECT @nCount = COUNT(*) FROM Instances WITH (ROWLOCK REPEATABLEREAD) 
	WHERE (uidInstanceID = @uidInstanceID AND uidProcessID = @uidProcessID AND uidActivationID = @uidActivationID )
	OPTION (KEEPFIXED PLAN)
	
	if ( @nCount = 0 )
	begin
		set @fSuccess = 10
		return
	end
	
end
IF (@uidInstanceStateID IS NULL)
BEGIN
	set @uidInstanceStateID = NewID()
END
IF @uidInstanceStateID = @uidInstanceID AND @nvcUserState IS NOT NULL
	UPDATE Instances SET nvcUserState = @nvcUserState where uidInstanceID = @uidInstanceID  AND uidAppOwnerID = N'AA15A77E-CA85-412E-B6B4-7979C5D4806E'
UPDATE [DynamicStateInfo_BizTalkServerApplication]  WITH (ROWLOCK UPDLOCK)
SET		dtTimeStamp = @dtTimeStamp,
		imgData = @imgData
WHERE uidInstanceID = @uidInstanceID AND uidInstanceStateID = @uidInstanceStateID
if ( @@ROWCOUNT = 0 )
BEGIN
	INSERT INTO [DynamicStateInfo_BizTalkServerApplication] WITH (ROWLOCK) (
			uidInstanceStateID,
			uidClassID,
			uidServiceID,
			uidInstanceID,
			dtTimeStamp,
			imgData)
		VALUES (
			@uidInstanceStateID,
			@uidClassID,
			@uidServiceID,
			@uidInstanceID,
			@dtTimeStamp,
			@imgData
			) 
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

