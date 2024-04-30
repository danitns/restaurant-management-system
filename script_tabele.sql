use restaurant;

drop table Bill_Product;
drop table Bill;
drop table Reservation;
drop table [Table];
drop table Product;
drop table Subcategory;
drop table Category;
drop table [User];
drop table [Role];

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
	constraint FK_USER_ROLE foreign key (RoleId) references [Role](Id),
	constraint UQ_USER_EMAIL unique(Email),
	constraint UQ_USER_PHONE unique(Phone)
)

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
	Price money not null,
	constraint FK_PRODUCT_SUBCATEGORY foreign key (SubcategoryId) references Subcategory(Id),
	constraint UQ_PRODUCT_NAME unique([Name])
)

create table [Table] (
	Id uniqueidentifier primary key,
	[Name] nvarchar(255) not null,
	Seats int not null,
	constraint UQ_TABLE_NAME unique([Name])
)

create table Reservation (
	Id uniqueidentifier primary key,
	UserId uniqueidentifier,
	Phone nvarchar(15),
	TableId uniqueidentifier,
	constraint FK_RESERVATION_USER foreign key (UserId) references [User](Id),
	constraint FK_RESERVATION_TABLE foreign key (TableId) references [Table](Id)
)

create table Bill (
	Id uniqueidentifier primary key,
	TableId uniqueidentifier not null,
	constraint FK_BILL_RESERVATION foreign key (Id) references Reservation(Id)
)

create table Bill_Product (
	BillId uniqueidentifier,
	ProductId uniqueidentifier,
	Quantity int not null,
	constraint PK_BILL_PRODUCT primary key(BillId, ProductId),
	constraint FK_BILL_PRODUCT_BILL foreign key (BillId) references Bill(Id),
	constraint FK_BILL_PRODUCT_PRODUCT foreign key (ProductId) references Product(Id)
)

insert into [Role] values
(1, 'User'),
(2, 'Waiter'),
(3, 'Admin')

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