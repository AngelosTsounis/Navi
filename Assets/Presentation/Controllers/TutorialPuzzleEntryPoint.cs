using System;
using VContainer.Unity;

namespace Navi.Presentation.Controllers
{
    // Runs the single TutorialPuzzleController instance as an entry point.
    public sealed class TutorialPuzzleEntryPoint : IStartable, IDisposable
    {
        private readonly TutorialPuzzleController _tutorial;
        private bool _started;

        public TutorialPuzzleEntryPoint(TutorialPuzzleController tutorial)
        {
            _tutorial = tutorial;
        }

        public void Start()
        {
            if (_started) return;
            _started = true;
            _tutorial.Start();
        }

        public void Dispose() => _tutorial.Dispose();
    }
}
