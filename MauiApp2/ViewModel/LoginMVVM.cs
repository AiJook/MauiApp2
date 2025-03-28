using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp2.Model.StudentNamespace;
using MauiApp2.Pages;
using Newtonsoft.Json;

namespace MauiApp2.ViewModel;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string email = "";

    [ObservableProperty]
    private string password = "";

    [ObservableProperty]
    private Student? currentUser;

    private List<Student> students;

    public LoginViewModel()
    {
        students = new List<Student>();
        _ = InitializeAsync();
    }

    public async Task InitializeAsync()
    {
        await LoadStudentsAsync();
    }

    private async Task LoadStudentsAsync()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("LoadStudentsAsync called");

            using var stream = await FileSystem.OpenAppPackageFileAsync("students.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();

            System.Diagnostics.Debug.WriteLine($"JSON content: {json}");

            students = JsonConvert.DeserializeObject<List<Student>>(json) ?? new List<Student>();
            System.Diagnostics.Debug.WriteLine($"Loaded {students.Count} students");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading students: {ex.Message}");
        }
    }

    [RelayCommand]
async Task Login()
{
    System.Diagnostics.Debug.WriteLine("🔵 Start Login Process");
    System.Diagnostics.Debug.WriteLine($"Email: {Email}, Password: {Password}");

    if (students == null || students.Count == 0)
    {
        System.Diagnostics.Debug.WriteLine("❌ Students list is empty!");
        return;
    }

    System.Diagnostics.Debug.WriteLine($"Total Students Loaded: {students.Count}");

    // หานักเรียนที่มีอีเมลและรหัสผ่านตรง
    var student = students.FirstOrDefault(s => s.Email == Email && s.Password == Password);
    if (student != null)
    {
        System.Diagnostics.Debug.WriteLine("✅ Login Success!");

        CurrentUser = student;

        // ส่งข้อมูลไปยัง IndexPage ผ่าน Query Parameters (UserEmail)
        var route = $"{nameof(IndexPage)}?UserEmail={student.Email}";
        await Shell.Current.GoToAsync(route);  // ไปยังหน้า IndexPage
    }
    else
    {
        System.Diagnostics.Debug.WriteLine("❌ Login Failed! Invalid Email or Password.");
    }
}

}


    