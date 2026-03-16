using UnityEngine;

public class ItemUsableFood : MonoBehaviour, IUsable
{
    [SerializeField] private int healAmount = 10;
    private GameObject _player;
    private Health _health;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _health = _player.GetComponent<Health>();
    }
    public void Use()
    {
        Debug.unityLogger.Log("Chomp chomp, ate food!");
        _health.HealHeath(healAmount);
        Destroy(gameObject);
    }
}
