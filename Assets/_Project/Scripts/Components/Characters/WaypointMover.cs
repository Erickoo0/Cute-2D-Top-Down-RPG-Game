using System.Collections;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    public Transform waypointParent;
    public float moveSpeed = 2f;
    public float waitTime = 2f;
    public float maxStartDelay = 4f;
    public bool loopWaypoints = true;

    private EntityAnimationController _entityAnimation;
    private Vector2[] waypoints;
    private int currentWaypointIndex;
    private bool isWaiting;

    private void Start()
    {
        // 1. Get the Animator 
        _entityAnimation = GetComponent<EntityAnimationController>();
        
        // 2. Get the waypoint parent
        if (waypointParent == null) waypointParent = transform.Find("Waypoint Parent");

        if (waypointParent != null)
        {
            SetupWaypoints();
            StartCoroutine(RandomStartDelay());
        }

    }
    
    private void SetupWaypoints()
    {
        waypoints = new Vector2[waypointParent.childCount];
        for (int i = 0; i < waypointParent.childCount; i++) 
        {
            waypoints[i] = waypointParent.GetChild(i).position;
        }
    }
    
    private void Update()
    {
        if (PauseManager.IsGamePaused || isWaiting || waypoints == null)
        {
            _entityAnimation?.StopAnimation();            
            return;
        }
        MoveToWaypoint();
    }
    
    private void MoveToWaypoint()
    {
        Vector2 target = waypoints[currentWaypointIndex]; // Get the current waypoint position
        Vector2 currentPosition = transform.position;
        float distance = Vector2.Distance(currentPosition, target);
        
        if (distance > 0.1f)
        {
            // 1. Calculate direction
            Vector2 moveDirection = (target - currentPosition).normalized;
            
            // 2. Tell the animation controller to update
            _entityAnimation?.UpdateAnimation(moveDirection);
            
            // 3. Move
            transform.position = Vector2.MoveTowards(currentPosition, target, moveSpeed * Time.deltaTime);
        }
        else
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    IEnumerator WaitAtWaypoint()
    {
        // Idle at waypoint for waitTIme
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        
        // Move to next waypoint
        // Repeat if LoopWaypoints is true
        currentWaypointIndex = loopWaypoints? (currentWaypointIndex + 1) % waypoints.Length : currentWaypointIndex;
        isWaiting = false;
    }

    IEnumerator RandomStartDelay()
    {
        isWaiting = true;
        float randomDelay = Random.Range(0.2f, maxStartDelay);
        yield return new WaitForSeconds(randomDelay);
        isWaiting = false;
    }
}
