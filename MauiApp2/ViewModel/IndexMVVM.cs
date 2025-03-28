using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp2.Model.StudentNamespace;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MauiApp2.ViewModel
{
   
    public partial class IndexPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<Student> students = new List<Student>();

        [ObservableProperty]
        private Student? loggedInUser;

        [ObservableProperty]
        private string userEmail;

        

        public IndexPageViewModel()
        {
            // ไม่โหลดข้อมูลที่ constructor เพราะ UserEmail ยังไม่ถูกเซ็ต
        }

        public async Task LoadStudentsAsync()
        {
            try
            {
                using var stream = await Microsoft.Maui.Storage.FileSystem.OpenAppPackageFileAsync("students.json");
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();
                students = JsonConvert.DeserializeObject<List<Student>>(json) ?? new List<Student>();

                if (string.IsNullOrEmpty(UserEmail)) return;

                loggedInUser = students?.FirstOrDefault(s => s.Email == UserEmail);
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading students: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task GoToProfile()
        {
            if (loggedInUser != null)
            {
                var studentJson = JsonConvert.SerializeObject(loggedInUser);
                await Shell.Current.GoToAsync($"ProfilePage?StudentData={Uri.EscapeDataString(studentJson)}");
            }
        }

        [RelayCommand]
        public async Task GoToRegister()
        {
            if (!string.IsNullOrEmpty(UserEmail))
            {
                await Shell.Current.GoToAsync($"RegisterCoursePage?UserEmail={Uri.EscapeDataString(UserEmail)}");
            }
        }

        [RelayCommand]
        public async Task GoToCourses()
        {
            if (!string.IsNullOrEmpty(UserEmail))
            {
                await Shell.Current.GoToAsync($"CoursePage?UserEmail={Uri.EscapeDataString(UserEmail)}");
            }
        }
    }
}
