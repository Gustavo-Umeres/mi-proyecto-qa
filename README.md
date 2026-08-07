# 🚀 Proyecto de Pruebas Automáticas Real-World (.NET 8 + Playwright + Allure + CI/CD)

Este proyecto simula **una arquitectura real de automatización de pruebas empresarial** lista para ser subida a GitHub.

---

## 🛠️ Tecnologías y Patrones Incluidos

* **Lenguaje & Framework:** .NET 8 (C#) + NUnit 4
* **Motor de Automatización:** Microsoft Playwright 1.45 (E2E Web y API Testing)
* **Patrón de Diseño:** Page Object Model (POM) en `Pages/LoginPage.cs`
* **Reportes:** Allure Report Framework (`allure-results`)
* **Orquestador de CI/CD:** GitHub Actions (`.github/workflows/qa-pipeline.yml`)
* **Matriz de Ejecución:** Ejecución en paralelo en `Chromium` y `Firefox`
* **Notificaciones por Correo:** Envío automático del resumen de ejecución vía SMTP (Email)
* **Seguridad:** Gestión de credenciales mediante variables de entorno e inyección de **GitHub Secrets**.

---

## 📁 Estructura del Proyecto

```text
dotnet8-playwright-cicd-real-world/
├── .github/
│   └── workflows/
│       └── qa-pipeline.yml        <-- Pipeline de GitHub Actions completo
├── Pages/
│   └── LoginPage.cs               <-- Page Object Model (POM)
├── Tests/
│   ├── LoginE2ETests.cs           <-- Pruebas de Interfaz gráfica (UI)
│   └── ApiIntegrationTests.cs     <-- Pruebas de Servicios de API
├── Net8PlaywrightQA.csproj        <-- Proyecto .NET 8 con dependencias NuGet
├── allureConfig.json              <-- Configuración de Allure
└── README.md
```

---

## 🔐 Configuración de Secretos en GitHub (GitHub Secrets)

Antes de ejecutar el pipeline en GitHub, debes configurar los siguientes secretos en tu repositorio (`Settings` -> `Secrets and variables` -> `Actions`):

| Nombre del Secreto | Descripción | Ejemplo de Valor |
| :--- | :--- | :--- |
| `QA_USER_EMAIL` | Usuario de prueba inyectado en el test | `standard_user` |
| `QA_USER_PASSWORD` | Contraseña de prueba inyectada | `secret_sauce` |
| `SMTP_USERNAME` | Correo desde el que se enviará la notificación | `tu-correo@gmail.com` |
| `SMTP_PASSWORD` | Contraseña de aplicación SMTP de Gmail/Outlook | `xxxx-xxxx-xxxx-xxxx` |
| `NOTIFY_EMAIL_TO` | Destinatario del reporte de correo | `mi-correo@empresa.com` |

---

## 🚀 Pasos para Subir este Proyecto a GitHub

1. Inicializar el repositorio Git local:
   ```bash
   git init
   git add .
   git commit -m "feat: Proyecto de QA Automation .NET 8 con Playwright y CI/CD"
   ```
2. Conectar con tu repositorio remoto de GitHub:
   ```bash
   git remote add origin https://github.com/tu-usuario/tu-repo-qa.git
   git branch -M main
   git push -u origin main
   ```
3. ¡Listo! Al subir el código a la rama `main`, GitHub Actions se activará automáticamente en la pestaña **Actions**.
