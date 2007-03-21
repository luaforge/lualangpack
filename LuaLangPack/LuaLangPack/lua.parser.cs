using System;using Tools;
namespace LuaLangImpl {
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
 nested . beginLine = begline ;
 nested . beginIndx = begchar ;
 nested . endLine = endline ;
 nested . endIndx = endchar ;
 nested . outline = outline ;
 c . FillScope ( nested );
 scope . nested . AddLast ( nested );
}
}
 public  void  FillScope ( LuaScope  scope , LuaFunction  f , int  begline , int  begchar , int  endline , int  endchar , bool  outline ){ if ( c != null ){ LuaScope  nested = new  LuaScope ( scope );
 f . Scope = nested ;
 nested . beginLine = begline ;
 nested . beginIndx = begchar ;
 nested . endLine = endline ;
 nested . endIndx = endchar ;
 nested . outline = outline ;
 c . FillScope ( nested , f );
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
 public  override  void  FillScope ( LuaScope  s , LuaTable  t ){ t . SetEnclosingScope ( s );
 e . FillScope (( LuaScope ) t , n );
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
 public  void  FillScope ( LuaScope  s ){ if ( f != null ){ s . FileScope . DeclareRegion ( open . Line -1, open . Position , close . Line -1, close . Position -1);
 f . FillScope ( s );
}
}
 public  void  FillScope ( LuaScope  s , var  v ){ LuaTable  table = new  LuaTable ();
 table . line = close . Line -1;
 table . pos = close . Position ;
 table . file = LuaScope . filename ;
 if ( f != null ){ s . FileScope . DeclareRegion ( open . Line -1, open . Position , close . Line -1, close . Position -1);
 f . FillScope ( s , table );
}
 v . Assign ( s , table );
}
 public  void  FillScope ( LuaScope  s , NAME  n ){ LuaTable  table = new  LuaTable ();
 table . name = n . s ;
 table . line = close . Line -1;
 table . pos = close . Position ;
 table . file = LuaScope . filename ;
 if ( f != null ){ s . FileScope . DeclareRegion ( open . Line -1, open . Position , close . Line -1, close . Position -1);
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
 name . file = LuaScope . filename ;
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
 ILuaName  fun = s . Lookup ( fname . name , Line -1, Position );
 if ( fun == null || fun . type != LuaType . Function ||(( LuaFunction ) fun ). RetStats . Count !=1) return ;
 else { LuaFunction  func =( LuaFunction ) fun ;
 s . RValueScope = func . Scope ;
 func . RetStats . First . Value . FillScope ( s , vl );
 s . RValueScope = null ;
}
}
 public  void  FillScope ( LuaScope  s , namelist  nl ){ ILuaName  fname = p . Resolve ( s );
 if ( fname == null ) return ;
 ILuaName  fun = s . Lookup ( fname . name , Line -1, Position );
 if ( fun == null || fun . type != LuaType . Function ||(( LuaFunction ) fun ). RetStats . Count !=1) return ;
 else { LuaFunction  func =( LuaFunction ) fun ;
 s . RValueScope = func . Scope ;
 func . RetStats . First . Value . FillScope ( s , nl );
 s . RValueScope = null ;
}
}
 public  void  FillScope ( LuaScope  s , NAME  n_left ){ ILuaName  fname = p . Resolve ( s );
 if ( fname == null ) return ;
 ILuaName  fun = s . Lookup ( fname . name , Line -1, Position );
 if ( fun == null || fun . type != LuaType . Function ||(( LuaFunction ) fun ). RetStats . Count !=1) return ;
 else { LuaFunction  func =( LuaFunction ) fun ;
 s . RValueScope = func . Scope ;
 func . RetStats . First . Value . FillScope ( s , n_left );
 s . RValueScope = null ;
}
}
 public  void  FillScope ( LuaScope  s , var  v_left ){ ILuaName  fname = p . Resolve ( s );
 if ( fname == null ) return ;
 ILuaName  fun = s . Lookup ( fname . name , Line -1, Position );
 if ( fun == null || fun . type != LuaType . Function ||(( LuaFunction ) fun ). RetStats . Count !=1) return ;
 else { LuaFunction  func =( LuaFunction ) fun ;
 s . RValueScope = func . Scope ;
 func . RetStats . First . Value . FillScope ( s , v_left );
 s . RValueScope = null ;
}
}
 public  ILuaName  Resolve ( LuaScope  s ){ ILuaName  fname = p . Resolve ( s );
 if ( fname == null ) return  null ;
 ILuaName  fun = s . Lookup ( fname . name , Line -1, Position );
 if ( fun == null || fun . type != LuaType . Function ||(( LuaFunction ) fun ). RetStats . Count !=1) return  null ;
 else { LuaFunction  func =( LuaFunction ) fun ;
 return  func . RetStats . First . Value . Resolve ( func . Scope );
}
}

public override string yyname { get { return "functioncall"; }}
public override int yynum { get { return 62; }}
public functioncall(Parser yyp):base(yyp){}}
//%+funcname+63
public class funcname : SYMBOL{
 private  NAME  n1 ;
 private  NAME  n2 ;
 private  funcname  fn ;
 public  funcname (Parser yyp, NAME  a ):base(((syntax)yyp)){ n1 = a ;
}
 public  funcname (Parser yyp, NAME  a , NAME  b ):base(((syntax)yyp)){ n1 = a ;
 n2 = b ;
}
 public  funcname (Parser yyp, NAME  a , funcname  b ):base(((syntax)yyp)){ n2 = a ;
 fn = b ;
}
 public  LuaFunction  FillScope ( LuaScope  s ){ if ( n1 != null && n2 == null ){ LuaFunction  f = new  LuaFunction ();
 f . name = n1 . s ;
 f . file = LuaScope . filename ;
 if ( s . GlobalScope (). ShallowLookupFunction ( n1 . s )== null ) s . GlobalScope (). Add ( f );
 return  f ;
}
 else  if ( fn != null && n2 != null ){ ILuaName  t = s . Lookup ( n2 . s , n2 . Line , n2 . Position );
 if ( t != null && t . type == LuaType . Table ) return  fn . FillScope (( LuaNamespace ) t );
 else  return  null ;
}
 else  if ( n1 != null && n2 != null ){ ILuaName  t = s . Lookup ( n1 . s , n1 . Line , n1 . Position );
 if ( t == null || t . type != LuaType . Table ){ t = new  LuaTable ( n1 . s , n1 . Line , n1 . Position );
 t . file = LuaScope . filename ;
 s . Add (( LuaTable ) t );
}
 LuaFunction  f = new  LuaFunction ();
 f . name = n2 . s ;
 f . file = LuaScope . filename ;
(( LuaTable ) t ). Add ( f );
 return  f ;
}
 else  return  null ;
}
 public  LuaFunction  FillScope ( LuaNamespace  s ){ if ( n1 != null && n2 == null ){ LuaFunction  f = new  LuaFunction ();
 f . name = n1 . s ;
 f . file = LuaScope . filename ;
 s . Add ( f );
 return  f ;
}
 else  if ( fn != null && n2 != null ){ ILuaName  t = s . Lookup ( n2 . s , n2 . Line , n2 . Position );
 if ( t != null && t . type == LuaType . Table ) return  fn . FillScope (( LuaNamespace ) t );
 else  return  null ;
}
 else  if ( n1 != null && n2 != null ){ ILuaName  t = s . Lookup ( n1 . s , n1 . Line , n1 . Position );
 if ( t == null || t . type != LuaType . Table ){ t = new  LuaTable ( n1 . s , n1 . Line , n1 . Position );
 t . file = LuaScope . filename ;
 s . Add (( LuaTable ) t );
}
 LuaFunction  f = new  LuaFunction ();
 f . name = n2 . s ;
 f . file = LuaScope . filename ;
(( LuaTable ) t ). Add ( f );
 return  f ;
}
 else  return  null ;
}

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
 public  void  FillScope ( LuaScope  s , var  v_left ){ LuaFunction  fun = new  LuaFunction ();
 fun . name ="?anon?";
 fun . file = LuaScope . filename ;
 f . FillScope ( s , fun );
 v_left . Assign ( s , fun );
}
 public  void  FillScope ( LuaScope  s , NAME  n_left ){ LuaFunction  fun = new  LuaFunction ();
 fun . name = n_left . s ;
 fun . line = n_left . Line -1;
 fun . pos = n_left . Position ;
 fun . file = LuaScope . filename ;
 f . FillScope ( s , fun );
 s . Add ( fun );
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
 else  if ( btrue || bfalse || number || nil || l != null ){ LuaName  rvalue = new  LuaName ();
 rvalue . name ="";
 rvalue . line = Line -1;
 rvalue . pos = Position ;
 rvalue . file = LuaScope . filename ;
 v . Assign ( s , rvalue );
}
}
 public  void  FillScope ( LuaScope  s , NAME  n ){ if ( f != null ){ f . FillScope ( s , n );
}
 else  if ( p != null ){ p . FillScope ( s , n );
}
 else  if ( t != null ){ t . FillScope ( s , n );
}
 else  if ( btrue || bfalse || number || nil || l != null ){ LuaName  name = new  LuaName ();
 name . name = n . s ;
 name . pos = n . Position ;
 name . line = n . Line -1;
 name . file = LuaScope . filename ;
 s . Add ( name );
}
}
 public  void  FillScope ( LuaScope  s , varlist  v ){ if ( f != null ){ f . FillScope ( s , v . v );
}
 else  if ( p != null ){ p . FillScope ( s , v );
}
 else  if ( t != null ){ t . FillScope ( s , v . v );
}
 else  if ( btrue || bfalse || number || nil || l != null ){ LuaName  rvalue = new  LuaName ();
 rvalue . name ="";
 rvalue . line = Line -1;
 rvalue . pos = Position ;
 rvalue . file = LuaScope . filename ;
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
 else  if ( btrue || bfalse || number || nil || l != null ){ LuaName  name = new  LuaName ();
 name . name = n . n . s ;
 name . pos = n . n . Position ;
 name . line = n . n . Line -1;
 name . file = LuaScope . filename ;
 s . Add ( name );
}
 if ( n . nl != null && p == null ) n . nl . FillScope ( s );
}
 public  ILuaName  Resolve ( LuaScope  s ){ if ( l != null ){ LuaName  name = new  LuaName ();
 name . name = l . s_nq ;
 name . pos = l . Position ;
 name . line = l . Line -1;
 name . file = LuaScope . filename ;
 return  name ;
}
 else  if ( p != null ){ return  p . Resolve ( s );
}
 return  null ;
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
 name . file = LuaScope . filename ;
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
 name . file = LuaScope . filename ;
 s . Add ( name );
}
 else  if ( rvalue . type == LuaType . Table ){ LuaTable  table = new  LuaTable (( LuaTable ) rvalue );
 table . line = n_left . line ;
 table . pos = n_left . pos ;
 table . name = n_left . name ;
 table . file = LuaScope . filename ;
 s . Add ( table );
}
 else  if ( rvalue . type == LuaType . Function ){ LuaFunction  fun = new  LuaFunction (( LuaFunction ) rvalue );
 fun . line = n_left . line ;
 fun . pos = n_left . pos ;
 fun . name = n_left . name ;
 fun . file = LuaScope . filename ;
 s . Add ( fun );
}
}
 protected  void  BaseAssign ( LuaNamespace  s , ILuaName  rvalue , NAME  n_left ){ NAME  lname = n_left ;
 if ( rvalue == null || rvalue . type == LuaType . Name ){ LuaName  name = new  LuaName (( LuaName ) rvalue );
 name . line = lname . Line -1;
 name . pos = lname . Position ;
 name . name = lname . s ;
 name . file = LuaScope . filename ;
 s . Add ( name );
}
 else  if ( rvalue . type == LuaType . Table ){ LuaTable  table = new  LuaTable (( LuaTable ) rvalue );
 table . line = lname . Line -1;
 table . pos = lname . Position ;
 table . name = lname . s ;
 table . file = LuaScope . filename ;
 s . Add ( table );
}
 else  if ( rvalue . type == LuaType . Function ){ LuaFunction  fun = new  LuaFunction (( LuaFunction ) rvalue );
 fun . line = lname . Line -1;
 fun . pos = lname . Position ;
 fun . name = lname . s ;
 fun . file = LuaScope . filename ;
 s . Add ( fun );
}
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
 if ( left == null || left . type != LuaType . Table || l == null ) return ;
 BaseAssign (( LuaTable ) left , rvalue , l );
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
 public  override  void  FillScope ( LuaScope  s ){ LuaFunction  f = fname . FillScope ( s );
 if ( f != null ){ f . line = body . Line -1;
 f . pos = body . Position ;
 body . FillScope ( s , f );
}
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
 f . file = LuaScope . filename ;
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
 public  override  void  FillScope ( LuaScope  s ){ b . FillScope ( s );
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
 public  override  void  FillScope ( LuaScope  s ){ b . FillScope ( s );
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
 public  override  void  FillScope ( LuaScope  s ){ b1 . FillScope ( s );
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
 public  override  void  FillScope ( LuaScope  s ){ b . FillScope ( s );
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
 public  override  void  FillScope ( LuaScope  s ){ b . FillScope ( s );
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
 public  override  void  FillScope ( LuaScope  s ){ b1 . FillScope ( s );
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
	((NAME)(yyq.StackAt(2).m_value))
	, 
	((funcname)(yyq.StackAt(0).m_value))
	 ){}}

public class funcname_2 : funcname {
  public funcname_2(Parser yyq):base(yyq, 
	((NAME)(yyq.StackAt(2).m_value))
	, 
	((NAME)(yyq.StackAt(0).m_value))
	 ){}}

public class funcname_3 : funcname {
  public funcname_3(Parser yyq):base(yyq, 
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

public class fieldsep_1 : fieldsep {
  public fieldsep_1(Parser yyq):base(yyq){}}

public class fieldsep_2 : fieldsep {
  public fieldsep_2(Parser yyq):base(yyq){}}

public class exp_9 : exp {
  public exp_9(Parser yyq):base(yyq){}}

public class exp_10 : exp {
  public exp_10(Parser yyq):base(yyq){}}

public class exp_11 : exp {
  public exp_11(Parser yyq):base(yyq){}}

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
1,1113,102,2,0,
105,5,179,1,519,
106,18,1,519,107,
20,108,4,10,67,
0,79,0,77,0,
77,0,65,0,1,
7,1,1,2,0,
1,1029,109,18,1,
1029,110,20,111,4,
6,69,0,78,0,
68,0,1,38,1,
1,2,0,1,1028,
112,18,1,1028,113,
20,114,4,10,98,
0,108,0,111,0,
99,0,107,0,1,
55,1,2,2,0,
1,502,115,18,1,
502,116,20,117,4,
6,101,0,120,0,
112,0,1,68,1,
2,2,0,1,1020,
118,18,1,1020,119,
20,120,4,4,68,
0,79,0,1,41,
1,1,2,0,1,
1019,121,18,1,1019,
122,20,123,4,14,
101,0,120,0,112,
0,108,0,105,0,
115,0,116,0,1,
69,1,2,2,0,
1,490,124,18,1,
490,107,2,0,1,
998,125,18,1,998,
126,20,127,4,4,
73,0,78,0,1,
42,1,1,2,0,
1,997,128,18,1,
997,129,20,130,4,
16,110,0,97,0,
109,0,101,0,108,
0,105,0,115,0,
116,0,1,61,1,
2,2,0,1,473,
131,18,1,473,116,
2,0,1,450,132,
18,1,450,133,20,
134,4,10,67,0,
79,0,76,0,79,
0,78,0,1,8,
1,1,2,0,1,
449,135,18,1,449,
136,20,137,4,8,
78,0,65,0,77,
0,69,0,1,3,
1,1,2,0,1,
461,138,18,1,461,
139,20,140,4,12,
65,0,83,0,83,
0,73,0,71,0,
78,0,1,33,1,
1,2,0,1,459,
141,18,1,459,136,
2,0,1,458,142,
18,1,458,143,20,
144,4,6,70,0,
79,0,82,0,1,
40,1,1,2,0,
1,457,145,18,1,
457,146,20,147,4,
16,102,0,117,0,
110,0,99,0,98,
0,111,0,100,0,
121,0,1,65,1,
2,2,0,1,455,
148,18,1,455,149,
20,150,4,16,102,
0,117,0,110,0,
99,0,110,0,97,
0,109,0,101,0,
1,63,1,2,2,
0,1,454,151,18,
1,454,149,2,0,
1,452,152,18,1,
452,153,20,154,4,
6,68,0,79,0,
84,0,1,16,1,
1,2,0,1,451,
155,18,1,451,136,
2,0,1,971,156,
18,1,971,110,2,
0,1,970,157,18,
1,970,113,2,0,
1,448,158,18,1,
448,159,20,160,4,
16,70,0,85,0,
78,0,67,0,84,
0,73,0,79,0,
78,0,1,45,1,
1,2,0,1,447,
161,18,1,447,146,
2,0,1,445,162,
18,1,445,136,2,
0,1,444,163,18,
1,444,159,2,0,
1,443,164,18,1,
443,165,20,166,4,
8,105,0,110,0,
105,0,116,0,1,
74,1,2,2,0,
1,441,167,18,1,
441,122,2,0,1,
961,168,18,1,961,
119,2,0,1,432,
169,18,1,432,122,
2,0,1,412,170,
18,1,412,107,2,
0,1,931,171,18,
1,931,110,2,0,
1,930,172,18,1,
930,110,2,0,1,
929,173,18,1,929,
174,20,175,4,12,
101,0,108,0,115,
0,101,0,105,0,
102,0,1,90,1,
2,2,0,1,395,
176,18,1,395,116,
2,0,1,908,177,
18,1,908,174,2,
0,1,383,178,18,
1,383,139,2,0,
1,382,179,18,1,
382,129,2,0,1,
380,180,18,1,380,
129,2,0,1,379,
181,18,1,379,107,
2,0,1,378,182,
18,1,378,136,2,
0,1,377,183,18,
1,377,184,20,185,
4,10,76,0,79,
0,67,0,65,0,
76,0,1,49,1,
1,2,0,1,372,
186,18,1,372,187,
20,188,4,14,118,
0,97,0,114,0,
108,0,105,0,115,
0,116,0,1,73,
1,2,2,0,1,
887,189,18,1,887,
190,20,191,4,12,
69,0,76,0,83,
0,69,0,73,0,
70,0,1,37,1,
1,2,0,1,874,
192,18,1,874,113,
2,0,1,351,193,
18,1,351,194,20,
195,4,12,82,0,
66,0,82,0,65,
0,67,0,75,0,
1,13,1,1,2,
0,1,868,196,18,
1,868,197,20,198,
4,8,69,0,76,
0,83,0,69,0,
1,36,1,1,2,
0,1,867,199,18,
1,867,113,2,0,
1,861,200,18,1,
861,201,20,202,4,
8,84,0,72,0,
69,0,78,0,1,
35,1,1,2,0,
1,335,203,18,1,
335,116,2,0,1,
333,204,18,1,333,
205,20,206,4,6,
97,0,114,0,103,
0,1,94,1,2,
2,0,1,332,207,
18,1,332,208,20,
209,4,18,102,0,
105,0,101,0,108,
0,100,0,108,0,
105,0,115,0,116,
0,1,59,1,2,
2,0,1,844,210,
18,1,844,116,2,
0,1,832,211,18,
1,832,190,2,0,
1,309,212,18,1,
309,213,20,214,4,
16,102,0,105,0,
101,0,108,0,100,
0,115,0,101,0,
112,0,1,125,1,
2,2,0,1,308,
215,18,1,308,107,
2,0,1,307,216,
18,1,307,217,20,
218,4,18,83,0,
69,0,77,0,73,
0,67,0,79,0,
76,0,79,0,78,
0,1,9,1,1,
2,0,1,306,219,
18,1,306,220,20,
221,4,10,102,0,
105,0,101,0,108,
0,100,0,1,56,
1,2,2,0,1,
305,222,18,1,305,
223,20,224,4,12,
82,0,66,0,82,
0,65,0,67,0,
69,0,1,15,1,
1,2,0,1,303,
225,18,1,303,223,
2,0,1,302,226,
18,1,302,208,2,
0,1,811,227,18,
1,811,102,2,0,
1,808,228,18,1,
808,102,2,0,1,
792,229,18,1,792,
217,2,0,1,776,
230,18,1,776,231,
20,232,4,8,115,
0,116,0,97,0,
116,0,1,75,1,
2,2,0,1,775,
233,18,1,775,102,
2,0,1,774,234,
18,1,774,122,2,
0,1,753,235,18,
1,753,139,2,0,
1,752,236,18,1,
752,187,2,0,1,
751,237,18,1,751,
110,2,0,1,750,
238,18,1,750,113,
2,0,1,748,239,
18,1,748,110,2,
0,1,213,240,18,
1,213,139,2,0,
1,212,241,18,1,
212,194,2,0,1,
211,242,18,1,211,
116,2,0,1,214,
243,18,1,214,116,
2,0,1,734,244,
18,1,734,119,2,
0,1,733,245,18,
1,733,110,2,0,
1,732,246,18,1,
732,113,2,0,1,
210,247,18,1,210,
248,20,249,4,12,
76,0,66,0,82,
0,65,0,67,0,
75,0,1,12,1,
1,2,0,1,730,
250,18,1,730,110,
2,0,1,717,251,
18,1,717,119,2,
0,1,700,252,18,
1,700,116,2,0,
1,174,253,18,1,
174,116,2,0,1,
173,254,18,1,173,
139,2,0,1,172,
255,18,1,172,136,
2,0,1,688,256,
18,1,688,257,20,
258,4,10,87,0,
72,0,73,0,76,
0,69,0,1,39,
1,1,2,0,1,
650,259,18,1,650,
260,20,261,4,10,
85,0,78,0,84,
0,73,0,76,0,
1,47,1,1,2,
0,1,649,262,18,
1,649,113,2,0,
1,147,263,18,1,
147,116,2,0,1,
662,264,18,1,662,
116,2,0,1,133,
265,18,1,133,266,
20,267,4,6,78,
0,73,0,76,0,
1,44,1,1,2,
0,1,132,268,18,
1,132,269,20,270,
4,10,70,0,65,
0,76,0,83,0,
69,0,1,51,1,
1,2,0,1,131,
271,18,1,131,272,
20,273,4,8,84,
0,82,0,85,0,
69,0,1,50,1,
1,2,0,1,130,
274,18,1,130,275,
20,276,4,12,78,
0,85,0,77,0,
66,0,69,0,82,
0,1,5,1,1,
2,0,1,129,277,
18,1,129,278,20,
279,4,14,76,0,
73,0,84,0,69,
0,82,0,65,0,
76,0,1,4,1,
1,2,0,1,128,
280,18,1,128,281,
20,282,4,16,102,
0,117,0,110,0,
99,0,116,0,105,
0,111,0,110,0,
1,67,1,2,2,
0,1,127,283,18,
1,127,284,20,285,
4,32,116,0,97,
0,98,0,108,0,
101,0,99,0,111,
0,110,0,115,0,
116,0,114,0,117,
0,99,0,116,0,
111,0,114,0,1,
60,1,2,2,0,
1,637,286,18,1,
637,287,20,288,4,
12,82,0,69,0,
80,0,69,0,65,
0,84,0,1,46,
1,1,2,0,1,
635,289,18,1,635,
110,2,0,1,634,
290,18,1,634,113,
2,0,1,109,291,
18,1,109,116,2,
0,1,623,292,18,
1,623,197,2,0,
1,622,293,18,1,
622,113,2,0,1,
98,294,18,1,98,
295,20,296,4,8,
117,0,110,0,111,
0,112,0,1,144,
1,2,2,0,1,
97,297,18,1,97,
298,20,299,4,12,
69,0,76,0,73,
0,80,0,83,0,
69,0,1,53,1,
1,2,0,1,96,
300,18,1,96,301,
20,302,4,6,118,
0,97,0,114,0,
1,70,1,2,2,
0,1,95,303,18,
1,95,304,20,305,
4,24,102,0,117,
0,110,0,99,0,
116,0,105,0,111,
0,110,0,99,0,
97,0,108,0,108,
0,1,62,1,2,
2,0,1,92,306,
18,1,92,307,20,
308,4,12,82,0,
80,0,65,0,82,
0,69,0,78,0,
1,11,1,1,2,
0,1,611,309,18,
1,611,201,2,0,
1,582,310,18,1,
582,311,20,312,4,
4,73,0,70,0,
1,34,1,1,2,
0,1,581,313,18,
1,581,122,2,0,
1,1053,314,18,1,
1053,110,2,0,1,
1052,315,18,1,1052,
113,2,0,1,560,
316,18,1,560,317,
20,318,4,12,82,
0,69,0,84,0,
85,0,82,0,78,
0,1,48,1,1,
2,0,1,76,319,
18,1,76,116,2,
0,1,594,320,18,
1,594,116,2,0,
1,1114,321,18,1,
1114,322,23,323,4,
6,69,0,79,0,
70,0,1,2,1,
6,2,0,1,1113,
104,1,69,324,18,
1,69,325,20,326,
4,12,76,0,80,
0,65,0,82,0,
69,0,78,0,1,
10,1,1,2,0,
1,1075,327,18,1,
1075,328,20,329,4,
14,112,0,97,0,
114,0,108,0,105,
0,115,0,116,0,
1,64,1,2,2,
0,1,548,330,18,
1,548,119,2,0,
1,531,331,18,1,
531,116,2,0,1,
62,332,18,1,62,
333,20,334,4,10,
98,0,105,0,110,
0,111,0,112,0,
1,143,1,2,2,
0,1,61,335,18,
1,61,336,20,337,
4,8,80,0,76,
0,85,0,83,0,
1,17,1,1,2,
0,1,60,338,18,
1,60,339,20,340,
4,10,77,0,73,
0,78,0,85,0,
83,0,1,18,1,
1,2,0,1,59,
341,18,1,59,342,
20,343,4,8,77,
0,85,0,76,0,
84,0,1,19,1,
1,2,0,1,58,
344,18,1,58,345,
20,346,4,6,77,
0,79,0,68,0,
1,21,1,1,2,
0,1,57,347,18,
1,57,348,20,349,
4,12,68,0,73,
0,86,0,73,0,
68,0,69,0,1,
22,1,1,2,0,
1,56,350,18,1,
56,351,20,352,4,
6,69,0,88,0,
80,0,1,23,1,
1,2,0,1,55,
353,18,1,55,354,
20,355,4,12,67,
0,79,0,78,0,
67,0,65,0,84,
0,1,52,1,1,
2,0,1,54,356,
18,1,54,357,20,
358,4,4,76,0,
84,0,1,26,1,
1,2,0,1,53,
359,18,1,53,360,
20,361,4,4,71,
0,84,0,1,28,
1,1,2,0,1,
52,362,18,1,52,
363,20,364,4,4,
71,0,69,0,1,
29,1,1,2,0,
1,51,365,18,1,
51,366,20,367,4,
4,76,0,69,0,
1,27,1,1,2,
0,1,50,368,18,
1,50,369,20,370,
4,4,69,0,81,
0,1,24,1,1,
2,0,1,49,371,
18,1,49,372,20,
373,4,6,65,0,
78,0,68,0,1,
30,1,1,2,0,
1,48,374,18,1,
48,375,20,376,4,
4,79,0,82,0,
1,31,1,1,2,
0,1,47,377,18,
1,47,378,20,379,
4,6,78,0,69,
0,81,0,1,25,
1,1,2,0,1,
46,380,18,1,46,
116,2,0,1,45,
381,18,1,45,382,
20,383,4,12,76,
0,66,0,82,0,
65,0,67,0,69,
0,1,14,1,1,
2,0,1,44,384,
18,1,44,205,2,
0,1,1051,385,18,
1,1051,110,2,0,
1,1050,386,18,1,
1050,307,2,0,1,
1049,387,18,1,1049,
328,2,0,1,40,
388,18,1,40,136,
2,0,1,39,389,
18,1,39,133,2,
0,1,559,390,18,
1,559,391,20,392,
4,10,66,0,82,
0,69,0,65,0,
75,0,1,43,1,
1,2,0,1,558,
393,18,1,558,110,
2,0,1,557,394,
18,1,557,113,2,
0,1,33,395,18,
1,33,396,20,397,
4,18,112,0,114,
0,101,0,102,0,
105,0,120,0,101,
0,120,0,112,0,
1,66,1,2,2,
0,1,1073,398,18,
1,1073,107,2,0,
1,1072,399,18,1,
1072,136,2,0,1,
1071,400,18,1,1071,
298,2,0,1,28,
401,18,1,28,248,
2,0,1,27,402,
18,1,27,136,2,
0,1,26,403,18,
1,26,153,2,0,
1,22,404,18,1,
22,396,2,0,1,
21,405,18,1,21,
107,2,0,1,20,
406,18,1,20,301,
2,0,1,19,407,
18,1,19,136,2,
0,1,17,408,18,
1,17,110,2,0,
1,16,409,18,1,
16,113,2,0,1,
15,410,18,1,15,
110,2,0,1,14,
411,18,1,14,307,
2,0,1,13,412,
18,1,13,325,2,
0,1,12,413,18,
1,12,146,2,0,
1,11,414,18,1,
11,159,2,0,1,
10,415,18,1,10,
307,2,0,1,9,
416,18,1,9,307,
2,0,1,8,417,
18,1,8,122,2,
0,1,7,418,18,
1,7,339,2,0,
1,6,419,18,1,
6,420,20,421,4,
6,78,0,79,0,
84,0,1,32,1,
1,2,0,1,5,
422,18,1,5,423,
20,424,4,10,80,
0,79,0,85,0,
78,0,68,0,1,
20,1,1,2,0,
1,4,425,18,1,
4,325,2,0,1,
3,426,18,1,3,
284,2,0,1,2,
427,18,1,2,278,
2,0,1,1,428,
18,1,1,396,2,
0,1,0,429,18,
1,0,0,2,0,
430,5,0,431,5,
193,1,195,432,19,
433,4,10,97,0,
114,0,103,0,95,
0,52,0,1,195,
434,5,4,1,40,
435,16,0,384,1,
22,436,16,0,204,
1,1,437,16,0,
204,1,33,438,16,
0,204,1,194,439,
19,440,4,12,117,
0,110,0,111,0,
112,0,95,0,51,
0,1,194,441,5,
23,1,45,442,16,
0,294,1,560,443,
16,0,294,1,887,
444,16,0,294,1,
650,445,16,0,294,
1,461,446,16,0,
294,1,412,447,16,
0,294,1,832,448,
16,0,294,1,173,
449,16,0,294,1,
688,450,16,0,294,
1,28,451,16,0,
294,1,309,452,16,
0,294,1,69,453,
16,0,294,1,213,
454,16,0,294,1,
210,455,16,0,294,
1,490,456,16,0,
294,1,582,457,16,
0,294,1,62,458,
16,0,294,1,998,
459,16,0,294,1,
383,460,16,0,294,
1,4,461,16,0,
294,1,98,462,16,
0,294,1,519,463,
16,0,294,1,753,
464,16,0,294,1,
193,465,19,466,4,
12,117,0,110,0,
111,0,112,0,95,
0,50,0,1,193,
441,1,192,467,19,
468,4,12,117,0,
110,0,111,0,112,
0,95,0,49,0,
1,192,441,1,191,
469,19,470,4,10,
97,0,114,0,103,
0,95,0,51,0,
1,191,434,1,190,
471,19,472,4,18,
112,0,97,0,114,
0,108,0,105,0,
115,0,116,0,95,
0,51,0,1,190,
473,5,2,1,1073,
474,16,0,327,1,
13,475,16,0,387,
1,189,476,19,477,
4,16,98,0,105,
0,110,0,111,0,
112,0,95,0,49,
0,53,0,1,189,
478,5,16,1,109,
479,16,0,332,1,
174,480,16,0,332,
1,502,481,16,0,
332,1,395,482,16,
0,332,1,662,483,
16,0,332,1,844,
484,16,0,332,1,
531,485,16,0,332,
1,473,486,16,0,
332,1,76,487,16,
0,332,1,700,488,
16,0,332,1,147,
489,16,0,332,1,
214,490,16,0,332,
1,211,491,16,0,
332,1,335,492,16,
0,332,1,594,493,
16,0,332,1,46,
494,16,0,332,1,
188,495,19,496,4,
16,98,0,105,0,
110,0,111,0,112,
0,95,0,49,0,
52,0,1,188,478,
1,187,497,19,498,
4,16,98,0,105,
0,110,0,111,0,
112,0,95,0,49,
0,51,0,1,187,
478,1,186,499,19,
500,4,16,98,0,
105,0,110,0,111,
0,112,0,95,0,
49,0,50,0,1,
186,478,1,185,501,
19,502,4,16,98,
0,105,0,110,0,
111,0,112,0,95,
0,49,0,49,0,
1,185,478,1,184,
503,19,504,4,16,
98,0,105,0,110,
0,111,0,112,0,
95,0,49,0,48,
0,1,184,478,1,
183,505,19,506,4,
14,98,0,105,0,
110,0,111,0,112,
0,95,0,57,0,
1,183,478,1,182,
507,19,508,4,14,
98,0,105,0,110,
0,111,0,112,0,
95,0,56,0,1,
182,478,1,181,509,
19,510,4,14,98,
0,105,0,110,0,
111,0,112,0,95,
0,55,0,1,181,
478,1,180,511,19,
512,4,14,98,0,
105,0,110,0,111,
0,112,0,95,0,
54,0,1,180,478,
1,179,513,19,514,
4,14,98,0,105,
0,110,0,111,0,
112,0,95,0,53,
0,1,179,478,1,
178,515,19,516,4,
14,98,0,105,0,
110,0,111,0,112,
0,95,0,52,0,
1,178,478,1,177,
517,19,518,4,14,
98,0,105,0,110,
0,111,0,112,0,
95,0,51,0,1,
177,478,1,176,519,
19,520,4,14,98,
0,105,0,110,0,
111,0,112,0,95,
0,50,0,1,176,
478,1,175,521,19,
522,4,14,98,0,
105,0,110,0,111,
0,112,0,95,0,
49,0,1,175,478,
1,174,523,19,524,
4,12,101,0,120,
0,112,0,95,0,
49,0,49,0,1,
174,525,5,23,1,
45,526,16,0,380,
1,560,527,16,0,
176,1,887,528,16,
0,210,1,650,529,
16,0,264,1,461,
530,16,0,131,1,
412,531,16,0,176,
1,832,532,16,0,
210,1,173,533,16,
0,253,1,688,534,
16,0,252,1,28,
535,16,0,203,1,
309,536,16,0,380,
1,69,537,16,0,
319,1,213,538,16,
0,243,1,210,539,
16,0,242,1,490,
540,16,0,115,1,
582,541,16,0,320,
1,62,542,16,0,
263,1,998,543,16,
0,176,1,383,544,
16,0,176,1,4,
545,16,0,176,1,
98,546,16,0,291,
1,519,547,16,0,
331,1,753,548,16,
0,176,1,173,549,
19,550,4,12,101,
0,120,0,112,0,
95,0,49,0,48,
0,1,173,525,1,
172,551,19,552,4,
10,101,0,120,0,
112,0,95,0,57,
0,1,172,525,1,
171,553,19,554,4,
20,102,0,105,0,
101,0,108,0,100,
0,115,0,101,0,
112,0,95,0,50,
0,1,171,555,5,
1,1,306,556,16,
0,212,1,170,557,
19,558,4,20,102,
0,105,0,101,0,
108,0,100,0,115,
0,101,0,112,0,
95,0,49,0,1,
170,555,1,169,559,
19,560,4,14,102,
0,105,0,101,0,
108,0,100,0,95,
0,49,0,1,169,
561,5,2,1,45,
562,16,0,219,1,
309,563,16,0,219,
1,168,564,19,565,
4,26,70,0,105,
0,101,0,108,0,
100,0,65,0,115,
0,115,0,105,0,
103,0,110,0,95,
0,49,0,1,168,
561,1,167,566,19,
567,4,32,70,0,
105,0,101,0,108,
0,100,0,69,0,
120,0,112,0,65,
0,115,0,115,0,
105,0,103,0,110,
0,95,0,49,0,
1,167,561,1,166,
568,19,569,4,10,
97,0,114,0,103,
0,95,0,50,0,
1,166,434,1,165,
570,19,571,4,10,
97,0,114,0,103,
0,95,0,49,0,
1,165,434,1,164,
572,19,573,4,20,
102,0,117,0,110,
0,99,0,116,0,
105,0,111,0,110,
0,95,0,49,0,
1,164,574,5,23,
1,45,575,16,0,
280,1,560,576,16,
0,280,1,887,577,
16,0,280,1,650,
578,16,0,280,1,
461,579,16,0,280,
1,412,580,16,0,
280,1,832,581,16,
0,280,1,173,582,
16,0,280,1,688,
583,16,0,280,1,
28,584,16,0,280,
1,309,585,16,0,
280,1,69,586,16,
0,280,1,213,587,
16,0,280,1,210,
588,16,0,280,1,
490,589,16,0,280,
1,582,590,16,0,
280,1,62,591,16,
0,280,1,998,592,
16,0,280,1,383,
593,16,0,280,1,
4,594,16,0,280,
1,98,595,16,0,
280,1,519,596,16,
0,280,1,753,597,
16,0,280,1,163,
598,19,599,4,20,
102,0,117,0,110,
0,99,0,98,0,
111,0,100,0,121,
0,95,0,52,0,
1,163,600,5,3,
1,445,601,16,0,
161,1,455,602,16,
0,145,1,11,603,
16,0,413,1,162,
604,19,605,4,20,
102,0,117,0,110,
0,99,0,98,0,
111,0,100,0,121,
0,95,0,51,0,
1,162,600,1,161,
606,19,607,4,20,
102,0,117,0,110,
0,99,0,98,0,
111,0,100,0,121,
0,95,0,50,0,
1,161,600,1,160,
608,19,609,4,20,
102,0,117,0,110,
0,99,0,98,0,
111,0,100,0,121,
0,95,0,49,0,
1,160,600,1,159,
610,19,611,4,20,
102,0,117,0,110,
0,99,0,110,0,
97,0,109,0,101,
0,95,0,51,0,
1,159,612,5,2,
1,448,613,16,0,
148,1,452,614,16,
0,151,1,158,615,
19,616,4,20,102,
0,117,0,110,0,
99,0,110,0,97,
0,109,0,101,0,
95,0,50,0,1,
158,612,1,157,617,
19,618,4,20,102,
0,117,0,110,0,
99,0,110,0,97,
0,109,0,101,0,
95,0,49,0,1,
157,612,1,156,619,
19,620,4,24,80,
0,97,0,99,0,
107,0,97,0,103,
0,101,0,82,0,
101,0,102,0,95,
0,49,0,1,156,
621,5,39,1,213,
622,16,0,300,1,
961,623,16,0,406,
1,210,624,16,0,
300,1,637,625,16,
0,406,1,98,626,
16,0,300,1,309,
627,16,0,300,1,
734,628,16,0,406,
1,519,629,16,0,
300,1,623,630,16,
0,406,1,1050,631,
16,0,406,1,832,
632,16,0,300,1,
490,633,16,0,300,
1,611,634,16,0,
406,1,717,635,16,
0,406,1,69,636,
16,0,300,1,173,
637,16,0,300,1,
62,638,16,0,300,
1,383,639,16,0,
300,1,1020,640,16,
0,406,1,582,641,
16,0,300,1,688,
642,16,0,300,1,
45,643,16,0,300,
1,792,644,16,0,
406,1,998,645,16,
0,300,1,461,646,
16,0,300,1,887,
647,16,0,300,1,
28,648,16,0,300,
1,776,649,16,0,
406,1,412,650,16,
0,300,1,560,651,
16,0,300,1,21,
652,16,0,406,1,
14,653,16,0,406,
1,548,654,16,0,
406,1,868,655,16,
0,406,1,650,656,
16,0,300,1,4,
657,16,0,300,1,
861,658,16,0,406,
1,753,659,16,0,
300,1,0,660,16,
0,406,1,155,661,
19,662,4,20,84,
0,97,0,98,0,
108,0,101,0,82,
0,101,0,102,0,
95,0,49,0,1,
155,621,1,154,663,
19,664,4,10,118,
0,97,0,114,0,
95,0,49,0,1,
154,621,1,153,665,
19,666,4,18,118,
0,97,0,114,0,
108,0,105,0,115,
0,116,0,95,0,
50,0,1,153,667,
5,16,1,734,668,
16,0,236,1,14,
669,16,0,236,1,
548,670,16,0,236,
1,961,671,16,0,
236,1,776,672,16,
0,236,1,861,673,
16,0,236,1,1050,
674,16,0,236,1,
637,675,16,0,236,
1,611,676,16,0,
236,1,792,677,16,
0,236,1,1020,678,
16,0,236,1,0,
679,16,0,236,1,
717,680,16,0,236,
1,623,681,16,0,
236,1,868,682,16,
0,236,1,21,683,
16,0,186,1,152,
684,19,685,4,18,
118,0,97,0,114,
0,108,0,105,0,
115,0,116,0,95,
0,49,0,1,152,
667,1,151,686,19,
687,4,20,110,0,
97,0,109,0,101,
0,108,0,105,0,
115,0,116,0,95,
0,50,0,1,151,
688,5,3,1,458,
689,16,0,128,1,
379,690,16,0,180,
1,377,691,16,0,
179,1,150,692,19,
693,4,20,110,0,
97,0,109,0,101,
0,108,0,105,0,
115,0,116,0,95,
0,49,0,1,150,
688,1,149,694,19,
695,4,22,112,0,
114,0,101,0,102,
0,105,0,120,0,
101,0,120,0,112,
0,95,0,51,0,
1,149,696,5,39,
1,213,697,16,0,
395,1,961,698,16,
0,428,1,210,699,
16,0,395,1,637,
700,16,0,428,1,
98,701,16,0,395,
1,309,702,16,0,
395,1,734,703,16,
0,428,1,519,704,
16,0,395,1,623,
705,16,0,428,1,
1050,706,16,0,428,
1,832,707,16,0,
395,1,490,708,16,
0,395,1,611,709,
16,0,428,1,717,
710,16,0,428,1,
69,711,16,0,395,
1,173,712,16,0,
395,1,62,713,16,
0,395,1,383,714,
16,0,395,1,1020,
715,16,0,428,1,
582,716,16,0,395,
1,688,717,16,0,
395,1,45,718,16,
0,395,1,792,719,
16,0,428,1,998,
720,16,0,395,1,
461,721,16,0,395,
1,887,722,16,0,
395,1,28,723,16,
0,395,1,776,724,
16,0,428,1,412,
725,16,0,395,1,
560,726,16,0,395,
1,21,727,16,0,
404,1,14,728,16,
0,428,1,548,729,
16,0,428,1,868,
730,16,0,428,1,
650,731,16,0,395,
1,4,732,16,0,
395,1,861,733,16,
0,428,1,753,734,
16,0,395,1,0,
735,16,0,428,1,
148,736,19,737,4,
22,112,0,114,0,
101,0,102,0,105,
0,120,0,101,0,
120,0,112,0,95,
0,50,0,1,148,
696,1,147,738,19,
739,4,22,112,0,
114,0,101,0,102,
0,105,0,120,0,
101,0,120,0,112,
0,95,0,49,0,
1,147,696,1,146,
740,19,741,4,28,
102,0,117,0,110,
0,99,0,116,0,
105,0,111,0,110,
0,99,0,97,0,
108,0,108,0,95,
0,50,0,1,146,
742,5,39,1,213,
743,16,0,303,1,
961,744,16,0,303,
1,210,745,16,0,
303,1,637,746,16,
0,303,1,98,747,
16,0,303,1,309,
748,16,0,303,1,
734,749,16,0,303,
1,519,750,16,0,
303,1,623,751,16,
0,303,1,1050,752,
16,0,303,1,832,
753,16,0,303,1,
490,754,16,0,303,
1,611,755,16,0,
303,1,717,756,16,
0,303,1,69,757,
16,0,303,1,173,
758,16,0,303,1,
62,759,16,0,303,
1,383,760,16,0,
303,1,1020,761,16,
0,303,1,582,762,
16,0,303,1,688,
763,16,0,303,1,
45,764,16,0,303,
1,792,765,16,0,
303,1,998,766,16,
0,303,1,461,767,
16,0,303,1,887,
768,16,0,303,1,
28,769,16,0,303,
1,776,770,16,0,
303,1,412,771,16,
0,303,1,560,772,
16,0,303,1,21,
773,16,0,303,1,
14,774,16,0,303,
1,548,775,16,0,
303,1,868,776,16,
0,303,1,650,777,
16,0,303,1,4,
778,16,0,303,1,
861,779,16,0,303,
1,753,780,16,0,
303,1,0,781,16,
0,303,1,145,782,
19,783,4,28,102,
0,117,0,110,0,
99,0,116,0,105,
0,111,0,110,0,
99,0,97,0,108,
0,108,0,95,0,
49,0,1,145,742,
1,144,784,19,296,
1,144,441,1,143,
785,19,334,1,143,
478,1,142,786,19,
787,4,10,101,0,
120,0,112,0,95,
0,56,0,1,142,
525,1,141,788,19,
789,4,10,101,0,
120,0,112,0,95,
0,55,0,1,141,
525,1,140,790,19,
791,4,10,101,0,
120,0,112,0,95,
0,54,0,1,140,
525,1,139,792,19,
793,4,10,101,0,
120,0,112,0,95,
0,53,0,1,139,
525,1,138,794,19,
795,4,10,101,0,
120,0,112,0,95,
0,52,0,1,138,
525,1,137,796,19,
797,4,10,101,0,
120,0,112,0,95,
0,51,0,1,137,
525,1,136,798,19,
799,4,10,101,0,
120,0,112,0,95,
0,50,0,1,136,
525,1,135,800,19,
801,4,10,101,0,
120,0,112,0,95,
0,49,0,1,135,
525,1,134,802,19,
803,4,18,101,0,
120,0,112,0,108,
0,105,0,115,0,
116,0,95,0,50,
0,1,134,804,5,
6,1,753,805,16,
0,234,1,383,806,
16,0,167,1,998,
807,16,0,121,1,
560,808,16,0,313,
1,412,809,16,0,
169,1,4,810,16,
0,417,1,133,811,
19,812,4,18,101,
0,120,0,112,0,
108,0,105,0,115,
0,116,0,95,0,
49,0,1,133,804,
1,132,813,19,814,
4,12,105,0,110,
0,105,0,116,0,
95,0,49,0,1,
132,815,5,1,1,
382,816,16,0,164,
1,131,817,19,818,
4,18,112,0,97,
0,114,0,108,0,
105,0,115,0,116,
0,95,0,50,0,
1,131,473,1,130,
819,19,820,4,18,
112,0,97,0,114,
0,108,0,105,0,
115,0,116,0,95,
0,49,0,1,130,
473,1,129,821,19,
822,4,36,116,0,
97,0,98,0,108,
0,101,0,99,0,
111,0,110,0,115,
0,116,0,114,0,
117,0,99,0,116,
0,111,0,114,0,
95,0,50,0,1,
129,823,5,27,1,
45,824,16,0,283,
1,560,825,16,0,
283,1,33,826,16,
0,426,1,887,827,
16,0,283,1,40,
828,16,0,426,1,
650,829,16,0,283,
1,461,830,16,0,
283,1,412,831,16,
0,283,1,832,832,
16,0,283,1,173,
833,16,0,283,1,
688,834,16,0,283,
1,28,835,16,0,
283,1,309,836,16,
0,283,1,69,837,
16,0,283,1,213,
838,16,0,283,1,
22,839,16,0,426,
1,210,840,16,0,
283,1,490,841,16,
0,283,1,582,842,
16,0,283,1,62,
843,16,0,283,1,
998,844,16,0,283,
1,383,845,16,0,
283,1,4,846,16,
0,283,1,98,847,
16,0,283,1,1,
848,16,0,426,1,
519,849,16,0,283,
1,753,850,16,0,
283,1,128,851,19,
852,4,36,116,0,
97,0,98,0,108,
0,101,0,99,0,
111,0,110,0,115,
0,116,0,114,0,
117,0,99,0,116,
0,111,0,114,0,
95,0,49,0,1,
128,823,1,127,853,
19,854,4,22,102,
0,105,0,101,0,
108,0,100,0,108,
0,105,0,115,0,
116,0,95,0,51,
0,1,127,855,5,
2,1,45,856,16,
0,226,1,309,857,
16,0,207,1,126,
858,19,859,4,22,
102,0,105,0,101,
0,108,0,100,0,
108,0,105,0,115,
0,116,0,95,0,
50,0,1,126,855,
1,125,860,19,214,
1,125,555,1,124,
861,19,862,4,22,
102,0,105,0,101,
0,108,0,100,0,
108,0,105,0,115,
0,116,0,95,0,
49,0,1,124,855,
1,123,863,19,864,
4,8,73,0,102,
0,95,0,49,0,
1,123,865,5,2,
1,887,866,16,0,
177,1,832,867,16,
0,173,1,122,868,
19,869,4,12,69,
0,108,0,115,0,
101,0,95,0,49,
0,1,122,865,1,
121,870,19,871,4,
16,69,0,108,0,
115,0,101,0,73,
0,102,0,95,0,
49,0,1,121,865,
1,120,872,19,873,
4,22,76,0,111,
0,99,0,97,0,
108,0,73,0,110,
0,105,0,116,0,
95,0,49,0,1,
120,874,5,15,1,
734,875,16,0,230,
1,14,876,16,0,
230,1,548,877,16,
0,230,1,961,878,
16,0,230,1,776,
879,16,0,230,1,
861,880,16,0,230,
1,1050,881,16,0,
230,1,637,882,16,
0,230,1,611,883,
16,0,230,1,792,
884,16,0,230,1,
1020,885,16,0,230,
1,717,886,16,0,
230,1,623,887,16,
0,230,1,868,888,
16,0,230,1,0,
889,16,0,230,1,
119,890,19,891,4,
30,76,0,111,0,
99,0,97,0,108,
0,78,0,97,0,
109,0,101,0,108,
0,105,0,115,0,
116,0,95,0,49,
0,1,119,874,1,
118,892,19,893,4,
30,76,0,111,0,
99,0,97,0,108,
0,70,0,117,0,
110,0,99,0,68,
0,101,0,99,0,
108,0,95,0,49,
0,1,118,874,1,
117,894,19,895,4,
20,70,0,117,0,
110,0,99,0,68,
0,101,0,99,0,
108,0,95,0,49,
0,1,117,874,1,
116,896,19,897,4,
10,70,0,111,0,
114,0,95,0,51,
0,1,116,874,1,
115,898,19,899,4,
10,70,0,111,0,
114,0,95,0,50,
0,1,115,874,1,
114,900,19,901,4,
10,70,0,111,0,
114,0,95,0,49,
0,1,114,874,1,
113,902,19,903,4,
12,115,0,116,0,
97,0,116,0,95,
0,52,0,1,113,
874,1,112,904,19,
905,4,16,82,0,
101,0,116,0,118,
0,97,0,108,0,
95,0,49,0,1,
112,874,1,111,906,
19,907,4,12,115,
0,116,0,97,0,
116,0,95,0,51,
0,1,111,874,1,
110,908,19,909,4,
14,83,0,69,0,
108,0,115,0,101,
0,95,0,49,0,
1,110,874,1,109,
910,19,911,4,18,
83,0,69,0,108,
0,115,0,101,0,
73,0,102,0,95,
0,49,0,1,109,
874,1,108,912,19,
913,4,10,83,0,
73,0,102,0,95,
0,49,0,1,108,
874,1,107,914,19,
915,4,16,82,0,
101,0,112,0,101,
0,97,0,116,0,
95,0,49,0,1,
107,874,1,106,916,
19,917,4,12,115,
0,116,0,97,0,
116,0,95,0,50,
0,1,106,874,1,
105,918,19,919,4,
14,87,0,104,0,
105,0,108,0,101,
0,95,0,49,0,
1,105,874,1,104,
920,19,921,4,12,
115,0,116,0,97,
0,116,0,95,0,
49,0,1,104,874,
1,103,922,19,923,
4,8,68,0,111,
0,95,0,49,0,
1,103,874,1,102,
924,19,925,4,28,
70,0,117,0,110,
0,99,0,116,0,
105,0,111,0,110,
0,67,0,97,0,
108,0,108,0,95,
0,49,0,1,102,
874,1,101,926,19,
927,4,24,65,0,
115,0,115,0,105,
0,103,0,110,0,
109,0,101,0,110,
0,116,0,95,0,
49,0,1,101,874,
1,100,928,19,929,
4,14,98,0,108,
0,111,0,99,0,
107,0,95,0,50,
0,1,100,930,5,
12,1,734,931,16,
0,238,1,548,932,
16,0,394,1,961,
933,16,0,157,1,
868,934,16,0,192,
1,637,935,16,0,
262,1,1050,936,16,
0,315,1,14,937,
16,0,409,1,611,
938,16,0,293,1,
861,939,16,0,199,
1,1020,940,16,0,
112,1,717,941,16,
0,246,1,623,942,
16,0,290,1,99,
943,19,944,4,14,
98,0,108,0,111,
0,99,0,107,0,
95,0,49,0,1,
99,930,1,98,945,
19,946,4,14,99,
0,104,0,117,0,
110,0,107,0,95,
0,52,0,1,98,
947,5,15,1,734,
948,16,0,233,1,
14,949,16,0,233,
1,548,950,16,0,
233,1,961,951,16,
0,233,1,776,952,
16,0,227,1,861,
953,16,0,233,1,
1050,954,16,0,233,
1,637,955,16,0,
233,1,611,956,16,
0,233,1,792,957,
16,0,228,1,1020,
958,16,0,233,1,
717,959,16,0,233,
1,623,960,16,0,
233,1,868,961,16,
0,233,1,0,962,
16,0,104,1,97,
963,19,964,4,14,
99,0,104,0,117,
0,110,0,107,0,
95,0,51,0,1,
97,947,1,96,965,
19,966,4,14,99,
0,104,0,117,0,
110,0,107,0,95,
0,50,0,1,96,
947,1,95,967,19,
968,4,14,99,0,
104,0,117,0,110,
0,107,0,95,0,
49,0,1,95,947,
1,94,969,19,206,
1,94,434,1,93,
970,19,971,4,8,
69,0,108,0,115,
0,101,0,1,93,
865,1,92,972,19,
973,4,12,69,0,
108,0,115,0,101,
0,73,0,102,0,
1,92,865,1,91,
974,19,975,4,4,
73,0,102,0,1,
91,865,1,90,976,
19,175,1,90,865,
1,89,977,19,978,
4,10,83,0,69,
0,108,0,115,0,
101,0,1,89,874,
1,88,979,19,980,
4,14,83,0,69,
0,108,0,115,0,
101,0,73,0,102,
0,1,88,874,1,
87,981,19,982,4,
6,83,0,73,0,
102,0,1,87,874,
1,86,983,19,984,
4,6,70,0,111,
0,114,0,1,86,
874,1,85,985,19,
986,4,12,82,0,
101,0,112,0,101,
0,97,0,116,0,
1,85,874,1,84,
987,19,988,4,10,
87,0,104,0,105,
0,108,0,101,0,
1,84,874,1,83,
989,19,990,4,4,
68,0,111,0,1,
83,874,1,82,991,
19,992,4,24,70,
0,117,0,110,0,
99,0,116,0,105,
0,111,0,110,0,
67,0,97,0,108,
0,108,0,1,82,
874,1,81,993,19,
994,4,26,76,0,
111,0,99,0,97,
0,108,0,70,0,
117,0,110,0,99,
0,68,0,101,0,
99,0,108,0,1,
81,874,1,80,995,
19,996,4,16,70,
0,117,0,110,0,
99,0,68,0,101,
0,99,0,108,0,
1,80,874,1,79,
997,19,998,4,12,
82,0,101,0,116,
0,118,0,97,0,
108,0,1,79,874,
1,78,999,19,1000,
4,26,76,0,111,
0,99,0,97,0,
108,0,78,0,97,
0,109,0,101,0,
108,0,105,0,115,
0,116,0,1,78,
874,1,77,1001,19,
1002,4,18,76,0,
111,0,99,0,97,
0,108,0,73,0,
110,0,105,0,116,
0,1,77,874,1,
76,1003,19,1004,4,
20,65,0,115,0,
115,0,105,0,103,
0,110,0,109,0,
101,0,110,0,116,
0,1,76,874,1,
75,1005,19,232,1,
75,874,1,74,1006,
19,166,1,74,815,
1,73,1007,19,188,
1,73,667,1,72,
1008,19,1009,4,16,
84,0,97,0,98,
0,108,0,101,0,
82,0,101,0,102,
0,1,72,621,1,
71,1010,19,1011,4,
20,80,0,97,0,
99,0,107,0,97,
0,103,0,101,0,
82,0,101,0,102,
0,1,71,621,1,
70,1012,19,302,1,
70,621,1,69,1013,
19,123,1,69,804,
1,68,1014,19,117,
1,68,525,1,67,
1015,19,282,1,67,
574,1,66,1016,19,
397,1,66,696,1,
65,1017,19,147,1,
65,600,1,64,1018,
19,329,1,64,473,
1,63,1019,19,150,
1,63,612,1,62,
1020,19,305,1,62,
742,1,61,1021,19,
130,1,61,688,1,
60,1022,19,285,1,
60,823,1,59,1023,
19,209,1,59,855,
1,58,1024,19,1025,
4,22,70,0,105,
0,101,0,108,0,
100,0,65,0,115,
0,115,0,105,0,
103,0,110,0,1,
58,561,1,57,1026,
19,1027,4,28,70,
0,105,0,101,0,
108,0,100,0,69,
0,120,0,112,0,
65,0,115,0,115,
0,105,0,103,0,
110,0,1,57,561,
1,56,1028,19,221,
1,56,561,1,55,
1029,19,114,1,55,
930,1,54,1030,19,
103,1,54,947,1,
53,1031,19,299,1,
53,1032,5,45,1,
213,1033,16,0,297,
1,412,1034,16,0,
297,1,210,1035,16,
0,297,1,98,1036,
16,0,297,1,309,
1037,16,0,297,1,
308,1038,17,1039,15,
1040,4,18,37,0,
102,0,105,0,101,
0,108,0,100,0,
115,0,101,0,112,
0,1,-1,1,5,
213,1,1,1,1,
1041,22,1,94,1,
307,1042,17,1043,15,
1040,1,-1,1,5,
213,1,1,1,1,
1044,22,1,95,1,
519,1045,16,0,297,
1,832,1046,16,0,
297,1,490,1047,16,
0,297,1,69,1048,
16,0,297,1,47,
1049,17,1050,15,1051,
4,12,37,0,98,
0,105,0,110,0,
111,0,112,0,1,
-1,1,5,333,1,
1,1,1,1052,22,
1,93,1,173,1053,
16,0,297,1,62,
1054,16,0,297,1,
383,1055,16,0,297,
1,61,1056,17,1057,
15,1051,1,-1,1,
5,333,1,1,1,
1,1058,22,1,79,
1,60,1059,17,1060,
15,1051,1,-1,1,
5,333,1,1,1,
1,1061,22,1,80,
1,59,1062,17,1063,
15,1051,1,-1,1,
5,333,1,1,1,
1,1064,22,1,81,
1,58,1065,17,1066,
15,1051,1,-1,1,
5,333,1,1,1,
1,1067,22,1,82,
1,57,1068,17,1069,
15,1051,1,-1,1,
5,333,1,1,1,
1,1070,22,1,83,
1,56,1071,17,1072,
15,1051,1,-1,1,
5,333,1,1,1,
1,1073,22,1,84,
1,55,1074,17,1075,
15,1051,1,-1,1,
5,333,1,1,1,
1,1076,22,1,85,
1,54,1077,17,1078,
15,1051,1,-1,1,
5,333,1,1,1,
1,1079,22,1,86,
1,53,1080,17,1081,
15,1051,1,-1,1,
5,333,1,1,1,
1,1082,22,1,87,
1,52,1083,17,1084,
15,1051,1,-1,1,
5,333,1,1,1,
1,1085,22,1,88,
1,51,1086,17,1087,
15,1051,1,-1,1,
5,333,1,1,1,
1,1088,22,1,89,
1,50,1089,17,1090,
15,1051,1,-1,1,
5,333,1,1,1,
1,1091,22,1,90,
1,49,1092,17,1093,
15,1051,1,-1,1,
5,333,1,1,1,
1,1094,22,1,91,
1,48,1095,17,1096,
15,1051,1,-1,1,
5,333,1,1,1,
1,1097,22,1,92,
1,582,1098,16,0,
297,1,688,1099,16,
0,297,1,45,1100,
16,0,297,1,998,
1101,16,0,297,1,
461,1102,16,0,297,
1,887,1103,16,0,
297,1,28,1104,16,
0,297,1,560,1105,
16,0,297,1,13,
1106,16,0,400,1,
4,1107,16,0,297,
1,650,1108,16,0,
297,1,7,1109,17,
1110,15,1111,4,10,
37,0,117,0,110,
0,111,0,112,0,
1,-1,1,5,295,
1,1,1,1,1112,
22,1,76,1,6,
1113,17,1114,15,1111,
1,-1,1,5,295,
1,1,1,1,1115,
22,1,77,1,5,
1116,17,1117,15,1111,
1,-1,1,5,295,
1,1,1,1,1118,
22,1,78,1,753,
1119,16,0,297,1,
1073,1120,16,0,400,
1,52,1121,19,355,
1,52,1122,5,46,
1,211,1123,16,0,
353,1,531,1124,16,
0,353,1,305,1125,
17,1126,15,1127,4,
34,37,0,116,0,
97,0,98,0,108,
0,101,0,99,0,
111,0,110,0,115,
0,116,0,114,0,
117,0,99,0,116,
0,111,0,114,0,
1,-1,1,5,1128,
20,852,1,128,1,
3,1,3,1,2,
1129,22,1,33,1,
97,1130,17,1131,15,
1132,4,8,37,0,
101,0,120,0,112,
0,1,-1,1,5,
116,1,1,1,1,
1133,22,1,51,1,
96,1134,17,1135,15,
1136,4,20,37,0,
112,0,114,0,101,
0,102,0,105,0,
120,0,101,0,120,
0,112,0,1,-1,
1,5,1137,20,739,
1,147,1,3,1,
2,1,1,1138,22,
1,54,1,95,1139,
17,1140,15,1136,1,
-1,1,5,1141,20,
737,1,148,1,3,
1,2,1,1,1142,
22,1,55,1,92,
1143,17,1144,15,1136,
1,-1,1,5,1145,
20,695,1,149,1,
3,1,4,1,3,
1146,22,1,56,1,
502,1147,16,0,353,
1,1053,1148,17,1149,
15,1150,4,18,37,
0,102,0,117,0,
110,0,99,0,98,
0,111,0,100,0,
121,0,1,-1,1,
5,1151,20,607,1,
161,1,3,1,6,
1,5,1152,22,1,
68,1,303,1153,17,
1154,15,1127,1,-1,
1,5,1155,20,822,
1,129,1,3,1,
4,1,3,1156,22,
1,34,1,1051,1157,
17,1158,15,1150,1,
-1,1,5,1159,20,
605,1,162,1,3,
1,5,1,4,1160,
22,1,69,1,76,
1161,16,0,353,1,
395,1162,16,0,353,
1,174,1163,16,0,
353,1,172,1164,17,
1165,15,1166,4,8,
37,0,118,0,97,
0,114,0,1,-1,
1,5,1167,20,664,
1,154,1,3,1,
2,1,1,1168,22,
1,61,1,594,1169,
16,0,353,1,700,
1170,16,0,353,1,
46,1171,16,0,353,
1,473,1172,16,0,
353,1,44,1173,17,
1174,15,1175,4,26,
37,0,102,0,117,
0,110,0,99,0,
116,0,105,0,111,
0,110,0,99,0,
97,0,108,0,108,
0,1,-1,1,5,
1176,20,741,1,146,
1,3,1,5,1,
4,1177,22,1,53,
1,147,1178,16,0,
353,1,127,1179,17,
1180,15,1132,1,-1,
1,5,1181,20,787,
1,142,1,3,1,
2,1,1,1182,22,
1,48,1,33,1183,
17,1184,15,1132,1,
-1,1,5,1185,20,
789,1,141,1,3,
1,2,1,1,1186,
22,1,47,1,351,
1187,17,1188,15,1189,
4,18,37,0,84,
0,97,0,98,0,
108,0,101,0,82,
0,101,0,102,0,
1,-1,1,5,1190,
20,662,1,155,1,
3,1,5,1,4,
1191,22,1,62,1,
20,1192,17,1135,1,
1,1138,1,27,1193,
17,1194,15,1195,4,
22,37,0,80,0,
97,0,99,0,107,
0,97,0,103,0,
101,0,82,0,101,
0,102,0,1,-1,
1,5,1196,20,620,
1,156,1,3,1,
4,1,3,1197,22,
1,63,1,133,1198,
17,1199,15,1132,1,
-1,1,5,1200,20,
801,1,135,1,3,
1,2,1,1,1201,
22,1,41,1,132,
1202,17,1203,15,1132,
1,-1,1,5,1204,
20,799,1,136,1,
3,1,2,1,1,
1205,22,1,42,1,
131,1206,17,1207,15,
1132,1,-1,1,5,
1208,20,797,1,137,
1,3,1,2,1,
1,1209,22,1,43,
1,130,1210,17,1211,
15,1132,1,-1,1,
5,1212,20,795,1,
138,1,3,1,2,
1,1,1213,22,1,
44,1,129,1214,17,
1215,15,1132,1,-1,
1,5,1216,20,793,
1,139,1,3,1,
2,1,1,1217,22,
1,45,1,128,1218,
17,1219,15,1132,1,
-1,1,5,1220,20,
791,1,140,1,3,
1,2,1,1,1221,
22,1,46,1,662,
1222,16,0,353,1,
19,1223,17,1165,1,
1,1168,1,12,1224,
17,1225,15,1226,4,
18,37,0,102,0,
117,0,110,0,99,
0,116,0,105,0,
111,0,110,0,1,
-1,1,5,1227,20,
573,1,164,1,3,
1,3,1,2,1228,
22,1,71,1,17,
1229,17,1230,15,1150,
1,-1,1,5,1231,
20,609,1,160,1,
3,1,5,1,4,
1232,22,1,67,1,
844,1233,16,0,353,
1,15,1234,17,1235,
15,1150,1,-1,1,
5,1236,20,599,1,
163,1,3,1,4,
1,3,1237,22,1,
70,1,335,1238,16,
0,353,1,333,1239,
17,1240,15,1175,1,
-1,1,5,1241,20,
783,1,145,1,3,
1,3,1,2,1242,
22,1,52,1,10,
1243,17,1244,15,1245,
4,8,37,0,97,
0,114,0,103,0,
1,-1,1,5,205,
1,2,1,2,1246,
22,1,72,1,9,
1247,17,1248,15,1245,
1,-1,1,5,1249,
20,571,1,165,1,
3,1,4,1,3,
1250,22,1,73,1,
2,1251,17,1252,15,
1245,1,-1,1,5,
205,1,1,1,1,
1253,22,1,75,1,
3,1254,17,1255,15,
1245,1,-1,1,5,
1256,20,569,1,166,
1,3,1,2,1,
1,1257,22,1,74,
1,109,1258,16,0,
353,1,214,1259,16,
0,353,1,51,1260,
19,270,1,51,1261,
5,43,1,213,1262,
16,0,268,1,412,
1263,16,0,268,1,
210,1264,16,0,268,
1,98,1265,16,0,
268,1,309,1266,16,
0,268,1,308,1038,
1,307,1042,1,519,
1267,16,0,268,1,
832,1268,16,0,268,
1,490,1269,16,0,
268,1,69,1270,16,
0,268,1,47,1049,
1,173,1271,16,0,
268,1,62,1272,16,
0,268,1,383,1273,
16,0,268,1,61,
1056,1,60,1059,1,
59,1062,1,58,1065,
1,57,1068,1,56,
1071,1,55,1074,1,
54,1077,1,53,1080,
1,52,1083,1,51,
1086,1,50,1089,1,
49,1092,1,48,1095,
1,582,1274,16,0,
268,1,688,1275,16,
0,268,1,45,1276,
16,0,268,1,998,
1277,16,0,268,1,
461,1278,16,0,268,
1,887,1279,16,0,
268,1,28,1280,16,
0,268,1,560,1281,
16,0,268,1,4,
1282,16,0,268,1,
650,1283,16,0,268,
1,7,1109,1,6,
1113,1,5,1116,1,
753,1284,16,0,268,
1,50,1285,19,273,
1,50,1286,5,43,
1,213,1287,16,0,
271,1,412,1288,16,
0,271,1,210,1289,
16,0,271,1,98,
1290,16,0,271,1,
309,1291,16,0,271,
1,308,1038,1,307,
1042,1,519,1292,16,
0,271,1,832,1293,
16,0,271,1,490,
1294,16,0,271,1,
69,1295,16,0,271,
1,47,1049,1,173,
1296,16,0,271,1,
62,1297,16,0,271,
1,383,1298,16,0,
271,1,61,1056,1,
60,1059,1,59,1062,
1,58,1065,1,57,
1068,1,56,1071,1,
55,1074,1,54,1077,
1,53,1080,1,52,
1083,1,51,1086,1,
50,1089,1,49,1092,
1,48,1095,1,582,
1299,16,0,271,1,
688,1300,16,0,271,
1,45,1301,16,0,
271,1,998,1302,16,
0,271,1,461,1303,
16,0,271,1,887,
1304,16,0,271,1,
28,1305,16,0,271,
1,560,1306,16,0,
271,1,4,1307,16,
0,271,1,650,1308,
16,0,271,1,7,
1109,1,6,1113,1,
5,1116,1,753,1309,
16,0,271,1,49,
1310,19,185,1,49,
1311,5,73,1,748,
1312,17,1313,15,1314,
4,10,37,0,115,
0,116,0,97,0,
116,0,1,-1,1,
5,1315,20,921,1,
104,1,3,1,3,
1,2,1316,22,1,
10,1,961,1317,16,
0,183,1,92,1143,
1,637,1318,16,0,
183,1,635,1319,17,
1320,15,1321,4,12,
37,0,83,0,69,
0,108,0,115,0,
101,0,1,-1,1,
5,1322,20,909,1,
110,1,3,1,8,
1,7,1323,22,1,
16,1,97,1130,1,
96,1134,1,95,1139,
1,734,1324,16,0,
183,1,305,1125,1,
1020,1325,16,0,183,
1,303,1153,1,730,
1326,17,1327,15,1314,
1,-1,1,5,1328,
20,917,1,106,1,
3,1,5,1,4,
1329,22,1,12,1,
1050,1330,16,0,183,
1,611,1331,16,0,
183,1,717,1332,16,
0,183,1,395,1333,
17,1334,15,1335,4,
16,37,0,101,0,
120,0,112,0,108,
0,105,0,115,0,
116,0,1,-1,1,
5,1336,20,803,1,
134,1,3,1,2,
1,1,1337,22,1,
40,1,971,1338,17,
1339,15,1340,4,8,
37,0,70,0,111,
0,114,0,1,-1,
1,5,1341,20,901,
1,114,1,3,1,
10,1,9,1342,22,
1,20,1,1029,1343,
17,1344,15,1340,1,
-1,1,5,1345,20,
897,1,116,1,3,
1,8,1,7,1346,
22,1,22,1,172,
1164,1,560,1347,17,
1348,15,1314,1,-1,
1,5,1349,20,907,
1,111,1,3,1,
2,1,1,1350,22,
1,17,1,559,1351,
17,1352,15,1314,1,
-1,1,5,1353,20,
903,1,113,1,3,
1,2,1,1,1354,
22,1,19,1,382,
1355,17,1356,15,1357,
4,28,37,0,76,
0,111,0,99,0,
97,0,108,0,78,
0,97,0,109,0,
101,0,108,0,105,
0,115,0,116,0,
1,-1,1,5,1358,
20,891,1,119,1,
3,1,3,1,2,
1359,22,1,25,1,
930,1360,17,1361,15,
1362,4,16,37,0,
83,0,69,0,108,
0,115,0,101,0,
73,0,102,0,1,
-1,1,5,1363,20,
911,1,109,1,3,
1,8,1,7,1364,
22,1,15,1,380,
1365,17,1366,15,1367,
4,18,37,0,110,
0,97,0,109,0,
101,0,108,0,105,
0,115,0,116,0,
1,-1,1,5,1368,
20,687,1,151,1,
3,1,4,1,3,
1369,22,1,58,1,
378,1370,17,1371,15,
1367,1,-1,1,5,
1372,20,693,1,150,
1,3,1,2,1,
1,1373,22,1,57,
1,15,1234,1,1051,
1157,1,44,1173,1,
581,1374,17,1375,15,
1376,4,14,37,0,
82,0,101,0,116,
0,118,0,97,0,
108,0,1,-1,1,
5,1377,20,905,1,
112,1,3,1,3,
1,2,1378,22,1,
18,1,17,1229,1,
623,1379,16,0,183,
1,792,1380,16,0,
183,1,558,1381,17,
1382,15,1340,1,-1,
1,5,1383,20,899,
1,115,1,3,1,
12,1,11,1384,22,
1,21,1,19,1223,
1,147,1385,17,1386,
15,1132,1,-1,1,
5,116,1,3,1,
3,1387,22,1,49,
1,27,1193,1,132,
1202,1,131,1206,1,
127,1179,1,33,1183,
1,548,1388,16,0,
183,1,459,1389,17,
1371,1,1,1373,1,
351,1187,1,457,1390,
17,1391,15,1392,4,
18,37,0,70,0,
117,0,110,0,99,
0,68,0,101,0,
99,0,108,0,1,
-1,1,5,1393,20,
895,1,117,1,3,
1,4,1,3,1394,
22,1,23,1,20,
1192,1,776,1395,16,
0,183,1,133,1198,
1,774,1396,17,1397,
15,1398,4,22,37,
0,65,0,115,0,
115,0,105,0,103,
0,110,0,109,0,
101,0,110,0,116,
0,1,-1,1,5,
1399,20,927,1,101,
1,3,1,4,1,
3,1400,22,1,7,
1,333,1239,1,130,
1210,1,129,1214,1,
128,1218,1,662,1401,
17,1402,15,1403,4,
14,37,0,82,0,
101,0,112,0,101,
0,97,0,116,0,
1,-1,1,5,1404,
20,915,1,107,1,
3,1,5,1,4,
1405,22,1,13,1,
447,1406,17,1407,15,
1408,4,28,37,0,
76,0,111,0,99,
0,97,0,108,0,
70,0,117,0,110,
0,99,0,68,0,
101,0,99,0,108,
0,1,-1,1,5,
1409,20,893,1,118,
1,3,1,5,1,
4,1410,22,1,24,
1,12,1224,1,1053,
1148,1,931,1411,17,
1412,15,1413,4,8,
37,0,83,0,73,
0,102,0,1,-1,
1,5,1414,20,913,
1,108,1,3,1,
6,1,5,1415,22,
1,14,1,443,1416,
17,1417,15,1418,4,
20,37,0,76,0,
111,0,99,0,97,
0,108,0,73,0,
110,0,105,0,116,
0,1,-1,1,5,
1419,20,873,1,120,
1,3,1,4,1,
3,1420,22,1,26,
1,14,1421,16,0,
183,1,441,1422,17,
1423,15,1424,4,10,
37,0,105,0,110,
0,105,0,116,0,
1,-1,1,5,1425,
20,814,1,132,1,
3,1,3,1,2,
1426,22,1,38,1,
868,1427,16,0,183,
1,10,1243,1,9,
1247,1,733,1428,17,
1429,15,1430,4,12,
37,0,87,0,104,
0,105,0,108,0,
101,0,1,-1,1,
5,1431,20,919,1,
105,1,3,1,6,
1,5,1432,22,1,
11,1,109,1433,17,
1434,15,1132,1,-1,
1,5,116,1,2,
1,2,1435,22,1,
50,1,2,1251,1,
861,1436,16,0,183,
1,432,1437,17,1438,
15,1335,1,-1,1,
5,1439,20,812,1,
133,1,3,1,4,
1,3,1440,22,1,
39,1,3,1254,1,
751,1441,17,1442,15,
1443,4,6,37,0,
68,0,111,0,1,
-1,1,5,1444,20,
923,1,103,1,3,
1,4,1,3,1445,
22,1,9,1,1,
1446,17,1447,15,1448,
4,26,37,0,70,
0,117,0,110,0,
99,0,116,0,105,
0,111,0,110,0,
67,0,97,0,108,
0,108,0,1,-1,
1,5,1449,20,925,
1,102,1,3,1,
2,1,1,1450,22,
1,8,1,0,1451,
16,0,183,1,48,
1452,19,318,1,48,
1453,5,73,1,748,
1312,1,961,1454,16,
0,316,1,92,1143,
1,637,1455,16,0,
316,1,635,1319,1,
97,1130,1,96,1134,
1,95,1139,1,734,
1456,16,0,316,1,
305,1125,1,1020,1457,
16,0,316,1,303,
1153,1,730,1326,1,
1050,1458,16,0,316,
1,611,1459,16,0,
316,1,717,1460,16,
0,316,1,395,1333,
1,971,1338,1,1029,
1343,1,172,1164,1,
560,1347,1,559,1351,
1,382,1355,1,930,
1360,1,380,1365,1,
378,1370,1,15,1234,
1,1051,1157,1,44,
1173,1,581,1374,1,
17,1229,1,623,1461,
16,0,316,1,792,
1462,16,0,316,1,
558,1381,1,19,1223,
1,147,1385,1,27,
1193,1,132,1202,1,
131,1206,1,127,1179,
1,33,1183,1,548,
1463,16,0,316,1,
459,1389,1,351,1187,
1,457,1390,1,20,
1192,1,776,1464,16,
0,316,1,133,1198,
1,774,1396,1,333,
1239,1,130,1210,1,
129,1214,1,128,1218,
1,662,1401,1,447,
1406,1,12,1224,1,
1053,1148,1,931,1411,
1,443,1416,1,14,
1465,16,0,316,1,
441,1422,1,868,1466,
16,0,316,1,10,
1243,1,9,1247,1,
733,1428,1,109,1433,
1,2,1251,1,861,
1467,16,0,316,1,
432,1437,1,3,1254,
1,751,1441,1,1,
1446,1,0,1468,16,
0,316,1,47,1469,
19,261,1,47,1470,
5,76,1,748,1312,
1,961,1471,17,1472,
15,1473,4,12,37,
0,98,0,108,0,
111,0,99,0,107,
0,1,-1,1,5,
1474,20,929,1,100,
1,3,1,1,1,
0,1475,22,1,6,
1,92,1143,1,305,
1125,1,635,1319,1,
717,1476,17,1472,1,
0,1475,1,97,1130,
1,96,1134,1,95,
1139,1,734,1477,17,
1472,1,0,1475,1,
733,1428,1,1020,1478,
17,1472,1,0,1475,
1,303,1153,1,730,
1326,1,1050,1479,17,
1472,1,0,1475,1,
559,1351,1,611,1480,
17,1472,1,0,1475,
1,931,1411,1,395,
1333,1,380,1365,1,
548,1481,17,1472,1,
0,1475,1,1029,1343,
1,172,1164,1,560,
1347,1,811,1482,17,
1483,15,1484,4,12,
37,0,99,0,104,
0,117,0,110,0,
107,0,1,-1,1,
5,1485,20,964,1,
97,1,3,1,3,
1,2,1486,22,1,
3,1,382,1355,1,
930,1360,1,808,1487,
17,1488,15,1484,1,
-1,1,5,1489,20,
946,1,98,1,3,
1,4,1,3,1490,
22,1,4,1,378,
1370,1,15,1234,1,
1053,1148,1,1051,1157,
1,44,1173,1,581,
1374,1,17,1229,1,
623,1491,17,1472,1,
0,1475,1,792,1492,
17,1493,15,1484,1,
-1,1,5,1494,20,
966,1,96,1,3,
1,3,1,2,1495,
22,1,2,1,558,
1381,1,19,1223,1,
147,1385,1,27,1193,
1,132,1202,1,131,
1206,1,127,1179,1,
33,1183,1,133,1198,
1,459,1389,1,351,
1187,1,457,1390,1,
20,1192,1,776,1496,
17,1497,15,1484,1,
-1,1,5,1498,20,
968,1,95,1,3,
1,2,1,1,1499,
22,1,1,1,775,
1500,17,1501,15,1473,
1,-1,1,5,1502,
20,944,1,99,1,
3,1,2,1,1,
1503,22,1,5,1,
774,1396,1,333,1239,
1,130,1210,1,129,
1214,1,128,1218,1,
662,1401,1,447,1406,
1,12,1224,1,109,
1433,1,637,1504,17,
1472,1,0,1475,1,
443,1416,1,14,1505,
17,1472,1,0,1475,
1,441,1422,1,868,
1506,17,1472,1,0,
1475,1,10,1243,1,
9,1247,1,971,1338,
1,649,1507,16,0,
259,1,2,1251,1,
861,1508,17,1472,1,
0,1475,1,432,1437,
1,3,1254,1,751,
1441,1,1,1446,1,
46,1509,19,288,1,
46,1510,5,73,1,
748,1312,1,961,1511,
16,0,286,1,92,
1143,1,637,1512,16,
0,286,1,635,1319,
1,97,1130,1,96,
1134,1,95,1139,1,
734,1513,16,0,286,
1,305,1125,1,1020,
1514,16,0,286,1,
303,1153,1,730,1326,
1,1050,1515,16,0,
286,1,611,1516,16,
0,286,1,717,1517,
16,0,286,1,395,
1333,1,971,1338,1,
1029,1343,1,172,1164,
1,560,1347,1,559,
1351,1,382,1355,1,
930,1360,1,380,1365,
1,378,1370,1,15,
1234,1,1051,1157,1,
44,1173,1,581,1374,
1,17,1229,1,623,
1518,16,0,286,1,
792,1519,16,0,286,
1,558,1381,1,19,
1223,1,147,1385,1,
27,1193,1,132,1202,
1,131,1206,1,127,
1179,1,33,1183,1,
548,1520,16,0,286,
1,459,1389,1,351,
1187,1,457,1390,1,
20,1192,1,776,1521,
16,0,286,1,133,
1198,1,774,1396,1,
333,1239,1,130,1210,
1,129,1214,1,128,
1218,1,662,1401,1,
447,1406,1,12,1224,
1,1053,1148,1,931,
1411,1,443,1416,1,
14,1522,16,0,286,
1,441,1422,1,868,
1523,16,0,286,1,
10,1243,1,9,1247,
1,733,1428,1,109,
1433,1,2,1251,1,
861,1524,16,0,286,
1,432,1437,1,3,
1254,1,751,1441,1,
1,1446,1,0,1525,
16,0,286,1,45,
1526,19,160,1,45,
1527,5,116,1,210,
1528,16,0,414,1,
461,1529,16,0,414,
1,459,1389,1,457,
1390,1,931,1411,1,
213,1530,16,0,414,
1,688,1531,16,0,
414,1,447,1406,1,
443,1416,1,441,1422,
1,1051,1157,1,432,
1437,1,662,1401,1,
173,1532,16,0,414,
1,172,1164,1,635,
1319,1,412,1533,16,
0,414,1,650,1534,
16,0,414,1,887,
1535,16,0,414,1,
1020,1536,16,0,158,
1,1050,1537,16,0,
158,1,637,1538,16,
0,158,1,383,1539,
16,0,414,1,395,
1333,1,611,1540,16,
0,158,1,868,1541,
16,0,158,1,147,
1385,1,623,1542,16,
0,158,1,861,1543,
16,0,158,1,382,
1355,1,380,1365,1,
378,1370,1,377,1544,
16,0,163,1,133,
1198,1,132,1202,1,
131,1206,1,130,1210,
1,129,1214,1,128,
1218,1,127,1179,1,
832,1545,16,0,414,
1,351,1187,1,109,
1433,1,582,1546,16,
0,414,1,581,1374,
1,97,1130,1,559,
1351,1,558,1381,1,
98,1547,16,0,414,
1,1053,1148,1,96,
1134,1,95,1139,1,
333,1239,1,92,1143,
1,560,1548,16,0,
414,1,309,1549,16,
0,414,1,308,1038,
1,792,1550,16,0,
158,1,1029,1343,1,
548,1551,16,0,158,
1,69,1552,16,0,
414,1,307,1042,1,
305,1125,1,59,1062,
1,303,1153,1,57,
1068,1,62,1553,16,
0,414,1,61,1056,
1,60,1059,1,776,
1554,16,0,158,1,
58,1065,1,774,1396,
1,56,1071,1,55,
1074,1,54,1077,1,
53,1080,1,52,1083,
1,51,1086,1,50,
1089,1,49,1092,1,
48,1095,1,47,1049,
1,45,1555,16,0,
414,1,44,1173,1,
998,1556,16,0,414,
1,519,1557,16,0,
414,1,961,1558,16,
0,158,1,753,1559,
16,0,414,1,751,
1441,1,33,1183,1,
748,1312,1,28,1560,
16,0,414,1,27,
1193,1,15,1234,1,
17,1229,1,20,1192,
1,19,1223,1,12,
1224,1,734,1561,16,
0,158,1,733,1428,
1,971,1338,1,14,
1562,16,0,158,1,
730,1326,1,490,1563,
16,0,414,1,5,
1116,1,10,1243,1,
9,1247,1,0,1564,
16,0,158,1,7,
1109,1,6,1113,1,
930,1360,1,4,1565,
16,0,414,1,3,
1254,1,2,1251,1,
1,1446,1,717,1566,
16,0,158,1,44,
1567,19,267,1,44,
1568,5,43,1,213,
1569,16,0,265,1,
412,1570,16,0,265,
1,210,1571,16,0,
265,1,98,1572,16,
0,265,1,309,1573,
16,0,265,1,308,
1038,1,307,1042,1,
519,1574,16,0,265,
1,832,1575,16,0,
265,1,490,1576,16,
0,265,1,69,1577,
16,0,265,1,47,
1049,1,173,1578,16,
0,265,1,62,1579,
16,0,265,1,383,
1580,16,0,265,1,
61,1056,1,60,1059,
1,59,1062,1,58,
1065,1,57,1068,1,
56,1071,1,55,1074,
1,54,1077,1,53,
1080,1,52,1083,1,
51,1086,1,50,1089,
1,49,1092,1,48,
1095,1,582,1581,16,
0,265,1,688,1582,
16,0,265,1,45,
1583,16,0,265,1,
998,1584,16,0,265,
1,461,1585,16,0,
265,1,887,1586,16,
0,265,1,28,1587,
16,0,265,1,560,
1588,16,0,265,1,
4,1589,16,0,265,
1,650,1590,16,0,
265,1,7,1109,1,
6,1113,1,5,1116,
1,753,1591,16,0,
265,1,43,1592,19,
392,1,43,1593,5,
73,1,748,1312,1,
961,1594,16,0,390,
1,92,1143,1,637,
1595,16,0,390,1,
635,1319,1,97,1130,
1,96,1134,1,95,
1139,1,734,1596,16,
0,390,1,305,1125,
1,1020,1597,16,0,
390,1,303,1153,1,
730,1326,1,1050,1598,
16,0,390,1,611,
1599,16,0,390,1,
717,1600,16,0,390,
1,395,1333,1,971,
1338,1,1029,1343,1,
172,1164,1,560,1347,
1,559,1351,1,382,
1355,1,930,1360,1,
380,1365,1,378,1370,
1,15,1234,1,1051,
1157,1,44,1173,1,
581,1374,1,17,1229,
1,623,1601,16,0,
390,1,792,1602,16,
0,390,1,558,1381,
1,19,1223,1,147,
1385,1,27,1193,1,
132,1202,1,131,1206,
1,127,1179,1,33,
1183,1,548,1603,16,
0,390,1,459,1389,
1,351,1187,1,457,
1390,1,20,1192,1,
776,1604,16,0,390,
1,133,1198,1,774,
1396,1,333,1239,1,
130,1210,1,129,1214,
1,128,1218,1,662,
1401,1,447,1406,1,
12,1224,1,1053,1148,
1,931,1411,1,443,
1416,1,14,1605,16,
0,390,1,441,1422,
1,868,1606,16,0,
390,1,10,1243,1,
9,1247,1,733,1428,
1,109,1433,1,2,
1251,1,861,1607,16,
0,390,1,432,1437,
1,3,1254,1,751,
1441,1,1,1446,1,
0,1608,16,0,390,
1,42,1609,19,127,
1,42,1610,5,4,
1,459,1389,1,997,
1611,16,0,125,1,
380,1365,1,378,1370,
1,41,1612,19,120,
1,41,1613,5,77,
1,748,1312,1,961,
1614,16,0,244,1,
92,1143,1,531,1615,
16,0,330,1,637,
1616,16,0,244,1,
635,1319,1,97,1130,
1,96,1134,1,95,
1139,1,734,1617,16,
0,244,1,305,1125,
1,1020,1618,16,0,
244,1,303,1153,1,
395,1333,1,1050,1619,
16,0,244,1,581,
1374,1,611,1620,16,
0,244,1,717,1621,
16,0,244,1,502,
1622,16,0,168,1,
1029,1343,1,172,1164,
1,560,1347,1,559,
1351,1,382,1355,1,
930,1360,1,380,1365,
1,700,1623,16,0,
251,1,378,1370,1,
1019,1624,16,0,118,
1,15,1234,1,17,
1229,1,1051,1157,1,
44,1173,1,147,1385,
1,19,1223,1,623,
1625,16,0,244,1,
792,1626,16,0,244,
1,558,1381,1,868,
1627,16,0,244,1,
971,1338,1,27,1193,
1,132,1202,1,33,
1183,1,127,1179,1,
861,1628,16,0,244,
1,548,1629,16,0,
244,1,459,1389,1,
351,1187,1,457,1390,
1,20,1192,1,776,
1630,16,0,244,1,
133,1198,1,774,1396,
1,131,1206,1,130,
1210,1,129,1214,1,
128,1218,1,662,1401,
1,447,1406,1,12,
1224,1,1053,1148,1,
931,1411,1,443,1416,
1,14,1631,16,0,
244,1,441,1422,1,
333,1239,1,10,1243,
1,9,1247,1,733,
1428,1,109,1433,1,
2,1251,1,730,1326,
1,432,1437,1,3,
1254,1,751,1441,1,
1,1446,1,0,1632,
16,0,244,1,40,
1633,19,144,1,40,
1634,5,73,1,748,
1312,1,961,1635,16,
0,142,1,92,1143,
1,637,1636,16,0,
142,1,635,1319,1,
97,1130,1,96,1134,
1,95,1139,1,734,
1637,16,0,142,1,
305,1125,1,1020,1638,
16,0,142,1,303,
1153,1,730,1326,1,
1050,1639,16,0,142,
1,611,1640,16,0,
142,1,717,1641,16,
0,142,1,395,1333,
1,971,1338,1,1029,
1343,1,172,1164,1,
560,1347,1,559,1351,
1,382,1355,1,930,
1360,1,380,1365,1,
378,1370,1,15,1234,
1,1051,1157,1,44,
1173,1,581,1374,1,
17,1229,1,623,1642,
16,0,142,1,792,
1643,16,0,142,1,
558,1381,1,19,1223,
1,147,1385,1,27,
1193,1,132,1202,1,
131,1206,1,127,1179,
1,33,1183,1,548,
1644,16,0,142,1,
459,1389,1,351,1187,
1,457,1390,1,20,
1192,1,776,1645,16,
0,142,1,133,1198,
1,774,1396,1,333,
1239,1,130,1210,1,
129,1214,1,128,1218,
1,662,1401,1,447,
1406,1,12,1224,1,
1053,1148,1,931,1411,
1,443,1416,1,14,
1646,16,0,142,1,
441,1422,1,868,1647,
16,0,142,1,10,
1243,1,9,1247,1,
733,1428,1,109,1433,
1,2,1251,1,861,
1648,16,0,142,1,
432,1437,1,3,1254,
1,751,1441,1,1,
1446,1,0,1649,16,
0,142,1,39,1650,
19,258,1,39,1651,
5,73,1,748,1312,
1,961,1652,16,0,
256,1,92,1143,1,
637,1653,16,0,256,
1,635,1319,1,97,
1130,1,96,1134,1,
95,1139,1,734,1654,
16,0,256,1,305,
1125,1,1020,1655,16,
0,256,1,303,1153,
1,730,1326,1,1050,
1656,16,0,256,1,
611,1657,16,0,256,
1,717,1658,16,0,
256,1,395,1333,1,
971,1338,1,1029,1343,
1,172,1164,1,560,
1347,1,559,1351,1,
382,1355,1,930,1360,
1,380,1365,1,378,
1370,1,15,1234,1,
1051,1157,1,44,1173,
1,581,1374,1,17,
1229,1,623,1659,16,
0,256,1,792,1660,
16,0,256,1,558,
1381,1,19,1223,1,
147,1385,1,27,1193,
1,132,1202,1,131,
1206,1,127,1179,1,
33,1183,1,548,1661,
16,0,256,1,459,
1389,1,351,1187,1,
457,1390,1,20,1192,
1,776,1662,16,0,
256,1,133,1198,1,
774,1396,1,333,1239,
1,130,1210,1,129,
1214,1,128,1218,1,
662,1401,1,447,1406,
1,12,1224,1,1053,
1148,1,931,1411,1,
443,1416,1,14,1663,
16,0,256,1,441,
1422,1,868,1664,16,
0,256,1,10,1243,
1,9,1247,1,733,
1428,1,109,1433,1,
2,1251,1,861,1665,
16,0,256,1,432,
1437,1,3,1254,1,
751,1441,1,1,1446,
1,0,1666,16,0,
256,1,38,1667,19,
111,1,38,1668,5,
88,1,459,1389,1,
457,1390,1,931,1411,
1,930,1360,1,929,
1669,16,0,172,1,
447,1406,1,443,1416,
1,441,1422,1,432,
1437,1,908,1670,17,
1671,15,1672,4,14,
37,0,69,0,108,
0,115,0,101,0,
73,0,102,0,1,
-1,1,5,1673,20,
871,1,121,1,3,
1,6,1,5,1674,
22,1,27,1,662,
1401,1,635,1319,1,
172,1164,1,395,1333,
1,622,1675,16,0,
171,1,1020,1478,1,
637,1504,1,874,1676,
17,1677,15,1678,4,
10,37,0,69,0,
108,0,115,0,101,
0,1,-1,1,5,
1679,20,869,1,122,
1,3,1,6,1,
5,1680,22,1,28,
1,634,1681,16,0,
289,1,611,1480,1,
868,1506,1,867,1682,
17,1683,15,1684,4,
6,37,0,73,0,
102,0,1,-1,1,
5,1685,20,864,1,
123,1,3,1,4,
1,3,1686,22,1,
29,1,147,1385,1,
623,1491,1,861,1508,
1,382,1355,1,380,
1365,1,378,1370,1,
128,1218,1,133,1198,
1,132,1202,1,131,
1206,1,130,1210,1,
129,1214,1,1050,1687,
16,0,385,1,127,
1179,1,811,1482,1,
351,1187,1,109,1433,
1,581,1374,1,97,
1130,1,96,1134,1,
95,1139,1,1053,1148,
1,1052,1688,16,0,
314,1,1051,1157,1,
333,1239,1,92,1143,
1,808,1487,1,560,
1347,1,559,1351,1,
558,1381,1,557,1689,
16,0,393,1,792,
1492,1,1029,1343,1,
1028,1690,16,0,109,
1,548,1481,1,305,
1125,1,303,1153,1,
776,1496,1,775,1500,
1,774,1396,1,44,
1173,1,732,1691,16,
0,245,1,33,1183,
1,751,1441,1,750,
1692,16,0,237,1,
748,1312,1,27,1193,
1,15,1234,1,14,
1693,16,0,410,1,
17,1229,1,20,1192,
1,19,1223,1,16,
1694,16,0,408,1,
734,1695,16,0,239,
1,733,1428,1,971,
1338,1,970,1696,16,
0,156,1,730,1326,
1,12,1224,1,10,
1243,1,9,1247,1,
961,1471,1,3,1254,
1,2,1251,1,1,
1446,1,717,1697,16,
0,250,1,37,1698,
19,191,1,37,1699,
5,77,1,748,1312,
1,961,1471,1,92,
1143,1,305,1125,1,
635,1319,1,717,1476,
1,97,1130,1,96,
1134,1,95,1139,1,
734,1477,1,733,1428,
1,1020,1478,1,303,
1153,1,730,1326,1,
622,1700,16,0,211,
1,808,1487,1,559,
1351,1,611,1480,1,
931,1411,1,395,1333,
1,1029,1343,1,172,
1164,1,560,1347,1,
811,1482,1,382,1355,
1,930,1360,1,380,
1365,1,378,1370,1,
15,1234,1,548,1481,
1,1051,1157,1,44,
1173,1,581,1374,1,
17,1229,1,623,1491,
1,792,1492,1,558,
1381,1,19,1223,1,
147,1385,1,27,1193,
1,132,1202,1,131,
1206,1,127,1179,1,
12,1224,1,33,1183,
1,133,1198,1,459,
1389,1,351,1187,1,
457,1390,1,20,1192,
1,776,1496,1,775,
1500,1,774,1396,1,
333,1239,1,130,1210,
1,129,1214,1,128,
1218,1,662,1401,1,
447,1406,1,14,1505,
1,1053,1148,1,637,
1504,1,443,1416,1,
1050,1479,1,441,1422,
1,868,1506,1,867,
1701,16,0,189,1,
10,1243,1,9,1247,
1,971,1338,1,109,
1433,1,2,1251,1,
861,1508,1,432,1437,
1,3,1254,1,751,
1441,1,1,1446,1,
36,1702,19,198,1,
36,1703,5,77,1,
748,1312,1,961,1471,
1,92,1143,1,305,
1125,1,635,1319,1,
717,1476,1,97,1130,
1,96,1134,1,95,
1139,1,734,1477,1,
733,1428,1,1020,1478,
1,303,1153,1,730,
1326,1,622,1704,16,
0,292,1,808,1487,
1,559,1351,1,611,
1480,1,931,1411,1,
395,1333,1,1029,1343,
1,172,1164,1,560,
1347,1,811,1482,1,
382,1355,1,930,1360,
1,380,1365,1,378,
1370,1,15,1234,1,
548,1481,1,1051,1157,
1,44,1173,1,581,
1374,1,17,1229,1,
623,1491,1,792,1492,
1,558,1381,1,19,
1223,1,147,1385,1,
27,1193,1,132,1202,
1,131,1206,1,127,
1179,1,12,1224,1,
33,1183,1,133,1198,
1,459,1389,1,351,
1187,1,457,1390,1,
20,1192,1,776,1496,
1,775,1500,1,774,
1396,1,333,1239,1,
130,1210,1,129,1214,
1,128,1218,1,662,
1401,1,447,1406,1,
14,1505,1,1053,1148,
1,637,1504,1,443,
1416,1,1050,1479,1,
441,1422,1,868,1506,
1,867,1705,16,0,
196,1,10,1243,1,
9,1247,1,971,1338,
1,109,1433,1,2,
1251,1,861,1508,1,
432,1437,1,3,1254,
1,751,1441,1,1,
1446,1,35,1706,19,
202,1,35,1707,5,
34,1,97,1130,1,
96,1134,1,95,1139,
1,92,1143,1,305,
1125,1,1053,1148,1,
303,1153,1,1051,1157,
1,172,1164,1,594,
1708,16,0,309,1,
44,1173,1,147,1385,
1,33,1183,1,132,
1202,1,351,1187,1,
27,1193,1,133,1198,
1,127,1179,1,131,
1206,1,130,1210,1,
129,1214,1,128,1218,
1,20,1192,1,19,
1223,1,17,1229,1,
844,1709,16,0,200,
1,15,1234,1,12,
1224,1,333,1239,1,
10,1243,1,9,1247,
1,2,1251,1,3,
1254,1,109,1433,1,
34,1710,19,312,1,
34,1711,5,73,1,
748,1312,1,961,1712,
16,0,310,1,92,
1143,1,637,1713,16,
0,310,1,635,1319,
1,97,1130,1,96,
1134,1,95,1139,1,
734,1714,16,0,310,
1,305,1125,1,1020,
1715,16,0,310,1,
303,1153,1,730,1326,
1,1050,1716,16,0,
310,1,611,1717,16,
0,310,1,717,1718,
16,0,310,1,395,
1333,1,971,1338,1,
1029,1343,1,172,1164,
1,560,1347,1,559,
1351,1,382,1355,1,
930,1360,1,380,1365,
1,378,1370,1,15,
1234,1,1051,1157,1,
44,1173,1,581,1374,
1,17,1229,1,623,
1719,16,0,310,1,
792,1720,16,0,310,
1,558,1381,1,19,
1223,1,147,1385,1,
27,1193,1,132,1202,
1,131,1206,1,127,
1179,1,33,1183,1,
548,1721,16,0,310,
1,459,1389,1,351,
1187,1,457,1390,1,
20,1192,1,776,1722,
16,0,310,1,133,
1198,1,774,1396,1,
333,1239,1,130,1210,
1,129,1214,1,128,
1218,1,662,1401,1,
447,1406,1,12,1224,
1,1053,1148,1,931,
1411,1,443,1416,1,
14,1723,16,0,310,
1,441,1422,1,868,
1724,16,0,310,1,
10,1243,1,9,1247,
1,733,1428,1,109,
1433,1,2,1251,1,
861,1725,16,0,310,
1,432,1437,1,3,
1254,1,751,1441,1,
1,1446,1,0,1726,
16,0,310,1,33,
1727,19,140,1,33,
1728,5,12,1,459,
1729,16,0,138,1,
20,1730,17,1731,15,
1732,4,16,37,0,
118,0,97,0,114,
0,108,0,105,0,
115,0,116,0,1,
-1,1,5,1733,20,
666,1,153,1,3,
1,2,1,1,1734,
22,1,60,1,19,
1223,1,752,1735,16,
0,235,1,382,1736,
16,0,178,1,380,
1365,1,172,1737,16,
0,254,1,378,1370,
1,27,1193,1,351,
1187,1,212,1738,16,
0,240,1,372,1739,
17,1740,15,1732,1,
-1,1,5,1741,20,
685,1,152,1,3,
1,4,1,3,1742,
22,1,59,1,32,
1743,19,421,1,32,
1744,5,43,1,213,
1745,16,0,419,1,
412,1746,16,0,419,
1,210,1747,16,0,
419,1,98,1748,16,
0,419,1,309,1749,
16,0,419,1,308,
1038,1,307,1042,1,
519,1750,16,0,419,
1,832,1751,16,0,
419,1,490,1752,16,
0,419,1,69,1753,
16,0,419,1,47,
1049,1,173,1754,16,
0,419,1,62,1755,
16,0,419,1,383,
1756,16,0,419,1,
61,1056,1,60,1059,
1,59,1062,1,58,
1065,1,57,1068,1,
56,1071,1,55,1074,
1,54,1077,1,53,
1080,1,52,1083,1,
51,1086,1,50,1089,
1,49,1092,1,48,
1095,1,582,1757,16,
0,419,1,688,1758,
16,0,419,1,45,
1759,16,0,419,1,
998,1760,16,0,419,
1,461,1761,16,0,
419,1,887,1762,16,
0,419,1,28,1763,
16,0,419,1,560,
1764,16,0,419,1,
4,1765,16,0,419,
1,650,1766,16,0,
419,1,7,1109,1,
6,1113,1,5,1116,
1,753,1767,16,0,
419,1,31,1768,19,
376,1,31,1769,5,
46,1,211,1770,16,
0,374,1,531,1771,
16,0,374,1,305,
1125,1,97,1130,1,
96,1134,1,95,1139,
1,92,1143,1,502,
1772,16,0,374,1,
1053,1148,1,303,1153,
1,1051,1157,1,76,
1773,16,0,374,1,
395,1774,16,0,374,
1,174,1775,16,0,
374,1,172,1164,1,
594,1776,16,0,374,
1,700,1777,16,0,
374,1,46,1778,16,
0,374,1,473,1779,
16,0,374,1,44,
1173,1,147,1780,16,
0,374,1,127,1179,
1,33,1183,1,351,
1187,1,20,1192,1,
27,1193,1,133,1198,
1,132,1202,1,131,
1206,1,130,1210,1,
129,1214,1,128,1218,
1,662,1781,16,0,
374,1,19,1223,1,
12,1224,1,17,1229,
1,844,1782,16,0,
374,1,15,1234,1,
335,1783,16,0,374,
1,333,1239,1,10,
1243,1,9,1247,1,
2,1251,1,3,1254,
1,109,1784,16,0,
374,1,214,1785,16,
0,374,1,30,1786,
19,373,1,30,1787,
5,46,1,211,1788,
16,0,371,1,531,
1789,16,0,371,1,
305,1125,1,97,1130,
1,96,1134,1,95,
1139,1,92,1143,1,
502,1790,16,0,371,
1,1053,1148,1,303,
1153,1,1051,1157,1,
76,1791,16,0,371,
1,395,1792,16,0,
371,1,174,1793,16,
0,371,1,172,1164,
1,594,1794,16,0,
371,1,700,1795,16,
0,371,1,46,1796,
16,0,371,1,473,
1797,16,0,371,1,
44,1173,1,147,1798,
16,0,371,1,127,
1179,1,33,1183,1,
351,1187,1,20,1192,
1,27,1193,1,133,
1198,1,132,1202,1,
131,1206,1,130,1210,
1,129,1214,1,128,
1218,1,662,1799,16,
0,371,1,19,1223,
1,12,1224,1,17,
1229,1,844,1800,16,
0,371,1,15,1234,
1,335,1801,16,0,
371,1,333,1239,1,
10,1243,1,9,1247,
1,2,1251,1,3,
1254,1,109,1802,16,
0,371,1,214,1803,
16,0,371,1,29,
1804,19,364,1,29,
1805,5,46,1,211,
1806,16,0,362,1,
531,1807,16,0,362,
1,305,1125,1,97,
1130,1,96,1134,1,
95,1139,1,92,1143,
1,502,1808,16,0,
362,1,1053,1148,1,
303,1153,1,1051,1157,
1,76,1809,16,0,
362,1,395,1810,16,
0,362,1,174,1811,
16,0,362,1,172,
1164,1,594,1812,16,
0,362,1,700,1813,
16,0,362,1,46,
1814,16,0,362,1,
473,1815,16,0,362,
1,44,1173,1,147,
1816,16,0,362,1,
127,1179,1,33,1183,
1,351,1187,1,20,
1192,1,27,1193,1,
133,1198,1,132,1202,
1,131,1206,1,130,
1210,1,129,1214,1,
128,1218,1,662,1817,
16,0,362,1,19,
1223,1,12,1224,1,
17,1229,1,844,1818,
16,0,362,1,15,
1234,1,335,1819,16,
0,362,1,333,1239,
1,10,1243,1,9,
1247,1,2,1251,1,
3,1254,1,109,1820,
16,0,362,1,214,
1821,16,0,362,1,
28,1822,19,361,1,
28,1823,5,46,1,
211,1824,16,0,359,
1,531,1825,16,0,
359,1,305,1125,1,
97,1130,1,96,1134,
1,95,1139,1,92,
1143,1,502,1826,16,
0,359,1,1053,1148,
1,303,1153,1,1051,
1157,1,76,1827,16,
0,359,1,395,1828,
16,0,359,1,174,
1829,16,0,359,1,
172,1164,1,594,1830,
16,0,359,1,700,
1831,16,0,359,1,
46,1832,16,0,359,
1,473,1833,16,0,
359,1,44,1173,1,
147,1834,16,0,359,
1,127,1179,1,33,
1183,1,351,1187,1,
20,1192,1,27,1193,
1,133,1198,1,132,
1202,1,131,1206,1,
130,1210,1,129,1214,
1,128,1218,1,662,
1835,16,0,359,1,
19,1223,1,12,1224,
1,17,1229,1,844,
1836,16,0,359,1,
15,1234,1,335,1837,
16,0,359,1,333,
1239,1,10,1243,1,
9,1247,1,2,1251,
1,3,1254,1,109,
1838,16,0,359,1,
214,1839,16,0,359,
1,27,1840,19,367,
1,27,1841,5,46,
1,211,1842,16,0,
365,1,531,1843,16,
0,365,1,305,1125,
1,97,1130,1,96,
1134,1,95,1139,1,
92,1143,1,502,1844,
16,0,365,1,1053,
1148,1,303,1153,1,
1051,1157,1,76,1845,
16,0,365,1,395,
1846,16,0,365,1,
174,1847,16,0,365,
1,172,1164,1,594,
1848,16,0,365,1,
700,1849,16,0,365,
1,46,1850,16,0,
365,1,473,1851,16,
0,365,1,44,1173,
1,147,1852,16,0,
365,1,127,1179,1,
33,1183,1,351,1187,
1,20,1192,1,27,
1193,1,133,1198,1,
132,1202,1,131,1206,
1,130,1210,1,129,
1214,1,128,1218,1,
662,1853,16,0,365,
1,19,1223,1,12,
1224,1,17,1229,1,
844,1854,16,0,365,
1,15,1234,1,335,
1855,16,0,365,1,
333,1239,1,10,1243,
1,9,1247,1,2,
1251,1,3,1254,1,
109,1856,16,0,365,
1,214,1857,16,0,
365,1,26,1858,19,
358,1,26,1859,5,
46,1,211,1860,16,
0,356,1,531,1861,
16,0,356,1,305,
1125,1,97,1130,1,
96,1134,1,95,1139,
1,92,1143,1,502,
1862,16,0,356,1,
1053,1148,1,303,1153,
1,1051,1157,1,76,
1863,16,0,356,1,
395,1864,16,0,356,
1,174,1865,16,0,
356,1,172,1164,1,
594,1866,16,0,356,
1,700,1867,16,0,
356,1,46,1868,16,
0,356,1,473,1869,
16,0,356,1,44,
1173,1,147,1870,16,
0,356,1,127,1179,
1,33,1183,1,351,
1187,1,20,1192,1,
27,1193,1,133,1198,
1,132,1202,1,131,
1206,1,130,1210,1,
129,1214,1,128,1218,
1,662,1871,16,0,
356,1,19,1223,1,
12,1224,1,17,1229,
1,844,1872,16,0,
356,1,15,1234,1,
335,1873,16,0,356,
1,333,1239,1,10,
1243,1,9,1247,1,
2,1251,1,3,1254,
1,109,1874,16,0,
356,1,214,1875,16,
0,356,1,25,1876,
19,379,1,25,1877,
5,46,1,211,1878,
16,0,377,1,531,
1879,16,0,377,1,
305,1125,1,97,1130,
1,96,1134,1,95,
1139,1,92,1143,1,
502,1880,16,0,377,
1,1053,1148,1,303,
1153,1,1051,1157,1,
76,1881,16,0,377,
1,395,1882,16,0,
377,1,174,1883,16,
0,377,1,172,1164,
1,594,1884,16,0,
377,1,700,1885,16,
0,377,1,46,1886,
16,0,377,1,473,
1887,16,0,377,1,
44,1173,1,147,1888,
16,0,377,1,127,
1179,1,33,1183,1,
351,1187,1,20,1192,
1,27,1193,1,133,
1198,1,132,1202,1,
131,1206,1,130,1210,
1,129,1214,1,128,
1218,1,662,1889,16,
0,377,1,19,1223,
1,12,1224,1,17,
1229,1,844,1890,16,
0,377,1,15,1234,
1,335,1891,16,0,
377,1,333,1239,1,
10,1243,1,9,1247,
1,2,1251,1,3,
1254,1,109,1892,16,
0,377,1,214,1893,
16,0,377,1,24,
1894,19,370,1,24,
1895,5,46,1,211,
1896,16,0,368,1,
531,1897,16,0,368,
1,305,1125,1,97,
1130,1,96,1134,1,
95,1139,1,92,1143,
1,502,1898,16,0,
368,1,1053,1148,1,
303,1153,1,1051,1157,
1,76,1899,16,0,
368,1,395,1900,16,
0,368,1,174,1901,
16,0,368,1,172,
1164,1,594,1902,16,
0,368,1,700,1903,
16,0,368,1,46,
1904,16,0,368,1,
473,1905,16,0,368,
1,44,1173,1,147,
1906,16,0,368,1,
127,1179,1,33,1183,
1,351,1187,1,20,
1192,1,27,1193,1,
133,1198,1,132,1202,
1,131,1206,1,130,
1210,1,129,1214,1,
128,1218,1,662,1907,
16,0,368,1,19,
1223,1,12,1224,1,
17,1229,1,844,1908,
16,0,368,1,15,
1234,1,335,1909,16,
0,368,1,333,1239,
1,10,1243,1,9,
1247,1,2,1251,1,
3,1254,1,109,1910,
16,0,368,1,214,
1911,16,0,368,1,
23,1912,19,352,1,
23,1913,5,46,1,
211,1914,16,0,350,
1,531,1915,16,0,
350,1,305,1125,1,
97,1130,1,96,1134,
1,95,1139,1,92,
1143,1,502,1916,16,
0,350,1,1053,1148,
1,303,1153,1,1051,
1157,1,76,1917,16,
0,350,1,395,1918,
16,0,350,1,174,
1919,16,0,350,1,
172,1164,1,594,1920,
16,0,350,1,700,
1921,16,0,350,1,
46,1922,16,0,350,
1,473,1923,16,0,
350,1,44,1173,1,
147,1924,16,0,350,
1,127,1179,1,33,
1183,1,351,1187,1,
20,1192,1,27,1193,
1,133,1198,1,132,
1202,1,131,1206,1,
130,1210,1,129,1214,
1,128,1218,1,662,
1925,16,0,350,1,
19,1223,1,12,1224,
1,17,1229,1,844,
1926,16,0,350,1,
15,1234,1,335,1927,
16,0,350,1,333,
1239,1,10,1243,1,
9,1247,1,2,1251,
1,3,1254,1,109,
1928,16,0,350,1,
214,1929,16,0,350,
1,22,1930,19,349,
1,22,1931,5,46,
1,211,1932,16,0,
347,1,531,1933,16,
0,347,1,305,1125,
1,97,1130,1,96,
1134,1,95,1139,1,
92,1143,1,502,1934,
16,0,347,1,1053,
1148,1,303,1153,1,
1051,1157,1,76,1935,
16,0,347,1,395,
1936,16,0,347,1,
174,1937,16,0,347,
1,172,1164,1,594,
1938,16,0,347,1,
700,1939,16,0,347,
1,46,1940,16,0,
347,1,473,1941,16,
0,347,1,44,1173,
1,147,1942,16,0,
347,1,127,1179,1,
33,1183,1,351,1187,
1,20,1192,1,27,
1193,1,133,1198,1,
132,1202,1,131,1206,
1,130,1210,1,129,
1214,1,128,1218,1,
662,1943,16,0,347,
1,19,1223,1,12,
1224,1,17,1229,1,
844,1944,16,0,347,
1,15,1234,1,335,
1945,16,0,347,1,
333,1239,1,10,1243,
1,9,1247,1,2,
1251,1,3,1254,1,
109,1946,16,0,347,
1,214,1947,16,0,
347,1,21,1948,19,
346,1,21,1949,5,
46,1,211,1950,16,
0,344,1,531,1951,
16,0,344,1,305,
1125,1,97,1130,1,
96,1134,1,95,1139,
1,92,1143,1,502,
1952,16,0,344,1,
1053,1148,1,303,1153,
1,1051,1157,1,76,
1953,16,0,344,1,
395,1954,16,0,344,
1,174,1955,16,0,
344,1,172,1164,1,
594,1956,16,0,344,
1,700,1957,16,0,
344,1,46,1958,16,
0,344,1,473,1959,
16,0,344,1,44,
1173,1,147,1960,16,
0,344,1,127,1179,
1,33,1183,1,351,
1187,1,20,1192,1,
27,1193,1,133,1198,
1,132,1202,1,131,
1206,1,130,1210,1,
129,1214,1,128,1218,
1,662,1961,16,0,
344,1,19,1223,1,
12,1224,1,17,1229,
1,844,1962,16,0,
344,1,15,1234,1,
335,1963,16,0,344,
1,333,1239,1,10,
1243,1,9,1247,1,
2,1251,1,3,1254,
1,109,1964,16,0,
344,1,214,1965,16,
0,344,1,20,1966,
19,424,1,20,1967,
5,43,1,213,1968,
16,0,422,1,412,
1969,16,0,422,1,
210,1970,16,0,422,
1,98,1971,16,0,
422,1,309,1972,16,
0,422,1,308,1038,
1,307,1042,1,519,
1973,16,0,422,1,
832,1974,16,0,422,
1,490,1975,16,0,
422,1,69,1976,16,
0,422,1,47,1049,
1,173,1977,16,0,
422,1,62,1978,16,
0,422,1,383,1979,
16,0,422,1,61,
1056,1,60,1059,1,
59,1062,1,58,1065,
1,57,1068,1,56,
1071,1,55,1074,1,
54,1077,1,53,1080,
1,52,1083,1,51,
1086,1,50,1089,1,
49,1092,1,48,1095,
1,582,1980,16,0,
422,1,688,1981,16,
0,422,1,45,1982,
16,0,422,1,998,
1983,16,0,422,1,
461,1984,16,0,422,
1,887,1985,16,0,
422,1,28,1986,16,
0,422,1,560,1987,
16,0,422,1,4,
1988,16,0,422,1,
650,1989,16,0,422,
1,7,1109,1,6,
1113,1,5,1116,1,
753,1990,16,0,422,
1,19,1991,19,343,
1,19,1992,5,46,
1,211,1993,16,0,
341,1,531,1994,16,
0,341,1,305,1125,
1,97,1130,1,96,
1134,1,95,1139,1,
92,1143,1,502,1995,
16,0,341,1,1053,
1148,1,303,1153,1,
1051,1157,1,76,1996,
16,0,341,1,395,
1997,16,0,341,1,
174,1998,16,0,341,
1,172,1164,1,594,
1999,16,0,341,1,
700,2000,16,0,341,
1,46,2001,16,0,
341,1,473,2002,16,
0,341,1,44,1173,
1,147,2003,16,0,
341,1,127,1179,1,
33,1183,1,351,1187,
1,20,1192,1,27,
1193,1,133,1198,1,
132,1202,1,131,1206,
1,130,1210,1,129,
1214,1,128,1218,1,
662,2004,16,0,341,
1,19,1223,1,12,
1224,1,17,1229,1,
844,2005,16,0,341,
1,15,1234,1,335,
2006,16,0,341,1,
333,1239,1,10,1243,
1,9,1247,1,2,
1251,1,3,1254,1,
109,2007,16,0,341,
1,214,2008,16,0,
341,1,18,2009,19,
340,1,18,2010,5,
89,1,473,2011,16,
0,338,1,461,2012,
16,0,418,1,210,
2013,16,0,418,1,
214,2014,16,0,338,
1,213,2015,16,0,
418,1,211,2016,16,
0,338,1,688,2017,
16,0,418,1,412,
2018,16,0,418,1,
662,2019,16,0,338,
1,172,1164,1,174,
2020,16,0,338,1,
173,2021,16,0,418,
1,650,2022,16,0,
418,1,887,2023,16,
0,418,1,395,2024,
16,0,338,1,147,
2025,16,0,338,1,
383,2026,16,0,418,
1,127,1179,1,133,
1198,1,132,1202,1,
131,1206,1,130,1210,
1,129,1214,1,128,
1218,1,844,2027,16,
0,338,1,594,2028,
16,0,338,1,832,
2029,16,0,418,1,
351,1187,1,109,2030,
16,0,338,1,582,
2031,16,0,418,1,
97,1130,1,96,1134,
1,95,1139,1,98,
2032,16,0,418,1,
1053,1148,1,335,2033,
16,0,338,1,1051,
1157,1,333,1239,1,
92,1143,1,560,2034,
16,0,418,1,308,
1038,1,76,2035,16,
0,338,1,309,2036,
16,0,418,1,69,
2037,16,0,418,1,
307,1042,1,305,1125,
1,59,1062,1,303,
1153,1,62,2038,16,
0,418,1,61,1056,
1,60,1059,1,56,
1071,1,58,1065,1,
57,1068,1,53,1080,
1,55,1074,1,54,
1077,1,531,2039,16,
0,338,1,52,1083,
1,51,1086,1,50,
1089,1,49,1092,1,
48,1095,1,47,1049,
1,46,2040,16,0,
338,1,45,2041,16,
0,418,1,44,1173,
1,998,2042,16,0,
418,1,519,2043,16,
0,418,1,753,2044,
16,0,418,1,33,
1183,1,28,2045,16,
0,418,1,27,1193,
1,502,2046,16,0,
338,1,20,1192,1,
19,1223,1,17,1229,
1,15,1234,1,12,
1224,1,490,2047,16,
0,418,1,10,1243,
1,9,1247,1,6,
1113,1,7,1109,1,
700,2048,16,0,338,
1,5,1116,1,4,
2049,16,0,418,1,
3,1254,1,2,1251,
1,17,2050,19,337,
1,17,2051,5,46,
1,211,2052,16,0,
335,1,531,2053,16,
0,335,1,305,1125,
1,97,1130,1,96,
1134,1,95,1139,1,
92,1143,1,502,2054,
16,0,335,1,1053,
1148,1,303,1153,1,
1051,1157,1,76,2055,
16,0,335,1,395,
2056,16,0,335,1,
174,2057,16,0,335,
1,172,1164,1,594,
2058,16,0,335,1,
700,2059,16,0,335,
1,46,2060,16,0,
335,1,473,2061,16,
0,335,1,44,1173,
1,147,2062,16,0,
335,1,127,1179,1,
33,1183,1,351,1187,
1,20,1192,1,27,
1193,1,133,1198,1,
132,1202,1,131,1206,
1,130,1210,1,129,
1214,1,128,1218,1,
662,2063,16,0,335,
1,19,1223,1,12,
1224,1,17,1229,1,
844,2064,16,0,335,
1,15,1234,1,335,
2065,16,0,335,1,
333,1239,1,10,1243,
1,9,1247,1,2,
1251,1,3,1254,1,
109,2066,16,0,335,
1,214,2067,16,0,
335,1,16,2068,19,
154,1,16,2069,5,
20,1,92,1143,1,
44,1173,1,33,2070,
16,0,403,1,172,
1164,1,27,1193,1,
449,2071,16,0,152,
1,22,2072,16,0,
403,1,305,1125,1,
351,1187,1,303,1153,
1,20,1192,1,19,
1223,1,10,1243,1,
9,1247,1,2,1251,
1,1,2073,16,0,
403,1,333,1239,1,
3,1254,1,96,1134,
1,95,1139,1,15,
2074,19,224,1,15,
2075,5,42,1,309,
2076,17,2077,15,2078,
4,20,37,0,102,
0,105,0,101,0,
108,0,100,0,108,
0,105,0,115,0,
116,0,1,-1,1,
5,2079,20,854,1,
127,1,3,1,3,
1,2,2080,22,1,
32,1,92,1143,1,
302,2081,16,0,225,
1,97,1130,1,96,
1134,1,95,1139,1,
308,1038,1,307,1042,
1,306,2082,17,2083,
15,2078,1,-1,1,
5,2084,20,862,1,
124,1,3,1,2,
1,1,2085,22,1,
30,1,305,1125,1,
1053,1148,1,303,1153,
1,1051,1157,1,174,
2086,17,2087,15,2088,
4,24,37,0,70,
0,105,0,101,0,
108,0,100,0,65,
0,115,0,115,0,
105,0,103,0,110,
0,1,-1,1,5,
2089,20,565,1,168,
1,3,1,4,1,
3,2090,22,1,97,
1,172,1164,1,46,
2091,17,2092,15,2093,
4,12,37,0,102,
0,105,0,101,0,
108,0,100,0,1,
-1,1,5,2094,20,
560,1,169,1,3,
1,2,1,1,2095,
22,1,98,1,45,
2096,16,0,222,1,
44,1173,1,147,1385,
1,33,1183,1,351,
1187,1,20,1192,1,
27,1193,1,133,1198,
1,132,1202,1,131,
1206,1,130,1210,1,
129,1214,1,128,1218,
1,127,1179,1,19,
1223,1,17,1229,1,
15,1234,1,12,1224,
1,333,1239,1,332,
2097,17,2098,15,2078,
1,-1,1,5,2099,
20,859,1,126,1,
3,1,4,1,3,
2100,22,1,31,1,
10,1243,1,9,1247,
1,2,1251,1,3,
1254,1,109,1433,1,
214,2101,17,2102,15,
2103,4,30,37,0,
70,0,105,0,101,
0,108,0,100,0,
69,0,120,0,112,
0,65,0,115,0,
115,0,105,0,103,
0,110,0,1,-1,
1,5,2104,20,567,
1,167,1,3,1,
6,1,5,2105,22,
1,96,1,14,2106,
19,383,1,14,2107,
5,63,1,213,2108,
16,0,381,1,412,
2109,16,0,381,1,
210,2110,16,0,381,
1,305,1125,1,98,
2111,16,0,381,1,
96,1134,1,95,1139,
1,308,1038,1,307,
1042,1,92,1143,1,
519,2112,16,0,381,
1,303,1153,1,832,
2113,16,0,381,1,
490,2114,16,0,381,
1,172,1164,1,69,
2115,16,0,381,1,
47,1049,1,173,2116,
16,0,381,1,62,
2117,16,0,381,1,
383,2118,16,0,381,
1,61,1056,1,60,
1059,1,59,1062,1,
58,1065,1,57,1068,
1,56,1071,1,55,
1074,1,54,1077,1,
53,1080,1,52,1083,
1,51,1086,1,50,
1089,1,49,1092,1,
48,1095,1,582,2119,
16,0,381,1,688,
2120,16,0,381,1,
45,2121,16,0,381,
1,44,1173,1,40,
2122,16,0,381,1,
33,2123,16,0,381,
1,998,2124,16,0,
381,1,461,2125,16,
0,381,1,887,2126,
16,0,381,1,351,
1187,1,28,2127,16,
0,381,1,27,1193,
1,560,2128,16,0,
381,1,22,2129,16,
0,381,1,20,1192,
1,19,1223,1,4,
2130,16,0,381,1,
10,1243,1,333,1239,
1,309,2131,16,0,
381,1,9,1247,1,
650,2132,16,0,381,
1,7,1109,1,6,
1113,1,5,1116,1,
753,2133,16,0,381,
1,3,1254,1,2,
1251,1,1,2134,16,
0,381,1,13,2135,
19,195,1,13,2136,
5,34,1,211,2137,
16,0,241,1,97,
1130,1,96,1134,1,
95,1139,1,92,1143,
1,305,1125,1,1053,
1148,1,303,1153,1,
1051,1157,1,172,1164,
1,44,1173,1,147,
1385,1,33,1183,1,
133,1198,1,132,1202,
1,351,1187,1,27,
1193,1,335,2138,16,
0,193,1,127,1179,
1,131,1206,1,130,
1210,1,129,1214,1,
128,1218,1,20,1192,
1,19,1223,1,17,
1229,1,15,1234,1,
12,1224,1,333,1239,
1,10,1243,1,9,
1247,1,2,1251,1,
3,1254,1,109,1433,
1,12,2139,19,249,
1,12,2140,5,23,
1,92,1143,1,44,
1173,1,351,1187,1,
33,2141,16,0,401,
1,172,1164,1,27,
1193,1,309,2142,16,
0,247,1,308,1038,
1,307,1042,1,305,
1125,1,22,2143,16,
0,401,1,303,1153,
1,20,1192,1,19,
1223,1,10,1243,1,
9,1247,1,45,2144,
16,0,247,1,2,
1251,1,1,2145,16,
0,401,1,333,1239,
1,3,1254,1,96,
1134,1,95,1139,1,
11,2146,19,308,1,
11,2147,5,42,1,
97,1130,1,96,1134,
1,95,1139,1,92,
1143,1,305,1125,1,
1053,1148,1,303,1153,
1,1051,1157,1,1049,
2148,16,0,386,1,
76,2149,16,0,306,
1,395,1333,1,172,
1164,1,44,1173,1,
147,1385,1,1072,2150,
17,2151,15,2152,4,
16,37,0,112,0,
97,0,114,0,108,
0,105,0,115,0,
116,0,1,-1,1,
5,2153,20,820,1,
130,1,3,1,2,
1,1,2154,22,1,
35,1,33,1183,1,
132,1202,1,351,1187,
1,27,1193,1,133,
1198,1,127,1179,1,
131,1206,1,130,1210,
1,129,1214,1,128,
1218,1,20,1192,1,
19,1223,1,17,1229,
1,2,1251,1,15,
1234,1,12,1224,1,
13,2155,16,0,411,
1,333,1239,1,10,
1243,1,9,1247,1,
8,2156,16,0,416,
1,4,2157,16,0,
415,1,1075,2158,17,
2159,15,2152,1,-1,
1,5,2160,20,818,
1,131,1,3,1,
4,1,3,2161,22,
1,36,1,432,1437,
1,3,1254,1,109,
1433,1,1071,2162,17,
2163,15,2152,1,-1,
1,5,328,1,1,
1,1,2164,22,1,
37,1,10,2165,19,
326,1,10,2166,5,
124,1,688,2167,16,
0,324,1,210,2168,
16,0,324,1,461,
2169,16,0,324,1,
459,1389,1,457,1390,
1,455,2170,16,0,
412,1,454,2171,17,
2172,15,2173,4,18,
37,0,102,0,117,
0,110,0,99,0,
110,0,97,0,109,
0,101,0,1,-1,
1,5,2174,20,618,
1,157,1,3,1,
4,1,3,2175,22,
1,64,1,931,1411,
1,213,2176,16,0,
324,1,451,2177,17,
2178,15,2173,1,-1,
1,5,2179,20,616,
1,158,1,3,1,
4,1,3,2180,22,
1,65,1,449,2181,
17,2182,15,2173,1,
-1,1,5,2183,20,
611,1,159,1,3,
1,2,1,1,2184,
22,1,66,1,447,
1406,1,445,2185,16,
0,412,1,443,1416,
1,441,1422,1,1051,
1157,1,432,1437,1,
412,2186,16,0,324,
1,662,1401,1,172,
1164,1,635,1319,1,
173,2187,16,0,324,
1,650,2188,16,0,
324,1,887,2189,16,
0,324,1,1020,2190,
16,0,324,1,1050,
2191,16,0,324,1,
637,2192,16,0,324,
1,383,2193,16,0,
324,1,395,1333,1,
611,2194,16,0,324,
1,868,2195,16,0,
324,1,147,1385,1,
623,2196,16,0,324,
1,861,2197,16,0,
324,1,382,1355,1,
380,1365,1,378,1370,
1,133,1198,1,132,
1202,1,131,1206,1,
130,1210,1,129,1214,
1,128,1218,1,127,
1179,1,832,2198,16,
0,324,1,351,1187,
1,109,1433,1,582,
2199,16,0,324,1,
581,1374,1,97,1130,
1,98,2200,16,0,
324,1,1053,1148,1,
96,1134,1,95,1139,
1,333,1239,1,92,
1143,1,548,2201,16,
0,324,1,560,2202,
16,0,324,1,559,
1351,1,558,1381,1,
69,2203,16,0,324,
1,792,2204,16,0,
324,1,1029,1343,1,
309,2205,16,0,324,
1,308,1038,1,307,
1042,1,930,1360,1,
305,1125,1,59,1062,
1,303,1153,1,57,
1068,1,62,2206,16,
0,324,1,61,1056,
1,60,1059,1,776,
2207,16,0,324,1,
58,1065,1,774,1396,
1,56,1071,1,55,
1074,1,54,1077,1,
53,1080,1,52,1083,
1,51,1086,1,50,
1089,1,49,1092,1,
48,1095,1,47,1049,
1,45,2208,16,0,
324,1,44,1173,1,
998,2209,16,0,324,
1,519,2210,16,0,
324,1,40,2211,16,
0,425,1,753,2212,
16,0,324,1,751,
1441,1,33,2213,16,
0,425,1,748,1312,
1,5,1116,1,28,
2214,16,0,324,1,
27,1193,1,15,1234,
1,17,1229,1,22,
2215,16,0,425,1,
21,2216,16,0,324,
1,20,1192,1,19,
1223,1,12,1224,1,
734,2217,16,0,324,
1,733,1428,1,971,
1338,1,14,2218,16,
0,324,1,730,1326,
1,490,2219,16,0,
324,1,11,2220,16,
0,412,1,10,1243,
1,9,1247,1,0,
2221,16,0,324,1,
7,1109,1,6,1113,
1,961,2222,16,0,
324,1,4,2223,16,
0,324,1,3,1254,
1,2,1251,1,1,
2224,16,0,425,1,
717,2225,16,0,324,
1,9,2226,19,218,
1,9,2227,5,63,
1,748,1312,1,930,
1360,1,92,1143,1,
635,1319,1,97,1130,
1,96,1134,1,95,
1139,1,306,2228,16,
0,216,1,305,1125,
1,1053,1148,1,303,
1153,1,730,1326,1,
1051,1157,1,931,1411,
1,395,1333,1,971,
1338,1,174,2086,1,
1029,1343,1,172,1164,
1,382,1355,1,380,
1365,1,378,1370,1,
46,2091,1,44,1173,
1,581,1374,1,15,
1234,1,560,1347,1,
559,1351,1,558,1381,
1,19,1223,1,147,
1385,1,27,1193,1,
132,1202,1,127,1179,
1,33,1183,1,459,
1389,1,351,1187,1,
457,1390,1,20,1192,
1,776,2229,16,0,
229,1,133,1198,1,
774,1396,1,131,1206,
1,130,1210,1,129,
1214,1,128,1218,1,
662,1401,1,447,1406,
1,17,1229,1,443,
1416,1,12,1224,1,
441,1422,1,333,1239,
1,10,1243,1,9,
1247,1,733,1428,1,
109,1433,1,2,1251,
1,432,1437,1,3,
1254,1,751,1441,1,
1,1446,1,214,2101,
1,8,2230,19,134,
1,8,2231,5,20,
1,92,1143,1,44,
1173,1,33,2232,16,
0,389,1,172,1164,
1,27,1193,1,449,
2233,16,0,132,1,
22,2234,16,0,389,
1,305,1125,1,351,
1187,1,303,1153,1,
20,1192,1,19,1223,
1,10,1243,1,9,
1247,1,2,1251,1,
1,2235,16,0,389,
1,333,1239,1,3,
1254,1,96,1134,1,
95,1139,1,7,2236,
19,108,1,7,2237,
5,42,1,92,1143,
1,97,1130,1,96,
1134,1,95,1139,1,
306,2238,16,0,215,
1,305,1125,1,1053,
1148,1,303,1153,1,
1051,1157,1,395,2239,
16,0,170,1,174,
2086,1,172,1164,1,
378,2240,16,0,181,
1,46,2091,1,473,
2241,16,0,124,1,
44,1173,1,147,1385,
1,33,1183,1,459,
2242,16,0,181,1,
351,1187,1,20,2243,
16,0,405,1,27,
1193,1,133,1198,1,
132,1202,1,131,1206,
1,130,1210,1,129,
1214,1,128,1218,1,
127,1179,1,19,1223,
1,17,1229,1,15,
1234,1,12,1224,1,
333,1239,1,10,1243,
1,9,1247,1,109,
1433,1,2,1251,1,
3,1254,1,1072,2244,
16,0,398,1,502,
2245,16,0,106,1,
214,2101,1,5,2246,
19,276,1,5,2247,
5,43,1,213,2248,
16,0,274,1,412,
2249,16,0,274,1,
210,2250,16,0,274,
1,98,2251,16,0,
274,1,309,2252,16,
0,274,1,308,1038,
1,307,1042,1,519,
2253,16,0,274,1,
832,2254,16,0,274,
1,490,2255,16,0,
274,1,69,2256,16,
0,274,1,47,1049,
1,173,2257,16,0,
274,1,62,2258,16,
0,274,1,383,2259,
16,0,274,1,61,
1056,1,60,1059,1,
59,1062,1,58,1065,
1,57,1068,1,56,
1071,1,55,1074,1,
54,1077,1,53,1080,
1,52,1083,1,51,
1086,1,50,1089,1,
49,1092,1,48,1095,
1,582,2260,16,0,
274,1,688,2261,16,
0,274,1,45,2262,
16,0,274,1,998,
2263,16,0,274,1,
461,2264,16,0,274,
1,887,2265,16,0,
274,1,28,2266,16,
0,274,1,560,2267,
16,0,274,1,4,
2268,16,0,274,1,
650,2269,16,0,274,
1,7,1109,1,6,
1113,1,5,1116,1,
753,2270,16,0,274,
1,4,2271,19,279,
1,4,2272,5,63,
1,213,2273,16,0,
277,1,412,2274,16,
0,277,1,210,2275,
16,0,277,1,305,
1125,1,98,2276,16,
0,277,1,96,1134,
1,95,1139,1,308,
1038,1,307,1042,1,
92,1143,1,519,2277,
16,0,277,1,303,
1153,1,832,2278,16,
0,277,1,490,2279,
16,0,277,1,172,
1164,1,69,2280,16,
0,277,1,47,1049,
1,173,2281,16,0,
277,1,62,2282,16,
0,277,1,383,2283,
16,0,277,1,61,
1056,1,60,1059,1,
59,1062,1,58,1065,
1,57,1068,1,56,
1071,1,55,1074,1,
54,1077,1,53,1080,
1,52,1083,1,51,
1086,1,50,1089,1,
49,1092,1,48,1095,
1,582,2284,16,0,
277,1,688,2285,16,
0,277,1,45,2286,
16,0,277,1,44,
1173,1,40,2287,16,
0,427,1,33,2288,
16,0,427,1,998,
2289,16,0,277,1,
461,2290,16,0,277,
1,887,2291,16,0,
277,1,351,1187,1,
28,2292,16,0,277,
1,27,1193,1,560,
2293,16,0,277,1,
22,2294,16,0,427,
1,20,1192,1,19,
1223,1,4,2295,16,
0,277,1,10,1243,
1,333,1239,1,309,
2296,16,0,277,1,
9,1247,1,650,2297,
16,0,277,1,7,
1109,1,6,1113,1,
5,1116,1,753,2298,
16,0,277,1,3,
1254,1,2,1251,1,
1,2299,16,0,427,
1,3,2300,19,137,
1,3,2301,5,127,
1,452,2302,16,0,
135,1,210,2303,16,
0,407,1,461,2304,
16,0,407,1,459,
1389,1,458,2305,16,
0,141,1,457,1390,
1,931,1411,1,213,
2306,16,0,407,1,
450,2307,16,0,155,
1,688,2308,16,0,
407,1,448,2309,16,
0,135,1,447,1406,
1,444,2310,16,0,
162,1,443,1416,1,
441,1422,1,432,1437,
1,412,2311,16,0,
407,1,662,1401,1,
172,1164,1,635,1319,
1,173,2312,16,0,
407,1,650,2313,16,
0,407,1,887,2314,
16,0,407,1,1053,
1148,1,1051,1157,1,
1050,2315,16,0,407,
1,637,2316,16,0,
407,1,383,2317,16,
0,407,1,395,1333,
1,611,2318,16,0,
407,1,868,2319,16,
0,407,1,147,1385,
1,623,2320,16,0,
407,1,861,2321,16,
0,407,1,382,1355,
1,380,1365,1,379,
2322,16,0,182,1,
378,1370,1,377,2323,
16,0,182,1,133,
1198,1,132,1202,1,
131,1206,1,130,1210,
1,129,1214,1,128,
1218,1,127,1179,1,
1073,2324,16,0,399,
1,832,2325,16,0,
407,1,351,1187,1,
109,1433,1,582,2326,
16,0,407,1,581,
1374,1,98,2327,16,
0,407,1,97,1130,
1,96,1134,1,95,
1139,1,333,1239,1,
92,1143,1,548,2328,
16,0,407,1,303,
1153,1,560,2329,16,
0,407,1,559,1351,
1,558,1381,1,69,
2330,16,0,407,1,
792,2331,16,0,407,
1,1029,1343,1,309,
2332,16,0,255,1,
308,1038,1,307,1042,
1,305,1125,1,59,
1062,1,1020,2333,16,
0,407,1,57,1068,
1,62,2334,16,0,
407,1,61,1056,1,
60,1059,1,776,2335,
16,0,407,1,58,
1065,1,774,1396,1,
56,1071,1,55,1074,
1,54,1077,1,53,
1080,1,52,1083,1,
51,1086,1,50,1089,
1,49,1092,1,48,
1095,1,47,1049,1,
45,2336,16,0,255,
1,44,1173,1,998,
2337,16,0,407,1,
519,2338,16,0,407,
1,39,2339,16,0,
388,1,961,2340,16,
0,407,1,753,2341,
16,0,407,1,751,
1441,1,33,1183,1,
748,1312,1,13,2342,
16,0,399,1,28,
2343,16,0,407,1,
27,1193,1,26,2344,
16,0,402,1,15,
1234,1,17,1229,1,
21,2345,16,0,407,
1,20,1192,1,19,
1223,1,12,1224,1,
734,2346,16,0,407,
1,733,1428,1,971,
1338,1,14,2347,16,
0,407,1,730,1326,
1,490,2348,16,0,
407,1,5,1116,1,
10,1243,1,9,1247,
1,0,2349,16,0,
407,1,7,1109,1,
6,1113,1,930,1360,
1,4,2350,16,0,
407,1,3,1254,1,
2,1251,1,1,1446,
1,717,2351,16,0,
407,1,2,2352,19,
323,1,2,2353,5,
62,1,748,1312,1,
930,1360,1,971,1338,
1,635,1319,1,97,
1130,1,96,1134,1,
95,1139,1,92,1143,
1,305,1125,1,1053,
1148,1,303,1153,1,
730,1326,1,559,1351,
1,931,1411,1,395,
1333,1,380,1365,1,
1029,1343,1,172,1164,
1,811,1482,1,382,
1355,1,808,1487,1,
378,1370,1,1051,1157,
1,44,1173,1,581,
1374,1,15,1234,1,
560,1347,1,792,1492,
1,558,1381,1,19,
1223,1,147,1385,1,
27,1193,1,132,1202,
1,127,1179,1,33,
1183,1,459,1389,1,
351,1187,1,457,1390,
1,20,1192,1,776,
1496,1,133,1198,1,
774,1396,1,131,1206,
1,130,1210,1,129,
1214,1,128,1218,1,
662,1401,1,447,1406,
1,17,1229,1,443,
1416,1,12,1224,1,
441,1422,1,333,1239,
1,10,1243,1,9,
1247,1,733,1428,1,
109,1433,1,2,1251,
1,432,1437,1,3,
1254,1,751,1441,1,
1,1446,2,1,0};
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
new Sfactory(this,"exp_11",new SCreator(exp_11_factory));
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
public static object exp_11_factory(Parser yyp) { return new exp_11(yyp); }
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
}
