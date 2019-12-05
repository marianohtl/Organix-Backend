insert into tipo(perfil) values ('comprador'), ('produtor'), ('admin')


insert into usuario(nome,CPF_CNPJ,email,senha,id_tipo) values ('Neeko', '76699934501','neeko@gmail.com','123',1),
	('Orianna','23345589902','orianna@gmail.com','321',2),
	('Syndra','12345677702','syndra@gmail.com','456',3);


insert into telefone(telefone, celular, id_usuario) values ('1158751040', '11998421371', 2), ('1138451240', '11993421340', 2), ('1128451340', '11928421901', 3)

insert into endereco(CEP, rua, bairro, cidade, estado, regiao, id_usuario) values ('22333000', 'n sei', 'n sei oq', 'SP', 'SP', 'Sul', 2),
 ('22443000', 'seila', 'seila oq', 'SP', 'SP', 'Leste', 3)


insert into pedido(data_pedido, status_pedido, id_usuario) values ('17/10/19','Processando', 2), ('01/10/19', 'Enviado', 3)

insert into produto(nome_produto) values ('Cenoura'), ('Batata')

insert into oferta(id_usuario, id_produto, estado_produto, preco, data_fabricacao, data_vencimento) values (3, 1, 'Bom', 1.99, '09/09/19', '12/12/19'),
(3, 1, 'Bom', 1.99, '09/09/19', '12/12/19')

insert into item_pedido(quantidade, id_pedido, id_oferta) values ('3KG', 1, 3), ('4 unidades', 1, 3)


insert into categoria_receita(nome_categoria) values ('Sopas'), ('Bolos'), ('Saladas'), ('Massas'), ('Tortas'), ('Lanches')

insert into receita(id_usuario, id_categoria_receita, nome_receita, ingredientes, tempo_preparo, porcoes, modo_preparo) values (2, 1, 'Sopa de macaco','Macaco, Agua, temperos', '20 min', '3', '...'),
(3, 2, 'Bolo de bolo', 'Bolos', '1 hora', '10', '...')


ALTER TABLE receita ADD imagem VARCHAR(255)
UPDATE receita SET imagem = 'imagem.jpg' WHERE id_receita > 0;


select * from endereco;
select * from receita;
select * from usuario;
select * from categoria_receita;
select * from produto;
select * from oferta;
select * from telefone;
select * from item_pedido;
select * from pedido;
