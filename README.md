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
        <li><a href="#cobertura-de-código">Cobertura de código</a></li>
      </ul>
    </li>
    <li><a href="#licensa">Licensa</a></li>
    <li><a href="#reconhecimentos">Reconhecimentos</a></li>
  </ol>
</details>

## O problema proposto
Balanceamento de carga é muito importante em ambientes Cloud. Estamos sempre tentando minimizar os custos para que possamos manter o número de servidores o menor possível. Em contrapartida a capacidade e performance aumenta quando adicionamos mais servidores. Em nosso ambiente de simulação, em cada tick  (unidade básica de tempo da simulação), os usuários conectam aos servidores disponíveis e executam uma tarefa. Cada tarefa leva um número de ticks para ser ﬁnalizada (o número de ticks de uma tarefa é representado por ttask ), e após isso o usuário se desconecta automaticamente.

Os servidores são máquinas virtuais que se auto criam para acomodar novos usuários. Cada servidor custa R$ 1,00 por tick e suporta no máximo umax usuários simultaneamente. Você deve ﬁnalizar servidores que não estão sendo mais usados. O desaﬁo é fazer um programa em Python que recebe usuários e os aloca nos servidores tentando manter o menor custo possível.

**Input** 

Um arquivo onde: a primeira linha possui o valor de ttask ;
a segunda linha possui o valor de umax ;
as demais linhas contém o número de novos usuários para cada tick.

**Output**

Um arquivo onde cada linha contém uma lista de servidores disponíveis no ﬁnal de cada tick , representado pelo número de usuários em cada servidor separados por vírgula e, ao ﬁnal, o custo total por utilização dos servidores

**Limites** 

1 ≤ ttask ≤ 10

1 ≤ umax ≤ 10

**Exemplo**

input.txt

> 4<br>
> 2<br>
> 1<br>
> 3<br>
> 0<br>
> 1<br>
> 0<br>
> 1<br>

output.txt
> 1<br>
> 2,2<br>
> 2,2<br>
> 2,2,1<br>
> 1,2,1<br>
> 2<br>
> 2<br>
> 1<br>
> 1<br>
> 0<br>
> 15<br>


**Detalhamento do exemplo**

ttask = 4 (valor da primeira linha do input.txt)

umax = 2 (valor da segundo linha do input.txt)

| Tick | Input | Output | Explicação                                                                                                                 |
|------|-------|--------|----------------------------------------------------------------------------------------------------------------------------|
| 1    | 1     | 1      | 1 servidor para 1 usuário (1 servidor criado)                                                                              |
| 2    | 3     | 2,2    | 2 servidores para 4 usuários (1 servidor criado)                                                                           |
| 3    | 0     | 2,2    | 2 servidores para 4 usuários (nenhum servidor criado ou removido)                                                          |
| 4    | 1     | 2,2,1  | 3 servidores para 5 usuários (1 servidor criado)                                                                           |
| 5    | 0     | 1,2,1  | 3 servidores para 4 usuários (nenhum servidor criado ou removido)                                                          |
| 6    | 1     | 2      | 1 servidor para 2 usuários (2 servidores removidos)                                                                        |
| 7    |       | 2      | 1 servidores para 2 usuários (nenhum servidor criado ou removido)                                                          |
| 8    |       | 1      | 1 servidor para 1 usuário (nenhum servidor criado ou removido)                                                             |
| 9    |       | 1      | 1 servidor para 1 usuário (nenhum servidor criado ou removido)                                                             |
| 10   |       | 0      | nenhum servidor e nenhum usuário (1 servidor removido)                                                                     |
|      |       | 15     | Custo total: R$ 1,00 * 5 ticks (primeira VM) + R$ 1,00 * 4 ticks (segunda VM) + R$ 1,00 * 6 ticks (terceira VM) = R$ 15,00 |


## A solução

Conforme solicitado, o programa foi escrito em Python 3, utilizando a IDE PyCharm Community.

### Modelagem de classes para o problema proposto

Para iniciar a resolução do problema, achei necessário modelar as classes para um melhor entendimento do que estava sendo exigido.

A classe Users praticamente não está sendo utilizada, mas era importante ressaltar que uma execução no servidor exige tanto uma tarefa quanto um usuário, e por esse motivo a classe foi criada.

O modelo de classes foi criado utilizando o programa [Draw IO](https://app.diagrams.net/). 

<div style="padding: 16px 0px 16px 16px; background-color: #fff"><img src="images/topaz_class_diagram.png" alt="diagrama_de_classes"></div>


#### Pré-requisitos
    * Python 3 (versão 3.7.4 ou superior, disponível no site do [Python](https://www.python.org/downloads/))

    * PIP (versão 19.0.3 ou superior, disponível no site do [PIP](https://pip.pypa.io/en/stable/installation/))
    
    * unittest (framework instalado automaticamente junto com o Python 3 [unittest](https://docs.python.org/3/library/unittest.html))

    * coverage (versão 5.5, instalação via PIP, conforme documentação do [Coverage](https://coverage.readthedocs.io/en/coverage-5.5/))

#### Preparação do ambiente

*A instalação do Python 3 e do PIP não são cobertas nessa explicação. Por favor siga a documentação supra-citada.*

Tendo o Python 3 e o pip instalados na máquina, instale o coverage:

```
C:\temp\topaz-test> pip install coverage
```

#### Execução do programa

Para executar o programa, clone a solução do github e entre no diretório do topaz-test

```
C:\temp\topaz-test> git clone https://github.com/brbmendes/topaz-test.git
C:\temp\topaz-test> cd topaz-test
```

Caso esteja utilizando o Windows, utilize o comando abaixo:

```
C:\temp\topaz-test> python.exe ./src/main.py ./input/input.txt
```

Onde:
- **python.exe** é o caminho do executável do python em sua máquina
- **./src/main.py** é o endereço do arquivo principal do programa
- **./input/input.txt** é o arquivo de entrada

Caso o programa execute com sucesso, vai ser impresso no console a seguinte mensagem:
```
C:\temp\topaz-test> python.exe ./src/main.py ./input/input.txt

O arquivo de saída output.txt foi gerado no caminho "topaz-test/output/output.txt"

Fim do programa.
```

Considerações:
- Caso de algum problema na execução, em que o executável python.exe não seja encontrado, verifique o local do executável do python em seu computador, e ajuste o comando de acordo com a sua necessidade.

- Caso não seja informado o programa de entrada, vai retornar a mensagem "Por favor informe um arquivo de entrada."

- Caso o arquivo esteja corrompido e não possa ser aberto, ou não exista, vai retornar a mensagem "Falha ao abrir o arquivo informado."

- Não foram realizados mais testes quanto ao tipo ou formato do arquivo de entrada. Pressupõe-se que o arquivo informado como entrada vai ser o esperado pelo programa.

#### Testes unitários

Para criação e execução dos testes unitários, foi utilizado o unittest.

Para executar os testes, utilize o comando

```
C:\temp\topaz-test> python.exe -m unittest -v
```

E a suíte de testes deve executar, gerando a saída abaixo:

```
C:\temp\topaz-test> python.exe -m unittest -v
test_add_a_server (tests.test_accounting.TestAccounting) ... ok
test_calculate_cost_usage_adding_a_server (tests.test_accounting.TestAccounting) ... ok
test_calculate_cost_usage_without_add_server (tests.test_accounting.TestAccounting) ... ok
test_default_value_cost_per_tick (tests.test_accounting.TestAccounting) ... ok
test_get_set_cost_per_tick (tests.test_accounting.TestAccounting) ... ok
test_start_accounting_without_servers (tests.test_accounting.TestAccounting) ... ok
test_get_id (tests.test_job.TestJob) ... ok
test_get_task (tests.test_job.TestJob) ... ok
test_get_user (tests.test_job.TestJob) ... ok
test_add_job_when_has_no_spaces_for_new_jobs (tests.test_server.TestServer) ... ok
test_add_job_when_has_spaces_for_new_jobs (tests.test_server.TestServer) ... ok
test_can_add_job_has_no_spaces_for_new_jobs (tests.test_server.TestServer) ... ok
test_can_add_job_has_spaces_for_new_jobs (tests.test_server.TestServer) ... ok
test_can_add_job_with_default_value (tests.test_server.TestServer) ... ok
test_get_id (tests.test_server.TestServer) ... ok
test_get_running_jobs_when_add_one_job (tests.test_server.TestServer) ... ok
test_get_uptime (tests.test_server.TestServer) ... ok
test_update_tasks_remove_finished (tests.test_server.TestServer) ... ok
test_update_uptime (tests.test_server.TestServer) ... ok
test_decrement_ticks (tests.test_task.TestTask) ... ok
test_get_id (tests.test_task.TestTask) ... ok
test_get_ticks (tests.test_task.TestTask) ... ok
test_get_id (tests.test_user.TestUser) ... ok

----------------------------------------------------------------------
Ran 23 tests in 0.011s

OK
```

#### Cobertura de código

Para execução da cobertura de código, foi utilizado o coverage.

Para executar a análise de cobertura de código utilizando o coverage com unittest, e gerar o relatório, utilize o comando

```
C:\temp\topaz-test> coverage run -m unittest discover && coverage report
```

A saída do comando deve ser a abaixo:

```
C:\temp\topaz-test> coverage run -m unittest discover && coverage report
.......................
----------------------------------------------------------------------
Ran 23 tests in 0.006s

OK
Name                          Stmts   Miss  Cover
-------------------------------------------------
src\entities\accountings.py      18      0   100%
src\entities\jobs.py             12      0   100%
src\entities\servers.py          35      0   100%
src\entities\tasks.py            11      0   100%
src\entities\users.py             6      0   100%
tests\__init__.py                 0      0   100%
tests\test_accounting.py         41      1    98%
tests\test_job.py                27      1    96%
tests\test_server.py             73      1    99%
tests\test_task.py               24      1    96%
tests\test_user.py               15      1    93%
-------------------------------------------------
TOTAL                           262      5    98%
```

### Licensa

Distribuído sobre a licença MIT. Consulte o arquivo LICENSE para maiores informações.

### Reconhecimentos

* [Img Shields](https://shields.io)
* [Escolha uma licença Open Source](https://choosealicense.com)
* [Gerador de tabelas para markdown](https://www.tablesgenerator.com/markdown_tables)



[contributors-shield]: https://img.shields.io/github/contributors/brbmendes/topaz-test?style=for-the-badge
[contributors-url]: https://github.com/brbmendes/topaz-test/graphs/contributors
[license-shield]: https://img.shields.io/github/license/brbmendes/topaz-test?style=for-the-badge
[license-url]: https://github.com/brunotrbr/topaz-test/blob/main/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/brunotrbr/