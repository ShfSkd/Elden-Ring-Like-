using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace SKD.UI.PlayerUI
{
    public class PlayerUIPopUpManager : MonoBehaviour
    {
        [Header("Message Pop-up")]
        [SerializeField] TextMeshProUGUI _popUpMessageText;
        [SerializeField] GameObject _popUpMessageGameObject;

        [Header("You Died Pop-up")]
        [SerializeField] GameObject _youDiedpopUpGameObject;
        [SerializeField] TextMeshProUGUI _youDiedpopUpBackgroundText;
        [SerializeField] TextMeshProUGUI _youDiedPopUpText;
        [SerializeField] CanvasGroup _youDiedpopUpCanvasGroup; // Allows us to set the alpha to fade over time

        [Header("Boss Defeated Pop-up")]
        [SerializeField] GameObject _bossDefetedPopUpGameObject;
        [SerializeField] TextMeshProUGUI _bossDefetedPopUpBackgroundText;
        [SerializeField] TextMeshProUGUI _bossDefetedPopUpText;
        [SerializeField] CanvasGroup _bossDefetedPopUpCanvasGroup;
        public void CloseAllPopUpsWindows()
        {
            _popUpMessageGameObject.SetActive(false);

            PlayerUIManger.instance._popUpWindowIsOpen = false;
        }
        public void SendPlayerMessagePopUp(string messageText)
        {
            PlayerUIManger.instance._popUpWindowIsOpen = true;
            _popUpMessageText.text = messageText;
            _popUpMessageGameObject.SetActive(true);

        }
        public void SendYouDiedPopUp()
        {
            // Active post processing effects
            _youDiedpopUpGameObject.SetActive(true);
            _youDiedPopUpText.characterSpacing = 0;
            StartCoroutine(StretchPopUpTextOverTime(_youDiedpopUpBackgroundText, 8f, 8.32f));
            StartCoroutine(FadeInPopUpOverTime(_youDiedpopUpCanvasGroup, 5f));
            StartCoroutine(WaitThenFadeOutPopUpOverTime(_youDiedpopUpCanvasGroup, 2f, 5f));
        }

        public void SendBossDefeatedPopUp(string bossDefeatedMessage)
        {
            _bossDefetedPopUpText.text = bossDefeatedMessage;
            _bossDefetedPopUpBackgroundText.text = bossDefeatedMessage;
            _bossDefetedPopUpGameObject.SetActive(true);
            _bossDefetedPopUpText.characterSpacing = 0;
            StartCoroutine(StretchPopUpTextOverTime(_bossDefetedPopUpBackgroundText, 8f, 8.32f));
            StartCoroutine(FadeInPopUpOverTime(_bossDefetedPopUpCanvasGroup, 5f));
            StartCoroutine(WaitThenFadeOutPopUpOverTime(_bossDefetedPopUpCanvasGroup, 2f, 5f));
        }

        private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
        {
            if (duration > 0)
            {
                text.characterSpacing = 0;
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                    yield return null;
                }
            }
        }
        private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
        {
            if (duration > 0)
            {
                canvas.alpha = 0;
            }
            float timer = 0;

            yield return null;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                yield return null;
            }
            canvas.alpha = 1;

            yield return null;
        }
        private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)
        {
            if (duration > 0)
            {
                while (delay > 0)
                {
                    delay -= Time.deltaTime;
                    yield return null;
                }

                canvas.alpha = 1;
            }
            float timer = 0;

            yield return null;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                yield return null;
            }
            canvas.alpha = 0;

            yield return null;
        }


    }
}