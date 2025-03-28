using MauiApp2.ViewModel;

namespace MauiApp2.Pages;

[QueryProperty("UserEmail", "UserEmail")]
public partial class CoursePage : ContentPage
{
    public string? UserEmail { get; set; }
    private CourseViewModel _viewModel;
    public CoursePage()
    {
        InitializeComponent();
        _viewModel = new CourseViewModel();
        BindingContext = _viewModel;
        System.Diagnostics.Debug.WriteLine("📌 BindingContext ของ CoursePage ถูกตั้งค่าแล้ว!");

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        System.Diagnostics.Debug.WriteLine("✅ OnAppearing() ถูกเรียก!");

        await _viewModel.LoadCoursesAsync(); // โหลดรายวิชาทันทีที่หน้าแสดง
        System.Diagnostics.Debug.WriteLine($"📊 จำนวนรายวิชา: {_viewModel.AvailableCourses.Count}");

        foreach (var course in _viewModel.AvailableCourses)
        {
            System.Diagnostics.Debug.WriteLine($"📚 ชื่อวิชา: {course.SubjectName}, รหัสวิชา: {course.SubjectId}");
        }


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
    private async void OnRegisterButtonClicked(object sender, EventArgs e)
{
    // // คำสั่งในการลงทะเบียนวิชา
    // var course = (sender as Button)?.BindingContext as Course;
    // if (course != null)
    // {
    //     System.Diagnostics.Debug.WriteLine($"ลงทะเบียนวิชา {course.SubjectName}");
    //     // เพิ่มโค้ดสำหรับลงทะเบียนที่นี่
    // }
}

private async void OnWithdrawButtonClicked(object sender, EventArgs e)
{
    // // คำสั่งในการถอนวิชา
    // var course = (sender as Button)?.BindingContext as Course;
    // if (course != null)
    // {
    //     System.Diagnostics.Debug.WriteLine($"ถอนวิชา {course.SubjectName}");
    //     // เพิ่มโค้ดสำหรับถอนวิชาที่นี่
    // }
}

    public async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // ส่ง UserEmail กลับไปที่หน้า IndexPage เมื่อกดปุ่มย้อนกลับ
        await Shell.Current.GoToAsync($"IndexPage?UserEmail={Uri.EscapeDataString(UserEmail)}");
    }

}
