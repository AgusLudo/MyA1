using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts
{
    public class CoinPickup : MonoBehaviour
    {
        [Range(0, 100)]
        public float highValueProbability = 10;

        public int value = 10;
        public int highValue = 50;

        public Material regularMaterial;
        public Material highValueMaterial;
        public GameObject scorePrefab;
        public GameObject particlesPrefab;

        private bool _isBigCoin;
        private int _score;
        private Canvas _canvas;
        private MeshRenderer[] _renderers;

        private void Start()
        {
            _canvas = FindObjectOfType<Canvas>();
            _renderers = GetComponentsInChildren<MeshRenderer>();

            RandomizeScore();
        }

        private void OnEnable()
        {
            RandomizeScore();

            if (_renderers == null) return;

            for (var i = 0; i < _renderers.Length; ++i)
            {
                _renderers[i].enabled = true;
            }
        }

        private void OnTriggerEnter([NotNull] Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            var position = transform.position;

            // Sonido normal o grave según el estado del debuff
            if (GameData.Singleton.IsScoreFrozen)
            {
                GameData.Singleton.SoundPickup.pitch = 0.4f;
                GameData.Singleton.SoundPickup.Play();
                Invoke(nameof(ResetPickupPitch), 0.1f);
            }
            else
            {
                GameData.Singleton.SoundPickup.pitch = 1f;
                GameData.Singleton.SoundPickup.Play();
            }

            int displayedScore = _score;

            if (GameData.Singleton.IsDoubleScoreActive)
            {
                displayedScore *= 2;
            }

            GameData.Singleton.AddScore(_score);

            // Partículas
            var pe = Instantiate(particlesPrefab, position, Quaternion.identity, transform);
            Destroy(pe, 2.5f);

            // Mostrar popup solo si el puntaje no está congelado
            if (!GameData.Singleton.IsScoreFrozen)
            {
                var scoreText = Instantiate(scorePrefab, _canvas.transform, true);
                scoreText.GetComponent<Text>().text = displayedScore.ToString();

                Debug.Assert(Camera.main != null, "Camera.main != null");
                var screenPoint = Camera.main.WorldToScreenPoint(position);
                scoreText.transform.position = screenPoint;
            }

            // Ocultar la moneda
            for (var i = 0; i < _renderers.Length; ++i)
            {
                _renderers[i].enabled = false;
            }
        }

        private void ResetPickupPitch()
        {
            GameData.Singleton.SoundPickup.pitch = 1f;
        }

        private void RandomizeScore()
        {
            _isBigCoin = Random.Range(1, 101) <= highValueProbability;
            _score = _isBigCoin ? highValue : value;
            var material = _isBigCoin ? highValueMaterial : regularMaterial;

            if (_renderers == null) return;

            for (var i = 0; i < _renderers.Length; ++i)
            {
                _renderers[i].enabled = true;
                _renderers[i].material = material;
            }
        }
    }
}