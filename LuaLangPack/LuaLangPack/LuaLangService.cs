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
        private LuaScope m_globalScope = new LuaScope((LuaScope)null);
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
            return (new LuaSource(this, buffer, GetColorizer(buffer)));
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
                   
                string line;
                req.View.GetTextStream(req.Line, 0, req.Line, int.MaxValue, out line); 
                int indx = req.TokenInfo.StartIndex;

                if (line[indx] == '.') // Table dereference, resolve lvalue
                {
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
                        var = line.Substring(indx - ast.yylx.yypos - 1, ast.yylx.yypos + 1);

                    // Forward parse var for resolution to value
                    p = new LuaVarParser.syntax();
                    ast = p.Parse(var);

                    if (ast != null)
                    {
                        if (ast.yyname == "error")
                            Console.Write("Parse Error: " + ast.Pos);
                        else
                        {
                            LuaVarParser.chunk node = (LuaVarParser.chunk)ast;
                            ILuaName resolvedN = node.Resolve( enclosing, req.Line, indx );
                            if (resolvedN == null || resolvedN.type != LuaType.Table)
                                return m_authScope;
                            else
                                resolvedT = (LuaTable)resolvedN;

                            LinkedList<LuaName> names = resolvedT.GetNames(req.Line, indx);
                            foreach (LuaName n in names)
                                m_authScope.m_luaDec.AddDeclaration(n.name, VARIABLE, n.line, n.pos);

                            LinkedList<LuaTable> tables = resolvedT.GetTables(req.Line, indx);
                            foreach (LuaTable t in tables)
                                m_authScope.m_luaDec.AddDeclaration(t.name, NAMESPACE, t.line, t.pos);

                            LinkedList<LuaFunction> funs = resolvedT.GetFunctions(req.Line, indx);
                            foreach (LuaFunction f in funs)
                                m_authScope.m_luaDec.AddDeclaration(f.name, METHOD, f.line, f.pos);
                        }
                    }
                }
                else
                {
                    m_authScope.m_luaDec.EnableIntrinsics();
                    do
                    {
                        LinkedList<LuaName> names = enclosing.GetNames(req.Line, indx);
                        foreach (LuaName n in names)
                            m_authScope.m_luaDec.AddDeclaration(n.name, VARIABLE, n.line, n.pos);

                        LinkedList<LuaTable> tables = enclosing.GetTables(req.Line, indx);
                        foreach (LuaTable t in tables)
                            m_authScope.m_luaDec.AddDeclaration(t.name, NAMESPACE, t.line, t.pos);

                        LinkedList<LuaFunction> funs = enclosing.GetFunctions(req.Line, indx);
                        foreach (LuaFunction f in funs)
                            m_authScope.m_luaDec.AddDeclaration(f.name, METHOD, f.line, f.pos);

                        enclosing = enclosing.Parent();
                    } while (enclosing != null);
                }            
            }
            ///////////////////////////////////////////////////////////////////
            // Handle Full File Parse /////////////////////////////////////////
            else if (req.Reason == ParseReason.Check)
            {
                // cleanup stale names from global space
                foreach (LinkedList<LuaName> names in m_globalScope.names.Values)
                {
                    LinkedList<LuaName> buf = new LinkedList<LuaName>();

                    foreach (LuaName name in names)
                        if (name.file == req.FileName)
                            buf.AddLast(name);

                    foreach (LuaName name in buf)
                        m_globalScope.names[name.name].Remove(name);

                    buf.Clear();
                }
                foreach (LinkedList<LuaTable> tables in m_globalScope.tables.Values)
                {
                    LinkedList<LuaTable> buf = new LinkedList<LuaTable>();

                    foreach (LuaTable table in tables)
                        if (table.file == req.FileName)
                            buf.AddLast(table);

                    foreach (LuaTable table in buf)
                        m_globalScope.tables[table.name].Remove(table);

                    buf.Clear();
                }
                foreach (LinkedList<LuaFunction> funs in m_globalScope.functions.Values)
                {
                    LinkedList<LuaFunction> buf = new LinkedList<LuaFunction>();

                    foreach (LuaFunction fun in funs)
                        if (fun.file == req.FileName)
                            buf.AddLast(fun);

                    foreach (LuaFunction fun in buf)
                        m_globalScope.functions[fun.name].Remove(fun);

                    buf.Clear();
                }

                // Create filescope object and fill it with info from ast
                LuaScope fileScope = new LuaScope( m_globalScope );
                fileScope.beginLine = 0;
                fileScope.endLine = Int32.MaxValue;
                LuaScope.filename = req.FileName;
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
                        try
                        {
                            fileScope.FileScope = fileScope;
                            node.FillScope(fileScope);
                        }
                       catch (NullReferenceException nr)
                        {
                         
                        }
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
                private SortedList<string, Declaration> m_decs = new SortedList<string, Declaration>(100);
                private List<Declaration> m_reserved = new List<Declaration>(21);
                private Dictionary<string, int> m_dups = new Dictionary<string, int>(100);
           
                public LuaDeclarations()
                {
                    m_reserved.Add(new Declaration("and", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("break", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("do", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("else", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("elseif", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("end", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("false", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("for", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("function", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("if", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("in", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("local", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("nil", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("not", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("or", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("repeat", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("return", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("until", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("then", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("true", KEYWORD, Int32.MaxValue, Int32.MaxValue));
                    m_reserved.Add(new Declaration("while", KEYWORD, Int32.MaxValue, Int32.MaxValue));   
                }

                public void EnableIntrinsics()
                {
                    foreach (Declaration dec in m_reserved)
                        m_decs.Add(dec.name, dec);
                }

                public void AddDeclaration(string dec, int glyph, int line, int pos)
                {
                    // TODO: may need to revisit when union/intersection types are implemented 
                    Declaration decl = null;

                    if( m_decs.ContainsKey(dec) ) 
                        decl = m_decs[dec];

                    if (decl == null)
                    {
                        m_decs.Add(dec, new Declaration(dec, glyph, line, pos));
                    }
                    else if (decl.line < line || (decl.line == line && decl.pos < pos))
                    {
                        m_decs[dec] = new Declaration(dec, glyph, line, pos);
                    }
                }
                
                public void ClearDeclarations()
                {
                    m_decs.Clear();
                    m_dups.Clear();
                }

                public override int GetCount()
                {
                    return m_decs.Count;
                }

                public override string GetDescription(int index)
                {
                   return "";   
                }

                public override string GetDisplayText(int index)
                {
                    if (index < m_decs.Count)
                        return m_decs.Values[index].name;
                    else
                        return " ";
                }

                public override int GetGlyph(int index)
                {
                    if (index < m_decs.Count)
                        return m_decs.Values[index].glyph;
                    else
                        return 0;
                }

                public override string GetName(int index)
                {
                    if (index == -1) // -1 seems to denote some default and will get exception
                        return " ";  // if we don't catch this. could not find docs on this but returning
                    else             // a space seems to have the desired behavior
                    {
                        if (index < m_decs.Count)
                            return m_decs.Values[index].name;
                        else
                            return " ";
                    }
                }

                internal class Declaration
                {
                    public Declaration(string n, int g, int l, int p)
                    {
                        name = n;
                        glyph = g;
                        line = l;
                        pos = p;
                    }
                    public string name;
                    public int glyph;
                    public int line;
                    public int pos;
                }
            }
        }
    }
}
