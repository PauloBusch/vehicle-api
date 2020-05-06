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

-- Cores
create table colors (
	id char(8) not null,
	name varchar(50) not null,
	hex char(7) not null,
	
	constraint PK_colors primary key(id)
);
alter table colors add unique index UQ_colors_name(name);

-- Combustíveis
create table fuels(
	id char(8) not null,
	name varchar(50) not null,
	
	constraint PK_fuels primary key(id)
);
alter table fuels add unique index UQ_fuels_name(name);

-- Tipos de veículos
create table vehicles_types (
	id char(8) not null,
	name varchar(50) not null,
	
	constraint PK_vehicles_types primary key(id)
);
alter table vehicles_types add unique index UQ_vehicles_types_name(name);

-- Veículos
create table vehicles(
	id char(8) not null,
	year int not null,    
	id_type char(8) not null,
	id_brand char(8) not null,
	id_model char(8) not null,
	id_color char(8) not null,
	id_fuel char(8) not null,
	
	constraint PK_vehicles primary key(id),
	constraint FK_vehicles_id_type foreign key(id_type) references vehicles_types(id),
	constraint FK_vehicles_id_brand foreign key(id_brand) references brands(id),
	constraint FK_vehicles_id_model foreign key(id_model) references models(id),
	constraint FK_vehicles_id_color foreign key(id_color) references colors(id),
	constraint FK_vehicles_id_fuel foreign key(id_fuel) references fuels(id)
);

-- Anuncios
create table announcements(
	id char(8) not null,
	price_purchase decimal(8, 2) not null,
	price_sale decimal(8, 2) not null,
	date_sale date null,
	id_vehicle char(8) not null,
	
	constraint PK_announcements primary key(id),
	constraint FK_announcements_id_vehicle foreign key(id_vehicle) references vehicles(id)
);
alter table announcements add index IDX_announcements_date_sale(date_sale);

-- DEFAULT VALUES ------------------------------------
insert into colors(id, name, hex) values
(new_id(), 'Branco', '#ffffff'),
(new_id(), 'Prata', '#bdbec0'),
(new_id(), 'Preto', '#000000'),
(new_id(), 'Cinza', '#939598'),
(new_id(), 'Marrom', '#b18457'),
(new_id(), 'Vermelho', '#cc1e2b'),
(new_id(), 'Azul', '#264599'),
(new_id(), 'Verde', '#00a651');

insert into fuels(id, name) values
(new_id(), 'Álcool'),
(new_id(), 'Gasolina'),
(new_id(), 'Álcool / Gasolina');

-- VIEWS ---------------------------------------------

-- TRIGGERS ------------------------------------------