using Ruminate.GUI.Framework;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Ruminate.GUI.Content {

    public sealed class Panel : Widget {

        private PanelRenderer PanelRenderer { get; set; }

        /*####################################################################*/
        /*                           Initialization                           */
        /*####################################################################*/

        public Panel(int x, int y, int width, int height) {

            Area = new Rectangle(x, y, width, height);
        }

        protected override void Attach() {

            PanelRenderer = new PanelRenderer(RenderManager);
        }

        /*####################################################################*/
        /*                               Logic                                */
        /*####################################################################*/

        protected internal override void Update() {
            
        }

        internal override void Draw() {

            throw new System.NotImplementedException();
        }
    }
}