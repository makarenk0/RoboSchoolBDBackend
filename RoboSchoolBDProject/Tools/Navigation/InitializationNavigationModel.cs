using System;
using StartPageView = RoboSchoolBDProject.View.StartPageView;

namespace RoboSchoolBDProject.Tools.Navigation
{
    internal class InitializationNavigationModel : BaseNavigationModel
    {
        public InitializationNavigationModel(IContentOwner contentOwner) : base(contentOwner)
        {
            
        }
   
        protected override void InitializeView(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.StartPage:
                    ViewsDictionary.Add(viewType, new StartPageView());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }

        }
    }
}
