create database dbcadastro;
use dbcadastro;
create table Usuarios(
Id int primary key auto_increment,
Nome varchar(200) not null,
Email varchar(200) not null,
Senha varchar(50) not null);

create table Produtos(
Id int primary key auto_increment,
Nome varchar(200) not null,
Descricao varchar(200) not null,
Preco decimal(10,2) not null,
Quantidade int not null);
