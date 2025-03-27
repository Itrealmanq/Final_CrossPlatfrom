namespace MiniprojectCross.Page;

public partial class LoginPage : ContentPage
{public LoginPage()
        {
            InitializeComponent();
            
            // Set up event handlers
            LoginButton.Clicked += OnLoginButtonClicked;
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;
            
            // Validate inputs
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("แจ้งเตือน", "กรุณากรอกอีเมลและรหัสผ่าน", "ตกลง");
                return;
            }
            
            // TODO: Implement your authentication logic here
            bool isAuthenticated = await AuthenticateUser(email, password);
            
            if (isAuthenticated)
            {
                // Navigate to main page
                await Navigation.PushAsync(new HomePage());
            }
            else
            {
                await DisplayAlert("แจ้งเตือน", "อีเมลหรือรหัสผ่านไม่ถูกต้อง", "ตกลง");
            }
        }
        
        private async Task<bool> AuthenticateUser(string email, string password)
        {
            // TODO: Implement actual authentication against your backend
            // This is just a placeholder
            return email == "1" && password == "1";
        }

      
}