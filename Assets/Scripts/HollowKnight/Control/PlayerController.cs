using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common;
using DesignPattern;
using DG.Tweening;
using HollowKnight.AbstractObject;
using HollowKnight.EnumData;
using HollowKnight.ObjectsBehaviourInterface;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace HollowKnight.Control
{
    public class PlayerController : MonoSingleton<PlayerController>,IDefenseBehaviour
    {
        //移动常数
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
        private int _nSlashIndex;
        private SlashType _slashType;
        private float _lastSlashTime;
        private float _invincibilityTime;
        private const float SlashInterval = 0.2f;
        
        //玩家身上的组件
        protected Animator AnimatorGameObject;
        protected Rigidbody2D RigidBodyGameObject;
        private Collider2D _collider2DSlash;
        private Collider2D _collider2DUpSlash;
        private Collider2D _collider2DDownSlash;
        
        private Vector2 _movementDirection;
        private int _nJumpCount;
        private int _nDirection;

        private bool _isStunned;
        private bool _isInvincible;
        private bool _isGrounded;
        private bool _isCanJump;
        private bool _isJumping;
        private bool _isSliding;
        private bool _isSlideJumping;
        
        private PlayerInputAssets _playerInputAssets;

        private PlayerInputAssets PlayerInputAssets
        {
            get { return _playerInputAssets ??= new PlayerInputAssets(); }
            set => _playerInputAssets = value;
        }
        
        public float VelocityX
        {
            get => RigidBodyGameObject.velocity.x;
            set => RigidBodyGameObject.velocity = new Vector2(value, VelocityY);
        }

        public float VelocityY
        {
            get => RigidBodyGameObject.velocity.y;
            set => RigidBodyGameObject.velocity = new Vector2(VelocityX, value);
        }
 
        protected new void Awake()
        {
            base.Awake();
            FindComponents();
            _lastSlashTime = Time.time;
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
            foreach (var tween in CommonFields.S_NeedRecyclesTween)
            {
                tween?.Kill();
            }
            PlayerInputAssets.PlayerInputMap.Move.performed -= CallBackMove;
            PlayerInputAssets.PlayerInputMap.Jump.started -= CallBackJump;
            PlayerInputAssets.PlayerInputMap.Jump.performed -= CallbackCancelJump;
            PlayerInputAssets.PlayerInputMap.Jump.canceled -= CallbackCancelJump;
            PlayerInputAssets.PlayerInputMap.Attack.started -= CallbackAttack;
            PlayerInputAssets.Disable();
        }

        private void FindComponents()
        {
            AnimatorGameObject = GetComponent<Animator>();
            RigidBodyGameObject = GetComponent<Rigidbody2D>();
            _collider2DSlash = transform.Find("Attacks/Slash").GetComponent<PolygonCollider2D>();
            _collider2DUpSlash = transform.Find("Attacks/UpSlash").GetComponent<PolygonCollider2D>();
            _collider2DDownSlash = transform.Find("Attacks/DownSlash").GetComponent<PolygonCollider2D>();
            PlayerInputAssets = new PlayerInputAssets();
        }

        private void CallBackMove(InputAction.CallbackContext context)
        {
            _movementDirection = context.ReadValue<Vector2>();
        }

        private void CallBackJump(InputAction.CallbackContext contextCallback)
        {
            if (_isStunned)
            {
                return;
            }
            if (_nJumpCount >= 2)
            {
                _isCanJump = false;
                return;
            }

            _nJumpCount++;
            
            if (_nJumpCount == 2)
            {
                AnimatorGameObject.SetTrigger(CommonFields.DoubleJump);
                VelocityY = 0;
            }
            else if (!_isSliding)
            {
                AnimatorGameObject.SetTrigger(CommonFields.Jump);
            }
            
            AnimatorGameObject.SetBool(CommonFields.Grounded, false);
            _isCanJump = true;
        }

        private IEnumerator SlidingJump()
        {
            _isSlideJumping = true;
            AnimatorGameObject.SetTrigger(CommonFields.SlideJump);
            AnimatorGameObject.SetBool(CommonFields.Sliding,false);
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
            if (Time.time - _lastSlashTime < SlashInterval || _isStunned || _isSliding)
            {
                return;
            }
            _lastSlashTime = Time.time;
            if (_movementDirection.y > 0)
            {
                _slashType = SlashType.UpSlash;
                AnimatorGameObject.Play("UpSlash");
            }
            else if (_movementDirection.y < 0)
            {
                _slashType = SlashType.DownSlash;
                AnimatorGameObject.Play("DownSlash");
            }
            else
            {
                _slashType = SlashType.Slash;
                var rangeSlash = _nSlashIndex++ % 2;
                AnimatorGameObject.Play($"Slash{rangeSlash}");
            }

            CommonFields.S_NeedRecyclesTween.Add(DOVirtual.DelayedCall(0.08f, () => SlashDetection(_slashType)));
        }

        private Collider2D _collider2D;
        private ContactFilter2D _contactFilter;
        private readonly List<Collider2D> _collider2Ds = new List<Collider2D>();
        
        private void SlashDetection(SlashType slashType)
        {
            _collider2D = slashType switch
            {
                SlashType.Slash => _collider2DSlash,
                SlashType.UpSlash => _collider2DUpSlash,
                SlashType.DownSlash => _collider2DDownSlash,
                _ => null
            };

            Physics2D.OverlapCollider(_collider2D, _contactFilter, _collider2Ds);
            foreach (var colliderItem in _collider2Ds)
            {
                if (colliderItem.name != name)
                {
                    colliderItem.GetComponent<IDefenseBehaviour>()?.BeHit(transform.position, 2);
                }
            }
        }

        public void BeHit(Vector2 attackerPosition,int damage = 0)
        {
            CommonMethod.CameraShake(0.5f);
            VelocityX = 0;
            VelocityY = 0;
            _isStunned = true;
            var backDirectionX = transform.position.x - attackerPosition.x > 0 ? 1 : -1;
            var backDirectionY = transform.position.y - attackerPosition.y > 0 ? 1 : -1;
            AnimatorGameObject.Play("Hit");
            RigidBodyGameObject.AddForce(new Vector2(15 * backDirectionX, 5 * backDirectionY), ForceMode2D.Impulse);
            DOVirtual.DelayedCall(0.25f, () => _isStunned = false);
        }
        

        private void FixedUpdate()
        {
            UpdateMoveX(MoveSpeed);
            UpdateMoveY(0, JumpPower);
            UpdateMovement();
            UpdateDirection();
            UpdateGravityScale();
        }

        private void UpdateMoveX(float speed)
        {
            if (_isSliding)
            {
                //滑墙时禁止向墙面移动
                if (Mathf.Approximately(_movementDirection.x, _nDirection))
                {
                    return;
                }
            }

            if (_movementDirection.x == 0 || _isSlideJumping || _isStunned)
            {
                return;
            }
            VelocityX = speed * _movementDirection.x;
        }

        private void UpdateMoveY(float speed, float jumpPower = 0)
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
            RigidBodyGameObject.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }


        private void UpdateMovement()
        {
            if (_isSliding)
            {
                AnimatorGameObject.SetInteger(CommonFields.Movement, 0);
            }
            else if (_isGrounded)
            {
                AnimatorGameObject.SetInteger(CommonFields.Movement, _movementDirection.x != 0 ? 1 : 0);
            }
            else
            {
                AnimatorGameObject.SetInteger(CommonFields.Movement, VelocityX != 0 ? 1 : 0);
            }
        }

        private void UpdateDirection()
        {
            if (_isStunned)
            {
                return;
            }
            
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
                AnimatorGameObject.SetFloat(CommonFields.SpeedY, VelocityY);
            }

            RigidBodyGameObject.gravityScale = gravityScale;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var normal = collision.contacts[0].normal;
            if (collision.gameObject.CompareTag("Ground") && normal == Vector2.up)
            {
                _isGrounded = true;
                AnimatorGameObject.SetBool(CommonFields.Grounded, true);
                AnimatorGameObject.SetBool(CommonFields.Sliding, false);
                _isCanJump = false;
                _isJumping = false;
                _isSlideJumping = false;
                _isSliding = false;
                _nJumpCount = 0;
            }
            
            if ((normal == Vector2.left || normal == Vector2.right) && !_isGrounded)
            {
                AnimatorGameObject.SetBool(CommonFields.Sliding, true);
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
                AnimatorGameObject.SetBool(CommonFields.Sliding, false);
                _isSliding = false;
                return;
            }

            if (_isGrounded)
            {
                _isGrounded = false;
                AnimatorGameObject.SetBool(CommonFields.Grounded, false);
            }
        }
    }
}