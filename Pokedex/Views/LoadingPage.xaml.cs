namespace Pokedex.Views;

public partial class LoadingPage : ContentPage
{
	public LoadingPage()
	{
		InitializeComponent();

		// Rimuove l'intera barra di navigazione
        NavigationPage.SetHasNavigationBar(this, false);
	}
}