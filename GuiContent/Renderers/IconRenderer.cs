using System;
using Microsoft.Xna.Framework.Graphics;
using Ruminate.GUI.Framework;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Ruminate.GUI.Content {

    public class IconRenderer : Renderer {

        public String Icon {RenderController

        public IconRenderer(RenderManager renderManager) : base(renderManager) { }

        public override void Draw(Skin skin, Rectangle area, ref Rectangle safeArea) {

            Skin = skin;

            RenderRectangle = area;
            safeArea = area;

            LoadFromSkin(Icon);

            Render(SpriteEffects.None);
        } 
    }
}
