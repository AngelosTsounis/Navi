using System;
using Navi.Presentation.Navigation;
using Navi.Presentation.Navigation.Enums;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

[Obsolete("Use MainMenuView for main menu button wiring.")]
public sealed class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private Button adventuresButton;

    private ScreenNavigator _nav;

    [Inject]
    public void Construct(ScreenNavigator nav) => _nav = nav;

    private void Awake()
    {
        if (adventuresButton == null)
            return;

        adventuresButton.onClick.AddListener(OnAdventuresClicked);
    }

    private void OnAdventuresClicked()
    {
        if (_nav == null)
            return;

        if (!_nav.TryShowOverlay(OverlayId.Adventures))
            UnityEngine.Debug.LogWarning("Adventures overlay is not registered in ScreenRegistry yet.");
    }
}
