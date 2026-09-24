(* File TypedFun/TypedFun.fs
   An explicitly typed strict first-order functional language. 
   sestoft@itu.dk 2009-09-11

   Different abstract syntax from the first-order and higher-order 
   functional language in Fun/Fun.fs and Fun/HigherFun.fs because
   of the explicit types on function parameters and function results.

   Does not admit mutually recursive function bindings.

   Every function takes exactly one argument.

   Type checking.  Explicit types on the argument and result of each
   declared function.  Expressions and variables may have type int or
   bool or a functional type.  Functions are monomorphically and
   explicitly typed.  

   There is no lexer or parser specification for this explicitly typed
   language because next week we shall infer types rather than check
   them.  
*)

module TypedFun

(* Environment operations *)

type 'v env = (string * 'v) list

let rec lookup env x =
    match env with 
    | []        -> failwith (x + " not found")
    | (y, v)::r -> if x=y then v else lookup r x;;

(* A type is int, bool, list, or function *)

type typ =
  | TypI                                (* int                         *)
  | TypB                                (* bool                        *)
  | TypL of typ                         (* 5.7 list, element type is typ  *)
  | TypF of typ * typ                   (* (argumenttype, resulttype)  *)

(* New abstract syntax with explicit types, instead of Absyn.expr: *)

type tyexpr = 
  | CstI of int
  | CstB of bool
  | Nil of typ (* 5.7  added types expressions for list components*)
  | Cons of tyexpr * tyexpr
  | Head of tyexpr
  | Tail of tyexpr
  | Var of string
  | Let of string * tyexpr * tyexpr
  | Prim of string * tyexpr * tyexpr
  | If of tyexpr * tyexpr * tyexpr
  | Letfun of string * string * typ * tyexpr * typ * tyexpr
          (* (f,       x,       xTyp, fBody,  rTyp, letBody *)
  | Call of tyexpr * tyexpr

(* A runtime value is an integer, a list, or a function closure *)

type value = 
  | Int of int
  | List of value list
  | Closure of string * string * tyexpr * value env       (* (f, x, fBody, fDeclEnv) *)

let rec eval (e : tyexpr) (env : value env) : value = (* 5.7 ADDED A BUNCH OF EVAL CODE FOR LISTS*)
    match e with
    | CstI i -> Int i
    | CstB b -> Int(if b then 1 else 0)
    | Nil _ -> List []
    | Cons(e1, e2) ->
      let v1 = eval e1 env
      match eval e2 env with
      | List vs -> List(v1 :: vs)
      | _ -> failwith "eval Cons: second argument is not a list"
    | Head e ->
      match eval e env with
      | List [] -> failwith "eval Head: empty list"
      | List (v::_) -> v
      | _ -> failwith "eval Head: argument is not a list"
    | Tail e ->
      match eval e env with
      | List [] -> failwith "eval Tail: empty list"
      | List (_::vs) -> List vs
      | _ -> failwith "eval Tail: argument is not a list"
    | Var x  -> lookup env x
    | Prim(ope, e1, e2) -> 
      let i1 =
        match eval e1 env with
        | Int i -> i
        | _ -> failwith "eval Prim: non-integer operand"
      let i2 =
        match eval e2 env with
        | Int i -> i
        | _ -> failwith "eval Prim: non-integer operand"
      Int(match ope with
          | "*" -> i1 * i2
          | "+" -> i1 + i2
          | "-" -> i1 - i2
          | "=" -> if i1 = i2 then 1 else 0
          | "<" -> if i1 < i2 then 1 else 0
          | _   -> failwith "unknown primitive")
    | Let(x, eRhs, letBody) -> 
      let xVal = eval eRhs env
      let bodyEnv = (x, xVal) :: env 
      eval letBody bodyEnv
    | If(e1, e2, e3) -> 
      let b =
        match eval e1 env with
        | Int i -> i
        | _ -> failwith "eval If: condition is not an integer"
      if b<>0 then eval e2 env else eval e3 env
    | Letfun(f, x, _, fBody, _, letBody) -> 
      let bodyEnv = (f, Closure(f, x, fBody, env)) :: env 
      eval letBody bodyEnv
    | Call(Var f, eArg) -> 
      let fClosure = lookup env f
      match fClosure with
      | Closure (f, x, fBody, fDeclEnv) ->
        let xVal = eval eArg env
        let fBodyEnv = (x, xVal) :: (f, fClosure) :: fDeclEnv
        eval fBody fBodyEnv
      | _ -> failwith "eval Call: not a function"
    | Call _ -> failwith "illegal function in Call"

(* Type checking for the first-order functional language: *)

let rec typ (e : tyexpr) (env : typ env) : typ =
    match e with
    | CstI i -> TypI
    | CstB b -> TypB
    | Nil t -> TypL t  (* 5.7 Added type checking for Nil, Cons, Head, Tail from here and down*)
    | Cons(e1, e2) ->
      let t1 = typ e1 env
      let t2 = typ e2 env
      match t2 with
      | TypL t when t = t1 -> t2
      | TypL _ -> failwith "Cons: element type differs from list type"
      | _ -> failwith "Cons: second argument is not a list"
    | Head e ->
      match typ e env with
      | TypL t -> t
      | _ -> failwith "Head: argument is not a list"
    | Tail e ->
      match typ e env with
      | TypL t -> TypL t
      | _ -> failwith "Tail: argument is not a list" (* 5.7 down to here *)
    | Var x  -> lookup env x 
    | Prim(ope, e1, e2) -> 
      let t1 = typ e1 env
      let t2 = typ e2 env
      match (ope, t1, t2) with
      | ("*", TypI, TypI) -> TypI
      | ("+", TypI, TypI) -> TypI
      | ("-", TypI, TypI) -> TypI
      | ("=", TypI, TypI) -> TypB
      | ("<", TypI, TypI) -> TypB
      | ("&", TypB, TypB) -> TypB
      | _   -> failwith "unknown op, or type error"
    | Let(x, eRhs, letBody) -> 
      let xTyp = typ eRhs env
      let letBodyEnv = (x, xTyp) :: env 
      typ letBody letBodyEnv
    | If(e1, e2, e3) -> 
      match typ e1 env with
      | TypB -> let t2 = typ e2 env
                let t3 = typ e3 env
                if t2 = t3 then t2
                else failwith "If: branch types differ"
      | _    -> failwith "If: condition not boolean"
    | Letfun(f, x, xTyp, fBody, rTyp, letBody) -> 
      let fTyp = TypF(xTyp, rTyp) 
      let fBodyEnv = (x, xTyp) :: (f, fTyp) :: env
      let letBodyEnv = (f, fTyp) :: env
      if typ fBody fBodyEnv = rTyp
      then typ letBody letBodyEnv
      else failwith ("Letfun: return type in " + f)
    | Call(Var f, eArg) -> 
      match lookup env f with
      | TypF(xTyp, rTyp) ->
        if typ eArg env = xTyp then rTyp
        else failwith "Call: wrong argument type"
      | _ -> failwith "Call: unknown function"
    | Call(_, eArg) -> failwith "Call: illegal function in call"

let typeCheck e = typ e [];;


(* Examples of successful type checking *)

let ex1 = Letfun("f1", "x", TypI, Prim("+", Var "x", CstI 1), TypI,
                 Call(Var "f1", CstI 12));;

(* Factorial *)

let ex2 = Letfun("fac", "x", TypI,
                 If(Prim("=", Var "x", CstI 0),
                    CstI 1,
                    Prim("*", Var "x", 
                              Call(Var "fac", 
                                   Prim("-", Var "x", CstI 1)))),
                 TypI,
                 Let("n", CstI 7, Call(Var "fac", Var "n")));;

let fac10 = eval ex2 [];;

let ex3 = Let("b", Prim("=", CstI 1, CstI 2),
              If(Var "b", CstI 3, CstI 4));;

let ex4 = Let("b", Prim("=", CstI 1, CstI 2),
              If(Var "b", Var "b", CstB false));;

let ex5 = If(Prim("=", CstI 11, CstI 12), CstI 111, CstI 666);;

let ex6 = Letfun("inf", "x", TypI, Call(Var "inf", Var "x"), TypI,
                 Call(Var "inf", CstI 0));;

let types = List.map typeCheck [ex1; ex2; ex3; ex4; ex5; ex6];;

(* Examples of type errors; should throw exception when run: *)

let exErr1 = Let("b", Prim("=", CstI 1, CstI 2),
                 If(Var "b", Var "b", CstI 6));;

let exErr2 = Letfun("f", "x", TypB, If(Var "x", CstI 11, CstI 22), TypI,
                    Call(Var "f", CstI 0));;

let exErr3 = Letfun("f", "x", TypB, Call(Var "f", CstI 22), TypI,
                    Call(Var "f", CstB true));;

let exErr4 = Letfun("f", "x", TypB, If(Var "x", CstI 11, CstI 22), TypB,
                    Call(Var "f", CstB true));;
