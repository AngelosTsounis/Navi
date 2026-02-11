using Navi.Presentation.Navigation.Enums;
using UnityEngine;

namespace Navi.Presentation.Navigation.Views
{
    public sealed class OverlayView : MonoBehaviour
    {
        [field: SerializeField] public OverlayId Id { get; private set; }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}
