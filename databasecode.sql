

Create database userManagement
use userManagement

create table users(
	id int primary key identity,
	name varchar(25) not null,
	email varchar(25) unique not null,
	password varchar(25) not null,
	contact varchar(25) not null,
	gender bit not null,
)
use userManagement
select * from users

create table roles(
	id int primary key identity,
	name varchar(25) unique not null
)

select * from roles
insert into roles values ('Auditor')
insert into roles values ('AuditorReViewer')
insert into roles values ('Sales')
insert into roles values ('Admin')

update roles set name = 'Admin'
where id = 4;



create table usersRoles(
	id int primary key identity,
	userId int foreign key references users(id),
	roleId int foreign key references roles(id)
)

select * from roles
select * from usersRoles

delete usersRoles where id > 14

insert into usersRoles values (1,1)  -- user , role
insert into usersRoles values (2,2)  -- user , role
insert into usersRoles values (3,3)  -- user , role
insert into usersRoles values (4,4)  -- user , role
insert into usersRoles values (4,3)


create table products(
	id int primary key identity,
	name varchar(25),
	title varchar(40),
	price money,
	logDescription varchar(500),
	shortDescription varchar(100),
	qrcode varchar(25),
	imageURL varchar(500)
)

select * from products

insert into products 
values 
('P1','P1 Product 1' , 2500,'The is Long Description The is Long Description The is Long Description','short Descriptionshort Descriptionshort Descriptionshort Description','qr23453rdp','https://th.bing.com/th/id/R.1fa573f73a62f6d09965ed6b8ef35a95?rik=QFHButRDG1ChTg&pid=ImgRaw&r=0'),
('P2','P2 Product 2' , 22500,'The is Long Description The is Long Description The is Long Description','short Descriptionshort Descriptionshort Descriptionshort Description','qr23453rdp','https://th.bing.com/th/id/R.1fa573f73a62f6d09965ed6b8ef35a95?rik=QFHButRDG1ChTg&pid=ImgRaw&r=0'),
('P3','P2 Product 3' , 500,'The is Long Description The is Long Description The is Long Description','short Descriptionshort Descriptionshort Descriptionshort Description','qr23453rdp','https://th.bing.com/th/id/R.1fa573f73a62f6d09965ed6b8ef35a95?rik=QFHButRDG1ChTg&pid=ImgRaw&r=0'),
('P4','P3 Product 4' , 2500,'The is Long Description The is Long Description The is Long Description','short Descriptionshort Descriptionshort Descriptionshort Description','qr23453rdp','https://th.bing.com/th/id/R.1fa573f73a62f6d09965ed6b8ef35a95?rik=QFHButRDG1ChTg&pid=ImgRaw&r=0'),
('P5','P4 Product 5' , 25400,'The is Long Description The is Long Description The is Long Description','short Descriptionshort Descriptionshort Descriptionshort Description','qr23453rdp','https://th.bing.com/th/id/R.1fa573f73a62f6d09965ed6b8ef35a95?rik=QFHButRDG1ChTg&pid=ImgRaw&r=0'),
('P6','P5 Product 6' , 2500,'The is Long Description The is Long Description The is Long Description','short Descriptionshort Descriptionshort Descriptionshort Description','qr23453rdp','https://th.bing.com/th/id/R.1fa573f73a62f6d09965ed6b8ef35a95?rik=QFHButRDG1ChTg&pid=ImgRaw&r=0'),
('P7','P6 Product 7' , 22500,'The is Long Description The is Long Description The is Long Description','short Descriptionshort Descriptionshort Descriptionshort Description','qr23453rdp','https://th.bing.com/th/id/R.1fa573f73a62f6d09965ed6b8ef35a95?rik=QFHButRDG1ChTg&pid=ImgRaw&r=0')

drop table products







