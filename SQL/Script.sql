USE [MyEmp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Emp](
	[EmpId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmpName] [varchar](255) NULL,
 CONSTRAINT [PK_Emp] PRIMARY KEY CLUSTERED 
(
	[EmpId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmpLogin](
	[LoginId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmpId] [bigint] NULL,
	[MAC] [varchar](50) NULL,
	[LoginTime] [datetime] NULL,
	[Location] [varchar](100) NULL,
	[IP] [varchar](50) NULL,
	[Cordinates] [varchar](100) NULL,
 CONSTRAINT [PK_EmpLogin] PRIMARY KEY CLUSTERED 
(
	[LoginId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FailureLogs](
	[FId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmpId] [bigint] NULL,
	[MacAddress] [varchar](50) NULL,
	[FDate] [datetime] NULL,
 CONSTRAINT [PK_FailureLogs] PRIMARY KEY CLUSTERED 
(
	[FId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserMacMaster](
	[MacId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmpId] [bigint] NULL,
	[MacAddress] [varchar](50) NULL,
	[LastLogin] [datetime] NULL,
	[AddedDate] [datetime] NULL,
	[Status] [bit] NULL,
	[Expiry] [datetime] NULL,
	[Type] [datetime] NULL,
	[IsLock] [bit] NULL,
	[LoginAttempt] [smallint] NULL,
 CONSTRAINT [PK_UserMacMaster] PRIMARY KEY CLUSTERED 
(
	[MacId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FailureLogs] ADD  CONSTRAINT [DF_FailureLogs_FDate]  DEFAULT (getutcdate()) FOR [FDate]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveFailureLog] (
	@FId BIGINT = 0
	,@EmpId BIGINT
	,@MacAddress VARCHAR(50)
	)
AS
BEGIN
	DECLARE @FailedCount INT = 0;
	DECLARE @IsLock BIT = NULL;

	SELECT TOP 1 @IsLock = IsLock
	FROM [dbo].[UserMacMaster]
	WHERE EmpId = @EmpId
		AND MacAddress = @MacAddress;

	IF @IsLock IS NOT NULL
	BEGIN
		IF @IsLock = 1
		BEGIN
			SELECT 'Your account is locked!' AS [Message]
				,1 AS IsSuccess
				,@EmpId AS Id;
		END
		ELSE
		BEGIN
			SELECT @FailedCount = COUNT(*)
			FROM [FailureLogs]
			WHERE EmpId = @EmpId
				AND MacAddress = @MacAddress
				AND CONVERT(DATE, FDate) = CONVERT(DATE, GETUTCDATE());

			IF @FailedCount < 4
			BEGIN
				INSERT INTO [dbo].[FailureLogs] (
					[EmpId]
					,[MacAddress]
					)
				VALUES (
					@EmpId
					,@MacAddress
					);

				SELECT 'You are left with ' + CAST((4 - @FailedCount) AS VARCHAR(10)) + ' attempts' AS [Message]
					,1 AS IsSuccess
					,@EmpId AS Id;
			END
			ELSE
			BEGIN
				UPDATE [UserMacMaster]
				SET IsLock = 1
				WHERE EmpId = @EmpId
					AND MacAddress = @MacAddress;

				SELECT 'Your account is locked as you exceeded 5 login attempts' AS [Message]
					,1 AS IsSuccess
					,@EmpId AS Id;
			END
		END
	END
	ELSE
	BEGIN
		SELECT 'User does not exist' AS [Message]
			,1 AS IsSuccess
			,@EmpId AS Id;
	END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[SP_GetEmpLogin]
as
begin
SELECT (
			ISNULL((
					SELECT 
						 [LoginId]
      ,[EmpId]
      ,[MAC]
      ,[LoginTime]
      ,[Location]
      ,[IP]
      ,[Cordinates]
						,ISNULL((
								SELECT TOP 1 ISNULL(E.EmpName, '') AS EmpName
								FROM Emp E
								WHERE E.EmpId = U.EmpId
								), '') AS EmpName
					
					FROM [EmpLogin] U
				
					FOR JSON path
					), '[]')
			);

			end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[SP_GetUsersMac]
as
begin
SELECT (
			ISNULL((
					SELECT 
						 [MacId]
      ,[EmpId]
      ,[MacAddress]
      ,[LastLogin]
      ,[AddedDate]
      ,[Status]
      ,[Expiry]
      ,[Type]
      ,[IsLock]
      ,[LoginAttempt]
						,ISNULL((
								SELECT TOP 1 ISNULL(E.EmpName, '') AS EmpName
								FROM Emp E
								WHERE E.EmpId = U.EmpId
								), '') AS EmpName
					
					FROM UserMacMaster U
				
					FOR JSON path
					), '[]')
			);

			end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[SP_SaveEmpLogin]
(
@LoginId  bigint=0,
	@EmpId  bigint  =NULL,
	@MAC  varchar (50) =NULL,
	@LoginTime  datetime  =NULL,
	@Location  varchar (100) =NULL,
	@IP  varchar (50) =NULL,
	@Cordinates  varchar (100) =NULL
)
as
begin

INSERT INTO [dbo].[EmpLogin]
           ([EmpId]
           ,[MAC]
           ,[LoginTime]
           ,[Location]
           ,[IP]
           ,[Cordinates])
     VALUES
           (@EmpId
           ,@MAC
           ,@LoginTime
           ,@Location
           ,@IP
           ,@Cordinates
		   )

		   	SELECT 'Saved successfully' AS [Message]
			,1 AS IsSuccess
			,@EmpId AS Id;
		   end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[SP_SaveUserMAC]
(
@MacId  bigint  =0,
	@EmpId  bigint  =NULL,
	@MacAddress varchar(50),
	@LastLogin  datetime  =NULL,
	@AddedDate  datetime  =NULL,
	@Status  bit  =NULL,
	@Expiry  datetime  =NULL,
	@Type  datetime  =NULL,
	@IsLock  bit  =NULL,
	@LoginAttempt  smallint  =NULL
	
)
as
begin

INSERT INTO [dbo].[UserMacMaster]
           ([EmpId]
		   ,[MacAddress]
           ,[LastLogin]
           ,[AddedDate]
           ,[Status]
           ,[Expiry]
           ,[Type]
           ,[IsLock]
           ,[LoginAttempt])
     VALUES
           (@EmpId
		   ,@MacAddress
           ,@LastLogin 
           ,@AddedDate
           ,@Status
           ,@Expiry 
           ,@Type
           ,@IsLock
           ,@LoginAttempt
		   )

		      	SELECT 'Saved successfully' AS [Message]
			,1 AS IsSuccess
			,@EmpId AS Id;
	end

	
		   
GO
