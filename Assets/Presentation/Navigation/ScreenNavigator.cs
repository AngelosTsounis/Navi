using Navi.Presentation.Navigation.Enums;
using Navi.Presentation.Navigation.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Navi.Presentation.Navigation
{
    public sealed class ScreenNavigator
    {
        private readonly Dictionary<ScreenId, ScreenView> _screens;
        private readonly Dictionary<OverlayId, OverlayView> _overlays;

        public ScreenNavigator(ScreenRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            _screens = registry.Screens.ToDictionary(s => s.Id);
            _overlays = registry.Overlays.ToDictionary(o => o.Id);
        }

        public void ShowScreen(ScreenId id)
        {
            foreach (var s in _screens.Values) s.Hide();
            foreach (var o in _overlays.Values) o.Hide();

            if (!_screens.TryGetValue(id, out var screen))
                throw new InvalidOperationException($"Screen not found: {id}");

            screen.Show();
        }

        public void ShowOverlay(OverlayId id)
        {
            if (!_overlays.TryGetValue(id, out var overlay))
                throw new InvalidOperationException($"Overlay not found: {id}");

            overlay.Show();
        }

        public void HideOverlay(OverlayId id)
        {
            if (_overlays.TryGetValue(id, out var overlay))
                overlay.Hide();
        }
    }
}
