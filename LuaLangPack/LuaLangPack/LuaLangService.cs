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


namespace Vsip.LuaLangPack 
{
    [Guid("306766fb-5a6c-3d49-a65c-531c5da476a4")]
    class LuaLangService : LanguageService
    {
        private const int VARIABLE = 138;
        private const int METHOD = 72;
        private const int NAMESPACE = 90;
        private LuaScanner m_scanner;
        private LuaAuthScope m_authScope = new LuaAuthScope();
        private LuaSource m_source;
        private LuaScope m_globalScope = new LuaScope(null);
        private Hashtable m_fileScopes = new Hashtable();

        public override Source CreateSource(IVsTextLines buffer)
        {
            m_source = new LuaSource(this, buffer, GetColorizer(buffer));
            return (m_source);
        }

        public override LanguagePreferences GetLanguagePreferences()
        {
            LanguagePreferences langPref = new LanguagePreferences();
            langPref.EnableCodeSense = true;
            langPref.AutoListMembers = true;
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
            req.Sink.ProcessHiddenRegions = false;

            if (req.Reason == ParseReason.MemberSelect ||
                req.Reason == ParseReason.MemberSelectAndHighlightBraces)
            {
                // use m_scanner to get the token just before the token on the line and 
                // col passed to us. Then populate m_authscope. We can start at the file scope
                // structure and work our way down until we find the block we're working in.
                // Then we work back up from there searching for a match for our token.
                m_authScope.m_luaDec.ClearDeclarations();
                TOKEN tok = null;
                Lexer lexer = new tokens();
                    
                string line = m_source.GetLine(req.Line);
                int indx = req.TokenInfo.StartIndex;

                if (line[indx] == '.') // Table dereference, get previous name
                {
                    m_authScope.m_luaDec.SupressIntrinsics();
                    lexer.Start(line);

                    do
                    {
                        tok = lexer.Next();
                    } while (lexer.yypos < indx);
                }

                LuaScope fileScope = (LuaScope)m_fileScopes[req.FileName];

                if (fileScope != null)
                {
                    LuaScope scope = fileScope.FindEnclosingScope(req.Line);
                    if (scope == null)
                        scope = m_globalScope;

                    if (tok != null)
                    {
                        LuaTable table = (LuaTable)scope.Lookup(tok.yytext, req.Line, indx);
                        if (table != null && table.type == LuaType.Table)
                        {
                            LinkedList<LuaName> names = table.GetNames(req.Line, indx);
                            foreach (LuaName n in names)
                                m_authScope.m_luaDec.AddDeclaration(n.name, VARIABLE);

                            LinkedList<LuaTable> tables = table.GetTables(req.Line, indx);
                            foreach (LuaTable t in tables)
                                m_authScope.m_luaDec.AddDeclaration(t.name, NAMESPACE);

                            LinkedList<LuaFunction> funs = table.GetFunctions(req.Line, indx);
                            foreach (LuaFunction f in funs)
                                m_authScope.m_luaDec.AddDeclaration(f.name, METHOD);
                        }
                    }
                    else
                    {
    //                    m_authScope.m_luaDec.EnableIntrinsics();

                        do
                        {
                            LinkedList<LuaName> names = scope.GetNames(req.Line, indx);
                            foreach (LuaName n in names)
                                m_authScope.m_luaDec.AddDeclaration(n.name, VARIABLE);

                            LinkedList<LuaTable> tables = scope.GetTables(req.Line, indx);
                            foreach (LuaTable t in tables)
                                m_authScope.m_luaDec.AddDeclaration(t.name, NAMESPACE);

                            LinkedList<LuaFunction> funs = scope.GetFunctions(req.Line, indx);
                            foreach (LuaFunction f in funs)
                                m_authScope.m_luaDec.AddDeclaration(f.name, METHOD);

                            scope = scope.Parent();
                        } while (scope != null);
                    }
                }            
            }
            else if (req.Reason == ParseReason.Check) // full file parse
            {
                LuaScope fileScope = new LuaScope( m_globalScope );
                fileScope.beginLine = 0;
                fileScope.endLine = Int32.MaxValue;
                Parser p = new syntax();
                SYMBOL ast;
                ast = p.Parse(req.Text);

                if (ast != null)
                {
                    if (ast.yyname == "error")
                        Console.Write("Parse Error: " + ast.Pos);
                    else if (ast.yyname == "chunk")
                    {
                        chunk node = (chunk)ast;
                        node.FillScope(fileScope);
                        fileScope.AddRegions(req.Sink);
                        req.Sink.ProcessHiddenRegions = true;

                        if (m_fileScopes.Contains(req.FileName))
                            m_fileScopes.Remove(req.FileName);

                        m_fileScopes[req.FileName] = fileScope;
                    }
                }
            }

            return m_authScope;
        }

        internal class LuaAuthScope : AuthoringScope
        {
            public LuaDeclarations m_luaDec = new LuaDeclarations();

            public override string GetDataTipText(int line, int col, out TextSpan span)
            {
                span = new TextSpan();
                return null;
            }

            public override Declarations GetDeclarations(IVsTextView view, int line, int col, TokenInfo info, ParseReason reason)
            {
                if (reason == ParseReason.MemberSelect || reason == ParseReason.MemberSelectAndHighlightBraces)
                    return m_luaDec;

                return null;
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

            // LuaDeclarations (intellisense member select support)
            internal class LuaDeclarations : Declarations
            {
                private const int KEYWORD = 206;
                private int m_reservedCount = 0;
                private List<Declaration> m_decs = new List<Declaration>(30);
                private List<Declaration> m_reserved = new List<Declaration>(21);
           
                public LuaDeclarations()
                {
                    m_reserved.Add(new Declaration("and", KEYWORD));
                    m_reserved.Add(new Declaration("break", KEYWORD));
                    m_reserved.Add(new Declaration("do", KEYWORD));
                    m_reserved.Add(new Declaration("else", KEYWORD));
                    m_reserved.Add(new Declaration("elseif", KEYWORD));
                    m_reserved.Add(new Declaration("end", KEYWORD));
                    m_reserved.Add(new Declaration("false", KEYWORD));
                    m_reserved.Add(new Declaration("for", KEYWORD));
                    m_reserved.Add(new Declaration("function", KEYWORD));
                    m_reserved.Add(new Declaration("if", KEYWORD));
                    m_reserved.Add(new Declaration("in", KEYWORD));
                    m_reserved.Add(new Declaration("local", KEYWORD));
                    m_reserved.Add(new Declaration("nil", KEYWORD));
                    m_reserved.Add(new Declaration("not", KEYWORD));
                    m_reserved.Add(new Declaration("or", KEYWORD));
                    m_reserved.Add(new Declaration("repeat", KEYWORD));
                    m_reserved.Add(new Declaration("return", KEYWORD));
                    m_reserved.Add(new Declaration("until", KEYWORD));
                    m_reserved.Add(new Declaration("then", KEYWORD));
                    m_reserved.Add(new Declaration("true", KEYWORD));
                    m_reserved.Add(new Declaration("while", KEYWORD));   
                }

                public void SupressIntrinsics()
                {
                    m_reservedCount = 0;
                }

                public void EnableIntrinsics()
                {
                    m_reservedCount = m_reserved.Count;
                }

                public void AddDeclaration(string dec, int glyph)
                {
                    m_decs.Add(new Declaration(dec, glyph));
                }
                
                public void ClearDeclarations()
                {
                    m_decs.Clear();
                }

                public override int GetCount()
                {
                    return m_decs.Count + m_reservedCount;
                }

                public override string GetDescription(int index)
                {
                   return "";   
                }

                public override string GetDisplayText(int index)
                {
                    if (index >= m_reservedCount)
                        return m_decs[index - m_reservedCount].name;
                    else
                        return m_reserved[index].name;
                }

                public override int GetGlyph(int index)
                {
                    if (index >= m_reservedCount)
                        return m_decs[index - m_reservedCount].glyph;
                    else
                        return m_reserved[index].glyph;
                }

                public override string GetName(int index)
                {
                    if (index == -1) // -1 seems to denote some default and will get exception
                        return " ";  // if we don't catch this. could not find docs on this but returning
                    else             // a space seems to have the desired behavior
                    {
                        if (index >= m_reservedCount)
                            return m_decs[index - m_reservedCount].name;
                        else
                            return m_reserved[index].name;
                    }
                }

                internal class Declaration
                {
                    public Declaration(string n, int g)
                    {
                        name = n;
                        glyph = g;
                    }
                    public string name;
                    public int glyph;
                }
            }
        }
    }
}
