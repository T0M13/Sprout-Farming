using UnityEngine;

public class CharacterCamera : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector3 offset;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (mainCamera == null) return;
        mainCamera.transform.position = transform.position + offset;
    }
}
