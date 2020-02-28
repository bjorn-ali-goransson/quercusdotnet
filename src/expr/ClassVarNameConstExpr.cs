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
 * Represents a PHP A::{$foo} constant call expression.
 */
public class ClassVarNameConstExpr : Expr {
  protected string _className;
  protected Expr _name;

  public ClassVarNameConstExpr(Location location, string className, Expr name)
  {
    super(location);

    _className = className.intern();
    _name = name;
  }

  public ClassVarNameConstExpr(String className, Expr name)
  {
    _className = className.intern();
    _name = name;
  }

  //
  // function call creation
  //

  /**
   * Creates a function call expression
   */
  @Override
  public Expr createCall(QuercusParser parser,
                         Location location,
                         ArrayList<Expr> args)
    
  {
    ExprFactory factory = parser.getExprFactory();

    return factory.createClassMethodCall(location, _className, _name, args);
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
    StringValue name = _name.evalStringValue(env);

    return env.getClass(_className).getConstant(env, name);
  }

  public override string toString()
  {
    return _className + "::{" + _name + "}";
  }
}

}
