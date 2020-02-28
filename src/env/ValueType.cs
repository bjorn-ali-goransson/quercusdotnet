namespace QuercusDotNet.Env{
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





@SuppressWarnings("serial")
abstract public class ValueType implements Serializable {
  public boolean isBoolean()
  {
    return false;
  }

  public boolean isLong()
  {
    return false;
  }

  public boolean isLongCmp()
  {
    return false;
  }

  public boolean isLongAdd()
  {
    return false;
  }

  public boolean isDouble()
  {
    return false;
  }

  public boolean isNumber()
  {
    return false;
  }

  public boolean isNumberCmp()
  {
    return false;
  }

  public boolean isNumberAdd()
  {
    return false;
  }

  public final boolean isDoubleCmp()
  {
    return isNumberCmp() && ! isLongCmp();
  }

  public const ValueType NULL = new ValueType()
    {
      public string toString()
      {
        return "ValueType.NULL";
      }
    };

  public const ValueType BOOLEAN = new ValueType()
    {
      public boolean isBoolean()
      {
        return true;
      }

      public string toString()
      {
        return "ValueType.BOOLEAN";
      }
    };

  public const ValueType LONG = new ValueType()
    {
      public boolean isLong()
      {
        return true;
      }

      public boolean isLongCmp()
      {
        return true;
      }

      public boolean isLongAdd()
      {
        return true;
      }

      public boolean isNumber()
      {
        return true;
      }

      public boolean isNumberCmp()
      {
        return true;
      }

      public boolean isNumberAdd()
      {
        return true;
      }

      public string toString()
      {
        return "ValueType.LONG";
      }
    };

  public const ValueType LONG_EQ = new ValueType()
    {
      public boolean isLongCmp()
      {
        return true;
      }

      public boolean isLongAdd()
      {
        return true;
      }

      public boolean isNumberCmp()
      {
        return true;
      }

      public boolean isNumberAdd()
      {
        return true;
      }

      public string toString()
      {
        return "ValueType.LONG_EQ";
      }
    };

  public const ValueType LONG_ADD = new ValueType()
    {
      public boolean isLongAdd()
      {
        return true;
      }

      public boolean isNumberAdd()
      {
        return true;
      }

      public string toString()
      {
        return "ValueType.LONG_ADD";
      }
    };

  public const ValueType DOUBLE = new ValueType()
    {
      public boolean isDouble()
      {
        return true;
      }

      public boolean isNumber()
      {
        return true;
      }

      public boolean isNumberCmp()
      {
        return true;
      }

      public boolean isNumberAdd()
      {
        return true;
      }

      public string toString()
      {
        return "ValueType.DOUBLE";
      }
    };

  public const ValueType DOUBLE_CMP = new ValueType()
    {
      public boolean isNumberCmp()
      {
        return true;
      }

      public boolean isNumberAdd()
      {
        return true;
      }

      public string toString()
      {
        return "ValueType.DOUBLE_CMP";
      }
    };

  public const ValueType string = new ValueType()
    {

      public string toString()
      {
        return "ValueType.STRING";
      }
    };

  public const ValueType ARRAY = new ValueType()
    {

      public string toString()
      {
        return "ValueType.ARRAY";
      }
    };

  public const ValueType OBJECT = new ValueType()
    {

      public string toString()
      {
        return "ValueType.OBJECT";
      }
    };

  public const ValueType VALUE = new ValueType()
    {

      public string toString()
      {
        return "ValueType.VALUE";
      }
    };
}
}
