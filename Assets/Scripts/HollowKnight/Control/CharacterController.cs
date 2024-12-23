using System;
using System.Collections;
using System.Threading;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace HollowKnight.Control
{
    public class CharacterController : MonoBehaviour
    {
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int Movement = Animator.StringToHash("Movement");
        private static readonly int SpeedY = Animator.StringToHash("SpeedY");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int DoubleJump = Animator.StringToHash("DoubleJump");
        private static readonly int Sliding = Animator.StringToHash("Sliding");
        private static readonly int SlideJump = Animator.StringToHash("SlideJump");
        
        //移动参数
        private const float MoveSpeed = 5f;
        private const float DashSpeed = 8f;
        private const float JumpPower = 5f;
        private const float SlideJumpSpeedY = 5f;
        private const float SlideJumpSpeedX = 5f;
        private const float NormalGravityScale = 0.5f;
        private const float SlideGravityScale = 1f;
        private const float JumpGravityScale = 5f;
        private const float FallingGravityScale = 7f;

        //战斗参数
        private int _nSlashType;
        private float lastSlashTime;
        private const float SlashInterval = 0.2f;
        
        private Vector2 _movement;
        private int _nJumpCount;
        private int _nDirection;
        
        private bool _isGrounded;
        private bool _isCanJump;
        private bool _isJumping;
        private bool _isSliding;
        private bool _isSlideJumping;
        
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
            lastSlashTime = Time.time;
        }

        private void OnEnable()
        {
            PlayerInputAssets.PlayerInputMap.Move.performed += CallBackMove;
            PlayerInputAssets.PlayerInputMap.Jump.started += CallBackJump;
            PlayerInputAssets.PlayerInputMap.Jump.performed += CallbackCancelJump;
            PlayerInputAssets.PlayerInputMap.Jump.canceled += CallbackCancelJump;
            PlayerInputAssets.PlayerInputMap.Attack.started += CallbackAttack;
            PlayerInputAssets.Enable();
        }

        private void OnDisable()
        {
            PlayerInputAssets.PlayerInputMap.Move.performed -= CallBackMove;
            PlayerInputAssets.PlayerInputMap.Jump.started -= CallBackJump;
            PlayerInputAssets.PlayerInputMap.Jump.performed -= CallbackCancelJump;
            PlayerInputAssets.PlayerInputMap.Jump.canceled -= CallbackCancelJump;
            PlayerInputAssets.PlayerInputMap.Attack.started -= CallbackAttack;
            PlayerInputAssets.Disable();
        }

        private void FindComponents()
        {
            _transWallDetect = transform.Find("Detects/WallDetect");
            _transGroundDetect = transform.Find("Detects/GroundDetect");
            PlayerInputAssets = new PlayerInputAssets();
            rigidbodyCharacter = GetComponent<Rigidbody2D>();
            _animatorCharacter = GetComponent<Animator>();
        }

        private void CallBackMove(InputAction.CallbackContext context)
        {
            _movement = context.ReadValue<Vector2>();
        }

        private void CallBackJump(InputAction.CallbackContext contextCallback)
        {
            if (_nJumpCount >= 2)
            {
                _isCanJump = false;
                return;
            }

            _nJumpCount++;
            
            if (_nJumpCount == 2)
            {
                _animatorCharacter.SetTrigger(DoubleJump);
                VelocityY = 0;
            }
            else if (!_isSliding)
            {
                _animatorCharacter.SetTrigger(Jump);
            }

            _isGrounded = false;
            _animatorCharacter.SetBool(Grounded, false);
            _isCanJump = true;
        }

        private IEnumerator SlidingJump()
        {
            _isSlideJumping = true;
            _animatorCharacter.SetTrigger(SlideJump);
            _animatorCharacter.SetBool(Sliding,false);
            VelocityY = 0;
            VelocityX = SlideJumpSpeedX * -_nDirection;
            yield return new WaitForSeconds(0.17f);
            yield return new WaitForFixedUpdate();
            _isSlideJumping = false;
        }

        private void CallbackCancelJump(InputAction.CallbackContext context)
        {
            _isCanJump = false;
        }

        private void CallbackAttack(InputAction.CallbackContext context)
        {
            if (_isCanJump || Time.time - lastSlashTime < SlashInterval)
            {
                return;
            }
            lastSlashTime = Time.time;
            if (_movement.y > 0)
            {
                _animatorCharacter.Play("UpSlash");
            }
            else if (_movement.y < 0)
            {
                _animatorCharacter.Play("DownSlash");
            }
            else
            {
                var rangeSlash = _nSlashType++ % 2;
                _animatorCharacter.Play($"Slash{rangeSlash}");
            }
        }
        

        private void FixedUpdate()
        {
            UpdateMove();
            UpdateMovement();
            UpdateDirection();
            UpdateJump();
            UpdateGravityScale();
        }

        private void UpdateMove()
        {
            if (_isSliding)
            {
                //滑墙时禁止向墙面移动
                if (Mathf.Approximately(_movement.x, _nDirection))
                {
                    return;
                }
            }

            if (_movement.x == 0 || _isSlideJumping)
            {
                return;
            }
            VelocityX = MoveSpeed * _movement.x;
        }


        private void UpdateMovement()
        {
            if (_isSliding)
            {
                _animatorCharacter.SetInteger(Movement, 0);
            }
            else if (_isGrounded)
            {
                _animatorCharacter.SetInteger(Movement, _movement.x != 0 ? 1 : 0);
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
            
            if (_isSliding && !_isSlideJumping)
            {
                StartCoroutine(SlidingJump());
                return;
            }
            rigidbodyCharacter.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
        }

        private void UpdateGravityScale()
        {
            float gravityScale;

            if (_isSliding)
            {
                gravityScale = SlideGravityScale;
            }
            else
            {
                gravityScale = VelocityY > 0.0f ? JumpGravityScale : FallingGravityScale;
                _animatorCharacter.SetFloat(SpeedY, VelocityY);
            }

            rigidbodyCharacter.gravityScale = gravityScale;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var normal = collision.contacts[0].normal;
            if (collision.gameObject.CompareTag("Ground") && normal == Vector2.up)
            {
                _isGrounded = true;
                _animatorCharacter.SetBool(Grounded, true);
                _animatorCharacter.SetBool(Sliding, false);
                _isCanJump = false;
                _isJumping = false;
                _isSlideJumping = false;
                _isSliding = false;
                _nJumpCount = 0;
            }

            if ((normal == Vector2.left || normal == Vector2.right) && !_isGrounded)
            {
                _animatorCharacter.SetBool(Sliding, true);
                VelocityY = 0;
                _isSliding = true;
                _isCanJump = false;
                _isSlideJumping = false;
                _isJumping = false;
                _nJumpCount = 0;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (_isSliding)
            {
                _animatorCharacter.SetBool(Sliding, false);
                _isSliding = false;
                return;
            }

            if (_isGrounded)
            {
                _animatorCharacter.SetBool(Grounded, false);
            }
        }
    }
}