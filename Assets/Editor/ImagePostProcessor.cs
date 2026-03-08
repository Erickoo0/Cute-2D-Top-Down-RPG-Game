using UnityEditor;
using UnityEngine;

public class ImagePostProcessor : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        TextureImporter importer = (TextureImporter)assetImporter;

        // Change these to match your specific project needs
        importer.textureType = TextureImporterType.Sprite;
        importer.spritePixelsPerUnit = 16; // Change this to your project's PPU
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        
        // This ensures the "Alpha is Transparency" is checked
        importer.alphaIsTransparency = true;
    }
}