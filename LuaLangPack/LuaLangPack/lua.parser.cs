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
 public  void  FillScope ( LuaScope  scope ){ if ( s != null ){ s . FillScope ( scope );
}
 if ( c != null ){ c . FillScope ( scope );
}
}
 public  void  FillScope ( LuaScope  scope , LuaFunction  f ){ if ( s != null ){ s . FillScope ( scope , f );
}
 if ( c != null ){ c . FillScope ( scope , f );
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
 public  void  FillScope ( LuaScope  scope ){ if ( c != null ){ LuaScope  nested = new  LuaScope ( scope );
 c . FillScope ( nested );
 nested . outline = false ;
 scope . nested . AddLast ( nested );
}
}
 public  void  FillScope ( LuaScope  scope , int  begline , int  begchar , int  endline , int  endchar , bool  outline ){ if ( c != null ){ LuaScope  nested = new  LuaScope ( scope );
 c . FillScope ( nested );
 nested . beginLine = begline ;
 nested . beginIndx = begchar ;
 nested . endLine = endline ;
 nested . endIndx = endchar ;
 nested . outline = outline ;
 scope . nested . AddLast ( nested );
}
}
 public  void  FillScope ( LuaScope  scope , LuaFunction  f , int  begline , int  begchar , int  endline , int  endchar , bool  outline ){ if ( c != null ){ LuaScope  nested = new  LuaScope ( scope );
 c . FillScope ( nested , f );
 nested . beginLine = begline ;
 nested . beginIndx = begchar ;
 nested . endLine = endline ;
 nested . endIndx = endchar ;
 nested . outline = outline ;
 scope . nested . AddLast ( nested );
}
}

public override string yyname { get { return "block"; }}
public override int yynum { get { return 55; }}
}
//%+field+56
public class field : SYMBOL{
 private  exp  e ;
 public  field (Parser yyp, exp  a ):base(((syntax)yyp)){ e = a ;
}
 public  virtual  void  FillScope ( LuaScope  s ){ e . FillScope ( s );
}
 public  virtual  void  FillScope ( LuaScope  s , LuaTable  t ){ e . FillScope ( s );
}

public override string yyname { get { return "field"; }}
public override int yynum { get { return 56; }}
public field(Parser yyp):base(yyp){}}
//%+FieldExpAssign+57
public class FieldExpAssign : field{
 private  exp  e1 ;
 private  exp  e2 ;
 public  FieldExpAssign (Parser yyp, exp  a , exp  b ):base(((syntax)yyp)){ e1 = a ;
 e2 = b ;
}
 public  override  void  FillScope ( LuaScope  s ){ e1 . FillScope ( s );
 e2 . FillScope ( s );
}
 public  override  void  FillScope ( LuaScope  s , LuaTable  t ){ e1 . FillScope ( s );
 e2 . FillScope ( s );
}

public override string yyname { get { return "FieldExpAssign"; }}
public override int yynum { get { return 57; }}
public FieldExpAssign(Parser yyp):base(yyp){}}
//%+FieldAssign+58
public class FieldAssign : field{
 private  NAME  n ;
 private  exp  e ;
 public  FieldAssign (Parser yyp, NAME  a , exp  b ):base(((syntax)yyp)){ n = a ;
 e = b ;
}
 public  override  void  FillScope ( LuaScope  s ){ e . FillScope ( s );
}
 public  override  void  FillScope ( LuaScope  s , LuaTable  t ){ e . FillScope ( s );
 LuaName  name = new  LuaName ();
 name . name = n . s ;
 name . line = n . Line -1;
 name . pos = n . Position ;
 t . Add ( name );
}

public override string yyname { get { return "FieldAssign"; }}
public override int yynum { get { return 58; }}
public FieldAssign(Parser yyp):base(yyp){}}
//%+fieldlist+59
public class fieldlist : SYMBOL{
 private  fieldlist  fl ;
 private  field  f ;
 public  fieldlist (Parser yyp, field  a , fieldlist  b ):base(((syntax)yyp)){ f = a ;
 fl = b ;
}
 public  fieldlist (Parser yyp, field  a ):base(((syntax)yyp)){ f = a ;
}
 public  void  FillScope ( LuaScope  s ){ f . FillScope ( s );
 if ( fl != null ){ fl . FillScope ( s );
}
}
 public  void  FillScope ( LuaScope  s , LuaTable  t ){ f . FillScope ( s , t );
 if ( fl != null ){ fl . FillScope ( s , t );
}
}

public override string yyname { get { return "fieldlist"; }}
public override int yynum { get { return 59; }}
public fieldlist(Parser yyp):base(yyp){}}
//%+tableconstructor+60
public class tableconstructor : SYMBOL{
 private  fieldlist  f ;
 private  LBRACE  open ;
 private  RBRACE  close ;
 public  tableconstructor (Parser yyp, fieldlist  a , LBRACE  b , RBRACE  c ):base(((syntax)yyp)){ f = a ;
 open = b ;
 close = c ;
}
 public  void  FillScope ( LuaScope  s ){ f . FillScope ( s );
}
 public  void  FillScope ( LuaScope  s , var  v ){ LuaTable  table = v . ResolveTable ( s );
 table . line = close . Line -1;
 table . pos = close . Position ;
 if ( f != null ){ s . DeclareRegion ( open . Line -1, open . Position , close . Line -1, close . Position -1);
 f . FillScope ( s , table );
}
}
 public  void  FillScope ( LuaScope  s , NAME  n ){ LuaTable  table = new  LuaTable ();
 table . name = n . s ;
 table . line = close . Line -1;
 table . pos = close . Position ;
 if ( f != null ){ s . DeclareRegion ( open . Line -1, open . Position , close . Line -1, close . Position -1);
 f . FillScope ( s , table );
}
 s . Add ( table );
}

public override string yyname { get { return "tableconstructor"; }}
public override int yynum { get { return 60; }}
public tableconstructor(Parser yyp):base(yyp){}}
//%+namelist+61
public class namelist : SYMBOL{
 public  namelist  nl ;
 public  NAME  n ;
 public  namelist (Parser yyp, NAME  a , namelist  b ):base(((syntax)yyp)){ n = a ;
 nl = b ;
}
 public  namelist (Parser yyp, NAME  a ):base(((syntax)yyp)){ n = a ;
}
 public  virtual  void  FillScope ( LuaScope  s ){ if ( nl != null ){ nl . FillScope ( s );
}
 LuaName  name = new  LuaName ();
 name . name = n . s ;
 name . line = n . Line -1;
 name . pos = n . Position ;
 s . Add ( name );
}

public override string yyname { get { return "namelist"; }}
public override int yynum { get { return 61; }}
public namelist(Parser yyp):base(yyp){}}
//%+functioncall+62
public class functioncall : SYMBOL{
 private  prefixexp  p ;
 private  arg  m_a ;
 public  functioncall (Parser yyp, prefixexp  a , arg  b ):base(((syntax)yyp)){ p = a ;
 m_a = b ;
}
 public  void  FillScope ( LuaScope  s ){}
 public  void  FillScope ( LuaScope  s , varlist  vl ){ ILuaName  fname = p . Resolve ( s );
 if ( fname == null ) return ;
 LuaFunction  fun =( LuaFunction ) s . Lookup ( fname . name , Line -1, Position );
 if ( fun == null || fun . type != LuaType . Function || fun . RetStats . Count !=1) return ;
 else { fun . RetStats . First . Value . FillScope ( s , vl );
}
}
 public  void  FillScope ( LuaScope  s , namelist  nl ){ ILuaName  fname = p . Resolve ( s );
 if ( fname == null ) return ;
 LuaFunction  fun =( LuaFunction ) s . Lookup ( fname . name , Line -1, Position );
 if ( fun == null || fun . type != LuaType . Function || fun . RetStats . Count !=1) return ;
 else { fun . RetStats . First . Value . FillScope ( s , nl );
}
}
 public  void  FillScope ( LuaScope  s , NAME  n_left ){ ILuaName  fname = p . Resolve ( s );
 if ( fname == null ) return ;
 LuaFunction  fun =( LuaFunction ) s . Lookup ( fname . name , Line -1, Position );
 if ( fun == null || fun . type != LuaType . Function || fun . RetStats . Count !=1) return ;
 else { fun . RetStats . First . Value . FillScope ( s , n_left );
}
}
 public  void  FillScope ( LuaScope  s , var  v_left ){ ILuaName  fname = p . Resolve ( s );
 if ( fname == null ) return ;
 LuaFunction  fun =( LuaFunction ) s . Lookup ( fname . name , Line -1, Position );
 if ( fun == null || fun . type != LuaType . Function || fun . RetStats . Count !=1) return ;
 else { fun . RetStats . First . Value . FillScope ( s , v_left );
}
}
 public  ILuaName  Resolve ( LuaScope  s ){ ILuaName  fname = p . Resolve ( s );
 if ( fname == null ) return  null ;
 LuaFunction  fun =( LuaFunction ) s . Lookup ( fname . name , Line -1, Position );
 if ( fun == null || fun . type != LuaType . Function || fun . RetStats . Count !=1) return  null ;
 else { return  fun . RetStats . First . Value . Resolve ( s );
}
}

public override string yyname { get { return "functioncall"; }}
public override int yynum { get { return 62; }}
public functioncall(Parser yyp):base(yyp){}}
//%+funcname+63
public class funcname : SYMBOL{
 public  NAME  name ;
 public  funcname (Parser yyp, NAME  a ):base(((syntax)yyp)){ name = a ;
}
 public  void  FillScope ( LuaScope  scope ){}

public override string yyname { get { return "funcname"; }}
public override int yynum { get { return 63; }}
public funcname(Parser yyp):base(yyp){}}
//%+parlist+64
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
public override int yynum { get { return 64; }}
public parlist(Parser yyp):base(yyp){}}
//%+funcbody+65
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
 public  void  FillScope ( LuaScope  s ){ if ( b != null ){ b . FillScope ( s , paren . Line -1, paren . Position , e . Line -1, e . Position +3, true );
}
}
 public  void  FillScope ( LuaScope  s , LuaFunction  f ){ if ( b != null ){ b . FillScope ( s , f , paren . Line -1, paren . Position , e . Line -1, e . Position +3, true );
}
}

public override string yyname { get { return "funcbody"; }}
public override int yynum { get { return 65; }}
public funcbody(Parser yyp):base(yyp){}}
//%+prefixexp+66
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
 public  void  FillScope ( LuaScope  s ){ if ( v != null ){ v . FillScope ( s );
}
 else  if ( fc != null ){ fc . FillScope ( s );
}
 else  if ( e != null ){ e . FillScope ( s );
}
}
 public  void  FillScope ( LuaScope  s , var  v_left ){ if ( v != null ){ v . FillScope ( s , v_left );
}
 else  if ( fc != null ){ fc . FillScope ( s , v_left );
}
}
 public  void  FillScope ( LuaScope  s , NAME  n ){ if ( v != null ){ v . FillScope ( s , n );
}
 else  if ( fc != null ){ fc . FillScope ( s , n );
}
}
 public  void  FillScope ( LuaScope  s , varlist  vl ){ if ( fc != null ){ fc . FillScope ( s , vl );
}
 else { FillScope ( s , vl . v );
}
}
 public  void  FillScope ( LuaScope  s , namelist  nl ){ if ( fc != null ){ fc . FillScope ( s , nl );
}
 else { FillScope ( s , nl . n );
}
}
 public  ILuaName  Resolve ( LuaScope  s ){ if ( v != null ){ return  v . Resolve ( s );
}
 else  if ( fc != null ){ return  fc . Resolve ( s );
}
 else  return  null ;
}

public override string yyname { get { return "prefixexp"; }}
public override int yynum { get { return 66; }}
public prefixexp(Parser yyp):base(yyp){}}
//%+function+67
public class function : SYMBOL{
 private  funcbody  f ;
 public  function (Parser yyp, funcbody  a ):base(((syntax)yyp)){ f = a ;
}
 public  void  FillScope ( LuaScope  s ){ f . FillScope ( s );
}
 public  void  FillScope ( LuaScope  s , var  v_left ){ f . FillScope ( s );
}
 public  void  FillScope ( LuaScope  s , NAME  n_left ){ f . FillScope ( s );
}

public override string yyname { get { return "function"; }}
public override int yynum { get { return 67; }}
public function(Parser yyp):base(yyp){}}
//%+exp+68
public class exp : SYMBOL{
 private  function  f ;
 private  prefixexp  p ;
 private  tableconstructor  t ;
 private  bool  nil = false ;
 private  bool  number = false ;
 private  bool  bfalse = false ;
 private  bool  btrue = false ;
 private  LITERAL  l ;
 public  exp (Parser yyp, function  a ):base(((syntax)yyp)){ f = a ;
}
 public  exp (Parser yyp, prefixexp  b ):base(((syntax)yyp)){ p = b ;
}
 public  exp (Parser yyp, NIL  a ):base(((syntax)yyp)){ nil = true ;
}
 public  exp (Parser yyp, FALSE  a ):base(((syntax)yyp)){ bfalse = true ;
}
 public  exp (Parser yyp, TRUE  a ):base(((syntax)yyp)){ btrue = true ;
}
 public  exp (Parser yyp, NUMBER  a ):base(((syntax)yyp)){ number = true ;
}
 public  exp (Parser yyp, tableconstructor  c ):base(((syntax)yyp)){ t = c ;
}
 public  exp (Parser yyp, LITERAL  d ):base(((syntax)yyp)){ l = d ;
}
 public  void  FillScope ( LuaScope  s ){ if ( f != null ){ f . FillScope ( s );
}
 else  if ( p != null ){ p . FillScope ( s );
}
 else  if ( t != null ){ t . FillScope ( s );
}
}
 public  void  FillScope ( LuaScope  s , var  v ){ if ( f != null ){ f . FillScope ( s , v );
}
 else  if ( p != null ){ p . FillScope ( s , v );
}
 else  if ( t != null ){ t . FillScope ( s , v );
}
 else  if ( btrue || bfalse || number || nil ){ LuaName  rvalue = new  LuaName ();
 rvalue . name ="";
 rvalue . line = Line -1;
 rvalue . pos = Position ;
 v . Assign ( s , rvalue );
}
}
 public  void  FillScope ( LuaScope  s , NAME  n ){ if ( f != null ){ f . FillScope ( s , n );
}
 else  if ( p != null ){ p . FillScope ( s , n );
}
 else  if ( t != null ){ t . FillScope ( s , n );
}
 else  if ( btrue || bfalse || number || nil ){ LuaName  name = new  LuaName ();
 name . name = n . s ;
 name . pos = n . Position ;
 name . line = n . Line -1;
 s . Add ( name );
}
}
 public  void  FillScope ( LuaScope  s , varlist  v ){ if ( f != null ){ f . FillScope ( s , v . v );
}
 else  if ( p != null ){ p . FillScope ( s , v );
}
 else  if ( t != null ){ t . FillScope ( s , v . v );
}
 else  if ( btrue || bfalse || number || nil ){ LuaName  rvalue = new  LuaName ();
 rvalue . name ="";
 rvalue . line = Line -1;
 rvalue . pos = Position ;
 v . v . Assign ( s , rvalue );
}
 if ( v . vl != null && p == null ) v . vl . FillScope ( s );
}
 public  void  FillScope ( LuaScope  s , namelist  n ){ if ( f != null ){ f . FillScope ( s , n . n );
}
 else  if ( p != null ){ p . FillScope ( s , n );
}
 else  if ( t != null ){ t . FillScope ( s , n . n );
}
 else  if ( btrue || bfalse || number || nil ){ LuaName  name = new  LuaName ();
 name . name = n . n . s ;
 name . pos = n . n . Position ;
 name . line = n . n . Line -1;
 s . Add ( name );
}
 if ( n . nl != null && p == null ) n . nl . FillScope ( s );
}
 public  ILuaName  Resolve ( LuaScope  s ){ if ( l != null ){ LuaName  name = new  LuaName ();
 name . name = l . s ;
 name . pos = l . Position ;
 name . line = l . Line -1;
 return  name ;
}
 else  return  null ;
}

public override string yyname { get { return "exp"; }}
public override int yynum { get { return 68; }}
public exp(Parser yyp):base(yyp){}}
//%+explist+69
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
 public  void  FillScope ( LuaScope  s , var  v ){ e . FillScope ( s , v );
}
 public  void  FillScope ( LuaScope  s , NAME  n ){ e . FillScope ( s , n );
}
 public  void  FillScope ( LuaScope  s , varlist  v ){ if ( l != null ){ e . FillScope ( s , v . v );
 l . FillScope ( s , v . vl );
}
 else { e . FillScope ( s , v );
}
}
 public  void  FillScope ( LuaScope  s , namelist  n ){ if ( l != null ){ e . FillScope ( s , n . n );
 l . FillScope ( s , n . nl );
}
 else { e . FillScope ( s , n );
}
}
 public  ILuaName  Resolve ( LuaScope  s ){ return  e . Resolve ( s );
}

public override string yyname { get { return "explist"; }}
public override int yynum { get { return 69; }}
public explist(Parser yyp):base(yyp){}}
//%+var+70
public class var : SYMBOL{
 private  NAME  n ;
 public  var (Parser yyp, NAME  a ):base(((syntax)yyp)){ n = a ;
}
 public  virtual  void  FillScope ( LuaScope  s ){ if ( s . GlobalScope (). ShallowLookupName ( n . s )== null ){ LuaName  name = new  LuaName ();
 name . line = n . Line -1;
 name . pos = n . Position ;
 name . name = n . s ;
 s . GlobalScope (). Add ( name );
}
}
 public  virtual  void  FillScope ( LuaScope  s , var  v_left ){ ILuaName  rvalue = s . Lookup ( n . s , n . Line -1, n . Position );
 v_left . Assign ( s , rvalue );
}
 public  virtual  void  FillScope ( LuaScope  s , NAME  n_left ){ ILuaName  rvalue = s . Lookup ( n . s , n . Line -1, n . Position );
 BaseAssign ( s , rvalue , n_left );
}
 public  virtual  void  Assign ( LuaScope  s , ILuaName  rvalue ){ LuaScope  tmp = s ;
 if ( s . Lookup ( n . s , n . Line -1, n . Position )== null ){ tmp = s . GlobalScope ();
}
 BaseAssign ( s , rvalue , n );
}
 public  virtual  ILuaName  Resolve ( LuaScope  s ){ return  s . Lookup ( n . s , n . Line -1, n . Position );
}
 protected  void  BaseAssign ( LuaNamespace  s , ILuaName  rvalue , ILuaName  n_left ){ if ( rvalue == null || rvalue . type == LuaType . Name ){ LuaName  name = new  LuaName (( LuaName ) rvalue );
 name . line = n_left . line ;
 name . pos = n_left . pos ;
 name . name = n_left . name ;
 s . Add ( name );
}
 else  if ( rvalue . type == LuaType . Table ){ LuaTable  table = new  LuaTable (( LuaTable ) rvalue );
 table . line = n_left . line ;
 table . pos = n_left . pos ;
 table . name = n_left . name ;
 s . Add ( table );
}
 else  if ( rvalue . type == LuaType . Function ){ LuaFunction  fun = new  LuaFunction (( LuaFunction ) rvalue );
 fun . line = n_left . line ;
 fun . pos = n_left . pos ;
 fun . name = n_left . name ;
 s . Add ( fun );
}
}
 protected  void  BaseAssign ( LuaNamespace  s , ILuaName  rvalue , NAME  n_left ){ NAME  lname = n_left ;
 if ( rvalue == null || rvalue . type == LuaType . Name ){ LuaName  name = new  LuaName (( LuaName ) rvalue );
 name . line = lname . Line -1;
 name . pos = lname . Position ;
 name . name = lname . s ;
 s . Add ( name );
}
 else  if ( rvalue . type == LuaType . Table ){ LuaTable  table = new  LuaTable (( LuaTable ) rvalue );
 table . line = lname . Line -1;
 table . pos = lname . Position ;
 table . name = lname . s ;
 s . Add ( table );
}
 else  if ( rvalue . type == LuaType . Function ){ LuaFunction  fun = new  LuaFunction (( LuaFunction ) rvalue );
 fun . line = lname . Line -1;
 fun . pos = lname . Position ;
 fun . name = lname . s ;
 s . Add ( fun );
}
}
 public  virtual  LuaTable  ResolveTable ( LuaScope  s ){ LuaTable  t = s . LookupTable ( n . s , n . Line -1, n . Position );
 if ( t == null ){ t = new  LuaTable ();
 t . name = n . s ;
 s . GlobalScope (). Add ( t );
}
 return  t ;
}

public override string yyname { get { return "var"; }}
public override int yynum { get { return 70; }}
public var(Parser yyp):base(yyp){}}
//%+PackageRef+71
public class PackageRef : var{
 private  NAME  n ;
 private  prefixexp  p ;
 public  PackageRef (Parser yyp, prefixexp  a , NAME  b ):base(((syntax)yyp)){ p = a ;
 n = b ;
}
 public  override  void  FillScope ( LuaScope  s ){ p . FillScope ( s );
}
 public  override  void  FillScope ( LuaScope  s , var  v_left ){ ILuaName  right = p . Resolve ( s );
 if ( right == null || right . type != LuaType . Table ) return ;
 LuaTable  t =( LuaTable ) right ;
 ILuaName  rvalue = t . Lookup ( n . s , n . Line -1, n . Position );
 v_left . Assign ( s , rvalue );
}
 public  override  void  FillScope ( LuaScope  s , NAME  n_left ){ ILuaName  right = p . Resolve ( s );
 if ( right == null || right . type != LuaType . Table ) return ;
 LuaTable  t =( LuaTable ) right ;
 ILuaName  rvalue = t . Lookup ( n . s , n . Line -1, n . Position );
 BaseAssign ( s , rvalue , n_left );
}
 public  override  ILuaName  Resolve ( LuaScope  s ){ ILuaName  name = p . Resolve ( s );
 if ( name != null && name . type == LuaType . Table ){ LuaTable  t =( LuaTable ) name ;
 return  t . Lookup ( n . s , n . Line -1, n . Position );
}
 else  return  null ;
}
 public  override  void  Assign ( LuaScope  s , ILuaName  rvalue ){ ILuaName  left = p . Resolve ( s );
 if ( left == null || left . type != LuaType . Table ) return ;
 BaseAssign (( LuaTable ) left , rvalue , n );
}
 public  override  LuaTable  ResolveTable ( LuaScope  s ){ return  null ;
}

public override string yyname { get { return "PackageRef"; }}
public override int yynum { get { return 71; }}
public PackageRef(Parser yyp):base(yyp){}}
//%+TableRef+72
public class TableRef : var{
 private  prefixexp  p ;
 private  exp  e ;
 public  TableRef (Parser yyp, prefixexp  a , exp  b ):base(((syntax)yyp)){ p = a ;
 e = b ;
}
 public  override  void  FillScope ( LuaScope  s ){ p . FillScope ( s );
 e . FillScope ( s );
}
 public  override  void  FillScope ( LuaScope  s , var  v_left ){ ILuaName  right = p . Resolve ( s );
 ILuaName  l = e . Resolve ( s );
 if ( right == null || right . type != LuaType . Table ) return ;
 LuaTable  t =( LuaTable ) right ;
 ILuaName  rvalue = t . Lookup ( l . name , l . line , l . pos );
 v_left . Assign ( s , rvalue );
}
 public  override  void  FillScope ( LuaScope  s , NAME  n ){ ILuaName  right = p . Resolve ( s );
 ILuaName  l = e . Resolve ( s );
 if ( right == null || right . type != LuaType . Table ) return ;
 LuaTable  t =( LuaTable ) right ;
 ILuaName  rvalue = t . Lookup ( l . name , l . line , l . pos );
 BaseAssign ( s , rvalue , n );
}
 public  override  ILuaName  Resolve ( LuaScope  s ){ ILuaName  name = p . Resolve ( s );
 ILuaName  l = e . Resolve ( s );
 if ( name != null && name . type == LuaType . Table ){ LuaTable  t =( LuaTable ) name ;
 return  t . Lookup ( l . name , l . line , l . pos );
}
 else  return  null ;
}
 public  override  void  Assign ( LuaScope  s , ILuaName  rvalue ){ ILuaName  left = p . Resolve ( s );
 ILuaName  l = e . Resolve ( s );
 if ( left == null || left . type != LuaType . Table ) return ;
 BaseAssign (( LuaTable ) left , rvalue , l );
}
 public  override  LuaTable  ResolveTable ( LuaScope  s ){ return  null ;
}

public override string yyname { get { return "TableRef"; }}
public override int yynum { get { return 72; }}
public TableRef(Parser yyp):base(yyp){}}
//%+varlist+73
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
public override int yynum { get { return 73; }}
public varlist(Parser yyp):base(yyp){}}
//%+init+74
public class init : SYMBOL{
 explist  e ;
 public  init (Parser yyp, explist  a ):base(((syntax)yyp)){ e = a ;
}
 public  void  FillScope ( LuaScope  s , namelist  n ){ e . FillScope ( s , n );
}

public override string yyname { get { return "init"; }}
public override int yynum { get { return 74; }}
public init(Parser yyp):base(yyp){}}
//%+stat+75
public class stat : SYMBOL{
 public  stat (Parser yyp):base(((syntax)yyp)){}
 public  virtual  void  FillScope ( LuaScope  s ){}
 public  virtual  void  FillScope ( LuaScope  s , LuaFunction  f ){ FillScope ( s );
}

public override string yyname { get { return "stat"; }}
public override int yynum { get { return 75; }}
}
//%+Assignment+76
public class Assignment : stat{
 private  varlist  v ;
 private  explist  e ;
 public  Assignment (Parser yyp, varlist  a , explist  b ):base(((syntax)yyp)){ v = a ;
 e = b ;
}
 public  override  void  FillScope ( LuaScope  s ){ e . FillScope ( s , v );
}

public override string yyname { get { return "Assignment"; }}
public override int yynum { get { return 76; }}
public Assignment(Parser yyp):base(yyp){}}
//%+LocalInit+77
public class LocalInit : stat{
 private  namelist  n ;
 private  init  i ;
 public  LocalInit (Parser yyp, namelist  a , init  b ):base(((syntax)yyp)){ n = a ;
 i = b ;
}
 public  override  void  FillScope ( LuaScope  s ){ i . FillScope ( s , n );
}

public override string yyname { get { return "LocalInit"; }}
public override int yynum { get { return 77; }}
public LocalInit(Parser yyp):base(yyp){}}
//%+LocalNamelist+78
public class LocalNamelist : stat{
 private  namelist  n ;
 public  LocalNamelist (Parser yyp, namelist  a ):base(((syntax)yyp)){ n = a ;
}
 public  override  void  FillScope ( LuaScope  s ){ n . FillScope ( s );
}

public override string yyname { get { return "LocalNamelist"; }}
public override int yynum { get { return 78; }}
public LocalNamelist(Parser yyp):base(yyp){}}
//%+Retval+79
public class Retval : stat{
 private  explist  e ;
 public  Retval (Parser yyp, explist  a ):base(((syntax)yyp)){ e = a ;
}
 public  override  void  FillScope ( LuaScope  s ){ e . FillScope ( s );
}
 public  override  void  FillScope ( LuaScope  s , LuaFunction  f ){ f . Add ( e );
}

public override string yyname { get { return "Retval"; }}
public override int yynum { get { return 79; }}
public Retval(Parser yyp):base(yyp){}}
//%+FuncDecl+80
public class FuncDecl : stat{
 funcname  fname ;
 funcbody  body ;
 public  FuncDecl (Parser yyp, funcname  a , funcbody  b ):base(((syntax)yyp)){ fname = a ;
 body = b ;
}
 public  override  void  FillScope ( LuaScope  s ){ LuaFunction  f = new  LuaFunction ();
 f . name = fname . name . s ;
 f . line = body . Line -1;
 f . pos = body . Position ;
 if ( s . GlobalScope (). ShallowLookupFunction ( fname . name . s )== null ) s . GlobalScope (). Add ( f );
 body . FillScope ( s , f );
}

public override string yyname { get { return "FuncDecl"; }}
public override int yynum { get { return 80; }}
public FuncDecl(Parser yyp):base(yyp){}}
//%+LocalFuncDecl+81
public class LocalFuncDecl : stat{
 funcbody  body ;
 NAME  name ;
 public  LocalFuncDecl (Parser yyp, NAME  a , funcbody  b ):base(((syntax)yyp)){ name = a ;
 body = b ;
}
 public  override  void  FillScope ( LuaScope  s ){ LuaFunction  f = new  LuaFunction ();
 f . name = name . s ;
 f . line = body . Line -1;
 f . pos = body . Position ;
 s . Add ( f );
 body . FillScope ( s , f );
}

public override string yyname { get { return "LocalFuncDecl"; }}
public override int yynum { get { return 81; }}
public LocalFuncDecl(Parser yyp):base(yyp){}}
//%+FunctionCall+82
public class FunctionCall : stat{
 prefixexp  p ;
 public  FunctionCall (Parser yyp, prefixexp  a ):base(((syntax)yyp)){ p = a ;
}
 public  override  void  FillScope ( LuaScope  scope ){ p . FillScope ( scope );
}

public override string yyname { get { return "FunctionCall"; }}
public override int yynum { get { return 82; }}
public FunctionCall(Parser yyp):base(yyp){}}
//%+Do+83
public class Do : stat{
 DO  d ;
 block  blk ;
 END  end ;
 public  Do (Parser yyp, DO  a , block  b , END  e ):base(((syntax)yyp)){ d = a ;
 blk = b ;
 end = e ;
}
 public  override  void  FillScope ( LuaScope  s ){ blk . FillScope ( s , d . Line -1, d . Position +1, end . Line -1, end . Position +3, true );
}

public override string yyname { get { return "Do"; }}
public override int yynum { get { return 83; }}
public Do(Parser yyp):base(yyp){}}
//%+While+84
public class While : stat{
 DO  d ;
 block  blk ;
 END  end ;
 public  While (Parser yyp, DO  a , block  b , END  e ):base(((syntax)yyp)){ d = a ;
 blk = b ;
 end = e ;
}
 public  override  void  FillScope ( LuaScope  s ){ blk . FillScope ( s , d . Line -1, d . Position +1, end . Line -1, end . Position +3, false );
}

public override string yyname { get { return "While"; }}
public override int yynum { get { return 84; }}
public While(Parser yyp):base(yyp){}}
//%+Repeat+85
public class Repeat : stat{
 REPEAT  rep ;
 block  blk ;
 UNTIL  until ;
 public  Repeat (Parser yyp, REPEAT  a , block  b , UNTIL  e ):base(((syntax)yyp)){ rep = a ;
 blk = b ;
 until = e ;
}
 public  override  void  FillScope ( LuaScope  s ){ blk . FillScope ( s , rep . Line -1, rep . Position +5, until . Line -1, until . Position , false );
}

public override string yyname { get { return "Repeat"; }}
public override int yynum { get { return 85; }}
public Repeat(Parser yyp):base(yyp){}}
//%+For+86
public class For : stat{
 DO  d ;
 block  blk ;
 END  end ;
 public  For (Parser yyp, DO  a , block  b , END  e ):base(((syntax)yyp)){ d = a ;
 blk = b ;
 end = e ;
}
 public  override  void  FillScope ( LuaScope  s ){ blk . FillScope ( s , d . Line -1, d . Position +1, end . Line -1, end . Position +3, false );
}

public override string yyname { get { return "For"; }}
public override int yynum { get { return 86; }}
public For(Parser yyp):base(yyp){}}
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
 else  if ( t != null ){}
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

public class Do_1 : Do {
  public Do_1(Parser yyq):base(yyq, 
	((DO)(yyq.StackAt(2).m_value))
	, 
	((block)(yyq.StackAt(1).m_value))
	, 
	((END)(yyq.StackAt(0).m_value))
	 ){}}

public class stat_1 : stat {
  public stat_1(Parser yyq):base(yyq){}}

public class While_1 : While {
  public While_1(Parser yyq):base(yyq, 
	((DO)(yyq.StackAt(2).m_value))
	, 
	((block)(yyq.StackAt(1).m_value))
	, 
	((END)(yyq.StackAt(0).m_value))
	 ){}}

public class stat_2 : stat {
  public stat_2(Parser yyq):base(yyq){}}

public class Repeat_1 : Repeat {
  public Repeat_1(Parser yyq):base(yyq, 
	((REPEAT)(yyq.StackAt(3).m_value))
	, 
	((block)(yyq.StackAt(2).m_value))
	, 
	((UNTIL)(yyq.StackAt(1).m_value))
	 ){}}

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

public class stat_3 : stat {
  public stat_3(Parser yyq):base(yyq){}}

public class Retval_1 : Retval {
  public Retval_1(Parser yyq):base(yyq, 
	((explist)(yyq.StackAt(0).m_value))
	 ){}}

public class stat_4 : stat {
  public stat_4(Parser yyq):base(yyq){}}

public class For_1 : For {
  public For_1(Parser yyq):base(yyq, 
	((DO)(yyq.StackAt(2).m_value))
	, 
	((block)(yyq.StackAt(1).m_value))
	, 
	((END)(yyq.StackAt(0).m_value))
	 ){}}

public class For_2 : For {
  public For_2(Parser yyq):base(yyq, 
	((DO)(yyq.StackAt(2).m_value))
	, 
	((block)(yyq.StackAt(1).m_value))
	, 
	((END)(yyq.StackAt(0).m_value))
	 ){}}

public class For_3 : For {
  public For_3(Parser yyq):base(yyq, 
	((DO)(yyq.StackAt(2).m_value))
	, 
	((block)(yyq.StackAt(1).m_value))
	, 
	((END)(yyq.StackAt(0).m_value))
	 ){}}

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

public class LocalNamelist_1 : LocalNamelist {
  public LocalNamelist_1(Parser yyq):base(yyq, 
	((namelist)(yyq.StackAt(0).m_value))
	 ){}}

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
  public tableconstructor_1(Parser yyq):base(yyq, null, 
	((LBRACE)(yyq.StackAt(1).m_value))
	, 
	((RBRACE)(yyq.StackAt(0).m_value))
	 ){}}

public class tableconstructor_2 : tableconstructor {
  public tableconstructor_2(Parser yyq):base(yyq, 
	((fieldlist)(yyq.StackAt(1).m_value))
	, 
	((LBRACE)(yyq.StackAt(2).m_value))
	, 
	((RBRACE)(yyq.StackAt(0).m_value))
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

public class exp_1 : exp {
  public exp_1(Parser yyq):base(yyq, 
	((NIL)(yyq.StackAt(0).m_value))
	 ){}}

public class exp_2 : exp {
  public exp_2(Parser yyq):base(yyq, 
	((FALSE)(yyq.StackAt(0).m_value))
	 ){}}

public class exp_3 : exp {
  public exp_3(Parser yyq):base(yyq, 
	((TRUE)(yyq.StackAt(0).m_value))
	 ){}}

public class exp_4 : exp {
  public exp_4(Parser yyq):base(yyq, 
	((NUMBER)(yyq.StackAt(0).m_value))
	 ){}}

public class exp_5 : exp {
  public exp_5(Parser yyq):base(yyq, 
	((LITERAL)(yyq.StackAt(0).m_value))
	 ){}}

public class exp_6 : exp {
  public exp_6(Parser yyq):base(yyq, 
	((function)(yyq.StackAt(0).m_value))
	 ){}}

public class exp_7 : exp {
  public exp_7(Parser yyq):base(yyq, 
	((prefixexp)(yyq.StackAt(0).m_value))
	 ){}}

public class exp_8 : exp {
  public exp_8(Parser yyq):base(yyq, 
	((tableconstructor)(yyq.StackAt(0).m_value))
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

public class namelist_1 : namelist {
  public namelist_1(Parser yyq):base(yyq, 
	((NAME)(yyq.StackAt(0).m_value))
	 ){}}

public class namelist_2 : namelist {
  public namelist_2(Parser yyq):base(yyq, 
	((NAME)(yyq.StackAt(2).m_value))
	, 
	((namelist)(yyq.StackAt(0).m_value))
	 ){}}

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
public class unop : SYMBOL {
	public unop(Parser yyq):base(yyq) { }
  public override string yyname { get { return "unop"; }}
  public override int yynum { get { return 144; }}}
public class binop : SYMBOL {
	public binop(Parser yyq):base(yyq) { }
  public override string yyname { get { return "binop"; }}
  public override int yynum { get { return 143; }}}
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

public class funcname_2 : funcname {
  public funcname_2(Parser yyq):base(yyq){}}

public class funcname_3 : funcname {
  public funcname_3(Parser yyq):base(yyq){}}

public class fieldsep_1 : fieldsep {
  public fieldsep_1(Parser yyq):base(yyq){}}

public class fieldsep_2 : fieldsep {
  public fieldsep_2(Parser yyq):base(yyq){}}

public class exp_9 : exp {
  public exp_9(Parser yyq):base(yyq){}}

public class exp_10 : exp {
  public exp_10(Parser yyq):base(yyq){}}

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

public class parlist_3 : parlist {
  public parlist_3(Parser yyq):base(yyq){}}

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
1,1090,102,2,0,
105,5,178,1,517,
106,18,1,517,107,
20,108,4,6,101,
0,120,0,112,0,
1,68,1,2,2,
0,1,1031,109,18,
1,1031,110,20,111,
4,6,69,0,78,
0,68,0,1,38,
1,1,2,0,1,
1030,112,18,1,1030,
113,20,114,4,10,
98,0,108,0,111,
0,99,0,107,0,
1,55,1,2,2,
0,1,1029,115,18,
1,1029,110,2,0,
1,1028,116,18,1,
1028,117,20,118,4,
12,82,0,80,0,
65,0,82,0,69,
0,78,0,1,11,
1,1,2,0,1,
506,119,18,1,506,
120,20,121,4,10,
67,0,79,0,77,
0,77,0,65,0,
1,7,1,1,2,
0,1,489,122,18,
1,489,107,2,0,
1,1007,123,18,1,
1007,110,2,0,1,
1006,124,18,1,1006,
113,2,0,1,478,
125,18,1,478,120,
2,0,1,998,126,
18,1,998,127,20,
128,4,4,68,0,
79,0,1,41,1,
1,2,0,1,997,
129,18,1,997,130,
20,131,4,14,101,
0,120,0,112,0,
108,0,105,0,115,
0,116,0,1,69,
1,2,2,0,1,
461,132,18,1,461,
107,2,0,1,977,
133,18,1,977,134,
20,135,4,4,73,
0,78,0,1,42,
1,1,2,0,1,
976,136,18,1,976,
137,20,138,4,16,
110,0,97,0,109,
0,101,0,108,0,
105,0,115,0,116,
0,1,61,1,2,
2,0,1,450,139,
18,1,450,140,20,
141,4,12,65,0,
83,0,83,0,73,
0,71,0,78,0,
1,33,1,1,2,
0,1,448,142,18,
1,448,143,20,144,
4,8,78,0,65,
0,77,0,69,0,
1,3,1,1,2,
0,1,447,145,18,
1,447,146,20,147,
4,6,70,0,79,
0,82,0,1,40,
1,1,2,0,1,
446,148,18,1,446,
149,20,150,4,16,
102,0,117,0,110,
0,99,0,98,0,
111,0,100,0,121,
0,1,65,1,2,
2,0,1,444,151,
18,1,444,152,20,
153,4,16,102,0,
117,0,110,0,99,
0,110,0,97,0,
109,0,101,0,1,
63,1,2,2,0,
1,443,154,18,1,
443,152,2,0,1,
441,155,18,1,441,
156,20,157,4,6,
68,0,79,0,84,
0,1,16,1,1,
2,0,1,440,158,
18,1,440,143,2,
0,1,439,159,18,
1,439,160,20,161,
4,10,67,0,79,
0,76,0,79,0,
78,0,1,8,1,
1,2,0,1,438,
162,18,1,438,143,
2,0,1,437,163,
18,1,437,164,20,
165,4,16,70,0,
85,0,78,0,67,
0,84,0,73,0,
79,0,78,0,1,
45,1,1,2,0,
1,436,166,18,1,
436,149,2,0,1,
434,167,18,1,434,
143,2,0,1,433,
168,18,1,433,164,
2,0,1,432,169,
18,1,432,170,20,
171,4,8,105,0,
110,0,105,0,116,
0,1,74,1,2,
2,0,1,430,172,
18,1,430,130,2,
0,1,950,173,18,
1,950,110,2,0,
1,949,174,18,1,
949,113,2,0,1,
421,175,18,1,421,
130,2,0,1,940,
176,18,1,940,127,
2,0,1,402,177,
18,1,402,120,2,
0,1,910,178,18,
1,910,110,2,0,
1,909,179,18,1,
909,110,2,0,1,
908,180,18,1,908,
181,20,182,4,12,
101,0,108,0,115,
0,101,0,105,0,
102,0,1,90,1,
2,2,0,1,385,
183,18,1,385,107,
2,0,1,374,184,
18,1,374,140,2,
0,1,373,185,18,
1,373,137,2,0,
1,371,186,18,1,
371,137,2,0,1,
370,187,18,1,370,
120,2,0,1,369,
188,18,1,369,143,
2,0,1,368,189,
18,1,368,190,20,
191,4,10,76,0,
79,0,67,0,65,
0,76,0,1,49,
1,1,2,0,1,
887,192,18,1,887,
181,2,0,1,363,
193,18,1,363,194,
20,195,4,14,118,
0,97,0,114,0,
108,0,105,0,115,
0,116,0,1,73,
1,2,2,0,1,
848,196,18,1,848,
197,20,198,4,8,
69,0,76,0,83,
0,69,0,1,36,
1,1,2,0,1,
867,199,18,1,867,
200,20,201,4,12,
69,0,76,0,83,
0,69,0,73,0,
70,0,1,37,1,
1,2,0,1,343,
202,18,1,343,203,
20,204,4,12,82,
0,66,0,82,0,
65,0,67,0,75,
0,1,13,1,1,
2,0,1,854,205,
18,1,854,113,2,
0,1,327,206,18,
1,327,107,2,0,
1,847,207,18,1,
847,113,2,0,1,
325,208,18,1,325,
209,20,210,4,6,
97,0,114,0,103,
0,1,94,1,2,
2,0,1,324,211,
18,1,324,212,20,
213,4,18,102,0,
105,0,101,0,108,
0,100,0,108,0,
105,0,115,0,116,
0,1,59,1,2,
2,0,1,841,214,
18,1,841,215,20,
216,4,8,84,0,
72,0,69,0,78,
0,1,35,1,1,
2,0,1,824,217,
18,1,824,107,2,
0,1,302,218,18,
1,302,219,20,220,
4,16,102,0,105,
0,101,0,108,0,
100,0,115,0,101,
0,112,0,1,125,
1,2,2,0,1,
301,221,18,1,301,
120,2,0,1,300,
222,18,1,300,223,
20,224,4,18,83,
0,69,0,77,0,
73,0,67,0,79,
0,76,0,79,0,
78,0,1,9,1,
1,2,0,1,299,
225,18,1,299,226,
20,227,4,10,102,
0,105,0,101,0,
108,0,100,0,1,
56,1,2,2,0,
1,298,228,18,1,
298,229,20,230,4,
12,82,0,66,0,
82,0,65,0,67,
0,69,0,1,15,
1,1,2,0,1,
296,231,18,1,296,
229,2,0,1,295,
232,18,1,295,212,
2,0,1,813,233,
18,1,813,200,2,
0,1,792,234,18,
1,792,102,2,0,
1,789,235,18,1,
789,102,2,0,1,
773,236,18,1,773,
223,2,0,1,757,
237,18,1,757,238,
20,239,4,8,115,
0,116,0,97,0,
116,0,1,75,1,
2,2,0,1,756,
240,18,1,756,102,
2,0,1,755,241,
18,1,755,130,2,
0,1,209,242,18,
1,209,140,2,0,
1,735,243,18,1,
735,140,2,0,1,
734,244,18,1,734,
194,2,0,1,733,
245,18,1,733,110,
2,0,1,732,246,
18,1,732,113,2,
0,1,210,247,18,
1,210,107,2,0,
1,730,248,18,1,
730,110,2,0,1,
208,249,18,1,208,
203,2,0,1,207,
250,18,1,207,107,
2,0,1,206,251,
18,1,206,252,20,
253,4,12,76,0,
66,0,82,0,65,
0,67,0,75,0,
1,12,1,1,2,
0,1,716,254,18,
1,716,127,2,0,
1,715,255,18,1,
715,110,2,0,1,
714,256,18,1,714,
113,2,0,1,712,
257,18,1,712,110,
2,0,1,699,258,
18,1,699,127,2,
0,1,171,259,18,
1,171,107,2,0,
1,170,260,18,1,
170,140,2,0,1,
169,261,18,1,169,
143,2,0,1,682,
262,18,1,682,107,
2,0,1,671,263,
18,1,671,264,20,
265,4,10,87,0,
72,0,73,0,76,
0,69,0,1,39,
1,1,2,0,1,
144,266,18,1,144,
107,2,0,1,131,
267,18,1,131,268,
20,269,4,6,78,
0,73,0,76,0,
1,44,1,1,2,
0,1,130,270,18,
1,130,271,20,272,
4,10,70,0,65,
0,76,0,83,0,
69,0,1,51,1,
1,2,0,1,129,
273,18,1,129,274,
20,275,4,8,84,
0,82,0,85,0,
69,0,1,50,1,
1,2,0,1,128,
276,18,1,128,277,
20,278,4,12,78,
0,85,0,77,0,
66,0,69,0,82,
0,1,5,1,1,
2,0,1,127,279,
18,1,127,280,20,
281,4,14,76,0,
73,0,84,0,69,
0,82,0,65,0,
76,0,1,4,1,
1,2,0,1,126,
282,18,1,126,283,
20,284,4,16,102,
0,117,0,110,0,
99,0,116,0,105,
0,111,0,110,0,
1,67,1,2,2,
0,1,125,285,18,
1,125,286,20,287,
4,32,116,0,97,
0,98,0,108,0,
101,0,99,0,111,
0,110,0,115,0,
116,0,114,0,117,
0,99,0,116,0,
111,0,114,0,1,
60,1,2,2,0,
1,645,288,18,1,
645,107,2,0,1,
1091,289,18,1,1091,
290,23,291,4,6,
69,0,79,0,70,
0,1,2,1,6,
2,0,1,618,292,
18,1,618,113,2,
0,1,634,293,18,
1,634,294,20,295,
4,10,85,0,78,
0,84,0,73,0,
76,0,1,47,1,
1,2,0,1,633,
296,18,1,633,113,
2,0,1,107,297,
18,1,107,107,2,
0,1,621,298,18,
1,621,299,20,300,
4,12,82,0,69,
0,80,0,69,0,
65,0,84,0,1,
46,1,1,2,0,
1,619,301,18,1,
619,110,2,0,1,
97,302,18,1,97,
303,20,304,4,8,
117,0,110,0,111,
0,112,0,1,144,
1,2,2,0,1,
96,305,18,1,96,
306,20,307,4,6,
118,0,97,0,114,
0,1,70,1,2,
2,0,1,95,308,
18,1,95,309,20,
310,4,24,102,0,
117,0,110,0,99,
0,116,0,105,0,
111,0,110,0,99,
0,97,0,108,0,
108,0,1,62,1,
2,2,0,1,92,
311,18,1,92,117,
2,0,1,1027,312,
18,1,1027,313,20,
314,4,14,112,0,
97,0,114,0,108,
0,105,0,115,0,
116,0,1,64,1,
2,2,0,1,607,
315,18,1,607,197,
2,0,1,606,316,
18,1,606,113,2,
0,1,1090,104,1,
76,317,18,1,76,
107,2,0,1,1053,
318,18,1,1053,313,
2,0,1,578,319,
18,1,578,107,2,
0,1,543,320,18,
1,543,113,2,0,
1,595,321,18,1,
595,215,2,0,1,
69,322,18,1,69,
323,20,324,4,12,
76,0,80,0,65,
0,82,0,69,0,
78,0,1,10,1,
1,2,0,1,567,
325,18,1,567,326,
20,327,4,4,73,
0,70,0,1,34,
1,1,2,0,1,
566,328,18,1,566,
130,2,0,1,62,
329,18,1,62,330,
20,331,4,10,98,
0,105,0,110,0,
111,0,112,0,1,
143,1,2,2,0,
1,61,332,18,1,
61,333,20,334,4,
8,80,0,76,0,
85,0,83,0,1,
17,1,1,2,0,
1,60,335,18,1,
60,336,20,337,4,
10,77,0,73,0,
78,0,85,0,83,
0,1,18,1,1,
2,0,1,59,338,
18,1,59,339,20,
340,4,8,77,0,
85,0,76,0,84,
0,1,19,1,1,
2,0,1,58,341,
18,1,58,342,20,
343,4,6,77,0,
79,0,68,0,1,
21,1,1,2,0,
1,57,344,18,1,
57,345,20,346,4,
12,68,0,73,0,
86,0,73,0,68,
0,69,0,1,22,
1,1,2,0,1,
56,347,18,1,56,
348,20,349,4,6,
69,0,88,0,80,
0,1,23,1,1,
2,0,1,55,350,
18,1,55,351,20,
352,4,12,67,0,
79,0,78,0,67,
0,65,0,84,0,
1,52,1,1,2,
0,1,54,353,18,
1,54,354,20,355,
4,4,76,0,84,
0,1,26,1,1,
2,0,1,53,356,
18,1,53,357,20,
358,4,4,71,0,
84,0,1,28,1,
1,2,0,1,52,
359,18,1,52,360,
20,361,4,4,71,
0,69,0,1,29,
1,1,2,0,1,
51,362,18,1,51,
363,20,364,4,4,
76,0,69,0,1,
27,1,1,2,0,
1,50,365,18,1,
50,366,20,367,4,
4,69,0,81,0,
1,24,1,1,2,
0,1,49,368,18,
1,49,369,20,370,
4,6,65,0,78,
0,68,0,1,30,
1,1,2,0,1,
48,371,18,1,48,
372,20,373,4,4,
79,0,82,0,1,
31,1,1,2,0,
1,47,374,18,1,
47,375,20,376,4,
6,78,0,69,0,
81,0,1,25,1,
1,2,0,1,46,
377,18,1,46,107,
2,0,1,45,378,
18,1,45,379,20,
380,4,12,76,0,
66,0,82,0,65,
0,67,0,69,0,
1,14,1,1,2,
0,1,44,381,18,
1,44,209,2,0,
1,1051,382,18,1,
1051,120,2,0,1,
1050,383,18,1,1050,
143,2,0,1,1049,
384,18,1,1049,385,
20,386,4,12,69,
0,76,0,73,0,
80,0,83,0,69,
0,1,53,1,1,
2,0,1,40,387,
18,1,40,143,2,
0,1,39,388,18,
1,39,160,2,0,
1,33,389,18,1,
33,390,20,391,4,
18,112,0,114,0,
101,0,102,0,105,
0,120,0,101,0,
120,0,112,0,1,
66,1,2,2,0,
1,534,392,18,1,
534,127,2,0,1,
28,393,18,1,28,
252,2,0,1,27,
394,18,1,27,143,
2,0,1,26,395,
18,1,26,156,2,
0,1,546,396,18,
1,546,397,20,398,
4,12,82,0,69,
0,84,0,85,0,
82,0,78,0,1,
48,1,1,2,0,
1,545,399,18,1,
545,400,20,401,4,
10,66,0,82,0,
69,0,65,0,75,
0,1,43,1,1,
2,0,1,544,402,
18,1,544,110,2,
0,1,22,403,18,
1,22,390,2,0,
1,21,404,18,1,
21,120,2,0,1,
20,405,18,1,20,
306,2,0,1,19,
406,18,1,19,143,
2,0,1,17,407,
18,1,17,110,2,
0,1,16,408,18,
1,16,113,2,0,
1,15,409,18,1,
15,110,2,0,1,
14,410,18,1,14,
117,2,0,1,13,
411,18,1,13,323,
2,0,1,12,412,
18,1,12,149,2,
0,1,11,413,18,
1,11,164,2,0,
1,10,414,18,1,
10,117,2,0,1,
9,415,18,1,9,
117,2,0,1,8,
416,18,1,8,130,
2,0,1,7,417,
18,1,7,336,2,
0,1,6,418,18,
1,6,419,20,420,
4,6,78,0,79,
0,84,0,1,32,
1,1,2,0,1,
5,421,18,1,5,
422,20,423,4,10,
80,0,79,0,85,
0,78,0,68,0,
1,20,1,1,2,
0,1,4,424,18,
1,4,323,2,0,
1,3,425,18,1,
3,286,2,0,1,
2,426,18,1,2,
280,2,0,1,1,
427,18,1,1,390,
2,0,1,0,428,
18,1,0,0,2,
0,429,5,0,430,
5,192,1,194,431,
19,432,4,10,97,
0,114,0,103,0,
95,0,52,0,1,
194,433,5,4,1,
40,434,16,0,381,
1,22,435,16,0,
208,1,1,436,16,
0,208,1,33,437,
16,0,208,1,193,
438,19,439,4,12,
117,0,110,0,111,
0,112,0,95,0,
51,0,1,193,440,
5,23,1,374,441,
16,0,302,1,567,
442,16,0,302,1,
546,443,16,0,302,
1,977,444,16,0,
302,1,506,445,16,
0,302,1,867,446,
16,0,302,1,735,
447,16,0,302,1,
170,448,16,0,302,
1,28,449,16,0,
302,1,450,450,16,
0,302,1,402,451,
16,0,302,1,634,
452,16,0,302,1,
69,453,16,0,302,
1,209,454,16,0,
302,1,302,455,16,
0,302,1,206,456,
16,0,302,1,62,
457,16,0,302,1,
813,458,16,0,302,
1,671,459,16,0,
302,1,478,460,16,
0,302,1,4,461,
16,0,302,1,97,
462,16,0,302,1,
45,463,16,0,302,
1,192,464,19,465,
4,12,117,0,110,
0,111,0,112,0,
95,0,50,0,1,
192,440,1,191,466,
19,467,4,12,117,
0,110,0,111,0,
112,0,95,0,49,
0,1,191,440,1,
190,468,19,469,4,
10,97,0,114,0,
103,0,95,0,51,
0,1,190,433,1,
189,470,19,471,4,
18,112,0,97,0,
114,0,108,0,105,
0,115,0,116,0,
95,0,51,0,1,
189,472,5,2,1,
1051,473,16,0,318,
1,13,474,16,0,
312,1,188,475,19,
476,4,16,98,0,
105,0,110,0,111,
0,112,0,95,0,
49,0,53,0,1,
188,477,5,16,1,
645,478,16,0,329,
1,107,479,16,0,
329,1,824,480,16,
0,329,1,385,481,
16,0,329,1,682,
482,16,0,329,1,
578,483,16,0,329,
1,144,484,16,0,
329,1,517,485,16,
0,329,1,171,486,
16,0,329,1,46,
487,16,0,329,1,
76,488,16,0,329,
1,489,489,16,0,
329,1,327,490,16,
0,329,1,210,491,
16,0,329,1,461,
492,16,0,329,1,
207,493,16,0,329,
1,187,494,19,495,
4,16,98,0,105,
0,110,0,111,0,
112,0,95,0,49,
0,52,0,1,187,
477,1,186,496,19,
497,4,16,98,0,
105,0,110,0,111,
0,112,0,95,0,
49,0,51,0,1,
186,477,1,185,498,
19,499,4,16,98,
0,105,0,110,0,
111,0,112,0,95,
0,49,0,50,0,
1,185,477,1,184,
500,19,501,4,16,
98,0,105,0,110,
0,111,0,112,0,
95,0,49,0,49,
0,1,184,477,1,
183,502,19,503,4,
16,98,0,105,0,
110,0,111,0,112,
0,95,0,49,0,
48,0,1,183,477,
1,182,504,19,505,
4,14,98,0,105,
0,110,0,111,0,
112,0,95,0,57,
0,1,182,477,1,
181,506,19,507,4,
14,98,0,105,0,
110,0,111,0,112,
0,95,0,56,0,
1,181,477,1,180,
508,19,509,4,14,
98,0,105,0,110,
0,111,0,112,0,
95,0,55,0,1,
180,477,1,179,510,
19,511,4,14,98,
0,105,0,110,0,
111,0,112,0,95,
0,54,0,1,179,
477,1,178,512,19,
513,4,14,98,0,
105,0,110,0,111,
0,112,0,95,0,
53,0,1,178,477,
1,177,514,19,515,
4,14,98,0,105,
0,110,0,111,0,
112,0,95,0,52,
0,1,177,477,1,
176,516,19,517,4,
14,98,0,105,0,
110,0,111,0,112,
0,95,0,51,0,
1,176,477,1,175,
518,19,519,4,14,
98,0,105,0,110,
0,111,0,112,0,
95,0,50,0,1,
175,477,1,174,520,
19,521,4,14,98,
0,105,0,110,0,
111,0,112,0,95,
0,49,0,1,174,
477,1,173,522,19,
523,4,12,101,0,
120,0,112,0,95,
0,49,0,48,0,
1,173,524,5,23,
1,374,525,16,0,
183,1,567,526,16,
0,319,1,546,527,
16,0,183,1,977,
528,16,0,183,1,
506,529,16,0,106,
1,867,530,16,0,
217,1,735,531,16,
0,183,1,170,532,
16,0,259,1,28,
533,16,0,206,1,
450,534,16,0,132,
1,402,535,16,0,
183,1,634,536,16,
0,288,1,69,537,
16,0,317,1,209,
538,16,0,247,1,
302,539,16,0,377,
1,206,540,16,0,
250,1,62,541,16,
0,266,1,813,542,
16,0,217,1,671,
543,16,0,262,1,
478,544,16,0,122,
1,4,545,16,0,
183,1,97,546,16,
0,297,1,45,547,
16,0,377,1,172,
548,19,549,4,10,
101,0,120,0,112,
0,95,0,57,0,
1,172,524,1,171,
550,19,551,4,20,
102,0,105,0,101,
0,108,0,100,0,
115,0,101,0,112,
0,95,0,50,0,
1,171,552,5,1,
1,299,553,16,0,
218,1,170,554,19,
555,4,20,102,0,
105,0,101,0,108,
0,100,0,115,0,
101,0,112,0,95,
0,49,0,1,170,
552,1,169,556,19,
557,4,20,102,0,
117,0,110,0,99,
0,110,0,97,0,
109,0,101,0,95,
0,51,0,1,169,
558,5,2,1,437,
559,16,0,151,1,
441,560,16,0,154,
1,168,561,19,562,
4,20,102,0,117,
0,110,0,99,0,
110,0,97,0,109,
0,101,0,95,0,
50,0,1,168,558,
1,167,563,19,564,
4,14,102,0,105,
0,101,0,108,0,
100,0,95,0,49,
0,1,167,565,5,
2,1,302,566,16,
0,225,1,45,567,
16,0,225,1,166,
568,19,569,4,26,
70,0,105,0,101,
0,108,0,100,0,
65,0,115,0,115,
0,105,0,103,0,
110,0,95,0,49,
0,1,166,565,1,
165,570,19,571,4,
32,70,0,105,0,
101,0,108,0,100,
0,69,0,120,0,
112,0,65,0,115,
0,115,0,105,0,
103,0,110,0,95,
0,49,0,1,165,
565,1,164,572,19,
573,4,10,97,0,
114,0,103,0,95,
0,50,0,1,164,
433,1,163,574,19,
575,4,10,97,0,
114,0,103,0,95,
0,49,0,1,163,
433,1,162,576,19,
577,4,20,102,0,
117,0,110,0,99,
0,116,0,105,0,
111,0,110,0,95,
0,49,0,1,162,
578,5,23,1,374,
579,16,0,282,1,
567,580,16,0,282,
1,546,581,16,0,
282,1,977,582,16,
0,282,1,506,583,
16,0,282,1,867,
584,16,0,282,1,
735,585,16,0,282,
1,170,586,16,0,
282,1,28,587,16,
0,282,1,450,588,
16,0,282,1,402,
589,16,0,282,1,
634,590,16,0,282,
1,69,591,16,0,
282,1,209,592,16,
0,282,1,302,593,
16,0,282,1,206,
594,16,0,282,1,
62,595,16,0,282,
1,813,596,16,0,
282,1,671,597,16,
0,282,1,478,598,
16,0,282,1,4,
599,16,0,282,1,
97,600,16,0,282,
1,45,601,16,0,
282,1,161,602,19,
603,4,20,102,0,
117,0,110,0,99,
0,98,0,111,0,
100,0,121,0,95,
0,52,0,1,161,
604,5,3,1,434,
605,16,0,166,1,
444,606,16,0,148,
1,11,607,16,0,
412,1,160,608,19,
609,4,20,102,0,
117,0,110,0,99,
0,98,0,111,0,
100,0,121,0,95,
0,51,0,1,160,
604,1,159,610,19,
611,4,20,102,0,
117,0,110,0,99,
0,98,0,111,0,
100,0,121,0,95,
0,50,0,1,159,
604,1,158,612,19,
613,4,20,102,0,
117,0,110,0,99,
0,98,0,111,0,
100,0,121,0,95,
0,49,0,1,158,
604,1,157,614,19,
615,4,20,102,0,
117,0,110,0,99,
0,110,0,97,0,
109,0,101,0,95,
0,49,0,1,157,
558,1,156,616,19,
617,4,24,80,0,
97,0,99,0,107,
0,97,0,103,0,
101,0,82,0,101,
0,102,0,95,0,
49,0,1,156,618,
5,39,1,534,619,
16,0,405,1,209,
620,16,0,305,1,
848,621,16,0,405,
1,97,622,16,0,
305,1,735,623,16,
0,305,1,841,624,
16,0,405,1,302,
625,16,0,305,1,
621,626,16,0,405,
1,940,627,16,0,
405,1,402,628,16,
0,305,1,506,629,
16,0,305,1,716,
630,16,0,405,1,
607,631,16,0,405,
1,69,632,16,0,
305,1,1028,633,16,
0,405,1,813,634,
16,0,305,1,170,
635,16,0,305,1,
62,636,16,0,305,
1,595,637,16,0,
405,1,699,638,16,
0,405,1,374,639,
16,0,305,1,478,
640,16,0,305,1,
45,641,16,0,305,
1,998,642,16,0,
405,1,567,643,16,
0,305,1,546,644,
16,0,305,1,671,
645,16,0,305,1,
28,646,16,0,305,
1,773,647,16,0,
405,1,450,648,16,
0,305,1,21,649,
16,0,405,1,14,
650,16,0,405,1,
977,651,16,0,305,
1,634,652,16,0,
305,1,867,653,16,
0,305,1,757,654,
16,0,405,1,4,
655,16,0,305,1,
0,656,16,0,405,
1,206,657,16,0,
305,1,155,658,19,
659,4,20,84,0,
97,0,98,0,108,
0,101,0,82,0,
101,0,102,0,95,
0,49,0,1,155,
618,1,154,660,19,
661,4,10,118,0,
97,0,114,0,95,
0,49,0,1,154,
618,1,153,662,19,
663,4,18,118,0,
97,0,114,0,108,
0,105,0,115,0,
116,0,95,0,50,
0,1,153,664,5,
16,1,757,665,16,
0,244,1,848,666,
16,0,244,1,14,
667,16,0,244,1,
607,668,16,0,244,
1,1028,669,16,0,
244,1,773,670,16,
0,244,1,841,671,
16,0,244,1,595,
672,16,0,244,1,
699,673,16,0,244,
1,998,674,16,0,
244,1,940,675,16,
0,244,1,534,676,
16,0,244,1,0,
677,16,0,244,1,
716,678,16,0,244,
1,21,679,16,0,
193,1,621,680,16,
0,244,1,152,681,
19,682,4,18,118,
0,97,0,114,0,
108,0,105,0,115,
0,116,0,95,0,
49,0,1,152,664,
1,151,683,19,684,
4,20,110,0,97,
0,109,0,101,0,
108,0,105,0,115,
0,116,0,95,0,
50,0,1,151,685,
5,3,1,370,686,
16,0,186,1,447,
687,16,0,136,1,
368,688,16,0,185,
1,150,689,19,690,
4,20,110,0,97,
0,109,0,101,0,
108,0,105,0,115,
0,116,0,95,0,
49,0,1,150,685,
1,149,691,19,692,
4,22,112,0,114,
0,101,0,102,0,
105,0,120,0,101,
0,120,0,112,0,
95,0,51,0,1,
149,693,5,39,1,
534,694,16,0,427,
1,209,695,16,0,
389,1,848,696,16,
0,427,1,97,697,
16,0,389,1,735,
698,16,0,389,1,
841,699,16,0,427,
1,302,700,16,0,
389,1,621,701,16,
0,427,1,940,702,
16,0,427,1,402,
703,16,0,389,1,
506,704,16,0,389,
1,716,705,16,0,
427,1,607,706,16,
0,427,1,69,707,
16,0,389,1,1028,
708,16,0,427,1,
813,709,16,0,389,
1,170,710,16,0,
389,1,62,711,16,
0,389,1,595,712,
16,0,427,1,699,
713,16,0,427,1,
374,714,16,0,389,
1,478,715,16,0,
389,1,45,716,16,
0,389,1,998,717,
16,0,427,1,567,
718,16,0,389,1,
546,719,16,0,389,
1,671,720,16,0,
389,1,28,721,16,
0,389,1,773,722,
16,0,427,1,450,
723,16,0,389,1,
21,724,16,0,403,
1,14,725,16,0,
427,1,977,726,16,
0,389,1,634,727,
16,0,389,1,867,
728,16,0,389,1,
757,729,16,0,427,
1,4,730,16,0,
389,1,0,731,16,
0,427,1,206,732,
16,0,389,1,148,
733,19,734,4,22,
112,0,114,0,101,
0,102,0,105,0,
120,0,101,0,120,
0,112,0,95,0,
50,0,1,148,693,
1,147,735,19,736,
4,22,112,0,114,
0,101,0,102,0,
105,0,120,0,101,
0,120,0,112,0,
95,0,49,0,1,
147,693,1,146,737,
19,738,4,28,102,
0,117,0,110,0,
99,0,116,0,105,
0,111,0,110,0,
99,0,97,0,108,
0,108,0,95,0,
50,0,1,146,739,
5,39,1,534,740,
16,0,308,1,209,
741,16,0,308,1,
848,742,16,0,308,
1,97,743,16,0,
308,1,735,744,16,
0,308,1,841,745,
16,0,308,1,302,
746,16,0,308,1,
621,747,16,0,308,
1,940,748,16,0,
308,1,402,749,16,
0,308,1,506,750,
16,0,308,1,716,
751,16,0,308,1,
607,752,16,0,308,
1,69,753,16,0,
308,1,1028,754,16,
0,308,1,813,755,
16,0,308,1,170,
756,16,0,308,1,
62,757,16,0,308,
1,595,758,16,0,
308,1,699,759,16,
0,308,1,374,760,
16,0,308,1,478,
761,16,0,308,1,
45,762,16,0,308,
1,998,763,16,0,
308,1,567,764,16,
0,308,1,546,765,
16,0,308,1,671,
766,16,0,308,1,
28,767,16,0,308,
1,773,768,16,0,
308,1,450,769,16,
0,308,1,21,770,
16,0,308,1,14,
771,16,0,308,1,
977,772,16,0,308,
1,634,773,16,0,
308,1,867,774,16,
0,308,1,757,775,
16,0,308,1,4,
776,16,0,308,1,
0,777,16,0,308,
1,206,778,16,0,
308,1,145,779,19,
780,4,28,102,0,
117,0,110,0,99,
0,116,0,105,0,
111,0,110,0,99,
0,97,0,108,0,
108,0,95,0,49,
0,1,145,739,1,
144,781,19,304,1,
144,440,1,143,782,
19,331,1,143,477,
1,142,783,19,784,
4,10,101,0,120,
0,112,0,95,0,
56,0,1,142,524,
1,141,785,19,786,
4,10,101,0,120,
0,112,0,95,0,
55,0,1,141,524,
1,140,787,19,788,
4,10,101,0,120,
0,112,0,95,0,
54,0,1,140,524,
1,139,789,19,790,
4,10,101,0,120,
0,112,0,95,0,
53,0,1,139,524,
1,138,791,19,792,
4,10,101,0,120,
0,112,0,95,0,
52,0,1,138,524,
1,137,793,19,794,
4,10,101,0,120,
0,112,0,95,0,
51,0,1,137,524,
1,136,795,19,796,
4,10,101,0,120,
0,112,0,95,0,
50,0,1,136,524,
1,135,797,19,798,
4,10,101,0,120,
0,112,0,95,0,
49,0,1,135,524,
1,134,799,19,800,
4,18,101,0,120,
0,112,0,108,0,
105,0,115,0,116,
0,95,0,50,0,
1,134,801,5,6,
1,977,802,16,0,
129,1,546,803,16,
0,328,1,402,804,
16,0,175,1,4,
805,16,0,416,1,
735,806,16,0,241,
1,374,807,16,0,
172,1,133,808,19,
809,4,18,101,0,
120,0,112,0,108,
0,105,0,115,0,
116,0,95,0,49,
0,1,133,801,1,
132,810,19,811,4,
12,105,0,110,0,
105,0,116,0,95,
0,49,0,1,132,
812,5,1,1,373,
813,16,0,169,1,
131,814,19,815,4,
18,112,0,97,0,
114,0,108,0,105,
0,115,0,116,0,
95,0,50,0,1,
131,472,1,130,816,
19,817,4,18,112,
0,97,0,114,0,
108,0,105,0,115,
0,116,0,95,0,
49,0,1,130,472,
1,129,818,19,819,
4,36,116,0,97,
0,98,0,108,0,
101,0,99,0,111,
0,110,0,115,0,
116,0,114,0,117,
0,99,0,116,0,
111,0,114,0,95,
0,50,0,1,129,
820,5,27,1,374,
821,16,0,285,1,
567,822,16,0,285,
1,40,823,16,0,
425,1,546,824,16,
0,285,1,977,825,
16,0,285,1,506,
826,16,0,285,1,
33,827,16,0,425,
1,867,828,16,0,
285,1,735,829,16,
0,285,1,170,830,
16,0,285,1,28,
831,16,0,285,1,
450,832,16,0,285,
1,402,833,16,0,
285,1,22,834,16,
0,425,1,634,835,
16,0,285,1,69,
836,16,0,285,1,
209,837,16,0,285,
1,302,838,16,0,
285,1,206,839,16,
0,285,1,62,840,
16,0,285,1,813,
841,16,0,285,1,
671,842,16,0,285,
1,478,843,16,0,
285,1,1,844,16,
0,425,1,4,845,
16,0,285,1,97,
846,16,0,285,1,
45,847,16,0,285,
1,128,848,19,849,
4,36,116,0,97,
0,98,0,108,0,
101,0,99,0,111,
0,110,0,115,0,
116,0,114,0,117,
0,99,0,116,0,
111,0,114,0,95,
0,49,0,1,128,
820,1,127,850,19,
851,4,22,102,0,
105,0,101,0,108,
0,100,0,108,0,
105,0,115,0,116,
0,95,0,51,0,
1,127,852,5,2,
1,302,853,16,0,
211,1,45,854,16,
0,232,1,126,855,
19,856,4,22,102,
0,105,0,101,0,
108,0,100,0,108,
0,105,0,115,0,
116,0,95,0,50,
0,1,126,852,1,
125,857,19,220,1,
125,552,1,124,858,
19,859,4,22,102,
0,105,0,101,0,
108,0,100,0,108,
0,105,0,115,0,
116,0,95,0,49,
0,1,124,852,1,
123,860,19,861,4,
8,73,0,102,0,
95,0,49,0,1,
123,862,5,2,1,
813,863,16,0,180,
1,867,864,16,0,
192,1,122,865,19,
866,4,12,69,0,
108,0,115,0,101,
0,95,0,49,0,
1,122,862,1,121,
867,19,868,4,16,
69,0,108,0,115,
0,101,0,73,0,
102,0,95,0,49,
0,1,121,862,1,
120,869,19,870,4,
22,76,0,111,0,
99,0,97,0,108,
0,73,0,110,0,
105,0,116,0,95,
0,49,0,1,120,
871,5,15,1,757,
872,16,0,237,1,
848,873,16,0,237,
1,14,874,16,0,
237,1,607,875,16,
0,237,1,1028,876,
16,0,237,1,773,
877,16,0,237,1,
841,878,16,0,237,
1,595,879,16,0,
237,1,699,880,16,
0,237,1,998,881,
16,0,237,1,940,
882,16,0,237,1,
534,883,16,0,237,
1,716,884,16,0,
237,1,0,885,16,
0,237,1,621,886,
16,0,237,1,119,
887,19,888,4,30,
76,0,111,0,99,
0,97,0,108,0,
78,0,97,0,109,
0,101,0,108,0,
105,0,115,0,116,
0,95,0,49,0,
1,119,871,1,118,
889,19,890,4,30,
76,0,111,0,99,
0,97,0,108,0,
70,0,117,0,110,
0,99,0,68,0,
101,0,99,0,108,
0,95,0,49,0,
1,118,871,1,117,
891,19,892,4,20,
70,0,117,0,110,
0,99,0,68,0,
101,0,99,0,108,
0,95,0,49,0,
1,117,871,1,116,
893,19,894,4,10,
70,0,111,0,114,
0,95,0,51,0,
1,116,871,1,115,
895,19,896,4,10,
70,0,111,0,114,
0,95,0,50,0,
1,115,871,1,114,
897,19,898,4,10,
70,0,111,0,114,
0,95,0,49,0,
1,114,871,1,113,
899,19,900,4,12,
115,0,116,0,97,
0,116,0,95,0,
52,0,1,113,871,
1,112,901,19,902,
4,16,82,0,101,
0,116,0,118,0,
97,0,108,0,95,
0,49,0,1,112,
871,1,111,903,19,
904,4,12,115,0,
116,0,97,0,116,
0,95,0,51,0,
1,111,871,1,110,
905,19,906,4,14,
83,0,69,0,108,
0,115,0,101,0,
95,0,49,0,1,
110,871,1,109,907,
19,908,4,18,83,
0,69,0,108,0,
115,0,101,0,73,
0,102,0,95,0,
49,0,1,109,871,
1,108,909,19,910,
4,10,83,0,73,
0,102,0,95,0,
49,0,1,108,871,
1,107,911,19,912,
4,16,82,0,101,
0,112,0,101,0,
97,0,116,0,95,
0,49,0,1,107,
871,1,106,913,19,
914,4,12,115,0,
116,0,97,0,116,
0,95,0,50,0,
1,106,871,1,105,
915,19,916,4,14,
87,0,104,0,105,
0,108,0,101,0,
95,0,49,0,1,
105,871,1,104,917,
19,918,4,12,115,
0,116,0,97,0,
116,0,95,0,49,
0,1,104,871,1,
103,919,19,920,4,
8,68,0,111,0,
95,0,49,0,1,
103,871,1,102,921,
19,922,4,28,70,
0,117,0,110,0,
99,0,116,0,105,
0,111,0,110,0,
67,0,97,0,108,
0,108,0,95,0,
49,0,1,102,871,
1,101,923,19,924,
4,24,65,0,115,
0,115,0,105,0,
103,0,110,0,109,
0,101,0,110,0,
116,0,95,0,49,
0,1,101,871,1,
100,925,19,926,4,
14,98,0,108,0,
111,0,99,0,107,
0,95,0,50,0,
1,100,927,5,12,
1,848,928,16,0,
205,1,14,929,16,
0,408,1,607,930,
16,0,292,1,1028,
931,16,0,112,1,
595,932,16,0,316,
1,841,933,16,0,
207,1,699,934,16,
0,256,1,998,935,
16,0,124,1,940,
936,16,0,174,1,
534,937,16,0,320,
1,716,938,16,0,
246,1,621,939,16,
0,296,1,99,940,
19,941,4,14,98,
0,108,0,111,0,
99,0,107,0,95,
0,49,0,1,99,
927,1,98,942,19,
943,4,14,99,0,
104,0,117,0,110,
0,107,0,95,0,
52,0,1,98,944,
5,15,1,757,945,
16,0,234,1,848,
946,16,0,240,1,
14,947,16,0,240,
1,607,948,16,0,
240,1,1028,949,16,
0,240,1,773,950,
16,0,235,1,841,
951,16,0,240,1,
595,952,16,0,240,
1,699,953,16,0,
240,1,998,954,16,
0,240,1,940,955,
16,0,240,1,534,
956,16,0,240,1,
716,957,16,0,240,
1,0,958,16,0,
104,1,621,959,16,
0,240,1,97,960,
19,961,4,14,99,
0,104,0,117,0,
110,0,107,0,95,
0,51,0,1,97,
944,1,96,962,19,
963,4,14,99,0,
104,0,117,0,110,
0,107,0,95,0,
50,0,1,96,944,
1,95,964,19,965,
4,14,99,0,104,
0,117,0,110,0,
107,0,95,0,49,
0,1,95,944,1,
94,966,19,210,1,
94,433,1,93,967,
19,968,4,8,69,
0,108,0,115,0,
101,0,1,93,862,
1,92,969,19,970,
4,12,69,0,108,
0,115,0,101,0,
73,0,102,0,1,
92,862,1,91,971,
19,972,4,4,73,
0,102,0,1,91,
862,1,90,973,19,
182,1,90,862,1,
89,974,19,975,4,
10,83,0,69,0,
108,0,115,0,101,
0,1,89,871,1,
88,976,19,977,4,
14,83,0,69,0,
108,0,115,0,101,
0,73,0,102,0,
1,88,871,1,87,
978,19,979,4,6,
83,0,73,0,102,
0,1,87,871,1,
86,980,19,981,4,
6,70,0,111,0,
114,0,1,86,871,
1,85,982,19,983,
4,12,82,0,101,
0,112,0,101,0,
97,0,116,0,1,
85,871,1,84,984,
19,985,4,10,87,
0,104,0,105,0,
108,0,101,0,1,
84,871,1,83,986,
19,987,4,4,68,
0,111,0,1,83,
871,1,82,988,19,
989,4,24,70,0,
117,0,110,0,99,
0,116,0,105,0,
111,0,110,0,67,
0,97,0,108,0,
108,0,1,82,871,
1,81,990,19,991,
4,26,76,0,111,
0,99,0,97,0,
108,0,70,0,117,
0,110,0,99,0,
68,0,101,0,99,
0,108,0,1,81,
871,1,80,992,19,
993,4,16,70,0,
117,0,110,0,99,
0,68,0,101,0,
99,0,108,0,1,
80,871,1,79,994,
19,995,4,12,82,
0,101,0,116,0,
118,0,97,0,108,
0,1,79,871,1,
78,996,19,997,4,
26,76,0,111,0,
99,0,97,0,108,
0,78,0,97,0,
109,0,101,0,108,
0,105,0,115,0,
116,0,1,78,871,
1,77,998,19,999,
4,18,76,0,111,
0,99,0,97,0,
108,0,73,0,110,
0,105,0,116,0,
1,77,871,1,76,
1000,19,1001,4,20,
65,0,115,0,115,
0,105,0,103,0,
110,0,109,0,101,
0,110,0,116,0,
1,76,871,1,75,
1002,19,239,1,75,
871,1,74,1003,19,
171,1,74,812,1,
73,1004,19,195,1,
73,664,1,72,1005,
19,1006,4,16,84,
0,97,0,98,0,
108,0,101,0,82,
0,101,0,102,0,
1,72,618,1,71,
1007,19,1008,4,20,
80,0,97,0,99,
0,107,0,97,0,
103,0,101,0,82,
0,101,0,102,0,
1,71,618,1,70,
1009,19,307,1,70,
618,1,69,1010,19,
131,1,69,801,1,
68,1011,19,108,1,
68,524,1,67,1012,
19,284,1,67,578,
1,66,1013,19,391,
1,66,693,1,65,
1014,19,150,1,65,
604,1,64,1015,19,
314,1,64,472,1,
63,1016,19,153,1,
63,558,1,62,1017,
19,310,1,62,739,
1,61,1018,19,138,
1,61,685,1,60,
1019,19,287,1,60,
820,1,59,1020,19,
213,1,59,852,1,
58,1021,19,1022,4,
22,70,0,105,0,
101,0,108,0,100,
0,65,0,115,0,
115,0,105,0,103,
0,110,0,1,58,
565,1,57,1023,19,
1024,4,28,70,0,
105,0,101,0,108,
0,100,0,69,0,
120,0,112,0,65,
0,115,0,115,0,
105,0,103,0,110,
0,1,57,565,1,
56,1025,19,227,1,
56,565,1,55,1026,
19,114,1,55,927,
1,54,1027,19,103,
1,54,944,1,53,
1028,19,386,1,53,
1029,5,2,1,1051,
1030,16,0,384,1,
13,1031,16,0,384,
1,52,1032,19,352,
1,52,1033,5,45,
1,210,1034,16,0,
350,1,207,1035,16,
0,350,1,96,1036,
17,1037,15,1038,4,
20,37,0,112,0,
114,0,101,0,102,
0,105,0,120,0,
101,0,120,0,112,
0,1,-1,1,5,
1039,20,736,1,147,
1,3,1,2,1,
1,1040,22,1,53,
1,95,1041,17,1042,
15,1038,1,-1,1,
5,1043,20,734,1,
148,1,3,1,2,
1,1,1044,22,1,
54,1,92,1045,17,
1046,15,1038,1,-1,
1,5,1047,20,692,
1,149,1,3,1,
4,1,3,1048,22,
1,55,1,517,1049,
16,0,350,1,298,
1050,17,1051,15,1052,
4,34,37,0,116,
0,97,0,98,0,
108,0,101,0,99,
0,111,0,110,0,
115,0,116,0,114,
0,117,0,99,0,
116,0,111,0,114,
0,1,-1,1,5,
1053,20,849,1,128,
1,3,1,3,1,
2,1054,22,1,33,
1,296,1055,17,1056,
15,1052,1,-1,1,
5,1057,20,819,1,
129,1,3,1,4,
1,3,1058,22,1,
34,1,76,1059,16,
0,350,1,824,1060,
16,0,350,1,171,
1061,16,0,350,1,
1031,1062,17,1063,15,
1064,4,18,37,0,
102,0,117,0,110,
0,99,0,98,0,
111,0,100,0,121,
0,1,-1,1,5,
1065,20,611,1,159,
1,3,1,6,1,
5,1066,22,1,67,
1,1029,1067,17,1068,
15,1064,1,-1,1,
5,1069,20,609,1,
160,1,3,1,5,
1,4,1070,22,1,
68,1,385,1071,16,
0,350,1,169,1072,
17,1073,15,1074,4,
8,37,0,118,0,
97,0,114,0,1,
-1,1,5,1075,20,
661,1,154,1,3,
1,2,1,1,1076,
22,1,60,1,489,
1077,16,0,350,1,
46,1078,16,0,350,
1,44,1079,17,1080,
15,1081,4,26,37,
0,102,0,117,0,
110,0,99,0,116,
0,105,0,111,0,
110,0,99,0,97,
0,108,0,108,0,
1,-1,1,5,1082,
20,738,1,146,1,
3,1,5,1,4,
1083,22,1,52,1,
578,1084,16,0,350,
1,682,1085,16,0,
350,1,144,1086,16,
0,350,1,33,1087,
17,1088,15,1089,4,
8,37,0,101,0,
120,0,112,0,1,
-1,1,5,1090,20,
786,1,141,1,3,
1,2,1,1,1091,
22,1,47,1,461,
1092,16,0,350,1,
129,1093,17,1094,15,
1089,1,-1,1,5,
1095,20,794,1,137,
1,3,1,2,1,
1,1096,22,1,43,
1,27,1097,17,1098,
15,1099,4,22,37,
0,80,0,97,0,
99,0,107,0,97,
0,103,0,101,0,
82,0,101,0,102,
0,1,-1,1,5,
1100,20,617,1,156,
1,3,1,4,1,
3,1101,22,1,62,
1,20,1102,17,1037,
1,1,1040,1,19,
1103,17,1073,1,1,
1076,1,131,1104,17,
1105,15,1089,1,-1,
1,5,1106,20,798,
1,135,1,3,1,
2,1,1,1107,22,
1,41,1,130,1108,
17,1109,15,1089,1,
-1,1,5,1110,20,
796,1,136,1,3,
1,2,1,1,1111,
22,1,42,1,343,
1112,17,1113,15,1114,
4,18,37,0,84,
0,97,0,98,0,
108,0,101,0,82,
0,101,0,102,0,
1,-1,1,5,1115,
20,659,1,155,1,
3,1,5,1,4,
1116,22,1,61,1,
128,1117,17,1118,15,
1089,1,-1,1,5,
1119,20,792,1,138,
1,3,1,2,1,
1,1120,22,1,44,
1,127,1121,17,1122,
15,1089,1,-1,1,
5,1123,20,790,1,
139,1,3,1,2,
1,1,1124,22,1,
45,1,126,1125,17,
1126,15,1089,1,-1,
1,5,1127,20,788,
1,140,1,3,1,
2,1,1,1128,22,
1,46,1,125,1129,
17,1130,15,1089,1,
-1,1,5,1131,20,
784,1,142,1,3,
1,2,1,1,1132,
22,1,48,1,17,
1133,17,1134,15,1064,
1,-1,1,5,1135,
20,613,1,158,1,
3,1,5,1,4,
1136,22,1,66,1,
15,1137,17,1138,15,
1064,1,-1,1,5,
1139,20,603,1,161,
1,3,1,4,1,
3,1140,22,1,69,
1,12,1141,17,1142,
15,1143,4,18,37,
0,102,0,117,0,
110,0,99,0,116,
0,105,0,111,0,
110,0,1,-1,1,
5,1144,20,577,1,
162,1,3,1,3,
1,2,1145,22,1,
70,1,10,1146,17,
1147,15,1148,4,8,
37,0,97,0,114,
0,103,0,1,-1,
1,5,209,1,2,
1,2,1149,22,1,
71,1,9,1150,17,
1151,15,1148,1,-1,
1,5,1152,20,575,
1,163,1,3,1,
4,1,3,1153,22,
1,72,1,327,1154,
16,0,350,1,3,
1155,17,1156,15,1148,
1,-1,1,5,1157,
20,573,1,164,1,
3,1,2,1,1,
1158,22,1,73,1,
325,1159,17,1160,15,
1081,1,-1,1,5,
1161,20,780,1,145,
1,3,1,3,1,
2,1162,22,1,51,
1,645,1163,16,0,
350,1,2,1164,17,
1165,15,1148,1,-1,
1,5,209,1,1,
1,1,1166,22,1,
74,1,107,1167,16,
0,350,1,51,1168,
19,272,1,51,1169,
5,43,1,209,1170,
16,0,270,1,634,
1171,16,0,270,1,
97,1172,16,0,270,
1,735,1173,16,0,
270,1,302,1174,16,
0,270,1,301,1175,
17,1176,15,1177,4,
18,37,0,102,0,
105,0,101,0,108,
0,100,0,115,0,
101,0,112,0,1,
-1,1,5,219,1,
1,1,1,1178,22,
1,93,1,300,1179,
17,1180,15,1177,1,
-1,1,5,219,1,
1,1,1,1181,22,
1,94,1,402,1182,
16,0,270,1,506,
1183,16,0,270,1,
69,1184,16,0,270,
1,374,1185,16,0,
270,1,50,1186,17,
1187,15,1188,4,12,
37,0,98,0,105,
0,110,0,111,0,
112,0,1,-1,1,
5,330,1,1,1,
1,1189,22,1,89,
1,813,1190,16,0,
270,1,170,1191,16,
0,270,1,62,1192,
16,0,270,1,61,
1193,17,1194,15,1188,
1,-1,1,5,330,
1,1,1,1,1195,
22,1,78,1,60,
1196,17,1197,15,1188,
1,-1,1,5,330,
1,1,1,1,1198,
22,1,79,1,59,
1199,17,1200,15,1188,
1,-1,1,5,330,
1,1,1,1,1201,
22,1,80,1,58,
1202,17,1203,15,1188,
1,-1,1,5,330,
1,1,1,1,1204,
22,1,81,1,57,
1205,17,1206,15,1188,
1,-1,1,5,330,
1,1,1,1,1207,
22,1,82,1,56,
1208,17,1209,15,1188,
1,-1,1,5,330,
1,1,1,1,1210,
22,1,83,1,55,
1211,17,1212,15,1188,
1,-1,1,5,330,
1,1,1,1,1213,
22,1,84,1,54,
1214,17,1215,15,1188,
1,-1,1,5,330,
1,1,1,1,1216,
22,1,85,1,53,
1217,17,1218,15,1188,
1,-1,1,5,330,
1,1,1,1,1219,
22,1,86,1,52,
1220,17,1221,15,1188,
1,-1,1,5,330,
1,1,1,1,1222,
22,1,87,1,51,
1223,17,1224,15,1188,
1,-1,1,5,330,
1,1,1,1,1225,
22,1,88,1,478,
1226,16,0,270,1,
49,1227,17,1228,15,
1188,1,-1,1,5,
330,1,1,1,1,
1229,22,1,90,1,
48,1230,17,1231,15,
1188,1,-1,1,5,
330,1,1,1,1,
1232,22,1,91,1,
47,1233,17,1234,15,
1188,1,-1,1,5,
330,1,1,1,1,
1235,22,1,92,1,
45,1236,16,0,270,
1,567,1237,16,0,
270,1,546,1238,16,
0,270,1,671,1239,
16,0,270,1,28,
1240,16,0,270,1,
450,1241,16,0,270,
1,977,1242,16,0,
270,1,867,1243,16,
0,270,1,7,1244,
17,1245,15,1246,4,
10,37,0,117,0,
110,0,111,0,112,
0,1,-1,1,5,
303,1,1,1,1,
1247,22,1,75,1,
6,1248,17,1249,15,
1246,1,-1,1,5,
303,1,1,1,1,
1250,22,1,76,1,
5,1251,17,1252,15,
1246,1,-1,1,5,
303,1,1,1,1,
1253,22,1,77,1,
4,1254,16,0,270,
1,206,1255,16,0,
270,1,50,1256,19,
275,1,50,1257,5,
43,1,209,1258,16,
0,273,1,634,1259,
16,0,273,1,97,
1260,16,0,273,1,
735,1261,16,0,273,
1,302,1262,16,0,
273,1,301,1175,1,
300,1179,1,402,1263,
16,0,273,1,506,
1264,16,0,273,1,
69,1265,16,0,273,
1,374,1266,16,0,
273,1,50,1186,1,
813,1267,16,0,273,
1,170,1268,16,0,
273,1,62,1269,16,
0,273,1,61,1193,
1,60,1196,1,59,
1199,1,58,1202,1,
57,1205,1,56,1208,
1,55,1211,1,54,
1214,1,53,1217,1,
52,1220,1,51,1223,
1,478,1270,16,0,
273,1,49,1227,1,
48,1230,1,47,1233,
1,45,1271,16,0,
273,1,567,1272,16,
0,273,1,546,1273,
16,0,273,1,671,
1274,16,0,273,1,
28,1275,16,0,273,
1,450,1276,16,0,
273,1,977,1277,16,
0,273,1,867,1278,
16,0,273,1,7,
1244,1,6,1248,1,
5,1251,1,4,1279,
16,0,273,1,206,
1280,16,0,273,1,
49,1281,19,191,1,
49,1282,5,72,1,
534,1283,16,0,189,
1,619,1284,17,1285,
15,1286,4,12,37,
0,83,0,69,0,
108,0,115,0,101,
0,1,-1,1,5,
1287,20,906,1,110,
1,3,1,8,1,
7,1288,22,1,16,
1,92,1045,1,95,
1041,1,421,1289,17,
1290,15,1291,4,16,
37,0,101,0,120,
0,112,0,108,0,
105,0,115,0,116,
0,1,-1,1,5,
1292,20,809,1,133,
1,3,1,4,1,
3,1293,22,1,39,
1,848,1294,16,0,
189,1,96,1036,1,
298,1050,1,950,1295,
17,1296,15,1297,4,
8,37,0,70,0,
111,0,114,0,1,
-1,1,5,1298,20,
898,1,114,1,3,
1,10,1,9,1299,
22,1,20,1,841,
1300,16,0,189,1,
733,1301,17,1302,15,
1303,4,6,37,0,
68,0,111,0,1,
-1,1,5,1304,20,
920,1,103,1,3,
1,4,1,3,1305,
22,1,9,1,730,
1306,17,1307,15,1308,
4,10,37,0,115,
0,116,0,97,0,
116,0,1,-1,1,
5,1309,20,918,1,
104,1,3,1,3,
1,2,1310,22,1,
10,1,621,1311,16,
0,189,1,940,1312,
16,0,189,1,296,
1055,1,716,1313,16,
0,189,1,715,1314,
17,1315,15,1316,4,
12,37,0,87,0,
104,0,105,0,108,
0,101,0,1,-1,
1,5,1317,20,916,
1,105,1,3,1,
6,1,5,1318,22,
1,11,1,607,1319,
16,0,189,1,712,
1320,17,1321,15,1308,
1,-1,1,5,1322,
20,914,1,106,1,
3,1,5,1,4,
1323,22,1,12,1,
1031,1062,1,1029,1067,
1,1028,1324,16,0,
189,1,385,1325,17,
1326,15,1291,1,-1,
1,5,1327,20,800,
1,134,1,3,1,
2,1,1,1328,22,
1,40,1,169,1072,
1,595,1329,16,0,
189,1,699,1330,16,
0,189,1,910,1331,
17,1332,15,1333,4,
8,37,0,83,0,
73,0,102,0,1,
-1,1,5,1334,20,
910,1,108,1,3,
1,6,1,5,1335,
22,1,14,1,909,
1336,17,1337,15,1338,
4,16,37,0,83,
0,69,0,108,0,
115,0,101,0,73,
0,102,0,1,-1,
1,5,1339,20,908,
1,109,1,3,1,
8,1,7,1340,22,
1,15,1,373,1341,
17,1342,15,1343,4,
28,37,0,76,0,
111,0,99,0,97,
0,108,0,78,0,
97,0,109,0,101,
0,108,0,105,0,
115,0,116,0,1,
-1,1,5,1344,20,
888,1,119,1,3,
1,3,1,2,1345,
22,1,25,1,371,
1346,17,1347,15,1348,
4,18,37,0,110,
0,97,0,109,0,
101,0,108,0,105,
0,115,0,116,0,
1,-1,1,5,1349,
20,684,1,151,1,
3,1,4,1,3,
1350,22,1,57,1,
369,1351,17,1352,15,
1348,1,-1,1,5,
1353,20,690,1,150,
1,3,1,2,1,
1,1354,22,1,56,
1,44,1079,1,1007,
1355,17,1356,15,1297,
1,-1,1,5,1357,
20,894,1,116,1,
3,1,8,1,7,
1358,22,1,22,1,
19,1103,1,0,1359,
16,0,189,1,144,
1360,17,1361,15,1089,
1,-1,1,5,107,
1,3,1,3,1362,
22,1,49,1,33,
1087,1,998,1363,16,
0,189,1,2,1164,
1,125,1129,1,10,
1146,1,566,1364,17,
1365,15,1366,4,14,
37,0,82,0,101,
0,116,0,118,0,
97,0,108,0,1,
-1,1,5,1367,20,
902,1,112,1,3,
1,3,1,2,1368,
22,1,18,1,131,
1104,1,130,1108,1,
129,1093,1,27,1097,
1,20,1102,1,127,
1121,1,773,1369,16,
0,189,1,436,1370,
17,1371,15,1372,4,
28,37,0,76,0,
111,0,99,0,97,
0,108,0,70,0,
117,0,110,0,99,
0,68,0,101,0,
99,0,108,0,1,
-1,1,5,1373,20,
890,1,118,1,3,
1,5,1,4,1374,
22,1,24,1,343,
1112,1,128,1117,1,
448,1375,17,1352,1,
1,1354,1,126,1125,
1,446,1376,17,1377,
15,1378,4,18,37,
0,70,0,117,0,
110,0,99,0,68,
0,101,0,99,0,
108,0,1,-1,1,
5,1379,20,892,1,
117,1,3,1,4,
1,3,1380,22,1,
23,1,17,1133,1,
325,1159,1,15,1137,
1,14,1381,16,0,
189,1,9,1150,1,
12,1141,1,546,1382,
17,1383,15,1308,1,
-1,1,5,1384,20,
904,1,111,1,3,
1,2,1,1,1385,
22,1,17,1,545,
1386,17,1387,15,1308,
1,-1,1,5,1388,
20,900,1,113,1,
3,1,2,1,1,
1389,22,1,19,1,
544,1390,17,1391,15,
1297,1,-1,1,5,
1392,20,896,1,115,
1,3,1,12,1,
11,1393,22,1,21,
1,757,1394,16,0,
189,1,755,1395,17,
1396,15,1397,4,22,
37,0,65,0,115,
0,115,0,105,0,
103,0,110,0,109,
0,101,0,110,0,
116,0,1,-1,1,
5,1398,20,924,1,
101,1,3,1,4,
1,3,1399,22,1,
7,1,3,1155,1,
432,1400,17,1401,15,
1402,4,20,37,0,
76,0,111,0,99,
0,97,0,108,0,
73,0,110,0,105,
0,116,0,1,-1,
1,5,1403,20,870,
1,120,1,3,1,
4,1,3,1404,22,
1,26,1,645,1405,
17,1406,15,1407,4,
14,37,0,82,0,
101,0,112,0,101,
0,97,0,116,0,
1,-1,1,5,1408,
20,912,1,107,1,
3,1,5,1,4,
1409,22,1,13,1,
430,1410,17,1411,15,
1412,4,10,37,0,
105,0,110,0,105,
0,116,0,1,-1,
1,5,1413,20,811,
1,132,1,3,1,
3,1,2,1414,22,
1,38,1,1,1415,
17,1416,15,1417,4,
26,37,0,70,0,
117,0,110,0,99,
0,116,0,105,0,
111,0,110,0,67,
0,97,0,108,0,
108,0,1,-1,1,
5,1418,20,922,1,
102,1,3,1,2,
1,1,1419,22,1,
8,1,107,1420,17,
1421,15,1089,1,-1,
1,5,107,1,2,
1,2,1422,22,1,
50,1,48,1423,19,
398,1,48,1424,5,
72,1,534,1425,16,
0,396,1,619,1284,
1,92,1045,1,95,
1041,1,421,1289,1,
848,1426,16,0,396,
1,96,1036,1,298,
1050,1,950,1295,1,
841,1427,16,0,396,
1,733,1301,1,730,
1306,1,621,1428,16,
0,396,1,940,1429,
16,0,396,1,296,
1055,1,716,1430,16,
0,396,1,715,1314,
1,607,1431,16,0,
396,1,712,1320,1,
1031,1062,1,1029,1067,
1,1028,1432,16,0,
396,1,385,1325,1,
169,1072,1,595,1433,
16,0,396,1,699,
1434,16,0,396,1,
910,1331,1,909,1336,
1,373,1341,1,371,
1346,1,369,1351,1,
44,1079,1,1007,1355,
1,19,1103,1,0,
1435,16,0,396,1,
144,1360,1,33,1087,
1,998,1436,16,0,
396,1,2,1164,1,
125,1129,1,10,1146,
1,566,1364,1,131,
1104,1,130,1108,1,
129,1093,1,27,1097,
1,20,1102,1,127,
1121,1,773,1437,16,
0,396,1,436,1370,
1,343,1112,1,128,
1117,1,448,1375,1,
126,1125,1,446,1376,
1,17,1133,1,325,
1159,1,15,1137,1,
14,1438,16,0,396,
1,9,1150,1,12,
1141,1,546,1382,1,
545,1386,1,544,1390,
1,757,1439,16,0,
396,1,755,1395,1,
3,1155,1,432,1400,
1,645,1405,1,430,
1410,1,1,1415,1,
107,1420,1,47,1440,
19,295,1,47,1441,
5,75,1,534,1442,
17,1443,15,1444,4,
12,37,0,98,0,
108,0,111,0,99,
0,107,0,1,-1,
1,5,1445,20,926,
1,100,1,3,1,
1,1,0,1446,22,
1,6,1,619,1284,
1,92,1045,1,95,
1041,1,421,1289,1,
848,1447,17,1443,1,
0,1446,1,633,1448,
16,0,293,1,96,
1036,1,298,1050,1,
950,1295,1,841,1449,
17,1443,1,0,1446,
1,733,1301,1,730,
1306,1,621,1450,17,
1443,1,0,1446,1,
940,1451,17,1443,1,
0,1446,1,296,1055,
1,716,1452,17,1443,
1,0,1446,1,715,
1314,1,607,1453,17,
1443,1,0,1446,1,
712,1320,1,1031,1062,
1,1029,1067,1,1028,
1454,17,1443,1,0,
1446,1,385,1325,1,
169,1072,1,595,1455,
17,1443,1,0,1446,
1,699,1456,17,1443,
1,0,1446,1,910,
1331,1,909,1336,1,
373,1341,1,371,1346,
1,369,1351,1,44,
1079,1,1007,1355,1,
792,1457,17,1458,15,
1459,4,12,37,0,
99,0,104,0,117,
0,110,0,107,0,
1,-1,1,5,1460,
20,961,1,97,1,
3,1,3,1,2,
1461,22,1,3,1,
789,1462,17,1463,15,
1459,1,-1,1,5,
1464,20,943,1,98,
1,3,1,4,1,
3,1465,22,1,4,
1,19,1103,1,144,
1360,1,33,1087,1,
998,1466,17,1443,1,
0,1446,1,2,1164,
1,125,1129,1,10,
1146,1,566,1364,1,
131,1104,1,130,1108,
1,129,1093,1,27,
1097,1,20,1102,1,
127,1121,1,773,1467,
17,1468,15,1459,1,
-1,1,5,1469,20,
963,1,96,1,3,
1,3,1,2,1470,
22,1,2,1,436,
1370,1,343,1112,1,
128,1117,1,448,1375,
1,126,1125,1,446,
1376,1,17,1133,1,
325,1159,1,15,1137,
1,14,1471,17,1443,
1,0,1446,1,9,
1150,1,12,1141,1,
546,1382,1,545,1386,
1,544,1390,1,757,
1472,17,1473,15,1459,
1,-1,1,5,1474,
20,965,1,95,1,
3,1,2,1,1,
1475,22,1,1,1,
756,1476,17,1477,15,
1444,1,-1,1,5,
1478,20,941,1,99,
1,3,1,2,1,
1,1479,22,1,5,
1,755,1395,1,3,
1155,1,432,1400,1,
645,1405,1,430,1410,
1,1,1415,1,107,
1420,1,46,1480,19,
300,1,46,1481,5,
72,1,534,1482,16,
0,298,1,619,1284,
1,92,1045,1,95,
1041,1,421,1289,1,
848,1483,16,0,298,
1,96,1036,1,298,
1050,1,950,1295,1,
841,1484,16,0,298,
1,733,1301,1,730,
1306,1,621,1485,16,
0,298,1,940,1486,
16,0,298,1,296,
1055,1,716,1487,16,
0,298,1,715,1314,
1,607,1488,16,0,
298,1,712,1320,1,
1031,1062,1,1029,1067,
1,1028,1489,16,0,
298,1,385,1325,1,
169,1072,1,595,1490,
16,0,298,1,699,
1491,16,0,298,1,
910,1331,1,909,1336,
1,373,1341,1,371,
1346,1,369,1351,1,
44,1079,1,1007,1355,
1,19,1103,1,0,
1492,16,0,298,1,
144,1360,1,33,1087,
1,998,1493,16,0,
298,1,2,1164,1,
125,1129,1,10,1146,
1,566,1364,1,131,
1104,1,130,1108,1,
129,1093,1,27,1097,
1,20,1102,1,127,
1121,1,773,1494,16,
0,298,1,436,1370,
1,343,1112,1,128,
1117,1,448,1375,1,
126,1125,1,446,1376,
1,17,1133,1,325,
1159,1,15,1137,1,
14,1495,16,0,298,
1,9,1150,1,12,
1141,1,546,1382,1,
545,1386,1,544,1390,
1,757,1496,16,0,
298,1,755,1395,1,
3,1155,1,432,1400,
1,645,1405,1,430,
1410,1,1,1415,1,
107,1420,1,45,1497,
19,165,1,45,1498,
5,115,1,716,1499,
16,0,163,1,715,
1314,1,712,1320,1,
950,1295,1,910,1331,
1,699,1500,16,0,
163,1,209,1501,16,
0,413,1,671,1502,
16,0,413,1,450,
1503,16,0,413,1,
448,1375,1,446,1376,
1,206,1504,16,0,
413,1,436,1370,1,
432,1400,1,909,1336,
1,430,1410,1,421,
1289,1,169,1072,1,
170,1505,16,0,413,
1,369,1351,1,645,
1405,1,402,1506,16,
0,413,1,619,1284,
1,848,1507,16,0,
163,1,634,1508,16,
0,413,1,867,1509,
16,0,413,1,607,
1510,16,0,163,1,
385,1325,1,144,1360,
1,621,1511,16,0,
163,1,368,1512,16,
0,168,1,1029,1067,
1,374,1513,16,0,
413,1,373,1341,1,
371,1346,1,131,1104,
1,130,1108,1,129,
1093,1,128,1117,1,
127,1121,1,126,1125,
1,125,1129,1,841,
1514,16,0,163,1,
595,1515,16,0,163,
1,107,1420,1,343,
1112,1,96,1036,1,
97,1516,16,0,413,
1,813,1517,16,0,
413,1,95,1041,1,
92,1045,1,567,1518,
16,0,413,1,566,
1364,1,546,1519,16,
0,413,1,325,1159,
1,544,1390,1,300,
1179,1,51,1223,1,
1031,1062,1,534,1520,
16,0,163,1,301,
1175,1,1028,1521,16,
0,163,1,69,1522,
16,0,413,1,56,
1208,1,545,1386,1,
57,1205,1,59,1199,
1,302,1523,16,0,
413,1,62,1524,16,
0,413,1,61,1193,
1,60,1196,1,298,
1050,1,58,1202,1,
296,1055,1,773,1525,
16,0,163,1,55,
1211,1,54,1214,1,
53,1217,1,52,1220,
1,1007,1355,1,50,
1186,1,49,1227,1,
48,1230,1,47,1233,
1,45,1526,16,0,
413,1,44,1079,1,
998,1527,16,0,163,
1,757,1528,16,0,
163,1,755,1395,1,
33,1087,1,28,1529,
16,0,413,1,506,
1530,16,0,413,1,
27,1097,1,15,1137,
1,977,1531,16,0,
413,1,20,1102,1,
19,1103,1,735,1532,
16,0,413,1,17,
1133,1,733,1301,1,
940,1533,16,0,163,
1,14,1534,16,0,
163,1,730,1306,1,
12,1141,1,10,1146,
1,9,1150,1,0,
1535,16,0,163,1,
7,1244,1,6,1248,
1,5,1251,1,4,
1536,16,0,413,1,
3,1155,1,2,1164,
1,1,1415,1,478,
1537,16,0,413,1,
44,1538,19,269,1,
44,1539,5,43,1,
209,1540,16,0,267,
1,634,1541,16,0,
267,1,97,1542,16,
0,267,1,735,1543,
16,0,267,1,302,
1544,16,0,267,1,
301,1175,1,300,1179,
1,402,1545,16,0,
267,1,506,1546,16,
0,267,1,69,1547,
16,0,267,1,374,
1548,16,0,267,1,
50,1186,1,813,1549,
16,0,267,1,170,
1550,16,0,267,1,
62,1551,16,0,267,
1,61,1193,1,60,
1196,1,59,1199,1,
58,1202,1,57,1205,
1,56,1208,1,55,
1211,1,54,1214,1,
53,1217,1,52,1220,
1,51,1223,1,478,
1552,16,0,267,1,
49,1227,1,48,1230,
1,47,1233,1,45,
1553,16,0,267,1,
567,1554,16,0,267,
1,546,1555,16,0,
267,1,671,1556,16,
0,267,1,28,1557,
16,0,267,1,450,
1558,16,0,267,1,
977,1559,16,0,267,
1,867,1560,16,0,
267,1,7,1244,1,
6,1248,1,5,1251,
1,4,1561,16,0,
267,1,206,1562,16,
0,267,1,43,1563,
19,401,1,43,1564,
5,72,1,534,1565,
16,0,399,1,619,
1284,1,92,1045,1,
95,1041,1,421,1289,
1,848,1566,16,0,
399,1,96,1036,1,
298,1050,1,950,1295,
1,841,1567,16,0,
399,1,733,1301,1,
730,1306,1,621,1568,
16,0,399,1,940,
1569,16,0,399,1,
296,1055,1,716,1570,
16,0,399,1,715,
1314,1,607,1571,16,
0,399,1,712,1320,
1,1031,1062,1,1029,
1067,1,1028,1572,16,
0,399,1,385,1325,
1,169,1072,1,595,
1573,16,0,399,1,
699,1574,16,0,399,
1,910,1331,1,909,
1336,1,373,1341,1,
371,1346,1,369,1351,
1,44,1079,1,1007,
1355,1,19,1103,1,
0,1575,16,0,399,
1,144,1360,1,33,
1087,1,998,1576,16,
0,399,1,2,1164,
1,125,1129,1,10,
1146,1,566,1364,1,
131,1104,1,130,1108,
1,129,1093,1,27,
1097,1,20,1102,1,
127,1121,1,773,1577,
16,0,399,1,436,
1370,1,343,1112,1,
128,1117,1,448,1375,
1,126,1125,1,446,
1376,1,17,1133,1,
325,1159,1,15,1137,
1,14,1578,16,0,
399,1,9,1150,1,
12,1141,1,546,1382,
1,545,1386,1,544,
1390,1,757,1579,16,
0,399,1,755,1395,
1,3,1155,1,432,
1400,1,645,1405,1,
430,1410,1,1,1415,
1,107,1420,1,42,
1580,19,135,1,42,
1581,5,4,1,976,
1582,16,0,133,1,
369,1351,1,448,1375,
1,371,1346,1,41,
1583,19,128,1,41,
1584,5,76,1,534,
1585,16,0,254,1,
619,1284,1,92,1045,
1,421,1289,1,848,
1586,16,0,254,1,
96,1036,1,95,1041,
1,950,1295,1,841,
1587,16,0,254,1,
733,1301,1,517,1588,
16,0,392,1,730,
1306,1,621,1589,16,
0,254,1,298,1050,
1,296,1055,1,716,
1590,16,0,254,1,
715,1314,1,607,1591,
16,0,254,1,712,
1320,1,1031,1062,1,
1029,1067,1,1028,1592,
16,0,254,1,385,
1325,1,169,1072,1,
489,1593,16,0,176,
1,595,1594,16,0,
254,1,699,1595,16,
0,254,1,910,1331,
1,909,1336,1,373,
1341,1,371,1346,1,
369,1351,1,44,1079,
1,1007,1355,1,0,
1596,16,0,254,1,
19,1103,1,682,1597,
16,0,258,1,940,
1598,16,0,254,1,
2,1164,1,144,1360,
1,33,1087,1,998,
1599,16,0,254,1,
997,1600,16,0,126,
1,125,1129,1,10,
1146,1,566,1364,1,
131,1104,1,130,1108,
1,129,1093,1,27,
1097,1,20,1102,1,
127,1121,1,773,1601,
16,0,254,1,436,
1370,1,343,1112,1,
128,1117,1,448,1375,
1,126,1125,1,446,
1376,1,17,1133,1,
325,1159,1,15,1137,
1,14,1602,16,0,
254,1,9,1150,1,
12,1141,1,546,1382,
1,545,1386,1,544,
1390,1,757,1603,16,
0,254,1,755,1395,
1,3,1155,1,432,
1400,1,645,1405,1,
430,1410,1,1,1415,
1,107,1420,1,40,
1604,19,147,1,40,
1605,5,72,1,534,
1606,16,0,145,1,
619,1284,1,92,1045,
1,95,1041,1,421,
1289,1,848,1607,16,
0,145,1,96,1036,
1,298,1050,1,950,
1295,1,841,1608,16,
0,145,1,733,1301,
1,730,1306,1,621,
1609,16,0,145,1,
940,1610,16,0,145,
1,296,1055,1,716,
1611,16,0,145,1,
715,1314,1,607,1612,
16,0,145,1,712,
1320,1,1031,1062,1,
1029,1067,1,1028,1613,
16,0,145,1,385,
1325,1,169,1072,1,
595,1614,16,0,145,
1,699,1615,16,0,
145,1,910,1331,1,
909,1336,1,373,1341,
1,371,1346,1,369,
1351,1,44,1079,1,
1007,1355,1,19,1103,
1,0,1616,16,0,
145,1,144,1360,1,
33,1087,1,998,1617,
16,0,145,1,2,
1164,1,125,1129,1,
10,1146,1,566,1364,
1,131,1104,1,130,
1108,1,129,1093,1,
27,1097,1,20,1102,
1,127,1121,1,773,
1618,16,0,145,1,
436,1370,1,343,1112,
1,128,1117,1,448,
1375,1,126,1125,1,
446,1376,1,17,1133,
1,325,1159,1,15,
1137,1,14,1619,16,
0,145,1,9,1150,
1,12,1141,1,546,
1382,1,545,1386,1,
544,1390,1,757,1620,
16,0,145,1,755,
1395,1,3,1155,1,
432,1400,1,645,1405,
1,430,1410,1,1,
1415,1,107,1420,1,
39,1621,19,265,1,
39,1622,5,72,1,
534,1623,16,0,263,
1,619,1284,1,92,
1045,1,95,1041,1,
421,1289,1,848,1624,
16,0,263,1,96,
1036,1,298,1050,1,
950,1295,1,841,1625,
16,0,263,1,733,
1301,1,730,1306,1,
621,1626,16,0,263,
1,940,1627,16,0,
263,1,296,1055,1,
716,1628,16,0,263,
1,715,1314,1,607,
1629,16,0,263,1,
712,1320,1,1031,1062,
1,1029,1067,1,1028,
1630,16,0,263,1,
385,1325,1,169,1072,
1,595,1631,16,0,
263,1,699,1632,16,
0,263,1,910,1331,
1,909,1336,1,373,
1341,1,371,1346,1,
369,1351,1,44,1079,
1,1007,1355,1,19,
1103,1,0,1633,16,
0,263,1,144,1360,
1,33,1087,1,998,
1634,16,0,263,1,
2,1164,1,125,1129,
1,10,1146,1,566,
1364,1,131,1104,1,
130,1108,1,129,1093,
1,27,1097,1,20,
1102,1,127,1121,1,
773,1635,16,0,263,
1,436,1370,1,343,
1112,1,128,1117,1,
448,1375,1,126,1125,
1,446,1376,1,17,
1133,1,325,1159,1,
15,1137,1,14,1636,
16,0,263,1,9,
1150,1,12,1141,1,
546,1382,1,545,1386,
1,544,1390,1,757,
1637,16,0,263,1,
755,1395,1,3,1155,
1,432,1400,1,645,
1405,1,430,1410,1,
1,1415,1,107,1420,
1,38,1638,19,111,
1,38,1639,5,87,
1,716,1640,16,0,
248,1,715,1314,1,
714,1641,16,0,255,
1,712,1320,1,950,
1295,1,949,1642,16,
0,173,1,910,1331,
1,908,1643,16,0,
179,1,448,1375,1,
446,1376,1,436,1370,
1,432,1400,1,909,
1336,1,430,1410,1,
421,1289,1,887,1644,
17,1645,15,1646,4,
14,37,0,69,0,
108,0,115,0,101,
0,73,0,102,0,
1,-1,1,5,1647,
20,868,1,121,1,
3,1,6,1,5,
1648,22,1,27,1,
169,1072,1,645,1405,
1,621,1450,1,848,
1447,1,847,1649,17,
1650,15,1651,4,6,
37,0,73,0,102,
0,1,-1,1,5,
1652,20,861,1,123,
1,3,1,4,1,
3,1653,22,1,29,
1,606,1654,16,0,
178,1,385,1325,1,
144,1360,1,369,1351,
1,619,1284,1,618,
1655,16,0,301,1,
854,1656,17,1657,15,
1658,4,10,37,0,
69,0,108,0,115,
0,101,0,1,-1,
1,5,1659,20,866,
1,122,1,3,1,
6,1,5,1660,22,
1,28,1,129,1093,
1,373,1341,1,371,
1346,1,131,1104,1,
130,1108,1,607,1453,
1,128,1117,1,127,
1121,1,126,1125,1,
125,1129,1,841,1449,
1,595,1455,1,534,
1442,1,107,1420,1,
343,1112,1,792,1457,
1,789,1462,1,96,
1036,1,95,1041,1,
92,1045,1,566,1364,
1,325,1159,1,1031,
1062,1,1030,1661,16,
0,109,1,1029,1067,
1,1028,1662,16,0,
115,1,546,1382,1,
545,1386,1,544,1390,
1,543,1663,16,0,
402,1,732,1664,16,
0,245,1,298,1050,
1,296,1055,1,773,
1467,1,1007,1355,1,
1006,1665,16,0,123,
1,44,1079,1,998,
1466,1,757,1472,1,
756,1476,1,755,1395,
1,33,1087,1,27,
1097,1,15,1137,1,
20,1102,1,19,1103,
1,16,1666,16,0,
407,1,17,1133,1,
733,1301,1,940,1451,
1,14,1667,16,0,
409,1,730,1306,1,
12,1141,1,10,1146,
1,9,1150,1,699,
1668,16,0,257,1,
3,1155,1,2,1164,
1,1,1415,1,37,
1669,19,201,1,37,
1670,5,76,1,534,
1442,1,619,1284,1,
92,1045,1,95,1041,
1,421,1289,1,848,
1447,1,847,1671,16,
0,199,1,96,1036,
1,298,1050,1,950,
1295,1,841,1449,1,
733,1301,1,730,1306,
1,621,1450,1,940,
1451,1,296,1055,1,
716,1452,1,715,1314,
1,607,1453,1,606,
1672,16,0,233,1,
712,1320,1,1031,1062,
1,1029,1067,1,1028,
1454,1,385,1325,1,
169,1072,1,595,1455,
1,699,1456,1,910,
1331,1,909,1336,1,
373,1341,1,371,1346,
1,369,1351,1,44,
1079,1,1007,1355,1,
792,1457,1,789,1462,
1,19,1103,1,144,
1360,1,33,1087,1,
998,1466,1,2,1164,
1,125,1129,1,10,
1146,1,566,1364,1,
131,1104,1,130,1108,
1,129,1093,1,27,
1097,1,20,1102,1,
127,1121,1,773,1467,
1,436,1370,1,343,
1112,1,128,1117,1,
448,1375,1,126,1125,
1,446,1376,1,17,
1133,1,325,1159,1,
15,1137,1,14,1471,
1,9,1150,1,12,
1141,1,546,1382,1,
545,1386,1,544,1390,
1,757,1472,1,756,
1476,1,755,1395,1,
3,1155,1,432,1400,
1,645,1405,1,430,
1410,1,1,1415,1,
107,1420,1,36,1673,
19,198,1,36,1674,
5,76,1,534,1442,
1,619,1284,1,92,
1045,1,95,1041,1,
421,1289,1,848,1447,
1,847,1675,16,0,
196,1,96,1036,1,
298,1050,1,950,1295,
1,841,1449,1,733,
1301,1,730,1306,1,
621,1450,1,940,1451,
1,296,1055,1,716,
1452,1,715,1314,1,
607,1453,1,606,1676,
16,0,315,1,712,
1320,1,1031,1062,1,
1029,1067,1,1028,1454,
1,385,1325,1,169,
1072,1,595,1455,1,
699,1456,1,910,1331,
1,909,1336,1,373,
1341,1,371,1346,1,
369,1351,1,44,1079,
1,1007,1355,1,792,
1457,1,789,1462,1,
19,1103,1,144,1360,
1,33,1087,1,998,
1466,1,2,1164,1,
125,1129,1,10,1146,
1,566,1364,1,131,
1104,1,130,1108,1,
129,1093,1,27,1097,
1,20,1102,1,127,
1121,1,773,1467,1,
436,1370,1,343,1112,
1,128,1117,1,448,
1375,1,126,1125,1,
446,1376,1,17,1133,
1,325,1159,1,15,
1137,1,14,1471,1,
9,1150,1,12,1141,
1,546,1382,1,545,
1386,1,544,1390,1,
757,1472,1,756,1476,
1,755,1395,1,3,
1155,1,432,1400,1,
645,1405,1,430,1410,
1,1,1415,1,107,
1420,1,35,1677,19,
216,1,35,1678,5,
33,1,92,1045,1,
1031,1062,1,325,1159,
1,1029,1067,1,33,
1087,1,131,1104,1,
130,1108,1,129,1093,
1,128,1117,1,127,
1121,1,126,1125,1,
125,1129,1,169,1072,
1,27,1097,1,343,
1112,1,824,1679,16,
0,214,1,20,1102,
1,19,1103,1,17,
1133,1,298,1050,1,
15,1137,1,296,1055,
1,107,1420,1,12,
1141,1,10,1146,1,
9,1150,1,578,1680,
16,0,321,1,3,
1155,1,2,1164,1,
144,1360,1,96,1036,
1,95,1041,1,44,
1079,1,34,1681,19,
327,1,34,1682,5,
72,1,534,1683,16,
0,325,1,619,1284,
1,92,1045,1,95,
1041,1,421,1289,1,
848,1684,16,0,325,
1,96,1036,1,298,
1050,1,950,1295,1,
841,1685,16,0,325,
1,733,1301,1,730,
1306,1,621,1686,16,
0,325,1,940,1687,
16,0,325,1,296,
1055,1,716,1688,16,
0,325,1,715,1314,
1,607,1689,16,0,
325,1,712,1320,1,
1031,1062,1,1029,1067,
1,1028,1690,16,0,
325,1,385,1325,1,
169,1072,1,595,1691,
16,0,325,1,699,
1692,16,0,325,1,
910,1331,1,909,1336,
1,373,1341,1,371,
1346,1,369,1351,1,
44,1079,1,1007,1355,
1,19,1103,1,0,
1693,16,0,325,1,
144,1360,1,33,1087,
1,998,1694,16,0,
325,1,2,1164,1,
125,1129,1,10,1146,
1,566,1364,1,131,
1104,1,130,1108,1,
129,1093,1,27,1097,
1,20,1102,1,127,
1121,1,773,1695,16,
0,325,1,436,1370,
1,343,1112,1,128,
1117,1,448,1375,1,
126,1125,1,446,1376,
1,17,1133,1,325,
1159,1,15,1137,1,
14,1696,16,0,325,
1,9,1150,1,12,
1141,1,546,1382,1,
545,1386,1,544,1390,
1,757,1697,16,0,
325,1,755,1395,1,
3,1155,1,432,1400,
1,645,1405,1,430,
1410,1,1,1415,1,
107,1420,1,33,1698,
19,141,1,33,1699,
5,12,1,734,1700,
16,0,243,1,20,
1701,17,1702,15,1703,
4,16,37,0,118,
0,97,0,114,0,
108,0,105,0,115,
0,116,0,1,-1,
1,5,1704,20,663,
1,153,1,3,1,
2,1,1,1705,22,
1,59,1,19,1103,
1,363,1706,17,1707,
15,1703,1,-1,1,
5,1708,20,682,1,
152,1,3,1,4,
1,3,1709,22,1,
58,1,448,1710,16,
0,139,1,343,1112,
1,208,1711,16,0,
242,1,169,1712,16,
0,260,1,373,1713,
16,0,184,1,27,
1097,1,371,1346,1,
369,1351,1,32,1714,
19,420,1,32,1715,
5,43,1,209,1716,
16,0,418,1,634,
1717,16,0,418,1,
97,1718,16,0,418,
1,735,1719,16,0,
418,1,302,1720,16,
0,418,1,301,1175,
1,300,1179,1,402,
1721,16,0,418,1,
506,1722,16,0,418,
1,69,1723,16,0,
418,1,374,1724,16,
0,418,1,50,1186,
1,813,1725,16,0,
418,1,170,1726,16,
0,418,1,62,1727,
16,0,418,1,61,
1193,1,60,1196,1,
59,1199,1,58,1202,
1,57,1205,1,56,
1208,1,55,1211,1,
54,1214,1,53,1217,
1,52,1220,1,51,
1223,1,478,1728,16,
0,418,1,49,1227,
1,48,1230,1,47,
1233,1,45,1729,16,
0,418,1,567,1730,
16,0,418,1,546,
1731,16,0,418,1,
671,1732,16,0,418,
1,28,1733,16,0,
418,1,450,1734,16,
0,418,1,977,1735,
16,0,418,1,867,
1736,16,0,418,1,
7,1244,1,6,1248,
1,5,1251,1,4,
1737,16,0,418,1,
206,1738,16,0,418,
1,31,1739,19,373,
1,31,1740,5,45,
1,210,1741,16,0,
371,1,207,1742,16,
0,371,1,96,1036,
1,95,1041,1,92,
1045,1,517,1743,16,
0,371,1,298,1050,
1,296,1055,1,76,
1744,16,0,371,1,
824,1745,16,0,371,
1,171,1746,16,0,
371,1,1031,1062,1,
1029,1067,1,385,1747,
16,0,371,1,169,
1072,1,489,1748,16,
0,371,1,46,1749,
16,0,371,1,44,
1079,1,578,1750,16,
0,371,1,682,1751,
16,0,371,1,144,
1752,16,0,371,1,
33,1087,1,461,1753,
16,0,371,1,129,
1093,1,27,1097,1,
20,1102,1,19,1103,
1,131,1104,1,130,
1108,1,343,1112,1,
128,1117,1,127,1121,
1,126,1125,1,125,
1129,1,17,1133,1,
15,1137,1,12,1141,
1,10,1146,1,9,
1150,1,327,1754,16,
0,371,1,3,1155,
1,325,1159,1,645,
1755,16,0,371,1,
2,1164,1,107,1756,
16,0,371,1,30,
1757,19,370,1,30,
1758,5,45,1,210,
1759,16,0,368,1,
207,1760,16,0,368,
1,96,1036,1,95,
1041,1,92,1045,1,
517,1761,16,0,368,
1,298,1050,1,296,
1055,1,76,1762,16,
0,368,1,824,1763,
16,0,368,1,171,
1764,16,0,368,1,
1031,1062,1,1029,1067,
1,385,1765,16,0,
368,1,169,1072,1,
489,1766,16,0,368,
1,46,1767,16,0,
368,1,44,1079,1,
578,1768,16,0,368,
1,682,1769,16,0,
368,1,144,1770,16,
0,368,1,33,1087,
1,461,1771,16,0,
368,1,129,1093,1,
27,1097,1,20,1102,
1,19,1103,1,131,
1104,1,130,1108,1,
343,1112,1,128,1117,
1,127,1121,1,126,
1125,1,125,1129,1,
17,1133,1,15,1137,
1,12,1141,1,10,
1146,1,9,1150,1,
327,1772,16,0,368,
1,3,1155,1,325,
1159,1,645,1773,16,
0,368,1,2,1164,
1,107,1774,16,0,
368,1,29,1775,19,
361,1,29,1776,5,
45,1,210,1777,16,
0,359,1,207,1778,
16,0,359,1,96,
1036,1,95,1041,1,
92,1045,1,517,1779,
16,0,359,1,298,
1050,1,296,1055,1,
76,1780,16,0,359,
1,824,1781,16,0,
359,1,171,1782,16,
0,359,1,1031,1062,
1,1029,1067,1,385,
1783,16,0,359,1,
169,1072,1,489,1784,
16,0,359,1,46,
1785,16,0,359,1,
44,1079,1,578,1786,
16,0,359,1,682,
1787,16,0,359,1,
144,1788,16,0,359,
1,33,1087,1,461,
1789,16,0,359,1,
129,1093,1,27,1097,
1,20,1102,1,19,
1103,1,131,1104,1,
130,1108,1,343,1112,
1,128,1117,1,127,
1121,1,126,1125,1,
125,1129,1,17,1133,
1,15,1137,1,12,
1141,1,10,1146,1,
9,1150,1,327,1790,
16,0,359,1,3,
1155,1,325,1159,1,
645,1791,16,0,359,
1,2,1164,1,107,
1792,16,0,359,1,
28,1793,19,358,1,
28,1794,5,45,1,
210,1795,16,0,356,
1,207,1796,16,0,
356,1,96,1036,1,
95,1041,1,92,1045,
1,517,1797,16,0,
356,1,298,1050,1,
296,1055,1,76,1798,
16,0,356,1,824,
1799,16,0,356,1,
171,1800,16,0,356,
1,1031,1062,1,1029,
1067,1,385,1801,16,
0,356,1,169,1072,
1,489,1802,16,0,
356,1,46,1803,16,
0,356,1,44,1079,
1,578,1804,16,0,
356,1,682,1805,16,
0,356,1,144,1806,
16,0,356,1,33,
1087,1,461,1807,16,
0,356,1,129,1093,
1,27,1097,1,20,
1102,1,19,1103,1,
131,1104,1,130,1108,
1,343,1112,1,128,
1117,1,127,1121,1,
126,1125,1,125,1129,
1,17,1133,1,15,
1137,1,12,1141,1,
10,1146,1,9,1150,
1,327,1808,16,0,
356,1,3,1155,1,
325,1159,1,645,1809,
16,0,356,1,2,
1164,1,107,1810,16,
0,356,1,27,1811,
19,364,1,27,1812,
5,45,1,210,1813,
16,0,362,1,207,
1814,16,0,362,1,
96,1036,1,95,1041,
1,92,1045,1,517,
1815,16,0,362,1,
298,1050,1,296,1055,
1,76,1816,16,0,
362,1,824,1817,16,
0,362,1,171,1818,
16,0,362,1,1031,
1062,1,1029,1067,1,
385,1819,16,0,362,
1,169,1072,1,489,
1820,16,0,362,1,
46,1821,16,0,362,
1,44,1079,1,578,
1822,16,0,362,1,
682,1823,16,0,362,
1,144,1824,16,0,
362,1,33,1087,1,
461,1825,16,0,362,
1,129,1093,1,27,
1097,1,20,1102,1,
19,1103,1,131,1104,
1,130,1108,1,343,
1112,1,128,1117,1,
127,1121,1,126,1125,
1,125,1129,1,17,
1133,1,15,1137,1,
12,1141,1,10,1146,
1,9,1150,1,327,
1826,16,0,362,1,
3,1155,1,325,1159,
1,645,1827,16,0,
362,1,2,1164,1,
107,1828,16,0,362,
1,26,1829,19,355,
1,26,1830,5,45,
1,210,1831,16,0,
353,1,207,1832,16,
0,353,1,96,1036,
1,95,1041,1,92,
1045,1,517,1833,16,
0,353,1,298,1050,
1,296,1055,1,76,
1834,16,0,353,1,
824,1835,16,0,353,
1,171,1836,16,0,
353,1,1031,1062,1,
1029,1067,1,385,1837,
16,0,353,1,169,
1072,1,489,1838,16,
0,353,1,46,1839,
16,0,353,1,44,
1079,1,578,1840,16,
0,353,1,682,1841,
16,0,353,1,144,
1842,16,0,353,1,
33,1087,1,461,1843,
16,0,353,1,129,
1093,1,27,1097,1,
20,1102,1,19,1103,
1,131,1104,1,130,
1108,1,343,1112,1,
128,1117,1,127,1121,
1,126,1125,1,125,
1129,1,17,1133,1,
15,1137,1,12,1141,
1,10,1146,1,9,
1150,1,327,1844,16,
0,353,1,3,1155,
1,325,1159,1,645,
1845,16,0,353,1,
2,1164,1,107,1846,
16,0,353,1,25,
1847,19,376,1,25,
1848,5,45,1,210,
1849,16,0,374,1,
207,1850,16,0,374,
1,96,1036,1,95,
1041,1,92,1045,1,
517,1851,16,0,374,
1,298,1050,1,296,
1055,1,76,1852,16,
0,374,1,824,1853,
16,0,374,1,171,
1854,16,0,374,1,
1031,1062,1,1029,1067,
1,385,1855,16,0,
374,1,169,1072,1,
489,1856,16,0,374,
1,46,1857,16,0,
374,1,44,1079,1,
578,1858,16,0,374,
1,682,1859,16,0,
374,1,144,1860,16,
0,374,1,33,1087,
1,461,1861,16,0,
374,1,129,1093,1,
27,1097,1,20,1102,
1,19,1103,1,131,
1104,1,130,1108,1,
343,1112,1,128,1117,
1,127,1121,1,126,
1125,1,125,1129,1,
17,1133,1,15,1137,
1,12,1141,1,10,
1146,1,9,1150,1,
327,1862,16,0,374,
1,3,1155,1,325,
1159,1,645,1863,16,
0,374,1,2,1164,
1,107,1864,16,0,
374,1,24,1865,19,
367,1,24,1866,5,
45,1,210,1867,16,
0,365,1,207,1868,
16,0,365,1,96,
1036,1,95,1041,1,
92,1045,1,517,1869,
16,0,365,1,298,
1050,1,296,1055,1,
76,1870,16,0,365,
1,824,1871,16,0,
365,1,171,1872,16,
0,365,1,1031,1062,
1,1029,1067,1,385,
1873,16,0,365,1,
169,1072,1,489,1874,
16,0,365,1,46,
1875,16,0,365,1,
44,1079,1,578,1876,
16,0,365,1,682,
1877,16,0,365,1,
144,1878,16,0,365,
1,33,1087,1,461,
1879,16,0,365,1,
129,1093,1,27,1097,
1,20,1102,1,19,
1103,1,131,1104,1,
130,1108,1,343,1112,
1,128,1117,1,127,
1121,1,126,1125,1,
125,1129,1,17,1133,
1,15,1137,1,12,
1141,1,10,1146,1,
9,1150,1,327,1880,
16,0,365,1,3,
1155,1,325,1159,1,
645,1881,16,0,365,
1,2,1164,1,107,
1882,16,0,365,1,
23,1883,19,349,1,
23,1884,5,45,1,
210,1885,16,0,347,
1,207,1886,16,0,
347,1,96,1036,1,
95,1041,1,92,1045,
1,517,1887,16,0,
347,1,298,1050,1,
296,1055,1,76,1888,
16,0,347,1,824,
1889,16,0,347,1,
171,1890,16,0,347,
1,1031,1062,1,1029,
1067,1,385,1891,16,
0,347,1,169,1072,
1,489,1892,16,0,
347,1,46,1893,16,
0,347,1,44,1079,
1,578,1894,16,0,
347,1,682,1895,16,
0,347,1,144,1896,
16,0,347,1,33,
1087,1,461,1897,16,
0,347,1,129,1093,
1,27,1097,1,20,
1102,1,19,1103,1,
131,1104,1,130,1108,
1,343,1112,1,128,
1117,1,127,1121,1,
126,1125,1,125,1129,
1,17,1133,1,15,
1137,1,12,1141,1,
10,1146,1,9,1150,
1,327,1898,16,0,
347,1,3,1155,1,
325,1159,1,645,1899,
16,0,347,1,2,
1164,1,107,1900,16,
0,347,1,22,1901,
19,346,1,22,1902,
5,45,1,210,1903,
16,0,344,1,207,
1904,16,0,344,1,
96,1036,1,95,1041,
1,92,1045,1,517,
1905,16,0,344,1,
298,1050,1,296,1055,
1,76,1906,16,0,
344,1,824,1907,16,
0,344,1,171,1908,
16,0,344,1,1031,
1062,1,1029,1067,1,
385,1909,16,0,344,
1,169,1072,1,489,
1910,16,0,344,1,
46,1911,16,0,344,
1,44,1079,1,578,
1912,16,0,344,1,
682,1913,16,0,344,
1,144,1914,16,0,
344,1,33,1087,1,
461,1915,16,0,344,
1,129,1093,1,27,
1097,1,20,1102,1,
19,1103,1,131,1104,
1,130,1108,1,343,
1112,1,128,1117,1,
127,1121,1,126,1125,
1,125,1129,1,17,
1133,1,15,1137,1,
12,1141,1,10,1146,
1,9,1150,1,327,
1916,16,0,344,1,
3,1155,1,325,1159,
1,645,1917,16,0,
344,1,2,1164,1,
107,1918,16,0,344,
1,21,1919,19,343,
1,21,1920,5,45,
1,210,1921,16,0,
341,1,207,1922,16,
0,341,1,96,1036,
1,95,1041,1,92,
1045,1,517,1923,16,
0,341,1,298,1050,
1,296,1055,1,76,
1924,16,0,341,1,
824,1925,16,0,341,
1,171,1926,16,0,
341,1,1031,1062,1,
1029,1067,1,385,1927,
16,0,341,1,169,
1072,1,489,1928,16,
0,341,1,46,1929,
16,0,341,1,44,
1079,1,578,1930,16,
0,341,1,682,1931,
16,0,341,1,144,
1932,16,0,341,1,
33,1087,1,461,1933,
16,0,341,1,129,
1093,1,27,1097,1,
20,1102,1,19,1103,
1,131,1104,1,130,
1108,1,343,1112,1,
128,1117,1,127,1121,
1,126,1125,1,125,
1129,1,17,1133,1,
15,1137,1,12,1141,
1,10,1146,1,9,
1150,1,327,1934,16,
0,341,1,3,1155,
1,325,1159,1,645,
1935,16,0,341,1,
2,1164,1,107,1936,
16,0,341,1,20,
1937,19,423,1,20,
1938,5,43,1,209,
1939,16,0,421,1,
634,1940,16,0,421,
1,97,1941,16,0,
421,1,735,1942,16,
0,421,1,302,1943,
16,0,421,1,301,
1175,1,300,1179,1,
402,1944,16,0,421,
1,506,1945,16,0,
421,1,69,1946,16,
0,421,1,374,1947,
16,0,421,1,50,
1186,1,813,1948,16,
0,421,1,170,1949,
16,0,421,1,62,
1950,16,0,421,1,
61,1193,1,60,1196,
1,59,1199,1,58,
1202,1,57,1205,1,
56,1208,1,55,1211,
1,54,1214,1,53,
1217,1,52,1220,1,
51,1223,1,478,1951,
16,0,421,1,49,
1227,1,48,1230,1,
47,1233,1,45,1952,
16,0,421,1,567,
1953,16,0,421,1,
546,1954,16,0,421,
1,671,1955,16,0,
421,1,28,1956,16,
0,421,1,450,1957,
16,0,421,1,977,
1958,16,0,421,1,
867,1959,16,0,421,
1,7,1244,1,6,
1248,1,5,1251,1,
4,1960,16,0,421,
1,206,1961,16,0,
421,1,19,1962,19,
340,1,19,1963,5,
45,1,210,1964,16,
0,338,1,207,1965,
16,0,338,1,96,
1036,1,95,1041,1,
92,1045,1,517,1966,
16,0,338,1,298,
1050,1,296,1055,1,
76,1967,16,0,338,
1,824,1968,16,0,
338,1,171,1969,16,
0,338,1,1031,1062,
1,1029,1067,1,385,
1970,16,0,338,1,
169,1072,1,489,1971,
16,0,338,1,46,
1972,16,0,338,1,
44,1079,1,578,1973,
16,0,338,1,682,
1974,16,0,338,1,
144,1975,16,0,338,
1,33,1087,1,461,
1976,16,0,338,1,
129,1093,1,27,1097,
1,20,1102,1,19,
1103,1,131,1104,1,
130,1108,1,343,1112,
1,128,1117,1,127,
1121,1,126,1125,1,
125,1129,1,17,1133,
1,15,1137,1,12,
1141,1,10,1146,1,
9,1150,1,327,1977,
16,0,338,1,3,
1155,1,325,1159,1,
645,1978,16,0,338,
1,2,1164,1,107,
1979,16,0,338,1,
18,1980,19,337,1,
18,1981,5,88,1,
461,1982,16,0,335,
1,450,1983,16,0,
417,1,210,1984,16,
0,335,1,209,1985,
16,0,417,1,207,
1986,16,0,335,1,
206,1987,16,0,417,
1,682,1988,16,0,
335,1,671,1989,16,
0,417,1,171,1990,
16,0,335,1,170,
1991,16,0,417,1,
169,1072,1,645,1992,
16,0,335,1,402,
1993,16,0,417,1,
634,1994,16,0,417,
1,867,1995,16,0,
417,1,385,1996,16,
0,335,1,144,1997,
16,0,335,1,374,
1998,16,0,417,1,
824,1999,16,0,335,
1,131,1104,1,130,
1108,1,129,1093,1,
128,1117,1,127,1121,
1,126,1125,1,125,
1129,1,107,2000,16,
0,335,1,343,1112,
1,96,1036,1,578,
2001,16,0,335,1,
97,2002,16,0,417,
1,813,2003,16,0,
417,1,95,1041,1,
92,1045,1,567,2004,
16,0,417,1,327,
2005,16,0,335,1,
546,2006,16,0,417,
1,325,1159,1,301,
1175,1,506,2007,16,
0,417,1,76,2008,
16,0,335,1,1031,
1062,1,1029,1067,1,
300,1179,1,298,1050,
1,69,2009,16,0,
417,1,296,1055,1,
302,2010,16,0,417,
1,62,2011,16,0,
417,1,61,1193,1,
60,1196,1,59,1199,
1,58,1202,1,57,
1205,1,56,1208,1,
55,1211,1,54,1214,
1,53,1217,1,52,
1220,1,51,1223,1,
50,1186,1,49,1227,
1,48,1230,1,47,
1233,1,46,2012,16,
0,335,1,45,2013,
16,0,417,1,44,
1079,1,517,2014,16,
0,335,1,33,1087,
1,28,2015,16,0,
417,1,27,1097,1,
977,2016,16,0,417,
1,20,1102,1,19,
1103,1,735,2017,16,
0,417,1,17,1133,
1,15,1137,1,12,
1141,1,489,2018,16,
0,335,1,10,1146,
1,9,1150,1,7,
1244,1,6,1248,1,
5,1251,1,4,2019,
16,0,417,1,3,
1155,1,2,1164,1,
478,2020,16,0,417,
1,17,2021,19,334,
1,17,2022,5,45,
1,210,2023,16,0,
332,1,207,2024,16,
0,332,1,96,1036,
1,95,1041,1,92,
1045,1,517,2025,16,
0,332,1,298,1050,
1,296,1055,1,76,
2026,16,0,332,1,
824,2027,16,0,332,
1,171,2028,16,0,
332,1,1031,1062,1,
1029,1067,1,385,2029,
16,0,332,1,169,
1072,1,489,2030,16,
0,332,1,46,2031,
16,0,332,1,44,
1079,1,578,2032,16,
0,332,1,682,2033,
16,0,332,1,144,
2034,16,0,332,1,
33,1087,1,461,2035,
16,0,332,1,129,
1093,1,27,1097,1,
20,1102,1,19,1103,
1,131,1104,1,130,
1108,1,343,1112,1,
128,1117,1,127,1121,
1,126,1125,1,125,
1129,1,17,1133,1,
15,1137,1,12,1141,
1,10,1146,1,9,
1150,1,327,2036,16,
0,332,1,3,1155,
1,325,1159,1,645,
2037,16,0,332,1,
2,1164,1,107,2038,
16,0,332,1,16,
2039,19,157,1,16,
2040,5,20,1,92,
1045,1,44,1079,1,
325,1159,1,33,2041,
16,0,395,1,169,
1072,1,27,1097,1,
343,1112,1,22,2042,
16,0,395,1,20,
1102,1,19,1103,1,
298,1050,1,438,2043,
16,0,155,1,296,
1055,1,10,1146,1,
9,1150,1,1,2044,
16,0,395,1,2,
1164,1,3,1155,1,
96,1036,1,95,1041,
1,15,2045,19,230,
1,15,2046,5,41,
1,210,2047,17,2048,
15,2049,4,30,37,
0,70,0,105,0,
101,0,108,0,100,
0,69,0,120,0,
112,0,65,0,115,
0,115,0,105,0,
103,0,110,0,1,
-1,1,5,2050,20,
571,1,165,1,3,
1,6,1,5,2051,
22,1,95,1,96,
1036,1,95,1041,1,
92,1045,1,302,2052,
17,2053,15,2054,4,
20,37,0,102,0,
105,0,101,0,108,
0,100,0,108,0,
105,0,115,0,116,
0,1,-1,1,5,
2055,20,851,1,127,
1,3,1,3,1,
2,2056,22,1,32,
1,301,1175,1,300,
1179,1,299,2057,17,
2058,15,2054,1,-1,
1,5,2059,20,859,
1,124,1,3,1,
2,1,1,2060,22,
1,30,1,298,1050,
1,296,1055,1,295,
2061,16,0,231,1,
1031,1062,1,1029,1067,
1,171,2062,17,2063,
15,2064,4,24,37,
0,70,0,105,0,
101,0,108,0,100,
0,65,0,115,0,
115,0,105,0,103,
0,110,0,1,-1,
1,5,2065,20,569,
1,166,1,3,1,
4,1,3,2066,22,
1,96,1,169,1072,
1,46,2067,17,2068,
15,2069,4,12,37,
0,102,0,105,0,
101,0,108,0,100,
0,1,-1,1,5,
2070,20,564,1,167,
1,3,1,2,1,
1,2071,22,1,97,
1,45,2072,16,0,
228,1,44,1079,1,
144,1360,1,33,1087,
1,19,1103,1,129,
1093,1,27,1097,1,
127,1121,1,131,1104,
1,130,1108,1,343,
1112,1,128,1117,1,
20,1102,1,126,1125,
1,125,1129,1,17,
1133,1,15,1137,1,
12,1141,1,10,1146,
1,9,1150,1,3,
1155,1,325,1159,1,
324,2073,17,2074,15,
2054,1,-1,1,5,
2075,20,856,1,126,
1,3,1,4,1,
3,2076,22,1,31,
1,2,1164,1,107,
1420,1,14,2077,19,
380,1,14,2078,5,
63,1,209,2079,16,
0,378,1,634,2080,
16,0,378,1,97,
2081,16,0,378,1,
96,1036,1,95,1041,
1,735,2082,16,0,
378,1,92,1045,1,
302,2083,16,0,378,
1,301,1175,1,300,
1179,1,298,1050,1,
296,1055,1,402,2084,
16,0,378,1,506,
2085,16,0,378,1,
169,1072,1,69,2086,
16,0,378,1,50,
1186,1,53,1217,1,
813,2087,16,0,378,
1,170,2088,16,0,
378,1,62,2089,16,
0,378,1,61,1193,
1,60,1196,1,59,
1199,1,58,1202,1,
57,1205,1,56,1208,
1,55,1211,1,54,
1214,1,374,2090,16,
0,378,1,52,1220,
1,51,1223,1,478,
2091,16,0,378,1,
49,1227,1,48,1230,
1,47,1233,1,45,
2092,16,0,378,1,
44,1079,1,40,2093,
16,0,378,1,450,
2094,16,0,378,1,
33,2095,16,0,378,
1,567,2096,16,0,
378,1,546,2097,16,
0,378,1,671,2098,
16,0,378,1,28,
2099,16,0,378,1,
27,1097,1,22,2100,
16,0,378,1,343,
1112,1,20,1102,1,
19,1103,1,977,2101,
16,0,378,1,867,
2102,16,0,378,1,
10,1146,1,9,1150,
1,4,2103,16,0,
378,1,7,1244,1,
6,1248,1,5,1251,
1,325,1159,1,3,
1155,1,2,1164,1,
1,2104,16,0,378,
1,206,2105,16,0,
378,1,13,2106,19,
204,1,13,2107,5,
33,1,327,2108,16,
0,202,1,1031,1062,
1,325,1159,1,1029,
1067,1,33,1087,1,
131,1104,1,130,1108,
1,129,1093,1,128,
1117,1,127,1121,1,
126,1125,1,125,1129,
1,169,1072,1,27,
1097,1,343,1112,1,
19,1103,1,20,1102,
1,207,2109,16,0,
249,1,17,1133,1,
298,1050,1,15,1137,
1,296,1055,1,107,
1420,1,12,1141,1,
10,1146,1,9,1150,
1,92,1045,1,3,
1155,1,2,1164,1,
144,1360,1,96,1036,
1,95,1041,1,44,
1079,1,12,2110,19,
253,1,12,2111,5,
23,1,92,1045,1,
44,1079,1,325,1159,
1,33,2112,16,0,
393,1,169,1072,1,
27,1097,1,343,1112,
1,20,1102,1,22,
2113,16,0,393,1,
19,1103,1,302,2114,
16,0,251,1,301,
1175,1,300,1179,1,
298,1050,1,296,1055,
1,10,1146,1,9,
1150,1,1,2115,16,
0,393,1,95,1041,
1,2,1164,1,3,
1155,1,96,1036,1,
45,2116,16,0,251,
1,11,2117,19,118,
1,11,2118,5,41,
1,421,1289,1,1027,
2119,16,0,116,1,
96,1036,1,95,1041,
1,92,1045,1,1053,
2120,17,2121,15,2122,
4,16,37,0,112,
0,97,0,114,0,
108,0,105,0,115,
0,116,0,1,-1,
1,5,2123,20,815,
1,131,1,3,1,
4,1,3,2124,22,
1,36,1,1050,2125,
17,2126,15,2122,1,
-1,1,5,2127,20,
817,1,130,1,3,
1,2,1,1,2128,
22,1,35,1,1049,
2129,17,2130,15,2122,
1,-1,1,5,313,
1,1,1,1,2131,
22,1,37,1,298,
1050,1,296,1055,1,
76,2132,16,0,311,
1,1031,1062,1,1029,
1067,1,385,1325,1,
169,1072,1,44,1079,
1,144,1360,1,33,
1087,1,131,1104,1,
129,1093,1,27,1097,
1,127,1121,1,126,
1125,1,130,1108,1,
343,1112,1,128,1117,
1,20,1102,1,19,
1103,1,125,1129,1,
17,1133,1,15,1137,
1,13,2133,16,0,
410,1,12,1141,1,
10,1146,1,9,1150,
1,8,2134,16,0,
415,1,4,2135,16,
0,414,1,325,1159,
1,3,1155,1,2,
1164,1,107,1420,1,
10,2136,19,324,1,
10,2137,5,123,1,
716,2138,16,0,322,
1,715,1314,1,712,
1320,1,950,1295,1,
910,1331,1,699,2139,
16,0,322,1,209,
2140,16,0,322,1,
671,2141,16,0,322,
1,450,2142,16,0,
322,1,448,1375,1,
446,1376,1,206,2143,
16,0,322,1,444,
2144,16,0,411,1,
443,2145,17,2146,15,
2147,4,18,37,0,
102,0,117,0,110,
0,99,0,110,0,
97,0,109,0,101,
0,1,-1,1,5,
152,1,3,1,3,
2148,22,1,63,1,
440,2149,17,2150,15,
2147,1,-1,1,5,
152,1,3,1,3,
2151,22,1,64,1,
438,2152,17,2153,15,
2147,1,-1,1,5,
2154,20,615,1,157,
1,3,1,2,1,
1,2155,22,1,65,
1,436,1370,1,434,
2156,16,0,411,1,
432,1400,1,909,1336,
1,430,1410,1,421,
1289,1,169,1072,1,
170,2157,16,0,322,
1,369,1351,1,645,
1405,1,402,2158,16,
0,322,1,848,2159,
16,0,322,1,634,
2160,16,0,322,1,
867,2161,16,0,322,
1,607,2162,16,0,
322,1,385,1325,1,
144,1360,1,621,2163,
16,0,322,1,619,
1284,1,374,2164,16,
0,322,1,373,1341,
1,371,1346,1,131,
1104,1,130,1108,1,
129,1093,1,128,1117,
1,127,1121,1,126,
1125,1,125,1129,1,
841,2165,16,0,322,
1,595,2166,16,0,
322,1,107,1420,1,
343,1112,1,96,1036,
1,97,2167,16,0,
322,1,813,2168,16,
0,322,1,95,1041,
1,92,1045,1,567,
2169,16,0,322,1,
566,1364,1,546,2170,
16,0,322,1,325,
1159,1,544,1390,1,
51,1223,1,1031,1062,
1,534,2171,16,0,
322,1,1029,1067,1,
1028,2172,16,0,322,
1,56,1208,1,69,
2173,16,0,322,1,
59,1199,1,545,1386,
1,57,1205,1,62,
2174,16,0,322,1,
61,1193,1,302,2175,
16,0,322,1,301,
1175,1,300,1179,1,
60,1196,1,298,1050,
1,58,1202,1,296,
1055,1,773,2176,16,
0,322,1,55,1211,
1,54,1214,1,53,
1217,1,52,1220,1,
1007,1355,1,50,1186,
1,49,1227,1,48,
1230,1,47,1233,1,
45,2177,16,0,322,
1,44,1079,1,40,
2178,16,0,424,1,
998,2179,16,0,322,
1,757,2180,16,0,
322,1,755,1395,1,
33,2181,16,0,424,
1,28,2182,16,0,
322,1,506,2183,16,
0,322,1,27,1097,
1,15,1137,1,21,
2184,16,0,322,1,
22,2185,16,0,424,
1,977,2186,16,0,
322,1,20,1102,1,
19,1103,1,735,2187,
16,0,322,1,17,
1133,1,733,1301,1,
940,2188,16,0,322,
1,14,2189,16,0,
322,1,730,1306,1,
12,1141,1,11,2190,
16,0,411,1,10,
1146,1,9,1150,1,
0,2191,16,0,322,
1,7,1244,1,6,
1248,1,5,1251,1,
4,2192,16,0,322,
1,3,1155,1,2,
1164,1,1,2193,16,
0,424,1,478,2194,
16,0,322,1,9,
2195,19,224,1,9,
2196,5,62,1,619,
1284,1,210,2047,1,
421,1289,1,96,1036,
1,95,1041,1,950,
1295,1,92,1045,1,
733,1301,1,730,1306,
1,299,2197,16,0,
222,1,298,1050,1,
296,1055,1,715,1314,
1,171,2062,1,712,
1320,1,1031,1062,1,
1029,1067,1,385,1325,
1,169,1072,1,910,
1331,1,909,1336,1,
373,1341,1,371,1346,
1,369,1351,1,44,
1079,1,46,2067,1,
1007,1355,1,436,1370,
1,144,1360,1,33,
1087,1,125,1129,1,
566,1364,1,2,1164,
1,19,1103,1,129,
1093,1,27,1097,1,
20,1102,1,127,1121,
1,131,1104,1,130,
1108,1,343,1112,1,
128,1117,1,448,1375,
1,126,1125,1,446,
1376,1,17,1133,1,
325,1159,1,15,1137,
1,10,1146,1,9,
1150,1,12,1141,1,
546,1382,1,545,1386,
1,544,1390,1,757,
2198,16,0,236,1,
755,1395,1,3,1155,
1,432,1400,1,645,
1405,1,430,1410,1,
1,1415,1,107,1420,
1,8,2199,19,161,
1,8,2200,5,20,
1,92,1045,1,44,
1079,1,325,1159,1,
33,2201,16,0,388,
1,169,1072,1,27,
1097,1,343,1112,1,
22,2202,16,0,388,
1,20,1102,1,19,
1103,1,298,1050,1,
438,2203,16,0,159,
1,296,1055,1,10,
1146,1,9,1150,1,
1,2204,16,0,388,
1,2,1164,1,3,
1155,1,96,1036,1,
95,1041,1,7,2205,
19,121,1,7,2206,
5,41,1,210,2047,
1,96,1036,1,95,
1041,1,92,1045,1,
1050,2207,16,0,382,
1,299,2208,16,0,
221,1,298,1050,1,
296,1055,1,171,2062,
1,1031,1062,1,1029,
1067,1,385,2209,16,
0,177,1,169,1072,
1,489,2210,16,0,
119,1,369,2211,16,
0,187,1,46,2067,
1,44,1079,1,33,
1087,1,144,1360,1,
448,2212,16,0,187,
1,461,2213,16,0,
125,1,19,1103,1,
129,1093,1,27,1097,
1,127,1121,1,131,
1104,1,130,1108,1,
343,1112,1,128,1117,
1,20,2214,16,0,
404,1,126,1125,1,
125,1129,1,17,1133,
1,15,1137,1,12,
1141,1,10,1146,1,
9,1150,1,325,1159,
1,3,1155,1,2,
1164,1,107,1420,1,
5,2215,19,278,1,
5,2216,5,43,1,
209,2217,16,0,276,
1,634,2218,16,0,
276,1,97,2219,16,
0,276,1,735,2220,
16,0,276,1,302,
2221,16,0,276,1,
301,1175,1,300,1179,
1,402,2222,16,0,
276,1,506,2223,16,
0,276,1,69,2224,
16,0,276,1,374,
2225,16,0,276,1,
50,1186,1,813,2226,
16,0,276,1,170,
2227,16,0,276,1,
62,2228,16,0,276,
1,61,1193,1,60,
1196,1,59,1199,1,
58,1202,1,57,1205,
1,56,1208,1,55,
1211,1,54,1214,1,
53,1217,1,52,1220,
1,51,1223,1,478,
2229,16,0,276,1,
49,1227,1,48,1230,
1,47,1233,1,45,
2230,16,0,276,1,
567,2231,16,0,276,
1,546,2232,16,0,
276,1,671,2233,16,
0,276,1,28,2234,
16,0,276,1,450,
2235,16,0,276,1,
977,2236,16,0,276,
1,867,2237,16,0,
276,1,7,1244,1,
6,1248,1,5,1251,
1,4,2238,16,0,
276,1,206,2239,16,
0,276,1,4,2240,
19,281,1,4,2241,
5,63,1,209,2242,
16,0,279,1,634,
2243,16,0,279,1,
97,2244,16,0,279,
1,96,1036,1,95,
1041,1,735,2245,16,
0,279,1,92,1045,
1,302,2246,16,0,
279,1,301,1175,1,
300,1179,1,298,1050,
1,296,1055,1,402,
2247,16,0,279,1,
506,2248,16,0,279,
1,169,1072,1,69,
2249,16,0,279,1,
50,1186,1,53,1217,
1,813,2250,16,0,
279,1,170,2251,16,
0,279,1,62,2252,
16,0,279,1,61,
1193,1,60,1196,1,
59,1199,1,58,1202,
1,57,1205,1,56,
1208,1,55,1211,1,
54,1214,1,374,2253,
16,0,279,1,52,
1220,1,51,1223,1,
478,2254,16,0,279,
1,49,1227,1,48,
1230,1,47,1233,1,
45,2255,16,0,279,
1,44,1079,1,40,
2256,16,0,426,1,
450,2257,16,0,279,
1,33,2258,16,0,
426,1,567,2259,16,
0,279,1,546,2260,
16,0,279,1,671,
2261,16,0,279,1,
28,2262,16,0,279,
1,27,1097,1,22,
2263,16,0,426,1,
343,1112,1,20,1102,
1,19,1103,1,977,
2264,16,0,279,1,
867,2265,16,0,279,
1,10,1146,1,9,
1150,1,4,2266,16,
0,279,1,7,1244,
1,6,1248,1,5,
1251,1,325,1159,1,
3,1155,1,2,1164,
1,1,2267,16,0,
426,1,206,2268,16,
0,279,1,3,2269,
19,144,1,3,2270,
5,126,1,716,2271,
16,0,406,1,715,
1314,1,712,1320,1,
950,1295,1,1051,2272,
16,0,383,1,910,
1331,1,699,2273,16,
0,406,1,209,2274,
16,0,406,1,671,
2275,16,0,406,1,
450,2276,16,0,406,
1,448,1375,1,447,
2277,16,0,142,1,
446,1376,1,206,2278,
16,0,406,1,441,
2279,16,0,162,1,
439,2280,16,0,158,
1,437,2281,16,0,
162,1,436,1370,1,
433,2282,16,0,167,
1,432,1400,1,909,
1336,1,430,1410,1,
421,1289,1,634,2283,
16,0,406,1,170,
2284,16,0,406,1,
169,1072,1,645,1405,
1,402,2285,16,0,
406,1,848,2286,16,
0,406,1,370,2287,
16,0,188,1,369,
1351,1,867,2288,16,
0,406,1,607,2289,
16,0,406,1,385,
1325,1,144,1360,1,
621,2290,16,0,406,
1,368,2291,16,0,
188,1,619,1284,1,
374,2292,16,0,406,
1,373,1341,1,371,
1346,1,131,1104,1,
130,1108,1,129,1093,
1,128,1117,1,127,
1121,1,126,1125,1,
125,1129,1,841,2293,
16,0,406,1,595,
2294,16,0,406,1,
107,1420,1,343,1112,
1,96,1036,1,97,
2295,16,0,406,1,
813,2296,16,0,406,
1,95,1041,1,92,
1045,1,567,2297,16,
0,406,1,566,1364,
1,546,2298,16,0,
406,1,325,1159,1,
544,1390,1,51,1223,
1,1031,1062,1,534,
2299,16,0,406,1,
1029,1067,1,1028,2300,
16,0,406,1,56,
1208,1,69,2301,16,
0,406,1,59,1199,
1,545,1386,1,57,
1205,1,62,2302,16,
0,406,1,61,1193,
1,302,2303,16,0,
261,1,301,1175,1,
300,1179,1,60,1196,
1,298,1050,1,58,
1202,1,296,1055,1,
773,2304,16,0,406,
1,55,1211,1,54,
1214,1,53,1217,1,
52,1220,1,1007,1355,
1,50,1186,1,49,
1227,1,48,1230,1,
47,1233,1,45,2305,
16,0,261,1,44,
1079,1,998,2306,16,
0,406,1,757,2307,
16,0,406,1,39,
2308,16,0,387,1,
755,1395,1,33,1087,
1,28,2309,16,0,
406,1,13,2310,16,
0,383,1,506,2311,
16,0,406,1,27,
1097,1,26,2312,16,
0,394,1,15,1137,
1,21,2313,16,0,
406,1,977,2314,16,
0,406,1,20,1102,
1,19,1103,1,735,
2315,16,0,406,1,
17,1133,1,733,1301,
1,940,2316,16,0,
406,1,14,2317,16,
0,406,1,730,1306,
1,12,1141,1,10,
1146,1,9,1150,1,
0,2318,16,0,406,
1,7,1244,1,6,
1248,1,5,1251,1,
4,2319,16,0,406,
1,3,1155,1,2,
1164,1,1,1415,1,
478,2320,16,0,406,
1,2,2321,19,291,
1,2,2322,5,61,
1,619,1284,1,421,
1289,1,96,1036,1,
95,1041,1,950,1295,
1,92,1045,1,733,
1301,1,730,1306,1,
298,1050,1,296,1055,
1,715,1314,1,712,
1320,1,1031,1062,1,
1029,1067,1,385,1325,
1,169,1072,1,910,
1331,1,909,1336,1,
373,1341,1,371,1346,
1,369,1351,1,44,
1079,1,1007,1355,1,
792,1457,1,789,1462,
1,436,1370,1,144,
1360,1,33,1087,1,
125,1129,1,2,1164,
1,566,1364,1,131,
1104,1,19,1103,1,
129,1093,1,27,1097,
1,20,1102,1,127,
1121,1,773,1467,1,
130,1108,1,343,1112,
1,128,1117,1,448,
1375,1,126,1125,1,
446,1376,1,17,1133,
1,325,1159,1,15,
1137,1,10,1146,1,
9,1150,1,12,1141,
1,546,1382,1,545,
1386,1,544,1390,1,
757,1472,1,755,1395,
1,3,1155,1,432,
1400,1,645,1405,1,
430,1410,1,1,1415,
1,107,1420,2,1,0};
new Sfactory(this,"error",new SCreator(error_factory));
new Sfactory(this,"exp_9",new SCreator(exp_9_factory));
new Sfactory(this,"exp_8",new SCreator(exp_8_factory));
new Sfactory(this,"FieldAssign",new SCreator(FieldAssign_factory));
new Sfactory(this,"functioncall_2",new SCreator(functioncall_2_factory));
new Sfactory(this,"binop_4",new SCreator(binop_4_factory));
new Sfactory(this,"ElseIf",new SCreator(ElseIf_factory));
new Sfactory(this,"binop_11",new SCreator(binop_11_factory));
new Sfactory(this,"LocalFuncDecl_1",new SCreator(LocalFuncDecl_1_factory));
new Sfactory(this,"prefixexp",new SCreator(prefixexp_factory));
new Sfactory(this,"LocalNamelist_1",new SCreator(LocalNamelist_1_factory));
new Sfactory(this,"prefixexp_2",new SCreator(prefixexp_2_factory));
new Sfactory(this,"LocalInit",new SCreator(LocalInit_factory));
new Sfactory(this,"FieldExpAssign",new SCreator(FieldExpAssign_factory));
new Sfactory(this,"FuncDecl_1",new SCreator(FuncDecl_1_factory));
new Sfactory(this,"stat_2",new SCreator(stat_2_factory));
new Sfactory(this,"Do",new SCreator(Do_factory));
new Sfactory(this,"funcname_1",new SCreator(funcname_1_factory));
new Sfactory(this,"explist",new SCreator(explist_factory));
new Sfactory(this,"parlist_2",new SCreator(parlist_2_factory));
new Sfactory(this,"parlist_3",new SCreator(parlist_3_factory));
new Sfactory(this,"parlist_1",new SCreator(parlist_1_factory));
new Sfactory(this,"FieldExpAssign_1",new SCreator(FieldExpAssign_1_factory));
new Sfactory(this,"Else_1",new SCreator(Else_1_factory));
new Sfactory(this,"LocalNamelist",new SCreator(LocalNamelist_factory));
new Sfactory(this,"Repeat_1",new SCreator(Repeat_1_factory));
new Sfactory(this,"binop_12",new SCreator(binop_12_factory));
new Sfactory(this,"fieldsep_2",new SCreator(fieldsep_2_factory));
new Sfactory(this,"TableRef",new SCreator(TableRef_factory));
new Sfactory(this,"function_1",new SCreator(function_1_factory));
new Sfactory(this,"funcbody_2",new SCreator(funcbody_2_factory));
new Sfactory(this,"chunk_4",new SCreator(chunk_4_factory));
new Sfactory(this,"namelist",new SCreator(namelist_factory));
new Sfactory(this,"Retval",new SCreator(Retval_factory));
new Sfactory(this,"Do_1",new SCreator(Do_1_factory));
new Sfactory(this,"functioncall_1",new SCreator(functioncall_1_factory));
new Sfactory(this,"parlist",new SCreator(parlist_factory));
new Sfactory(this,"FuncDecl",new SCreator(FuncDecl_factory));
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
new Sfactory(this,"funcbody",new SCreator(funcbody_factory));
new Sfactory(this,"funcbody_3",new SCreator(funcbody_3_factory));
new Sfactory(this,"LocalFuncDecl",new SCreator(LocalFuncDecl_factory));
new Sfactory(this,"var",new SCreator(var_factory));
new Sfactory(this,"TableRef_1",new SCreator(TableRef_1_factory));
new Sfactory(this,"binop_14",new SCreator(binop_14_factory));
new Sfactory(this,"SElseIf_1",new SCreator(SElseIf_1_factory));
new Sfactory(this,"LocalInit_1",new SCreator(LocalInit_1_factory));
new Sfactory(this,"tableconstructor",new SCreator(tableconstructor_factory));
new Sfactory(this,"tableconstructor_1",new SCreator(tableconstructor_1_factory));
new Sfactory(this,"PackageRef",new SCreator(PackageRef_factory));
new Sfactory(this,"function",new SCreator(function_factory));
new Sfactory(this,"SIf_1",new SCreator(SIf_1_factory));
new Sfactory(this,"namelist_1",new SCreator(namelist_1_factory));
new Sfactory(this,"funcname_2",new SCreator(funcname_2_factory));
new Sfactory(this,"stat",new SCreator(stat_factory));
new Sfactory(this,"unop_1",new SCreator(unop_1_factory));
new Sfactory(this,"exp",new SCreator(exp_factory));
new Sfactory(this,"fieldlist",new SCreator(fieldlist_factory));
new Sfactory(this,"arg_2",new SCreator(arg_2_factory));
new Sfactory(this,"arg_1",new SCreator(arg_1_factory));
new Sfactory(this,"arg_4",new SCreator(arg_4_factory));
new Sfactory(this,"binop_15",new SCreator(binop_15_factory));
new Sfactory(this,"unop",new SCreator(unop_factory));
new Sfactory(this,"FunctionCall",new SCreator(FunctionCall_factory));
new Sfactory(this,"binop_8",new SCreator(binop_8_factory));
new Sfactory(this,"exp_10",new SCreator(exp_10_factory));
new Sfactory(this,"Assignment",new SCreator(Assignment_factory));
new Sfactory(this,"tableconstructor_2",new SCreator(tableconstructor_2_factory));
new Sfactory(this,"arg_3",new SCreator(arg_3_factory));
new Sfactory(this,"If",new SCreator(If_factory));
new Sfactory(this,"Else",new SCreator(Else_factory));
new Sfactory(this,"explist_1",new SCreator(explist_1_factory));
new Sfactory(this,"explist_2",new SCreator(explist_2_factory));
new Sfactory(this,"SElse_1",new SCreator(SElse_1_factory));
new Sfactory(this,"binop_3",new SCreator(binop_3_factory));
new Sfactory(this,"If_1",new SCreator(If_1_factory));
new Sfactory(this,"block_2",new SCreator(block_2_factory));
new Sfactory(this,"funcname_3",new SCreator(funcname_3_factory));
new Sfactory(this,"chunk",new SCreator(chunk_factory));
new Sfactory(this,"fieldlist_3",new SCreator(fieldlist_3_factory));
new Sfactory(this,"block_1",new SCreator(block_1_factory));
new Sfactory(this,"binop",new SCreator(binop_factory));
new Sfactory(this,"Retval_1",new SCreator(Retval_1_factory));
new Sfactory(this,"PackageRef_1",new SCreator(PackageRef_1_factory));
new Sfactory(this,"unop_2",new SCreator(unop_2_factory));
new Sfactory(this,"For_1",new SCreator(For_1_factory));
new Sfactory(this,"For_3",new SCreator(For_3_factory));
new Sfactory(this,"fieldlist_2",new SCreator(fieldlist_2_factory));
new Sfactory(this,"While",new SCreator(While_factory));
new Sfactory(this,"var_1",new SCreator(var_1_factory));
new Sfactory(this,"ElseIf_1",new SCreator(ElseIf_1_factory));
new Sfactory(this,"stat_1",new SCreator(stat_1_factory));
new Sfactory(this,"While_1",new SCreator(While_1_factory));
new Sfactory(this,"varlist_1",new SCreator(varlist_1_factory));
new Sfactory(this,"SIf",new SCreator(SIf_factory));
new Sfactory(this,"chunk_1",new SCreator(chunk_1_factory));
new Sfactory(this,"Assignment_1",new SCreator(Assignment_1_factory));
new Sfactory(this,"elseif",new SCreator(elseif_factory));
new Sfactory(this,"functioncall",new SCreator(functioncall_factory));
new Sfactory(this,"varlist",new SCreator(varlist_factory));
new Sfactory(this,"binop_9",new SCreator(binop_9_factory));
new Sfactory(this,"namelist_2",new SCreator(namelist_2_factory));
new Sfactory(this,"chunk_3",new SCreator(chunk_3_factory));
new Sfactory(this,"FieldAssign_1",new SCreator(FieldAssign_1_factory));
new Sfactory(this,"FunctionCall_1",new SCreator(FunctionCall_1_factory));
new Sfactory(this,"init",new SCreator(init_factory));
new Sfactory(this,"For_2",new SCreator(For_2_factory));
new Sfactory(this,"binop_7",new SCreator(binop_7_factory));
new Sfactory(this,"funcbody_4",new SCreator(funcbody_4_factory));
new Sfactory(this,"For",new SCreator(For_factory));
new Sfactory(this,"binop_10",new SCreator(binop_10_factory));
new Sfactory(this,"unop_3",new SCreator(unop_3_factory));
new Sfactory(this,"binop_5",new SCreator(binop_5_factory));
new Sfactory(this,"block",new SCreator(block_factory));
new Sfactory(this,"Repeat",new SCreator(Repeat_factory));
new Sfactory(this,"fieldsep_1",new SCreator(fieldsep_1_factory));
new Sfactory(this,"fieldsep",new SCreator(fieldsep_factory));
new Sfactory(this,"funcbody_1",new SCreator(funcbody_1_factory));
new Sfactory(this,"prefixexp_1",new SCreator(prefixexp_1_factory));
new Sfactory(this,"binop_2",new SCreator(binop_2_factory));
new Sfactory(this,"exp_7",new SCreator(exp_7_factory));
new Sfactory(this,"exp_6",new SCreator(exp_6_factory));
new Sfactory(this,"exp_5",new SCreator(exp_5_factory));
new Sfactory(this,"exp_4",new SCreator(exp_4_factory));
new Sfactory(this,"exp_3",new SCreator(exp_3_factory));
new Sfactory(this,"exp_2",new SCreator(exp_2_factory));
new Sfactory(this,"exp_1",new SCreator(exp_1_factory));
}
public static object error_factory(Parser yyp) { return new error(yyp); }
public static object exp_9_factory(Parser yyp) { return new exp_9(yyp); }
public static object exp_8_factory(Parser yyp) { return new exp_8(yyp); }
public static object FieldAssign_factory(Parser yyp) { return new FieldAssign(yyp); }
public static object functioncall_2_factory(Parser yyp) { return new functioncall_2(yyp); }
public static object binop_4_factory(Parser yyp) { return new binop_4(yyp); }
public static object ElseIf_factory(Parser yyp) { return new ElseIf(yyp); }
public static object binop_11_factory(Parser yyp) { return new binop_11(yyp); }
public static object LocalFuncDecl_1_factory(Parser yyp) { return new LocalFuncDecl_1(yyp); }
public static object prefixexp_factory(Parser yyp) { return new prefixexp(yyp); }
public static object LocalNamelist_1_factory(Parser yyp) { return new LocalNamelist_1(yyp); }
public static object prefixexp_2_factory(Parser yyp) { return new prefixexp_2(yyp); }
public static object LocalInit_factory(Parser yyp) { return new LocalInit(yyp); }
public static object FieldExpAssign_factory(Parser yyp) { return new FieldExpAssign(yyp); }
public static object FuncDecl_1_factory(Parser yyp) { return new FuncDecl_1(yyp); }
public static object stat_2_factory(Parser yyp) { return new stat_2(yyp); }
public static object Do_factory(Parser yyp) { return new Do(yyp); }
public static object funcname_1_factory(Parser yyp) { return new funcname_1(yyp); }
public static object explist_factory(Parser yyp) { return new explist(yyp); }
public static object parlist_2_factory(Parser yyp) { return new parlist_2(yyp); }
public static object parlist_3_factory(Parser yyp) { return new parlist_3(yyp); }
public static object parlist_1_factory(Parser yyp) { return new parlist_1(yyp); }
public static object FieldExpAssign_1_factory(Parser yyp) { return new FieldExpAssign_1(yyp); }
public static object Else_1_factory(Parser yyp) { return new Else_1(yyp); }
public static object LocalNamelist_factory(Parser yyp) { return new LocalNamelist(yyp); }
public static object Repeat_1_factory(Parser yyp) { return new Repeat_1(yyp); }
public static object binop_12_factory(Parser yyp) { return new binop_12(yyp); }
public static object fieldsep_2_factory(Parser yyp) { return new fieldsep_2(yyp); }
public static object TableRef_factory(Parser yyp) { return new TableRef(yyp); }
public static object function_1_factory(Parser yyp) { return new function_1(yyp); }
public static object funcbody_2_factory(Parser yyp) { return new funcbody_2(yyp); }
public static object chunk_4_factory(Parser yyp) { return new chunk_4(yyp); }
public static object namelist_factory(Parser yyp) { return new namelist(yyp); }
public static object Retval_factory(Parser yyp) { return new Retval(yyp); }
public static object Do_1_factory(Parser yyp) { return new Do_1(yyp); }
public static object functioncall_1_factory(Parser yyp) { return new functioncall_1(yyp); }
public static object parlist_factory(Parser yyp) { return new parlist(yyp); }
public static object FuncDecl_factory(Parser yyp) { return new FuncDecl(yyp); }
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
public static object funcbody_factory(Parser yyp) { return new funcbody(yyp); }
public static object funcbody_3_factory(Parser yyp) { return new funcbody_3(yyp); }
public static object LocalFuncDecl_factory(Parser yyp) { return new LocalFuncDecl(yyp); }
public static object var_factory(Parser yyp) { return new var(yyp); }
public static object TableRef_1_factory(Parser yyp) { return new TableRef_1(yyp); }
public static object binop_14_factory(Parser yyp) { return new binop_14(yyp); }
public static object SElseIf_1_factory(Parser yyp) { return new SElseIf_1(yyp); }
public static object LocalInit_1_factory(Parser yyp) { return new LocalInit_1(yyp); }
public static object tableconstructor_factory(Parser yyp) { return new tableconstructor(yyp); }
public static object tableconstructor_1_factory(Parser yyp) { return new tableconstructor_1(yyp); }
public static object PackageRef_factory(Parser yyp) { return new PackageRef(yyp); }
public static object function_factory(Parser yyp) { return new function(yyp); }
public static object SIf_1_factory(Parser yyp) { return new SIf_1(yyp); }
public static object namelist_1_factory(Parser yyp) { return new namelist_1(yyp); }
public static object funcname_2_factory(Parser yyp) { return new funcname_2(yyp); }
public static object stat_factory(Parser yyp) { return new stat(yyp); }
public static object unop_1_factory(Parser yyp) { return new unop_1(yyp); }
public static object exp_factory(Parser yyp) { return new exp(yyp); }
public static object fieldlist_factory(Parser yyp) { return new fieldlist(yyp); }
public static object arg_2_factory(Parser yyp) { return new arg_2(yyp); }
public static object arg_1_factory(Parser yyp) { return new arg_1(yyp); }
public static object arg_4_factory(Parser yyp) { return new arg_4(yyp); }
public static object binop_15_factory(Parser yyp) { return new binop_15(yyp); }
public static object unop_factory(Parser yyp) { return new unop(yyp); }
public static object FunctionCall_factory(Parser yyp) { return new FunctionCall(yyp); }
public static object binop_8_factory(Parser yyp) { return new binop_8(yyp); }
public static object exp_10_factory(Parser yyp) { return new exp_10(yyp); }
public static object Assignment_factory(Parser yyp) { return new Assignment(yyp); }
public static object tableconstructor_2_factory(Parser yyp) { return new tableconstructor_2(yyp); }
public static object arg_3_factory(Parser yyp) { return new arg_3(yyp); }
public static object If_factory(Parser yyp) { return new If(yyp); }
public static object Else_factory(Parser yyp) { return new Else(yyp); }
public static object explist_1_factory(Parser yyp) { return new explist_1(yyp); }
public static object explist_2_factory(Parser yyp) { return new explist_2(yyp); }
public static object SElse_1_factory(Parser yyp) { return new SElse_1(yyp); }
public static object binop_3_factory(Parser yyp) { return new binop_3(yyp); }
public static object If_1_factory(Parser yyp) { return new If_1(yyp); }
public static object block_2_factory(Parser yyp) { return new block_2(yyp); }
public static object funcname_3_factory(Parser yyp) { return new funcname_3(yyp); }
public static object chunk_factory(Parser yyp) { return new chunk(yyp); }
public static object fieldlist_3_factory(Parser yyp) { return new fieldlist_3(yyp); }
public static object block_1_factory(Parser yyp) { return new block_1(yyp); }
public static object binop_factory(Parser yyp) { return new binop(yyp); }
public static object Retval_1_factory(Parser yyp) { return new Retval_1(yyp); }
public static object PackageRef_1_factory(Parser yyp) { return new PackageRef_1(yyp); }
public static object unop_2_factory(Parser yyp) { return new unop_2(yyp); }
public static object For_1_factory(Parser yyp) { return new For_1(yyp); }
public static object For_3_factory(Parser yyp) { return new For_3(yyp); }
public static object fieldlist_2_factory(Parser yyp) { return new fieldlist_2(yyp); }
public static object While_factory(Parser yyp) { return new While(yyp); }
public static object var_1_factory(Parser yyp) { return new var_1(yyp); }
public static object ElseIf_1_factory(Parser yyp) { return new ElseIf_1(yyp); }
public static object stat_1_factory(Parser yyp) { return new stat_1(yyp); }
public static object While_1_factory(Parser yyp) { return new While_1(yyp); }
public static object varlist_1_factory(Parser yyp) { return new varlist_1(yyp); }
public static object SIf_factory(Parser yyp) { return new SIf(yyp); }
public static object chunk_1_factory(Parser yyp) { return new chunk_1(yyp); }
public static object Assignment_1_factory(Parser yyp) { return new Assignment_1(yyp); }
public static object elseif_factory(Parser yyp) { return new elseif(yyp); }
public static object functioncall_factory(Parser yyp) { return new functioncall(yyp); }
public static object varlist_factory(Parser yyp) { return new varlist(yyp); }
public static object binop_9_factory(Parser yyp) { return new binop_9(yyp); }
public static object namelist_2_factory(Parser yyp) { return new namelist_2(yyp); }
public static object chunk_3_factory(Parser yyp) { return new chunk_3(yyp); }
public static object FieldAssign_1_factory(Parser yyp) { return new FieldAssign_1(yyp); }
public static object FunctionCall_1_factory(Parser yyp) { return new FunctionCall_1(yyp); }
public static object init_factory(Parser yyp) { return new init(yyp); }
public static object For_2_factory(Parser yyp) { return new For_2(yyp); }
public static object binop_7_factory(Parser yyp) { return new binop_7(yyp); }
public static object funcbody_4_factory(Parser yyp) { return new funcbody_4(yyp); }
public static object For_factory(Parser yyp) { return new For(yyp); }
public static object binop_10_factory(Parser yyp) { return new binop_10(yyp); }
public static object unop_3_factory(Parser yyp) { return new unop_3(yyp); }
public static object binop_5_factory(Parser yyp) { return new binop_5(yyp); }
public static object block_factory(Parser yyp) { return new block(yyp); }
public static object Repeat_factory(Parser yyp) { return new Repeat(yyp); }
public static object fieldsep_1_factory(Parser yyp) { return new fieldsep_1(yyp); }
public static object fieldsep_factory(Parser yyp) { return new fieldsep(yyp); }
public static object funcbody_1_factory(Parser yyp) { return new funcbody_1(yyp); }
public static object prefixexp_1_factory(Parser yyp) { return new prefixexp_1(yyp); }
public static object binop_2_factory(Parser yyp) { return new binop_2(yyp); }
public static object exp_7_factory(Parser yyp) { return new exp_7(yyp); }
public static object exp_6_factory(Parser yyp) { return new exp_6(yyp); }
public static object exp_5_factory(Parser yyp) { return new exp_5(yyp); }
public static object exp_4_factory(Parser yyp) { return new exp_4(yyp); }
public static object exp_3_factory(Parser yyp) { return new exp_3(yyp); }
public static object exp_2_factory(Parser yyp) { return new exp_2(yyp); }
public static object exp_1_factory(Parser yyp) { return new exp_1(yyp); }
}
public class syntax: Parser {
public syntax():base(new yysyntax(),new tokens()) {}
public syntax(YyParser syms):base(syms,new tokens()) {}
public syntax(YyParser syms,ErrorHandler erh):base(syms,new tokens(erh)) {}

 }
