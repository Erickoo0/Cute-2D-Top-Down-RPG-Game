using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform _panelToMove;
    private Canvas _canvas;
    
    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        FindPanel();
    }

    private void FindPanel()
    {
        // 1. Find the UIManager in the scene/parents
        UIManager manager = GetComponentInParent<UIManager>();

        if (manager == null)
        {
            Debug.LogWarning("DraggablePanel: No UIManager found in parents! Defaulting to self.");
            _panelToMove = GetComponent<RectTransform>();
            return;
        }

        // 2. Climb up from 'this' until the parent is the UIManager
        Transform current = transform;
        while (current.parent != null && current.parent.gameObject != manager.gameObject)
        {
            current = current.parent;
        }

        // 3. 'current' is now the direct child of the UIManager (The Main Panel)
        _panelToMove = current.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _panelToMove.SetAsLastSibling(); // Bring the dragged panel to the top of the stack
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Add the mouse delta (movement) to the panel's position.
        // Dividing by the canvas scale factor ensures the panel stays exactly under the mouse,
        // regardless of whether your canvas scales with screen size.
        _panelToMove.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        return;
    }
}
