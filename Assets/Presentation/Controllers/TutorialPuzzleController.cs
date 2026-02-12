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
            RefreshView(); // initial draw + solved state
        }

        public void Dispose()
        {
            if (_game != null)
                _game.Changed -= OnGameChanged;
        }

        private void OnTilePressed(int index)
        {
            if (_game == null) return;

            // Attempt move; game will raise Changed if it actually moved
            _game.TryMoveIndex(index);
        }

        private void OnGameChanged() => RefreshView();

        private void RefreshView()
        {
            if (_game == null) return;

            bool solved = _game.IsSolved();
            _view.Render(showEmptyPiece: solved);     // reveal 0 only when solved
            _view.SetSolved(solved);                  // show/hide solved label
        }
    }
}
