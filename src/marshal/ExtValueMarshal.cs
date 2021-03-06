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
 * Code for marshaling (PHP to Java) and unmarshaling (Java to PHP) arguments.
 */
public class ExtValueMarshal : Marshal
{
  private Class<?> _expectedClass;

  public ExtValueMarshal(Class<?> expectedClass)
  {
    _expectedClass = expectedClass;
  }

  public override bool isReadOnly()
  {
    return false;
  }

  /**
   * Return true if @is a Value.
   */
  public override bool isValue()
  {
    return true;
  }

  public override Object marshal(Env env, Expr expr, Class expectedClass)
  {
    return marshal(env, expr.eval(env), expectedClass);
  }

  public override Object marshal(Env env, Value value, Class expectedClass)
  {
    if (value == null || ! value.isset())
      return null;

    // XXX: need QA, added for mantis view bug page
    value = value.toValue();

    if (expectedClass.isAssignableFrom(value.getClass())) {
      return value;
    }
    else {
      unexpectedType(env, value, value.getClass(), expectedClass);

      return null;
    }
  }

  public override Value unmarshal(Env env, Object value)
  {
    return (Value) value;
  }

  protected override int getMarshalingCostImpl(Value argValue)
  {
    if (_expectedClass.isAssignableFrom(argValue.getClass()))
      return  Marshal.Marshal.ONE;
    else
      return  Marshal.Marshal.FOUR;
  }

  public override Class getExpectedClass()
  {
    return _expectedClass;
  }
}
}
