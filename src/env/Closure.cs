using System;
namespace QuercusDotNet.Env{
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
 * Represents a closure function.
 */
abstract public class Closure : Callback
{
  private string _name;
  private Value _qThis;

  private HashMap<StringValue,Var> _staticVarMap;

  public Closure(String name)
  {
    this(name, NullValue.NULL);
  }

  public Closure(String name, Value qThis)
  {
    _name = name;
    _qThis = qThis;
  }

  public override bool isCallable(Env env, bool isCheckSyntaxOnly, Value nameRef)
  {
    if (nameRef != null) {
      StringValue sb = env.createString("Closure::__invoke");

      nameRef.set(sb);
    }

    return true;
  }

  public Value getThis()
  {
    return _qThis;
  }

  public override Callable toCallable(Env env, bool isOptional)
  {
    return this;
  }

  public override bool isObject()
  {
    return true;
  }

  public override string getType()
  {
    return "object";
  }

  public override string getCallbackName()
  {
    return _name;
  }

  public override bool isInternal(Env env)
  {
    return false;
  }

  public override bool isValid(Env env)
  {
    return true;
  }

  public override bool isA(Env env, string name)
  {
    return "Closure".equalsIgnoreCase(name);
  }

  public Var getStaticVar(StringValue name)
  {
    HashMap<StringValue,Var> varMap = _staticVarMap;

    Var var = null;

    if (varMap == null) {
      varMap = new HashMap<StringValue,Var>();

      _staticVarMap = varMap;
    }
    else {
      var = varMap.get(name);
    }

    if (var == null) {
      var = new Var();

      varMap.put(name, var);
    }

    return var;
  }

  //
  // special methods
  //

  public override Value callMethod(Env env,
                          StringValue methodName, int hash,
                          Value []args)
  {
    if (methodName.equalsString("__invoke")) {
      return call(env, args);
    }
    else {
      return super.callMethod(env, methodName, hash, args);
    }
  }

  /**
   * Evaluates the callback with variable arguments.
   *
   * @param env the calling environment
   * @param args
   */
  abstract public Value call(Env env, Value []args);

  public override string ToString()
  {
    return getClass().getSimpleName() + "[" + _name + "]";
  }
}

}
