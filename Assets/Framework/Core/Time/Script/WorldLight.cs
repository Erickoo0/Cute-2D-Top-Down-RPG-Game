using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

[RequireComponent(typeof(Light2D))]
public class WorldLight : MonoBehaviour
{
    [SerializeField] private WorldTime worldTime;
    [SerializeField] private Gradient gradient;
    
    private Light2D _light2D;

    private void Awake()
    {
        EventBus.OnWorldTimeChanged += UpdateWorldLight;
        _light2D = GetComponent<Light2D>();
    }

    private void OnDestroy() => EventBus.OnWorldTimeChanged -= UpdateWorldLight;
    
    private void UpdateWorldLight(object sender, TimeSpan time)
    {
        _light2D.color = gradient.Evaluate(PercentOfDay(time));
    }

    /// <summary>
    /// Gets the time in THIS day taking into account total time (days / months passed)
    /// </summary>
    private float PercentOfDay(TimeSpan time)
    {
        return (float)time.TotalMinutes % WorldTimeConstants.MinutesInDay / WorldTimeConstants.MinutesInDay;
    }
}
