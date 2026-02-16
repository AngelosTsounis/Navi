using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Navi.Infrastructure.DataDefinitions;

[CustomEditor(typeof(PuzzleDefinitionSO))]
public sealed class PuzzleDefinitionSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Auto Fill Piece Sprites (from same folder)"))
        {
            AutoFill((PuzzleDefinitionSO)target);
        }
    }

    private static void AutoFill(PuzzleDefinitionSO def)
    {
        if (def == null) return;

        string path = AssetDatabase.GetAssetPath(def);
        string folder = System.IO.Path.GetDirectoryName(path);

        var guids = AssetDatabase.FindAssets("t:Sprite", new[] { folder });
        var sprites = guids
            .Select(g => AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(g)))
            .Where(s => s != null)
            .OrderBy(s => GetRow(s.name))
            .ThenBy(s => GetCol(s.name))
            .ToArray();

        int expected = def.Size * def.Size;
        if (sprites.Length < expected)
        {
            Debug.LogError($"Not enough sprites in folder '{folder}'. Found {sprites.Length}, expected {expected}.");
            return;
        }

        // take exactly expected count
        sprites = sprites.Take(expected).ToArray();

        // Write into serialized field "pieceSprites"
        var so = new SerializedObject(def);
        var prop = so.FindProperty("pieceSprites");

        prop.arraySize = expected;
        for (int i = 0; i < expected; i++)
            prop.GetArrayElementAtIndex(i).objectReferenceValue = sprites[i];

        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(def);

        Debug.Log($"Auto-filled {expected} sprites for '{def.name}' from folder '{folder}'.");
    }

    // Extract row/col from names like: tile_2_1, tile_2_1_0 etc.
    private static int GetRow(string name)
    {
        var m = Regex.Match(name, @"(\d+)[^\d]+(\d+)");
        return m.Success ? int.Parse(m.Groups[1].Value) : int.MaxValue;
    }

    private static int GetCol(string name)
    {
        var m = Regex.Match(name, @"(\d+)[^\d]+(\d+)");
        return m.Success ? int.Parse(m.Groups[2].Value) : int.MaxValue;
    }
}
