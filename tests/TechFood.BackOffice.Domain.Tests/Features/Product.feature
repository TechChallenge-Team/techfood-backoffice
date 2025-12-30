#language: pt-BR
Funcionalidade: Gerenciamento de Produtos
  Como um administrador do restaurante
  Eu quero gerenciar produtos
  Para que eu possa manter o cardápio atualizado

Cenário: Criar um produto com parâmetros válidos
  Dado que eu tenho um nome de produto "X-Burguer"
  E eu tenho uma descrição de produto "Delicioso hambúrguer"
  E eu tenho um ID de categoria válido
  E eu tenho um nome de arquivo de imagem de produto "burger.png"
  E eu tenho um preço de 19.99
  Quando eu criar o produto
  Então o produto deve ser criado com sucesso
  E o nome do produto deve ser "X-Burguer"
  E a descrição do produto deve ser "Delicioso hambúrguer"
  E o ID da categoria do produto não deve ser vazio
  E o nome do arquivo de imagem do produto deve ser "burger.png"
  E o preço do produto deve ser 19.99
  E o produto deve estar em estoque

Cenário: Múltiplos produtos devem ter IDs únicos
  Dado que eu criei um produto chamado "Produto 1"
  E eu criei outro produto chamado "Produto 2"
  Então cada produto deve ter um ID único
  E os IDs dos produtos não devem ser vazios

Esquema do Cenário: Criar produtos com diferentes preços válidos
  Dado que eu tenho um nome de produto "Test Product"
  E eu tenho uma descrição de produto "Description"
  E eu tenho um ID de categoria válido
  E eu tenho um nome de arquivo de imagem de produto "test.png"
  E eu tenho um preço de <preco>
  Quando eu criar o produto
  Então o produto deve ser criado com sucesso
  E o preço do produto deve ser <preco>
  
  Exemplos:
    | preco  |
    | 0.01   |
    | 1.00   |
    | 99.99  |
    | 999.99 |

Cenário: Marcar produto como fora de estoque
  Dado que eu tenho um produto válido em estoque
  Quando eu marcar o produto como fora de estoque
  Então o produto deve estar fora de estoque

Cenário: Marcar produto como em estoque
  Dado que eu tenho um produto válido fora de estoque
  Quando eu marcar o produto como em estoque
  Então o produto deve estar em estoque

Cenário: Produto deve estar em estoque por padrão
  Dado que eu tenho um nome de produto "New Product"
  E eu tenho uma descrição de produto "Description"
  E eu tenho um ID de categoria válido
  E eu tenho um nome de arquivo de imagem de produto "image.png"
  E eu tenho um preço de 19.99
  Quando eu criar o produto
  Então o produto deve estar em estoque

Cenário: Criar produto com nome longo
  Dado que eu tenho um nome de produto com 200 caracteres
  E eu tenho uma descrição de produto "Description"
  E eu tenho um ID de categoria válido
  E eu tenho um nome de arquivo de imagem de produto "image.png"
  E eu tenho um preço de 19.99
  Quando eu criar o produto
  Então o produto deve ser criado com sucesso
  E o nome do produto deve ter 200 caracteres

Cenário: Criar produto com caracteres especiais
  Dado que eu tenho um nome de produto "X-Burguer & Café"
  E eu tenho uma descrição de produto "Delicioso hambúrguer com café"
  E eu tenho um ID de categoria válido
  E eu tenho um nome de arquivo de imagem de produto "burger_cafe.png"
  E eu tenho um preço de 19.99
  Quando eu criar o produto
  Então o produto deve ser criado com sucesso
  E o nome do produto deve ser "X-Burguer & Café"
  E a descrição do produto deve ser "Delicioso hambúrguer com café"
