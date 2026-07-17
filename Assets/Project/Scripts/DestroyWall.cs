using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Project.Scripts
{
    public class DestroyWall : MonoBehaviour
    {
        public GameObject[] bricks;
        public GameObject explosionPrefab;

        private readonly List<Rigidbody> _bricksRbs = new List<Rigidbody>();
        private readonly List<Vector3> _bricksPositions = new List<Vector3>();
        private readonly List<Quaternion> _bricksRolations = new List<Quaternion>();
        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            foreach (var brick in bricks)
            {
                _bricksRbs.Add(brick.GetComponent<Rigidbody>());
                _bricksPositions.Add(brick.transform.localPosition);
                _bricksRolations.Add(brick.transform.rotation);
            }
        }

        private void OnEnable()
        {
            // Reactivar collision
            _collider.enabled = true;

            // Resetear ladrillos
            for (var index = 0; index < bricks.Length; index++)
            {
                var brick = bricks[index];
                _bricksRbs[index].isKinematic = true;
                brick.transform.localPosition = _bricksPositions[index];
                brick.transform.rotation = _bricksRolations[index];
            }

            // SAcar explosiones
            foreach (var ps in GetComponentsInChildren<ParticleSystem>())
            {
                Destroy(ps.gameObject);
            }
        }

        private void OnCollisionEnter([NotNull] Collision other)
        {
            if (!other.gameObject.CompareTag("Spell")) return;

            GameData.Singleton.SoundExplosion.Play();

            var spellPosition = other.contacts[0].point;
            var explosion = Instantiate(explosionPrefab, spellPosition, Quaternion.identity, transform);
            Destroy(explosion, 2.5f); 
            
            _collider.enabled = false;

            
            foreach (var rb in _bricksRbs)
            {
                rb.isKinematic = false;
            }
        }
    }
}
