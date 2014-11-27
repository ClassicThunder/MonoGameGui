using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Ruminate.GUI.Framework {

    /// <summary>
    /// Stateless rule used to create reusuable render code that converts an area
    /// and skin into a graphic. 
    /// </summary>
    public abstract class Renderer {

        /*####################################################################*/
        /*                            Properties                              */
        /*####################################################################*/

        protected SpriteBatch SpriteBatch { get; set; }

        protected Skin Skin;

        protected Rectangle
            RenderRectangle,
            SourceRectangle;

        /*####################################################################*/
        /*                             Functions                              */
        /*####################################################################*/

        protected Renderer(RenderController renderManager) {

            SpriteBatch = renderManager.SpriteBatch;
            RenderRectangle = SourceRectangle = Rectangle.Empty;
        }

        public abstract void Draw(Skin skin, Rectangle area, ref Rectangle safeArea);

        /*####################################################################*/
        /*                              Helpers                               */
        /*####################################################################*/

        protected void LoadFromSkin(string name) {
            SourceRectangle = Skin.Map[name];
        }

        protected void Render(SpriteEffects effects) {
            SpriteBatch.Draw(
                Skin.Texture, 
                RenderRectangle, 
                SourceRectangle,
                Color.White,
                0, 
                Vector2.Zero, 
                effects,
                0);
        }        
    }
}
