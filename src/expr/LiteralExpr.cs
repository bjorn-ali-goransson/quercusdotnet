using System;
namespace QuercusDotNet.Expr{
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
 * Represents a literal expression.
 */
public class LiteralExpr : Expr {
  private final Value _value;

  public LiteralExpr(Value value)
  {
    _value = value;
  }

  protected Value getValue()
  {
    return _value;
  }

  /**
   * Returns true for a literal expression.
   */
  @Override
  public bool isLiteral()
  {
    return true;
  }

  /**
   * Returns true if a static true value.
   */
  public override bool isTrue()
  {
    if (_value == BooleanValue.TRUE)
      return true;
    else if (_value instanceof LongValue)
      return _value.toLong() != 0;
    else
      return false;
  }

  /**
   * Returns true if a static true value.
   */
  public override bool isFalse()
  {
    if (_value == BooleanValue.FALSE)
      return true;
    else if (_value instanceof LongValue)
      return _value.toLong() == 0;
    else
      return false;
  }

  /**
   * Returns true for a long value.
   */
  public bool isLong()
  {
    return _value.isLongConvertible();
  }

  /**
   * Returns true for a double value.
   */
  public bool isDouble()
  {
    return _value.isDoubleConvertible();
  }

  /*
   *
   */
  public bool isBoolean()
  {
    return _value.isBoolean();
  }

  /**
   * Evaluates the expression as a constant.
   *
   * @return the expression value.
   */
  public Value evalConstant()
  {
    return _value;
  }

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value eval(Env env)
  {
    return _value;
  }

  public override QuercusClass evalQuercusClass(Env env)
  {
    string className = evalString(env);

    return env.getClass(className);
  }

  public string toString()
  {
    return _value.toString();
  }
}

}
