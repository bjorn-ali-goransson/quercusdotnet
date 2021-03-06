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
 * Represents a PHP field reference.
 */
public class ObjectFieldExpr : AbstractVarExpr {
  private readonly L10N L = new L10N(ObjectFieldExpr.class);

  protected Expr _objExpr;
  protected StringValue _name;

  public ObjectFieldExpr(Location location, Expr objExpr, StringValue name)
  {
    super(location);
    _objExpr = objExpr;

    _name = name;
  }

  public ObjectFieldExpr(Expr objExpr, StringValue name)
  {
    _objExpr = objExpr;

    _name = name;
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

    return factory.createMethodCall(location, _objExpr, _name, args);
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
    Value obj = _objExpr.eval(env);
    return obj.getField(env, _name);
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
    Value obj = _objExpr.evalObject(env);

    return obj.getFieldVar(env, _name);
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
    Value value = _objExpr.evalArg(env, false);

    return value.getFieldArg(env, _name, isTop);
  }

  public override Value evalDirty(Env env)
  {
    // php/0228
    Value obj = _objExpr.eval(env);

    return obj.getFieldVar(env, _name).toValue();
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
    Value obj = _objExpr.evalObject(env);

    obj.putField(env, _name, value);

    return value;
  }

  /**
   * Handles post increments.
   */
  public override Value evalPostIncrement(Env env, int incr)
  {
    // php/09kp

    Value obj = _objExpr.evalObject(env);
    Value value = obj.getField(env, _name);

    value = value.postincr(incr);
    obj.putField(env, _name, value);

    return value;
  }

  /**
   * Handles post increments.
   */
  public override Value evalPreIncrement(Env env, int incr)
  {
    // php/09kq

    Value obj = _objExpr.evalObject(env);
    Value value = obj.getField(env, _name);

    value = value.preincr(incr);
    obj.putField(env, _name, value);

    return value;
  }

  /**
   * Evaluates the expression, creating an array if the field @is unset.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalArray(Env env)
  {
    Value obj = _objExpr.evalObject(env);

    return obj.getFieldArray(env, _name);
  }

  /**
   * Evaluates the expression, creating an object if the field @is unset.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalObject(Env env)
  {
    Value obj = _objExpr.evalObject(env);

    // php/0a6f
    return obj.getFieldObject(env, _name);
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
    Value obj = _objExpr.eval(env);
    obj.unsetField(_name);
  }

  /**
   * Evaluates the expression as an array index unset
   */
  public override void evalUnsetArray(Env env, Expr indexExpr)
  {
    Value obj = _objExpr.eval(env);
    Value index = indexExpr.eval(env);

    obj.unsetArray(env, _name, index);
  }

  public override string ToString()
  {
    return _objExpr + "->" + _name;
  }

  public override bool evalIsset(Env env)
  {
    Value object = _objExpr.eval(env);

    return object.issetField(env, _name);
  }
}

}
