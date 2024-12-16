using System;
using System.Collections;
using System.Threading;
using DG.Tweening;
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
        private static readonly int Sliding = Animator.StringToHash("Sliding");
        private static readonly int SlideJump = Animator.StringToHash("SlideJump");

        private const float MoveSpeed = 5f;
        private const float DashSpeed = 8f;
        private const float JumpPower = 5f;
        private const float SlideJumpPower = 20f;
        private const float SlideJumpPowerX = 4f;
        private const float NormalGravityScale = 0.5f;
        private const float SlideGravityScale = 1f;
        private const float JumpGravityScale = 5f;
        private const float FallingGravityScale = 7f;

        private Vector2 _movement;
        private int _nJumpCount;
        private int _nDirection;

        private bool _isJumping;
        private bool _isCanJump;
        private bool _isSliding;

        private Transform _transWallDetect;
        private Transform _transGroundDetect;

        private float VelocityX
        {
            get => rigidbodyCharacter.velocity.x;
            set => rigidbodyCharacter.velocity = new Vector2(value, VelocityY);
        }

        private float VelocityY
        {
            get => rigidbodyCharacter.velocity.y;
            set => rigidbodyCharacter.velocity = new Vector2(VelocityX, value);
        }

        private PlayerInputAssets playerInputAssets;

        private PlayerInputAssets PlayerInputAssets
        {
            get { return playerInputAssets ??= new PlayerInputAssets(); }
            set => playerInputAssets = value;
        }

        private Rigidbody2D rigidbodyCharacter;
        private Animator _animatorCharacter;

        private void Awake()
        {
            FindComponents();
        }

        private void OnEnable()
        {
            PlayerInputAssets.PlayerInputMap.Move.performed += CallBackMove;
            PlayerInputAssets.PlayerInputMap.Jump.started += CallBackJump;
            PlayerInputAssets.PlayerInputMap.Jump.performed += CallbackCancelJump;
            playerInputAssets.PlayerInputMap.Jump.canceled += CallbackCancelJump;
        }

        private void OnDisable()
        {
            playerInputAssets.PlayerInputMap.Move.performed -= CallBackMove;
            PlayerInputAssets.PlayerInputMap.Jump.started -= CallBackJump;
            PlayerInputAssets.PlayerInputMap.Jump.performed -= CallbackCancelJump;
            playerInputAssets.PlayerInputMap.Jump.canceled -= CallbackCancelJump;
            playerInputAssets.Disable();
        }

        private void FindComponents()
        {
            _transWallDetect = transform.Find("Detects/WallDetect");
            _transGroundDetect = transform.Find("Detects/GroundDetect");
            PlayerInputAssets = new PlayerInputAssets();
            PlayerInputAssets.Enable();
            rigidbodyCharacter = GetComponent<Rigidbody2D>();
            _animatorCharacter = GetComponent<Animator>();
        }

        private void CallBackMove(InputAction.CallbackContext context)
        {
            _movement = context.ReadValue<Vector2>();
            if (_movement.x == 0 && !_isSliding)
            {
                //惯性移动
                VelocityX = 0.5f * _nDirection;
            }
        }

        private void CallBackJump(InputAction.CallbackContext contextCallback)
        {
            if (_nJumpCount >= 2)
            {
                _isCanJump = false;
                return;
            }

            _nJumpCount++;
            if (_isSliding)
            {
                StartCoroutine(SlidingJump());
                return;
            }

            if (_nJumpCount == 2)
            {
                _nJumpCount++;
                _animatorCharacter.SetTrigger(DoubleJump);
                VelocityY = 0;
            }
            else if (!_isSliding)
            {
                _animatorCharacter.SetTrigger(Jump);
            }

            _animatorCharacter.SetBool(Grounded, false);
            _isCanJump = true;
        }

        private IEnumerator SlidingJump()
        {
            // _isSliding = false;
            _animatorCharacter.SetTrigger(SlideJump);
            // _animatorCharacter.SetBool(Sliding,false);
            VelocityY = 0;
            VelocityX = SlideJumpPowerX * -_nDirection;
            rigidbodyCharacter.AddForce(Vector2.up * SlideJumpPower, ForceMode2D.Impulse);
            PlayerInputAssets.PlayerInputMap.Move.Disable();
            yield return new WaitForSeconds(0.1f);
            PlayerInputAssets.PlayerInputMap.Move.Enable();
        }

        private void CallbackCancelJump(InputAction.CallbackContext context)
        {
            _isCanJump = false;
        }

        private void FixedUpdate()
        {
            UpdateMove();
            UpdateVelocity();
            UpdateDirection();
            UpdateJump();
            UpdateGravityScale();
        }

        private void UpdateMove()
        {
            if (_isSliding)
            {
                if (Mathf.Approximately(_movement.x, _nDirection))
                {
                    return;
                }
            }

            if (_movement.x == 0)
            {
                return;
            }

            VelocityX = MoveSpeed * _movement.x;
        }


        private void UpdateVelocity()
        {
            if (_isSliding)
            {
                _animatorCharacter.SetInteger(Movement, VelocityX != 0 ? 1 : 0);
            }
            else
            {
                _animatorCharacter.SetInteger(Movement, VelocityX != 0 ? 1 : 0);
            }
        }

        private void UpdateDirection()
        {
            if (VelocityX < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                _nDirection = -1;
            }

            if (VelocityX > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                _nDirection = 1;
            }
        }

        private void UpdateJump()
        {
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

            if (_isSliding)
            {
                gravityScale = SlideGravityScale;
            }
            else
            {
                gravityScale = VelocityY > 0.0f ? JumpGravityScale : FallingGravityScale;
            }

            rigidbodyCharacter.gravityScale = gravityScale;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var normal = collision.contacts[0].normal;
            if (collision.gameObject.CompareTag("Ground") && normal == Vector2.up)
            {
                _animatorCharacter.SetBool(Grounded, true);
                _isCanJump = false;
                _isJumping = false;
                _isSliding = false;
                _nJumpCount = 0;
            }

            if (normal == Vector2.left || normal == Vector2.right)
            {
                _animatorCharacter.SetBool(Sliding, true);
                VelocityY = 0;
                _isSliding = true;
                _isCanJump = false;
                _isJumping = false;
                _nJumpCount = 0;
                _movement = Vector2.zero;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (_isSliding)
            {
                _animatorCharacter.SetBool(Sliding, false);
                _isSliding = false;
            }
        }
    }
}