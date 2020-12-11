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
 * Represents a PHP array value.
 */
public class ArrayValueWrapper : ArrayValue {
  private ArrayValue _array;

  protected ArrayValueWrapper(ArrayValue array)
  {
    _array = array;
  }

  /**
   * Returns the wrapped array.
   */
  public override ArrayValue getArray()
  {
    return _array;
  }
  
  /**
   * Copy for assignment.
   */
  public override Value copy()
  {
    return _array.copy();
  }
  
  /**
   * Copy for assignment.
   */
  public override Value copySaveFunArg()
  {
    return _array.copySaveFunArg();
  }
  
  /**
   * Copy for serialization
   */
  public override Value copy(Env env, IdentityHashMap<Value,Value> map)
  {
    return _array.copy(env, map);
  }

  /**
   * Returns the size.
   */
  public override int getSize()
  {
    return _array.getSize();
  }

  /**
   * Clears the array
   */
  public override void clear()
  {
    _array.clear();
  }
  
  /**
   * Adds a new value.
   */
  public override Value put(Value key, Value value)
  {
    return _array.put(key, value);
  }
  
  /**
   * Adds a new value.
   */
  public override ArrayValue append(Value key, Value value)
  {
    return _array.append(key, value);
  }

  /**
   * Add
   */
  public override Value put(Value value)
  {
    return _array.put(value);
  }

  /**
   * Add to front.
   */
  public override ArrayValue unshift(Value value)
  {
    return _array.unshift(value);
  }

  /**
   * Splices values
   */
  public override ArrayValue splice(int start, int end, ArrayValue replace)
  {
    return _array.splice(start, end, replace);
  }

  /**
   * Returns the value as an argument which may be a reference.
   */
  public override Value getArg(Value index, bool isTop)
  {
    return _array.getArg(index, isTop);
  }

  /**
   * Sets the array ref.
   */
  public override Var putVar()
  {
    return _array.putVar();
  }

  /**
   * Creatse a tail index.
   */
  public override Value createTailKey()
  {
    return _array.createTailKey();
  }

  /**
   * Gets a new value.
   */
  public override Value get(Value key)
  {
    return _array.get(key);
  }

  /**
   * Removes a value.
   */
  public override Value remove(Value key)
  {
    return _array.remove(key);
  }
  
  /**
   * Returns true if the index isset().
   */
  public override bool isset(Value key)
  {
    return _array.isset(key);
  }

  /**
   * Returns the array ref.
   */
  public override Var getVar(Value index)
  {
    return _array.getVar(index);
  }
  
  /**
   * Pops the top value.
   */
  public override Value pop(Env env)
  {
    return _array.pop(env);
  }

  /**
   * Shuffles the array
   */
  public override Value shuffle()
  {
    return _array.shuffle();
  }

  /**
   * Returns the head.
   */
  public override Entry getHead()
  {
    return _array.getHead();
  }

  /**
   * Returns the tail.
   */
  protected override Entry getTail()
  {
    return _array.getTail();
  }
  
  /**
   * Returns the current value.
   */
  public override Value current()
  {
    return _array.current();
  }

  /**
   * Returns the current key
   */
  public override Value key()
  {
    return _array.key();
  }

  /**
   * Returns true if there are more elements.
   */
  public override bool hasCurrent()
  {
    return _array.hasCurrent();
  }

  /**
   * Returns the next value.
   */
  public override Value next()
  {
    return _array.next();
  }

  /**
   * Returns the previous value.
   */
  public override Value prev()
  {
    return _array.prev();
  }

  /**
   * The each iterator
   */
  public override Value each()
  {
    return _array.each();
  }

  /**
   * Returns the first value.
   */
  public override Value reset()
  {
    return _array.reset();
  }

  /**
   * Returns the last value.
   */
  public override Value end()
  {
    return _array.end();
  }
  
  /**
   * Returns the corresponding key if this array contains the given value
   *
   * @param value to search for in the array
   *
   * @return the key if it @is found in the array, NULL otherwise
   */
  public override Value contains(Value key)
  {
    return _array.contains(key);
  }
  
  /**
   * Returns the corresponding key if this array contains the given value
   *
   * @param value to search for in the array
   *
   * @return the key if it @is found in the array, NULL otherwise
   */
  public override Value containsStrict(Value key)
  {
    return _array.containsStrict(key);
  }
  
  /**
   * Returns the corresponding value if this array contains the given key
   * 
   * @param key to search for in the array
   * 
   * @return the value if it @is found in the array, NULL otherwise
   */
  public override Value containsKey(Value key)
  {
    return _array.containsKey(key);
  }

  public override Value add(Value rValue)
  {
    return _array.add(rValue);
  }

  public override Iterator<Map.Entry<Value, Value>> getIterator(Env env)
  {
    return _array.getIterator(env);
  }

  public override Iterator<Value> getKeyIterator(Env env)
  {
    return _array.getKeyIterator(env);
  }

  public override Iterator<Value> getValueIterator(Env env)
  {
    return _array.getValueIterator(env);
  }
}

}
