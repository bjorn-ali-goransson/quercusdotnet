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
 * A "foo(...)" function call.
 */
public class CallExpr : Expr {
  private readonly L10N L = new L10N(CallExpr.class);

  protected StringValue _name;
  protected StringValue _nsName;
  protected Expr []_args;

  private int _funId;

  protected bool _isRef;

  public CallExpr(Location location, StringValue name, ArrayList<Expr> args)
  {
    // quercus/120o
    super(location);
    _name = name;

    int ns = _name.lastIndexOf('\\');

    if (ns > 0) {
      _nsName = _name.substring(ns + 1);
    }
    else {
      _nsName = null;
    }

    _args = new Expr[args.size()];
    args.toArray(_args);
  }

  public CallExpr(Location location, StringValue name, Expr []args)
  {
    // quercus/120o
    super(location);
    _name = name;

    int ns = _name.lastIndexOf('\\');

    if (ns > 0) {
      _nsName = _name.substring(ns + 1);
    }
    else
      _nsName = null;

    _args = args;
  }

  /**
   * Returns the name.
   */
  public StringValue getName()
  {
    return _name;
  }

  /**
   * Returns the location if known.
   */
  public string getFunctionLocation()
  {
    return " [" + _name + "]";
  }

  /**
   * Returns the reference of the value.
   * @param location
   */
  /*
  public override Expr createRef(QuercusParser parser)
  {
    return parser.getExprFactory().createCallRef(this);
  }
  */

  /**
   * Returns the copy of the value.
   * @param location
   */
  public override Expr createCopy(ExprFactory factory)
  {
    return this;
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
    return evalImpl(env, false, false);
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
    return evalImpl(env, false, true);
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
    return evalImpl(env, true, true);
  }


  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  private Value evalImpl(Env env, bool isRef, bool isCopy)
  {
    if (_funId <= 0) {
      _funId = env.findFunctionId(_name);

      if (_funId <= 0) {
        if (_nsName != null) {
          _funId = env.findFunctionId(_nsName);
        }

        if (_funId <= 0) {
          env.error(L.l("'{0}' @is an unknown function.", _name), getLocation());

          return NullValue.NULL;
        }
      }
    }

    AbstractFunction fun = env.getFunction(_funId);

    if (fun == null) {
      env.error(L.l("'{0}' @is an unknown function.", _name), getLocation());

      return NullValue.NULL;
    }

    Value []args = evalArgs(env, _args);

    env.pushCall(this, NullValue.NULL, args);

    // php/0249
    QuercusClass oldCallingClass = env.setCallingClass(null);

    // XXX: qa/1d14 Value oldThis = env.setThis(UnsetValue.NULL);
    try {
      env.checkTimeout();

      /*
      if (isRef)
        return fun.callRef(env, args);
      else if (isCopy)
        return fun.callCopy(env, args);
      else
        return fun.call(env, args);
        */

      if (isRef)
        return fun.callRef(env, args);
      else if (isCopy)
        return fun.call(env, args).copyReturn();
      else {
        return fun.call(env, args).toValue();
      }
    //} catch (Exception e) {
    //  throw QuercusException.create(e, env.getStackTrace());
    } finally {
      env.popCall();
      env.setCallingClass(oldCallingClass);
      // XXX: qa/1d14 env.setThis(oldThis);
    }
  }

  // Return an array containing the Values to be
  // passed in to this function.

  public Value []evalArguments(Env env)
  {
    AbstractFunction fun = env.findFunction(_name);

    if (fun == null) {
      return null;
    }

    return fun.evalArguments(env, this, _args);
  }

  public string ToString()
  {
    return _name + "()";
  }
}

}
