# DnD Character Manager

DnD Character Manager is a backend-focused microservice application for managing Dungeons & Dragons character-related data such as characters, items, and skills.

The project is built with C#/.NET and follows a microservice-based architecture where each domain area is separated into its own service. The system uses RabbitMQ for asynchronous communication and separate databases for service-owned data.

## Project Overview

The application is split into multiple services:

- **Character Microservice**  
  Manages character-related data and character state.

- **Item Microservice**  
  Manages item data such as item names, prices, and descriptions.

- **Skill Microservice**  
  Manages skills and related character abilities.

- **Saga Coordinator**  
  Coordinates distributed workflows between services where multiple operations need to happen across service boundaries.

## Architecture

The project uses a microservice architecture with separated service boundaries. Each service is structured using layered/Clean Architecture-style separation:

```text
