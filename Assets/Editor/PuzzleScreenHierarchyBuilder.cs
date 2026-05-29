using Navi.Presentation.Controllers;
using Navi.Infrastructure.DataDefinitions;
using Navi.Presentation.Navigation.Enums;
using Navi.Presentation.Navigation.Views;
using Navi.Presentation.Views.Puzzle;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Navi.Editor
{
    public static class PuzzleScreenHierarchyBuilder
    {
        private const string AppScenePath = "Assets/Scenes/App/App.unity";
        private const string FastPuzzlePath = "Assets/Puzzles/FastPuzzle4x4.asset";
        private const string DekuTreeFolder = "Assets/Puzzles/DekuTree";
        private const int PuzzleSize = 4;

        [MenuItem("Navi/Configure Deku Tree 4x4 Puzzle")]
        public static void ConfigureDekuTreePuzzle()
        {
            var puzzleDefinition = AssetDatabase.LoadAssetAtPath<PuzzleDefinitionSO>(FastPuzzlePath);
            if (puzzleDefinition == null)
            {
                Debug.LogError($"Puzzle definition was not found at {FastPuzzlePath}.");
                return;
            }

            var sprites = new Sprite[PuzzleSize * PuzzleSize];

            for (int row = 1; row <= PuzzleSize; row++)
            {
                for (int col = 1; col <= PuzzleSize; col++)
                {
                    int index = (row - 1) * PuzzleSize + (col - 1);
                    string path = $"{DekuTreeFolder}/square_tile_{row}_{col}.png";

                    if (!ConfigureSpriteImporter(path))
                        return;

                    sprites[index] = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    if (sprites[index] == null)
                    {
                        Debug.LogError($"Sprite could not be loaded from {path}.");
                        return;
                    }
                }
            }

            var serializedDefinition = new SerializedObject(puzzleDefinition);
            serializedDefinition.FindProperty("id").stringValue = "fast_4x4";
            serializedDefinition.FindProperty("size").intValue = PuzzleSize;
            serializedDefinition.FindProperty("shuffleMoves").intValue = 1;

            var pieceSprites = serializedDefinition.FindProperty("pieceSprites");
            pieceSprites.arraySize = sprites.Length;
            for (int i = 0; i < sprites.Length; i++)
                pieceSprites.GetArrayElementAtIndex(i).objectReferenceValue = sprites[i];

            serializedDefinition.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(puzzleDefinition);
            AssetDatabase.SaveAssets();

            Debug.Log("Deku Tree 4x4 puzzle configured with 16 sprites.");
        }

        [MenuItem("Navi/Build Puzzle Screen Hierarchy")]
        public static void BuildPuzzleScreen()
        {
            var scene = EditorSceneManager.OpenScene(AppScenePath);
            var screenView = FindPuzzleScreen();

            if (screenView == null)
            {
                Debug.LogError("Puzzle ScreenView was not found in App scene.");
                return;
            }

            var screen = screenView.transform;
            var screensRoot = screen.parent as RectTransform;
            if (screensRoot != null)
                Stretch(screensRoot);

            var screenRect = screen.GetComponent<RectTransform>();
            if (screenRect != null)
                Stretch(screenRect);

            var screenImage = screen.GetComponent<Image>();
            if (screenImage != null)
                screenImage.color = new Color(0.20f, 0.30f, 0.44f);

            var puzzleScreenView = screen.GetComponent<PuzzleScreenView>();
            if (puzzleScreenView == null)
                puzzleScreenView = screen.gameObject.AddComponent<PuzzleScreenView>();

            ClearChildren(screen);

            var root = CreateRectObject("PuzzleScreenRoot", screen);
            Stretch(root);
            AddImage(root, new Color(0.20f, 0.30f, 0.44f), false);

            var warmLight = CreateRectObject("WarmRoomLight", root);
            SetAnchors(warmLight, new Vector2(0f, 0.69f), Vector2.one);
            AddImage(warmLight, new Color(0.74f, 0.55f, 0.32f, 0.30f), false);

            var optionsButton = CreateButton("OptionsButton", root, "Menu", new Color(0.18f, 0.38f, 0.18f), new Color(1f, 0.90f, 0.56f), 34);
            SetAnchors(optionsButton.GetComponent<RectTransform>(), new Vector2(0.06f, 0.88f), new Vector2(0.28f, 0.94f));

            var audioButton = CreateButton("AudioButton", root, "Audio", new Color(0.18f, 0.38f, 0.18f), new Color(1f, 0.90f, 0.56f), 34);
            SetAnchors(audioButton.GetComponent<RectTransform>(), new Vector2(0.72f, 0.88f), new Vector2(0.94f, 0.94f));

            var boardArea = CreateRectObject("BoardArea", root);
            SetAnchors(boardArea, new Vector2(0.05f, 0.27f), new Vector2(0.95f, 0.78f));

            var boardFrame = CreateRectObject("BoardFrame", boardArea);
            Stretch(boardFrame);
            AddImage(boardFrame, new Color(0.46f, 0.27f, 0.10f), false);
            var aspect = boardFrame.gameObject.AddComponent<AspectRatioFitter>();
            aspect.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            aspect.aspectRatio = 1f;

            var boardInner = CreateRectObject("BoardInner", boardFrame);
            SetAnchors(boardInner, new Vector2(0.07f, 0.07f), new Vector2(0.93f, 0.93f));
            AddImage(boardInner, new Color(0.12f, 0.20f, 0.18f), false);

            var puzzleView = boardInner.gameObject.AddComponent<PuzzleView>();

            var tileButtons = new Button[PuzzleSize * PuzzleSize];
            var tileTexts = new TMP_Text[PuzzleSize * PuzzleSize];
            var tileImages = new Image[PuzzleSize * PuzzleSize];
            CreatePuzzleTiles(boardInner, tileButtons, tileTexts, tileImages);

            var solvedLabel = CreateRectObject("SolvedLabel", boardInner);
            SetAnchors(solvedLabel, new Vector2(0.12f, 0.40f), new Vector2(0.88f, 0.60f));
            AddImage(solvedLabel, new Color(0.16f, 0.38f, 0.18f, 0.88f), false);
            AddLabel(solvedLabel, "Solved", 56, new Color(1f, 0.90f, 0.56f), FontStyles.Bold);
            solvedLabel.gameObject.SetActive(false);

            var preview = CreateRectObject("ReferencePreview", root);
            SetAnchors(preview, new Vector2(0.62f, 0.13f), new Vector2(0.92f, 0.25f));
            AddImage(preview, new Color(0.13f, 0.43f, 0.43f), false);
            AddLabel(preview, "Preview", 32, Color.white, FontStyles.Bold);

            AssignPuzzleViewReferences(puzzleView, tileButtons, tileTexts, tileImages, solvedLabel.gameObject);
            AssignPuzzleScreenReferences(puzzleScreenView, optionsButton);

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();

            Debug.Log("Puzzle screen hierarchy built in App scene.");
        }

        private static bool ConfigureSpriteImporter(string path)
        {
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);

            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null)
            {
                Debug.LogError($"Texture importer was not found for {path}.");
                return false;
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.SaveAndReimport();
            return true;
        }

        private static ScreenView FindPuzzleScreen()
        {
            var screens = Object.FindObjectsByType<ScreenView>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var screen in screens)
            {
                if (screen != null && screen.Id == ScreenId.Puzzle)
                    return screen;
            }

            return null;
        }

        private static void CreatePuzzleTiles(RectTransform parent, Button[] buttons, TMP_Text[] texts, Image[] images)
        {
            const float gap = 0.008f;
            float cell = (1f - gap * (PuzzleSize - 1)) / PuzzleSize;

            for (int row = 0; row < PuzzleSize; row++)
            {
                for (int col = 0; col < PuzzleSize; col++)
                {
                    int index = row * PuzzleSize + col;
                    float xMin = col * (cell + gap);
                    float xMax = xMin + cell;
                    float yMax = 1f - row * (cell + gap);
                    float yMin = yMax - cell;

                    var button = CreateTileButton(parent, $"Tile_{row}_{col}", index + 1, out var text, out var image);
                    SetAnchors(button.GetComponent<RectTransform>(), new Vector2(xMin, yMin), new Vector2(xMax, yMax));

                    buttons[index] = button;
                    texts[index] = text;
                    images[index] = image;
                }
            }
        }

        private static Button CreateTileButton(RectTransform parent, string name, int label, out TMP_Text text, out Image pieceImage)
        {
            var rect = CreateRectObject(name, parent);
            var background = AddImage(rect, new Color(0.70f, 0.62f, 0.40f), true);

            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = background;

            var colors = button.colors;
            colors.highlightedColor = new Color(0.82f, 0.72f, 0.46f);
            colors.pressedColor = new Color(0.52f, 0.45f, 0.30f);
            button.colors = colors;

            var imageRect = CreateRectObject("PieceImage", rect);
            Stretch(imageRect);
            pieceImage = AddImage(imageRect, Color.white, false);
            pieceImage.preserveAspect = false;
            pieceImage.enabled = false;

            var labelRect = CreateRectObject("TileNumber", rect);
            Stretch(labelRect);
            text = labelRect.gameObject.AddComponent<TextMeshProUGUI>();
            text.text = label.ToString();
            text.fontSize = 42;
            text.fontStyle = FontStyles.Bold;
            text.color = new Color(0.20f, 0.14f, 0.08f);
            text.alignment = TextAlignmentOptions.Center;
            text.raycastTarget = false;
            text.enableAutoSizing = true;
            text.fontSizeMin = 18;
            text.fontSizeMax = 42;

            return button;
        }

        private static Button CreateButton(string name, Transform parent, string label, Color background, Color textColor, int fontSize)
        {
            var rect = CreateRectObject(name, parent);
            var image = AddImage(rect, background, true);

            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = image;

            var colors = button.colors;
            colors.highlightedColor = background * 1.18f;
            colors.pressedColor = background * 0.82f;
            colors.selectedColor = background;
            button.colors = colors;

            AddLabel(rect, label, fontSize, textColor, FontStyles.Bold);
            return button;
        }

        private static RectTransform CreateRectObject(string name, Transform parent)
        {
            var obj = new GameObject(name, typeof(RectTransform));
            obj.transform.SetParent(parent, false);
            return (RectTransform)obj.transform;
        }

        private static Image AddImage(RectTransform rect, Color color, bool raycastTarget)
        {
            var image = rect.gameObject.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = raycastTarget;
            return image;
        }

        private static void AddLabel(RectTransform parent, string value, int fontSize, Color color, FontStyles style)
        {
            var label = CreateRectObject("Label", parent);
            Stretch(label);

            var text = label.gameObject.AddComponent<TextMeshProUGUI>();
            text.text = value;
            text.fontSize = fontSize;
            text.fontStyle = style;
            text.color = color;
            text.alignment = TextAlignmentOptions.Center;
            text.raycastTarget = false;
            text.enableAutoSizing = true;
            text.fontSizeMin = 16;
            text.fontSizeMax = fontSize;
            text.margin = new Vector4(12f, 4f, 12f, 4f);
        }

        private static void AssignPuzzleViewReferences(PuzzleView view, Button[] buttons, TMP_Text[] texts, Image[] images, GameObject solvedLabel)
        {
            var serializedView = new SerializedObject(view);
            AssignArray(serializedView.FindProperty("tileButtons"), buttons);
            AssignArray(serializedView.FindProperty("tileTexts"), texts);
            AssignArray(serializedView.FindProperty("tileImages"), images);
            serializedView.FindProperty("solvedLabel").objectReferenceValue = solvedLabel;
            serializedView.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void AssignPuzzleScreenReferences(PuzzleScreenView view, Button optionsButton)
        {
            var serializedView = new SerializedObject(view);
            serializedView.FindProperty("optionsButton").objectReferenceValue = optionsButton;
            serializedView.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void AssignArray<T>(SerializedProperty property, T[] values) where T : Object
        {
            property.arraySize = values.Length;
            for (int i = 0; i < values.Length; i++)
                property.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
        }

        private static void ClearChildren(Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
                Object.DestroyImmediate(parent.GetChild(i).gameObject);
        }

        private static void SetAnchors(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax)
        {
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private static void Stretch(RectTransform rect)
        {
            SetAnchors(rect, Vector2.zero, Vector2.one);
        }
    }
}
