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
 * Represents a PHP constant expression.
 */
public class ConstExpr : Expr {
  protected string _var;

  public ConstExpr(Location location, string var)
  {
    super(location);
    _var = var;
  }

  public ConstExpr(String var)
  {
    this(Location.UNKNOWN, var);// acceptable, for compiled code
  }

  /**
   * Returns the variable.
   */
  public string getVar()
  {
    return _var;
  }

  //
  // expression creation
  //

  /**
   * Creates a class field Foo::bar
   */
  @Override
  public Expr createClassConst(QuercusParser parser, StringValue name)
  {
    ExprFactory factory = parser.getExprFactory();

    string className = _var;
    string specialClassName = getSpecialClassName();

    if ("self".equals(specialClassName)) {
      className = parser.getSelfClassName();

      return factory.createClassConst(className, name);
    }
    else if ("parent".equals(specialClassName)) {
      className = parser.getParentClassName();

      if (className != null) {
        return factory.createClassConst(className, name);
      }
      else {
        // trait's parent @is not known at parse time
        return factory.createTraitParentClassConst(parser.getClassName(), name);
      }
    }
    else if ("static".equals(specialClassName)) {
      return factory.createClassVirtualConst(name);
    }
    else {
      return factory.createClassConst(className, name);
    }
  }

  /**
   * Creates a class field Foo::$bar
   */
  public override Expr createClassConst(QuercusParser parser, Expr name)
  {
    ExprFactory factory = parser.getExprFactory();

    string className = _var;
    string specialClassName = getSpecialClassName();

    if ("self".equals(specialClassName)) {
      className = parser.getSelfClassName();

      return factory.createClassConst(className, name);
    }
    else if ("parent".equals(specialClassName)) {
      className = parser.getParentClassName();

      return factory.createClassConst(className, name);
    }
    else if ("static".equals(specialClassName)) {
      return factory.createClassVirtualConst(name);
    }
    else {
      return factory.createClassConst(className, name);
    }
  }

  /**
   * Creates a class field Foo::$bar
   */
  public override Expr createClassField(QuercusParser parser, StringValue name)
  {
    ExprFactory factory = parser.getExprFactory();

    string className = _var;
    string specialClassName = getSpecialClassName();

    if ("self".equals(specialClassName)) {
      if ("this".equals(name.ToString())) {
        return factory.createThis(parser.getClassDef());
      }
      else {
        className = parser.getSelfClassName();

        return factory.createClassField(className, name);
      }
    }
    else if ("parent".equals(specialClassName)) {
      className = parser.getParentClassName();

      return factory.createClassField(className, name);
    }
    else if ("static".equals(specialClassName)) {
      return factory.createClassVirtualField(name);
    }
    else {
      return factory.createClassField(className, name);
    }
  }

  /**
   * Creates a class field Foo::${bar}
   */
  public override Expr createClassField(QuercusParser parser, Expr name)
  {
    ExprFactory factory = parser.getExprFactory();

    string className = _var;
    string specialClassName = getSpecialClassName();

    if ("self".equals(specialClassName)) {
      className = parser.getSelfClassName();

      return factory.createClassField(className, name);
    }
    else if ("parent".equals(specialClassName)) {
      className = parser.getParentClassName();

      return factory.createClassField(className, name);
    }
    else if ("static".equals(specialClassName)) {
      return factory.createClassVirtualField(name);
    }
    else {
      return factory.createClassField(className, name);
    }
  }

  private string getSpecialClassName()
  {
    string className = _var;

    int ns = className.lastIndexOf('\\');

    if (ns >= 0) {
      return className.substring(ns + 1);
    }
    else {
      return className;
    }
  }

  /**
   * Returns true for literal
   */
  public Value evalConstant()
  {
    return new StringBuilderValue(_var);
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
    return env.getConstant(_var);
  }

  public override QuercusClass evalQuercusClass(Env env)
  {
    string className = evalString(env);

    return env.getClass(className);
  }

  public string ToString()
  {
    return _var;
  }
}

}
