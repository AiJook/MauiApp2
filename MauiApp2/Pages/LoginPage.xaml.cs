using MauiApp2.ViewModel;

namespace MauiApp2.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		BindingContext = new LoginViewModel();
	}
	
}
public class LoginData
{
	public string Email { get; set; } = "";
	public string Password { get; set; } = "";
}