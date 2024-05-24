using Player;
using Player.Shooting;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Header("Player")] 
    [SerializeField] private CustomPlayerInput playerInput;
    [Space(10)]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerShooting playerShooting;

    private void Awake()
    {
        playerInput.Init();
        playerController.Init(playerInput);
        playerShooting.Init(playerInput);
    }

    private void Update()
    {
        playerInput.UpdateInput();
    }
}