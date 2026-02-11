using Navi.Presentation.Navigation;
using Navi.Presentation.Navigation.Enums;
using VContainer.Unity;

namespace Navi.Presentation.Controllers
{
    public sealed class AppStartController : IStartable
    {
        private readonly ScreenNavigator _nav;

        public AppStartController(ScreenNavigator nav)
        {
            _nav = nav;
        }

        public void Start()
        {
            _nav.ShowScreen(ScreenId.Puzzle);
        }
    }
}
