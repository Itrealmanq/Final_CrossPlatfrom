using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiniprojectCross.Page;

namespace MiniprojectCross.ViewModel;

public partial class HomeViewModel : ObservableObject
{
    // นำทางไปที่ ProfilePage
    [RelayCommand]
    async Task ProfileAsync()
    {
        await GoToPage(nameof(ProfilePage));
    }

    // นำทางไปที่ LoginPage
    [RelayCommand]
    async Task LoginAsync()
    {
        await GoToPage(nameof(LoginPage));
    }
    // เมธอดกลางสำหรับการนำทาง
    async Task GoToPage(string page)
    {
        await Shell.Current.GoToAsync(page);
    }
}
