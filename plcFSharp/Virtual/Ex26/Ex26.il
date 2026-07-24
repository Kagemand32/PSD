// IL code: Ex26
.class private auto ansi '<Module>'
{
} // end of class <Module>

.class public auto ansi beforefieldinit Ex26
	extends [System.Runtime]System.Object
{
	// Methods
	.method public hidebysig static 
		void Main (
			string[] args
		) cil managed 
	{
		// Method begins at RVA 0x2050
		// Header size: 12
		// Code size: 74 (0x4a)
		.maxstack 4
		.entrypoint
		.locals init (
			[0] int64,
			[1] int64,
			[2] int64
		)

		IL_0000: ldarg.0
		IL_0001: ldc.i4.0
		IL_0002: ldelem.ref
		IL_0003: call int64 [System.Runtime]System.Int64::Parse(string)
		IL_0008: stloc.0
		IL_0009: ldc.i4.0
		IL_000a: conv.i8
		IL_000b: stloc.1
		IL_000c: ldc.i4.2
		IL_000d: conv.i8
		IL_000e: stloc.2
		IL_000f: br.s IL_0023
		// loop start (head: IL_0023)
			IL_0011: ldloc.2
			IL_0012: call bool Ex26::isPrime(int64)
			IL_0017: brfalse.s IL_001e

			IL_0019: ldloc.1
			IL_001a: ldc.i4.1
			IL_001b: conv.i8
			IL_001c: add
			IL_001d: stloc.1

			IL_001e: ldloc.2
			IL_001f: ldc.i4.1
			IL_0020: conv.i8
			IL_0021: add
			IL_0022: stloc.2

			IL_0023: ldloc.2
			IL_0024: ldloc.0
			IL_0025: blt.s IL_0011
		// end loop

		IL_0027: ldstr "range = "
		IL_002c: ldloca.s 0
		IL_002e: call instance string [System.Runtime]System.Int64::ToString()
		IL_0033: ldstr ", primes = "
		IL_0038: ldloca.s 1
		IL_003a: call instance string [System.Runtime]System.Int64::ToString()
		IL_003f: call string [System.Runtime]System.String::Concat(string, string, string, string)
		IL_0044: call void [System.Console]System.Console::WriteLine(string)
		IL_0049: ret
	} // end of method Ex26::Main

	.method private hidebysig static 
		bool isPrime (
			int64 n
		) cil managed 
	{
		// Method begins at RVA 0x20a8
		// Header size: 12
		// Code size: 35 (0x23)
		.maxstack 2
		.locals init (
			[0] int64
		)

		IL_0000: ldc.i4.2
		IL_0001: conv.i8
		IL_0002: stloc.0
		IL_0003: br.s IL_000a
		// loop start (head: IL_000a)
			IL_0005: ldloc.0
			IL_0006: ldc.i4.1
			IL_0007: conv.i8
			IL_0008: add
			IL_0009: stloc.0

			IL_000a: ldloc.0
			IL_000b: ldloc.0
			IL_000c: mul
			IL_000d: ldarg.0
			IL_000e: bgt.s IL_0015

			IL_0010: ldarg.0
			IL_0011: ldloc.0
			IL_0012: rem
			IL_0013: brtrue.s IL_0005
		// end loop

		IL_0015: ldarg.0
		IL_0016: ldc.i4.2
		IL_0017: conv.i8
		IL_0018: blt.s IL_0021

		IL_001a: ldloc.0
		IL_001b: ldloc.0
		IL_001c: mul
		IL_001d: ldarg.0
		IL_001e: cgt
		IL_0020: ret

		IL_0021: ldc.i4.0
		IL_0022: ret
	} // end of method Ex26::isPrime

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x20d7
		// Header size: 1
		// Code size: 7 (0x7)
		.maxstack 8

		IL_0000: ldarg.0
		IL_0001: call instance void [System.Runtime]System.Object::.ctor()
		IL_0006: ret
	} // end of method Ex26::.ctor

} // end of class Ex26

You are not using the latest version of the tool, please update.
Latest version is '10.1.1.8388' (yours is '9.1.0.7988')
