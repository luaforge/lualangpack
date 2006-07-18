using System;using Tools;
//%+chunk+54
public class chunk : SYMBOL{
 stat  s ;
 chunk  c ;
 public  chunk (Parser yyp, stat  a ):base(((syntax)yyp)){ s = a ;
}
 public  chunk (Parser yyp, stat  a , chunk  b ):base(((syntax)yyp)){ s = a ;
 c = b ;
}
 public  void  FillScope ( LuaScope  scope ){ if ( s != null ) s . FillScope ( scope );
 if ( c != null ) c . FillScope ( scope );
}

public override string yyname { get { return "chunk"; }}
public override int yynum { get { return 54; }}
public chunk(Parser yyp):base(yyp){}}
//%+block+55
public class block : SYMBOL{
 chunk  c ;
 public  block (Parser yyp, chunk  a ):base(((syntax)yyp)){ c = a ;
}
 public  void  FillScope ( LuaScope  scope ){ LuaScope  nested = new  LuaScope ( scope );
 c . FillScope ( nested );
 scope . nested . AddLast ( nested );
}

public override string yyname { get { return "block"; }}
public override int yynum { get { return 55; }}
public block(Parser yyp):base(yyp){}}
//%+unop+56
public class unop : SYMBOL{

public override string yyname { get { return "unop"; }}
public override int yynum { get { return 56; }}
public unop(Parser yyp):base(yyp){}}
//%+binop+57
public class binop : SYMBOL{

public override string yyname { get { return "binop"; }}
public override int yynum { get { return 57; }}
public binop(Parser yyp):base(yyp){}}
//%+functioncall+58
public class functioncall : SYMBOL{
 private  prefixexp  p ;
 private  arg  m_a ;
 public  functioncall (Parser yyp, prefixexp  a , arg  b ):base(((syntax)yyp)){ p = a ;
 m_a = b ;
}
 public  virtual  void  FillScope ( LuaScope  s ){ p . FillScope ( s );
 m_a . FillScope ( s );
}
 public  Object  Eval (){ return  null ;
}

public override string yyname { get { return "functioncall"; }}
public override int yynum { get { return 58; }}
public functioncall(Parser yyp):base(yyp){}}
//%+funcname+59
public class funcname : SYMBOL{
 NAME  name ;
 public  funcname (Parser yyp, NAME  a ):base(((syntax)yyp)){ name = a ;
}
 public  void  FillScope ( LuaScope  scope ){}

public override string yyname { get { return "funcname"; }}
public override int yynum { get { return 59; }}
public funcname(Parser yyp):base(yyp){}}
//%+parlist+60
public class parlist : SYMBOL{
 parlist  p ;
 NAME  name ;
 public  parlist (Parser yyp, NAME  a ):base(((syntax)yyp)){ name = a ;
}
 public  parlist (Parser yyp, NAME  a , parlist  b ):base(((syntax)yyp)){ name = a ;
 p = b ;
}

public override string yyname { get { return "parlist"; }}
public override int yynum { get { return 60; }}
public parlist(Parser yyp):base(yyp){}}
//%+funcbody+61
public class funcbody : SYMBOL{
 block  b ;
 parlist  p ;
 END  e ;
 public  funcbody (Parser yyp, block  a , END  c ):base(((syntax)yyp)){ b = a ;
 e = c ;
}
 public  funcbody (Parser yyp, block  a , parlist  pl , END  c ):base(((syntax)yyp)){ b = a ;
 p = pl ;
 e = c ;
}
 public  void  FillScope ( LuaScope  s ){ s . endLine = e . Line ;
 b . FillScope ( s );
}

public override string yyname { get { return "funcbody"; }}
public override int yynum { get { return 61; }}
public funcbody(Parser yyp):base(yyp){}}
//%+prefixexp+62
public class prefixexp : SYMBOL{
 private  var  v ;
 private  functioncall  fc ;
 private  exp  e ;
 public  prefixexp (Parser yyp, var  a ):base(((syntax)yyp)){ v = a ;
}
 public  prefixexp (Parser yyp, functioncall  a ):base(((syntax)yyp)){ fc = a ;
}
 public  prefixexp (Parser yyp, exp  a ):base(((syntax)yyp)){ e = a ;
}
 public  virtual  void  FillScope ( LuaScope  s ){ if ( v != null ){ v . FillScope ( s );
}
 else  if ( fc != null ){ fc . FillScope ( s );
}
 else  if ( e != null ){ e . FillScope ( s );
}
}
 public  void  FillScope ( LuaScope  s , var  v ){ FillScope ( s );
}
 public  LuaTable  GetTable ( LuaScope  s ){ if ( v != null ){ return  s . Lookup ( v . Eval ());
}
 else  if ( e != null ){ return  s . Lookup ( e . Eval ());
}
 else  if ( fc != null ){ return  s . Lookup ( fc . Eval ());
}
 else { return  null ;
}
}

public override string yyname { get { return "prefixexp"; }}
public override int yynum { get { return 62; }}
public prefixexp(Parser yyp):base(yyp){}}
//%+field+63
public class field : SYMBOL{
 private  exp  e ;
 public  field (Parser yyp, exp  a ):base(((syntax)yyp)){ e = a ;
}
 public  virtual  void  FillScope ( LuaScope  s ){ e . FillScope ( s );
}
 public  virtual  void  FillScope ( LuaScope  s , var  v ){ v . FieldAssign ( s , e . Eval ());
}

public override string yyname { get { return "field"; }}
public override int yynum { get { return 63; }}
public field(Parser yyp):base(yyp){}}
//%+FieldExpAssign+64
public class FieldExpAssign : field{
 private  exp  e1 ;
 private  exp  e2 ;
 public  FieldExpAssign (Parser yyp, exp  a , exp  b ):base(((syntax)yyp)){ e1 = a ;
 e2 = b ;
}
 public  override  void  FillScope ( LuaScope  s ){ e1 . FillScope ( s );
 e2 . FillScope ( s );
}
 public  override  void  FillScope ( LuaScope  s , var  v ){}

public override string yyname { get { return "FieldExpAssign"; }}
public override int yynum { get { return 64; }}
public FieldExpAssign(Parser yyp):base(yyp){}}
//%+FieldAssign+65
public class FieldAssign : field{
 private  NAME  n ;
 private  exp  e ;
 public  FieldAssign (Parser yyp, NAME  a , exp  b ):base(((syntax)yyp)){ n = a ;
 e = b ;
}
 public  override  void  FillScope ( LuaScope  s ){ e . FillScope ( s );
}
 public  override  void  FillScope ( LuaScope  s , var  v ){ v . FieldAssign ( s , n . s );
}

public override string yyname { get { return "FieldAssign"; }}
public override int yynum { get { return 65; }}
public FieldAssign(Parser yyp):base(yyp){}}
//%+fieldlist+66
public class fieldlist : SYMBOL{
 private  fieldlist  fl ;
 private  field  f ;
 public  fieldlist (Parser yyp, field  a , fieldlist  b ):base(((syntax)yyp)){ f = a ;
 fl = b ;
}
 public  fieldlist (Parser yyp, field  a ):base(((syntax)yyp)){ f = a ;
}
 public  virtual  void  FillScope ( LuaScope  s ){ f . FillScope ( s );
 if ( fl != null ){ fl . FillScope ( s );
}
}
 public  virtual  void  FillScope ( LuaScope  s , var  v ){ f . FillScope ( s , v );
 if ( fl != null ){ fl . FillScope ( s , v );
}
}

public override string yyname { get { return "fieldlist"; }}
public override int yynum { get { return 66; }}
public fieldlist(Parser yyp):base(yyp){}}
//%+tableconstructor+67
public class tableconstructor : SYMBOL{
 private  fieldlist  f ;
 public  tableconstructor (Parser yyp, fieldlist  a ):base(((syntax)yyp)){ f = a ;
}
 public  virtual  void  FillScope ( LuaScope  s ){ if ( f != null ){ f . FillScope ( s );
}
}
 public  virtual  void  FillScope ( LuaScope  s , var  v ){ v . CreateTable ( s );
 if ( f != null ){ f . FillScope ( s , v );
}
}

public override string yyname { get { return "tableconstructor"; }}
public override int yynum { get { return 67; }}
public tableconstructor(Parser yyp):base(yyp){}}
//%+function+68
public class function : SYMBOL{
 private  funcbody  f ;
 public  function (Parser yyp, funcbody  a ):base(((syntax)yyp)){ f = a ;
}
 public  virtual  void  FillScope ( LuaScope  s ){ f . FillScope ( s );
}
 public  virtual  void  FillScope ( LuaScope  s , var  v ){ f . FillScope ( s );
}

public override string yyname { get { return "function"; }}
public override int yynum { get { return 68; }}
public function(Parser yyp):base(yyp){}}
//%+exp+69
public class exp : SYMBOL{
 private  function  f ;
 private  prefixexp  p ;
 public  exp (Parser yyp, function  a ):base(((syntax)yyp)){ f = a ;
}
 public  exp (Parser yyp, prefixexp  a ):base(((syntax)yyp)){ p = a ;
}
 public  virtual  void  FillScope ( LuaScope  s ){ if ( f != null ){ f . FillScope ( s );
}
 else  if ( p != null ){ p . FillScope ( s );
}
}
 public  virtual  void  FillScope ( LuaScope  s , var  v ){ if ( f != null ){ f . FillScope ( s , v );
}
 else  if ( p != null ){ p . FillScope ( s , v );
}
}
 public  virtual  Object  Eval (){ return  null ;
}

public override string yyname { get { return "exp"; }}
public override int yynum { get { return 69; }}
public exp(Parser yyp):base(yyp){}}
//%+Binop+70
public class Binop : exp{
 private  exp  e1 ;
 private  exp  e2 ;
 private  binop  b ;
 public  Binop (Parser yyp, exp  a , binop  b , exp  c ):base(((syntax)yyp)){ e1 = a ;
 e2 = c ;
}
 public  override  void  FillScope ( LuaScope  s ){ e1 . FillScope ( s );
 e2 . FillScope ( s );
}
 public  override  void  FillScope ( LuaScope  s , var  v ){ e1 . FillScope ( s , v );
 e2 . FillScope ( s , v );
}
 public  override  Object  Eval (){ return  null ;
}

public override string yyname { get { return "Binop"; }}
public override int yynum { get { return 70; }}
public Binop(Parser yyp):base(yyp){}}
//%+Unop+71
public class Unop : exp{
 private  exp  e ;
 public  Unop (Parser yyp, unop  a , exp  b ):base(((syntax)yyp)){ e = b ;
}
 public  override  void  FillScope ( LuaScope  s ){ e . FillScope ( s );
}
 public  override  void  FillScope ( LuaScope  s , var  v ){ e . FillScope ( s , v );
}
 public  override  Object  Eval (){ return  null ;
}

public override string yyname { get { return "Unop"; }}
public override int yynum { get { return 71; }}
public Unop(Parser yyp):base(yyp){}}
//%+ExpTableDec+72
public class ExpTableDec : exp{
 private  tableconstructor  t ;
 public  ExpTableDec (Parser yyp, tableconstructor  a ):base(((syntax)yyp)){ t = a ;
}
 public  override  void  FillScope ( LuaScope  s ){ t . FillScope ( s );
}
 public  override  void  FillScope ( LuaScope  s , var  v ){ t . FillScope ( s , v );
}
 public  override  Object  Eval (){ return  t ;
}

public override string yyname { get { return "ExpTableDec"; }}
public override int yynum { get { return 72; }}
public ExpTableDec(Parser yyp):base(yyp){}}
//%+Atom+73
public class Atom : exp{
 LITERAL  l ;
 NUMBER  n ;
 bool  t = false ;
 bool  nil = false ;
 public  Atom (Parser yyp, LITERAL  a ):base(((syntax)yyp)){ l = a ;
}
 public  Atom (Parser yyp, NUMBER  a ):base(((syntax)yyp)){ n = a ;
}
 public  Atom (Parser yyp, FALSE  a ):base(((syntax)yyp)){ t = false ;
}
 public  Atom (Parser yyp, TRUE  a ):base(((syntax)yyp)){ t = true ;
}
 public  Atom (Parser yyp, NIL  a ):base(((syntax)yyp)){ nil = true ;
}
 public  override  Object  Eval (){ if ( l != null ){ return  l ;
}
 else  if ( n != null ){ return  n ;
}
 else  if ( nil ){ return  null ;
}
 else { return  t ;
}
}

public override string yyname { get { return "Atom"; }}
public override int yynum { get { return 73; }}
public Atom(Parser yyp):base(yyp){}}
//%+explist+74
public class explist : SYMBOL{
 private  explist  l ;
 private  exp  e ;
 public  explist (Parser yyp, exp  a , explist  b ):base(((syntax)yyp)){ e = a ;
 l = b ;
}
 public  explist (Parser yyp, exp  a ):base(((syntax)yyp)){ e = a ;
}
 public  void  FillScope ( LuaScope  s ){ if ( l != null ){ l . FillScope ( s );
}
 e . FillScope ( s );
}
 public  void  FillScope ( LuaScope  s , varlist  v ){ if ( l != null ){ l . FillScope ( s , v . vl );
}
 e . FillScope ( s , v . v );
}

public override string yyname { get { return "explist"; }}
public override int yynum { get { return 74; }}
public explist(Parser yyp):base(yyp){}}
//%+var+75
public class var : SYMBOL{
 private  NAME  n ;
 public  var (Parser yyp, NAME  a ):base(((syntax)yyp)){ n = a ;
}
 public  virtual  void  FillScope ( LuaScope  scope ){}
 public  virtual  void  CreateTable ( LuaScope  s ){ s . tables . Add ( n . s , new  LuaTable ());
}
 public  virtual  void  FieldAssign ( LuaScope  s , object  name ){ LuaTable  t = s . Lookup ( n . s );
 t . vals . AddLast ( name );
}
 public  virtual  Object  Eval (){ return  n ;
}

public override string yyname { get { return "var"; }}
public override int yynum { get { return 75; }}
public var(Parser yyp):base(yyp){}}
//%+PackageRef+76
public class PackageRef : var{
 private  NAME  n ;
 prefixexp  p ;
 public  PackageRef (Parser yyp, prefixexp  a , NAME  b ):base(((syntax)yyp)){ p = a ;
 n = b ;
}
 public  override  void  FillScope ( LuaScope  s ){}
 public  override  void  CreateTable ( LuaScope  s ){ LuaTable  t = p . GetTable ( s );
 if ( t != null ){ t . tables . Add ( n . s , new  LuaTable ());
}
}
 public  override  void  FieldAssign ( LuaScope  s , object  name ){ LuaTable  t = p . GetTable ( s );
 if ( t != null ){ t =( LuaTable ) t .tables[ n . s ];
 if ( t != null ){ t . vals . AddLast ( name );
}
}
}

public override string yyname { get { return "PackageRef"; }}
public override int yynum { get { return 76; }}
public PackageRef(Parser yyp):base(yyp){}}
//%+TableRef+77
public class TableRef : var{
 prefixexp  p ;
 exp  e ;
 public  TableRef (Parser yyp, prefixexp  a , exp  b ):base(((syntax)yyp)){ p = a ;
 e = b ;
}
 public  override  void  FillScope ( LuaScope  s ){}
 public  override  void  CreateTable ( LuaScope  s ){ LuaTable  t = p . GetTable ( s );
 if ( t != null ){ t . tables . Add ( e . Eval (), new  LuaTable ());
}
}
 public  override  void  FieldAssign ( LuaScope  s , object  name ){ LuaTable  t = p . GetTable ( s );
 if ( t != null ){ t =( LuaTable ) t .tables[ e . Eval ()];
 if ( t != null ){ t . vals . AddLast ( name );
}
}
}

public override string yyname { get { return "TableRef"; }}
public override int yynum { get { return 77; }}
public TableRef(Parser yyp):base(yyp){}}
//%+varlist+78
public class varlist : SYMBOL{
 public  var  v ;
 public  varlist  vl ;
 public  varlist (Parser yyp, var  a , varlist  b ):base(((syntax)yyp)){ v = a ;
 vl = b ;
}
 public  varlist (Parser yyp, var  a ):base(((syntax)yyp)){ v = a ;
}
 public  void  FillScope ( LuaScope  scope ){ v . FillScope ( scope );
 if ( vl != null ){ vl . FillScope ( scope );
}
}

public override string yyname { get { return "varlist"; }}
public override int yynum { get { return 78; }}
public varlist(Parser yyp):base(yyp){}}
//%+stat+79
public class stat : SYMBOL{
 public  stat (Parser yyp):base(((syntax)yyp)){}
 public  virtual  void  FillScope ( LuaScope  scope ){}

public override string yyname { get { return "stat"; }}
public override int yynum { get { return 79; }}
}
//%+Assignment+80
public class Assignment : stat{
 varlist  v ;
 explist  e ;
 public  Assignment (Parser yyp, varlist  a , explist  b ):base(((syntax)yyp)){ v = a ;
 e = b ;
}
 public  override  void  FillScope ( LuaScope  s ){ v . FillScope ( s );
 e . FillScope ( s , v );
}

public override string yyname { get { return "Assignment"; }}
public override int yynum { get { return 80; }}
public Assignment(Parser yyp):base(yyp){}}
//%+Retval+81
public class Retval : stat{
 private  explist  e ;
 public  Retval (Parser yyp, explist  a ):base(((syntax)yyp)){ e = a ;
}

public override string yyname { get { return "Retval"; }}
public override int yynum { get { return 81; }}
public Retval(Parser yyp):base(yyp){}}
//%+FuncDecl+82
public class FuncDecl : stat{
 funcname  fname ;
 funcbody  body ;
 public  FuncDecl (Parser yyp, funcname  a , funcbody  b ):base(((syntax)yyp)){ fname = a ;
 body = b ;
}
 public  override  void  FillScope ( LuaScope  scope ){ scope . beginLine = fname . Line ;
 fname . FillScope ( scope );
 body . FillScope ( scope );
}

public override string yyname { get { return "FuncDecl"; }}
public override int yynum { get { return 82; }}
public FuncDecl(Parser yyp):base(yyp){}}
//%+LocalFuncDecl+83
public class LocalFuncDecl : stat{
 funcbody  body ;
 NAME  name ;
 public  LocalFuncDecl (Parser yyp, NAME  a , funcbody  b ):base(((syntax)yyp)){ name = a ;
 body = b ;
}
 public  override  void  FillScope ( LuaScope  scope ){ scope . beginLine = name . Line ;
 body . FillScope ( scope );
}

public override string yyname { get { return "LocalFuncDecl"; }}
public override int yynum { get { return 83; }}
public LocalFuncDecl(Parser yyp):base(yyp){}}
//%+arg+84
public class arg : SYMBOL{
 private  explist  e ;
 private  tableconstructor  t ;
 public  arg (Parser yyp, tableconstructor  a ):base(((syntax)yyp)){ t = a ;
}
 public  arg (Parser yyp, explist  a ):base(((syntax)yyp)){ a = e ;
}
 public  virtual  void  FillScope ( LuaScope  s ){ if ( e != null ){ e . FillScope ( s );
}
 else  if ( t != null ){ t . FillScope ( s );
}
}

public override string yyname { get { return "arg"; }}
public override int yynum { get { return 84; }}
public arg(Parser yyp):base(yyp){}}

public class chunk_1 : chunk {
  public chunk_1(Parser yyq):base(yyq, 
	((stat)(yyq.StackAt(0).m_value))
	 ){}}

public class chunk_2 : chunk {
  public chunk_2(Parser yyq):base(yyq, 
	((stat)(yyq.StackAt(1).m_value))
	 ){}}

public class chunk_3 : chunk {
  public chunk_3(Parser yyq):base(yyq, 
	((stat)(yyq.StackAt(1).m_value))
	, 
	((chunk)(yyq.StackAt(0).m_value))
	 ){}}

public class chunk_4 : chunk {
  public chunk_4(Parser yyq):base(yyq, 
	((stat)(yyq.StackAt(2).m_value))
	, 
	((chunk)(yyq.StackAt(0).m_value))
	 ){}}

public class block_1 : block {
  public block_1(Parser yyq):base(yyq, 
	((chunk)(yyq.StackAt(0).m_value))
	 ){}}

public class Assignment_1 : Assignment {
  public Assignment_1(Parser yyq):base(yyq, 
	((varlist)(yyq.StackAt(2).m_value))
	, 
	((explist)(yyq.StackAt(0).m_value))
	 ){}}

public class stat_1 : stat {
  public stat_1(Parser yyq):base(yyq){}}

public class stat_2 : stat {
  public stat_2(Parser yyq):base(yyq){}}

public class stat_3 : stat {
  public stat_3(Parser yyq):base(yyq){}}

public class stat_4 : stat {
  public stat_4(Parser yyq):base(yyq){}}

public class stat_5 : stat {
  public stat_5(Parser yyq):base(yyq){}}

public class stat_6 : stat {
  public stat_6(Parser yyq):base(yyq){}}

public class stat_7 : stat {
  public stat_7(Parser yyq):base(yyq){}}

public class stat_8 : stat {
  public stat_8(Parser yyq):base(yyq){}}

public class Retval_1 : Retval {
  public Retval_1(Parser yyq):base(yyq, 
	((explist)(yyq.StackAt(0).m_value))
	 ){}}

public class stat_9 : stat {
  public stat_9(Parser yyq):base(yyq){}}

public class stat_10 : stat {
  public stat_10(Parser yyq):base(yyq){}}

public class stat_11 : stat {
  public stat_11(Parser yyq):base(yyq){}}

public class stat_12 : stat {
  public stat_12(Parser yyq):base(yyq){}}

public class FuncDecl_1 : FuncDecl {
  public FuncDecl_1(Parser yyq):base(yyq, 
	((funcname)(yyq.StackAt(1).m_value))
	, 
	((funcbody)(yyq.StackAt(0).m_value))
	 ){}}

public class LocalFuncDecl_1 : LocalFuncDecl {
  public LocalFuncDecl_1(Parser yyq):base(yyq, 
	((NAME)(yyq.StackAt(1).m_value))
	, 
	((funcbody)(yyq.StackAt(0).m_value))
	 ){}}

public class stat_13 : stat {
  public stat_13(Parser yyq):base(yyq){}}

public class stat_14 : stat {
  public stat_14(Parser yyq):base(yyq){}}
public class elseif : SYMBOL {
	public elseif(Parser yyq):base(yyq) { }
  public override string yyname { get { return "elseif"; }}
  public override int yynum { get { return 96; }}}

public class fieldlist_1 : fieldlist {
  public fieldlist_1(Parser yyq):base(yyq, 
	((field)(yyq.StackAt(0).m_value))
	 ){}}

public class fieldlist_2 : fieldlist {
  public fieldlist_2(Parser yyq):base(yyq, 
	((field)(yyq.StackAt(2).m_value))
	, 
	((fieldlist)(yyq.StackAt(0).m_value))
	 ){}}

public class tableconstructor_1 : tableconstructor {
  public tableconstructor_1(Parser yyq):base(yyq){}}

public class tableconstructor_2 : tableconstructor {
  public tableconstructor_2(Parser yyq):base(yyq, 
	((fieldlist)(yyq.StackAt(1).m_value))
	 ){}}

public class parlist_1 : parlist {
  public parlist_1(Parser yyq):base(yyq, 
	((NAME)(yyq.StackAt(0).m_value))
	 ){}}

public class parlist_2 : parlist {
  public parlist_2(Parser yyq):base(yyq, 
	((NAME)(yyq.StackAt(2).m_value))
	, 
	((parlist)(yyq.StackAt(0).m_value))
	 ){}}
public class init : SYMBOL {
	public init(Parser yyq):base(yyq) { }
  public override string yyname { get { return "init"; }}
  public override int yynum { get { return 109; }}}

public class explist_1 : explist {
  public explist_1(Parser yyq):base(yyq, 
	((exp)(yyq.StackAt(2).m_value))
	,
	((explist)(yyq.StackAt(0).m_value))
	 ){}}

public class explist_2 : explist {
  public explist_2(Parser yyq):base(yyq, 
	((exp)(yyq.StackAt(0).m_value))
	 ){}}

public class Atom_1 : Atom {
  public Atom_1(Parser yyq):base(yyq, 
	((NIL)(yyq.StackAt(0).m_value))
	 ){}}

public class Atom_2 : Atom {
  public Atom_2(Parser yyq):base(yyq, 
	((FALSE)(yyq.StackAt(0).m_value))
	 ){}}

public class Atom_3 : Atom {
  public Atom_3(Parser yyq):base(yyq, 
	((TRUE)(yyq.StackAt(0).m_value))
	 ){}}

public class Atom_4 : Atom {
  public Atom_4(Parser yyq):base(yyq, 
	((NUMBER)(yyq.StackAt(0).m_value))
	 ){}}

public class Atom_5 : Atom {
  public Atom_5(Parser yyq):base(yyq, 
	((LITERAL)(yyq.StackAt(0).m_value))
	 ){}}

public class exp_1 : exp {
  public exp_1(Parser yyq):base(yyq, 
	((function)(yyq.StackAt(0).m_value))
	 ){}}

public class exp_2 : exp {
  public exp_2(Parser yyq):base(yyq, 
	((prefixexp)(yyq.StackAt(0).m_value))
	 ){}}

public class ExpTableDec_1 : ExpTableDec {
  public ExpTableDec_1(Parser yyq):base(yyq, 
	((tableconstructor)(yyq.StackAt(0).m_value))
	 ){}}

public class Binop_1 : Binop {
  public Binop_1(Parser yyq):base(yyq, 
	((exp)(yyq.StackAt(2).m_value))
	,
	((binop)(yyq.StackAt(1).m_value))
	,
	((exp)(yyq.StackAt(0).m_value))
	 ){}}

public class Unop_1 : Unop {
  public Unop_1(Parser yyq):base(yyq, 
	((unop)(yyq.StackAt(1).m_value))
	,
	((exp)(yyq.StackAt(0).m_value))
	 ){}}

public class functioncall_1 : functioncall {
  public functioncall_1(Parser yyq):base(yyq, 
	((prefixexp)(yyq.StackAt(1).m_value))
	, 
	((arg)(yyq.StackAt(0).m_value))
	 ){}}

public class functioncall_2 : functioncall {
  public functioncall_2(Parser yyq):base(yyq, 
	((prefixexp)(yyq.StackAt(3).m_value))
	, 
	((arg)(yyq.StackAt(0).m_value))
	 ){}}

public class prefixexp_1 : prefixexp {
  public prefixexp_1(Parser yyq):base(yyq, 
	((var)(yyq.StackAt(0).m_value))
	 ){}}

public class prefixexp_2 : prefixexp {
  public prefixexp_2(Parser yyq):base(yyq, 
	((functioncall)(yyq.StackAt(0).m_value))
	 ){}}

public class prefixexp_3 : prefixexp {
  public prefixexp_3(Parser yyq):base(yyq, 
	((exp)(yyq.StackAt(1).m_value))
	 ){}}
public class namelist : SYMBOL {
	public namelist(Parser yyq):base(yyq) { }
  public override string yyname { get { return "namelist"; }}
  public override int yynum { get { return 104; }}}

public class varlist_1 : varlist {
  public varlist_1(Parser yyq):base(yyq, 
	((var)(yyq.StackAt(2).m_value))
	, 
	((varlist)(yyq.StackAt(0).m_value))
	 ){}}

public class varlist_2 : varlist {
  public varlist_2(Parser yyq):base(yyq, 
	((var)(yyq.StackAt(0).m_value))
	 ){}}

public class var_1 : var {
  public var_1(Parser yyq):base(yyq, 
	((NAME)(yyq.StackAt(0).m_value))
	 ){}}

public class TableRef_1 : TableRef {
  public TableRef_1(Parser yyq):base(yyq, 
	((prefixexp)(yyq.StackAt(3).m_value))
	, 
	((exp)(yyq.StackAt(1).m_value))
	 ){}}

public class PackageRef_1 : PackageRef {
  public PackageRef_1(Parser yyq):base(yyq, 
	((prefixexp)(yyq.StackAt(2).m_value))
	, 
	((NAME)(yyq.StackAt(0).m_value))
	 ){}}

public class funcname_1 : funcname {
  public funcname_1(Parser yyq):base(yyq, 
	((NAME)(yyq.StackAt(0).m_value))
	 ){}}

public class funcbody_1 : funcbody {
  public funcbody_1(Parser yyq):base(yyq, 
	((block)(yyq.StackAt(1).m_value))
	, 
	((END)(yyq.StackAt(0).m_value))
	 ){}}

public class funcbody_2 : funcbody {
  public funcbody_2(Parser yyq):base(yyq, 
	((block)(yyq.StackAt(1).m_value))
	, 
	((parlist)(yyq.StackAt(3).m_value))
	, 
	((END)(yyq.StackAt(0).m_value))
	 ){}}

public class function_1 : function {
  public function_1(Parser yyq):base(yyq, 
	((funcbody)(yyq.StackAt(0).m_value))
	 ){}}

public class arg_1 : arg {
  public arg_1(Parser yyq):base(yyq, 
	((explist)(yyq.StackAt(1).m_value))
	 ){}}

public class arg_2 : arg {
  public arg_2(Parser yyq):base(yyq, 
	((tableconstructor)(yyq.StackAt(0).m_value))
	 ){}}
public class fieldsep : SYMBOL {
	public fieldsep(Parser yyq):base(yyq) { }
  public override string yyname { get { return "fieldsep"; }}
  public override int yynum { get { return 112; }}}

public class FieldExpAssign_1 : FieldExpAssign {
  public FieldExpAssign_1(Parser yyq):base(yyq, 
	((exp)(yyq.StackAt(3).m_value))
	, 
	((exp)(yyq.StackAt(0).m_value))
	 ){}}

public class FieldAssign_1 : FieldAssign {
  public FieldAssign_1(Parser yyq):base(yyq, 
	((NAME)(yyq.StackAt(2).m_value))
	, 
	((exp)(yyq.StackAt(0).m_value))
	 ){}}

public class field_1 : field {
  public field_1(Parser yyq):base(yyq, 
	((exp)(yyq.StackAt(0).m_value))
	 ){}}
public class yysyntax: YyParser {
  public override object Action(Parser yyq,SYMBOL yysym, int yyact) {
    switch(yyact) {
	 case -1: break; //// keep compiler happy
}  return null; }

public class elseif_1 : elseif {
  public elseif_1(Parser yyq):base(yyq){}}

public class namelist_1 : namelist {
  public namelist_1(Parser yyq):base(yyq){}}

public class funcname_2 : funcname {
  public funcname_2(Parser yyq):base(yyq){}}

public class funcname_3 : funcname {
  public funcname_3(Parser yyq):base(yyq){}}

public class init_1 : init {
  public init_1(Parser yyq):base(yyq){}}

public class elseif_2 : elseif {
  public elseif_2(Parser yyq):base(yyq){}}

public class funcbody_3 : funcbody {
  public funcbody_3(Parser yyq):base(yyq){}}

public class namelist_2 : namelist {
  public namelist_2(Parser yyq):base(yyq){}}

public class namelist_3 : namelist {
  public namelist_3(Parser yyq):base(yyq){}}

public class parlist_3 : parlist {
  public parlist_3(Parser yyq):base(yyq){}}

public class fieldsep_1 : fieldsep {
  public fieldsep_1(Parser yyq):base(yyq){}}

public class fieldsep_2 : fieldsep {
  public fieldsep_2(Parser yyq):base(yyq){}}

public class binop_1 : binop {
  public binop_1(Parser yyq):base(yyq){}}

public class binop_2 : binop {
  public binop_2(Parser yyq):base(yyq){}}

public class binop_3 : binop {
  public binop_3(Parser yyq):base(yyq){}}

public class binop_4 : binop {
  public binop_4(Parser yyq):base(yyq){}}

public class binop_5 : binop {
  public binop_5(Parser yyq):base(yyq){}}

public class binop_6 : binop {
  public binop_6(Parser yyq):base(yyq){}}

public class binop_7 : binop {
  public binop_7(Parser yyq):base(yyq){}}

public class binop_8 : binop {
  public binop_8(Parser yyq):base(yyq){}}

public class binop_9 : binop {
  public binop_9(Parser yyq):base(yyq){}}

public class binop_10 : binop {
  public binop_10(Parser yyq):base(yyq){}}

public class binop_11 : binop {
  public binop_11(Parser yyq):base(yyq){}}

public class binop_12 : binop {
  public binop_12(Parser yyq):base(yyq){}}

public class binop_13 : binop {
  public binop_13(Parser yyq):base(yyq){}}

public class funcbody_4 : funcbody {
  public funcbody_4(Parser yyq):base(yyq){}}

public class arg_3 : arg {
  public arg_3(Parser yyq):base(yyq){}}

public class unop_1 : unop {
  public unop_1(Parser yyq):base(yyq){}}

public class unop_2 : unop {
  public unop_2(Parser yyq):base(yyq){}}

public class unop_3 : unop {
  public unop_3(Parser yyq):base(yyq){}}

public class arg_4 : arg {
  public arg_4(Parser yyq):base(yyq){}}
public yysyntax():base() { arr = new int[] { 
101,4,6,52,0,
46,0,53,0,102,
20,103,4,10,99,
0,104,0,117,0,
110,0,107,0,1,
54,1,2,104,18,
1,1036,102,2,0,
105,5,171,1,953,
106,18,1,953,107,
20,108,4,6,69,
0,78,0,68,0,
1,38,1,1,2,
0,1,922,109,18,
1,922,110,20,111,
4,16,110,0,97,
0,109,0,101,0,
108,0,105,0,115,
0,116,0,1,104,
1,2,2,0,1,
469,112,18,1,469,
113,20,114,4,6,
101,0,120,0,112,
0,1,69,1,2,
2,0,1,682,115,
18,1,682,116,20,
117,4,10,98,0,
108,0,111,0,99,
0,107,0,1,55,
1,2,2,0,1,
943,118,18,1,943,
119,20,120,4,14,
101,0,120,0,112,
0,108,0,105,0,
115,0,116,0,1,
74,1,2,2,0,
1,703,121,18,1,
703,122,20,123,4,
12,65,0,83,0,
83,0,73,0,71,
0,78,0,1,33,
1,1,2,0,1,
702,124,18,1,702,
125,20,126,4,14,
118,0,97,0,114,
0,108,0,105,0,
115,0,116,0,1,
78,1,2,2,0,
1,700,127,18,1,
700,107,2,0,1,
699,128,18,1,699,
116,2,0,1,654,
129,18,1,654,113,
2,0,1,458,130,
18,1,458,131,20,
132,4,10,67,0,
79,0,77,0,77,
0,65,0,1,7,
1,1,2,0,1,
1037,133,18,1,1037,
134,23,135,4,6,
69,0,79,0,70,
0,1,2,1,6,
2,0,1,669,136,
18,1,669,137,20,
138,4,4,68,0,
79,0,1,41,1,
1,2,0,1,896,
139,18,1,896,107,
2,0,1,685,140,
18,1,685,137,2,
0,1,923,141,18,
1,923,142,20,143,
4,4,73,0,78,
0,1,42,1,1,
2,0,1,683,144,
18,1,683,107,2,
0,1,443,145,18,
1,443,113,2,0,
1,834,146,18,1,
834,147,20,148,4,
12,101,0,108,0,
115,0,101,0,105,
0,102,0,1,96,
1,2,2,0,1,
200,149,18,1,200,
113,2,0,1,199,
150,18,1,199,122,
2,0,1,198,151,
18,1,198,152,20,
153,4,12,76,0,
66,0,82,0,65,
0,67,0,75,0,
1,12,1,1,2,
0,1,197,154,18,
1,197,113,2,0,
1,196,155,18,1,
196,156,20,157,4,
12,82,0,66,0,
82,0,65,0,67,
0,75,0,1,13,
1,1,2,0,1,
432,158,18,1,432,
122,2,0,1,430,
159,18,1,430,160,
20,161,4,8,78,
0,65,0,77,0,
69,0,1,5,1,
1,2,0,1,429,
162,18,1,429,163,
20,164,4,6,70,
0,79,0,82,0,
1,40,1,1,2,
0,1,428,165,18,
1,428,166,20,167,
4,16,102,0,117,
0,110,0,99,0,
98,0,111,0,100,
0,121,0,1,61,
1,2,2,0,1,
426,168,18,1,426,
169,20,170,4,16,
102,0,117,0,110,
0,99,0,110,0,
97,0,109,0,101,
0,1,59,1,2,
2,0,1,425,171,
18,1,425,169,2,
0,1,423,172,18,
1,423,173,20,174,
4,6,68,0,79,
0,84,0,1,16,
1,1,2,0,1,
422,175,18,1,422,
160,2,0,1,421,
176,18,1,421,177,
20,178,4,10,67,
0,79,0,76,0,
79,0,78,0,1,
8,1,1,2,0,
1,420,179,18,1,
420,160,2,0,1,
419,180,18,1,419,
181,20,182,4,16,
70,0,85,0,78,
0,67,0,84,0,
73,0,79,0,78,
0,1,45,1,1,
2,0,1,418,183,
18,1,418,166,2,
0,1,895,184,18,
1,895,116,2,0,
1,416,185,18,1,
416,160,2,0,1,
415,186,18,1,415,
181,2,0,1,414,
187,18,1,414,188,
20,189,4,8,105,
0,110,0,105,0,
116,0,1,109,1,
2,2,0,1,412,
190,18,1,412,119,
2,0,1,833,191,
18,1,833,107,2,
0,1,886,192,18,
1,886,137,2,0,
1,760,193,18,1,
760,102,2,0,1,
855,194,18,1,855,
147,2,0,1,643,
195,18,1,643,196,
20,197,4,10,87,
0,72,0,73,0,
76,0,69,0,1,
39,1,1,2,0,
1,403,198,18,1,
403,119,2,0,1,
163,199,18,1,163,
113,2,0,1,162,
200,18,1,162,122,
2,0,1,161,201,
18,1,161,160,2,
0,1,608,202,18,
1,608,203,20,204,
4,10,85,0,78,
0,84,0,73,0,
76,0,1,47,1,
1,2,0,1,977,
205,18,1,977,107,
2,0,1,1036,104,
1,975,206,18,1,
975,107,2,0,1,
384,207,18,1,384,
131,2,0,1,807,
208,18,1,807,209,
20,210,4,8,84,
0,72,0,69,0,
78,0,1,35,1,
1,2,0,1,581,
211,18,1,581,212,
20,213,4,8,69,
0,76,0,83,0,
69,0,1,36,1,
1,2,0,1,781,
214,18,1,781,215,
20,216,4,12,69,
0,76,0,83,0,
69,0,73,0,70,
0,1,37,1,1,
2,0,1,619,217,
18,1,619,113,2,
0,1,856,218,18,
1,856,107,2,0,
1,138,219,18,1,
138,113,2,0,1,
593,220,18,1,593,
107,2,0,1,592,
221,18,1,592,116,
2,0,1,358,222,
18,1,358,122,2,
0,1,369,223,18,
1,369,113,2,0,
1,607,224,18,1,
607,116,2,0,1,
125,225,18,1,125,
226,20,227,4,6,
78,0,73,0,76,
0,1,44,1,1,
2,0,1,124,228,
18,1,124,229,20,
230,4,10,70,0,
65,0,76,0,83,
0,69,0,1,51,
1,1,2,0,1,
123,231,18,1,123,
232,20,233,4,8,
84,0,82,0,85,
0,69,0,1,50,
1,1,2,0,1,
122,234,18,1,122,
235,20,236,4,12,
78,0,85,0,77,
0,66,0,69,0,
82,0,1,6,1,
1,2,0,1,121,
237,18,1,121,238,
20,239,4,14,76,
0,73,0,84,0,
69,0,82,0,65,
0,76,0,1,3,
1,1,2,0,1,
120,240,18,1,120,
241,20,242,4,16,
102,0,117,0,110,
0,99,0,116,0,
105,0,111,0,110,
0,1,68,1,2,
2,0,1,119,243,
18,1,119,244,20,
245,4,32,116,0,
97,0,98,0,108,
0,101,0,99,0,
111,0,110,0,115,
0,116,0,114,0,
117,0,99,0,116,
0,111,0,114,0,
1,67,1,2,2,
0,1,357,246,18,
1,357,110,2,0,
1,595,247,18,1,
595,248,20,249,4,
12,82,0,69,0,
80,0,69,0,65,
0,84,0,1,46,
1,1,2,0,1,
355,250,18,1,355,
110,2,0,1,354,
251,18,1,354,131,
2,0,1,353,252,
18,1,353,160,2,
0,1,352,253,18,
1,352,254,20,255,
4,10,76,0,79,
0,67,0,65,0,
76,0,1,49,1,
1,2,0,1,569,
256,18,1,569,209,
2,0,1,999,257,
18,1,999,258,20,
259,4,14,112,0,
97,0,114,0,108,
0,105,0,115,0,
116,0,1,60,1,
2,2,0,1,347,
260,18,1,347,125,
2,0,1,997,261,
18,1,997,131,2,
0,1,995,262,18,
1,995,263,20,264,
4,12,69,0,76,
0,73,0,80,0,
83,0,69,0,1,
53,1,1,2,0,
1,103,265,18,1,
103,113,2,0,1,
580,266,18,1,580,
116,2,0,1,327,
267,18,1,327,156,
2,0,1,973,268,
18,1,973,258,2,
0,1,813,269,18,
1,813,116,2,0,
1,93,270,18,1,
93,271,20,272,4,
8,117,0,110,0,
111,0,112,0,1,
56,1,2,2,0,
1,92,273,18,1,
92,274,20,275,4,
6,118,0,97,0,
114,0,1,75,1,
2,2,0,1,91,
276,18,1,91,277,
20,278,4,24,102,
0,117,0,110,0,
99,0,116,0,105,
0,111,0,110,0,
99,0,97,0,108,
0,108,0,1,58,
1,2,2,0,1,
757,279,18,1,757,
102,2,0,1,952,
280,18,1,952,116,
2,0,1,88,281,
18,1,88,282,20,
283,4,12,82,0,
80,0,65,0,82,
0,69,0,78,0,
1,11,1,1,2,
0,1,313,284,18,
1,313,113,2,0,
1,976,285,18,1,
976,116,2,0,1,
543,286,18,1,543,
287,20,288,4,4,
73,0,70,0,1,
34,1,1,2,0,
1,944,289,18,1,
944,137,2,0,1,
725,290,18,1,725,
291,20,292,4,8,
115,0,116,0,97,
0,116,0,1,79,
1,2,2,0,1,
520,293,18,1,520,
107,2,0,1,286,
294,18,1,286,295,
20,296,4,18,83,
0,69,0,77,0,
73,0,67,0,79,
0,76,0,79,0,
78,0,1,9,1,
1,2,0,1,554,
297,18,1,554,113,
2,0,1,792,298,
18,1,792,113,2,
0,1,74,299,18,
1,74,113,2,0,
1,996,300,18,1,
996,160,2,0,1,
311,301,18,1,311,
302,20,303,4,6,
97,0,114,0,103,
0,1,84,1,2,
2,0,1,288,304,
18,1,288,305,20,
306,4,16,102,0,
105,0,101,0,108,
0,100,0,115,0,
101,0,112,0,1,
112,1,2,2,0,
1,309,307,18,1,
309,308,20,309,4,
18,102,0,105,0,
101,0,108,0,100,
0,108,0,105,0,
115,0,116,0,1,
66,1,2,2,0,
1,287,310,18,1,
287,131,2,0,1,
67,311,18,1,67,
312,20,313,4,12,
76,0,80,0,65,
0,82,0,69,0,
78,0,1,10,1,
1,2,0,1,285,
314,18,1,285,315,
20,316,4,10,102,
0,105,0,101,0,
108,0,100,0,1,
63,1,2,2,0,
1,284,317,18,1,
284,318,20,319,4,
12,82,0,66,0,
82,0,65,0,67,
0,69,0,1,15,
1,1,2,0,1,
542,320,18,1,542,
119,2,0,1,522,
321,18,1,522,322,
20,323,4,12,82,
0,69,0,84,0,
85,0,82,0,78,
0,1,48,1,1,
2,0,1,521,324,
18,1,521,325,20,
326,4,10,66,0,
82,0,69,0,65,
0,75,0,1,43,
1,1,2,0,1,
60,327,18,1,60,
328,20,329,4,10,
98,0,105,0,110,
0,111,0,112,0,
1,57,1,2,2,
0,1,59,330,18,
1,59,331,20,332,
4,8,80,0,76,
0,85,0,83,0,
1,17,1,1,2,
0,1,58,333,18,
1,58,334,20,335,
4,10,77,0,73,
0,78,0,85,0,
83,0,1,18,1,
1,2,0,1,57,
336,18,1,57,337,
20,338,4,8,77,
0,85,0,76,0,
84,0,1,19,1,
1,2,0,1,56,
339,18,1,56,340,
20,341,4,6,77,
0,79,0,68,0,
1,21,1,1,2,
0,1,55,342,18,
1,55,343,20,344,
4,12,68,0,73,
0,86,0,73,0,
68,0,69,0,1,
22,1,1,2,0,
1,54,345,18,1,
54,346,20,347,4,
6,69,0,88,0,
80,0,1,23,1,
1,2,0,1,53,
348,18,1,53,349,
20,350,4,12,67,
0,79,0,78,0,
67,0,65,0,84,
0,1,52,1,1,
2,0,1,52,351,
18,1,52,352,20,
353,4,4,76,0,
84,0,1,26,1,
1,2,0,1,51,
354,18,1,51,355,
20,356,4,4,71,
0,84,0,1,28,
1,1,2,0,1,
50,357,18,1,50,
358,20,359,4,4,
71,0,69,0,1,
29,1,1,2,0,
1,49,360,18,1,
49,361,20,362,4,
4,76,0,69,0,
1,27,1,1,2,
0,1,48,363,18,
1,48,364,20,365,
4,4,69,0,81,
0,1,24,1,1,
2,0,1,47,366,
18,1,47,367,20,
368,4,6,78,0,
69,0,81,0,1,
25,1,1,2,0,
1,46,369,18,1,
46,113,2,0,1,
45,370,18,1,45,
371,20,372,4,12,
76,0,66,0,82,
0,65,0,67,0,
69,0,1,14,1,
1,2,0,1,44,
373,18,1,44,302,
2,0,1,282,374,
18,1,282,318,2,
0,1,281,375,18,
1,281,308,2,0,
1,519,376,18,1,
519,116,2,0,1,
40,377,18,1,40,
160,2,0,1,39,
378,18,1,39,177,
2,0,1,495,379,
18,1,495,113,2,
0,1,33,380,18,
1,33,381,20,382,
4,18,112,0,114,
0,101,0,102,0,
105,0,120,0,101,
0,120,0,112,0,
1,62,1,2,2,
0,1,510,383,18,
1,510,137,2,0,
1,724,384,18,1,
724,102,2,0,1,
723,385,18,1,723,
119,2,0,1,28,
386,18,1,28,152,
2,0,1,27,387,
18,1,27,160,2,
0,1,26,388,18,
1,26,173,2,0,
1,741,389,18,1,
741,295,2,0,1,
484,390,18,1,484,
131,2,0,1,22,
391,18,1,22,381,
2,0,1,21,392,
18,1,21,131,2,
0,1,20,393,18,
1,20,274,2,0,
1,19,394,18,1,
19,160,2,0,1,
974,395,18,1,974,
282,2,0,1,17,
396,18,1,17,107,
2,0,1,16,397,
18,1,16,116,2,
0,1,15,398,18,
1,15,107,2,0,
1,14,399,18,1,
14,282,2,0,1,
13,400,18,1,13,
312,2,0,1,12,
401,18,1,12,166,
2,0,1,11,402,
18,1,11,181,2,
0,1,10,403,18,
1,10,282,2,0,
1,9,404,18,1,
9,282,2,0,1,
8,405,18,1,8,
119,2,0,1,7,
406,18,1,7,334,
2,0,1,6,407,
18,1,6,408,20,
409,4,6,78,0,
79,0,84,0,1,
32,1,1,2,0,
1,5,410,18,1,
5,411,20,412,4,
10,80,0,79,0,
85,0,78,0,68,
0,1,20,1,1,
2,0,1,4,413,
18,1,4,312,2,
0,1,3,414,18,
1,3,244,2,0,
1,2,415,18,1,
2,238,2,0,1,
1,416,18,1,1,
381,2,0,1,0,
417,18,1,0,0,
2,0,418,5,0,
419,5,175,1,179,
420,19,421,4,10,
97,0,114,0,103,
0,95,0,52,0,
1,179,422,5,4,
1,40,423,16,0,
373,1,22,424,16,
0,301,1,1,425,
16,0,301,1,33,
426,16,0,301,1,
178,427,19,428,4,
12,117,0,110,0,
111,0,112,0,95,
0,51,0,1,178,
429,5,23,1,93,
430,16,0,270,1,
703,431,16,0,270,
1,608,432,16,0,
270,1,458,433,16,
0,270,1,643,434,
16,0,270,1,484,
435,16,0,270,1,
923,436,16,0,270,
1,358,437,16,0,
270,1,28,438,16,
0,270,1,543,439,
16,0,270,1,384,
440,16,0,270,1,
162,441,16,0,270,
1,67,442,16,0,
270,1,196,443,16,
0,270,1,813,444,
16,0,270,1,60,
445,16,0,270,1,
199,446,16,0,270,
1,432,447,16,0,
270,1,781,448,16,
0,270,1,288,449,
16,0,270,1,522,
450,16,0,270,1,
4,451,16,0,270,
1,45,452,16,0,
270,1,177,453,19,
454,4,12,117,0,
110,0,111,0,112,
0,95,0,50,0,
1,177,429,1,176,
455,19,456,4,12,
117,0,110,0,111,
0,112,0,95,0,
49,0,1,176,429,
1,175,457,19,458,
4,10,97,0,114,
0,103,0,95,0,
51,0,1,175,422,
1,174,459,19,460,
4,20,102,0,117,
0,110,0,99,0,
98,0,111,0,100,
0,121,0,95,0,
52,0,1,174,461,
5,3,1,416,462,
16,0,183,1,426,
463,16,0,165,1,
11,464,16,0,401,
1,173,465,19,466,
4,16,98,0,105,
0,110,0,111,0,
112,0,95,0,49,
0,51,0,1,173,
467,5,16,1,619,
468,16,0,327,1,
792,469,16,0,327,
1,554,470,16,0,
327,1,200,471,16,
0,327,1,313,472,
16,0,327,1,197,
473,16,0,327,1,
495,474,16,0,327,
1,103,475,16,0,
327,1,654,476,16,
0,327,1,469,477,
16,0,327,1,443,
478,16,0,327,1,
74,479,16,0,327,
1,46,480,16,0,
327,1,163,481,16,
0,327,1,369,482,
16,0,327,1,138,
483,16,0,327,1,
172,484,19,485,4,
16,98,0,105,0,
110,0,111,0,112,
0,95,0,49,0,
50,0,1,172,467,
1,171,486,19,487,
4,16,98,0,105,
0,110,0,111,0,
112,0,95,0,49,
0,49,0,1,171,
467,1,170,488,19,
489,4,16,98,0,
105,0,110,0,111,
0,112,0,95,0,
49,0,48,0,1,
170,467,1,169,490,
19,491,4,14,98,
0,105,0,110,0,
111,0,112,0,95,
0,57,0,1,169,
467,1,168,492,19,
493,4,14,98,0,
105,0,110,0,111,
0,112,0,95,0,
56,0,1,168,467,
1,167,494,19,495,
4,14,98,0,105,
0,110,0,111,0,
112,0,95,0,55,
0,1,167,467,1,
166,496,19,497,4,
14,98,0,105,0,
110,0,111,0,112,
0,95,0,54,0,
1,166,467,1,165,
498,19,499,4,14,
98,0,105,0,110,
0,111,0,112,0,
95,0,53,0,1,
165,467,1,164,500,
19,501,4,14,98,
0,105,0,110,0,
111,0,112,0,95,
0,52,0,1,164,
467,1,163,502,19,
503,4,14,98,0,
105,0,110,0,111,
0,112,0,95,0,
51,0,1,163,467,
1,162,504,19,505,
4,14,98,0,105,
0,110,0,111,0,
112,0,95,0,50,
0,1,162,467,1,
161,506,19,507,4,
14,98,0,105,0,
110,0,111,0,112,
0,95,0,49,0,
1,161,467,1,160,
508,19,509,4,20,
102,0,105,0,101,
0,108,0,100,0,
115,0,101,0,112,
0,95,0,50,0,
1,160,510,5,1,
1,285,511,16,0,
304,1,159,512,19,
513,4,20,102,0,
105,0,101,0,108,
0,100,0,115,0,
101,0,112,0,95,
0,49,0,1,159,
510,1,158,514,19,
515,4,18,112,0,
97,0,114,0,108,
0,105,0,115,0,
116,0,95,0,51,
0,1,158,516,5,
2,1,997,517,16,
0,257,1,13,518,
16,0,268,1,157,
519,19,520,4,20,
110,0,97,0,109,
0,101,0,108,0,
105,0,115,0,116,
0,95,0,51,0,
1,157,521,5,3,
1,352,522,16,0,
246,1,354,523,16,
0,250,1,429,524,
16,0,109,1,156,
525,19,526,4,20,
110,0,97,0,109,
0,101,0,108,0,
105,0,115,0,116,
0,95,0,50,0,
1,156,521,1,155,
527,19,528,4,20,
102,0,117,0,110,
0,99,0,98,0,
111,0,100,0,121,
0,95,0,51,0,
1,155,461,1,154,
529,19,530,4,16,
101,0,108,0,115,
0,101,0,105,0,
102,0,95,0,50,
0,1,154,531,5,
2,1,813,532,16,
0,146,1,781,533,
16,0,194,1,153,
534,19,535,4,12,
105,0,110,0,105,
0,116,0,95,0,
49,0,1,153,536,
5,1,1,357,537,
16,0,187,1,152,
538,19,539,4,20,
102,0,117,0,110,
0,99,0,110,0,
97,0,109,0,101,
0,95,0,51,0,
1,152,540,5,2,
1,423,541,16,0,
171,1,419,542,16,
0,168,1,151,543,
19,544,4,20,102,
0,117,0,110,0,
99,0,110,0,97,
0,109,0,101,0,
95,0,50,0,1,
151,540,1,150,545,
19,546,4,20,110,
0,97,0,109,0,
101,0,108,0,105,
0,115,0,116,0,
95,0,49,0,1,
150,521,1,149,547,
19,548,4,16,101,
0,108,0,115,0,
101,0,105,0,102,
0,95,0,49,0,
1,149,531,1,148,
549,19,550,4,14,
102,0,105,0,101,
0,108,0,100,0,
95,0,49,0,1,
148,551,5,2,1,
288,552,16,0,314,
1,45,553,16,0,
314,1,147,554,19,
555,4,26,70,0,
105,0,101,0,108,
0,100,0,65,0,
115,0,115,0,105,
0,103,0,110,0,
95,0,49,0,1,
147,551,1,146,556,
19,557,4,32,70,
0,105,0,101,0,
108,0,100,0,69,
0,120,0,112,0,
65,0,115,0,115,
0,105,0,103,0,
110,0,95,0,49,
0,1,146,551,1,
145,558,19,559,4,
10,97,0,114,0,
103,0,95,0,50,
0,1,145,422,1,
144,560,19,561,4,
10,97,0,114,0,
103,0,95,0,49,
0,1,144,422,1,
143,562,19,563,4,
20,102,0,117,0,
110,0,99,0,116,
0,105,0,111,0,
110,0,95,0,49,
0,1,143,564,5,
23,1,93,565,16,
0,240,1,703,566,
16,0,240,1,608,
567,16,0,240,1,
458,568,16,0,240,
1,643,569,16,0,
240,1,484,570,16,
0,240,1,923,571,
16,0,240,1,358,
572,16,0,240,1,
28,573,16,0,240,
1,543,574,16,0,
240,1,384,575,16,
0,240,1,162,576,
16,0,240,1,67,
577,16,0,240,1,
196,578,16,0,240,
1,813,579,16,0,
240,1,60,580,16,
0,240,1,199,581,
16,0,240,1,432,
582,16,0,240,1,
781,583,16,0,240,
1,288,584,16,0,
240,1,522,585,16,
0,240,1,4,586,
16,0,240,1,45,
587,16,0,240,1,
142,588,19,589,4,
20,102,0,117,0,
110,0,99,0,98,
0,111,0,100,0,
121,0,95,0,50,
0,1,142,461,1,
141,590,19,591,4,
20,102,0,117,0,
110,0,99,0,98,
0,111,0,100,0,
121,0,95,0,49,
0,1,141,461,1,
140,592,19,593,4,
20,102,0,117,0,
110,0,99,0,110,
0,97,0,109,0,
101,0,95,0,49,
0,1,140,540,1,
139,594,19,595,4,
24,80,0,97,0,
99,0,107,0,97,
0,103,0,101,0,
82,0,101,0,102,
0,95,0,49,0,
1,139,596,5,38,
1,741,597,16,0,
393,1,923,598,16,
0,273,1,522,599,
16,0,273,1,93,
600,16,0,273,1,
199,601,16,0,273,
1,196,602,16,0,
273,1,944,603,16,
0,393,1,725,604,
16,0,393,1,510,
605,16,0,393,1,
595,606,16,0,393,
1,288,607,16,0,
273,1,608,608,16,
0,273,1,67,609,
16,0,273,1,813,
610,16,0,273,1,
384,611,16,0,273,
1,703,612,16,0,
273,1,60,613,16,
0,273,1,886,614,
16,0,393,1,807,
615,16,0,393,1,
484,616,16,0,273,
1,162,617,16,0,
273,1,581,618,16,
0,393,1,45,619,
16,0,273,1,685,
620,16,0,393,1,
358,621,16,0,273,
1,569,622,16,0,
393,1,781,623,16,
0,273,1,458,624,
16,0,273,1,28,
625,16,0,273,1,
669,626,16,0,393,
1,21,627,16,0,
393,1,14,628,16,
0,393,1,974,629,
16,0,393,1,543,
630,16,0,273,1,
4,631,16,0,273,
1,432,632,16,0,
273,1,643,633,16,
0,273,1,0,634,
16,0,393,1,138,
635,19,636,4,20,
84,0,97,0,98,
0,108,0,101,0,
82,0,101,0,102,
0,95,0,49,0,
1,138,596,1,137,
637,19,638,4,10,
118,0,97,0,114,
0,95,0,49,0,
1,137,596,1,136,
639,19,640,4,18,
118,0,97,0,114,
0,108,0,105,0,
115,0,116,0,95,
0,50,0,1,136,
641,5,15,1,21,
642,16,0,260,1,
595,643,16,0,124,
1,685,644,16,0,
124,1,569,645,16,
0,124,1,14,646,
16,0,124,1,725,
647,16,0,124,1,
886,648,16,0,124,
1,944,649,16,0,
124,1,974,650,16,
0,124,1,581,651,
16,0,124,1,741,
652,16,0,124,1,
510,653,16,0,124,
1,807,654,16,0,
124,1,669,655,16,
0,124,1,0,656,
16,0,124,1,135,
657,19,658,4,18,
118,0,97,0,114,
0,108,0,105,0,
115,0,116,0,95,
0,49,0,1,135,
641,1,134,659,19,
660,4,22,112,0,
114,0,101,0,102,
0,105,0,120,0,
101,0,120,0,112,
0,95,0,51,0,
1,134,661,5,38,
1,741,662,16,0,
416,1,923,663,16,
0,380,1,522,664,
16,0,380,1,93,
665,16,0,380,1,
199,666,16,0,380,
1,196,667,16,0,
380,1,944,668,16,
0,416,1,725,669,
16,0,416,1,510,
670,16,0,416,1,
595,671,16,0,416,
1,288,672,16,0,
380,1,608,673,16,
0,380,1,67,674,
16,0,380,1,813,
675,16,0,380,1,
384,676,16,0,380,
1,703,677,16,0,
380,1,60,678,16,
0,380,1,886,679,
16,0,416,1,807,
680,16,0,416,1,
484,681,16,0,380,
1,162,682,16,0,
380,1,581,683,16,
0,416,1,45,684,
16,0,380,1,685,
685,16,0,416,1,
358,686,16,0,380,
1,569,687,16,0,
416,1,781,688,16,
0,380,1,458,689,
16,0,380,1,28,
690,16,0,380,1,
669,691,16,0,416,
1,21,692,16,0,
391,1,14,693,16,
0,416,1,974,694,
16,0,416,1,543,
695,16,0,380,1,
4,696,16,0,380,
1,432,697,16,0,
380,1,643,698,16,
0,380,1,0,699,
16,0,416,1,133,
700,19,701,4,22,
112,0,114,0,101,
0,102,0,105,0,
120,0,101,0,120,
0,112,0,95,0,
50,0,1,133,661,
1,132,702,19,703,
4,22,112,0,114,
0,101,0,102,0,
105,0,120,0,101,
0,120,0,112,0,
95,0,49,0,1,
132,661,1,131,704,
19,705,4,28,102,
0,117,0,110,0,
99,0,116,0,105,
0,111,0,110,0,
99,0,97,0,108,
0,108,0,95,0,
50,0,1,131,706,
5,38,1,741,707,
16,0,276,1,923,
708,16,0,276,1,
522,709,16,0,276,
1,93,710,16,0,
276,1,199,711,16,
0,276,1,196,712,
16,0,276,1,944,
713,16,0,276,1,
725,714,16,0,276,
1,510,715,16,0,
276,1,595,716,16,
0,276,1,288,717,
16,0,276,1,608,
718,16,0,276,1,
67,719,16,0,276,
1,813,720,16,0,
276,1,384,721,16,
0,276,1,703,722,
16,0,276,1,60,
723,16,0,276,1,
886,724,16,0,276,
1,807,725,16,0,
276,1,484,726,16,
0,276,1,162,727,
16,0,276,1,581,
728,16,0,276,1,
45,729,16,0,276,
1,685,730,16,0,
276,1,358,731,16,
0,276,1,569,732,
16,0,276,1,781,
733,16,0,276,1,
458,734,16,0,276,
1,28,735,16,0,
276,1,669,736,16,
0,276,1,21,737,
16,0,276,1,14,
738,16,0,276,1,
974,739,16,0,276,
1,543,740,16,0,
276,1,4,741,16,
0,276,1,432,742,
16,0,276,1,643,
743,16,0,276,1,
0,744,16,0,276,
1,130,745,19,746,
4,28,102,0,117,
0,110,0,99,0,
116,0,105,0,111,
0,110,0,99,0,
97,0,108,0,108,
0,95,0,49,0,
1,130,706,1,129,
747,19,748,4,12,
85,0,110,0,111,
0,112,0,95,0,
49,0,1,129,749,
5,23,1,93,750,
16,0,265,1,703,
751,16,0,223,1,
608,752,16,0,217,
1,458,753,16,0,
112,1,643,754,16,
0,129,1,484,755,
16,0,379,1,923,
756,16,0,223,1,
358,757,16,0,223,
1,28,758,16,0,
284,1,543,759,16,
0,297,1,384,760,
16,0,223,1,162,
761,16,0,199,1,
67,762,16,0,299,
1,196,763,16,0,
154,1,813,764,16,
0,298,1,60,765,
16,0,219,1,199,
766,16,0,149,1,
432,767,16,0,145,
1,781,768,16,0,
298,1,288,769,16,
0,369,1,522,770,
16,0,223,1,4,
771,16,0,223,1,
45,772,16,0,369,
1,128,773,19,774,
4,14,66,0,105,
0,110,0,111,0,
112,0,95,0,49,
0,1,128,749,1,
127,775,19,776,4,
26,69,0,120,0,
112,0,84,0,97,
0,98,0,108,0,
101,0,68,0,101,
0,99,0,95,0,
49,0,1,127,749,
1,126,777,19,778,
4,10,101,0,120,
0,112,0,95,0,
50,0,1,126,749,
1,125,779,19,780,
4,10,101,0,120,
0,112,0,95,0,
49,0,1,125,749,
1,124,781,19,782,
4,12,65,0,116,
0,111,0,109,0,
95,0,53,0,1,
124,749,1,123,783,
19,784,4,12,65,
0,116,0,111,0,
109,0,95,0,52,
0,1,123,749,1,
122,785,19,786,4,
12,65,0,116,0,
111,0,109,0,95,
0,51,0,1,122,
749,1,121,787,19,
788,4,12,65,0,
116,0,111,0,109,
0,95,0,50,0,
1,121,749,1,120,
789,19,790,4,12,
65,0,116,0,111,
0,109,0,95,0,
49,0,1,120,749,
1,119,791,19,792,
4,18,101,0,120,
0,112,0,108,0,
105,0,115,0,116,
0,95,0,50,0,
1,119,793,5,6,
1,703,794,16,0,
385,1,923,795,16,
0,118,1,358,796,
16,0,190,1,522,
797,16,0,320,1,
4,798,16,0,405,
1,384,799,16,0,
198,1,118,800,19,
801,4,18,101,0,
120,0,112,0,108,
0,105,0,115,0,
116,0,95,0,49,
0,1,118,793,1,
117,802,19,803,4,
18,112,0,97,0,
114,0,108,0,105,
0,115,0,116,0,
95,0,50,0,1,
117,516,1,116,804,
19,805,4,18,112,
0,97,0,114,0,
108,0,105,0,115,
0,116,0,95,0,
49,0,1,116,516,
1,115,806,19,807,
4,36,116,0,97,
0,98,0,108,0,
101,0,99,0,111,
0,110,0,115,0,
116,0,114,0,117,
0,99,0,116,0,
111,0,114,0,95,
0,50,0,1,115,
808,5,27,1,93,
809,16,0,243,1,
703,810,16,0,243,
1,608,811,16,0,
243,1,40,812,16,
0,414,1,458,813,
16,0,243,1,33,
814,16,0,414,1,
643,815,16,0,243,
1,484,816,16,0,
243,1,923,817,16,
0,243,1,358,818,
16,0,243,1,28,
819,16,0,243,1,
543,820,16,0,243,
1,22,821,16,0,
414,1,384,822,16,
0,243,1,162,823,
16,0,243,1,67,
824,16,0,243,1,
196,825,16,0,243,
1,813,826,16,0,
243,1,60,827,16,
0,243,1,199,828,
16,0,243,1,432,
829,16,0,243,1,
781,830,16,0,243,
1,288,831,16,0,
243,1,522,832,16,
0,243,1,4,833,
16,0,243,1,1,
834,16,0,414,1,
45,835,16,0,243,
1,114,836,19,837,
4,36,116,0,97,
0,98,0,108,0,
101,0,99,0,111,
0,110,0,115,0,
116,0,114,0,117,
0,99,0,116,0,
111,0,114,0,95,
0,49,0,1,114,
808,1,113,838,19,
839,4,22,102,0,
105,0,101,0,108,
0,100,0,108,0,
105,0,115,0,116,
0,95,0,50,0,
1,113,840,5,2,
1,288,841,16,0,
307,1,45,842,16,
0,375,1,112,843,
19,306,1,112,510,
1,111,844,19,845,
4,22,102,0,105,
0,101,0,108,0,
100,0,108,0,105,
0,115,0,116,0,
95,0,49,0,1,
111,840,1,110,846,
19,847,4,14,115,
0,116,0,97,0,
116,0,95,0,49,
0,52,0,1,110,
848,5,14,1,595,
849,16,0,290,1,
685,850,16,0,290,
1,569,851,16,0,
290,1,14,852,16,
0,290,1,725,853,
16,0,290,1,886,
854,16,0,290,1,
944,855,16,0,290,
1,974,856,16,0,
290,1,581,857,16,
0,290,1,741,858,
16,0,290,1,510,
859,16,0,290,1,
807,860,16,0,290,
1,669,861,16,0,
290,1,0,862,16,
0,290,1,109,863,
19,189,1,109,536,
1,108,864,19,865,
4,14,115,0,116,
0,97,0,116,0,
95,0,49,0,51,
0,1,108,848,1,
107,866,19,867,4,
30,76,0,111,0,
99,0,97,0,108,
0,70,0,117,0,
110,0,99,0,68,
0,101,0,99,0,
108,0,95,0,49,
0,1,107,848,1,
106,868,19,869,4,
20,70,0,117,0,
110,0,99,0,68,
0,101,0,99,0,
108,0,95,0,49,
0,1,106,848,1,
105,870,19,871,4,
14,115,0,116,0,
97,0,116,0,95,
0,49,0,50,0,
1,105,848,1,104,
872,19,111,1,104,
521,1,103,873,19,
874,4,14,115,0,
116,0,97,0,116,
0,95,0,49,0,
49,0,1,103,848,
1,102,875,19,876,
4,14,115,0,116,
0,97,0,116,0,
95,0,49,0,48,
0,1,102,848,1,
101,877,19,878,4,
12,115,0,116,0,
97,0,116,0,95,
0,57,0,1,101,
848,1,100,879,19,
880,4,16,82,0,
101,0,116,0,118,
0,97,0,108,0,
95,0,49,0,1,
100,848,1,99,881,
19,882,4,12,115,
0,116,0,97,0,
116,0,95,0,56,
0,1,99,848,1,
98,883,19,884,4,
12,115,0,116,0,
97,0,116,0,95,
0,55,0,1,98,
848,1,97,885,19,
886,4,12,115,0,
116,0,97,0,116,
0,95,0,54,0,
1,97,848,1,96,
887,19,148,1,96,
531,1,95,888,19,
889,4,12,115,0,
116,0,97,0,116,
0,95,0,53,0,
1,95,848,1,94,
890,19,891,4,12,
115,0,116,0,97,
0,116,0,95,0,
52,0,1,94,848,
1,93,892,19,893,
4,12,115,0,116,
0,97,0,116,0,
95,0,51,0,1,
93,848,1,92,894,
19,895,4,12,115,
0,116,0,97,0,
116,0,95,0,50,
0,1,92,848,1,
91,896,19,897,4,
12,115,0,116,0,
97,0,116,0,95,
0,49,0,1,91,
848,1,90,898,19,
899,4,24,65,0,
115,0,115,0,105,
0,103,0,110,0,
109,0,101,0,110,
0,116,0,95,0,
49,0,1,90,848,
1,89,900,19,901,
4,14,98,0,108,
0,111,0,99,0,
107,0,95,0,49,
0,1,89,902,5,
11,1,595,903,16,
0,224,1,685,904,
16,0,128,1,569,
905,16,0,266,1,
14,906,16,0,397,
1,886,907,16,0,
184,1,944,908,16,
0,280,1,974,909,
16,0,285,1,581,
910,16,0,221,1,
510,911,16,0,376,
1,807,912,16,0,
269,1,669,913,16,
0,115,1,88,914,
19,915,4,14,99,
0,104,0,117,0,
110,0,107,0,95,
0,52,0,1,88,
916,5,14,1,595,
917,16,0,384,1,
685,918,16,0,384,
1,569,919,16,0,
384,1,14,920,16,
0,384,1,725,921,
16,0,193,1,886,
922,16,0,384,1,
944,923,16,0,384,
1,974,924,16,0,
384,1,581,925,16,
0,384,1,741,926,
16,0,279,1,510,
927,16,0,384,1,
807,928,16,0,384,
1,669,929,16,0,
384,1,0,930,16,
0,104,1,87,931,
19,932,4,14,99,
0,104,0,117,0,
110,0,107,0,95,
0,51,0,1,87,
916,1,86,933,19,
934,4,14,99,0,
104,0,117,0,110,
0,107,0,95,0,
50,0,1,86,916,
1,85,935,19,936,
4,14,99,0,104,
0,117,0,110,0,
107,0,95,0,49,
0,1,85,916,1,
84,937,19,303,1,
84,422,1,83,938,
19,939,4,26,76,
0,111,0,99,0,
97,0,108,0,70,
0,117,0,110,0,
99,0,68,0,101,
0,99,0,108,0,
1,83,848,1,82,
940,19,941,4,16,
70,0,117,0,110,
0,99,0,68,0,
101,0,99,0,108,
0,1,82,848,1,
81,942,19,943,4,
12,82,0,101,0,
116,0,118,0,97,
0,108,0,1,81,
848,1,80,944,19,
945,4,20,65,0,
115,0,115,0,105,
0,103,0,110,0,
109,0,101,0,110,
0,116,0,1,80,
848,1,79,946,19,
292,1,79,848,1,
78,947,19,126,1,
78,641,1,77,948,
19,949,4,16,84,
0,97,0,98,0,
108,0,101,0,82,
0,101,0,102,0,
1,77,596,1,76,
950,19,951,4,20,
80,0,97,0,99,
0,107,0,97,0,
103,0,101,0,82,
0,101,0,102,0,
1,76,596,1,75,
952,19,275,1,75,
596,1,74,953,19,
120,1,74,793,1,
73,954,19,955,4,
8,65,0,116,0,
111,0,109,0,1,
73,749,1,72,956,
19,957,4,22,69,
0,120,0,112,0,
84,0,97,0,98,
0,108,0,101,0,
68,0,101,0,99,
0,1,72,749,1,
71,958,19,959,4,
8,85,0,110,0,
111,0,112,0,1,
71,749,1,70,960,
19,961,4,10,66,
0,105,0,110,0,
111,0,112,0,1,
70,749,1,69,962,
19,114,1,69,749,
1,68,963,19,242,
1,68,564,1,67,
964,19,245,1,67,
808,1,66,965,19,
309,1,66,840,1,
65,966,19,967,4,
22,70,0,105,0,
101,0,108,0,100,
0,65,0,115,0,
115,0,105,0,103,
0,110,0,1,65,
551,1,64,968,19,
969,4,28,70,0,
105,0,101,0,108,
0,100,0,69,0,
120,0,112,0,65,
0,115,0,115,0,
105,0,103,0,110,
0,1,64,551,1,
63,970,19,316,1,
63,551,1,62,971,
19,382,1,62,661,
1,61,972,19,167,
1,61,461,1,60,
973,19,259,1,60,
516,1,59,974,19,
170,1,59,540,1,
58,975,19,278,1,
58,706,1,57,976,
19,329,1,57,467,
1,56,977,19,272,
1,56,429,1,55,
978,19,117,1,55,
902,1,54,979,19,
103,1,54,916,1,
53,980,19,264,1,
53,981,5,2,1,
997,982,16,0,262,
1,13,983,16,0,
262,1,52,984,19,
350,1,52,985,5,
45,1,103,986,16,
0,348,1,313,987,
16,0,348,1,311,
988,17,989,15,990,
4,26,37,0,102,
0,117,0,110,0,
99,0,116,0,105,
0,111,0,110,0,
99,0,97,0,108,
0,108,0,1,-1,
1,5,991,20,746,
1,130,1,3,1,
3,1,2,992,22,
1,46,1,200,993,
16,0,348,1,92,
994,17,995,15,996,
4,20,37,0,112,
0,114,0,101,0,
102,0,105,0,120,
0,101,0,120,0,
112,0,1,-1,1,
5,997,20,703,1,
132,1,3,1,2,
1,1,998,22,1,
48,1,91,999,17,
1000,15,996,1,-1,
1,5,1001,20,701,
1,133,1,3,1,
2,1,1,1002,22,
1,49,1,197,1003,
16,0,348,1,88,
1004,17,1005,15,996,
1,-1,1,5,1006,
20,660,1,134,1,
3,1,4,1,3,
1007,22,1,50,1,
619,1008,16,0,348,
1,977,1009,17,1010,
15,1011,4,18,37,
0,102,0,117,0,
110,0,99,0,98,
0,111,0,100,0,
121,0,1,-1,1,
5,1012,20,589,1,
142,1,3,1,6,
1,5,1013,22,1,
62,1,74,1014,16,
0,348,1,284,1015,
17,1016,15,1017,4,
34,37,0,116,0,
97,0,98,0,108,
0,101,0,99,0,
111,0,110,0,115,
0,116,0,114,0,
117,0,99,0,116,
0,111,0,114,0,
1,-1,1,5,1018,
20,837,1,114,1,
3,1,3,1,2,
1019,22,1,28,1,
282,1020,17,1021,15,
1017,1,-1,1,5,
1022,20,807,1,115,
1,3,1,4,1,
3,1023,22,1,29,
1,495,1024,16,0,
348,1,163,1025,16,
0,348,1,161,1026,
17,1027,15,1028,4,
8,37,0,118,0,
97,0,114,0,1,
-1,1,5,1029,20,
638,1,137,1,3,
1,2,1,1,1030,
22,1,55,1,369,
1031,16,0,348,1,
46,1032,16,0,348,
1,44,1033,17,1034,
15,990,1,-1,1,
5,1035,20,705,1,
131,1,3,1,5,
1,4,1036,22,1,
47,1,792,1037,16,
0,348,1,469,1038,
16,0,348,1,654,
1039,16,0,348,1,
33,1040,17,1041,15,
1042,4,8,37,0,
101,0,120,0,112,
0,1,-1,1,5,
1043,20,778,1,126,
1,3,1,2,1,
1,1044,22,1,42,
1,138,1045,16,0,
348,1,443,1046,16,
0,348,1,27,1047,
17,1048,15,1049,4,
22,37,0,80,0,
97,0,99,0,107,
0,97,0,103,0,
101,0,82,0,101,
0,102,0,1,-1,
1,5,1050,20,595,
1,139,1,3,1,
4,1,3,1051,22,
1,57,1,15,1052,
17,1053,15,1011,1,
-1,1,5,166,1,
3,1,3,1054,22,
1,64,1,12,1055,
17,1056,15,1057,4,
18,37,0,102,0,
117,0,110,0,99,
0,116,0,105,0,
111,0,110,0,1,
-1,1,5,1058,20,
563,1,143,1,3,
1,3,1,2,1059,
22,1,65,1,17,
1060,17,1061,15,1011,
1,-1,1,5,1062,
20,591,1,141,1,
3,1,5,1,4,
1063,22,1,61,1,
124,1064,17,1065,15,
1066,4,10,37,0,
65,0,116,0,111,
0,109,0,1,-1,
1,5,1067,20,788,
1,121,1,3,1,
2,1,1,1068,22,
1,37,1,19,1069,
17,1027,1,1,1030,
1,20,1070,17,995,
1,1,998,1,554,
1071,16,0,348,1,
125,1072,17,1073,15,
1066,1,-1,1,5,
1074,20,790,1,120,
1,3,1,2,1,
1,1075,22,1,36,
1,119,1076,17,1077,
15,1078,4,24,37,
0,69,0,120,0,
112,0,84,0,97,
0,98,0,108,0,
101,0,68,0,101,
0,99,0,1,-1,
1,5,1079,20,776,
1,127,1,3,1,
2,1,1,1080,22,
1,43,1,123,1081,
17,1082,15,1066,1,
-1,1,5,1083,20,
786,1,122,1,3,
1,2,1,1,1084,
22,1,38,1,122,
1085,17,1086,15,1066,
1,-1,1,5,1087,
20,784,1,123,1,
3,1,2,1,1,
1088,22,1,39,1,
121,1089,17,1090,15,
1066,1,-1,1,5,
1091,20,782,1,124,
1,3,1,2,1,
1,1092,22,1,40,
1,120,1093,17,1094,
15,1042,1,-1,1,
5,1095,20,780,1,
125,1,3,1,2,
1,1,1096,22,1,
41,1,975,1097,17,
1098,15,1011,1,-1,
1,5,166,1,4,
1,4,1099,22,1,
63,1,10,1100,17,
1101,15,1102,4,8,
37,0,97,0,114,
0,103,0,1,-1,
1,5,302,1,2,
1,2,1103,22,1,
66,1,9,1104,17,
1105,15,1102,1,-1,
1,5,1106,20,561,
1,144,1,3,1,
4,1,3,1107,22,
1,67,1,327,1108,
17,1109,15,1110,4,
18,37,0,84,0,
97,0,98,0,108,
0,101,0,82,0,
101,0,102,0,1,
-1,1,5,1111,20,
636,1,138,1,3,
1,5,1,4,1112,
22,1,56,1,3,
1113,17,1114,15,1102,
1,-1,1,5,1115,
20,559,1,145,1,
3,1,2,1,1,
1116,22,1,68,1,
2,1117,17,1118,15,
1102,1,-1,1,5,
302,1,1,1,1,
1119,22,1,69,1,
51,1120,19,230,1,
51,1121,5,102,1,
953,1122,17,1123,15,
1124,4,10,37,0,
115,0,116,0,97,
0,116,0,1,-1,
1,5,1125,20,871,
1,105,1,3,1,
8,1,7,1126,22,
1,19,1,703,1127,
16,0,228,1,700,
1128,17,1129,15,1124,
1,-1,1,5,1130,
20,895,1,92,1,
3,1,4,1,3,
1131,22,1,8,1,
458,1132,16,0,228,
1,896,1133,17,1134,
15,1124,1,-1,1,
5,1135,20,876,1,
102,1,3,1,10,
1,9,1136,22,1,
17,1,923,1137,16,
0,228,1,683,1138,
17,1139,15,1124,1,
-1,1,5,1140,20,
893,1,93,1,3,
1,6,1,5,1141,
22,1,9,1,199,
1142,16,0,228,1,
196,1143,16,0,228,
1,432,1144,16,0,
228,1,430,1145,17,
1146,15,1147,4,18,
37,0,110,0,97,
0,109,0,101,0,
108,0,105,0,115,
0,116,0,1,-1,
1,5,110,1,1,
1,1,1148,22,1,
51,1,428,1149,17,
1150,15,1151,4,18,
37,0,70,0,117,
0,110,0,99,0,
68,0,101,0,99,
0,108,0,1,-1,
1,5,1152,20,869,
1,106,1,3,1,
4,1,3,1153,22,
1,20,1,418,1154,
17,1155,15,1156,4,
28,37,0,76,0,
111,0,99,0,97,
0,108,0,70,0,
117,0,110,0,99,
0,68,0,101,0,
99,0,108,0,1,
-1,1,5,1157,20,
867,1,107,1,3,
1,5,1,4,1158,
22,1,21,1,414,
1159,17,1160,15,1124,
1,-1,1,5,1161,
20,847,1,110,1,
3,1,4,1,3,
1162,22,1,23,1,
412,1163,17,1164,15,
1165,4,10,37,0,
105,0,110,0,105,
0,116,0,1,-1,
1,5,188,1,2,
1,2,1166,22,1,
33,1,855,1167,17,
1168,15,1124,1,-1,
1,5,1169,20,886,
1,97,1,3,1,
7,1,6,1170,22,
1,12,1,643,1171,
16,0,228,1,403,
1172,17,1173,15,1174,
4,16,37,0,101,
0,120,0,112,0,
108,0,105,0,115,
0,116,0,1,-1,
1,5,1175,20,801,
1,118,1,3,1,
4,1,3,1176,22,
1,34,1,162,1177,
16,0,228,1,161,
1026,1,608,1178,16,
0,228,1,384,1179,
16,0,228,1,619,
1180,17,1181,15,1124,
1,-1,1,5,1182,
20,891,1,94,1,
3,1,5,1,4,
1183,22,1,10,1,
856,1184,17,1185,15,
1124,1,-1,1,5,
1186,20,889,1,95,
1,3,1,6,1,
5,1187,22,1,11,
1,138,1188,17,1189,
15,1190,4,12,37,
0,66,0,105,0,
110,0,111,0,112,
0,1,-1,1,5,
1191,20,774,1,128,
1,3,1,4,1,
3,1192,22,1,44,
1,358,1193,16,0,
228,1,369,1194,17,
1195,15,1174,1,-1,
1,5,1196,20,792,
1,119,1,3,1,
2,1,1,1197,22,
1,35,1,355,1198,
17,1199,15,1147,1,
-1,1,5,110,1,
3,1,3,1200,22,
1,52,1,125,1072,
1,124,1064,1,123,
1081,1,122,1085,1,
121,1089,1,120,1093,
1,119,1076,1,357,
1201,17,1202,15,1124,
1,-1,1,5,1203,
20,865,1,108,1,
3,1,3,1,2,
1204,22,1,22,1,
834,1205,17,1206,15,
1207,4,14,37,0,
101,0,108,0,115,
0,101,0,105,0,
102,0,1,-1,1,
5,147,1,4,1,
4,1208,22,1,24,
1,833,1209,17,1210,
15,1207,1,-1,1,
5,147,1,4,1,
4,1211,22,1,25,
1,593,1212,17,1213,
15,1124,1,-1,1,
5,1214,20,884,1,
98,1,3,1,8,
1,7,1215,22,1,
13,1,353,1216,17,
1217,15,1147,1,-1,
1,5,110,1,1,
1,1,1148,1,103,
1218,17,1219,15,1220,
4,10,37,0,85,
0,110,0,111,0,
112,0,1,-1,1,
5,1221,20,748,1,
129,1,3,1,3,
1,2,1222,22,1,
45,1,92,994,1,
813,1223,16,0,228,
1,93,1224,16,0,
228,1,88,1004,1,
91,999,1,781,1225,
16,0,228,1,327,
1108,1,282,1020,1,
311,988,1,288,1226,
16,0,228,1,44,
1033,1,67,1227,16,
0,228,1,45,1228,
16,0,228,1,543,
1229,16,0,228,1,
542,1230,17,1231,15,
1232,4,14,37,0,
82,0,101,0,116,
0,118,0,97,0,
108,0,1,-1,1,
5,1233,20,880,1,
100,1,3,1,3,
1,2,1234,22,1,
15,1,48,1235,17,
1236,15,1237,4,12,
37,0,98,0,105,
0,110,0,111,0,
112,0,1,-1,1,
5,328,1,1,1,
1,1238,22,1,84,
1,47,1239,17,1240,
15,1237,1,-1,1,
5,328,1,1,1,
1,1241,22,1,85,
1,521,1242,17,1243,
15,1124,1,-1,1,
5,1244,20,878,1,
101,1,3,1,2,
1,1,1245,22,1,
16,1,60,1246,16,
0,228,1,59,1247,
17,1248,15,1237,1,
-1,1,5,328,1,
1,1,1,1249,22,
1,73,1,58,1250,
17,1251,15,1237,1,
-1,1,5,328,1,
1,1,1,1252,22,
1,74,1,57,1253,
17,1254,15,1237,1,
-1,1,5,328,1,
1,1,1,1255,22,
1,75,1,56,1256,
17,1257,15,1237,1,
-1,1,5,328,1,
1,1,1,1258,22,
1,76,1,55,1259,
17,1260,15,1237,1,
-1,1,5,328,1,
1,1,1,1261,22,
1,77,1,54,1262,
17,1263,15,1237,1,
-1,1,5,328,1,
1,1,1,1264,22,
1,78,1,53,1265,
17,1266,15,1237,1,
-1,1,5,328,1,
1,1,1,1267,22,
1,79,1,52,1268,
17,1269,15,1237,1,
-1,1,5,328,1,
1,1,1,1270,22,
1,80,1,51,1271,
17,1272,15,1237,1,
-1,1,5,328,1,
1,1,1,1273,22,
1,81,1,50,1274,
17,1275,15,1237,1,
-1,1,5,328,1,
1,1,1,1276,22,
1,82,1,49,1277,
17,1278,15,1237,1,
-1,1,5,328,1,
1,1,1,1279,22,
1,83,1,287,1280,
17,1281,15,1282,4,
18,37,0,102,0,
105,0,101,0,108,
0,100,0,115,0,
101,0,112,0,1,
-1,1,5,305,1,
1,1,1,1283,22,
1,86,1,286,1284,
17,1285,15,1282,1,
-1,1,5,305,1,
1,1,1,1286,22,
1,87,1,284,1015,
1,522,1287,16,0,
228,1,760,1288,17,
1289,15,1290,4,12,
37,0,99,0,104,
0,117,0,110,0,
107,0,1,-1,1,
5,1291,20,932,1,
87,1,3,1,3,
1,2,1292,22,1,
3,1,520,1293,17,
1294,15,1124,1,-1,
1,5,1295,20,874,
1,103,1,3,1,
12,1,11,1296,22,
1,18,1,757,1297,
17,1298,15,1290,1,
-1,1,5,1299,20,
915,1,88,1,3,
1,4,1,3,1300,
22,1,4,1,33,
1040,1,28,1301,16,
0,228,1,27,1047,
1,19,1069,1,741,
1302,17,1303,15,1290,
1,-1,1,5,1304,
20,934,1,86,1,
3,1,3,1,2,
1305,22,1,2,1,
484,1306,16,0,228,
1,977,1009,1,20,
1070,1,975,1097,1,
17,1060,1,15,1052,
1,6,1307,17,1308,
15,1309,4,10,37,
0,117,0,110,0,
111,0,112,0,1,
-1,1,5,271,1,
1,1,1,1310,22,
1,71,1,12,1055,
1,7,1311,17,1312,
15,1309,1,-1,1,
5,271,1,1,1,
1,1313,22,1,70,
1,10,1100,1,9,
1104,1,725,1314,17,
1315,15,1290,1,-1,
1,5,1316,20,936,
1,85,1,3,1,
2,1,1,1317,22,
1,1,1,724,1318,
17,1319,15,1320,4,
12,37,0,98,0,
108,0,111,0,99,
0,107,0,1,-1,
1,5,1321,20,901,
1,89,1,3,1,
2,1,1,1322,22,
1,5,1,723,1323,
17,1324,15,1325,4,
22,37,0,65,0,
115,0,115,0,105,
0,103,0,110,0,
109,0,101,0,110,
0,116,0,1,-1,
1,5,1326,20,899,
1,90,1,3,1,
4,1,3,1327,22,
1,6,1,5,1328,
17,1329,15,1309,1,
-1,1,5,271,1,
1,1,1,1330,22,
1,72,1,4,1331,
16,0,228,1,3,
1113,1,2,1117,1,
1,1332,17,1333,15,
1124,1,-1,1,5,
1334,20,897,1,91,
1,3,1,2,1,
1,1335,22,1,7,
1,50,1336,19,233,
1,50,1337,5,102,
1,953,1122,1,703,
1338,16,0,231,1,
700,1128,1,458,1339,
16,0,231,1,896,
1133,1,923,1340,16,
0,231,1,683,1138,
1,199,1341,16,0,
231,1,196,1342,16,
0,231,1,432,1343,
16,0,231,1,430,
1145,1,428,1149,1,
418,1154,1,414,1159,
1,412,1163,1,855,
1167,1,643,1344,16,
0,231,1,403,1172,
1,162,1345,16,0,
231,1,161,1026,1,
608,1346,16,0,231,
1,384,1347,16,0,
231,1,619,1180,1,
856,1184,1,138,1188,
1,358,1348,16,0,
231,1,369,1194,1,
355,1198,1,125,1072,
1,124,1064,1,123,
1081,1,122,1085,1,
121,1089,1,120,1093,
1,119,1076,1,357,
1201,1,834,1205,1,
833,1209,1,593,1212,
1,353,1216,1,103,
1218,1,92,994,1,
813,1349,16,0,231,
1,93,1350,16,0,
231,1,88,1004,1,
91,999,1,781,1351,
16,0,231,1,327,
1108,1,282,1020,1,
311,988,1,288,1352,
16,0,231,1,44,
1033,1,67,1353,16,
0,231,1,45,1354,
16,0,231,1,543,
1355,16,0,231,1,
542,1230,1,48,1235,
1,47,1239,1,521,
1242,1,60,1356,16,
0,231,1,59,1247,
1,58,1250,1,57,
1253,1,56,1256,1,
55,1259,1,54,1262,
1,53,1265,1,52,
1268,1,51,1271,1,
50,1274,1,49,1277,
1,287,1280,1,286,
1284,1,284,1015,1,
522,1357,16,0,231,
1,760,1288,1,520,
1293,1,757,1297,1,
33,1040,1,28,1358,
16,0,231,1,27,
1047,1,19,1069,1,
741,1302,1,484,1359,
16,0,231,1,977,
1009,1,20,1070,1,
975,1097,1,17,1060,
1,15,1052,1,6,
1307,1,12,1055,1,
7,1311,1,10,1100,
1,9,1104,1,725,
1314,1,724,1318,1,
723,1323,1,5,1328,
1,4,1360,16,0,
231,1,3,1113,1,
2,1117,1,1,1332,
1,49,1361,19,255,
1,49,1362,5,71,
1,855,1167,1,723,
1323,1,103,1218,1,
510,1363,16,0,253,
1,953,1122,1,91,
999,1,522,1364,17,
1365,15,1124,1,-1,
1,5,1366,20,882,
1,99,1,3,1,
2,1,1,1367,22,
1,14,1,414,1159,
1,92,994,1,412,
1163,1,88,1004,1,
807,1368,16,0,253,
1,619,1180,1,725,
1369,16,0,253,1,
403,1172,1,700,1128,
1,977,1009,1,944,
1370,16,0,253,1,
284,1015,1,282,1020,
1,741,1371,16,0,
253,1,595,1372,16,
0,253,1,593,1212,
1,161,1026,1,569,
1373,16,0,253,1,
0,1374,16,0,253,
1,369,1194,1,581,
1375,16,0,253,1,
44,1033,1,685,1376,
16,0,253,1,327,
1108,1,683,1138,1,
896,1133,1,12,1055,
1,357,1201,1,1,
1332,1,355,1198,1,
33,1040,1,353,1216,
1,138,1188,1,886,
1377,16,0,253,1,
27,1047,1,856,1184,
1,669,1378,16,0,
253,1,14,1379,16,
0,253,1,3,1113,
1,20,1070,1,15,
1052,1,119,1076,1,
17,1060,1,418,1154,
1,19,1069,1,125,
1072,1,124,1064,1,
123,1081,1,122,1085,
1,121,1089,1,120,
1093,1,975,1097,1,
974,1380,16,0,253,
1,10,1100,1,9,
1104,1,2,1117,1,
542,1230,1,834,1205,
1,833,1209,1,521,
1242,1,520,1293,1,
430,1145,1,311,988,
1,428,1149,1,48,
1381,19,323,1,48,
1382,5,71,1,855,
1167,1,723,1323,1,
103,1218,1,510,1383,
16,0,321,1,953,
1122,1,91,999,1,
522,1364,1,414,1159,
1,92,994,1,412,
1163,1,88,1004,1,
807,1384,16,0,321,
1,619,1180,1,725,
1385,16,0,321,1,
403,1172,1,700,1128,
1,977,1009,1,944,
1386,16,0,321,1,
284,1015,1,282,1020,
1,741,1387,16,0,
321,1,595,1388,16,
0,321,1,593,1212,
1,161,1026,1,569,
1389,16,0,321,1,
0,1390,16,0,321,
1,369,1194,1,581,
1391,16,0,321,1,
44,1033,1,685,1392,
16,0,321,1,327,
1108,1,683,1138,1,
896,1133,1,12,1055,
1,357,1201,1,1,
1332,1,355,1198,1,
33,1040,1,353,1216,
1,138,1188,1,886,
1393,16,0,321,1,
27,1047,1,856,1184,
1,669,1394,16,0,
321,1,14,1395,16,
0,321,1,3,1113,
1,20,1070,1,15,
1052,1,119,1076,1,
17,1060,1,418,1154,
1,19,1069,1,125,
1072,1,124,1064,1,
123,1081,1,122,1085,
1,121,1089,1,120,
1093,1,975,1097,1,
974,1396,16,0,321,
1,10,1100,1,9,
1104,1,2,1117,1,
542,1230,1,834,1205,
1,833,1209,1,521,
1242,1,520,1293,1,
430,1145,1,311,988,
1,428,1149,1,47,
1397,19,204,1,47,
1398,5,63,1,855,
1167,1,723,1323,1,
103,1218,1,741,1302,
1,953,1122,1,91,
999,1,522,1364,1,
414,1159,1,92,994,
1,412,1163,1,88,
1004,1,834,1205,1,
619,1180,1,725,1314,
1,403,1172,1,700,
1128,1,607,1399,16,
0,202,1,284,1015,
1,282,1020,1,593,
1212,1,161,1026,1,
369,1194,1,977,1009,
1,44,1033,1,683,
1138,1,896,1133,1,
357,1201,1,1,1332,
1,355,1198,1,33,
1040,1,353,1216,1,
138,1188,1,2,1117,
1,3,1113,1,856,
1184,1,27,1047,1,
12,1055,1,15,1052,
1,20,1070,1,724,
1318,1,119,1076,1,
17,1060,1,418,1154,
1,19,1069,1,125,
1072,1,124,1064,1,
123,1081,1,122,1085,
1,121,1089,1,120,
1093,1,975,1097,1,
760,1288,1,10,1100,
1,9,1104,1,757,
1297,1,542,1230,1,
327,1108,1,833,1209,
1,521,1242,1,520,
1293,1,430,1145,1,
311,988,1,428,1149,
1,46,1400,19,249,
1,46,1401,5,71,
1,855,1167,1,723,
1323,1,103,1218,1,
510,1402,16,0,247,
1,953,1122,1,91,
999,1,522,1364,1,
414,1159,1,92,994,
1,412,1163,1,88,
1004,1,807,1403,16,
0,247,1,619,1180,
1,725,1404,16,0,
247,1,403,1172,1,
700,1128,1,977,1009,
1,944,1405,16,0,
247,1,284,1015,1,
282,1020,1,741,1406,
16,0,247,1,595,
1407,16,0,247,1,
593,1212,1,161,1026,
1,569,1408,16,0,
247,1,0,1409,16,
0,247,1,369,1194,
1,581,1410,16,0,
247,1,44,1033,1,
685,1411,16,0,247,
1,327,1108,1,683,
1138,1,896,1133,1,
12,1055,1,357,1201,
1,1,1332,1,355,
1198,1,33,1040,1,
353,1216,1,138,1188,
1,886,1412,16,0,
247,1,27,1047,1,
856,1184,1,669,1413,
16,0,247,1,14,
1414,16,0,247,1,
3,1113,1,20,1070,
1,15,1052,1,119,
1076,1,17,1060,1,
418,1154,1,19,1069,
1,125,1072,1,124,
1064,1,123,1081,1,
122,1085,1,121,1089,
1,120,1093,1,975,
1097,1,974,1415,16,
0,247,1,10,1100,
1,9,1104,1,2,
1117,1,542,1230,1,
834,1205,1,833,1209,
1,521,1242,1,520,
1293,1,430,1145,1,
311,988,1,428,1149,
1,45,1416,19,182,
1,45,1417,5,115,
1,953,1122,1,944,
1418,16,0,180,1,
703,1419,16,0,402,
1,700,1128,1,458,
1420,16,0,402,1,
669,1421,16,0,180,
1,896,1133,1,685,
1422,16,0,180,1,
923,1423,16,0,402,
1,683,1138,1,199,
1424,16,0,402,1,
196,1425,16,0,402,
1,432,1426,16,0,
402,1,430,1145,1,
428,1149,1,412,1163,
1,418,1154,1,414,
1159,1,834,1205,1,
886,1427,16,0,180,
1,855,1167,1,643,
1428,16,0,402,1,
403,1172,1,162,1429,
16,0,402,1,161,
1026,1,608,1430,16,
0,402,1,384,1431,
16,0,402,1,833,
1209,1,619,1180,1,
856,1184,1,138,1188,
1,595,1432,16,0,
180,1,358,1433,16,
0,402,1,369,1194,
1,125,1072,1,124,
1064,1,123,1081,1,
122,1085,1,121,1089,
1,120,1093,1,119,
1076,1,357,1201,1,
807,1434,16,0,180,
1,355,1198,1,593,
1212,1,353,1216,1,
352,1435,16,0,186,
1,103,1218,1,581,
1436,16,0,180,1,
92,994,1,813,1437,
16,0,402,1,91,
999,1,93,1438,16,
0,402,1,88,1004,
1,569,1439,16,0,
180,1,781,1440,16,
0,402,1,327,1108,
1,282,1020,1,311,
988,1,49,1277,1,
44,1033,1,67,1441,
16,0,402,1,45,
1442,16,0,402,1,
543,1443,16,0,402,
1,542,1230,1,48,
1235,1,47,1239,1,
521,1242,1,60,1444,
16,0,402,1,59,
1247,1,58,1250,1,
57,1253,1,56,1256,
1,55,1259,1,54,
1262,1,53,1265,1,
52,1268,1,51,1271,
1,50,1274,1,288,
1445,16,0,402,1,
287,1280,1,286,1284,
1,284,1015,1,522,
1446,16,0,402,1,
760,1288,1,520,1293,
1,757,1297,1,33,
1040,1,510,1447,16,
0,180,1,28,1448,
16,0,402,1,27,
1047,1,19,1069,1,
741,1449,16,0,180,
1,484,1450,16,0,
402,1,977,1009,1,
20,1070,1,975,1097,
1,974,1451,16,0,
180,1,17,1060,1,
6,1307,1,15,1052,
1,14,1452,16,0,
180,1,12,1055,1,
7,1311,1,10,1100,
1,9,1104,1,725,
1453,16,0,180,1,
724,1318,1,723,1323,
1,5,1328,1,4,
1454,16,0,402,1,
3,1113,1,2,1117,
1,1,1332,1,0,
1455,16,0,180,1,
44,1456,19,227,1,
44,1457,5,102,1,
953,1122,1,703,1458,
16,0,225,1,700,
1128,1,458,1459,16,
0,225,1,896,1133,
1,923,1460,16,0,
225,1,683,1138,1,
199,1461,16,0,225,
1,196,1462,16,0,
225,1,432,1463,16,
0,225,1,430,1145,
1,428,1149,1,418,
1154,1,414,1159,1,
412,1163,1,855,1167,
1,643,1464,16,0,
225,1,403,1172,1,
162,1465,16,0,225,
1,161,1026,1,608,
1466,16,0,225,1,
384,1467,16,0,225,
1,619,1180,1,856,
1184,1,138,1188,1,
358,1468,16,0,225,
1,369,1194,1,355,
1198,1,125,1072,1,
124,1064,1,123,1081,
1,122,1085,1,121,
1089,1,120,1093,1,
119,1076,1,357,1201,
1,834,1205,1,833,
1209,1,593,1212,1,
353,1216,1,103,1218,
1,92,994,1,813,
1469,16,0,225,1,
93,1470,16,0,225,
1,88,1004,1,91,
999,1,781,1471,16,
0,225,1,327,1108,
1,282,1020,1,311,
988,1,288,1472,16,
0,225,1,44,1033,
1,67,1473,16,0,
225,1,45,1474,16,
0,225,1,543,1475,
16,0,225,1,542,
1230,1,48,1235,1,
47,1239,1,521,1242,
1,60,1476,16,0,
225,1,59,1247,1,
58,1250,1,57,1253,
1,56,1256,1,55,
1259,1,54,1262,1,
53,1265,1,52,1268,
1,51,1271,1,50,
1274,1,49,1277,1,
287,1280,1,286,1284,
1,284,1015,1,522,
1477,16,0,225,1,
760,1288,1,520,1293,
1,757,1297,1,33,
1040,1,28,1478,16,
0,225,1,27,1047,
1,19,1069,1,741,
1302,1,484,1479,16,
0,225,1,977,1009,
1,20,1070,1,975,
1097,1,17,1060,1,
15,1052,1,6,1307,
1,12,1055,1,7,
1311,1,10,1100,1,
9,1104,1,725,1314,
1,724,1318,1,723,
1323,1,5,1328,1,
4,1480,16,0,225,
1,3,1113,1,2,
1117,1,1,1332,1,
43,1481,19,326,1,
43,1482,5,71,1,
855,1167,1,723,1323,
1,103,1218,1,510,
1483,16,0,324,1,
953,1122,1,91,999,
1,522,1364,1,414,
1159,1,92,994,1,
412,1163,1,88,1004,
1,807,1484,16,0,
324,1,619,1180,1,
725,1485,16,0,324,
1,403,1172,1,700,
1128,1,977,1009,1,
944,1486,16,0,324,
1,284,1015,1,282,
1020,1,741,1487,16,
0,324,1,595,1488,
16,0,324,1,593,
1212,1,161,1026,1,
569,1489,16,0,324,
1,0,1490,16,0,
324,1,369,1194,1,
581,1491,16,0,324,
1,44,1033,1,685,
1492,16,0,324,1,
327,1108,1,683,1138,
1,896,1133,1,12,
1055,1,357,1201,1,
1,1332,1,355,1198,
1,33,1040,1,353,
1216,1,138,1188,1,
886,1493,16,0,324,
1,27,1047,1,856,
1184,1,669,1494,16,
0,324,1,14,1495,
16,0,324,1,3,
1113,1,20,1070,1,
15,1052,1,119,1076,
1,17,1060,1,418,
1154,1,19,1069,1,
125,1072,1,124,1064,
1,123,1081,1,122,
1085,1,121,1089,1,
120,1093,1,975,1097,
1,974,1496,16,0,
324,1,10,1100,1,
9,1104,1,2,1117,
1,542,1230,1,834,
1205,1,833,1209,1,
521,1242,1,520,1293,
1,430,1145,1,311,
988,1,428,1149,1,
42,1497,19,143,1,
42,1498,5,4,1,
922,1499,16,0,141,
1,353,1216,1,355,
1198,1,430,1145,1,
41,1500,19,138,1,
41,1501,5,75,1,
855,1167,1,723,1323,
1,103,1218,1,510,
1502,16,0,140,1,
953,1122,1,91,999,
1,522,1364,1,414,
1159,1,92,994,1,
412,1163,1,88,1004,
1,943,1503,16,0,
289,1,807,1504,16,
0,140,1,619,1180,
1,725,1505,16,0,
140,1,403,1172,1,
700,1128,1,944,1506,
16,0,140,1,284,
1015,1,282,1020,1,
495,1507,16,0,383,
1,595,1508,16,0,
140,1,593,1212,1,
469,1509,16,0,192,
1,161,1026,1,569,
1510,16,0,140,1,
0,1511,16,0,140,
1,10,1100,1,369,
1194,1,355,1198,1,
581,1512,16,0,140,
1,44,1033,1,685,
1513,16,0,140,1,
327,1108,1,683,1138,
1,896,1133,1,12,
1055,1,1,1332,1,
357,1201,1,3,1113,
1,654,1514,16,0,
136,1,33,1040,1,
353,1216,1,138,1188,
1,886,1515,16,0,
140,1,27,1047,1,
856,1184,1,669,1516,
16,0,140,1,14,
1517,16,0,140,1,
15,1052,1,20,1070,
1,17,1060,1,119,
1076,1,19,1069,1,
418,1154,1,121,1089,
1,125,1072,1,124,
1064,1,123,1081,1,
122,1085,1,977,1009,
1,120,1093,1,975,
1097,1,974,1518,16,
0,140,1,741,1519,
16,0,140,1,9,
1104,1,2,1117,1,
542,1230,1,834,1205,
1,833,1209,1,521,
1242,1,520,1293,1,
430,1145,1,311,988,
1,428,1149,1,40,
1520,19,164,1,40,
1521,5,71,1,855,
1167,1,723,1323,1,
103,1218,1,510,1522,
16,0,162,1,953,
1122,1,91,999,1,
522,1364,1,414,1159,
1,92,994,1,412,
1163,1,88,1004,1,
807,1523,16,0,162,
1,619,1180,1,725,
1524,16,0,162,1,
403,1172,1,700,1128,
1,977,1009,1,944,
1525,16,0,162,1,
284,1015,1,282,1020,
1,741,1526,16,0,
162,1,595,1527,16,
0,162,1,593,1212,
1,161,1026,1,569,
1528,16,0,162,1,
0,1529,16,0,162,
1,369,1194,1,581,
1530,16,0,162,1,
44,1033,1,685,1531,
16,0,162,1,327,
1108,1,683,1138,1,
896,1133,1,12,1055,
1,357,1201,1,1,
1332,1,355,1198,1,
33,1040,1,353,1216,
1,138,1188,1,886,
1532,16,0,162,1,
27,1047,1,856,1184,
1,669,1533,16,0,
162,1,14,1534,16,
0,162,1,3,1113,
1,20,1070,1,15,
1052,1,119,1076,1,
17,1060,1,418,1154,
1,19,1069,1,125,
1072,1,124,1064,1,
123,1081,1,122,1085,
1,121,1089,1,120,
1093,1,975,1097,1,
974,1535,16,0,162,
1,10,1100,1,9,
1104,1,2,1117,1,
542,1230,1,834,1205,
1,833,1209,1,521,
1242,1,520,1293,1,
430,1145,1,311,988,
1,428,1149,1,39,
1536,19,197,1,39,
1537,5,71,1,855,
1167,1,723,1323,1,
103,1218,1,510,1538,
16,0,195,1,953,
1122,1,91,999,1,
522,1364,1,414,1159,
1,92,994,1,412,
1163,1,88,1004,1,
807,1539,16,0,195,
1,619,1180,1,725,
1540,16,0,195,1,
403,1172,1,700,1128,
1,977,1009,1,944,
1541,16,0,195,1,
284,1015,1,282,1020,
1,741,1542,16,0,
195,1,595,1543,16,
0,195,1,593,1212,
1,161,1026,1,569,
1544,16,0,195,1,
0,1545,16,0,195,
1,369,1194,1,581,
1546,16,0,195,1,
44,1033,1,685,1547,
16,0,195,1,327,
1108,1,683,1138,1,
896,1133,1,12,1055,
1,357,1201,1,1,
1332,1,355,1198,1,
33,1040,1,353,1216,
1,138,1188,1,886,
1548,16,0,195,1,
27,1047,1,856,1184,
1,669,1549,16,0,
195,1,14,1550,16,
0,195,1,3,1113,
1,20,1070,1,15,
1052,1,119,1076,1,
17,1060,1,418,1154,
1,19,1069,1,125,
1072,1,124,1064,1,
123,1081,1,122,1085,
1,121,1089,1,120,
1093,1,975,1097,1,
974,1551,16,0,195,
1,10,1100,1,9,
1104,1,2,1117,1,
542,1230,1,834,1205,
1,833,1209,1,521,
1242,1,520,1293,1,
430,1145,1,311,988,
1,428,1149,1,38,
1552,19,108,1,38,
1553,5,74,1,855,
1167,1,723,1323,1,
103,1218,1,741,1302,
1,953,1122,1,952,
1554,16,0,106,1,
91,999,1,522,1364,
1,414,1159,1,92,
994,1,412,1163,1,
88,1004,1,834,1205,
1,619,1180,1,725,
1314,1,403,1172,1,
700,1128,1,699,1555,
16,0,127,1,976,
1556,16,0,205,1,
284,1015,1,282,1020,
1,813,1557,16,0,
191,1,682,1558,16,
0,144,1,593,1212,
1,592,1559,16,0,
220,1,161,1026,1,
2,1117,1,369,1194,
1,977,1009,1,580,
1560,16,0,218,1,
44,1033,1,974,1561,
16,0,206,1,12,
1055,1,683,1138,1,
896,1133,1,895,1562,
16,0,139,1,519,
1563,16,0,293,1,
357,1201,1,1,1332,
1,355,1198,1,33,
1040,1,353,1216,1,
138,1188,1,14,1564,
16,0,398,1,3,
1113,1,856,1184,1,
27,1047,1,16,1565,
16,0,396,1,15,
1052,1,20,1070,1,
724,1318,1,119,1076,
1,17,1060,1,418,
1154,1,19,1069,1,
125,1072,1,124,1064,
1,123,1081,1,122,
1085,1,121,1089,1,
120,1093,1,975,1097,
1,760,1288,1,10,
1100,1,9,1104,1,
757,1297,1,542,1230,
1,327,1108,1,833,
1209,1,521,1242,1,
520,1293,1,430,1145,
1,311,988,1,428,
1149,1,37,1566,19,
216,1,37,1567,5,
63,1,855,1167,1,
723,1323,1,103,1218,
1,741,1302,1,953,
1122,1,91,999,1,
522,1364,1,414,1159,
1,92,994,1,412,
1163,1,88,1004,1,
834,1205,1,619,1180,
1,725,1314,1,403,
1172,1,700,1128,1,
284,1015,1,282,1020,
1,593,1212,1,161,
1026,1,369,1194,1,
977,1009,1,580,1568,
16,0,214,1,44,
1033,1,683,1138,1,
896,1133,1,357,1201,
1,1,1332,1,355,
1198,1,33,1040,1,
353,1216,1,138,1188,
1,2,1117,1,3,
1113,1,856,1184,1,
27,1047,1,12,1055,
1,15,1052,1,20,
1070,1,724,1318,1,
119,1076,1,17,1060,
1,418,1154,1,19,
1069,1,125,1072,1,
124,1064,1,123,1081,
1,122,1085,1,121,
1089,1,120,1093,1,
975,1097,1,760,1288,
1,10,1100,1,9,
1104,1,757,1297,1,
542,1230,1,327,1108,
1,833,1209,1,521,
1242,1,520,1293,1,
430,1145,1,311,988,
1,428,1149,1,36,
1569,19,213,1,36,
1570,5,63,1,855,
1167,1,723,1323,1,
103,1218,1,741,1302,
1,953,1122,1,91,
999,1,522,1364,1,
414,1159,1,92,994,
1,412,1163,1,88,
1004,1,834,1205,1,
619,1180,1,725,1314,
1,403,1172,1,700,
1128,1,284,1015,1,
282,1020,1,593,1212,
1,161,1026,1,369,
1194,1,977,1009,1,
580,1571,16,0,211,
1,44,1033,1,683,
1138,1,896,1133,1,
357,1201,1,1,1332,
1,355,1198,1,33,
1040,1,353,1216,1,
138,1188,1,2,1117,
1,3,1113,1,856,
1184,1,27,1047,1,
12,1055,1,15,1052,
1,20,1070,1,724,
1318,1,119,1076,1,
17,1060,1,418,1154,
1,19,1069,1,125,
1072,1,124,1064,1,
123,1081,1,122,1085,
1,121,1089,1,120,
1093,1,975,1097,1,
760,1288,1,10,1100,
1,9,1104,1,757,
1297,1,542,1230,1,
327,1108,1,833,1209,
1,521,1242,1,520,
1293,1,430,1145,1,
311,988,1,428,1149,
1,35,1572,19,210,
1,35,1573,5,33,
1,327,1108,1,138,
1188,1,88,1004,1,
792,1574,16,0,208,
1,27,1047,1,977,
1009,1,975,1097,1,
33,1040,1,125,1072,
1,124,1064,1,123,
1081,1,122,1085,1,
121,1089,1,120,1093,
1,119,1076,1,20,
1070,1,161,1026,1,
19,1069,1,17,1060,
1,10,1100,1,15,
1052,1,12,1055,1,
284,1015,1,9,1104,
1,282,1020,1,103,
1218,1,2,1117,1,
3,1113,1,311,988,
1,44,1033,1,92,
994,1,91,999,1,
554,1575,16,0,256,
1,34,1576,19,288,
1,34,1577,5,71,
1,855,1167,1,723,
1323,1,103,1218,1,
510,1578,16,0,286,
1,953,1122,1,91,
999,1,522,1364,1,
414,1159,1,92,994,
1,412,1163,1,88,
1004,1,807,1579,16,
0,286,1,619,1180,
1,725,1580,16,0,
286,1,403,1172,1,
700,1128,1,977,1009,
1,944,1581,16,0,
286,1,284,1015,1,
282,1020,1,741,1582,
16,0,286,1,595,
1583,16,0,286,1,
593,1212,1,161,1026,
1,569,1584,16,0,
286,1,0,1585,16,
0,286,1,369,1194,
1,581,1586,16,0,
286,1,44,1033,1,
685,1587,16,0,286,
1,327,1108,1,683,
1138,1,896,1133,1,
12,1055,1,357,1201,
1,1,1332,1,355,
1198,1,33,1040,1,
353,1216,1,138,1188,
1,886,1588,16,0,
286,1,27,1047,1,
856,1184,1,669,1589,
16,0,286,1,14,
1590,16,0,286,1,
3,1113,1,20,1070,
1,15,1052,1,119,
1076,1,17,1060,1,
418,1154,1,19,1069,
1,125,1072,1,124,
1064,1,123,1081,1,
122,1085,1,121,1089,
1,120,1093,1,975,
1097,1,974,1591,16,
0,286,1,10,1100,
1,9,1104,1,2,
1117,1,542,1230,1,
834,1205,1,833,1209,
1,521,1242,1,520,
1293,1,430,1145,1,
311,988,1,428,1149,
1,33,1592,19,123,
1,33,1593,5,12,
1,20,1594,17,1595,
15,1596,4,16,37,
0,118,0,97,0,
114,0,108,0,105,
0,115,0,116,0,
1,-1,1,5,1597,
20,640,1,136,1,
3,1,2,1,1,
1598,22,1,54,1,
19,1069,1,430,1599,
16,0,158,1,198,
1600,16,0,150,1,
702,1601,16,0,121,
1,357,1602,16,0,
222,1,355,1198,1,
353,1216,1,327,1108,
1,27,1047,1,347,
1603,17,1604,15,1596,
1,-1,1,5,1605,
20,658,1,135,1,
3,1,4,1,3,
1606,22,1,53,1,
161,1607,16,0,200,
1,32,1608,19,409,
1,32,1609,5,102,
1,953,1122,1,703,
1610,16,0,407,1,
700,1128,1,458,1611,
16,0,407,1,896,
1133,1,923,1612,16,
0,407,1,683,1138,
1,199,1613,16,0,
407,1,196,1614,16,
0,407,1,432,1615,
16,0,407,1,430,
1145,1,428,1149,1,
418,1154,1,414,1159,
1,412,1163,1,855,
1167,1,643,1616,16,
0,407,1,403,1172,
1,162,1617,16,0,
407,1,161,1026,1,
608,1618,16,0,407,
1,384,1619,16,0,
407,1,619,1180,1,
856,1184,1,138,1188,
1,358,1620,16,0,
407,1,369,1194,1,
355,1198,1,125,1072,
1,124,1064,1,123,
1081,1,122,1085,1,
121,1089,1,120,1093,
1,119,1076,1,357,
1201,1,834,1205,1,
833,1209,1,593,1212,
1,353,1216,1,103,
1218,1,92,994,1,
813,1621,16,0,407,
1,93,1622,16,0,
407,1,88,1004,1,
91,999,1,781,1623,
16,0,407,1,327,
1108,1,282,1020,1,
311,988,1,288,1624,
16,0,407,1,44,
1033,1,67,1625,16,
0,407,1,45,1626,
16,0,407,1,543,
1627,16,0,407,1,
542,1230,1,48,1235,
1,47,1239,1,521,
1242,1,60,1628,16,
0,407,1,59,1247,
1,58,1250,1,57,
1253,1,56,1256,1,
55,1259,1,54,1262,
1,53,1265,1,52,
1268,1,51,1271,1,
50,1274,1,49,1277,
1,287,1280,1,286,
1284,1,284,1015,1,
522,1629,16,0,407,
1,760,1288,1,520,
1293,1,757,1297,1,
33,1040,1,28,1630,
16,0,407,1,27,
1047,1,19,1069,1,
741,1302,1,484,1631,
16,0,407,1,977,
1009,1,20,1070,1,
975,1097,1,17,1060,
1,15,1052,1,6,
1307,1,12,1055,1,
7,1311,1,10,1100,
1,9,1104,1,725,
1314,1,724,1318,1,
723,1323,1,5,1328,
1,4,1632,16,0,
407,1,3,1113,1,
2,1117,1,1,1332,
1,29,1633,19,359,
1,29,1634,5,45,
1,103,1635,16,0,
357,1,313,1636,16,
0,357,1,311,988,
1,200,1637,16,0,
357,1,92,994,1,
91,999,1,197,1638,
16,0,357,1,88,
1004,1,619,1639,16,
0,357,1,977,1009,
1,74,1640,16,0,
357,1,284,1015,1,
282,1020,1,495,1641,
16,0,357,1,163,
1642,16,0,357,1,
161,1026,1,369,1643,
16,0,357,1,46,
1644,16,0,357,1,
44,1033,1,792,1645,
16,0,357,1,469,
1646,16,0,357,1,
654,1647,16,0,357,
1,33,1040,1,138,
1648,16,0,357,1,
443,1649,16,0,357,
1,27,1047,1,15,
1052,1,12,1055,1,
17,1060,1,124,1064,
1,19,1069,1,20,
1070,1,554,1650,16,
0,357,1,125,1072,
1,119,1076,1,123,
1081,1,122,1085,1,
121,1089,1,120,1093,
1,975,1097,1,10,
1100,1,9,1104,1,
327,1108,1,3,1113,
1,2,1117,1,28,
1651,19,356,1,28,
1652,5,45,1,103,
1653,16,0,354,1,
313,1654,16,0,354,
1,311,988,1,200,
1655,16,0,354,1,
92,994,1,91,999,
1,197,1656,16,0,
354,1,88,1004,1,
619,1657,16,0,354,
1,977,1009,1,74,
1658,16,0,354,1,
284,1015,1,282,1020,
1,495,1659,16,0,
354,1,163,1660,16,
0,354,1,161,1026,
1,369,1661,16,0,
354,1,46,1662,16,
0,354,1,44,1033,
1,792,1663,16,0,
354,1,469,1664,16,
0,354,1,654,1665,
16,0,354,1,33,
1040,1,138,1666,16,
0,354,1,443,1667,
16,0,354,1,27,
1047,1,15,1052,1,
12,1055,1,17,1060,
1,124,1064,1,19,
1069,1,20,1070,1,
554,1668,16,0,354,
1,125,1072,1,119,
1076,1,123,1081,1,
122,1085,1,121,1089,
1,120,1093,1,975,
1097,1,10,1100,1,
9,1104,1,327,1108,
1,3,1113,1,2,
1117,1,27,1669,19,
362,1,27,1670,5,
45,1,103,1671,16,
0,360,1,313,1672,
16,0,360,1,311,
988,1,200,1673,16,
0,360,1,92,994,
1,91,999,1,197,
1674,16,0,360,1,
88,1004,1,619,1675,
16,0,360,1,977,
1009,1,74,1676,16,
0,360,1,284,1015,
1,282,1020,1,495,
1677,16,0,360,1,
163,1678,16,0,360,
1,161,1026,1,369,
1679,16,0,360,1,
46,1680,16,0,360,
1,44,1033,1,792,
1681,16,0,360,1,
469,1682,16,0,360,
1,654,1683,16,0,
360,1,33,1040,1,
138,1684,16,0,360,
1,443,1685,16,0,
360,1,27,1047,1,
15,1052,1,12,1055,
1,17,1060,1,124,
1064,1,19,1069,1,
20,1070,1,554,1686,
16,0,360,1,125,
1072,1,119,1076,1,
123,1081,1,122,1085,
1,121,1089,1,120,
1093,1,975,1097,1,
10,1100,1,9,1104,
1,327,1108,1,3,
1113,1,2,1117,1,
26,1687,19,353,1,
26,1688,5,45,1,
103,1689,16,0,351,
1,313,1690,16,0,
351,1,311,988,1,
200,1691,16,0,351,
1,92,994,1,91,
999,1,197,1692,16,
0,351,1,88,1004,
1,619,1693,16,0,
351,1,977,1009,1,
74,1694,16,0,351,
1,284,1015,1,282,
1020,1,495,1695,16,
0,351,1,163,1696,
16,0,351,1,161,
1026,1,369,1697,16,
0,351,1,46,1698,
16,0,351,1,44,
1033,1,792,1699,16,
0,351,1,469,1700,
16,0,351,1,654,
1701,16,0,351,1,
33,1040,1,138,1702,
16,0,351,1,443,
1703,16,0,351,1,
27,1047,1,15,1052,
1,12,1055,1,17,
1060,1,124,1064,1,
19,1069,1,20,1070,
1,554,1704,16,0,
351,1,125,1072,1,
119,1076,1,123,1081,
1,122,1085,1,121,
1089,1,120,1093,1,
975,1097,1,10,1100,
1,9,1104,1,327,
1108,1,3,1113,1,
2,1117,1,25,1705,
19,368,1,25,1706,
5,45,1,103,1707,
16,0,366,1,313,
1708,16,0,366,1,
311,988,1,200,1709,
16,0,366,1,92,
994,1,91,999,1,
197,1710,16,0,366,
1,88,1004,1,619,
1711,16,0,366,1,
977,1009,1,74,1712,
16,0,366,1,284,
1015,1,282,1020,1,
495,1713,16,0,366,
1,163,1714,16,0,
366,1,161,1026,1,
369,1715,16,0,366,
1,46,1716,16,0,
366,1,44,1033,1,
792,1717,16,0,366,
1,469,1718,16,0,
366,1,654,1719,16,
0,366,1,33,1040,
1,138,1720,16,0,
366,1,443,1721,16,
0,366,1,27,1047,
1,15,1052,1,12,
1055,1,17,1060,1,
124,1064,1,19,1069,
1,20,1070,1,554,
1722,16,0,366,1,
125,1072,1,119,1076,
1,123,1081,1,122,
1085,1,121,1089,1,
120,1093,1,975,1097,
1,10,1100,1,9,
1104,1,327,1108,1,
3,1113,1,2,1117,
1,24,1723,19,365,
1,24,1724,5,45,
1,103,1725,16,0,
363,1,313,1726,16,
0,363,1,311,988,
1,200,1727,16,0,
363,1,92,994,1,
91,999,1,197,1728,
16,0,363,1,88,
1004,1,619,1729,16,
0,363,1,977,1009,
1,74,1730,16,0,
363,1,284,1015,1,
282,1020,1,495,1731,
16,0,363,1,163,
1732,16,0,363,1,
161,1026,1,369,1733,
16,0,363,1,46,
1734,16,0,363,1,
44,1033,1,792,1735,
16,0,363,1,469,
1736,16,0,363,1,
654,1737,16,0,363,
1,33,1040,1,138,
1738,16,0,363,1,
443,1739,16,0,363,
1,27,1047,1,15,
1052,1,12,1055,1,
17,1060,1,124,1064,
1,19,1069,1,20,
1070,1,554,1740,16,
0,363,1,125,1072,
1,119,1076,1,123,
1081,1,122,1085,1,
121,1089,1,120,1093,
1,975,1097,1,10,
1100,1,9,1104,1,
327,1108,1,3,1113,
1,2,1117,1,23,
1741,19,347,1,23,
1742,5,45,1,103,
1743,16,0,345,1,
313,1744,16,0,345,
1,311,988,1,200,
1745,16,0,345,1,
92,994,1,91,999,
1,197,1746,16,0,
345,1,88,1004,1,
619,1747,16,0,345,
1,977,1009,1,74,
1748,16,0,345,1,
284,1015,1,282,1020,
1,495,1749,16,0,
345,1,163,1750,16,
0,345,1,161,1026,
1,369,1751,16,0,
345,1,46,1752,16,
0,345,1,44,1033,
1,792,1753,16,0,
345,1,469,1754,16,
0,345,1,654,1755,
16,0,345,1,33,
1040,1,138,1756,16,
0,345,1,443,1757,
16,0,345,1,27,
1047,1,15,1052,1,
12,1055,1,17,1060,
1,124,1064,1,19,
1069,1,20,1070,1,
554,1758,16,0,345,
1,125,1072,1,119,
1076,1,123,1081,1,
122,1085,1,121,1089,
1,120,1093,1,975,
1097,1,10,1100,1,
9,1104,1,327,1108,
1,3,1113,1,2,
1117,1,22,1759,19,
344,1,22,1760,5,
45,1,103,1761,16,
0,342,1,313,1762,
16,0,342,1,311,
988,1,200,1763,16,
0,342,1,92,994,
1,91,999,1,197,
1764,16,0,342,1,
88,1004,1,619,1765,
16,0,342,1,977,
1009,1,74,1766,16,
0,342,1,284,1015,
1,282,1020,1,495,
1767,16,0,342,1,
163,1768,16,0,342,
1,161,1026,1,369,
1769,16,0,342,1,
46,1770,16,0,342,
1,44,1033,1,792,
1771,16,0,342,1,
469,1772,16,0,342,
1,654,1773,16,0,
342,1,33,1040,1,
138,1774,16,0,342,
1,443,1775,16,0,
342,1,27,1047,1,
15,1052,1,12,1055,
1,17,1060,1,124,
1064,1,19,1069,1,
20,1070,1,554,1776,
16,0,342,1,125,
1072,1,119,1076,1,
123,1081,1,122,1085,
1,121,1089,1,120,
1093,1,975,1097,1,
10,1100,1,9,1104,
1,327,1108,1,3,
1113,1,2,1117,1,
21,1777,19,341,1,
21,1778,5,45,1,
103,1779,16,0,339,
1,313,1780,16,0,
339,1,311,988,1,
200,1781,16,0,339,
1,92,994,1,91,
999,1,197,1782,16,
0,339,1,88,1004,
1,619,1783,16,0,
339,1,977,1009,1,
74,1784,16,0,339,
1,284,1015,1,282,
1020,1,495,1785,16,
0,339,1,163,1786,
16,0,339,1,161,
1026,1,369,1787,16,
0,339,1,46,1788,
16,0,339,1,44,
1033,1,792,1789,16,
0,339,1,469,1790,
16,0,339,1,654,
1791,16,0,339,1,
33,1040,1,138,1792,
16,0,339,1,443,
1793,16,0,339,1,
27,1047,1,15,1052,
1,12,1055,1,17,
1060,1,124,1064,1,
19,1069,1,20,1070,
1,554,1794,16,0,
339,1,125,1072,1,
119,1076,1,123,1081,
1,122,1085,1,121,
1089,1,120,1093,1,
975,1097,1,10,1100,
1,9,1104,1,327,
1108,1,3,1113,1,
2,1117,1,20,1795,
19,412,1,20,1796,
5,102,1,953,1122,
1,703,1797,16,0,
410,1,700,1128,1,
458,1798,16,0,410,
1,896,1133,1,923,
1799,16,0,410,1,
683,1138,1,199,1800,
16,0,410,1,196,
1801,16,0,410,1,
432,1802,16,0,410,
1,430,1145,1,428,
1149,1,418,1154,1,
414,1159,1,412,1163,
1,855,1167,1,643,
1803,16,0,410,1,
403,1172,1,162,1804,
16,0,410,1,161,
1026,1,608,1805,16,
0,410,1,384,1806,
16,0,410,1,619,
1180,1,856,1184,1,
138,1188,1,358,1807,
16,0,410,1,369,
1194,1,355,1198,1,
125,1072,1,124,1064,
1,123,1081,1,122,
1085,1,121,1089,1,
120,1093,1,119,1076,
1,357,1201,1,834,
1205,1,833,1209,1,
593,1212,1,353,1216,
1,103,1218,1,92,
994,1,813,1808,16,
0,410,1,93,1809,
16,0,410,1,88,
1004,1,91,999,1,
781,1810,16,0,410,
1,327,1108,1,282,
1020,1,311,988,1,
288,1811,16,0,410,
1,44,1033,1,67,
1812,16,0,410,1,
45,1813,16,0,410,
1,543,1814,16,0,
410,1,542,1230,1,
48,1235,1,47,1239,
1,521,1242,1,60,
1815,16,0,410,1,
59,1247,1,58,1250,
1,57,1253,1,56,
1256,1,55,1259,1,
54,1262,1,53,1265,
1,52,1268,1,51,
1271,1,50,1274,1,
49,1277,1,287,1280,
1,286,1284,1,284,
1015,1,522,1816,16,
0,410,1,760,1288,
1,520,1293,1,757,
1297,1,33,1040,1,
28,1817,16,0,410,
1,27,1047,1,19,
1069,1,741,1302,1,
484,1818,16,0,410,
1,977,1009,1,20,
1070,1,975,1097,1,
17,1060,1,15,1052,
1,6,1307,1,12,
1055,1,7,1311,1,
10,1100,1,9,1104,
1,725,1314,1,724,
1318,1,723,1323,1,
5,1328,1,4,1819,
16,0,410,1,3,
1113,1,2,1117,1,
1,1332,1,19,1820,
19,338,1,19,1821,
5,45,1,103,1822,
16,0,336,1,313,
1823,16,0,336,1,
311,988,1,200,1824,
16,0,336,1,92,
994,1,91,999,1,
197,1825,16,0,336,
1,88,1004,1,619,
1826,16,0,336,1,
977,1009,1,74,1827,
16,0,336,1,284,
1015,1,282,1020,1,
495,1828,16,0,336,
1,163,1829,16,0,
336,1,161,1026,1,
369,1830,16,0,336,
1,46,1831,16,0,
336,1,44,1033,1,
792,1832,16,0,336,
1,469,1833,16,0,
336,1,654,1834,16,
0,336,1,33,1040,
1,138,1835,16,0,
336,1,443,1836,16,
0,336,1,27,1047,
1,15,1052,1,12,
1055,1,17,1060,1,
124,1064,1,19,1069,
1,20,1070,1,554,
1837,16,0,336,1,
125,1072,1,119,1076,
1,123,1081,1,122,
1085,1,121,1089,1,
120,1093,1,975,1097,
1,10,1100,1,9,
1104,1,327,1108,1,
3,1113,1,2,1117,
1,18,1838,19,335,
1,18,1839,5,114,
1,953,1122,1,469,
1840,16,0,333,1,
703,1841,16,0,406,
1,700,1128,1,458,
1842,16,0,406,1,
923,1843,16,0,406,
1,683,1138,1,443,
1844,16,0,333,1,
200,1845,16,0,333,
1,199,1846,16,0,
406,1,197,1847,16,
0,333,1,196,1848,
16,0,406,1,418,
1154,1,432,1849,16,
0,406,1,430,1145,
1,428,1149,1,896,
1133,1,654,1850,16,
0,333,1,414,1159,
1,412,1163,1,855,
1167,1,643,1851,16,
0,406,1,403,1172,
1,163,1852,16,0,
333,1,162,1853,16,
0,406,1,161,1026,
1,608,1854,16,0,
406,1,384,1855,16,
0,406,1,833,1209,
1,619,1856,16,0,
333,1,856,1184,1,
138,1857,16,0,333,
1,358,1858,16,0,
406,1,369,1859,16,
0,333,1,125,1072,
1,124,1064,1,123,
1081,1,122,1085,1,
121,1089,1,120,1093,
1,119,1076,1,357,
1201,1,834,1205,1,
355,1198,1,593,1212,
1,353,1216,1,103,
1860,16,0,333,1,
92,994,1,813,1861,
16,0,406,1,93,
1862,16,0,406,1,
88,1004,1,91,999,
1,781,1863,16,0,
406,1,327,1108,1,
313,1864,16,0,333,
1,554,1865,16,0,
333,1,792,1866,16,
0,333,1,74,1867,
16,0,333,1,282,
1020,1,311,988,1,
49,1277,1,44,1033,
1,67,1868,16,0,
406,1,45,1869,16,
0,406,1,543,1870,
16,0,406,1,542,
1230,1,48,1235,1,
47,1239,1,521,1242,
1,60,1871,16,0,
406,1,59,1247,1,
58,1250,1,57,1253,
1,56,1256,1,55,
1259,1,54,1262,1,
53,1265,1,52,1268,
1,51,1271,1,50,
1274,1,288,1872,16,
0,406,1,287,1280,
1,286,1284,1,46,
1873,16,0,333,1,
284,1015,1,522,1874,
16,0,406,1,760,
1288,1,520,1293,1,
757,1297,1,33,1040,
1,17,1060,1,28,
1875,16,0,406,1,
27,1047,1,19,1069,
1,741,1302,1,484,
1876,16,0,406,1,
977,1009,1,20,1070,
1,975,1097,1,495,
1877,16,0,333,1,
15,1052,1,6,1307,
1,12,1055,1,7,
1311,1,10,1100,1,
9,1104,1,725,1314,
1,724,1318,1,723,
1323,1,5,1328,1,
4,1878,16,0,406,
1,3,1113,1,2,
1117,1,1,1332,1,
17,1879,19,332,1,
17,1880,5,45,1,
103,1881,16,0,330,
1,313,1882,16,0,
330,1,311,988,1,
200,1883,16,0,330,
1,92,994,1,91,
999,1,197,1884,16,
0,330,1,88,1004,
1,619,1885,16,0,
330,1,977,1009,1,
74,1886,16,0,330,
1,284,1015,1,282,
1020,1,495,1887,16,
0,330,1,163,1888,
16,0,330,1,161,
1026,1,369,1889,16,
0,330,1,46,1890,
16,0,330,1,44,
1033,1,792,1891,16,
0,330,1,469,1892,
16,0,330,1,654,
1893,16,0,330,1,
33,1040,1,138,1894,
16,0,330,1,443,
1895,16,0,330,1,
27,1047,1,15,1052,
1,12,1055,1,17,
1060,1,124,1064,1,
19,1069,1,20,1070,
1,554,1896,16,0,
330,1,125,1072,1,
119,1076,1,123,1081,
1,122,1085,1,121,
1089,1,120,1093,1,
975,1097,1,10,1100,
1,9,1104,1,327,
1108,1,3,1113,1,
2,1117,1,16,1897,
19,174,1,16,1898,
5,20,1,92,994,
1,420,1899,16,0,
172,1,88,1004,1,
33,1900,16,0,388,
1,311,988,1,27,
1047,1,20,1070,1,
22,1901,16,0,388,
1,161,1026,1,19,
1069,1,10,1100,1,
327,1108,1,9,1104,
1,1,1902,16,0,
388,1,3,1113,1,
2,1117,1,44,1033,
1,284,1015,1,91,
999,1,282,1020,1,
15,1903,19,319,1,
15,1904,5,38,1,
103,1218,1,311,988,
1,309,1905,17,1906,
15,1907,4,20,37,
0,102,0,105,0,
101,0,108,0,100,
0,108,0,105,0,
115,0,116,0,1,
-1,1,5,1908,20,
839,1,113,1,3,
1,4,1,3,1909,
22,1,27,1,200,
1910,17,1911,15,1912,
4,30,37,0,70,
0,105,0,101,0,
108,0,100,0,69,
0,120,0,112,0,
65,0,115,0,115,
0,105,0,103,0,
110,0,1,-1,1,
5,1913,20,557,1,
146,1,3,1,6,
1,5,1914,22,1,
88,1,92,994,1,
91,999,1,88,1004,
1,975,1097,1,285,
1915,17,1916,15,1907,
1,-1,1,5,1917,
20,845,1,111,1,
3,1,2,1,1,
1918,22,1,26,1,
284,1015,1,282,1020,
1,281,1919,16,0,
374,1,163,1920,17,
1921,15,1922,4,24,
37,0,70,0,105,
0,101,0,108,0,
100,0,65,0,115,
0,115,0,105,0,
103,0,110,0,1,
-1,1,5,1923,20,
555,1,147,1,3,
1,4,1,3,1924,
22,1,89,1,161,
1026,1,46,1925,17,
1926,15,1927,4,12,
37,0,102,0,105,
0,101,0,108,0,
100,0,1,-1,1,
5,1928,20,550,1,
148,1,3,1,2,
1,1,1929,22,1,
90,1,977,1009,1,
45,1930,16,0,317,
1,44,1033,1,33,
1040,1,138,1188,1,
27,1047,1,122,1085,
1,12,1055,1,124,
1064,1,20,1070,1,
19,1069,1,125,1072,
1,17,1060,1,123,
1081,1,15,1052,1,
121,1089,1,120,1093,
1,119,1076,1,10,
1100,1,9,1104,1,
327,1108,1,3,1113,
1,2,1117,1,14,
1931,19,372,1,14,
1932,5,104,1,953,
1122,1,703,1933,16,
0,370,1,700,1128,
1,458,1934,16,0,
370,1,896,1133,1,
923,1935,16,0,370,
1,683,1138,1,199,
1936,16,0,370,1,
196,1937,16,0,370,
1,432,1938,16,0,
370,1,430,1145,1,
428,1149,1,418,1154,
1,414,1159,1,412,
1163,1,855,1167,1,
643,1939,16,0,370,
1,403,1172,1,162,
1940,16,0,370,1,
161,1026,1,608,1941,
16,0,370,1,384,
1942,16,0,370,1,
619,1180,1,856,1184,
1,138,1188,1,358,
1943,16,0,370,1,
369,1194,1,355,1198,
1,125,1072,1,124,
1064,1,123,1081,1,
122,1085,1,121,1089,
1,120,1093,1,119,
1076,1,357,1201,1,
834,1205,1,833,1209,
1,593,1212,1,353,
1216,1,103,1218,1,
92,994,1,813,1944,
16,0,370,1,93,
1945,16,0,370,1,
88,1004,1,91,999,
1,781,1946,16,0,
370,1,327,1108,1,
282,1020,1,311,988,
1,288,1947,16,0,
370,1,44,1033,1,
67,1948,16,0,370,
1,45,1949,16,0,
370,1,543,1950,16,
0,370,1,542,1230,
1,48,1235,1,47,
1239,1,521,1242,1,
60,1951,16,0,370,
1,59,1247,1,58,
1250,1,57,1253,1,
56,1256,1,55,1259,
1,54,1262,1,53,
1265,1,52,1268,1,
51,1271,1,50,1274,
1,49,1277,1,287,
1280,1,286,1284,1,
40,1952,16,0,370,
1,284,1015,1,522,
1953,16,0,370,1,
760,1288,1,520,1293,
1,757,1297,1,33,
1954,16,0,370,1,
28,1955,16,0,370,
1,27,1047,1,19,
1069,1,741,1302,1,
484,1956,16,0,370,
1,22,1957,16,0,
370,1,977,1009,1,
20,1070,1,975,1097,
1,17,1060,1,15,
1052,1,6,1307,1,
12,1055,1,7,1311,
1,10,1100,1,9,
1104,1,725,1314,1,
724,1318,1,723,1323,
1,5,1328,1,4,
1958,16,0,370,1,
3,1113,1,2,1117,
1,1,1959,16,0,
370,1,13,1960,19,
157,1,13,1961,5,
36,1,103,1218,1,
313,1962,16,0,267,
1,311,988,1,92,
994,1,91,999,1,
88,1004,1,975,1097,
1,288,1963,16,0,
155,1,287,1280,1,
286,1284,1,284,1015,
1,282,1020,1,161,
1026,1,977,1009,1,
45,1964,16,0,155,
1,44,1033,1,33,
1040,1,138,1188,1,
27,1047,1,124,1064,
1,122,1085,1,119,
1076,1,20,1070,1,
19,1069,1,125,1072,
1,17,1060,1,123,
1081,1,15,1052,1,
121,1089,1,120,1093,
1,12,1055,1,10,
1100,1,9,1104,1,
327,1108,1,3,1113,
1,2,1117,1,12,
1965,19,153,1,12,
1966,5,34,1,103,
1218,1,311,988,1,
92,994,1,91,999,
1,197,1967,16,0,
151,1,88,1004,1,
975,1097,1,284,1015,
1,282,1020,1,161,
1026,1,977,1009,1,
44,1033,1,33,1968,
16,0,386,1,124,
1064,1,138,1188,1,
27,1047,1,119,1076,
1,122,1085,1,22,
1969,16,0,386,1,
20,1070,1,19,1069,
1,125,1072,1,17,
1060,1,123,1081,1,
15,1052,1,121,1089,
1,120,1093,1,12,
1055,1,10,1100,1,
9,1104,1,327,1108,
1,3,1113,1,2,
1117,1,1,1970,16,
0,386,1,11,1971,
19,283,1,11,1972,
5,41,1,103,1218,
1,311,988,1,92,
994,1,91,999,1,
88,1004,1,403,1172,
1,975,1097,1,74,
1973,16,0,281,1,
284,1015,1,282,1020,
1,996,1974,17,1975,
15,1976,4,16,37,
0,112,0,97,0,
114,0,108,0,105,
0,115,0,116,0,
1,-1,1,5,1977,
20,805,1,116,1,
3,1,2,1,1,
1978,22,1,30,1,
161,1026,1,369,1194,
1,977,1009,1,44,
1033,1,999,1979,17,
1980,15,1976,1,-1,
1,5,1981,20,803,
1,117,1,3,1,
4,1,3,1982,22,
1,31,1,33,1040,
1,995,1983,17,1984,
15,1976,1,-1,1,
5,258,1,1,1,
1,1985,22,1,32,
1,138,1188,1,27,
1047,1,10,1100,1,
122,1085,1,12,1055,
1,124,1064,1,13,
1986,16,0,399,1,
20,1070,1,19,1069,
1,125,1072,1,17,
1060,1,123,1081,1,
15,1052,1,121,1089,
1,120,1093,1,119,
1076,1,973,1987,16,
0,395,1,9,1104,
1,8,1988,16,0,
404,1,327,1108,1,
4,1989,16,0,403,
1,3,1113,1,2,
1117,1,10,1990,19,
313,1,10,1991,5,
123,1,953,1122,1,
944,1992,16,0,311,
1,703,1993,16,0,
311,1,700,1128,1,
458,1994,16,0,311,
1,669,1995,16,0,
311,1,685,1996,16,
0,311,1,923,1997,
16,0,311,1,683,
1138,1,426,1998,16,
0,400,1,199,1999,
16,0,311,1,196,
2000,16,0,311,1,
418,1154,1,432,2001,
16,0,311,1,430,
1145,1,428,1149,1,
412,1163,1,425,2002,
17,2003,15,2004,4,
18,37,0,102,0,
117,0,110,0,99,
0,110,0,97,0,
109,0,101,0,1,
-1,1,5,169,1,
3,1,3,2005,22,
1,58,1,422,2006,
17,2007,15,2004,1,
-1,1,5,169,1,
3,1,3,2008,22,
1,59,1,420,2009,
17,2010,15,2004,1,
-1,1,5,2011,20,
593,1,140,1,3,
1,2,1,1,2012,
22,1,60,1,896,
1133,1,416,2013,16,
0,400,1,414,1159,
1,834,1205,1,886,
2014,16,0,311,1,
855,1167,1,643,2015,
16,0,311,1,403,
1172,1,162,2016,16,
0,311,1,161,1026,
1,608,2017,16,0,
311,1,384,2018,16,
0,311,1,833,1209,
1,619,1180,1,856,
1184,1,138,1188,1,
358,2019,16,0,311,
1,369,1194,1,355,
1198,1,125,1072,1,
124,1064,1,123,1081,
1,122,1085,1,121,
1089,1,120,1093,1,
119,1076,1,357,1201,
1,595,2020,16,0,
311,1,781,2021,16,
0,311,1,593,1212,
1,353,1216,1,569,
2022,16,0,311,1,
103,1218,1,581,2023,
16,0,311,1,92,
994,1,813,2024,16,
0,311,1,93,2025,
16,0,311,1,88,
1004,1,91,999,1,
807,2026,16,0,311,
1,327,1108,1,282,
1020,1,311,988,1,
49,1277,1,44,1033,
1,67,2027,16,0,
311,1,45,2028,16,
0,311,1,543,2029,
16,0,311,1,542,
1230,1,48,1235,1,
47,1239,1,521,1242,
1,60,2030,16,0,
311,1,59,1247,1,
58,1250,1,57,1253,
1,56,1256,1,55,
1259,1,54,1262,1,
53,1265,1,52,1268,
1,51,1271,1,50,
1274,1,288,2031,16,
0,311,1,287,1280,
1,286,1284,1,40,
2032,16,0,413,1,
284,1015,1,522,2033,
16,0,311,1,760,
1288,1,520,1293,1,
757,1297,1,33,2034,
16,0,413,1,510,
2035,16,0,311,1,
19,1069,1,28,2036,
16,0,311,1,27,
1047,1,21,2037,16,
0,311,1,741,2038,
16,0,311,1,484,
2039,16,0,311,1,
22,2040,16,0,413,
1,977,1009,1,20,
1070,1,975,1097,1,
974,2041,16,0,311,
1,17,1060,1,6,
1307,1,15,1052,1,
14,2042,16,0,311,
1,7,1311,1,12,
1055,1,11,2043,16,
0,400,1,10,1100,
1,9,1104,1,725,
2044,16,0,311,1,
724,1318,1,723,1323,
1,5,1328,1,4,
2045,16,0,311,1,
3,1113,1,2,1117,
1,1,2046,16,0,
413,1,0,2047,16,
0,311,1,9,2048,
19,296,1,9,2049,
5,62,1,855,1167,
1,103,1218,1,953,
1122,1,91,999,1,
522,1364,1,200,1910,
1,92,994,1,412,
1163,1,88,1004,1,
834,1205,1,619,1180,
1,725,2050,16,0,
389,1,403,1172,1,
723,1323,1,593,1212,
1,285,2051,16,0,
294,1,284,1015,1,
282,1020,1,700,1128,
1,163,1920,1,161,
1026,1,46,1925,1,
369,1194,1,977,1009,
1,44,1033,1,414,
1159,1,683,1138,1,
896,1133,1,357,1201,
1,355,1198,1,33,
1040,1,353,1216,1,
138,1188,1,856,1184,
1,27,1047,1,12,
1055,1,1,1332,1,
20,1070,1,15,1052,
1,119,1076,1,17,
1060,1,418,1154,1,
19,1069,1,125,1072,
1,124,1064,1,123,
1081,1,122,1085,1,
121,1089,1,120,1093,
1,975,1097,1,3,
1113,1,10,1100,1,
9,1104,1,2,1117,
1,542,1230,1,327,
1108,1,833,1209,1,
521,1242,1,520,1293,
1,430,1145,1,311,
988,1,428,1149,1,
8,2052,19,178,1,
8,2053,5,20,1,
92,994,1,420,2054,
16,0,176,1,88,
1004,1,33,2055,16,
0,378,1,311,988,
1,27,1047,1,20,
1070,1,22,2056,16,
0,378,1,161,1026,
1,19,1069,1,10,
1100,1,327,1108,1,
9,1104,1,1,2057,
16,0,378,1,3,
1113,1,2,1117,1,
44,1033,1,284,1015,
1,91,999,1,282,
1020,1,7,2058,19,
132,1,7,2059,5,
41,1,103,1218,1,
311,988,1,200,1910,
1,92,994,1,91,
999,1,88,1004,1,
977,1009,1,285,2060,
16,0,310,1,284,
1015,1,282,1020,1,
996,2061,16,0,261,
1,163,1920,1,161,
1026,1,369,2062,16,
0,207,1,46,1925,
1,44,1033,1,469,
2063,16,0,390,1,
33,1040,1,353,2064,
16,0,251,1,138,
1188,1,443,2065,16,
0,130,1,27,1047,
1,12,1055,1,15,
1052,1,124,1064,1,
17,1060,1,20,2066,
16,0,392,1,19,
1069,1,125,1072,1,
119,1076,1,123,1081,
1,122,1085,1,121,
1089,1,120,1093,1,
975,1097,1,10,1100,
1,9,1104,1,327,
1108,1,2,1117,1,
3,1113,1,430,2067,
16,0,251,1,6,
2068,19,236,1,6,
2069,5,102,1,953,
1122,1,703,2070,16,
0,234,1,700,1128,
1,458,2071,16,0,
234,1,896,1133,1,
923,2072,16,0,234,
1,683,1138,1,199,
2073,16,0,234,1,
196,2074,16,0,234,
1,432,2075,16,0,
234,1,430,1145,1,
428,1149,1,418,1154,
1,414,1159,1,412,
1163,1,855,1167,1,
643,2076,16,0,234,
1,403,1172,1,162,
2077,16,0,234,1,
161,1026,1,608,2078,
16,0,234,1,384,
2079,16,0,234,1,
619,1180,1,856,1184,
1,138,1188,1,358,
2080,16,0,234,1,
369,1194,1,355,1198,
1,125,1072,1,124,
1064,1,123,1081,1,
122,1085,1,121,1089,
1,120,1093,1,119,
1076,1,357,1201,1,
834,1205,1,833,1209,
1,593,1212,1,353,
1216,1,103,1218,1,
92,994,1,813,2081,
16,0,234,1,93,
2082,16,0,234,1,
88,1004,1,91,999,
1,781,2083,16,0,
234,1,327,1108,1,
282,1020,1,311,988,
1,288,2084,16,0,
234,1,44,1033,1,
67,2085,16,0,234,
1,45,2086,16,0,
234,1,543,2087,16,
0,234,1,542,1230,
1,48,1235,1,47,
1239,1,521,1242,1,
60,2088,16,0,234,
1,59,1247,1,58,
1250,1,57,1253,1,
56,1256,1,55,1259,
1,54,1262,1,53,
1265,1,52,1268,1,
51,1271,1,50,1274,
1,49,1277,1,287,
1280,1,286,1284,1,
284,1015,1,522,2089,
16,0,234,1,760,
1288,1,520,1293,1,
757,1297,1,33,1040,
1,28,2090,16,0,
234,1,27,1047,1,
19,1069,1,741,1302,
1,484,2091,16,0,
234,1,977,1009,1,
20,1070,1,975,1097,
1,17,1060,1,15,
1052,1,6,1307,1,
12,1055,1,7,1311,
1,10,1100,1,9,
1104,1,725,1314,1,
724,1318,1,723,1323,
1,5,1328,1,4,
2092,16,0,234,1,
3,1113,1,2,1117,
1,1,1332,1,5,
2093,19,161,1,5,
2094,5,126,1,953,
1122,1,944,2095,16,
0,394,1,703,2096,
16,0,394,1,700,
1128,1,458,2097,16,
0,394,1,669,2098,
16,0,394,1,685,
2099,16,0,394,1,
923,2100,16,0,394,
1,683,1138,1,199,
2101,16,0,394,1,
196,2102,16,0,394,
1,418,1154,1,432,
2103,16,0,394,1,
430,1145,1,429,2104,
16,0,159,1,428,
1149,1,412,1163,1,
423,2105,16,0,179,
1,421,2106,16,0,
175,1,419,2107,16,
0,179,1,896,1133,
1,415,2108,16,0,
185,1,414,1159,1,
834,1205,1,886,2109,
16,0,394,1,855,
1167,1,643,2110,16,
0,394,1,403,1172,
1,162,2111,16,0,
394,1,161,1026,1,
608,2112,16,0,394,
1,384,2113,16,0,
394,1,833,1209,1,
619,1180,1,856,1184,
1,138,1188,1,593,
1212,1,358,2114,16,
0,394,1,369,1194,
1,125,1072,1,124,
1064,1,123,1081,1,
122,1085,1,121,1089,
1,120,1093,1,119,
1076,1,357,1201,1,
595,2115,16,0,394,
1,355,1198,1,354,
2116,16,0,252,1,
353,1216,1,352,2117,
16,0,252,1,103,
1218,1,581,2118,16,
0,394,1,92,994,
1,813,2119,16,0,
394,1,91,999,1,
93,2120,16,0,394,
1,88,1004,1,569,
2121,16,0,394,1,
807,2122,16,0,394,
1,327,1108,1,542,
1230,1,282,1020,1,
311,988,1,49,1277,
1,44,1033,1,67,
2123,16,0,394,1,
45,2124,16,0,201,
1,543,2125,16,0,
394,1,781,2126,16,
0,394,1,48,1235,
1,47,1239,1,521,
1242,1,60,2127,16,
0,394,1,59,1247,
1,58,1250,1,57,
1253,1,56,1256,1,
55,1259,1,54,1262,
1,53,1265,1,52,
1268,1,51,1271,1,
50,1274,1,288,2128,
16,0,201,1,287,
1280,1,286,1284,1,
284,1015,1,522,2129,
16,0,394,1,760,
1288,1,520,1293,1,
997,2130,16,0,300,
1,757,1297,1,39,
2131,16,0,377,1,
33,1040,1,510,2132,
16,0,394,1,19,
1069,1,28,2133,16,
0,394,1,27,1047,
1,26,2134,16,0,
387,1,21,2135,16,
0,394,1,741,2136,
16,0,394,1,484,
2137,16,0,394,1,
977,1009,1,20,1070,
1,975,1097,1,974,
2138,16,0,394,1,
17,1060,1,6,1307,
1,15,1052,1,14,
2139,16,0,394,1,
13,2140,16,0,300,
1,12,1055,1,7,
1311,1,10,1100,1,
9,1104,1,725,2141,
16,0,394,1,724,
1318,1,723,1323,1,
5,1328,1,4,2142,
16,0,394,1,3,
1113,1,2,1117,1,
1,1332,1,0,2143,
16,0,394,1,3,
2144,19,239,1,3,
2145,5,104,1,953,
1122,1,703,2146,16,
0,237,1,700,1128,
1,458,2147,16,0,
237,1,896,1133,1,
923,2148,16,0,237,
1,683,1138,1,199,
2149,16,0,237,1,
196,2150,16,0,237,
1,432,2151,16,0,
237,1,430,1145,1,
428,1149,1,418,1154,
1,414,1159,1,412,
1163,1,855,1167,1,
643,2152,16,0,237,
1,403,1172,1,162,
2153,16,0,237,1,
161,1026,1,608,2154,
16,0,237,1,384,
2155,16,0,237,1,
619,1180,1,856,1184,
1,138,1188,1,358,
2156,16,0,237,1,
369,1194,1,355,1198,
1,125,1072,1,124,
1064,1,123,1081,1,
122,1085,1,121,1089,
1,120,1093,1,119,
1076,1,357,1201,1,
834,1205,1,833,1209,
1,593,1212,1,353,
1216,1,103,1218,1,
92,994,1,813,2157,
16,0,237,1,93,
2158,16,0,237,1,
88,1004,1,91,999,
1,781,2159,16,0,
237,1,327,1108,1,
282,1020,1,311,988,
1,288,2160,16,0,
237,1,44,1033,1,
67,2161,16,0,237,
1,45,2162,16,0,
237,1,543,2163,16,
0,237,1,542,1230,
1,48,1235,1,47,
1239,1,521,1242,1,
60,2164,16,0,237,
1,59,1247,1,58,
1250,1,57,1253,1,
56,1256,1,55,1259,
1,54,1262,1,53,
1265,1,52,1268,1,
51,1271,1,50,1274,
1,49,1277,1,287,
1280,1,286,1284,1,
40,2165,16,0,415,
1,284,1015,1,522,
2166,16,0,237,1,
760,1288,1,520,1293,
1,757,1297,1,33,
2167,16,0,415,1,
28,2168,16,0,237,
1,27,1047,1,19,
1069,1,741,1302,1,
484,2169,16,0,237,
1,22,2170,16,0,
415,1,977,1009,1,
20,1070,1,975,1097,
1,17,1060,1,15,
1052,1,6,1307,1,
12,1055,1,7,1311,
1,10,1100,1,9,
1104,1,725,1314,1,
724,1318,1,723,1323,
1,5,1328,1,4,
2171,16,0,237,1,
3,1113,1,2,1117,
1,1,2172,16,0,
415,1,2,2173,19,
135,1,2,2174,5,
61,1,855,1167,1,
723,1323,1,103,1218,
1,741,1302,1,953,
1122,1,91,999,1,
522,1364,1,414,1159,
1,92,994,1,412,
1163,1,88,1004,1,
834,1205,1,619,1180,
1,725,1314,1,403,
1172,1,700,1128,1,
284,1015,1,282,1020,
1,593,1212,1,161,
1026,1,369,1194,1,
977,1009,1,44,1033,
1,683,1138,1,896,
1133,1,357,1201,1,
355,1198,1,33,1040,
1,353,1216,1,138,
1188,1,2,1117,1,
1,1332,1,856,1184,
1,27,1047,1,12,
1055,1,3,1113,1,
20,1070,1,15,1052,
1,119,1076,1,17,
1060,1,418,1154,1,
19,1069,1,125,1072,
1,124,1064,1,123,
1081,1,122,1085,1,
121,1089,1,120,1093,
1,975,1097,1,760,
1288,1,10,1100,1,
9,1104,1,757,1297,
1,542,1230,1,327,
1108,1,833,1209,1,
521,1242,1,520,1293,
1,430,1145,1,311,
988,1,428,1149,2,
1,0};
new Sfactory(this,"error",new SCreator(error_factory));
new Sfactory(this,"Atom_1",new SCreator(Atom_1_factory));
new Sfactory(this,"field_1",new SCreator(field_1_factory));
new Sfactory(this,"functioncall_2",new SCreator(functioncall_2_factory));
new Sfactory(this,"ExpTableDec",new SCreator(ExpTableDec_factory));
new Sfactory(this,"Unop",new SCreator(Unop_factory));
new Sfactory(this,"binop_11",new SCreator(binop_11_factory));
new Sfactory(this,"stat_9",new SCreator(stat_9_factory));
new Sfactory(this,"LocalFuncDecl_1",new SCreator(LocalFuncDecl_1_factory));
new Sfactory(this,"init_1",new SCreator(init_1_factory));
new Sfactory(this,"prefixexp_2",new SCreator(prefixexp_2_factory));
new Sfactory(this,"elseif_1",new SCreator(elseif_1_factory));
new Sfactory(this,"namelist_3",new SCreator(namelist_3_factory));
new Sfactory(this,"FieldExpAssign",new SCreator(FieldExpAssign_factory));
new Sfactory(this,"FuncDecl_1",new SCreator(FuncDecl_1_factory));
new Sfactory(this,"stat_2",new SCreator(stat_2_factory));
new Sfactory(this,"funcname_1",new SCreator(funcname_1_factory));
new Sfactory(this,"explist",new SCreator(explist_factory));
new Sfactory(this,"parlist_2",new SCreator(parlist_2_factory));
new Sfactory(this,"parlist_3",new SCreator(parlist_3_factory));
new Sfactory(this,"parlist_1",new SCreator(parlist_1_factory));
new Sfactory(this,"stat_10",new SCreator(stat_10_factory));
new Sfactory(this,"stat_7",new SCreator(stat_7_factory));
new Sfactory(this,"binop_12",new SCreator(binop_12_factory));
new Sfactory(this,"fieldsep_2",new SCreator(fieldsep_2_factory));
new Sfactory(this,"TableRef",new SCreator(TableRef_factory));
new Sfactory(this,"function_1",new SCreator(function_1_factory));
new Sfactory(this,"binop_8",new SCreator(binop_8_factory));
new Sfactory(this,"funcbody_2",new SCreator(funcbody_2_factory));
new Sfactory(this,"chunk_4",new SCreator(chunk_4_factory));
new Sfactory(this,"stat_12",new SCreator(stat_12_factory));
new Sfactory(this,"exp_1",new SCreator(exp_1_factory));
new Sfactory(this,"namelist",new SCreator(namelist_factory));
new Sfactory(this,"Retval",new SCreator(Retval_factory));
new Sfactory(this,"functioncall_1",new SCreator(functioncall_1_factory));
new Sfactory(this,"parlist",new SCreator(parlist_factory));
new Sfactory(this,"binop_6",new SCreator(binop_6_factory));
new Sfactory(this,"arg",new SCreator(arg_factory));
new Sfactory(this,"funcname",new SCreator(funcname_factory));
new Sfactory(this,"prefixexp_3",new SCreator(prefixexp_3_factory));
new Sfactory(this,"binop_13",new SCreator(binop_13_factory));
new Sfactory(this,"binop_4",new SCreator(binop_4_factory));
new Sfactory(this,"stat_3",new SCreator(stat_3_factory));
new Sfactory(this,"varlist_2",new SCreator(varlist_2_factory));
new Sfactory(this,"binop_1",new SCreator(binop_1_factory));
new Sfactory(this,"fieldlist_2",new SCreator(fieldlist_2_factory));
new Sfactory(this,"chunk_2",new SCreator(chunk_2_factory));
new Sfactory(this,"fieldlist_1",new SCreator(fieldlist_1_factory));
new Sfactory(this,"prefixexp",new SCreator(prefixexp_factory));
new Sfactory(this,"stat_4",new SCreator(stat_4_factory));
new Sfactory(this,"field",new SCreator(field_factory));
new Sfactory(this,"Atom_2",new SCreator(Atom_2_factory));
new Sfactory(this,"funcbody",new SCreator(funcbody_factory));
new Sfactory(this,"funcbody_3",new SCreator(funcbody_3_factory));
new Sfactory(this,"stat_14",new SCreator(stat_14_factory));
new Sfactory(this,"LocalFuncDecl",new SCreator(LocalFuncDecl_factory));
new Sfactory(this,"TableRef_1",new SCreator(TableRef_1_factory));
new Sfactory(this,"Binop_1",new SCreator(Binop_1_factory));
new Sfactory(this,"tableconstructor",new SCreator(tableconstructor_factory));
new Sfactory(this,"PackageRef",new SCreator(PackageRef_factory));
new Sfactory(this,"function",new SCreator(function_factory));
new Sfactory(this,"Unop_1",new SCreator(Unop_1_factory));
new Sfactory(this,"namelist_1",new SCreator(namelist_1_factory));
new Sfactory(this,"stat_11",new SCreator(stat_11_factory));
new Sfactory(this,"funcname_2",new SCreator(funcname_2_factory));
new Sfactory(this,"stat",new SCreator(stat_factory));
new Sfactory(this,"unop_1",new SCreator(unop_1_factory));
new Sfactory(this,"exp",new SCreator(exp_factory));
new Sfactory(this,"FieldExpAssign_1",new SCreator(FieldExpAssign_1_factory));
new Sfactory(this,"fieldlist",new SCreator(fieldlist_factory));
new Sfactory(this,"var",new SCreator(var_factory));
new Sfactory(this,"arg_2",new SCreator(arg_2_factory));
new Sfactory(this,"arg_1",new SCreator(arg_1_factory));
new Sfactory(this,"stat_13",new SCreator(stat_13_factory));
new Sfactory(this,"unop",new SCreator(unop_factory));
new Sfactory(this,"Assignment",new SCreator(Assignment_factory));
new Sfactory(this,"tableconstructor_2",new SCreator(tableconstructor_2_factory));
new Sfactory(this,"Atom_3",new SCreator(Atom_3_factory));
new Sfactory(this,"arg_3",new SCreator(arg_3_factory));
new Sfactory(this,"arg_4",new SCreator(arg_4_factory));
new Sfactory(this,"explist_1",new SCreator(explist_1_factory));
new Sfactory(this,"explist_2",new SCreator(explist_2_factory));
new Sfactory(this,"binop_3",new SCreator(binop_3_factory));
new Sfactory(this,"ExpTableDec_1",new SCreator(ExpTableDec_1_factory));
new Sfactory(this,"Atom",new SCreator(Atom_factory));
new Sfactory(this,"funcname_3",new SCreator(funcname_3_factory));
new Sfactory(this,"chunk",new SCreator(chunk_factory));
new Sfactory(this,"block_1",new SCreator(block_1_factory));
new Sfactory(this,"binop",new SCreator(binop_factory));
new Sfactory(this,"elseif_2",new SCreator(elseif_2_factory));
new Sfactory(this,"Retval_1",new SCreator(Retval_1_factory));
new Sfactory(this,"PackageRef_1",new SCreator(PackageRef_1_factory));
new Sfactory(this,"FieldAssign",new SCreator(FieldAssign_factory));
new Sfactory(this,"var_1",new SCreator(var_1_factory));
new Sfactory(this,"stat_1",new SCreator(stat_1_factory));
new Sfactory(this,"unop_2",new SCreator(unop_2_factory));
new Sfactory(this,"varlist_1",new SCreator(varlist_1_factory));
new Sfactory(this,"Binop",new SCreator(Binop_factory));
new Sfactory(this,"chunk_1",new SCreator(chunk_1_factory));
new Sfactory(this,"Assignment_1",new SCreator(Assignment_1_factory));
new Sfactory(this,"elseif",new SCreator(elseif_factory));
new Sfactory(this,"functioncall",new SCreator(functioncall_factory));
new Sfactory(this,"stat_5",new SCreator(stat_5_factory));
new Sfactory(this,"varlist",new SCreator(varlist_factory));
new Sfactory(this,"stat_8",new SCreator(stat_8_factory));
new Sfactory(this,"binop_9",new SCreator(binop_9_factory));
new Sfactory(this,"namelist_2",new SCreator(namelist_2_factory));
new Sfactory(this,"chunk_3",new SCreator(chunk_3_factory));
new Sfactory(this,"FieldAssign_1",new SCreator(FieldAssign_1_factory));
new Sfactory(this,"Atom_5",new SCreator(Atom_5_factory));
new Sfactory(this,"init",new SCreator(init_factory));
new Sfactory(this,"binop_7",new SCreator(binop_7_factory));
new Sfactory(this,"funcbody_4",new SCreator(funcbody_4_factory));
new Sfactory(this,"binop_10",new SCreator(binop_10_factory));
new Sfactory(this,"unop_3",new SCreator(unop_3_factory));
new Sfactory(this,"binop_5",new SCreator(binop_5_factory));
new Sfactory(this,"block",new SCreator(block_factory));
new Sfactory(this,"stat_6",new SCreator(stat_6_factory));
new Sfactory(this,"tableconstructor_1",new SCreator(tableconstructor_1_factory));
new Sfactory(this,"fieldsep_1",new SCreator(fieldsep_1_factory));
new Sfactory(this,"fieldsep",new SCreator(fieldsep_factory));
new Sfactory(this,"funcbody_1",new SCreator(funcbody_1_factory));
new Sfactory(this,"prefixexp_1",new SCreator(prefixexp_1_factory));
new Sfactory(this,"Atom_4",new SCreator(Atom_4_factory));
new Sfactory(this,"FuncDecl",new SCreator(FuncDecl_factory));
new Sfactory(this,"exp_2",new SCreator(exp_2_factory));
new Sfactory(this,"binop_2",new SCreator(binop_2_factory));
}
public static object error_factory(Parser yyp) { return new error(yyp); }
public static object Atom_1_factory(Parser yyp) { return new Atom_1(yyp); }
public static object field_1_factory(Parser yyp) { return new field_1(yyp); }
public static object functioncall_2_factory(Parser yyp) { return new functioncall_2(yyp); }
public static object ExpTableDec_factory(Parser yyp) { return new ExpTableDec(yyp); }
public static object Unop_factory(Parser yyp) { return new Unop(yyp); }
public static object binop_11_factory(Parser yyp) { return new binop_11(yyp); }
public static object stat_9_factory(Parser yyp) { return new stat_9(yyp); }
public static object LocalFuncDecl_1_factory(Parser yyp) { return new LocalFuncDecl_1(yyp); }
public static object init_1_factory(Parser yyp) { return new init_1(yyp); }
public static object prefixexp_2_factory(Parser yyp) { return new prefixexp_2(yyp); }
public static object elseif_1_factory(Parser yyp) { return new elseif_1(yyp); }
public static object namelist_3_factory(Parser yyp) { return new namelist_3(yyp); }
public static object FieldExpAssign_factory(Parser yyp) { return new FieldExpAssign(yyp); }
public static object FuncDecl_1_factory(Parser yyp) { return new FuncDecl_1(yyp); }
public static object stat_2_factory(Parser yyp) { return new stat_2(yyp); }
public static object funcname_1_factory(Parser yyp) { return new funcname_1(yyp); }
public static object explist_factory(Parser yyp) { return new explist(yyp); }
public static object parlist_2_factory(Parser yyp) { return new parlist_2(yyp); }
public static object parlist_3_factory(Parser yyp) { return new parlist_3(yyp); }
public static object parlist_1_factory(Parser yyp) { return new parlist_1(yyp); }
public static object stat_10_factory(Parser yyp) { return new stat_10(yyp); }
public static object stat_7_factory(Parser yyp) { return new stat_7(yyp); }
public static object binop_12_factory(Parser yyp) { return new binop_12(yyp); }
public static object fieldsep_2_factory(Parser yyp) { return new fieldsep_2(yyp); }
public static object TableRef_factory(Parser yyp) { return new TableRef(yyp); }
public static object function_1_factory(Parser yyp) { return new function_1(yyp); }
public static object binop_8_factory(Parser yyp) { return new binop_8(yyp); }
public static object funcbody_2_factory(Parser yyp) { return new funcbody_2(yyp); }
public static object chunk_4_factory(Parser yyp) { return new chunk_4(yyp); }
public static object stat_12_factory(Parser yyp) { return new stat_12(yyp); }
public static object exp_1_factory(Parser yyp) { return new exp_1(yyp); }
public static object namelist_factory(Parser yyp) { return new namelist(yyp); }
public static object Retval_factory(Parser yyp) { return new Retval(yyp); }
public static object functioncall_1_factory(Parser yyp) { return new functioncall_1(yyp); }
public static object parlist_factory(Parser yyp) { return new parlist(yyp); }
public static object binop_6_factory(Parser yyp) { return new binop_6(yyp); }
public static object arg_factory(Parser yyp) { return new arg(yyp); }
public static object funcname_factory(Parser yyp) { return new funcname(yyp); }
public static object prefixexp_3_factory(Parser yyp) { return new prefixexp_3(yyp); }
public static object binop_13_factory(Parser yyp) { return new binop_13(yyp); }
public static object binop_4_factory(Parser yyp) { return new binop_4(yyp); }
public static object stat_3_factory(Parser yyp) { return new stat_3(yyp); }
public static object varlist_2_factory(Parser yyp) { return new varlist_2(yyp); }
public static object binop_1_factory(Parser yyp) { return new binop_1(yyp); }
public static object fieldlist_2_factory(Parser yyp) { return new fieldlist_2(yyp); }
public static object chunk_2_factory(Parser yyp) { return new chunk_2(yyp); }
public static object fieldlist_1_factory(Parser yyp) { return new fieldlist_1(yyp); }
public static object prefixexp_factory(Parser yyp) { return new prefixexp(yyp); }
public static object stat_4_factory(Parser yyp) { return new stat_4(yyp); }
public static object field_factory(Parser yyp) { return new field(yyp); }
public static object Atom_2_factory(Parser yyp) { return new Atom_2(yyp); }
public static object funcbody_factory(Parser yyp) { return new funcbody(yyp); }
public static object funcbody_3_factory(Parser yyp) { return new funcbody_3(yyp); }
public static object stat_14_factory(Parser yyp) { return new stat_14(yyp); }
public static object LocalFuncDecl_factory(Parser yyp) { return new LocalFuncDecl(yyp); }
public static object TableRef_1_factory(Parser yyp) { return new TableRef_1(yyp); }
public static object Binop_1_factory(Parser yyp) { return new Binop_1(yyp); }
public static object tableconstructor_factory(Parser yyp) { return new tableconstructor(yyp); }
public static object PackageRef_factory(Parser yyp) { return new PackageRef(yyp); }
public static object function_factory(Parser yyp) { return new function(yyp); }
public static object Unop_1_factory(Parser yyp) { return new Unop_1(yyp); }
public static object namelist_1_factory(Parser yyp) { return new namelist_1(yyp); }
public static object stat_11_factory(Parser yyp) { return new stat_11(yyp); }
public static object funcname_2_factory(Parser yyp) { return new funcname_2(yyp); }
public static object stat_factory(Parser yyp) { return new stat(yyp); }
public static object unop_1_factory(Parser yyp) { return new unop_1(yyp); }
public static object exp_factory(Parser yyp) { return new exp(yyp); }
public static object FieldExpAssign_1_factory(Parser yyp) { return new FieldExpAssign_1(yyp); }
public static object fieldlist_factory(Parser yyp) { return new fieldlist(yyp); }
public static object var_factory(Parser yyp) { return new var(yyp); }
public static object arg_2_factory(Parser yyp) { return new arg_2(yyp); }
public static object arg_1_factory(Parser yyp) { return new arg_1(yyp); }
public static object stat_13_factory(Parser yyp) { return new stat_13(yyp); }
public static object unop_factory(Parser yyp) { return new unop(yyp); }
public static object Assignment_factory(Parser yyp) { return new Assignment(yyp); }
public static object tableconstructor_2_factory(Parser yyp) { return new tableconstructor_2(yyp); }
public static object Atom_3_factory(Parser yyp) { return new Atom_3(yyp); }
public static object arg_3_factory(Parser yyp) { return new arg_3(yyp); }
public static object arg_4_factory(Parser yyp) { return new arg_4(yyp); }
public static object explist_1_factory(Parser yyp) { return new explist_1(yyp); }
public static object explist_2_factory(Parser yyp) { return new explist_2(yyp); }
public static object binop_3_factory(Parser yyp) { return new binop_3(yyp); }
public static object ExpTableDec_1_factory(Parser yyp) { return new ExpTableDec_1(yyp); }
public static object Atom_factory(Parser yyp) { return new Atom(yyp); }
public static object funcname_3_factory(Parser yyp) { return new funcname_3(yyp); }
public static object chunk_factory(Parser yyp) { return new chunk(yyp); }
public static object block_1_factory(Parser yyp) { return new block_1(yyp); }
public static object binop_factory(Parser yyp) { return new binop(yyp); }
public static object elseif_2_factory(Parser yyp) { return new elseif_2(yyp); }
public static object Retval_1_factory(Parser yyp) { return new Retval_1(yyp); }
public static object PackageRef_1_factory(Parser yyp) { return new PackageRef_1(yyp); }
public static object FieldAssign_factory(Parser yyp) { return new FieldAssign(yyp); }
public static object var_1_factory(Parser yyp) { return new var_1(yyp); }
public static object stat_1_factory(Parser yyp) { return new stat_1(yyp); }
public static object unop_2_factory(Parser yyp) { return new unop_2(yyp); }
public static object varlist_1_factory(Parser yyp) { return new varlist_1(yyp); }
public static object Binop_factory(Parser yyp) { return new Binop(yyp); }
public static object chunk_1_factory(Parser yyp) { return new chunk_1(yyp); }
public static object Assignment_1_factory(Parser yyp) { return new Assignment_1(yyp); }
public static object elseif_factory(Parser yyp) { return new elseif(yyp); }
public static object functioncall_factory(Parser yyp) { return new functioncall(yyp); }
public static object stat_5_factory(Parser yyp) { return new stat_5(yyp); }
public static object varlist_factory(Parser yyp) { return new varlist(yyp); }
public static object stat_8_factory(Parser yyp) { return new stat_8(yyp); }
public static object binop_9_factory(Parser yyp) { return new binop_9(yyp); }
public static object namelist_2_factory(Parser yyp) { return new namelist_2(yyp); }
public static object chunk_3_factory(Parser yyp) { return new chunk_3(yyp); }
public static object FieldAssign_1_factory(Parser yyp) { return new FieldAssign_1(yyp); }
public static object Atom_5_factory(Parser yyp) { return new Atom_5(yyp); }
public static object init_factory(Parser yyp) { return new init(yyp); }
public static object binop_7_factory(Parser yyp) { return new binop_7(yyp); }
public static object funcbody_4_factory(Parser yyp) { return new funcbody_4(yyp); }
public static object binop_10_factory(Parser yyp) { return new binop_10(yyp); }
public static object unop_3_factory(Parser yyp) { return new unop_3(yyp); }
public static object binop_5_factory(Parser yyp) { return new binop_5(yyp); }
public static object block_factory(Parser yyp) { return new block(yyp); }
public static object stat_6_factory(Parser yyp) { return new stat_6(yyp); }
public static object tableconstructor_1_factory(Parser yyp) { return new tableconstructor_1(yyp); }
public static object fieldsep_1_factory(Parser yyp) { return new fieldsep_1(yyp); }
public static object fieldsep_factory(Parser yyp) { return new fieldsep(yyp); }
public static object funcbody_1_factory(Parser yyp) { return new funcbody_1(yyp); }
public static object prefixexp_1_factory(Parser yyp) { return new prefixexp_1(yyp); }
public static object Atom_4_factory(Parser yyp) { return new Atom_4(yyp); }
public static object FuncDecl_factory(Parser yyp) { return new FuncDecl(yyp); }
public static object exp_2_factory(Parser yyp) { return new exp_2(yyp); }
public static object binop_2_factory(Parser yyp) { return new binop_2(yyp); }
}
public class syntax: Parser {
public syntax():base(new yysyntax(),new tokens()) {}
public syntax(YyParser syms):base(syms,new tokens()) {}
public syntax(YyParser syms,ErrorHandler erh):base(syms,new tokens(erh)) {}

 }
