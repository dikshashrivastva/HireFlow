# HireFlow — Real-time Job Application Tracker

A production-grade real-time notification system built with ASP.NET Core, 
SignalR, RabbitMQ and MassTransit. Candidates receive instant live updates 
when recruiters change their application status — no page refresh needed.

---

## What It Does

- Recruiter updates a candidate's application status (Shortlisted, Interview Scheduled, Rejected, Offer Received)
- Update is published as a message to **RabbitMQ** message broker
- **MassTransit** consumer picks up the message instantly
- Message is pushed live to the candidate's browser via **SignalR WebSockets**
- Candidate sees a toast notification and their dashboard updates in real time
- Supports **bulk updates** — notify hundreds of candidates simultaneously

---

## Architecture
```
Recruiter API Call
      ↓
NotificationsController
      ↓
RabbitMQ (notification-queue)
      ↓
MassTransit Consumer
      ↓
SignalR Hub (NotificationHub)
      ↓
Candidate's Browser (Live Update)
```

---

## Tech Stack

| Technology | Purpose |
|---|---|
| ASP.NET Core 10 | Backend Web API |
| SignalR | Real-time WebSocket communication |
| RabbitMQ | Message broker (pub/sub) |
| MassTransit | RabbitMQ abstraction & consumer management |
| HTML/CSS/JS | Frontend dashboard |

---

## Key Concepts Demonstrated

- **Event-driven architecture** — services communicate via messages, not direct calls
- **Publisher/Subscriber pattern** — publishers don't know who consumes their messages
- **SignalR Groups** — notifications are targeted to specific users, not broadcast to everyone
- **Decoupled architecture** — the API and notification delivery are completely independent
- **Real-time WebSockets** — browser receives updates without polling or refreshing

---

## Project Structure
```
NotificationApi/
├── Controllers/
│   └── NotificationsController.cs   ← API endpoints (send/bulk-update)
├── Consumers/
│   └── NotificationConsumer.cs      ← Reads from RabbitMQ, pushes to SignalR
├── Hubs/
│   └── NotificationHub.cs           ← SignalR hub, manages connections & groups
├── Models/
│   └── NotificationMessage.cs       ← Message contract
├── wwwroot/
│   └── index.html                   ← Candidate dashboard UI
└── Program.cs                       ← App configuration & service registration
```

---

## How To Run Locally

### Prerequisites
- .NET 10 SDK
- RabbitMQ installed locally ([download here](https://www.rabbitmq.com/install-windows.html))
- Erlang installed ([download here](https://www.erlang.org/downloads))

### Steps

**1. Start RabbitMQ**
```bash
net start RabbitMQ
```

**2. Clone the repository**
```bash
git clone https://github.com/YOUR_USERNAME/hireflow.git
cd hireflow/NotificationApi
```

**3. Run the API**
```bash
dotnet run
```

**4. Open the dashboard**
```
http://localhost:5126/index.html
```

**5. Connect as a candidate**

Type one of these IDs and click Connect:
- `candidate_google`
- `candidate_microsoft`
- `candidate_amazon`
- `candidate_flipkart`

**6. Send a notification via Swagger**
```
http://localhost:5126/swagger
```

Use `POST /api/notifications/update-status` with:
```json
{
  "userId": "candidate_google",
  "type": "StatusUpdate",
  "message": "Congratulations! You have been shortlisted.",
  "companyName": "Google",
  "jobTitle": "Software Engineer III",
  "status": "Shortlisted",
  "timestamp": "2024-01-01T00:00:00Z"
}
```

---

## Notification Status Types

| Status | Meaning | Color |
|---|---|---|
| `Reviewed` | Application has been reviewed | Blue |
| `Shortlisted` | Selected for next round | Green |
| `InterviewScheduled` | Interview date confirmed | Purple |
| `Rejected` | Application not selected | Red |
| `OfferReceived` | Job offer extended | Gold |

---

## Demo

Open 4 browser tabs, connect each with a different candidate ID, then use 
the bulk-update endpoint to notify all 4 simultaneously — watch all tabs 
update in real time.

---

*Built with ASP.NET Core 10 · SignalR · RabbitMQ · MassTransit*