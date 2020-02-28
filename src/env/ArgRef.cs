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
 * Represents a reference to a PHP variable in a function call.
 */
public class ArgRef : Value
  : Serializable
{
  private Var _var;

  public ArgRef(Var var)
  {
    _var = var;
  }

  @Override
  public bool hasCurrent()
  {
    return _var.hasCurrent();
  }

  /**
   * Returns true for an implementation of a class
   */
  public override bool isA(Env env, string name)
  {
    return _var.isA(env, name);
  }

  /**
   * True for a long
   */
  public override bool isLongConvertible()
  {
    return _var.isLongConvertible();
  }

  /**
   * True to a double.
   */
  public override bool isDoubleConvertible()
  {
    return _var.isDoubleConvertible();
  }

  /**
   * True for a number
   */
  public override bool isNumberConvertible()
  {
    return _var.isNumberConvertible();
  }

  /**
   * Returns true for a long-value.
   */
  public bool isLong()
  {
    return _var.isLong();
  }

  /**
   * Returns true for a long-value.
   */
  public bool isDouble()
  {
    return _var.isDouble();
  }

  public override ArrayValue toArrayValue(Env env)
  {
    // php/3co1
    return _var.toArrayValue(env);
  }

  /**
   * Converts to a boolean.
   */
  public override bool toBoolean()
  {
    return _var.toBoolean();
  }

  /**
   * Converts to a long.
   */
  public override long toLong()
  {
    return _var.toLong();
  }

  /**
   * Converts to a double.
   */
  public override double toDouble()
  {
    return _var.toDouble();
  }

  /**
   * Converts to a string.
   * @param env
   */
  public override StringValue ToString(Env env)
  {
    return _var.ToString(env);
  }

  /**
   * Converts to an object.
   */
  public override Value toObject(Env env)
  {
    return _var.toObject(env);
  }

  /**
   * Converts to an object.
   */
  public override Object toJavaObject()
  {
    return _var.toJavaObject();
  }

  /**
   * Converts to a raw value.
   */
  public override Value toValue()
  {
    return _var.toValue();
  }

  /**
   * Returns true for an object.
   */
  public override bool isObject()
  {
    return _var.isObject();
  }

  /**
   * Returns true for an array.
   */
  public override bool isArray()
  {
    return _var.isArray();
  }

  /**
   * Copy the value.
   */
  public override Value copy()
  {
    // quercus/0d05
    return this;
  }

  /**
   * Converts to an argument value.
   */
  public override Value toLocalValueReadOnly()
  {
    return _var;
  }

  /**
   * Converts to an argument value.
   */
  public override Value toLocalValue()
  {
    // php/0471, php/3d4a
    return _var.toLocalValue();
  }

  /**
   * Converts to an argument value.
   */
  public override Value toLocalRef()
  {
    return _var;
  }

  /**
   * Converts to an argument value.
   */
  public override Var toLocalVar()
  {
    return _var;
  }

  /**
   * Converts to an argument value.
   */
  public override Value toRefValue()
  {
    return _var;
  }

  /**
   * Converts to a variable
   */
  public override Var toVar()
  {
    return _var;
  }

  /**
   * Converts to a reference variable
   */
  public override Var toLocalVarDeclAsRef()
  {
    return _var;
  }

  public override StringValue ToStringValue()
  {
    return _var.ToStringValue();
  }

  public override StringValue toBinaryValue(Env env)
  {
    return _var.toBinaryValue(env);
  }

  public override StringValue toUnicodeValue(Env env)
  {
    return _var.toUnicodeValue(env);
  }

  public override StringValue ToStringBuilder()
  {
    return _var.ToStringBuilder();
  }

  public override StringValue ToStringBuilder(Env env)
  {
    return _var.ToStringBuilder(env);
  }

  public override java.io.InputStream toInputStream()
  {
    return _var.toInputStream();
  }

  public override Value append(Value index, Value value)
  {
    return _var.append(index, value);
  }

  public override Value containsKey(Value key)
  {
    return _var.containsKey(key);
  }

  public override Value copyArrayItem()
  {
    return _var.copyArrayItem();
  }

  public override Value current()
  {
    return _var.current();
  }

  public override Value getArray()
  {
    return _var.getArray();
  }

  public override Value getArray(Value index)
  {
    return _var.getArray(index);
  }

  public override int getCount(Env env)
  {
    return _var.getCount(env);
  }

  public override Value[] getKeyArray(Env env)
  {
    return _var.getKeyArray(env);
  }

  public override Value key()
  {
    return _var.key();
  }

  public override Value next()
  {
    return _var.next();
  }

  public override ArrayValue toArray()
  {
    return _var.toArray();
  }

  public override Value toAutoArray()
  {
    return _var.toAutoArray();
  }

  /**
   * Negates the value.
   */
  public override Value neg()
  {
    return _var.neg();
  }

  /**
   * Adds to the following value.
   */
  public override Value add(Value rValue)
  {
    return _var.add(rValue);
  }

  /**
   * Adds to the following value.
   */
  public override Value add(long rValue)
  {
    return _var.add(rValue);
  }

  /**
   * Pre-increment the following value.
   */
  public override Value preincr(int incr)
  {
    return _var.preincr(incr);
  }

  /**
   * Post-increment the following value.
   */
  public override Value postincr(int incr)
  {
    return _var.postincr(incr);
  }

  /**
   * Increment the following value.
   */
  public override Value increment(int incr)
  {
    return _var.increment(incr);
  }

  /**
   * Subtracts to the following value.
   */
  public override Value sub(Value rValue)
  {
    return _var.sub(rValue);
  }

  /**
   * Subtracts to the following value.
   */
  public override Value sub(long rValue)
  {
    return _var.sub(rValue);
  }

  /**
   * Multiplies to the following value.
   */
  public override Value mul(Value rValue)
  {
    return _var.mul(rValue);
  }

  /**
   * Multiplies to the following value.
   */
  public override Value mul(long lValue)
  {
    return _var.mul(lValue);
  }

  /**
   * Divides the following value.
   */
  public override Value div(Value rValue)
  {
    return _var.div(rValue);
  }

  /**
   * Shifts left by the value.
   */
  public override Value lshift(Value rValue)
  {
    return _var.lshift(rValue);
  }

  /**
   * Shifts right by the value.
   */
  public override Value rshift(Value rValue)
  {
    return _var.rshift(rValue);
  }

  /**
   * Absolute value.
   */
  public Value abs()
  {
    return _var.abs();
  }

  /**
   * Returns true for equality
   */
  public override bool eql(Value rValue)
  {
    return _var.eql(rValue);
  }

  /**
   * Returns the array/object size
   */
  public override int getSize()
  {
    return _var.getSize();
  }

  public override Iterator<Map.Entry<Value, Value>> getIterator(Env env)
  {
    return _var.getIterator(env);
  }

  public override Iterator<Value> getKeyIterator(Env env)
  {
    return _var.getKeyIterator(env);
  }

  public override Iterator<Value> getValueIterator(Env env)
  {
    return _var.getValueIterator(env);
  }

  /**
   * Returns the array ref.
   */
  public override Value get(Value index)
  {
    return _var.get(index);
  }

  /**
   * Returns the array ref.
   */
  public override Var getVar(Value index)
  {
    return _var.getVar(index);
  }

  /**
   * Returns the array ref.
   */
  public override Value put(Value index, Value value)
  {
    return _var.put(index, value);
  }

  /**
   * Returns the array ref.
   */
  public override Value put(Value value)
  {
    return _var.put(value);
  }

  /**
   * Returns the character at an index
   */
  /* XXX: need test first
  public Value charAt(long index)
  {
    return _ref.charAt(index);
  }
  */

  /**
   * Evaluates a method.
   */
  public override Value callMethod(Env env,
                          StringValue methodName, int hash,
                          Value []args)
  {
    return _var.callMethod(env, methodName, hash, args);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash)
  {
    return _var.callMethod(env, methodName, hash);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethod(Env env,
                          StringValue methodName, int hash,
                          Value a1)
  {
    return _var.callMethod(env, methodName, hash, a1);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash,
                          Value a1, Value a2)
  {
    return _var.callMethod(env, methodName, hash, a1, a2);
  }

  /**
   * Evaluates a method with 3 args.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash,
                          Value a1, Value a2, Value a3)
  {
    return _var.callMethod(env, methodName, hash,
                           a1, a2, a3);
  }

  /**
   * Evaluates a method with 4 args.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash,
                          Value a1, Value a2, Value a3, Value a4)
  {
    return _var.callMethod(env, methodName, hash,
                           a1, a2, a3, a4);
  }

  /**
   * Evaluates a method with 5 args.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash,
                          Value a1, Value a2, Value a3, Value a4, Value a5)
  {
    return _var.callMethod(env, methodName, hash,
                           a1, a2, a3, a4, a5);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethodRef(Env env,
                             StringValue methodName, int hash,
                             Value []args)
  {
    return _var.callMethodRef(env, methodName, hash, args);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethodRef(Env env, StringValue methodName, int hash)
  {
    return _var.callMethodRef(env, methodName, hash);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethodRef(Env env,
                             StringValue methodName, int hash,
                             Value a1)
  {
    return _var.callMethodRef(env, methodName, hash, a1);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethodRef(Env env, StringValue methodName, int hash,
                             Value a1, Value a2)
  {
    return _var.callMethodRef(env, methodName, hash,
                              a1, a2);
  }

  /**
   * Evaluates a method with 3 args.
   */
  public override Value callMethodRef(Env env, StringValue methodName, int hash,
                             Value a1, Value a2, Value a3)
  {
    return _var.callMethodRef(env, methodName, hash, a1, a2, a3);
  }

  /**
   * Evaluates a method with 4 args.
   */
  public override Value callMethodRef(Env env, StringValue methodName, int hash,
                             Value a1, Value a2, Value a3, Value a4)
  {
    return _var.callMethodRef(env, methodName, hash,
                              a1, a2, a3, a4);
  }

  /**
   * Evaluates a method with 5 args.
   */
  public override Value callMethodRef(Env env,
                             StringValue methodName, int hash,
                             Value a1, Value a2, Value a3, Value a4, Value a5)
  {
    return _var.callMethodRef(env, methodName, hash,
                              a1, a2, a3, a4, a5);
  }

  /**
   * Evaluates a method.
   */
  /*
  public override Value callClassMethod(Env env, AbstractFunction fun, Value []args)
  {
    return _var.callClassMethod(env, fun, args);
  }
  */

  /**
   * Serializes the value.
   */
  public void serialize(Env env, StringBuilder sb)
  {
    _var.serialize(env, sb);
  }

  /*
   * Serializes the value.
   *
   * @param sb holds result of serialization
   * @param serializeMap holds reference indexes
   */
  public void serialize(Env env, StringBuilder sb, SerializeMap serializeMap)
  {
    _var.serialize(env, sb, serializeMap);
  }

  /**
   * Prints the value.
   * @param env
   */
  public override void print(Env env)
  {
    _var.print(env);
  }

  public override void varDumpImpl(Env env,
                          WriteStream @out,
                          int depth,
                          IdentityHashMap<Value,String> valueSet)
    
  {
    @out.print("&");
    toValue().varDumpImpl(env, @out, depth, valueSet);
  }

  protected override void printRImpl(Env env,
                            WriteStream @out,
                            int depth,
                            IdentityHashMap<Value, String> valueSet)
    
  {
    toValue().printRImpl(env, @out, depth, valueSet);
  }

  //
  // Java Serialization
  //

  public Object writeReplace()
  {
    return _var;
  }
}

}
