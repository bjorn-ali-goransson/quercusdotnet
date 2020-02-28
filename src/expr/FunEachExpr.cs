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
 * Represents a PHP each expression.
 */
public class FunEachExpr : AbstractUnaryExpr {
  private L10N L = new L10N(FunEachExpr.class);
  
  private bool _isVar;
  
  public FunEachExpr(Location location, Expr expr)
    
  {
    super(location, expr);
    
    _isVar = expr.isVar();
  }

  public FunEachExpr(Expr expr)
   : base(expr) {
    
    _isVar = expr.isVar();
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
    if (! _isVar) {
      env.error(L.l("each() argument must be a variable at '{0}'", getExpr()));
      
      return NullValue.NULL;
    }
    
    Value var = getExpr().evalRef(env);
    Value value = var.toValue();

    if (value instanceof ArrayValue) {
      ArrayValue array = (ArrayValue) value;

      return array.each();
    }
    else {
      env.warning(L.l("each() argument must be an array at '{0}'",
                      value.getClass().getSimpleName()));
    
      return BooleanValue.FALSE;
    }
  }
}

}
