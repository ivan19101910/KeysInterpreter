using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
	public abstract class TreeNode
	{
		public TreeNode Parent { get; set; }

		public abstract bool HasChildren { get; }
		public abstract bool IsFinished { get; }

		public TreeNode(TreeNode parent = null)
		{
			Parent = parent;
		}

		public abstract TreeNode Clone(TreeNode parent = null);
		public abstract bool Equals(TreeNode otherNode);

	}
}
