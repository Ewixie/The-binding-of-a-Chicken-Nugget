using UnityEngine;

namespace Player
{
   public class CharacterView : MonoBehaviour
   {
      [SerializeField] private bool facingLeft;
      [SerializeField] private Animator animator;
      private Vector3 _baseScale;
      private CustomPlayerInput _playerInput;
      
      
      #region Animator Bools

      private static readonly int GoingSide = Animator.StringToHash("GoingSide");
      private static readonly int GoingSideBackwards= Animator.StringToHash("GoingSideBackwards");
      private static readonly int GoingUp = Animator.StringToHash("GoingUp");
      private static readonly int GoingDown= Animator.StringToHash("GoingDown");

      #endregion
      
      
      private bool _isInitialized;

      public void Init(CustomPlayerInput input)
      {
         if (_isInitialized) return;
         _playerInput = input;
         _baseScale = transform.localScale;
         _isInitialized = true;
      }

      private void Update()
      {
         animator.SetBool(GoingSide, _playerInput.RawMovement.x != 0);
         animator.SetBool(GoingUp, _playerInput.RawMovement.y > 0);
         animator.SetBool(GoingDown, _playerInput.RawMovement.y < 0);
         
         if (_playerInput.IsShooting)
         {
            animator.SetBool(GoingSideBackwards, Mathf.Sign(_playerInput.Movement.x) != Mathf.Sign(_playerInput.ShootingDirection.x));
         }
         
         if (_playerInput.RawMovement.x != 0) UpdateFlip();

         CheckIdle();

      }

      private void CheckIdle()
      {
         if (_playerInput.RawMovement == Vector2.zero)
         {
            animator.SetBool(GoingSide, false);
            animator.SetBool(GoingUp, false);
            animator.SetBool(GoingDown, false);
            animator.SetBool(GoingSideBackwards, false);
         }
      }

      private void UpdateFlip()
      {
         float sign = Mathf.Sign(_playerInput.Movement.x);
         bool backwards;
         backwards = _playerInput.IsShooting
            ? Mathf.Sign(_playerInput.Movement.x) != Mathf.Sign(_playerInput.ShootingDirection.x)
            : false;
         float directionX = (backwards ? -1 : 1) * sign * (facingLeft ? 1 : -1);
         transform.localScale = new Vector3(_baseScale.x * directionX, _baseScale.y, _baseScale.z);
      }
      
   }
}
