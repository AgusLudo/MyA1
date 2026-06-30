using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts
{
    public class PowerupUI : MonoBehaviour
    {
        public static PowerupUI Singleton;

        [Header("X2")]
        public Image x2Image;
        public Text x2Text;

        [Header("Escudo")]
        public Image shieldImage;
        public Text shieldText;

        [Header("Controles invertidos")]
        public Image invertImage;
        public Text invertText;

        [Header("Puntaje congelado")]
        public Image frozenImage;
        public Text frozenText;

        private float x2Timer;
        private float x2Duration;

        private float shieldTimer;
        private float shieldDuration;

        private float invertTimer;
        private float invertDuration;

        private float frozenTimer;
        private float frozenDuration;

        private void Awake()
        {
            Singleton = this;

            x2Image.gameObject.SetActive(false);
            x2Text.gameObject.SetActive(false);

            shieldImage.gameObject.SetActive(false);
            shieldText.gameObject.SetActive(false);

            invertImage.gameObject.SetActive(false);
            invertText.gameObject.SetActive(false);

            frozenImage.gameObject.SetActive(false);
            frozenText.gameObject.SetActive(false);
        }

        private void Update()
        {
            UpdateEffect(ref x2Timer, x2Duration, x2Image, x2Text);
            UpdateEffect(ref shieldTimer, shieldDuration, shieldImage, shieldText);
            UpdateEffect(ref invertTimer, invertDuration, invertImage, invertText);
            UpdateEffect(ref frozenTimer, frozenDuration, frozenImage, frozenText);
        }

        private void UpdateEffect(ref float timer, float duration, Image image, Text text)
        {
            if (timer <= 0)
                return;

            timer -= Time.deltaTime;

            if (timer < 0)
                timer = 0;

            text.text = Mathf.CeilToInt(timer) + "s";

            if (timer <= 0)
            {
                image.gameObject.SetActive(false);
                text.gameObject.SetActive(false);
            }
        }

        public void ShowDoubleScore(float duration)
        {
            x2Duration = duration;
            x2Timer = duration;

            x2Image.gameObject.SetActive(true);
            x2Text.gameObject.SetActive(true);

            x2Text.text = Mathf.CeilToInt(duration) + "s";
        }

        public void ShowShield(float duration)
        {
            shieldDuration = duration;
            shieldTimer = duration;

            shieldImage.gameObject.SetActive(true);
            shieldText.gameObject.SetActive(true);

            shieldText.text = Mathf.CeilToInt(duration) + "s";
        }

        public void ShowInvert(float duration)
        {
            invertDuration = duration;
            invertTimer = duration;

            invertImage.gameObject.SetActive(true);
            invertText.gameObject.SetActive(true);

            invertText.text = Mathf.CeilToInt(duration) + "s";
        }

        public void ShowFrozen(float duration)
        {
            frozenDuration = duration;
            frozenTimer = duration;

            frozenImage.gameObject.SetActive(true);
            frozenText.gameObject.SetActive(true);

            frozenText.text = Mathf.CeilToInt(duration) + "s";
        }
    }
}