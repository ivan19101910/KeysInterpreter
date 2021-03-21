using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interpreter;

namespace MAPZ1_Interpreter
{
    public class CycleTreeNode : TreeNode
    {
		public string Name { get; set; }
		public TreeNode[] Expressions { get; set; }
		public TreeNode[] Arguments { get; set; }
		public override bool HasChildren => true;
		public override bool IsFinished => false;

		public CycleTreeNode(string name, TreeNode[] expressions,TreeNode[] arguments, TreeNode parent = null) : base(parent)
		{
			Name = name;
			Expressions = new TreeNode[expressions.Length];
			Arguments = new TreeNode[arguments.Length];

			for (int i = 0; i < expressions.Length; ++i)
			{
				Expressions[i] = expressions[i];
				Expressions[i].Parent = this;				
			}

			for(int i = 0; i < arguments.Length; ++i)
			{
				Arguments[i] = arguments[i];
				Arguments[i].Parent = this;
			}
		}

		public override TreeNode Clone(TreeNode parent = null)
		{
			var newParameters = new TreeNode[Expressions.Length];

			for (int i = 0; i < newParameters.Length; ++i)
			{
				newParameters[i] = Expressions[i].Clone(this);
			}
			return new FunctionTreeNode(Name, newParameters, parent);
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append(Name);
			builder.Append('(');
			if (Expressions.Length > 0) builder.Append(Expressions[0]);
			for (int i = 1; i < Expressions.Length; ++i)
			{
				builder.Append(", ");
				builder.Append(Expressions[i]);
			}
			builder.Append(")");
			return builder.ToString();
		}

		public override bool Equals(TreeNode other)
		{
			if (other is FunctionTreeNode ufNode)
			{
				if (Name != ufNode.Name) return false;
				if (Expressions.Length != ufNode.Parameters.Length) return false;

				for (int i = 0; i < Expressions.Length; ++i)
				{
					if (Expressions[i] != ufNode.Parameters[i]) return false;
				}

				return true;
			}
			else return false;
		}
	}
}

