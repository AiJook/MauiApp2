using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp2.Model;
using MauiApp2.Model.CourseModel;
using Microsoft.Maui.Storage;

namespace MauiApp2.ViewModel
{
    public partial class CourseViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Course> availableCourses = new();

        public CourseViewModel()
        {
        }

        public async Task LoadCoursesAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("🔄 กำลังโหลด courses.json...");

                using var stream = await FileSystem.OpenAppPackageFileAsync("courses.json");
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();

                var courses = JsonSerializer.Deserialize<List<Course>>(json);

                if (courses != null && courses.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"✅ โหลดรายวิชา {courses.Count} รายการสำเร็จ!");

                    // อัปเดต AvailableCourses บน Main Thread
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        AvailableCourses.Clear(); // ลบข้อมูลเก่าออก
                        foreach (var course in courses)
                        {
                            System.Diagnostics.Debug.WriteLine($"📚 ชื่อวิชา: {course.SubjectName ?? "ไม่มีชื่อ"} รหัสวิชา: {course.SubjectId ?? "ไม่มีรหัส"}");
                            AvailableCourses.Add(course); // เพิ่มวิชาใหม่
                        }
                    });
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("❌ ไม่พบข้อมูลใน JSON!");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error loading courses: {ex.Message}");
            }
        }

    }
}
