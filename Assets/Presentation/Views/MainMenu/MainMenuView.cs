using Navi.Core.Domain;
using Navi.Presentation.Navigation;
using Navi.Presentation.Navigation.Enums;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Navi.Presentation.Views.MainMenu
{
    public sealed class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button playButton;

        private ScreenNavigator _nav;
        private GameSession _session;

        [Inject]
        public void Construct(ScreenNavigator nav, GameSession session)
        {
            _nav = nav;
            _session = session;
        }

        private void Awake()
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(OnPlayClicked);
        }

        private void OnPlayClicked()
        {
            _session.CurrentPuzzleId = new PuzzleId("tutorial_3x3");

            _nav.ShowScreen(ScreenId.Puzzle);
        }
    }
}
