using Microsoft.Xna.Framework.Graphics;
using Ruminate.GUI.Framework;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Ruminate.GUI.Content {

    public class HorizontalSlidingDoorRenderer : Renderer {

        private const string
            Edge = "HSDEdge",
            Center = "HSDCenter";

        public HorizontalSlidingDoorRenderer(RenderController renderManager) : base(renderManager) { }

        public override void Draw(Skin skin, Rectangle area, ref Rectangle safeArea) {

            Skin = skin;

            var edgeWidth = skin.Map[Edge].Width;
            var edgeHeight = skin.Map[Edge].Height;

            // ##### Left Edge ##### //

            LoadFromSkin(Edge);

            RenderRectangle.X = area.Left;
            RenderRectangle.Y = area.Top;
            RenderRectangle.Width = edgeWidth;
            RenderRectangle.Height = edgeHeight;

            Render(SpriteEffects.None);

            // ##### Center ##### //

            LoadFromSkin(Center);

            RenderRectangle.X += edgeHeight;
            RenderRectangle.Width = area.Width - (2 * edgeWidth);

            Render(SpriteEffects.None);

            // ##### Buttom Edge ##### //

            LoadFromSkin(Edge);

            RenderRectangle.X = area.Right - edgeHeight;
            RenderRectangle.Width = edgeHeight;

            Render(SpriteEffects.FlipVertically);
        }
    }
}
