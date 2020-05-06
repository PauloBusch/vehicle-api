create database questor_vehicle;
create user 'questor'@'%' identified by '123';
grant all privileges on *.* to 'questor'@'%' with grant option;