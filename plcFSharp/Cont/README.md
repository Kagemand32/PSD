# Compiling and loading continuation-based evaluators

Chapter 11 introduces the concept of continuation, which helps
understand such notions as tail call, exceptions and exception
handling, execution stack, and back- tracking.  A continuation is an
explicit representation of the rest of the compu-tation.
Usually this is implicit in a program: after executing one statement,
the computation will continue with the next statement; when returning
from a method, the computation will continue where the method was
called; and so on.  Making the continuation explicit has the advantage
that we can ignore it (and so model abnormal termination), and that we
can have more than one continuation (and so model exception handling
and backtracking).


## A. Loading two continuation-based interpreters for a functional
   language with exceptions:

```bash
dotnet fsi Contfun.fs
```

```fsharp
open Contfun;;
eval1 ex1 [];; 
eval1 ex2 [("n", Int 10)];;
#q;;
```

## B. Loading two continuation-based interpreters for an imperative
   language with exceptions:

```bash
dotnet fsi Contimp.fs
```

```fsharp
open Contimp;;
run1 ex1;;
run1 ex2;;
run2 ex3;;
#q;;
```

## C. Loading a continuation-based interpreter for micro-Icon, a language
   in which an expression can have multiple results:

```bash
dotnet fsi Icon.fs
```

```fsharp
open Icon;;
run ex1;;
run ex2;;
run ex3and;;
run ex3or;;
#q;;
```

## D. Compile and run a Java implementation of factorial in
   continuation-passing style:

```bash
cd Factorial/

javac Factorial.java
java Factorial 10

javac Factorial2.java
java Factorial2 10
```

## E. Compile and run a C# implementation of factorial in
   continuation-passing style:

```bash
cd Factorial/

dotnet build Factorial.csproj
dotnet run 10
```

## F. Compile and run example illustrating longjmp and setjmp in C (under
   Linux and MacOS):

See `README` in root folder for how to install Clang.

```bash
clang testlongjmp.c -o testlongjmp
./testlongjmp 10
./testlongjmp 11
```
