use restaurant;

drop table if exists UserBenefit;
drop table if exists Benefit;
drop table if exists ProductReview;
drop table if exists Review;
drop table if exists Reservation;
drop table if exists [Table];
drop table if exists Product;
drop table if exists Subcategory;
drop table if exists Category;
drop table if exists RestaurantSchedule;
drop table if exists Restaurant;
drop table if exists RestaurantType;
drop table if exists City;
drop table if exists [User];
drop table if exists [Role];

create table [Role] (
	Id int primary key,
	[Name] nvarchar(255) not null,
	constraint UQ_ROLE_NAME unique([Name])
)

create table [User] (
	Id uniqueidentifier primary key,
	RoleId int not null,
	FirstName nvarchar(255) not null,
	LastName nvarchar(255) not null,
	Email nvarchar(255) not null,
	[Password] nvarchar(max) not null,
	PasswordHash uniqueidentifier not null,
	Phone nvarchar(15) not null,
	Birthdate date not null,
	Points int not null default(0),
	constraint FK_USER_ROLE foreign key (RoleId) references [Role](Id),
	constraint UQ_USER_EMAIL unique(Email),
	constraint UQ_USER_PHONE unique(Phone)
)

create table [City] (
	Id int primary key,
	[Name] nvarchar(255) not null,
	constraint UQ_CITY_NAME unique([Name])
)

create table RestaurantType (
	Id int primary key,
	[Name] nvarchar(255) not null,
	constraint UQ_RESTAURANT_TYPE_NAME unique([Name])
)

create table Restaurant (
	Id uniqueidentifier primary key,
	Picture varbinary(max),
	[Name] nvarchar(255) not null,
	[Address] nvarchar(255) not null,
	[Description] nvarchar(max) not null,
	RestaurantTypeId int not null,
	CityId int not null,
	UserId uniqueidentifier not null,
	constraint FK_RESTAURANT_CITY foreign key(CityId) references City(Id),
	constraint FK_RESTAURANT_USER foreign key (UserId) references [User](Id),
	constraint FK_RESTAURANT_RESTAURANT_TYPE foreign key(RestaurantTypeId) references RestaurantType(Id),
	constraint UQ_RESTAURANT_NAME unique([Name])
)

create table RestaurantSchedule (
    Id uniqueidentifier PRIMARY KEY,
    RestaurantId uniqueidentifier NOT NULL,
    DayOfWeek int NOT NULL,
    OpeningTime time NOT NULL,
    ClosingTime time NOT NULL,
    CONSTRAINT FK_RESTAURANTSCHEDULE_RESTAURANT FOREIGN KEY (RestaurantId) REFERENCES Restaurant(Id),
    CONSTRAINT UQ_RESTAURANTSCHEDULE_UNIQUE UNIQUE (RestaurantId, DayOfWeek)
);

create table Category (
	Id int primary key,
	[Name] nvarchar(255) not null,
	constraint UQ_CATEGORY_NAME unique([Name])
)

create table Subcategory (
	Id int primary key,
	CategoryId int not null,
	[Name] nvarchar(255) not null,
	constraint FK_SUBCATEOGRY_CATEGORY foreign key (CategoryId) references Category(Id),
	constraint UQ_SUBCATEGORY_NAME unique([Name])
)

create table Product (
	Id uniqueidentifier primary key,
	Picture varbinary(max),
	[Name] nvarchar(255) not null,
	SubcategoryId int not null,
	RestaurantId uniqueidentifier not null,
	Price money not null,
	constraint FK_PRODUCT_SUBCATEGORY foreign key (SubcategoryId) references Subcategory(Id),
	constraint FK_PRODUCT_RESTAURANT foreign key (RestaurantId) references Restaurant(Id),
	constraint UQ_PRODUCT_NAME unique([Name], RestaurantId)
)

create table [Table] (
	Id uniqueidentifier primary key,
	[Name] nvarchar(255) not null,
	Seats int not null,
	RestaurantId uniqueidentifier not null,
	constraint FK_TABLE_RESTAURANT foreign key (RestaurantId) references Restaurant(Id),
	constraint UQ_TABLE_NAME unique([Name], RestaurantId)
)

create table Reservation (
	Id uniqueidentifier primary key,
	UserId uniqueidentifier,
	Phone nvarchar(15),
	TableId uniqueidentifier,
	[Date] datetime not null,
	[Status] int not null default(0),
	TotalAmount int,
	constraint FK_RESERVATION_USER foreign key (UserId) references [User](Id),
	constraint FK_RESERVATION_TABLE foreign key (TableId) references [Table](Id)
)

create table Review(
	Id uniqueidentifier primary key,
	Rating int,
	[Text] nvarchar(500)
	constraint FK_REVIEW_RESERVATION foreign key (Id) references Reservation(Id)
)

create table ProductReview(
	Id uniqueidentifier primary key,
	ReviewId uniqueidentifier,
	ProductId uniqueidentifier,
	ImageContent varbinary(max),
	[Text] nvarchar(500),
	Rating int,
	Quantity int,
	constraint FK_PRODUCTREVIEW_REVIEW foreign key (ReviewId) references Review(Id),
	constraint FK_PRODUCTREVIEW_PRODUCT foreign key (ProductId) references Product(Id)
)

create table Benefit(
	Id uniqueidentifier primary key,
	RestaurantId uniqueidentifier,
	[Name] nvarchar(128) not null,
	Details nvarchar(500) not null,
	[Value] int not null,
	constraint FK_BENEFIT_RESTAURANT foreign key (RestaurantId) references Restaurant(Id)
)

create table UserBenefit(
	Id uniqueidentifier primary key,
	UserId uniqueidentifier,
	BenefitId uniqueidentifier,
	ExpiryDate datetime,
	UsedOn datetime,
	IsActive bit,
	constraint FK_USERBENEFIT_USER foreign key (UserId) references [User](Id),
	constraint FK_USERBENEFIT_BENEFIT foreign key (BenefitId) references Benefit(Id)
)

insert into City values 
(1, 'Bucharest'),
(2, 'Cluj'),
(3, 'Timisoara'),
(4, 'Brasov')

insert into [Role] values
(1, 'User'),
(2, 'Manager'),
(3, 'Admin'),
(4, 'PendingManager')

insert into [Category] values
(1, 'Drinks'),
(2, 'Food')

insert into [Subcategory] values
(1, 1, 'Soft Drinks'),
(2, 1, 'Coffee'),
(3, 1, 'Cocktails'),
(4, 1, 'Spirits'),
(5, 1, 'Beer'),
(6, 1, 'Shots'),
(7, 2, 'Starter'),
(8, 2, 'Pizza'),
(9, 2, 'Pasta'),
(10, 2, 'Burger'),
(11, 2, 'Dessert'),
(12, 2, 'Breakfast'),
(13, 2, 'Side'),
(14, 2, 'Main Course')

insert into RestaurantType values
(1, 'Restaurant'),
(2, 'Bar'),
(3, 'Pub'),
(4, 'CoffeeShop'),
(5, 'Bistro')



--INSERT INTO Benefit (Id, RestaurantId, Name, Details, Value)  
--VALUES  
--(NEWID(), '8B1F82F5-3B5F-451F-8A52-7C1F85E2AF6E', '10% Discount', 'This voucher gives you 10% discount on your next order', 10),  
--(NEWID(), '8B1F82F5-3B5F-451F-8A52-7C1F85E2AF6E', '15% Discount', 'This voucher gives you 15% discount', 15),  
--(NEWID(), '8B1F82F5-3B5F-451F-8A52-7C1F85E2AF6E', '20% Discount', 'This voucher gives you 20% discount on your next order', 20),  
--(NEWID(), '8B1F82F5-3B5F-451F-8A52-7C1F85E2AF6E', '25% Discount', 'This voucher gives you 25% discount', 25),  
--(NEWID(), '8B1F82F5-3B5F-451F-8A52-7C1F85E2AF6E', '30% Discount', 'This voucher gives you 30% discount', 30),  
--(NEWID(), '8B1F82F5-3B5F-451F-8A52-7C1F85E2AF6E', '35% Discount', 'This voucher gives you 35% discount', 35),  
--(NEWID(), '8B1F82F5-3B5F-451F-8A52-7C1F85E2AF6E', '40% Discount', 'This voucher gives you 40% discount', 40),  
--(NEWID(), '8B1F82F5-3B5F-451F-8A52-7C1F85E2AF6E', '45% Discount', 'This voucher gives you 45% discount', 45),  
--(NEWID(), '8B1F82F5-3B5F-451F-8A52-7C1F85E2AF6E', '50% Discount', 'This voucher gives you 50% discount', 50),  
--(NEWID(), '8B1F82F5-3B5F-451F-8A52-7C1F85E2AF6E', '55% Discount', 'This voucher gives you 55% discount', 55);

--INSERT INTO UserBenefit (Id, BenefitId, UserId, ExpiryDate, IsActive) 
--VALUES 
--(NEWID(), '698DD571-B9A9-46E3-9455-368BCC2CA2A3', '3F2860A5-00D5-408D-955D-E2230905E04F', DATEADD(MONTH, 1, GETDATE()), 0),
--(NEWID(), '196AD77B-83AC-474E-B78C-3EAD9FD59A6E', '3F2860A5-00D5-408D-955D-E2230905E04F', DATEADD(MONTH, 1, GETDATE()), 0),
--(NEWID(), '9A8E1DA0-4FB8-4137-B72D-4460979637AC', '3F2860A5-00D5-408D-955D-E2230905E04F', DATEADD(MONTH, 1, GETDATE()), 0),
--(NEWID(), '0E221A15-0625-418A-B1CA-4A0DD5F6203E', '3F2860A5-00D5-408D-955D-E2230905E04F', DATEADD(MONTH, 1, GETDATE()), 0),
--(NEWID(), '0299E89E-CDA7-459A-B9CD-50E0D488D571', '3F2860A5-00D5-408D-955D-E2230905E04F', DATEADD(MONTH, 1, GETDATE()), 0),
--(NEWID(), 'B2D58B88-FC61-435F-9F7A-5172BBCC5F32', '3F2860A5-00D5-408D-955D-E2230905E04F', DATEADD(MONTH, 1, GETDATE()), 0),
--(NEWID(), '2E2DA7AC-50EF-4601-BBA2-72779438936E', '3F2860A5-00D5-408D-955D-E2230905E04F', DATEADD(MONTH, 1, GETDATE()), 0),
--(NEWID(), '7EE569E2-DA50-4619-BBEB-871023E1AB9A', '3F2860A5-00D5-408D-955D-E2230905E04F', DATEADD(MONTH, 1, GETDATE()), 0),
--(NEWID(), '65E9891A-8CDC-4803-B4D1-8F9552FA8AB8', '3F2860A5-00D5-408D-955D-E2230905E04F', DATEADD(MONTH, 1, GETDATE()), 0),
--(NEWID(), 'DB1B9134-5C41-4E3A-8B9E-96F713EBE6DB', '3F2860A5-00D5-408D-955D-E2230905E04F', DATEADD(MONTH, 1, GETDATE()), 0);