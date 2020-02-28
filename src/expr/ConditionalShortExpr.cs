using System;
namespace QuercusDotNet.Expr{
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
 * Represents a conditional expression.
 */
public class ConditionalShortExpr : Expr {
  protected Expr _test;
  protected Expr _falseExpr;

  public ConditionalShortExpr(Expr test, Expr falseExpr)
  {
    _test = test;

    _falseExpr = falseExpr;
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
    Value value = _test.eval(env);

    if (value.toBoolean())
      return value.copy(); // php/03cj, php/03ck
    else
      return _falseExpr.evalCopy(env); // php/03cl
  }

  public string toString()
  {
    return "(" + _test + " ?: " + _falseExpr + ")";
  }
}

}
