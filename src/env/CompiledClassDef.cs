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
 * Represents a PHP class value.
 */
public class CompiledClassDef : ClassDef {
  private ArrayList<String> _fieldNames
    = new ArrayList<String>();

  private IdentityIntMap _fieldMap2
    = new IdentityIntMap();

  protected LinkedHashMap<StringValue,ClassField> _fieldMap
    = new LinkedHashMap<StringValue,ClassField>();

  //protected ArrayValue _extFields = new ArrayValueImpl();
  protected Value _parent;
  protected bool _isFinal;

  public CompiledClassDef(String name,
                          string parent,
                          string []ifaceList)
  {
    super(null, name, parent, ifaceList, ClassDef.NULL_STRING_ARRAY);
  }

  public CompiledClassDef(String name,
                          string parent,
                          string []ifaceList,
                          string []traitList)
   : base(null, name, parent, ifaceList, traitList) {
  }

  public CompiledClassDef(Location location,
                          string name,
                          string parent,
                          string []ifaceList)
   : base(location, name, parent, ifaceList, ClassDef.NULL_STRING_ARRAY) {
  }

  public CompiledClassDef(Location location,
                          string name,
                          string parent,
                          string []ifaceList,
                          bool isFinal)
  {
    this(location, name, parent, ifaceList, ClassDef.NULL_STRING_ARRAY, isFinal);
  }

  public CompiledClassDef(Location location,
                          string name,
                          string parent,
                          string []ifaceList,
                          string []traitList)
   : base(location, name, parent, ifaceList, traitList) {
  }

  public CompiledClassDef(Location location,
                          string name,
                          string parent,
                          string []ifaceList,
                          string []traitList,
                          bool isFinal)
   : base(location, name, parent, ifaceList, traitList) {

    _isFinal = isFinal;
  }

  /**
   * Initialize the quercus class.
   */
  public void initClassDef()
  {
  }

  /**
   * Initialize the quercus class methods.
   */
  public override void initClassMethods(QuercusClass cl, string bindingClassName)
  {
  }

  /**
   * Initialize the quercus class fields.
   */
  public override void initClassFields(QuercusClass cl, string bindingClassName)
  {
  }

  /**
   * Returns true for a class.
   */
  public override bool isFinal()
  {
    return _isFinal;
  }

  /**
   * Returns the field index.
   */
  public int findFieldIndex(String name)
  {
    return _fieldMap2.get(name);
  }

  /**
   * Returns the key set.
   */
  public ArrayList<String> getFieldNames()
  {
    return _fieldNames;
  }

  /**
   * Returns the field index.
   */
  protected void addFieldIndex(String name, int id)
  {
    _fieldMap2.put(name, id);
    _fieldNames.add(name);
  }

  public override ClassField getField(StringValue name)
  {
    return _fieldMap.get(name);
  }

  /**
   * Returns the constructor
   */
  public override AbstractFunction findConstructor()
  {
    return null;
  }

  /**
   * Creates a new instance.
   */
  public override void initInstance(Env env, Value value, bool isInitFieldValues)
  {
  }

  /**
   * Adds a value.
   */
  public Value get(Value name)
  {
    throw new UnsupportedOperationException();
    /*
    if (_extFields != null) {
      Value value = _extFields.get(name);
      return value;
    }
    else
      return NullValue.NULL;
    */
  }

  /**
   * Returns a reference to the field
   */
  public Value getRef(Value name)
  {
    throw new UnsupportedOperationException();
    /*
    if (_extFields == null)
      _extFields = new ArrayValue();

    Value ref = _extFields.getRef(name);

    return ref;
    */
  }

  /**
   * Returns a reference to the field
   */
  public Value getArgRef(Value name)
  {
    throw new UnsupportedOperationException();
    /*
    if (_extFields == null)
      _extFields = new ArrayValue();

    return _extFields.getArgRef(name);
    */
  }

  /**
   * Returns the field value, if unset, creates an array.
   */
  public Value getArray(Value name)
  {
    throw new UnsupportedOperationException();

    /*
    Value value = get(name);

    if (! value.isset()) {
      value = new ArrayValue();

      put(name, value);
    }

    return value;
    */
  }

  /**
   * Returns the field value, if unset, creates an object.
   */
  public Value getObject(Env env, Value name)
  {
    throw new UnsupportedOperationException();
    /*
    Value value = get(name);

    if (! value.isset()) {
      value = env.createObject();

      put(name, value);
    }

    return value;
    */
  }

  /**
   * Returns the field value, if unset, creates an ArgGetValue.
   */
  public Value getArg(Value name)
  {
    throw new UnsupportedOperationException();

    /*
    Value value = get(name);

    if (value.isset()) {
      return value;
    }
    else {
      // quercus/3d55
      return new ArgGetValue(this, name);
    }

    return null;
    */
  }

  /**
   * Adds a value.
   */
  public Value put(Value name, Value value)
  {
    throw new UnsupportedOperationException();

    /*
    if (_extFields == null)
      _extFields = new ArrayValue();

    _extFields.put(name, value);

    return value;
    */
  }

  /**
   * Adds a value.
   */
  public Value put(Value value)
  {
    throw new UnsupportedOperationException();

    /*
    if (_extFields == null)
      _extFields = new ArrayValue();

    _extFields.put(value);

    return value;
    */
  }

  /**
   * Adds a value.
   */
  public Value putRef()
  {
    throw new UnsupportedOperationException();

    /*
    // quercus/3d8i

    if (_extFields == null)
      _extFields = new ArrayValue();

    return _extFields.putRef();
    */
  }

  /**
   * Removes a value.
   */
  public Value remove(Value name)
  {
    throw new UnsupportedOperationException();
    /*
    // quercus/3d91

    if (_extFields != null) {
      Value value = _extFields.remove(name);
      return value;
    }
    else
      return NullValue.NULL;
    */
  }

  /**
   * Creates a new instance.
   */
  public Value newInstance()
  {
    /*
    try {
      return getClass().newInstance();
    } catch (Exception e) {
      throw new QuercusRuntimeException(e);
    }
    */
    throw new UnsupportedOperationException();
  }

  /**
   * Eval new
   */
  public override Value callNew(Env env, Expr []args)
  {
    return null;
  }

  /**
   * Eval new
   */
  public override Value callNew(Env env, Value []args)
  {
    return null;
  }
}

}
