Create dAtAbAsE Organix

use Organix

create table usuario(
	id_usuario int identity primary key,
	nome varchar(255) not null,
	CPF_CNPJ varchar(14) not null,
	email varchar(255) not null,
	senha varchar(255) not null,
	id_tipo int foreign key references tipo(id_tipo),
);
create table telefone(
	id_telefone int identity primary key,
	telefone varchar(255) not null,
	celular varchar(255) not null,
	id_usuario int foreign key references usuario(id_usuario)
);
create table endereco(
	id_endereco int identity primary key,
	CEP varchar(9) not null,
	rua varchar(255) not null,
	bairro varchar(255) not null,
	cidade varchar(255) not null,
	estado varchar(255) not null,
	regiao varchar(255) not null,
	id_usuario int foreign key references usuario(id_usuario)
);
create table pedido(
	id_pedido int identity primary key,
	data_pedido date not null,
	status_pedido varchar(255) not null,
	id_usuario int foreign key references usuario(id_usuario)
);
create table item_pedido(
	id_item_pedido int identity primary key,
	quantidade varchar(255) not null,
	id_pedido int foreign key references pedido(id_pedido),
	id_oferta int foreign key references oferta(id_oferta)
);
create table oferta(
	id_oferta int identity primary key,
	estado_produto varchar(255) not null,
	preco money not null,
	data_fabricacao date not null,
	data_vencimento date not null,
	id_usuario int foreign key references usuario(id_usuario),
	id_produto int foreign key references produto(id_produto)
);
create table receita(
	id_receita int identity primary key,
	nome_receita varchar(255) not null,
	ingredientes text not null,
	tempo_preparo varchar(255) not null,
	porcoes varchar(255) not null,
	modo_preparo text not null,
	id_usuario int foreign key references usuario(id_usuario),
	id_categoria_receita int foreign key references categoria_receita(id_categoria_receita)
);
create table categoria_receita(
	id_categoria_receita int identity primary key,
	nome_categoria varchar(255)
);
create table produto(
	id_produto int identity primary key,
	nome_produto varchar(255)
);
create table tipo(
	id_tipo int identity primary key,
	perfil varchar(255) not null
);