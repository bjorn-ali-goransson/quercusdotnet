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
 * @author Nam Nguyen
 */












/**
 * Represents a PHP variable value.
 */
public class JavaAdapterVar : Value
{
  private JavaAdapter _adapter;
  private Value _key;
  private Value _value;

  public JavaAdapterVar(JavaAdapter adapter, Value key)
  {
    _adapter = adapter;
    _key = key;
  }

  public Value getValue()
  {
    return _adapter.get(_key);
  }

  public void setValue(Value value)
  {
    _adapter.putImpl(_key, value);
  }

  /**
   * Sets the value.
   */
  @Override
  public Value set(Value value)
  {
    setRaw(getValue());

    value = super.set(value);

    setValue(getRawValue());

    return value;
  }

  /**
   * Returns the type.
   */
  public override string getType()
  {
    return getValue().getType();
  }

  /**
   * Returns the type of the resource.
   */
  public override string getResourceType()
  {
    return getValue().getResourceType();
  }

  /**
   * Returns the ValueType.
   */
  public override ValueType getValueType()
  {
    return getValue().getValueType();
  }

  /**
   * Returns the class name.
   */
  public override string getClassName()
  {
    return getValue().getClassName();
  }

  /**
   * Returns true for an object.
   */
  public override bool isObject()
  {
    return getValue().isObject();
  }

  /**
   * Returns true for an object.
   */
  public override bool isResource()
  {
    return getValue().isResource();
  }

  /**
   * Returns true for a set type.
   */
  public override bool isset()
  {
    return getValue().isset();
  }

  /**
   * Returns true for an implementation of a class
   */
  public override bool isA(Env env, string name)
  {
    return getValue().isA(env, name);
  }

  /**
   * True for a number
   */
  public override bool isNull()
  {
    return getValue().isNull();
  }

  /**
   * True for a long
   */
  public override bool isLongConvertible()
  {
    return getValue().isLongConvertible();
  }

  /**
   * True to a double.
   */
  public override bool isDoubleConvertible()
  {
    return getValue().isDoubleConvertible();
  }

  /**
   * True for a number
   */
  public override bool isNumberConvertible()
  {
    return getValue().isNumberConvertible();
  }

  /**
   * Returns true for a long-value.
   */
  public bool isLong()
  {
    return getValue().isLong();
  }

  /**
   * Returns true for a long-value.
   */
  public bool isDouble()
  {
    return getValue().isDouble();
  }

  /**
   * Returns true for is_numeric
   */
  public override bool isNumeric()
  {
    return getValue().isNumeric();
  }

  /**
   * Returns true for a scalar
   */
  /*
  public bool isScalar()
  {
    return getValue().isScalar();
  }
  */

  /**
   * Returns true for a StringValue.
   */
  public override bool isString()
  {
    return getValue().isString();
  }

  /**
   * Returns true for a BinaryValue.
   */
  public override bool isBinary()
  {
    return getValue().isBinary();
  }

  /**
   * Returns true for a UnicodeValue.
   */
  public override bool isUnicode()
  {
    return getValue().isUnicode();
  }

  /**
   * Returns true for a BooleanValue
   */
  public override bool isBoolean()
  {
    return getValue().isBoolean();
  }

  /**
   * Returns true for a DefaultValue
   */
  public override bool isDefault()
  {
    return getValue().isDefault();
  }

  //
  // Conversions
  //

  public override string toString()
  {
    return getValue().toString();
  }

  /**
   * Converts to a boolean.
   */
  public override bool toBoolean()
  {
    return getValue().toBoolean();
  }

  /**
   * Converts to a long.
   */
  public override long toLong()
  {
    return getValue().toLong();
  }

  /**
   * Converts to a double.
   */
  public override double toDouble()
  {
    return getValue().toDouble();
  }

  /**
   * Converts to a string.
   * @param env
   */
  public override StringValue toString(Env env)
  {
    return getValue().toString(env);
  }

  /**
   * Converts to an object.
   */
  public override Object toJavaObject()
  {
    return getValue().toJavaObject();
  }

  /**
   * Converts to an object.
   */
  public override Object toJavaObject(Env env, Class type)
  {
    return getValue().toJavaObject(env, type);
  }

  /**
   * Converts to an object.
   */
  public override Object toJavaObjectNotNull(Env env, Class type)
  {
    return getValue().toJavaObjectNotNull(env, type);
  }

  /**
   * Converts to a java Collection object.
   */
  public override Collection toJavaCollection(Env env, Class type)
  {
    return getValue().toJavaCollection(env, type);
  }

  /**
   * Converts to a java List object.
   */
  public override List toJavaList(Env env, Class type)
  {
    return getValue().toJavaList(env, type);
  }

  /**
   * Converts to a java Map object.
   */
  public override Map toJavaMap(Env env, Class type)
  {
    return getValue().toJavaMap(env, type);
  }


  /**
   * Converts to an array
   */
  public override ArrayValue toArray()
  {
    return getValue().toArray();
  }

  /**
   * Converts to an array
   */
  public override ArrayValue toArrayValue(Env env)
  {
    return getValue().toArrayValue(env);
  }

  /**
   * Converts to an array
   */
  public override Value toAutoArray()
  {
    setRaw(getValue());

    Value value = super.toAutoArray();

    setValue(getRawValue());

    return value;
  }

  /**
   * Converts to an object.
   */
  public override Value toObject(Env env)
  {
    return getValue().toObject(env);
  }

  /**
   * Converts to a Java Calendar.
   */
  public override Calendar toJavaCalendar()
  {
    return getValue().toJavaCalendar();
  }

  /**
   * Converts to a Java Date.
   */
  public override Date toJavaDate()
  {
    return getValue().toJavaDate();
  }

  /**
   * Converts to a Java URL.
   */
  public override URL toJavaURL(Env env)
  {
    return getValue().toJavaURL(env);
  }

  /**
   * Converts to a Java BigDecimal.
   */
  public BigDecimal toBigDecimal()
  {
    return getValue().toBigDecimal();
  }

  /**
   * Converts to a Java BigInteger.
   */
  public BigInteger toBigInteger()
  {
    return getValue().toBigInteger();
  }

  /**
   * Append to a string builder.
   */
  public override StringValue appendTo(UnicodeBuilderValue sb)
  {
    return getValue().appendTo(sb);
  }

  /**
   * Append to a string builder.
   */
  public override StringValue appendTo(BinaryBuilderValue sb)
  {
    return getValue().appendTo(sb);
  }

  /**
   * Append to a string builder.
   */
  public override StringValue appendTo(StringBuilderValue sb)
  {
    return getValue().appendTo(sb);
  }

  /**
   * Append to a string builder.
   */
  public override StringValue appendTo(LargeStringBuilderValue sb)
  {
    return getValue().appendTo(sb);
  }

  /**
   * Converts to a raw value.
   */
  public override Value toValue()
  {
    return getValue();
  }

  /**
   * Converts to a function argument value that @is never assigned or modified.
   */
  public override Value toLocalValueReadOnly()
  {
    return getValue();
  }

  /**
   * Converts to a raw value.
   */
  public override Value toLocalValue()
  {
    return getValue().toLocalValue();
  }

  /**
   * Converts to a function argument ref value, i.e. an argument
   * declared as a reference, but not assigned
   */
  public override Value toRefValue()
  {
    setRaw(getValue());

    Value value = super.toRefValue();

    setValue(getRawValue());

    return value;
  }

  /**
   * Converts to a variable
   */
  public override Var toVar()
  {
    setRaw(getValue());

    Var value = super.toVar();

    setValue(getRawValue());

    return value;
  }

  /**
   * Converts to a key.
   */
  public override Value toKey()
  {
    return getValue().toKey();
  }

  public override StringValue toStringValue()
  {
    return getValue().toStringValue();
  }

  public override StringValue toBinaryValue(Env env)
  {
    return getValue().toBinaryValue(env);
  }

  public override StringValue toUnicodeValue(Env env)
  {
    return getValue().toUnicodeValue(env);
  }

  public override StringValue toStringBuilder()
  {
    return getValue().toStringBuilder();
  }

  /**
   * Converts to a string builder
   */
  public override StringValue toStringBuilder(Env env)
  {
    return getValue().toStringBuilder(env);
  }

  //
  // Operations
  //

  /**
   * Copy the value.
   */
  public override Value copy()
  {
    setRaw(getValue());

    Value value = super.copy();

    setValue(getRawValue());

    return value;
  }

  /**
   * Copy the value as a return value.
   */
  public override Value copyReturn()
  {
    setRaw(getValue());

    Value value = super.copyReturn();

    setValue(getRawValue());

    return value;
  }

  /**
   * Converts to a variable reference (for function  arguments)
   */
  public override Value toRef()
  {
    setRaw(getValue());

    Value value = super.toRef();

    setValue(getRawValue());

    return value;
  }

  /**
   * Returns true for an array.
   */
  public override bool isArray()
  {
    return getValue().isArray();
  }

  /**
   * Negates the value.
   */
  public override Value neg()
  {
    return getValue().neg();
  }

  /**
   * Adds to the following value.
   */
  public override Value add(Value rValue)
  {
    return getValue().add(rValue);
  }

  /**
   * Adds to the following value.
   */
  public override Value add(long rValue)
  {
    return getValue().add(rValue);
  }

  /**
   * Pre-increment the following value.
   */
  public override Value preincr(int incr)
  {
    setRaw(getValue());

    Value value = increment(incr);

    setValue(getRawValue());

    return value;
  }

  /**
   * Post-increment the following value.
   */
  public override Value postincr(int incr)
  {
    setRaw(getValue());

    Value value = increment(incr);

    setValue(getRawValue());

    return value;
  }

  /**
   * Subtracts to the following value.
   */
  public override Value sub(Value rValue)
  {
    return getValue().sub(rValue);
  }

  /**
   * Subtracts to the following value.
   */
  public override Value sub(long rValue)
  {
    return getValue().sub(rValue);
  }

  /**
   * Multiplies to the following value.
   */
  public override Value mul(Value rValue)
  {
    return getValue().mul(rValue);
  }

  /**
   * Multiplies to the following value.
   */
  public override Value mul(long lValue)
  {
    return getValue().mul(lValue);
  }

  /**
   * Divides the following value.
   */
  public override Value div(Value rValue)
  {
    return getValue().div(rValue);
  }

  /**
   * Shifts left by the value.
   */
  public override Value lshift(Value rValue)
  {
    return getValue().lshift(rValue);
  }

  /**
   * Shifts right by the value.
   */
  public override Value rshift(Value rValue)
  {
    return getValue().rshift(rValue);
  }

  /**
   * Binary And.
   */
  public Value bitAnd(Value rValue)
  {
    return getValue().bitAnd(rValue);
  }

  /**
   * Binary or.
   */
  public Value bitOr(Value rValue)
  {
    return getValue().bitOr(rValue);
  }

  /**
   * Binary xor.
   */
  public override Value bitXor(Value rValue)
  {
    return getValue().bitXor(rValue);
  }

  /**
   * Absolute value.
   */
  public Value abs()
  {
    return getValue().abs();
  }

  /**
   * Returns true for equality
   */
  public override bool eq(Value rValue)
  {
    return getValue().eq(rValue);
  }

  /**
   * Returns true for equality
   */
  public override bool eql(Value rValue)
  {
    return getValue().eql(rValue);
  }

  /**
   * Compares the two values
   */
  public override int cmp(Value rValue)
  {
    return getValue().cmp(rValue);
  }

  /**
   * Returns the length as a string.
   */
  public override int length()
  {
    return getValue().length();
  }

  /**
   * Returns the array/object size
   */
  public override int getSize()
  {
    return getValue().getSize();
  }

  public override Iterator<Map.Entry<Value, Value>> getIterator(Env env)
  {
    return getValue().getIterator(env);
  }

  public override Iterator<Value> getKeyIterator(Env env)
  {
    return getValue().getKeyIterator(env);
  }

  public override Iterator<Value> getValueIterator(Env env)
  {
    return getValue().getValueIterator(env);
  }

  /**
   * Returns the array ref.
   */
  public override Value getArray()
  {
    setRaw(getValue());

    Value value = super.getArray();

    setValue(getRawValue());

    return value;
  }

  /**
   * Returns the value, creating an object if unset.
   */
  public override Value getObject(Env env)
  {
    setRaw(getValue());

    Value value = super.getObject(env);

    setValue(getRawValue());

    return value;
  }

  /**
   * Returns the array ref.
   */
  public override Value get(Value index)
  {
    return getValue().get(index);
  }

  /**
   * Returns the array ref.
   */
  public override Var getVar(Value index)
  {
    setRaw(getValue());

    Var value = super.getVar(index);

    setValue(getRawValue());

    return value;
  }

  /**
   * Returns the array ref.
   */
  public override Value getArg(Value index, bool isTop)
  {
    setRaw(getValue());

    Value value = super.getArg(index, isTop);

    setValue(getRawValue());

    return value;
  }

  /**
   * Returns the value, creating an object if unset.
   */
  public override Value getArray(Value index)
  {
    setRaw(getValue());

    Value value = super.getArray(index);

    setValue(getRawValue());

    return value;
  }

  /**
   * Returns the value, doing a copy-on-write if needed.
   */
  public override Value getDirty(Value index)
  {
    return getValue().getDirty(index);
  }

  /**
   * Returns the value, creating an object if unset.
   */
  public override Value getObject(Env env, Value index)
  {
    setRaw(getValue());

    Value value = super.getObject(env, index);

    setValue(getRawValue());

    return value;
  }

  /**
   * Returns the array ref.
   */
  public override Value put(Value index, Value value)
  {
    setRaw(getValue());

    Value retValue = super.put(index, value);

    setValue(getRawValue());

    return retValue;
  }

  /**
   * Returns the array ref.
   */
  public override Value put(Value value)
  {
    setRaw(getValue());

    Value retValue = super.put(value);

    setValue(getRawValue());

    return retValue;
  }

  /**
   * Returns the array ref.
   */
  public override Var putVar()
  {
    setRaw(getValue());

    Var retValue = super.putVar();

    setValue(getRawValue());

    return retValue;
  }

  /**
   * Sets the array value, returning the new array, e.g. to handle
   * string update ($a[0] = 'A').
   */
  public override Value append(Value index, Value value)
  {
    setRaw(getValue());

    Value retValue = super.append(index, value);

    setValue(getRawValue());

    return retValue;
  }

  /**
   * Return unset the value.
   */
  public override Value remove(Value index)
  {
    return getValue().remove(index);
  }

  /**
   * Returns the field ref.
   */
  public override Value getField(Env env, StringValue index)
  {
    return getValue().getField(env, index);
  }

  /**
   * Returns the field ref.
   */
  public override Var getFieldVar(Env env, StringValue index)
  {
    setRaw(getValue());

    Var retValue = super.getFieldVar(env, index);

    setValue(getRawValue());

    return retValue;
  }

  /**
   * Returns the array ref.
   */
  public override Value getFieldArg(Env env, StringValue index, bool isTop)
  {
    setRaw(getValue());

    Value retValue = super.getFieldArg(env, index, isTop);

    setValue(getRawValue());

    return retValue;
  }

  /**
   * Returns the field value as an array
   */
  public override Value getFieldArray(Env env, StringValue index)
  {
    setRaw(getValue());

    Value retValue = super.getFieldArray(env, index);

    setValue(getRawValue());

    return retValue;
  }

  /**
   * Returns the field value as an object
   */
  public override Value getFieldObject(Env env, StringValue index)
  {
    setRaw(getValue());

    Value retValue = super.getFieldObject(env, index);

    setValue(getRawValue());

    return retValue;
  }

  /**
   * Sets the field.
   */
  public override Value putField(Env env, StringValue index, Value value)
  {
    setRaw(getValue());

    Value retValue = super.putField(env, index, value);

    setValue(getRawValue());

    return retValue;
  }

  /**
   * Initializes a new field, does not call __set if it @is defined.
   */
  public override void initField(Env env,
                        StringValue name,
                        StringValue canonicalName,
                        Value value)
  {
    setRaw(getValue());

    super.initField(env, name, canonicalName, value);

    setValue(getRawValue());
  }

  /**
   * Sets the field.
   */
  public override Value putThisField(Env env, StringValue index, Value value)
  {
    setRaw(getValue());

    Value retValue = super.putThisField(env, index, value);

    setValue(getRawValue());

    return retValue;
  }

  /**
   * Unsets the field.
   */
  public override void unsetField(StringValue index)
  {
    getValue().unsetField(index);
  }

  /**
   * Takes the values of this array, unmarshalls them to objects of type
   * <i>elementType</i>, and puts them in a java array.
   */
  public override Object valuesToArray(Env env, Class elementType)
  {
    return getValue().valuesToArray(env, elementType);
  }

  /**
   * Returns the character at an index
   */
  public override Value charValueAt(long index)
  {
    return getValue().charValueAt(index);
  }

  /**
   * Sets the character at an index
   */
  public override Value setCharValueAt(long index, Value value)
  {
    return getValue().setCharValueAt(index, value);
  }

  /**
   * Returns true if there are more elements.
   */
  public override bool hasCurrent()
  {
    return getValue().hasCurrent();
  }

  /**
   * Returns the current key
   */
  public override Value key()
  {
    return getValue().key();
  }

  /**
   * Returns the current value
   */
  public override Value current()
  {
    return getValue().current();
  }

  /**
   * Returns the current value
   */
  public override Value next()
  {
    return getValue().next();
  }

  /**
   * Returns the previous value
   */
  public override Value prev()
  {
    return getValue().prev();
  }

  /**
   * Returns the end value.
   */
  public override Value end()
  {
    return getValue().end();
  }

  /**
   * Returns the array pointer.
   */
  public override Value reset()
  {
    return getValue().reset();
  }

  /**
   * Shuffles the array.
   */
  public override Value shuffle()
  {
    return getValue().shuffle();
  }

  /**
   * Pops the top array element.
   */
  public override Value pop(Env env)
  {
    return getValue().pop(env);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash,
                          Value []args)
  {
    return getValue().callMethod(env, methodName, hash, args);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash)
  {
    return getValue().callMethod(env, methodName, hash);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash,
                          Value a0)
  {
    return getValue().callMethod(env, methodName, hash, a0);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash,
                          Value a0, Value a1)
  {
    return getValue().callMethod(env, methodName, hash, a0, a1);
  }

  /**
   * Evaluates a method with 3 args.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash,
                          Value a0, Value a1, Value a2)
  {
    return getValue().callMethod(env, methodName, hash, a0, a1, a2);
  }

  /**
   * Evaluates a method with 4 args.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash,
                          Value a0, Value a1, Value a2, Value a3)
  {
    return getValue().callMethod(env, methodName, hash,
                                 a0, a1, a2, a3);
  }

  /**
   * Evaluates a method with 5 args.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash,
                          Value a0, Value a1, Value a2, Value a3, Value a4)
  {
    return getValue().callMethod(env, methodName, hash,
                                 a0, a1, a2, a3, a4);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethodRef(Env env,
                             StringValue methodName, int hash,
                             Value []args)
  {
    return getValue().callMethodRef(env, methodName, hash, args);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethodRef(Env env, StringValue methodName, int hash)
  {
    return getValue().callMethodRef(env, methodName, hash);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethodRef(Env env, StringValue methodName, int hash,
                             Value a0)
  {
    return getValue().callMethodRef(env, methodName, hash, a0);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethodRef(Env env, StringValue methodName, int hash,
                             Value a0, Value a1)
  {
    return getValue().callMethodRef(env, methodName, hash,
                                    a0, a1);
  }

  /**
   * Evaluates a method with 3 args.
   */
  public override Value callMethodRef(Env env, StringValue methodName, int hash,
                             Value a0, Value a1, Value a2)
  {
    return getValue().callMethodRef(env, methodName, hash,
                                    a0, a1, a2);
  }

  /**
   * Evaluates a method with 4 args.
   */
  public override Value callMethodRef(Env env, StringValue methodName, int hash,
                             Value a0, Value a1, Value a2, Value a3)
  {
    return getValue().callMethodRef(env, methodName, hash,
                                    a0, a1, a2, a3);
  }

  /**
   * Evaluates a method with 5 args.
   */
  public override Value callMethodRef(Env env, StringValue methodName, int hash,
                             Value a0, Value a1, Value a2, Value a3, Value a4)
  {
    return getValue().callMethodRef(env, methodName, hash,
                                    a0, a1, a2, a3, a4);
  }

  /**
   * Evaluates a method.
   */
  /*
  public override Value callClassMethod(Env env, AbstractFunction fun, Value []args)
  {
    return getValue().callClassMethod(env, fun, args);
  }
  */

  /**
   * Prints the value.
   */
  public override void print(Env env)
  {
    getValue().print(env);
  }

  /**
   * Serializes the value.
   */
  public override void serialize(Env env, StringBuilder sb, SerializeMap map)
  {
    getValue().serialize(env, sb, map);
  }

  public override void varDumpImpl(Env env,
                          WriteStream @out,
                          int depth,
                          IdentityHashMap<Value, String> valueSet)
    
  {
    @out.print("&");
    getValue().varDump(env, @out, depth, valueSet);
  }

  private void setRaw(Value value)
  {
    _value = value;
  }

  private Value getRawValue()
  {
    return _value;
  }

  //
  // Java Serialization
  //

  public Object writeReplace()
  {
    return getValue();
  }
}

}
