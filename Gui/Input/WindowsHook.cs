using System;
using System.Runtime.InteropServices;

//http://www.gamedev.net/topic/457783-xna-getting-text-from-keyboard/
namespace Microsoft.Xna.Framework.Input {

    public class CharacterEventArgs : EventArgs {

        private readonly char character;
        private readonly int lParam;

        public CharacterEventArgs(char character, int lParam) {
            this.character = character;
            this.lParam = lParam;
        }

        public char Character { get { return character; } }
        public int Param { get { return lParam; } }
        public int RepeatCount { get { return lParam & 0xffff; } }
        public bool ExtendedKey { get { return (lParam & (1 << 24)) > 0; } }
        public bool AltPressed { get { return (lParam & (1 << 29)) > 0; } }
        public bool PreviousState { get { return (lParam & (1 << 30)) > 0; } }
        public bool TransitionState { get { return (lParam & (1 << 31)) > 0; } }

    }

    public class KeyEventArgs : EventArgs {

        private Keys keyCode;

        public KeyEventArgs(Keys keyCode) {
            this.keyCode = keyCode;
        }

        public Keys KeyCode { get { return keyCode; } }
    }

    public class MouseEventArgs : EventArgs {

        private MouseButton button;
        private int clicks;
        private int x;
        private int y;
        private int delta;

        public MouseButton Button { get { return button; } }
        public int Clicks { get { return clicks; } }
        public int X { get { return x; } }
        public int Y { get { return y; } }
        public Point Location { get { return new Point(x, y); } }
        public int Delta { get { return delta; } }

        public MouseEventArgs(MouseButton button, int clicks, int x, int y, int delta) {
            this.button = button;
            this.clicks = clicks;
            this.x = x;
            this.y = y;
            this.delta = delta;
        }

    }

    public delegate void CharEnteredHandler(object sender, CharacterEventArgs e);
    public delegate void KeyEventHandler(object sender, KeyEventArgs e);
    public delegate void MouseEventHandler(object sender, MouseEventArgs e);

    /// <summary>       /// Mouse Key Flags from WinUser.h for mouse related WM messages.</summary>     
    [Flags]
    public enum MouseKeys {
        LButton = 0x01,
        RButton = 0x02,
        Shift = 0x04,
        Control = 0x08,
        MButton = 0x10,
        XButton1 = 0x20,
        XButton2 = 0x40
    }
    public enum MouseButton {
        None,
        Left,
        Right,
        Middle,
        X1,
        X2
    }

    public static class InputSystem {

        #region Events

        /// <summary>Event raised when a character has been entered.</summary>          
        public static event CharEnteredHandler CharEntered;

        /// <summary>Event raised when a key has been pressed down. May fire multiple times due to keyboard repeat.</summary>           
        public static event KeyEventHandler KeyDown;

        /// <summary>Event raised when a key has been released.</summary>
        public static event KeyEventHandler KeyUp;

        /// <summary>Event raised when a mouse button is pressed.</summary>
        public static event MouseEventHandler MouseDown;

        /// <summary>Event raised when a mouse button is released.</summary>    
        public static event MouseEventHandler MouseUp;

        /// <summary>Event raised when the mouse changes location.</summary>
        public static event MouseEventHandler MouseMove;

        /// <summary>Event raised when the mouse has hovered in the same location for a short period of time.</summary>         
        public static event MouseEventHandler MouseHover;

        /// <summary>Event raised when the mouse wheel has been moved.</summary>        
        public static event MouseEventHandler MouseWheel;

        /// <summary>Event raised when a mouse button has been double clicked.</summary>        
        public static event MouseEventHandler MouseDoubleClick;

        #endregion

        delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        
        public static bool Initialized { get; private set; }

        static IntPtr prevWndProc;
        static WndProc hookProcDelegate;
        static IntPtr hIMC;

        #region Win32 Constants
        const int GWL_WNDPROC = -4;
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_CHAR = 0x102;
        const int WM_IME_SETCONTEXT = 0x281;
        const int WM_INPUTLANGCHANGE = 0x51;
        const int WM_GETDLGCODE = 0x87;
        const int WM_IME_COMPOSITION = 0x10F;
        const int DLGC_WANTALLKEYS = 4;
        const int WM_MOUSEMOVE = 0x200;
        const int WM_LBUTTONDOWN = 0x201;
        const int WM_LBUTTONUP = 0x202;
        const int WM_LBUTTONDBLCLK = 0x203;
        const int WM_RBUTTONDOWN = 0x204;
        const int WM_RBUTTONUP = 0x205;
        const int WM_RBUTTONDBLCLK = 0x206;
        const int WM_MBUTTONDOWN = 0x207;
        const int WM_MBUTTONUP = 0x208;
        const int WM_MBUTTONDBLCLK = 0x209;
        const int WM_MOUSEWHEEL = 0x20A;
        const int WM_XBUTTONDOWN = 0x20B;
        const int WM_XBUTTONUP = 0x20C;
        const int WM_XBUTTONDBLCLK = 0x20D;
        const int WM_MOUSEHOVER = 0x2A1;
        #endregion

        #region DLL Imports
        [DllImport("Imm32.dll")]
        static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("Imm32.dll")]
        static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

        [DllImport("user32.dll")]
        static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        #endregion

        public static Point MouseLocation {
            get {
                var state = Mouse.GetState();
                return new Point(state.X, state.Y);
            }
        }

        public static bool ShiftDown {
            get {
                var state = Keyboard.GetState();
                return state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift);
            }
        }

        public static bool CtrlDown {
            get {
                var state = Keyboard.GetState();
                return state.IsKeyDown(Keys.LeftControl) || state.IsKeyDown(Keys.RightControl);
            }
        }

        public static bool AltDown {
            get {
                var state = Keyboard.GetState();
                return state.IsKeyDown(Keys.LeftAlt) || state.IsKeyDown(Keys.RightAlt);
            }
        }

        /// <summary>Initialize the TextInput with the given GameWindow.</summary>      
        /// <param name="window">The XNA window to which text input should be linked.</param>   
        public static void Initialize(GameWindow window) {

            if (Initialized) { return; }

            hookProcDelegate = HookProc;
            prevWndProc = (IntPtr)SetWindowLong(window.Handle, GWL_WNDPROC, (int)Marshal.GetFunctionPointerForDelegate(hookProcDelegate));
            hIMC = ImmGetContext(window.Handle);

            Initialized = true;
        }

        static IntPtr HookProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam) {

            var returnCode = CallWindowProc(prevWndProc, hWnd, msg, wParam, lParam);

            switch (msg) {
                case WM_GETDLGCODE:
                    returnCode = (IntPtr)(returnCode.ToInt32() | DLGC_WANTALLKEYS);
                    break;
                case WM_KEYDOWN:
                    if (KeyDown != null)
                        KeyDown(null, new KeyEventArgs((Keys)wParam));
                    break;
                case WM_KEYUP:
                    if (KeyUp != null)
                        KeyUp(null, new KeyEventArgs((Keys)wParam));
                    break;
                case WM_CHAR:
                    if (CharEntered != null)
                        CharEntered(null, new CharacterEventArgs((char)wParam, lParam.ToInt32()));
                    break;
                case WM_IME_SETCONTEXT:
                    if (wParam.ToInt32() == 1)
                        ImmAssociateContext(hWnd, hIMC);
                    break;
                case WM_INPUTLANGCHANGE:
                    ImmAssociateContext(hWnd, hIMC);
                    returnCode = (IntPtr)1;
                    break;

                // Mouse messages                       
                case WM_MOUSEMOVE:
                    if (MouseMove != null) {
                        short x, y;
                        MouseLocationFromLParam(lParam.ToInt32(), out x, out y);
                        MouseMove(null, new MouseEventArgs(MouseButton.None, 0, x, y, 0));
                    }
                    break;
                case WM_MOUSEHOVER:
                    if (MouseHover != null) {
                        short x, y;
                        MouseLocationFromLParam(lParam.ToInt32(), out x, out y);
                        MouseHover(null, new MouseEventArgs(MouseButton.None, 0, x, y, 0));
                    }
                    break;
                case WM_MOUSEWHEEL:
                    if (MouseWheel != null) {
                        short x, y;
                        MouseLocationFromLParam(lParam.ToInt32(), out x, out y);
                        MouseWheel(null, new MouseEventArgs(MouseButton.None, 0, x, y, (wParam.ToInt32() >> 16) / 120));
                    }
                    break;
                case WM_LBUTTONDOWN:
                    RaiseMouseDownEvent(MouseButton.Left, wParam.ToInt32(), lParam.ToInt32());
                    break;
                case WM_LBUTTONUP:
                    RaiseMouseUpEvent(MouseButton.Left, wParam.ToInt32(), lParam.ToInt32());
                    break;
                case WM_LBUTTONDBLCLK:
                    RaiseMouseDblClickEvent(MouseButton.Left, wParam.ToInt32(), lParam.ToInt32());
                    break;
                case WM_RBUTTONDOWN:
                    RaiseMouseDownEvent(MouseButton.Right, wParam.ToInt32(), lParam.ToInt32());
                    break;
                case WM_RBUTTONUP:
                    RaiseMouseUpEvent(MouseButton.Right, wParam.ToInt32(), lParam.ToInt32());
                    break;
                case WM_RBUTTONDBLCLK:
                    RaiseMouseDblClickEvent(MouseButton.Right, wParam.ToInt32(), lParam.ToInt32());
                    break;
                case WM_MBUTTONDOWN:
                    RaiseMouseDownEvent(MouseButton.Middle, wParam.ToInt32(), lParam.ToInt32());
                    break;
                case WM_MBUTTONUP:
                    RaiseMouseUpEvent(MouseButton.Middle, wParam.ToInt32(), lParam.ToInt32());
                    break;
                case WM_MBUTTONDBLCLK:
                    RaiseMouseDblClickEvent(MouseButton.Middle, wParam.ToInt32(), lParam.ToInt32());
                    break;
                case WM_XBUTTONDOWN:
                    if ((wParam.ToInt32() & 0x10000) != 0) {
                        RaiseMouseDownEvent(MouseButton.X1, wParam.ToInt32(), lParam.ToInt32());
                    } else if ((wParam.ToInt32() & 0x20000) != 0) {
                        RaiseMouseDownEvent(MouseButton.X2, wParam.ToInt32(), lParam.ToInt32());
                    }
                    break;
                case WM_XBUTTONUP:
                    if ((wParam.ToInt32() & 0x10000) != 0) {
                        RaiseMouseUpEvent(MouseButton.X1, wParam.ToInt32(), lParam.ToInt32());
                    } else if ((wParam.ToInt32() & 0x20000) != 0) {
                        RaiseMouseUpEvent(MouseButton.X2, wParam.ToInt32(), lParam.ToInt32());
                    }
                    break;
                case WM_XBUTTONDBLCLK:
                    if ((wParam.ToInt32() & 0x10000) != 0) {
                        RaiseMouseDblClickEvent(MouseButton.X1, wParam.ToInt32(), lParam.ToInt32());
                    } else if ((wParam.ToInt32() & 0x20000) != 0) {
                        RaiseMouseDblClickEvent(MouseButton.X2, wParam.ToInt32(), lParam.ToInt32());
                    }
                    break;
            }

            return returnCode;
        }

        #region Mouse Message Helpers
        static void RaiseMouseDownEvent(MouseButton button, int wParam, int lParam) {
            
            if (MouseDown == null) { return; }

            short x, y;
            MouseLocationFromLParam(lParam, out x, out y);
            MouseDown(null, new MouseEventArgs(button, 1, x, y, 0));
        }

        static void RaiseMouseUpEvent(MouseButton button, int wParam, int lParam) {
            
            if (MouseUp == null) { return; }

            short x, y;
            MouseLocationFromLParam(lParam, out x, out y);
            MouseUp(null, new MouseEventArgs(button, 1, x, y, 0));
        }

        static void RaiseMouseDblClickEvent(MouseButton button, int wParam, int lParam) {
            
            if (MouseDoubleClick == null) { return; }

            short x, y;
            MouseLocationFromLParam(lParam, out x, out y);
            MouseDoubleClick(null, new MouseEventArgs(button, 1, x, y, 0));
        }

        static void MouseLocationFromLParam(int lParam, out short x, out short y) {

            x = (short)(lParam & 0xFFFF);
            y = (short)(lParam >> 16);
        }
        #endregion
    }
}