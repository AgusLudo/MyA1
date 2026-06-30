using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts
{
    public class DisplayStats : MonoBehaviour
    {
        public Text lastScore;
        public Text highestScore;

        private void OnEnable()
        {
            if (PlayerPrefs.HasKey(PlayerPrefKeys.LastScore))
            {
                lastScore.text = $"Ultimo puntaje: {PlayerPrefs.GetInt(PlayerPrefKeys.LastScore)}";
            }
            else
            {
                lastScore.text = "Ultimo puntaje: 0";
            }

            if (PlayerPrefs.HasKey(PlayerPrefKeys.Highscore))
            {
                highestScore.text = $"Puntaje alto: {PlayerPrefs.GetInt(PlayerPrefKeys.Highscore)}";
            }
            else
            {
                highestScore.text = "Puntaje alto: 0";
            }
        }
    }
}
