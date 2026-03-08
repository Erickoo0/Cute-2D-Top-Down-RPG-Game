using System.IO;
using Unity.Cinemachine;
using UnityEngine;


public class SaveController : MonoBehaviour
{
    private string _savePath;
    private CinemachineConfiner2D _confiner;

    private void Awake()
    {
        // Cache the confiner at the start
        _confiner = GameObject.FindAnyObjectByType<CinemachineConfiner2D>();      
        _savePath = Path.Combine(Application.persistentDataPath, "Save.json");
    }

    public void SaveGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null || _confiner == null) return;
        
        SaveData saveData = new SaveData
        {
            playerPosition = player.transform.position,
            // Access the actual object assigned to the confinger
            mapBoundaryName = _confiner.BoundingShape2D != null? _confiner.BoundingShape2D.gameObject.name : ""
        };

        File.WriteAllText(_savePath, JsonUtility.ToJson(saveData));
        Debug.Log("Game Saved!");
    }

    public void LoadGame()
    {
        if (File.Exists(_savePath))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_savePath));

            // 1. Restore Player Position
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) player.transform.position = saveData.playerPosition;
            
            // 2. Restore Camera Boundary
            if (!string.IsNullOrEmpty(saveData.mapBoundaryName) && _confiner != null)
            {
                GameObject boundaryObject = GameObject.Find(saveData.mapBoundaryName);
                if (boundaryObject != null)
                {
                    var collider = boundaryObject.GetComponent<Collider2D>();
                    _confiner.BoundingShape2D = collider;
                    _confiner.InvalidateBoundingShapeCache();//Recalculates bounding shape
                }
            }
            Debug.Log("Game Loaded!");
        }
        else
        {
            SaveGame();
        }
    }
}

