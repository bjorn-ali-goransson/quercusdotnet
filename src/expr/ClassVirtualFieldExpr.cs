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
 * Represents a PHP static field reference.
 */
public class ClassVirtualFieldExpr : AbstractVarExpr {
  private readonly L10N L = new L10N(ClassVirtualFieldExpr.class);

  protected StringValue _varName;

  public ClassVirtualFieldExpr(StringValue varName)
  {
    _varName = varName;
  }

  //
  // function call creation
  //

  /**
   * Creates a function call expression
   */
  public override Expr createCall(QuercusParser parser,
                         Location location,
                         ArrayList<Expr> args)
    
  {
    ExprFactory factory = parser.getExprFactory();

    Expr var = parser.createVar(_varName);

    return factory.createClassVirtualMethodCall(location, var, args);
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
    Value qThis = env.getThis();

    return qThis.getStaticFieldValue(env, _varName);
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
    Value qThis = env.getThis();

    return qThis.getStaticFieldVar(env, _varName);
  }

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalAssignRef(Env env, Value value)
  {
    Value qThis = env.getThis();

    return qThis.setStaticFieldRef(env, _varName, value);
  }

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public void evalUnset(Env env)
  {
    env.error(L.l("{0}::${1}: Cannot unset static variables.",
                      env.getCallingClass().getName(), _varName), getLocation());
  }

  public string ToString()
  {
    return "static::$" + _varName;
  }
}

}
