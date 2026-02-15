using System;
using UnityEngine;
using UnityEngine.UI;

namespace Navi.Presentation.Views.Intro
{
    public sealed class IntroView : MonoBehaviour
    {
        [SerializeField] private Button continueButton;

        public event Action ContinueClicked;

        private void Awake()
        {
            if (continueButton == null)
            {
                UnityEngine.Debug.LogError("IntroView: continueButton not assigned.", this);
                return;
            }

            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(() => ContinueClicked?.Invoke());
        }
    }
}
