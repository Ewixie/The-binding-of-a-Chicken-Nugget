using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerController playerController;
    
    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        
        playerController.Init(_playerInput);
    }

    private void Update()
    {
        _playerInput.UpdateInput();
    }
}