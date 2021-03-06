using System;
namespace QuercusDotNet.Program{
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
 * Represents a formal argument.
 */
public class Arg {
  private StringValue _name;
  private Expr _default;

  private bool _isReference;
  private string _expectedClass;

  public Arg(StringValue name,
             Expr defaultExpr,
             bool isReference,
             string expectedClass)
  {
    _name = name;
    _default = defaultExpr;
    _isReference = isReference;
    _expectedClass = expectedClass;

    if (_default == null) {
      throw new IllegalStateException();
    }
  }

  /**
   * Evaluates the default expr.
   */
  public Value eval(Env env)
  {
    return _default.eval(env);
  }

  /**
   * Returns the argument name.
   */
  public StringValue getName()
  {
    return _name;
  }

  /**
   * Returns the default expression
   */
  public Expr getDefault()
  {
    return _default;
  }

  /**
   * Returns true for a reference argument.
   */
  public bool isReference()
  {
    return _isReference;
  }

  /**
   * Returns true if the argument @is required.
   */
  public bool isRequired()
  {
    return _default == ParamRequiredExpr.REQUIRED;
  }

  /**
   * Returns the expected classname
   */
  public string getExpectedClass()
  {
    return _expectedClass;
  }

  public string ToString()
  {
    return "Arg[" + _expectedClass + " $" + _name + "]";
  }
}

}
