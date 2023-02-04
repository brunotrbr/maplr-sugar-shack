# maplr-sugar-shack
Maplr Backend Technical Test

[![Contributors][contributors-shield]][contributors-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<p align="center">
  <h3 align="center">Maplr: handle sells and stocks of maple syrup</h3>

  <p align="center">
    Maplr technical test
    <br />
</p>

<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary>Conteúdo</summary>
  <ol>
    <li>
      <a href="#o-problema-proposto">O problema proposto</a>
    </li>
    <li>
      <a href="#a-solução">A solução</a>
      <ul>
        <li><a href="#modelagem-de-classes-para-o-problema-proposto">Modelagem de classes para o problema proposto</a></li>
        <li><a href="#pré-requisitos">Pré-requisitos</a></li>
        <li><a href="#preparação-do-ambiente">Preparação do ambiente</a></li>
        <li><a href="#execução-do-programa">Execução do programa</a></li>
        <li><a href="#testes-unitários">Testes unitários</a></li>
      </ul>
    </li>
    <li><a href="#licensa">Licensa</a></li>
    <li><a href="#reconhecimentos">Reconhecimentos</a></li>
  </ol>
</details>

## O problema proposto
Balanceamento de carga é muito importante em ambientes Cloud. Estamos sempre tentando minimizar os custos para que possamos manter o número de servidores o menor possível. Em contrapartida a capacidade e performance aumenta quando adicionamos mais servidores. Em nosso ambiente de simulação, em cada tick  (unidade básica de tempo da simulação), os usuários conectam aos servidores disponíveis e executam uma tarefa. Cada tarefa leva um número de ticks para ser ﬁnalizada (o número de ticks de uma tarefa é representado por ttask ), e após isso o usuário se desconecta automaticamente.

Os servidores são máquinas virtuais que se auto criam para acomodar novos usuários. Cada servidor custa R$ 1,00 por tick e suporta no máximo umax usuários simultaneamente. Você deve ﬁnalizar servidores que não estão sendo mais usados. O desaﬁo é fazer um programa em Python que recebe usuários e os aloca nos servidores tentando manter o menor custo possível.


## A solução

Conforme solicitado, o programa foi escrito em .NET 6, utilizando a IDE Visual Studio. Foram criados xxxxxxx (descrever tudo do projeto), e as chamadas dos endpoints podem ser via postman ou swagger.

&nbsp;

**Database** 

Utilizaddo Entity Framework In memory
Populado arquivo .json conforme abaixo, para alimentar o banco de dados na inicialização do programa (classe DataGenerator)

```json
[
  {
    "id": "f6fb258d-c33f-4de3-9288-253bc86234b0",
    "name": "Amber Maple Syrup",
    "description": "Winding Road 100% Pure Amber Maple Syrup",
    "image": "https://www.windingroadmaple.ca/uploads/userfiles/images/bottles.png",
    "price": 8.00,
    "stock": 10,
    "type": 1
  },
  {
    "id": "880c684d-770b-4274-9891-c00e08d37f2f",
    "name": "Dark Maple Syrup",
    "description": "Winding Road 100% Pure Dark Maple Syrup",
    "image": "https://www.windingroadmaple.ca/uploads/userfiles/images/bottles.png",
    "price": 8.50,
    "stock": 15,
    "type": 2
  },
  {
    "id": "d7e356c8-5aa1-4cc6-b5a8-c76f51d7906e",
    "name": "Clear Maple Syrup",
    "description": "Winding Road 100% Pure Clear Maple Syrup",
    "image": "https://www.windingroadmaple.ca/uploads/userfiles/images/bottles.png",
    "price": 7.50,
    "stock": 18,
    "type": 3
  }
]
```

&nbsp;

### Modelagem de classes para o problema proposto

A modelagem de classes foi realizada de acordo com o contrato estabelecido no teste.

No contrato não consta referências a ID's/chaves primárias para armazenamento dos dados no banco de dados.  No início cheguei a definir ID's para as tabelas **order**, **product** e **cart**, mas optei por remover eles durante o desenvolvimento.

O motivo de remover foi porque no contrato a inserção/remoção utilizava o productId somente. Nesse caso, eu poderia adicionar dois elementos no carrinho com o mesmo productId, mas na hora de remover teria a ambiguidade de não saber qual deveria ser removido. Neste caso, mantive o acordado no contrato.

Entendo que o DTO do contrato é uma classe auxiliar para transporte dos dados entre as camadas do sistema, e que as models devem ser a representação do banco de dados. Contudo  optei por manter os dados do banco de dados iguais aos DTOs.

&nbsp;

#### Pré-requisitos
    .NET 6.0 e .NET CLI (Dotnet version 6.0.302. Not tested with other versions)

    Docker (Docker version 20.10.17. Not tested with other versions)
    
    Docker Compose (Docker Compose version v2.6.1. Not tested with other versions)

&nbsp;

#### Preparação do ambiente

*A instalação do .NET/.NET CLI, docker e docker-compose não são cobertas nessa explicação. Por favor siga a documentação supra-citada.*

&nbsp;

Clone o programa do repositório https://github.com/brunotrbr/maplr-sugar-shack e entre no diretório:

```
[Windows]
PS C:\> git clone git@github.com:brunotrbr/maplr-sugar-shack.git
PS C:\> cd maplr-sugar-shack

[Linux]
user@machine:~$ git clone git@github.com:brunotrbr/maplr-sugar-shack.git
user@machine:~$ cd maplr-sugar-shack
```

&nbsp;

Para preparar o ambiente de execução, execute o seguinte script no terminal/console do seu computador:

```
[Windows]
PS C:\maplr-sugar-shack> .\prepare_env.ps1

[Linux]
user@machine:maplr-sugar-shack$ source prepare_env.sh
```

&nbsp;

#### Execução do programa

Execute o comando docker-compose up para realizar o build do projeto e inicializar ele automaticamente:

```
[Windows]
PS C:\maplr-sugar-shack> docker-compose up

[Linux]
user@machine:maplr-sugar-shack$ docker-compose up
```

Alternativamente, utilize os comandos `dotnet xxx` para rodar os projetos de forma independente:

```
[Windows]
// adicionar output

[Linux]
// adicionar output
```


Caso o programa execute com sucesso, vai ser impresso no console a seguinte informação, referente aos testes criados para verificar se o endpoint **order** e as validações de pedidos repetidos estão funcionando corretamente:

```
// adicionar output
```

Optei por criar testes de sistema somente para a OrderController pois ele automaticamente utiliza outras Controllers nas validações de pedido repetido, cobrindo assim mais pontos do sistema.

Obs: 

Podem ser efetuados testes manuais em todos os endpoints disponibilizados no contrato, tanto através de ferramentas como o postman quanto pelo swagger no endereço http://localhost/swagger/index.html

Considerações adicionais:

autenticação somente em order

IBaseController

Put cart

Mappers

Order - pedidos para o mesmo productID


&nbsp;

#### Testes

Para criação e execução dos testes unitários, foi utilizado o unittest.

Para executar os testes, utilize o comando

```
[Windows]
// adicionar output

[Linux]
// adicionar output
```

E a suíte de testes deve executar, gerando a saída abaixo:

```
[Windows]
// adicionar output

[Linux]
// adicionar output
```


### Licensa

Distribuído sobre a licença MIT. Consulte o arquivo LICENSE para maiores informações.

### Reconhecimentos

* [Img Shields](https://shields.io)
* [Escolha uma licença Open Source](https://choosealicense.com)
* [Gerador de tabelas para markdown](https://www.tablesgenerator.com/markdown_tables)



[contributors-shield]: https://img.shields.io/github/contributors/brunotrbr/maplr-sugar-shack?style=for-the-badge
[contributors-url]: https://github.com/brunotrbr/maplr-sugar-shack/graphs/contributors
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/brunotrbr/