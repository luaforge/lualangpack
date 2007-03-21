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
    protected LuaScope m_root;

    public LuaNamespace() { }
    public LuaNamespace(LuaNamespace ns)
    {
        if (ns == null)
            return;

        foreach (LinkedList<LuaName> names in ns.m_names.Values)
        {
            foreach (LuaName name in names)
            {
                LuaName n = new LuaName(name);
                Add(n);
            }
        }
        foreach (LinkedList<LuaTable> tables in ns.m_tables.Values)
        {
            foreach (LuaTable table in tables)
            {
                LuaTable t = new LuaTable(table);
                Add(t);
            }
        }
        foreach (LinkedList<LuaFunction> funs in ns.m_functions.Values)
        {
            foreach (LuaFunction fun in funs)
            {
                LuaFunction f = new LuaFunction(fun);
                Add(f);
            }
        }
    }

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
            {
                if (this == m_root)
                {
                    retVal = t;
                    break;
                }
                else if ((t.line < line || (t.line == line && t.pos < pos)) &&
                    (t.line > retVal.line || (t.line == retVal.line && t.pos > retVal.pos)))
                    retVal = t;
            }

            if (retVal.line != -1)
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
            {
                if (this == m_root)
                {
                    retVal = n;
                    break;
                }
                else if ((n.line < line || (n.line == line && n.pos < pos)) &&
                    (n.line > retVal.line || (n.line == retVal.line && n.pos > retVal.pos)))
                    retVal = n;
            }

            if (retVal.line != -1)
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
            {
                if (this == m_root)
                {
                    retVal = f;
                    break;
                }
                else if ((f.line < line || (f.line == line && f.pos < pos)) &&
                    (f.line > retVal.line || (f.line == retVal.line && f.pos > retVal.pos)))
                    retVal = f;
            }

            if (retVal.line != -1)
                return retVal;
        }

        return null;
    }
    public virtual ILuaName Lookup(string name, int line, int pos)
    {
        ILuaName n1 = LookupTable(name, line, pos);
        ILuaName n2 = LookupName(name, line, pos);
        ILuaName n3 = LookupFunction(name, line, pos);
        List<ILuaName> l = new List<ILuaName>(3);
        ILuaName obj = null;

        l.Add(n1);
        l.Add(n2);
        l.Add(n3);

        foreach (ILuaName n in l)
        {
            if (obj == null && n != null)
                obj = n;
            else if ((obj != null && n != null) && (n.line >= obj.line && n.pos > obj.pos))
                obj = n;
        }

        return obj;
    }

    protected ILuaName noVLookup(string name, int line, int pos)
    {
        ILuaName n1 = LookupTable(name, line, pos);
        ILuaName n2 = LookupName(name, line, pos);
        ILuaName n3 = LookupFunction(name, line, pos);
        List<ILuaName> l = new List<ILuaName>(3);
        ILuaName obj = null;

        l.Add(n1);
        l.Add(n2);
        l.Add(n3);

        foreach (ILuaName n in l)
        {
            if (obj == null && n != null)
                obj = n;
            else if ((obj != null && n != null) && (n.line >= obj.line && n.pos > obj.pos))
                obj = n;
        }

        return obj;    
    }

    public LuaName ShallowLookupName(string name)
    {
        if (m_names.ContainsKey(name) && m_names[name].Count > 0)
            return m_names[name].First.Value;
        else
            return null;
    }
    public LuaFunction ShallowLookupFunction(string name)
    {
        if (m_functions.ContainsKey(name) && m_functions[name].Count > 0 )
            return m_functions[name].First.Value;
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
            {
                if (this == m_root)
                {
                    tmp = t;
                    break;
                }
                else if ((t.line < line || (t.line == line && t.pos < pos)) &&
                    (t.line > tmp.line || (t.line == tmp.line && t.pos > tmp.pos)))
                {
                    tmp = t;
                }
            }

            if( tmp.line != -1 )
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
            {
                if (this == m_root)
                {
                    tmp = n;
                    break;
                }
                else if ((n.line < line || (n.line == line && n.pos < pos)) &&
                    (n.line > tmp.line || (n.line == tmp.line && n.pos > tmp.pos)))
                {
                    tmp = n;
                }
            }

            if (tmp.line != -1)
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
            {
                if (this == m_root)
                {
                    tmp = f;
                    break;
                }
                else if ((f.line < line || (f.line == line && f.pos < pos)) &&
                    (f.line > tmp.line || (f.line == tmp.line && f.pos > tmp.pos)))
                {
                    tmp = f;
                }
            }

            if (tmp.line != -1)
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

public enum LuaType {
    Name,
    Function,
    Table,
    RetValSet
}

public interface ILuaName
{
    #region Properties
    string name
    {
        get; set;
    }
    string file
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
    LuaType type
    {
        get;
    }
    #endregion 
}

public class LuaName : ILuaName
{
    #region LuaName Implementation
    public string name
    {
        get { return m_name; }
        set { m_name = value; }
    }
    public string file
    {
        get { return m_file; }
        set { m_file = value; }
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
    public LuaType type
    {
        get { return m_type; }
    }
    #endregion

    public LuaName() { }
    public LuaName(string name, int line, int pos)
    {
        m_line = line;
        m_name = name;
        m_pos = pos;
    }
    public LuaName(LuaName n)
    {
        if (n != null)
        {
            m_line = n.m_line;
            m_name = n.m_name;
            m_pos = n.m_pos;
            m_type = n.m_type;
        }
    }

    private int m_line = -1;
    private int m_pos = -1;
    private LuaType m_type = LuaType.Name;
    private string m_name;
    private string m_file;
}

public class RetValSet : ILuaName
{
    #region LuaName Implementation
    public string name
    {
        get { return m_name; }
        set { m_name = value; }
    }
    public string file
    {
        get { return m_file; }
        set { m_file = value; }
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
    public LuaType type
    {
        get { return m_type; }
    }
    #endregion

    public RetValSet(LuaLangImpl.explist e)
    {
        m_retVals = e;
    }
    public void FillScope(LuaScope s, LuaLangImpl.varlist vl)
    {
        m_retVals.FillScope(s, vl);
    }
    public void FillScope(LuaScope s, LuaLangImpl.namelist nl)
    {
        m_retVals.FillScope(s, nl);
    }
    public void FillScope(LuaScope s, LuaLangImpl.var v)
    {
        m_retVals.FillScope(s, v);
    }
    public void FillScope(LuaScope s, LuaLangImpl.NAME n)
    {
        m_retVals.FillScope(s, n);
    }
    public ILuaName Resolve(LuaScope s)
    {
        return m_retVals.Resolve(s);
    }

    private int m_line = 0;
    private int m_pos = 0;
    private string m_name;
    private string m_file;
    private LuaType m_type = LuaType.RetValSet;
    private LuaLangImpl.explist m_retVals;
}

public class LuaFunction : ILuaName
{
    #region LuaName Implementation
    public string name
    {
        get { return m_name; }
        set { m_name = value; }
    }
    public string file
    {
        get { return m_file; }
        set { m_file = value; }
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
    public LuaType type
    {
        get { return m_type; }
    }
    #endregion

    public LuaFunction() { }
    public LuaFunction( string name, int line, int pos ) 
    {
        m_line = line;
        m_name = name;
        m_pos = pos;
    }
    public LuaFunction(LuaFunction f)
    {
        m_line = f.m_line;
        m_name = f.m_name;
        m_pos = f.m_pos;
        m_type = f.m_type;
        m_scope = f.m_scope;
        m_arguments = f.m_arguments;
        m_retVals = f.m_retVals;
    }

    public void Add(LuaLangImpl.explist e)
    {
        m_retVals.AddLast(new RetValSet(e)); 
    }
    public void Add( string n )
    { 
        m_arguments.AddLast(n); 
    }

    public LinkedList<RetValSet> RetStats
    {
        get { return m_retVals; }
    }

    public LuaScope Scope
    {
        get { return m_scope; }
        set { m_scope = value; }
    }

    private int m_line = -1;
    private int m_pos = -1;
    private string m_name;
    private string m_file;
    private LuaScope m_scope;
    private LuaType m_type = LuaType.Function;
    private LinkedList<string> m_arguments = new LinkedList<string>();
    private LinkedList<RetValSet> m_retVals = new LinkedList<RetValSet>();
}

public class LuaScope : LuaNamespace
{
    #region Public Data Members
    public int beginLine;
    public int endLine;
    public int beginIndx;
    public int endIndx;
    public bool outline;
    public static string filename;  
    #endregion
    
    #region Private Data Members
    protected LuaScope m_parent;
    private LuaScope m_rValueScope = null;
    private static LuaScope m_fileScope = null;
    private LinkedList<LuaScope> m_nested = new LinkedList<LuaScope>();
    private LinkedList<TextSpan> m_regions = new LinkedList<TextSpan>();
    #endregion

    public LuaScope RValueScope
    {
        set { m_rValueScope = value; }
    }

    public LuaScope FileScope // accessor for parser
    {
        set { m_fileScope = value; }
        get { return m_fileScope; }
    }

    public virtual LinkedList<LuaScope> nested 
    {
        get { return m_nested; }    
    }

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

    public LuaScope() 
    {
  //      m_root = this;
  //      m_parent = null;
  //      beginLine = -1; endLine = -1; beginIndx = -1; endIndx = -1;
    }

    public LuaScope(LuaTable t) : base(t) { 
    }

    // There's never a case where we want to look only at the current scope since Lua
    // is lexically scoped. We always want to look at all parent scopes as well until
    // we find the name or run out of parents.
    public override ILuaName Lookup(string name, int line, int pos)
    {
        // hack alert. the ast ended up not being structured well for handling assignments
        // where the rvalue is a function call. when the assignment is actually made, the 
        // ast only has visibility to the scope, and all parent scope, of the lvalue. 
        // however, if the rvalue is a function call the return value can't always be resolved
        // since it may be in a child or orthogonal scope. the workaround is to set a reference
        // to the rvalue scope in the lavalue 
        if (m_rValueScope != null)
        {
            ILuaName ret = m_rValueScope.Lookup(name, line, pos);
            if (ret != null)
                return ret;
        }
        // end workaround

        ILuaName obj = base.Lookup(name, line, pos);
        if (obj != null)
            return obj;
        else if (m_parent != null)
            return m_parent.Lookup(name, line, pos);
        else
            return null;
    }

    private LuaScope FindEnclosingScopeAux(int line)
    {
        LuaScope retVal = null;

        if (line >= beginLine && line <= endLine) // We're in this scope, see if we're in a child scope
        {
            retVal = this;
            LuaScope tmp;

            foreach (LuaScope scope in m_nested)
            {
                tmp = scope.FindEnclosingScopeAux(line);
                if (tmp != null)
                {
                    retVal = tmp;
                    break;
                }
            }
        }

        return retVal;    
    }
 
    public LuaScope FindEnclosingScope(int line)
    {
        LuaScope retVal = FindEnclosingScopeAux(line);
         
        if( retVal == null )
            retVal = m_root;

        return retVal;
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
            span.iStartLine = beginLine;
            span.iEndLine = endLine;
            span.iEndIndex = endIndx - 1;
            span.iStartIndex = beginIndx;
            sink.AddHiddenRegion(span);
        }

        foreach (TextSpan span in m_regions)
            sink.AddHiddenRegion(span);

        foreach (LuaScope scope in m_nested)
            scope.AddRegions(sink);
    }

    private void InitStdLibGlobals()
    {
        LuaName name = null;
        LuaTable t = new LuaTable("io", 0, 0);
        LuaFunction n = new LuaFunction("close", 0, 0);
        t.Add(n);
        n = new LuaFunction("flush", 0, 0);
        t.Add(n);
        n = new LuaFunction("input", 0, 0);
        t.Add(n);
        n = new LuaFunction("lines", 0, 0);
        t.Add(n);
        n = new LuaFunction("open", 0, 0);
        t.Add(n);
        n = new LuaFunction("output", 0, 0);
        t.Add(n);
        n = new LuaFunction("read", 0, 0);
        t.Add(n);
        n = new LuaFunction("tmpfile", 0, 0);
        t.Add(n);
        n = new LuaFunction("type", 0, 0);
        t.Add(n);
        n = new LuaFunction("write", 0, 0);
        t.Add(n);
        name = new LuaName("stdin", 0, 0);
        t.Add(name);
        name = new LuaName("stdout", 0, 0);
        t.Add(name);
        name = new LuaName("stderr", 0, 0);
        t.Add(name);
        Add(t);

        t = new LuaTable("string", 0, 0);
        n = new LuaFunction("byte", 0, 0);
        t.Add(n);
        n = new LuaFunction("char", 0, 0);
        t.Add(n);
        n = new LuaFunction("dump", 0, 0);
        t.Add(n);
        n = new LuaFunction("find", 0, 0);
        t.Add(n);
        n = new LuaFunction("format", 0, 0);
        t.Add(n);
        n = new LuaFunction("gfind", 0, 0);
        t.Add(n);
        n = new LuaFunction("gsub", 0, 0);
        t.Add(n);
        n = new LuaFunction("len", 0, 0);
        t.Add(n);
        n = new LuaFunction("lower", 0, 0);
        t.Add(n);
        n = new LuaFunction("rep", 0, 0);
        t.Add(n);
        n = new LuaFunction("sub", 0, 0);
        t.Add(n);
        n = new LuaFunction("upper", 0, 0);
        t.Add(n);
        Add(t);

        t = new LuaTable("coroutine", 0, 0);
        n = new LuaFunction("create", 0, 0);
        t.Add(n);
        n = new LuaFunction("resume", 0, 0);
        t.Add(n);
        n = new LuaFunction("status", 0, 0);
        t.Add(n);
        n = new LuaFunction("wrap", 0, 0);
        t.Add(n);
        n = new LuaFunction("yield", 0, 0);
        t.Add(n);
        Add(t);

        t = new LuaTable("table", 0, 0);
        n = new LuaFunction("concat", 0, 0);
        t.Add(n);
        n = new LuaFunction("foreach", 0, 0);
        t.Add(n);
        n = new LuaFunction("foreachi", 0, 0);
        t.Add(n);
        n = new LuaFunction("getn", 0, 0);
        t.Add(n);
        n = new LuaFunction("sort", 0, 0);
        t.Add(n);
        n = new LuaFunction("insert", 0, 0);
        t.Add(n);
        n = new LuaFunction("remove", 0, 0);
        t.Add(n);
        n = new LuaFunction("setn", 0, 0);
        t.Add(n);
        Add(t);

        t = new LuaTable("math", 0, 0);
        n = new LuaFunction("abs", 0, 0);
        t.Add(n);
        n = new LuaFunction("acos", 0, 0);
        t.Add(n);
        n = new LuaFunction("asin", 0, 0);
        t.Add(n);
        n = new LuaFunction("atan", 0, 0);
        t.Add(n);
        n = new LuaFunction("atan2", 0, 0);
        t.Add(n);
        n = new LuaFunction("ceil", 0, 0);
        t.Add(n);
        n = new LuaFunction("cos", 0, 0);
        t.Add(n);
        n = new LuaFunction("deg", 0, 0);
        t.Add(n);
        n = new LuaFunction("exp", 0, 0);
        t.Add(n);
        n = new LuaFunction("floor", 0, 0);
        t.Add(n);
        n = new LuaFunction("log", 0, 0);
        t.Add(n);
        n = new LuaFunction("log10", 0, 0);
        t.Add(n);
        n = new LuaFunction("max", 0, 0);
        t.Add(n);
        n = new LuaFunction("min", 0, 0);
        t.Add(n);
        n = new LuaFunction("mod", 0, 0);
        t.Add(n);
        n = new LuaFunction("pow", 0, 0);
        t.Add(n);
        n = new LuaFunction("rad", 0, 0);
        t.Add(n);
        n = new LuaFunction("sin", 0, 0);
        t.Add(n);
        n = new LuaFunction("sqrt", 0, 0);
        t.Add(n);
        n = new LuaFunction("tan", 0, 0);
        t.Add(n);
        n = new LuaFunction("frexp", 0, 0);
        t.Add(n);
        n = new LuaFunction("ldexp", 0, 0);
        t.Add(n);
        n = new LuaFunction("random", 0, 0);
        t.Add(n);
        n = new LuaFunction("randomseed", 0, 0);
        t.Add(n);
        name = new LuaName("pi", 0, 0);
        t.Add(name);
        Add(t);

        t = new LuaTable("os", 0, 0);
        n = new LuaFunction("date", 0, 0);
        t.Add(n);
        n = new LuaFunction("difftime", 0, 0);
        t.Add(n);
        n = new LuaFunction("execute", 0, 0);
        t.Add(n);
        n = new LuaFunction("exit", 0, 0);
        t.Add(n);
        n = new LuaFunction("getenv", 0, 0);
        t.Add(n);
        n = new LuaFunction("remove", 0, 0);
        t.Add(n);
        n = new LuaFunction("rename", 0, 0);
        t.Add(n);
        n = new LuaFunction("time", 0, 0);
        t.Add(n);
        n = new LuaFunction("tmpname", 0, 0);
        t.Add(n);
        Add(t);

        t = new LuaTable("debug", 0, 0);
        n = new LuaFunction("debug", 0, 0);
        t.Add(n);
        n = new LuaFunction("gethook", 0, 0);
        t.Add(n);
        n = new LuaFunction("getinfo", 0, 0);
        t.Add(n);
        n = new LuaFunction("getlocal", 0, 0);
        t.Add(n);
        n = new LuaFunction("getupvalue", 0, 0);
        t.Add(n);
        n = new LuaFunction("setlocal", 0, 0);
        t.Add(n);
        n = new LuaFunction("sethook", 0, 0);
        t.Add(n);
        n = new LuaFunction("traceback", 0, 0);
        t.Add(n);
        Add(t);

        LuaFunction f = new LuaFunction("assert", 0, 0);
        Add(f);
        f = new LuaFunction("collectgarbage", 0, 0);
        Add(f);
        f = new LuaFunction("dofile", 0, 0);
        Add(f);
        f = new LuaFunction("error", 0, 0);
        Add(f);
        f = new LuaFunction("getfenv", 0, 0);
        Add(f);
        f = new LuaFunction("getmetatable", 0, 0);
        Add(f);
        f = new LuaFunction("gcinfo", 0, 0);
        Add(f);
        f = new LuaFunction("ipairs", 0, 0);
        Add(f);
        f = new LuaFunction("loadfile", 0, 0);
        Add(f);
        f = new LuaFunction("loadlib", 0, 0);
        Add(f);
        f = new LuaFunction("loadstring", 0, 0);
        Add(f);
        f = new LuaFunction("next", 0, 0);
        Add(f);
        f = new LuaFunction("pairs", 0, 0);
        Add(f);
        f = new LuaFunction("pcall", 0, 0);
        Add(f);
        f = new LuaFunction("print", 0, 0);
        Add(f);
        f = new LuaFunction("rawequal", 0, 0);
        Add(f);
        f = new LuaFunction("rawget", 0, 0);
        Add(f);
        f = new LuaFunction("rawset", 0, 0);
        Add(f);
        f = new LuaFunction("require", 0, 0);
        Add(f);
        f = new LuaFunction("setfenv", 0, 0);
        Add(f);
        f = new LuaFunction("setmetatable", 0, 0);
        Add(f);
        f = new LuaFunction("tonumber", 0, 0);
        Add(f);
        f = new LuaFunction("tostring", 0, 0);
        Add(f);
        f = new LuaFunction("type", 0, 0);
        Add(f);
        f = new LuaFunction("unpack", 0, 0);
        Add(f);
        f = new LuaFunction("xpcall", 0, 0);
        Add(f);
    }
}

public class LuaTable : LuaScope, ILuaName
{
    #region LuaName Implementation
    public string name
    {
        get { return m_name; }
        set { m_name = value; }
    }
    public string file
    {
        get { return m_file; }
        set { m_file = value; }
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
    public LuaType type
    {
        get { return m_type; }
    }
    #endregion

    public LuaTable(){ }
    public LuaTable(LuaScope s)
        : base(s.Parent())
    {
           
    }
    public LuaTable(string name, int line, int pos) : base(null)
    {
        m_line = line; m_pos = pos; m_name = name;
    }
    public LuaTable(LuaTable t)
        : base(t)
    {
        m_line = t.m_line;
        m_name = t.m_name;
        m_pos = t.m_pos;
        m_type = t.m_type;
    }

    public override LinkedList<LuaScope> nested
    {
        get { return m_parent.nested; }
    }

    public void SetEnclosingScope( LuaScope s )
    {
        m_root = s.GlobalScope();
        m_parent = s;
    }

    public override ILuaName Lookup(string name, int line, int pos)
    {
        return noVLookup(name, line, pos);
    }

    private int m_line = -1;
    private int m_pos = -1;
    private string m_name;
    private string m_file;
    private LuaType m_type = LuaType.Table;
}