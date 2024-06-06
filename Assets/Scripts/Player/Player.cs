using Player.Shooting;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
        public Transform PlayerTransform => playerController.transform;

        [Header("Player")] 
        [SerializeField] private CustomPlayerInput playerInput;
        [Space(10)]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerShooting playerShooting;
        [SerializeField] private CharacterView characterView;

        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            
            playerInput.Init();
            playerController.Init(playerInput);
            playerShooting.Init(playerInput);
            characterView.Init(playerInput);
        }

        private void Update()
        {
            playerInput.UpdateInput();
        }
    }
}