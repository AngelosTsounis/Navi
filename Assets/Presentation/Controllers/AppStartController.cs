using Navi.Core.Domain;
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
        private readonly GameSession _session;

        public AppStartController(ScreenNavigator nav, IPlayerProgress playerProgress, GameSession session)
        {
            _nav = nav;
            _progress = playerProgress;
            _session = session;
        }

        public void Start()
        {
            _progress.Load();

            if (!_progress.HasSeenIntro)
            {
                _session.CurrentPuzzleId = new PuzzleId("tutorial_3x3");

                _nav.ShowScreen(ScreenId.Intro);
            }
            else
            {
                _nav.ShowScreen(ScreenId.MainMenu);
            }
        }
    }
}
