using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Ruminate.GUI.Framework;

namespace Ruminate.GUI.Content {

    public sealed class RadioButton : Widget {

        private enum RenderMode { Default, Hover, Pressed }
        private RenderMode _renderMode { get; set; }

        /*####################################################################*/
        /*                               Grouping                             */
        /*####################################################################*/
        #region Grouping
        //TODO: Don't like using statics, I would rather have them in a container.
        private static readonly Dictionary<string, List<RadioButton>> Groups = 
            new Dictionary<string, List<RadioButton>>();

        private static void AddRadioButton(string group, RadioButton button) {

            if (!Groups.ContainsKey(group)) {
                Groups.Add(group, new List<RadioButton>());
            }

            Groups[group].Add(button);
        }
        #endregion

        /*####################################################################*/
        /*                               Variables                            */
        /*####################################################################*/

        /* ## Local Variables ## */
        private bool _innerIsChecked;
        private readonly string _group;

        /* ## Exposed Variables ## */
        public WidgetEvent OnCheck { get; set; }
        public WidgetEvent OnUnCheck { get; set; }

        public string Label { get; set; }
        
        public bool IsChecked 
        {
            get 
            {
                return _innerIsChecked;
            }
            private set 
            {

                if (!value) {
                    throw new ArgumentException("IsChecked can only be set to true");
                }

                _innerIsChecked = true;
                if (OnCheck != null) { OnCheck(this); }

                foreach (var radioButton in Groups[_group]) {
                    if (radioButton == this) { continue; }

                    radioButton._innerIsChecked = false;
                    if (radioButton.OnUnCheck != null) {
                        radioButton.OnUnCheck(radioButton);                        
                    }

                    _renderMode = RenderMode.Default;
                }
            }
        }

        /*####################################################################*/
        /*                           Initialization                           */
        /*####################################################################*/

        public RadioButton(int x, int y, string group, string label) {

            _renderMode = RenderMode.Default;

            Area = new Rectangle(x, y, 0, 0);

            Label = label;
            _innerIsChecked = false;

            _group = group;
            AddRadioButton(group, this);
        }

        protected internal override void Update() {
            
        }

        internal override void Draw() {
        }

        /*####################################################################*/
        /*                                Events                              */
        /*####################################################################*/

        protected internal override void MouseClick(MouseEventArgs e) 
        {
            IsChecked = true;
        }

        protected internal override void EnterPressed()
        {
            _renderMode = RenderMode.Pressed;
        }

        public override Rectangle Area { get; protected set; }
        public override Rectangle SafeArea { get; protected set; }

        protected internal override void EnterHover()
        {
            if (_renderMode != RenderMode.Pressed)
            {
                _renderMode = RenderMode.Hover;
            }
        }

        protected internal override void ExitHover() {
            if (_renderMode != RenderMode.Pressed)
            {
                _renderMode = RenderMode.Default;
            }
        }
    }
}