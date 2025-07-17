using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PathPresetLibrary", menuName = "Path/Path Preset Library")]
public class PathPresetLibrary : ScriptableObject
{
    public List<PathPreset> presets = new();
}
