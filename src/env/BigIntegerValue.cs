using System;
namespace QuercusDotNet.Env{
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
 * Represents a Quercus java BigInteger value.
 */
public class BigIntegerValue : JavaValue {
  private const Logger log
    = Logger.getLogger(JavaURLValue.class.getName());

  private BigInteger _val;

  public BigIntegerValue(Env env, BigInteger val, JavaClassDef def)
  {
    super(env, val, def);
    _val = val;
  }
  
  /**
   * Converts to a long.
   */
  @Override
  public long toLong()
  {
    return _val.longValue();
  }
  
  /**
   * Converts to a double.
   */
  public override double toDouble()
  {
    return _val.doubleValue();
  }
  
  /**
   * Converts to a Java BigDecimal.
   */
  public override BigDecimal toBigDecimal()
  {
    return new BigDecimal(toString());
  }
  
  /**
   * Converts to a Java BigDecimal.
   */
  public override BigInteger toBigInteger()
  {
    return _val;
  }
  
  /**
   * Returns true for a double-value.
   */
  public override bool isDoubleConvertible()
  {
    return true;
  }

  /**
   * Returns true for a long-value.
   */
  public override bool isLongConvertible()
  {
    return true;
  }
  
  public string toString()
  {
    return "BigInteger[" + _val.toString() + "]";
  }
}

}
