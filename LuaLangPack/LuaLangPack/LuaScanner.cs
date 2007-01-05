using System;
using System.Collections;
using System.Text;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Tools;

namespace Vsip.LuaLangPack
{
    class LuaScanner : IScanner 
    {
        private Hashtable tokenInf = new Hashtable();
        private Lexer lexer = new tokens();
        private string srcBuf;
        private int nestBlock = 0;
        private int yypos = 0;
        private bool synch = false;
        private TOKEN m_lastTok = null;
        private enum ParseState
        {
            InText = 0,
            InComment = 1
        }

        public LuaScanner()
        {
            TokenInfo inf = new TokenInfo();
            inf.Color = TokenColor.String;
            inf.Type = TokenType.Literal;
            inf.Trigger = TokenTriggers.None;
            tokenInf.Add("LITERAL", inf);

            inf = new TokenInfo();
            inf.Color = TokenColor.Comment;
            inf.Type = TokenType.Comment;
            inf.Trigger = TokenTriggers.None;
            tokenInf.Add("COMMENT", inf);

            inf = new TokenInfo();
            inf.Color = TokenColor.Keyword;
            inf.Type = TokenType.Keyword;
            inf.Trigger = TokenTriggers.None;
            tokenInf.Add("FUNCTION", inf);
            tokenInf.Add("IF", inf);
            tokenInf.Add("THEN", inf);
            tokenInf.Add("ELSE", inf);
            tokenInf.Add("ELSEIF", inf);
            tokenInf.Add("END", inf);
            tokenInf.Add("FOR", inf);
            tokenInf.Add("IN", inf);
            tokenInf.Add("DO", inf);
            tokenInf.Add("WHILE", inf);
            tokenInf.Add("UNTIL", inf);
            tokenInf.Add("TRUE", inf);
            tokenInf.Add("FALSE", inf);
            tokenInf.Add("REPEAT", inf);
            tokenInf.Add("NIL", inf);
            tokenInf.Add("BREAK", inf);
            tokenInf.Add("RETURN", inf);
            tokenInf.Add("LOCAL", inf);
            tokenInf.Add("AND", inf);
            tokenInf.Add("OR", inf);
            tokenInf.Add("NOT", inf);

            inf = new TokenInfo();
            inf.Color = TokenColor.Text;
            inf.Type = TokenType.Operator;
            inf.Trigger = TokenTriggers.None;
            tokenInf.Add("POUND", inf);
            tokenInf.Add("MOD", inf);
            tokenInf.Add("PLUS", inf);
            tokenInf.Add("MINUS", inf);
            tokenInf.Add("DIVIDE", inf);
            tokenInf.Add("EXP", inf);
            tokenInf.Add("EQ", inf);
            tokenInf.Add("NEQ", inf);
            tokenInf.Add("LT", inf);
            tokenInf.Add("LE", inf);
            tokenInf.Add("GT", inf);
            tokenInf.Add("GE", inf);
            tokenInf.Add("ASSIGN", inf);
            tokenInf.Add("MULT", inf);
            tokenInf.Add("CONCAT", inf);
            tokenInf.Add("LPAREN", inf);
            tokenInf.Add("RPAREN", inf);
            tokenInf.Add("LBRACK", inf);
            tokenInf.Add("RBRACK", inf);
            tokenInf.Add("LBRACE", inf);
            tokenInf.Add("RBRACE", inf);

            inf = new TokenInfo();
            inf.Color = TokenColor.Text;
            inf.Type = TokenType.Operator;
            inf.Trigger = TokenTriggers.MemberSelect;
            tokenInf.Add("DOT", inf);
            tokenInf.Add("COLON", inf);
        }

        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {
            // Handle comments as a special case. We don't tokenize them in the lexer
            // since we don't want to send them to the parser.  
            TOKEN tok = null;
            if (state != (int)ParseState.InComment)
            {
                if (synch) // handles picking up after end of block comment
                {
                    synch = false;
                    while (lexer.yypos < yypos)
                        lexer.Advance();
                }
                
                yypos = lexer.yypos;
                tok = lexer.Next();
            }
            
            if (state == (int)ParseState.InComment)
            {
                if (yypos >= srcBuf.Length)
                {
                    yypos = 0;
                    return false;
                }

                TokenInfo inf = (TokenInfo)tokenInf["COMMENT"];
                tokenInfo.Color = inf.Color;
                tokenInfo.Type = inf.Type;
                tokenInfo.Trigger = inf.Trigger;
                tokenInfo.StartIndex = yypos;

                // Handle nested comment blocks
                int len = srcBuf.Length;
                char[] cs = new char[len];
                cs = srcBuf.ToCharArray();
                int i = yypos;
                for (; (i+1) < len; ++i)
                {
                    if (cs[i] == '[' && cs[i + 1] == '[')
                    {
                        ++i; ++nestBlock;
                    }
                    else if (cs[i] == ']' && cs[i + 1] == ']')
                    {
                        ++i; --nestBlock;
                    }

                    if (nestBlock < 0)
                        break;
                }

                if (nestBlock < 0)
                {
                    tokenInfo.EndIndex = i;
                    state = (int)ParseState.InText;
                    nestBlock = 0;
                }
                else
                    tokenInfo.EndIndex = srcBuf.Length;

                // We always scan the whole line in this case so safe to always
                // return false indecating there are no more tokens on this line.
                yypos = tokenInfo.EndIndex;
                synch = true;
                return true;
            }
            else if (tok != null)
            {
                if (tokenInf.Contains(tok.yyname))
                {
                    TokenInfo inf = (TokenInfo)tokenInf[tok.yyname];
                    tokenInfo.Color = inf.Color;
                    tokenInfo.Type = inf.Type;
                    tokenInfo.Trigger = inf.Trigger;
                }
                else
                {
                    tokenInfo.Color = TokenColor.Text;
                    tokenInfo.Type = TokenType.Unknown;
                    tokenInfo.Trigger = TokenTriggers.None;

                    if (tok.yytext.Length == 1)
                    {
                        if (tokenInf.Contains(m_lastTok.yyname))
                        {
                            if (((TokenInfo)tokenInf[m_lastTok.yyname]).Trigger != TokenTriggers.MemberSelect)
                                tokenInfo.Trigger = TokenTriggers.MemberSelect;
                        }
                        else
                            tokenInfo.Trigger = TokenTriggers.MemberSelect;
                    }
                }

                tokenInfo.StartIndex = tok.Position;
                tokenInfo.EndIndex = lexer.yypos - 1;
                m_lastTok = tok;
            }
            else if (yypos < srcBuf.Length && srcBuf.Contains("--[["))
            {
                tokenInfo.StartIndex = srcBuf.IndexOf("--[[", yypos);
                yypos += 5;
                tokenInfo.EndIndex = yypos + 4;
                TokenInfo inf = (TokenInfo)tokenInf["COMMENT"];
                tokenInfo.Color = inf.Color;
                tokenInfo.Type = inf.Type;
                tokenInfo.Trigger = inf.Trigger;
                state = (int)ParseState.InComment; 
            }
            else if (yypos < srcBuf.Length && srcBuf.Contains("--"))
            {
                tokenInfo.StartIndex = srcBuf.IndexOf("--", yypos);
                tokenInfo.EndIndex = srcBuf.Length;
                TokenInfo inf = (TokenInfo)tokenInf["COMMENT"];
                tokenInfo.Color = inf.Color;
                tokenInfo.Type = inf.Type;
                tokenInfo.Trigger = inf.Trigger;
            }
            else
            {
                yypos = 0;
                return false;
            }

            return true;
        }

        public void SetSource(string source, int offset)
        {
            srcBuf = source;
            lexer.Start(source);
            while (lexer.yypos < offset)
                lexer.Advance();
        }
    }
}
