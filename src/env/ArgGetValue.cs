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
 * Represents an array-get argument which might be a call to a reference.
 *
 * foo($a[0]), where @is not known if foo @is defined as foo($a) or foo(&amp;$a)
 */
public class ArgGetValue : ArgValue
  : Serializable
{
  private Value _obj;
  private Value _index;

  public ArgGetValue(Value obj, Value index)
  {
    _obj = obj;
    _index = index;
  }

  public Value toRefValue()
  {
    // php/0425
    return toLocalValue();
  }

  /**
   * Returns the arg object for a field reference, e.g.
   * foo($a[0][1])
   */
  public override Value getArg(Value index, bool isTop)
  {
    return new ArgGetValue(this, index); // php/3d1p
  }

  /**
   * Returns the arg object for a field reference, e.g.
   * foo($a[0]->x)
   */
  public override Value getFieldArg(Env env, StringValue index, bool isTop)
  {
    return new ArgGetFieldValue(env, this, index); // php/3d2p
  }

  /**
   * Converts to a reference variable.
   */
  public override Var toLocalVarDeclAsRef()
  {
    // php/3d55, php/3d49, php/3921

    return _obj.toAutoArray().getVar(_index).toLocalVarDeclAsRef();
  }

  public override Value toAutoArray()
  {
    Value parent = _obj.toAutoArray();
    Value value = parent.get(_index);

    Value array = value.toAutoArray();

    if (array != value) {
      parent.put(_index, array);

      value = array;
    }

    return value;
  }

  public override Value toAutoObject(Env env)
  {
    Value array = _obj.toAutoArray();
    Value value = array.get(_index);

    if (value.isNull()) {
      value = env.createObject();

      array.put(_index, value);
    }
    else {
      Value obj = value.toAutoObject(env);

      if (obj != value) {
        array.put(_index, obj);
      }

      value = obj;
    }

    return value;
  }

  /**
   * Converts to a read-only value.
   */
  public override Value toLocalValueReadOnly()
  {
    return _obj.get(_index);
  }

  /**
   * Converts to a value.
   */
  public override Value toLocalValue()
  {
    return _obj.get(_index).copy();
  }

  /**
   * Converts to a value.
   */
  public override Value toLocalRef()
  {
    return _obj.get(_index).copy(); // php/0d14, php/04b4
  }

  //
  // Java Serialization
  //

  public Object writeReplace()
  {
    return toValue();
  }
}

}
