using Inventory;
using System;
using UnityEngine;

public class Chest : MonoBehaviour, InteractableInterface
{
    [Header("References")]
    public ChestsManager _manager;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _spriteChild;
    [SerializeField] private ChestInventoryController _inventory;
    [SerializeField] private string currentAnimaton;
    [Header("Stats")]
    public bool isOpen;

    const string OPEN_CHEST = "ChestOpen_Animation";
    const string CLOSE_CHEST = "ChestClose_Animation";

    [SerializeField] private bool isNormal = true;

    const string OPEN_CHEST_SIDE = "Chest_SideOpen_Animation";
    const string CLOSE_CHEST_SIDE = "Chest_SideClose_Animation";

    private void Awake()
    {
        _animator = _spriteChild.GetComponent<Animator>();
        _inventory = GetComponent<ChestInventoryController>();
        isOpen = false;
    }

    public void Interact()
    {
        ToggleChest();
    }

    private void ToggleChest()
    {
        if (isOpen)
        {
            CloseChest();
        }
        else
        {
            _manager.CloseAllChests();
            OpenChest();
        }

    }

    public void OpenChest()
    {
        HandleChestDirAndAnimation();
        isOpen = true;
        _inventory.inventoryUI.gameObject.SetActive(isOpen);
    }

    public void CloseChest()
    {
        HandleChestDirAndAnimation();
        isOpen = false;
        _inventory.inventoryUI.gameObject.SetActive(isOpen);
    }


    private void HandleChestDirAndAnimation()
    {
        if (isNormal)
            AnimateChest();
        else
            AnimateChestSide();
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isOpen)
        {
            CloseChest();
        }
    }

}
