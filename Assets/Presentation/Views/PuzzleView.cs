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
        [SerializeField] private TMP_Text[] tileTexts = Array.Empty<TMP_Text>();
        [SerializeField] private Image[] tileImages = Array.Empty<Image>();

        [SerializeField] private GameObject solvedLabel;

        private PuzzleGame _game;
        private Action<int> _onTilePressed;

        // Sprites in SOLVED order (top-left -> bottom-right). Provided by controller.
        private Sprite[] _sprites = Array.Empty<Sprite>();

        public void SetSprites(Sprite[] sprites)
        {
            _sprites = sprites ?? Array.Empty<Sprite>();
        }

        public void Bind(PuzzleGame game, Action<int> onTilePressed)
        {
            _game = game;
            _onTilePressed = onTilePressed;

            for (int i = 0; i < tileButtons.Length; i++)
            {
                int idx = i;
                var btn = tileButtons[i];
                if (btn == null) continue;

                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => _onTilePressed?.Invoke(idx));
            }

            SetSolved(false);
        }

        public void Render(bool showEmptyPiece)
        {
            if (_game == null) return;

            int tileCount = _game.Tiles.Count;
            int renderCount = Math.Min(tileButtons.Length, tileCount);

            bool hasSpritesForAllTiles = _sprites.Length == tileCount;

            for (int i = 0; i < renderCount; i++)
            {
                int val = _game.Tiles[i]; // 0 = empty

                // TEXT (optional)
                if (i < tileTexts.Length && tileTexts[i] != null)
                    tileTexts[i].text = val == 0 ? string.Empty : val.ToString();

                // BUTTON interaction (empty not clickable)
                if (i < tileButtons.Length && tileButtons[i] != null)
                    tileButtons[i].interactable = val != 0;

                // SPRITE
                if (i < tileImages.Length && tileImages[i] != null)
                    ApplySprite(tileImages[i], val, tileCount, showEmptyPiece, hasSpritesForAllTiles);
            }
        }

        private void ApplySprite(Image img, int tileValue, int tileCount, bool showEmptyPiece, bool hasSpritesForAllTiles)
        {
            // If sprites aren't configured, hide images safely.
            if (_sprites.Length == 0)
            {
                img.enabled = false;
                return;
            }

            // Empty tile (0)
            if (tileValue == 0)
            {
                if (showEmptyPiece && hasSpritesForAllTiles)
                {
                    img.enabled = true;
                    img.sprite = _sprites[tileCount - 1]; // last piece shown on solve (matches your current rule)
                }
                else
                {
                    img.enabled = false; // hidden during play
                }
                return;
            }

            // Normal tile: val 1 -> sprite[0], val 2 -> sprite[1], ...
            int spriteIndex = tileValue - 1;
            if ((uint)spriteIndex < (uint)_sprites.Length)
            {
                img.enabled = true;
                img.sprite = _sprites[spriteIndex];
            }
            else
            {
                img.enabled = false;
            }
        }

        public void SetSolved(bool solved)
        {
            if (solvedLabel != null)
                solvedLabel.SetActive(solved);
        }
    }
}
