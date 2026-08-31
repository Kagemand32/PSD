using System;
using System.Collections.Generic;

abstract class Expr
{
    public abstract int eval(Dictionary<string, int> env);
    public abstract Expr simplify();
    public override abstract string ToString();

    protected static bool IsZero(Expr e)
    {
        return e is CstI c && c.Value == 0;
    }

    protected static bool IsOne(Expr e)
    {
        return e is CstI c && c.Value == 1;
    }

    protected static bool IsSameExpr(Expr e1, Expr e2)
    {
        return e1.ToString() == e2.ToString();
    }
}

class CstI : Expr
{
    private readonly int i;

    public CstI(int i)
    {
        this.i = i;
    }

    public int Value => i;

    public override int eval(Dictionary<string, int> env)
    {
        return i;
    }

    public override Expr simplify()
    {
        return new CstI(i);
    }

    public override string ToString()
    {
        return i.ToString();
    }
}

class Var : Expr
{
    private readonly string name;

    public Var(string name)
    {
        this.name = name;
    }

    public override int eval(Dictionary<string, int> env)
    {
        return env[name];
    }

    public override Expr simplify()
    {
        return new Var(name);
    }

    public override string ToString()
    {
        return name;
    }
}

abstract class Binop : Expr
{
    protected readonly Expr e1;
    protected readonly Expr e2;

    protected Binop(Expr e1, Expr e2)
    {
        this.e1 = e1;
        this.e2 = e2;
    }
}

class Add : Binop
{
    public Add(Expr e1, Expr e2) : base(e1, e2)
    {
    }

    public override int eval(Dictionary<string, int> env)
    {
        return e1.eval(env) + e2.eval(env);
    }
    public override Expr simplify()
    {
        Expr left = e1.simplify();
        Expr right = e2.simplify();

        if (IsZero(left)) return right;
        if (IsZero(right)) return left;

        return new Add(left, right);
    }
    public override string ToString()
    {
        return "(" + e1 + "+" + e2 + ")";
    }
}

class Mul : Binop
{
    public Mul(Expr e1, Expr e2) : base(e1, e2)
    {
    }

    public override int eval(Dictionary<string, int> env)
    {
        return e1.eval(env) * e2.eval(env);
    }
    public override Expr simplify()
    {
        Expr left = e1.simplify();
        Expr right = e2.simplify();

        if (IsZero(left) || IsZero(right)) return new CstI(0);
        if (IsOne(left)) return right;
        if (IsOne(right)) return left;

        return new Mul(left, right);
    }
    public override string ToString()
    {
        return "(" + e1 + "*" + e2 + ")";
    }
}

class Sub : Binop
{
    public Sub(Expr e1, Expr e2) : base(e1, e2)
    {
    }

    public override int eval(Dictionary<string, int> env)
    {
        return e1.eval(env) - e2.eval(env);
    }
    public override Expr simplify()
    {
        Expr left = e1.simplify();
        Expr right = e2.simplify();

        if (IsZero(right)) return left;
        if (IsSameExpr(left, right)) return new CstI(0);

        return new Sub(left, right);
    }
    public override string ToString()
    {
        return "(" + e1 + "-" + e2 + ")";
    }
}

public class Ex1Expr
{
    public static void Main(string[] args)
    {
        Expr e1 = new CstI(17);
        Expr e2 = new Add(new CstI(3), new Var("a"));
        Expr e3 = new Add(new Mul(new Var("b"), new CstI(9)), new Var("a"));

        Dictionary<string, int> env0 = new Dictionary<string, int>();
        env0["a"] = 3;
        env0["c"] = 78;
        env0["baf"] = 666;
        env0["b"] = 111;

        Console.WriteLine("Env: " + env0.ToString());
        Console.WriteLine("Expression 1: " + e1);
        Console.WriteLine("Expression 2: " + e2);
        Console.WriteLine("Expression 3: " + e3);
        Console.WriteLine();
        Console.WriteLine(e1 + " = " + e1.eval(env0));
        Console.WriteLine(e2 + " = " + e2.eval(env0));
        Console.WriteLine(e3 + " = " + e3.eval(env0));

        Expr e = new Add(new CstI(0), new Mul(new Var("x"), new CstI(1)));
        Console.WriteLine(e.simplify()); // prints x
    }
}
