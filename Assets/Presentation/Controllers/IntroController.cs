using Navi.Core.Interfaces;
using Navi.Presentation.Navigation;
using Navi.Presentation.Navigation.Enums;
using Navi.Presentation.Views.Intro;
using VContainer.Unity;

namespace Navi.Presentation.Controllers
{
    public sealed class IntroController : IStartable
    {
        private readonly IPlayerProgress _progress;
        private readonly ScreenNavigator _nav;
        private readonly IntroView _view;

        public IntroController(IPlayerProgress progress, ScreenNavigator nav, IntroView view)
        {
            _progress = progress;
            _nav = nav;
            _view = view;
        }

        public void Start()
        {
            _view.ContinueClicked += OnContinueClicked;
        }

        private void OnContinueClicked()
        {
            _progress.HasSeenIntro = true;
            _progress.Save();

            _nav.ShowScreen(ScreenId.MainMenu);
        }
    }
}
