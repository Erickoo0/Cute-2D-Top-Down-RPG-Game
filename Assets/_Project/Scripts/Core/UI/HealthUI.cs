using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("UI References")] 
    [SerializeField] private Health healthUI;
    [SerializeField] private Image hpBarFill;
    [SerializeField] private TextMeshProUGUI hpText;
    //[SerializeField] private Text healthText;


    private void Start()
    {
        healthUI.OnHpUpdated += UpdateHealthUI;
        UpdateHealthUI(healthUI.HpCurrent);
    }

    private void OnDestroy()
    {
        healthUI.OnHpUpdated -= UpdateHealthUI;
    }

    private void UpdateHealthUI(float hpCurrent)
    {
        float hpPercent = hpCurrent / healthUI.hpMax;
        hpBarFill.fillAmount = hpPercent;
        hpText.text = ($"Hp: {hpCurrent}/{healthUI.hpMax}");
    }
}