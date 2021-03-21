using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Interpreter
{
    class ParseRules
    {
		public static bool IsValidNumber(string str)
		{
			Regex numRegex = new Regex(@"^\d");
			return numRegex.IsMatch(str);
		}

		public static bool IsValidIdentifier(string str)
		{
			if (str != "Repeat")
				return Regex.IsMatch(str, @"^[a-z A-Z]");
			else
				return false;
		}

		public static bool IsValidSymbol(string symbol)
		{
			return Regex.IsMatch(symbol, @"([ (),+=:{}])");
		}

		public static bool IsValidString(string str)
		{
			return Regex.IsMatch(str, "^\".*\"$");
		}

		public static bool IsValidKeyWord(string str)
		{
			if (str == "Repeat")
				return true;
			else
				return false;
			
		}

	}
}
