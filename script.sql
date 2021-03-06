USE [master]
GO
/****** Object:  Database [JobSearch]    Script Date: 4/19/2019 3:08:43 PM ******/
CREATE DATABASE [JobSearch]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'JobSearch', FILENAME = N'{filepath}' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'JobSearch_log', FILENAME = N'{filepath}' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [JobSearch] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [JobSearch].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [JobSearch] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [JobSearch] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [JobSearch] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [JobSearch] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [JobSearch] SET ARITHABORT OFF 
GO
ALTER DATABASE [JobSearch] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [JobSearch] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [JobSearch] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [JobSearch] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [JobSearch] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [JobSearch] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [JobSearch] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [JobSearch] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [JobSearch] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [JobSearch] SET  DISABLE_BROKER 
GO
ALTER DATABASE [JobSearch] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [JobSearch] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [JobSearch] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [JobSearch] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [JobSearch] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [JobSearch] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [JobSearch] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [JobSearch] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [JobSearch] SET  MULTI_USER 
GO
ALTER DATABASE [JobSearch] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [JobSearch] SET DB_CHAINING OFF 
GO
ALTER DATABASE [JobSearch] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [JobSearch] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [JobSearch] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [JobSearch] SET QUERY_STORE = OFF
GO
USE [JobSearch]
GO
/****** Object:  Table [dbo].[Application]    Script Date: 4/19/2019 3:08:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Application](
	[ApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationDate] [date] NOT NULL,
	[CompanyID] [int] NOT NULL,
 CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED 
(
	[ApplicationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Jobs]    Script Date: 4/19/2019 3:08:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Jobs](
	[CompanyID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [nvarchar](50) NOT NULL,
	[PositionID] [int] NOT NULL,
	[LocationID] [int] NOT NULL,
	[SalaryRange] [nvarchar](50) NULL,
	[RatingID] [int] NOT NULL,
	[CEOName] [nvarchar](50) NULL,
	[MissionStatement] [nvarchar](max) NULL,
	[Benefits] [nvarchar](250) NULL,
	[Comments] [nvarchar](max) NULL,
	[RecruiterID] [int] NULL,
	[JobLink] [nvarchar](max) NULL,
	[Date] [date] NOT NULL,
 CONSTRAINT [PK_Jobs] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Location]    Script Date: 4/19/2019 3:08:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[LocationID] [int] IDENTITY(1,1) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[StateID] [int] NOT NULL,
	[CityRating] [tinyint] NOT NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[LocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Position]    Script Date: 4/19/2019 3:08:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Position](
	[PositionID] [int] IDENTITY(1,1) NOT NULL,
	[JobTitle] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Position] PRIMARY KEY CLUSTERED 
(
	[PositionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rating]    Script Date: 4/19/2019 3:08:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rating](
	[RatingID] [int] IDENTITY(1,1) NOT NULL,
	[RatingDescription] [nvarchar](25) NOT NULL,
 CONSTRAINT [PK_Rating] PRIMARY KEY CLUSTERED 
(
	[RatingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recruiters]    Script Date: 4/19/2019 3:08:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recruiters](
	[RecruiterID] [int] IDENTITY(1,1) NOT NULL,
	[Recruiter_Name] [nvarchar](50) NULL,
	[Email] [nvarchar](75) NULL,
	[PhoneNumber] [nvarchar](10) NULL,
	[LinkedInLink] [nvarchar](max) NULL,
 CONSTRAINT [PK_Recruiters] PRIMARY KEY CLUSTERED 
(
	[RecruiterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[States]    Script Date: 4/19/2019 3:08:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[States](
	[StateID] [int] IDENTITY(1,1) NOT NULL,
	[StateAbbreviation] [nvarchar](2) NOT NULL,
	[State] [nvarchar](50) NOT NULL,
	[Capital] [nvarchar](65) NOT NULL,
	[LargestCity] [nvarchar](65) NOT NULL,
 CONSTRAINT [PK_States] PRIMARY KEY CLUSTERED 
(
	[StateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Jobs] ON 

INSERT [dbo].[Jobs] ([CompanyID], [CompanyName], [PositionID], [LocationID], [SalaryRange], [RatingID], [CEOName], [MissionStatement], [Benefits], [Comments], [RecruiterID], [JobLink], [Date]) VALUES (1, N'Test Company', 11, 2, N'100,000', 2, N'', N'', N'', N'', NULL, N'', CAST(N'2019-04-18' AS Date))
SET IDENTITY_INSERT [dbo].[Jobs] OFF
SET IDENTITY_INSERT [dbo].[Location] ON 

INSERT [dbo].[Location] ([LocationID], [City], [StateID], [CityRating], [Notes]) VALUES (2, N'Seattle', 47, 10, N'Most desired city as of right now')
INSERT [dbo].[Location] ([LocationID], [City], [StateID], [CityRating], [Notes]) VALUES (3, N'Raleigh', 33, 8, N'')
INSERT [dbo].[Location] ([LocationID], [City], [StateID], [CityRating], [Notes]) VALUES (6, N'Denver', 6, 6, N'')
SET IDENTITY_INSERT [dbo].[Location] OFF
SET IDENTITY_INSERT [dbo].[Position] ON 

INSERT [dbo].[Position] ([PositionID], [JobTitle]) VALUES (5, N'Software Developer')
INSERT [dbo].[Position] ([PositionID], [JobTitle]) VALUES (11, N'Software Engineer')
SET IDENTITY_INSERT [dbo].[Position] OFF
SET IDENTITY_INSERT [dbo].[Rating] ON 

INSERT [dbo].[Rating] ([RatingID], [RatingDescription]) VALUES (1, N'Overqualified')
INSERT [dbo].[Rating] ([RatingID], [RatingDescription]) VALUES (2, N'Qualified')
INSERT [dbo].[Rating] ([RatingID], [RatingDescription]) VALUES (3, N'Challenging')
INSERT [dbo].[Rating] ([RatingID], [RatingDescription]) VALUES (5, N'Need more to apply')
SET IDENTITY_INSERT [dbo].[Rating] OFF
SET IDENTITY_INSERT [dbo].[Recruiters] ON 

INSERT [dbo].[Recruiters] ([RecruiterID], [Recruiter_Name], [Email], [PhoneNumber], [LinkedInLink]) VALUES (1, N'', N'', N'1234', N'')
SET IDENTITY_INSERT [dbo].[Recruiters] OFF
SET IDENTITY_INSERT [dbo].[States] ON 

INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (1, N'AL', N'Alabama', N'Montgomery', N'Birmingham')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (2, N'AK', N'Alaska', N'Juneau', N'Anchorage')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (3, N'AZ', N'Arizona', N'Phoenix', N'Phoenix')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (4, N'AR', N'Arkansas', N'Little Rock', N'Little Rock')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (5, N'CA', N'California', N'Sacramento', N'Los Angeles')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (6, N'CO', N'Colorado', N'Denver', N'Denver')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (7, N'CT', N'Connecticut', N'Hartford', N'Bridgeport')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (8, N'DE', N'Delaware', N'Dover', N'Wilmington')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (9, N'FL', N'Florida', N'Tallahassee', N'Jacksonville')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (10, N'GA', N'Georgia', N'Atlanta', N'Atlanta')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (11, N'HI', N'Hawaii', N'Honolulu', N'Honolulu')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (12, N'ID', N'Idaho', N'Boise', N'Boise')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (13, N'IL', N'Illinois', N'Springfield', N'Chicago')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (14, N'IN', N'Indiana', N'Indianapolis', N'Indianapolis')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (15, N'IA', N'Iowa', N'Des Moines', N'Des Moines')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (16, N'KS', N'Kansas', N'Topeka', N'Wichita')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (17, N'KY', N'Kentucky', N'Frankfort', N'Louisville')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (18, N'LA', N'Louisiana', N'Baton Rouge', N'New Orleans')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (19, N'ME', N'Maine', N'Augusta', N'Portland')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (20, N'MD', N'Maryland', N'Annapolis', N'Baltimore')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (21, N'MA', N'Massachusetts', N'Boston', N'Boston')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (22, N'MI', N'Michigan', N'Lansing', N'Detroit')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (23, N'MN', N'Minnesota', N'Saint Paul', N'Minneapolis')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (24, N'MS', N'Mississippi', N'Jackson', N'Jackson')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (25, N'MO', N'Missouri', N'Jefferson City', N'Kansas City')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (26, N'MT', N'Montana', N'Helena', N'Billings')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (27, N'NE', N'Nebraska', N'Lincoln', N'Omaha')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (28, N'NV', N'Nevada', N'Carson City', N'Las Vegas')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (29, N'NH', N'New Hampshire', N'Concord', N'Manchester')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (30, N'NJ', N'New Jersey', N'Trenton', N'Newark')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (31, N'NM', N'New Mexico', N'Albany', N'Albuquerque')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (32, N'NY', N'New York', N'Santa Fe', N'New York')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (33, N'NC', N'North Carolina', N'Raleigh', N'Charlotte')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (34, N'ND', N'North Dakota', N'Bismark', N'Fargo')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (35, N'OH', N'Ohio', N'Columbus', N'Columbus')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (36, N'OK', N'Oklahoma', N'Oklahoma City', N'Oklahoma City')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (37, N'OR', N'Oregon', N'Salem', N'Portland')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (38, N'PA', N'Pennsylvania', N'Harrisburg', N'Philadelphia')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (39, N'RI', N'Rhode Island', N'Providence', N'Providence')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (40, N'SC', N'South Carolina', N'Columbia', N'Columbia')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (41, N'SD', N'South Dakota', N'Pierre', N'Sioux Falls')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (42, N'TN', N'Tennessee', N'Nashville', N'Memphis')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (43, N'TX', N'Texas', N'Austin', N'Houston')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (44, N'UT', N'Utah', N'Salt Lake City', N'Salt Lake City')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (45, N'VT', N'Vermont', N'Montpelier', N'Burlington')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (46, N'VA', N'Virginia', N'Richmond', N'Virginia Beach')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (47, N'WA', N'Washington', N'Olympia', N'Seattle')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (48, N'WV', N'West Virginia', N'Charleston', N'Charleston')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (49, N'WI', N'Wisconsin', N'Madison', N'Milwaukee')
INSERT [dbo].[States] ([StateID], [StateAbbreviation], [State], [Capital], [LargestCity]) VALUES (50, N'WY', N'Wyoming', N'Cheyenne', N'Cheyenne')
SET IDENTITY_INSERT [dbo].[States] OFF
ALTER TABLE [dbo].[Application]  WITH CHECK ADD  CONSTRAINT [FK_Application_Jobs] FOREIGN KEY([CompanyID])
REFERENCES [dbo].[Jobs] ([CompanyID])
GO
ALTER TABLE [dbo].[Application] CHECK CONSTRAINT [FK_Application_Jobs]
GO
ALTER TABLE [dbo].[Jobs]  WITH CHECK ADD  CONSTRAINT [FK_Jobs_Location] FOREIGN KEY([LocationID])
REFERENCES [dbo].[Location] ([LocationID])
GO
ALTER TABLE [dbo].[Jobs] CHECK CONSTRAINT [FK_Jobs_Location]
GO
ALTER TABLE [dbo].[Jobs]  WITH CHECK ADD  CONSTRAINT [FK_Jobs_Position1] FOREIGN KEY([PositionID])
REFERENCES [dbo].[Position] ([PositionID])
GO
ALTER TABLE [dbo].[Jobs] CHECK CONSTRAINT [FK_Jobs_Position1]
GO
ALTER TABLE [dbo].[Jobs]  WITH CHECK ADD  CONSTRAINT [FK_Jobs_Rating] FOREIGN KEY([RatingID])
REFERENCES [dbo].[Rating] ([RatingID])
GO
ALTER TABLE [dbo].[Jobs] CHECK CONSTRAINT [FK_Jobs_Rating]
GO
ALTER TABLE [dbo].[Jobs]  WITH CHECK ADD  CONSTRAINT [FK_Jobs_Recruiters] FOREIGN KEY([RecruiterID])
REFERENCES [dbo].[Recruiters] ([RecruiterID])
GO
ALTER TABLE [dbo].[Jobs] CHECK CONSTRAINT [FK_Jobs_Recruiters]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_States] FOREIGN KEY([StateID])
REFERENCES [dbo].[States] ([StateID])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_States]
GO
USE [master]
GO
ALTER DATABASE [JobSearch] SET  READ_WRITE 
GO
