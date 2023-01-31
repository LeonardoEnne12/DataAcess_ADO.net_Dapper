SELECT * FROM [Post]
SELECT * FROM [User]
SELECT * FROM [Category]
SELECT * FROM [Tag]
SELECT * FROM [Role]
SELECT * FROM [PostTag]


INSERT INTO [Post] ([CategoryId],[AuthorId],[Title],[Summary],[Body],[Slug])
VALUES(
    1,
    1,
    'Teste : Começando com EF Core',
    'Teste : Neste artigo vamos aprender EF core',
    '<p>Hello world Forever</p>',
    'teste-comecando-ef-core'
)

INSERT INTO [PostTag] ([PostId],[TagId])
VALUES(
    1,
    3
)

INSERT INTO [PostTag] ([PostId],[TagId])
VALUES(
    5,
    3
)