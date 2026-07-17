using JetBrains.Annotations;
using UnityEngine;

namespace Project.Scripts
{
    public class Deactivate : MonoBehaviour
    {
        private bool _deactivationScheduled;

        private void OnCollisionExit([NotNull] Collision other)
        {
            if (PlayerController.Dead) return;
            if (!other.gameObject.CompareTag("Player") || _deactivationScheduled) return;

           
            _deactivationScheduled = true;
            Invoke(nameof(SetInactive), 4.0f);
        }

        private void SetInactive()
        {
            gameObject.SetActive(false);
            _deactivationScheduled = false;
        }
    }
}
