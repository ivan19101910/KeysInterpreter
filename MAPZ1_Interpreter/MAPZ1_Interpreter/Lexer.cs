using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Interpreter
{
    class Lexer : ILexer
    {
        public Token[] Tokenize(string str)
        {
			if (String.IsNullOrEmpty(str))
				throw new InvalidOperationException($"String is empty");

			List<Token> tokens = new List<Token>();
			List<string> words;

			words = Regex.Split(str, @"([ (),+=:{}])").Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
			for(int i = 0; i < words.Count; ++i)
			{
				tokens.Add(GetNextToken(words[i]));
			}

			return tokens.ToArray();

		}

		public Token GetNextToken(string str)
		{
			if (ParseRules.IsValidKeyWord(str))
			{
				return new KeywordToken(str);
			}
			if (ParseRules.IsValidString(str))
			{
				return new StringToken(str);
			}
			if (ParseRules.IsValidIdentifier(str))
			{
				return new IdentifierToken(str);
			}
			if (ParseRules.IsValidNumber(str))
			{
				return new NumberToken(int.Parse(str));
			}
			if (ParseRules.IsValidSymbol(str))
			{
				return new SymbolToken(str);
			}			

			throw new ArgumentException($"Unexpected token: {str}");
		}
	}
    
}
