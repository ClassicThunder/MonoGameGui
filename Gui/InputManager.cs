using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Ruminate.DataStructures;

namespace Ruminate.GUI.Framework {

    // Handles/triggers all of the mouse or keyboard events. State management occurs 
    // asynchronously while triggers are handled synchronously.
    internal class InputManager {

        private readonly Root<Widget> _dom;

        // When ever these Widgets are not not null they are in the state
        // specified by their names. Used to trigger events and by the widget
        // class to see what state the widget is in. 
        private Widget _hoverWidget;
        internal Widget HoverWidget {
            get {
                return _hoverWidget;
            } private set {
                if (value == _hoverWidget) { return; }
                if (value != null) { value.EnterHover(); } 
                if (_hoverWidget != null) { _hoverWidget.ExitHover(); }
                _hoverWidget = value;
            }
        }

        private Widget _pressedWidget;
        internal Widget PressedWidget {
            get {
                return _pressedWidget;
            } private set {
                if (value == _pressedWidget) { return; }
                if (value != null) { value.EnterPressed(); }
                if (_pressedWidget != null) { _pressedWidget.ExitPressed(); }
                _pressedWidget = value;
            }
        }

        private Widget _focusedWidget;
        internal Widget FocusedWidget {
            get {
                return _focusedWidget;
            } private set {
                if (value == _focusedWidget) { return; }
                if (value != null) { value.EnterFocus(); }
                if (_focusedWidget != null) { _focusedWidget.ExitFocus(); }
                _focusedWidget = value;
            }
        }        

        // Ties into the events in InputSystem. Top events are asynchronous and 
        // used for state management. The rest of them are synchronous and used to
        // trigger user specified event handlers.
        internal InputManager(Game game, Root<Widget> dom) {

            _dom = dom;

            InputSystem.Initialize(game.Window);

            /* ## Input Events to Manage Internal State ## */
            #region Manage Internal State

            InputSystem.MouseMove += delegate {

                if (FocusedWidget != null && FocusedWidget.AbsoluteInputArea.Contains(InputSystem.MouseLocation) && FocusedWidget.BlocksInput) {
                    return;
                }

                HoverWidget = FindHover();
                if (PressedWidget == null) {
                    return;
                }

                if (!PressedWidget.AbsoluteInputArea.Contains(InputSystem.MouseLocation)) {
                    PressedWidget = null;
                }
            };

            InputSystem.MouseUp += delegate(object sender, MouseEventArgs e) {

                if (e.Button != MouseButton.Left) { return; }

                if (PressedWidget != null) {
                    PressedWidget.MouseClick(e);
                }

                PressedWidget = null;
            };

            InputSystem.MouseDown += delegate(Object o, MouseEventArgs e) {

                if (e.Button != MouseButton.Left) { return; }

                if (HoverWidget == null && FocusedWidget != null && !FocusedWidget.BlocksInput) { return; }

                FocusedWidget = HoverWidget;
                PressedWidget = HoverWidget;
            };

            InputSystem.MouseDoubleClick += delegate(Object o, MouseEventArgs e) {

                if (e.Button != MouseButton.Left) { return; }

                if (HoverWidget == null) { return; }

                FocusedWidget = HoverWidget;
                PressedWidget = HoverWidget;
            };
            #endregion

            /* ## Input Events to fire the focused widget's event handlers ## */
            #region Widget Triggers            

            InputSystem.CharEntered += delegate(Object o, CharacterEventArgs e) {
                if (FocusedWidget != null) {
                    FocusedWidget.CharEntered(e);
                }
            };

            InputSystem.KeyDown += delegate(Object o, KeyEventArgs e) {
                if (FocusedWidget != null) {
                    FocusedWidget.KeyDown(e);
                }
            };

            InputSystem.KeyUp += delegate(Object o, KeyEventArgs e) {
                if (FocusedWidget != null) {
                    FocusedWidget.KeyUp(e);
                }
            };

            InputSystem.MouseDoubleClick += delegate(Object o, MouseEventArgs e) {
                if (FocusedWidget != null) {
                    FocusedWidget.MouseDoubleClick(e);
                }
            };

            InputSystem.MouseDown += delegate(Object o, MouseEventArgs e) {
                if (FocusedWidget != null) {
                    FocusedWidget.MouseDown(e);
                }
            };

            InputSystem.MouseMove += delegate(Object o, MouseEventArgs e) {
                if (FocusedWidget != null) {
                    FocusedWidget.MouseMove(e);
                }
            };

            InputSystem.MouseUp += delegate(Object o, MouseEventArgs e) {
                if (FocusedWidget != null) {
                    FocusedWidget.MouseUp(e);
                }                
            };

            InputSystem.MouseWheel += delegate(Object o, MouseEventArgs e) {
                if (FocusedWidget != null) {
                    FocusedWidget.MouseWheel(e);
                }
            };

            #endregion
        }

        // Finds the element the mouse is currently being hovered over.
        Widget _hover;

        private Widget FindHover() {
            _hover = null;
            foreach (var child in _dom.Children) {
                DfsFindHover(child);
            }
            return _hover;
        }

        private void DfsFindHover(TreeNode<Widget> node) {

            if (!node.Data.AbsoluteArea.Contains(InputSystem.MouseLocation)) {
                return;
            }

            if (node.Parent.Data != null && !node.Parent.Data.AbsoluteInputArea.Contains(InputSystem.MouseLocation)) {
                return;
            }

            if (!node.Data.Active || !node.Data.Visible) {
                return;
            }

            _hover = node.Data;            
           
            foreach (var child in node.Children) {
                DfsFindHover(child);
            }
        }
    }
}