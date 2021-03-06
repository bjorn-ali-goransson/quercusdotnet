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
 * Case-insensitive method mapping
 */
public class MethodMap<V>
{
  private readonly L10N L = new L10N(MethodMap.class);

  private QuercusClass _quercusClass;
  private ClassDef _classDef;

  private Entry<V> []_entries = new Entry[16];
  private int _prime = Primes.getBiggestPrime(_entries.length);
  private int _size;

  private Entry<V> _head;
  private Entry<V> _tail;

  public MethodMap(QuercusClass quercusClass, ClassDef classDef)
  {
    _quercusClass = quercusClass;
    _classDef = classDef;
  }

  public void put(StringValue name, V value)
  {
    if (_entries.length <= _size * 4)
      resize();

    int hash = name.hashCodeCaseInsensitive();

    int bucket = (hash & 0x7fffffff) % _prime;

    Entry<V> entry;
    for (entry = _entries[bucket]; entry != null; entry = entry.getNext()) {
      StringValue entryKey = entry.getKey();

      if (name == entryKey || name.equalsIgnoreCase(entryKey)) {
        entry.setValue(value);

        return;
      }
    }

    entry = createEntry(name, value);

    entry._next = _entries[bucket];
    _entries[bucket] = entry;
    _size++;

  }

  private Entry<V> createEntry(StringValue name, V value)
  {
    Entry<V> entry = new Entry<V>(name, value);

    if (_head == null) {
      _head = entry;
    }

    if (_tail != null) {
      _tail.setAbsoluteNext(entry);
    }

    _tail = entry;

    return entry;
  }

  public bool containsKey(StringValue key)
  {
    int hash = key.hashCodeCaseInsensitive();

    int bucket = (hash & 0x7fffffff) % _prime;

    for (Entry<V> entry = _entries[bucket];
         entry != null;
         entry = entry.getNext()) {
      StringValue entryKey = entry.getKey();

      if (key == entryKey || key.equalsIgnoreCase(entryKey))
        return true;
    }

    return false;
  }

  public V get(final StringValue key, int hash)
  {
    return get(key, hash, false);
  }

  public V getStatic(final StringValue key, int hash)
  {
    return get(key, hash, true);
  }

  public V get(final StringValue key, int hash, bool isStatic)
  {
    int bucket = (hash & 0x7fffffff) % _prime;

    for (Entry<V> entry = _entries[bucket];
         entry != null;
         entry = entry.getNext()) {
      StringValue entryKey = entry.getKey();

      if (key == entryKey || key.equalsIgnoreCase(entryKey))
        return entry._value;
    }

    AbstractFunction call = null;

    if (isStatic) {
      if (_quercusClass != null) {
        call = _quercusClass.getCallStatic();
      }
      else if (_classDef != null) {
        call = _classDef.getCallStatic();
      }
    }
    else {
      if (_quercusClass != null) {
        call = _quercusClass.getCall();
      }
      else if (_classDef != null) {
        call = _classDef.getCall();
      }
    }

    if (call != null) {
      return (V) new FunSpecialCall(call, key);
    }

    Env env = Env.getCurrent();

    if (_quercusClass != null) {
      env.error(L.l("Call to undefined method {0}::{1}",
                    _quercusClass.getName(), key));
    }
    else {
      env.error(L.l("Call to undefined function {0}",
                    key));
    }

    throw new IllegalStateException(L.l("Call to undefined function {0}",
                                        key));
  }

  public V getRaw(StringValue key)
  {
    int hash = key.hashCodeCaseInsensitive();

    int bucket = (hash & 0x7fffffff) % _prime;

    for (Entry<V> entry = _entries[bucket];
         entry != null;
         entry = entry.getNext()) {
      StringValue entryKey = entry.getKey();

      if (key == entryKey || key.equalsIgnoreCase(entryKey))
        return entry.getValue();
    }

    return null;
  }

  public V get(StringValue key)
  {
    return get(key, key.hashCodeCaseInsensitive());
  }

  public Iterable<V> values()
  {
    return new ValueIterator(_head);
  }

  private bool match(char []a, char []b, int length)
  {
    if (a.length != length)
      return false;

    for (int i = length - 1; i >= 0; i--) {
      int chA = a[i];
      int chB = b[i];

      if (chA == chB) {
      }
      /*
      else if ((chA & ~0x20) != (chB & ~0x20))
        return false;
      */
      else {
        if ('A' <= chA && chA <= 'Z')
          chA += 'a' - 'A';

        if ('A' <= chB && chB <= 'Z')
          chB += 'a' - 'A';

        if (chA != chB)
          return false;
      }
    }

    return true;
  }

  private void resize()
  {
    Entry<V> []newEntries = new Entry[2 * _entries.length];
    int newPrime = Primes.getBiggestPrime(newEntries.length);

    for (int i = 0; i < _entries.length; i++) {
      Entry<V> entry = _entries[i];

      while (entry != null) {
        Entry<V> next = entry.getNext();

        int hash = entry._key.hashCodeCaseInsensitive();
        int bucket = (hash & 0x7fffffff) % newPrime;

        entry.setNext(newEntries[bucket]);
        newEntries[bucket] = entry;

        entry = next;
      }
    }

    _entries = newEntries;
    _prime = newPrime;
  }

 static class Entry<V> {
    private StringValue _key;
    private V _value;

    private Entry<V> _next;
    private Entry<V> _absoluteNext;

    Entry(StringValue key, V value)
    {
      _key = key;
      _value = value;
    }

    public StringValue getKey()
    {
      return _key;
    }

    public V getValue()
    {
      return _value;
    }

    public void setValue(V value)
    {
      _value = value;
    }

    public Entry<V> getNext()
    {
      return _next;
    }

    public void setNext(Entry<V> next)
    {
      _next = next;
    }

    public Entry<V> getAbsoluteNext()
    {
      return _absoluteNext;
    }

    public void setAbsoluteNext(Entry<V> next)
    {
      _absoluteNext = next;
    }
  }

 static class ValueIterator<V> : Iterable<V>, Iterator<V>
  {
    Entry<V> _next;

    public ValueIterator(Entry<V> next)
    {
      _next = next;
    }

    private void getNext()
    {
      Entry<V> current = _next;

      _next = current.getAbsoluteNext();
    }

    public bool hasNext()
    {
      return _next != null;
    }

    public V next()
    {
      V value = _next._value;

      getNext();

      return value;
    }

    public Iterator<V> iterator()
    {
      return this;
    }

    public void remove()
    {
      throw new UnsupportedOperationException();
    }
  }
}
}
