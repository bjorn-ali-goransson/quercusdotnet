namespace QuercusDotNet.Marshal{
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
 * @author Scott Ferguson
 */








/**
 * Code for marshaling (PHP to Java) and unmarshaling (Java to PHP) arguments.
 */
public class StringMarshal : Marshal {
  public readonly StringMarshal MARSHAL = new StringMarshal();

  @Override
  public boolean isString()
  {
    return true;
  }

  public override boolean isReadOnly()
  {
    return true;
  }

  public override Object marshal(Env env, Expr expr, Class expectedClass)
  {
    return expr.evalString(env);
  }

  public override Object marshal(Env env, Value value, Class expectedClass)
  {
    return value.toJavaString();
  }

  public override Value unmarshal(Env env, Object value)
  {
    if (value == null)
      return NullValue.NULL;
    else
      return env.createString((String) value);
  }

  protected override int getMarshalingCostImpl(Value argValue)
  {
    return argValue.toStringMarshalCost();
    /*
    if (argValue.isString()) {
      if (argValue.isUnicode())
        return Marshal.UNICODE_STRING_COST;
      else if (argValue.isBinary())
        return Marshal.BINARY_STRING_COST;
      else
        return Marshal.PHP5_STRING_COST;
    }
    else if (! (argValue.isArray() || argValue.isObject()))
      return Marshal.THREE;
    else
      return Marshal.FOUR;
    */
  }

  public override int getMarshalingCost(Expr expr)
  {
    if (expr.isString())
      return Marshal.ZERO;
    else
      return Marshal.FOUR;
  }

  public override Class getExpectedClass()
  {
    return String.class;
  }
}
}
