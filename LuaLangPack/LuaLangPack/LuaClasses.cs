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

public class LuaNamespace
{
    public void Add(LuaName n)
    {
        if (!m_names.ContainsKey(n.name))
            m_names.Add(n.name, new LinkedList<LuaName>());

        m_names[n.name].AddLast(n);
    }
    public void Add(LuaTable t)
    {
        if (!m_tables.ContainsKey(t.name))
            m_tables.Add(t.name, new LinkedList<LuaTable>());

        m_tables[t.name].AddLast(t);
    }
    public void Add(LuaFunction f)
    {
        if (!m_functions.ContainsKey(f.name))
            m_functions.Add(f.name, new LinkedList<LuaFunction>());

        m_functions[f.name].AddLast(f);
    }

    public LuaTable LookupTable(string name, int line, int pos)
    {
        LuaTable retVal = new LuaTable();

        if (m_tables.ContainsKey(name))
        {
            foreach (LuaTable t in m_tables[name])
                if (t.line <= line && t.pos < pos && t.line >= retVal.line && t.pos > retVal.pos)
                    retVal = t;

            if (retVal.line != 0)
                return retVal;
        }

        return null;
    }
    public LuaName LookupName(string name, int line, int pos)
    {
        LuaName retVal = new LuaName();

        if (m_names.ContainsKey(name))
        {
            foreach (LuaName n in m_names[name])
                if (n.line <= line && n.pos < pos && n.line >= retVal.line && n.pos > retVal.pos)
                    retVal = n;

            if (retVal.line != 0)
                return retVal;
        }

        return null;
    }
    public LuaFunction LookupFunction(string name, int line, int pos)
    {
        LuaFunction retVal = new LuaFunction();

        if (m_functions.ContainsKey(name))
        {
            foreach (LuaFunction f in m_functions[name])
                if (f.line <= line && f.pos < pos && f.line >= retVal.line && f.pos > retVal.pos)
                    retVal = f;

            if (retVal.line != 0)
                return retVal;
        }

        return null;
    }
    public LuaName Lookup(string name, int line, int pos)
    {
        ILuaName n1 = LookupTable(name, line, pos);
        ILuaName n2 = LookupName(name, line, pos);
        ILuaName n3 = LookupFunction(name, line, pos);
        List<ILuaName> l = new List<ILuaName>(3);

        if (n1 == null)
            n1 = new LuaName();
        if (n2 == null)
            n2 = new LuaName();
        if (n3 == null)
            n3 = new LuaName();

        l.Add(n1);
        l.Add(n2);
        l.Add(n3);

        LuaName obj = new LuaName();

        foreach (LuaName n in l)
            if (n.line >= obj.line && n.pos > obj.pos)
                obj = n;

        if (obj.line != 0)
            return obj;
        else
            return null;
    }

    public LinkedList<LuaTable> GetTables( int line, int pos )
    {
        LinkedList<LuaTable> ret = new LinkedList<LuaTable>();

        foreach (LinkedList<LuaTable> table in m_tables.Values)
        {
            LuaTable tmp = new LuaTable();

            foreach (LuaTable t in table)
                if (t.line <= line && t.pos < pos && t.line >= tmp.line && t.pos > tmp.pos)
                    tmp = t;

            if( tmp.line != 0 )
                ret.AddLast(tmp);
        }

        return ret;
    }
    public LinkedList<LuaName> GetNames(int line, int pos)
    {
        LinkedList<LuaName> ret = new LinkedList<LuaName>();

        foreach (LinkedList<LuaName> names in m_names.Values)
        {
            LuaName tmp = new LuaName();

            foreach (LuaName n in names)
                if (n.line <= line && n.pos < pos && n.line >= tmp.line && n.pos > tmp.pos)
                    tmp = n;

            if (tmp.line != 0)
                ret.AddLast(tmp);
        }

        return ret;
    }
    public LinkedList<LuaFunction> GetFunctions(int line, int pos)
    {
        LinkedList<LuaFunction> ret = new LinkedList<LuaFunction>();

        foreach (LinkedList<LuaFunction> functions in m_functions.Values)
        {
            LuaFunction tmp = new LuaFunction();

            foreach (LuaFunction f in functions)
                if (f.line <= line && f.pos < pos && f.line >= tmp.line && f.pos > tmp.pos)
                    tmp = f;

            if (tmp.line != 0)
                ret.AddLast(tmp);
        }

        return ret;
    }


    #region Direct Accessors
    public Dictionary<string, LinkedList<LuaName>> names
    {
        get
        {
            return m_names;
        }
    }
    public Dictionary<string, LinkedList<LuaTable>> tables
    {
        get
        {
            return m_tables;
        }
    }
    public Dictionary<string, LinkedList<LuaFunction>> functions
    {
        get
        {
            return m_functions;
        }
    }
    #endregion


    // We have a list inside of each hash entry to handle multiple definitions of the same name in the
    // same namespace. Consider the following code snippet.
    // foo = {}
    // ...
    // foo = {name = "Lua"}
    // If we dereference the table foo between these two definitions and then after the second definition
    // we expect to see different results. Therefore, we must record every state of a name within the 
    // same namespace.
    private Dictionary<string, LinkedList<LuaName>> m_names = new Dictionary<string, LinkedList<LuaName>>();
    private Dictionary<string, LinkedList<LuaTable>> m_tables = new Dictionary<string, LinkedList<LuaTable>>();
    private Dictionary<string, LinkedList<LuaFunction>> m_functions = new Dictionary<string, LinkedList<LuaFunction>>();
}

public interface ILuaName
{
    string name
    {
        get; set;
    }
    int line
    {
        get; set;
    }
    int pos
    {
        get; set;
    }
}

public class LuaName : ILuaName
{
    #region LuaName Implementation
    public string name
    {
        get { return m_name; }
        set { m_name = value; }
    }
    public int line
    {
        get { return m_line; }
        set { m_line = value; }
    }
    public int pos
    {
        get { return m_pos; }
        set { m_pos = value; }
    }
    #endregion

    private int m_line = 0;
    private int m_pos = 0;
    private string m_name;
}

public class LuaTable : LuaNamespace, ILuaName
{
    #region LuaName Implementation
    public string name
    {
        get { return m_name; }
        set { m_name = value; }
    }
    public int line
    {
        get { return m_line; }
        set { m_line = value; }
    }
    public int pos
    {
        get { return m_pos; }
        set { m_pos = value; }
    }
    #endregion 

    private int m_line = 0;
    private int m_pos = 0;
    private string m_name;
}

public class LuaFunction : ILuaName
{
    #region LuaName Implementation
    public string name
    {
        get { return m_name; }
        set { m_name = value; }
    }
    public int line
    {
        get { return m_line; }
        set { m_line = value; }
    }
    public int pos
    {
        get { return m_pos; }
        set { m_pos = value; }
    }
    #endregion

    private int m_line = 0;
    private int m_pos = 0;
    private string m_name;

    public LinkedList<string> arguments = new LinkedList<string>();
}


public class LuaScope : LuaNamespace
{
    #region Public Data Members
    public int beginLine;
    public int endLine;
    public int beginIndx;
    public int endIndx;
    public bool outline;
    public LinkedList<LuaScope> nested = new LinkedList<LuaScope>();
    #endregion
    
    #region Private Data Members
    private LuaScope m_root;
    private LuaScope m_parent;
    private LinkedList<TextSpan> m_regions = new LinkedList<TextSpan>();
    #endregion

    public LuaScope(LuaScope parent)
    {
        if (parent == null)
        {
            m_root = this;
            InitStdLibGlobals();
        }
        else
            m_root = parent.m_root;

        m_parent = parent;

        beginLine = -1; endLine = -1; beginIndx = -1; endIndx = -1;
    }

    public LuaScope FindEnclosingScope(int line)
    {
        if (line >= beginLine && line <= endLine) // We're in this scope, see if we're in a child scope
        {
            if (nested.Count == 0)

            foreach (LuaScope scope in nested)
            {
                if (scope.FindEnclosingScope(line) != this)
                    return scope;
            }

            return this;
        }
        else
            return m_parent;
    }

    public LuaScope GlobalScope()
    {
        return m_root;
    }

    public void DeclareRegion(int beginLine, int beginPos, int endLine, int endPos)
    {
        TextSpan span = new TextSpan();
        span.iStartLine = beginLine;
        span.iStartIndex = beginPos;
        span.iEndLine = endLine;
        span.iEndIndex = endPos;
        m_regions.AddLast(span);
    }

    public LuaScope Parent()
    {
        return m_parent;
    }

    public void AddRegions(AuthoringSink sink)
    {
        if (outline == true && beginLine != endLine && endLine != Int32.MaxValue)
        {
            TextSpan span = new TextSpan();
            span.iStartLine = beginLine - 1;
            span.iEndLine = endLine - 1;
            span.iEndIndex = endIndx - 1;
            span.iStartIndex = beginIndx;
            sink.AddHiddenRegion(span);
        }

        foreach (TextSpan span in m_regions)
            sink.AddHiddenRegion(span);

        foreach (LuaScope scope in nested)
            scope.AddRegions(sink);
    }

    private void InitStdLibGlobals()
    {
        LuaTable t = new LuaTable();
        t.name = "io";
        LuaFunction n = new LuaFunction();
        n.name = "close";
        t.Add(n);
        n = new LuaFunction();
        n.name = "flush";
        t.Add(n);
        n = new LuaFunction();
        n.name = "input";
        t.Add(n);
        n = new LuaFunction();
        n.name = "lines";
        t.Add(n);
        n = new LuaFunction();
        n.name = "open";
        t.Add(n);
        n = new LuaFunction();
        n.name = "output";
        t.Add(n);
        n = new LuaFunction();
        n.name = "read";
        t.Add(n);
        n = new LuaFunction();
        n.name = "tmpfile";
        t.Add(n);
        n = new LuaFunction();
        n.name = "type";
        t.Add(n);
        n = new LuaFunction();
        n.name = "write";
        t.Add(n);
        Add(t);

        t = new LuaTable();
        t.name = "string";
        n = new LuaFunction();
        n.name = "byte";
        t.Add(n);
        n = new LuaFunction();
        n.name = "char";
        t.Add(n);
        n = new LuaFunction();
        n.name = "dump";
        t.Add(n);
        n = new LuaFunction();
        n.name = "find";
        t.Add(n);
        n = new LuaFunction();
        n.name = "format";
        t.Add(n);
        n = new LuaFunction();
        n.name = "gfind";
        t.Add(n);
        n = new LuaFunction();
        n.name = "gsub";
        t.Add(n);
        n = new LuaFunction();
        n.name = "len";
        t.Add(n);
        n = new LuaFunction();
        n.name = "lower";
        t.Add(n);
        n = new LuaFunction();
        n.name = "rep";
        t.Add(n);
        n = new LuaFunction();
        n.name = "sub";
        t.Add(n);
        n = new LuaFunction();
        n.name = "upper";
        t.Add(n);
        Add(t);

        LuaFunction f = new LuaFunction();
        f.name = "assert";
        Add(f);
        f = new LuaFunction();
        f.name = "collectgarbage";
        Add(f);
        f = new LuaFunction();
        f.name = "dofile";
        Add(f);
        f = new LuaFunction();
        f.name = "error";
        Add(f);
        f = new LuaFunction();
        f.name = "getfenv";
        Add(f);
        f = new LuaFunction();
        f.name = "getmetatable";
        Add(f);
        f = new LuaFunction();
        f.name = "gcinfo";
        Add(f);
        f = new LuaFunction();
        f.name = "ipairs";
        Add(f);
        f = new LuaFunction();
        f.name = "loadfile";
        Add(f);
        f = new LuaFunction();
        f.name = "loadlib";
        Add(f);
        f = new LuaFunction();
        f.name = "loadstring";
        Add(f);
        f = new LuaFunction();
        f.name = "next";
        Add(f);
        f = new LuaFunction();
        f.name = "pairs";
        Add(f);
        f = new LuaFunction();
        f.name = "pcall";
        Add(f);
        f = new LuaFunction();
        f.name = "print";
        Add(f);
        f = new LuaFunction();
        f.name = "rawequal";
        Add(f);
        f = new LuaFunction();
        f.name = "rawget";
        Add(f);
        f = new LuaFunction();
        f.name = "rawset";
        Add(f);
        f = new LuaFunction();
        f.name = "require";
        Add(f);
        f = new LuaFunction();
        f.name = "setfenv";
        Add(f);
        f = new LuaFunction();
        f.name = "setmetatable";
        Add(f);
        f = new LuaFunction();
        f.name = "tonumber";
        Add(f);
        f = new LuaFunction();
        f.name = "tostring";
        Add(f);
        f = new LuaFunction();
        f.name = "type";
        Add(f);
        f = new LuaFunction();
        f.name = "unpack";
        Add(f);
        f = new LuaFunction();
        f.name = "xpcall";
        Add(f);
    }
}