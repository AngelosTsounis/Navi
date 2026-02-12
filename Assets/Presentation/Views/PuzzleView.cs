using System;
using Navi.Core.Domain;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Navi.Presentation.Controllers
{
    public sealed class PuzzleView : MonoBehaviour
    {
        [Header("Buttons (click targets)")]
        [SerializeField] private Button[] tileButtons = Array.Empty<Button>();

        [Header("Optional number text")]
        [SerializeField] private TMP_Text[] tileTexts = Array.Empty<TMP_Text>();

        [Header("Image component on each tile (NOT a separate child required, but ok if you use one)")]
        [SerializeField] private Image[] tileImages = Array.Empty<Image>();

        [Header("Sprites in SOLVED order (top-left -> bottom-right). Length must be size*size.")]
        [SerializeField] private Sprite[] pieceSprites = Array.Empty<Sprite>();

        [SerializeField] private GameObject solvedLabel;

        private PuzzleGame _game;
        private Action<int> _onTilePressed;

        public void Bind(PuzzleGame game, Action<int> onTilePressed)
        {
            _game = game;
            _onTilePressed = onTilePressed;

            for (int i = 0; i < tileButtons.Length; i++)
            {
                int idx = i;
                tileButtons[i].onClick.RemoveAllListeners();
                tileButtons[i].onClick.AddListener(() => _onTilePressed?.Invoke(idx));
            }

            SetSolved(false);
        }

        public void Render(bool showEmptyPiece)
        {
            if (_game == null) return;

            int count = _game.Tiles.Count;

            for (int i = 0; i < tileButtons.Length && i < count; i++)
            {
                int val = _game.Tiles[i]; // 0 = empty

                // ----- TEXT (optional)
                if (i < tileTexts.Length && tileTexts[i] != null)
                    tileTexts[i].text = (val == 0) ? "" : val.ToString();

                // ----- BUTTON interaction (empty not clickable)
                if (tileButtons[i] != null)
                    tileButtons[i].interactable = (val != 0);

                // ----- SPRITE (IMPORTANT)
                if (i < tileImages.Length && tileImages[i] != null)
                {
                    if (val == 0)
                    {
                        if (showEmptyPiece && pieceSprites.Length == count)
                        {
                            // empty becomes the LAST piece when solved
                            tileImages[i].enabled = true;
                            tileImages[i].sprite = pieceSprites[count - 1];
                        }
                        else
                        {
                            tileImages[i].enabled = false; // hide empty during play
                        }
                    }
                    else
                    {
                        // tile identity -> sprite mapping
                        // val 1 should show sprite[0], val 2 -> sprite[1], ...
                        int spriteIndex = val - 1;

                        if (spriteIndex >= 0 && spriteIndex < pieceSprites.Length)
                        {
                            tileImages[i].enabled = true;
                            tileImages[i].sprite = pieceSprites[spriteIndex];
                        }
                        else
                        {
                            // if sprites not set correctly, at least don't break rendering
                            tileImages[i].enabled = false;
                        }
                    }
                }
            }
        }

        public void SetSolved(bool solved)
        {
            if (solvedLabel != null)
                solvedLabel.SetActive(solved);
        }
    }
}
