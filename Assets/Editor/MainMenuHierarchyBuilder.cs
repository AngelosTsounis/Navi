using Navi.Presentation.Views.MainMenu;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Navi.Editor
{
    public static class MainMenuHierarchyBuilder
    {
        private const string AppScenePath = "Assets/Scenes/App/App.unity";

        [MenuItem("Navi/Build Main Menu Hierarchy")]
        public static void BuildAppMainMenu()
        {
            var scene = EditorSceneManager.OpenScene(AppScenePath);
            var view = Object.FindFirstObjectByType<MainMenuView>(FindObjectsInactive.Include);

            if (view == null)
            {
                Debug.LogError("MainMenuView was not found in App scene.");
                return;
            }

            var screen = view.transform;
            ClearLegacyDebugObjects();

            var screensRoot = screen.parent as RectTransform;
            if (screensRoot != null)
                Stretch(screensRoot);

            var screenRect = screen.GetComponent<RectTransform>();
            if (screenRect != null)
                Stretch(screenRect);

            var screenImage = screen.GetComponent<Image>();
            if (screenImage != null)
                screenImage.color = new Color(0.18f, 0.29f, 0.18f);

            ClearChildren(screen);

            var root = CreateRectObject("MainMenuRoot", screen);
            Stretch(root);
            AddImage(root, new Color(0.18f, 0.29f, 0.18f), false);

            var topLight = CreateRectObject("TopLight", root);
            SetAnchors(topLight, new Vector2(0f, 0.72f), Vector2.one);
            AddImage(topLight, new Color(0.92f, 0.82f, 0.35f, 0.22f), false);

            var frame = CreateRectObject("MenuFrame", root);
            SetAnchors(frame, new Vector2(0.08f, 0.36f), new Vector2(0.82f, 0.74f));
            AddImage(frame, new Color(0.47f, 0.29f, 0.12f), false);

            var inner = CreateRectObject("MenuInner", frame);
            SetAnchors(inner, new Vector2(0.04f, 0.04f), new Vector2(0.96f, 0.96f));
            AddImage(inner, new Color(0.75f, 0.64f, 0.42f), false);

            var title = CreateRectObject("Title", inner);
            SetAnchors(title, new Vector2(0f, 0.83f), Vector2.one);
            AddImage(title, new Color(0.36f, 0.20f, 0.08f), false);
            AddLabel(title, "Puzzle Menu", 56, new Color(0.98f, 0.88f, 0.60f), FontStyles.Bold);

            var list = CreateRectObject("ButtonList", inner);
            SetAnchors(list, new Vector2(0.06f, 0.19f), new Vector2(0.94f, 0.81f));

            var surprisePuzzleButton = CreateMenuButton(list, "Surprise Puzzle", 0, true);
            var adventuresButton = CreateMenuButton(list, "Adventures", 1, false);
            var galleryButton = CreateMenuButton(list, "Gallery", 2, false);
            var shopButton = CreateMenuButton(list, "Shop   39", 3, false);
            var settingsButton = CreateMenuButton(list, "Settings", 4, false);

            var exitButton = CreateButton("ExitButton", inner, "Exit", new Color(0.45f, 0.24f, 0.08f), new Color(1f, 0.88f, 0.58f));
            SetAnchors(exitButton.GetComponent<RectTransform>(), new Vector2(0.28f, 0.04f), new Vector2(0.72f, 0.16f));

            var resetIntroButton = CreateButton("ResetIntroButton", root, "Reset Intro", new Color(0.20f, 0.20f, 0.20f, 0.75f), new Color(1f, 0.88f, 0.58f));
            SetAnchors(resetIntroButton.GetComponent<RectTransform>(), new Vector2(0.06f, 0.08f), new Vector2(0.36f, 0.13f));

            var portrait = CreateRectObject("PortraitPlaceholder", root);
            SetAnchors(portrait, new Vector2(0.67f, 0.37f), new Vector2(0.94f, 0.55f));
            AddImage(portrait, new Color(0.13f, 0.44f, 0.42f), false);
            AddLabel(portrait, "Navi", 36, Color.white, FontStyles.Bold);

            CreateCurrencyRow(root);
            AssignButtonReferences(view, surprisePuzzleButton, adventuresButton, galleryButton, shopButton, settingsButton, exitButton, resetIntroButton);

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();

            Debug.Log("Main menu hierarchy built in App scene.");
        }

        private static void ClearLegacyDebugObjects()
        {
            var transforms = Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var transform in transforms)
            {
                if (transform.name == "DevDebugPanel" || transform.name == "IntroResetButton")
                    Object.DestroyImmediate(transform.gameObject);
            }
        }

        private static void ClearChildren(Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
                Object.DestroyImmediate(parent.GetChild(i).gameObject);
        }

        private static Button CreateMenuButton(Transform parent, string label, int row, bool highlighted)
        {
            const float rowCount = 5f;
            const float gap = 0.025f;
            float rowHeight = (1f - gap * (rowCount - 1f)) / rowCount;
            float yMax = 1f - row * (rowHeight + gap);
            float yMin = yMax - rowHeight;

            var background = highlighted
                ? new Color(0.33f, 0.52f, 0.14f)
                : new Color(0.80f, 0.70f, 0.49f);
            var text = highlighted
                ? new Color(1f, 0.93f, 0.67f)
                : new Color(0.25f, 0.18f, 0.10f);

            var button = CreateButton(label.Replace(" ", string.Empty) + "Button", parent, label, background, text);
            SetAnchors(button.GetComponent<RectTransform>(), new Vector2(0f, yMin), new Vector2(1f, yMax));
            return button;
        }

        private static Button CreateButton(string name, Transform parent, string label, Color background, Color text)
        {
            var rect = CreateRectObject(name, parent);
            var image = AddImage(rect, background, true);

            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = image;

            var colors = button.colors;
            colors.highlightedColor = background * 1.18f;
            colors.pressedColor = background * 0.82f;
            colors.selectedColor = background;
            colors.disabledColor = new Color(background.r, background.g, background.b, 0.45f);
            button.colors = colors;

            AddLabel(rect, label, 42, text, FontStyles.Bold);
            return button;
        }

        private static void CreateCurrencyRow(Transform root)
        {
            var row = CreateRectObject("CurrencyRow", root);
            SetAnchors(row, new Vector2(0.54f, 0.25f), new Vector2(0.94f, 0.30f));
            AddImage(row, new Color(0.18f, 0.14f, 0.08f, 0.65f), false);
            AddLabel(row, "100    25    10", 34, new Color(1f, 0.92f, 0.65f), FontStyles.Bold);
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
            text.fontSizeMin = 20;
            text.fontSizeMax = fontSize;
            text.margin = new Vector4(20f, 6f, 20f, 6f);
        }

        private static void AssignButtonReferences(
            MainMenuView view,
            Button surprisePuzzleButton,
            Button adventuresButton,
            Button galleryButton,
            Button shopButton,
            Button settingsButton,
            Button exitButton,
            Button resetIntroButton)
        {
            var serializedView = new SerializedObject(view);
            serializedView.FindProperty("surprisePuzzleButton").objectReferenceValue = surprisePuzzleButton;
            serializedView.FindProperty("adventuresButton").objectReferenceValue = adventuresButton;
            serializedView.FindProperty("galleryButton").objectReferenceValue = galleryButton;
            serializedView.FindProperty("shopButton").objectReferenceValue = shopButton;
            serializedView.FindProperty("settingsButton").objectReferenceValue = settingsButton;
            serializedView.FindProperty("exitButton").objectReferenceValue = exitButton;
            serializedView.FindProperty("resetIntroButton").objectReferenceValue = resetIntroButton;
            serializedView.ApplyModifiedPropertiesWithoutUndo();
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
