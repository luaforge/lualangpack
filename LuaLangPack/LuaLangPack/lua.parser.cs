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
 public  void  FillScope ( LuaScope  scope ){ if ( s != null ){ LuaScope  nested = new  LuaScope ( scope );
 s . FillScope ( nested );
 scope . nested . AddLast ( nested );
}
 if ( c != null ){ LuaScope  nested = new  LuaScope ( scope );
 c . FillScope ( nested );
 scope . nested . AddLast ( nested );
}
}

public override string yyname { get { return "chunk"; }}
public override int yynum { get { return 54; }}
public chunk(Parser yyp):base(yyp){}}
//%+block+55
public class block : SYMBOL{
 chunk  c ;
 public  block (Parser yyp, chunk  a ):base(((syntax)yyp)){ c = a ;
}
 public  void  FillScope ( LuaScope  scope ){ c . FillScope ( scope );
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
 public  NAME  name ;
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
 public  void  FillScope ( LuaScope  s ){}

public override string yyname { get { return "parlist"; }}
public override int yynum { get { return 60; }}
public parlist(Parser yyp):base(yyp){}}
//%+funcbody+61
public class funcbody : SYMBOL{
 block  b ;
 parlist  p ;
 END  e ;
 RPAREN  paren ;
 public  funcbody (Parser yyp, block  a , END  c , RPAREN  d ):base(((syntax)yyp)){ b = a ;
 e = c ;
 paren = d ;
}
 public  funcbody (Parser yyp, block  a , parlist  pl , END  c , RPAREN  d ):base(((syntax)yyp)){ b = a ;
 p = pl ;
 e = c ;
 paren = d ;
}
 public  funcbody (Parser yyp, parlist  pl , RPAREN  d , END  c ):base(((syntax)yyp)){ p = pl ;
 paren = d ;
 e = c ;
}
 public  funcbody (Parser yyp, RPAREN  d , END  c ):base(((syntax)yyp)){ paren = d ;
 e = c ;
}
 public  void  FillScope ( LuaScope  s ){ s . beginLine = paren . Line ;
 s . beginIndx = paren . Position ;
 s . endLine = e . Line ;
 s . endIndx = e . Position +3;
 if ( b != null ){ b . FillScope ( s );
}
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
//%+init+79
public class init : SYMBOL{
 explist  e ;
 public  init (Parser yyp, explist  a ):base(((syntax)yyp)){ e = a ;
}
 public  void  FillScope ( LuaScope  s ){ e . FillScope ( s );
}

public override string yyname { get { return "init"; }}
public override int yynum { get { return 79; }}
public init(Parser yyp):base(yyp){}}
//%+stat+80
public class stat : SYMBOL{
 public  stat (Parser yyp):base(((syntax)yyp)){}
 public  virtual  void  FillScope ( LuaScope  scope ){}

public override string yyname { get { return "stat"; }}
public override int yynum { get { return 80; }}
}
//%+Assignment+81
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
public override int yynum { get { return 81; }}
public Assignment(Parser yyp):base(yyp){}}
//%+LocalInit+82
public class LocalInit : stat{
 namelist  n ;
 init  i ;
 public  LocalInit (Parser yyp, namelist  a , init  b ):base(((syntax)yyp)){ n = a ;
 i = b ;
}
 public  override  void  FillScope ( LuaScope  s ){ i . FillScope ( s );
}

public override string yyname { get { return "LocalInit"; }}
public override int yynum { get { return 82; }}
public LocalInit(Parser yyp):base(yyp){}}
//%+Retval+83
public class Retval : stat{
 private  explist  e ;
 public  Retval (Parser yyp, explist  a ):base(((syntax)yyp)){ e = a ;
}
 public  override  void  FillScope ( LuaScope  scope ){ e . FillScope ( scope );
}

public override string yyname { get { return "Retval"; }}
public override int yynum { get { return 83; }}
public Retval(Parser yyp):base(yyp){}}
//%+FuncDecl+84
public class FuncDecl : stat{
 funcname  fname ;
 funcbody  body ;
 public  FuncDecl (Parser yyp, funcname  a , funcbody  b ):base(((syntax)yyp)){ fname = a ;
 body = b ;
}
 public  override  void  FillScope ( LuaScope  scope ){ body . FillScope ( scope );
}

public override string yyname { get { return "FuncDecl"; }}
public override int yynum { get { return 84; }}
public FuncDecl(Parser yyp):base(yyp){}}
//%+LocalFuncDecl+85
public class LocalFuncDecl : stat{
 funcbody  body ;
 NAME  name ;
 public  LocalFuncDecl (Parser yyp, NAME  a , funcbody  b ):base(((syntax)yyp)){ name = a ;
 body = b ;
}
 public  override  void  FillScope ( LuaScope  scope ){ body . FillScope ( scope );
}

public override string yyname { get { return "LocalFuncDecl"; }}
public override int yynum { get { return 85; }}
public LocalFuncDecl(Parser yyp):base(yyp){}}
//%+arg+86
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
public override int yynum { get { return 86; }}
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

public class stat_9 : stat {
  public stat_9(Parser yyq):base(yyq){}}

public class Retval_1 : Retval {
  public Retval_1(Parser yyq):base(yyq, 
	((explist)(yyq.StackAt(0).m_value))
	 ){}}

public class stat_10 : stat {
  public stat_10(Parser yyq):base(yyq){}}

public class stat_11 : stat {
  public stat_11(Parser yyq):base(yyq){}}

public class stat_12 : stat {
  public stat_12(Parser yyq):base(yyq){}}

public class stat_13 : stat {
  public stat_13(Parser yyq):base(yyq){}}

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

public class stat_14 : stat {
  public stat_14(Parser yyq):base(yyq){}}

public class LocalInit_1 : LocalInit {
  public LocalInit_1(Parser yyq):base(yyq, 
	((namelist)(yyq.StackAt(1).m_value))
	, 
	((init)(yyq.StackAt(0).m_value))
	 ){}}
public class elseif : SYMBOL {
	public elseif(Parser yyq):base(yyq) { }
  public override string yyname { get { return "elseif"; }}
  public override int yynum { get { return 99; }}}

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

public class init_1 : init {
  public init_1(Parser yyq):base(yyq, 
	((explist)(yyq.StackAt(0).m_value))
	 ){}}

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
  public override int yynum { get { return 107; }}}

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
	, 
	((RPAREN)(yyq.StackAt(2).m_value))
	 ){}}

public class funcbody_2 : funcbody {
  public funcbody_2(Parser yyq):base(yyq, 
	((block)(yyq.StackAt(1).m_value))
	, 
	((parlist)(yyq.StackAt(3).m_value))
	, 
	((END)(yyq.StackAt(0).m_value))
	, 
	((RPAREN)(yyq.StackAt(2).m_value))
	 ){}}

public class funcbody_3 : funcbody {
  public funcbody_3(Parser yyq):base(yyq, 
	((parlist)(yyq.StackAt(2).m_value))
	, 
	((RPAREN)(yyq.StackAt(1).m_value))
	, 
	((END)(yyq.StackAt(0).m_value))
	 ){}}

public class funcbody_4 : funcbody {
  public funcbody_4(Parser yyq):base(yyq, 
	((RPAREN)(yyq.StackAt(1).m_value))
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
  public override int yynum { get { return 114; }}}

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

public class namelist_1 : namelist {
  public namelist_1(Parser yyq):base(yyq){}}

public class funcname_2 : funcname {
  public funcname_2(Parser yyq):base(yyq){}}

public class funcname_3 : funcname {
  public funcname_3(Parser yyq):base(yyq){}}

public class namelist_2 : namelist {
  public namelist_2(Parser yyq):base(yyq){}}

public class namelist_3 : namelist {
  public namelist_3(Parser yyq):base(yyq){}}

public class elseif_1 : elseif {
  public elseif_1(Parser yyq):base(yyq){}}

public class elseif_2 : elseif {
  public elseif_2(Parser yyq):base(yyq){}}

public class elseif_3 : elseif {
  public elseif_3(Parser yyq):base(yyq){}}

public class fieldsep_1 : fieldsep {
  public fieldsep_1(Parser yyq):base(yyq){}}

public class fieldsep_2 : fieldsep {
  public fieldsep_2(Parser yyq):base(yyq){}}

public class parlist_3 : parlist {
  public parlist_3(Parser yyq):base(yyq){}}

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

public class binop_14 : binop {
  public binop_14(Parser yyq):base(yyq){}}

public class binop_15 : binop {
  public binop_15(Parser yyq):base(yyq){}}

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
1,1089,102,2,0,
105,5,177,1,517,
106,18,1,517,107,
20,108,4,6,101,
0,120,0,112,0,
1,69,1,2,2,
0,1,1030,109,18,
1,1030,110,20,111,
4,6,69,0,78,
0,68,0,1,38,
1,1,2,0,1,
1029,112,18,1,1029,
113,20,114,4,10,
98,0,108,0,111,
0,99,0,107,0,
1,55,1,2,2,
0,1,1028,115,18,
1,1028,110,2,0,
1,1027,116,18,1,
1027,117,20,118,4,
12,82,0,80,0,
65,0,82,0,69,
0,78,0,1,11,
1,1,2,0,1,
1026,119,18,1,1026,
120,20,121,4,14,
112,0,97,0,114,
0,108,0,105,0,
115,0,116,0,1,
60,1,2,2,0,
1,489,122,18,1,
489,107,2,0,1,
1006,123,18,1,1006,
110,2,0,1,1005,
124,18,1,1005,113,
2,0,1,478,125,
18,1,478,126,20,
127,4,10,67,0,
79,0,77,0,77,
0,65,0,1,7,
1,1,2,0,1,
997,128,18,1,997,
129,20,130,4,4,
68,0,79,0,1,
41,1,1,2,0,
1,996,131,18,1,
996,132,20,133,4,
14,101,0,120,0,
112,0,108,0,105,
0,115,0,116,0,
1,74,1,2,2,
0,1,461,134,18,
1,461,107,2,0,
1,976,135,18,1,
976,136,20,137,4,
4,73,0,78,0,
1,42,1,1,2,
0,1,975,138,18,
1,975,139,20,140,
4,16,110,0,97,
0,109,0,101,0,
108,0,105,0,115,
0,116,0,1,107,
1,2,2,0,1,
450,141,18,1,450,
142,20,143,4,12,
65,0,83,0,83,
0,73,0,71,0,
78,0,1,33,1,
1,2,0,1,448,
144,18,1,448,145,
20,146,4,8,78,
0,65,0,77,0,
69,0,1,5,1,
1,2,0,1,447,
147,18,1,447,148,
20,149,4,6,70,
0,79,0,82,0,
1,40,1,1,2,
0,1,446,150,18,
1,446,151,20,152,
4,16,102,0,117,
0,110,0,99,0,
98,0,111,0,100,
0,121,0,1,61,
1,2,2,0,1,
444,153,18,1,444,
154,20,155,4,16,
102,0,117,0,110,
0,99,0,110,0,
97,0,109,0,101,
0,1,59,1,2,
2,0,1,443,156,
18,1,443,154,2,
0,1,441,157,18,
1,441,158,20,159,
4,6,68,0,79,
0,84,0,1,16,
1,1,2,0,1,
440,160,18,1,440,
145,2,0,1,439,
161,18,1,439,162,
20,163,4,10,67,
0,79,0,76,0,
79,0,78,0,1,
8,1,1,2,0,
1,438,164,18,1,
438,145,2,0,1,
437,165,18,1,437,
166,20,167,4,16,
70,0,85,0,78,
0,67,0,84,0,
73,0,79,0,78,
0,1,45,1,1,
2,0,1,436,168,
18,1,436,151,2,
0,1,434,169,18,
1,434,145,2,0,
1,433,170,18,1,
433,166,2,0,1,
432,171,18,1,432,
172,20,173,4,8,
105,0,110,0,105,
0,116,0,1,79,
1,2,2,0,1,
430,174,18,1,430,
132,2,0,1,949,
175,18,1,949,110,
2,0,1,948,176,
18,1,948,113,2,
0,1,421,177,18,
1,421,132,2,0,
1,939,178,18,1,
939,129,2,0,1,
402,179,18,1,402,
126,2,0,1,909,
180,18,1,909,110,
2,0,1,908,181,
18,1,908,110,2,
0,1,907,182,18,
1,907,183,20,184,
4,12,101,0,108,
0,115,0,101,0,
105,0,102,0,1,
99,1,2,2,0,
1,385,185,18,1,
385,107,2,0,1,
374,186,18,1,374,
142,2,0,1,373,
187,18,1,373,139,
2,0,1,371,188,
18,1,371,139,2,
0,1,370,189,18,
1,370,126,2,0,
1,369,190,18,1,
369,145,2,0,1,
368,191,18,1,368,
192,20,193,4,10,
76,0,79,0,67,
0,65,0,76,0,
1,49,1,1,2,
0,1,886,194,18,
1,886,183,2,0,
1,363,195,18,1,
363,196,20,197,4,
14,118,0,97,0,
114,0,108,0,105,
0,115,0,116,0,
1,78,1,2,2,
0,1,846,198,18,
1,846,113,2,0,
1,866,199,18,1,
866,200,20,201,4,
12,69,0,76,0,
83,0,69,0,73,
0,70,0,1,37,
1,1,2,0,1,
343,202,18,1,343,
203,20,204,4,12,
82,0,66,0,82,
0,65,0,67,0,
75,0,1,13,1,
1,2,0,1,853,
205,18,1,853,113,
2,0,1,823,206,
18,1,823,107,2,
0,1,327,207,18,
1,327,107,2,0,
1,847,208,18,1,
847,209,20,210,4,
8,69,0,76,0,
83,0,69,0,1,
36,1,1,2,0,
1,325,211,18,1,
325,212,20,213,4,
6,97,0,114,0,
103,0,1,86,1,
2,2,0,1,323,
214,18,1,323,215,
20,216,4,18,102,
0,105,0,101,0,
108,0,100,0,108,
0,105,0,115,0,
116,0,1,66,1,
2,2,0,1,840,
217,18,1,840,218,
20,219,4,8,84,
0,72,0,69,0,
78,0,1,35,1,
1,2,0,1,302,
220,18,1,302,221,
20,222,4,16,102,
0,105,0,101,0,
108,0,100,0,115,
0,101,0,112,0,
1,114,1,2,2,
0,1,301,223,18,
1,301,126,2,0,
1,300,224,18,1,
300,225,20,226,4,
18,83,0,69,0,
77,0,73,0,67,
0,79,0,76,0,
79,0,78,0,1,
9,1,1,2,0,
1,299,227,18,1,
299,228,20,229,4,
10,102,0,105,0,
101,0,108,0,100,
0,1,63,1,2,
2,0,1,298,230,
18,1,298,231,20,
232,4,12,82,0,
66,0,82,0,65,
0,67,0,69,0,
1,15,1,1,2,
0,1,296,233,18,
1,296,231,2,0,
1,295,234,18,1,
295,215,2,0,1,
812,235,18,1,812,
200,2,0,1,791,
236,18,1,791,102,
2,0,1,788,237,
18,1,788,102,2,
0,1,772,238,18,
1,772,225,2,0,
1,756,239,18,1,
756,240,20,241,4,
8,115,0,116,0,
97,0,116,0,1,
80,1,2,2,0,
1,755,242,18,1,
755,102,2,0,1,
754,243,18,1,754,
132,2,0,1,210,
244,18,1,210,107,
2,0,1,209,245,
18,1,209,142,2,
0,1,734,246,18,
1,734,142,2,0,
1,733,247,18,1,
733,196,2,0,1,
731,248,18,1,731,
110,2,0,1,730,
249,18,1,730,113,
2,0,1,208,250,
18,1,208,251,20,
252,4,12,76,0,
66,0,82,0,65,
0,67,0,75,0,
1,12,1,1,2,
0,1,207,253,18,
1,207,107,2,0,
1,206,254,18,1,
206,203,2,0,1,
716,255,18,1,716,
129,2,0,1,715,
256,18,1,715,110,
2,0,1,714,257,
18,1,714,113,2,
0,1,712,258,18,
1,712,110,2,0,
1,699,259,18,1,
699,129,2,0,1,
171,260,18,1,171,
107,2,0,1,170,
261,18,1,170,142,
2,0,1,169,262,
18,1,169,145,2,
0,1,682,263,18,
1,682,107,2,0,
1,671,264,18,1,
671,265,20,266,4,
10,87,0,72,0,
73,0,76,0,69,
0,1,39,1,1,
2,0,1,144,267,
18,1,144,107,2,
0,1,131,268,18,
1,131,269,20,270,
4,6,78,0,73,
0,76,0,1,44,
1,1,2,0,1,
130,271,18,1,130,
272,20,273,4,10,
70,0,65,0,76,
0,83,0,69,0,
1,51,1,1,2,
0,1,129,274,18,
1,129,275,20,276,
4,8,84,0,82,
0,85,0,69,0,
1,50,1,1,2,
0,1,128,277,18,
1,128,278,20,279,
4,12,78,0,85,
0,77,0,66,0,
69,0,82,0,1,
6,1,1,2,0,
1,127,280,18,1,
127,281,20,282,4,
14,76,0,73,0,
84,0,69,0,82,
0,65,0,76,0,
1,3,1,1,2,
0,1,126,283,18,
1,126,284,20,285,
4,16,102,0,117,
0,110,0,99,0,
116,0,105,0,111,
0,110,0,1,68,
1,2,2,0,1,
125,286,18,1,125,
287,20,288,4,32,
116,0,97,0,98,
0,108,0,101,0,
99,0,111,0,110,
0,115,0,116,0,
114,0,117,0,99,
0,116,0,111,0,
114,0,1,67,1,
2,2,0,1,645,
289,18,1,645,107,
2,0,1,618,290,
18,1,618,113,2,
0,1,634,291,18,
1,634,292,20,293,
4,10,85,0,78,
0,84,0,73,0,
76,0,1,47,1,
1,2,0,1,633,
294,18,1,633,113,
2,0,1,1048,295,
18,1,1048,296,20,
297,4,12,69,0,
76,0,73,0,80,
0,83,0,69,0,
1,53,1,1,2,
0,1,107,298,18,
1,107,107,2,0,
1,621,299,18,1,
621,300,20,301,4,
12,82,0,69,0,
80,0,69,0,65,
0,84,0,1,46,
1,1,2,0,1,
619,302,18,1,619,
110,2,0,1,97,
303,18,1,97,304,
20,305,4,8,117,
0,110,0,111,0,
112,0,1,56,1,
2,2,0,1,96,
306,18,1,96,307,
20,308,4,6,118,
0,97,0,114,0,
1,75,1,2,2,
0,1,95,309,18,
1,95,310,20,311,
4,24,102,0,117,
0,110,0,99,0,
116,0,105,0,111,
0,110,0,99,0,
97,0,108,0,108,
0,1,58,1,2,
2,0,1,92,312,
18,1,92,117,2,
0,1,607,313,18,
1,607,209,2,0,
1,606,314,18,1,
606,113,2,0,1,
1090,315,18,1,1090,
316,23,317,4,6,
69,0,79,0,70,
0,1,2,1,6,
2,0,1,1089,104,
1,76,318,18,1,
76,107,2,0,1,
1052,319,18,1,1052,
120,2,0,1,578,
320,18,1,578,107,
2,0,1,543,321,
18,1,543,113,2,
0,1,595,322,18,
1,595,218,2,0,
1,69,323,18,1,
69,324,20,325,4,
12,76,0,80,0,
65,0,82,0,69,
0,78,0,1,10,
1,1,2,0,1,
567,326,18,1,567,
327,20,328,4,4,
73,0,70,0,1,
34,1,1,2,0,
1,566,329,18,1,
566,132,2,0,1,
62,330,18,1,62,
331,20,332,4,10,
98,0,105,0,110,
0,111,0,112,0,
1,57,1,2,2,
0,1,61,333,18,
1,61,334,20,335,
4,8,80,0,76,
0,85,0,83,0,
1,17,1,1,2,
0,1,60,336,18,
1,60,337,20,338,
4,10,77,0,73,
0,78,0,85,0,
83,0,1,18,1,
1,2,0,1,59,
339,18,1,59,340,
20,341,4,8,77,
0,85,0,76,0,
84,0,1,19,1,
1,2,0,1,58,
342,18,1,58,343,
20,344,4,6,77,
0,79,0,68,0,
1,21,1,1,2,
0,1,57,345,18,
1,57,346,20,347,
4,12,68,0,73,
0,86,0,73,0,
68,0,69,0,1,
22,1,1,2,0,
1,56,348,18,1,
56,349,20,350,4,
6,69,0,88,0,
80,0,1,23,1,
1,2,0,1,55,
351,18,1,55,352,
20,353,4,12,67,
0,79,0,78,0,
67,0,65,0,84,
0,1,52,1,1,
2,0,1,54,354,
18,1,54,355,20,
356,4,4,76,0,
84,0,1,26,1,
1,2,0,1,53,
357,18,1,53,358,
20,359,4,4,71,
0,84,0,1,28,
1,1,2,0,1,
52,360,18,1,52,
361,20,362,4,4,
71,0,69,0,1,
29,1,1,2,0,
1,51,363,18,1,
51,364,20,365,4,
4,76,0,69,0,
1,27,1,1,2,
0,1,50,366,18,
1,50,367,20,368,
4,4,69,0,81,
0,1,24,1,1,
2,0,1,49,369,
18,1,49,370,20,
371,4,6,65,0,
78,0,68,0,1,
30,1,1,2,0,
1,48,372,18,1,
48,373,20,374,4,
4,79,0,82,0,
1,31,1,1,2,
0,1,47,375,18,
1,47,376,20,377,
4,6,78,0,69,
0,81,0,1,25,
1,1,2,0,1,
46,378,18,1,46,
107,2,0,1,45,
379,18,1,45,380,
20,381,4,12,76,
0,66,0,82,0,
65,0,67,0,69,
0,1,14,1,1,
2,0,1,44,382,
18,1,44,212,2,
0,1,1050,383,18,
1,1050,126,2,0,
1,1049,384,18,1,
1049,145,2,0,1,
40,385,18,1,40,
145,2,0,1,39,
386,18,1,39,162,
2,0,1,506,387,
18,1,506,126,2,
0,1,33,388,18,
1,33,389,20,390,
4,18,112,0,114,
0,101,0,102,0,
105,0,120,0,101,
0,120,0,112,0,
1,62,1,2,2,
0,1,534,391,18,
1,534,129,2,0,
1,28,392,18,1,
28,251,2,0,1,
27,393,18,1,27,
145,2,0,1,26,
394,18,1,26,158,
2,0,1,546,395,
18,1,546,396,20,
397,4,12,82,0,
69,0,84,0,85,
0,82,0,78,0,
1,48,1,1,2,
0,1,545,398,18,
1,545,399,20,400,
4,10,66,0,82,
0,69,0,65,0,
75,0,1,43,1,
1,2,0,1,544,
401,18,1,544,110,
2,0,1,22,402,
18,1,22,389,2,
0,1,21,403,18,
1,21,126,2,0,
1,20,404,18,1,
20,307,2,0,1,
19,405,18,1,19,
145,2,0,1,17,
406,18,1,17,110,
2,0,1,16,407,
18,1,16,113,2,
0,1,15,408,18,
1,15,110,2,0,
1,14,409,18,1,
14,117,2,0,1,
13,410,18,1,13,
324,2,0,1,12,
411,18,1,12,151,
2,0,1,11,412,
18,1,11,166,2,
0,1,10,413,18,
1,10,117,2,0,
1,9,414,18,1,
9,117,2,0,1,
8,415,18,1,8,
132,2,0,1,7,
416,18,1,7,337,
2,0,1,6,417,
18,1,6,418,20,
419,4,6,78,0,
79,0,84,0,1,
32,1,1,2,0,
1,5,420,18,1,
5,421,20,422,4,
10,80,0,79,0,
85,0,78,0,68,
0,1,20,1,1,
2,0,1,4,423,
18,1,4,324,2,
0,1,3,424,18,
1,3,287,2,0,
1,2,425,18,1,
2,281,2,0,1,
1,426,18,1,1,
389,2,0,1,0,
427,18,1,0,0,
2,0,428,5,0,
429,5,182,1,184,
430,19,431,4,10,
97,0,114,0,103,
0,95,0,52,0,
1,184,432,5,4,
1,40,433,16,0,
382,1,22,434,16,
0,211,1,1,435,
16,0,211,1,33,
436,16,0,211,1,
183,437,19,438,4,
12,117,0,110,0,
111,0,112,0,95,
0,51,0,1,183,
439,5,23,1,374,
440,16,0,303,1,
567,441,16,0,303,
1,812,442,16,0,
303,1,546,443,16,
0,303,1,976,444,
16,0,303,1,170,
445,16,0,303,1,
28,446,16,0,303,
1,450,447,16,0,
303,1,402,448,16,
0,303,1,634,449,
16,0,303,1,69,
450,16,0,303,1,
209,451,16,0,303,
1,302,452,16,0,
303,1,206,453,16,
0,303,1,62,454,
16,0,303,1,671,
455,16,0,303,1,
506,456,16,0,303,
1,478,457,16,0,
303,1,734,458,16,
0,303,1,4,459,
16,0,303,1,97,
460,16,0,303,1,
866,461,16,0,303,
1,45,462,16,0,
303,1,182,463,19,
464,4,12,117,0,
110,0,111,0,112,
0,95,0,50,0,
1,182,439,1,181,
465,19,466,4,12,
117,0,110,0,111,
0,112,0,95,0,
49,0,1,181,439,
1,180,467,19,468,
4,10,97,0,114,
0,103,0,95,0,
51,0,1,180,432,
1,179,469,19,470,
4,16,98,0,105,
0,110,0,111,0,
112,0,95,0,49,
0,53,0,1,179,
471,5,16,1,645,
472,16,0,330,1,
107,473,16,0,330,
1,823,474,16,0,
330,1,385,475,16,
0,330,1,682,476,
16,0,330,1,578,
477,16,0,330,1,
144,478,16,0,330,
1,517,479,16,0,
330,1,171,480,16,
0,330,1,46,481,
16,0,330,1,76,
482,16,0,330,1,
489,483,16,0,330,
1,327,484,16,0,
330,1,210,485,16,
0,330,1,461,486,
16,0,330,1,207,
487,16,0,330,1,
178,488,19,489,4,
16,98,0,105,0,
110,0,111,0,112,
0,95,0,49,0,
52,0,1,178,471,
1,177,490,19,491,
4,16,98,0,105,
0,110,0,111,0,
112,0,95,0,49,
0,51,0,1,177,
471,1,176,492,19,
493,4,16,98,0,
105,0,110,0,111,
0,112,0,95,0,
49,0,50,0,1,
176,471,1,175,494,
19,495,4,16,98,
0,105,0,110,0,
111,0,112,0,95,
0,49,0,49,0,
1,175,471,1,174,
496,19,497,4,16,
98,0,105,0,110,
0,111,0,112,0,
95,0,49,0,48,
0,1,174,471,1,
173,498,19,499,4,
14,98,0,105,0,
110,0,111,0,112,
0,95,0,57,0,
1,173,471,1,172,
500,19,501,4,14,
98,0,105,0,110,
0,111,0,112,0,
95,0,56,0,1,
172,471,1,171,502,
19,503,4,14,98,
0,105,0,110,0,
111,0,112,0,95,
0,55,0,1,171,
471,1,170,504,19,
505,4,14,98,0,
105,0,110,0,111,
0,112,0,95,0,
54,0,1,170,471,
1,169,506,19,507,
4,14,98,0,105,
0,110,0,111,0,
112,0,95,0,53,
0,1,169,471,1,
168,508,19,509,4,
14,98,0,105,0,
110,0,111,0,112,
0,95,0,52,0,
1,168,471,1,167,
510,19,511,4,14,
98,0,105,0,110,
0,111,0,112,0,
95,0,51,0,1,
167,471,1,166,512,
19,513,4,14,98,
0,105,0,110,0,
111,0,112,0,95,
0,50,0,1,166,
471,1,165,514,19,
515,4,14,98,0,
105,0,110,0,111,
0,112,0,95,0,
49,0,1,165,471,
1,164,516,19,517,
4,18,112,0,97,
0,114,0,108,0,
105,0,115,0,116,
0,95,0,51,0,
1,164,518,5,2,
1,1050,519,16,0,
319,1,13,520,16,
0,119,1,163,521,
19,522,4,20,102,
0,105,0,101,0,
108,0,100,0,115,
0,101,0,112,0,
95,0,50,0,1,
163,523,5,1,1,
299,524,16,0,220,
1,162,525,19,526,
4,20,102,0,105,
0,101,0,108,0,
100,0,115,0,101,
0,112,0,95,0,
49,0,1,162,523,
1,161,527,19,528,
4,16,101,0,108,
0,115,0,101,0,
105,0,102,0,95,
0,51,0,1,161,
529,5,2,1,812,
530,16,0,182,1,
866,531,16,0,194,
1,160,532,19,533,
4,16,101,0,108,
0,115,0,101,0,
105,0,102,0,95,
0,50,0,1,160,
529,1,159,534,19,
535,4,16,101,0,
108,0,115,0,101,
0,105,0,102,0,
95,0,49,0,1,
159,529,1,158,536,
19,537,4,20,110,
0,97,0,109,0,
101,0,108,0,105,
0,115,0,116,0,
95,0,51,0,1,
158,538,5,3,1,
370,539,16,0,188,
1,447,540,16,0,
138,1,368,541,16,
0,187,1,157,542,
19,543,4,20,110,
0,97,0,109,0,
101,0,108,0,105,
0,115,0,116,0,
95,0,50,0,1,
157,538,1,156,544,
19,545,4,20,102,
0,117,0,110,0,
99,0,110,0,97,
0,109,0,101,0,
95,0,51,0,1,
156,546,5,2,1,
437,547,16,0,153,
1,441,548,16,0,
156,1,155,549,19,
550,4,20,102,0,
117,0,110,0,99,
0,110,0,97,0,
109,0,101,0,95,
0,50,0,1,155,
546,1,154,551,19,
552,4,20,110,0,
97,0,109,0,101,
0,108,0,105,0,
115,0,116,0,95,
0,49,0,1,154,
538,1,153,553,19,
554,4,14,102,0,
105,0,101,0,108,
0,100,0,95,0,
49,0,1,153,555,
5,2,1,302,556,
16,0,227,1,45,
557,16,0,227,1,
152,558,19,559,4,
26,70,0,105,0,
101,0,108,0,100,
0,65,0,115,0,
115,0,105,0,103,
0,110,0,95,0,
49,0,1,152,555,
1,151,560,19,561,
4,32,70,0,105,
0,101,0,108,0,
100,0,69,0,120,
0,112,0,65,0,
115,0,115,0,105,
0,103,0,110,0,
95,0,49,0,1,
151,555,1,150,562,
19,563,4,10,97,
0,114,0,103,0,
95,0,50,0,1,
150,432,1,149,564,
19,565,4,10,97,
0,114,0,103,0,
95,0,49,0,1,
149,432,1,148,566,
19,567,4,20,102,
0,117,0,110,0,
99,0,116,0,105,
0,111,0,110,0,
95,0,49,0,1,
148,568,5,23,1,
374,569,16,0,283,
1,567,570,16,0,
283,1,812,571,16,
0,283,1,546,572,
16,0,283,1,976,
573,16,0,283,1,
170,574,16,0,283,
1,28,575,16,0,
283,1,450,576,16,
0,283,1,402,577,
16,0,283,1,634,
578,16,0,283,1,
69,579,16,0,283,
1,209,580,16,0,
283,1,302,581,16,
0,283,1,206,582,
16,0,283,1,62,
583,16,0,283,1,
671,584,16,0,283,
1,506,585,16,0,
283,1,478,586,16,
0,283,1,734,587,
16,0,283,1,4,
588,16,0,283,1,
97,589,16,0,283,
1,866,590,16,0,
283,1,45,591,16,
0,283,1,147,592,
19,593,4,20,102,
0,117,0,110,0,
99,0,98,0,111,
0,100,0,121,0,
95,0,52,0,1,
147,594,5,3,1,
434,595,16,0,168,
1,444,596,16,0,
150,1,11,597,16,
0,411,1,146,598,
19,599,4,20,102,
0,117,0,110,0,
99,0,98,0,111,
0,100,0,121,0,
95,0,51,0,1,
146,594,1,145,600,
19,601,4,20,102,
0,117,0,110,0,
99,0,98,0,111,
0,100,0,121,0,
95,0,50,0,1,
145,594,1,144,602,
19,603,4,20,102,
0,117,0,110,0,
99,0,98,0,111,
0,100,0,121,0,
95,0,49,0,1,
144,594,1,143,604,
19,605,4,20,102,
0,117,0,110,0,
99,0,110,0,97,
0,109,0,101,0,
95,0,49,0,1,
143,546,1,142,606,
19,607,4,24,80,
0,97,0,99,0,
107,0,97,0,103,
0,101,0,82,0,
101,0,102,0,95,
0,49,0,1,142,
608,5,39,1,534,
609,16,0,404,1,
209,610,16,0,306,
1,206,611,16,0,
306,1,847,612,16,
0,404,1,97,613,
16,0,306,1,734,
614,16,0,306,1,
840,615,16,0,404,
1,302,616,16,0,
306,1,621,617,16,
0,404,1,939,618,
16,0,404,1,402,
619,16,0,306,1,
506,620,16,0,306,
1,976,621,16,0,
306,1,716,622,16,
0,404,1,607,623,
16,0,404,1,170,
624,16,0,306,1,
69,625,16,0,306,
1,1027,626,16,0,
404,1,812,627,16,
0,306,1,62,628,
16,0,306,1,595,
629,16,0,404,1,
699,630,16,0,404,
1,374,631,16,0,
306,1,478,632,16,
0,306,1,45,633,
16,0,306,1,997,
634,16,0,404,1,
567,635,16,0,306,
1,671,636,16,0,
306,1,28,637,16,
0,306,1,772,638,
16,0,404,1,450,
639,16,0,306,1,
21,640,16,0,404,
1,14,641,16,0,
404,1,634,642,16,
0,306,1,546,643,
16,0,306,1,866,
644,16,0,306,1,
756,645,16,0,404,
1,4,646,16,0,
306,1,0,647,16,
0,404,1,141,648,
19,649,4,20,84,
0,97,0,98,0,
108,0,101,0,82,
0,101,0,102,0,
95,0,49,0,1,
141,608,1,140,650,
19,651,4,10,118,
0,97,0,114,0,
95,0,49,0,1,
140,608,1,139,652,
19,653,4,18,118,
0,97,0,114,0,
108,0,105,0,115,
0,116,0,95,0,
50,0,1,139,654,
5,16,1,21,655,
16,0,195,1,756,
656,16,0,247,1,
847,657,16,0,247,
1,534,658,16,0,
247,1,595,659,16,
0,247,1,1027,660,
16,0,247,1,14,
661,16,0,247,1,
772,662,16,0,247,
1,840,663,16,0,
247,1,699,664,16,
0,247,1,997,665,
16,0,247,1,607,
666,16,0,247,1,
939,667,16,0,247,
1,716,668,16,0,
247,1,0,669,16,
0,247,1,621,670,
16,0,247,1,138,
671,19,672,4,18,
118,0,97,0,114,
0,108,0,105,0,
115,0,116,0,95,
0,49,0,1,138,
654,1,137,673,19,
674,4,22,112,0,
114,0,101,0,102,
0,105,0,120,0,
101,0,120,0,112,
0,95,0,51,0,
1,137,675,5,39,
1,534,676,16,0,
426,1,209,677,16,
0,388,1,206,678,
16,0,388,1,847,
679,16,0,426,1,
97,680,16,0,388,
1,734,681,16,0,
388,1,840,682,16,
0,426,1,302,683,
16,0,388,1,621,
684,16,0,426,1,
939,685,16,0,426,
1,402,686,16,0,
388,1,506,687,16,
0,388,1,976,688,
16,0,388,1,716,
689,16,0,426,1,
607,690,16,0,426,
1,170,691,16,0,
388,1,69,692,16,
0,388,1,1027,693,
16,0,426,1,812,
694,16,0,388,1,
62,695,16,0,388,
1,595,696,16,0,
426,1,699,697,16,
0,426,1,374,698,
16,0,388,1,478,
699,16,0,388,1,
45,700,16,0,388,
1,997,701,16,0,
426,1,567,702,16,
0,388,1,671,703,
16,0,388,1,28,
704,16,0,388,1,
772,705,16,0,426,
1,450,706,16,0,
388,1,21,707,16,
0,402,1,14,708,
16,0,426,1,634,
709,16,0,388,1,
546,710,16,0,388,
1,866,711,16,0,
388,1,756,712,16,
0,426,1,4,713,
16,0,388,1,0,
714,16,0,426,1,
136,715,19,716,4,
22,112,0,114,0,
101,0,102,0,105,
0,120,0,101,0,
120,0,112,0,95,
0,50,0,1,136,
675,1,135,717,19,
718,4,22,112,0,
114,0,101,0,102,
0,105,0,120,0,
101,0,120,0,112,
0,95,0,49,0,
1,135,675,1,134,
719,19,720,4,28,
102,0,117,0,110,
0,99,0,116,0,
105,0,111,0,110,
0,99,0,97,0,
108,0,108,0,95,
0,50,0,1,134,
721,5,39,1,534,
722,16,0,309,1,
209,723,16,0,309,
1,206,724,16,0,
309,1,847,725,16,
0,309,1,97,726,
16,0,309,1,734,
727,16,0,309,1,
840,728,16,0,309,
1,302,729,16,0,
309,1,621,730,16,
0,309,1,939,731,
16,0,309,1,402,
732,16,0,309,1,
506,733,16,0,309,
1,976,734,16,0,
309,1,716,735,16,
0,309,1,607,736,
16,0,309,1,170,
737,16,0,309,1,
69,738,16,0,309,
1,1027,739,16,0,
309,1,812,740,16,
0,309,1,62,741,
16,0,309,1,595,
742,16,0,309,1,
699,743,16,0,309,
1,374,744,16,0,
309,1,478,745,16,
0,309,1,45,746,
16,0,309,1,997,
747,16,0,309,1,
567,748,16,0,309,
1,671,749,16,0,
309,1,28,750,16,
0,309,1,772,751,
16,0,309,1,450,
752,16,0,309,1,
21,753,16,0,309,
1,14,754,16,0,
309,1,634,755,16,
0,309,1,546,756,
16,0,309,1,866,
757,16,0,309,1,
756,758,16,0,309,
1,4,759,16,0,
309,1,0,760,16,
0,309,1,133,761,
19,762,4,28,102,
0,117,0,110,0,
99,0,116,0,105,
0,111,0,110,0,
99,0,97,0,108,
0,108,0,95,0,
49,0,1,133,721,
1,132,763,19,764,
4,12,85,0,110,
0,111,0,112,0,
95,0,49,0,1,
132,765,5,23,1,
374,766,16,0,185,
1,567,767,16,0,
320,1,812,768,16,
0,206,1,546,769,
16,0,185,1,976,
770,16,0,185,1,
170,771,16,0,260,
1,28,772,16,0,
207,1,450,773,16,
0,134,1,402,774,
16,0,185,1,634,
775,16,0,289,1,
69,776,16,0,318,
1,209,777,16,0,
244,1,302,778,16,
0,378,1,206,779,
16,0,253,1,62,
780,16,0,267,1,
671,781,16,0,263,
1,506,782,16,0,
106,1,478,783,16,
0,122,1,734,784,
16,0,185,1,4,
785,16,0,185,1,
97,786,16,0,298,
1,866,787,16,0,
206,1,45,788,16,
0,378,1,131,789,
19,790,4,14,66,
0,105,0,110,0,
111,0,112,0,95,
0,49,0,1,131,
765,1,130,791,19,
792,4,26,69,0,
120,0,112,0,84,
0,97,0,98,0,
108,0,101,0,68,
0,101,0,99,0,
95,0,49,0,1,
130,765,1,129,793,
19,794,4,10,101,
0,120,0,112,0,
95,0,50,0,1,
129,765,1,128,795,
19,796,4,10,101,
0,120,0,112,0,
95,0,49,0,1,
128,765,1,127,797,
19,798,4,12,65,
0,116,0,111,0,
109,0,95,0,53,
0,1,127,765,1,
126,799,19,800,4,
12,65,0,116,0,
111,0,109,0,95,
0,52,0,1,126,
765,1,125,801,19,
802,4,12,65,0,
116,0,111,0,109,
0,95,0,51,0,
1,125,765,1,124,
803,19,804,4,12,
65,0,116,0,111,
0,109,0,95,0,
50,0,1,124,765,
1,123,805,19,806,
4,12,65,0,116,
0,111,0,109,0,
95,0,49,0,1,
123,765,1,122,807,
19,808,4,18,101,
0,120,0,112,0,
108,0,105,0,115,
0,116,0,95,0,
50,0,1,122,809,
5,6,1,976,810,
16,0,131,1,546,
811,16,0,329,1,
402,812,16,0,177,
1,4,813,16,0,
415,1,734,814,16,
0,243,1,374,815,
16,0,174,1,121,
816,19,817,4,18,
101,0,120,0,112,
0,108,0,105,0,
115,0,116,0,95,
0,49,0,1,121,
809,1,120,818,19,
819,4,12,105,0,
110,0,105,0,116,
0,95,0,49,0,
1,120,820,5,1,
1,373,821,16,0,
171,1,119,822,19,
823,4,18,112,0,
97,0,114,0,108,
0,105,0,115,0,
116,0,95,0,50,
0,1,119,518,1,
118,824,19,825,4,
18,112,0,97,0,
114,0,108,0,105,
0,115,0,116,0,
95,0,49,0,1,
118,518,1,117,826,
19,827,4,36,116,
0,97,0,98,0,
108,0,101,0,99,
0,111,0,110,0,
115,0,116,0,114,
0,117,0,99,0,
116,0,111,0,114,
0,95,0,50,0,
1,117,828,5,27,
1,374,829,16,0,
286,1,40,830,16,
0,424,1,567,831,
16,0,286,1,812,
832,16,0,286,1,
546,833,16,0,286,
1,976,834,16,0,
286,1,33,835,16,
0,424,1,170,836,
16,0,286,1,28,
837,16,0,286,1,
450,838,16,0,286,
1,402,839,16,0,
286,1,22,840,16,
0,424,1,634,841,
16,0,286,1,69,
842,16,0,286,1,
209,843,16,0,286,
1,302,844,16,0,
286,1,206,845,16,
0,286,1,62,846,
16,0,286,1,671,
847,16,0,286,1,
506,848,16,0,286,
1,478,849,16,0,
286,1,734,850,16,
0,286,1,1,851,
16,0,424,1,4,
852,16,0,286,1,
97,853,16,0,286,
1,866,854,16,0,
286,1,45,855,16,
0,286,1,116,856,
19,857,4,36,116,
0,97,0,98,0,
108,0,101,0,99,
0,111,0,110,0,
115,0,116,0,114,
0,117,0,99,0,
116,0,111,0,114,
0,95,0,49,0,
1,116,828,1,115,
858,19,859,4,22,
102,0,105,0,101,
0,108,0,100,0,
108,0,105,0,115,
0,116,0,95,0,
50,0,1,115,860,
5,2,1,302,861,
16,0,214,1,45,
862,16,0,234,1,
114,863,19,222,1,
114,523,1,113,864,
19,865,4,22,102,
0,105,0,101,0,
108,0,100,0,108,
0,105,0,115,0,
116,0,95,0,49,
0,1,113,860,1,
112,866,19,867,4,
22,76,0,111,0,
99,0,97,0,108,
0,73,0,110,0,
105,0,116,0,95,
0,49,0,1,112,
868,5,15,1,756,
869,16,0,239,1,
847,870,16,0,239,
1,534,871,16,0,
239,1,595,872,16,
0,239,1,1027,873,
16,0,239,1,14,
874,16,0,239,1,
772,875,16,0,239,
1,840,876,16,0,
239,1,699,877,16,
0,239,1,997,878,
16,0,239,1,607,
879,16,0,239,1,
939,880,16,0,239,
1,716,881,16,0,
239,1,0,882,16,
0,239,1,621,883,
16,0,239,1,111,
884,19,885,4,14,
115,0,116,0,97,
0,116,0,95,0,
49,0,52,0,1,
111,868,1,110,886,
19,887,4,30,76,
0,111,0,99,0,
97,0,108,0,70,
0,117,0,110,0,
99,0,68,0,101,
0,99,0,108,0,
95,0,49,0,1,
110,868,1,109,888,
19,889,4,20,70,
0,117,0,110,0,
99,0,68,0,101,
0,99,0,108,0,
95,0,49,0,1,
109,868,1,108,890,
19,891,4,14,115,
0,116,0,97,0,
116,0,95,0,49,
0,51,0,1,108,
868,1,107,892,19,
140,1,107,538,1,
106,893,19,894,4,
14,115,0,116,0,
97,0,116,0,95,
0,49,0,50,0,
1,106,868,1,105,
895,19,896,4,14,
115,0,116,0,97,
0,116,0,95,0,
49,0,49,0,1,
105,868,1,104,897,
19,898,4,14,115,
0,116,0,97,0,
116,0,95,0,49,
0,48,0,1,104,
868,1,103,899,19,
900,4,16,82,0,
101,0,116,0,118,
0,97,0,108,0,
95,0,49,0,1,
103,868,1,102,901,
19,902,4,12,115,
0,116,0,97,0,
116,0,95,0,57,
0,1,102,868,1,
101,903,19,904,4,
12,115,0,116,0,
97,0,116,0,95,
0,56,0,1,101,
868,1,100,905,19,
906,4,12,115,0,
116,0,97,0,116,
0,95,0,55,0,
1,100,868,1,99,
907,19,184,1,99,
529,1,98,908,19,
909,4,12,115,0,
116,0,97,0,116,
0,95,0,54,0,
1,98,868,1,97,
910,19,911,4,12,
115,0,116,0,97,
0,116,0,95,0,
53,0,1,97,868,
1,96,912,19,913,
4,12,115,0,116,
0,97,0,116,0,
95,0,52,0,1,
96,868,1,95,914,
19,915,4,12,115,
0,116,0,97,0,
116,0,95,0,51,
0,1,95,868,1,
94,916,19,917,4,
12,115,0,116,0,
97,0,116,0,95,
0,50,0,1,94,
868,1,93,918,19,
919,4,12,115,0,
116,0,97,0,116,
0,95,0,49,0,
1,93,868,1,92,
920,19,921,4,24,
65,0,115,0,115,
0,105,0,103,0,
110,0,109,0,101,
0,110,0,116,0,
95,0,49,0,1,
92,868,1,91,922,
19,923,4,14,98,
0,108,0,111,0,
99,0,107,0,95,
0,49,0,1,91,
924,5,12,1,595,
925,16,0,314,1,
847,926,16,0,205,
1,534,927,16,0,
321,1,1027,928,16,
0,112,1,14,929,
16,0,407,1,840,
930,16,0,198,1,
699,931,16,0,257,
1,997,932,16,0,
124,1,607,933,16,
0,290,1,939,934,
16,0,176,1,716,
935,16,0,249,1,
621,936,16,0,294,
1,90,937,19,938,
4,14,99,0,104,
0,117,0,110,0,
107,0,95,0,52,
0,1,90,939,5,
15,1,756,940,16,
0,236,1,847,941,
16,0,242,1,534,
942,16,0,242,1,
595,943,16,0,242,
1,1027,944,16,0,
242,1,14,945,16,
0,242,1,772,946,
16,0,237,1,840,
947,16,0,242,1,
699,948,16,0,242,
1,997,949,16,0,
242,1,607,950,16,
0,242,1,939,951,
16,0,242,1,716,
952,16,0,242,1,
0,953,16,0,104,
1,621,954,16,0,
242,1,89,955,19,
956,4,14,99,0,
104,0,117,0,110,
0,107,0,95,0,
51,0,1,89,939,
1,88,957,19,958,
4,14,99,0,104,
0,117,0,110,0,
107,0,95,0,50,
0,1,88,939,1,
87,959,19,960,4,
14,99,0,104,0,
117,0,110,0,107,
0,95,0,49,0,
1,87,939,1,86,
961,19,213,1,86,
432,1,85,962,19,
963,4,26,76,0,
111,0,99,0,97,
0,108,0,70,0,
117,0,110,0,99,
0,68,0,101,0,
99,0,108,0,1,
85,868,1,84,964,
19,965,4,16,70,
0,117,0,110,0,
99,0,68,0,101,
0,99,0,108,0,
1,84,868,1,83,
966,19,967,4,12,
82,0,101,0,116,
0,118,0,97,0,
108,0,1,83,868,
1,82,968,19,969,
4,18,76,0,111,
0,99,0,97,0,
108,0,73,0,110,
0,105,0,116,0,
1,82,868,1,81,
970,19,971,4,20,
65,0,115,0,115,
0,105,0,103,0,
110,0,109,0,101,
0,110,0,116,0,
1,81,868,1,80,
972,19,241,1,80,
868,1,79,973,19,
173,1,79,820,1,
78,974,19,197,1,
78,654,1,77,975,
19,976,4,16,84,
0,97,0,98,0,
108,0,101,0,82,
0,101,0,102,0,
1,77,608,1,76,
977,19,978,4,20,
80,0,97,0,99,
0,107,0,97,0,
103,0,101,0,82,
0,101,0,102,0,
1,76,608,1,75,
979,19,308,1,75,
608,1,74,980,19,
133,1,74,809,1,
73,981,19,982,4,
8,65,0,116,0,
111,0,109,0,1,
73,765,1,72,983,
19,984,4,22,69,
0,120,0,112,0,
84,0,97,0,98,
0,108,0,101,0,
68,0,101,0,99,
0,1,72,765,1,
71,985,19,986,4,
8,85,0,110,0,
111,0,112,0,1,
71,765,1,70,987,
19,988,4,10,66,
0,105,0,110,0,
111,0,112,0,1,
70,765,1,69,989,
19,108,1,69,765,
1,68,990,19,285,
1,68,568,1,67,
991,19,288,1,67,
828,1,66,992,19,
216,1,66,860,1,
65,993,19,994,4,
22,70,0,105,0,
101,0,108,0,100,
0,65,0,115,0,
115,0,105,0,103,
0,110,0,1,65,
555,1,64,995,19,
996,4,28,70,0,
105,0,101,0,108,
0,100,0,69,0,
120,0,112,0,65,
0,115,0,115,0,
105,0,103,0,110,
0,1,64,555,1,
63,997,19,229,1,
63,555,1,62,998,
19,390,1,62,675,
1,61,999,19,152,
1,61,594,1,60,
1000,19,121,1,60,
518,1,59,1001,19,
155,1,59,546,1,
58,1002,19,311,1,
58,721,1,57,1003,
19,332,1,57,471,
1,56,1004,19,305,
1,56,439,1,55,
1005,19,114,1,55,
924,1,54,1006,19,
103,1,54,939,1,
53,1007,19,297,1,
53,1008,5,2,1,
1050,1009,16,0,295,
1,13,1010,16,0,
295,1,52,1011,19,
353,1,52,1012,5,
45,1,210,1013,16,
0,351,1,207,1014,
16,0,351,1,96,
1015,17,1016,15,1017,
4,20,37,0,112,
0,114,0,101,0,
102,0,105,0,120,
0,101,0,120,0,
112,0,1,-1,1,
5,1018,20,718,1,
135,1,3,1,2,
1,1,1019,22,1,
50,1,95,1020,17,
1021,15,1017,1,-1,
1,5,1022,20,716,
1,136,1,3,1,
2,1,1,1023,22,
1,51,1,92,1024,
17,1025,15,1017,1,
-1,1,5,1026,20,
674,1,137,1,3,
1,4,1,3,1027,
22,1,52,1,517,
1028,16,0,351,1,
298,1029,17,1030,15,
1031,4,34,37,0,
116,0,97,0,98,
0,108,0,101,0,
99,0,111,0,110,
0,115,0,116,0,
114,0,117,0,99,
0,116,0,111,0,
114,0,1,-1,1,
5,1032,20,857,1,
116,1,3,1,3,
1,2,1033,22,1,
30,1,296,1034,17,
1035,15,1031,1,-1,
1,5,1036,20,827,
1,117,1,3,1,
4,1,3,1037,22,
1,31,1,76,1038,
16,0,351,1,823,
1039,16,0,351,1,
171,1040,16,0,351,
1,1030,1041,17,1042,
15,1043,4,18,37,
0,102,0,117,0,
110,0,99,0,98,
0,111,0,100,0,
121,0,1,-1,1,
5,1044,20,601,1,
145,1,3,1,6,
1,5,1045,22,1,
64,1,1028,1046,17,
1047,15,1043,1,-1,
1,5,1048,20,599,
1,146,1,3,1,
5,1,4,1049,22,
1,65,1,385,1050,
16,0,351,1,169,
1051,17,1052,15,1053,
4,8,37,0,118,
0,97,0,114,0,
1,-1,1,5,1054,
20,651,1,140,1,
3,1,2,1,1,
1055,22,1,57,1,
489,1056,16,0,351,
1,46,1057,16,0,
351,1,44,1058,17,
1059,15,1060,4,26,
37,0,102,0,117,
0,110,0,99,0,
116,0,105,0,111,
0,110,0,99,0,
97,0,108,0,108,
0,1,-1,1,5,
1061,20,720,1,134,
1,3,1,5,1,
4,1062,22,1,49,
1,578,1063,16,0,
351,1,682,1064,16,
0,351,1,144,1065,
16,0,351,1,33,
1066,17,1067,15,1068,
4,8,37,0,101,
0,120,0,112,0,
1,-1,1,5,1069,
20,794,1,129,1,
3,1,2,1,1,
1070,22,1,44,1,
461,1071,16,0,351,
1,129,1072,17,1073,
15,1074,4,10,37,
0,65,0,116,0,
111,0,109,0,1,
-1,1,5,1075,20,
802,1,125,1,3,
1,2,1,1,1076,
22,1,40,1,27,
1077,17,1078,15,1079,
4,22,37,0,80,
0,97,0,99,0,
107,0,97,0,103,
0,101,0,82,0,
101,0,102,0,1,
-1,1,5,1080,20,
607,1,142,1,3,
1,4,1,3,1081,
22,1,59,1,20,
1082,17,1016,1,1,
1019,1,19,1083,17,
1052,1,1,1055,1,
131,1084,17,1085,15,
1074,1,-1,1,5,
1086,20,806,1,123,
1,3,1,2,1,
1,1087,22,1,38,
1,130,1088,17,1089,
15,1074,1,-1,1,
5,1090,20,804,1,
124,1,3,1,2,
1,1,1091,22,1,
39,1,343,1092,17,
1093,15,1094,4,18,
37,0,84,0,97,
0,98,0,108,0,
101,0,82,0,101,
0,102,0,1,-1,
1,5,1095,20,649,
1,141,1,3,1,
5,1,4,1096,22,
1,58,1,128,1097,
17,1098,15,1074,1,
-1,1,5,1099,20,
800,1,126,1,3,
1,2,1,1,1100,
22,1,41,1,127,
1101,17,1102,15,1074,
1,-1,1,5,1103,
20,798,1,127,1,
3,1,2,1,1,
1104,22,1,42,1,
126,1105,17,1106,15,
1068,1,-1,1,5,
1107,20,796,1,128,
1,3,1,2,1,
1,1108,22,1,43,
1,125,1109,17,1110,
15,1111,4,24,37,
0,69,0,120,0,
112,0,84,0,97,
0,98,0,108,0,
101,0,68,0,101,
0,99,0,1,-1,
1,5,1112,20,792,
1,130,1,3,1,
2,1,1,1113,22,
1,45,1,17,1114,
17,1115,15,1043,1,
-1,1,5,1116,20,
603,1,144,1,3,
1,5,1,4,1117,
22,1,63,1,15,
1118,17,1119,15,1043,
1,-1,1,5,1120,
20,593,1,147,1,
3,1,4,1,3,
1121,22,1,66,1,
12,1122,17,1123,15,
1124,4,18,37,0,
102,0,117,0,110,
0,99,0,116,0,
105,0,111,0,110,
0,1,-1,1,5,
1125,20,567,1,148,
1,3,1,3,1,
2,1126,22,1,67,
1,10,1127,17,1128,
15,1129,4,8,37,
0,97,0,114,0,
103,0,1,-1,1,
5,212,1,2,1,
2,1130,22,1,68,
1,9,1131,17,1132,
15,1129,1,-1,1,
5,1133,20,565,1,
149,1,3,1,4,
1,3,1134,22,1,
69,1,327,1135,16,
0,351,1,3,1136,
17,1137,15,1129,1,
-1,1,5,1138,20,
563,1,150,1,3,
1,2,1,1,1139,
22,1,70,1,325,
1140,17,1141,15,1060,
1,-1,1,5,1142,
20,762,1,133,1,
3,1,3,1,2,
1143,22,1,48,1,
645,1144,16,0,351,
1,2,1145,17,1146,
15,1129,1,-1,1,
5,212,1,1,1,
1,1147,22,1,71,
1,107,1148,16,0,
351,1,51,1149,19,
273,1,51,1150,5,
43,1,209,1151,16,
0,271,1,634,1152,
16,0,271,1,97,
1153,16,0,271,1,
734,1154,16,0,271,
1,812,1155,16,0,
271,1,302,1156,16,
0,271,1,301,1157,
17,1158,15,1159,4,
18,37,0,102,0,
105,0,101,0,108,
0,100,0,115,0,
101,0,112,0,1,
-1,1,5,221,1,
1,1,1,1160,22,
1,90,1,300,1161,
17,1162,15,1159,1,
-1,1,5,221,1,
1,1,1,1163,22,
1,91,1,402,1164,
16,0,271,1,506,
1165,16,0,271,1,
69,1166,16,0,271,
1,374,1167,16,0,
271,1,50,1168,17,
1169,15,1170,4,12,
37,0,98,0,105,
0,110,0,111,0,
112,0,1,-1,1,
5,331,1,1,1,
1,1171,22,1,86,
1,170,1172,16,0,
271,1,62,1173,16,
0,271,1,61,1174,
17,1175,15,1170,1,
-1,1,5,331,1,
1,1,1,1176,22,
1,75,1,60,1177,
17,1178,15,1170,1,
-1,1,5,331,1,
1,1,1,1179,22,
1,76,1,59,1180,
17,1181,15,1170,1,
-1,1,5,331,1,
1,1,1,1182,22,
1,77,1,58,1183,
17,1184,15,1170,1,
-1,1,5,331,1,
1,1,1,1185,22,
1,78,1,57,1186,
17,1187,15,1170,1,
-1,1,5,331,1,
1,1,1,1188,22,
1,79,1,56,1189,
17,1190,15,1170,1,
-1,1,5,331,1,
1,1,1,1191,22,
1,80,1,55,1192,
17,1193,15,1170,1,
-1,1,5,331,1,
1,1,1,1194,22,
1,81,1,54,1195,
17,1196,15,1170,1,
-1,1,5,331,1,
1,1,1,1197,22,
1,82,1,53,1198,
17,1199,15,1170,1,
-1,1,5,331,1,
1,1,1,1200,22,
1,83,1,52,1201,
17,1202,15,1170,1,
-1,1,5,331,1,
1,1,1,1203,22,
1,84,1,51,1204,
17,1205,15,1170,1,
-1,1,5,331,1,
1,1,1,1206,22,
1,85,1,478,1207,
16,0,271,1,49,
1208,17,1209,15,1170,
1,-1,1,5,331,
1,1,1,1,1210,
22,1,87,1,48,
1211,17,1212,15,1170,
1,-1,1,5,331,
1,1,1,1,1213,
22,1,88,1,47,
1214,17,1215,15,1170,
1,-1,1,5,331,
1,1,1,1,1216,
22,1,89,1,45,
1217,16,0,271,1,
567,1218,16,0,271,
1,671,1219,16,0,
271,1,28,1220,16,
0,271,1,450,1221,
16,0,271,1,976,
1222,16,0,271,1,
546,1223,16,0,271,
1,866,1224,16,0,
271,1,7,1225,17,
1226,15,1227,4,10,
37,0,117,0,110,
0,111,0,112,0,
1,-1,1,5,304,
1,1,1,1,1228,
22,1,72,1,6,
1229,17,1230,15,1227,
1,-1,1,5,304,
1,1,1,1,1231,
22,1,73,1,5,
1232,17,1233,15,1227,
1,-1,1,5,304,
1,1,1,1,1234,
22,1,74,1,4,
1235,16,0,271,1,
206,1236,16,0,271,
1,50,1237,19,276,
1,50,1238,5,43,
1,209,1239,16,0,
274,1,634,1240,16,
0,274,1,97,1241,
16,0,274,1,734,
1242,16,0,274,1,
812,1243,16,0,274,
1,302,1244,16,0,
274,1,301,1157,1,
300,1161,1,402,1245,
16,0,274,1,506,
1246,16,0,274,1,
69,1247,16,0,274,
1,374,1248,16,0,
274,1,50,1168,1,
170,1249,16,0,274,
1,62,1250,16,0,
274,1,61,1174,1,
60,1177,1,59,1180,
1,58,1183,1,57,
1186,1,56,1189,1,
55,1192,1,54,1195,
1,53,1198,1,52,
1201,1,51,1204,1,
478,1251,16,0,274,
1,49,1208,1,48,
1211,1,47,1214,1,
45,1252,16,0,274,
1,567,1253,16,0,
274,1,671,1254,16,
0,274,1,28,1255,
16,0,274,1,450,
1256,16,0,274,1,
976,1257,16,0,274,
1,546,1258,16,0,
274,1,866,1259,16,
0,274,1,7,1225,
1,6,1229,1,5,
1232,1,4,1260,16,
0,274,1,206,1261,
16,0,274,1,49,
1262,19,193,1,49,
1263,5,71,1,534,
1264,16,0,191,1,
619,1265,17,1266,15,
1267,4,10,37,0,
115,0,116,0,97,
0,116,0,1,-1,
1,5,1268,20,904,
1,101,1,3,1,
8,1,7,1269,22,
1,14,1,421,1270,
17,1271,15,1272,4,
16,37,0,101,0,
120,0,112,0,108,
0,105,0,115,0,
116,0,1,-1,1,
5,1273,20,817,1,
121,1,3,1,4,
1,3,1274,22,1,
36,1,847,1275,16,
0,191,1,96,1015,
1,95,1020,1,949,
1276,17,1277,15,1267,
1,-1,1,5,1278,
20,896,1,105,1,
3,1,10,1,9,
1279,22,1,18,1,
92,1024,1,840,1280,
16,0,191,1,731,
1281,17,1282,15,1267,
1,-1,1,5,1283,
20,917,1,94,1,
3,1,4,1,3,
1284,22,1,8,1,
621,1285,16,0,191,
1,298,1029,1,939,
1286,16,0,191,1,
296,1034,1,373,1287,
17,1288,15,1267,1,
-1,1,5,1289,20,
885,1,111,1,3,
1,3,1,2,1290,
22,1,23,1,716,
1291,16,0,191,1,
715,1292,17,1293,15,
1267,1,-1,1,5,
1294,20,915,1,95,
1,3,1,6,1,
5,1295,22,1,9,
1,607,1296,16,0,
191,1,712,1297,17,
1298,15,1267,1,-1,
1,5,1299,20,913,
1,96,1,3,1,
5,1,4,1300,22,
1,10,1,1030,1041,
1,1028,1046,1,385,
1301,17,1302,15,1272,
1,-1,1,5,1303,
20,808,1,122,1,
3,1,2,1,1,
1304,22,1,37,1,
169,1051,1,595,1305,
16,0,191,1,699,
1306,16,0,191,1,
909,1307,17,1308,15,
1267,1,-1,1,5,
1309,20,909,1,98,
1,3,1,6,1,
5,1310,22,1,12,
1,908,1311,17,1312,
15,1267,1,-1,1,
5,1313,20,906,1,
100,1,3,1,8,
1,7,1314,22,1,
13,1,371,1315,17,
1316,15,1317,4,18,
37,0,110,0,97,
0,109,0,101,0,
108,0,105,0,115,
0,116,0,1,-1,
1,5,139,1,3,
1,3,1318,22,1,
54,1,369,1319,17,
1320,15,1317,1,-1,
1,5,139,1,1,
1,1,1321,22,1,
53,1,44,1058,1,
1006,1322,17,1323,15,
1267,1,-1,1,5,
1324,20,891,1,108,
1,3,1,8,1,
7,1325,22,1,20,
1,3,1136,1,126,
1105,1,144,1326,17,
1327,15,1328,4,12,
37,0,66,0,105,
0,110,0,111,0,
112,0,1,-1,1,
5,1329,20,790,1,
131,1,3,1,4,
1,3,1330,22,1,
46,1,33,1066,1,
19,1083,1,997,1331,
16,0,191,1,125,
1109,1,0,1332,16,
0,191,1,566,1333,
17,1334,15,1335,4,
14,37,0,82,0,
101,0,116,0,118,
0,97,0,108,0,
1,-1,1,5,1336,
20,900,1,103,1,
3,1,3,1,2,
1337,22,1,16,1,
127,1101,1,130,1088,
1,129,1072,1,27,
1077,1,20,1082,1,
1027,1338,16,0,191,
1,131,1084,1,772,
1339,16,0,191,1,
343,1092,1,128,1097,
1,448,1340,17,1341,
15,1317,1,-1,1,
5,139,1,1,1,
1,1321,1,432,1342,
17,1343,15,1344,4,
20,37,0,76,0,
111,0,99,0,97,
0,108,0,73,0,
110,0,105,0,116,
0,1,-1,1,5,
1345,20,867,1,112,
1,3,1,4,1,
3,1346,22,1,24,
1,446,1347,17,1348,
15,1349,4,18,37,
0,70,0,117,0,
110,0,99,0,68,
0,101,0,99,0,
108,0,1,-1,1,
5,1350,20,889,1,
109,1,3,1,4,
1,3,1351,22,1,
21,1,17,1114,1,
10,1127,1,15,1118,
1,14,1352,16,0,
191,1,9,1131,1,
12,1122,1,546,1353,
17,1354,15,1267,1,
-1,1,5,1355,20,
902,1,102,1,3,
1,2,1,1,1356,
22,1,15,1,545,
1357,17,1358,15,1267,
1,-1,1,5,1359,
20,898,1,104,1,
3,1,2,1,1,
1360,22,1,17,1,
544,1361,17,1362,15,
1267,1,-1,1,5,
1363,20,894,1,106,
1,3,1,12,1,
11,1364,22,1,19,
1,436,1365,17,1366,
15,1367,4,28,37,
0,76,0,111,0,
99,0,97,0,108,
0,70,0,117,0,
110,0,99,0,68,
0,101,0,99,0,
108,0,1,-1,1,
5,1368,20,887,1,
110,1,3,1,5,
1,4,1369,22,1,
22,1,756,1370,16,
0,191,1,2,1145,
1,754,1371,17,1372,
15,1373,4,22,37,
0,65,0,115,0,
115,0,105,0,103,
0,110,0,109,0,
101,0,110,0,116,
0,1,-1,1,5,
1374,20,921,1,92,
1,3,1,4,1,
3,1375,22,1,6,
1,325,1140,1,645,
1376,17,1377,15,1267,
1,-1,1,5,1378,
20,911,1,97,1,
3,1,5,1,4,
1379,22,1,11,1,
430,1380,17,1381,15,
1382,4,10,37,0,
105,0,110,0,105,
0,116,0,1,-1,
1,5,1383,20,819,
1,120,1,3,1,
3,1,2,1384,22,
1,35,1,1,1385,
17,1386,15,1267,1,
-1,1,5,1387,20,
919,1,93,1,3,
1,2,1,1,1388,
22,1,7,1,107,
1389,17,1390,15,1391,
4,10,37,0,85,
0,110,0,111,0,
112,0,1,-1,1,
5,1392,20,764,1,
132,1,3,1,3,
1,2,1393,22,1,
47,1,48,1394,19,
397,1,48,1395,5,
71,1,534,1396,16,
0,395,1,619,1265,
1,421,1270,1,847,
1397,16,0,395,1,
96,1015,1,95,1020,
1,949,1276,1,92,
1024,1,840,1398,16,
0,395,1,731,1281,
1,621,1399,16,0,
395,1,298,1029,1,
939,1400,16,0,395,
1,296,1034,1,373,
1287,1,716,1401,16,
0,395,1,715,1292,
1,607,1402,16,0,
395,1,712,1297,1,
1030,1041,1,1028,1046,
1,385,1301,1,169,
1051,1,595,1403,16,
0,395,1,699,1404,
16,0,395,1,909,
1307,1,908,1311,1,
371,1315,1,369,1319,
1,44,1058,1,1006,
1322,1,3,1136,1,
126,1105,1,144,1326,
1,33,1066,1,19,
1083,1,997,1405,16,
0,395,1,125,1109,
1,0,1406,16,0,
395,1,566,1333,1,
127,1101,1,130,1088,
1,129,1072,1,27,
1077,1,20,1082,1,
1027,1407,16,0,395,
1,131,1084,1,772,
1408,16,0,395,1,
343,1092,1,128,1097,
1,448,1340,1,432,
1342,1,446,1347,1,
17,1114,1,10,1127,
1,15,1118,1,14,
1409,16,0,395,1,
9,1131,1,12,1122,
1,546,1353,1,545,
1357,1,544,1361,1,
436,1365,1,756,1410,
16,0,395,1,2,
1145,1,754,1371,1,
325,1140,1,645,1376,
1,430,1380,1,1,
1385,1,107,1389,1,
47,1411,19,293,1,
47,1412,5,62,1,
619,1265,1,421,1270,
1,633,1413,16,0,
291,1,96,1015,1,
95,1020,1,949,1276,
1,92,1024,1,731,
1281,1,298,1029,1,
296,1034,1,373,1287,
1,715,1292,1,712,
1297,1,1030,1041,1,
1028,1046,1,385,1301,
1,169,1051,1,909,
1307,1,908,1311,1,
371,1315,1,772,1414,
17,1415,15,1416,4,
12,37,0,99,0,
104,0,117,0,110,
0,107,0,1,-1,
1,5,1417,20,958,
1,88,1,3,1,
3,1,2,1418,22,
1,2,1,369,1319,
1,44,1058,1,1006,
1322,1,791,1419,17,
1420,15,1416,1,-1,
1,5,1421,20,956,
1,89,1,3,1,
3,1,2,1422,22,
1,3,1,3,1136,
1,788,1423,17,1424,
15,1416,1,-1,1,
5,1425,20,938,1,
90,1,3,1,4,
1,3,1426,22,1,
4,1,144,1326,1,
33,1066,1,19,1083,
1,125,1109,1,566,
1333,1,126,1105,1,
129,1072,1,27,1077,
1,20,1082,1,127,
1101,1,131,1084,1,
130,1088,1,343,1092,
1,128,1097,1,448,
1340,1,432,1342,1,
446,1347,1,17,1114,
1,2,1145,1,15,
1118,1,10,1127,1,
9,1131,1,12,1122,
1,546,1353,1,545,
1357,1,544,1361,1,
436,1365,1,756,1427,
17,1428,15,1416,1,
-1,1,5,1429,20,
960,1,87,1,3,
1,2,1,1,1430,
22,1,1,1,755,
1431,17,1432,15,1433,
4,12,37,0,98,
0,108,0,111,0,
99,0,107,0,1,
-1,1,5,1434,20,
923,1,91,1,3,
1,2,1,1,1435,
22,1,5,1,754,
1371,1,325,1140,1,
645,1376,1,430,1380,
1,1,1385,1,107,
1389,1,46,1436,19,
301,1,46,1437,5,
71,1,534,1438,16,
0,299,1,619,1265,
1,421,1270,1,847,
1439,16,0,299,1,
96,1015,1,95,1020,
1,949,1276,1,92,
1024,1,840,1440,16,
0,299,1,731,1281,
1,621,1441,16,0,
299,1,298,1029,1,
939,1442,16,0,299,
1,296,1034,1,373,
1287,1,716,1443,16,
0,299,1,715,1292,
1,607,1444,16,0,
299,1,712,1297,1,
1030,1041,1,1028,1046,
1,385,1301,1,169,
1051,1,595,1445,16,
0,299,1,699,1446,
16,0,299,1,909,
1307,1,908,1311,1,
371,1315,1,369,1319,
1,44,1058,1,1006,
1322,1,3,1136,1,
126,1105,1,144,1326,
1,33,1066,1,19,
1083,1,997,1447,16,
0,299,1,125,1109,
1,0,1448,16,0,
299,1,566,1333,1,
127,1101,1,130,1088,
1,129,1072,1,27,
1077,1,20,1082,1,
1027,1449,16,0,299,
1,131,1084,1,772,
1450,16,0,299,1,
343,1092,1,128,1097,
1,448,1340,1,432,
1342,1,446,1347,1,
17,1114,1,10,1127,
1,15,1118,1,14,
1451,16,0,299,1,
9,1131,1,12,1122,
1,546,1353,1,545,
1357,1,544,1361,1,
436,1365,1,756,1452,
16,0,299,1,2,
1145,1,754,1371,1,
325,1140,1,645,1376,
1,430,1380,1,1,
1385,1,107,1389,1,
45,1453,19,167,1,
45,1454,5,114,1,
716,1455,16,0,165,
1,715,1292,1,712,
1297,1,949,1276,1,
939,1456,16,0,165,
1,908,1311,1,209,
1457,16,0,412,1,
671,1458,16,0,412,
1,450,1459,16,0,
412,1,448,1340,1,
446,1347,1,206,1460,
16,0,412,1,436,
1365,1,432,1342,1,
909,1307,1,430,1380,
1,607,1461,16,0,
165,1,421,1270,1,
634,1462,16,0,412,
1,170,1463,16,0,
412,1,169,1051,1,
645,1376,1,402,1464,
16,0,412,1,847,
1465,16,0,165,1,
369,1319,1,866,1466,
16,0,412,1,385,
1301,1,144,1326,1,
621,1467,16,0,165,
1,368,1468,16,0,
170,1,619,1265,1,
374,1469,16,0,412,
1,373,1287,1,371,
1315,1,131,1084,1,
130,1088,1,129,1072,
1,128,1097,1,127,
1101,1,126,1105,1,
125,1109,1,840,1470,
16,0,165,1,595,
1471,16,0,165,1,
534,1472,16,0,165,
1,107,1389,1,343,
1092,1,95,1020,1,
97,1473,16,0,412,
1,96,1015,1,812,
1474,16,0,412,1,
92,1024,1,567,1475,
16,0,412,1,566,
1333,1,325,1140,1,
544,1361,1,300,1161,
1,1030,1041,1,301,
1157,1,1028,1046,1,
1027,1476,16,0,165,
1,298,1029,1,69,
1477,16,0,412,1,
546,1478,16,0,412,
1,545,1357,1,57,
1186,1,50,1168,1,
55,1192,1,302,1479,
16,0,412,1,62,
1480,16,0,412,1,
61,1174,1,60,1177,
1,59,1180,1,58,
1183,1,296,1034,1,
56,1189,1,772,1481,
16,0,165,1,54,
1195,1,53,1198,1,
52,1201,1,51,1204,
1,1006,1322,1,49,
1208,1,48,1211,1,
47,1214,1,45,1482,
16,0,412,1,44,
1058,1,997,1483,16,
0,165,1,756,1484,
16,0,165,1,754,
1371,1,33,1066,1,
28,1485,16,0,412,
1,506,1486,16,0,
412,1,27,1077,1,
20,1082,1,17,1114,
1,976,1487,16,0,
412,1,19,1083,1,
734,1488,16,0,412,
1,14,1489,16,0,
165,1,15,1118,1,
731,1281,1,12,1122,
1,5,1232,1,10,
1127,1,9,1131,1,
0,1490,16,0,165,
1,7,1225,1,6,
1229,1,699,1491,16,
0,165,1,4,1492,
16,0,412,1,3,
1136,1,2,1145,1,
1,1385,1,478,1493,
16,0,412,1,44,
1494,19,270,1,44,
1495,5,43,1,209,
1496,16,0,268,1,
634,1497,16,0,268,
1,97,1498,16,0,
268,1,734,1499,16,
0,268,1,812,1500,
16,0,268,1,302,
1501,16,0,268,1,
301,1157,1,300,1161,
1,402,1502,16,0,
268,1,506,1503,16,
0,268,1,69,1504,
16,0,268,1,374,
1505,16,0,268,1,
50,1168,1,170,1506,
16,0,268,1,62,
1507,16,0,268,1,
61,1174,1,60,1177,
1,59,1180,1,58,
1183,1,57,1186,1,
56,1189,1,55,1192,
1,54,1195,1,53,
1198,1,52,1201,1,
51,1204,1,478,1508,
16,0,268,1,49,
1208,1,48,1211,1,
47,1214,1,45,1509,
16,0,268,1,567,
1510,16,0,268,1,
671,1511,16,0,268,
1,28,1512,16,0,
268,1,450,1513,16,
0,268,1,976,1514,
16,0,268,1,546,
1515,16,0,268,1,
866,1516,16,0,268,
1,7,1225,1,6,
1229,1,5,1232,1,
4,1517,16,0,268,
1,206,1518,16,0,
268,1,43,1519,19,
400,1,43,1520,5,
71,1,534,1521,16,
0,398,1,619,1265,
1,421,1270,1,847,
1522,16,0,398,1,
96,1015,1,95,1020,
1,949,1276,1,92,
1024,1,840,1523,16,
0,398,1,731,1281,
1,621,1524,16,0,
398,1,298,1029,1,
939,1525,16,0,398,
1,296,1034,1,373,
1287,1,716,1526,16,
0,398,1,715,1292,
1,607,1527,16,0,
398,1,712,1297,1,
1030,1041,1,1028,1046,
1,385,1301,1,169,
1051,1,595,1528,16,
0,398,1,699,1529,
16,0,398,1,909,
1307,1,908,1311,1,
371,1315,1,369,1319,
1,44,1058,1,1006,
1322,1,3,1136,1,
126,1105,1,144,1326,
1,33,1066,1,19,
1083,1,997,1530,16,
0,398,1,125,1109,
1,0,1531,16,0,
398,1,566,1333,1,
127,1101,1,130,1088,
1,129,1072,1,27,
1077,1,20,1082,1,
1027,1532,16,0,398,
1,131,1084,1,772,
1533,16,0,398,1,
343,1092,1,128,1097,
1,448,1340,1,432,
1342,1,446,1347,1,
17,1114,1,10,1127,
1,15,1118,1,14,
1534,16,0,398,1,
9,1131,1,12,1122,
1,546,1353,1,545,
1357,1,544,1361,1,
436,1365,1,756,1535,
16,0,398,1,2,
1145,1,754,1371,1,
325,1140,1,645,1376,
1,430,1380,1,1,
1385,1,107,1389,1,
42,1536,19,137,1,
42,1537,5,4,1,
448,1340,1,975,1538,
16,0,135,1,369,
1319,1,371,1315,1,
41,1539,19,130,1,
41,1540,5,75,1,
534,1541,16,0,255,
1,619,1265,1,421,
1270,1,847,1542,16,
0,255,1,96,1015,
1,95,1020,1,949,
1276,1,92,1024,1,
840,1543,16,0,255,
1,517,1544,16,0,
391,1,621,1545,16,
0,255,1,298,1029,
1,939,1546,16,0,
255,1,296,1034,1,
373,1287,1,716,1547,
16,0,255,1,715,
1292,1,607,1548,16,
0,255,1,712,1297,
1,1030,1041,1,1028,
1046,1,385,1301,1,
169,1051,1,489,1549,
16,0,178,1,595,
1550,16,0,255,1,
699,1551,16,0,255,
1,909,1307,1,908,
1311,1,371,1315,1,
369,1319,1,44,1058,
1,1006,1322,1,0,
1552,16,0,255,1,
3,1136,1,682,1553,
16,0,259,1,126,
1105,1,125,1109,1,
144,1326,1,33,1066,
1,19,1083,1,997,
1554,16,0,255,1,
996,1555,16,0,128,
1,2,1145,1,566,
1333,1,127,1101,1,
130,1088,1,129,1072,
1,27,1077,1,20,
1082,1,1027,1556,16,
0,255,1,131,1084,
1,772,1557,16,0,
255,1,343,1092,1,
128,1097,1,448,1340,
1,432,1342,1,446,
1347,1,17,1114,1,
10,1127,1,15,1118,
1,14,1558,16,0,
255,1,9,1131,1,
12,1122,1,546,1353,
1,545,1357,1,544,
1361,1,436,1365,1,
756,1559,16,0,255,
1,731,1281,1,754,
1371,1,325,1140,1,
645,1376,1,430,1380,
1,1,1385,1,107,
1389,1,40,1560,19,
149,1,40,1561,5,
71,1,534,1562,16,
0,147,1,619,1265,
1,421,1270,1,847,
1563,16,0,147,1,
96,1015,1,95,1020,
1,949,1276,1,92,
1024,1,840,1564,16,
0,147,1,731,1281,
1,621,1565,16,0,
147,1,298,1029,1,
939,1566,16,0,147,
1,296,1034,1,373,
1287,1,716,1567,16,
0,147,1,715,1292,
1,607,1568,16,0,
147,1,712,1297,1,
1030,1041,1,1028,1046,
1,385,1301,1,169,
1051,1,595,1569,16,
0,147,1,699,1570,
16,0,147,1,909,
1307,1,908,1311,1,
371,1315,1,369,1319,
1,44,1058,1,1006,
1322,1,3,1136,1,
126,1105,1,144,1326,
1,33,1066,1,19,
1083,1,997,1571,16,
0,147,1,125,1109,
1,0,1572,16,0,
147,1,566,1333,1,
127,1101,1,130,1088,
1,129,1072,1,27,
1077,1,20,1082,1,
1027,1573,16,0,147,
1,131,1084,1,772,
1574,16,0,147,1,
343,1092,1,128,1097,
1,448,1340,1,432,
1342,1,446,1347,1,
17,1114,1,10,1127,
1,15,1118,1,14,
1575,16,0,147,1,
9,1131,1,12,1122,
1,546,1353,1,545,
1357,1,544,1361,1,
436,1365,1,756,1576,
16,0,147,1,2,
1145,1,754,1371,1,
325,1140,1,645,1376,
1,430,1380,1,1,
1385,1,107,1389,1,
39,1577,19,266,1,
39,1578,5,71,1,
534,1579,16,0,264,
1,619,1265,1,421,
1270,1,847,1580,16,
0,264,1,96,1015,
1,95,1020,1,949,
1276,1,92,1024,1,
840,1581,16,0,264,
1,731,1281,1,621,
1582,16,0,264,1,
298,1029,1,939,1583,
16,0,264,1,296,
1034,1,373,1287,1,
716,1584,16,0,264,
1,715,1292,1,607,
1585,16,0,264,1,
712,1297,1,1030,1041,
1,1028,1046,1,385,
1301,1,169,1051,1,
595,1586,16,0,264,
1,699,1587,16,0,
264,1,909,1307,1,
908,1311,1,371,1315,
1,369,1319,1,44,
1058,1,1006,1322,1,
3,1136,1,126,1105,
1,144,1326,1,33,
1066,1,19,1083,1,
997,1588,16,0,264,
1,125,1109,1,0,
1589,16,0,264,1,
566,1333,1,127,1101,
1,130,1088,1,129,
1072,1,27,1077,1,
20,1082,1,1027,1590,
16,0,264,1,131,
1084,1,772,1591,16,
0,264,1,343,1092,
1,128,1097,1,448,
1340,1,432,1342,1,
446,1347,1,17,1114,
1,10,1127,1,15,
1118,1,14,1592,16,
0,264,1,9,1131,
1,12,1122,1,546,
1353,1,545,1357,1,
544,1361,1,436,1365,
1,756,1593,16,0,
264,1,2,1145,1,
754,1371,1,325,1140,
1,645,1376,1,430,
1380,1,1,1385,1,
107,1389,1,38,1594,
19,111,1,38,1595,
5,77,1,619,1265,
1,853,1596,17,1597,
15,1598,4,14,37,
0,101,0,108,0,
115,0,101,0,105,
0,102,0,1,-1,
1,5,183,1,5,
1,5,1599,22,1,
26,1,421,1270,1,
1027,1600,16,0,115,
1,846,1601,17,1602,
15,1598,1,-1,1,
5,183,1,3,1,
3,1603,22,1,27,
1,96,1015,1,95,
1020,1,949,1276,1,
948,1604,16,0,175,
1,731,1281,1,730,
1605,16,0,248,1,
298,1029,1,618,1606,
16,0,302,1,296,
1034,1,373,1287,1,
715,1292,1,714,1607,
16,0,256,1,606,
1608,16,0,180,1,
712,1297,1,791,1419,
1,1030,1041,1,1029,
1609,16,0,109,1,
1028,1046,1,385,1301,
1,169,1051,1,699,
1610,16,0,258,1,
909,1307,1,908,1311,
1,907,1611,16,0,
181,1,371,1315,1,
369,1319,1,1,1385,
1,44,1058,1,1006,
1322,1,1005,1612,16,
0,123,1,3,1136,
1,2,1145,1,788,
1423,1,10,1127,1,
144,1326,1,33,1066,
1,19,1083,1,126,
1105,1,125,1109,1,
20,1082,1,566,1333,
1,886,1613,17,1614,
15,1598,1,-1,1,
5,183,1,5,1,
5,1615,22,1,25,
1,130,1088,1,129,
1072,1,27,1077,1,
543,1616,16,0,401,
1,127,1101,1,131,
1084,1,772,1414,1,
343,1092,1,128,1097,
1,448,1340,1,432,
1342,1,446,1347,1,
17,1114,1,16,1617,
16,0,406,1,15,
1118,1,14,1618,16,
0,408,1,9,1131,
1,12,1122,1,546,
1353,1,545,1357,1,
544,1361,1,436,1365,
1,756,1427,1,755,
1431,1,754,1371,1,
325,1140,1,645,1376,
1,430,1380,1,92,
1024,1,107,1389,1,
37,1619,19,201,1,
37,1620,5,63,1,
619,1265,1,421,1270,
1,846,1621,16,0,
199,1,96,1015,1,
95,1020,1,949,1276,
1,92,1024,1,731,
1281,1,298,1029,1,
296,1034,1,373,1287,
1,715,1292,1,606,
1622,16,0,235,1,
712,1297,1,1030,1041,
1,1028,1046,1,385,
1301,1,169,1051,1,
909,1307,1,908,1311,
1,371,1315,1,369,
1319,1,44,1058,1,
1006,1322,1,791,1419,
1,3,1136,1,788,
1423,1,144,1326,1,
33,1066,1,19,1083,
1,126,1105,1,125,
1109,1,566,1333,1,
130,1088,1,129,1072,
1,27,1077,1,20,
1082,1,127,1101,1,
131,1084,1,772,1414,
1,343,1092,1,128,
1097,1,448,1340,1,
432,1342,1,446,1347,
1,17,1114,1,2,
1145,1,15,1118,1,
10,1127,1,9,1131,
1,12,1122,1,546,
1353,1,545,1357,1,
544,1361,1,436,1365,
1,756,1427,1,755,
1431,1,754,1371,1,
325,1140,1,645,1376,
1,430,1380,1,1,
1385,1,107,1389,1,
36,1623,19,210,1,
36,1624,5,63,1,
619,1265,1,421,1270,
1,846,1625,16,0,
208,1,96,1015,1,
95,1020,1,949,1276,
1,92,1024,1,731,
1281,1,298,1029,1,
296,1034,1,373,1287,
1,715,1292,1,606,
1626,16,0,313,1,
712,1297,1,1030,1041,
1,1028,1046,1,385,
1301,1,169,1051,1,
909,1307,1,908,1311,
1,371,1315,1,369,
1319,1,44,1058,1,
1006,1322,1,791,1419,
1,3,1136,1,788,
1423,1,144,1326,1,
33,1066,1,19,1083,
1,126,1105,1,125,
1109,1,566,1333,1,
130,1088,1,129,1072,
1,27,1077,1,20,
1082,1,127,1101,1,
131,1084,1,772,1414,
1,343,1092,1,128,
1097,1,448,1340,1,
432,1342,1,446,1347,
1,17,1114,1,2,
1145,1,15,1118,1,
10,1127,1,9,1131,
1,12,1122,1,546,
1353,1,545,1357,1,
544,1361,1,436,1365,
1,756,1427,1,755,
1431,1,754,1371,1,
325,1140,1,645,1376,
1,430,1380,1,1,
1385,1,107,1389,1,
35,1627,19,219,1,
35,1628,5,33,1,
92,1024,1,44,1058,
1,325,1140,1,1028,
1046,1,33,1066,1,
131,1084,1,130,1088,
1,129,1072,1,128,
1097,1,127,1101,1,
126,1105,1,125,1109,
1,1030,1041,1,169,
1051,1,27,1077,1,
343,1092,1,823,1629,
16,0,217,1,20,
1082,1,19,1083,1,
17,1114,1,298,1029,
1,15,1118,1,296,
1034,1,107,1389,1,
12,1122,1,10,1127,
1,9,1131,1,578,
1630,16,0,322,1,
3,1136,1,2,1145,
1,144,1326,1,96,
1015,1,95,1020,1,
34,1631,19,328,1,
34,1632,5,71,1,
534,1633,16,0,326,
1,619,1265,1,421,
1270,1,847,1634,16,
0,326,1,96,1015,
1,95,1020,1,949,
1276,1,92,1024,1,
840,1635,16,0,326,
1,731,1281,1,621,
1636,16,0,326,1,
298,1029,1,939,1637,
16,0,326,1,296,
1034,1,373,1287,1,
716,1638,16,0,326,
1,715,1292,1,607,
1639,16,0,326,1,
712,1297,1,1030,1041,
1,1028,1046,1,385,
1301,1,169,1051,1,
595,1640,16,0,326,
1,699,1641,16,0,
326,1,909,1307,1,
908,1311,1,371,1315,
1,369,1319,1,44,
1058,1,1006,1322,1,
3,1136,1,126,1105,
1,144,1326,1,33,
1066,1,19,1083,1,
997,1642,16,0,326,
1,125,1109,1,0,
1643,16,0,326,1,
566,1333,1,127,1101,
1,130,1088,1,129,
1072,1,27,1077,1,
20,1082,1,1027,1644,
16,0,326,1,131,
1084,1,772,1645,16,
0,326,1,343,1092,
1,128,1097,1,448,
1340,1,432,1342,1,
446,1347,1,17,1114,
1,10,1127,1,15,
1118,1,14,1646,16,
0,326,1,9,1131,
1,12,1122,1,546,
1353,1,545,1357,1,
544,1361,1,436,1365,
1,756,1647,16,0,
326,1,2,1145,1,
754,1371,1,325,1140,
1,645,1376,1,430,
1380,1,1,1385,1,
107,1389,1,33,1648,
19,143,1,33,1649,
5,12,1,20,1650,
17,1651,15,1652,4,
16,37,0,118,0,
97,0,114,0,108,
0,105,0,115,0,
116,0,1,-1,1,
5,1653,20,653,1,
139,1,3,1,2,
1,1,1654,22,1,
56,1,343,1092,1,
733,1655,16,0,246,
1,19,1083,1,363,
1656,17,1657,15,1652,
1,-1,1,5,1658,
20,672,1,138,1,
3,1,4,1,3,
1659,22,1,55,1,
448,1660,16,0,141,
1,208,1661,16,0,
245,1,169,1662,16,
0,261,1,373,1663,
16,0,186,1,27,
1077,1,371,1315,1,
369,1319,1,32,1664,
19,419,1,32,1665,
5,43,1,209,1666,
16,0,417,1,634,
1667,16,0,417,1,
97,1668,16,0,417,
1,734,1669,16,0,
417,1,812,1670,16,
0,417,1,302,1671,
16,0,417,1,301,
1157,1,300,1161,1,
402,1672,16,0,417,
1,506,1673,16,0,
417,1,69,1674,16,
0,417,1,374,1675,
16,0,417,1,50,
1168,1,170,1676,16,
0,417,1,62,1677,
16,0,417,1,61,
1174,1,60,1177,1,
59,1180,1,58,1183,
1,57,1186,1,56,
1189,1,55,1192,1,
54,1195,1,53,1198,
1,52,1201,1,51,
1204,1,478,1678,16,
0,417,1,49,1208,
1,48,1211,1,47,
1214,1,45,1679,16,
0,417,1,567,1680,
16,0,417,1,671,
1681,16,0,417,1,
28,1682,16,0,417,
1,450,1683,16,0,
417,1,976,1684,16,
0,417,1,546,1685,
16,0,417,1,866,
1686,16,0,417,1,
7,1225,1,6,1229,
1,5,1232,1,4,
1687,16,0,417,1,
206,1688,16,0,417,
1,31,1689,19,374,
1,31,1690,5,45,
1,210,1691,16,0,
372,1,207,1692,16,
0,372,1,96,1015,
1,95,1020,1,92,
1024,1,517,1693,16,
0,372,1,298,1029,
1,296,1034,1,76,
1694,16,0,372,1,
823,1695,16,0,372,
1,171,1696,16,0,
372,1,1030,1041,1,
1028,1046,1,385,1697,
16,0,372,1,169,
1051,1,489,1698,16,
0,372,1,46,1699,
16,0,372,1,44,
1058,1,578,1700,16,
0,372,1,682,1701,
16,0,372,1,144,
1702,16,0,372,1,
33,1066,1,461,1703,
16,0,372,1,129,
1072,1,27,1077,1,
20,1082,1,19,1083,
1,131,1084,1,130,
1088,1,343,1092,1,
128,1097,1,127,1101,
1,126,1105,1,125,
1109,1,17,1114,1,
15,1118,1,12,1122,
1,10,1127,1,9,
1131,1,327,1704,16,
0,372,1,3,1136,
1,325,1140,1,645,
1705,16,0,372,1,
2,1145,1,107,1706,
16,0,372,1,30,
1707,19,371,1,30,
1708,5,45,1,210,
1709,16,0,369,1,
207,1710,16,0,369,
1,96,1015,1,95,
1020,1,92,1024,1,
517,1711,16,0,369,
1,298,1029,1,296,
1034,1,76,1712,16,
0,369,1,823,1713,
16,0,369,1,171,
1714,16,0,369,1,
1030,1041,1,1028,1046,
1,385,1715,16,0,
369,1,169,1051,1,
489,1716,16,0,369,
1,46,1717,16,0,
369,1,44,1058,1,
578,1718,16,0,369,
1,682,1719,16,0,
369,1,144,1720,16,
0,369,1,33,1066,
1,461,1721,16,0,
369,1,129,1072,1,
27,1077,1,20,1082,
1,19,1083,1,131,
1084,1,130,1088,1,
343,1092,1,128,1097,
1,127,1101,1,126,
1105,1,125,1109,1,
17,1114,1,15,1118,
1,12,1122,1,10,
1127,1,9,1131,1,
327,1722,16,0,369,
1,3,1136,1,325,
1140,1,645,1723,16,
0,369,1,2,1145,
1,107,1724,16,0,
369,1,29,1725,19,
362,1,29,1726,5,
45,1,210,1727,16,
0,360,1,207,1728,
16,0,360,1,96,
1015,1,95,1020,1,
92,1024,1,517,1729,
16,0,360,1,298,
1029,1,296,1034,1,
76,1730,16,0,360,
1,823,1731,16,0,
360,1,171,1732,16,
0,360,1,1030,1041,
1,1028,1046,1,385,
1733,16,0,360,1,
169,1051,1,489,1734,
16,0,360,1,46,
1735,16,0,360,1,
44,1058,1,578,1736,
16,0,360,1,682,
1737,16,0,360,1,
144,1738,16,0,360,
1,33,1066,1,461,
1739,16,0,360,1,
129,1072,1,27,1077,
1,20,1082,1,19,
1083,1,131,1084,1,
130,1088,1,343,1092,
1,128,1097,1,127,
1101,1,126,1105,1,
125,1109,1,17,1114,
1,15,1118,1,12,
1122,1,10,1127,1,
9,1131,1,327,1740,
16,0,360,1,3,
1136,1,325,1140,1,
645,1741,16,0,360,
1,2,1145,1,107,
1742,16,0,360,1,
28,1743,19,359,1,
28,1744,5,45,1,
210,1745,16,0,357,
1,207,1746,16,0,
357,1,96,1015,1,
95,1020,1,92,1024,
1,517,1747,16,0,
357,1,298,1029,1,
296,1034,1,76,1748,
16,0,357,1,823,
1749,16,0,357,1,
171,1750,16,0,357,
1,1030,1041,1,1028,
1046,1,385,1751,16,
0,357,1,169,1051,
1,489,1752,16,0,
357,1,46,1753,16,
0,357,1,44,1058,
1,578,1754,16,0,
357,1,682,1755,16,
0,357,1,144,1756,
16,0,357,1,33,
1066,1,461,1757,16,
0,357,1,129,1072,
1,27,1077,1,20,
1082,1,19,1083,1,
131,1084,1,130,1088,
1,343,1092,1,128,
1097,1,127,1101,1,
126,1105,1,125,1109,
1,17,1114,1,15,
1118,1,12,1122,1,
10,1127,1,9,1131,
1,327,1758,16,0,
357,1,3,1136,1,
325,1140,1,645,1759,
16,0,357,1,2,
1145,1,107,1760,16,
0,357,1,27,1761,
19,365,1,27,1762,
5,45,1,210,1763,
16,0,363,1,207,
1764,16,0,363,1,
96,1015,1,95,1020,
1,92,1024,1,517,
1765,16,0,363,1,
298,1029,1,296,1034,
1,76,1766,16,0,
363,1,823,1767,16,
0,363,1,171,1768,
16,0,363,1,1030,
1041,1,1028,1046,1,
385,1769,16,0,363,
1,169,1051,1,489,
1770,16,0,363,1,
46,1771,16,0,363,
1,44,1058,1,578,
1772,16,0,363,1,
682,1773,16,0,363,
1,144,1774,16,0,
363,1,33,1066,1,
461,1775,16,0,363,
1,129,1072,1,27,
1077,1,20,1082,1,
19,1083,1,131,1084,
1,130,1088,1,343,
1092,1,128,1097,1,
127,1101,1,126,1105,
1,125,1109,1,17,
1114,1,15,1118,1,
12,1122,1,10,1127,
1,9,1131,1,327,
1776,16,0,363,1,
3,1136,1,325,1140,
1,645,1777,16,0,
363,1,2,1145,1,
107,1778,16,0,363,
1,26,1779,19,356,
1,26,1780,5,45,
1,210,1781,16,0,
354,1,207,1782,16,
0,354,1,96,1015,
1,95,1020,1,92,
1024,1,517,1783,16,
0,354,1,298,1029,
1,296,1034,1,76,
1784,16,0,354,1,
823,1785,16,0,354,
1,171,1786,16,0,
354,1,1030,1041,1,
1028,1046,1,385,1787,
16,0,354,1,169,
1051,1,489,1788,16,
0,354,1,46,1789,
16,0,354,1,44,
1058,1,578,1790,16,
0,354,1,682,1791,
16,0,354,1,144,
1792,16,0,354,1,
33,1066,1,461,1793,
16,0,354,1,129,
1072,1,27,1077,1,
20,1082,1,19,1083,
1,131,1084,1,130,
1088,1,343,1092,1,
128,1097,1,127,1101,
1,126,1105,1,125,
1109,1,17,1114,1,
15,1118,1,12,1122,
1,10,1127,1,9,
1131,1,327,1794,16,
0,354,1,3,1136,
1,325,1140,1,645,
1795,16,0,354,1,
2,1145,1,107,1796,
16,0,354,1,25,
1797,19,377,1,25,
1798,5,45,1,210,
1799,16,0,375,1,
207,1800,16,0,375,
1,96,1015,1,95,
1020,1,92,1024,1,
517,1801,16,0,375,
1,298,1029,1,296,
1034,1,76,1802,16,
0,375,1,823,1803,
16,0,375,1,171,
1804,16,0,375,1,
1030,1041,1,1028,1046,
1,385,1805,16,0,
375,1,169,1051,1,
489,1806,16,0,375,
1,46,1807,16,0,
375,1,44,1058,1,
578,1808,16,0,375,
1,682,1809,16,0,
375,1,144,1810,16,
0,375,1,33,1066,
1,461,1811,16,0,
375,1,129,1072,1,
27,1077,1,20,1082,
1,19,1083,1,131,
1084,1,130,1088,1,
343,1092,1,128,1097,
1,127,1101,1,126,
1105,1,125,1109,1,
17,1114,1,15,1118,
1,12,1122,1,10,
1127,1,9,1131,1,
327,1812,16,0,375,
1,3,1136,1,325,
1140,1,645,1813,16,
0,375,1,2,1145,
1,107,1814,16,0,
375,1,24,1815,19,
368,1,24,1816,5,
45,1,210,1817,16,
0,366,1,207,1818,
16,0,366,1,96,
1015,1,95,1020,1,
92,1024,1,517,1819,
16,0,366,1,298,
1029,1,296,1034,1,
76,1820,16,0,366,
1,823,1821,16,0,
366,1,171,1822,16,
0,366,1,1030,1041,
1,1028,1046,1,385,
1823,16,0,366,1,
169,1051,1,489,1824,
16,0,366,1,46,
1825,16,0,366,1,
44,1058,1,578,1826,
16,0,366,1,682,
1827,16,0,366,1,
144,1828,16,0,366,
1,33,1066,1,461,
1829,16,0,366,1,
129,1072,1,27,1077,
1,20,1082,1,19,
1083,1,131,1084,1,
130,1088,1,343,1092,
1,128,1097,1,127,
1101,1,126,1105,1,
125,1109,1,17,1114,
1,15,1118,1,12,
1122,1,10,1127,1,
9,1131,1,327,1830,
16,0,366,1,3,
1136,1,325,1140,1,
645,1831,16,0,366,
1,2,1145,1,107,
1832,16,0,366,1,
23,1833,19,350,1,
23,1834,5,45,1,
210,1835,16,0,348,
1,207,1836,16,0,
348,1,96,1015,1,
95,1020,1,92,1024,
1,517,1837,16,0,
348,1,298,1029,1,
296,1034,1,76,1838,
16,0,348,1,823,
1839,16,0,348,1,
171,1840,16,0,348,
1,1030,1041,1,1028,
1046,1,385,1841,16,
0,348,1,169,1051,
1,489,1842,16,0,
348,1,46,1843,16,
0,348,1,44,1058,
1,578,1844,16,0,
348,1,682,1845,16,
0,348,1,144,1846,
16,0,348,1,33,
1066,1,461,1847,16,
0,348,1,129,1072,
1,27,1077,1,20,
1082,1,19,1083,1,
131,1084,1,130,1088,
1,343,1092,1,128,
1097,1,127,1101,1,
126,1105,1,125,1109,
1,17,1114,1,15,
1118,1,12,1122,1,
10,1127,1,9,1131,
1,327,1848,16,0,
348,1,3,1136,1,
325,1140,1,645,1849,
16,0,348,1,2,
1145,1,107,1850,16,
0,348,1,22,1851,
19,347,1,22,1852,
5,45,1,210,1853,
16,0,345,1,207,
1854,16,0,345,1,
96,1015,1,95,1020,
1,92,1024,1,517,
1855,16,0,345,1,
298,1029,1,296,1034,
1,76,1856,16,0,
345,1,823,1857,16,
0,345,1,171,1858,
16,0,345,1,1030,
1041,1,1028,1046,1,
385,1859,16,0,345,
1,169,1051,1,489,
1860,16,0,345,1,
46,1861,16,0,345,
1,44,1058,1,578,
1862,16,0,345,1,
682,1863,16,0,345,
1,144,1864,16,0,
345,1,33,1066,1,
461,1865,16,0,345,
1,129,1072,1,27,
1077,1,20,1082,1,
19,1083,1,131,1084,
1,130,1088,1,343,
1092,1,128,1097,1,
127,1101,1,126,1105,
1,125,1109,1,17,
1114,1,15,1118,1,
12,1122,1,10,1127,
1,9,1131,1,327,
1866,16,0,345,1,
3,1136,1,325,1140,
1,645,1867,16,0,
345,1,2,1145,1,
107,1868,16,0,345,
1,21,1869,19,344,
1,21,1870,5,45,
1,210,1871,16,0,
342,1,207,1872,16,
0,342,1,96,1015,
1,95,1020,1,92,
1024,1,517,1873,16,
0,342,1,298,1029,
1,296,1034,1,76,
1874,16,0,342,1,
823,1875,16,0,342,
1,171,1876,16,0,
342,1,1030,1041,1,
1028,1046,1,385,1877,
16,0,342,1,169,
1051,1,489,1878,16,
0,342,1,46,1879,
16,0,342,1,44,
1058,1,578,1880,16,
0,342,1,682,1881,
16,0,342,1,144,
1882,16,0,342,1,
33,1066,1,461,1883,
16,0,342,1,129,
1072,1,27,1077,1,
20,1082,1,19,1083,
1,131,1084,1,130,
1088,1,343,1092,1,
128,1097,1,127,1101,
1,126,1105,1,125,
1109,1,17,1114,1,
15,1118,1,12,1122,
1,10,1127,1,9,
1131,1,327,1884,16,
0,342,1,3,1136,
1,325,1140,1,645,
1885,16,0,342,1,
2,1145,1,107,1886,
16,0,342,1,20,
1887,19,422,1,20,
1888,5,43,1,209,
1889,16,0,420,1,
634,1890,16,0,420,
1,97,1891,16,0,
420,1,734,1892,16,
0,420,1,812,1893,
16,0,420,1,302,
1894,16,0,420,1,
301,1157,1,300,1161,
1,402,1895,16,0,
420,1,506,1896,16,
0,420,1,69,1897,
16,0,420,1,374,
1898,16,0,420,1,
50,1168,1,170,1899,
16,0,420,1,62,
1900,16,0,420,1,
61,1174,1,60,1177,
1,59,1180,1,58,
1183,1,57,1186,1,
56,1189,1,55,1192,
1,54,1195,1,53,
1198,1,52,1201,1,
51,1204,1,478,1901,
16,0,420,1,49,
1208,1,48,1211,1,
47,1214,1,45,1902,
16,0,420,1,567,
1903,16,0,420,1,
671,1904,16,0,420,
1,28,1905,16,0,
420,1,450,1906,16,
0,420,1,976,1907,
16,0,420,1,546,
1908,16,0,420,1,
866,1909,16,0,420,
1,7,1225,1,6,
1229,1,5,1232,1,
4,1910,16,0,420,
1,206,1911,16,0,
420,1,19,1912,19,
341,1,19,1913,5,
45,1,210,1914,16,
0,339,1,207,1915,
16,0,339,1,96,
1015,1,95,1020,1,
92,1024,1,517,1916,
16,0,339,1,298,
1029,1,296,1034,1,
76,1917,16,0,339,
1,823,1918,16,0,
339,1,171,1919,16,
0,339,1,1030,1041,
1,1028,1046,1,385,
1920,16,0,339,1,
169,1051,1,489,1921,
16,0,339,1,46,
1922,16,0,339,1,
44,1058,1,578,1923,
16,0,339,1,682,
1924,16,0,339,1,
144,1925,16,0,339,
1,33,1066,1,461,
1926,16,0,339,1,
129,1072,1,27,1077,
1,20,1082,1,19,
1083,1,131,1084,1,
130,1088,1,343,1092,
1,128,1097,1,127,
1101,1,126,1105,1,
125,1109,1,17,1114,
1,15,1118,1,12,
1122,1,10,1127,1,
9,1131,1,327,1927,
16,0,339,1,3,
1136,1,325,1140,1,
645,1928,16,0,339,
1,2,1145,1,107,
1929,16,0,339,1,
18,1930,19,338,1,
18,1931,5,88,1,
461,1932,16,0,336,
1,450,1933,16,0,
416,1,210,1934,16,
0,336,1,209,1935,
16,0,416,1,207,
1936,16,0,336,1,
206,1937,16,0,416,
1,682,1938,16,0,
336,1,671,1939,16,
0,416,1,171,1940,
16,0,336,1,170,
1941,16,0,416,1,
169,1051,1,645,1942,
16,0,336,1,402,
1943,16,0,416,1,
634,1944,16,0,416,
1,866,1945,16,0,
416,1,385,1946,16,
0,336,1,144,1947,
16,0,336,1,374,
1948,16,0,416,1,
131,1084,1,130,1088,
1,129,1072,1,128,
1097,1,127,1101,1,
126,1105,1,125,1109,
1,107,1949,16,0,
336,1,823,1950,16,
0,336,1,343,1092,
1,578,1951,16,0,
336,1,95,1020,1,
97,1952,16,0,416,
1,96,1015,1,812,
1953,16,0,416,1,
92,1024,1,296,1034,
1,567,1954,16,0,
416,1,327,1955,16,
0,336,1,325,1140,
1,300,1161,1,506,
1956,16,0,416,1,
76,1957,16,0,336,
1,1030,1041,1,301,
1157,1,1028,1046,1,
298,1029,1,69,1958,
16,0,416,1,546,
1959,16,0,416,1,
302,1960,16,0,416,
1,62,1961,16,0,
416,1,61,1174,1,
60,1177,1,59,1180,
1,58,1183,1,57,
1186,1,56,1189,1,
55,1192,1,54,1195,
1,53,1198,1,52,
1201,1,51,1204,1,
50,1168,1,49,1208,
1,48,1211,1,47,
1214,1,46,1962,16,
0,336,1,45,1963,
16,0,416,1,44,
1058,1,517,1964,16,
0,336,1,33,1066,
1,28,1965,16,0,
416,1,27,1077,1,
20,1082,1,17,1114,
1,976,1966,16,0,
416,1,19,1083,1,
734,1967,16,0,416,
1,15,1118,1,12,
1122,1,489,1968,16,
0,336,1,10,1127,
1,9,1131,1,7,
1225,1,6,1229,1,
5,1232,1,4,1969,
16,0,416,1,3,
1136,1,2,1145,1,
478,1970,16,0,416,
1,17,1971,19,335,
1,17,1972,5,45,
1,210,1973,16,0,
333,1,207,1974,16,
0,333,1,96,1015,
1,95,1020,1,92,
1024,1,517,1975,16,
0,333,1,298,1029,
1,296,1034,1,76,
1976,16,0,333,1,
823,1977,16,0,333,
1,171,1978,16,0,
333,1,1030,1041,1,
1028,1046,1,385,1979,
16,0,333,1,169,
1051,1,489,1980,16,
0,333,1,46,1981,
16,0,333,1,44,
1058,1,578,1982,16,
0,333,1,682,1983,
16,0,333,1,144,
1984,16,0,333,1,
33,1066,1,461,1985,
16,0,333,1,129,
1072,1,27,1077,1,
20,1082,1,19,1083,
1,131,1084,1,130,
1088,1,343,1092,1,
128,1097,1,127,1101,
1,126,1105,1,125,
1109,1,17,1114,1,
15,1118,1,12,1122,
1,10,1127,1,9,
1131,1,327,1986,16,
0,333,1,3,1136,
1,325,1140,1,645,
1987,16,0,333,1,
2,1145,1,107,1988,
16,0,333,1,16,
1989,19,159,1,16,
1990,5,20,1,92,
1024,1,44,1058,1,
325,1140,1,33,1991,
16,0,394,1,169,
1051,1,27,1077,1,
343,1092,1,22,1992,
16,0,394,1,20,
1082,1,19,1083,1,
298,1029,1,438,1993,
16,0,157,1,296,
1034,1,10,1127,1,
9,1131,1,1,1994,
16,0,394,1,2,
1145,1,3,1136,1,
96,1015,1,95,1020,
1,15,1995,19,232,
1,15,1996,5,38,
1,210,1997,17,1998,
15,1999,4,30,37,
0,70,0,105,0,
101,0,108,0,100,
0,69,0,120,0,
112,0,65,0,115,
0,115,0,105,0,
103,0,110,0,1,
-1,1,5,2000,20,
561,1,151,1,3,
1,6,1,5,2001,
22,1,92,1,96,
1015,1,95,1020,1,
92,1024,1,299,2002,
17,2003,15,2004,4,
20,37,0,102,0,
105,0,101,0,108,
0,100,0,108,0,
105,0,115,0,116,
0,1,-1,1,5,
2005,20,865,1,113,
1,3,1,2,1,
1,2006,22,1,28,
1,298,1029,1,296,
1034,1,295,2007,16,
0,233,1,1030,1041,
1,1028,1046,1,171,
2008,17,2009,15,2010,
4,24,37,0,70,
0,105,0,101,0,
108,0,100,0,65,
0,115,0,115,0,
105,0,103,0,110,
0,1,-1,1,5,
2011,20,559,1,152,
1,3,1,4,1,
3,2012,22,1,93,
1,169,1051,1,46,
2013,17,2014,15,2015,
4,12,37,0,102,
0,105,0,101,0,
108,0,100,0,1,
-1,1,5,2016,20,
554,1,153,1,3,
1,2,1,1,2017,
22,1,94,1,45,
2018,16,0,230,1,
44,1058,1,144,1326,
1,33,1066,1,131,
1084,1,129,1072,1,
27,1077,1,127,1101,
1,126,1105,1,130,
1088,1,343,1092,1,
128,1097,1,20,1082,
1,19,1083,1,125,
1109,1,17,1114,1,
15,1118,1,12,1122,
1,10,1127,1,9,
1131,1,2,1145,1,
325,1140,1,3,1136,
1,323,2019,17,2020,
15,2004,1,-1,1,
5,2021,20,859,1,
115,1,3,1,4,
1,3,2022,22,1,
29,1,107,1389,1,
14,2023,19,381,1,
14,2024,5,63,1,
209,2025,16,0,379,
1,634,2026,16,0,
379,1,97,2027,16,
0,379,1,96,1015,
1,95,1020,1,92,
1024,1,812,2028,16,
0,379,1,302,2029,
16,0,379,1,301,
1157,1,300,1161,1,
298,1029,1,296,1034,
1,402,2030,16,0,
379,1,506,2031,16,
0,379,1,169,1051,
1,69,2032,16,0,
379,1,50,1168,1,
53,1198,1,170,2033,
16,0,379,1,62,
2034,16,0,379,1,
61,1174,1,60,1177,
1,59,1180,1,58,
1183,1,57,1186,1,
56,1189,1,55,1192,
1,54,1195,1,374,
2035,16,0,379,1,
52,1201,1,51,1204,
1,478,2036,16,0,
379,1,49,1208,1,
48,1211,1,47,1214,
1,45,2037,16,0,
379,1,44,1058,1,
40,2038,16,0,379,
1,343,1092,1,33,
2039,16,0,379,1,
567,2040,16,0,379,
1,671,2041,16,0,
379,1,28,2042,16,
0,379,1,27,1077,
1,22,2043,16,0,
379,1,450,2044,16,
0,379,1,20,1082,
1,19,1083,1,9,
1131,1,976,2045,16,
0,379,1,10,1127,
1,546,2046,16,0,
379,1,866,2047,16,
0,379,1,734,2048,
16,0,379,1,4,
2049,16,0,379,1,
7,1225,1,6,1229,
1,5,1232,1,325,
1140,1,3,1136,1,
2,1145,1,1,2050,
16,0,379,1,206,
2051,16,0,379,1,
13,2052,19,204,1,
13,2053,5,36,1,
96,1015,1,95,1020,
1,92,1024,1,302,
2054,16,0,254,1,
301,1157,1,300,1161,
1,298,1029,1,296,
1034,1,1030,1041,1,
1028,1046,1,169,1051,
1,45,2055,16,0,
254,1,44,1058,1,
144,1326,1,33,1066,
1,131,1084,1,129,
1072,1,27,1077,1,
127,1101,1,126,1105,
1,130,1088,1,343,
1092,1,128,1097,1,
20,1082,1,19,1083,
1,125,1109,1,17,
1114,1,15,1118,1,
12,1122,1,10,1127,
1,9,1131,1,327,
2056,16,0,202,1,
325,1140,1,3,1136,
1,2,1145,1,107,
1389,1,12,2057,19,
252,1,12,2058,5,
34,1,207,2059,16,
0,250,1,96,1015,
1,95,1020,1,92,
1024,1,298,1029,1,
296,1034,1,1030,1041,
1,1028,1046,1,169,
1051,1,44,1058,1,
144,1326,1,343,1092,
1,33,2060,16,0,
392,1,131,1084,1,
129,1072,1,27,1077,
1,127,1101,1,126,
1105,1,130,1088,1,
22,2061,16,0,392,
1,128,1097,1,20,
1082,1,19,1083,1,
125,1109,1,17,1114,
1,15,1118,1,12,
1122,1,10,1127,1,
9,1131,1,325,1140,
1,3,1136,1,2,
1145,1,1,2062,16,
0,392,1,107,1389,
1,11,2063,19,118,
1,11,2064,5,41,
1,421,1270,1,96,
1015,1,95,1020,1,
92,1024,1,1052,2065,
17,2066,15,2067,4,
16,37,0,112,0,
97,0,114,0,108,
0,105,0,115,0,
116,0,1,-1,1,
5,2068,20,823,1,
119,1,3,1,4,
1,3,2069,22,1,
33,1,1049,2070,17,
2071,15,2067,1,-1,
1,5,2072,20,825,
1,118,1,3,1,
2,1,1,2073,22,
1,32,1,1048,2074,
17,2075,15,2067,1,
-1,1,5,120,1,
1,1,1,2076,22,
1,34,1,298,1029,
1,296,1034,1,76,
2077,16,0,312,1,
1030,1041,1,1028,1046,
1,385,1301,1,1026,
2078,16,0,116,1,
169,1051,1,44,1058,
1,144,1326,1,33,
1066,1,131,1084,1,
129,1072,1,27,1077,
1,127,1101,1,126,
1105,1,130,1088,1,
343,1092,1,128,1097,
1,20,1082,1,19,
1083,1,125,1109,1,
17,1114,1,15,1118,
1,13,2079,16,0,
409,1,12,1122,1,
10,1127,1,9,1131,
1,8,2080,16,0,
414,1,4,2081,16,
0,413,1,325,1140,
1,3,1136,1,2,
1145,1,107,1389,1,
10,2082,19,325,1,
10,2083,5,122,1,
716,2084,16,0,323,
1,715,1292,1,712,
1297,1,949,1276,1,
939,2085,16,0,323,
1,908,1311,1,209,
2086,16,0,323,1,
671,2087,16,0,323,
1,450,2088,16,0,
323,1,448,1340,1,
446,1347,1,206,2089,
16,0,323,1,444,
2090,16,0,410,1,
443,2091,17,2092,15,
2093,4,18,37,0,
102,0,117,0,110,
0,99,0,110,0,
97,0,109,0,101,
0,1,-1,1,5,
154,1,3,1,3,
2094,22,1,60,1,
440,2095,17,2096,15,
2093,1,-1,1,5,
154,1,3,1,3,
2097,22,1,61,1,
438,2098,17,2099,15,
2093,1,-1,1,5,
2100,20,605,1,143,
1,3,1,2,1,
1,2101,22,1,62,
1,436,1365,1,434,
2102,16,0,410,1,
432,1342,1,909,1307,
1,430,1380,1,607,
2103,16,0,323,1,
421,1270,1,634,2104,
16,0,323,1,170,
2105,16,0,323,1,
169,1051,1,645,1376,
1,402,2106,16,0,
323,1,847,2107,16,
0,323,1,369,1319,
1,866,2108,16,0,
323,1,385,1301,1,
144,1326,1,621,2109,
16,0,323,1,619,
1265,1,374,2110,16,
0,323,1,373,1287,
1,371,1315,1,131,
1084,1,130,1088,1,
129,1072,1,128,1097,
1,127,1101,1,126,
1105,1,125,1109,1,
840,2111,16,0,323,
1,595,2112,16,0,
323,1,534,2113,16,
0,323,1,107,1389,
1,545,1357,1,343,
1092,1,95,1020,1,
97,2114,16,0,323,
1,96,1015,1,812,
2115,16,0,323,1,
92,1024,1,567,2116,
16,0,323,1,566,
1333,1,546,2117,16,
0,323,1,325,1140,
1,544,1361,1,1030,
1041,1,301,1157,1,
1028,1046,1,1027,2118,
16,0,323,1,69,
2119,16,0,323,1,
50,1168,1,55,1192,
1,57,1186,1,59,
1180,1,61,1174,1,
302,2120,16,0,323,
1,62,2121,16,0,
323,1,300,1161,1,
60,1177,1,298,1029,
1,58,1183,1,296,
1034,1,56,1189,1,
772,2122,16,0,323,
1,54,1195,1,53,
1198,1,52,1201,1,
51,1204,1,1006,1322,
1,49,1208,1,48,
1211,1,47,1214,1,
45,2123,16,0,323,
1,44,1058,1,997,
2124,16,0,323,1,
40,2125,16,0,423,
1,756,2126,16,0,
323,1,754,1371,1,
33,2127,16,0,423,
1,28,2128,16,0,
323,1,506,2129,16,
0,323,1,27,1077,
1,20,1082,1,17,
1114,1,22,2130,16,
0,423,1,21,2131,
16,0,323,1,976,
2132,16,0,323,1,
19,1083,1,734,2133,
16,0,323,1,14,
2134,16,0,323,1,
15,1118,1,731,1281,
1,5,1232,1,12,
1122,1,11,2135,16,
0,410,1,10,1127,
1,9,1131,1,0,
2136,16,0,323,1,
7,1225,1,6,1229,
1,699,2137,16,0,
323,1,4,2138,16,
0,323,1,3,1136,
1,2,1145,1,1,
2139,16,0,423,1,
478,2140,16,0,323,
1,9,2141,19,226,
1,9,2142,5,61,
1,619,1265,1,210,
1997,1,421,1270,1,
96,1015,1,95,1020,
1,949,1276,1,92,
1024,1,373,1287,1,
731,1281,1,299,2143,
16,0,224,1,298,
1029,1,296,1034,1,
385,1301,1,715,1292,
1,712,1297,1,1030,
1041,1,1028,1046,1,
171,2008,1,169,1051,
1,909,1307,1,908,
1311,1,371,1315,1,
369,1319,1,46,2013,
1,44,1058,1,1006,
1322,1,3,1136,1,
144,1326,1,33,1066,
1,19,1083,1,125,
1109,1,566,1333,1,
126,1105,1,129,1072,
1,27,1077,1,20,
1082,1,127,1101,1,
131,1084,1,130,1088,
1,343,1092,1,128,
1097,1,448,1340,1,
432,1342,1,446,1347,
1,17,1114,1,15,
1118,1,10,1127,1,
9,1131,1,12,1122,
1,546,1353,1,545,
1357,1,544,1361,1,
436,1365,1,756,2144,
16,0,238,1,2,
1145,1,754,1371,1,
325,1140,1,645,1376,
1,430,1380,1,1,
1385,1,107,1389,1,
8,2145,19,163,1,
8,2146,5,20,1,
92,1024,1,44,1058,
1,325,1140,1,33,
2147,16,0,386,1,
169,1051,1,27,1077,
1,343,1092,1,22,
2148,16,0,386,1,
20,1082,1,19,1083,
1,298,1029,1,438,
2149,16,0,161,1,
296,1034,1,10,1127,
1,9,1131,1,1,
2150,16,0,386,1,
2,1145,1,3,1136,
1,96,1015,1,95,
1020,1,7,2151,19,
127,1,7,2152,5,
41,1,210,1997,1,
96,1015,1,95,1020,
1,92,1024,1,1049,
2153,16,0,383,1,
299,2154,16,0,223,
1,298,1029,1,296,
1034,1,171,2008,1,
1030,1041,1,1028,1046,
1,385,2155,16,0,
179,1,169,1051,1,
489,2156,16,0,387,
1,369,2157,16,0,
189,1,46,2013,1,
44,1058,1,33,1066,
1,144,1326,1,448,
2158,16,0,189,1,
461,2159,16,0,125,
1,19,1083,1,129,
1072,1,27,1077,1,
127,1101,1,131,1084,
1,130,1088,1,343,
1092,1,128,1097,1,
20,2160,16,0,403,
1,126,1105,1,125,
1109,1,17,1114,1,
15,1118,1,12,1122,
1,10,1127,1,9,
1131,1,325,1140,1,
3,1136,1,2,1145,
1,107,1389,1,6,
2161,19,279,1,6,
2162,5,43,1,209,
2163,16,0,277,1,
634,2164,16,0,277,
1,97,2165,16,0,
277,1,734,2166,16,
0,277,1,812,2167,
16,0,277,1,302,
2168,16,0,277,1,
301,1157,1,300,1161,
1,402,2169,16,0,
277,1,506,2170,16,
0,277,1,69,2171,
16,0,277,1,374,
2172,16,0,277,1,
50,1168,1,170,2173,
16,0,277,1,62,
2174,16,0,277,1,
61,1174,1,60,1177,
1,59,1180,1,58,
1183,1,57,1186,1,
56,1189,1,55,1192,
1,54,1195,1,53,
1198,1,52,1201,1,
51,1204,1,478,2175,
16,0,277,1,49,
1208,1,48,1211,1,
47,1214,1,45,2176,
16,0,277,1,567,
2177,16,0,277,1,
671,2178,16,0,277,
1,28,2179,16,0,
277,1,450,2180,16,
0,277,1,976,2181,
16,0,277,1,546,
2182,16,0,277,1,
866,2183,16,0,277,
1,7,1225,1,6,
1229,1,5,1232,1,
4,2184,16,0,277,
1,206,2185,16,0,
277,1,5,2186,19,
146,1,5,2187,5,
125,1,716,2188,16,
0,405,1,715,1292,
1,712,1297,1,949,
1276,1,939,2189,16,
0,405,1,908,1311,
1,209,2190,16,0,
405,1,671,2191,16,
0,405,1,450,2192,
16,0,405,1,448,
1340,1,447,2193,16,
0,144,1,446,1347,
1,206,2194,16,0,
405,1,441,2195,16,
0,164,1,439,2196,
16,0,160,1,437,
2197,16,0,164,1,
436,1365,1,433,2198,
16,0,169,1,432,
1342,1,909,1307,1,
430,1380,1,607,2199,
16,0,405,1,421,
1270,1,634,2200,16,
0,405,1,170,2201,
16,0,405,1,169,
1051,1,645,1376,1,
402,2202,16,0,405,
1,847,2203,16,0,
405,1,370,2204,16,
0,190,1,369,1319,
1,866,2205,16,0,
405,1,385,1301,1,
144,1326,1,621,2206,
16,0,405,1,368,
2207,16,0,190,1,
619,1265,1,374,2208,
16,0,405,1,373,
1287,1,371,1315,1,
131,1084,1,130,1088,
1,129,1072,1,128,
1097,1,127,1101,1,
126,1105,1,125,1109,
1,840,2209,16,0,
405,1,595,2210,16,
0,405,1,534,2211,
16,0,405,1,107,
1389,1,545,1357,1,
343,1092,1,95,1020,
1,97,2212,16,0,
405,1,96,1015,1,
812,2213,16,0,405,
1,1050,2214,16,0,
384,1,92,1024,1,
567,2215,16,0,405,
1,566,1333,1,546,
2216,16,0,405,1,
325,1140,1,544,1361,
1,50,1168,1,1030,
1041,1,1028,1046,1,
1027,2217,16,0,405,
1,69,2218,16,0,
405,1,59,1180,1,
55,1192,1,57,1186,
1,62,2219,16,0,
405,1,61,1174,1,
302,2220,16,0,262,
1,301,1157,1,300,
1161,1,60,1177,1,
298,1029,1,58,1183,
1,296,1034,1,56,
1189,1,772,2221,16,
0,405,1,54,1195,
1,53,1198,1,52,
1201,1,51,1204,1,
1006,1322,1,49,1208,
1,48,1211,1,47,
1214,1,45,2222,16,
0,262,1,44,1058,
1,39,2223,16,0,
385,1,997,2224,16,
0,405,1,756,2225,
16,0,405,1,754,
1371,1,33,1066,1,
28,2226,16,0,405,
1,506,2227,16,0,
405,1,27,1077,1,
26,2228,16,0,393,
1,17,1114,1,20,
1082,1,21,2229,16,
0,405,1,976,2230,
16,0,405,1,19,
1083,1,734,2231,16,
0,405,1,14,2232,
16,0,405,1,15,
1118,1,731,1281,1,
13,2233,16,0,384,
1,12,1122,1,5,
1232,1,10,1127,1,
9,1131,1,0,2234,
16,0,405,1,7,
1225,1,6,1229,1,
699,2235,16,0,405,
1,4,2236,16,0,
405,1,3,1136,1,
2,1145,1,1,1385,
1,478,2237,16,0,
405,1,3,2238,19,
282,1,3,2239,5,
63,1,209,2240,16,
0,280,1,634,2241,
16,0,280,1,97,
2242,16,0,280,1,
96,1015,1,95,1020,
1,92,1024,1,812,
2243,16,0,280,1,
302,2244,16,0,280,
1,301,1157,1,300,
1161,1,298,1029,1,
296,1034,1,402,2245,
16,0,280,1,506,
2246,16,0,280,1,
169,1051,1,69,2247,
16,0,280,1,50,
1168,1,53,1198,1,
170,2248,16,0,280,
1,62,2249,16,0,
280,1,61,1174,1,
60,1177,1,59,1180,
1,58,1183,1,57,
1186,1,56,1189,1,
55,1192,1,54,1195,
1,374,2250,16,0,
280,1,52,1201,1,
51,1204,1,478,2251,
16,0,280,1,49,
1208,1,48,1211,1,
47,1214,1,45,2252,
16,0,280,1,44,
1058,1,40,2253,16,
0,425,1,343,1092,
1,33,2254,16,0,
425,1,567,2255,16,
0,280,1,671,2256,
16,0,280,1,28,
2257,16,0,280,1,
27,1077,1,22,2258,
16,0,425,1,450,
2259,16,0,280,1,
20,1082,1,19,1083,
1,9,1131,1,976,
2260,16,0,280,1,
10,1127,1,546,2261,
16,0,280,1,866,
2262,16,0,280,1,
734,2263,16,0,280,
1,4,2264,16,0,
280,1,7,1225,1,
6,1229,1,5,1232,
1,325,1140,1,3,
1136,1,2,1145,1,
1,2265,16,0,425,
1,206,2266,16,0,
280,1,2,2267,19,
317,1,2,2268,5,
60,1,619,1265,1,
421,1270,1,96,1015,
1,95,1020,1,949,
1276,1,92,1024,1,
731,1281,1,298,1029,
1,296,1034,1,373,
1287,1,715,1292,1,
712,1297,1,1030,1041,
1,1028,1046,1,385,
1301,1,169,1051,1,
909,1307,1,908,1311,
1,371,1315,1,772,
1414,1,369,1319,1,
44,1058,1,1006,1322,
1,791,1419,1,3,
1136,1,788,1423,1,
144,1326,1,33,1066,
1,19,1083,1,125,
1109,1,566,1333,1,
126,1105,1,129,1072,
1,27,1077,1,20,
1082,1,127,1101,1,
131,1084,1,130,1088,
1,343,1092,1,128,
1097,1,448,1340,1,
432,1342,1,446,1347,
1,17,1114,1,15,
1118,1,10,1127,1,
9,1131,1,12,1122,
1,546,1353,1,545,
1357,1,544,1361,1,
436,1365,1,756,1427,
1,2,1145,1,754,
1371,1,325,1140,1,
645,1376,1,430,1380,
1,1,1385,1,107,
1389,2,1,0};
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
new Sfactory(this,"PackageRef_1",new SCreator(PackageRef_1_factory));
new Sfactory(this,"funcbody_2",new SCreator(funcbody_2_factory));
new Sfactory(this,"chunk_4",new SCreator(chunk_4_factory));
new Sfactory(this,"stat_12",new SCreator(stat_12_factory));
new Sfactory(this,"exp_1",new SCreator(exp_1_factory));
new Sfactory(this,"namelist",new SCreator(namelist_factory));
new Sfactory(this,"Retval",new SCreator(Retval_factory));
new Sfactory(this,"LocalInit",new SCreator(LocalInit_factory));
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
new Sfactory(this,"binop_14",new SCreator(binop_14_factory));
new Sfactory(this,"Binop_1",new SCreator(Binop_1_factory));
new Sfactory(this,"LocalInit_1",new SCreator(LocalInit_1_factory));
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
new Sfactory(this,"binop_15",new SCreator(binop_15_factory));
new Sfactory(this,"unop",new SCreator(unop_factory));
new Sfactory(this,"binop_8",new SCreator(binop_8_factory));
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
new Sfactory(this,"chunk",new SCreator(chunk_factory));
new Sfactory(this,"block_1",new SCreator(block_1_factory));
new Sfactory(this,"binop",new SCreator(binop_factory));
new Sfactory(this,"elseif_2",new SCreator(elseif_2_factory));
new Sfactory(this,"Retval_1",new SCreator(Retval_1_factory));
new Sfactory(this,"funcname_3",new SCreator(funcname_3_factory));
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
new Sfactory(this,"elseif_3",new SCreator(elseif_3_factory));
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
public static object PackageRef_1_factory(Parser yyp) { return new PackageRef_1(yyp); }
public static object funcbody_2_factory(Parser yyp) { return new funcbody_2(yyp); }
public static object chunk_4_factory(Parser yyp) { return new chunk_4(yyp); }
public static object stat_12_factory(Parser yyp) { return new stat_12(yyp); }
public static object exp_1_factory(Parser yyp) { return new exp_1(yyp); }
public static object namelist_factory(Parser yyp) { return new namelist(yyp); }
public static object Retval_factory(Parser yyp) { return new Retval(yyp); }
public static object LocalInit_factory(Parser yyp) { return new LocalInit(yyp); }
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
public static object binop_14_factory(Parser yyp) { return new binop_14(yyp); }
public static object Binop_1_factory(Parser yyp) { return new Binop_1(yyp); }
public static object LocalInit_1_factory(Parser yyp) { return new LocalInit_1(yyp); }
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
public static object binop_15_factory(Parser yyp) { return new binop_15(yyp); }
public static object unop_factory(Parser yyp) { return new unop(yyp); }
public static object binop_8_factory(Parser yyp) { return new binop_8(yyp); }
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
public static object chunk_factory(Parser yyp) { return new chunk(yyp); }
public static object block_1_factory(Parser yyp) { return new block_1(yyp); }
public static object binop_factory(Parser yyp) { return new binop(yyp); }
public static object elseif_2_factory(Parser yyp) { return new elseif_2(yyp); }
public static object Retval_1_factory(Parser yyp) { return new Retval_1(yyp); }
public static object funcname_3_factory(Parser yyp) { return new funcname_3(yyp); }
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
public static object elseif_3_factory(Parser yyp) { return new elseif_3(yyp); }
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
