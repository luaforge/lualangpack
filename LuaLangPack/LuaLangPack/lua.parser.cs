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
 public  block (Parser yyp):base(((syntax)yyp)){}
 public  void  FillScope ( LuaScope  scope ){ if ( c != null ) c . FillScope ( scope );
}

public override string yyname { get { return "block"; }}
public override int yynum { get { return 55; }}
}
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
 public  void  FillScope ( LuaScope  s ){ if ( p != null ) p . FillScope ( s );
}

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
 public  virtual  void  FillScope ( LuaScope  s , var  v ){}

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
 public  override  void  FillScope ( LuaScope  s , var  v ){}

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
 public  virtual  void  FillScope ( LuaScope  s , var  v ){ if ( f != null ){ f . FillScope ( s , v );
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
 public  virtual  void  FillScope ( LuaScope  s ){ LuaScope  nested = new  LuaScope ( s );
 f . FillScope ( nested );
 s . nested . AddLast ( nested );
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
//%+FunctionCall+86
public class FunctionCall : stat{
 prefixexp  p ;
 public  FunctionCall (Parser yyp, prefixexp  a ):base(((syntax)yyp)){ p = a ;
}
 public  override  void  FillScope ( LuaScope  scope ){ p . FillScope ( scope );
}

public override string yyname { get { return "FunctionCall"; }}
public override int yynum { get { return 86; }}
public FunctionCall(Parser yyp):base(yyp){}}
//%+SIf+87
public class SIf : stat{
 exp  e ;
 block  b ;
 public  SIf (Parser yyp, exp  a , block  i ):base(((syntax)yyp)){ e = a ;
 b = i ;
}
 public  override  void  FillScope ( LuaScope  s ){ e . FillScope ( s );
 b . FillScope ( s );
}

public override string yyname { get { return "SIf"; }}
public override int yynum { get { return 87; }}
public SIf(Parser yyp):base(yyp){}}
//%+SElseIf+88
public class SElseIf : stat{
 exp  e ;
 block  b ;
 elseif  eli ;
 public  SElseIf (Parser yyp, exp  a , block  i , elseif  j ):base(((syntax)yyp)){ e = a ;
 b = i ;
 eli = j ;
}
 public  override  void  FillScope ( LuaScope  s ){ e . FillScope ( s );
 b . FillScope ( s );
 eli . FillScope ( s );
}

public override string yyname { get { return "SElseIf"; }}
public override int yynum { get { return 88; }}
public SElseIf(Parser yyp):base(yyp){}}
//%+SElse+89
public class SElse : stat{
 exp  e ;
 block  b1 ;
 block  b2 ;
 public  SElse (Parser yyp, exp  a , block  i , block  j ):base(((syntax)yyp)){ e = a ;
 b1 = i ;
 b2 = j ;
}
 public  override  void  FillScope ( LuaScope  s ){ e . FillScope ( s );
 b1 . FillScope ( s );
 b2 . FillScope ( s );
}

public override string yyname { get { return "SElse"; }}
public override int yynum { get { return 89; }}
public SElse(Parser yyp):base(yyp){}}
//%+elseif+90
public class elseif : SYMBOL{
 public  elseif (Parser yyp):base(((syntax)yyp)){}
 public  virtual  void  FillScope ( LuaScope  scope ){}

public override string yyname { get { return "elseif"; }}
public override int yynum { get { return 90; }}
}
//%+If+91
public class If : elseif{
 exp  e ;
 block  b ;
 public  If (Parser yyp, exp  a , block  i ):base(((syntax)yyp)){ e = a ;
 b = i ;
}
 public  override  void  FillScope ( LuaScope  s ){ e . FillScope ( s );
 b . FillScope ( s );
}

public override string yyname { get { return "If"; }}
public override int yynum { get { return 91; }}
public If(Parser yyp):base(yyp){}}
//%+ElseIf+92
public class ElseIf : elseif{
 exp  e ;
 block  b ;
 elseif  eli ;
 public  ElseIf (Parser yyp, exp  a , block  i , elseif  j ):base(((syntax)yyp)){ e = a ;
 b = i ;
 eli = j ;
}
 public  override  void  FillScope ( LuaScope  s ){ e . FillScope ( s );
 b . FillScope ( s );
 eli . FillScope ( s );
}

public override string yyname { get { return "ElseIf"; }}
public override int yynum { get { return 92; }}
public ElseIf(Parser yyp):base(yyp){}}
//%+Else+93
public class Else : elseif{
 exp  e ;
 block  b1 ;
 block  b2 ;
 public  Else (Parser yyp, exp  a , block  i , block  j ):base(((syntax)yyp)){ e = a ;
 b1 = i ;
 b2 = j ;
}
 public  override  void  FillScope ( LuaScope  s ){ e . FillScope ( s );
 b1 . FillScope ( s );
 b2 . FillScope ( s );
}

public override string yyname { get { return "Else"; }}
public override int yynum { get { return 93; }}
public Else(Parser yyp):base(yyp){}}
//%+arg+94
public class arg : SYMBOL{
 private  explist  e ;
 private  tableconstructor  t ;
 public  arg (Parser yyp, tableconstructor  a ):base(((syntax)yyp)){ t = a ;
}
 public  arg (Parser yyp, explist  a ):base(((syntax)yyp)){ e = a ;
}
 public  virtual  void  FillScope ( LuaScope  s ){ if ( e != null ){ e . FillScope ( s );
}
 else  if ( t != null ){ t . FillScope ( s );
}
}

public override string yyname { get { return "arg"; }}
public override int yynum { get { return 94; }}
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

public class block_2 : block {
  public block_2(Parser yyq):base(yyq){}}

public class Assignment_1 : Assignment {
  public Assignment_1(Parser yyq):base(yyq, 
	((varlist)(yyq.StackAt(2).m_value))
	, 
	((explist)(yyq.StackAt(0).m_value))
	 ){}}

public class FunctionCall_1 : FunctionCall {
  public FunctionCall_1(Parser yyq):base(yyq, 
	((prefixexp)(yyq.StackAt(0).m_value))
	 ){}}

public class stat_1 : stat {
  public stat_1(Parser yyq):base(yyq){}}

public class stat_2 : stat {
  public stat_2(Parser yyq):base(yyq){}}

public class stat_3 : stat {
  public stat_3(Parser yyq):base(yyq){}}

public class stat_4 : stat {
  public stat_4(Parser yyq):base(yyq){}}

public class SIf_1 : SIf {
  public SIf_1(Parser yyq):base(yyq, 
	((exp)(yyq.StackAt(3).m_value))
	, 
	((block)(yyq.StackAt(1).m_value))
	 ){}}

public class SElseIf_1 : SElseIf {
  public SElseIf_1(Parser yyq):base(yyq, 
	((exp)(yyq.StackAt(5).m_value))
	, 
	((block)(yyq.StackAt(3).m_value))
	, 
	((elseif)(yyq.StackAt(1).m_value))
	 ){}}

public class SElse_1 : SElse {
  public SElse_1(Parser yyq):base(yyq, 
	((exp)(yyq.StackAt(5).m_value))
	, 
	((block)(yyq.StackAt(3).m_value))
	, 
	((block)(yyq.StackAt(1).m_value))
	 ){}}

public class stat_5 : stat {
  public stat_5(Parser yyq):base(yyq){}}

public class Retval_1 : Retval {
  public Retval_1(Parser yyq):base(yyq, 
	((explist)(yyq.StackAt(0).m_value))
	 ){}}

public class stat_6 : stat {
  public stat_6(Parser yyq):base(yyq){}}

public class stat_7 : stat {
  public stat_7(Parser yyq):base(yyq){}}

public class stat_8 : stat {
  public stat_8(Parser yyq):base(yyq){}}

public class stat_9 : stat {
  public stat_9(Parser yyq):base(yyq){}}

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

public class stat_10 : stat {
  public stat_10(Parser yyq):base(yyq){}}

public class LocalInit_1 : LocalInit {
  public LocalInit_1(Parser yyq):base(yyq, 
	((namelist)(yyq.StackAt(1).m_value))
	, 
	((init)(yyq.StackAt(0).m_value))
	 ){}}

public class ElseIf_1 : ElseIf {
  public ElseIf_1(Parser yyq):base(yyq, 
	((exp)(yyq.StackAt(4).m_value))
	, 
	((block)(yyq.StackAt(2).m_value))
	, 
	((elseif)(yyq.StackAt(0).m_value))
	 ){}}

public class Else_1 : Else {
  public Else_1(Parser yyq):base(yyq, 
	((exp)(yyq.StackAt(4).m_value))
	, 
	((block)(yyq.StackAt(2).m_value))
	, 
	((block)(yyq.StackAt(0).m_value))
	 ){}}

public class If_1 : If {
  public If_1(Parser yyq):base(yyq, 
	((exp)(yyq.StackAt(2).m_value))
	, 
	((block)(yyq.StackAt(0).m_value))
	 ){}}

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

public class fieldlist_3 : fieldlist {
  public fieldlist_3(Parser yyq):base(yyq, 
	((field)(yyq.StackAt(1).m_value))
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
  public override int yynum { get { return 115; }}}

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
  public override int yynum { get { return 125; }}}

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
0,116,0,1,115,
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
69,0,1,4,1,
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
90,1,2,2,0,
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
103,0,1,94,1,
2,2,0,1,324,
214,18,1,324,215,
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
1,125,1,2,2,
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
18,1,208,203,2,
0,1,207,251,18,
1,207,107,2,0,
1,206,252,18,1,
206,253,20,254,4,
12,76,0,66,0,
82,0,65,0,67,
0,75,0,1,12,
1,1,2,0,1,
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
5,1,1,2,0,
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
28,253,2,0,1,
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
429,5,191,1,193,
430,19,431,4,10,
97,0,114,0,103,
0,95,0,52,0,
1,193,432,5,4,
1,40,433,16,0,
382,1,22,434,16,
0,211,1,1,435,
16,0,211,1,33,
436,16,0,211,1,
192,437,19,438,4,
12,117,0,110,0,
111,0,112,0,95,
0,51,0,1,192,
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
303,1,191,463,19,
464,4,12,117,0,
110,0,111,0,112,
0,95,0,50,0,
1,191,439,1,190,
465,19,466,4,12,
117,0,110,0,111,
0,112,0,95,0,
49,0,1,190,439,
1,189,467,19,468,
4,10,97,0,114,
0,103,0,95,0,
51,0,1,189,432,
1,188,469,19,470,
4,16,98,0,105,
0,110,0,111,0,
112,0,95,0,49,
0,53,0,1,188,
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
187,488,19,489,4,
16,98,0,105,0,
110,0,111,0,112,
0,95,0,49,0,
52,0,1,187,471,
1,186,490,19,491,
4,16,98,0,105,
0,110,0,111,0,
112,0,95,0,49,
0,51,0,1,186,
471,1,185,492,19,
493,4,16,98,0,
105,0,110,0,111,
0,112,0,95,0,
49,0,50,0,1,
185,471,1,184,494,
19,495,4,16,98,
0,105,0,110,0,
111,0,112,0,95,
0,49,0,49,0,
1,184,471,1,183,
496,19,497,4,16,
98,0,105,0,110,
0,111,0,112,0,
95,0,49,0,48,
0,1,183,471,1,
182,498,19,499,4,
14,98,0,105,0,
110,0,111,0,112,
0,95,0,57,0,
1,182,471,1,181,
500,19,501,4,14,
98,0,105,0,110,
0,111,0,112,0,
95,0,56,0,1,
181,471,1,180,502,
19,503,4,14,98,
0,105,0,110,0,
111,0,112,0,95,
0,55,0,1,180,
471,1,179,504,19,
505,4,14,98,0,
105,0,110,0,111,
0,112,0,95,0,
54,0,1,179,471,
1,178,506,19,507,
4,14,98,0,105,
0,110,0,111,0,
112,0,95,0,53,
0,1,178,471,1,
177,508,19,509,4,
14,98,0,105,0,
110,0,111,0,112,
0,95,0,52,0,
1,177,471,1,176,
510,19,511,4,14,
98,0,105,0,110,
0,111,0,112,0,
95,0,51,0,1,
176,471,1,175,512,
19,513,4,14,98,
0,105,0,110,0,
111,0,112,0,95,
0,50,0,1,175,
471,1,174,514,19,
515,4,14,98,0,
105,0,110,0,111,
0,112,0,95,0,
49,0,1,174,471,
1,173,516,19,517,
4,18,112,0,97,
0,114,0,108,0,
105,0,115,0,116,
0,95,0,51,0,
1,173,518,5,2,
1,1050,519,16,0,
319,1,13,520,16,
0,119,1,172,521,
19,522,4,20,102,
0,105,0,101,0,
108,0,100,0,115,
0,101,0,112,0,
95,0,50,0,1,
172,523,5,1,1,
299,524,16,0,220,
1,171,525,19,526,
4,20,102,0,105,
0,101,0,108,0,
100,0,115,0,101,
0,112,0,95,0,
49,0,1,171,523,
1,170,527,19,528,
4,20,110,0,97,
0,109,0,101,0,
108,0,105,0,115,
0,116,0,95,0,
51,0,1,170,529,
5,3,1,370,530,
16,0,188,1,447,
531,16,0,138,1,
368,532,16,0,187,
1,169,533,19,534,
4,20,110,0,97,
0,109,0,101,0,
108,0,105,0,115,
0,116,0,95,0,
50,0,1,169,529,
1,168,535,19,536,
4,20,102,0,117,
0,110,0,99,0,
110,0,97,0,109,
0,101,0,95,0,
51,0,1,168,537,
5,2,1,437,538,
16,0,153,1,441,
539,16,0,156,1,
167,540,19,541,4,
20,102,0,117,0,
110,0,99,0,110,
0,97,0,109,0,
101,0,95,0,50,
0,1,167,537,1,
166,542,19,543,4,
20,110,0,97,0,
109,0,101,0,108,
0,105,0,115,0,
116,0,95,0,49,
0,1,166,529,1,
165,544,19,545,4,
14,102,0,105,0,
101,0,108,0,100,
0,95,0,49,0,
1,165,546,5,2,
1,302,547,16,0,
227,1,45,548,16,
0,227,1,164,549,
19,550,4,26,70,
0,105,0,101,0,
108,0,100,0,65,
0,115,0,115,0,
105,0,103,0,110,
0,95,0,49,0,
1,164,546,1,163,
551,19,552,4,32,
70,0,105,0,101,
0,108,0,100,0,
69,0,120,0,112,
0,65,0,115,0,
115,0,105,0,103,
0,110,0,95,0,
49,0,1,163,546,
1,162,553,19,554,
4,10,97,0,114,
0,103,0,95,0,
50,0,1,162,432,
1,161,555,19,556,
4,10,97,0,114,
0,103,0,95,0,
49,0,1,161,432,
1,160,557,19,558,
4,20,102,0,117,
0,110,0,99,0,
116,0,105,0,111,
0,110,0,95,0,
49,0,1,160,559,
5,23,1,374,560,
16,0,283,1,567,
561,16,0,283,1,
812,562,16,0,283,
1,546,563,16,0,
283,1,976,564,16,
0,283,1,170,565,
16,0,283,1,28,
566,16,0,283,1,
450,567,16,0,283,
1,402,568,16,0,
283,1,634,569,16,
0,283,1,69,570,
16,0,283,1,209,
571,16,0,283,1,
302,572,16,0,283,
1,206,573,16,0,
283,1,62,574,16,
0,283,1,671,575,
16,0,283,1,506,
576,16,0,283,1,
478,577,16,0,283,
1,734,578,16,0,
283,1,4,579,16,
0,283,1,97,580,
16,0,283,1,866,
581,16,0,283,1,
45,582,16,0,283,
1,159,583,19,584,
4,20,102,0,117,
0,110,0,99,0,
98,0,111,0,100,
0,121,0,95,0,
52,0,1,159,585,
5,3,1,434,586,
16,0,168,1,444,
587,16,0,150,1,
11,588,16,0,411,
1,158,589,19,590,
4,20,102,0,117,
0,110,0,99,0,
98,0,111,0,100,
0,121,0,95,0,
51,0,1,158,585,
1,157,591,19,592,
4,20,102,0,117,
0,110,0,99,0,
98,0,111,0,100,
0,121,0,95,0,
50,0,1,157,585,
1,156,593,19,594,
4,20,102,0,117,
0,110,0,99,0,
98,0,111,0,100,
0,121,0,95,0,
49,0,1,156,585,
1,155,595,19,596,
4,20,102,0,117,
0,110,0,99,0,
110,0,97,0,109,
0,101,0,95,0,
49,0,1,155,537,
1,154,597,19,598,
4,24,80,0,97,
0,99,0,107,0,
97,0,103,0,101,
0,82,0,101,0,
102,0,95,0,49,
0,1,154,599,5,
39,1,534,600,16,
0,404,1,209,601,
16,0,306,1,206,
602,16,0,306,1,
847,603,16,0,404,
1,97,604,16,0,
306,1,734,605,16,
0,306,1,840,606,
16,0,404,1,302,
607,16,0,306,1,
621,608,16,0,404,
1,939,609,16,0,
404,1,402,610,16,
0,306,1,506,611,
16,0,306,1,976,
612,16,0,306,1,
716,613,16,0,404,
1,607,614,16,0,
404,1,170,615,16,
0,306,1,69,616,
16,0,306,1,1027,
617,16,0,404,1,
812,618,16,0,306,
1,62,619,16,0,
306,1,595,620,16,
0,404,1,699,621,
16,0,404,1,374,
622,16,0,306,1,
478,623,16,0,306,
1,45,624,16,0,
306,1,997,625,16,
0,404,1,567,626,
16,0,306,1,671,
627,16,0,306,1,
28,628,16,0,306,
1,772,629,16,0,
404,1,450,630,16,
0,306,1,21,631,
16,0,404,1,14,
632,16,0,404,1,
634,633,16,0,306,
1,546,634,16,0,
306,1,866,635,16,
0,306,1,756,636,
16,0,404,1,4,
637,16,0,306,1,
0,638,16,0,404,
1,153,639,19,640,
4,20,84,0,97,
0,98,0,108,0,
101,0,82,0,101,
0,102,0,95,0,
49,0,1,153,599,
1,152,641,19,642,
4,10,118,0,97,
0,114,0,95,0,
49,0,1,152,599,
1,151,643,19,644,
4,18,118,0,97,
0,114,0,108,0,
105,0,115,0,116,
0,95,0,50,0,
1,151,645,5,16,
1,21,646,16,0,
195,1,756,647,16,
0,247,1,847,648,
16,0,247,1,534,
649,16,0,247,1,
595,650,16,0,247,
1,1027,651,16,0,
247,1,14,652,16,
0,247,1,772,653,
16,0,247,1,840,
654,16,0,247,1,
699,655,16,0,247,
1,997,656,16,0,
247,1,607,657,16,
0,247,1,939,658,
16,0,247,1,716,
659,16,0,247,1,
0,660,16,0,247,
1,621,661,16,0,
247,1,150,662,19,
663,4,18,118,0,
97,0,114,0,108,
0,105,0,115,0,
116,0,95,0,49,
0,1,150,645,1,
149,664,19,665,4,
22,112,0,114,0,
101,0,102,0,105,
0,120,0,101,0,
120,0,112,0,95,
0,51,0,1,149,
666,5,39,1,534,
667,16,0,426,1,
209,668,16,0,388,
1,206,669,16,0,
388,1,847,670,16,
0,426,1,97,671,
16,0,388,1,734,
672,16,0,388,1,
840,673,16,0,426,
1,302,674,16,0,
388,1,621,675,16,
0,426,1,939,676,
16,0,426,1,402,
677,16,0,388,1,
506,678,16,0,388,
1,976,679,16,0,
388,1,716,680,16,
0,426,1,607,681,
16,0,426,1,170,
682,16,0,388,1,
69,683,16,0,388,
1,1027,684,16,0,
426,1,812,685,16,
0,388,1,62,686,
16,0,388,1,595,
687,16,0,426,1,
699,688,16,0,426,
1,374,689,16,0,
388,1,478,690,16,
0,388,1,45,691,
16,0,388,1,997,
692,16,0,426,1,
567,693,16,0,388,
1,671,694,16,0,
388,1,28,695,16,
0,388,1,772,696,
16,0,426,1,450,
697,16,0,388,1,
21,698,16,0,402,
1,14,699,16,0,
426,1,634,700,16,
0,388,1,546,701,
16,0,388,1,866,
702,16,0,388,1,
756,703,16,0,426,
1,4,704,16,0,
388,1,0,705,16,
0,426,1,148,706,
19,707,4,22,112,
0,114,0,101,0,
102,0,105,0,120,
0,101,0,120,0,
112,0,95,0,50,
0,1,148,666,1,
147,708,19,709,4,
22,112,0,114,0,
101,0,102,0,105,
0,120,0,101,0,
120,0,112,0,95,
0,49,0,1,147,
666,1,146,710,19,
711,4,28,102,0,
117,0,110,0,99,
0,116,0,105,0,
111,0,110,0,99,
0,97,0,108,0,
108,0,95,0,50,
0,1,146,712,5,
39,1,534,713,16,
0,309,1,209,714,
16,0,309,1,206,
715,16,0,309,1,
847,716,16,0,309,
1,97,717,16,0,
309,1,734,718,16,
0,309,1,840,719,
16,0,309,1,302,
720,16,0,309,1,
621,721,16,0,309,
1,939,722,16,0,
309,1,402,723,16,
0,309,1,506,724,
16,0,309,1,976,
725,16,0,309,1,
716,726,16,0,309,
1,607,727,16,0,
309,1,170,728,16,
0,309,1,69,729,
16,0,309,1,1027,
730,16,0,309,1,
812,731,16,0,309,
1,62,732,16,0,
309,1,595,733,16,
0,309,1,699,734,
16,0,309,1,374,
735,16,0,309,1,
478,736,16,0,309,
1,45,737,16,0,
309,1,997,738,16,
0,309,1,567,739,
16,0,309,1,671,
740,16,0,309,1,
28,741,16,0,309,
1,772,742,16,0,
309,1,450,743,16,
0,309,1,21,744,
16,0,309,1,14,
745,16,0,309,1,
634,746,16,0,309,
1,546,747,16,0,
309,1,866,748,16,
0,309,1,756,749,
16,0,309,1,4,
750,16,0,309,1,
0,751,16,0,309,
1,145,752,19,753,
4,28,102,0,117,
0,110,0,99,0,
116,0,105,0,111,
0,110,0,99,0,
97,0,108,0,108,
0,95,0,49,0,
1,145,712,1,144,
754,19,755,4,12,
85,0,110,0,111,
0,112,0,95,0,
49,0,1,144,756,
5,23,1,374,757,
16,0,185,1,567,
758,16,0,320,1,
812,759,16,0,206,
1,546,760,16,0,
185,1,976,761,16,
0,185,1,170,762,
16,0,260,1,28,
763,16,0,207,1,
450,764,16,0,134,
1,402,765,16,0,
185,1,634,766,16,
0,289,1,69,767,
16,0,318,1,209,
768,16,0,244,1,
302,769,16,0,378,
1,206,770,16,0,
251,1,62,771,16,
0,267,1,671,772,
16,0,263,1,506,
773,16,0,106,1,
478,774,16,0,122,
1,734,775,16,0,
185,1,4,776,16,
0,185,1,97,777,
16,0,298,1,866,
778,16,0,206,1,
45,779,16,0,378,
1,143,780,19,781,
4,14,66,0,105,
0,110,0,111,0,
112,0,95,0,49,
0,1,143,756,1,
142,782,19,783,4,
26,69,0,120,0,
112,0,84,0,97,
0,98,0,108,0,
101,0,68,0,101,
0,99,0,95,0,
49,0,1,142,756,
1,141,784,19,785,
4,10,101,0,120,
0,112,0,95,0,
50,0,1,141,756,
1,140,786,19,787,
4,10,101,0,120,
0,112,0,95,0,
49,0,1,140,756,
1,139,788,19,789,
4,12,65,0,116,
0,111,0,109,0,
95,0,53,0,1,
139,756,1,138,790,
19,791,4,12,65,
0,116,0,111,0,
109,0,95,0,52,
0,1,138,756,1,
137,792,19,793,4,
12,65,0,116,0,
111,0,109,0,95,
0,51,0,1,137,
756,1,136,794,19,
795,4,12,65,0,
116,0,111,0,109,
0,95,0,50,0,
1,136,756,1,135,
796,19,797,4,12,
65,0,116,0,111,
0,109,0,95,0,
49,0,1,135,756,
1,134,798,19,799,
4,18,101,0,120,
0,112,0,108,0,
105,0,115,0,116,
0,95,0,50,0,
1,134,800,5,6,
1,976,801,16,0,
131,1,546,802,16,
0,329,1,402,803,
16,0,177,1,4,
804,16,0,415,1,
734,805,16,0,243,
1,374,806,16,0,
174,1,133,807,19,
808,4,18,101,0,
120,0,112,0,108,
0,105,0,115,0,
116,0,95,0,49,
0,1,133,800,1,
132,809,19,810,4,
12,105,0,110,0,
105,0,116,0,95,
0,49,0,1,132,
811,5,1,1,373,
812,16,0,171,1,
131,813,19,814,4,
18,112,0,97,0,
114,0,108,0,105,
0,115,0,116,0,
95,0,50,0,1,
131,518,1,130,815,
19,816,4,18,112,
0,97,0,114,0,
108,0,105,0,115,
0,116,0,95,0,
49,0,1,130,518,
1,129,817,19,818,
4,36,116,0,97,
0,98,0,108,0,
101,0,99,0,111,
0,110,0,115,0,
116,0,114,0,117,
0,99,0,116,0,
111,0,114,0,95,
0,50,0,1,129,
819,5,27,1,374,
820,16,0,286,1,
40,821,16,0,424,
1,567,822,16,0,
286,1,812,823,16,
0,286,1,546,824,
16,0,286,1,976,
825,16,0,286,1,
33,826,16,0,424,
1,170,827,16,0,
286,1,28,828,16,
0,286,1,450,829,
16,0,286,1,402,
830,16,0,286,1,
22,831,16,0,424,
1,634,832,16,0,
286,1,69,833,16,
0,286,1,209,834,
16,0,286,1,302,
835,16,0,286,1,
206,836,16,0,286,
1,62,837,16,0,
286,1,671,838,16,
0,286,1,506,839,
16,0,286,1,478,
840,16,0,286,1,
734,841,16,0,286,
1,1,842,16,0,
424,1,4,843,16,
0,286,1,97,844,
16,0,286,1,866,
845,16,0,286,1,
45,846,16,0,286,
1,128,847,19,848,
4,36,116,0,97,
0,98,0,108,0,
101,0,99,0,111,
0,110,0,115,0,
116,0,114,0,117,
0,99,0,116,0,
111,0,114,0,95,
0,49,0,1,128,
819,1,127,849,19,
850,4,22,102,0,
105,0,101,0,108,
0,100,0,108,0,
105,0,115,0,116,
0,95,0,51,0,
1,127,851,5,2,
1,302,852,16,0,
214,1,45,853,16,
0,234,1,126,854,
19,855,4,22,102,
0,105,0,101,0,
108,0,100,0,108,
0,105,0,115,0,
116,0,95,0,50,
0,1,126,851,1,
125,856,19,222,1,
125,523,1,124,857,
19,858,4,22,102,
0,105,0,101,0,
108,0,100,0,108,
0,105,0,115,0,
116,0,95,0,49,
0,1,124,851,1,
123,859,19,860,4,
8,73,0,102,0,
95,0,49,0,1,
123,861,5,2,1,
812,862,16,0,182,
1,866,863,16,0,
194,1,122,864,19,
865,4,12,69,0,
108,0,115,0,101,
0,95,0,49,0,
1,122,861,1,121,
866,19,867,4,16,
69,0,108,0,115,
0,101,0,73,0,
102,0,95,0,49,
0,1,121,861,1,
120,868,19,869,4,
22,76,0,111,0,
99,0,97,0,108,
0,73,0,110,0,
105,0,116,0,95,
0,49,0,1,120,
870,5,15,1,756,
871,16,0,239,1,
847,872,16,0,239,
1,534,873,16,0,
239,1,595,874,16,
0,239,1,1027,875,
16,0,239,1,14,
876,16,0,239,1,
772,877,16,0,239,
1,840,878,16,0,
239,1,699,879,16,
0,239,1,997,880,
16,0,239,1,607,
881,16,0,239,1,
939,882,16,0,239,
1,716,883,16,0,
239,1,0,884,16,
0,239,1,621,885,
16,0,239,1,119,
886,19,887,4,14,
115,0,116,0,97,
0,116,0,95,0,
49,0,48,0,1,
119,870,1,118,888,
19,889,4,30,76,
0,111,0,99,0,
97,0,108,0,70,
0,117,0,110,0,
99,0,68,0,101,
0,99,0,108,0,
95,0,49,0,1,
118,870,1,117,890,
19,891,4,20,70,
0,117,0,110,0,
99,0,68,0,101,
0,99,0,108,0,
95,0,49,0,1,
117,870,1,116,892,
19,893,4,12,115,
0,116,0,97,0,
116,0,95,0,57,
0,1,116,870,1,
115,894,19,140,1,
115,529,1,114,895,
19,896,4,12,115,
0,116,0,97,0,
116,0,95,0,56,
0,1,114,870,1,
113,897,19,898,4,
12,115,0,116,0,
97,0,116,0,95,
0,55,0,1,113,
870,1,112,899,19,
900,4,12,115,0,
116,0,97,0,116,
0,95,0,54,0,
1,112,870,1,111,
901,19,902,4,16,
82,0,101,0,116,
0,118,0,97,0,
108,0,95,0,49,
0,1,111,870,1,
110,903,19,904,4,
12,115,0,116,0,
97,0,116,0,95,
0,53,0,1,110,
870,1,109,905,19,
906,4,14,83,0,
69,0,108,0,115,
0,101,0,95,0,
49,0,1,109,870,
1,108,907,19,908,
4,18,83,0,69,
0,108,0,115,0,
101,0,73,0,102,
0,95,0,49,0,
1,108,870,1,107,
909,19,910,4,10,
83,0,73,0,102,
0,95,0,49,0,
1,107,870,1,106,
911,19,912,4,12,
115,0,116,0,97,
0,116,0,95,0,
52,0,1,106,870,
1,105,913,19,914,
4,12,115,0,116,
0,97,0,116,0,
95,0,51,0,1,
105,870,1,104,915,
19,916,4,12,115,
0,116,0,97,0,
116,0,95,0,50,
0,1,104,870,1,
103,917,19,918,4,
12,115,0,116,0,
97,0,116,0,95,
0,49,0,1,103,
870,1,102,919,19,
920,4,28,70,0,
117,0,110,0,99,
0,116,0,105,0,
111,0,110,0,67,
0,97,0,108,0,
108,0,95,0,49,
0,1,102,870,1,
101,921,19,922,4,
24,65,0,115,0,
115,0,105,0,103,
0,110,0,109,0,
101,0,110,0,116,
0,95,0,49,0,
1,101,870,1,100,
923,19,924,4,14,
98,0,108,0,111,
0,99,0,107,0,
95,0,50,0,1,
100,925,5,12,1,
595,926,16,0,314,
1,847,927,16,0,
205,1,534,928,16,
0,321,1,1027,929,
16,0,112,1,14,
930,16,0,407,1,
840,931,16,0,198,
1,699,932,16,0,
257,1,997,933,16,
0,124,1,607,934,
16,0,290,1,939,
935,16,0,176,1,
716,936,16,0,249,
1,621,937,16,0,
294,1,99,938,19,
939,4,14,98,0,
108,0,111,0,99,
0,107,0,95,0,
49,0,1,99,925,
1,98,940,19,941,
4,14,99,0,104,
0,117,0,110,0,
107,0,95,0,52,
0,1,98,942,5,
15,1,756,943,16,
0,236,1,847,944,
16,0,242,1,534,
945,16,0,242,1,
595,946,16,0,242,
1,1027,947,16,0,
242,1,14,948,16,
0,242,1,772,949,
16,0,237,1,840,
950,16,0,242,1,
699,951,16,0,242,
1,997,952,16,0,
242,1,607,953,16,
0,242,1,939,954,
16,0,242,1,716,
955,16,0,242,1,
0,956,16,0,104,
1,621,957,16,0,
242,1,97,958,19,
959,4,14,99,0,
104,0,117,0,110,
0,107,0,95,0,
51,0,1,97,942,
1,96,960,19,961,
4,14,99,0,104,
0,117,0,110,0,
107,0,95,0,50,
0,1,96,942,1,
95,962,19,963,4,
14,99,0,104,0,
117,0,110,0,107,
0,95,0,49,0,
1,95,942,1,94,
964,19,213,1,94,
432,1,93,965,19,
966,4,8,69,0,
108,0,115,0,101,
0,1,93,861,1,
92,967,19,968,4,
12,69,0,108,0,
115,0,101,0,73,
0,102,0,1,92,
861,1,91,969,19,
970,4,4,73,0,
102,0,1,91,861,
1,90,971,19,184,
1,90,861,1,89,
972,19,973,4,10,
83,0,69,0,108,
0,115,0,101,0,
1,89,870,1,88,
974,19,975,4,14,
83,0,69,0,108,
0,115,0,101,0,
73,0,102,0,1,
88,870,1,87,976,
19,977,4,6,83,
0,73,0,102,0,
1,87,870,1,86,
978,19,979,4,24,
70,0,117,0,110,
0,99,0,116,0,
105,0,111,0,110,
0,67,0,97,0,
108,0,108,0,1,
86,870,1,85,980,
19,981,4,26,76,
0,111,0,99,0,
97,0,108,0,70,
0,117,0,110,0,
99,0,68,0,101,
0,99,0,108,0,
1,85,870,1,84,
982,19,983,4,16,
70,0,117,0,110,
0,99,0,68,0,
101,0,99,0,108,
0,1,84,870,1,
83,984,19,985,4,
12,82,0,101,0,
116,0,118,0,97,
0,108,0,1,83,
870,1,82,986,19,
987,4,18,76,0,
111,0,99,0,97,
0,108,0,73,0,
110,0,105,0,116,
0,1,82,870,1,
81,988,19,989,4,
20,65,0,115,0,
115,0,105,0,103,
0,110,0,109,0,
101,0,110,0,116,
0,1,81,870,1,
80,990,19,241,1,
80,870,1,79,991,
19,173,1,79,811,
1,78,992,19,197,
1,78,645,1,77,
993,19,994,4,16,
84,0,97,0,98,
0,108,0,101,0,
82,0,101,0,102,
0,1,77,599,1,
76,995,19,996,4,
20,80,0,97,0,
99,0,107,0,97,
0,103,0,101,0,
82,0,101,0,102,
0,1,76,599,1,
75,997,19,308,1,
75,599,1,74,998,
19,133,1,74,800,
1,73,999,19,1000,
4,8,65,0,116,
0,111,0,109,0,
1,73,756,1,72,
1001,19,1002,4,22,
69,0,120,0,112,
0,84,0,97,0,
98,0,108,0,101,
0,68,0,101,0,
99,0,1,72,756,
1,71,1003,19,1004,
4,8,85,0,110,
0,111,0,112,0,
1,71,756,1,70,
1005,19,1006,4,10,
66,0,105,0,110,
0,111,0,112,0,
1,70,756,1,69,
1007,19,108,1,69,
756,1,68,1008,19,
285,1,68,559,1,
67,1009,19,288,1,
67,819,1,66,1010,
19,216,1,66,851,
1,65,1011,19,1012,
4,22,70,0,105,
0,101,0,108,0,
100,0,65,0,115,
0,115,0,105,0,
103,0,110,0,1,
65,546,1,64,1013,
19,1014,4,28,70,
0,105,0,101,0,
108,0,100,0,69,
0,120,0,112,0,
65,0,115,0,115,
0,105,0,103,0,
110,0,1,64,546,
1,63,1015,19,229,
1,63,546,1,62,
1016,19,390,1,62,
666,1,61,1017,19,
152,1,61,585,1,
60,1018,19,121,1,
60,518,1,59,1019,
19,155,1,59,537,
1,58,1020,19,311,
1,58,712,1,57,
1021,19,332,1,57,
471,1,56,1022,19,
305,1,56,439,1,
55,1023,19,114,1,
55,925,1,54,1024,
19,103,1,54,942,
1,53,1025,19,297,
1,53,1026,5,2,
1,1050,1027,16,0,
295,1,13,1028,16,
0,295,1,52,1029,
19,353,1,52,1030,
5,45,1,210,1031,
16,0,351,1,207,
1032,16,0,351,1,
96,1033,17,1034,15,
1035,4,20,37,0,
112,0,114,0,101,
0,102,0,105,0,
120,0,101,0,120,
0,112,0,1,-1,
1,5,1036,20,709,
1,147,1,3,1,
2,1,1,1037,22,
1,52,1,95,1038,
17,1039,15,1035,1,
-1,1,5,1040,20,
707,1,148,1,3,
1,2,1,1,1041,
22,1,53,1,92,
1042,17,1043,15,1035,
1,-1,1,5,1044,
20,665,1,149,1,
3,1,4,1,3,
1045,22,1,54,1,
517,1046,16,0,351,
1,298,1047,17,1048,
15,1049,4,34,37,
0,116,0,97,0,
98,0,108,0,101,
0,99,0,111,0,
110,0,115,0,116,
0,114,0,117,0,
99,0,116,0,111,
0,114,0,1,-1,
1,5,1050,20,848,
1,128,1,3,1,
3,1,2,1051,22,
1,32,1,296,1052,
17,1053,15,1049,1,
-1,1,5,1054,20,
818,1,129,1,3,
1,4,1,3,1055,
22,1,33,1,76,
1056,16,0,351,1,
823,1057,16,0,351,
1,171,1058,16,0,
351,1,1030,1059,17,
1060,15,1061,4,18,
37,0,102,0,117,
0,110,0,99,0,
98,0,111,0,100,
0,121,0,1,-1,
1,5,1062,20,592,
1,157,1,3,1,
6,1,5,1063,22,
1,66,1,1028,1064,
17,1065,15,1061,1,
-1,1,5,1066,20,
590,1,158,1,3,
1,5,1,4,1067,
22,1,67,1,385,
1068,16,0,351,1,
169,1069,17,1070,15,
1071,4,8,37,0,
118,0,97,0,114,
0,1,-1,1,5,
1072,20,642,1,152,
1,3,1,2,1,
1,1073,22,1,59,
1,489,1074,16,0,
351,1,46,1075,16,
0,351,1,44,1076,
17,1077,15,1078,4,
26,37,0,102,0,
117,0,110,0,99,
0,116,0,105,0,
111,0,110,0,99,
0,97,0,108,0,
108,0,1,-1,1,
5,1079,20,711,1,
146,1,3,1,5,
1,4,1080,22,1,
51,1,578,1081,16,
0,351,1,682,1082,
16,0,351,1,144,
1083,16,0,351,1,
33,1084,17,1085,15,
1086,4,8,37,0,
101,0,120,0,112,
0,1,-1,1,5,
1087,20,785,1,141,
1,3,1,2,1,
1,1088,22,1,46,
1,461,1089,16,0,
351,1,129,1090,17,
1091,15,1092,4,10,
37,0,65,0,116,
0,111,0,109,0,
1,-1,1,5,1093,
20,793,1,137,1,
3,1,2,1,1,
1094,22,1,42,1,
27,1095,17,1096,15,
1097,4,22,37,0,
80,0,97,0,99,
0,107,0,97,0,
103,0,101,0,82,
0,101,0,102,0,
1,-1,1,5,1098,
20,598,1,154,1,
3,1,4,1,3,
1099,22,1,61,1,
20,1100,17,1034,1,
1,1037,1,19,1101,
17,1070,1,1,1073,
1,131,1102,17,1103,
15,1092,1,-1,1,
5,1104,20,797,1,
135,1,3,1,2,
1,1,1105,22,1,
40,1,130,1106,17,
1107,15,1092,1,-1,
1,5,1108,20,795,
1,136,1,3,1,
2,1,1,1109,22,
1,41,1,343,1110,
17,1111,15,1112,4,
18,37,0,84,0,
97,0,98,0,108,
0,101,0,82,0,
101,0,102,0,1,
-1,1,5,1113,20,
640,1,153,1,3,
1,5,1,4,1114,
22,1,60,1,128,
1115,17,1116,15,1092,
1,-1,1,5,1117,
20,791,1,138,1,
3,1,2,1,1,
1118,22,1,43,1,
127,1119,17,1120,15,
1092,1,-1,1,5,
1121,20,789,1,139,
1,3,1,2,1,
1,1122,22,1,44,
1,126,1123,17,1124,
15,1086,1,-1,1,
5,1125,20,787,1,
140,1,3,1,2,
1,1,1126,22,1,
45,1,125,1127,17,
1128,15,1129,4,24,
37,0,69,0,120,
0,112,0,84,0,
97,0,98,0,108,
0,101,0,68,0,
101,0,99,0,1,
-1,1,5,1130,20,
783,1,142,1,3,
1,2,1,1,1131,
22,1,47,1,17,
1132,17,1133,15,1061,
1,-1,1,5,1134,
20,594,1,156,1,
3,1,5,1,4,
1135,22,1,65,1,
15,1136,17,1137,15,
1061,1,-1,1,5,
1138,20,584,1,159,
1,3,1,4,1,
3,1139,22,1,68,
1,12,1140,17,1141,
15,1142,4,18,37,
0,102,0,117,0,
110,0,99,0,116,
0,105,0,111,0,
110,0,1,-1,1,
5,1143,20,558,1,
160,1,3,1,3,
1,2,1144,22,1,
69,1,10,1145,17,
1146,15,1147,4,8,
37,0,97,0,114,
0,103,0,1,-1,
1,5,212,1,2,
1,2,1148,22,1,
70,1,9,1149,17,
1150,15,1147,1,-1,
1,5,1151,20,556,
1,161,1,3,1,
4,1,3,1152,22,
1,71,1,327,1153,
16,0,351,1,3,
1154,17,1155,15,1147,
1,-1,1,5,1156,
20,554,1,162,1,
3,1,2,1,1,
1157,22,1,72,1,
325,1158,17,1159,15,
1078,1,-1,1,5,
1160,20,753,1,145,
1,3,1,3,1,
2,1161,22,1,50,
1,645,1162,16,0,
351,1,2,1163,17,
1164,15,1147,1,-1,
1,5,212,1,1,
1,1,1165,22,1,
73,1,107,1166,16,
0,351,1,51,1167,
19,273,1,51,1168,
5,43,1,209,1169,
16,0,271,1,634,
1170,16,0,271,1,
97,1171,16,0,271,
1,734,1172,16,0,
271,1,812,1173,16,
0,271,1,302,1174,
16,0,271,1,301,
1175,17,1176,15,1177,
4,18,37,0,102,
0,105,0,101,0,
108,0,100,0,115,
0,101,0,112,0,
1,-1,1,5,221,
1,1,1,1,1178,
22,1,92,1,300,
1179,17,1180,15,1177,
1,-1,1,5,221,
1,1,1,1,1181,
22,1,93,1,402,
1182,16,0,271,1,
506,1183,16,0,271,
1,69,1184,16,0,
271,1,374,1185,16,
0,271,1,50,1186,
17,1187,15,1188,4,
12,37,0,98,0,
105,0,110,0,111,
0,112,0,1,-1,
1,5,331,1,1,
1,1,1189,22,1,
88,1,170,1190,16,
0,271,1,62,1191,
16,0,271,1,61,
1192,17,1193,15,1188,
1,-1,1,5,331,
1,1,1,1,1194,
22,1,77,1,60,
1195,17,1196,15,1188,
1,-1,1,5,331,
1,1,1,1,1197,
22,1,78,1,59,
1198,17,1199,15,1188,
1,-1,1,5,331,
1,1,1,1,1200,
22,1,79,1,58,
1201,17,1202,15,1188,
1,-1,1,5,331,
1,1,1,1,1203,
22,1,80,1,57,
1204,17,1205,15,1188,
1,-1,1,5,331,
1,1,1,1,1206,
22,1,81,1,56,
1207,17,1208,15,1188,
1,-1,1,5,331,
1,1,1,1,1209,
22,1,82,1,55,
1210,17,1211,15,1188,
1,-1,1,5,331,
1,1,1,1,1212,
22,1,83,1,54,
1213,17,1214,15,1188,
1,-1,1,5,331,
1,1,1,1,1215,
22,1,84,1,53,
1216,17,1217,15,1188,
1,-1,1,5,331,
1,1,1,1,1218,
22,1,85,1,52,
1219,17,1220,15,1188,
1,-1,1,5,331,
1,1,1,1,1221,
22,1,86,1,51,
1222,17,1223,15,1188,
1,-1,1,5,331,
1,1,1,1,1224,
22,1,87,1,478,
1225,16,0,271,1,
49,1226,17,1227,15,
1188,1,-1,1,5,
331,1,1,1,1,
1228,22,1,89,1,
48,1229,17,1230,15,
1188,1,-1,1,5,
331,1,1,1,1,
1231,22,1,90,1,
47,1232,17,1233,15,
1188,1,-1,1,5,
331,1,1,1,1,
1234,22,1,91,1,
45,1235,16,0,271,
1,567,1236,16,0,
271,1,671,1237,16,
0,271,1,28,1238,
16,0,271,1,450,
1239,16,0,271,1,
976,1240,16,0,271,
1,546,1241,16,0,
271,1,866,1242,16,
0,271,1,7,1243,
17,1244,15,1245,4,
10,37,0,117,0,
110,0,111,0,112,
0,1,-1,1,5,
304,1,1,1,1,
1246,22,1,74,1,
6,1247,17,1248,15,
1245,1,-1,1,5,
304,1,1,1,1,
1249,22,1,75,1,
5,1250,17,1251,15,
1245,1,-1,1,5,
304,1,1,1,1,
1252,22,1,76,1,
4,1253,16,0,271,
1,206,1254,16,0,
271,1,50,1255,19,
276,1,50,1256,5,
43,1,209,1257,16,
0,274,1,634,1258,
16,0,274,1,97,
1259,16,0,274,1,
734,1260,16,0,274,
1,812,1261,16,0,
274,1,302,1262,16,
0,274,1,301,1175,
1,300,1179,1,402,
1263,16,0,274,1,
506,1264,16,0,274,
1,69,1265,16,0,
274,1,374,1266,16,
0,274,1,50,1186,
1,170,1267,16,0,
274,1,62,1268,16,
0,274,1,61,1192,
1,60,1195,1,59,
1198,1,58,1201,1,
57,1204,1,56,1207,
1,55,1210,1,54,
1213,1,53,1216,1,
52,1219,1,51,1222,
1,478,1269,16,0,
274,1,49,1226,1,
48,1229,1,47,1232,
1,45,1270,16,0,
274,1,567,1271,16,
0,274,1,671,1272,
16,0,274,1,28,
1273,16,0,274,1,
450,1274,16,0,274,
1,976,1275,16,0,
274,1,546,1276,16,
0,274,1,866,1277,
16,0,274,1,7,
1243,1,6,1247,1,
5,1250,1,4,1278,
16,0,274,1,206,
1279,16,0,274,1,
49,1280,19,193,1,
49,1281,5,71,1,
534,1282,16,0,191,
1,619,1283,17,1284,
15,1285,4,12,37,
0,83,0,69,0,
108,0,115,0,101,
0,1,-1,1,5,
1286,20,906,1,109,
1,3,1,8,1,
7,1287,22,1,15,
1,421,1288,17,1289,
15,1290,4,16,37,
0,101,0,120,0,
112,0,108,0,105,
0,115,0,116,0,
1,-1,1,5,1291,
20,808,1,133,1,
3,1,4,1,3,
1292,22,1,38,1,
847,1293,16,0,191,
1,96,1033,1,95,
1038,1,949,1294,17,
1295,15,1296,4,10,
37,0,115,0,116,
0,97,0,116,0,
1,-1,1,5,1297,
20,898,1,113,1,
3,1,10,1,9,
1298,22,1,19,1,
92,1042,1,840,1299,
16,0,191,1,731,
1300,17,1301,15,1296,
1,-1,1,5,1302,
20,918,1,103,1,
3,1,4,1,3,
1303,22,1,9,1,
621,1304,16,0,191,
1,298,1047,1,939,
1305,16,0,191,1,
296,1052,1,373,1306,
17,1307,15,1296,1,
-1,1,5,1308,20,
887,1,119,1,3,
1,3,1,2,1309,
22,1,24,1,716,
1310,16,0,191,1,
715,1311,17,1312,15,
1296,1,-1,1,5,
1313,20,916,1,104,
1,3,1,6,1,
5,1314,22,1,10,
1,607,1315,16,0,
191,1,712,1316,17,
1317,15,1296,1,-1,
1,5,1318,20,914,
1,105,1,3,1,
5,1,4,1319,22,
1,11,1,1030,1059,
1,1028,1064,1,385,
1320,17,1321,15,1290,
1,-1,1,5,1322,
20,799,1,134,1,
3,1,2,1,1,
1323,22,1,39,1,
169,1069,1,595,1324,
16,0,191,1,699,
1325,16,0,191,1,
909,1326,17,1327,15,
1328,4,8,37,0,
83,0,73,0,102,
0,1,-1,1,5,
1329,20,910,1,107,
1,3,1,6,1,
5,1330,22,1,13,
1,908,1331,17,1332,
15,1333,4,16,37,
0,83,0,69,0,
108,0,115,0,101,
0,73,0,102,0,
1,-1,1,5,1334,
20,908,1,108,1,
3,1,8,1,7,
1335,22,1,14,1,
371,1336,17,1337,15,
1338,4,18,37,0,
110,0,97,0,109,
0,101,0,108,0,
105,0,115,0,116,
0,1,-1,1,5,
139,1,3,1,3,
1339,22,1,56,1,
369,1340,17,1341,15,
1338,1,-1,1,5,
139,1,1,1,1,
1342,22,1,55,1,
44,1076,1,1006,1343,
17,1344,15,1296,1,
-1,1,5,1345,20,
893,1,116,1,3,
1,8,1,7,1346,
22,1,21,1,3,
1154,1,126,1123,1,
144,1347,17,1348,15,
1349,4,12,37,0,
66,0,105,0,110,
0,111,0,112,0,
1,-1,1,5,1350,
20,781,1,143,1,
3,1,4,1,3,
1351,22,1,48,1,
33,1084,1,19,1101,
1,997,1352,16,0,
191,1,125,1127,1,
0,1353,16,0,191,
1,566,1354,17,1355,
15,1356,4,14,37,
0,82,0,101,0,
116,0,118,0,97,
0,108,0,1,-1,
1,5,1357,20,902,
1,111,1,3,1,
3,1,2,1358,22,
1,17,1,127,1119,
1,130,1106,1,129,
1090,1,27,1095,1,
20,1100,1,1027,1359,
16,0,191,1,131,
1102,1,772,1360,16,
0,191,1,343,1110,
1,128,1115,1,448,
1361,17,1362,15,1338,
1,-1,1,5,139,
1,1,1,1,1342,
1,432,1363,17,1364,
15,1365,4,20,37,
0,76,0,111,0,
99,0,97,0,108,
0,73,0,110,0,
105,0,116,0,1,
-1,1,5,1366,20,
869,1,120,1,3,
1,4,1,3,1367,
22,1,25,1,446,
1368,17,1369,15,1370,
4,18,37,0,70,
0,117,0,110,0,
99,0,68,0,101,
0,99,0,108,0,
1,-1,1,5,1371,
20,891,1,117,1,
3,1,4,1,3,
1372,22,1,22,1,
17,1132,1,10,1145,
1,15,1136,1,14,
1373,16,0,191,1,
9,1149,1,12,1140,
1,546,1374,17,1375,
15,1296,1,-1,1,
5,1376,20,904,1,
110,1,3,1,2,
1,1,1377,22,1,
16,1,545,1378,17,
1379,15,1296,1,-1,
1,5,1380,20,900,
1,112,1,3,1,
2,1,1,1381,22,
1,18,1,544,1382,
17,1383,15,1296,1,
-1,1,5,1384,20,
896,1,114,1,3,
1,12,1,11,1385,
22,1,20,1,436,
1386,17,1387,15,1388,
4,28,37,0,76,
0,111,0,99,0,
97,0,108,0,70,
0,117,0,110,0,
99,0,68,0,101,
0,99,0,108,0,
1,-1,1,5,1389,
20,889,1,118,1,
3,1,5,1,4,
1390,22,1,23,1,
756,1391,16,0,191,
1,2,1163,1,754,
1392,17,1393,15,1394,
4,22,37,0,65,
0,115,0,115,0,
105,0,103,0,110,
0,109,0,101,0,
110,0,116,0,1,
-1,1,5,1395,20,
922,1,101,1,3,
1,4,1,3,1396,
22,1,7,1,325,
1158,1,645,1397,17,
1398,15,1296,1,-1,
1,5,1399,20,912,
1,106,1,3,1,
5,1,4,1400,22,
1,12,1,430,1401,
17,1402,15,1403,4,
10,37,0,105,0,
110,0,105,0,116,
0,1,-1,1,5,
1404,20,810,1,132,
1,3,1,3,1,
2,1405,22,1,37,
1,1,1406,17,1407,
15,1408,4,26,37,
0,70,0,117,0,
110,0,99,0,116,
0,105,0,111,0,
110,0,67,0,97,
0,108,0,108,0,
1,-1,1,5,1409,
20,920,1,102,1,
3,1,2,1,1,
1410,22,1,8,1,
107,1411,17,1412,15,
1413,4,10,37,0,
85,0,110,0,111,
0,112,0,1,-1,
1,5,1414,20,755,
1,144,1,3,1,
3,1,2,1415,22,
1,49,1,48,1416,
19,397,1,48,1417,
5,71,1,534,1418,
16,0,395,1,619,
1283,1,421,1288,1,
847,1419,16,0,395,
1,96,1033,1,95,
1038,1,949,1294,1,
92,1042,1,840,1420,
16,0,395,1,731,
1300,1,621,1421,16,
0,395,1,298,1047,
1,939,1422,16,0,
395,1,296,1052,1,
373,1306,1,716,1423,
16,0,395,1,715,
1311,1,607,1424,16,
0,395,1,712,1316,
1,1030,1059,1,1028,
1064,1,385,1320,1,
169,1069,1,595,1425,
16,0,395,1,699,
1426,16,0,395,1,
909,1326,1,908,1331,
1,371,1336,1,369,
1340,1,44,1076,1,
1006,1343,1,3,1154,
1,126,1123,1,144,
1347,1,33,1084,1,
19,1101,1,997,1427,
16,0,395,1,125,
1127,1,0,1428,16,
0,395,1,566,1354,
1,127,1119,1,130,
1106,1,129,1090,1,
27,1095,1,20,1100,
1,1027,1429,16,0,
395,1,131,1102,1,
772,1430,16,0,395,
1,343,1110,1,128,
1115,1,448,1361,1,
432,1363,1,446,1368,
1,17,1132,1,10,
1145,1,15,1136,1,
14,1431,16,0,395,
1,9,1149,1,12,
1140,1,546,1374,1,
545,1378,1,544,1382,
1,436,1386,1,756,
1432,16,0,395,1,
2,1163,1,754,1392,
1,325,1158,1,645,
1397,1,430,1401,1,
1,1406,1,107,1411,
1,47,1433,19,293,
1,47,1434,5,74,
1,534,1435,17,1436,
15,1437,4,12,37,
0,98,0,108,0,
111,0,99,0,107,
0,1,-1,1,5,
1438,20,924,1,100,
1,3,1,1,1,
0,1439,22,1,6,
1,619,1283,1,421,
1288,1,847,1440,17,
1436,1,0,1439,1,
96,1033,1,95,1038,
1,949,1294,1,92,
1042,1,840,1441,17,
1436,1,0,1439,1,
731,1300,1,621,1442,
17,1436,1,0,1439,
1,298,1047,1,939,
1443,17,1436,1,0,
1439,1,296,1052,1,
373,1306,1,716,1444,
17,1436,1,0,1439,
1,715,1311,1,607,
1445,17,1436,1,0,
1439,1,712,1316,1,
1030,1059,1,1028,1064,
1,385,1320,1,169,
1069,1,595,1446,17,
1436,1,0,1439,1,
699,1447,17,1436,1,
0,1439,1,909,1326,
1,908,1331,1,371,
1336,1,369,1340,1,
44,1076,1,126,1123,
1,1006,1343,1,791,
1448,17,1449,15,1450,
4,12,37,0,99,
0,104,0,117,0,
110,0,107,0,1,
-1,1,5,1451,20,
959,1,97,1,3,
1,3,1,2,1452,
22,1,3,1,3,
1154,1,788,1453,17,
1454,15,1450,1,-1,
1,5,1455,20,941,
1,98,1,3,1,
4,1,3,1456,22,
1,4,1,2,1163,
1,144,1347,1,33,
1084,1,19,1101,1,
997,1457,17,1436,1,
0,1439,1,125,1127,
1,10,1145,1,566,
1354,1,127,1119,1,
130,1106,1,129,1090,
1,27,1095,1,20,
1100,1,1027,1458,17,
1436,1,0,1439,1,
131,1102,1,772,1459,
17,1460,15,1450,1,
-1,1,5,1461,20,
961,1,96,1,3,
1,3,1,2,1462,
22,1,2,1,343,
1110,1,128,1115,1,
448,1361,1,432,1363,
1,446,1368,1,17,
1132,1,12,1140,1,
15,1136,1,14,1463,
17,1436,1,0,1439,
1,9,1149,1,633,
1464,16,0,291,1,
546,1374,1,545,1378,
1,544,1382,1,436,
1386,1,756,1465,17,
1466,15,1450,1,-1,
1,5,1467,20,963,
1,95,1,3,1,
2,1,1,1468,22,
1,1,1,755,1469,
17,1470,15,1437,1,
-1,1,5,1471,20,
939,1,99,1,3,
1,2,1,1,1472,
22,1,5,1,754,
1392,1,325,1158,1,
645,1397,1,430,1401,
1,1,1406,1,107,
1411,1,46,1473,19,
301,1,46,1474,5,
71,1,534,1475,16,
0,299,1,619,1283,
1,421,1288,1,847,
1476,16,0,299,1,
96,1033,1,95,1038,
1,949,1294,1,92,
1042,1,840,1477,16,
0,299,1,731,1300,
1,621,1478,16,0,
299,1,298,1047,1,
939,1479,16,0,299,
1,296,1052,1,373,
1306,1,716,1480,16,
0,299,1,715,1311,
1,607,1481,16,0,
299,1,712,1316,1,
1030,1059,1,1028,1064,
1,385,1320,1,169,
1069,1,595,1482,16,
0,299,1,699,1483,
16,0,299,1,909,
1326,1,908,1331,1,
371,1336,1,369,1340,
1,44,1076,1,1006,
1343,1,3,1154,1,
126,1123,1,144,1347,
1,33,1084,1,19,
1101,1,997,1484,16,
0,299,1,125,1127,
1,0,1485,16,0,
299,1,566,1354,1,
127,1119,1,130,1106,
1,129,1090,1,27,
1095,1,20,1100,1,
1027,1486,16,0,299,
1,131,1102,1,772,
1487,16,0,299,1,
343,1110,1,128,1115,
1,448,1361,1,432,
1363,1,446,1368,1,
17,1132,1,10,1145,
1,15,1136,1,14,
1488,16,0,299,1,
9,1149,1,12,1140,
1,546,1374,1,545,
1378,1,544,1382,1,
436,1386,1,756,1489,
16,0,299,1,2,
1163,1,754,1392,1,
325,1158,1,645,1397,
1,430,1401,1,1,
1406,1,107,1411,1,
45,1490,19,167,1,
45,1491,5,114,1,
716,1492,16,0,165,
1,715,1311,1,712,
1316,1,949,1294,1,
939,1493,16,0,165,
1,908,1331,1,209,
1494,16,0,412,1,
671,1495,16,0,412,
1,450,1496,16,0,
412,1,448,1361,1,
446,1368,1,206,1497,
16,0,412,1,436,
1386,1,432,1363,1,
909,1326,1,430,1401,
1,607,1498,16,0,
165,1,421,1288,1,
634,1499,16,0,412,
1,170,1500,16,0,
412,1,169,1069,1,
645,1397,1,402,1501,
16,0,412,1,847,
1502,16,0,165,1,
369,1340,1,866,1503,
16,0,412,1,385,
1320,1,144,1347,1,
621,1504,16,0,165,
1,368,1505,16,0,
170,1,619,1283,1,
374,1506,16,0,412,
1,373,1306,1,371,
1336,1,131,1102,1,
130,1106,1,129,1090,
1,128,1115,1,127,
1119,1,126,1123,1,
125,1127,1,840,1507,
16,0,165,1,595,
1508,16,0,165,1,
534,1509,16,0,165,
1,107,1411,1,343,
1110,1,95,1038,1,
97,1510,16,0,412,
1,96,1033,1,812,
1511,16,0,412,1,
92,1042,1,567,1512,
16,0,412,1,566,
1354,1,325,1158,1,
544,1382,1,300,1179,
1,1030,1059,1,301,
1175,1,1028,1064,1,
1027,1513,16,0,165,
1,298,1047,1,69,
1514,16,0,412,1,
546,1515,16,0,412,
1,545,1378,1,57,
1204,1,50,1186,1,
55,1210,1,302,1516,
16,0,412,1,62,
1517,16,0,412,1,
61,1192,1,60,1195,
1,59,1198,1,58,
1201,1,296,1052,1,
56,1207,1,772,1518,
16,0,165,1,54,
1213,1,53,1216,1,
52,1219,1,51,1222,
1,1006,1343,1,49,
1226,1,48,1229,1,
47,1232,1,45,1519,
16,0,412,1,44,
1076,1,997,1520,16,
0,165,1,756,1521,
16,0,165,1,754,
1392,1,33,1084,1,
28,1522,16,0,412,
1,506,1523,16,0,
412,1,27,1095,1,
20,1100,1,17,1132,
1,976,1524,16,0,
412,1,19,1101,1,
734,1525,16,0,412,
1,14,1526,16,0,
165,1,15,1136,1,
731,1300,1,12,1140,
1,5,1250,1,10,
1145,1,9,1149,1,
0,1527,16,0,165,
1,7,1243,1,6,
1247,1,699,1528,16,
0,165,1,4,1529,
16,0,412,1,3,
1154,1,2,1163,1,
1,1406,1,478,1530,
16,0,412,1,44,
1531,19,270,1,44,
1532,5,43,1,209,
1533,16,0,268,1,
634,1534,16,0,268,
1,97,1535,16,0,
268,1,734,1536,16,
0,268,1,812,1537,
16,0,268,1,302,
1538,16,0,268,1,
301,1175,1,300,1179,
1,402,1539,16,0,
268,1,506,1540,16,
0,268,1,69,1541,
16,0,268,1,374,
1542,16,0,268,1,
50,1186,1,170,1543,
16,0,268,1,62,
1544,16,0,268,1,
61,1192,1,60,1195,
1,59,1198,1,58,
1201,1,57,1204,1,
56,1207,1,55,1210,
1,54,1213,1,53,
1216,1,52,1219,1,
51,1222,1,478,1545,
16,0,268,1,49,
1226,1,48,1229,1,
47,1232,1,45,1546,
16,0,268,1,567,
1547,16,0,268,1,
671,1548,16,0,268,
1,28,1549,16,0,
268,1,450,1550,16,
0,268,1,976,1551,
16,0,268,1,546,
1552,16,0,268,1,
866,1553,16,0,268,
1,7,1243,1,6,
1247,1,5,1250,1,
4,1554,16,0,268,
1,206,1555,16,0,
268,1,43,1556,19,
400,1,43,1557,5,
71,1,534,1558,16,
0,398,1,619,1283,
1,421,1288,1,847,
1559,16,0,398,1,
96,1033,1,95,1038,
1,949,1294,1,92,
1042,1,840,1560,16,
0,398,1,731,1300,
1,621,1561,16,0,
398,1,298,1047,1,
939,1562,16,0,398,
1,296,1052,1,373,
1306,1,716,1563,16,
0,398,1,715,1311,
1,607,1564,16,0,
398,1,712,1316,1,
1030,1059,1,1028,1064,
1,385,1320,1,169,
1069,1,595,1565,16,
0,398,1,699,1566,
16,0,398,1,909,
1326,1,908,1331,1,
371,1336,1,369,1340,
1,44,1076,1,1006,
1343,1,3,1154,1,
126,1123,1,144,1347,
1,33,1084,1,19,
1101,1,997,1567,16,
0,398,1,125,1127,
1,0,1568,16,0,
398,1,566,1354,1,
127,1119,1,130,1106,
1,129,1090,1,27,
1095,1,20,1100,1,
1027,1569,16,0,398,
1,131,1102,1,772,
1570,16,0,398,1,
343,1110,1,128,1115,
1,448,1361,1,432,
1363,1,446,1368,1,
17,1132,1,10,1145,
1,15,1136,1,14,
1571,16,0,398,1,
9,1149,1,12,1140,
1,546,1374,1,545,
1378,1,544,1382,1,
436,1386,1,756,1572,
16,0,398,1,2,
1163,1,754,1392,1,
325,1158,1,645,1397,
1,430,1401,1,1,
1406,1,107,1411,1,
42,1573,19,137,1,
42,1574,5,4,1,
448,1361,1,975,1575,
16,0,135,1,369,
1340,1,371,1336,1,
41,1576,19,130,1,
41,1577,5,75,1,
534,1578,16,0,255,
1,619,1283,1,421,
1288,1,847,1579,16,
0,255,1,96,1033,
1,95,1038,1,949,
1294,1,92,1042,1,
840,1580,16,0,255,
1,517,1581,16,0,
391,1,621,1582,16,
0,255,1,298,1047,
1,939,1583,16,0,
255,1,296,1052,1,
373,1306,1,716,1584,
16,0,255,1,715,
1311,1,607,1585,16,
0,255,1,712,1316,
1,1030,1059,1,1028,
1064,1,385,1320,1,
169,1069,1,489,1586,
16,0,178,1,595,
1587,16,0,255,1,
699,1588,16,0,255,
1,909,1326,1,908,
1331,1,371,1336,1,
369,1340,1,44,1076,
1,1006,1343,1,0,
1589,16,0,255,1,
3,1154,1,682,1590,
16,0,259,1,126,
1123,1,125,1127,1,
144,1347,1,33,1084,
1,19,1101,1,997,
1591,16,0,255,1,
996,1592,16,0,128,
1,2,1163,1,566,
1354,1,127,1119,1,
130,1106,1,129,1090,
1,27,1095,1,20,
1100,1,1027,1593,16,
0,255,1,131,1102,
1,772,1594,16,0,
255,1,343,1110,1,
128,1115,1,448,1361,
1,432,1363,1,446,
1368,1,17,1132,1,
10,1145,1,15,1136,
1,14,1595,16,0,
255,1,9,1149,1,
12,1140,1,546,1374,
1,545,1378,1,544,
1382,1,436,1386,1,
756,1596,16,0,255,
1,731,1300,1,754,
1392,1,325,1158,1,
645,1397,1,430,1401,
1,1,1406,1,107,
1411,1,40,1597,19,
149,1,40,1598,5,
71,1,534,1599,16,
0,147,1,619,1283,
1,421,1288,1,847,
1600,16,0,147,1,
96,1033,1,95,1038,
1,949,1294,1,92,
1042,1,840,1601,16,
0,147,1,731,1300,
1,621,1602,16,0,
147,1,298,1047,1,
939,1603,16,0,147,
1,296,1052,1,373,
1306,1,716,1604,16,
0,147,1,715,1311,
1,607,1605,16,0,
147,1,712,1316,1,
1030,1059,1,1028,1064,
1,385,1320,1,169,
1069,1,595,1606,16,
0,147,1,699,1607,
16,0,147,1,909,
1326,1,908,1331,1,
371,1336,1,369,1340,
1,44,1076,1,1006,
1343,1,3,1154,1,
126,1123,1,144,1347,
1,33,1084,1,19,
1101,1,997,1608,16,
0,147,1,125,1127,
1,0,1609,16,0,
147,1,566,1354,1,
127,1119,1,130,1106,
1,129,1090,1,27,
1095,1,20,1100,1,
1027,1610,16,0,147,
1,131,1102,1,772,
1611,16,0,147,1,
343,1110,1,128,1115,
1,448,1361,1,432,
1363,1,446,1368,1,
17,1132,1,10,1145,
1,15,1136,1,14,
1612,16,0,147,1,
9,1149,1,12,1140,
1,546,1374,1,545,
1378,1,544,1382,1,
436,1386,1,756,1613,
16,0,147,1,2,
1163,1,754,1392,1,
325,1158,1,645,1397,
1,430,1401,1,1,
1406,1,107,1411,1,
39,1614,19,266,1,
39,1615,5,71,1,
534,1616,16,0,264,
1,619,1283,1,421,
1288,1,847,1617,16,
0,264,1,96,1033,
1,95,1038,1,949,
1294,1,92,1042,1,
840,1618,16,0,264,
1,731,1300,1,621,
1619,16,0,264,1,
298,1047,1,939,1620,
16,0,264,1,296,
1052,1,373,1306,1,
716,1621,16,0,264,
1,715,1311,1,607,
1622,16,0,264,1,
712,1316,1,1030,1059,
1,1028,1064,1,385,
1320,1,169,1069,1,
595,1623,16,0,264,
1,699,1624,16,0,
264,1,909,1326,1,
908,1331,1,371,1336,
1,369,1340,1,44,
1076,1,1006,1343,1,
3,1154,1,126,1123,
1,144,1347,1,33,
1084,1,19,1101,1,
997,1625,16,0,264,
1,125,1127,1,0,
1626,16,0,264,1,
566,1354,1,127,1119,
1,130,1106,1,129,
1090,1,27,1095,1,
20,1100,1,1027,1627,
16,0,264,1,131,
1102,1,772,1628,16,
0,264,1,343,1110,
1,128,1115,1,448,
1361,1,432,1363,1,
446,1368,1,17,1132,
1,10,1145,1,15,
1136,1,14,1629,16,
0,264,1,9,1149,
1,12,1140,1,546,
1374,1,545,1378,1,
544,1382,1,436,1386,
1,756,1630,16,0,
264,1,2,1163,1,
754,1392,1,325,1158,
1,645,1397,1,430,
1401,1,1,1406,1,
107,1411,1,38,1631,
19,111,1,38,1632,
5,86,1,716,1444,
1,715,1311,1,714,
1633,16,0,256,1,
712,1316,1,949,1294,
1,948,1634,16,0,
175,1,939,1443,1,
908,1331,1,448,1361,
1,446,1368,1,436,
1386,1,432,1363,1,
909,1326,1,430,1401,
1,907,1635,16,0,
181,1,421,1288,1,
169,1069,1,886,1636,
17,1637,15,1638,4,
14,37,0,69,0,
108,0,115,0,101,
0,73,0,102,0,
1,-1,1,5,1639,
20,867,1,121,1,
3,1,6,1,5,
1640,22,1,26,1,
645,1397,1,621,1442,
1,847,1440,1,846,
1641,17,1642,15,1643,
4,6,37,0,73,
0,102,0,1,-1,
1,5,1644,20,860,
1,123,1,3,1,
4,1,3,1645,22,
1,28,1,607,1445,
1,606,1646,16,0,
180,1,385,1320,1,
144,1347,1,369,1340,
1,619,1283,1,618,
1647,16,0,302,1,
853,1648,17,1649,15,
1650,4,10,37,0,
69,0,108,0,115,
0,101,0,1,-1,
1,5,1651,20,865,
1,122,1,3,1,
6,1,5,1652,22,
1,27,1,373,1306,
1,371,1336,1,131,
1102,1,130,1106,1,
129,1090,1,128,1115,
1,127,1119,1,126,
1123,1,125,1127,1,
840,1441,1,595,1446,
1,107,1411,1,343,
1110,1,791,1448,1,
788,1453,1,96,1033,
1,95,1038,1,92,
1042,1,566,1354,1,
325,1158,1,1030,1059,
1,1029,1653,16,0,
109,1,1028,1064,1,
1027,1654,16,0,115,
1,546,1374,1,545,
1378,1,544,1382,1,
543,1655,16,0,401,
1,298,1047,1,296,
1052,1,534,1435,1,
772,1459,1,1006,1343,
1,1005,1656,16,0,
123,1,44,1076,1,
997,1457,1,756,1465,
1,755,1469,1,754,
1392,1,33,1084,1,
27,1095,1,20,1100,
1,19,1101,1,14,
1657,16,0,408,1,
17,1132,1,16,1658,
16,0,406,1,15,
1136,1,731,1300,1,
730,1659,16,0,248,
1,12,1140,1,10,
1145,1,9,1149,1,
699,1660,16,0,258,
1,3,1154,1,2,
1163,1,1,1406,1,
37,1661,19,201,1,
37,1662,5,75,1,
534,1435,1,619,1283,
1,421,1288,1,847,
1440,1,846,1663,16,
0,199,1,96,1033,
1,95,1038,1,949,
1294,1,92,1042,1,
840,1441,1,731,1300,
1,621,1442,1,298,
1047,1,939,1443,1,
296,1052,1,373,1306,
1,716,1444,1,715,
1311,1,607,1445,1,
606,1664,16,0,235,
1,712,1316,1,1030,
1059,1,1028,1064,1,
385,1320,1,169,1069,
1,595,1446,1,699,
1447,1,909,1326,1,
908,1331,1,371,1336,
1,369,1340,1,44,
1076,1,126,1123,1,
1006,1343,1,791,1448,
1,3,1154,1,788,
1453,1,144,1347,1,
33,1084,1,19,1101,
1,997,1457,1,125,
1127,1,2,1163,1,
566,1354,1,127,1119,
1,130,1106,1,129,
1090,1,27,1095,1,
20,1100,1,1027,1458,
1,131,1102,1,772,
1459,1,343,1110,1,
128,1115,1,448,1361,
1,432,1363,1,446,
1368,1,17,1132,1,
10,1145,1,15,1136,
1,14,1463,1,9,
1149,1,12,1140,1,
546,1374,1,545,1378,
1,544,1382,1,436,
1386,1,756,1465,1,
755,1469,1,754,1392,
1,325,1158,1,645,
1397,1,430,1401,1,
1,1406,1,107,1411,
1,36,1665,19,210,
1,36,1666,5,75,
1,534,1435,1,619,
1283,1,421,1288,1,
847,1440,1,846,1667,
16,0,208,1,96,
1033,1,95,1038,1,
949,1294,1,92,1042,
1,840,1441,1,731,
1300,1,621,1442,1,
298,1047,1,939,1443,
1,296,1052,1,373,
1306,1,716,1444,1,
715,1311,1,607,1445,
1,606,1668,16,0,
313,1,712,1316,1,
1030,1059,1,1028,1064,
1,385,1320,1,169,
1069,1,595,1446,1,
699,1447,1,909,1326,
1,908,1331,1,371,
1336,1,369,1340,1,
44,1076,1,126,1123,
1,1006,1343,1,791,
1448,1,3,1154,1,
788,1453,1,144,1347,
1,33,1084,1,19,
1101,1,997,1457,1,
125,1127,1,2,1163,
1,566,1354,1,127,
1119,1,130,1106,1,
129,1090,1,27,1095,
1,20,1100,1,1027,
1458,1,131,1102,1,
772,1459,1,343,1110,
1,128,1115,1,448,
1361,1,432,1363,1,
446,1368,1,17,1132,
1,10,1145,1,15,
1136,1,14,1463,1,
9,1149,1,12,1140,
1,546,1374,1,545,
1378,1,544,1382,1,
436,1386,1,756,1465,
1,755,1469,1,754,
1392,1,325,1158,1,
645,1397,1,430,1401,
1,1,1406,1,107,
1411,1,35,1669,19,
219,1,35,1670,5,
33,1,92,1042,1,
44,1076,1,325,1158,
1,1028,1064,1,33,
1084,1,131,1102,1,
130,1106,1,129,1090,
1,128,1115,1,127,
1119,1,126,1123,1,
125,1127,1,1030,1059,
1,169,1069,1,27,
1095,1,343,1110,1,
823,1671,16,0,217,
1,20,1100,1,19,
1101,1,17,1132,1,
298,1047,1,15,1136,
1,296,1052,1,107,
1411,1,12,1140,1,
10,1145,1,9,1149,
1,578,1672,16,0,
322,1,3,1154,1,
2,1163,1,144,1347,
1,96,1033,1,95,
1038,1,34,1673,19,
328,1,34,1674,5,
71,1,534,1675,16,
0,326,1,619,1283,
1,421,1288,1,847,
1676,16,0,326,1,
96,1033,1,95,1038,
1,949,1294,1,92,
1042,1,840,1677,16,
0,326,1,731,1300,
1,621,1678,16,0,
326,1,298,1047,1,
939,1679,16,0,326,
1,296,1052,1,373,
1306,1,716,1680,16,
0,326,1,715,1311,
1,607,1681,16,0,
326,1,712,1316,1,
1030,1059,1,1028,1064,
1,385,1320,1,169,
1069,1,595,1682,16,
0,326,1,699,1683,
16,0,326,1,909,
1326,1,908,1331,1,
371,1336,1,369,1340,
1,44,1076,1,1006,
1343,1,3,1154,1,
126,1123,1,144,1347,
1,33,1084,1,19,
1101,1,997,1684,16,
0,326,1,125,1127,
1,0,1685,16,0,
326,1,566,1354,1,
127,1119,1,130,1106,
1,129,1090,1,27,
1095,1,20,1100,1,
1027,1686,16,0,326,
1,131,1102,1,772,
1687,16,0,326,1,
343,1110,1,128,1115,
1,448,1361,1,432,
1363,1,446,1368,1,
17,1132,1,10,1145,
1,15,1136,1,14,
1688,16,0,326,1,
9,1149,1,12,1140,
1,546,1374,1,545,
1378,1,544,1382,1,
436,1386,1,756,1689,
16,0,326,1,2,
1163,1,754,1392,1,
325,1158,1,645,1397,
1,430,1401,1,1,
1406,1,107,1411,1,
33,1690,19,143,1,
33,1691,5,12,1,
20,1692,17,1693,15,
1694,4,16,37,0,
118,0,97,0,114,
0,108,0,105,0,
115,0,116,0,1,
-1,1,5,1695,20,
644,1,151,1,3,
1,2,1,1,1696,
22,1,58,1,343,
1110,1,733,1697,16,
0,246,1,19,1101,
1,363,1698,17,1699,
15,1694,1,-1,1,
5,1700,20,663,1,
150,1,3,1,4,
1,3,1701,22,1,
57,1,448,1702,16,
0,141,1,208,1703,
16,0,245,1,169,
1704,16,0,261,1,
373,1705,16,0,186,
1,27,1095,1,371,
1336,1,369,1340,1,
32,1706,19,419,1,
32,1707,5,43,1,
209,1708,16,0,417,
1,634,1709,16,0,
417,1,97,1710,16,
0,417,1,734,1711,
16,0,417,1,812,
1712,16,0,417,1,
302,1713,16,0,417,
1,301,1175,1,300,
1179,1,402,1714,16,
0,417,1,506,1715,
16,0,417,1,69,
1716,16,0,417,1,
374,1717,16,0,417,
1,50,1186,1,170,
1718,16,0,417,1,
62,1719,16,0,417,
1,61,1192,1,60,
1195,1,59,1198,1,
58,1201,1,57,1204,
1,56,1207,1,55,
1210,1,54,1213,1,
53,1216,1,52,1219,
1,51,1222,1,478,
1720,16,0,417,1,
49,1226,1,48,1229,
1,47,1232,1,45,
1721,16,0,417,1,
567,1722,16,0,417,
1,671,1723,16,0,
417,1,28,1724,16,
0,417,1,450,1725,
16,0,417,1,976,
1726,16,0,417,1,
546,1727,16,0,417,
1,866,1728,16,0,
417,1,7,1243,1,
6,1247,1,5,1250,
1,4,1729,16,0,
417,1,206,1730,16,
0,417,1,31,1731,
19,374,1,31,1732,
5,45,1,210,1733,
16,0,372,1,207,
1734,16,0,372,1,
96,1033,1,95,1038,
1,92,1042,1,517,
1735,16,0,372,1,
298,1047,1,296,1052,
1,76,1736,16,0,
372,1,823,1737,16,
0,372,1,171,1738,
16,0,372,1,1030,
1059,1,1028,1064,1,
385,1739,16,0,372,
1,169,1069,1,489,
1740,16,0,372,1,
46,1741,16,0,372,
1,44,1076,1,578,
1742,16,0,372,1,
682,1743,16,0,372,
1,144,1744,16,0,
372,1,33,1084,1,
461,1745,16,0,372,
1,129,1090,1,27,
1095,1,20,1100,1,
19,1101,1,131,1102,
1,130,1106,1,343,
1110,1,128,1115,1,
127,1119,1,126,1123,
1,125,1127,1,17,
1132,1,15,1136,1,
12,1140,1,10,1145,
1,9,1149,1,327,
1746,16,0,372,1,
3,1154,1,325,1158,
1,645,1747,16,0,
372,1,2,1163,1,
107,1748,16,0,372,
1,30,1749,19,371,
1,30,1750,5,45,
1,210,1751,16,0,
369,1,207,1752,16,
0,369,1,96,1033,
1,95,1038,1,92,
1042,1,517,1753,16,
0,369,1,298,1047,
1,296,1052,1,76,
1754,16,0,369,1,
823,1755,16,0,369,
1,171,1756,16,0,
369,1,1030,1059,1,
1028,1064,1,385,1757,
16,0,369,1,169,
1069,1,489,1758,16,
0,369,1,46,1759,
16,0,369,1,44,
1076,1,578,1760,16,
0,369,1,682,1761,
16,0,369,1,144,
1762,16,0,369,1,
33,1084,1,461,1763,
16,0,369,1,129,
1090,1,27,1095,1,
20,1100,1,19,1101,
1,131,1102,1,130,
1106,1,343,1110,1,
128,1115,1,127,1119,
1,126,1123,1,125,
1127,1,17,1132,1,
15,1136,1,12,1140,
1,10,1145,1,9,
1149,1,327,1764,16,
0,369,1,3,1154,
1,325,1158,1,645,
1765,16,0,369,1,
2,1163,1,107,1766,
16,0,369,1,29,
1767,19,362,1,29,
1768,5,45,1,210,
1769,16,0,360,1,
207,1770,16,0,360,
1,96,1033,1,95,
1038,1,92,1042,1,
517,1771,16,0,360,
1,298,1047,1,296,
1052,1,76,1772,16,
0,360,1,823,1773,
16,0,360,1,171,
1774,16,0,360,1,
1030,1059,1,1028,1064,
1,385,1775,16,0,
360,1,169,1069,1,
489,1776,16,0,360,
1,46,1777,16,0,
360,1,44,1076,1,
578,1778,16,0,360,
1,682,1779,16,0,
360,1,144,1780,16,
0,360,1,33,1084,
1,461,1781,16,0,
360,1,129,1090,1,
27,1095,1,20,1100,
1,19,1101,1,131,
1102,1,130,1106,1,
343,1110,1,128,1115,
1,127,1119,1,126,
1123,1,125,1127,1,
17,1132,1,15,1136,
1,12,1140,1,10,
1145,1,9,1149,1,
327,1782,16,0,360,
1,3,1154,1,325,
1158,1,645,1783,16,
0,360,1,2,1163,
1,107,1784,16,0,
360,1,28,1785,19,
359,1,28,1786,5,
45,1,210,1787,16,
0,357,1,207,1788,
16,0,357,1,96,
1033,1,95,1038,1,
92,1042,1,517,1789,
16,0,357,1,298,
1047,1,296,1052,1,
76,1790,16,0,357,
1,823,1791,16,0,
357,1,171,1792,16,
0,357,1,1030,1059,
1,1028,1064,1,385,
1793,16,0,357,1,
169,1069,1,489,1794,
16,0,357,1,46,
1795,16,0,357,1,
44,1076,1,578,1796,
16,0,357,1,682,
1797,16,0,357,1,
144,1798,16,0,357,
1,33,1084,1,461,
1799,16,0,357,1,
129,1090,1,27,1095,
1,20,1100,1,19,
1101,1,131,1102,1,
130,1106,1,343,1110,
1,128,1115,1,127,
1119,1,126,1123,1,
125,1127,1,17,1132,
1,15,1136,1,12,
1140,1,10,1145,1,
9,1149,1,327,1800,
16,0,357,1,3,
1154,1,325,1158,1,
645,1801,16,0,357,
1,2,1163,1,107,
1802,16,0,357,1,
27,1803,19,365,1,
27,1804,5,45,1,
210,1805,16,0,363,
1,207,1806,16,0,
363,1,96,1033,1,
95,1038,1,92,1042,
1,517,1807,16,0,
363,1,298,1047,1,
296,1052,1,76,1808,
16,0,363,1,823,
1809,16,0,363,1,
171,1810,16,0,363,
1,1030,1059,1,1028,
1064,1,385,1811,16,
0,363,1,169,1069,
1,489,1812,16,0,
363,1,46,1813,16,
0,363,1,44,1076,
1,578,1814,16,0,
363,1,682,1815,16,
0,363,1,144,1816,
16,0,363,1,33,
1084,1,461,1817,16,
0,363,1,129,1090,
1,27,1095,1,20,
1100,1,19,1101,1,
131,1102,1,130,1106,
1,343,1110,1,128,
1115,1,127,1119,1,
126,1123,1,125,1127,
1,17,1132,1,15,
1136,1,12,1140,1,
10,1145,1,9,1149,
1,327,1818,16,0,
363,1,3,1154,1,
325,1158,1,645,1819,
16,0,363,1,2,
1163,1,107,1820,16,
0,363,1,26,1821,
19,356,1,26,1822,
5,45,1,210,1823,
16,0,354,1,207,
1824,16,0,354,1,
96,1033,1,95,1038,
1,92,1042,1,517,
1825,16,0,354,1,
298,1047,1,296,1052,
1,76,1826,16,0,
354,1,823,1827,16,
0,354,1,171,1828,
16,0,354,1,1030,
1059,1,1028,1064,1,
385,1829,16,0,354,
1,169,1069,1,489,
1830,16,0,354,1,
46,1831,16,0,354,
1,44,1076,1,578,
1832,16,0,354,1,
682,1833,16,0,354,
1,144,1834,16,0,
354,1,33,1084,1,
461,1835,16,0,354,
1,129,1090,1,27,
1095,1,20,1100,1,
19,1101,1,131,1102,
1,130,1106,1,343,
1110,1,128,1115,1,
127,1119,1,126,1123,
1,125,1127,1,17,
1132,1,15,1136,1,
12,1140,1,10,1145,
1,9,1149,1,327,
1836,16,0,354,1,
3,1154,1,325,1158,
1,645,1837,16,0,
354,1,2,1163,1,
107,1838,16,0,354,
1,25,1839,19,377,
1,25,1840,5,45,
1,210,1841,16,0,
375,1,207,1842,16,
0,375,1,96,1033,
1,95,1038,1,92,
1042,1,517,1843,16,
0,375,1,298,1047,
1,296,1052,1,76,
1844,16,0,375,1,
823,1845,16,0,375,
1,171,1846,16,0,
375,1,1030,1059,1,
1028,1064,1,385,1847,
16,0,375,1,169,
1069,1,489,1848,16,
0,375,1,46,1849,
16,0,375,1,44,
1076,1,578,1850,16,
0,375,1,682,1851,
16,0,375,1,144,
1852,16,0,375,1,
33,1084,1,461,1853,
16,0,375,1,129,
1090,1,27,1095,1,
20,1100,1,19,1101,
1,131,1102,1,130,
1106,1,343,1110,1,
128,1115,1,127,1119,
1,126,1123,1,125,
1127,1,17,1132,1,
15,1136,1,12,1140,
1,10,1145,1,9,
1149,1,327,1854,16,
0,375,1,3,1154,
1,325,1158,1,645,
1855,16,0,375,1,
2,1163,1,107,1856,
16,0,375,1,24,
1857,19,368,1,24,
1858,5,45,1,210,
1859,16,0,366,1,
207,1860,16,0,366,
1,96,1033,1,95,
1038,1,92,1042,1,
517,1861,16,0,366,
1,298,1047,1,296,
1052,1,76,1862,16,
0,366,1,823,1863,
16,0,366,1,171,
1864,16,0,366,1,
1030,1059,1,1028,1064,
1,385,1865,16,0,
366,1,169,1069,1,
489,1866,16,0,366,
1,46,1867,16,0,
366,1,44,1076,1,
578,1868,16,0,366,
1,682,1869,16,0,
366,1,144,1870,16,
0,366,1,33,1084,
1,461,1871,16,0,
366,1,129,1090,1,
27,1095,1,20,1100,
1,19,1101,1,131,
1102,1,130,1106,1,
343,1110,1,128,1115,
1,127,1119,1,126,
1123,1,125,1127,1,
17,1132,1,15,1136,
1,12,1140,1,10,
1145,1,9,1149,1,
327,1872,16,0,366,
1,3,1154,1,325,
1158,1,645,1873,16,
0,366,1,2,1163,
1,107,1874,16,0,
366,1,23,1875,19,
350,1,23,1876,5,
45,1,210,1877,16,
0,348,1,207,1878,
16,0,348,1,96,
1033,1,95,1038,1,
92,1042,1,517,1879,
16,0,348,1,298,
1047,1,296,1052,1,
76,1880,16,0,348,
1,823,1881,16,0,
348,1,171,1882,16,
0,348,1,1030,1059,
1,1028,1064,1,385,
1883,16,0,348,1,
169,1069,1,489,1884,
16,0,348,1,46,
1885,16,0,348,1,
44,1076,1,578,1886,
16,0,348,1,682,
1887,16,0,348,1,
144,1888,16,0,348,
1,33,1084,1,461,
1889,16,0,348,1,
129,1090,1,27,1095,
1,20,1100,1,19,
1101,1,131,1102,1,
130,1106,1,343,1110,
1,128,1115,1,127,
1119,1,126,1123,1,
125,1127,1,17,1132,
1,15,1136,1,12,
1140,1,10,1145,1,
9,1149,1,327,1890,
16,0,348,1,3,
1154,1,325,1158,1,
645,1891,16,0,348,
1,2,1163,1,107,
1892,16,0,348,1,
22,1893,19,347,1,
22,1894,5,45,1,
210,1895,16,0,345,
1,207,1896,16,0,
345,1,96,1033,1,
95,1038,1,92,1042,
1,517,1897,16,0,
345,1,298,1047,1,
296,1052,1,76,1898,
16,0,345,1,823,
1899,16,0,345,1,
171,1900,16,0,345,
1,1030,1059,1,1028,
1064,1,385,1901,16,
0,345,1,169,1069,
1,489,1902,16,0,
345,1,46,1903,16,
0,345,1,44,1076,
1,578,1904,16,0,
345,1,682,1905,16,
0,345,1,144,1906,
16,0,345,1,33,
1084,1,461,1907,16,
0,345,1,129,1090,
1,27,1095,1,20,
1100,1,19,1101,1,
131,1102,1,130,1106,
1,343,1110,1,128,
1115,1,127,1119,1,
126,1123,1,125,1127,
1,17,1132,1,15,
1136,1,12,1140,1,
10,1145,1,9,1149,
1,327,1908,16,0,
345,1,3,1154,1,
325,1158,1,645,1909,
16,0,345,1,2,
1163,1,107,1910,16,
0,345,1,21,1911,
19,344,1,21,1912,
5,45,1,210,1913,
16,0,342,1,207,
1914,16,0,342,1,
96,1033,1,95,1038,
1,92,1042,1,517,
1915,16,0,342,1,
298,1047,1,296,1052,
1,76,1916,16,0,
342,1,823,1917,16,
0,342,1,171,1918,
16,0,342,1,1030,
1059,1,1028,1064,1,
385,1919,16,0,342,
1,169,1069,1,489,
1920,16,0,342,1,
46,1921,16,0,342,
1,44,1076,1,578,
1922,16,0,342,1,
682,1923,16,0,342,
1,144,1924,16,0,
342,1,33,1084,1,
461,1925,16,0,342,
1,129,1090,1,27,
1095,1,20,1100,1,
19,1101,1,131,1102,
1,130,1106,1,343,
1110,1,128,1115,1,
127,1119,1,126,1123,
1,125,1127,1,17,
1132,1,15,1136,1,
12,1140,1,10,1145,
1,9,1149,1,327,
1926,16,0,342,1,
3,1154,1,325,1158,
1,645,1927,16,0,
342,1,2,1163,1,
107,1928,16,0,342,
1,20,1929,19,422,
1,20,1930,5,43,
1,209,1931,16,0,
420,1,634,1932,16,
0,420,1,97,1933,
16,0,420,1,734,
1934,16,0,420,1,
812,1935,16,0,420,
1,302,1936,16,0,
420,1,301,1175,1,
300,1179,1,402,1937,
16,0,420,1,506,
1938,16,0,420,1,
69,1939,16,0,420,
1,374,1940,16,0,
420,1,50,1186,1,
170,1941,16,0,420,
1,62,1942,16,0,
420,1,61,1192,1,
60,1195,1,59,1198,
1,58,1201,1,57,
1204,1,56,1207,1,
55,1210,1,54,1213,
1,53,1216,1,52,
1219,1,51,1222,1,
478,1943,16,0,420,
1,49,1226,1,48,
1229,1,47,1232,1,
45,1944,16,0,420,
1,567,1945,16,0,
420,1,671,1946,16,
0,420,1,28,1947,
16,0,420,1,450,
1948,16,0,420,1,
976,1949,16,0,420,
1,546,1950,16,0,
420,1,866,1951,16,
0,420,1,7,1243,
1,6,1247,1,5,
1250,1,4,1952,16,
0,420,1,206,1953,
16,0,420,1,19,
1954,19,341,1,19,
1955,5,45,1,210,
1956,16,0,339,1,
207,1957,16,0,339,
1,96,1033,1,95,
1038,1,92,1042,1,
517,1958,16,0,339,
1,298,1047,1,296,
1052,1,76,1959,16,
0,339,1,823,1960,
16,0,339,1,171,
1961,16,0,339,1,
1030,1059,1,1028,1064,
1,385,1962,16,0,
339,1,169,1069,1,
489,1963,16,0,339,
1,46,1964,16,0,
339,1,44,1076,1,
578,1965,16,0,339,
1,682,1966,16,0,
339,1,144,1967,16,
0,339,1,33,1084,
1,461,1968,16,0,
339,1,129,1090,1,
27,1095,1,20,1100,
1,19,1101,1,131,
1102,1,130,1106,1,
343,1110,1,128,1115,
1,127,1119,1,126,
1123,1,125,1127,1,
17,1132,1,15,1136,
1,12,1140,1,10,
1145,1,9,1149,1,
327,1969,16,0,339,
1,3,1154,1,325,
1158,1,645,1970,16,
0,339,1,2,1163,
1,107,1971,16,0,
339,1,18,1972,19,
338,1,18,1973,5,
88,1,461,1974,16,
0,336,1,450,1975,
16,0,416,1,210,
1976,16,0,336,1,
209,1977,16,0,416,
1,207,1978,16,0,
336,1,206,1979,16,
0,416,1,682,1980,
16,0,336,1,671,
1981,16,0,416,1,
171,1982,16,0,336,
1,170,1983,16,0,
416,1,169,1069,1,
645,1984,16,0,336,
1,402,1985,16,0,
416,1,634,1986,16,
0,416,1,866,1987,
16,0,416,1,385,
1988,16,0,336,1,
144,1989,16,0,336,
1,374,1990,16,0,
416,1,131,1102,1,
130,1106,1,129,1090,
1,128,1115,1,127,
1119,1,126,1123,1,
125,1127,1,107,1991,
16,0,336,1,823,
1992,16,0,336,1,
343,1110,1,578,1993,
16,0,336,1,95,
1038,1,97,1994,16,
0,416,1,96,1033,
1,812,1995,16,0,
416,1,92,1042,1,
296,1052,1,567,1996,
16,0,416,1,327,
1997,16,0,336,1,
325,1158,1,300,1179,
1,506,1998,16,0,
416,1,76,1999,16,
0,336,1,1030,1059,
1,301,1175,1,1028,
1064,1,298,1047,1,
69,2000,16,0,416,
1,546,2001,16,0,
416,1,302,2002,16,
0,416,1,62,2003,
16,0,416,1,61,
1192,1,60,1195,1,
59,1198,1,58,1201,
1,57,1204,1,56,
1207,1,55,1210,1,
54,1213,1,53,1216,
1,52,1219,1,51,
1222,1,50,1186,1,
49,1226,1,48,1229,
1,47,1232,1,46,
2004,16,0,336,1,
45,2005,16,0,416,
1,44,1076,1,517,
2006,16,0,336,1,
33,1084,1,28,2007,
16,0,416,1,27,
1095,1,20,1100,1,
17,1132,1,976,2008,
16,0,416,1,19,
1101,1,734,2009,16,
0,416,1,15,1136,
1,12,1140,1,489,
2010,16,0,336,1,
10,1145,1,9,1149,
1,7,1243,1,6,
1247,1,5,1250,1,
4,2011,16,0,416,
1,3,1154,1,2,
1163,1,478,2012,16,
0,416,1,17,2013,
19,335,1,17,2014,
5,45,1,210,2015,
16,0,333,1,207,
2016,16,0,333,1,
96,1033,1,95,1038,
1,92,1042,1,517,
2017,16,0,333,1,
298,1047,1,296,1052,
1,76,2018,16,0,
333,1,823,2019,16,
0,333,1,171,2020,
16,0,333,1,1030,
1059,1,1028,1064,1,
385,2021,16,0,333,
1,169,1069,1,489,
2022,16,0,333,1,
46,2023,16,0,333,
1,44,1076,1,578,
2024,16,0,333,1,
682,2025,16,0,333,
1,144,2026,16,0,
333,1,33,1084,1,
461,2027,16,0,333,
1,129,1090,1,27,
1095,1,20,1100,1,
19,1101,1,131,1102,
1,130,1106,1,343,
1110,1,128,1115,1,
127,1119,1,126,1123,
1,125,1127,1,17,
1132,1,15,1136,1,
12,1140,1,10,1145,
1,9,1149,1,327,
2028,16,0,333,1,
3,1154,1,325,1158,
1,645,2029,16,0,
333,1,2,1163,1,
107,2030,16,0,333,
1,16,2031,19,159,
1,16,2032,5,20,
1,92,1042,1,44,
1076,1,325,1158,1,
33,2033,16,0,394,
1,169,1069,1,27,
1095,1,343,1110,1,
22,2034,16,0,394,
1,20,1100,1,19,
1101,1,298,1047,1,
438,2035,16,0,157,
1,296,1052,1,10,
1145,1,9,1149,1,
1,2036,16,0,394,
1,2,1163,1,3,
1154,1,96,1033,1,
95,1038,1,15,2037,
19,232,1,15,2038,
5,41,1,210,2039,
17,2040,15,2041,4,
30,37,0,70,0,
105,0,101,0,108,
0,100,0,69,0,
120,0,112,0,65,
0,115,0,115,0,
105,0,103,0,110,
0,1,-1,1,5,
2042,20,552,1,163,
1,3,1,6,1,
5,2043,22,1,94,
1,96,1033,1,95,
1038,1,92,1042,1,
302,2044,17,2045,15,
2046,4,20,37,0,
102,0,105,0,101,
0,108,0,100,0,
108,0,105,0,115,
0,116,0,1,-1,
1,5,2047,20,850,
1,127,1,3,1,
3,1,2,2048,22,
1,31,1,301,1175,
1,300,1179,1,299,
2049,17,2050,15,2046,
1,-1,1,5,2051,
20,858,1,124,1,
3,1,2,1,1,
2052,22,1,29,1,
298,1047,1,296,1052,
1,295,2053,16,0,
233,1,1030,1059,1,
1028,1064,1,171,2054,
17,2055,15,2056,4,
24,37,0,70,0,
105,0,101,0,108,
0,100,0,65,0,
115,0,115,0,105,
0,103,0,110,0,
1,-1,1,5,2057,
20,550,1,164,1,
3,1,4,1,3,
2058,22,1,95,1,
169,1069,1,46,2059,
17,2060,15,2061,4,
12,37,0,102,0,
105,0,101,0,108,
0,100,0,1,-1,
1,5,2062,20,545,
1,165,1,3,1,
2,1,1,2063,22,
1,96,1,45,2064,
16,0,230,1,44,
1076,1,144,1347,1,
33,1084,1,19,1101,
1,129,1090,1,27,
1095,1,127,1119,1,
131,1102,1,130,1106,
1,343,1110,1,128,
1115,1,20,1100,1,
126,1123,1,125,1127,
1,17,1132,1,15,
1136,1,12,1140,1,
10,1145,1,9,1149,
1,3,1154,1,325,
1158,1,324,2065,17,
2066,15,2046,1,-1,
1,5,2067,20,855,
1,126,1,3,1,
4,1,3,2068,22,
1,30,1,2,1163,
1,107,1411,1,14,
2069,19,381,1,14,
2070,5,63,1,209,
2071,16,0,379,1,
634,2072,16,0,379,
1,97,2073,16,0,
379,1,96,1033,1,
95,1038,1,92,1042,
1,812,2074,16,0,
379,1,302,2075,16,
0,379,1,301,1175,
1,300,1179,1,298,
1047,1,296,1052,1,
402,2076,16,0,379,
1,506,2077,16,0,
379,1,169,1069,1,
69,2078,16,0,379,
1,50,1186,1,53,
1216,1,170,2079,16,
0,379,1,62,2080,
16,0,379,1,61,
1192,1,60,1195,1,
59,1198,1,58,1201,
1,57,1204,1,56,
1207,1,55,1210,1,
54,1213,1,374,2081,
16,0,379,1,52,
1219,1,51,1222,1,
478,2082,16,0,379,
1,49,1226,1,48,
1229,1,47,1232,1,
45,2083,16,0,379,
1,44,1076,1,40,
2084,16,0,379,1,
343,1110,1,33,2085,
16,0,379,1,567,
2086,16,0,379,1,
671,2087,16,0,379,
1,28,2088,16,0,
379,1,27,1095,1,
22,2089,16,0,379,
1,450,2090,16,0,
379,1,20,1100,1,
19,1101,1,9,1149,
1,976,2091,16,0,
379,1,10,1145,1,
546,2092,16,0,379,
1,866,2093,16,0,
379,1,734,2094,16,
0,379,1,4,2095,
16,0,379,1,7,
1243,1,6,1247,1,
5,1250,1,325,1158,
1,3,1154,1,2,
1163,1,1,2096,16,
0,379,1,206,2097,
16,0,379,1,13,
2098,19,204,1,13,
2099,5,33,1,327,
2100,16,0,202,1,
44,1076,1,325,1158,
1,1028,1064,1,33,
1084,1,131,1102,1,
130,1106,1,129,1090,
1,128,1115,1,127,
1119,1,126,1123,1,
125,1127,1,1030,1059,
1,169,1069,1,27,
1095,1,343,1110,1,
19,1101,1,20,1100,
1,207,2101,16,0,
250,1,17,1132,1,
298,1047,1,15,1136,
1,296,1052,1,107,
1411,1,12,1140,1,
10,1145,1,9,1149,
1,92,1042,1,3,
1154,1,2,1163,1,
144,1347,1,96,1033,
1,95,1038,1,12,
2102,19,254,1,12,
2103,5,23,1,92,
1042,1,44,1076,1,
325,1158,1,33,2104,
16,0,392,1,169,
1069,1,27,1095,1,
343,1110,1,20,1100,
1,22,2105,16,0,
392,1,19,1101,1,
302,2106,16,0,252,
1,301,1175,1,300,
1179,1,298,1047,1,
296,1052,1,10,1145,
1,9,1149,1,1,
2107,16,0,392,1,
95,1038,1,2,1163,
1,3,1154,1,96,
1033,1,45,2108,16,
0,252,1,11,2109,
19,118,1,11,2110,
5,41,1,421,1288,
1,96,1033,1,95,
1038,1,92,1042,1,
1052,2111,17,2112,15,
2113,4,16,37,0,
112,0,97,0,114,
0,108,0,105,0,
115,0,116,0,1,
-1,1,5,2114,20,
814,1,131,1,3,
1,4,1,3,2115,
22,1,35,1,1049,
2116,17,2117,15,2113,
1,-1,1,5,2118,
20,816,1,130,1,
3,1,2,1,1,
2119,22,1,34,1,
1048,2120,17,2121,15,
2113,1,-1,1,5,
120,1,1,1,1,
2122,22,1,36,1,
298,1047,1,296,1052,
1,76,2123,16,0,
312,1,1030,1059,1,
1028,1064,1,385,1320,
1,1026,2124,16,0,
116,1,169,1069,1,
44,1076,1,144,1347,
1,33,1084,1,131,
1102,1,129,1090,1,
27,1095,1,127,1119,
1,126,1123,1,130,
1106,1,343,1110,1,
128,1115,1,20,1100,
1,19,1101,1,125,
1127,1,17,1132,1,
15,1136,1,13,2125,
16,0,409,1,12,
1140,1,10,1145,1,
9,1149,1,8,2126,
16,0,414,1,4,
2127,16,0,413,1,
325,1158,1,3,1154,
1,2,1163,1,107,
1411,1,10,2128,19,
325,1,10,2129,5,
122,1,716,2130,16,
0,323,1,715,1311,
1,712,1316,1,949,
1294,1,939,2131,16,
0,323,1,908,1331,
1,209,2132,16,0,
323,1,671,2133,16,
0,323,1,450,2134,
16,0,323,1,448,
1361,1,446,1368,1,
206,2135,16,0,323,
1,444,2136,16,0,
410,1,443,2137,17,
2138,15,2139,4,18,
37,0,102,0,117,
0,110,0,99,0,
110,0,97,0,109,
0,101,0,1,-1,
1,5,154,1,3,
1,3,2140,22,1,
62,1,440,2141,17,
2142,15,2139,1,-1,
1,5,154,1,3,
1,3,2143,22,1,
63,1,438,2144,17,
2145,15,2139,1,-1,
1,5,2146,20,596,
1,155,1,3,1,
2,1,1,2147,22,
1,64,1,436,1386,
1,434,2148,16,0,
410,1,432,1363,1,
909,1326,1,430,1401,
1,607,2149,16,0,
323,1,421,1288,1,
634,2150,16,0,323,
1,170,2151,16,0,
323,1,169,1069,1,
645,1397,1,402,2152,
16,0,323,1,847,
2153,16,0,323,1,
369,1340,1,866,2154,
16,0,323,1,385,
1320,1,144,1347,1,
621,2155,16,0,323,
1,619,1283,1,374,
2156,16,0,323,1,
373,1306,1,371,1336,
1,131,1102,1,130,
1106,1,129,1090,1,
128,1115,1,127,1119,
1,126,1123,1,125,
1127,1,840,2157,16,
0,323,1,595,2158,
16,0,323,1,534,
2159,16,0,323,1,
107,1411,1,545,1378,
1,343,1110,1,95,
1038,1,97,2160,16,
0,323,1,96,1033,
1,812,2161,16,0,
323,1,92,1042,1,
567,2162,16,0,323,
1,566,1354,1,546,
2163,16,0,323,1,
325,1158,1,544,1382,
1,1030,1059,1,301,
1175,1,1028,1064,1,
1027,2164,16,0,323,
1,69,2165,16,0,
323,1,50,1186,1,
55,1210,1,57,1204,
1,59,1198,1,61,
1192,1,302,2166,16,
0,323,1,62,2167,
16,0,323,1,300,
1179,1,60,1195,1,
298,1047,1,58,1201,
1,296,1052,1,56,
1207,1,772,2168,16,
0,323,1,54,1213,
1,53,1216,1,52,
1219,1,51,1222,1,
1006,1343,1,49,1226,
1,48,1229,1,47,
1232,1,45,2169,16,
0,323,1,44,1076,
1,997,2170,16,0,
323,1,40,2171,16,
0,423,1,756,2172,
16,0,323,1,754,
1392,1,33,2173,16,
0,423,1,28,2174,
16,0,323,1,506,
2175,16,0,323,1,
27,1095,1,20,1100,
1,17,1132,1,22,
2176,16,0,423,1,
21,2177,16,0,323,
1,976,2178,16,0,
323,1,19,1101,1,
734,2179,16,0,323,
1,14,2180,16,0,
323,1,15,1136,1,
731,1300,1,5,1250,
1,12,1140,1,11,
2181,16,0,410,1,
10,1145,1,9,1149,
1,0,2182,16,0,
323,1,7,1243,1,
6,1247,1,699,2183,
16,0,323,1,4,
2184,16,0,323,1,
3,1154,1,2,1163,
1,1,2185,16,0,
423,1,478,2186,16,
0,323,1,9,2187,
19,226,1,9,2188,
5,61,1,619,1283,
1,210,2039,1,421,
1288,1,96,1033,1,
95,1038,1,949,1294,
1,92,1042,1,373,
1306,1,731,1300,1,
299,2189,16,0,224,
1,298,1047,1,296,
1052,1,385,1320,1,
715,1311,1,712,1316,
1,1030,1059,1,1028,
1064,1,171,2054,1,
169,1069,1,909,1326,
1,908,1331,1,371,
1336,1,369,1340,1,
46,2059,1,44,1076,
1,1006,1343,1,3,
1154,1,144,1347,1,
33,1084,1,19,1101,
1,125,1127,1,566,
1354,1,126,1123,1,
129,1090,1,27,1095,
1,20,1100,1,127,
1119,1,131,1102,1,
130,1106,1,343,1110,
1,128,1115,1,448,
1361,1,432,1363,1,
446,1368,1,17,1132,
1,15,1136,1,10,
1145,1,9,1149,1,
12,1140,1,546,1374,
1,545,1378,1,544,
1382,1,436,1386,1,
756,2190,16,0,238,
1,2,1163,1,754,
1392,1,325,1158,1,
645,1397,1,430,1401,
1,1,1406,1,107,
1411,1,8,2191,19,
163,1,8,2192,5,
20,1,92,1042,1,
44,1076,1,325,1158,
1,33,2193,16,0,
386,1,169,1069,1,
27,1095,1,343,1110,
1,22,2194,16,0,
386,1,20,1100,1,
19,1101,1,298,1047,
1,438,2195,16,0,
161,1,296,1052,1,
10,1145,1,9,1149,
1,1,2196,16,0,
386,1,2,1163,1,
3,1154,1,96,1033,
1,95,1038,1,7,
2197,19,127,1,7,
2198,5,41,1,210,
2039,1,96,1033,1,
95,1038,1,92,1042,
1,1049,2199,16,0,
383,1,299,2200,16,
0,223,1,298,1047,
1,296,1052,1,171,
2054,1,1030,1059,1,
1028,1064,1,385,2201,
16,0,179,1,169,
1069,1,489,2202,16,
0,387,1,369,2203,
16,0,189,1,46,
2059,1,44,1076,1,
33,1084,1,144,1347,
1,448,2204,16,0,
189,1,461,2205,16,
0,125,1,19,1101,
1,129,1090,1,27,
1095,1,127,1119,1,
131,1102,1,130,1106,
1,343,1110,1,128,
1115,1,20,2206,16,
0,403,1,126,1123,
1,125,1127,1,17,
1132,1,15,1136,1,
12,1140,1,10,1145,
1,9,1149,1,325,
1158,1,3,1154,1,
2,1163,1,107,1411,
1,5,2207,19,279,
1,5,2208,5,43,
1,209,2209,16,0,
277,1,634,2210,16,
0,277,1,97,2211,
16,0,277,1,734,
2212,16,0,277,1,
812,2213,16,0,277,
1,302,2214,16,0,
277,1,301,1175,1,
300,1179,1,402,2215,
16,0,277,1,506,
2216,16,0,277,1,
69,2217,16,0,277,
1,374,2218,16,0,
277,1,50,1186,1,
170,2219,16,0,277,
1,62,2220,16,0,
277,1,61,1192,1,
60,1195,1,59,1198,
1,58,1201,1,57,
1204,1,56,1207,1,
55,1210,1,54,1213,
1,53,1216,1,52,
1219,1,51,1222,1,
478,2221,16,0,277,
1,49,1226,1,48,
1229,1,47,1232,1,
45,2222,16,0,277,
1,567,2223,16,0,
277,1,671,2224,16,
0,277,1,28,2225,
16,0,277,1,450,
2226,16,0,277,1,
976,2227,16,0,277,
1,546,2228,16,0,
277,1,866,2229,16,
0,277,1,7,1243,
1,6,1247,1,5,
1250,1,4,2230,16,
0,277,1,206,2231,
16,0,277,1,4,
2232,19,146,1,4,
2233,5,125,1,716,
2234,16,0,405,1,
715,1311,1,712,1316,
1,949,1294,1,939,
2235,16,0,405,1,
908,1331,1,209,2236,
16,0,405,1,671,
2237,16,0,405,1,
450,2238,16,0,405,
1,448,1361,1,447,
2239,16,0,144,1,
446,1368,1,206,2240,
16,0,405,1,441,
2241,16,0,164,1,
439,2242,16,0,160,
1,437,2243,16,0,
164,1,436,1386,1,
433,2244,16,0,169,
1,432,1363,1,909,
1326,1,430,1401,1,
607,2245,16,0,405,
1,421,1288,1,634,
2246,16,0,405,1,
170,2247,16,0,405,
1,169,1069,1,645,
1397,1,402,2248,16,
0,405,1,847,2249,
16,0,405,1,370,
2250,16,0,190,1,
369,1340,1,866,2251,
16,0,405,1,385,
1320,1,144,1347,1,
621,2252,16,0,405,
1,368,2253,16,0,
190,1,619,1283,1,
374,2254,16,0,405,
1,373,1306,1,371,
1336,1,131,1102,1,
130,1106,1,129,1090,
1,128,1115,1,127,
1119,1,126,1123,1,
125,1127,1,840,2255,
16,0,405,1,595,
2256,16,0,405,1,
534,2257,16,0,405,
1,107,1411,1,545,
1378,1,343,1110,1,
95,1038,1,97,2258,
16,0,405,1,96,
1033,1,812,2259,16,
0,405,1,1050,2260,
16,0,384,1,92,
1042,1,567,2261,16,
0,405,1,566,1354,
1,546,2262,16,0,
405,1,325,1158,1,
544,1382,1,50,1186,
1,1030,1059,1,1028,
1064,1,1027,2263,16,
0,405,1,69,2264,
16,0,405,1,59,
1198,1,55,1210,1,
57,1204,1,62,2265,
16,0,405,1,61,
1192,1,302,2266,16,
0,262,1,301,1175,
1,300,1179,1,60,
1195,1,298,1047,1,
58,1201,1,296,1052,
1,56,1207,1,772,
2267,16,0,405,1,
54,1213,1,53,1216,
1,52,1219,1,51,
1222,1,1006,1343,1,
49,1226,1,48,1229,
1,47,1232,1,45,
2268,16,0,262,1,
44,1076,1,39,2269,
16,0,385,1,997,
2270,16,0,405,1,
756,2271,16,0,405,
1,754,1392,1,33,
1084,1,28,2272,16,
0,405,1,506,2273,
16,0,405,1,27,
1095,1,26,2274,16,
0,393,1,17,1132,
1,20,1100,1,21,
2275,16,0,405,1,
976,2276,16,0,405,
1,19,1101,1,734,
2277,16,0,405,1,
14,2278,16,0,405,
1,15,1136,1,731,
1300,1,13,2279,16,
0,384,1,12,1140,
1,5,1250,1,10,
1145,1,9,1149,1,
0,2280,16,0,405,
1,7,1243,1,6,
1247,1,699,2281,16,
0,405,1,4,2282,
16,0,405,1,3,
1154,1,2,1163,1,
1,1406,1,478,2283,
16,0,405,1,3,
2284,19,282,1,3,
2285,5,63,1,209,
2286,16,0,280,1,
634,2287,16,0,280,
1,97,2288,16,0,
280,1,96,1033,1,
95,1038,1,92,1042,
1,812,2289,16,0,
280,1,302,2290,16,
0,280,1,301,1175,
1,300,1179,1,298,
1047,1,296,1052,1,
402,2291,16,0,280,
1,506,2292,16,0,
280,1,169,1069,1,
69,2293,16,0,280,
1,50,1186,1,53,
1216,1,170,2294,16,
0,280,1,62,2295,
16,0,280,1,61,
1192,1,60,1195,1,
59,1198,1,58,1201,
1,57,1204,1,56,
1207,1,55,1210,1,
54,1213,1,374,2296,
16,0,280,1,52,
1219,1,51,1222,1,
478,2297,16,0,280,
1,49,1226,1,48,
1229,1,47,1232,1,
45,2298,16,0,280,
1,44,1076,1,40,
2299,16,0,425,1,
343,1110,1,33,2300,
16,0,425,1,567,
2301,16,0,280,1,
671,2302,16,0,280,
1,28,2303,16,0,
280,1,27,1095,1,
22,2304,16,0,425,
1,450,2305,16,0,
280,1,20,1100,1,
19,1101,1,9,1149,
1,976,2306,16,0,
280,1,10,1145,1,
546,2307,16,0,280,
1,866,2308,16,0,
280,1,734,2309,16,
0,280,1,4,2310,
16,0,280,1,7,
1243,1,6,1247,1,
5,1250,1,325,1158,
1,3,1154,1,2,
1163,1,1,2311,16,
0,425,1,206,2312,
16,0,280,1,2,
2313,19,317,1,2,
2314,5,60,1,619,
1283,1,421,1288,1,
96,1033,1,95,1038,
1,949,1294,1,92,
1042,1,731,1300,1,
298,1047,1,296,1052,
1,373,1306,1,715,
1311,1,712,1316,1,
1030,1059,1,1028,1064,
1,385,1320,1,169,
1069,1,909,1326,1,
908,1331,1,371,1336,
1,772,1459,1,369,
1340,1,44,1076,1,
1006,1343,1,791,1448,
1,3,1154,1,788,
1453,1,144,1347,1,
33,1084,1,19,1101,
1,125,1127,1,566,
1354,1,126,1123,1,
129,1090,1,27,1095,
1,20,1100,1,127,
1119,1,131,1102,1,
130,1106,1,343,1110,
1,128,1115,1,448,
1361,1,432,1363,1,
446,1368,1,17,1132,
1,15,1136,1,10,
1145,1,9,1149,1,
12,1140,1,546,1374,
1,545,1378,1,544,
1382,1,436,1386,1,
756,1465,1,2,1163,
1,754,1392,1,325,
1158,1,645,1397,1,
430,1401,1,1,1406,
1,107,1411,2,1,0};
new Sfactory(this,"error",new SCreator(error_factory));
new Sfactory(this,"FieldAssign",new SCreator(FieldAssign_factory));
new Sfactory(this,"Atom_1",new SCreator(Atom_1_factory));
new Sfactory(this,"functioncall_2",new SCreator(functioncall_2_factory));
new Sfactory(this,"ExpTableDec",new SCreator(ExpTableDec_factory));
new Sfactory(this,"Unop",new SCreator(Unop_factory));
new Sfactory(this,"binop_4",new SCreator(binop_4_factory));
new Sfactory(this,"ElseIf",new SCreator(ElseIf_factory));
new Sfactory(this,"binop_11",new SCreator(binop_11_factory));
new Sfactory(this,"stat_9",new SCreator(stat_9_factory));
new Sfactory(this,"LocalFuncDecl_1",new SCreator(LocalFuncDecl_1_factory));
new Sfactory(this,"prefixexp",new SCreator(prefixexp_factory));
new Sfactory(this,"prefixexp_2",new SCreator(prefixexp_2_factory));
new Sfactory(this,"FuncDecl_1",new SCreator(FuncDecl_1_factory));
new Sfactory(this,"namelist_3",new SCreator(namelist_3_factory));
new Sfactory(this,"stat_2",new SCreator(stat_2_factory));
new Sfactory(this,"funcname_1",new SCreator(funcname_1_factory));
new Sfactory(this,"explist",new SCreator(explist_factory));
new Sfactory(this,"parlist_2",new SCreator(parlist_2_factory));
new Sfactory(this,"parlist_3",new SCreator(parlist_3_factory));
new Sfactory(this,"parlist_1",new SCreator(parlist_1_factory));
new Sfactory(this,"stat_10",new SCreator(stat_10_factory));
new Sfactory(this,"Else_1",new SCreator(Else_1_factory));
new Sfactory(this,"stat_7",new SCreator(stat_7_factory));
new Sfactory(this,"binop_12",new SCreator(binop_12_factory));
new Sfactory(this,"fieldsep_2",new SCreator(fieldsep_2_factory));
new Sfactory(this,"TableRef",new SCreator(TableRef_factory));
new Sfactory(this,"function_1",new SCreator(function_1_factory));
new Sfactory(this,"PackageRef_1",new SCreator(PackageRef_1_factory));
new Sfactory(this,"funcbody_2",new SCreator(funcbody_2_factory));
new Sfactory(this,"chunk_4",new SCreator(chunk_4_factory));
new Sfactory(this,"namelist",new SCreator(namelist_factory));
new Sfactory(this,"Retval",new SCreator(Retval_factory));
new Sfactory(this,"LocalInit",new SCreator(LocalInit_factory));
new Sfactory(this,"functioncall_1",new SCreator(functioncall_1_factory));
new Sfactory(this,"parlist",new SCreator(parlist_factory));
new Sfactory(this,"binop_6",new SCreator(binop_6_factory));
new Sfactory(this,"arg",new SCreator(arg_factory));
new Sfactory(this,"funcname",new SCreator(funcname_factory));
new Sfactory(this,"SElseIf",new SCreator(SElseIf_factory));
new Sfactory(this,"prefixexp_3",new SCreator(prefixexp_3_factory));
new Sfactory(this,"SElse",new SCreator(SElse_factory));
new Sfactory(this,"binop_13",new SCreator(binop_13_factory));
new Sfactory(this,"init_1",new SCreator(init_1_factory));
new Sfactory(this,"stat_3",new SCreator(stat_3_factory));
new Sfactory(this,"varlist_2",new SCreator(varlist_2_factory));
new Sfactory(this,"binop_1",new SCreator(binop_1_factory));
new Sfactory(this,"chunk_2",new SCreator(chunk_2_factory));
new Sfactory(this,"fieldlist_1",new SCreator(fieldlist_1_factory));
new Sfactory(this,"field_1",new SCreator(field_1_factory));
new Sfactory(this,"stat_4",new SCreator(stat_4_factory));
new Sfactory(this,"field",new SCreator(field_factory));
new Sfactory(this,"Atom_2",new SCreator(Atom_2_factory));
new Sfactory(this,"funcbody",new SCreator(funcbody_factory));
new Sfactory(this,"funcbody_3",new SCreator(funcbody_3_factory));
new Sfactory(this,"LocalFuncDecl",new SCreator(LocalFuncDecl_factory));
new Sfactory(this,"var",new SCreator(var_factory));
new Sfactory(this,"TableRef_1",new SCreator(TableRef_1_factory));
new Sfactory(this,"binop_14",new SCreator(binop_14_factory));
new Sfactory(this,"Binop_1",new SCreator(Binop_1_factory));
new Sfactory(this,"SElseIf_1",new SCreator(SElseIf_1_factory));
new Sfactory(this,"LocalInit_1",new SCreator(LocalInit_1_factory));
new Sfactory(this,"tableconstructor",new SCreator(tableconstructor_factory));
new Sfactory(this,"PackageRef",new SCreator(PackageRef_factory));
new Sfactory(this,"function",new SCreator(function_factory));
new Sfactory(this,"Unop_1",new SCreator(Unop_1_factory));
new Sfactory(this,"SIf_1",new SCreator(SIf_1_factory));
new Sfactory(this,"namelist_1",new SCreator(namelist_1_factory));
new Sfactory(this,"funcname_2",new SCreator(funcname_2_factory));
new Sfactory(this,"stat",new SCreator(stat_factory));
new Sfactory(this,"unop_1",new SCreator(unop_1_factory));
new Sfactory(this,"exp",new SCreator(exp_factory));
new Sfactory(this,"FieldExpAssign_1",new SCreator(FieldExpAssign_1_factory));
new Sfactory(this,"fieldlist",new SCreator(fieldlist_factory));
new Sfactory(this,"arg_2",new SCreator(arg_2_factory));
new Sfactory(this,"arg_1",new SCreator(arg_1_factory));
new Sfactory(this,"arg_4",new SCreator(arg_4_factory));
new Sfactory(this,"binop_15",new SCreator(binop_15_factory));
new Sfactory(this,"unop",new SCreator(unop_factory));
new Sfactory(this,"FunctionCall",new SCreator(FunctionCall_factory));
new Sfactory(this,"binop_8",new SCreator(binop_8_factory));
new Sfactory(this,"Assignment",new SCreator(Assignment_factory));
new Sfactory(this,"tableconstructor_2",new SCreator(tableconstructor_2_factory));
new Sfactory(this,"Atom_3",new SCreator(Atom_3_factory));
new Sfactory(this,"arg_3",new SCreator(arg_3_factory));
new Sfactory(this,"If",new SCreator(If_factory));
new Sfactory(this,"Else",new SCreator(Else_factory));
new Sfactory(this,"Binop",new SCreator(Binop_factory));
new Sfactory(this,"explist_1",new SCreator(explist_1_factory));
new Sfactory(this,"explist_2",new SCreator(explist_2_factory));
new Sfactory(this,"SElse_1",new SCreator(SElse_1_factory));
new Sfactory(this,"binop_3",new SCreator(binop_3_factory));
new Sfactory(this,"ExpTableDec_1",new SCreator(ExpTableDec_1_factory));
new Sfactory(this,"block_2",new SCreator(block_2_factory));
new Sfactory(this,"chunk",new SCreator(chunk_factory));
new Sfactory(this,"fieldlist_3",new SCreator(fieldlist_3_factory));
new Sfactory(this,"block_1",new SCreator(block_1_factory));
new Sfactory(this,"binop",new SCreator(binop_factory));
new Sfactory(this,"Retval_1",new SCreator(Retval_1_factory));
new Sfactory(this,"funcname_3",new SCreator(funcname_3_factory));
new Sfactory(this,"unop_2",new SCreator(unop_2_factory));
new Sfactory(this,"Atom",new SCreator(Atom_factory));
new Sfactory(this,"If_1",new SCreator(If_1_factory));
new Sfactory(this,"fieldlist_2",new SCreator(fieldlist_2_factory));
new Sfactory(this,"var_1",new SCreator(var_1_factory));
new Sfactory(this,"ElseIf_1",new SCreator(ElseIf_1_factory));
new Sfactory(this,"stat_1",new SCreator(stat_1_factory));
new Sfactory(this,"varlist_1",new SCreator(varlist_1_factory));
new Sfactory(this,"SIf",new SCreator(SIf_factory));
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
new Sfactory(this,"FunctionCall_1",new SCreator(FunctionCall_1_factory));
new Sfactory(this,"init",new SCreator(init_factory));
new Sfactory(this,"FieldExpAssign",new SCreator(FieldExpAssign_factory));
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
new Sfactory(this,"binop_2",new SCreator(binop_2_factory));
new Sfactory(this,"exp_2",new SCreator(exp_2_factory));
new Sfactory(this,"exp_1",new SCreator(exp_1_factory));
}
public static object error_factory(Parser yyp) { return new error(yyp); }
public static object FieldAssign_factory(Parser yyp) { return new FieldAssign(yyp); }
public static object Atom_1_factory(Parser yyp) { return new Atom_1(yyp); }
public static object functioncall_2_factory(Parser yyp) { return new functioncall_2(yyp); }
public static object ExpTableDec_factory(Parser yyp) { return new ExpTableDec(yyp); }
public static object Unop_factory(Parser yyp) { return new Unop(yyp); }
public static object binop_4_factory(Parser yyp) { return new binop_4(yyp); }
public static object ElseIf_factory(Parser yyp) { return new ElseIf(yyp); }
public static object binop_11_factory(Parser yyp) { return new binop_11(yyp); }
public static object stat_9_factory(Parser yyp) { return new stat_9(yyp); }
public static object LocalFuncDecl_1_factory(Parser yyp) { return new LocalFuncDecl_1(yyp); }
public static object prefixexp_factory(Parser yyp) { return new prefixexp(yyp); }
public static object prefixexp_2_factory(Parser yyp) { return new prefixexp_2(yyp); }
public static object FuncDecl_1_factory(Parser yyp) { return new FuncDecl_1(yyp); }
public static object namelist_3_factory(Parser yyp) { return new namelist_3(yyp); }
public static object stat_2_factory(Parser yyp) { return new stat_2(yyp); }
public static object funcname_1_factory(Parser yyp) { return new funcname_1(yyp); }
public static object explist_factory(Parser yyp) { return new explist(yyp); }
public static object parlist_2_factory(Parser yyp) { return new parlist_2(yyp); }
public static object parlist_3_factory(Parser yyp) { return new parlist_3(yyp); }
public static object parlist_1_factory(Parser yyp) { return new parlist_1(yyp); }
public static object stat_10_factory(Parser yyp) { return new stat_10(yyp); }
public static object Else_1_factory(Parser yyp) { return new Else_1(yyp); }
public static object stat_7_factory(Parser yyp) { return new stat_7(yyp); }
public static object binop_12_factory(Parser yyp) { return new binop_12(yyp); }
public static object fieldsep_2_factory(Parser yyp) { return new fieldsep_2(yyp); }
public static object TableRef_factory(Parser yyp) { return new TableRef(yyp); }
public static object function_1_factory(Parser yyp) { return new function_1(yyp); }
public static object PackageRef_1_factory(Parser yyp) { return new PackageRef_1(yyp); }
public static object funcbody_2_factory(Parser yyp) { return new funcbody_2(yyp); }
public static object chunk_4_factory(Parser yyp) { return new chunk_4(yyp); }
public static object namelist_factory(Parser yyp) { return new namelist(yyp); }
public static object Retval_factory(Parser yyp) { return new Retval(yyp); }
public static object LocalInit_factory(Parser yyp) { return new LocalInit(yyp); }
public static object functioncall_1_factory(Parser yyp) { return new functioncall_1(yyp); }
public static object parlist_factory(Parser yyp) { return new parlist(yyp); }
public static object binop_6_factory(Parser yyp) { return new binop_6(yyp); }
public static object arg_factory(Parser yyp) { return new arg(yyp); }
public static object funcname_factory(Parser yyp) { return new funcname(yyp); }
public static object SElseIf_factory(Parser yyp) { return new SElseIf(yyp); }
public static object prefixexp_3_factory(Parser yyp) { return new prefixexp_3(yyp); }
public static object SElse_factory(Parser yyp) { return new SElse(yyp); }
public static object binop_13_factory(Parser yyp) { return new binop_13(yyp); }
public static object init_1_factory(Parser yyp) { return new init_1(yyp); }
public static object stat_3_factory(Parser yyp) { return new stat_3(yyp); }
public static object varlist_2_factory(Parser yyp) { return new varlist_2(yyp); }
public static object binop_1_factory(Parser yyp) { return new binop_1(yyp); }
public static object chunk_2_factory(Parser yyp) { return new chunk_2(yyp); }
public static object fieldlist_1_factory(Parser yyp) { return new fieldlist_1(yyp); }
public static object field_1_factory(Parser yyp) { return new field_1(yyp); }
public static object stat_4_factory(Parser yyp) { return new stat_4(yyp); }
public static object field_factory(Parser yyp) { return new field(yyp); }
public static object Atom_2_factory(Parser yyp) { return new Atom_2(yyp); }
public static object funcbody_factory(Parser yyp) { return new funcbody(yyp); }
public static object funcbody_3_factory(Parser yyp) { return new funcbody_3(yyp); }
public static object LocalFuncDecl_factory(Parser yyp) { return new LocalFuncDecl(yyp); }
public static object var_factory(Parser yyp) { return new var(yyp); }
public static object TableRef_1_factory(Parser yyp) { return new TableRef_1(yyp); }
public static object binop_14_factory(Parser yyp) { return new binop_14(yyp); }
public static object Binop_1_factory(Parser yyp) { return new Binop_1(yyp); }
public static object SElseIf_1_factory(Parser yyp) { return new SElseIf_1(yyp); }
public static object LocalInit_1_factory(Parser yyp) { return new LocalInit_1(yyp); }
public static object tableconstructor_factory(Parser yyp) { return new tableconstructor(yyp); }
public static object PackageRef_factory(Parser yyp) { return new PackageRef(yyp); }
public static object function_factory(Parser yyp) { return new function(yyp); }
public static object Unop_1_factory(Parser yyp) { return new Unop_1(yyp); }
public static object SIf_1_factory(Parser yyp) { return new SIf_1(yyp); }
public static object namelist_1_factory(Parser yyp) { return new namelist_1(yyp); }
public static object funcname_2_factory(Parser yyp) { return new funcname_2(yyp); }
public static object stat_factory(Parser yyp) { return new stat(yyp); }
public static object unop_1_factory(Parser yyp) { return new unop_1(yyp); }
public static object exp_factory(Parser yyp) { return new exp(yyp); }
public static object FieldExpAssign_1_factory(Parser yyp) { return new FieldExpAssign_1(yyp); }
public static object fieldlist_factory(Parser yyp) { return new fieldlist(yyp); }
public static object arg_2_factory(Parser yyp) { return new arg_2(yyp); }
public static object arg_1_factory(Parser yyp) { return new arg_1(yyp); }
public static object arg_4_factory(Parser yyp) { return new arg_4(yyp); }
public static object binop_15_factory(Parser yyp) { return new binop_15(yyp); }
public static object unop_factory(Parser yyp) { return new unop(yyp); }
public static object FunctionCall_factory(Parser yyp) { return new FunctionCall(yyp); }
public static object binop_8_factory(Parser yyp) { return new binop_8(yyp); }
public static object Assignment_factory(Parser yyp) { return new Assignment(yyp); }
public static object tableconstructor_2_factory(Parser yyp) { return new tableconstructor_2(yyp); }
public static object Atom_3_factory(Parser yyp) { return new Atom_3(yyp); }
public static object arg_3_factory(Parser yyp) { return new arg_3(yyp); }
public static object If_factory(Parser yyp) { return new If(yyp); }
public static object Else_factory(Parser yyp) { return new Else(yyp); }
public static object Binop_factory(Parser yyp) { return new Binop(yyp); }
public static object explist_1_factory(Parser yyp) { return new explist_1(yyp); }
public static object explist_2_factory(Parser yyp) { return new explist_2(yyp); }
public static object SElse_1_factory(Parser yyp) { return new SElse_1(yyp); }
public static object binop_3_factory(Parser yyp) { return new binop_3(yyp); }
public static object ExpTableDec_1_factory(Parser yyp) { return new ExpTableDec_1(yyp); }
public static object block_2_factory(Parser yyp) { return new block_2(yyp); }
public static object chunk_factory(Parser yyp) { return new chunk(yyp); }
public static object fieldlist_3_factory(Parser yyp) { return new fieldlist_3(yyp); }
public static object block_1_factory(Parser yyp) { return new block_1(yyp); }
public static object binop_factory(Parser yyp) { return new binop(yyp); }
public static object Retval_1_factory(Parser yyp) { return new Retval_1(yyp); }
public static object funcname_3_factory(Parser yyp) { return new funcname_3(yyp); }
public static object unop_2_factory(Parser yyp) { return new unop_2(yyp); }
public static object Atom_factory(Parser yyp) { return new Atom(yyp); }
public static object If_1_factory(Parser yyp) { return new If_1(yyp); }
public static object fieldlist_2_factory(Parser yyp) { return new fieldlist_2(yyp); }
public static object var_1_factory(Parser yyp) { return new var_1(yyp); }
public static object ElseIf_1_factory(Parser yyp) { return new ElseIf_1(yyp); }
public static object stat_1_factory(Parser yyp) { return new stat_1(yyp); }
public static object varlist_1_factory(Parser yyp) { return new varlist_1(yyp); }
public static object SIf_factory(Parser yyp) { return new SIf(yyp); }
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
public static object FunctionCall_1_factory(Parser yyp) { return new FunctionCall_1(yyp); }
public static object init_factory(Parser yyp) { return new init(yyp); }
public static object FieldExpAssign_factory(Parser yyp) { return new FieldExpAssign(yyp); }
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
public static object binop_2_factory(Parser yyp) { return new binop_2(yyp); }
public static object exp_2_factory(Parser yyp) { return new exp_2(yyp); }
public static object exp_1_factory(Parser yyp) { return new exp_1(yyp); }
}
public class syntax: Parser {
public syntax():base(new yysyntax(),new tokens()) {}
public syntax(YyParser syms):base(syms,new tokens()) {}
public syntax(YyParser syms,ErrorHandler erh):base(syms,new tokens(erh)) {}

 }
