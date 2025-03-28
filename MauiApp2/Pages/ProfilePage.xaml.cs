using MauiApp2.Model.StudentNamespace;
using Newtonsoft.Json;

namespace MauiApp2.Pages;

[QueryProperty("StudentData", "StudentData")]
public partial class ProfilePage : ContentPage
{
    private Student loggedInUser;

    // กำหนด Property เพื่อรับข้อมูล
    public string StudentData { get; set; }

    public ProfilePage()
    {
        InitializeComponent();
    }

    // เมื่อข้อมูลได้รับการโหลดจาก QueryProperty
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // ตรวจสอบและแสดงข้อมูลที่ส่งมา
        if (!string.IsNullOrEmpty(StudentData))
        {
            loggedInUser = JsonConvert.DeserializeObject<Student>(StudentData);
            nameLabel.Text = loggedInUser?.Name;
            emailLabel.Text = loggedInUser?.Email;
        }
    }
    public async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // ส่ง UserEmail กลับไปที่หน้า IndexPage เมื่อกดปุ่มย้อนกลับ
        await Shell.Current.GoToAsync($"IndexPage?UserEmail={Uri.EscapeDataString(loggedInUser.Email)}");
    }


}
