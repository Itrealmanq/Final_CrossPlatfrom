using MiniprojectCross.Page;

namespace MiniprojectCross;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
		Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
		Routing.RegisterRoute(nameof(CurrentTermRegistrationPage), typeof(CurrentTermRegistrationPage));
		Routing.RegisterRoute(nameof(PastTermRegistrationPage), typeof(PastTermRegistrationPage));
		Routing.RegisterRoute(nameof(CourseRegistrationPage), typeof(CourseRegistrationPage));
		Routing.RegisterRoute(nameof(CourseWithdrawalPage), typeof(CourseWithdrawalPage));
	}
}
