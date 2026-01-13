using System;
using System.Collections;
using SKD.World_Manager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace SKD.Character.Player
{
    public class PlayerUILoadingScreenManager : MonoBehaviour
    {
        [SerializeField] GameObject _loadingScreen;
        [SerializeField] CanvasGroup _canvasGroup;
        private Coroutine _fadeLoadingScreenCoroutine;

        void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
        }
        void OnSceneChanged(Scene arg0, Scene arg1)
        {
            DeactivateLoadingScreen();
        }
        public void ActivateLoadingScreen()
        {
            if (_loadingScreen.activeSelf)
                return;

            _canvasGroup.alpha = 1;
            _loadingScreen.SetActive(true);
        }
        public void DeactivateLoadingScreen(float delay = 1)
        {
            if (!_loadingScreen.activeSelf)
                return;

            // Id we are already fading away the loading screen return
            if (_fadeLoadingScreenCoroutine != null)
                return;

            _fadeLoadingScreenCoroutine = StartCoroutine(FadeLoadingScreen(1, delay));
        }
        private IEnumerator FadeLoadingScreen(float duration, float delay)
        {
            while(WorldAIManager.Instance._isPerformingLoadingOpartion)
            {
                yield return null;

            }
            _loadingScreen.SetActive(true);

            if (duration > 0)
            {
                while(delay > 0)
                {
                    delay -= Time.deltaTime;
                    yield return null;
                }
                _canvasGroup.alpha = 1;
                float elapsedTime = 0;
                yield return null;

                while(elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    _canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
                    yield return null;
                }
            }
            _canvasGroup.alpha = 0;
            _loadingScreen.SetActive(false);
            _fadeLoadingScreenCoroutine = null;
            yield return null;
        }
    }
}