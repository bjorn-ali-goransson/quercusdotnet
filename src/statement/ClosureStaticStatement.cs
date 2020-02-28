using System;
namespace QuercusDotNet.Statement{
/*
 * Copyright (c) 1998-2013 Caucho Technology -- all rights reserved
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
 * @author Nam Nguyen
 */












/**
 * Represents a static statement in a PHP program.
 */
public class ClosureStaticStatement
  : Statement
{
  protected VarExpr _var;
  protected Expr _initValue;

  /**
   * Creates the echo statement.
   */
  public ClosureStaticStatement(Location location,
                                VarExpr var,
                                Expr initValue)
  {
    super(location);

    _var = var;
    _initValue = initValue;
  }

  public Value execute(Env env)
  {
    Closure closure = env.getClosure();

    try {
      Var var = closure.getStaticVar(_var.getName());

      env.setRef(_var.getName(), var);

      if (! var.isset() && _initValue != null) {
        var.set(_initValue.eval(env));
      }

    }
    catch (RuntimeException e) {
      rethrow(e, RuntimeException.class);
    }

    return null;
  }
}

}
