namespace Ruminate.GUI.Framework {

    /// <summary>
    /// 
    /// </summary>
    internal interface IInputData {
        InputData InputData { get; set; }
    }

    internal class InputData : IWidgetComponent {

        public WidgetBase Widget { get; private set; }

        /*####################################################################*/
        /*                              Input State                           */
        /*####################################################################*/

        internal bool IsVisible { get; set; }
        internal bool IsActive { get; set; }

        internal bool IsPressed { private set; get; }
        internal bool IsFocused { private set; get; }
        internal bool IsHover { private set; get; }

        /// <summary>
        /// When true this widget blocks any other widget from 
        /// receiving input until the mouse leave this widget's
        /// input area.
        /// </summary>
        internal protected bool BlocksInput { get; set; }        

        /*####################################################################*/
        /*                               Functions                            */
        /*####################################################################*/

        internal InputData(WidgetBase widget) {
            Widget = widget;
        }

        internal void UpdateInputData(bool isPressed, bool isFocused, bool isHover) {
            IsPressed = isPressed;
            IsFocused = isFocused;
            IsHover = isHover;
        }        
    }
}
