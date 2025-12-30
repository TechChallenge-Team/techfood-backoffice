#language: pt-BR
Funcionalidade: Gerenciamento de Usuários
  Como um administrador do sistema
  Eu quero gerenciar usuários
  Para que eu possa controlar o acesso ao sistema

Cenário: Criar um usuário com parâmetros válidos incluindo email
  Dado que eu tenho um nome de usuário completo "Admin User"
  E eu tenho um username "admin"
  E eu tenho uma função "Administrator"
  E eu tenho um email de usuário "admin@techfood.com"
  Quando eu criar o usuário
  Então o usuário deve ser criado com sucesso
  E o nome completo do usuário deve ser "Admin User"
  E o username deve ser "admin"
  E a função do usuário deve ser "Administrator"
  E o email do usuário deve ser "admin@techfood.com"

Cenário: Criar um usuário sem email
  Dado que eu tenho um nome de usuário completo "Manager User"
  E eu tenho um username "manager"
  E eu tenho uma função "Manager"
  E eu não tenho um email de usuário
  Quando eu criar o usuário
  Então o usuário deve ser criado com sucesso
  E o nome completo do usuário deve ser "Manager User"
  E o username deve ser "manager"
  E a função do usuário deve ser "Manager"
  E o email do usuário deve ser nulo

Cenário: Definir senha válida para o usuário
  Dado que eu tenho um usuário válido
  E eu tenho um hash de senha "$2a$10$abcdefghijklmnopqrstuvwxyz"
  Quando eu definir a senha do usuário
  Então o hash de senha do usuário deve ser "$2a$10$abcdefghijklmnopqrstuvwxyz"

Cenário: Definir função válida para o usuário
  Dado que eu tenho um usuário válido
  E eu tenho uma nova função "SuperAdmin"
  Quando eu definir a função do usuário
  Então a função do usuário deve ser "SuperAdmin"

Cenário: Usuário deve herdar de Entity e implementar IAggregateRoot
  Dado que eu tenho um usuário válido
  Quando eu verificar a hierarquia do usuário
  Então o usuário deve ser do tipo Entity
  E o usuário deve implementar IAggregateRoot

Cenário: Propriedades do usuário devem ser somente leitura exceto Password e Role
  Dado que eu tenho um usuário válido
  Quando eu obter as propriedades do usuário
  Então as propriedades Name, Username e Email devem ser somente leitura
  E as propriedades Role e PasswordHash podem ser alteradas através de métodos
