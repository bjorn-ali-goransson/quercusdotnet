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
 * Represents an expression that @is assignable
 */
abstract public class AbstractVarExpr : Expr {
  public AbstractVarExpr(Location location)
  {
    super(location);
  }

  public AbstractVarExpr()
  {
  }


  /**
   * Returns true if the expression @is a var/left-hand-side.
   */
  public override bool isVar()
  {
    return true;
  }

  /**
   * Marks the value as assigned
   */
  public void assign(QuercusParser parser)
  {
    // XXX: used by list, e.g. quercus/03l8.  need further tests
  }

  /**
   * Creates the assignment.
   */
  public override Expr createAssign(QuercusParser parser, Expr value)
  {
    return value.createAssignFrom(parser, this);
  }

  /**
   * Creates the assignment.
   */
  public override Expr createAssignRef(QuercusParser parser,
                              Expr value)
  {
    return parser.getExprFactory().createAssignRef(this, value);
  }

  /**
   * Creates the reference
   * @param location
   */
  public override Expr createRef(QuercusParser parser)
  {
    return parser.getExprFactory().createRef(this);
  }

  /**
   * Creates the copy.
   * @param location
   */
  public override Expr createCopy(ExprFactory factory)
  {
    return factory.createCopy(this);
  }

  /**
   * Creates the assignment.
   */
  public override Statement createUnset(ExprFactory factory, Location location)
  {
    return factory.createExpr(location, factory.createUnsetVar(this));
  }

  /**
   * Evaluates the expression, returning a Value.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  abstract override public Value eval(Env env);

  /**
   * Evaluates the expression as a reference (by RefExpr).
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  abstract override public Var evalVar(Env env);

  /**
   * Evaluates the expression as a reference when possible.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalRef(Env env)
  {
    return evalVar(env);
  }

  /**
   * Evaluates the expression as an argument.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalArg(Env env, bool isTop)
  {
    return evalVar(env);
  }

  /**
   * Evaluates the expression and copies the result for an assignment.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalCopy(Env env)
  {
    return eval(env).copy();
  }

  /**
   * Evaluates the expression as an array.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalArray(Env env)
  {
    return evalVar(env).toAutoArray();
  }

  /**
   * Evaluates the expression as an object.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalObject(Env env)
  {
    return evalVar(env).toObject(env);
  }

  /**
   * Evaluates the expression as an argument.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  abstract public void evalUnset(Env env);

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalAssignValue(Env env, Value value)
  {
    return evalAssignRef(env, value);
  }

  /**
   * Assign the variable with a new reference value.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  abstract public Value evalAssignRef(Env env, Value value);
}

}
