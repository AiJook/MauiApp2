using MauiApp2.Model.RegisterCourseModel;
using MauiApp2.ViewModel;

namespace MauiApp2.Pages;

[QueryProperty("UserEmail", "UserEmail")]
public partial class RegisterCoursePage : ContentPage
{
    public string? UserEmail { get; set; }
    private RegisterCourseViewModel _viewModel;
    public RegisterCoursePage()
    {
        InitializeComponent();
        _viewModel = new RegisterCourseViewModel();
        BindingContext = _viewModel;
        System.Diagnostics.Debug.WriteLine("📌 BindingContext ของ RegisterCoursePage ถูกตั้งค่าแล้ว!");

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        System.Diagnostics.Debug.WriteLine("✅ OnAppearing() ถูกเรียก!");

        await _viewModel.LoadRegisterCoursesAsync(UserEmail); // โหลดรายวิชาทันทีที่หน้าแสดง

        if (!string.IsNullOrEmpty(UserEmail))
        {
            System.Diagnostics.Debug.WriteLine($"📩 UserEmail received in CoursePage: {UserEmail}");

            // สมมติว่ามี Label แสดง email
            // emailLabel.Text = $"ลงทะเบียนในชื่อ: {UserEmail}";
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("❌ No UserEmail received");
        }
    }

    public async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // ส่ง UserEmail กลับไปที่หน้า IndexPage เมื่อกดปุ่มย้อนกลับ
        await Shell.Current.GoToAsync($"IndexPage?UserEmail={Uri.EscapeDataString(UserEmail)}");
    }

}
