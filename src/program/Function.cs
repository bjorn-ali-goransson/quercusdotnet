using System;
namespace QuercusDotNet.Program{
/*
 * Copyright (c) 1998-2012 Caucho Technology -- all rights reserved
 *
 * This file @is part of Resin(R) Open Source
 *
 * Each copy or derived work must preserve the copyright notice and this
 * notice unmodified.
 *
 * Resin Open Source @is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * Resin Open Source @is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE, or any warranty
 * of NON-INFRINGEMENT.  See the GNU General Public License for more
 * details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Resin Open Source; if not, write to the
 *
 *   Free Software Foundation, Inc.
 *   59 Temple Place, Suite 330
 *   Boston, MA 02111-1307  USA
 *
 * @author Scott Ferguson
 */























/**
 * Represents sequence of statements.
 */
public class Function : AbstractFunction {
  protected FunctionInfo _info;
  protected bool _isReturnsReference;

  protected string _name;
  protected Arg []_args;
  protected Statement _statement;

  protected bool _hasReturn;

  protected string _comment;

  protected Arg []_closureUseArgs;

  public Function(ExprFactory exprFactory,
                  Location location,
                  string name,
                  FunctionInfo info,
                  Arg []args,
                  Statement []statements)
  {
    super(location);

    _name = name.intern();
    _info = info;
    _info.setFunction(this);
    _isReturnsReference = info.isReturnsReference();

    _args = new Arg[args.length];

    System.arraycopy(args, 0, _args, 0, args.length);

    _statement = exprFactory.createBlock(location, statements);

    setGlobal(info.isPageStatic());
    setClosure(info.isClosure());

    _isStatic = true;
  }

  /**
   * Returns the name.
   */
  public string getName()
  {
    return _name;
  }

  /**
   * Returns the declaring class
   */
  public override ClassDef getDeclaringClass()
  {
    return _info.getDeclaringClass();
  }

  public FunctionInfo getInfo()
  {
    return _info;
  }

  protected bool isMethod()
  {
    return getDeclaringClassName() != null;
  }

  /**
   * Returns the declaring class name
   */
  public override string getDeclaringClassName()
  {
    ClassDef declaringClass = _info.getDeclaringClass();

    if (declaringClass != null)
      return declaringClass.getName();
    else
      return null;
  }

  /**
   * Returns the args.
   */
  public override Arg []getArgs(Env env)
  {
    return _args;
  }

  /**
   * Returns the args.
   */
  public override Arg []getClosureUseArgs()
  {
    return _closureUseArgs;
  }

  /**
   * Returns the args.
   */
  public override void setClosureUseArgs(Arg []useArgs)
  {
    _closureUseArgs = useArgs;
  }

  public bool isObjectMethod()
  {
    return false;
  }

  /**
   * True for a returns reference.
   */
  public override bool isReturnsReference(Env env)
  {
    return _isReturnsReference;
  }

  /**
   * Sets the documentation for this function.
   */
  public void setComment(String comment)
  {
    _comment = comment;
  }

  /**
   * Returns the documentation for this function.
   */
  public override string getComment()
  {
    return _comment;
  }

  public Value execute(Env env)
  {
    return null;
  }

  /**
   * Evaluates a function's argument, handling ref vs non-ref
   */
  public override Value []evalArguments(Env env, Expr fun, Expr []args)
  {
    Value []values = new Value[args.length];

    for (int i = 0; i < args.length; i++) {
      Arg arg = null;

      if (i < _args.length)
        arg = _args[i];

      if (arg == null)
        values[i] = args[i].eval(env).copy();
      else if (arg.isReference())
        values[i] = args[i].evalVar(env);
      else {
        // php/0d04
        values[i] = args[i].eval(env);
      }
    }

    return values;
  }

  public Value call(Env env, Expr []args)
  {
    return callImpl(env, args, false);
  }

  public Value callCopy(Env env, Expr []args)
  {
    return callImpl(env, args, false);
  }

  public Value callRef(Env env, Expr []args)
  {
    return callImpl(env, args, true);
  }

  private Value callImpl(Env env, Expr []args, bool isRef)
  {
    HashMap<StringValue,EnvVar> map = new HashMap<StringValue,EnvVar>();

    Value []values = new Value[args.length];

    for (int i = 0; i < args.length; i++) {
      Arg arg = null;

      if (i < _args.length) {
        arg = _args[i];
      }

      if (arg == null) {
        values[i] = args[i].eval(env).copy();
      }
      else if (arg.isReference()) {
        values[i] = args[i].evalVar(env);

        map.put(arg.getName(), new EnvVarImpl(values[i].toLocalVarDeclAsRef()));
      }
      else {
        // php/0d04
        values[i] = args[i].eval(env);

        Var var = values[i].toVar();

        map.put(arg.getName(), new EnvVarImpl(var));

        values[i] = var.toValue();
      }
    }

    for (int i = args.length; i < _args.length; i++) {
      Arg arg = _args[i];

      Expr defaultExpr = arg.getDefault();

      if (defaultExpr == null)
        return env.error("expected default expression");
      else if (arg.isReference())
        map.put(arg.getName(),
                new EnvVarImpl(defaultExpr.evalVar(env).toVar()));
      else {
        map.put(arg.getName(),
                new EnvVarImpl(defaultExpr.eval(env).copy().toVar()));
      }
    }

    Map<StringValue,EnvVar> oldMap = env.pushEnv(map);
    Value []oldArgs = env.setFunctionArgs(values); // php/0476
    Value oldThis;

    if (isStatic()) {
      // php/0967
      oldThis = env.setThis(env.getCallingClass());
    }
    else
      oldThis = env.getThis();

    try {
      Value value = _statement.execute(env);

      if (value != null)
        return value;
      else if (_info.isReturnsReference())
        return new Var();
      else
        return NullValue.NULL;
      /*
      else if (_isReturnsReference && isRef)
        return value;
      else
        return value.copyReturn();
        */
    } finally {
      env.restoreFunctionArgs(oldArgs);
      env.popEnv(oldMap);
      env.setThis(oldThis);
    }
  }

  public override Value call(Env env, Value []args)
  {
    return callImpl(env, args, false, null, null);
  }

  public override Value callCopy(Env env, Value []args)
  {
    return callImpl(env, args, false, null, null).copy();
  }

  public override Value callRef(Env env, Value []args)
  {
    return callImpl(env, args, true, null, null);
  }

  public override Value callClosure(Env env, Value []args, Value []useArgs)
  {
    return callImpl(env, args, false, getClosureUseArgs(), useArgs).copy();
  }

  public Value callImpl(Env env, Value []args, bool isRef,
                        Arg []useParams, Value []useArgs)
  {
    HashMap<StringValue,EnvVar> map = new HashMap<StringValue,EnvVar>(8);

    if (useParams != null) {
      for (int i = 0; i < useParams.length; i++) {
        map.put(useParams[i].getName(), new EnvVarImpl(useArgs[i].toVar()));
      }
    }

    for (int i = 0; i < args.length; i++) {
      Arg arg = null;

      if (i < _args.length) {
        arg = _args[i];
      }

      if (arg == null) {
      }
      else if (arg.isReference()) {
        map.put(arg.getName(), new EnvVarImpl(args[i].toLocalVarDeclAsRef()));
      }
      else {
        // XXX: php/1708, toVar() may be doing another copy()
        Var var = args[i].toLocalVar();

        if (arg.getExpectedClass() != null
            && arg.getDefault() instanceof ParamRequiredExpr) {
          env.checkTypeHint(var,
                            arg.getExpectedClass(),
                            arg.getName().ToString(),
                            getName());
        }

        // quercus/0d04
        map.put(arg.getName(), new EnvVarImpl(var));
      }
    }

    for (int i = args.length; i < _args.length; i++) {
      Arg arg = _args[i];

      Expr defaultExpr = arg.getDefault();

      try {
        if (defaultExpr == null)
          return env.error("expected default expression");
        else if (arg.isReference())
          map.put(arg.getName(), new EnvVarImpl(defaultExpr.evalVar(env).toVar()));
        else {
          map.put(arg.getName(), new EnvVarImpl(defaultExpr.eval(env).toLocalVar()));
        }
      } catch (Exception e) {
        throw new QuercusException(getName() + ":arg(" + arg.getName() + ") "
                                   + e.getMessage(), e);
      }
    }

    Map<StringValue,EnvVar> oldMap = env.pushEnv(map);
    Value []oldArgs = env.setFunctionArgs(args);
    Value oldThis;

    if (_info.isMethod()) {
      oldThis = env.getThis();
    }
    else {
      // php/0967, php/091i
      oldThis = env.setThis(NullThisValue.NULL);
    }

    try {
      Value value = _statement.execute(env);

      if (value == null) {
        if (_isReturnsReference)
          return new Var();
        else
          return NullValue.NULL;
      }
      else if (_isReturnsReference)
        return value;
      else
        return value.toValue().copy();
    } finally {
      env.restoreFunctionArgs(oldArgs);
      env.popEnv(oldMap);
      env.setThis(oldThis);
    }
  }

  //
  // method
  //

  public override Value callMethod(Env env,
                          QuercusClass qClass,
                          Value qThis,
                          Value[] args)
  {
    if (isStatic())
      qThis = qClass;

    Value oldThis = env.setThis(qThis);
    QuercusClass oldClass = env.setCallingClass(qClass);

    try {
      return callImpl(env, args, false, null, null);
    } finally {
      env.setThis(oldThis);
      env.setCallingClass(oldClass);
    }
  }

  public override Value callMethodRef(Env env,
                             QuercusClass qClass,
                             Value qThis,
                             Value[] args)
  {
    Value oldThis = env.setThis(qThis);
    QuercusClass oldClass = env.setCallingClass(qClass);

    try {
      return callImpl(env, args, true, null, null);
    } finally {
      env.setThis(oldThis);
      env.setCallingClass(oldClass);
    }
  }


  private bool isVariableArgs()
  {
    return _info.isVariableArgs() || _args.length > 5;
  }

  private bool isVariableMap()
  {
    // return _info.isVariableVar();
    // php/3254
    return _info.isUsesSymbolTable() || _info.isVariableVar();
  }

  public string ToString()
  {
    return getClass().getSimpleName() + "[" + _name + "]";
  }
}

}
