using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    private string newPresetName = "";

    public override void OnInspectorGUI()
    {
        GridManager gridManager = (GridManager)target;
        serializedObject.Update();

        // Отрисуем только нужные поля вручную
        EditorGUILayout.LabelField("Preset Settings", EditorStyles.boldLabel);
        gridManager.useCustomPreset = EditorGUILayout.Toggle("Use Custom Preset", gridManager.useCustomPreset);
        gridManager.presetLibrary = (PathPresetLibrary)EditorGUILayout.ObjectField("Preset Library", gridManager.presetLibrary, typeof(PathPresetLibrary), false);

        if (gridManager.presetLibrary != null)
        {
            List<string> presetNames = gridManager.presetLibrary.presets.Select(p => p.name).ToList();

            int newIndex = EditorGUILayout.Popup("Select Preset", gridManager.selectedPresetIndex, presetNames.ToArray());
            if (newIndex != gridManager.selectedPresetIndex)
            {
                gridManager.selectedPresetIndex = newIndex;
                gridManager.customPreset = new List<Vector2Int>(gridManager.presetLibrary.presets[newIndex].path);
                EditorUtility.SetDirty(gridManager);
            }

            if (gridManager.selectedPresetIndex >= 0 && gridManager.selectedPresetIndex < presetNames.Count)
            {
                EditorGUILayout.LabelField("Selected Preset", presetNames[gridManager.selectedPresetIndex]);
            }

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Save Current Path As Preset", EditorStyles.boldLabel);

            newPresetName = EditorGUILayout.TextField("Preset Name", newPresetName);

            if (GUILayout.Button("Save/Overwrite Preset"))
            {
                if (!string.IsNullOrWhiteSpace(newPresetName))
                {
                    if (gridManager.pathCells == null || gridManager.pathCells.Count == 0)
                    {
                        EditorUtility.DisplayDialog("Error", "Route is empty. Generate the route first before saving.", "OK");
                        return;
                    }

                    int existingIndex = gridManager.presetLibrary.presets.FindIndex(p => p.name == newPresetName);
                    if (existingIndex >= 0)
                    {
                        // Перезапись
                        gridManager.presetLibrary.presets[existingIndex].path = new List<Vector2Int>(gridManager.pathCells);
                        gridManager.selectedPresetIndex = existingIndex;
                    }
                    else
                    {
                        // Создание нового
                        var newPreset = new PathPreset { name = newPresetName, path = new List<Vector2Int>(gridManager.pathCells) };
                        gridManager.presetLibrary.presets.Add(newPreset);
                        gridManager.selectedPresetIndex = gridManager.presetLibrary.presets.Count - 1;
                    }

                    EditorUtility.SetDirty(gridManager.presetLibrary);
                    EditorUtility.SetDirty(gridManager);
                    AssetDatabase.SaveAssets();
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Please enter a valid preset name.", "OK");
                }
            }

            if (GUILayout.Button("Delete Selected Preset"))
            {
                if (gridManager.selectedPresetIndex >= 0 && gridManager.selectedPresetIndex < gridManager.presetLibrary.presets.Count)
                {
                    gridManager.presetLibrary.presets.RemoveAt(gridManager.selectedPresetIndex);
                    gridManager.selectedPresetIndex = 0;

                    if (gridManager.presetLibrary.presets.Count > 0)
                        gridManager.customPreset = new List<Vector2Int>(gridManager.presetLibrary.presets[0].path);
                    else
                        gridManager.customPreset.Clear();

                    EditorUtility.SetDirty(gridManager.presetLibrary);
                    EditorUtility.SetDirty(gridManager);
                    AssetDatabase.SaveAssets();
                }
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Assign a PresetLibrary to manage presets.", MessageType.Info);
        }

        EditorGUILayout.Space(10);

        // Остальные поля отрисуем как обычно (всё, кроме уже отрисованных)
        DrawPropertiesExcluding(serializedObject,
            "useCustomPreset",
            "presetLibrary",
            "selectedPresetIndex",
            "customPreset");

        serializedObject.ApplyModifiedProperties();
    }
}
