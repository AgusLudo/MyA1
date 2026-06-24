using System.Collections;
using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        public static GameObject Player;
        public static GameObject CurrentPlatform;
        public static bool Dead;

        public static event Action<int> OnLivesChanged;
        public static event Action OnPlayerDied;
        public static event Action OnGameOver;

        public float jumpForce = 5;
        public GameObject magic;
        public Transform magicStartPosition;

        

        private static readonly int IsJumping = Animator.StringToHash("isJumping");
        private static readonly int IsMagic = Animator.StringToHash("isMagic");
        private static readonly int IsDead = Animator.StringToHash("isDead");
        private static readonly int IsFalling = Animator.StringToHash("isFalling");

        private Animator _anim;
        private Rigidbody _rb;
        private Rigidbody _magicRb;
        private bool _canTurn;
        private Vector3 _startPosition;
        private int _livesLeft;
        private bool _falling;
        private bool _controlsInverted;
        private Coroutine _controlsInvertedCoroutine;

        private void Awake()
        {
            Player = gameObject;
            _anim = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _startPosition = Player.transform.position;
            _magicRb = magic.GetComponent<Rigidbody>();

            GenerateWorld.RunDummy();

            Dead = false;
            _livesLeft = PlayerPrefs.GetInt(PlayerPrefKeys.Lives);

            OnLivesChanged?.Invoke(_livesLeft);
        }

        // ReSharper disable once InconsistentNaming
        

        private void OnCollisionEnter([CanBeNull] Collision other)
        {
            var isDangerousCollision = other != null && (other.gameObject.CompareTag("Fire") ||
                                                         other.gameObject.CompareTag("Wall") ||
                                                         other.gameObject.CompareTag("OuterSpace"));
            var isLifeThreat = _falling ||
                   (isDangerousCollision && !GameData.Singleton.IsInvincibleActive);
            if (isLifeThreat && !Dead)
            {
                _anim.SetTrigger(_falling ? IsFalling : IsDead);

                Dead = true;
                OnPlayerDied?.Invoke();

                --_livesLeft;
                PlayerPrefs.SetInt(PlayerPrefKeys.Lives, _livesLeft);
                OnLivesChanged?.Invoke(_livesLeft);

                if (_livesLeft > 0)
                {
                    Invoke(nameof(RestartGame), 2);
                }
                else
                {
                    OnGameOver?.Invoke();
                }
            }
        }

        private void OnTriggerEnter([NotNull] Collider other)
        {
            
            if (other is BoxCollider && !GenerateWorld.LastPlatform.CompareTag("platformTSection"))
            {
                GenerateWorld.RunDummy();
            }

            
            if (other is SphereCollider)
            {
                _canTurn = true;
            }
        }

        private void OnTriggerExit([NotNull] Collider other)
        {
           
            if (other is SphereCollider)
            {
                _canTurn = false;
            }
        }

        private void Update()
        {
            if (Dead) return;

            // morir al caer al vacio
            if (transform.position.y <= -10f)
            {
                _falling = true;
                OnCollisionEnter(null);
                return;
            }

            if (CurrentPlatform != null)
            {
                const float shortFallingDistance = 2f;
                if (transform.position.y < CurrentPlatform.transform.position.y - shortFallingDistance)
                {
                    _anim.SetTrigger(IsFalling);
                }
            }

            var rotateDown = Input.GetButtonDown("Rotate");
            var rotate = Input.GetAxisRaw("Rotate") * (rotateDown ? 1 : 0);

            var shiftDown = Input.GetButtonDown("Horizontal");
            var shift = Input.GetAxisRaw("Horizontal") * (shiftDown ? 1 : 0);

            if (_controlsInverted)
            {
                rotate *= -1;
                shift *= -1;
            }

            var delayedDummySpawn = false;
            if (Input.GetButtonDown("Jump") && !_anim.GetBool(IsJumping))
            {
                _anim.SetBool(IsJumping, true);
                GameData.Singleton.SoundJump.Play();
                _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else if (Input.GetButtonDown("Fire1"))
            {
                _anim.SetBool(IsMagic, true);
            }
            else if (rotate > 0 && _canTurn)
            {
                transform.Rotate(Vector3.up * 90);
                GameData.Singleton.SoundWhoosh.Play();
                delayedDummySpawn = true;
                _canTurn = false;
            }
            else if (rotate < 0 && _canTurn)
            {
                transform.Rotate(Vector3.up * -90);
                GameData.Singleton.SoundWhoosh.Play();
                delayedDummySpawn = true;
                _canTurn= false;
            }
            else if (shift > 0)
            {
                transform.Translate(0.5f, 0, 0);
                
                
            }
            else if (shift < 0)
            {
                transform.Translate(-0.5f, 0, 0);
               
                
            }

            if (!delayedDummySpawn) return;
            var tf = transform;
            GenerateWorld.DummyTraveller.transform.forward = -tf.forward;

            GenerateWorld.RunDummy();

            // Build more platforms into the future, unless we just generated a T-section
            if (!GenerateWorld.LastPlatform.CompareTag("platformTSection"))
            {
                GenerateWorld.RunDummy();
            }

            transform.position = new Vector3(_startPosition.x, tf.position.y, _startPosition.z);
        }
        private void ResetRightRotation()
        {
            transform.Rotate(0f, -45f, 0f);
        }

        private void ResetLeftRotation()
        {
            transform.Rotate(0f, 45f, 0f);
        }

        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        public void ActivateInvertedControls(float duration)
        {
            if (_controlsInvertedCoroutine != null)
            {
                StopCoroutine(_controlsInvertedCoroutine);
            }

            _controlsInvertedCoroutine = StartCoroutine(InvertedControlsRoutine(duration));
        }

        private IEnumerator InvertedControlsRoutine(float duration)
        {
            _controlsInverted = true;

            yield return new WaitForSeconds(duration);

            _controlsInverted = false;
            _controlsInvertedCoroutine = null;
        }

        [UsedImplicitly]
        private void PreCastMagic()
        {
            GameData.Singleton.SoundCastMagic.Play();
        }

        [UsedImplicitly]
        private void CastMagic()
        {
            magic.transform.position = magicStartPosition.position;
            magic.SetActive(true);
            _magicRb.AddForce(transform.forward * 20, ForceMode.Impulse);
            Invoke(nameof(KillMagic), 1);
        }

        [UsedImplicitly]
        private void FootStepLeft() => GameData.Singleton.SoundFootstep1.Play();

        [UsedImplicitly]
        private void FootStepRight() => GameData.Singleton.SoundFootstep2.Play();

        private void KillMagic()
        {
            magic.SetActive(false);

            // Reset spell forces.
            _magicRb.velocity = Vector3.zero;
        }

        [UsedImplicitly]
        private void StopJump()
        {
            _anim.SetBool(IsJumping, false);
        }

        [UsedImplicitly]
        private void StopMagic()
        {
            _anim.SetBool(IsMagic, false);
        }
    }
}
