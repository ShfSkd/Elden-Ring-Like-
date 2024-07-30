using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SKD.Menu_Screen
{
    public class UI_Match_Scroll_Wheel_To_Selected_Button : MonoBehaviour
    {
        [SerializeField] GameObject _currentSelected;
        [SerializeField] GameObject _previsusSelected;
        [SerializeField] RectTransform _currentSelectedTransform;
        [SerializeField] RectTransform _contentPanel;
        [SerializeField] ScrollRect _scrollRect;

        private void Update()
        {
            _currentSelected = EventSystem.current.currentSelectedGameObject;

            if (_currentSelected != null)
            {
                if (_currentSelected != _previsusSelected)
                {
                    _previsusSelected = _currentSelected;
                    _currentSelectedTransform = _currentSelected.GetComponent<RectTransform>();
                    SnapTo(_currentSelectedTransform);
                }
            }
        }
        private void SnapTo(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();

            Vector2 newPosition = (Vector2)_scrollRect.transform.InverseTransformPoint(_contentPanel.position) - (Vector2)_scrollRect.transform.InverseTransformPoint(target.position);

            // we only want to look the position in the y axis (Up and down)
            newPosition.x = 0;
            
            _contentPanel.anchoredPosition = newPosition;
        }
    }
}