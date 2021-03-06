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
 * A "$foo(...)" function call.
 */
public class CallVarExpr : Expr {
  private readonly L10N L = new L10N(CallExpr.class);
  
  protected Expr _name;
  protected Expr []_args;

  public CallVarExpr(Location location, Expr name, ArrayList<Expr> args)
  {
    super(location);
    _name = name;

    _args = new Expr[args.size()];
    args.toArray(_args);
  }

  public CallVarExpr(Location location, Expr name, Expr []args)
   : base(location) {
    _name = name;

    _args = args;
  }

  public CallVarExpr(Expr name, ArrayList<Expr> args)
  {
    this(Location.UNKNOWN, name, args);
  }

  public CallVarExpr(Expr name, Expr []args)
  {
    this(Location.UNKNOWN, name, args);
  }

  /**
   * Returns the reference of the value.
   * @param location
   */
  public Expr createRef(QuercusParser parser)
  {
    return parser.getFactory().createRef(this);
  }

  /**
   * Returns the copy of the value.
   * @param location
   */
  public Expr createCopy(ExprFactory factory)
  {
    return this;
  }
  
  public override Value eval(Env env)
  {
    return evalImpl(env, false, false);
  }
  
  
  public override Value evalRef(Env env)
  {
    return evalImpl(env, true, false);
  }
  
  
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
  public Value evalImpl(Env env, bool isRef, bool isCopy)
  {
    Value value = _name.eval(env);
    
    Value []args = evalArgs(env, _args);

    env.pushCall(this, NullValue.NULL, null);

    try {
      env.checkTimeout();
      
      if (isRef)
        return value.callRef(env, args);
      else if (isCopy)
        return value.call(env, args).copyReturn();
      else
        return value.call(env, args).toValue();
    } finally {
      env.popCall();
    }
  }
  
  public string ToString()
  {
    return _name + "()";
  }
}

}
