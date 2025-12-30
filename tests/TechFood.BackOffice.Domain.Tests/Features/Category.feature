#language: pt-BR
Funcionalidade: Gerenciamento de Categorias
  Como um administrador do restaurante
  Eu quero gerenciar categorias de produtos
  Para que eu possa organizar os itens do menu

Cenário: Criar uma categoria com parâmetros válidos
  Dado que eu tenho um nome de categoria "Lanche"
  E eu tenho um nome de arquivo de imagem "lanche.png"
  E eu tenho uma ordem de classificação de 0
  Quando eu criar a categoria
  Então a categoria deve ser criada com sucesso
  E o nome da categoria deve ser "Lanche"
  E o nome do arquivo de imagem deve ser "lanche.png"
  E a ordem de classificação deve ser 0

Cenário: Criar uma categoria com nome inválido
  Dado que eu tenho um nome de categoria vazio
  E eu tenho um nome de arquivo de imagem "lanche.png"
  E eu tenho uma ordem de classificação de 0
  Quando eu tentar criar a categoria
  Então uma exceção de domínio deve ser lançada



Esquema do Cenário: Criar categoria com ordem de classificação negativa
  Dado que eu tenho um nome de categoria "Lanche"
  E eu tenho um nome de arquivo de imagem "lanche.png"
  E eu tenho uma ordem de classificação de <ordem>
  Quando eu tentar criar a categoria
  Então uma exceção de domínio deve ser lançada
  
  Exemplos:
    | ordem |
    | -1    |
    | -10   |

Cenário: Múltiplas categorias devem ter IDs únicos
  Dado que eu criei uma categoria chamada "Lanche"
  E eu criei outra categoria chamada "Bebida"
  Então cada categoria deve ter um ID único
  E os IDs não devem ser vazios

Esquema do Cenário: Criar categorias com diferentes ordens de classificação válidas
  Dado que eu tenho um nome de categoria "Categoria Teste"
  E eu tenho um nome de arquivo de imagem "test.png"
  E eu tenho uma ordem de classificação de <ordem>
  Quando eu criar a categoria
  Então a categoria deve ser criada com sucesso
  E a ordem de classificação deve ser <ordem>
  
  Exemplos:
    | ordem |
    | 0     |
    | 1     |
    | 99    |
    | 1000  |

Cenário: Criar categoria com nome longo
  Dado que eu tenho um nome de categoria com 100 caracteres
  E eu tenho um nome de arquivo de imagem "image.png"
  E eu tenho uma ordem de classificação de 0
  Quando eu criar a categoria
  Então a categoria deve ser criada com sucesso
  E o nome da categoria deve ter 100 caracteres

Cenário: Criar categoria com caracteres especiais
  Dado que eu tenho um nome de categoria "Café & Açaí"
  E eu tenho um nome de arquivo de imagem "café_açaí.png"
  E eu tenho uma ordem de classificação de 0
  Quando eu criar a categoria
  Então a categoria deve ser criada com sucesso
  E o nome da categoria deve ser "Café & Açaí"
  E o nome do arquivo de imagem deve ser "café_açaí.png"

Cenário: Múltiplas categorias podem ter a mesma ordem de classificação
  Dado que eu criei uma categoria "Categoria 1" com ordem 1
  E eu criei uma categoria "Categoria 2" com ordem 1
  Então ambas categorias devem ter a ordem de classificação 1
