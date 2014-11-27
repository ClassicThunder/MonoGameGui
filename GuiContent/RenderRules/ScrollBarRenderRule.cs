using Microsoft.Xna.Framework;
using Ruminate.GUI.Framework;

namespace Ruminate.GUI.Content {

    public abstract class ScrollBarRenderRule : RenderRule {

        protected SlidingDoorRenderer Bar, Holder;

// ReSharper disable InconsistentNaming
        protected Rectangle barArea;
        public Rectangle BarArea { get { return barArea; } }

        protected Rectangle holderArea;
        public Rectangle HolderArea { get { return holderArea; } }

        protected Rectangle increaseArea;
        public Rectangle IncreaseArea { get { return increaseArea; } }

        protected Rectangle decreaseArea;
        public Rectangle DecreaseArea { get { return decreaseArea; } }
// ReSharper restore InconsistentNaming

        public float ChildOffset { get { return float.IsNaN(BarOffset / Ratio) ? 0 : BarOffset / Ratio; } }
        public abstract float BarOffset { get; set; }
        public float MaxBarOffset { get; protected set; }
        public float Ratio { get; protected set; }

        public bool Both { get; set; }

        public abstract void CalculateRatio(Rectangle renderArea);

        public void SetRenderManager(RenderController manager) {
            RenderController = manager;
        }

        public override void Draw() {
            
            Holder.Render(RenderController.SpriteBatch, HolderArea);
            Bar.Render(RenderController.SpriteBatch, BarArea);
        }

        public override void DrawNoClipping() { }
    }
}