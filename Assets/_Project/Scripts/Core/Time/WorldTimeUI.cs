using System;
using TMPro;
using UnityEngine;


public class WorldTimeUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI worldTimeText;
    private WorldTimeCalendar _worldTimeCalendar;
    
    private void Awake()
    {
        EventBus.OnWorldTimeChanged += UpdateWorldTimeUI;
        _worldTimeCalendar = GetComponent<WorldTimeCalendar>();
    }

    private void OnDestroy() => EventBus.OnWorldTimeChanged -= UpdateWorldTimeUI;

    private void UpdateWorldTimeUI(object sender, TimeSpan time)
    {
        worldTimeText.text = _worldTimeCalendar.GetDate();
    }

}
