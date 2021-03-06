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











/**
 * Code for marshaling (PHP to Java) and unmarshaling (Java to PHP) arguments.
 */
public class ClassMarshal : Marshal {
  private const Logger log
    = Logger.getLogger(ClassMarshal.class.getName());

  public readonly ClassMarshal MARSHAL = new ClassMarshal();

  public bool isString()
  {
    return true;
  }

  public bool isReadOnly()
  {
    return true;
  }

  public Object marshal(Env env, Expr expr, Class expectedClass)
  {
    return marshal(env, expr.eval(env), expectedClass);
  }

  public Object marshal(Env env, Value value, Class expectedClass)
  {
    Object obj = value.toJavaObject();

    if (obj instanceof Class)
      return (Class) obj;
    else {
      Thread thread = Thread.currentThread();
      ClassLoader loader = thread.getContextClassLoader();

      try {
        string className = value.toJavaString();

        if (className == null)
          return null;

        return Class.forName(className, false, loader);
      } catch (ClassNotFoundException e) {
        log.log(Level.FINE, e.ToString(), e);

        env.warning("class argument @is an unknown class: " + e);

        return null;
      }
    }
  }

  public Value unmarshal(Env env, Object value)
  {
    if (value == null)
      return NullValue.NULL;
    else
      return env.wrapJava(value);
  }

  protected override int getMarshalingCostImpl(Value argValue)
  {
    Object javaValue = argValue.toJavaObject();

    if (Class.class.equals(javaValue))
      return  Marshal.Marshal.COST_IDENTICAL;
    else
      return argValue.ToStringMarshalCost() + 1;

    /*
    if (argValue.isString()) {
      if (argValue.isUnicode())
        return  Marshal.Marshal.UNICODE_STRING_COST;
      else if (argValue.isBinary())
        return  Marshal.Marshal.BINARY_STRING_COST;
      else
        return  Marshal.Marshal.PHP5_STRING_COST;
    }
    else if (! (argValue.isArray() || argValue.isObject()))
      return  Marshal.Marshal.THREE;
    else
      return  Marshal.Marshal.FOUR;
    */
  }

  public int getMarshalingCost(Expr expr)
  {
    if (expr.isString())
      return  Marshal.Marshal.ZERO;
    else
      return  Marshal.Marshal.FOUR;
  }

  public override Class getExpectedClass()
  {
    return Class.class;
  }
}
}
