using UnityEngine;

namespace MossWolfGames.Shared.Runtime.Gui
{
    public class GuiDrawViewModel
    {
        public Rect CanvasRect { get; set; }
        public Vector2 PanPosition { get; set; }

        public float MinZoom { get; set; }

        public float MaxZoom { get; set; }

        /// <summary>
        /// The zoom value used for camera controls
        /// </summary>
        public float LinearZoom { get; set; } = 1;

        /// <summary>
        /// The zoom value used for rendering.
        /// </summary>
        public float ActualZoom
        {
            get
            {
                float range = MaxZoom - MinZoom;
                float factor = (LinearZoom - MinZoom) / range;

                factor *= factor;

                return MinZoom + range * Mathf.Clamp01(factor);
            }
        }
    }
}