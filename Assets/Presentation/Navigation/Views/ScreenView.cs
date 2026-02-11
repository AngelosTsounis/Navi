using Navi.Presentation.Navigation.Enums;
using UnityEngine;

namespace Navi.Presentation.Navigation.Views
{
    public sealed class ScreenView : MonoBehaviour
    {
        [field: SerializeField] public ScreenId Id { get; private set; }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}
