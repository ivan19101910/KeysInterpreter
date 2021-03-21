using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAPZ1_Interpreter;

namespace Interpreter
{
    class Parser
    {
		public class ConstructionContext
		{
			public Token[] tokens;
			public int currentToken;

			public bool TryGetNextToken(out Token token)
			{
				if (currentToken > tokens.Length)
				{
					token = null;
					return false;
				}

				token = tokens[currentToken - 1];
				++currentToken;
				return true;
			}

			public bool TryPeekNextToken(out Token token)
			{
				if (currentToken > tokens.Length)
				{
					token = null;
					return false;
				}

				token = tokens[currentToken - 1];
				return true;
			}

			public void PutBackToken()
			{
				if (currentToken <= 0)
				{
					throw new Exception("No tokens to put back");
				}

				--currentToken;
			}

			public void EatLastToken()
			{
				if (currentToken > tokens.Length)
				{
					throw new Exception("No tokens to eat");
				}

				++currentToken;
			}

			public ConstructionContext(Token[] tokens)
			{
				this.tokens = (Token[])tokens.Clone();
				this.currentToken = 1;
			}
		}		

		public TreeNode Construct(Token[] tokens)
		{
			ConstructionContext context = new ConstructionContext(tokens);
			return BuildTree(context);
		}

		public TreeNode BuildTree(ConstructionContext context)
		{
			List<TreeNode> parameters = new List<TreeNode>();
			List<TreeNode> expressions = new List<TreeNode>();
			var statements = new List<TreeNode>();
			FunctionTreeNode func;
			string name = "Undefined";
			string expressionName = "Undefined";

			for (int i = 0; i < context.tokens.Length; ++i)
			{
				context.TryGetNextToken(out Token token);
				switch (token)
				{
					case IdentifierToken identifierToken://maybe add check for defined functions
						name = identifierToken.ToString();//if we here, its function
						break;

					case NumberToken numberToken:
						parameters.Add(new NumberTreeNode(int.Parse(numberToken.ToString())));
						break;

					case StringToken stringToken:
						parameters.Add(new StringTreeNode(stringToken.ToString()));
						break;

					case SymbolToken symbolToken:
						break;

					case KeywordToken keywordToken:
						name = keywordToken.ToString();

						if (context.TryGetNextToken(out Token tok))
						{
							if (tok.ToString() == "(")
							{
								for(; ; )//read arguments
								{
									context.TryGetNextToken(out Token token1);
									if (token1 is NumberToken)
									{
										parameters.Add(new NumberTreeNode(int.Parse(token1.ToString())));
									}
									else if(token1.ToString() == ",")
									{

									}
									else if (token1.ToString() == ")")
									{
										break;
									}
									else
										throw new ArgumentException($"Unexpected parameter {token1.ToString()}");
									
								}
							}
							context.TryGetNextToken(out Token tok2);
							if (tok2 == null)
							{
								throw new ArgumentException($"Must be open and close brackets in \"{name}\" cycle");
							}
							else if (tok2.ToString() == "{")
							{
								for (; ; )//read expressions
								{
									if (context.TryGetNextToken(out Token token1))
									{
										switch (token1)
										{
											case IdentifierToken identifierToken:
												
												expressionName = identifierToken.ToString();
												break;

											case NumberToken numberToken:
												expressions.Add(new NumberTreeNode(int.Parse(numberToken.ToString())));
												break;

											case StringToken stringToken:
												expressions.Add(new StringTreeNode(stringToken.ToString()));
												break;

											case SymbolToken symbolToken:
												if (symbolToken.ToString() == ")")
												{
													statements.Add(new FunctionTreeNode(expressionName, expressions.ToArray()));
													expressions.Clear();
												}
												break;
										}
									}
									if(token1 == null)
									{
										//throw new ArgumentException($"Unexpected token \"{token1}\"");
										throw new ArgumentException($"Need close brace!");
									}
									if (token1.ToString() == "}")
									{
										return new CycleTreeNode(name, statements.ToArray(), parameters.ToArray());
									}
									//else
										//throw new ArgumentException($"Unexpected expression in cycle {token1.ToString()}");

								}
							}
													
							else
							{
								throw new ArgumentException($"Unexpected expression{tok2.ToString()}");
							}

						}
						else
						{
							throw new ArgumentException($"Cycle \"{name}\" must be have open and close braces");//when we have Repeat without brackets
						} 

					default:
						throw new ArgumentException($"Something wrong in your code {token}");
				}
				
			}

			func = new FunctionTreeNode(name, parameters.ToArray());
			return func;
		}
	}
}

