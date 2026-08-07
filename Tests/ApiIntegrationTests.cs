using Allure.NUnit.Attributes;
using Microsoft.Playwright;
using NUnit.Framework;

namespace Net8PlaywrightQA.Tests
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Suite de API - Microservicios")]
    [AllureFeature("API de Usuarios")]
    public class ApiIntegrationTests
    {
        private IAPIRequestContext _apiContext = null!;

        [SetUp]
        public async Task Setup()
        {
            var playwright = await Playwright.CreateAsync();
            _apiContext = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
            {
                BaseURL = "https://jsonplaceholder.typicode.com"
            });
        }

        [Test]
        [AllureSeverity(Allure.Net.Commons.SeverityLevel.blocker)]
        [AllureDescription("API-01 - Validar respuesta exitosa HTTP 200 y Contrato de JSON")]
        public async Task API01_ObtenerUsuario_DebeResponder200OK()
        {
            var response = await _apiContext.GetAsync("/users/1");
            Assert.That(response.Status, Is.EqualTo(200), "El status HTTP debe ser 200");

            var json = await response.JsonAsync();
            Assert.That(json.Value.GetProperty("name").GetString(), Is.Not.Null.And.Not.Empty);
        }

        [TearDown]
        public async Task TearDown()
        {
            await _apiContext.DisposeAsync();
        }
    }
}
