using Microsoft.Xna.Framework.Graphics;
using Ruminate.GUI.Framework;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Ruminate.GUI.Content {

    public class VerticalSlidingDoorRenderer : Renderer {

        private const string
            Edge = "VSDEdge",
            Center = "VSDCenter";

        public VerticalSlidingDoorRenderer(RenderManager renderManager) : base(renderManager) { }

        public override void Draw(Skin skin, Rectangle area, ref Rectangle safeArea) {

            Skin = skin;

            var edgeWidth = skin.Map[Edge].Width;
            var edgeHeight = skin.Map[Edge].Height;

            // ##### Top Edge ##### //

            LoadFromSkin(Edge);

            RenderRectangle.X = area.Left;
            RenderRectangle.Y = area.Top;
            RenderRectangle.Width = edgeWidth;
            RenderRectangle.Height = edgeHeight;

            Render(SpriteEffects.None);

            // ##### Center ##### //

            LoadFromSkin(Center);

            RenderRectangle.Y = area.Top + edgeHeight;
            RenderRectangle.Height = area.Width - (2 * edgeHeight);

            Render(SpriteEffects.None);

            // ##### Buttom Edge ##### //

            LoadFromSkin(Edge);

            RenderRectangle.Y = area.Bottom - edgeHeight;
            RenderRectangle.Height = edgeHeight;

            Render(SpriteEffects.FlipVertically);
        }
    }
}
