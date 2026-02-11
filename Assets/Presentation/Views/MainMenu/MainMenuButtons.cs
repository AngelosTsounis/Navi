using Navi.Presentation.Navigation;
using Navi.Presentation.Navigation.Enums;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public sealed class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private Button adventuresButton;

    private ScreenNavigator _nav;

    [Inject]
    public void Construct(ScreenNavigator nav) => _nav = nav;

    private void Awake()
    {
        adventuresButton.onClick.AddListener(() => _nav.ShowOverlay(OverlayId.Adventures));
    }
}
