using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts
{
    public class DoubleScorePickup : MonoBehaviour
    {
        public float duration = 10f;

        public float moveDistance = 2f;
        public float moveSpeed = 2f;

        private MeshRenderer[] _renderers;
        private Collider _collider;
        private Vector3 _startPosition;

        private void Awake()
        {
            _renderers = GetComponentsInChildren<MeshRenderer>();
            _collider = GetComponent<Collider>();

            // guarda la posición que vos le pusiste en el prefab
            _startPosition = transform.localPosition;
        }

        private void OnEnable()
        {
            // vuelve a la posición original cuando el pool lo reutiliza
            transform.localPosition = _startPosition;

            foreach (var r in _renderers)
            {
                r.enabled = true;
            }

            _collider.enabled = true;
        }

        private void Update()
        {
            float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;

            transform.localPosition = new Vector3(
                _startPosition.x + offset,
                _startPosition.y,
                _startPosition.z
            );
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            GameData.Singleton.ActivateDoubleScore(duration);
            PowerupUI.Singleton.ShowDoubleScore(duration);

            foreach (var r in _renderers)
            {
                r.enabled = false;
            }

            _collider.enabled = false;
        }
    }
}