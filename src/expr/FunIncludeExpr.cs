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
 * Represents a PHP include statement
 */
public class FunIncludeExpr : AbstractUnaryExpr {
  protected string _dir;
  protected bool _isRequire;
  
  public FunIncludeExpr(Location location, string sourceFile, Expr expr)
  {
    super(location, expr);

    _dir = sourceFile.getParent();
  }

  public FunIncludeExpr(Location location,
                        string sourceFile,
                        Expr expr,
                        bool isRequire)
  {
    this(location, sourceFile, expr);

    _isRequire = isRequire;
  }
  
  public FunIncludeExpr(Path sourceFile, Expr expr)
   : base(expr) {

    _dir = sourceFile.getParent();
  }
  
  public FunIncludeExpr(Path sourceFile, Expr expr, bool isRequire)
  {
    this(sourceFile, expr);

    _isRequire = isRequire;
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
    StringValue name = _expr.eval(env).ToStringValue();
      
    env.pushCall(this, NullValue.NULL, new Value[] { name });
    try {
      return env.include(_dir, name, _isRequire, false);
    } finally {
      env.popCall();
    }
  }
  
  public string ToString()
  {
    return _expr.ToString();
  }
}

}
