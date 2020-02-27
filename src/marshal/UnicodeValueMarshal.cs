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









public class UnicodeValueMarshal extends Marshal
{
  public static final Marshal MARSHAL = new UnicodeValueMarshal();
  
  public boolean isReadOnly()
  {
    return true;
  }
  
  /**
   * Return true if is a Value.
   */
  @Override
  public boolean isValue()
  {
    return true;
  }

  public Object marshal(Env env, Expr expr, Class expectedClass)
  {
    return expr.eval(env).toUnicodeValue(env);
  }

  public Object marshal(Env env, Value value, Class expectedClass)
  {
    return value.toUnicodeValue(env);
  }

  public Value unmarshal(Env env, Object value)
  {
    if (value instanceof UnicodeValue)
      return (UnicodeValue) value;
    else if (value instanceof Value)
      return ((Value) value).toUnicodeValue(env);
    else
      return env.createString(String.valueOf(value));
  }
  
  @Override
  protected int getMarshalingCostImpl(Value argValue)
  {
    return argValue.toUnicodeValueMarshalCost();

    /*
    if (argValue.isUnicode())
      return Marshal.ZERO;
    else if (argValue.isString())
      return Marshal.TWO;
    else if (! (argValue.isArray() || argValue.isObject()))
      return Marshal.THREE;
    else
      return Marshal.FOUR;
    */
  }
  
  @Override
  public Class getExpectedClass()
  {
    return UnicodeValue.class;
  }
}
