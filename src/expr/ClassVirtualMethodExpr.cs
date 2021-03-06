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
 * Represents a PHP parent:: method call expression.
 * XXX: better name?
 */
public class ClassVirtualMethodExpr : Expr {
  private readonly L10N L = new L10N(ClassVirtualMethodExpr.class);

  protected StringValue _methodName;

  private int _hash;
  protected Expr []_args;

  protected bool _isMethod;

  public ClassVirtualMethodExpr(Location location,
                                StringValue methodName,
                                ArrayList<Expr> args)
  {
    super(location);

    _methodName = methodName;
    _hash = methodName.hashCodeCaseInsensitive();

    _args = new Expr[args.size()];
    args.toArray(_args);
  }

  public ClassVirtualMethodExpr(Location location,
                                StringValue name,
                                Expr []args)
   : base(location) {

    _methodName =  name;
    _hash = name.hashCodeCaseInsensitive();

    _args = args;
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
    Value qThis = env.getThis();

    QuercusClass cls = qThis.getQuercusClass();

    if (cls == null) {
      env.error(L.l("no calling class found"), getLocation());

      return NullValue.NULL;
    }

    Value []values = evalArgs(env, _args);

    env.pushCall(this, cls, values);

    try {
      env.checkTimeout();

      return cls.callMethod(env, qThis, _methodName, _hash, values);
    } finally {
      env.popCall();
    }
  }

  public string ToString()
  {
    return "static::" + _methodName + "()";
  }
}

}
