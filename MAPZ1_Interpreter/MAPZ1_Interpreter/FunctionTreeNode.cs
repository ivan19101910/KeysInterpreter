using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
	public class FunctionTreeNode : TreeNode
	{
		public string Name { get; set; }
		public TreeNode[] Parameters { get; set; }
		public override bool HasChildren => true;
		public override bool IsFinished => true;

		public FunctionTreeNode(string name, TreeNode[] parameters, TreeNode parent = null) : base(parent)
		{
			Name = name;
			Parameters = new TreeNode[parameters.Length];
			for (int i = 0; i < parameters.Length; ++i)
			{
				Parameters[i] = parameters[i];
				Parameters[i].Parent = this;
			}
		}

		public override TreeNode Clone(TreeNode parent = null)
		{
			var newParameters = new TreeNode[Parameters.Length];

			for (int i = 0; i < newParameters.Length; ++i)
			{
				newParameters[i] = Parameters[i].Clone(this);
			}
			return new FunctionTreeNode(Name, newParameters, parent);
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append(Name);
			builder.Append('(');
			if (Parameters.Length > 0) builder.Append(Parameters[0]);
			for (int i = 1; i < Parameters.Length; ++i)
			{
				builder.Append(", ");
				builder.Append(Parameters[i]);
			}
			builder.Append(")");
			return builder.ToString();
		}

		public override bool Equals(TreeNode other)
		{
			if (other is FunctionTreeNode ufNode)
			{
				if (Name != ufNode.Name) return false;
				if (Parameters.Length != ufNode.Parameters.Length) 
					return false;

				for (int i = 0; i < Parameters.Length; ++i)
				{
					if (Parameters[i] != ufNode.Parameters[i]) 
						return false;
				}
				
				return true;
			}
			else return false;
		}
	}
}
