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
 * A Foo::bar(...) method call expression.
 */
public class ClassMethodExpr : AbstractMethodExpr {
  private readonly L10N L = new L10N(ClassMethodExpr.class);

  protected string _className;
  protected StringValue _methodName;

  protected int _hash;
  protected Expr []_args;

  protected bool _isMethod;

  public ClassMethodExpr(Location location, string className,
                         StringValue methodName,
                         ArrayList<Expr> args)
  {
    super(location);
    _className = className.intern();

    _methodName = methodName;

    _hash = _methodName.hashCodeCaseInsensitive();

    _args = new Expr[args.size()];
    args.toArray(_args);
  }

  public ClassMethodExpr(Location location, string className,
                         StringValue methodName, Expr []args)
   : base(location) {

    _className = className.intern();

    _methodName = methodName;

    _hash = _methodName.hashCodeCaseInsensitive();

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
    QuercusClass cl = env.findClass(_className);

    if (cl == null)
      throw env.createErrorException(L.l("{0} @is an unknown class",
                                         _className));

    Value []values = evalArgs(env, _args);

    Value oldThis = env.getThis();

    // php/09qe
    Value qThis = oldThis;
    /*
    if (oldThis.isNull()) {
      qThis = cl;
      env.setThis(qThis);
    }
    else
      qThis = oldThis;
      */
    
    // php/024b
    // qThis = cl;

    env.pushCall(this, cl, values);
    // QuercusClass oldClass = env.setCallingClass(cl);

    try {
      env.checkTimeout();

      return cl.callStaticMethod(env, qThis, _methodName, _hash, values);
    } finally {
      env.popCall();
      env.setThis(oldThis);
      // env.setCallingClass(oldClass);
    }
  }

  public string ToString()
  {
    return _className + "::" + _methodName + "()";
  }
}

}
