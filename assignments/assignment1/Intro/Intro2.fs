(* Programming language concepts for software developers, 2010-08-28 *)

(* Evaluating simple expressions with variables *)

module Intro2

(* Association lists map object language variables to their values *)

let env = [("a", 3); ("c", 78); ("baf", 666); ("b", 111); ("v", 1); ("w", 2); ("x", 3); ("y", 4); ("z", 5)] //modified for exercises
let emptyenv = [] (* the empty environment *)

let rec lookup env x =
    match env with 
    | []        -> failwith (x + " not found")
    | (y, v)::r -> if x=y then v else lookup r x

let cvalue = lookup env "c"

type expr = 
  | CstI of int
  | Var of string
  | Prim of string * expr * expr
  | If of expr * expr * expr (*1.1 (iv)*)


(* Original Code *)

(*let rec eval e (env : (string * int) list) : int =
    match e with
    | CstI i            -> i
    | Var x             -> lookup env x 
    | Prim("+", e1, e2) -> eval e1 env + eval e2 env
    | Prim("*", e1, e2) -> eval e1 env * eval e2 env
    | Prim("-", e1, e2) -> eval e1 env - eval e2 env
    | Prim _            -> failwith "unknown primitive";;
*)
(*Exercise 1.1 (i) and (v)*)
let rec eval e (env : (string * int) list) : int =
    match e with
    | CstI i            -> i
    | Var x             -> lookup env x 
    | Prim("+", e1, e2) -> eval e1 env + eval e2 env
    | Prim("*", e1, e2) -> eval e1 env * eval e2 env
    | Prim("-", e1, e2) -> eval e1 env - eval e2 env
    | Prim("min", e1, e2) -> min (eval e1 env) (eval e2 env)
    | Prim("max", e1, e2) -> max (eval e1 env) (eval e2 env)
    | Prim("==",e1,e2) -> 
        match (e1, e2) with
        | (x,y) when eval x env = eval y env -> 1
        | _ -> 0   
    | If (e1, e2, e3) -> if eval e1 env <> 0 then eval e2 env else eval e3 env // 1.1 (iv)
    | Prim _ -> failwith "unknown primitive"
(* End of exercise *)

(* 1.1 (ii) *)
let example1 = Prim("max", Prim("*", Var "b", CstI 9), Var "a")
let example2 = Prim("+", Prim("==", CstI 2, CstI 3), CstI 1)
let result1 = eval example1 env // 999
let result2 = eval example2 env // 1
(* End of exercise *)

(* 1.1 (iii)*)
let rec eval3 e (env : (string * int) list) : int =
    match e with
    | CstI i -> i
    | Var x -> lookup env x
    | Prim (op, e1, e2) ->
        let i1 = eval3 e1 env
        let i2 = eval3 e2 env
        match op with
        | "+" -> i1 + i2
        | "-" -> i1 - i2
        | "*" -> i1 * i2
        | "min" -> min i1 i2
        | "max" -> max i1 i2
        | "==" -> if (i1 = i2) then 1 else 0 
        | _ -> failwith "unknown primitive"
    | If (e1, e2, e3) -> if eval3 e1 env <> 0 then eval3 e2 env else eval3 e3 env    
(* End of Exercise*)

(* End of 1.1*)
(* 1.2 (i)*)
type aexpr2 =
    | CstI of int
    | Var of string
    | Add of aexpr2 * aexpr2
    | Mul of aexpr2 * aexpr2
    | Sub of aexpr2 * aexpr2
let rec eval2 e (env : (string * int) list) : int =
    match e with
    | CstI i            -> i
    | Var x             -> lookup env x 
    | Add (e1, e2)      -> eval2 e1 env + eval2 e2 env
    | Sub (e1, e2)      -> eval2 e1 env - eval2 e2 env
    | Mul (e1, e2)      -> eval2 e1 env * eval2 e2 env
(* End of exercise *)

// ("v", 1); ("w", 2); ("x", 3); ("y", 4); ("z", 5)
(*1.2 (ii)*)
let e5 = Sub( Var "v", Add(Var "w", Var "z")) // -6
let e6 = Mul(CstI 2, Sub(Var "v", Add(Var "w", Var "z"))) // -12
let e7 = Add(Var "x", Add(Var "y", Add(Var "z", Var "v"))) // 13

(*1.2 (iii)*)
let rec fmt : aexpr2 -> string = function
    | CstI f -> string f
    | Var x -> x
    | Add (e1, e2) -> "(" + fmt e1 + " + " + fmt e2 + ")"
    | Sub (e1, e2) -> "(" + fmt e1 + " - " + fmt e2 + ")"
    | Mul (e1, e2) -> "(" + fmt e1 + " * " + fmt e2 + ")"

(* 1.2 (iv)*)
(* Apologies for the messy code. Ideas for the ugly part is explained with comments*)
(* A lot of repeat code because I do not know how to make functions with operations*)
let rec simplify : aexpr2 -> aexpr2 = function
    | CstI i ->  CstI i
    | Var x -> Var x
    | Add (CstI 0, e) -> simplify e
    | Add (e, CstI 0) -> simplify e
    | Add (e1, e2) -> 
        (* Want to see if we can keep simplifying. But want to avoid looping infinitely
        If simplifying makes no difference (Add (simplify e1, simplify e2) = Add (e1, e2)) Then any possible simplification
        would have been found beforehand.
        If not, we should get away with trying to simplify further without looping
        Same goes for mult and sub *)

        if (simplify e1, simplify e2) = (e1, e2) 
        then Add (e1,e2) 
        else simplify (Add (simplify e1, simplify e2))
    | Sub (e1, e2) when e1 = e2 -> CstI 0
    | Sub (CstI 0, e) -> simplify e
    | Sub (e, CstI 0) -> simplify e
    | Sub (e1, e2) -> 
        if (simplify e1, simplify e2) = (e1, e2) 
        then Sub (e1,e2) 
        else simplify (Sub (simplify e1, simplify e2))
    | Mul (e1, e2) when e1 = CstI 0 || e2 = CstI 0 -> CstI 0
    | Mul (CstI 1, e) -> simplify e
    | Mul (e, CstI 1) -> simplify e
    | Mul (e1, e2) ->
        if (simplify e1, simplify e2) = (e1, e2) 
        then Mul (e1,e2) 
        else simplify (Mul (simplify e1, simplify e2))
(* End of exercise *)

(* Test cases
let complicated1 = Add (CstI 0, Var "x")
let simpleStr1 = complicated1 |> simplify |> fmt 

let complicated2 = Mul (CstI 0, Add (CstI 0, Var "x"))
let simpleStr2 = complicated2 |> simplify |> fmt 

let complicated3 = Add (CstI 2, Sub (Var "x", Var "x"))
let simpleStr3 = complicated3 |> simplify |> fmt 

let alreadySimple = Add (CstI 2, Add(CstI 4, CstI 5))
let simplifiedAlreadySimple = alreadySimple |> simplify |> fmt 
let bookExample = Mul (Add (CstI 1, CstI 0), Add (Var "x", CstI 0))

let bookExampleSimplified = bookExample |> simplify |> fmt

*)
(* 1.2 (V) *)
let rec symbolicDiff (dx, e: aexpr2) =
    match e with
    | CstI _ ->  CstI 0
    | Var x when x = dx -> CstI 1
    | Var x when x <> dx -> CstI 0
    | Add (e1, e2) -> Add (symbolicDiff (dx, e1), symbolicDiff (dx, e2))
    | Sub (e1, e2) ->  Sub (symbolicDiff (dx, e1), symbolicDiff (dx, e2))
    | Mul (e1, e2) -> Add (Mul ( symbolicDiff (dx, e1), e2), Mul (symbolicDiff (dx, e2), e1))    
    | Var(_) -> failwith "Empty variable"  
(* End of exercise *)