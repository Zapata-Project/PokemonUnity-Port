using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TextureImportHelper : AssetPostprocessor
{
    /// <summary>
    /// Plugs in after a texture has been imported to change the default unity import settings
    /// </summary>
    /// <param name="texture"></param>
    void OnPostprocessTexture(Texture2D texture)
    {
        TextureImporter importer = assetImporter as TextureImporter;

        importer.compressionQuality = 0;
        importer.spritePixelsPerUnit = 16;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.filterMode = FilterMode.Point;
        importer.alphaIsTransparency = true;
        //GlobalVariables.global.debug($"Imported:{assetPath}");
    }
}