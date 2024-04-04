namespace Bookstore_OOP.View;
using Bookstore_OOP.ViewModel;
public partial class DirectoryView : ContentPage
{
    private DirectoryViewModel _directoryViewModel;
    public DirectoryView()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        _directoryViewModel = new();
        BindingContext = _directoryViewModel;

        base.OnNavigatedTo(args);
    }
}