using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathPreset
{
    public string name;
    public List<Vector2Int> path = new();
}
