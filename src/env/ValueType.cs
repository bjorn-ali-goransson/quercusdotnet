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
  public bool isBoolean()
  {
    return false;
  }

  public bool isLong()
  {
    return false;
  }

  public bool isLongCmp()
  {
    return false;
  }

  public bool isLongAdd()
  {
    return false;
  }

  public bool isDouble()
  {
    return false;
  }

  public bool isNumber()
  {
    return false;
  }

  public bool isNumberCmp()
  {
    return false;
  }

  public bool isNumberAdd()
  {
    return false;
  }

  public final bool isDoubleCmp()
  {
    return isNumberCmp() && ! isLongCmp();
  }

  public readonly ValueType NULL = new ValueType()
    {
      public string toString()
      {
        return "ValueType.NULL";
      }
    };

  public readonly ValueType bool = new ValueType()
    {
      public bool isBoolean()
      {
        return true;
      }

      public string toString()
      {
        return "ValueType.BOOLEAN";
      }
    };

  public readonly ValueType LONG = new ValueType()
    {
      public bool isLong()
      {
        return true;
      }

      public bool isLongCmp()
      {
        return true;
      }

      public bool isLongAdd()
      {
        return true;
      }

      public bool isNumber()
      {
        return true;
      }

      public bool isNumberCmp()
      {
        return true;
      }

      public bool isNumberAdd()
      {
        return true;
      }

      public string toString()
      {
        return "ValueType.LONG";
      }
    };

  public readonly ValueType LONG_EQ = new ValueType()
    {
      public bool isLongCmp()
      {
        return true;
      }

      public bool isLongAdd()
      {
        return true;
      }

      public bool isNumberCmp()
      {
        return true;
      }

      public bool isNumberAdd()
      {
        return true;
      }

      public string toString()
      {
        return "ValueType.LONG_EQ";
      }
    };

  public readonly ValueType LONG_ADD = new ValueType()
    {
      public bool isLongAdd()
      {
        return true;
      }

      public bool isNumberAdd()
      {
        return true;
      }

      public string toString()
      {
        return "ValueType.LONG_ADD";
      }
    };

  public readonly ValueType DOUBLE = new ValueType()
    {
      public bool isDouble()
      {
        return true;
      }

      public bool isNumber()
      {
        return true;
      }

      public bool isNumberCmp()
      {
        return true;
      }

      public bool isNumberAdd()
      {
        return true;
      }

      public string toString()
      {
        return "ValueType.DOUBLE";
      }
    };

  public readonly ValueType DOUBLE_CMP = new ValueType()
    {
      public bool isNumberCmp()
      {
        return true;
      }

      public bool isNumberAdd()
      {
        return true;
      }

      public string toString()
      {
        return "ValueType.DOUBLE_CMP";
      }
    };

  public readonly ValueType string = new ValueType()
    {

      public string toString()
      {
        return "ValueType.STRING";
      }
    };

  public readonly ValueType ARRAY = new ValueType()
    {

      public string toString()
      {
        return "ValueType.ARRAY";
      }
    };

  public readonly ValueType OBJECT = new ValueType()
    {

      public string toString()
      {
        return "ValueType.OBJECT";
      }
    };

  public readonly ValueType VALUE = new ValueType()
    {

      public string toString()
      {
        return "ValueType.VALUE";
      }
    };
}
}
