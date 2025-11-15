use MakarovKV
drop table if exists Requests, Users;

create table Users
(
	ID int Identity(1,1) primary key,
	Last_Name nvarchar(40) not null, --Фамилия
	FirstName nvarchar(40) not null, --Имя
	MiddleName nvarchar(40) default '', --Отчество
	Login nvarchar(40) not null unique,
	Password nvarchar(20) not null,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('admin', 'client', 'cleaner')),
	CreatedAt datetime default getdate()
)

insert into Users(Last_Name, FirstName, MiddleName, Login, Password, Role) values
('Иванов', 'Иван', '', 'CAT', '123', 'admin'),
('Петров', 'Пётр', 'Петрович', 'Dog', '123', 'client'),
('Сидоров', 'Дмитрий', 'Анатольевич', 'Parrot', '123', 'cleaner')
select * from Users

create table Requests
(
	ID int identity(1,1) primary key,
	UserID int,
	Area int not null,
	RequestsServicesId int,
	CleaningDate date,
	CityId int,
	District nvarchar(30)
)
ALTER TABLE Requests ADD FOREIGN KEY (UserId) REFERENCES Users(id);
alter table Requests add foreign key (RequestsServicesId) references RequestServices(ID)
alter table requests add foreign key (CityId) references Cities(ID)