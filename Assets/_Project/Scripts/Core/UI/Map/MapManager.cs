using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MapManager : MonoBehaviour, ISaveable
{
    public static MapManager Instance { get; private set; }

    [SerializeField] private GameObject mapMenuPanel;
    
    public GameObject map;
    public Color highlightColor = Color.yellow;
    public Color defaultColor = new Color(1f, 1f, 1f, 0.5f);
    public RectTransform playerIconTransform;
    
    private List<Image> _mapTiles;
    private string _currentTileName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.unityLogger.Log("Multiple MapManagers detected. Destroying one.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        // Get all the map tiles
        _mapTiles = map.GetComponentsInChildren<Image>().ToList();
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!mapMenuPanel.activeSelf)EventBus.RequestOpenMenu(mapMenuPanel);
        else if (mapMenuPanel.activeSelf) EventBus.RequestCloseMenu(mapMenuPanel);
    }

    
    public void HighlightTile(string currentTileName)
    {
        _currentTileName = currentTileName;
        
        foreach (Image tile in _mapTiles)
        {
            tile.color = defaultColor;
        }

        Image currentTile = _mapTiles.Find(x => x.name == _currentTileName);

        if (currentTile != null)
        {
            currentTile.color = highlightColor;
            playerIconTransform.position = currentTile.GetComponent<RectTransform>().position;
        }
        else
        {
            Debug.unityLogger.Log("Map tile not found");
        }
    }

    public void PopulateSaveData(SaveData saveData)
    {
        // Save the current tile name to the saveData
        saveData.mapBoundaryName = _currentTileName;
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        if (!string.IsNullOrEmpty(saveData.mapBoundaryName))
        {
            HighlightTile(saveData.mapBoundaryName);
        }
    }
}
