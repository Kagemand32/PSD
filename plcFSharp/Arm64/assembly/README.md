# Assembling and running Arm64 assembly code

You need the folder `Arm64/assembly/`.

All examples use the Clang compiler and its built-in assembler.

See README in root folder for instructions on installing Clang and
LLVM.

---

## 1. Linux

Assembly code files for Linux have prefix `lin`, as in `linsimple.s`.

To assemble, link and run:

```bash
clang linsimple.s -o linsimple
./linsimple
```

The `linfacc.s` example shows how to call an assembly-defined
recursive function `fac` from C code, and how to call C functions from
assembly.

To compile, assemble, link and run:

```bash
clang -c driver.c
clang linfacc.s driver.o -o linfacc
./linfacc 20
```

---

## 2. MacOS

Assembly code files for MacOS have prefix `mac`, as in `macsimple.s`.

To assemble, link and run:

```bash
clang -arch arm64 macsimple.s -o macsimple
./macsimple
```

The `macfacc.s` example shows how to call an assembly-defined
recursive function `fac` from C code, and how to call C functions from
assembly.

To compile, assemble, link and run:

```bash
clang -c driver.c
clang -arch arm64 macfacc.s driver.o -o macfacc
./macfacc 20
```

MacOS Arm64 assembly files differ from Linux Arm64 files in these ways:

- External names must be preceded by an underscore (`_main`, `_printf`)
- The page and page offset addresses are obtained by `@PAGE` and `@PAGEOFF`
- The second argument to `_printf` must be given on the stack, not in `x1`

---

## 3. Windows ARM

You will need Clang and LLVM for Windows on Arm64 (WOA), and your
computer must have an Arm64 processor. See README in root folder for
instructions on installing Clang and LLVM.

Intel `x86`, `x86_64`, `Amd64`, and similar architectures will not
work with these examples.

Assembly code files for Windows have prefix `win`, as in `winsimple.s`

TODO