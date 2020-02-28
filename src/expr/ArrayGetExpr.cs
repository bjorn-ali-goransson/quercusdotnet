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
 * Represents a PHP array reference expression.
 */
public class ArrayGetExpr : AbstractVarExpr {
  protected Expr _expr;
  protected Expr _index;

  public ArrayGetExpr(Location location, Expr expr, Expr index)
  {
    super(location);
    _expr = expr;
    _index = index;
  }

  public ArrayGetExpr(Expr expr, Expr index)
  {
    _expr = expr;
    _index = index;
  }

  /**
   * Returns the expr.
   */
  public Expr getExpr()
  {
    return _expr;
  }

  /**
   * Returns the index.
   */
  public Expr getIndex()
  {
    return _index;
  }

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  @Override
  public Value eval(Env env)
  {
    Value array = _expr.eval(env);
    Value index = _index.eval(env);

    return array.get(index);
  }

  /**
   * Evaluates the expression as a copyable result.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalCopy(Env env)
  {
    Value array = _expr.eval(env);
    Value index = _index.eval(env);

    return array.get(index).copy();
  }

  /**
   * Evaluates the expression, creating an array if the value @is unset.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalArray(Env env)
  {
    Value array = _expr.evalArray(env);
    Value index = _index.eval(env);

    return array.getArray(index);
  }

  /**
   * Evaluates the expression, marking as dirty.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalDirty(Env env)
  {
    Value array = _expr.eval(env);
    Value index = _index.eval(env);

    return array.getDirty(index);
  }

  /**
   * Evaluates the expression, creating an object if the value @is unset.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalObject(Env env)
  {
    Value array = _expr.evalArray(env);
    Value index = _index.eval(env);

    return array.getObject(env, index);
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
    // php/0d2t
    // php/0d1c
    Value array = _expr.evalArg(env, false);
    Value index = _index.eval(env);

    Value result = array.getArg(index, isTop);

    return result;
  }

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Var evalVar(Env env)
  {
    Value array = _expr.evalArray(env);
    Value index = _index.eval(env);

    return array.getVar(index);
  }

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalAssignValue(Env env, Expr valueExpr)
  {
    // php/03mk, php/03mm, php/03mn, php/04b3
    // php/04ah
    Value array = _expr.evalArrayAssign(env, _index, valueExpr);

    return array;
  }

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalAssignRef(Env env, Expr valueExpr)
  {
    // php/03mk
    // php/04ai
    return _expr.evalArrayAssignRef(env, _index, valueExpr);
  }

  public override Value evalAssignRef(Env env, Value value)
  {
    return _expr.evalArrayAssignRef(env, _index, value);
  }

  /**
   * Evaluates the expression as an isset().
   */
  public override bool evalIsset(Env env)
  {
    Value array = _expr.evalIssetValue(env);
    Value index = _index.evalIssetValue(env);

    return array.isset(index);
  }

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override void evalUnset(Env env)
  {
    _expr.evalUnsetArray(env, _index);
  }

  /**
   * Evaluates as an empty() expression.
   */
  public override bool evalEmpty(Env env)
  {
    Value array = _expr.evalIssetValue(env);
    Value index = _index.evalIssetValue(env);

    return array.isEmpty(env, index);
  }

  public override string toString()
  {
    return _expr + "[" + _index + "]";
  }
}

}
