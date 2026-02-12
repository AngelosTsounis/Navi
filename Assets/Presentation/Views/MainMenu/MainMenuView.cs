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

        [Inject]
        public void Construct(ScreenNavigator nav)
        {
            _nav = nav;
        }

        private void Awake()
        {
            if (playButton == null)
            {
                UnityEngine.Debug.LogError("MainMenuView: playButton is not assigned.", this);
                return;
            }

            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(OnPlayClicked);
        }

        private void OnPlayClicked()
        {
            _nav.ShowScreen(ScreenId.Puzzle);
        }
    }
}
