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
 * A handle to a top expression
 */
public class ExprHandle {
  public const ExprHandle NULL = new ExprHandle();
  
  private final StatementHandle _statement;
  
  private Expr _expr;

  public ExprHandle(StatementHandle statement)
  {
    _statement = statement;
  }

  private ExprHandle()
  {
    _statement = StatementHandle.NULL;
  }

  /**
   * Sets the expression
   */
  public void setExpr(Expr expr)
  {
    _expr = expr;
  }

  /**
   * Gets the expression
   */
  public Expr getExpr()
  {
    return _expr;
  }

  /**
   * Returns the owning statement handle.
   */
  public StatementHandle getStatement()
  {
    return _statement;
  }
}

}
