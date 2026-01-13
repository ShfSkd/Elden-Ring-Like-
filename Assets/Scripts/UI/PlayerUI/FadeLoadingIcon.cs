using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace SKD.UI.PlayerUI
{
    public class FadeLoadingIcon : MonoBehaviour
    {
        [SerializeField] Image _fadeImage;
        private Coroutine _fadeCoroutine;

        void OnEnable()
        {
            FadeUiImage();
        }
        void OnDisable()
        {
            if(_fadeCoroutine != null)
                StopCoroutine(_fadeCoroutine);
        }
        public void FadeUiImage()
        {
             if(_fadeCoroutine != null)
                 StopCoroutine(_fadeCoroutine);

             _fadeCoroutine = StartCoroutine(FadeCoroutine(true));
        }
        IEnumerator FadeCoroutine(bool fadeAway)
        {
            if (fadeAway)
            {
                for (float i = 1; i >=0 ; i-=Time.unscaledDeltaTime)
                {
                    _fadeImage.color = new Color(1, 1, 1, i);
                    yield return null;
                }
                _fadeCoroutine=StartCoroutine(FadeCoroutine(false));
            }
            else
            {
                for (float i = 0; i <=1 ; i+=Time.unscaledDeltaTime)
                {
                    _fadeImage.color = new Color(1, 1, 1, i);
                    yield return null;
                }
                _fadeCoroutine=StartCoroutine(FadeCoroutine(true));
            }
        }
    }
}