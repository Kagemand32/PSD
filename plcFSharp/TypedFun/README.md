# Compiling and loading the typed micro-ML evaluator

## A. Loading evaluator and type checker for the explicitly typed language

```bash
dotnet fsi TypedFun.fs
```

```fsharp
open TypedFun;;

// These typechecks should succeed:
typeCheck (Prim("+", CstI 5, CstI 7));;
```

```fsharp
typeCheck ex1;;
```

```fsharp
// This typecheck should throw exception:
typeCheck exErr1;;
```

examples of lists
```fsharp
typeCheck (Cons(CstI 1, Nil TypI));;
typeCheck (Head(Cons(CstB true, Nil TypB)));;
typeCheck (Nil (TypL TypI));;
```

```fsharp
#q;;
```
