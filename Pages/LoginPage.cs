using Microsoft.Playwright;

namespace Net8PlaywrightQA.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;

        // Selectores de los elementos de la interfaz
        private readonly string _usernameInput = "#user-name";
        private readonly string _passwordInput = "#password";
        private readonly string _loginButton = "#login-button";
        private readonly string _errorMessage = "[data-test='error']";

        public LoginPage(IPage page)
        {
            _page = page;
        }

        public async Task NavigateToAsync(string baseUrl)
        {
            await _page.GotoAsync(baseUrl);
        }

        public async Task PerformLoginAsync(string username, string password)
        {
            await _page.FillAsync(_usernameInput, username);
            await _page.FillAsync(_passwordInput, password);
            await _page.ClickAsync(_loginButton);
        }

        public async Task<string> GetErrorMessageAsync()
        {
            return await _page.TextContentAsync(_errorMessage) ?? string.Empty;
        }
    }
}
