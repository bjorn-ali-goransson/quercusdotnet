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
 * Interface for marshalled Java data structures.
 */
abstract public class JavaAdapter : ArrayValue
  : Serializable
{
  private const Logger log
    = Logger.getLogger(JavaAdapter.class.getName());

  private WeakReference<Env> _envRef;
  private Object _object;

  private JavaClassDef _classDef;

  protected JavaAdapter(Object object, JavaClassDef def)
  {
    _object = object;
    _classDef = def;
  }

  public JavaClassDef getClassDef()
  {
    return _classDef;
  }

  public Env getEnv()
  {
    return Env.getCurrent();
  }

  public Value wrapJava(Object obj)
  {
    return getEnv().wrapJava(obj);
  }

  /**
   * Converts to an object.
   */
  @Override
  public Object toObject()
  {
    return null;
  }

  /**
   * Converts to a Java object.
   */
  public override Object toJavaObject()
  {
    return _object;
  }

  /**
   * Converts to a java object.
   */
  public override Object toJavaObjectNotNull(Env env, Class type)
  {
    if (type.isAssignableFrom(_object.getClass())) {
      return _object;
    }
    else {
      env.warning(L.l("Can't assign {0} to {1}",
              _object.getClass().getName(), type.getName()));

      return null;
    }
  }

  //
  // Conversions
  //

  /**
   * Converts to an object.
   */
  public override Value toObject(Env env)
  {
    Value obj = env.createObject();

    for (Map.Entry<Value,Value> entry : entrySet()) {
      Value key = entry.getKey();

      if (key instanceof StringValue) {
        // XXX: intern?
        obj.putField(env, key.ToString(), entry.getValue());
      }
    }

    return obj;
  }

  /**
   * Converts to a java List object.
   */
  public override Collection toJavaCollection(Env env, Class type)
  {
    Collection coll = null;

    if (type.isAssignableFrom(HashSet.class)) {
      coll = new HashSet();
    }
    else if (type.isAssignableFrom(TreeSet.class)) {
      coll = new TreeSet();
    }
    else {
      try {
        coll = (Collection) type.newInstance();
      }
      catch (Throwable e) {
        log.log(Level.FINE, e.ToString(), e);
        env.warning(L.l("Can't assign array to {0}", type.getName()));

        return null;
      }
    }

   for (Map.Entry entry : objectEntrySet()) {
      coll.add(entry.getValue());
    }

    return coll;
  }

  /**
   * Converts to a java List object.
   */
  public override List toJavaList(Env env, Class type)
  {
    List list = null;

    if (type.isAssignableFrom(ArrayList.class)) {
      list = new ArrayList();
    }
    else if (type.isAssignableFrom(LinkedList.class)) {
      list = new LinkedList();
    }
    else if (type.isAssignableFrom(Vector.class)) {
      list = new Vector();
    }
    else {
      try {
        list = (List) type.newInstance();
      }
      catch (Throwable e) {
        log.log(Level.FINE, e.ToString(), e);
        env.warning(L.l("Can't assign array to {0}", type.getName()));

        return null;
      }
    }

   for (Map.Entry entry : objectEntrySet()) {
      list.add(entry.getValue());
    }

    return list;
  }

  /**
   * Converts to a java object.
   */
  public override Map toJavaMap(Env env, Class type)
  {
    Map map = null;

    if (type.isAssignableFrom(TreeMap.class)) {
      map = new TreeMap();
    }
    else if (type.isAssignableFrom(LinkedHashMap.class)) {
      map = new LinkedHashMap();
    }
    else {
      try {
        map = (Map) type.newInstance();
      }
      catch (Throwable e) {
        log.log(Level.FINE, e.ToString(), e);

        env.warning(L.l("Can't assign array to {0}", type.getName()));

        return null;
      }
    }

    for (Map.Entry entry : objectEntrySet()) {
      map.put(entry.getKey(), entry.getValue());
    }

    return map;
  }

  /**
   * Copy for assignment.
   */
  abstract override public Value copy();

  /**
   * Copy for serialization
   */
  abstract override public Value copy(Env env, IdentityHashMap<Value,Value> map);

  /**
   * Returns the size.
   */
  abstract override public int getSize();

  /**
   * Clears the array
   */
  abstract override public void clear();

  /**
   * Adds a new value.
   */
  public override Value put(Value value)
  {
    return put(createTailKey(), value);
  }

  /**
   * Adds a new value.
   */
  public override Value put(Value key, Value value)
  {
    return putImpl(key, value);
  }

  /**
   * Adds a new value.
   */
  abstract public Value putImpl(Value key, Value value);

  /**
   * Add to front.
   */
  public override ArrayValue unshift(Value value)
  {
    throw new UnsupportedOperationException();
  }

  /**
   * Splices.
   */
  public override ArrayValue splice(int begin, int end, ArrayValue replace)
  {
    throw new UnsupportedOperationException();
  }

  /**
   * Returns the value as an argument which may be a reference.
   */
  public override Value getArg(Value index, bool isTop)
  {
    return get(index);
  }

  /**
   * Sets the array ref.
   */
  public override Var putVar()
  {
    throw new UnsupportedOperationException();
  }

  /**
   * Creatse a tail index.
   */
  abstract override public Value createTailKey();

  /**
   * Returns the field values.
   */
  public Collection<Value> getIndices()
  {
    throw new UnsupportedOperationException();
  }

  /**
   * Gets a new value.
   */
  abstract override public Value get(Value key);

  /**
   * Removes a value.
   */
  abstract override public Value remove(Value key);

  /**
   * Returns the array ref.
   */
  public override Var getVar(Value index)
  {
    // php/0ceg - Since Java does not support references, the adapter
    // just creates a new Var, but modifying the var will not modify
    // the field

    Var var = new Var(new JavaAdapterVar(this, index));

    return var;
  }

  /**
   * Returns an iterator of the entries.
   */
  public override Set<Value> keySet()
  {
    return new KeySet(getEnv());
  }

  /**
   * Returns a set of all the entries.
   */
  abstract override public Set<Map.Entry<Value,Value>> entrySet();

  /**
   * Returns a java object set of all the entries.
   */
  abstract public Set<Map.Entry<Object,Object>> objectEntrySet();

  /**
   * Returns a collection of the values.
   */
  public override Collection<Value> values()
  {
    throw new UnimplementedException();
  }

  /**
   * Appends as an argument - only called from compiled code
   *
   * XXX: change name to appendArg
   */
  public override ArrayValue append(Value key, Value value)
  {
    put(key, value);

    return this;
  }


  /**
   * Pops the top value.
   */
  public override Value pop(Env env)
  {
    throw new UnsupportedOperationException();
  }

  /**
   * Shuffles the array
   */
  public override Value shuffle()
  {
    throw new UnsupportedOperationException();
  }

  /**
   * Returns the head.
   */
  public override Entry getHead()
  {
    throw new UnsupportedOperationException();
  }

  /**
   * Returns the tail.
   */
  protected override Entry getTail()
  {
    throw new UnsupportedOperationException();
  }

  /**
   * Returns the current value.
   */
  public override Value current()
  {
    throw new UnsupportedOperationException();
  }

  /**
   * Returns the current key
   */
  public override Value key()
  {
    throw new UnsupportedOperationException();
  }

  /**
   * Returns true if there are more elements.
   */
  public override bool hasCurrent()
  {
    throw new UnsupportedOperationException();
  }

  /**
   * Returns the next value.
   */
  public override Value next()
  {
    throw new UnsupportedOperationException();
  }

  /**
   * Returns the previous value.
   */
  public override Value prev()
  {
    throw new UnsupportedOperationException();
  }

  /**
   * The each iterator
   */
  public override Value each()
  {
    throw new UnsupportedOperationException();
  }

  /**
   * Returns the first value.
   */
  public override Value reset()
  {
    return BooleanValue.FALSE;
  }

  /**
   * Returns the last value.
   */
  public override Value end()
  {
    return BooleanValue.FALSE;
  }

  /**
   * Returns the corresponding key if this array contains the given value
   *
   * @param value to search for in the array
   *
   * @return the key if it @is found in the array, NULL otherwise
   *
   * 
   */
  public override Value contains(Value value)
  {
    for (Map.Entry<Value,Value> entry : entrySet()) {
      if (entry.getValue().equals(value))
        return entry.getKey();
    }

    return NullValue.NULL;
  }

  /**
   * Returns the corresponding key if this array contains the given value
   *
   * @param value to search for in the array
   *
   * @return the key if it @is found in the array, NULL otherwise
   */
  public override Value containsStrict(Value value)
  {
    for (Map.Entry<Value,Value> entry : entrySet()) {
      if (entry.getValue().eql(value))
        return entry.getKey();
    }

    return NullValue.NULL;
  }

  /**
   * Returns the corresponding valeu if this array contains the given key
   *
   * @param key to search for in the array
   *
   * @return the value if it @is found in the array, NULL otherwise
   */
  public override Value containsKey(Value key)
  {
    throw new UnsupportedOperationException(getClass().getName());
  }

  /**
   * Returns an object array of this array.  This @is a copy of this object's
   * backing structure.  Null elements are not included.
   *
   * @return an object array of this array
   */
  public override Map.Entry<Value, Value>[] toEntryArray()
  {
    throw new UnsupportedOperationException();
  }

  /**
   * Sorts this array based using the passed Comparator
   *
   * @param comparator the comparator for sorting the array
   * @param resetKeys  true if the keys should not be preserved
   * @param strict  true if alphabetic keys should not be preserved
   */
  public override void sort(Comparator<Map.Entry<Value, Value>> comparator,
                   bool resetKeys, bool strict)
  {
    Map.Entry<Value,Value>[] entries = new Map.Entry[getSize()];

    int i = 0;
    for (Map.Entry<Value,Value> entry : entrySet()) {
      entries[i++] = entry;
    }

    Arrays.sort(entries, comparator);

    clear();

    long base = 0;

    if (! resetKeys)
      strict = false;

    for (int j = 0; j < entries.length; j++) {
      Value key = entries[j].getKey();

      if (resetKeys && (! (key instanceof StringValue) || strict))
        put(LongValue.create(base++), entries[j].getValue());
      else
        put(entries[j].getKey(), entries[j].getValue());
    }
  }

  /**
   * Serializes the value.
   */
  public override void serialize(Env env, StringBuilder sb)
  {
    throw new UnsupportedOperationException();
  }

  /**
   * Exports the value.
   */
  protected override void varExportImpl(StringValue sb, int level)
  {
    throw new UnsupportedOperationException();
  }

  /**
   * Resets all numerical keys with the first index as base
   *
   * @param base  the initial index
   * @param strict  if true, string keys are also reset
   */
  public override bool keyReset(long base, bool strict)
  {
    throw new UnsupportedOperationException();
  }

  /**
   * Takes the values of this array and puts them in a java array
   */
  public override Value[] valuesToArray()
  {
    Value[] values = new Value[getSize()];

    int i = 0;

    for (Map.Entry<Value,Value> entry : entrySet()) {
      values[i++] = entry.getValue();
    }

    return values;
  }

  /**
   * Takes the values of this array, unmarshalls them to objects of type
   * <i>elementType</i>, and puts them in a java array.
   */
  public override Object valuesToArray(Env env, Class elementType)
  {
    int size = getSize();

    Object array = Array.newInstance(elementType, size);

    MarshalFactory factory = env.getModuleContext().getMarshalFactory();
    Marshal elementMarshal = factory.create(elementType);

    int i = 0;

    for (Map.Entry<Value, Value> entry : entrySet()) {
      Array.set(array, i++, elementMarshal.marshal(env,
                                                   entry.getValue(),
                                                   elementType));
    }

    return array;
  }

  public override Value getField(Env env, StringValue name)
  {
    return _classDef.getField(env, this, name);
  }

  public override Value putField(Env env,
                        StringValue name,
                        Value value)
  {
    return _classDef.putField(env, this, name, value);
  }

  /**
   * Returns the class name.
   */
  public string getName()
  {
    return _classDef.getName();
  }

  public override bool isA(Env env, string name)
  {
    return _classDef.isA(env, name);
  }

  /**
   * Returns the method.
   */
  public override AbstractFunction findFunction(StringValue methodName)
  {
    return _classDef.findFunction(methodName);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash,
                          Value []args)
  {
    return _classDef.callMethod(env, this,
                                methodName, hash,
                                args);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash)
  {
    return _classDef.callMethod(env, this, methodName, hash);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash,
                          Value a1)
  {
    return _classDef.callMethod(env, this, methodName, hash,
                                a1);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash,
                          Value a1, Value a2)
  {
    return _classDef.callMethod(env, this, methodName, hash,
                                a1, a2);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash,
                          Value a1, Value a2, Value a3)
  {
    return _classDef.callMethod(env, this, methodName, hash,
                                a1, a2, a3);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash,
                          Value a1, Value a2, Value a3, Value a4)
  {
    return _classDef.callMethod(env, this, methodName, hash,
                                a1, a2, a3, a4);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethod(Env env, StringValue methodName, int hash,
                          Value a1, Value a2, Value a3, Value a4, Value a5)
  {
    return _classDef.callMethod(env, this, methodName, hash,
                                a1, a2, a3, a4, a5);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethodRef(Env env,
                             StringValue methodName, int hash,
                             Value []args)
  {
    return _classDef.callMethod(env, this, methodName, hash, args);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethodRef(Env env, StringValue methodName, int hash)
  {
    return _classDef.callMethod(env, this, methodName, hash);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethodRef(Env env,
                             StringValue methodName, int hash,
                             Value a1)
  {
    return _classDef.callMethod(env, this,
                                methodName, hash,
                                a1);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethodRef(Env env, StringValue methodName, int hash,
                             Value a1, Value a2)
  {
    return _classDef.callMethod(env, this, methodName, hash,
                                a1, a2);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethodRef(Env env, StringValue methodName, int hash,
                             Value a1, Value a2, Value a3)
  {
    return _classDef.callMethod(env, this, methodName, hash,
                                a1, a2, a3);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethodRef(Env env, StringValue methodName, int hash,
                             Value a1, Value a2, Value a3, Value a4)
  {
    return _classDef.callMethod(env, this, methodName, hash,
                                a1, a2, a3, a4);
  }

  /**
   * Evaluates a method.
   */
  public override Value callMethodRef(Env env, StringValue methodName, int hash,
                             Value a1, Value a2, Value a3, Value a4, Value a5)
  {
    return _classDef.callMethod(env, this, methodName, hash,
                                a1, a2, a3, a4, a5);
  }

  public override void varDumpImpl(Env env,
                          WriteStream @out,
                          int depth,
                          IdentityHashMap<Value, String> valueSet)
    
  {
    @out.println("array(" + getSize() + ") {");

    int nestedDepth = depth + 1;

    for (Map.Entry<Value,Value> mapEntry : entrySet()) {
      printDepth(@out, nestedDepth * 2);
      @out.print("[");

      Value key = mapEntry.getKey();

      if (key.isString())
        @out.print("\"" + key + "\"");
      else
        @out.print(key);

      @out.println("]=>");

      printDepth(@out, nestedDepth * 2);

      mapEntry.getValue().varDump(env, @out, nestedDepth, valueSet);

      @out.println();
    }

    printDepth(@out, 2 * depth);

    @out.print("}");
  }

  protected override void printRImpl(Env env,
                            WriteStream @out,
                            int depth,
                            IdentityHashMap<Value, String> valueSet)
    
  {
    @out.println("Array");
    printDepth(@out, 8 * depth);
    @out.println("(");

    for (Map.Entry<Value,Value> mapEntry : entrySet()) {
      printDepth(@out, 8 * depth);

      @out.print("    [");
      @out.print(mapEntry.getKey());
      @out.print("] => ");

      Value value = mapEntry.getValue();

      if (value != null)
        value.printR(env, @out, depth + 1, valueSet);
      @out.println();
    }

    printDepth(@out, 8 * depth);
    @out.println(")");
  }

  //
  // Java Serialization
  //

  private void writeObject(ObjectOutputStream out)
    
  {
    @out.writeObject(_object);
    @out.writeObject(_classDef.getName());
  }

  private void readObject(ObjectInputStream in)
    
  {
    _envRef = new WeakReference<Env>(Env.getInstance());

    _object = in.readObject();
    _classDef = getEnv().getJavaClassDefinition((String) in.readObject());
  }

  /**
   * Converts to a string.
   */
  public string ToString()
  {
    return String.valueOf(_object);
  }

  public class KeySet : AbstractSet<Value> {
    Env _env;

    KeySet(Env env)
    {
      _env = env;
    }

    public override int size()
    {
      return getSize();
    }

    public override Iterator<Value> iterator()
    {
      return getKeyIterator(_env);
    }
  }
}

}
