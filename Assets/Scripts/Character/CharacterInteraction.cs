using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float interactionRadius;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private InteractableInterface interactable;
    [SerializeField] private LayerMask interactionMask;
    [Header("Input Settings")]
    [SerializeField] private InputActionReference _playerInteraction;
    [Header("Gizmos")]
    [SerializeField] private bool showGizmos;


    #region Input
    private void OnEnable()
    {
        _playerInteraction.action.Enable();
    }

    private void OnDisable()
    {
        _playerInteraction.action.Disable();
    }
    #endregion

    private void Update()
    {
        _collider = Physics2D.OverlapCircle(transform.position, interactionRadius, interactionMask);
        if (_collider == null) return;
        if (_collider.GetComponent<InteractableInterface>() != null && !_collider.isTrigger)
        {
            interactable = _collider.GetComponent<InteractableInterface>();
            if (_playerInteraction.action.triggered)
                Interact();
        }
        else
        {
            return;
        }
    }

    private void Interact()
    {
        interactable.Interact();
    }

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
