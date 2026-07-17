using UnityEngine;

namespace Project.Scripts
{
    public class Scroll : MonoBehaviour
    {
        private GameObject _player;

        private void Start()
        {
            _player = PlayerController.Player;
        }

        private void FixedUpdate()
        {
            if (PlayerController.Dead) return;

            const float speed = -0.1f;
            transform.position += _player.transform.forward * speed;

            var currentPlatform = PlayerController.CurrentPlatform;
            if (currentPlatform == null) return;

            const float stairSlope = 0.06f;
            if (currentPlatform.CompareTag("stairsUp"))
            {
                // Mover mundo escalaeras
                transform.Translate(0, -stairSlope, 0);
            }
            else if (currentPlatform.CompareTag("stairsDown"))
            {
                // Para abajo
                transform.Translate(0, stairSlope, 0);
            }
        }
    }
}
