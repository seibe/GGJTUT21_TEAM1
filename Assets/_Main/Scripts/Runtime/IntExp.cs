#nullable enable
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Game
{
    public sealed class IntExp
    {
        List<char> m_Source;

        public IntExp()
        {
            m_Source = new List<char>();
        }

        public IntExp(string exp)
        {
            exp = Regex.Replace(exp, @"\s", "");
            m_Source = new List<char>(exp.ToCharArray());
        }

        public int Count => m_Source.Count;

        public int Calculate()
        {
            var tokens = ExpToTokens(m_Source);
            var rpn = ConvertTokenToRpn(tokens);
            var ret = rpn.Calculate();
#if DEBUG
            UnityEngine.Debug.Log($"{nameof(IntExp)}: {string.Join(" ", m_Source)} => {rpn} => {ret}\n");
#endif //DEBUG
            return ret;
        }

        public bool TryCalculate(out int result)
        {
            try
            {
                result = Calculate();
                return true;
            }
            catch (System.ArithmeticException)
            {
                result = default;
                return false;
            }
        }

        public void Push(in char c)
        {
            m_Source.Add(c);
        }

        public void Unshift(in char c)
        {
            m_Source.Insert(0, c);
        }

        private static List<Token> ExpToTokens(List<char> exp)
        {
            var ret = new List<Token>();
            var builder = new StringBuilder();

            foreach (var c in exp)
            {
                switch (c)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    {
                        builder.Append(c);
                    }
                    break;

                    default:
                    {
                        if (builder.Length > 0)
                        {
                            var value = (builder.Length == 1)
                                ? builder[0] - '0'
                                : int.Parse(builder.ToString());
                            ret.Add(new Token(value));
                            builder.Clear();
                        }

                        switch (c)
                        {
                            case '+':
                                ret.Add(Token.Plus);
                                break;

                            case '-':
                                ret.Add(Token.Minus);
                                break;

                            case '*':
                                ret.Add(Token.Mul);
                                break;

                            case '/':
                                ret.Add(Token.Div);
                                break;

                            case '(':
                                ret.Add(Token.ParenL);
                                break;

                            case ')':
                                ret.Add(Token.ParenR);
                                break;
                        }
                    }
                    break;
                }
            }

            if (builder.Length > 0)
            {
                var value = (builder.Length == 1)
                    ? builder[0] - '0'
                    : int.Parse(builder.ToString());
                ret.Add(new Token(value));
                builder.Clear();
            }

            ret.TrimExcess();
            return ret;
        }

        private static Rpn ConvertTokenToRpn(List<Token> tokens)
        {
            var ret = new Rpn();
            var it = tokens.GetEnumerator();
            it.MoveNext();
            ParseExpr(ref it, ret);
            return ret;

            /*
             * 四則演算 (BNF記法)
             * <expr>   ::= <term> [ ('+'|'-') <term> ]*
             * <term>   ::= <factor> [ ('*'|'/') <factor> ]*
             * <factor> ::= <number> | '(' <expr> ')'
             */

            static void ParseExpr(ref List<Token>.Enumerator it, Rpn rpn)
            {
                ParseTerm(ref it, rpn);

                var token = it.Current;
                while (token == Token.Plus || token == Token.Minus)
                {
                    it.MoveNext();
                    ParseTerm(ref it, rpn);
                    rpn.Add(token);
                    token = it.Current;
                }
            }

            static void ParseTerm(ref List<Token>.Enumerator it, Rpn rpn)
            {
                ParseFactor(ref it, rpn);

                var token = it.Current;
                while (token == Token.Mul || token == Token.Div)
                {
                    it.MoveNext();
                    ParseFactor(ref it, rpn);
                    rpn.Add(token);
                    token = it.Current;
                }
            }

            static void ParseFactor(ref List<Token>.Enumerator it, Rpn rpn)
            {
                var token = it.Current;
                if (token == Token.ParenL)
                {
                    it.MoveNext();
                    ParseExpr(ref it, rpn);
                    it.MoveNext();
                }
                else
                {
                    ParseNumber(ref it, rpn);
                }
            }

            static void ParseNumber(ref List<Token>.Enumerator it, Rpn rpn)
            {
                var token = it.Current;
                var sign = 1;
                if (token == Token.Plus)
                {
                    it.MoveNext();
                    token = it.Current;
                }
                else if (token == Token.Minus)
                {
                    sign = -1;
                    it.MoveNext();
                    token = it.Current;
                }
                if (token.IsSymbol)
                {
                    throw new System.ArithmeticException();
                }
                rpn.Add(new Token(token.Value * sign));
                it.MoveNext();
            }
        }

        sealed class Rpn
        {
            private readonly List<Token> m_TokenList;
            private readonly Stack<int> m_Stack;

            public Rpn()
            {
                m_TokenList = new List<Token>();
                m_Stack = new Stack<int>();
            }

            public void Add(Token token)
                => m_TokenList.Add(token);

            public int Calculate()
            {
                m_Stack.Clear();
                foreach (var token in m_TokenList)
                {
                    if (token.IsSymbol)
                    {
                        if (token == Token.Plus)
                        {
                            var b = m_Stack.Pop();
                            var a = m_Stack.Pop();
                            m_Stack.Push(a + b);
                        }
                        else if (token == Token.Minus)
                        {
                            var b = m_Stack.Pop();
                            var a = m_Stack.Pop();
                            m_Stack.Push(a - b);
                        }
                        else if (token == Token.Mul)
                        {
                            var b = m_Stack.Pop();
                            var a = m_Stack.Pop();
                            m_Stack.Push(a * b);
                        }
                        else if (token == Token.Div)
                        {
                            var b = m_Stack.Pop();
                            var a = m_Stack.Pop();
                            m_Stack.Push(a / b);
                        }
                    }
                    else
                    {
                        m_Stack.Push(token.Value);
                    }
                }
                return m_Stack.Pop();
            }

            public override string ToString()
                => string.Join(" ", m_TokenList);
        }

        readonly struct Token : System.IEquatable<Token>
        {
            public static readonly Token Plus = new Token(true, 0);
            public static readonly Token Minus = new Token(true, 1);
            public static readonly Token Mul = new Token(true, 2);
            public static readonly Token Div = new Token(true, 3);
            public static readonly Token ParenL = new Token(true, 4);
            public static readonly Token ParenR = new Token(true, 5);

            public readonly int Value;
            public readonly bool IsSymbol;

            public Token(in bool isSymbol, in int value)
            {
                IsSymbol = isSymbol;
                Value = value;
            }

            public Token(in int value)
            {
                IsSymbol = false;
                Value = value;
            }

            public readonly bool Equals(Token other)
                => IsSymbol == other.IsSymbol
                && Value == other.Value;

            public readonly override bool Equals(object obj)
                => (obj is Token other) && Equals(other);

            public readonly override int GetHashCode()
            {
                int hashCode = 1066518186;
                hashCode = hashCode * -1521134295 + Value.GetHashCode();
                hashCode = hashCode * -1521134295 + IsSymbol.GetHashCode();
                return hashCode;
            }

            public readonly override string ToString()
            {
                if (IsSymbol)
                {
                    return Value switch
                    {
                        0 => "+",
                        1 => "-",
                        2 => "*",
                        3 => "/",
                        4 => "(",
                        5 => ")",
                        _ => throw new System.NotImplementedException(),
                    };
                }
                return Value.ToString();
            }

            public static bool operator ==(Token left, Token right)
                => left.Equals(right);

            public static bool operator !=(Token left, Token right)
                => !left.Equals(right);
        }
    }
}
