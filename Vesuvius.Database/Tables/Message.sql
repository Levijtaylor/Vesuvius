

CREATE TABLE [dbo].[Message](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TypeID] [int] NOT NULL,
	[Content] [varchar](max) NULL,
	[TimeStamp] [datetime] NOT NULL,
	[ChannelID] [int] NOT NULL,
	[Data] [image] NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Channel] FOREIGN KEY([ChannelID])
REFERENCES [dbo].[Channel] ([ID])
GO

ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Channel]
GO

ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_MsgType] FOREIGN KEY([TypeID])
REFERENCES [dbo].[MsgType] ([ID])
GO

ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_MsgType]
GO

ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO

ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_User]
GO

