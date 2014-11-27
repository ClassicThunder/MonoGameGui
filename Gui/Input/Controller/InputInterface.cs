using Microsoft.Xna.Framework.Input;

namespace Ruminate.GUI.Framework {

    /// <summary>
    /// 
    /// </summary>
    internal interface IInputInterface {

        /*####################################################################*/
        /*                                Events                              */
        /*####################################################################*/

        //Interaction with internal layout system
        void EnterHover();
        void ExitHover();

        void EnterPressed();
        void ExitPressed();

        void EnterFocus();
        void ExitFocus();

        //Interaction with input system        
        void KeyDown(KeyEventArgs e);
        void CharEntered(CharacterEventArgs e);
        void KeyUp(KeyEventArgs e);

        void MouseHover(MouseEventArgs e);
        void MouseClick(MouseEventArgs e);
        void MouseDoubleClick(MouseEventArgs e);

        void MouseDown(MouseEventArgs e);
        void MouseUp(MouseEventArgs e);
        void MouseMove(MouseEventArgs e);
        void MouseWheel(MouseEventArgs e);
    }
}
