using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HollowKnight.Control
{
    public class CharacterController : MonoBehaviour
    {
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int Movement = Animator.StringToHash("Movement");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int DoubleJump = Animator.StringToHash("DoubleJump");

        private const float MoveSpeed = 5f;
        private const float DashSpeed = 8f;
        private const float JumpPower = 5f;
        private const float NormalGravityScale = 0.5f;
        private const float SlideGravityScale = 1f;
        private const float JumpGravityScale = 5f;
        private const float FallingGravityScale = 7f;
        
        private Vector2 _movement;
        private int _nJumpCount;
        
        private bool _isJumping;
        private bool _isCanJump;
        private bool _isSliding;

        private PlayerInputAssets playerInputAssets;
        private PlayerInputAssets PlayerInputAssets
        {
            get { return playerInputAssets ??= new PlayerInputAssets(); }
            set => playerInputAssets = value;
        }

        private Rigidbody2D rigidbodyCharacter;
        private Animator animatorCharacter;
        
        private void Awake()
        {
            FindComponents();
        }

        private void OnEnable()
        {
            PlayerInputAssets.PlayerInputMap.Move.performed += CallBackMove;
            PlayerInputAssets.PlayerInputMap.Jump.started += CallBackJump;
            PlayerInputAssets.PlayerInputMap.Jump.performed += CancelJump;
            playerInputAssets.PlayerInputMap.Jump.canceled += CancelJump;
        }

        private void OnDisable()
        {
            playerInputAssets.PlayerInputMap.Move.performed -= CallBackMove;
            PlayerInputAssets.PlayerInputMap.Jump.started -= CallBackJump;
            PlayerInputAssets.PlayerInputMap.Jump.performed -= CancelJump;
            playerInputAssets.PlayerInputMap.Jump.canceled -= CancelJump;
            playerInputAssets.Disable();
        }
        
        private void FindComponents()
        {
            PlayerInputAssets = new PlayerInputAssets();
            PlayerInputAssets.Enable();
            rigidbodyCharacter = GetComponent<Rigidbody2D>();
            animatorCharacter = GetComponent<Animator>();
        }
        
        private void CallBackMove(InputAction.CallbackContext context)
        {
            _movement = context.ReadValue<Vector2>();
            if (_movement.x == 0)
            {
                rigidbodyCharacter.velocity = new Vector2(0.4f, rigidbodyCharacter.velocity.y);
            }
        }
        
        private void CallBackJump(InputAction.CallbackContext contextCallback)
        {
            if (_nJumpCount >= 2)
            {
                return;
            }
            _nJumpCount++;
            if (_nJumpCount == 2)
            {
                _nJumpCount++;
                animatorCharacter.SetTrigger(DoubleJump);
                rigidbodyCharacter.velocity = new Vector2(rigidbodyCharacter.velocity.x, 0);
            }
            else
            {
                animatorCharacter.SetTrigger(Jump);
            }

            animatorCharacter.SetBool(Grounded, false);
            _isCanJump = true;
        }

        private void CancelJump(InputAction.CallbackContext context)
        {
            _isCanJump = false;
        }

        private void FixedUpdate()
        {
            UpdateMove();
            UpdateJump();
            UpdateGravityScale();
        }

        private void Update()
        {

        }
        
        private void UpdateMove()
        {
            if (_movement.x == 0)
            {
                animatorCharacter.SetInteger(Movement, 0);
                return;
            }
            animatorCharacter.SetInteger(Movement,1);
            rigidbodyCharacter.velocity = new Vector2(MoveSpeed * _movement.x, rigidbodyCharacter.velocity.y);
        }
        
        private void UpdateJump()
        {
            Debug.Log("y轴方向的速度" + rigidbodyCharacter.velocity.y);
            if (!_isCanJump)
            {
                return;
            }
            _isJumping = true;
            
            rigidbodyCharacter.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
        }

        private void UpdateGravityScale()
        {
            var gravityScale = NormalGravityScale;

            if (_isJumping)
            {
                if (_isSliding && _movement.x != 0)
                {
                    gravityScale = SlideGravityScale;
                }
                else
                {
                    gravityScale = rigidbodyCharacter.velocity.y > 0.0f ? JumpGravityScale : FallingGravityScale;
                }
            }
            
            rigidbodyCharacter.gravityScale = gravityScale;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                animatorCharacter.SetBool(Grounded, true);
                _isCanJump = false;
                _isJumping = false;
                _nJumpCount = 0;
            }
        }
    }
}