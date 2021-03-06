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
 * Represents a PHP reference argument.
 */
public class UnaryRefExpr : AbstractUnaryExpr {
  public UnaryRefExpr(Location location, Expr expr)
  {
    super(location, expr);
  }

  public UnaryRefExpr(Expr expr)
   : base(expr) {
  }

  /**
   * Returns true for a reference.
   */
  public bool isRef()
  {
    return true;
  }
  
  /**
   * Creates an assignment using this value as the right hand side.
   */
  public override Expr createAssignFrom(QuercusParser parser,
                               AbstractVarExpr leftHandSide)
  {
    ExprFactory factory = parser.getExprFactory();
    
    return factory.createAssignRef(leftHandSide, _expr);
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
    // quercus/0d28
    Value value = getExpr().evalVar(env);
    
    return value.toRef();
  }

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalArg(Env env, bool isTop)
  {
    Value value = getExpr().evalVar(env);
    
    return value.toArgRef();
  }

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public Var evalVar(Env env)
  {
    Var value = getExpr().evalVar(env);

    // php/112d
    return value;
    /*
    if (value instanceof Var)
      return new RefVar((Var) value);
    else
      return value;
    */
  }

  public string ToString()
  {
    return _expr.ToString();
  }
}

}
