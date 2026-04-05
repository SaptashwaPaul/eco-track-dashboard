# Eco-Track // Global Logistics Radar 🌍⚡

Eco-Track is a real-time, event-driven supply chain monitoring dashboard. It tracks global assets geographically and streams live disruption events (weather, labor strikes, material shortages) instantly to the client without requiring page refreshes.

## 🚀 Tech Stack
* **Frontend:** Angular 19 (Zoneless Architecture, Signals), Leaflet.js (GIS Mapping)
* **Backend:** .NET 10 Web API, SignalR (WebSockets), Clean Architecture
* **Database:** PostgreSQL, Entity Framework Core
* **DevOps:** Docker, Docker Compose

## 🧠 Architectural Highlights
* **Real-Time Telemetry:** Implements **SignalR** to push asynchronous disruption events from a .NET Background Worker directly to the Angular client.
* **Geospatial Data Aggregation:** Uses custom algorithms to prevent Leaflet marker collision (the "pancake effect") by dynamically clustering and tallying simultaneous events at identical GPS coordinates.
* **Reactive UI:** Built entirely on **Angular Signals** for granular, high-performance DOM updates. Includes a pure CSS variables implementation for instant Light/Dark mode toggling.
* **Entity DTO Mapping:** Strictly separates database entities from network payloads to prevent JSON circular reference crashes and secure internal database structures.

🚀 Quick Start (Zero Setup Required)
This entire architecture is fully containerized. You do not need .NET, Node, or PostgreSQL installed on your machine to run this project.

Ensure Docker Desktop is running.

Clone this repository.

Run the following command in the root directory:
docker compose up --build -d
