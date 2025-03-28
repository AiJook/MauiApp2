using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp2.Model.StudentNamespace;
using Newtonsoft.Json;

namespace MauiApp2.ViewModel;

public partial class ProfileMVVM : ObservableObject
{
    [ObservableProperty]
    private string email = "";

    [ObservableProperty]
    private string password = "";

    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private string age = "";

    [ObservableProperty]
    private string studentId = "";

    private List<Student> students;

    public ProfileMVVM()
    {
        students = new List<Student>();
        _ = InitializeAsync();
    }
     public async Task InitializeAsync()
    {
        await LoadProfileAsync();
    }


    private async Task LoadProfileAsync()
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


            var student = students.FirstOrDefault(s => s.Email == Email);
            if (student != null)
            {
                Name = student.Name;
                Age = student.Age.ToString();
                StudentId = student.StudentId;
                Password = student.Password;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Student not found");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading students: {ex.Message}");
        }
        
    }
}