using System.IO;
using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// Handles saving and loading player data and camera boundaries using JSON
/// </summary>
public class SaveController : MonoBehaviour
{
    private string _savePath;
    private CinemachineConfiner2D _confiner;

    private void Awake()
    {
        // 1. Find the Cinemachine Confiner 2D in the scene
        _confiner = GameObject.FindAnyObjectByType<CinemachineConfiner2D>();   
        
        // 2. Define where the save file lives "persistentDataPath" is a special unity folder
        // that works across Windows, Mac, Andriod, and IOS
        _savePath = Path.Combine(Application.persistentDataPath, "Save.json");
        
        // 3. Load the game at start
        LoadGame();
    }

    public void SaveGame()
    {
        // 1. Locate the player object in the scene using its tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Exit early if no player or confiner
        if (player == null || _confiner == null) return;
        
        // 2. Create a new instance of our data container and fill it with current info
        SaveData saveData = new SaveData
        {
            playerPosition = player.transform.position,
            
            // If the camera has a boundary assigned, save its name so we can find it again
            mapBoundaryName = _confiner.BoundingShape2D != null? _confiner.BoundingShape2D.gameObject.name : ""
        };

        // 3. Convert the SaveData object into a JSON string (text)
        string json = JsonUtility.ToJson(saveData);
        
        // 4. Write that text to the hard drive file location _savePath
        File.WriteAllText(_savePath, json);
        
        Debug.Log("Game Saved to: {_savePath}");
    }

    public void LoadGame()
    {
        // 1. Check if the file actually exists before trying to read it
        if (File.Exists(_savePath))
        {
            // 2. Read the text from the file and convert it back into a SaveData object
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_savePath));

            // 3. Restore the Player position
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) player.transform.position = saveData.playerPosition;
            
            // 4. Restore the camera boundary
            if (!string.IsNullOrEmpty(saveData.mapBoundaryName) && _confiner != null)
            {
                // Find the GameObject in scene that matches the mapBoundaryName
                GameObject boundaryObject = GameObject.Find(saveData.mapBoundaryName);
                if (boundaryObject != null)
                {
                    // Get the collider from the GameObject
                    var collider = boundaryObject.GetComponent<Collider2D>();
                    _confiner.BoundingShape2D = collider;
                    _confiner.InvalidateBoundingShapeCache();//Recalculates bounding shape
                }
            }
            Debug.Log("Game Loaded!");
        }
        else
        {
            // If no save file exists, create one immediately
            SaveGame();
        }
    }
}

