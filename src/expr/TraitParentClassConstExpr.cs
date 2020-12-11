using System;
namespace QuercusDotNet.Expr{
/*
 * Copyright (c) 1998-2013 Caucho Technology -- all rights reserved
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
 * Represents a PHP parent::FOO constant call expression.
 */
public class TraitParentClassConstExpr : Expr {
  protected string _traitName;
  protected StringValue _name;

  public TraitParentClassConstExpr(Location location,
                                   string traitName, StringValue name)
  {
    super(location);;

    _traitName = traitName;
    _name = name;
  }

  public TraitParentClassConstExpr(String traitName, StringValue name)
  {
    _traitName = traitName;
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

    return factory.createTraitParentClassMethodCall(location, _traitName, _name, args);
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
    Value qThis = env.getThis();

    QuercusClass parent = qThis.getQuercusClass().getTraitParent(env, _traitName);

    return parent.getConstant(env, _name);
  }

  public string ToString()
  {
    return "parent::" + _name;
  }
}

}
