using System;
using Navi.Core.Domain;
using Navi.Core.Interfaces;
using Navi.Presentation.Navigation;
using Navi.Presentation.Navigation.Enums;
using VContainer.Unity;

namespace Navi.Presentation.Controllers
{
    // Runs the intro puzzle and the current fast puzzle screen.
    public sealed class TutorialPuzzleController : IStartable, IDisposable
    {
        private readonly PuzzleFactory _factory;
        private readonly PuzzleView _view;
        private readonly GameSession _session;
        private readonly IPuzzleCatalog _catalog;
        private readonly ScreenNavigator _nav;

        private PuzzleGame _game;
        private bool _hasSolved;

        public event Action Solved;

        public TutorialPuzzleController(
            PuzzleFactory factory,
            PuzzleView view,
            GameSession session,
            IPuzzleCatalog catalog,
            ScreenNavigator nav)
        {
            _factory = factory;
            _view = view;
            _session = session;
            _catalog = catalog;
            _nav = nav;
        }

        public void Start()
        {
            _nav.ScreenShown += OnScreenShown;

            // Safety: if Intro screen is already visible when we subscribe
            if (_nav.CurrentScreenId == ScreenId.Intro)
                StartPuzzleIfNeeded();
        }

        public void Dispose()
        {
            _nav.ScreenShown -= OnScreenShown;

            if (_game != null)
                _game.Changed -= OnGameChanged;
        }

        private void OnScreenShown(ScreenId id)
        {
            if (id == ScreenId.Intro)
                StartPuzzleIfNeeded();

            if (id == ScreenId.Puzzle)
                StartNewPuzzle();
        }

        private void StartPuzzleIfNeeded()
        {
            if (_game != null) return;

            StartNewPuzzle();
        }

        private void StartNewPuzzle()
        {
            if (_game != null)
                _game.Changed -= OnGameChanged;

            _hasSolved = false;

            var def = _catalog.Get(_session.CurrentPuzzleId);

            // IMPORTANT: view needs sprites from definition
            _view.SetSprites(def.PieceSprites);

            _game = _factory.CreateFromDefinition(def);
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
            if (solved)
                UnityEngine.Debug.Log("Puzzle is SOLVED. Tiles=" + string.Join(",", _game.Tiles));


            _view.Render(showEmptyPiece: solved);
            _view.SetSolved(solved);

            if (solved && !_hasSolved && _nav.CurrentScreenId == ScreenId.Intro)
            {
                UnityEngine.Debug.Log("Solved event invoked");
                _hasSolved = true;
                Solved?.Invoke();
            }
        }
    }
}
