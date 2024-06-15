using Entities;
using Player.Data;
using Player.Shooting;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
        public Transform PlayerTransform => playerController.transform;
        public Vector2 PlayerVelocity => playerController.Velocity;

        [Header("Player")] 
        [SerializeField] private PlayerData data;
        [SerializeField] private CustomPlayerInput playerInput;
        [Space(10)] [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerShooting playerShooting;
        [SerializeField] private CharacterView characterView;
        [SerializeField] private PlayerDamageable playerDamageable;

        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            data.Init();
            playerInput.Init();
            playerController.Init(data, playerInput);
            playerShooting.Init(data, playerInput);
            characterView.Init(data, playerInput);
            playerDamageable.Init(data);
        }
        
        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void Update()
        {
            playerInput.UpdateInput();
        }
    }
}