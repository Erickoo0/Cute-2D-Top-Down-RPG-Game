using Unity;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    //[SerializeField] private Text healthText;
    private Health Health;

    private void Start()
    {
        Health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        Health.OnHealthChanged += UpdateHealthUI;
        UpdateHealthUI(Health.HealthCurrent);
    }

    private void OnDestroy()
    {
        Health.OnHealthChanged -= UpdateHealthUI;
    }

    private void UpdateHealthUI(int currentHealth)
    {
        float healthPercent = (float)currentHealth / Health.healthMax;
        healthBarFill.fillAmount = healthPercent;
    }

}