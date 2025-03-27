using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiniprojectCross.Model;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;

namespace MiniprojectCross.ViewModel
{
    public partial class ShowDataStudent : ObservableObject
    {
        [ObservableProperty]
        private User user;

        [ObservableProperty]
        private PreviousSemester selectedPreviousSemester;

        [ObservableProperty]
        private ObservableCollection<AvailableCourse> inMemoryAvailableCourses;

        [ObservableProperty]
        private ObservableCollection<CurrentSemesterSubject> inMemoryRegisteredCourses;

        [ObservableProperty]
        private ObservableCollection<AvailableCourse> filteredAvailableCourses;

        [ObservableProperty]
        private string searchText;

        public ObservableCollection<CurrentSemesterSubject> CurrentSemesterSubjects =>
            new ObservableCollection<CurrentSemesterSubject>(User?.CurrentSemester?.Subjects ?? new List<CurrentSemesterSubject>());

        public ObservableCollection<PreviousSemesterSubject> PreviousSemesterSubjects =>
            new ObservableCollection<PreviousSemesterSubject>(SelectedPreviousSemester?.Subjects ?? new List<PreviousSemesterSubject>());

        public string FullName => User?.Student?.Profile != null ?
            $"{User.Student.Profile.Firstname} {User.Student.Profile.Lastname}" : string.Empty;

        public string CurrentSemesterTermDisplay =>
            User?.CurrentSemester != null ?
            $"ภาคการศึกษา {User.CurrentSemester.Term}/{User.CurrentSemester.DisplayAcademicYear}" :
            "ภาคการศึกษา N/A";

        public string SelectedPreviousSemesterDisplay =>
            SelectedPreviousSemester != null ?
            $"ภาคการศึกษา {SelectedPreviousSemester.Term}/{SelectedPreviousSemester.DisplayAcademicYear}" :
            "ภาคการศึกษา N/A";

        public string PreviousSemester1Display =>
            User?.PreviousSemesters?.Count > 0 ?
            $"{User.PreviousSemesters[0].Term}/{User.PreviousSemesters[0].DisplayAcademicYear}" :
            "N/A";

        public string PreviousSemester2Display =>
            User?.PreviousSemesters?.Count > 1 ?
            $"{User.PreviousSemesters[1].Term}/{User.PreviousSemesters[1].DisplayAcademicYear}" :
            "N/A";

        public string PreviousSemester3Display =>
            User?.PreviousSemesters?.Count > 2 ?
            $"{User.PreviousSemesters[2].Term}/{User.PreviousSemesters[2].DisplayAcademicYear}" :
            "N/A";

        public long TotalRegisteredCredits => InMemoryRegisteredCourses?.Sum(s => s.Credits) ?? 0;

        public ShowDataStudent()
        {
            LoadDataAsync();
        }

        async Task LoadDataAsync()
        {
            var user = await ReadJsonAsync();
            if (user != null)
            {
                User = user;
                SelectedPreviousSemester = User.PreviousSemesters?.FirstOrDefault();

                // Initialize in-memory registered courses with previously registered courses
                InMemoryRegisteredCourses = new ObservableCollection<CurrentSemesterSubject>(
                    User.CurrentSemester?.Subjects ?? new List<CurrentSemesterSubject>());

                // Initialize in-memory available courses, excluding already registered courses
                var registeredCourseIdsAndSections = InMemoryRegisteredCourses
                    .Select(s => (s.Id, s.Section))
                    .ToHashSet();
                InMemoryAvailableCourses = new ObservableCollection<AvailableCourse>(
                    (User.AvailableCourses ?? new List<AvailableCourse>())
                        .Where(c => !registeredCourseIdsAndSections.Contains((c.Id, c.Section)))
                        .ToList());

                // Initialize filtered available courses
                FilteredAvailableCourses = new ObservableCollection<AvailableCourse>(InMemoryAvailableCourses);

                OnPropertyChanged(nameof(CurrentSemesterSubjects));
                OnPropertyChanged(nameof(PreviousSemesterSubjects));
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(CurrentSemesterTermDisplay));
                OnPropertyChanged(nameof(PreviousSemester1Display));
                OnPropertyChanged(nameof(PreviousSemester2Display));
                OnPropertyChanged(nameof(PreviousSemester3Display));
                OnPropertyChanged(nameof(SelectedPreviousSemesterDisplay));
                OnPropertyChanged(nameof(TotalRegisteredCredits));
            }
        }

        [RelayCommand]
        public void SelectSemester(PreviousSemester semester)
        {
            SelectedPreviousSemester = semester;
            OnPropertyChanged(nameof(PreviousSemesterSubjects));
            OnPropertyChanged(nameof(PreviousSemester1Display));
            OnPropertyChanged(nameof(PreviousSemester2Display));
            OnPropertyChanged(nameof(PreviousSemester3Display));
            OnPropertyChanged(nameof(SelectedPreviousSemesterDisplay));
        }

        [RelayCommand]
        public void RegisterCourse(AvailableCourse course)
        {
            // Remove the course from in-memory available courses
            InMemoryAvailableCourses.Remove(course);
            FilteredAvailableCourses.Remove(course);

            // Convert the course to a CurrentSemesterSubject and add it to in-memory registered courses
            var subject = course.ToCurrentSemesterSubject();
            InMemoryRegisteredCourses.Add(subject);

            // Update total credits
            OnPropertyChanged(nameof(TotalRegisteredCredits));
        }

        [RelayCommand]
        public async Task WithdrawCourse(CurrentSemesterSubject subject)
        {
            // Confirmation dialog
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "ยืนยันการถอนวิชา",
                $"คุณต้องการถอนวิชา {subject.Name} หรือไม่?",
                "ตกลง",
                "ยกเลิก");

            if (!confirm) return;

            // Remove the subject from in-memory registered courses
            InMemoryRegisteredCourses.Remove(subject);

            // Convert back to AvailableCourse and add to in-memory available courses
            var course = new AvailableCourse
            {
                Id = subject.Id,
                Name = subject.Name,
                NameEng = subject.NameEng,
                Section = subject.Section,
                Credits = subject.Credits,
                Instructor = subject.Instructor,
                Schedule = subject.Schedule,
                MidtermExam = subject.MidtermExam,
                FinalExam = subject.FinalExam,
                AvailableSeats = 1, // Reset to 1 for simplicity
                MaxSeats = 40, // Default value, adjust as needed
                Prerequisite = new List<string>() // Adjust based on actual prerequisites
            };
            InMemoryAvailableCourses.Add(course);
            FilteredAvailableCourses.Add(course);

            // Update total credits
            OnPropertyChanged(nameof(TotalRegisteredCredits));
        }

        [RelayCommand]
        public void SearchCourses()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredAvailableCourses = new ObservableCollection<AvailableCourse>(InMemoryAvailableCourses);
            }
            else
            {
                var filtered = InMemoryAvailableCourses
                    .Where(c => c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                c.NameEng.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                c.Id.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                FilteredAvailableCourses = new ObservableCollection<AvailableCourse>(filtered);
            }
        }

        async Task<User> ReadJsonAsync()
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("mockdata.json");
                using var reader = new StreamReader(stream);
                var contents = await reader.ReadToEndAsync();
                return User.FromJson(contents);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}