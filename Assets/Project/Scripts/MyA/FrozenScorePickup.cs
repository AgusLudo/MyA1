using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts
{
    public class FrozenScorePickup : MonoBehaviour
    {
        public float duration = 10f;

        public float moveDistanceX = 2f;
        public float moveDistanceZ = 1f;
        public float moveSpeed = 2f;

        private MeshRenderer[] _renderers;
        private Vector3 _startPosition;

        private void Awake()
        {
            _renderers = GetComponentsInChildren<MeshRenderer>();
        }

        private void Start()
        {
            _startPosition = transform.localPosition;
        }

        private void Update()
        {
            float offsetX = Mathf.Sin(Time.time * moveSpeed) * moveDistanceX;
            float offsetZ = Mathf.Cos(Time.time * moveSpeed) * moveDistanceZ;

            transform.localPosition =
                _startPosition + new Vector3(offsetX, 0f, offsetZ);
        }

        private void OnEnable()
        {
            foreach (var r in _renderers)
            {
                r.enabled = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            GameData.Singleton.ActivateScoreFrozen(duration);

            foreach (var r in _renderers)
            {
                r.enabled = false;
            }
        }
    }
}
