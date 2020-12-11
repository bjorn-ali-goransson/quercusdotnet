using System;
namespace QuercusDotNet.Expr{
/*
 * Copyright (c) 1998-2014 Caucho Technology -- all rights reserved
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
public class ThisFieldExpr : AbstractVarExpr {
  protected ThisExpr _qThis;

  protected StringValue _name;

  protected bool _isInit;

  public ThisFieldExpr(Location location,
                       ThisExpr qThis,
                       StringValue name)
  {
    super(location);

    _qThis = qThis;
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

    return factory.createThisMethod(location,
                                    _qThis, _name, args);
  }

  public void init()
  {
    /// XXX: have this called by QuercusParser after class parsing

    if (! _isInit) {
      _isInit = true;

      ClassField entry = _qThis.getClassDef().getField(_name);

      if (entry != null) {
        _name = entry.getCanonicalName();
      }
    }
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
    init();

    Value obj = env.getThis();

    if (obj.isNull()) {
      return env.thisError(getLocation());
    }

    return obj.getThisField(env, _name);
  }

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public Value evalCopy(Env env)
  {
    init();

    Value obj = env.getThis();

    if (obj.isNull()) {
      return env.thisError(getLocation());
    }

    return obj.getThisField(env, _name).copy();
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
    init();

    Value obj = env.getThis();

    if (obj.isNull()) {
      env.thisError(getLocation());

      return new Var();
    }

    return obj.getThisFieldVar(env, _name);
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
    init();

    Value obj = env.getThis();

    if (obj.isNull()) {
      return env.thisError(getLocation());
    }

    return obj.getThisFieldArg(env, _name);
  }

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value evalAssignValue(Env env, Value value)
  {
    init();

    Value obj = env.getThis();

    if (obj.isNull()) {
      return env.thisError(getLocation());
    }

    obj.putThisField(env, _name, value);

    return value;
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
    init();

    Value obj = env.getThis();

    if (obj.isNull()) {
      return env.thisError(getLocation());
    }

    obj.putThisField(env, _name, value);

    return value;
  }

  /**
   * Evaluates as an array index assign ($a[index] = value).
   */
  public override Value evalArrayAssign(Env env, Expr indexExpr, Expr valueExpr)
  {
    init();

    Value obj = env.getThis();

    if (obj.isNull()) {
      return env.thisError(getLocation());
    }

    // php/044i
    Value fieldVar = obj.getThisFieldArray(env, _name);
    Value index = indexExpr.eval(env);

    Value value = valueExpr.evalCopy(env);

    return fieldVar.putThisFieldArray(env, obj, _name, index, value);
  }

  /**
   * Evaluates as an array index assign ($a[index] = value).
   */
  public override Value evalArrayAssignRef(Env env, Expr indexExpr, Expr valueExpr)
  {
    init();

    Value obj = env.getThis();

    if (obj.isNull()) {
      return env.thisError(getLocation());
    }

    // php/044i
    Value fieldVar = obj.getThisFieldArray(env, _name);
    Value index = indexExpr.eval(env);

    Value value = valueExpr.evalRef(env);

    return fieldVar.putThisFieldArray(env, obj, _name, index, value);
  }

  /**
   * Evaluates the expression, creating an array if the value @is unset..
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public Value evalArray(Env env)
  {
    init();

    Value obj = env.getThis();

    if (obj.isNull()) {
      return env.thisError(getLocation());
    }

    return obj.getThisFieldArray(env, _name);
  }

  /**
   * Evaluates the expression, creating an array if the value @is unset..
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public Value evalObject(Env env)
  {
    init();

    Value obj = env.getThis();

    if (obj.isNull()) {
      return env.thisError(getLocation());
    }

    return obj.getThisFieldObject(env, _name);
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
    init();

    Value obj = env.getThis();

    if (obj.isNull()) {
      env.thisError(getLocation());
    }

    obj.unsetThisField(_name);
  }

  public string ToString()
  {
    return "$this->" + _name;
  }
}

}
