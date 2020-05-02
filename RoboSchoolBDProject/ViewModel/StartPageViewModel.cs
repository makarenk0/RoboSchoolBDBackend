using RoboSchoolBDProject.Models;
using RoboSchoolBDProject.Tools.MVVM;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RoboSchoolBDProject.ViewModel
{
    class StartPageViewModel : BaseViewModel
    {
        private RelayCommand<object> _signManagerCommand;
        private RelayCommand<object> _signTeacherCommand;
        private RelayCommand<object> _signAdministartorCommand;


        HttpClientHandler clientHandler;

        static HttpClient client;

        public StartPageViewModel()
        {
            clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
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

        private async void SignManagerImp(object obj)
        {
            #region Testing
            Manager manager = await GetAPIAsync("https://localhost:44354/api/manager/1");  //change when backend start
            Console.WriteLine("API respond: ");
            Console.WriteLine(manager.Id);
            Console.WriteLine(manager.Name);
            #endregion

        }
        private void SignTeacherImp(object obj)
        {

        }
        private void SignAdministartorImp(object obj)
        {

        }

        #region Testing
        static async Task<Manager> GetAPIAsync(string path)

        {

            Manager manager = null;

            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)

            {

                manager = await response.Content.ReadAsAsync<Manager>();

            }

            return manager;

        }
        #endregion

    }
}
