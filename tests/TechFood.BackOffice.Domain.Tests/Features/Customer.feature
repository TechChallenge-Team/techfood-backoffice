#language: pt-BR
Funcionalidade: Gerenciamento de Clientes
  Como um administrador do restaurante
  Eu quero gerenciar clientes
  Para que eu possa manter o cadastro atualizado

Cenário: Criar um cliente com parâmetros válidos incluindo telefone
  Dado que eu tenho um nome completo "João Silva"
  E eu tenho um email "joao.silva@email.com"
  E eu tenho um CPF "11144477735"
  E eu tenho um telefone com código do país "+55", DDD "11" e número "999888777"
  Quando eu criar o cliente
  Então o cliente deve ser criado com sucesso
  E o nome completo do cliente deve ser "João Silva"
  E o email do cliente deve ser "joao.silva@email.com"
  E o CPF do cliente deve ser "11144477735"
  E o tipo do documento deve ser CPF
  E o telefone do cliente deve ter código do país "+55"
  E o telefone do cliente deve ter DDD "11"
  E o telefone do cliente deve ter número "999888777"

Cenário: Criar um cliente sem telefone
  Dado que eu tenho um nome completo "Maria Santos"
  E eu tenho um email "maria.santos@email.com"
  E eu tenho um CPF "11144477735"
  E eu não tenho telefone
  Quando eu criar o cliente
  Então o cliente deve ser criado com sucesso
  E o nome completo do cliente deve ser "Maria Santos"
  E o email do cliente deve ser "maria.santos@email.com"
  E o telefone do cliente deve ser nulo

Cenário: Cliente deve herdar de Entity e implementar IAggregateRoot
  Dado que eu tenho um cliente válido
  Quando eu verificar a hierarquia do cliente
  Então o cliente deve ser do tipo Entity
  E o cliente deve implementar IAggregateRoot

Cenário: Propriedades do cliente devem ser somente leitura
  Dado que eu tenho um cliente válido
  Quando eu obter as propriedades do cliente
  Então as propriedades Name, Email, Document e Phone devem ser somente leitura
