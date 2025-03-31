# 🌍 WeatherApp – Environmental Monitoring System

This is a cross-platform mobile app built with .NET MAUI for a regional environmental agency. It helps monitor air quality, weather conditions, and water quality using real-time and historical data from sensors.

---

## 📱 Features

### ✅ Environmental Scientist
- View historical air quality data (NO₂, PM2.5)
- Visual display using CollectionView
- Real-time sensor integration (in progress)

### ✅ Administrator
- Register new users
- Login system with role-based access
- Placeholder for user management panel

### ✅ Operations Manager *(Upcoming)*
- Maintenance scheduling
- Sensor status dashboard

---

## 🔐 Authentication

- User registration with username, password, and role
- Login with validation
- Role-based navigation (Scientist → Air Quality page, Admin → Admin Dashboard, etc.)
- In-memory user storage (SQLite planned)

---

## 🛠 Tech Stack

- .NET MAUI (C#)
- EPPlus for reading Excel files
- MVU/MVVM-friendly structure
- GitHub for version control, branching & PRs

---

## 🚀 Getting Started

### Requirements
- Visual Studio 2022+ with .NET MAUI workload
- Android Emulator / Device
- macOS or Windows

### How to Run
1. Clone this repo:
   ```bash
   git clone https://github.com/Adam24NU/WeatherApp.git
2.Open in Visual Studio
3. Set the target to Android Emulator
4.Press ▶️ to build & run

## 🤝 Contributing

We use GitHub issues and feature branches for development.  
Each feature is implemented under a separate branch and merged via pull requests with code reviews.

✅ Branching strategy: `main` (stable) + `feature/*`  
✅ Tasks are tracked using [GitHub Issues](https://github.com/Adam24NU/WeatherApp/issues)

Team members are expected to:
- Create 3+ features tied to user stories
- Add tests and documentation
- Participate in PR reviews

## 👥 Team
- Adam - Developer, UI/UX, GitHub workflow
- Bart -
- Jamie -

## 🧪 Testing

- Unit testing (in progress)
- Testable services for Excel parsing coming soon


## 🧭 Roadmap

- [x] Air Quality Page with Excel integration
- [x] User Registration & Login
- [ ] Admin Panel for user management
- [ ] Real-time sensor data
- [ ] SQLite user storage
- [ ] Operations Manager dashboard

## 📄 License

This project is licensed under the terms of the MIT License.  
See the [LICENSE](https://github.com/Adam24NU/WeatherApp/blob/main/LICENSE) file for details.
