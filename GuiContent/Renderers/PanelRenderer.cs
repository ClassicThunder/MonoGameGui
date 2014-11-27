using Microsoft.Xna.Framework.Graphics;
using Ruminate.GUI.Framework;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Ruminate.GUI.Content
{
    public class PanelRenderer : Renderer {

        private const string 
            Corner = "PanelCorner",
            EdgeLeft = "PanelEdgeLeft",
            EdgeTop = "PanelEdgeTop",
            BackGround = "PanelCenter";

        public PanelRenderer(RenderController renderManager) : base(renderManager) { }

        public override void Draw(Skin skin, Rectangle area, ref Rectangle safeArea) {

            Skin = skin;
            var borderWidth = skin.Map[BackGround].Width;

            // ##### Draw Background ##### //

            LoadFromSkin(BackGround);

            RenderRectangle.X = area.Left + borderWidth;
            RenderRectangle.Y = area.Top + borderWidth;
            RenderRectangle.Width = area.Width - (2 * borderWidth);
            RenderRectangle.Height = area.Height - (2 * borderWidth);

            Render(SpriteEffects.None);

            // ##### Draw Corners ##### //

            LoadFromSkin(Corner);

            RenderRectangle.Width = borderWidth;
            RenderRectangle.Height = borderWidth;

            //Top Left
            RenderRectangle.X = area.Left;
            RenderRectangle.Y = area.Top;
            Render(SpriteEffects.None);

            //Top Right
            RenderRectangle.X = area.Right - borderWidth;
            RenderRectangle.Y = area.Top;
            Render(SpriteEffects.FlipHorizontally);

            //Bottom Right
            RenderRectangle.X = area.Right - borderWidth;
            RenderRectangle.Y = area.Bottom - borderWidth;
            Render(SpriteEffects.FlipVertically);

            //Bottom Left
            RenderRectangle.X = area.Left;
            RenderRectangle.Y = area.Bottom - borderWidth;
            Render(SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);

            // ##### Draw Edges ##### //

            LoadFromSkin(EdgeTop);

            //Top Edge
            RenderRectangle.X = area.Left + borderWidth;
            RenderRectangle.Y = area.Top;
            RenderRectangle.Width = area.Width - (2 * borderWidth);
            SpriteBatch.Draw(skin.Texture, RenderRectangle, SourceRectangle, Color.White);

            //Bottom Edge
            RenderRectangle.Y = area.Bottom - borderWidth;
            SpriteBatch.Draw(skin.Texture, RenderRectangle, SourceRectangle, Color.White);

            LoadFromSkin(EdgeLeft);

            //Left Edge
            RenderRectangle.X = area.Left;
            RenderRectangle.Y = area.Top + borderWidth;
            RenderRectangle.Width = borderWidth;
            RenderRectangle.Height = area.Height - (2 * borderWidth);
            SpriteBatch.Draw(skin.Texture, RenderRectangle, SourceRectangle, Color.White);

            //Right Edge
            RenderRectangle.X = area.Right - borderWidth;
            SpriteBatch.Draw(skin.Texture, RenderRectangle, SourceRectangle, Color.White);

            //Safe Area
            safeArea.X = borderWidth;
            safeArea.Y = borderWidth;
            safeArea.Width = area.X - 2*borderWidth;
            safeArea.Height = area.Y - 2 * borderWidth;
        } 
    }
}
