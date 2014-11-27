using Microsoft.Xna.Framework.Input;

namespace Ruminate.GUI.Framework {

    internal abstract class Widget : IInputInterface {

        private WidgetBase WidgetBase { get; set; }

        internal Widget() {
             WidgetBase = new WidgetBase();
        }

        /*####################################################################*/
        /*                                Input                               */
        /*####################################################################*/

        public virtual void EnterHover() {
            throw new System.NotImplementedException();
        }

        public virtual void ExitHover() {
            throw new System.NotImplementedException();
        }

        public virtual void EnterPressed() {
            throw new System.NotImplementedException();
        }

        public virtual void ExitPressed() {
            throw new System.NotImplementedException();
        }

        public virtual void EnterFocus() {
            throw new System.NotImplementedException();
        }

        public virtual void ExitFocus() {
            throw new System.NotImplementedException();
        }

        public virtual void KeyDown(KeyEventArgs e) {
            throw new System.NotImplementedException();
        }

        public virtual void CharEntered(CharacterEventArgs e) {
            throw new System.NotImplementedException();
        }

        public virtual void KeyUp(KeyEventArgs e) {
            throw new System.NotImplementedException();
        }

        public virtual void MouseHover(MouseEventArgs e) {
            throw new System.NotImplementedException();
        }

        public virtual void MouseClick(MouseEventArgs e) {
            throw new System.NotImplementedException();
        }

        public virtual void MouseDoubleClick(MouseEventArgs e) {
            throw new System.NotImplementedException();
        }

        public virtual void MouseDown(MouseEventArgs e) {
            throw new System.NotImplementedException();
        }

        public virtual void MouseUp(MouseEventArgs e) {
            throw new System.NotImplementedException();
        }

        public virtual void MouseMove(MouseEventArgs e) {
            throw new System.NotImplementedException();
        }

        public virtual void MouseWheel(MouseEventArgs e) {
            throw new System.NotImplementedException();
        }
    }
}
