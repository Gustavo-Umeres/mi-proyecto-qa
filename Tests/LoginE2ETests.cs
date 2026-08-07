using Allure.NUnit;
using Allure.NUnit.Attributes;
using Allure.Net.Commons;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Net8PlaywrightQA.Pages;

namespace Net8PlaywrightQA.Tests
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Suite E2E - Autenticación y Seguridad")]
    [AllureFeature("Login de Usuarios")]
    public class LoginE2ETests : PageTest
    {
        private LoginPage _loginPage = null!;
        private string _baseUrl = "https://www.saucedemo.com";

        public override BrowserNewContextOptions ContextOptions()
        {
            return new BrowserNewContextOptions
            {
                RecordVideoDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "evidencias", "videos"),
                RecordVideoSize = new RecordVideoSize { Width = 1280, Height = 720 }
            };
        }

        [SetUp]
        public void Setup()
        {
            _loginPage = new LoginPage(Page);
        }

        [Test]
        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureDescription("TC01 - Login Exitoso con Credenciales de Entorno/Secretos")]
        public async Task TC01_LoginExitoso_DebeRedireccionarAProductos()
        {
            // Lectura segura de secretos o variables de entorno (Simulado en CI/CD)
            string userEmail = Environment.GetEnvironmentVariable("QA_USER_EMAIL") ?? "standard_user";
            string userPassword = Environment.GetEnvironmentVariable("QA_USER_PASSWORD") ?? "secret_sauce";

            await _loginPage.NavigateToAsync(_baseUrl);
            await _loginPage.PerformLoginAsync(userEmail, userPassword);

            // Validar que la URL cambió a la página de inventario
            await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*inventory.html"));
        }

        [Test]
        [AllureSeverity(Allure.Net.Commons.SeverityLevel.normal)]
        [AllureDescription("TC02 - Quality Gate Check: Login con Credenciales Inválidas")]
        public async Task TC02_LoginInvalido_DebeMostrarMensajeDeError()
        {
            await _loginPage.NavigateToAsync(_baseUrl);
            await _loginPage.PerformLoginAsync("usuario_bloqueado", "password_incorrecta");

            string errorText = await _loginPage.GetErrorMessageAsync();
            Assert.That(errorText, Does.Contain("Username and password do not match"), "El mensaje de error debe ser visible");
        }

        [Test]
        [AllureSeverity(Allure.Net.Commons.SeverityLevel.minor)]
        [AllureDescription("TC03 - Validar Visibilidad del Logo de Login en el Front-End")]
        public async Task TC03_ValidarVisibilidadDelLogo()
        {
            await _loginPage.NavigateToAsync(_baseUrl);
            await Expect(Page.Locator(".login_logo")).ToBeVisibleAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            // Cierra la página para forzar que el buffer de video se escriba en el archivo
            await Page.CloseAsync();

            // Adjuntar screenshot si falló
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                string evidenciasDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "evidencias");
                Directory.CreateDirectory(evidenciasDir);
                string screenshotPath = Path.Combine(evidenciasDir, $"fallo_{TestContext.CurrentContext.Test.Name}.png");
                await Page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath });
                TestContext.AddTestAttachment(screenshotPath, "Evidencia de Fallo (Screenshot)");
                AllureApi.AddAttachment($"Captura_Fallo_{TestContext.CurrentContext.Test.Name}", "image/png", screenshotPath);
            }

            // Adjuntar el video grabado a NUnit y Allure (Para PASADOS y FALLADOS)
            if (Page.Video != null)
            {
                try
                {
                    string videoPath = await Page.Video.PathAsync();
                    if (!string.IsNullOrEmpty(videoPath) && File.Exists(videoPath))
                    {
                        TestContext.AddTestAttachment(videoPath, "Video de Ejecución (Playwright)");
                        AllureApi.AddAttachment($"Video_{TestContext.CurrentContext.Test.Name}", "video/webm", videoPath);
                    }
                }
                catch
                {
                    // Si el video aún no se ha liberado por completo, ignorar excepción silenciosamente
                }
            }
        }
    }
}
