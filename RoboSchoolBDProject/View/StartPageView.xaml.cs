using RoboSchoolBDProject.Tools.Navigation;
using RoboSchoolBDProject.ViewModel;
using System.Windows.Controls;


namespace RoboSchoolBDProject.View
{
    /// <summary>
    /// Логика взаимодействия для StartPageView.xaml
    /// </summary>
    public partial class StartPageView : UserControl, INavigatable
    {
        public StartPageView()
        {
            DataContext = new StartPageViewModel();
            InitializeComponent();
        }
    }
}
