using MiniprojectCross.ViewModel;

namespace MiniprojectCross.Page;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
		BindingContext = new ShowDataStudent();
	}
	private async void OnDetailsClickedback(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}