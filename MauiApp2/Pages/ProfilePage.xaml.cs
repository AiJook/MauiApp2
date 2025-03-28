using MauiApp2.Model.StudentNamespace;
using Newtonsoft.Json;

namespace MauiApp2.Pages;

[QueryProperty(nameof(StudentData), nameof(StudentData))]
public partial class ProfilePage : ContentPage
{
    private Student _loggedInUser;

    // Property สำหรับรับข้อมูลจาก QueryProperty
    public string StudentData { get; set; }

    public ProfilePage()
    {
        InitializeComponent();
        BindingContext = _loggedInUser;
    }

    // เมื่อข้อมูลได้รับการโหลดจาก QueryProperty
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (!string.IsNullOrEmpty(StudentData))
        {




            if (_loggedInUser != null)
            {

                _loggedInUser = JsonConvert.DeserializeObject<Student>(StudentData);
                emailLabel.Text = _loggedInUser?.Email;
                nameLabel.Text = _loggedInUser?.Name;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("❌ ไม่สามารถ Deserialize StudentData ได้!");
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("❌ StudentData เป็นค่าว่างหรือ null!");
        }
    }


    public async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // ตรวจสอบว่า _loggedInUser ไม่เป็น null ก่อนส่งข้อมูลกลับ
        if (_loggedInUser != null)
        {
            await Shell.Current.GoToAsync($"IndexPage?UserEmail={Uri.EscapeDataString(_loggedInUser.Email)}");
        }
        else
        {
            await Shell.Current.GoToAsync("IndexPage");
        }
    }
}
