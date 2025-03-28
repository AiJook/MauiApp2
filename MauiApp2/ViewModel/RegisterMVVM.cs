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
		private ObservableCollection<RegisterCourseModel> registeredSubjects = new();
		private List<Student> students;
		private List<RegisterCourseModel> allRegistrations;
		[ObservableProperty]
		private string email = "";

		[ObservableProperty]
		private string password = "";


		public RegisterCourseViewModel()
		{
		}

		public async Task LoadRegisterCoursesAsync(string studentEmail)
		{
			try
			{
				System.Diagnostics.Debug.WriteLine("🔄 กำลังโหลด students.json และ registrations.json...");

				// โหลดข้อมูลนักเรียนจาก students.json
				var studentStream = await FileSystem.OpenAppPackageFileAsync("students.json");
				var studentReader = new StreamReader(studentStream);
				var studentJson = await studentReader.ReadToEndAsync();
				System.Diagnostics.Debug.WriteLine(studentJson);


				students = JsonSerializer.Deserialize<List<Student>>(studentJson);
				System.Diagnostics.Debug.WriteLine($"จำนวนข้อมูลนักเรียนใน students.json: {students.Count}");

				foreach (var student in students)
				{
					System.Diagnostics.Debug.WriteLine($"👤 ชื่อ: {student.Name}, อีเมล: {student.Email}");
				}

				if (students == null || students.Count == 0)
				{
					System.Diagnostics.Debug.WriteLine("❌ ไม่พบข้อมูลนักเรียน!");
					return;
				}

				// ค้นหานักเรียนจากอีเมล
				// var studentData = students.FirstOrDefault(s => s.Email == studentEmail);
				var studentData = students.FirstOrDefault(s => string.Equals(s.Email, studentEmail, StringComparison.OrdinalIgnoreCase));

				if (studentData == null)
				{
					System.Diagnostics.Debug.WriteLine($"❌ ไม่พบนักเรียนที่มีอีเมล: {studentEmail}");
					return;
				}

				string studentId = studentData.StudentId; // ได้ studentId มาแล้ว

				System.Diagnostics.Debug.WriteLine($"✅ พบ StudentId: {studentId} จากอีเมล {studentEmail}");

				// โหลดข้อมูลรายวิชาที่ลงทะเบียนจาก registrations.json
				var registerStream = await FileSystem.OpenAppPackageFileAsync("registrations.json");
				var registerReader = new StreamReader(registerStream);
				var registerJson = await registerReader.ReadToEndAsync();
				System.Diagnostics.Debug.WriteLine(registerJson);

				allRegistrations = JsonSerializer.Deserialize<List<RegisterCourseModel>>(registerJson);

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

					// เพิ่มข้อมูลการลงทะเบียนของนักเรียนที่ตรงกับ studentId ลงใน registeredSubjects
					registeredSubjects.Add(studentRegistration);
					System.Diagnostics.Debug.WriteLine($"📚 StudentId: {studentRegistration.StudentId}, จำนวนการลงทะเบียน: {studentRegistration.RegisteredCourses.Count}");

					// foreach (var registeredCourse in studentRegistration.RegisteredCourses)
					// {
					// 	System.Diagnostics.Debug.WriteLine($"📚 เทอม: {registeredCourse.Term}, ปี: {registeredCourse.Year}, จำนวนวิชา: {registeredCourse.Subjects.Count}");

					// 	foreach (var subject in registeredCourse.Subjects)
					// 	{
					// 		System.Diagnostics.Debug.WriteLine($"📚 ชื่อวิชา: {subject.SubjectName}, รหัสวิชา: {subject.SubjectId}");
					// 	}
					// }
				});
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"❌ Error loading courses: {ex.Message}");
			}
		}
	}
}
