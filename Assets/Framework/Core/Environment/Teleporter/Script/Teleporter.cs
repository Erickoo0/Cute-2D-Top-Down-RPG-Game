using UnityEngine;

public enum TeleportFacing { Up, Down, Left, Right }

public class Teleporter : MonoBehaviour
{
    [Header("Teleporter Settings")] 
    [SerializeField] private Teleporter targetDestinationTeleporter;
    private string _targetDestinationTag;
    [SerializeField] private string targetDestinationTag;
    [SerializeField] private float teleportCooldown = 2f;
    [SerializeField] private TeleportFacing faceDirection;
    private float _teleportCooldownTimer;

    private void Awake()
    {
        _targetDestinationTag = targetDestinationTeleporter.targetDestinationTag;
    }
    
    private void Update()
    {
        if (_teleportCooldownTimer > 0f)
            _teleportCooldownTimer -= Time.deltaTime;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Validation checks
        if (!other.CompareTag("Player") || _teleportCooldownTimer > 0 || other.isTrigger) return;
        if (targetDestinationTeleporter == null || targetDestinationTag == null || targetDestinationTag == null || targetDestinationTag == "")
        {
            Debug.LogWarning("Teleporter: missing variables!");
            return;
        }
        
        // 2. Teleport
        other.transform.position = targetDestinationTeleporter.transform.position;
        
        // 3. Set the face direction
        var playerAnimator = other.GetComponent<EntityAnimator>(); // Replace with your actual class name
        if (playerAnimator != null)
        {
            Vector2 lookDir = targetDestinationTeleporter.GetFacingDirection();
            playerAnimator.FaceDirection(lookDir);
        }
        
        // 4. Set the cooldown for BOTH teleporters
        SetTeleporterCooldown();
        targetDestinationTeleporter.SetTeleporterCooldown();
        
        // 5. Set the location tag
        if (MapManager.Instance == null) return;
        MapManager.Instance.SetLocationTag(targetDestinationTag);
    }

    private void SetTeleporterCooldown()
    {
        _teleportCooldownTimer = teleportCooldown;
    }
    
    private Vector2 GetFacingDirection()
    {
        return faceDirection switch
        {
            TeleportFacing.Up    => Vector2.up,
            TeleportFacing.Down  => Vector2.down,
            TeleportFacing.Left  => Vector2.left,
            TeleportFacing.Right => Vector2.right,
            _                    => Vector2.zero
        };
    }
}
