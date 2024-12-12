using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HollowKnight.Control
{
    public class CharacterController : MonoBehaviour
    {
        private const float MoveSpeed = 4f;
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

        private Rigidbody2D _rigidBody2DCharacter;
        private Animator _animatorCharacter;
        
        private void Awake()
        {
            FindComponents();
        }

        private void OnEnable()
        {
            PlayerInputAssets.PlayerInputMap.Move.performed += context => _movement = context.ReadValue<Vector2>();
            // PlayerInputAssets.PlayerInputMap.Move.canceled += context => _movement = new Vector2(0, _movement.y);
            PlayerInputAssets.PlayerInputMap.Jump.started += Jump;
            PlayerInputAssets.PlayerInputMap.Jump.performed += CancelJump;
            playerInputAssets.PlayerInputMap.Jump.canceled += CancelJump;
        }

        private void OnDisable()
        {
            PlayerInputAssets.PlayerInputMap.Jump.started -= Jump;
            PlayerInputAssets.PlayerInputMap.Jump.performed -= CancelJump;
            playerInputAssets.PlayerInputMap.Jump.canceled -= CancelJump;
            playerInputAssets.Disable();
        }
        
        private void FindComponents()
        {
            PlayerInputAssets = new PlayerInputAssets();
            PlayerInputAssets.Enable();
            _rigidBody2DCharacter = GetComponent<Rigidbody2D>();
            _animatorCharacter = GetComponent<Animator>();
        }
        

        private void Jump(InputAction.CallbackContext contextCallback)
        {
            if (_nJumpCount >= 2)
            {
                return;
            }
            _nJumpCount++;
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
                return;
            }
            _rigidBody2DCharacter.velocity = new Vector2(MoveSpeed * _movement.x, _rigidBody2DCharacter.velocity.y);
        }
        
        private void UpdateJump()
        {
            if (!_isCanJump)
            {
                return;
            }
            _isJumping = true;
            
            if (_nJumpCount == 2)
            {
                _nJumpCount++;
                _rigidBody2DCharacter.velocity = new Vector2(_rigidBody2DCharacter.velocity.x, 0);
            }
            _rigidBody2DCharacter.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
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
                    gravityScale = _rigidBody2DCharacter.velocity.y > 0.0f ? JumpGravityScale : FallingGravityScale;
                }
            }
            
            _rigidBody2DCharacter.gravityScale = gravityScale;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            _isCanJump = false;
            _isJumping = false;
            _nJumpCount = 0;
        }
    }
}