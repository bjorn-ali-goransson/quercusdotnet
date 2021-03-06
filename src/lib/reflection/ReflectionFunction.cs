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











public class ReflectionFunction : ReflectionFunctionAbstract
  : Reflector
{
  public const int IS_DEPRECATED = 1024 * 256; //262144;  //2^18

  protected readonly L10N L = new L10N(ReflectionFunction.class);

  protected ReflectionFunction(Callable callable)
  {
    super(callable);
  }

  private void __clone()
  {
  }

  public static ReflectionFunction __construct(Env env, Value nameV)
  {
    Callable callable;

    if (nameV instanceof Callable) {
      callable = (Callable) nameV;
    }
    else {
      AbstractFunction fun = env.findFunction(nameV.ToStringValue(env));

      if (fun == null) {
        env.error(L.l("function '{0}' does not exist", nameV));
      }

      callable = (Callable) fun;
    }

    return new ReflectionFunction(callable);
  }

  public Value export(Env env,
                      string name,
                      @Optional bool isReturn)
  {
    return null;
  }

  public Value invoke(Env env, Value []args)
  {
    return getCallable().call(env, args);
  }

  public Value invokeArgs(Env env, ArrayValue args)
  {
    return getCallable().call(env, args.getValueArray(env));
  }

  public override string ToString()
  {
    return getClass().getSimpleName() + "[" + getName() + "]";
  }
}
}
