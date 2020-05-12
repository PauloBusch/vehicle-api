-- DATABASES ------------------------------------------
create database questor_vehicle;

-- USERS DATABASE -------------------------------------
create user 'questor'@'%' identified by '123';
grant all privileges on *.* to 'questor'@'%' with grant option;

-- FUNCTIONS -----------------------------------------
DELIMITER $$
	CREATE FUNCTION new_id() RETURNS CHAR(8)
	DETERMINISTIC
	BEGIN return (select LEFT(UUID(), 8));
end$$

-- TABLES --------------------------------------------
-- Usuários
create table users(
	id char(8) not null, 
    name varchar(150) not null,
	login varchar(50) not null,
    password varchar(80) not null,
    
    constraint PK_users primary key(id)
);
alter table users add unique index UQ_users_login(login);

-- Marcas
create table brands(
	id char(8) not null,
	name varchar(200) not null,
	
	constraint PK_brands primary key(id)
);
alter table brands add unique index UQ_brands_name(name);

-- Modelos
create table models (
	id char(8) not null,
	name varchar(200) not null,
	
	constraint PK_models primary key(id)
);
alter table models add unique index UQ_models_name(name);

-- Cores: Enum
create table colors (
	id int not null auto_increment,
	name varchar(50) not null,
	hex char(7) not null,
	
	constraint PK_colors primary key(id)
);
alter table colors add unique index UQ_colors_name(name);

-- Combustíveis: Enum
create table fuels(
	id int not null auto_increment,
	name varchar(50) not null,
	
	constraint PK_fuels primary key(id)
);
alter table fuels add unique index UQ_fuels_name(name);

-- Veículos
create table vehicles(
	id char(8) not null,
	year int not null,   
	id_fuel int not null,
	id_color int not null,
	id_brand char(8) not null,
	id_model char(8) not null,
    date_creation datetime default current_timestamp,
	
	constraint PK_vehicles primary key(id),
	constraint FK_vehicles_id_brand foreign key(id_brand) references brands(id) on delete cascade,
	constraint FK_vehicles_id_model foreign key(id_model) references models(id) on delete cascade,
	constraint FK_vehicles_id_color foreign key(id_color) references colors(id) on delete cascade,
	constraint FK_vehicles_id_fuel foreign key(id_fuel) references fuels(id) on delete cascade
);

-- Anuncios
create table announcements(
	id char(8) not null,
	price_purchase decimal(8, 2) not null,
	price_sale decimal(8, 2) not null,
	date_sale date null,
	id_vehicle char(8) not null,
    date_creation datetime default current_timestamp,
	
	constraint PK_announcements primary key(id),
	constraint FK_announcements_id_vehicle foreign key(id_vehicle) references vehicles(id) on delete cascade
);
alter table announcements add index IDX_announcements_date_sale(date_sale);

create table contacts (
	id char(8) not null,
    name varchar(150) not null,
    phone varchar(15) not null,
    
    constraint PK_contacts primary key(id)
);
alter table contacts add constraint unique index UQ_contacts_name(name);
alter table contacts add constraint unique index UQ_contacts_phone(phone);

create table reservations (
	id char(8) not null,
    id_announcement char(8) not null,
    id_contact char(8) not null,
    date_sale datetime null,
    date_creation datetime default current_timestamp,
    
    constraint PK_reservations primary key(id),
    constraint FK_reservations_id_announcement foreign key (id_announcement) references announcements(id) on delete cascade,
    constraint FK_reservations_id_contact foreign key (id_contact) references contacts(id) on delete cascade
);
alter table reservations add index IDX_reservations_date_sale(date_sale);

-- DEFAULT VALUES ------------------------------------
insert into users(id, login, name, password) values
(new_id(), 'joão', 'João', 'InddsksGcJidlvfsjkvTdg==');

insert into colors(id, name, hex) values
(1, 'Branco', '#ffffff'),
(2, 'Prata', '#bdbec0'),
(3, 'Preto', '#000000'),
(4, 'Cinza', '#939598'),
(5, 'Marrom', '#b18457'),
(6, 'Vermelho', '#cc1e2b'),
(7, 'Azul', '#264599'),
(8, 'Verde', '#00a651');

insert into fuels(id, name) values
(1, 'Álcool'),
(2, 'Gasolina'),
(3, 'Álcool / Gasolina');

-- VIEWS ---------------------------------------------

-- TRIGGERS ------------------------------------------