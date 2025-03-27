using MiniprojectCross.ViewModel;

namespace MiniprojectCross.Page;

public partial class CourseRegistrationPage : ContentPage
{
    public CourseRegistrationPage()
    {
        InitializeComponent();
        BindingContext = new ShowDataStudent();
    }

    private async void OnClickedHome(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var viewModel = BindingContext as ShowDataStudent;
        if (viewModel != null)
        {
            viewModel.SearchCoursesCommand.Execute(null);
        }
    }
}