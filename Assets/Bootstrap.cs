using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private PlayerInput _playerInput;
    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void Update()
    {
        _playerInput.UpdateInput();
    }
}