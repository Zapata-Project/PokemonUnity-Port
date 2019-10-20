using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapCollider))]
public class MapCollisionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var mapCollider = (MapCollider)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Debug Colliders"))
        {
            mapCollider.GenerateCollisionTexture();
        }
        if (GUILayout.Button("Export Colliders Image"))
        {
            string path = EditorUtility.OpenFolderPanel("Select Target folder", "", "");
            ExportCollisionMap(path);
        }
        if (GUILayout.Button("Import Colliders"))
        {
            string path = EditorUtility.OpenFilePanel("Select Collision Map Image", "", "png");
            if (!string.IsNullOrWhiteSpace(path))
            {
                var fileContent = File.ReadAllBytes(path);
                var texture = new Texture2D(1,1);
                texture.LoadImage(fileContent);
                mapCollider.ImportCollisionMap(texture);
            }
            
        }

        void ExportCollisionMap(string path)
        {
            mapCollider.GenerateCollisionTexture();
            var imageBytes = mapCollider.collisionVisual.EncodeToPNG();
            File.WriteAllBytes($"{path}/collisionMap{Time.time}.png", imageBytes);
        }
    }
}
