using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    class StringTreeNode : TreeNode
    {
		public string Content { get; set; }
		public override bool HasChildren => false;
		public override bool IsFinished => true;

		public StringTreeNode(string content, TreeNode parent = null) : base(parent)
		{
			Content = content;
		}

		public override TreeNode Clone(TreeNode parent = null)
		{
			return new StringTreeNode(Content, parent);
		}

		public override string ToString()
		{
			return Content;
		}

		public override bool Equals(TreeNode other)
		{
			return (other is StringTreeNode nNode)
				&& (Content == nNode.Content);
		}
	}
}
