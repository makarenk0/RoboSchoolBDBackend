using RoboSchoolBDProject.Tools.MVVM;

namespace RoboSchoolBDProject.ViewModel
{
    class StartPageViewModel : BaseViewModel
    {
        private RelayCommand<object> _signManagerCommand;
        private RelayCommand<object> _signTeacherCommand;
        private RelayCommand<object> _signAdministartorCommand;
    
        public StartPageViewModel()
        {
           
        }

        #region Commands
        public RelayCommand<object> SignManagerCommand
        {
            get
            {
                return _signManagerCommand ?? (_signManagerCommand = new RelayCommand<object>(SignManagerImp,
                    o => true));
            }
        }
        public RelayCommand<object> SignTeacherCommand
        {
            get
            {
                return _signTeacherCommand ?? (_signTeacherCommand = new RelayCommand<object>(SignTeacherImp,
                    o => true));
            }
        }
        public RelayCommand<object> SignAdministartorCommand
        {
            get
            {
                return _signAdministartorCommand ?? (_signAdministartorCommand = new RelayCommand<object>(SignAdministartorImp,
                    o => true));
            }
        }
        #endregion

        private void SignManagerImp(object obj)
        {
      
        }
        private void SignTeacherImp(object obj)
        {

        }
        private void SignAdministartorImp(object obj)
        {

        }

    }
}
