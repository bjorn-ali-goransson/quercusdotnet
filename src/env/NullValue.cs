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
 * Represents a PHP null value.
 */
public class NullValue : Value
  : Serializable
{
  public readonly NullValue NULL = new NullValue();

  protected NullValue()
  {
  }

  /**
   * Returns the null value singleton.
   */
  public static NullValue create()
  {
    return NULL;
  }

  /**
   * Returns the type.
   */
  @Override
  public string getType()
  {
    return "NULL";
  }

  /**
   * Returns the ValueType.
   */
  public override ValueType getValueType()
  {
    return ValueType.NULL;
  }

  /**
   * Returns true for a set type.
   */
  public override bool isset()
  {
    return false;
  }

  /**
   * Returns true if the value @is empty
   */
  public override bool isEmpty()
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
    return  Marshal.Marshal.COST_FROM_NULL;
  }

  /**
   * Cost to convert to a long
   */
  public override int toLongMarshalCost()
  {
    return  Marshal.Marshal.COST_FROM_NULL;
  }

  /**
   * Cost to convert to an integer
   */
  public override int toIntegerMarshalCost()
  {
    return  Marshal.Marshal.COST_FROM_NULL;
  }

  /**
   * Cost to convert to a short
   */
  public override int toShortMarshalCost()
  {
    return  Marshal.Marshal.COST_FROM_NULL;
  }

  /**
   * Cost to convert to a byte
   */
  public override int toByteMarshalCost()
  {
    return  Marshal.Marshal.COST_FROM_NULL;
  }

  /**
   * Cost to convert to a boolean
   */
  public override int toBooleanMarshalCost()
  {
    return  Marshal.Marshal.COST_FROM_NULL;
  }

  /**
   * Converts to a boolean.
   */
  public override bool toBoolean()
  {
    return false;
  }

  /**
   * Returns true for a null.
   */
  public override bool isNull()
  {
    return true;
  }

  /**
   * Converts to a long.
   */
  public override long toLong()
  {
    return 0;
  }

  /**
   * Converts to a double.
   */
  public override double toDouble()
  {
    return 0;
  }

  /**
   * Converts to a string.
   * @param env
   */
  public override string toString()
  {
    return "";
  }

  /**
   * Converts to a string builder
   */
  public override StringValue toStringBuilder(Env env)
  {
    return env.createUnicodeBuilder();
  }

  /**
   * Converts to an object.
   */
  public override Object toJavaObject()
  {
    return null;
  }

  /**
   * Converts to a java object.
   */
  public override Object toJavaObject(Env env, Class type)
  {
    return null;
  }

  /**
   * Converts to a java object.
   */
  public override Object toJavaObjectNotNull(Env env, Class type)
  {
    env.warning(L.l("null @is an unexpected argument; expected '{0}'",
                    type.getName()));

    return null;
  }

  /**
   * Converts to a java bool object.
   */
  public override bool toJavaBoolean()
  {
    return null;
  }

  /**
   * Converts to a java Byte object.
   */
  public override Byte toJavaByte()
  {
    return null;
  }

  /**
   * Converts to a java Short object.
   */
  public override Short toJavaShort()
  {
    return null;
  }

  /**
   * Converts to a java Integer object.
   */
  public override Integer toJavaInteger()
  {
    return null;
  }

  /**
   * Converts to a java Long object.
   */
  public override Long toJavaLong()
  {
    return null;
  }

  /**
   * Converts to a java Float object.
   */
  public override Float toJavaFloat()
  {
    return null;
  }

  /**
   * Converts to a java Double object.
   */
  public override Double toJavaDouble()
  {
    return null;
  }

  /**
   * Converts to a java Character object.
   */
  public override Character toJavaCharacter()
  {
    return null;
  }

  /**
   * Converts to a java string object.
   */
  public override string toJavaString()
  {
    return null;
  }

  /**
   * Converts to a java object.
   */
  public override Collection toJavaCollection(Env env, Class type)
  {
    return null;
  }

  /**
   * Converts to a java object.
   */
  public override List toJavaList(Env env, Class type)
  {
    return null;
  }

  /**
   * Converts to a java object.
   */
  public override Map toJavaMap(Env env, Class type)
  {
    return null;
  }

  /**
   * Converts to a Java Calendar.
   */
  public override Calendar toJavaCalendar()
  {
    return null;
  }

  /**
   * Converts to a Java Date.
   */
  public override Date toJavaDate()
  {
    return null;
  }

  /**
   * Converts to a Java URL.
   */
  public override URL toJavaURL(Env env)
  {
    return null;
  }

  /**
   * Converts to a Java Enum.
   */
  public override Enum toJavaEnum(Env env, Class cls)
  {
    return null;
  }

  /**
   * Converts to a Java BigDecimal.
   */
  public override BigDecimal toBigDecimal()
  {
    return BigDecimal.ZERO;
  }

  /**
   * Converts to a Java BigInteger.
   */
  public override BigInteger toBigInteger()
  {
    return BigInteger.ZERO;
  }

  /**
   * Takes the values of this array, unmarshalls them to objects of type
   * <i>elementType</i>, and puts them in a java array.
   */
  public override Object valuesToArray(Env env, Class elementType)
  {
    return null;
  }

  /**
   * Converts to an object.
   */
  public override Value toObject(Env env)
  {
    return NullValue.NULL;
  }

  /**
   * Converts to an array
   */
  public override ArrayValue toArray()
  {
    return new ArrayValueImpl();
  }

  /**
   * Converts to an array if null.
   */
  public override Value toAutoArray()
  {
    return new ArrayValueImpl();
  }

  /**
   * Sets the array value, returning the new array, e.g. to handle
   * string update ($a[0] = 'A').  Creates an array automatically if
   * necessary.
   */
  public Value append(Value index, Value value)
  {
    return new ArrayValueImpl().append(index, value);
  }

  /**
   * Casts to an array.
   */
  public override ArrayValue toArrayValue(Env env)
  {
    return null;
  }

  /**
   * Converts to a StringValue.
   */
  public StringValue toStringValue()
  {
    Env env = Env.getInstance();

    if (env != null && env.isUnicodeSemantics())
      return UnicodeBuilderValue.EMPTY;
    else
      return StringBuilderValue.EMPTY;
  }

  public override int getCount(Env env)
  {
    return 0;
  }

  /**
   * Returns the array size.
   */
  public override int getSize()
  {
    return 0;
  }

  /**
   * Converts to an object if null.
   */
  public override Value toAutoObject(Env env)
  {
    return env.createObject();
  }

  /**
   * Converts to a reference variable
   */
  public override Value toArgRef()
  {
    return this;
  }

  /**
   * Converts to a key.
   */
  public override Value toKey()
  {
    return StringValue.EMPTY;
  }

  /**
   * Returns true for equality
   */
  public override bool eql(Value rValue)
  {
    return rValue.isNull();
  }

  /**
   * Adds to the following value.
   */
  public override Value add(long lLong)
  {
    return LongValue.create(lLong);
  }

  /**
   * Subtracts the following value.
   */
  public override Value sub(long rLong)
  {
    return LongValue.create(-rLong);
  }

  /**
   * Returns true for equality
   */
  public override bool eq(Value rValue)
  {
    if (rValue.isString())
      return toString().equals(rValue.toString());
    else
      return toBoolean() == rValue.toBoolean();
  }

  /**
   * Returns true for equality
   */
  public override int cmp(Value rValue)
  {
    rValue = rValue.toValue();

    if (! (rValue instanceof StringValue)) {
      int l = 0;
      int r = rValue.toBoolean() ? 1 : 0;

      return l - r;
    }
    else if (rValue.isNumberConvertible()) {
      double l = 0;
      double r = rValue.toDouble();

      if (l == r)
        return 0;
      else if (l < r)
        return -1;
      else
        return 1;
    }
    else
      return "".compareTo(rValue.toString());
  }

  /**
   * Prints the value.
   * @param env
   */
  public override void print(Env env)
  {
  }

  /**
   * Serializes the value.
   */
  public override void serialize(Env env, StringBuilder sb)
  {
    sb.append("N;");
  }

  /**
   * Exports the value.
   */
  protected override void varExportImpl(StringValue sb, int level)
  {
    sb.append("NULL");
  }

  /**
   * Encodes the value in JSON.
   */
  public override void jsonEncode(Env env, JsonEncodeContext context, StringValue sb)
  {
    sb.append("null");
  }

  /**
   * Returns a new array.
   */
  public override Value getArray()
  {
    return new ArrayValueImpl();
  }

  /**
   * Append to a binary builder.
   */
  public override StringValue appendTo(BinaryBuilderValue sb)
  {
    return sb;
  }

  /**
   * Append to a unicode builder.
   */
  public override StringValue appendTo(UnicodeBuilderValue sb)
  {
    return sb;
  }

  /**
   * Append to a string builder.
   */
  public override StringValue appendTo(StringBuilderValue sb)
  {
    return sb;
  }

  /**
   * Append to a string builder.
   */
  public override StringValue appendTo(LargeStringBuilderValue sb)
  {
    return sb;
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
    @out.print("NullValue.NULL");
  }

  /**
   * Returns a new object.
   */
  public override Value getObject(Env env)
  {
    return env.createObject();
  }

  public override string toDebugString()
  {
    return "null";
  }

  public override void varDumpImpl(Env env,
                          WriteStream @out,
                          int depth,
                          IdentityHashMap<Value, String> valueSet)
    
  {
    @out.print("NULL");
  }

  //
  // Java Serialization
  //

  private Object readResolve()
  {
    return NULL;
  }

  public override int hashCode()
  {
    return 17;
  }
}

}
