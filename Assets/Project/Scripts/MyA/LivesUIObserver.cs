using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts
{
    public class LivesUIObserver : MonoBehaviour
    {
        [SerializeField] private Texture aliveIcon;
        [SerializeField] private Texture deadIcon;
        [SerializeField] private RawImage[] icons;

        private void OnEnable()
        {
            PlayerController.OnLivesChanged += UpdateLivesUI;
        }

        private void OnDisable()
        {
            PlayerController.OnLivesChanged -= UpdateLivesUI;
        }

        private void UpdateLivesUI(int livesLeft)
        {
            for (var i = 0; i < icons.Length; i++)
            {
                icons[i].texture = i < livesLeft ? aliveIcon : deadIcon;
            }
        }
    }
}
