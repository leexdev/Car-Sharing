CREATE DATABASE CarSharingDB;
GO

USE CarSharingDB
GO

-- Tạo bảng Users (Người dùng)
CREATE TABLE Users (
    UserId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(50) NOT NULL,
	Avatar NVARCHAR(MAX) DEFAULT N'/Content/assets/img/defaultuser.jpg',
    Email NVARCHAR(100) NOT NULL,
    FullName NVARCHAR(100) DEFAULT N'',
    Phone NVARCHAR(20) DEFAULT N'',
    Address NVARCHAR(200) DEFAULT N'',
	Role NVARCHAR(20) NOT NULL DEFAULT N'user',
	isDeleted BIT NOT NULL DEFAULT 0
);
GO

-- Tạo bảng Provinces (Tỉnh)
CREATE TABLE Province (
    ProvinceId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    ProvinceName NVARCHAR(100) NOT NULL,
    isDeleted BIT NOT NULL DEFAULT 0
);
GO

-- Tạo bảng Districts (Huyện)
CREATE TABLE District (
    DistrictId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    DistrictName NVARCHAR(100) NOT NULL,
    ProvinceId UNIQUEIDENTIFIER NOT NULL,
    isDeleted BIT NOT NULL DEFAULT 0,
    
    FOREIGN KEY (ProvinceId) REFERENCES Province(ProvinceId)
);
GO

-- Tạo bảng VehicleTypes (Loại xe)
CREATE TABLE VehicleType (
    VehicleTypeId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    VehicleTypeName NVARCHAR(50) NOT NULL,
    isDeleted BIT NOT NULL DEFAULT 0
);
GO

-- Tạo bảng Brands (Hãng xe)
CREATE TABLE VehicleBrand (
    BrandId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    BrandName NVARCHAR(50) NOT NULL,
    VehicleTypeId UNIQUEIDENTIFIER NOT NULL,
    isDeleted BIT NOT NULL DEFAULT 0,
    
    FOREIGN KEY (VehicleTypeId) REFERENCES VehicleType(VehicleTypeId)
);
GO

-- Tạo bảng Models (Mẫu xe)
CREATE TABLE VehicleVariant (
    VariantId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    VariantName NVARCHAR(50) NOT NULL,
    VehicleTypeId UNIQUEIDENTIFIER NOT NULL,
    isDeleted BIT NOT NULL DEFAULT 0,
    
    FOREIGN KEY (VehicleTypeId) REFERENCES VehicleType(VehicleTypeId)
);
GO

-- Tạo bảng Vehicles (Xe)
CREATE TABLE Vehicle (
    VehicleId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    VehicleName NVARCHAR(MAX) NOT NULL,
	Content NVARCHAR(MAX) NOT NULL,
    UserId UNIQUEIDENTIFIER NOT NULL,
    BrandId UNIQUEIDENTIFIER NOT NULL,
    VariantId UNIQUEIDENTIFIER NOT NULL,
	ImageVehicle NVARCHAR(200) NOT NULL,
    Year INT NOT NULL,
    RegistrationNumber NVARCHAR(50) NOT NULL,
	VehiclePrice DECIMAL(10, 2) NOT NULL,
    DistrictId UNIQUEIDENTIFIER NOT NULL,
	isDeleted BIT NOT NULL DEFAULT 0,

	FOREIGN KEY (UserId) REFERENCES Users(UserId),
	FOREIGN KEY (BrandId) REFERENCES dbo.VehicleBrand(BrandId),
	FOREIGN KEY (VariantId) REFERENCES dbo.VehicleVariant(VariantId),
	FOREIGN KEY (DistrictId) REFERENCES dbo.District(DistrictId)
);
GO

-- Tạo bảng Bookings (Đặt xe)
CREATE TABLE Booking (
    BookingId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL,
    VehicleId UNIQUEIDENTIFIER NOT NULL,
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NOT NULL,
    TotalPrice DECIMAL(10, 2) NOT NULL,
	Note NVARCHAR(MAX),
    Status NVARCHAR(50) NOT NULL,
	isDeleted BIT NOT NULL DEFAULT 0,

	FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (VehicleId) REFERENCES Vehicle(VehicleId),
);
GO

-- Tạo bảng Reviews (Đánh giá)
CREATE TABLE Review (
    ReviewId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL,
    VehicleId UNIQUEIDENTIFIER NOT NULL,
    Rating INT NOT NULL,
    Comment NVARCHAR(200) NOT NULL,
	PushlishDate DATETIME NOT NULL DEFAULT GETDATE(),
	isDeleted BIT NOT NULL DEFAULT 0,

    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (VehicleId) REFERENCES Vehicle(VehicleId),
);
GO

INSERT INTO dbo.VehicleType (VehicleTypeId, VehicleTypeName, isDeleted) VALUES (N'FEEEE076-5169-4C53-BE0A-BB759088B002', N'Ô tô', DEFAULT)
INSERT INTO dbo.VehicleType (VehicleTypeId, VehicleTypeName, isDeleted) VALUES (N'FC5EE9D1-FCF8-4A35-8FBD-E4AE31CC6C16', N'Xe máy', DEFAULT)

INSERT INTO dbo.VehicleVariant (VariantId, VariantName, VehicleTypeId, isDeleted) VALUES (N'2B3D8488-0460-4F8B-A18B-03A5F5665102', N'Số sàn', N'FEEEE076-5169-4C53-BE0A-BB759088B002', DEFAULT)
INSERT INTO dbo.VehicleVariant (VariantId, VariantName, VehicleTypeId, isDeleted) VALUES (N'A476B41D-512B-4262-9F48-A9CB72CA91CA', N'Số tự động', N'FEEEE076-5169-4C53-BE0A-BB759088B002', DEFAULT)
INSERT INTO dbo.VehicleVariant (VariantId, VariantName, VehicleTypeId, isDeleted) VALUES (N'B324DE76-E395-4C1B-B043-1C37B566FA60', N'Xe số', N'FC5EE9D1-FCF8-4A35-8FBD-E4AE31CC6C16', DEFAULT)
INSERT INTO dbo.VehicleVariant (VariantId, VariantName, VehicleTypeId, isDeleted) VALUES (N'6E8A363C-6F94-4712-8D09-9F4E876B7616', N'Xe côn tay', N'FC5EE9D1-FCF8-4A35-8FBD-E4AE31CC6C16', DEFAULT)
INSERT INTO dbo.VehicleVariant (VariantId, VariantName, VehicleTypeId, isDeleted) VALUES (N'4C710385-7A22-4E35-9534-D0FA1B45ABC4', N'Xe ga', N'FC5EE9D1-FCF8-4A35-8FBD-E4AE31CC6C16', DEFAULT)

INSERT INTO dbo.VehicleBrand (BrandId, BrandName, VehicleTypeId, isDeleted) VALUES (N'FB612D38-81CC-4D41-B4E8-D094F57E5D3D', N'Honda', N'FEEEE076-5169-4C53-BE0A-BB759088B002', DEFAULT)
INSERT INTO dbo.VehicleBrand (BrandId, BrandName, VehicleTypeId, isDeleted) VALUES (N'110D3B86-D0F8-47CB-80C4-0CB98444D1BC', N'Toyota', N'FEEEE076-5169-4C53-BE0A-BB759088B002', DEFAULT)
INSERT INTO dbo.VehicleBrand (BrandId, BrandName, VehicleTypeId, isDeleted) VALUES (N'35799DBE-E9D6-48CB-A7D2-FED0D0791F9D', N'HuynhDai', N'FEEEE076-5169-4C53-BE0A-BB759088B002', DEFAULT)
INSERT INTO dbo.VehicleBrand (BrandId, BrandName, VehicleTypeId, isDeleted) VALUES (N'E6B7B09E-1462-4AF2-B1B2-0F1077CFD83F', N'Vinfast', N'FEEEE076-5169-4C53-BE0A-BB759088B002', DEFAULT)

INSERT INTO dbo.VehicleBrand (BrandId, BrandName, VehicleTypeId, isDeleted) VALUES (N'CB07AAE6-4DAA-41E6-9425-6708BAFCC3C9', N'Honda', N'FC5EE9D1-FCF8-4A35-8FBD-E4AE31CC6C16', DEFAULT)
INSERT INTO dbo.VehicleBrand (BrandId, BrandName, VehicleTypeId, isDeleted) VALUES (N'6D8AAE58-7CF1-41D3-A150-58B61B095A13', N'Yamaha', N'FC5EE9D1-FCF8-4A35-8FBD-E4AE31CC6C16', DEFAULT)
INSERT INTO dbo.VehicleBrand (BrandId, BrandName, VehicleTypeId, isDeleted) VALUES (N'605BF24C-2753-4932-B7EA-0A2947A07E22', N'Kawasaki', N'FC5EE9D1-FCF8-4A35-8FBD-E4AE31CC6C16', DEFAULT)

INSERT INTO dbo.Users (UserId, Username, Password, Avatar, Email, FullName, Phone, Address, Role, isDeleted) VALUES (N'F5FC5234-9824-48B4-87D3-1861DF176627', N'admin', N'admin', DEFAULT, N'lepro2883@gmail.com', N'Nguyễn Ngọc Lễ', N'0337378867', N'Khánh Hòa', N'admin', DEFAULT)
INSERT INTO dbo.Users (UserId, Username, Password, Avatar, Email, FullName, Phone, Address, Role, isDeleted) VALUES (N'280FB15A-DAAB-4F65-9DA9-D0ADB3B17745', N'user', N'user', DEFAULT, N'lepro2882@gmail.com', N'Nguyễn Văn A', N'123456', N'Hồ Chí Minh', N'user', DEFAULT)

INSERT INTO dbo.Province (ProvinceId, ProvinceName, isDeleted) VALUES (N'D03F3582-847A-4175-83AF-38748606D774', N'Tp Hồ Chí Minh', DEFAULT)
INSERT INTO dbo.Province (ProvinceId, ProvinceName, isDeleted) VALUES (N'39B62B58-998A-4C97-BDC2-A806F81ABDDB', N'Bình Dương', DEFAULT)
INSERT INTO dbo.Province (ProvinceId, ProvinceName, isDeleted) VALUES (N'E31A9706-4FB4-4159-9D24-F37F4A150FC7', N'Đồng Nai', DEFAULT)
INSERT INTO dbo.Province (ProvinceId, ProvinceName, isDeleted) VALUES (N'8368632A-9606-4335-A0F8-23917F61BDD3', N'Vũng Tàu', DEFAULT)

INSERT INTO dbo.District (DistrictId, DistrictName, ProvinceId, isDeleted) VALUES (N'86075FCD-7A29-4C8D-804F-D6E3E862B6DA', N'Quận 4', N'D03F3582-847A-4175-83AF-38748606D774', DEFAULT)

INSERT INTO dbo.Vehicle (VehicleId, VehicleName, Content, UserId, BrandId, VariantId, ImageVehicle, Year, RegistrationNumber, VehiclePrice, DistrictId, isDeleted) 
VALUES (N'410991D7-FA2C-4351-82B8-F228AAAE67AB', N'VINFAST LUX A 2.0 2020', N'Thông tin chi tiết', N'F5FC5234-9824-48B4-87D3-1861DF176627', N'E6B7B09E-1462-4AF2-B1B2-0F1077CFD83F', N'A476B41D-512B-4262-9F48-A9CB72CA91CA', N'/Content/assets/img/NAck_WBdKcVY88Th9rGAIQ.jpg', 2020, N'60A-917.53', 930000, N'86075FCD-7A29-4C8D-804F-D6E3E862B6DA', DEFAULT)
INSERT INTO dbo.Vehicle (VehicleId, VehicleName, Content, UserId, BrandId, VariantId, ImageVehicle, Year, RegistrationNumber, VehiclePrice, DistrictId, isDeleted) 
VALUES (N'9EEACB0D-90BF-4B04-BE10-1E7E810B35A3', N'HONDA CITY 2019', N'Thông tin chi tiết', N'F5FC5234-9824-48B4-87D3-1861DF176627', N'FB612D38-81CC-4D41-B4E8-D094F57E5D3D', N'A476B41D-512B-4262-9F48-A9CB72CA91CA', N'/Content/assets/img/NAck_WBdKcVY88Th9rGAIQ.jpg', 2019, N'51H-219.66', 750000, N'86075FCD-7A29-4C8D-804F-D6E3E862B6DA', DEFAULT)

INSERT INTO	dbo.Review (ReviewId, UserId, VehicleId, Rating, Comment, PushlishDate, isDeleted) VALUES (DEFAULT, N'280FB15A-DAAB-4F65-9DA9-D0ADB3B17745', N'410991D7-FA2C-4351-82B8-F228AAAE67AB', 5, N'Xe mới, sạch sẽ', DEFAULT, DEFAULT)
INSERT INTO	dbo.Review (ReviewId, UserId, VehicleId, Rating, Comment, PushlishDate, isDeleted) VALUES (DEFAULT, N'280FB15A-DAAB-4F65-9DA9-D0ADB3B17745', N'410991D7-FA2C-4351-82B8-F228AAAE67AB', 3, N'Xe mới, sạch sẽ', DEFAULT, DEFAULT)
INSERT INTO	dbo.Review (ReviewId, UserId, VehicleId, Rating, Comment, PushlishDate, isDeleted) VALUES (DEFAULT, N'280FB15A-DAAB-4F65-9DA9-D0ADB3B17745', N'410991D7-FA2C-4351-82B8-F228AAAE67AB', 2, N'Xe mới, sạch sẽ', DEFAULT, DEFAULT)

INSERT INTO	dbo.Review (ReviewId, UserId, VehicleId, Rating, Comment, PushlishDate, isDeleted) VALUES (DEFAULT, N'280FB15A-DAAB-4F65-9DA9-D0ADB3B17745', N'9EEACB0D-90BF-4B04-BE10-1E7E810B35A3', 3, N'Xe mới, sạch sẽ', DEFAULT, DEFAULT)
INSERT INTO	dbo.Review (ReviewId, UserId, VehicleId, Rating, Comment, PushlishDate, isDeleted) VALUES (DEFAULT, N'280FB15A-DAAB-4F65-9DA9-D0ADB3B17745', N'9EEACB0D-90BF-4B04-BE10-1E7E810B35A3', 4, N'Xe mới, sạch sẽ', DEFAULT, DEFAULT)
INSERT INTO	dbo.Review (ReviewId, UserId, VehicleId, Rating, Comment, PushlishDate, isDeleted) VALUES (DEFAULT, N'280FB15A-DAAB-4F65-9DA9-D0ADB3B17745', N'9EEACB0D-90BF-4B04-BE10-1E7E810B35A3', 5, N'Xe mới, sạch sẽ', DEFAULT, DEFAULT)
INSERT INTO	dbo.Review (ReviewId, UserId, VehicleId, Rating, Comment, PushlishDate, isDeleted) VALUES (DEFAULT, N'280FB15A-DAAB-4F65-9DA9-D0ADB3B17745', N'9EEACB0D-90BF-4B04-BE10-1E7E810B35A3', 1, N'Xe mới, sạch sẽ', DEFAULT, DEFAULT)
INSERT INTO	dbo.Review (ReviewId, UserId, VehicleId, Rating, Comment, PushlishDate, isDeleted) VALUES (DEFAULT, N'280FB15A-DAAB-4F65-9DA9-D0ADB3B17745', N'9EEACB0D-90BF-4B04-BE10-1E7E810B35A3', 5, N'Xe mới, sạch sẽ', DEFAULT, DEFAULT)
