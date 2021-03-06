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
 * @author Nam Nguyen
 */












/**
 * Represents a PHP function expression of the form "new static()".
 */
public class ObjectNewStaticExpr : Expr {
  private readonly L10N L = new L10N(ObjectNewStaticExpr.class);
  protected Expr []_args;

  public ObjectNewStaticExpr(Location location, ArrayList<Expr> args)
  {
    super(location);

    _args = new Expr[args.size()];
    args.toArray(_args);
  }

  public ObjectNewStaticExpr(Location location, Expr []args)
   : base(location) {
    _args = args;
  }

  public ObjectNewStaticExpr(ArrayList<Expr> args)
  {
    this(Location.UNKNOWN, args);
  }

  public ObjectNewStaticExpr(Expr []args)
  {
    this(Location.UNKNOWN, args);
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
    Value []args = new Value[_args.length];

    for (int i = 0; i < args.length; i++) {
      args[i] = _args[i].evalArg(env, true);
    }

    env.pushCall(this, NullValue.NULL, args);

    try {
      QuercusClass cl = env.getCallingClass();

      env.checkTimeout();

      return cl.callNew(env, args);
    } finally {
      env.popCall();
    }
  }

  public string ToString()
  {
    return "new static()";
  }
}
}
