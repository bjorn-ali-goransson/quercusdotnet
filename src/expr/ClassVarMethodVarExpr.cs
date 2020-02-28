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
 * Represents a PHP static method expression ${class}:${foo}(...).
 */
public class ClassVarMethodVarExpr : Expr
{
  private readonly L10N L = new L10N(ClassVarMethodVarExpr.class);

  protected Expr _className;
  protected Expr _methodName;
  protected Expr []_args;

  protected Expr []_fullArgs;

  protected AbstractFunction _fun;
  protected bool _isMethod;

  public ClassVarMethodVarExpr(Location location,
                               Expr className,
                               Expr methodName,
                               ArrayList<Expr> args)
  {
    super(location);

    _className = className;
    _methodName = methodName;

    _args = new Expr[args.size()];
    args.toArray(_args);
  }

  //
  // expr creation
  //

  /**
   * Returns the reference of the value.
   * @param location
   */
  @Override
  public Expr createRef(QuercusParser parser)
  {
    return parser.getFactory().createRef(this);
  }

  /**
   * Returns the copy of the value.
   * @param location
   */
  public override Expr createCopy(ExprFactory factory)
  {
    return factory.createCopy(this);
  }

  //
  // evaluation
  //

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value eval(Env env)
  {
    string className = _className.evalString(env);

    QuercusClass cl = env.findClass(className);

    if (cl == null) {
      env.error(L.l("no matching class {0}", className), getLocation());
    }

    StringValue methodName = _methodName.evalStringValue(env);
    int hash = methodName.hashCodeCaseInsensitive();
    Value []args = evalArgs(env, _args);

    return cl.callMethod(env, env.getThis(),
                         methodName, hash,
                         args);
  }

  public string toString()
  {
    return _className + "::" + _methodName + "()";
  }
}

}
