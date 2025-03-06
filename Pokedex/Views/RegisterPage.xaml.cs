using Newtonsoft.Json;
using Pokedex.ViewModel;
using System.Text;

namespace Pokedex.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPageViewModel _viewModel;
    
    private bool _registerSuccess;

    public RegisterPage()
	{
		InitializeComponent();

        _viewModel = new RegisterPageViewModel();
        BindingContext = _viewModel;
	}

    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        string username = Username.Text; //Recupero Username
        string password = Password.Text; //Recupero Password
        string confirmPassword = ConfirmPassword.Text; //Recupero Conferma Password

        if (_viewModel != null)
        {
            _registerSuccess = await _viewModel.RegisterClicked(username, password, confirmPassword);

            if (_registerSuccess)
            {
                await DisplayAlert("Success", "Registration successful! You can now login.", "OK");
                await Navigation.PopAsync(); // Torna alla pagina di login
            }
            else
            {
                ErrorLabel.IsVisible = true;                
            }
        }        
    }
}