using Navi.Presentation.Navigation;
using Navi.Presentation.Navigation.Enums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VContainer;

namespace Navi.Presentation.Views.Puzzle
{
    public sealed class PuzzleScreenView : MonoBehaviour
    {
        [SerializeField] private Button optionsButton;

        private ScreenNavigator _nav;

        [Inject]
        public void Construct(ScreenNavigator nav)
        {
            _nav = nav;
        }

        private void Awake()
        {
            Bind(optionsButton, OnOptionsClicked);
        }

        private static void Bind(Button button, UnityAction action)
        {
            if (button == null)
                return;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }

        private void OnOptionsClicked()
        {
            _nav.ShowScreen(ScreenId.MainMenu);
        }
    }
}
