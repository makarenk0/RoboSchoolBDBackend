using RoboSchoolBDProject.ViewModel;
using System.Windows.Controls;


namespace RoboSchoolBDProject.View
{
    /// <summary>
    /// Логика взаимодействия для StartPageView.xaml
    /// </summary>
    public partial class StartPageView : UserControl
    {
        public StartPageView()
        {
            DataContext = new StartPageViewModel();
            InitializeComponent();
        }
    }
}
