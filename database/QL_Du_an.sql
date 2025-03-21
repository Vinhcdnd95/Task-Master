USE [master]
GO
/****** Object:  Database [Quản lý công việc]    Script Date: 3/13/2025 10:04:29 AM ******/
CREATE DATABASE [Quản lý công việc]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Quản lý công việc', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Quản lý công việc.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Quản lý công việc_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Quản lý công việc_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Quản lý công việc] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Quản lý công việc].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Quản lý công việc] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Quản lý công việc] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Quản lý công việc] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Quản lý công việc] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Quản lý công việc] SET ARITHABORT OFF 
GO
ALTER DATABASE [Quản lý công việc] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Quản lý công việc] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Quản lý công việc] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Quản lý công việc] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Quản lý công việc] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Quản lý công việc] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Quản lý công việc] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Quản lý công việc] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Quản lý công việc] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Quản lý công việc] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Quản lý công việc] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Quản lý công việc] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Quản lý công việc] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Quản lý công việc] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Quản lý công việc] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Quản lý công việc] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Quản lý công việc] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Quản lý công việc] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Quản lý công việc] SET  MULTI_USER 
GO
ALTER DATABASE [Quản lý công việc] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Quản lý công việc] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Quản lý công việc] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Quản lý công việc] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Quản lý công việc] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Quản lý công việc] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Quản lý công việc] SET QUERY_STORE = ON
GO
ALTER DATABASE [Quản lý công việc] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Quản lý công việc]
GO
/****** Object:  Table [dbo].[assignment]    Script Date: 3/13/2025 10:04:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[assignment](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[task_id] [int] NOT NULL,
	[due_date] [datetime] NULL,
 CONSTRAINT [PK_assignment] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC,
	[task_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[board]    Script Date: 3/13/2025 10:04:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[board](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NULL,
	[description] [nvarchar](300) NULL,
	[createdAt] [datetime] NULL,
 CONSTRAINT [PK_Board] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[list]    Script Date: 3/13/2025 10:04:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[list](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NULL,
	[board_id] [int] NULL,
	[createdAt] [datetime] NULL,
 CONSTRAINT [PK_List] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[task]    Script Date: 3/13/2025 10:04:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[task](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NULL,
	[description] [nvarchar](200) NULL,
	[list_id] [int] NULL,
	[is_actived] [bit] NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user]    Script Date: 3/13/2025 10:04:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](50) NULL,
	[password] [varchar](50) NULL,
 CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[board] ON 

INSERT [dbo].[board] ([id], [name], [description], [createdAt]) VALUES (1, N'Board 1', N'board', NULL)
SET IDENTITY_INSERT [dbo].[board] OFF
GO
SET IDENTITY_INSERT [dbo].[list] ON 

INSERT [dbo].[list] ([id], [name], [board_id], [createdAt]) VALUES (1, N'List 1', 1, NULL)
SET IDENTITY_INSERT [dbo].[list] OFF
GO
SET IDENTITY_INSERT [dbo].[task] ON 

INSERT [dbo].[task] ([id], [name], [description], [list_id], [is_actived]) VALUES (1, N'Task 1', N'Thực hiện task1', 1, NULL)
SET IDENTITY_INSERT [dbo].[task] OFF
GO
SET IDENTITY_INSERT [dbo].[user] ON 

INSERT [dbo].[user] ([id], [username], [password]) VALUES (1, N'admin', N'admin')
SET IDENTITY_INSERT [dbo].[user] OFF
GO
ALTER TABLE [dbo].[assignment]  WITH CHECK ADD  CONSTRAINT [FK_assignment_Task] FOREIGN KEY([task_id])
REFERENCES [dbo].[task] ([id])
GO
ALTER TABLE [dbo].[assignment] CHECK CONSTRAINT [FK_assignment_Task]
GO
ALTER TABLE [dbo].[assignment]  WITH CHECK ADD  CONSTRAINT [FK_assignment_user] FOREIGN KEY([user_id])
REFERENCES [dbo].[user] ([id])
GO
ALTER TABLE [dbo].[assignment] CHECK CONSTRAINT [FK_assignment_user]
GO
ALTER TABLE [dbo].[list]  WITH CHECK ADD  CONSTRAINT [FK_List_Board] FOREIGN KEY([board_id])
REFERENCES [dbo].[board] ([id])
GO
ALTER TABLE [dbo].[list] CHECK CONSTRAINT [FK_List_Board]
GO
ALTER TABLE [dbo].[task]  WITH CHECK ADD  CONSTRAINT [FK_Task_List] FOREIGN KEY([list_id])
REFERENCES [dbo].[list] ([id])
GO
ALTER TABLE [dbo].[task] CHECK CONSTRAINT [FK_Task_List]
GO
USE [master]
GO
ALTER DATABASE [Quản lý công việc] SET  READ_WRITE 
GO
