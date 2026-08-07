using Allure.NUnit.Attributes;
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

        [TearDown]
        public async Task TearDown()
        {
            // Si el test falló, guardar evidencia (screenshot) para el pipeline de CI/CD
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                Directory.CreateDirectory("evidencias");
                string screenshotPath = Path.Combine("evidencias", $"fallo_{TestContext.CurrentContext.Test.Name}.png");
                await Page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath });
                TestContext.AddTestAttachment(screenshotPath, "Evidencia de Fallo");
            }
        }
    }
}
