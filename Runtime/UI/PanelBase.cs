using UnityEngine;

namespace MossWolfGames.Shared.Runtime.UI
{
    public abstract class PanelBase : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The gameobject that will be deactivated when this panel is hidden, and activated when this panel is shown.")]
        private GameObject root;

        [SerializeField]
        private bool showOnAwake = false;

        private bool visible;
        public bool Visible
        {
            get => visible;
            set
            {
                if(visible == value)
                {
                    return;
                }
                visible = value;
                OnVisibleChanged();
            }
        }

        protected virtual void Awake()
        {
            visible = showOnAwake;
            root.SetActive(visible);
            if(visible)
            {
                OnShow();
            }
        }

        protected virtual void OnEnable() { }

        protected virtual void OnDisable() { }

        protected virtual void OnDestroy() { }

        protected virtual void OnShow() { }

        protected virtual void OnHide() { }

        private void OnVisibleChanged()
        {
            if(visible)
            {
                root.SetActive(true);
                OnShow();
            }
            else
            {
                OnHide();
                root.SetActive(false);
            }
        }
    }
}