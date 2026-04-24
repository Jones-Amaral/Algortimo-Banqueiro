# Algoritmo do Banqueiro - Trabalho Prático de Sistemas Operacionais
Este repositório possui o algoritmo do banqueiro que visa evitar a ocorrência de deadlocks em um sistema de multithreads.

# Aluno e matrícula:
João Vitor Alves Amaral - 882594

# Sobre o projeto
Este programa gerencia recursos de 5 clientes (customers) e 3 recursos (resources), que tem a quantidade definida pelo usuário.
O banqueiro só atende as solicitações do cliente (request_resources) caso o estado seja seguro (isSafe) e após a utilização ele libera o recurso (release_resurce), essas solicitações são feitas pelos threads que estão em um looping contínuo, utilizando o lock mutex.

## Pre-Requisitos
-> .Net SDK instalado (v 6.0 ou superior)

## Como compilar
-> Abrir o terminal na pasta raiz
-> Executar o comando: dotnet build

## Como executar
Para executar basta digitar dotnet run e os números de cada recurso dos 3 recursos, ex:
-> dotnet run 3, 6, 8

### Estrutura
Utiliza-se as estruturas de vetores e matrízes:
-> available: Recursos disponíveis
-> maximum: Máximo de recursos que um cliente pode solicitar (que é gerada aleatoriamente)
-> allocation: Recursos que estão sendo alocados a um cliente
-> need: recursos que o cliente ainda pode solicitar (que é o máximo - alocado)
