using System;
using UnityEngine;
using UnityEngine.UI;

namespace MossWolfGames.Shared.Runtime.UI
{
    public class ButtonBase : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        public event Action OnClickedE;

        private void Reset()
        {
            if (button == null)
            {
                button = GetComponentInChildren<Button>();
            }
            if (button == null)
            {
                button = GetComponentInParent<Button>();
            }
        }

        private void OnEnable()
        {
            button.onClick.AddListener(Button_OnClick);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(Button_OnClick);
        }

        protected virtual void OnClicked() { }

        private void Button_OnClick()
        {
            OnClicked();
            OnClickedE?.Invoke();
        }
    }
}