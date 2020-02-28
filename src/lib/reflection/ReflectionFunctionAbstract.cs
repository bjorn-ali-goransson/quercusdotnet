using System;
namespace QuercusDotNet.lib.reflection {
/*
 * Copyright (c) 1998-2012 Caucho Technology -- all rights reserved
 *
 * This file is part of Resin(R) Open Source
 *
 * Each copy or derived work must preserve the copyright notice and this
 * notice unmodified.
 *
 * Resin Open Source is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * Resin Open Source is distributed in the hope that it will be useful,
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












public abstract class ReflectionFunctionAbstract
{
  private Callable _callable;

  protected ReflectionFunctionAbstract(Callable callable)
  {
    _callable = callable;

    if (callable == null) {
      throw new NullPointerException();
    }
  }

  protected Callable getCallable()
  {
    return _callable;
  }

  private void __clone()
  {
  }

  public string getName()
  {
    return _callable.getCallbackName();
  }

  public bool isInternal()
  {
    return false;
  }

  public bool isUserDefined()
  {
    return false;
  }

  public string getFileName(Env env)
  {
    return _callable.getDeclFileName(env);
  }

  public int getStartLine(Env env)
  {
    return _callable.getDeclStartLine(env);
  }

  public int getEndLine(Env env)
  {
    // TODO
    return _callable.getDeclEndLine(env);
  }

  @ReturnNullAsFalse
  public string getDocComment(Env env)
  {
    return _callable.getDeclComment(env);
  }

  public ArrayValue getStaticVariables()
  {
    // TODO
    return null;
  }

  public bool returnsReference(Env env)
  {
    return _callable.isReturnsReference(env);
  }

  public ArrayValue getParameters(Env env)
  {
    ArrayValue array = new ArrayValueImpl();

    Arg []args = _callable.getArgs(env);

    for (int i = 0; i < args.length; i++) {
      array.put(env.wrapJava(new ReflectionParameter(_callable, args[i])));
    }

    return array;
  }

  public int getNumberOfParameters(Env env)
  {
    return _callable.getArgs(env).length;
  }

  public int getNumberOfRequiredParameters(Env env)
  {
    Arg []args = _callable.getArgs(env);

    int requiredParams = 0;
    for (int i = 0; i < args.length; i++) {
      if (args[i].getDefault() instanceof ParamRequiredExpr)
        requiredParams++;
    }

    return requiredParams;
  }
}
}
