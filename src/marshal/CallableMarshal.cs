using System;
namespace QuercusDotNet.Marshal{
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








public class CallableMarshal : Marshal
{
  public readonly Marshal MARSHAL = new CallableMarshal(false);
  public readonly Marshal MARSHAL_OPTIONAL = new CallableMarshal(true);

  protected bool _isOptional;

  protected CallableMarshal(bool isOptional)
  {
    _isOptional = isOptional;
  }

  public bool isReadOnly()
  {
    return true;
  }

  @Override
  public Object marshal(Env env, Expr expr, Class expectedClass)
  {
    return marshal(env, expr.eval(env), expectedClass);
  }

  protected override Object marshalImpl(Env env, Value value, Class<?> expectedClass)
  {
    Callable callable = value.toCallable(env, _isOptional);

    return callable;
  }

  public Value unmarshal(Env env, Object value)
  {
    throw new UnsupportedOperationException();
  }

  protected override int getMarshalingCostImpl(Value argValue)
  {
    if (argValue instanceof Callable) {
      return Marshal.ZERO;
    }
    else if (argValue.isString()) {
      return Marshal.THREE;
    }
    else {
      return Marshal.FOUR;
    }
  }

  public override Class getExpectedClass()
  {
    return Callable.class;
  }
}
}
