using System;
using Navi.Core.Domain;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Navi.Presentation.Controllers
{
    public sealed class PuzzleView : MonoBehaviour
    {
        [SerializeField] private Button[] tileButtons = Array.Empty<Button>();
        [SerializeField] private TMP_Text[] tileTexts = Array.Empty<TMP_Text>();
        [SerializeField] private GameObject solvedLabel;

        private PuzzleGame _game;
        private Action<int> _onTilePressed;

        public void Bind(PuzzleGame game, Action<int> onTilePressed)
        {
            _game = game;
            _onTilePressed = onTilePressed;

            // Wire button clicks once
            for (int i = 0; i < tileButtons.Length; i++)
            {
                int idx = i;
                tileButtons[i].onClick.RemoveAllListeners();
                tileButtons[i].onClick.AddListener(() => _onTilePressed?.Invoke(idx));
            }

            if (solvedLabel != null) solvedLabel.SetActive(false);
        }

        public void Render()
        {
            if (_game == null) return;

            for (int i = 0; i < tileButtons.Length; i++)
            {
                int val = _game.Tiles[i];
                bool isEmpty = val == 0;

                if (i < tileTexts.Length && tileTexts[i] != null)
                    tileTexts[i].text = isEmpty ? "" : val.ToString();

                tileButtons[i].interactable = !isEmpty;
            }
        }

        public void ShowSolved()
        {
            if (solvedLabel != null) solvedLabel.SetActive(true);
        }
    }
}
