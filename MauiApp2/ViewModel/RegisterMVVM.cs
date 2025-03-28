using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp2.Model.RegisterCourseModel;
using MauiApp2.Model.StudentNamespace;
using Microsoft.Maui.Storage;

namespace MauiApp2.ViewModel
{
	public partial class RegisterCourseViewModel : ObservableObject
	{
		[ObservableProperty]
		private ObservableCollection<Subject> registeredSubjects = new();
		private List<Student> students;


		public RegisterCourseViewModel()
		{
		}

		public async Task LoadRegisterCoursesAsync(string studentEmail)
		{
			try
			{
				System.Diagnostics.Debug.WriteLine("🔄 กำลังโหลด students.json และ registrations.json...");

				// โหลดข้อมูลนักเรียนจาก students.json
				using var studentStream = await FileSystem.OpenAppPackageFileAsync("students.json");
				using var studentReader = new StreamReader(studentStream);
				var studentJson = await studentReader.ReadToEndAsync();

				students = JsonSerializer.Deserialize<List<Student>>(studentJson);

				if (students == null || students.Count == 0)
				{
					System.Diagnostics.Debug.WriteLine("❌ ไม่พบข้อมูลนักเรียน!");
					return;
				}

				// ค้นหานักเรียนจากอีเมล
				var studentData = students.FirstOrDefault(s => s.Email == studentEmail);

				if (studentData == null)
				{
					System.Diagnostics.Debug.WriteLine($"❌ ไม่พบนักเรียนที่มีอีเมล: {studentEmail}");
					return;
				}

				string studentId = studentData.StudentId; // ได้ studentId มาแล้ว

				System.Diagnostics.Debug.WriteLine($"✅ พบ StudentId: {studentId} จากอีเมล {studentEmail}");

				// โหลดข้อมูลรายวิชาที่ลงทะเบียนจาก registrations.json
				using var registerStream = await FileSystem.OpenAppPackageFileAsync("registrations.json");
				using var registerReader = new StreamReader(registerStream);
				var registerJson = await registerReader.ReadToEndAsync();

				var allRegistrations = JsonSerializer.Deserialize<List<RegisterCourseModel>>(registerJson);

				if (allRegistrations == null || allRegistrations.Count == 0)
				{
					System.Diagnostics.Debug.WriteLine("❌ ไม่พบข้อมูลการลงทะเบียนวิชา!");
					return;
				}

				// ค้นหาข้อมูลการลงทะเบียนของนักเรียน
				var studentRegistration = allRegistrations.FirstOrDefault(r => r.StudentId == studentId);

				if (studentRegistration == null || studentRegistration.RegisteredCourses.Count == 0)
				{
					System.Diagnostics.Debug.WriteLine("❌ นักเรียนคนนี้ยังไม่ได้ลงทะเบียนวิชา!");
					return;
				}

				System.Diagnostics.Debug.WriteLine($"✅ โหลดข้อมูลการลงทะเบียนสำหรับ StudentId: {studentId} สำเร็จ!");

				// อัปเดต registeredSubjects บน Main Thread
				MainThread.BeginInvokeOnMainThread(() =>
				{
					registeredSubjects.Clear(); // ลบข้อมูลเก่าออก

					foreach (var course in studentRegistration.RegisteredCourses)
					{
						foreach (var subject in course.Subjects)
						{
							System.Diagnostics.Debug.WriteLine($"📚 ชื่อวิชา: {subject.SubjectName}, รหัสวิชา: {subject.SubjectId}");
							registeredSubjects.Add(subject);
						}
					}
				});
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"❌ Error loading courses: {ex.Message}");
			}
		}
	}
}
