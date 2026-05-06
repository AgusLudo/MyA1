using UnityEngine;

namespace Project.Scripts
{
    public class PlayerAudioObserver : MonoBehaviour
    {
        private void OnEnable()
        {
            PlayerController.OnPlayerDied += PlayDeathSound;
        }

        private void OnDisable()
        {
            PlayerController.OnPlayerDied -= PlayDeathSound;
        }

        private void PlayDeathSound()
        {
            GameData.Singleton.SoundDying.Play();
        }
    }
}