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
        private bool _isBoundAndValid;

        public void Bind(PuzzleGame game, Action<int> onTilePressed)
        {
            _game = game;
            _onTilePressed = onTilePressed;
            _isBoundAndValid = false;

            if (_game == null)
            {
                Debug.LogError("PuzzleView.Bind called with null game.", this);
                return;
            }

            if (tileButtons == null || tileButtons.Length == 0)
            {
                Debug.LogError("PuzzleView has no tileButtons assigned.", this);
                return;
            }

            if (_game.Tiles.Count != tileButtons.Length)
            {
                Debug.LogError(
                    $"PuzzleView tileButtons length ({tileButtons.Length}) does not match game tile count ({_game.Tiles.Count}).",
                    this
                );
                return;
            }

            // Wire button clicks once
            for (int i = 0; i < tileButtons.Length; i++)
            {
                int idx = i;
                tileButtons[i].onClick.RemoveAllListeners();
                tileButtons[i].onClick.AddListener(() => _onTilePressed?.Invoke(idx));
            }

            if (solvedLabel != null) solvedLabel.SetActive(false);
            _isBoundAndValid = true;
        }

        public void Render()
        {
            if (!_isBoundAndValid || _game == null) return;

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
