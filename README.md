# Sistema de Reserva de Salas de Estudo


## Visão Geral

Este projeto é a modelagem e prototipagem de uma aplicação de "Reserva de Salas de Estudo" para um campus universitário. O objetivo principal é aplicar conceitos avançados de Projeto de Software Orientado a Objetos, com foco na lógica de domínio e na implementação de Padrões de Projeto (Design Patterns).


## Requisitos Funcionais Implementados

- **RF-01:** Listar salas disponíveis em um intervalo de datas.

- **RF-02:** Permitir criação, modificação e cancelamento de reservas.

- **RF-03:** Detectar e impedir colisões de horário.

- **RF-04:** Sistema de notificação imediata para alterações/cancelamentos.

- **RF-05:** Geração de relatório diário de reservas confirmadas.

- **RF-06:** Implementação do Padrão Decorator para serviços extras



## Padrões de Projeto Utilizados

A arquitetura do sistema foi desenvolvida em **C#** e aplica os seguintes padrões GoF:

1. **Singleton:** Gerenciamento do repositório de reservas em memória, garantindo thread-safety.

2. **Factory Method:** Instanciação flexível de diferentes tipos de salas (Estudo Individual, Trabalho em Grupo, Laboratório).

3. **Strategy:** Dinamismo na política de detecção de colisões de reserva (ex: "primeiro a reservar" vs. "prioridade docente").

4. **Observer:** Mecanismo de push/pull para propagar eventos e notificações aos usuários envolvidos.



## Estrutura do Repositório

- `/src`: Código-fonte principal da aplicação.

- `/docs`: Diagramas e documentações complementares.



## Autores

- Letícia Abrahão Moreira

- Pedro Henrique Novello D'Elia Porto de Almeida 

- Enrico Manzolli Bertoni (Extensão)