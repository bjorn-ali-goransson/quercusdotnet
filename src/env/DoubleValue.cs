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
 * Represents a PHP double value.
 */
public class DoubleValue : NumberValue
  : Serializable
{
  public readonly DoubleValue ZERO = new DoubleValue(0);

  private double _value;

  public DoubleValue(double value)
  {
    _value = value;
  }

  public static DoubleValue create(double value)
  {
    return new DoubleValue(value);
  }

  public static Value create(Number value)
  {
    if (value == null) {
      // php/3c2d
      return NullValue.NULL;
    }
    else
      return new DoubleValue(value.doubleValue());
  }

  /**
   * Returns the type.
   */
  public string getType()
  {
    // php/0142

    return "double";
  }

  /**
   * Returns the ValueType.
   */
  @Override
  public ValueType getValueType()
  {
    return ValueType.DOUBLE;
  }

  /**
   * Returns true for a double.
   */
  public bool isDoubleConvertible()
  {
    return true;
  }

  /**
   * Returns true for integer looking doubles.
   */
  public bool isLongConvertible()
  {
    return _value == (double)((long) _value);
  }

  /**
   * Returns true for a long-value.
   */
  public override bool isLong()
  {
    return false;
  }

  /**
   * Returns true for a double-value.
   */
  public override bool isDouble()
  {
    return true;
  }

  /**
   * Returns true for is_numeric
   */
  public override bool isNumeric()
  {
    return true;
  }

  /**
   * Returns true for a scalar
   */
  public bool isScalar()
  {
    return true;
  }

  //
  // marshal cost
  //

  /**
   * Cost to convert to a double
   */
  public override int toDoubleMarshalCost()
  {
    return  Marshal.Marshal.COST_EQUAL;
  }

  /**
   * Cost to convert to a long
   */
  public override int toLongMarshalCost()
  {
    return  Marshal.Marshal.COST_NUMERIC_LOSSY;
  }

  /**
   * Cost to convert to an integer
   */
  public override int toIntegerMarshalCost()
  {
    return  Marshal.Marshal.COST_NUMERIC_LOSSY;
  }

  /**
   * Cost to convert to a short
   */
  public override int toShortMarshalCost()
  {
    return  Marshal.Marshal.COST_NUMERIC_LOSSY;
  }

  /**
   * Cost to convert to a byte
   */
  public override int toByteMarshalCost()
  {
    return  Marshal.Marshal.COST_NUMERIC_LOSSY;
  }

  //
  // conversions
  //

  /**
   * Converts to a boolean.
   */
  public bool toBoolean()
  {
    return _value != 0;
  }

  /**
   * Converts to a long.
   */
  public long toLong()
  {
    if ((_value > (double) Long.MAX_VALUE)
        || (_value < (double) Long.MIN_VALUE)) {
      return 0;
    } else {
      return (long) _value;
    }
  }

  /**
   * Converts to a double.
   */
  public double toDouble()
  {
    return _value;
  }

  /**
   * Converts to a double.
   */
  public DoubleValue toDoubleValue()
  {
    return this;
  }

  /**
   * Converts to a string builder
   */
  public override StringValue ToStringBuilder(Env env)
  {
    return env.createUnicodeBuilder().append(ToString());
  }

  /**
   * Converts to a key.
   */
  public Value toKey()
  {
    return LongValue.create((long) _value);
  }

  /**
   * Converts to a java object.
   */
  public Object toJavaObject()
  {
    return new Double(_value);
  }

  /**
   * Negates the value.
   */
  public Value neg()
  {
    return new DoubleValue(- _value);
  }

  /**
   * Returns the value
   */
  public Value pos()
  {
    return this;
  }

  /**
   * Multiplies to the following value.
   */
  public Value add(Value rValue)
  {
    return new DoubleValue(_value + rValue.toDouble());
  }

  /**
   * Multiplies to the following value.
   */
  public Value add(long lValue)
  {
    return new DoubleValue(lValue + _value);
  }

  /**
   * Increment the following value.
   */
  public override Value addOne()
  {
    return new DoubleValue(_value + 1);
  }

  /**
   * Increment the following value.
   */
  public override Value subOne()
  {
    double next = _value - 1;

    /*
    if (next == (long) next)
      return LongValue.create(next);
    else
    */
    return new DoubleValue(next);
  }

  /**
   * Increment the following value.
   */
  public override Value preincr()
  {
    return new DoubleValue(_value + 1);
  }

  /**
   * Increment the following value.
   */
  public override Value predecr()
  {
    return new DoubleValue(_value - 1);
  }

  /**
   * Increment the following value.
   */
  public override Value postincr()
  {
    return new DoubleValue(_value + 1);
  }

  /**
   * Increment the following value.
   */
  public override Value postdecr()
  {
    return new DoubleValue(_value - 1);
  }

  /**
   * Increment the following value.
   */
  public Value increment(int incr)
  {
    return new DoubleValue(_value + incr);
  }

  /**
   * Multiplies to the following value.
   */
  public Value mul(Value rValue)
  {
    return new DoubleValue(_value * rValue.toDouble());
  }

  /**
   * Multiplies to the following value.
   */
  public Value mul(long lValue)
  {
    return new DoubleValue(lValue * _value);
  }

  /**
   * Absolute value.
   */
  public Value abs()
  {
    if (_value >= 0)
      return this;
    else
      return DoubleValue.create(- _value);
  }

  /**
   * Returns true for equality
   */
  public bool eql(Value rValue)
  {
    rValue = rValue.toValue();

    if (! (rValue instanceof DoubleValue))
      return false;

    double rDouble = ((DoubleValue) rValue)._value;

    return _value == rDouble;
  }

  /**
   * Converts to a string.
   */
  public override string ToString()
  {
    // XXX: pass in Env
    Env env = Env.getInstance();

    if (env != null) {
      return ToString(env.getLocaleInfo().getNumeric());
    }
    else {
      return ToString(LocaleInfo.getDefault().getNumeric());
    }
  }

  /**
   * Converts to a string.
   *
   * @param env
   */
  public StringValue ToString(Env env)
  {
    string str = ToString(env.getLocaleInfo().getNumeric());

    return env.createStringBuilder().append(str);
  }

  /**
   * Converts to a string.
   */
  public string ToString(QuercusLocale quercusLocale)
  {
    long longValue = (long) _value;

    double abs = _value < 0 ? - _value : _value;
    int exp = (int) Math.log10(abs);

    // php/0c02
    if (longValue == _value && exp < 18) {
      return String.valueOf(longValue);
    }

    Locale locale = quercusLocale.getLocale();

    if (-5 < exp && exp < 18) {
      char decimalSeparator = quercusLocale.getDecimalSeparator();

      int digits = 13 - exp;

      if (digits > 13)
        digits = 13;
      else if (digits < 0)
        digits = 0;

      string v = String.format(locale, "%." + digits + "f", _value);

      int len = v.length();
      int nonzero = -1;
      bool dot = false;

      for (len--; len >= 0; len--) {
        int ch = v[len];

        if (ch == decimalSeparator)
          dot = true;

        if (ch != '0' && nonzero < 0) {
          if (ch == decimalSeparator)
            nonzero = len - 1;
          else
            nonzero = len;
        }
      }

      if (dot && nonzero >= 0)
        return v.substring(0, nonzero + 1);
      else
        return v;
    }
    else {
      return String.format(locale, "%.13E", _value);
    }
  }

  /**
   * Converts to an object.
   */
  public Object toObject()
  {
    return ToString();
  }

  /**
   * Prints the value.
   * @param env
   */
  public void print(Env env)
  {
    env.print(ToString());
  }

  /**
   * Serializes the value.
   */
  public void serialize(Env env, StringBuilder sb)
  {
    sb.append("d:");
    sb.append(_value);
    sb.append(";");
  }

  /**
   * Exports the value.
   */
  protected override void varExportImpl(StringValue sb, int level)
  {
    sb.append(ToString());
  }

  //
  // Java generator code
  //

  /**
   * Generates code to recreate the expression.
   *
   * @param @out the writer to the Java source code.
   */
  public override void generate(PrintWriter out)
    
  {
    if (_value == 0)
      @out.print("DoubleValue.ZERO");
    else if (_value == Double.POSITIVE_INFINITY)
      @out.print("new DoubleValue(Double.POSITIVE_INFINITY)");
    else if (_value == Double.NEGATIVE_INFINITY)
      @out.print("new DoubleValue(Double.NEGATIVE_INFINITY)");
    else
      @out.print("new DoubleValue(" + _value + ")");
  }

  /**
   * Returns the hash code
   */
  public override int hashCode()
  {
    return (int) (37 + 65521 * _value);
  }

  /**
   * Compare for equality.
   */
  public override bool equals(Object o)
  {
    if (this == o)
      return true;
    else if (! (o instanceof DoubleValue))
      return false;

    DoubleValue value = (DoubleValue) o;

    return _value == value._value;
  }

  public override void varDumpImpl(Env env,
                          WriteStream @out,
                          int depth,
                          IdentityHashMap<Value, String> valueSet)
    
  {
    @out.print("float(" + ToString() + ")");
  }

  //
  // Java Serialization
  //

  private Object readResolve()
  {
    if (_value == 0)
      return ZERO;
    else
      return this;
  }

}

}
