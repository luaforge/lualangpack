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
      #region IScanner Members

      private Hashtable tokenInf = new Hashtable();
      private Lexer lexer = new tokens();
      private string srcBuf;

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

         inf = new TokenInfo();
         inf.Color = TokenColor.Text;
         inf.Type = TokenType.Operator;
         inf.Trigger = TokenTriggers.None;
         tokenInf.Add("AND", inf);
         tokenInf.Add("OR", inf);
         tokenInf.Add("NOT", inf);
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
         int yypos = lexer.yypos;
         
         TOKEN tok = lexer.Next();
         if (tok != null)
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
             }

             tokenInfo.StartIndex = tok.Position;
             tokenInfo.EndIndex = lexer.yypos - 1;
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
             return false;
         
         return true;
      }

      public void SetSource(string source, int offset)
      {
         lexer.Start(source);
         srcBuf = source;
         while (lexer.yypos < offset)
            lexer.Advance();
      }

      #endregion
   }
}
