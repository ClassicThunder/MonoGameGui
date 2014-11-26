using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ruminate.DataStructures;
using Ruminate.GUI.Content;

namespace Ruminate.GUI.Framework {

    public class RenderManager {

        /*####################################################################*/
        /*                            Render State                            */
        /*####################################################################*/

        private RasterizerState RasterizerState { get; set; }

        public GraphicsDevice GraphicsDevice { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }

        /*####################################################################*/
        /*                             Skin Info                              */
        /*####################################################################*/

        public string DefaultSkin { get; set; }
        public string DefaultText { get; set; }

        public Dictionary<string, Skin> Skins { get; private set; }
        public Dictionary<string, Text> Texts { get; private set; }

        /*####################################################################*/
        /*                              Options                               */
        /*####################################################################*/

        public Texture2D SelectionColor { get; set; }
        public Color HighlightingColor {
            get {
                var data = new Color[1];
                SelectionColor.GetData(data);
                return data[0];
            }
            set {
                SelectionColor.SetData(new[] { value });
            }
        }

        /*####################################################################*/
        /*                             Renderers                              */
        /*####################################################################*/

        internal PanelRenderer PanelRenderer { get; private set; }
        internal VerticalSlidingDoorRenderer VerticalSlidingDoorRenderer { get; private set; }
        internal HorizontalSlidingDoorRenderer HorizontalSlidingDoorRenderer { get; private set; }
        
        private void BuildRenderers()
        {
            PanelRenderer = new PanelRenderer(this);
            VerticalSlidingDoorRenderer = new VerticalSlidingDoorRenderer(this);
            HorizontalSlidingDoorRenderer = new HorizontalSlidingDoorRenderer(this);
        }

        /*####################################################################*/
        /*                           Initialization                           */
        /*####################################################################*/

        internal RenderManager(GraphicsDevice device) {

            Skins = new Dictionary<string, Skin>();
            Texts = new Dictionary<string, Text>();

            GraphicsDevice = device;
            RasterizerState = new RasterizerState { ScissorTestEnable = true };
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            BuildRenderers();
        }        

        /*####################################################################*/
        /*                         State Management                           */
        /*####################################################################*/

        internal void Draw(Root<Widget> dom) {

            SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState);

            dom.DfsOperationChildren(node => {
                if (!node.Data.Visible) return;
                GraphicsDevice.ScissorRectangle = node.Data.SissorArea;
                node.Data.Draw();
            });
            
            dom.DfsOperationChildren(node => {
                if (!node.Data.Visible) return;
                GraphicsDevice.ScissorRectangle = node.Data.ClippingArea;
                node.Data.DrawNoClipping();
            });

            SpriteBatch.End();

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        /*####################################################################*/
        /*                          Skin Management                           */
        /*####################################################################*/

        internal void AddSkin(string name, Skin skin) {
            Skins.Add(name, skin);
        }

        internal void AddText(string name, Text textRenderer) {
            Texts.Add(name, textRenderer);
        }
    }
}
