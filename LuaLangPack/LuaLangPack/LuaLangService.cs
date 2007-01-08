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

        static public string StringReverse(string str)
        {
            int len = str.Length;
            char[] charArray = new char[len];     
            for (int i = 1; i < len; i++)
                charArray[i - 1] = str[len - i];
            return new string(charArray);
        }

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

            ///////////////////////////////////////////////////////////////////
            // Handle Intellisense popup contents /////////////////////////////
            if (req.Reason == ParseReason.MemberSelect ||
                req.Reason == ParseReason.MemberSelectAndHighlightBraces)
            {
                LuaTable resolvedT = null;
                m_authScope.m_luaDec.ClearDeclarations();

                LuaScope fileScope = (LuaScope)m_fileScopes[req.FileName];
                if (fileScope == null)
                    return m_authScope;

                LuaScope enclosing = fileScope.FindEnclosingScope(req.Line);
                if (enclosing == null)
                    enclosing = m_globalScope;
                    
                string line = m_source.GetLine(req.Line);
                int indx = req.TokenInfo.StartIndex;

                if (line[indx] == '.') // Table dereference, resolve lvalue
                {
                    m_authScope.m_luaDec.SupressIntrinsics(); // don't show keywords ect.

                    // Reverse parse string to find var we're referencing
                    string rev = StringReverse(line.Substring(0, indx));
                    string var = null;
                    Parser p = new ReverseLangImpl.syntax();
                    SYMBOL ast;
                    ast = p.Parse(rev);
                    if (ast == null)
                        return m_authScope;
                    else if (ast.yyname == "error")
                        var = line.Substring(indx - ast.Position, ast.Position);
                    else
                        var = line.Substring(indx - ast.yylx.yypos, ast.yylx.yypos);

                    // Forward parse var for resolution to value
                    p = new LuaVarParser.syntax();
                    ast = p.Parse(var);

                    if (ast != null)
                    {
                        if (ast.yyname == "error")
                            Console.Write("Parse Error: " + ast.Pos);
                        else
                        {
                            LuaVarParser.var node = (LuaVarParser.var)ast;
                            resolvedT = (LuaTable)node.Resolve( enclosing, req.Line, indx );
                            if (resolvedT == null || resolvedT.type != LuaType.Table)
                                return m_authScope;

                            LinkedList<LuaName> names = resolvedT.GetNames(req.Line, indx);
                            foreach (LuaName n in names)
                                m_authScope.m_luaDec.AddDeclaration(n.name, VARIABLE);

                            LinkedList<LuaTable> tables = resolvedT.GetTables(req.Line, indx);
                            foreach (LuaTable t in tables)
                                m_authScope.m_luaDec.AddDeclaration(t.name, NAMESPACE);

                            LinkedList<LuaFunction> funs = resolvedT.GetFunctions(req.Line, indx);
                            foreach (LuaFunction f in funs)
                                m_authScope.m_luaDec.AddDeclaration(f.name, METHOD);
                        }
                    }
                }
                else
                {
                    // m_authScope.m_luaDec.EnableIntrinsics();
                    do
                    {
                        LinkedList<LuaName> names = enclosing.GetNames(req.Line, indx);
                        foreach (LuaName n in names)
                            m_authScope.m_luaDec.AddDeclaration(n.name, VARIABLE);

                        LinkedList<LuaTable> tables = enclosing.GetTables(req.Line, indx);
                        foreach (LuaTable t in tables)
                            m_authScope.m_luaDec.AddDeclaration(t.name, NAMESPACE);

                        LinkedList<LuaFunction> funs = enclosing.GetFunctions(req.Line, indx);
                        foreach (LuaFunction f in funs)
                            m_authScope.m_luaDec.AddDeclaration(f.name, METHOD);

                        enclosing = enclosing.Parent();
                    } while (enclosing != null);
                }            
            }
            ///////////////////////////////////////////////////////////////////
            // Handle Full File Parse /////////////////////////////////////////
            else if (req.Reason == ParseReason.Check)
            {
                LuaScope fileScope = new LuaScope( m_globalScope );
                fileScope.beginLine = 0;
                fileScope.endLine = Int32.MaxValue;
                Parser p = new LuaLangImpl.syntax();
                SYMBOL ast;
                ast = p.Parse(req.Text);

                if (ast != null)
                {
                    if (ast.yyname == "error")
                        Console.Write("Parse Error: " + ast.Pos);
                    else if (ast.yyname == "chunk")
                    {
                        LuaLangImpl.chunk node = (LuaLangImpl.chunk)ast;
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
