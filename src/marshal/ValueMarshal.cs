using System;
namespace QuercusDotNet.Marshal{
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








public class ValueMarshal : Marshal
{
  public readonly Marshal MARSHAL = new ValueMarshal(false);
  public readonly Marshal MARSHAL_PASS_THRU = new ValueMarshal(true);

  private bool _isPassThru;

  protected ValueMarshal(bool isPassThru)
  {
    _isPassThru = isPassThru;
  }

  public bool isReadOnly()
  {
    return false;
  }

  /**
   * Return true if @is a Value.
   */
  public override bool isValue()
  {
    return true;
  }

  public Object marshal(Env env, Expr expr, Class expectedClass)
  {
    return expr.eval(env);
  }

  public Object marshal(Env env, Value value, Class expectedClass)
  {
    // php/0433
    // php/3c81
    return value.toLocalValueReadOnly(); // non-copy
  }

  public Value unmarshal(Env env, Object value)
  {
    return (Value) value;
  }

  protected override int getMarshalingCostImpl(Value argValue)
  {
    return  Marshal.Marshal.COST_VALUE;
  }

  /*
  public override int getMarshalingCost(Expr expr)
  {
    return  Marshal.Marshal.FOUR;
  }
  */

  public override Class getExpectedClass()
  {
    return Value.class;
  }
}
}
