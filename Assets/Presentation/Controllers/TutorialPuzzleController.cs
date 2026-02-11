using System;
using Navi.Core.Domain;
using VContainer.Unity;

namespace Navi.Presentation.Controllers
{
    public sealed class TutorialPuzzleController : IStartable, IDisposable
    {
        private readonly PuzzleFactory _factory;
        private readonly PuzzleView _view;

        private PuzzleGame _game;

        public TutorialPuzzleController(PuzzleFactory factory, PuzzleView view)
        {
            _factory = factory;
            _view = view;
        }

        public void Start()
        {
            _game = _factory.CreateTutorial3x3();
            _game.Changed += OnGameChanged;

            _view.Bind(_game, OnTilePressed);
            _view.Render();
        }

        public void Dispose()
        {
            if (_game != null)
                _game.Changed -= OnGameChanged;
        }

        private void OnTilePressed(int index)
        {
            if (_game == null) return;

            if (_game.TryMoveIndex(index))
            {
                if (_game.IsSolved())
                    _view.ShowSolved();
            }
        }

        private void OnGameChanged() => _view.Render();
    }
}
