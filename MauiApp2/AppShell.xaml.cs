using MauiApp2.Pages;

namespace MauiApp2;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
		Routing.RegisterRoute(nameof(IndexPage), typeof(IndexPage));
		Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
		Routing.RegisterRoute(nameof(CoursePage), typeof(CoursePage));
		Routing.RegisterRoute(nameof(RegisterCoursePage), typeof(RegisterCoursePage));
	}
}
