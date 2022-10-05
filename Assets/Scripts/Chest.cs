using UnityEngine;

public class Chest : MonoBehaviour, InteractableInterface
{
    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _spriteChild;
    [SerializeField] private string currentAnimaton;
    [Header("Stats")]
    [SerializeField] private bool isOpen;

    const string OPEN_CHEST = "ChestOpen_Animation";
    const string CLOSE_CHEST = "ChestClose_Animation";

    [SerializeField] private bool isNormal = true;

    const string OPEN_CHEST_SIDE = "Chest_SideOpen_Animation";
    const string CLOSE_CHEST_SIDE = "Chest_SideClose_Animation";

    private void Awake()
    {
        _animator = _spriteChild.GetComponent<Animator>();
    }

    public void Interact()
    {
        ToggleChest();
    }

    private void ToggleChest()
    {
        if (isNormal)
            AnimateChest();
        else
            AnimateChestSide();
        isOpen = !isOpen;

    }

    private void AnimateChest()
    {
        if (isOpen)
        {
            ChangeAnimationState(CLOSE_CHEST);
        }
        else
        {
            ChangeAnimationState(OPEN_CHEST);
        }
    }

    private void AnimateChestSide()
    {
        if (isOpen)
        {
            ChangeAnimationState(CLOSE_CHEST_SIDE);
        }
        else
        {
            ChangeAnimationState(OPEN_CHEST_SIDE);
        }
    }

    private void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        _animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }


}
