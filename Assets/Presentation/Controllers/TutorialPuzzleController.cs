using System;
using Navi.Core.Domain;
using Navi.Presentation.Navigation;
using Navi.Presentation.Navigation.Enums;
using VContainer.Unity;

namespace Navi.Presentation.Controllers
{
    public sealed class TutorialPuzzleController : IStartable, IDisposable
    {
        private readonly PuzzleFactory _factory;
        private readonly PuzzleView _view;
        private readonly ScreenNavigator _nav;

        private PuzzleGame _game;

        public TutorialPuzzleController(PuzzleFactory factory, PuzzleView view, ScreenNavigator nav)
        {
            _factory = factory;
            _view = view;
            _nav = nav;
        }

        public void Start()
        {
            _nav.ScreenShown += OnScreenShown;

            // If we already are on Puzzle (event may have fired before we subscribed)
            if (_nav.CurrentScreenId == ScreenId.Puzzle)
                StartPuzzleIfNeeded();
        }

        public void Dispose()
        {
            _nav.ScreenShown -= OnScreenShown;
            if (_game != null) _game.Changed -= OnGameChanged;
        }

        private void OnScreenShown(ScreenId id)
        {
            if (id == ScreenId.Puzzle)
                StartPuzzleIfNeeded();
        }

        private void StartPuzzleIfNeeded()
        {
            if (_game != null) return;

            _game = _factory.CreateTutorial3x3();
            _game.Changed += OnGameChanged;

            _view.Bind(_game, OnTilePressed);
            RefreshView();
        }

        private void OnTilePressed(int index) => _game?.TryMoveIndex(index);

        private void OnGameChanged() => RefreshView();

        private void RefreshView()
        {
            if (_game == null) return;

            bool solved = _game.IsSolved();
            _view.Render(showEmptyPiece: solved);
            _view.SetSolved(solved);
        }
    }
}
