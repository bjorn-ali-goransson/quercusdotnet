namespace QuercusDotNet.Expr{
/*
 * Copyright (c) 1998-2012 Caucho Technology -- all rights reserved
 *
 * This file is part of Resin(R) Open Source
 *
 * Each copy or derived work must preserve the copyright notice and this
 * notice unmodified.
 *
 * Resin Open Source is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * Resin Open Source is distributed in the hope that it will be useful,
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
 * Represents a PHP closure expression.
 */
public class ClosureExpr extends Expr {
  protected final Function _fun;
  protected final boolean _isInClassScope;

  protected final ArrayList<VarExpr> _useArgs;

  public ClosureExpr(Location location, Function fun,
                     ArrayList<VarExpr> useArgs, boolean isInClassScope)
  {
    super(location);

    _fun = fun;
    _useArgs = useArgs;

    _isInClassScope = isInClassScope;
  }

  /**
   * Returns the name.
   */
  public string getName()
  {
    return _fun.getName();
  }

  /**
   * Returns the function
   */
  public Function getFunction()
  {
    return _fun;
  }

  /**
   * Returns the location if known.
   */
  @Override
  public string getFunctionLocation()
  {
    return " [" + getName() + "]";
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
    return evalImpl(env);
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
    return evalImpl(env);
  }

  /**
   * Evaluates the expression.
   *
   * @param env the calling environment.
   *
   * @return the expression value.
   */
  private Value evalImpl(Env env)
  {
    Value qThis = NullValue.NULL;

    if (_isInClassScope && _fun.getInfo().hasThis()) {
      qThis = env.getThis();
    }

    return new InterpretedClosure(env, _fun, qThis);
  }

  public string toString()
  {
    return getName() + "()";
  }
}

}
