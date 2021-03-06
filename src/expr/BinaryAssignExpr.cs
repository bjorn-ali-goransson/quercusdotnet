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
 * Represents a PHP assignment expression.
 */
public class BinaryAssignExpr : Expr {
  protected AbstractVarExpr _var;
  protected Expr _value;

  public BinaryAssignExpr(Location location, AbstractVarExpr var, Expr value)
  {
    super(location);

    _var = var;
    _value = value;
  }

  public BinaryAssignExpr(AbstractVarExpr var, Expr value)
  {
    _var = var;
    _value = value;
  }

  /**
   * Creates a assignment
   * @param location
   */
  public override Expr createCopy(ExprFactory factory)
  {
    // quercus/3d9e
    return factory.createCopy(this);
  }

  /**
   * Returns true if a static false value.
   */
  public override bool isAssign()
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
  public override Value eval(Env env)
  {
    return _var.evalAssignValue(env, _value);
  }

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalCopy(Env env)
  {
    // php/0d9e
    return eval(env).copy();
  }

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalRef(Env env)
  {
    Value value = _value.evalCopy(env);

    _var.evalAssignValue(env, value);

    // php/03d9, php/03mk
    return _var.eval(env);
  }

  public string ToString()
  {
    return _var + "=" + _value;
  }
}

}
