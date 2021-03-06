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
 * Code for marshalling arguments.
 */
public class JavaMapMarshal : JavaMarshal {
  private readonly L10N L = new L10N(JavaMapMarshal.class);

  public JavaMapMarshal(JavaClassDef def, bool isNotNull)
  {
    this(def, isNotNull, false);
  }

  public JavaMapMarshal(JavaClassDef def,
                        bool isNotNull,
                        bool isUnmarshalNullAsFalse)
  {
    super(def, isNotNull, isUnmarshalNullAsFalse);
  }

  public override Object marshal(Env env, Value value, Class argClass)
  {
    if (! value.isset()) {
      if (_isNotNull) {
        unexpectedNull(env, argClass);
      }

      return null;
    }

    Object obj = value.toJavaMap(env, argClass);

    if (obj == null) {
      if (_isNotNull) {
        unexpectedNull(env, argClass);
      }

      return null;
    }
    else if (! argClass.isAssignableFrom(obj.getClass())) {
      unexpectedType(env, value, obj.getClass(), argClass);

      return null;
    }

    return obj;
  }

  protected override int getMarshalingCostImpl(Value argValue)
  {
    if (argValue instanceof JavaMapAdapter
        && getExpectedClass().isAssignableFrom(argValue.toJavaObject().getClass()))
      return  Marshal.Marshal.ZERO;
    else if (argValue.isArray())
      return  Marshal.Marshal.THREE;
    else
      return  Marshal.Marshal.FOUR;
  }
}

}
