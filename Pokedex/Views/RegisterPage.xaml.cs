using Newtonsoft.Json;
using System.Text;

namespace Pokedex.Views;

public partial class RegisterPage : ContentPage
{
    private string RegisterUrl = "http://192.168.1.14:5000/register";

    public RegisterPage()
	{
		InitializeComponent();
	}

    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        string username = Username.Text; //Recupero Username
        string password = Password.Text; //Recupero Password
        string confirmPassword = ConfirmPassword.Text; //Recupero Conferma Password

        //I campi devono essere inseriti
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Please fill all fields.", "OK");
            return;
        }

        //Le due password devono coincidere
        if (password != confirmPassword)
        {
            await DisplayAlert("Error", "Passwords do not match.", "OK");
            return;
        }

        HttpClient client = new HttpClient();

        try
        {
            var credentials = new { username, password };
            string json = JsonConvert.SerializeObject(credentials);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(RegisterUrl, content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Success", "Registration successful! You can now login.", "OK");
                await Navigation.PopAsync(); // Torna alla pagina di login
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", $"Server error: {response.StatusCode}\n{errorContent}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}