--SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'

EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all"
 DELETE FROM DemandDetails
 DELETE FROM Voucher
 DELETE FROM Citizen
 DELETE FROM CitizenLedgerDetails
 DELETE FROM Corrections
 DELETE FROM Demand
 DELETE FROM Form1
 DELETE FROM Form10
 DELETE FROM Form10Det
 DELETE FROM Form2
 DELETE FROM Form3
 DELETE FROM Form4
 DELETE FROM LedgerDetails
 DELETE FROM VoucherLedgerDetails
 DELETE FROM RVdetails
 DELETE FROM CBRunning
 DELETE FROM DemandDetails
 DELETE FROM DemandLedgerDetails
 DELETE FROM DemandPeriod
 DELETE FROM DemandYear
 DELETE FROM Period

 exec sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all"
DBCC CHECKIDENT ( 'DemandDetails', RESEED, 0) 
DBCC CHECKIDENT ( 'Voucher', RESEED, 0) 
DBCC CHECKIDENT ( 'Citizen', RESEED, 0) 
DBCC CHECKIDENT ( 'CitizenLedgerDetails', RESEED, 0) 
DBCC CHECKIDENT ( 'Corrections', RESEED, 0) 
DBCC CHECKIDENT ( 'Demand', RESEED, 0) 
DBCC CHECKIDENT ( 'Form1', RESEED, 0) 
DBCC CHECKIDENT ( 'Form10', RESEED, 0) 
DBCC CHECKIDENT ( 'Form10Det', RESEED, 0) 
DBCC CHECKIDENT ( 'Form2', RESEED, 0) 
DBCC CHECKIDENT ( 'Form3', RESEED, 0) 
DBCC CHECKIDENT ( 'Form4', RESEED, 0) 
DBCC CHECKIDENT ( 'LedgerDetails', RESEED, 0) 
DBCC CHECKIDENT ( 'VoucherLedgerDetails', RESEED, 0) 
DBCC CHECKIDENT ( 'RVdetails', RESEED, 0) 
DBCC CHECKIDENT ( 'CBRunning', RESEED, 0) 
DBCC CHECKIDENT ( 'DemandDetails', RESEED, 0) 
DBCC CHECKIDENT ( 'DemandLedgerDetails', RESEED, 0) 
DBCC CHECKIDENT ( 'DemandPeriod', RESEED, 0) 
DBCC CHECKIDENT ( 'DemandYear', RESEED, 0) 
DBCC CHECKIDENT ( 'Period', RESEED, 0) 

  INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'1', N'Boss')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'3', N'Type1')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'2', N'Type2')

INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'4b2b4144-fca2-476b-b4bb-da5352b309f3', N'ra@ra.com', 0, N'AIi/VESYzptG/sv0OOeXVLd0LCt5Se1EqLzqr4p2F7EQQvNhx4Ft7bU7Bh88ma3swA==', N'2be228cf-6f9f-46c1-b104-7f6f9ab53b22', NULL, 0, 0, NULL, 1, 0, N'ra@ra.com')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'4b2b4144-fca2-476b-b4bb-da5352b309f3', N'1')