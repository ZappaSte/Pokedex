using Newtonsoft.Json;
using Pokedex.Models;
using Pokedex.ViewModel;
using System.Net.Http;
using System.Text;

namespace Pokedex.Views;

public partial class LoginPage : ContentPage
{
    public LoginPageViewModel _viewModel;

    private bool _loginSuccess;

    public LoginPage()
    {
        InitializeComponent();

        _viewModel = new LoginPageViewModel();
        BindingContext = _viewModel;

        // Rimuove l'intera barra di navigazione
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        string username = Username.Text;
        string password = Password.Text;

        if (_viewModel != null)
        {
            _loginSuccess = await _viewModel.LoginClicked(username, password);

            if (_loginSuccess)
            {
                await Navigation.PushAsync(new PokemonList());
            }
            else
            {
                ErrorLabel.IsVisible = true;
            }
        }

    }

    private async void OnRegisterButtonClickedAsync(object sender, EventArgs e)
    {
        // Naviga alla pagina di registrazione
        await Navigation.PushAsync(new RegisterPage());
    }
}