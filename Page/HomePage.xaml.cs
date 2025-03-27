using MiniprojectCross.ViewModel;

namespace MiniprojectCross.Page;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
		BindingContext = new ShowDataStudent();
	}
	private async void OnDetailsClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(CurrentTermRegistrationPage));
	}
	private async void OnHistoryButtonClicked(object sender, EventArgs e)
{
    await Navigation.PushAsync(new PastTermRegistrationPage());
}
private async void OnDetailsClickedProfile(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(ProfilePage));
	}
	private async void OnClickedLogin(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
	private async void OnCourseButtonClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(CourseRegistrationPage));
	}
private async void OnWithdawalButtonClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(CourseWithdrawalPage));
	}
}