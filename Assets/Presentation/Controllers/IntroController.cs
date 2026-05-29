using Navi.Core.Interfaces;
using Navi.Presentation.Navigation;
using Navi.Presentation.Navigation.Enums;
using VContainer.Unity;

namespace Navi.Presentation.Controllers
{
    public sealed class IntroController : IStartable, System.IDisposable
    {
        private readonly IPlayerProgress _progress;
        private readonly ScreenNavigator _nav;
        private readonly TutorialPuzzleController _tutorial;

        private bool _completed;

        public IntroController(
            IPlayerProgress progress,
            ScreenNavigator nav,
            TutorialPuzzleController tutorial)
        {
            _progress = progress;
            _nav = nav;
            _tutorial = tutorial;
        }

        public void Start()
        {
            UnityEngine.Debug.Log("IntroController subscribed to tutorial.Solved");

            _tutorial.Solved += OnTutorialSolved;
        }

        public void Dispose()
        {
            _tutorial.Solved -= OnTutorialSolved;
        }

        private void OnTutorialSolved()
        {
            UnityEngine.Debug.Log("IntroController received Solved, navigating to MainMenu");

            if (_completed)
                return;

            _completed = true;

            _progress.HasSeenIntro = true;
            _progress.Save();

            _nav.ShowScreen(ScreenId.MainMenu);
        }
    }
}
