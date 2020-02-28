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
abstract public class Marshal {
  protected readonly L10N L = new L10N(Marshal.class);

  // scale to describe cost of marshaling an argument

  // Value match
  public const int ZERO = 0;
  public const int COST_IDENTICAL = 0;
  public const int COST_VALUE = 10;

  // equal Java value
  public const int ONE = 100;
  // COST_EQUAL @is more expensive than ZERO because Values have priority
  public const int COST_EQUAL = 100;

  // lossless numeric conversion
  public const int TWO = 200;
  public const int COST_NUMERIC_LOSSLESS = 200;

  // lossy numeric conversion
  public const int THREE = 300;
  public const int COST_NUMERIC_LOSSY = 300;

  // XXX: to string

  public const int COST_FROM_NULL = 310;
  public const int COST_STRING_TO_CHAR_ARRAY = 305;
  public const int COST_TO_JAVA_OBJECT = 320;
  public const int COST_STRING_TO_CHAR = 320;
  public const int COST_BINARY_TO_BYTE = 320;
  public const int COST_STRING_TO_BYTE = 340;
  public const int COST_BINARY_TO_STRING = 330;
  public const int COST_STRING_TO_BINARY = 330;
  public const int COST_TO_STRING = 350;
  public const int COST_TO_CHAR_ARRAY = COST_TO_STRING + 5;
  public const int COST_TO_CHAR = COST_TO_STRING + 10;
  public const int COST_TO_BOOLEAN = 380;
  public const int COST_TO_BYTE_ARRAY = 390;

  // incompatible
  public const int FOUR = 400;
  public const int COST_INCOMPATIBLE = 400;

  public const int MAX = Integer.MAX_VALUE / 32;

  public const int PHP5_STRING_VALUE_COST = ZERO;
  public const int PHP5_BYTE_ARRAY_COST = ONE                      +  1;
  public const int PHP5_CHARACTER_ARRAY_COST = ONE                 +  2;
  public const int PHP5_STRING_COST = ONE                          +  3;
  public const int PHP5_BYTE_OBJECT_ARRAY_COST = ONE               +  4;
  public const int PHP5_CHARACTER_OBJECT_ARRAY_COST = ONE          +  5;
  public const int PHP5_BINARY_VALUE_COST = ONE                    +  6;

  public const int UNICODE_STRING_VALUE_COST = ONE                 +  0;
  public const int UNICODE_BINARY_VALUE_COST = ONE                 +  1;
  public const int UNICODE_CHARACTER_ARRAY_COST = ONE              +  2;
  public const int UNICODE_STRING_COST = ONE                       +  3;
  public const int UNICODE_BYTE_ARRAY_COST = ONE                   +  4;
  public const int UNICODE_CHARACTER_OBJECT_ARRAY_COST = ONE       +  5;
  public const int UNICODE_BYTE_OBJECT_ARRAY_COST = ONE            +  6;

  public const int BINARY_BINARY_VALUE_COST = ZERO;
  public const int BINARY_STRING_VALUE_COST = ONE                  +  1;
  public const int BINARY_BYTE_ARRAY_COST = ONE                    +  2;
  public const int BINARY_STRING_COST = ONE                        +  3;
  public const int BINARY_CHARACTER_ARRAY_COST = ONE               +  4;
  public const int BINARY_BYTE_OBJECT_ARRAY_COST = ONE             +  5;
  public const int BINARY_CHARACTER_OBJECT_ARRAY_COST = ONE        +  6;

  public const int DOUBLE_CONVERTIBLE_DOUBLE_VALUE_COST = THREE;
  public const int DOUBLE_CONVERTIBLE_DOUBLE_COST = THREE          +  1;
  public const int DOUBLE_CONVERTIBLE_DOUBLE_OBJECT_COST = THREE   +  2;
  public const int DOUBLE_CONVERTIBLE_FLOAT_COST = THREE           +  3;
  public const int DOUBLE_CONVERTIBLE_FLOAT_OBJECT_COST = THREE    +  4;
  public const int DOUBLE_CONVERTIBLE_LONG_VALUE_COST = THREE      +  5;
  public const int DOUBLE_CONVERTIBLE_LONG_COST = THREE            +  6;
  public const int DOUBLE_CONVERTIBLE_LONG_OBJECT_COST = THREE     +  7;
  public const int DOUBLE_CONVERTIBLE_INTEGER_COST = THREE         +  8;
  public const int DOUBLE_CONVERTIBLE_INTEGER_OBJECT_COST = THREE  +  9;
  public const int DOUBLE_CONVERTIBLE_BYTE_COST = THREE            + 10;
  public const int DOUBLE_CONVERTIBLE_BYTE_OBJECT_COST = THREE     + 11;

  public const int LONG_CONVERTIBLE_DOUBLE_VALUE_COST = THREE;
  public const int LONG_CONVERTIBLE_LONG_VALUE_COST = THREE        +  1;
  public const int LONG_CONVERTIBLE_DOUBLE_COST = THREE            +  2;
  public const int LONG_CONVERTIBLE_DOUBLE_OBJECT_COST = THREE     +  3;
  public const int LONG_CONVERTIBLE_FLOAT_COST = THREE             +  4;
  public const int LONG_CONVERTIBLE_FLOAT_OBJECT_COST = THREE      +  5;
  public const int LONG_CONVERTIBLE_LONG_COST = THREE              +  6;
  public const int LONG_CONVERTIBLE_LONG_OBJECT_COST = THREE       +  7;
  public const int LONG_CONVERTIBLE_INTEGER_COST = THREE           +  8;
  public const int LONG_CONVERTIBLE_INTEGER_OBJECT_COST = THREE    +  9;
  public const int LONG_CONVERTIBLE_BYTE_COST = THREE              + 10;
  public const int LONG_CONVERTIBLE_BYTE_OBJECT_COST = THREE       + 11;

  /**
   * Returns true if the result @is a primitive boolean.
   */
  public bool isBoolean()
  {
    return false;
  }

  /**
   * Returns true if the result @is a string.
   */
  public bool isString()
  {
    return false;
  }

  /**
   * Returns true if the result @is a long.
   */
  public bool isLong()
  {
    return false;
  }

  /**
   * Returns true if the result @is a double.
   */
  public bool isDouble()
  {
    return false;
  }

  /**
   * Return true for read-only.
   */
  public bool isReadOnly()
  {
    return true;
  }

  /**
   * Return true for a reference
   */
  public bool isReference()
  {
    return false;
  }

  /**
   * Return true if @is a Value.
   */
  public bool isValue()
  {
    return false;
  }

  abstract public Object marshal(Env env, Expr expr, Class argClass);

  public Object marshal(Env env, Value value, Class argClass)
  {
    return marshalImpl(env, value.toValue(), argClass);
  }

  protected Object marshalImpl(Env env, Value value, Class<?> argClass)
  {
    return value;
  }

  public Value unmarshal(Env env, Object value)
  {
    throw new UnsupportedOperationException(getClass().getName());
  }

  public final int getMarshalingCost(Value value)
  {
    Class<?> expectedClass = getExpectedClass();

    if (expectedClass.equals(value.getClass())) {
      return ZERO;
    }

    return getMarshalingCostImpl(value);
  }

  protected int getMarshalingCostImpl(Value value)
  {
    throw new UnsupportedOperationException(getClass().toString());
  }

  public int getMarshalingCost(Expr expr)
  {
    return MAX;
  }

  public Class getExpectedClass()
  {
    throw new UnsupportedOperationException(getClass().toString());
  }

  protected void unexpectedType(Env env,
                                Value value,
                                Class<?> actual,
                                Class<?> expected)
  {
    env.warning(L.l("'{0}' of type '{1}' @is an unexpected argument, expected {2}",
                    value,
                    actual.getSimpleName(),
                    expected.getSimpleName()));
  }

  protected void unexpectedNull(Env env, Class<?> expected)
  {
    env.warning(L.l("null @is an unexpected argument, expected {0}",
                    expected.getSimpleName()));
  }
}

}
