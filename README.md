# NeredeKal Project README

## Project Overview
NeredeKal is a microservices-based hotel management and reporting application built with .NET and Docker.

## Services
- **Hotel Service**: Manages hotel information, authorized persons, and contact details
- **Report Service**: Generates reports based on hotel locations
- **Database Services**: 
  - PostgreSQL for Hotel and Report databases
  - Redis for caching
- **Message Broker**: RabbitMQ for inter-service communication
- **Logging & Monitoring**: 
  - Elasticsearch for log storage
  - Kibana for log visualization

## Docker Compose Setup
The `docker-compose.yml` file defines the following services:
- RabbitMQ (Message Broker)
- PostgreSQL databases for Hotel and Report services
- Hotel Service API
- Report Service API
- Elasticsearch
- Kibana
- Redis

## API Endpoints
### Hotel Service
- GET `/api/hotel`: List all hotels
- GET `/api/hotel/:hotelId`: Get hotel by Id
- GET `/api/hotel/location/:location`: Get hotels by location
- POST `/api/hotel`: Create a new hotel
- PUT `/api/hotel/:hotelId`: Update hotel details
- DELETE `/api/hotel/:hotelId`: Delete a hotel
- Manage authorized persons and contact information

### Report Service
- GET `/api/report`: List reports
- GET `/api/report/:reportId`: Get report by Id
- POST `/api/report`: Create a new report by location

## Local Development
1. Ensure Docker and Docker Compose are installed
2. Clone the repository
3. Run `docker-compose up` to start all services
4. Access services on specified ports (e.g., Hotel Service on 5076, Report Service on 5078)

## Postman Collection
A Postman collection is provided for easy API testing and exploration.

## Environment
- Development environment configured
- Connection strings and credentials managed through environment variables

## Project Local Addresses

- **Hotel Service**: `http://localhost:5076`
- **Report Service**: `http://localhost:5078`
- **RabbitMQ Management**: `http://localhost:15672`
- **Redis**: `localhost:6379`
- **Elasticsearch**: `http://localhost:9200`
- **Kibana**: `http://localhost:5601`

## Database Ports
- **Hotel Database**: `localhost:5432`
- **Report Database**: `localhost:5434`
