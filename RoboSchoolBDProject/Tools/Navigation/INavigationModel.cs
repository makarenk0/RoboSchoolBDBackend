namespace RoboSchoolBDProject.Tools.Navigation
{
    internal enum ViewType
    {
       StartPage
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}
