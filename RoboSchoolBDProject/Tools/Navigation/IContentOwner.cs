using System.Windows.Controls;

namespace RoboSchoolBDProject.Tools.Navigation
{
    internal interface IContentOwner
    {
        INavigatable Content { get; set; }
    }
}
