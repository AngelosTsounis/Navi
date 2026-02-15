using UnityEngine;
using UnityEngine.UI;

namespace Navi.Presentation.Views.Debug
{
    public sealed class DevDebugView : MonoBehaviour
    {
        [SerializeField] private Button resetIntroButton;

        public System.Action ResetIntroClicked;

        private void Awake()
        {
            resetIntroButton.onClick.AddListener(() =>
            {
                ResetIntroClicked?.Invoke();
            });
        }
    }
}
