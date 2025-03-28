using MauiApp2.ViewModel;

namespace MauiApp2.Pages;

[QueryProperty(nameof(UserEmail), "UserEmail")]
public partial class IndexPage : ContentPage
{
    public string UserEmail { get; set; }

    public IndexPage()
    {
        InitializeComponent();
        var viewModel = new IndexPageViewModel();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is IndexPageViewModel viewModel)
        {
            viewModel.UserEmail = UserEmail;
            viewModel.LoadStudentsAsync(); // โหลดข้อมูลใหม่เมื่อหน้าแสดง
        }
    }
}
