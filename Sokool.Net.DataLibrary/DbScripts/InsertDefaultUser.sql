-- Add Roles
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'0fad14c2-b652-4182-ae19-c3a1cabf0fe5', N'User', N'USER', N'bfc90aa5-f601-453b-93fc-9db780ba8b61');
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'2cf68345-1291-4438-af87-cdb7129c31d6', N'Support', N'SUPPORT', N'1a671960-9f82-46eb-90ec-e3d6cca45743');
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'95e83848-7f49-4745-abb7-801a36932aeb', N'Admin', N'ADMIN', N'28f73652-7b54-49f5-9c69-eed7fceda83f');
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'ea13256a-d916-4558-a539-cce93b2f2d7c', N'SuperUser', N'SUPERUSER', N'c428753e-7614-4d6d-855c-075dc734551a');

-- Get Unique Identifier
DECLARE @UNI UNIQUEIDENTIFIER;
SET @UNI = NEWID(); 

-- Add Users
INSERT INTO [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName]) VALUES (@UNI, N'admin@sokool.net', N'ADMIN@SOKOOL.NET', N'admin@sokool.net', N'ADMIN@SOKOOL.NET', 1, N'AQAAAAEAACcQAAAAEE1yq4hJ0qouKvhbbSssowmbEHHgAx2Qvtz4Q15TVqvW+nAUEMQGAfufjEfTsFo6rg==', N'7CL2RSYEYQ5FFCJ63JLLEHDOWYLYK2T2', N'a3770546-cef3-4947-8e9d-eb5290f7cc52', NULL, 0, 0, NULL, 1, 0, N'Admin', N'User');


-- Add UserRoles
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (@UNI, N'95e83848-7f49-4745-abb7-801a36932aeb');
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (@UNI, N'ea13256a-d916-4558-a539-cce93b2f2d7c');


-- Add UserClaims
INSERT INTO [dbo].[AspNetUserClaims] ([UserId], [ClaimType], [ClaimValue]) VALUES (@UNI, N'CreateUser', N'true');
INSERT INTO [dbo].[AspNetUserClaims] ([UserId], [ClaimType], [ClaimValue]) VALUES (@UNI, N'EditUser', N'true');
INSERT INTO [dbo].[AspNetUserClaims] ([UserId], [ClaimType], [ClaimValue]) VALUES (@UNI, N'DeleteUser', N'true');
INSERT INTO [dbo].[AspNetUserClaims] ([UserId], [ClaimType], [ClaimValue]) VALUES (@UNI, N'EditUsersInRole', N'true');
INSERT INTO [dbo].[AspNetUserClaims] ([UserId], [ClaimType], [ClaimValue]) VALUES (@UNI, N'ManageUserRoles', N'true');
INSERT INTO [dbo].[AspNetUserClaims] ([UserId], [ClaimType], [ClaimValue]) VALUES (@UNI, N'ManageUserClaims', N'true');
INSERT INTO [dbo].[AspNetUserClaims] ([UserId], [ClaimType], [ClaimValue]) VALUES (@UNI, N'CreateRole', N'true');
INSERT INTO [dbo].[AspNetUserClaims] ([UserId], [ClaimType], [ClaimValue]) VALUES (@UNI, N'EditRole', N'true');
INSERT INTO [dbo].[AspNetUserClaims] ([UserId], [ClaimType], [ClaimValue]) VALUES (@UNI, N'DeleteRole', N'true');

