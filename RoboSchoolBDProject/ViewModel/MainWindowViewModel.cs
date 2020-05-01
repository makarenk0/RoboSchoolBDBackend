using RoboSchoolBDProject.Tools.Managers;
using RoboSchoolBDProject.Tools.MVVM;
using RoboSchoolBDProject.Tools.Navigation;


namespace RoboSchoolBDProject.ViewModel
{
    class MainWindowViewModel : BaseViewModel, IContentOwner
    {
        private INavigatable _content;
       

        public MainWindowViewModel()
        {
            NavigationManager.Instance.Initialize(new InitializationNavigationModel(this));
            NavigationManager.Instance.Navigate(ViewType.StartPage);
        }


        public INavigatable Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }
    }
}
