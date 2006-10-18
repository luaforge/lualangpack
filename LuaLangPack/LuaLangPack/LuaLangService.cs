using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using System.Runtime.InteropServices;
using Tools; 

public class LuaTable
{
   public string name;
   public LinkedList<object> vals = new LinkedList<object>();
   public Hashtable tables = new Hashtable();
}

public class LuaScope
{
   public LuaScope( LuaScope parent ) {
      m_parent = parent;
      beginLine = -1; endLine = -1; beginIndx = -1; endIndx = -1;
   }

   // Searches through current and parent scopes for given table (Lua has lexical scoping)
   public LuaTable Lookup(object name)
   {
      if (tables.Contains(name))
         return ((LuaTable)tables[name]);
      else
         return( m_parent.Lookup(name) );
   }

   public void AddRegions(AuthoringSink sink)
   {
      if (beginLine != endLine)
      {
         TextSpan span = new TextSpan();
         span.iStartLine = beginLine - 1;
         span.iEndLine = endLine - 1;
         span.iEndIndex = endIndx - 1;
         span.iStartIndex = beginIndx;
         sink.AddHiddenRegion(span);
      }
            
      foreach( LuaScope scope in nested )
      {
         scope.AddRegions(sink);
      }
   }

   public int beginLine;
   public int endLine;
   public int beginIndx;
   public int endIndx;
   public LinkedList<LuaScope> nested = new LinkedList<LuaScope>();
   public Hashtable tables = new Hashtable();

   private LuaScope m_parent;
} 

namespace Vsip.LuaLangPack 
{
   [Guid("306766fb-5a6c-3d49-a65c-531c5da476a4")]
   class LuaLangService : LanguageService
   {
      private LuaScanner m_scanner;
      private LuaAuthScope m_authScope = new LuaAuthScope();
      private LuaSource m_source;

      public override Source CreateSource(IVsTextLines buffer)
      {
         m_source = new LuaSource(this, buffer, GetColorizer(buffer));
         return (m_source);
      }

      public override LanguagePreferences GetLanguagePreferences()
      {
         LanguagePreferences langPref = new LanguagePreferences();
         langPref.EnableCodeSense = true;
         langPref.EnableAsyncCompletion = true;
         langPref.AutoOutlining = true;
         langPref.EnableCommenting = true;
         langPref.MaxRegionTime = 10000;
         return langPref;
      }

      public override IScanner GetScanner(IVsTextLines buffer)
      {
         if( m_scanner == null )
            m_scanner = new LuaScanner();

         return m_scanner;
      }

      public override string Name
      {
         get { return "VS 2005 Lua Language Service"; }
      }

      public override AuthoringScope ParseSource(ParseRequest req)
      {
         if (req.Reason == ParseReason.DisplayMemberList)
         {
            
         }
         else if (req.Reason == ParseReason.Check) // parse all code passed to us
         {
            LuaScope scope = new LuaScope( null );
            Parser p = new syntax();
            SYMBOL ast;
            ast = p.Parse(req.Text);

            if (ast == null)
                return m_authScope;

            if (ast.yyname == "error")
            {
                Console.Write("Parse Error: " + ast.Pos);
            }
            else if (ast.yyname == "chunk")
            {
                chunk node = (chunk)ast;
                node.FillScope(scope);
                scope.AddRegions(req.Sink);
                req.Sink.ProcessHiddenRegions = true;
            }
         }

         return m_authScope;
      }

      internal class LuaAuthScope : AuthoringScope
      {
         public override string GetDataTipText(int line, int col, out TextSpan span)
         {
            span = new TextSpan();
            return null;
         }

         public override Declarations GetDeclarations(IVsTextView view, int line, int col, TokenInfo info, ParseReason reason)
         {
            if (reason == ParseReason.DisplayMemberList)
            {
               return null;
            }
            else
            {
               return null;
            }
         }

         public override Methods GetMethods(int line, int col, string name)
         {
            return null;
         }

         public override string Goto(VSConstants.VSStd97CmdID cmd, IVsTextView textView, int line, int col, out TextSpan span)
         {
            span = new TextSpan();
            return null;
         }

         internal class LuaDeclarations : Declarations
         {
            public override int GetCount()
            {
               return 0;
            }

            public override string GetDescription(int index)
            {
               return "";   
            }

            public override string GetDisplayText(int index)
            {
               return ""; 
            }

            public override int GetGlyph(int index)
            {
               return 0; 
            }

            public override string GetName(int index)
            {
               return ""; 
            }
         }
      }
   }
}
