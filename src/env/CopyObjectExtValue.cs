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
 * Represents a copy of an object value for serialization/apc
 */
public class CopyObjectExtValue : ObjectExtValue
{
  private CopyRoot _root;

  public CopyObjectExtValue(Env env, ObjectExtValue copy, CopyRoot root)
  {
    super(env, copy, root);

    _root = root;
  }

  /**
   * Returns the array ref.
   */
  public override Var getFieldVar(Env env, StringValue name)
  {
    _root.setModified();

    return super.getFieldVar(env, name);
  }

  /**
   * Returns the array ref.
   */
  public override Var getThisFieldVar(Env env, StringValue name)
  {
    _root.setModified();

    return super.getThisFieldVar(env, name);
  }

  /**
   * Returns the value as an argument which may be a reference.
   */
  public override Value getFieldArg(Env env, StringValue name, bool isTop)
  {
    _root.setModified();

    return super.getFieldArg(env, name, isTop);
  }

  /**
   * Returns the value as an argument which may be a reference.
   */
  public override Value getThisFieldArg(Env env, StringValue name)
  {
    _root.setModified();

    return super.getThisFieldArg(env, name);
  }

  /**
   * Returns the value as an argument which may be a reference.
   */
  public override Value getFieldArgRef(Env env, StringValue name)
  {
    _root.setModified();

    return super.getFieldArgRef(env, name);
  }

  /**
   * Returns the value as an argument which may be a reference.
   */
  public override Value getThisFieldArgRef(Env env, StringValue name)
  {
    _root.setModified();

    return super.getThisFieldArgRef(env, name);
  }

  /**
   * Adds a new value.
   */
  public override Value putField(Env env, StringValue name, Value value)
  {
    _root.setModified();

    return super.putField(env, name, value);
  }

  /**
   * Sets/adds field to this object.
   */
  public override Value putThisField(Env env, StringValue name, Value value)
  {
    _root.setModified();

    return super.putThisField(env, name, value);
  }

  protected Value putFieldExt(Env env, StringValue name, Value value)
  {
    return null;
  }

  /**
   * Adds a new value to the object.
   */
  public override void initField(Env env, StringValue name,
                        StringValue canonicalName, Value value)
  {
    _root.setModified();

    super.initField(env, canonicalName, value);
  }

  /**
   * Removes a value.
   */
  public override void unsetField(StringValue name)
  {
    _root.setModified();

    super.unsetField(name);
  }

  /**
   * Removes the field ref.
   */
  public override void unsetArray(Env env, StringValue name, Value index)
  {
    _root.setModified();

    super.unsetArray(env, name, index);
  }

  /**
   * Removes the field ref.
   */
  public void unsetThisArray(Env env, StringValue name, Value index)
  {
    _root.setModified();

    super.unsetThisArray(env, name, index);
  }

  /**
   * Sets the array value with the given key.
   */
  public override Value put(Value key, Value value)
  {
    _root.setModified();

    return super.put(key, value);
  }

  /**
   * Appends a new array value
   */
  public override Value put(Value value)
  {
    _root.setModified();

    return super.put(value);
  }

  /**
   * Unsets the array value
   */
  public override Value remove(Value key)
  {
    _root.setModified();

    return super.remove(key);
  }
}

}
