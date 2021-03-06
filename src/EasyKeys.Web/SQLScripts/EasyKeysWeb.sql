/****** Object:  Table [dbo].[Audit]    Script Date: 9/23/2021 4:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Audit](
	[AuditID] [int] IDENTITY(1,1) NOT NULL,
	[AuditEventID] [int] NULL,
	[DetailDescription] [varchar](max) NULL,
	[LogTime] [datetime] NOT NULL,
	[InsertDateTime] [smalldatetime] NULL,
	[LastUpdateDateTime] [smalldatetime] NULL,
	[Code] [varchar](50) NULL,
	[Deleted] [bit] NULL,
	[InsertApp] [varchar](30) NULL,
	[LastUpdateApp] [varchar](30) NULL,
 CONSTRAINT [PK_Audit] PRIMARY KEY CLUSTERED 
(
	[AuditID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailLog]    Script Date: 9/23/2021 4:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailLog](
	[EmailLogID] [int] IDENTITY(1,1) NOT NULL,
	[ToEmailList] [varchar](500) NULL,
	[FromEmail] [varchar](500) NULL,
	[BCCEmailList] [varchar](500) NULL,
	[Subject] [varchar](500) NULL,
	[BodyIsHTML] [bit] NULL,
	[Body] [varchar](max) NULL,
	[CreatedDateTime] [datetime] NULL,
	[Sent] [bit] NULL,
	[SentDateTime] [datetime] NULL,
	[InsertDateTime] [smalldatetime] NULL,
	[LastUpdateDateTime] [smalldatetime] NULL,
	[Code] [varchar](50) NULL,
	[Deleted] [bit] NULL,
	[InsertApp] [varchar](30) NULL,
	[LastUpdateApp] [varchar](30) NULL,
	[EmailRequestID] [int] NULL,
 CONSTRAINT [PK_EmailLog] PRIMARY KEY CLUSTERED 
(
	[EmailLogID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Audit] ADD  CONSTRAINT [DF_Audit_LogTime]  DEFAULT (getdate()) FOR [LogTime]
GO
ALTER TABLE [dbo].[EmailLog] ADD  CONSTRAINT [DF__EmailLog__EmailR__3D614D2E]  DEFAULT ((0)) FOR [EmailRequestID]
GO
/****** Object:  StoredProcedure [dbo].[usp_Audit_delete]    Script Date: 9/23/2021 4:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_Audit_delete]
	@AuditID int = null
AS

SET NOCOUNT ON

-- DELETE FROM [dbo].[Audit]
UPDATE [dbo].[Audit]
	set Deleted = 1
WHERE
	[AuditID] = @AuditID

--endregion
GO
/****** Object:  StoredProcedure [dbo].[usp_Audit_insert]    Script Date: 9/23/2021 4:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--region [dbo].[usp_Audit_insert]


CREATE PROCEDURE [dbo].[usp_Audit_insert]
	@InsertApp varchar(30) = null ,
	@AuditEventID int = null ,
	@DetailDescription text = null ,
	@LogTime datetime = null ,
	@Code varchar(50) = null ,
	@AuditID int OUTPUT
AS

SET NOCOUNT ON


IF(@Code is null)
BEGIN
	set @code = newid()
END 

INSERT INTO [dbo].[Audit] (
	InsertApp,
	LastUpdateApp,
	InsertDateTime,
	LastUpdateDateTime,
	[AuditEventID],
	[DetailDescription],
	[LogTime],
	[Code]
) VALUES (
	@InsertApp,
	@InsertApp,
	CAST(SYSDATETIMEOFFSET() AT TIME ZONE 'US Eastern Standard Time' AS smalldatetime),
	CAST(SYSDATETIMEOFFSET() AT TIME ZONE 'US Eastern Standard Time' AS smalldatetime),
	@AuditEventID,
	@DetailDescription,
	@LogTime,
	@Code
)

SET @AuditID = SCOPE_IDENTITY()

--endregion
GO
/****** Object:  StoredProcedure [dbo].[usp_Audit_save]    Script Date: 9/23/2021 4:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--region [dbo].[usp_Audit_save]


CREATE PROCEDURE [dbo].[usp_Audit_save]
	@AuditID int = null ,
	@AuditEventID int = null ,
	@DetailDescription text = null ,
	@LogTime datetime = null
AS

SET NOCOUNT ON

IF EXISTS(SELECT [AuditID] FROM [dbo].[Audit] WHERE [AuditID] = @AuditID)
BEGIN
	UPDATE [dbo].[Audit] SET
		[AuditEventID] = @AuditEventID,
		[DetailDescription] = @DetailDescription,
		[LogTime] = @LogTime
	WHERE
		[AuditID] = @AuditID
END
ELSE
BEGIN
	INSERT INTO [dbo].[Audit] (
		[AuditID],
		[AuditEventID],
		[DetailDescription],
		[LogTime]
	) VALUES (
		@AuditID,
		@AuditEventID,
		@DetailDescription,
		@LogTime
	)
END

--endregion
GO
/****** Object:  StoredProcedure [dbo].[usp_Audit_update]    Script Date: 9/23/2021 4:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--region [dbo].[usp_Audit_update]


CREATE PROCEDURE [dbo].[usp_Audit_update]
	@LastUpdateApp varchar(30) = null ,
	@AuditID int = null ,
	@AuditEventID int = null ,
	@DetailDescription text = null ,
	@LogTime datetime = null ,
	@Code varchar(50) = null
AS

SET NOCOUNT ON

UPDATE [dbo].[Audit] SET 
		LastUpdateDateTime = CAST(SYSDATETIMEOFFSET() AT TIME ZONE 'US Eastern Standard Time' AS smalldatetime),
		LastUpdateApp = @LastUpdateApp,
	[AuditEventID] = @AuditEventID,
	[DetailDescription] = @DetailDescription,
	[LogTime] = @LogTime,
	[Code] = @Code
WHERE
	[AuditID] = @AuditID

--endregion
GO
/****** Object:  StoredProcedure [dbo].[usp_EmailLog_delete]    Script Date: 9/23/2021 4:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--region [dbo].[usp_EmailLog_delete]


CREATE PROCEDURE [dbo].[usp_EmailLog_delete]
	@EmailLogID int = null
AS

SET NOCOUNT ON

-- DELETE FROM [dbo].[EmailLog]
UPDATE [dbo].[EmailLog]
	set Deleted = 1
WHERE
	[EmailLogID] = @EmailLogID

--endregion
GO
/****** Object:  StoredProcedure [dbo].[usp_EmailLog_GetAllPendingEmails]    Script Date: 9/23/2021 4:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  exec usp_EmailLog_GetAllPendingEmails 5

CREATE PROCEDURE [dbo].[usp_EmailLog_GetAllPendingEmails]
(
	@diffMinutes int
)
AS

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ COMMITTED


 
SELECT  
	l.[EmailLogID],
	l.[ToEmailList],
	l.[FromEmail],
	l.[BCCEmailList],
	l.[Subject],
	l.[BodyIsHTML],
	l.[Body],
	l.[CreatedDateTime],
	l.[Sent],
	l.[SentDateTime],
	l.[Code],
	l.[EmailRequestID]
FROM
	[dbo].[vw_EmailLog] l with (nolock) 
	left join dbo.vw_EmailRequest r on l.EmailRequestID = r.EmailRequestID
	left join dbo.vw_EmailType t on r.EmailTypeID = t.EmailTypeID
	where   Sent = 0 
	and DATEDIFF(hour, l.CreatedDateTime, CAST(SYSDATETIMEOFFSET() AT TIME ZONE 'US Eastern Standard Time' AS smalldatetime))  < 25
	and ToEmailList <> 'guest@easy.keys.com'
	and ToEmailList <> 'guest2@easy.keys.com'
	and ToEmailList <> 'guest3@easy.keys.com'
	and ToEmailList <> 'guest4@easy.keys.com'
	and ToEmailList <> 'guest5@easy.keys.com'
	and ToEmailList not like 'vrtest20%'   
	and ToEmailList <> 'guest@fakemail.com'
	and isnull(t.Code,'') <> 'OrderCustomerOrderReceiptShipped'
	and ToEmailList <> ''

union all
 

SELECT   
	l.[EmailLogID],
	l.[ToEmailList],
	l.[FromEmail],
	l.[BCCEmailList],
	l.[Subject],
	l.[BodyIsHTML],
	l.[Body],
	l.[CreatedDateTime],
	l.[Sent],
	l.[SentDateTime],
	l.[Code],
	l.[EmailRequestID] 

FROM
	[dbo].[vw_EmailLog] l with (nolock) 
	join dbo.vw_EmailRequest r with (nolock) on l.EmailRequestID = r.EmailRequestID
	join dbo.vw_EmailType t with (nolock) on r.EmailTypeID = t.EmailTypeID
	join dbo.vw_OrderEK o with (nolock) on o.CartID =  r.emailtypesubjectCode 
	left join dbo.vw_ShippingMethod s with (nolock) on o.ShippingMethodID = s.shippingmethodid
	left join dbo.vw_OrderShipmentUSPS u with (nolock) on o.OrderID = u.orderid
	left join dbo.vw_OrderShipmentUSPS u2 with (nolock) on u.OrderID = u2.orderid and u.OrderShipmentUSPSID < u2.OrderShipmentUSPSid
	where   
		u2.OrderShipmentUSPSID is null
	and DATEDIFF(hour, l.CreatedDateTime, CAST(SYSDATETIMEOFFSET() AT TIME ZONE 'US Eastern Standard Time' AS smalldatetime))  < 25
	and l.Sent = 0 
	and l.ToEmailList <> 'guest@easy.keys.com'
	and l.ToEmailList <> 'guest2@easy.keys.com'
	and l.ToEmailList <> 'guest3@easy.keys.com'
	and l.ToEmailList <> 'guest4@easy.keys.com'
	and l.ToEmailList <> 'guest5@easy.keys.com'
	and l.ToEmailList not like 'vrtest20%'   
	and l.ToEmailList <> 'guest@fakemail.com'
	and t.Code = 'OrderCustomerOrderReceiptShipped'
	and
	(
		( 
			isnull(s.Name,'') not like '%USPS%'
		)
		or
		(
			isnull(s.Name,'') like '%USPS%' 
			and u.OrderShipmentUSPSID is not null

		)
	)
	and ToEmailList <> ''
GO
/****** Object:  StoredProcedure [dbo].[usp_EmailLog_insert]    Script Date: 9/23/2021 4:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--region [dbo].[usp_EmailLog_insert]


CREATE PROCEDURE [dbo].[usp_EmailLog_insert]
	@InsertApp varchar(30) = null ,
	@ToEmailList varchar(500) = null ,
	@FromEmail varchar(500) = null ,
	@BCCEmailList varchar(500) = null ,
	@Subject varchar(500) = null ,
	@BodyIsHTML bit = null ,
	@Body text = null ,
	@CreatedDateTime datetime = null ,
	@Sent bit = null ,
	@SentDateTime datetime = null ,
	@Code varchar(50) = null ,
	@EmailRequestID int = null ,
	@EmailLogID int OUTPUT
AS

SET NOCOUNT ON


IF(@Code is null)
BEGIN
	set @code = newid()
END 

INSERT INTO [dbo].[EmailLog] (
	InsertApp,
	LastUpdateApp,
	InsertDateTime,
	LastUpdateDateTime,
	[ToEmailList],
	[FromEmail],
	[BCCEmailList],
	[Subject],
	[BodyIsHTML],
	[Body],
	[CreatedDateTime],
	[Sent],
	[SentDateTime],
	[Code],
	[EmailRequestID]
) VALUES (
	@InsertApp,
	@InsertApp,
	CAST(SYSDATETIMEOFFSET() AT TIME ZONE 'US Eastern Standard Time' AS smalldatetime),
	CAST(SYSDATETIMEOFFSET() AT TIME ZONE 'US Eastern Standard Time' AS smalldatetime),
	@ToEmailList,
	@FromEmail,
	@BCCEmailList,
	@Subject,
	@BodyIsHTML,
	@Body,
	@CreatedDateTime,
	@Sent,
	@SentDateTime,
	@Code,
	@EmailRequestID
)

SET @EmailLogID = SCOPE_IDENTITY()

--endregion
GO
/****** Object:  StoredProcedure [dbo].[usp_EmailLog_Search]    Script Date: 9/23/2021 4:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_EmailLog_Search]
(
	@SearchString varchar(30),
	@StartDate DateTime,
	@EndDate DateTime
)
AS

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ COMMITTED

SELECT TOP 1000 
	[EmailLogID],
	[ToEmailList],
	[FromEmail],
	[BCCEmailList],
	[Subject],
	[BodyIsHTML],
	[Body],
	[CreatedDateTime],
	[Sent],
	[SentDateTime],
	[Code],
	[EmailRequestID]
FROM
	[dbo].[EmailLog] with (nolock) 
Where isnull(Deleted,0) = 0 
	And CharIndex(@SearchString, [ToEmailList]) > 0
	And [SentDateTime] Between @StartDate And @EndDate
	order by [SentDateTime] desc
--endregion
GO
/****** Object:  StoredProcedure [dbo].[usp_EmailLog_select]    Script Date: 9/23/2021 4:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--region [dbo].[usp_EmailLog_select]

CREATE PROCEDURE [dbo].[usp_EmailLog_select]
	@EmailLogID int = null
AS

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ COMMITTED

SELECT TOP 5000
	[EmailLogID],
	[ToEmailList],
	[FromEmail],
	[BCCEmailList],
	[Subject],
	[BodyIsHTML],
	[Body],
	[CreatedDateTime],
	[Sent],
	[SentDateTime],
	[Code],
	[EmailRequestID]
FROM
	[dbo].[EmailLog] with (nolock)
WHERE isnull(Deleted,0) = 0 and
	[EmailLogID] = @EmailLogID

--endregion
GO
/****** Object:  StoredProcedure [dbo].[usp_EmailLog_selectAll]    Script Date: 9/23/2021 4:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--region [dbo].[usp_EmailLog_selectAll]

CREATE PROCEDURE [dbo].[usp_EmailLog_selectAll]
AS

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ COMMITTED

SELECT TOP 100 
	[EmailLogID],
	[ToEmailList],
	[FromEmail],
	[BCCEmailList],
	[Subject],
	[BodyIsHTML],
	[Body],
	[CreatedDateTime],
	[Sent],
	[SentDateTime],
	[Code],
	[EmailRequestID]
FROM
	[dbo].[EmailLog] with (nolock) where  isnull(Deleted,0) = 0 

--endregion
GO
/****** Object:  StoredProcedure [dbo].[usp_EmailLog_selectByCode]    Script Date: 9/23/2021 4:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--region [dbo].[usp_EmailLog_select]ByCode

CREATE PROCEDURE [dbo].[usp_EmailLog_selectByCode]
	@Code varchar(50) = null  
AS

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ COMMITTED

SELECT TOP 5000
	*
FROM
	[dbo].[vw_EmailLog] 
WHERE Code = @Code 

--endregion
GO
/****** Object:  StoredProcedure [dbo].[usp_EmailLog_update]    Script Date: 9/23/2021 4:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--region [dbo].[usp_EmailLog_update]

CREATE PROCEDURE [dbo].[usp_EmailLog_update]
	@LastUpdateApp varchar(30) = null ,
	@EmailLogID int = null ,
	@ToEmailList varchar(500) = null ,
	@FromEmail varchar(500) = null ,
	@BCCEmailList varchar(500) = null ,
	@Subject varchar(500) = null ,
	@BodyIsHTML bit = null ,
	@Body text = null ,
	@CreatedDateTime datetime = null ,
	@Sent bit = null ,
	@SentDateTime datetime = null ,
	@Code varchar(50) = null ,
	@EmailRequestID int = null
AS

SET NOCOUNT ON

UPDATE [dbo].[EmailLog] SET 
		LastUpdateDateTime = CAST(SYSDATETIMEOFFSET() AT TIME ZONE 'US Eastern Standard Time' AS smalldatetime),
		LastUpdateApp = @LastUpdateApp,
	[ToEmailList] = @ToEmailList,
	[FromEmail] = @FromEmail,
	[BCCEmailList] = @BCCEmailList,
	[Subject] = @Subject,
	[BodyIsHTML] = @BodyIsHTML,
	[Body] = @Body,
	[CreatedDateTime] = @CreatedDateTime,
	[Sent] = @Sent,
	[SentDateTime] = @SentDateTime,
	[Code] = @Code,
	[EmailRequestID] = @EmailRequestID
WHERE
	[EmailLogID] = @EmailLogID

--endregion
GO
