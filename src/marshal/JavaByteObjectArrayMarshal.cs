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







public class JavaByteObjectArrayMarshal : JavaArrayMarshal
{
  public const Marshal MARSHAL
    = new JavaByteObjectArrayMarshal();

  public override Value unmarshal(Env env, Object value)
  {
    Byte []byteValue = (Byte []) value;

    if (byteValue == null)
      return NullValue.NULL;

    byte []data = new byte[byteValue.length];
    for (int i = 0; i < data.length; i++)
      data[i] = byteValue[i];

    return env.createBinaryBuilder(data);
  }

  protected override int getMarshalingCostImpl(Value argValue)
  {
    return  Marshal.Marshal.COST_INCOMPATIBLE;
    /*
    if (argValue.isString()) {
      if (argValue.isUnicode())
        return  Marshal.Marshal.UNICODE_BYTE_OBJECT_ARRAY_COST;
      else if (argValue.isBinary())
        return  Marshal.Marshal.BINARY_BYTE_OBJECT_ARRAY_COST;
      else
        return  Marshal.Marshal.PHP5_BYTE_OBJECT_ARRAY_COST;
    }
    else if (argValue.isArray())
      return  Marshal.Marshal.THREE;
    else
      return  Marshal.Marshal.FOUR;
    */
  }

  public override Class getExpectedClass()
  {
    return Byte[].class;
  }
}
}
