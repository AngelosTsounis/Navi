using System;
using System.Collections.Generic;
using UnityEngine;
using Navi.Presentation.Navigation.Views;

namespace Navi.Presentation.Navigation
{
    public sealed class ScreenRegistry : MonoBehaviour
    {
        [SerializeField] private ScreenView[] screens = Array.Empty<ScreenView>();
        [SerializeField] private OverlayView[] overlays = Array.Empty<OverlayView>();

        public IReadOnlyList<ScreenView> Screens => screens;
        public IReadOnlyList<OverlayView> Overlays => overlays;
    }
}
