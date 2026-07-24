# Programming Language Constructs

## Code Structure

Each chapter refers to what files and directories are used for the
topic. All code resides in directory `plcFSharp`.

For instance, the chapter on micro-SML refers to the subdirectories
`MicroVM` and `MicroSML` found in `plcFSharp`. This means that all
exampels and exercises can be completed by having those two
directories available in a folder, e.g.,

```code
.../ExerciseMicroSML/MicroVM
                    /MicroSML
```

You can also make code changes directly in the cloned repository. We
recommend you copy the needed folders such that you do not need to
implement all exercises in the same set of files.

## Example Code

The table below should what directories contain example code used in
each chapter.

| Chapter | Title | Example Code |
|--------:|:-----:|:-------------|
| 1 | Introduction | [Intro](Intro/README.md) |
| 2 | Interpreters and Compilers | [Intcomp](Intcomp/README.md) |
| 3 | From Concrete Syntax to Abstract Syntax | [Usql](Usql/README.md), [Expr](Expr/README.md) |
| 4 | A First-Order Functional Language | [Fun](Fun/README.md), [TypedFun](TypedFun/README.md) |
| 5 | Higher-Order Functions | [Fun](Fun/README.md) |
| 6 | Polymorphic Types | [Fun](Fun/README.md) |
| 7 | Imperative Languages | [Imp](Imp/README.md), [MicroC](MicroC/README.md) |
| 8 | Compiling Micro-C | [MicroC](MicroC/README.md) |
| 9 | Real-World Abstract Machines | [Virtual](Virtual/README.md) |
| 10 | Garbage Collection | [ListC](ListC/README.md), [MicroVM](MicroVM/README.md) |
| 11 | Continuations | [Cont](Cont/README.md) |
| 12 | A Locally Optimizing Compiler | [MicroC](MicroC/README.md) |
| 13 | Compiling Micro-SML | [MicroSML](MicroSML/README.md), [MicroVM](MicroVM/README.md) |
| 14 | Typing Micro-Java | [MicroJ](MicroJ/README.md) |
| 15 | Compiling Micro-Java | [MicroJ](MicroJ/README.md), [MicroVM](MicroVM/README.md) |
| 16 | Real Machine Code: Arm64 | [Arm64](Arm64/README.md) |
| App. A | Crash Course in F# | [Intro](Intro/README.md) |


## Platform Dependencies

The supporting files work on Linux, MacOS and Windows platforms.

You need the following installed:

- .NET SDK, version 8+, see [Install .NET on Windows, Linux, and
  macOS](https://learn.microsoft.com/da-dk/dotnet/core/install/)

- Java SE 25+ compiler,
  [Linux, macOS, Windows](https://www.oracle.com/europe/java/technologies/downloads/).

- Clang and LLVM, a cross platform compiler supporting C and C++. The
  Clang compiler is a cross platform compiler and works on Linux,
  Windows and MacOS.

### Installing Clang, Linux

One approach is to install using a package manager:

- Debian: `sudo apt install clang`

The provided code has also been tested with default `gcc` compiler on
various Linux distributions.

### Installing Clang, MacOS

Clang is the default compiler on MacOS. No need for further
installation. You can also compile with `gcc` which is likely an alias
for `clang`.

### Installing Clang, Windows x86_64

There are two dependencies that must be installed:

1. [MSVC toolchain](https://visualstudio.microsoft.com/downloads)

Find **Build Tools for Visual Studio 2026**, download the installer,
and use it to select and install these files:

- TODO

2. `clang` for Windows x86_64, [llvm](https://releases.llvm.org)

- `LLVM-22.1.0-win64.exe` (or later)

Consult `MicroVM/README.md` to test the installation on the micro
virtual machine, `microvm.c`.

You may check version and any later version should work:

```bash
  $ clang --version
clang version 22.1.0 (https://github.com/llvm/llvm-project 4434dabb69916856b824f68a64b029c67175e532)
Target: x86_64-pc-windows-msvc
```

### Installing Clang, Windows ARM

There are two dependencies that must be installed:

1. [MSVC toolchain](https://visualstudio.microsoft.com/downloads)

Find **Build Tools for Visual Studio 2026**, download the installer,
and use it to select and install these files:

- MSVC v143 - VS 2022 C++ ARM64/ARM64EC build tools
- MSVC v143 - VS 2022 C++ ARM64/ARM64EC Spectre-mitigated libs
- Windows 11 SDK

2. `clang` for Windows ARM, [llvm](https://releases.llvm.org)

- `LLVM-22.1.0-woa64.exe` (or later)

Consult `MicroVM/README.md` to test the installation on the micro
virtual machine, `microvm.c`.
