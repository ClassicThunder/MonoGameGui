using System.Collections.Generic;
using System.Linq;

namespace Ruminate.GUI.Framework {

    internal interface IWidgetComponent {
        WidgetBase Widget { get; }
    }

    internal class WidgetBase : IInputData {

        /*####################################################################*/
        /*                         TreeNode Component                         */
        /*####################################################################*/

        public TreeNode<WidgetBase> TreeNode { get; set; }

        public WidgetBase Parent {
            get {
                return TreeNode.Parent.Data;
            }
        }

        public IEnumerable<WidgetBase> Children {
            get {
                return from node in TreeNode.Children select node.Data;
            }
            set {
                TreeNode.Children.Clear();
                AddWidgets(value);
            }
        }


        public void AddWidget(WidgetBase widget) {
            TreeNode.AddChild(widget.TreeNode);
        }

        public void AddWidgets(IEnumerable<WidgetBase> widgets) {
            TreeNode.AddChildren(widgets.Select(node => node.TreeNode));
        }

        public void RemoveWidget(WidgetBase widget) {
            TreeNode.RemoveChild(widget.TreeNode);
        }

        public InputData InputData { get; set; }

        /*####################################################################*/
        /*                         Layout Components                          */
        /*####################################################################*/

        public LayoutData AbsoluteLayoutData { get; set; }
        public LayoutData RelativeLayoutData { get; set; }
    }
}
