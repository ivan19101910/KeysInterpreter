using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public abstract class Token
    {
        public override string ToString()
        {
            return "?";
        }
    }

    public class SymbolToken : Token
    {
        public string Symbol { get; set; }

        public SymbolToken(string symbol)
        {
            Symbol = symbol;
        }

        public override string ToString()
        {
            return Symbol.ToString();
        }
    }

    public class IdentifierToken : Token
    {
        public string Identifier { get; set; }

        public IdentifierToken(string identifier)
        {
            Identifier = identifier;
        }

        public override string ToString()
        {
            return Identifier;
        }
    }

    public class NumberToken : Token
    {
        public int Value { get; set; }

        public NumberToken(int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class StringToken : Token
    {
        public string Contents { get; set; }

        public StringToken(string contents)
        {
            Contents = contents;
        }

        public override string ToString()
        {
            return Contents;
        }
    }

    public class KeywordToken : Token
    {
        public string Contents { get; set; }

        public KeywordToken(string contents)
        {
            Contents = contents;
        }

        public override string ToString()
        {
            return Contents;
        }
    }
}
