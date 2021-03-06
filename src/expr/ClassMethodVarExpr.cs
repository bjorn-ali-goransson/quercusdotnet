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
 * Represents a PHP static method expression.
 */
public class ClassMethodVarExpr : AbstractMethodExpr
{
  private readonly L10N L = new L10N(ClassMethodVarExpr.class);

  protected string _className;
  protected Expr _nameExpr;
  protected Expr []_args;

  protected Expr []_fullArgs;

  protected AbstractFunction _fun;
  protected bool _isMethod;

  public ClassMethodVarExpr(Location location,
                            string className,
                            Expr nameExpr,
                            ArrayList<Expr> args)
  {
    super(location);

    _className = className.intern();
    _nameExpr = nameExpr;

    _args = new Expr[args.size()];
    args.toArray(_args);
  }

  public ClassMethodVarExpr(Location location,
                            string className,
                            Expr nameExpr,
                            Expr []args)
   : base(location) {

    _className = className.intern();
    _nameExpr = nameExpr;

    _args = args;
  }

  public ClassMethodVarExpr(String className,
                            Expr nameExpr,
                            ArrayList<Expr> args)
  {
    this(Location.UNKNOWN, className, nameExpr, args);
  }

  public ClassMethodVarExpr(String className, Expr nameExpr, Expr []args)
  {
    this(Location.UNKNOWN, className, nameExpr, args);
  }

  /**
   * Returns the reference of the value.
   * @param location
   */
  public override Expr createRef(QuercusParser parser)
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

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  public override Value eval(Env env)
  {
    QuercusClass cl = env.findClass(_className);

    if (cl == null) {
      env.error(L.l("no matching class {0}", _className), getLocation());
    }

    // qa/0954 - static calls pass the current $this
    Value qThis = env.getThis();

    StringValue methodName = _nameExpr.evalStringValue(env);

    Value []args = evalArgs(env, _args);
    int hash = methodName.hashCodeCaseInsensitive();

    return cl.callStaticMethod(env, qThis, methodName, hash, args);
  }

  public string ToString()
  {
    return _nameExpr + "()";
  }
}

}
