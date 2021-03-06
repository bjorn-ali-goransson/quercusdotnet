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










/**
 * Code for marshaling arguments.
 */
public class JavaMarshal : Marshal {
  private readonly L10N L = new L10N(JavaMarshal.class);

  protected JavaClassDef _def;
  protected bool _isNotNull;
  protected bool _isUnmarshalNullAsFalse;

  public JavaMarshal(JavaClassDef def,
                      bool isNotNull)
  {
    this(def, isNotNull, false);
  }

  public JavaMarshal(JavaClassDef def,
                      bool isNotNull,
                      bool isUnmarshalNullAsFalse)
  {
    _def = def;
    _isNotNull = isNotNull;
    _isUnmarshalNullAsFalse = isUnmarshalNullAsFalse;
  }

  public override Object marshal(Env env, Expr expr, Class argClass)
  {
    Value value = expr.eval(env);

    return marshal(env, value, argClass);
  }

  public override Object marshal(Env env, Value value, Class argClass)
  {
    if (! value.isset()) {
      if (_isNotNull) {
        unexpectedNull(env, argClass);
      }

      return null;
    }

    Object obj = value.toJavaObject();

    if (obj == null) {
      if (_isNotNull) {
        unexpectedNull(env, argClass);
      }

      return null;
    }
    else if (! argClass.isAssignableFrom(obj.getClass())) {
      //env.error(L.l("Can't assign {0} to {1}", obj, argClass));
      unexpectedType(env, value, obj.getClass(), argClass);
      return null;
    }

    return obj;
  }

  public override Value unmarshal(Env env, Object value)
  {
    return env.wrapJava(value, _def, _isUnmarshalNullAsFalse);
  }

  protected override int getMarshalingCostImpl(Value argValue)
  {
    Class type = _def.getType();

    if (argValue instanceof JavaValue
        && type.isAssignableFrom(argValue.toJavaObject().getClass()))
      return  Marshal.Marshal.ZERO;
    else
      return  Marshal.Marshal.FOUR;
  }

  public override class getExpectedClass()
  {
    return _def.getType();
  }
}

}
