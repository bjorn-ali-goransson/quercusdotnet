using System;
namespace QuercusDotNet.lib.reflection {
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
 * @author Nam Nguyen
 */


















public class ReflectionMethod : ReflectionFunctionAbstract
  implements Reflector
{
  private readonly L10N L = new L10N(ReflectionMethod.class);

  public const int IS_STATIC = 1;
  public const int IS_ABSTRACT = 2;
  public const int IS_FINAL = 4;

  public const int IS_PUBLIC = 256;
  public const int IS_PROTECTED = 512;
  public const int IS_PRIVATE = 1024;

  private string _clsName;

  protected ReflectionMethod(AbstractFunction method)
  {
    super(method);
  }

  protected ReflectionMethod(String clsName, AbstractFunction method)
   : base(method) {

    _clsName = clsName;
  }

  public static ReflectionMethod __construct(
      Env env, Value obj, StringValue name)
  {
    string clsName;

    if (obj.isObject())
      clsName = obj.getClassName();
    else
      clsName = obj.toString();

    return new ReflectionMethod(clsName, env.getClass(clsName).getFunction(name));
  }

  public static string export(Env env,
                              Value cls,
                              string name,
                              @Optional bool isReturn)
  {
    return null;
  }

  public Value invoke(Env env, ObjectValue object, Value []args)
  {
    return getFunction().callMethod(env, object.getQuercusClass(), object, args);
  }

  public Value invokeArgs(Env env, ObjectValue object, ArrayValue args)
  {
    AbstractFunction fun = getFunction();

    Expr expr = new CallExpr(Location.UNKNOWN, env.createString(getName()), (Expr[]) null);
    env.pushCall(expr, object, args.getValueArray(env));

    try {
      return fun.callMethod(env, object.getQuercusClass(), object,
                            args.getValueArray(env));
    }
    finally {
      env.popCall();
    }


  }

  public bool isFinal()
  {
    return getFunction().isFinal();
  }

  public bool isAbstract()
  {
    return getFunction().isAbstract();
  }

  public bool isPublic()
  {
    return getFunction().isPublic();
  }

  public bool isPrivate()
  {
    return getFunction().isPrivate();
  }

  public bool isProtected()
  {
    return getFunction().isProtected();
  }

  public bool isStatic()
  {
    return getFunction().isStatic();
  }

  public bool isConstructor()
  {
    return false;
  }

  public bool isDestructor()
  {
    return false;
  }

  public int getModifiers()
  {
    int flag = 1024 * 64; //2^6, some out-of-the-blue number?

    if (isProtected())
      flag |= IS_PROTECTED;
    //else if (isPrivate())
      //flag |= IS_PRIVATE;
    else if (isPublic())
      flag |= IS_PUBLIC;

    if (isFinal())
      flag |= IS_FINAL;
    if (isAbstract())
      flag |= IS_ABSTRACT;
    if (isStatic())
      flag |= IS_STATIC;

    return flag;
  }

  public ReflectionClass getDeclaringClass(Env env)
  {
    string clsName = getFunction().getDeclaringClassName();

    if (clsName == null)
      throw new QuercusException(
          L.l("class name @is null {0}: {1}",
              getFunction(), getFunction().getClass()));

    return new ReflectionClass(env, clsName);
  }

  @Override
  public ArrayValue getParameters(Env env)
  {
    ArrayValue array = new ArrayValueImpl();

    AbstractFunction fun = getFunction();
    Arg []args = fun.getArgs(env);

    for (int i = 0; i < args.length; i++) {
      array.put(env.wrapJava(new ReflectionParameter(_clsName, fun, args[i])));
    }

    return array;
  }

  public void setAccessible(boolean isAccessible)
  {
    // XXX: always accessible from Reflection for quercus
  }

  private AbstractFunction getFunction()
  {
    return (AbstractFunction) getCallable();
  }

  public string toString()
  {
    string name;

    if (_clsName != null)
      name = _clsName + "->" + getName();
    else
      name = getName();

    return getClass().getSimpleName() + "[" + name + "]";
  }
}
}
