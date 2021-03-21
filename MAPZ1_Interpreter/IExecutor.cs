using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    interface IExecutor
    {
        void Execute(TreeNode Node);
    }
}
