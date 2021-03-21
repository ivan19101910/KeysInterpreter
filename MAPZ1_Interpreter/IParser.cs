using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    interface IParser
    {
        TreeNode Construct(Token[] tokens);
    }
}
