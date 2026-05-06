using UnityEngine;

namespace Project.Scripts
{
    public class GameOverObserver : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;

        private void OnEnable()
        {
            PlayerController.OnGameOver += ShowGameOverPanel;
        }

        private void OnDisable()
        {
            PlayerController.OnGameOver -= ShowGameOverPanel;
        }

        private void ShowGameOverPanel()
        {
            gameOverPanel.SetActive(true);
        }
    }
}
