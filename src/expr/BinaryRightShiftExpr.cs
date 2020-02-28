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
 * Represents a PHP right shift expression.
 */
public class BinaryRightShiftExpr : AbstractBinaryExpr {
  public BinaryRightShiftExpr(Location location, Expr left, Expr right)
  {
    super(location, left, right);
  }

  public BinaryRightShiftExpr(Expr left, Expr right)
   : base(left, right) {
  }

  /**
   * Returns true for a long expression.
   */
  public bool isLong()
  {
    return true;
  }

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public Value eval(Env env)
  {
    Value lValue = _left.eval(env);
    Value rValue = _right.eval(env);

    return lValue.rshift(rValue);
  }

  /**
   * Evaluates the expression as a long.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public long evalLong(Env env)
  {
    Value lValue = _left.eval(env);
    Value rValue = _right.eval(env);

    return lValue.toLong() >> rValue.toLong();
  }

  public string toString()
  {
    return "(" + _left + " >> " + _right + ")";
  }
}

}
