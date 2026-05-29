using Navi.Core.Domain;
using Navi.Core.Interfaces;
using Navi.Presentation.Navigation;
using Navi.Presentation.Navigation.Enums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

namespace Navi.Presentation.Views.MainMenu
{
    public sealed class MainMenuView : MonoBehaviour
    {
        [FormerlySerializedAs("playButton")]
        [SerializeField] private Button surprisePuzzleButton;
        [SerializeField] private Button adventuresButton;
        [SerializeField] private Button galleryButton;
        [SerializeField] private Button shopButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button resetIntroButton;

        private ScreenNavigator _nav;
        private GameSession _session;
        private IPlayerProgress _progress;

        [Inject]
        public void Construct(ScreenNavigator nav, GameSession session, IPlayerProgress progress)
        {
            _nav = nav;
            _session = session;
            _progress = progress;
        }

        private void Awake()
        {
            Bind(surprisePuzzleButton, OnSurprisePuzzleClicked);
            Bind(adventuresButton, OnAdventuresClicked);
            Bind(galleryButton, OnNotImplementedClicked);
            Bind(shopButton, OnNotImplementedClicked);
            Bind(settingsButton, OnNotImplementedClicked);
            Bind(exitButton, OnExitClicked);
            Bind(resetIntroButton, OnResetIntroClicked);
        }

        private static void Bind(Button button, UnityAction action)
        {
            if (button == null)
                return;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }

        private void OnSurprisePuzzleClicked()
        {
            _session.CurrentPuzzleId = new PuzzleId("fast_4x4");
            _nav.ShowScreen(ScreenId.Puzzle);
        }

        private void OnAdventuresClicked()
        {
            if (!_nav.TryShowOverlay(OverlayId.Adventures))
                UnityEngine.Debug.LogWarning("Adventures overlay is not registered in ScreenRegistry yet.");
        }

        private static void OnNotImplementedClicked()
        {
            UnityEngine.Debug.Log("This main menu option is not implemented yet.");
        }

        private void OnResetIntroClicked()
        {
            _progress.ResetIntro();
            UnityEngine.Debug.Log("Intro reset from main menu test button.");
        }

        private static void OnExitClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
