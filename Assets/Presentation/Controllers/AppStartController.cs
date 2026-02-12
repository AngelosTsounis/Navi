using Navi.Core.Interfaces;
using Navi.Presentation.Navigation;
using Navi.Presentation.Navigation.Enums;
using VContainer.Unity;

namespace Navi.Presentation.Controllers
{
    public sealed class AppStartController : IStartable
    {
        private readonly ScreenNavigator _nav;
        private readonly IPlayerProgress _progress;

        public AppStartController(ScreenNavigator nav, IPlayerProgress playerProgress)
        {
            _nav = nav;
            _progress = playerProgress;
        }

        public void Start()
        { 
            _progress.Load();

            if (_progress.HasSeenIntro)
            {
                _nav.ShowScreen(ScreenId.MainMenu);
            }
            else
            {
                _nav.ShowScreen(ScreenId.Intro);
            }
        }
    }
}
