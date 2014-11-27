using System.Collections.Generic;

namespace Ruminate {

    internal interface ITreeNode<T> where T : class {
        TreeNode<T> TreeNode { get; }
    }

    internal delegate void NodeOperation<T>(TreeNode<T> node) where T : class;

    internal class Root<T> : TreeNode<T> where T : class {

        public NodeOperation<T> OnAttachedToRoot { get; set; }
        public NodeOperation<T> OnChildrenChanged { get; set; }

        public Root() : base(null) { Root = this; }
    }

    internal class TreeNode<T> where T : class {

        public T Data { get; set; }

        public Root<T> Root { get; set; }

        public bool IsRoot { get { return Root == this; } }
        public bool Attached { get { return (Root != null); } }

        public TreeNode<T> Parent { get; set; }
        public List<TreeNode<T>> Children { get; set; }

        public TreeNode(T data) {

            Data = data;
            Children = new List<TreeNode<T>>();
        }

        /*####################################################################*/
        /*                        Add/Remove Children                         */
        /*####################################################################*/

        // Add/Remove via Data
        public void AddChild(ITreeNode<T> data) {

            AddChild(data.TreeNode);

            if (IsRoot) {
                DfsOperationChildren(node => Root.OnChildrenChanged(node));
            } else if (Attached) {
                DfsOperation(node => Root.OnChildrenChanged(node));
            }
        }

        public void AddChildren(IEnumerable<ITreeNode<T>> children) {
            
            foreach (var child in children) {
                AddChild(child.TreeNode);
            }

            if (IsRoot) {
                DfsOperationChildren(node => Root.OnChildrenChanged(node));
            } else if (Attached) {
                DfsOperation(node => Root.OnChildrenChanged(node));
            }
        }

        public void RemoveChild(ITreeNode<T> data) {

            foreach (var treeNode in Children) {
                if (!treeNode.Data.Equals(data)) { continue; }
                RemoveChild(treeNode);
                return;
            }

            if (IsRoot) {
                DfsOperationChildren(node => Root.OnChildrenChanged(node));
            } else if (Attached) {
                DfsOperation(node => Root.OnChildrenChanged(node));
            }
        }

        // Add/Remove via TreeNode
        public void AddChild(TreeNode<T> child) {

            child.Parent = this;            
            Children.Add(child);

            if (Root == null) { return;}

            child.Root = Root;
            Root.OnAttachedToRoot(this);
        }

        public void AddChildren(IEnumerable<TreeNode<T>> children) {

            foreach (var child in children) {
                AddChild(child);
            }

            if (IsRoot) {
                DfsOperationChildren(node => Root.OnChildrenChanged(node));
            } else if (Attached) {
                DfsOperation(node => Root.OnChildrenChanged(node));
            }
        }

        public void RemoveChild(TreeNode<T> child) {

            if (child.Parent != this || !Children.Contains(child)) { return; }

            child.Parent = null;
            Children.Remove(child);
        }

        /*####################################################################*/
        /*                           DFS Operations                           */
        /*####################################################################*/

        public void DfsOperationChildren(NodeOperation<T> operation) {

            foreach (var child in Children) {
                child.DfsOperation(operation);
            }
        }

        public void DfsOperation(NodeOperation<T> operation) {

            operation(this);
            foreach (var child in Children) {
                child.DfsOperation(operation);
            }
        }
    }
}
