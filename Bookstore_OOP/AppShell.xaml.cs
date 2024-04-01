using Bookstore_OOP.View;
namespace Bookstore_OOP
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("AdminMainPage", typeof(AdminMainPage));
            Routing.RegisterRoute("UserMainPage", typeof(UserShell));
        }
    }
}