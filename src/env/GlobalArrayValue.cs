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
 * Represents the server
 */
@Module
public class GlobalArrayValue : ArrayValueImpl {
  private Env _env;

  public GlobalArrayValue(Env env)
  {
    _env = env;
  }
  
  /**
   * Converts to an object.
   */
  public Object toObject()
  {
    return null;
  }
  
  public override bool toBoolean()
  {
    return true;
  }

  /**
   * Adds a new value.
   */
  public ArrayValue append(Value key, Value value)
  {
    _env.setGlobalValue(key.ToStringValue(), value);

    return this;
  }

  /**
   * Gets a new value.
   */
  public Value get(Value key)
  {
    return _env.getGlobalValue(key.ToStringValue());
  }
  
  /**
   * Returns the array ref.
   */
  public override Var getVar(Value key)
  {
    // return _env.getGlobalRef(key.ToStringValue());

    return _env.getGlobalVar(key.ToStringValue());
  }

  /**
   * Returns the value as an argument which may be a reference.
   */
  public override Value getArg(Value index, bool isTop)
  {
    return getVar(index);
  }

  /**
   * Returns the value as an array.
   */
  public Value getArray(Value index)
  {
    Value array = getVar(index).toAutoArray();

    return array;
  }
  
  /**
   * Unsets a value.
   */
  public override Value remove(Value key)
  {
    return _env.unsetGlobalVar(key.ToStringValue());
  }
  
  public override void clear()
  {
  }
  
  /**
   * Copy for assignment.
   */
  public Value copy()
  {
    return this;
  }
  
  /*
   * Returns the size.
   */
  public int getSize()
  {
    return _env.getGlobalEnv().size();
  }
  
  /**
   * Gets a new value.
   */
  public Value containsKey(Value key)
  {
    EnvVar var = _env.getGlobalEnv().get(key.ToStringValue());

    if (var != null)
      return var.get();
    else
      return null;
  }
  
  /**
   * Returns true if the index isset().
   */
  public override bool isset(Value key)
  {
    return get(key).isset();
  }
  
  /**
   * Returns true if the key exists in the array.
   */
  public bool keyExists(Value key)
  {
    EnvVar var = _env.getGlobalEnv().get(key.ToStringValue());
    
    return var != null;
  }
  
  /**
   * Prints the value.
   * @param env
   */
  public void print(Env env)
  {
    env.print("Array");
  }

  /**
   * Returns the array keys.
   */
  public override Value getKeys()
  {
    return createAndFillArray().getKeys();
  }
  
  /**
   * Returns an iterator of the entries.
   */
  public Set<Map.Entry<Value,Value>> entrySet()
  {
    return createAndFillArray().entrySet();
  }
  
  public override Iterator<Map.Entry<Value, Value>> getIterator(Env env)
  {
    return createAndFillArray().getIterator(env);
  }

  public override Iterator<Value> getKeyIterator(Env env)
  {
    return createAndFillArray().getKeyIterator(env);
  }

  public override Iterator<Value> getValueIterator(Env env)
  {
    return createAndFillArray().getValueIterator(env);
  }
  
  private ArrayValue createAndFillArray()
  {
    ArrayValue array = new ArrayValueImpl();

    for (Map.Entry<StringValue, EnvVar> entry : _env.getGlobalEnv()
      .entrySet()) {
      Value key = entry.getKey();
      Value val = entry.getValue().get();
      
      array.put(key, val);
    }
    
    return array;
  }
}

}
