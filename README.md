# ⚡ TaskPulse - Real-World ASP.NET Core 8.0 Web Application

TaskPulse is a modern, real-world task & project management web application developed using **ASP.NET Core 8.0**, **C#**, **MVC Controllers**, **RESTful Web APIs**, and an interactive glassmorphic UI layout.

---

## 🚀 Key Features

- **Dashboard Analytics**: Real-time counter metrics for Total, In-Progress, Completed, and Urgent tasks with automated progress visualization.
- **RESTful API**: Full JSON API backend endpoints under `/api/tasks` supporting standard HTTP methods (`GET`, `POST`, `PUT`, `DELETE`, `PATCH`).
- **Dynamic Search & Filtering**: Multi-parameter search by title, description, task priority, category, or status.
- **Zero Database Setup**: Includes an in-memory repository populated with seed tasks so it runs instantly after installing the .NET SDK.
- **Responsive Modern UI**: Modern dark-mode styling built with Vanilla CSS variables and glassmorphism UI components.

---

## 📦 Required SDK

To run this ASP.NET Core application on your macOS system, you need the official **.NET 8.0 SDK** (or .NET 9.0 SDK).

### Option 1: Via Homebrew (Recommended for macOS)
Open your terminal and run:
```bash
brew install --cask dotnet-sdk
```

### Option 2: Direct Installer from Microsoft
1. Visit Microsoft's download page: [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Download the **.NET SDK 8.0** package matching your Mac architecture:
   - **Arm64** (Apple Silicon M1/M2/M3/M4)
   - **x64** (Intel Mac)
3. Open the `.pkg` installer and complete setup.

Verify your SDK installation:
```bash
dotnet --version
```

---

## 🛠️ Step-by-Step Instructions to Run

### Step 1: Open Terminal in Project Directory
```bash
cd /Users/meerthikasr/Desktop/c-sharp-assign
```

### Step 2: Restore Dependencies
```bash
dotnet restore
```

### Step 3: Build the Application
```bash
dotnet build
```

### Step 4: Launch the Web Application
```bash
dotnet run
```

### Step 5: Open in Browser
Navigate to your local server URL:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`

---

## 📡 REST API Documentation

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| **GET** | `/api/tasks` | Fetch all tasks (supports query params `status`, `priority`, `category`, `search`) |
| **GET** | `/api/tasks/{id}` | Retrieve details for a specific task |
| **POST** | `/api/tasks` | Create a new task item |
| **PUT** | `/api/tasks/{id}` | Update an existing task |
| **DELETE** | `/api/tasks/{id}` | Remove a task |
| **PATCH** | `/api/tasks/{id}/toggle` | Toggle completion status |
| **GET** | `/api/tasks/stats` | Fetch real-time dashboard analytics counters |

---

## 📁 Codebase Directory Structure

```
c-sharp-assign/
├── TaskPulse.csproj              # ASP.NET Core project config file
├── Program.cs                    # App builder, dependency injection & middleware pipeline
├── appsettings.json              # Runtime configuration settings
├── Models/                       # Data models & enums
│   ├── TaskItem.cs
│   ├── TaskPriorityEnum.cs
│   ├── TaskStatusEnum.cs
│   └── DashboardStats.cs
├── Services/                     # Business logic & repository services
│   ├── ITaskService.cs
│   └── TaskService.cs
├── Controllers/                  # Web Controllers & API Endpoints
│   ├── HomeController.cs
│   └── Api/TasksApiController.cs
└── Views/                        # Razor HTML UI views
    ├── Shared/_Layout.cshtml
    ├── Home/Index.cshtml
    └── Home/Error.cshtml
```
