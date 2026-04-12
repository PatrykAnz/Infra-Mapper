# Infra-Mapper

**PatrykAnz · 96160**

**Infra-Mapper** is a cloud-oriented full-stack application: a **React** (Vite) client calls an **ASP.NET Core** API to **list, add, toggle completion, and delete tasks** persisted in **SQL Server**. Routes live under **`/api/tasks`**; the API ships **OpenAPI (Swagger)**. The repo includes **Docker Compose** for local runs and targets **Microsoft Azure** (App Service for UI and API, Azure SQL, Key Vault with managed identity where configured, **GitHub Actions** for frontend deployment).

The architecture is **cloud-native** in spirit: **PaaS** on Azure for hosting and supporting services.

## Layered architecture

| Layer | Repository / local component | Azure service |
|-------|------------------------------|---------------|
| **Presentation** | React, TypeScript, Vite (single-page application) | **Azure App Service** (Linux, Node.js 24 LTS) — static production build served from the Web App |
| **Application** | ASP.NET Core 9 Web API, Entity Framework Core, OpenAPI (Swagger) | **Azure App Service** (Linux, .NET 9) |
| **Data** | SQL Server, EF Core migrations | **Azure SQL Database** (logical server and database in resource group `rg-cloud-infra-mapper`) |
| **Configuration & secrets** | Azure.Identity, Key Vault configuration provider, optional Docker Compose for local SQL Edge | **Azure Key Vault** (`kv-infra-mapper`), accessed via **managed identity** and RBAC where configured |
| **Delivery** | GitHub Actions workflow (`.github/workflows/`) | Deployment to the frontend Web App; OIDC and Azure AD app registration for CI authentication |
| **Quality** | xUnit test project (`TaskManager.Tests`) | — |

The presentation tier is implemented as a **static front-end bundle** deployed to a **Node-based App Service**. That differs from the standalone **Azure Static Web Apps** product, which is not used in this repository’s current Azure footprint.
