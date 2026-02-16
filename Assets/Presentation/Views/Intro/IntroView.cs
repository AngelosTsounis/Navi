using System;
using UnityEngine;
using UnityEngine.UI;

namespace Navi.Presentation.Views.Intro
{
    public sealed class IntroView : MonoBehaviour
    {
        [SerializeField] private Button continueButton;

        public event Action ContinueClicked;

        private void OnEnable()
        {
            if (continueButton == null)
            {
                return;
            }

            continueButton.onClick.AddListener(OnContinueClicked);
        }

        private void OnDisable()
        {
            if (continueButton != null)
                continueButton.onClick.RemoveListener(OnContinueClicked);
        }

        private void OnContinueClicked()
        {
            ContinueClicked?.Invoke();
        }
    }
}
