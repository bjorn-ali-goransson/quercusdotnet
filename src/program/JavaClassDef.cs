using System;
namespace QuercusDotNet.Program{
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
 * Represents an introspected Java class.
 */
public class JavaClassDef : ClassDef : InstanceInitializer {
  private const Logger log
    = Logger.getLogger(JavaClassDef.class.getName());
  private readonly L10N L = new L10N(JavaClassDef.class);

  private ModuleContext _moduleContext;

  private string _name;
  private Class<?> _type;

  private QuercusClass _quercusClass;

  private HashSet<String> _instanceOfSet;
  private HashSet<String> _instanceOfSetLowerCase;

  private bool _isAbstract;
  private bool _isInterface;
  private bool _isDelegate;
  private bool _isPhpClass;

  private string _resourceType;

  private JavaClassDef _componentDef;

  protected volatile bool _isInit;

  private HashMap<String, Value> _constMap
    = new HashMap<String, Value>();

  private HashMap<String, Object> _constJavaMap
    = new HashMap<String, Object>();

  private MethodMap<AbstractJavaMethod> _functionMap
    = new MethodMap<AbstractJavaMethod>(null, this);

  private HashMap<String, AbstractJavaMethod> _getMap
    = new HashMap<String, AbstractJavaMethod>();

  private HashMap<String, AbstractJavaMethod> _setMap
    = new HashMap<String, AbstractJavaMethod>();

  // _fieldMap stores all public non-static fields
  // used by getField and setField
  private HashMap<String, FieldMarshalPair> _fieldMap
    = new HashMap<String, FieldMarshalPair> ();

  private AbstractJavaMethod _cons;
  private AbstractJavaMethod __construct;
  private AbstractJavaMethod __destruct;

  private JavaMethod __fieldGet;
  private JavaMethod __fieldSet;

  private FunctionArrayDelegate _funArrayDelegate;
  private ArrayDelegate _arrayDelegate;

  private JavaMethod __call;
  private JavaMethod __callStatic;

  private JavaMethod __ToString;

  private Method _printRImpl;
  private Method _varDumpImpl;
  private Method _jsonEncode;
  private Method _entrySet;
  
  private Method _isset;

  private TraversableDelegate _traversableDelegate;
  private CountDelegate _countDelegate;

  private AbstractFunction _serializeFun;
  private AbstractFunction _unserializeFun;

  private Method _iteratorMethod;

  private Marshal _marshal;

  private string _extension;

  public JavaClassDef(ModuleContext moduleContext, string name, Class<?> type)
  {
    super(null, name, null,
          ClassDef.NULL_STRING_ARRAY, ClassDef.NULL_STRING_ARRAY);

    _moduleContext = moduleContext;
    _name = name;
    _type = type;

    _isAbstract = Modifier.isAbstract(type.getModifiers());
    _isInterface = type.isInterface();
    _isDelegate = type.isAnnotationPresent(ClassImplementation.class);

    if (type.isArray() && ! isArray()) {
      throw new IllegalStateException(L.l("'{0}' needs to be called with JavaArrayClassDef", type));
    }
  }

  public JavaClassDef(ModuleContext moduleContext,
                      string name,
                      Class<?> type,
                      string extension)
  {
    this(moduleContext, name, type);

    _extension = extension;

    moduleContext.addExtensionClass(extension, name);
  }

  private void fillInstanceOfSet(Class<?> type, bool isTop)
  {
    if (type == null)
      return;

    if (isTop && _isDelegate) {
      _instanceOfSet.add(_name);
      _instanceOfSetLowerCase.add(_name.toLowerCase(Locale.ENGLISH));
    }
    else {
      string name = type.getSimpleName();

      _instanceOfSet.add(name);
      _instanceOfSetLowerCase.add(name.toLowerCase(Locale.ENGLISH));
    }

    fillInstanceOfSet(type.getSuperclass(), false);

    Class<?> []ifaceList = type.getInterfaces();
    if (ifaceList != null) {
      for (Class<?> iface : ifaceList)
        fillInstanceOfSet(iface, false);
    }
  }

  public static JavaClassDef create(ModuleContext moduleContext,
                                    string name, Class<?> type)
  {
    if (Double.class.isAssignableFrom(type)
        || Float.class.isAssignableFrom(type))
      return new DoubleClassDef(moduleContext);
    else if (Long.class.isAssignableFrom(type)
             || Integer.class.isAssignableFrom(type)
             || Short.class.isAssignableFrom(type)
             || Byte.class.isAssignableFrom(type))
      return new LongClassDef(moduleContext);
    else if (BigDecimal.class.isAssignableFrom(type))
      return new BigDecimalClassDef(moduleContext);
    else if (BigInteger.class.isAssignableFrom(type))
      return new BigIntegerClassDef(moduleContext);
    else if (String.class.isAssignableFrom(type)
             || Character.class.isAssignableFrom(type))
      return new StringClassDef(moduleContext);
    else if (Boolean.class.isAssignableFrom(type))
      return new BooleanClassDef(moduleContext);
    else if (Calendar.class.isAssignableFrom(type))
      return new CalendarClassDef(moduleContext);
    else if (Date.class.isAssignableFrom(type))
      return new DateClassDef(moduleContext, type);
    else if (URL.class.isAssignableFrom(type))
      return new URLClassDef(moduleContext);
    else if (Map.class.isAssignableFrom(type))
      return new JavaMapClassDef(moduleContext, name, type);
    else if (List.class.isAssignableFrom(type))
      return new JavaListClassDef(moduleContext, name, type);
    else if (Collection.class.isAssignableFrom(type)
             && ! Queue.class.isAssignableFrom(type))
      return new JavaCollectionClassDef(moduleContext, name, type);
    else
      return null;
  }

  /**
   * Returns the class name.
   */
  public override string getName()
  {
    return _name;
  }

  /**
   * Returns the class name.
   */
  public string getSimpleName()
  {
    return _type.getSimpleName();
  }

  public Class<?> getType()
  {
    return _type;
  }

  /**
   * Returns the type of this resource.
   */
  public string getResourceType()
  {
    return _resourceType;
  }

  protected ModuleContext getModuleContext()
  {
    return _moduleContext;
  }

  /**
   * Returns the name of the extension that this class @is part of.
   */
  public override string getExtension()
  {
    return _extension;
  }

  public override bool isA(Env env, string name)
  {
    if (_instanceOfSet == null) {
      _instanceOfSet = new HashSet<String>();
      _instanceOfSetLowerCase = new HashSet<String>();

      fillInstanceOfSet(_type, true);
    }

    return (_instanceOfSet.contains(name)
            || _instanceOfSetLowerCase.contains(name.toLowerCase(Locale.ENGLISH)));
  }

  /**
   * Adds the interfaces to the set
   */
  public override void addInterfaces(HashSet<String> interfaceSet)
  {
    addInterfaces(interfaceSet, _type, true);
  }

  protected void addInterfaces(HashSet<String> interfaceSet,
                               Class<?> type,
                               bool isTop)
  {
    if (type == null)
      return;

    interfaceSet.add(_name.toLowerCase(Locale.ENGLISH));
    interfaceSet.add(type.getSimpleName().toLowerCase(Locale.ENGLISH));

    if (type.getInterfaces() != null) {
      for (Class<?> iface : type.getInterfaces()) {
        addInterfaces(interfaceSet, iface, false);
      }
    }

    // php/1z21
    addInterfaces(interfaceSet, type.getSuperclass(), false);
  }

  private bool hasInterface(String name, Class<?> type)
  {
    Class<?>[] interfaces = type.getInterfaces();

    if (interfaces != null) {
      for (Class<?> intfc : interfaces) {
        if (intfc.getSimpleName().equalsIgnoreCase(name))
          return true;

        if (hasInterface(name, intfc))
          return true;
      }
    }

    return false;
  }

  public override bool isAbstract()
  {
    return _isAbstract;
  }

  public bool isArray()
  {
    return false;
  }

  public override bool isInterface()
  {
    return _isInterface;
  }

  public bool isDelegate()
  {
    return _isDelegate;
  }

  public void setPhpClass(bool isPhpClass)
  {
    _isPhpClass = isPhpClass;
  }

  public bool isPhpClass()
  {
    return _isPhpClass;
  }

  public JavaClassDef getComponentDef()
  {
    if (_componentDef == null) {
      Class<?> compType = getType().getComponentType();
      _componentDef = _moduleContext.getJavaClassDefinition(compType.getName());
    }

    return _componentDef;
  }

  public Value wrap(Env env, Object obj)
  {
    if (! _isInit)
      init();

    if (_resourceType != null)
      return new JavaResourceValue(env, obj, this);
    else
      return new JavaValue(env, obj, this);
  }

  private int cmpObject(Object lValue, Object rValue)
  {
    if (lValue == rValue)
      return 0;

    if (lValue == null)
      return -1;

    if (rValue == null)
      return 1;

    if (lValue instanceof Comparable) {
      if (!(rValue instanceof Comparable))
        return -1;

      return ((Comparable) lValue).compareTo(rValue);
    }
    else if (rValue instanceof Comparable) {
      return 1;
    }

    if (lValue.equals(rValue))
      return 0;

    string lName = lValue.getClass().getName();
    string rName = rValue.getClass().getName();

    return lName.compareTo(rName);
  }

  public int cmpObject(Object lValue, Object rValue, JavaClassDef rClassDef)
  {
    int cmp = cmpObject(lValue, rValue);

    if (cmp != 0)
        return cmp;

    // attributes
    // XX: not sure how to do this, to imitate PHP objects,
    // should getters be involved as well?

    for (
      Map.Entry<String, FieldMarshalPair> lEntry : _fieldMap.entrySet()) {
      string lFieldName = lEntry.getKey();
      FieldMarshalPair rFieldPair = rClassDef._fieldMap.get(lFieldName);

      if (rFieldPair == null)
        return 1;

      FieldMarshalPair lFieldPair = lEntry.getValue();

      try {
        Object lResult = lFieldPair._field.get(lValue);
        Object rResult = rFieldPair._field.get(lValue);

        int resultCmp = cmpObject(lResult, rResult);

        if (resultCmp != 0)
          return resultCmp;
      }
      catch (IllegalAccessException e) {
        log.log(Level.FINE,  e.getMessage(), e);
        return 0;
      }
    }

    return 0;
  }

  /**
   * Returns the field getter.
   *
   * @param name
   * @return Value attained through invoking getter
   */
  public Value getField(Env env, Value qThis, StringValue nameV)
  {
    string name = nameV.ToString();

    AbstractJavaMethod get = _getMap.get(name);

    if (get != null) {
      try {
        return get.callMethod(env, getQuercusClass(), qThis);
      } catch (Exception e) {
        log.log(Level.FINE, e.getMessage(), e);

        return null;
      }
    }

    FieldMarshalPair fieldPair = _fieldMap.get(name);
    if (fieldPair != null) {
      try {
        Object result = fieldPair._field.get(qThis.toJavaObject());
        return fieldPair._marshal.unmarshal(env, result);
      } catch (Exception e) {
        log.log(Level.FINE,  e.getMessage(), e);

        return null;
      }
    }

    AbstractFunction phpGet = qThis.getQuercusClass().getFieldGet();

    if (phpGet != null) {
      return phpGet.callMethod(env, getQuercusClass(), qThis, nameV);
    }

    if (__fieldGet != null) {
      try {
        return __fieldGet.callMethod(env, getQuercusClass(), qThis, nameV);
      } catch (Exception e) {
        log.log(Level.FINE,  e.getMessage(), e);

        return null;
      }
    }

    return null;
  }

  public Value putField(Env env,
                        Value qThis,
                        StringValue nameV,
                        Value value)
  {
    string name = nameV.ToString();

    AbstractJavaMethod setter = _setMap.get(name);
    if (setter != null) {
      try {
        return setter.callMethod(env, getQuercusClass(), qThis, value);
      } catch (Exception e) {
        log.log(Level.FINE, e.getMessage(), e);

        return NullValue.NULL;
      }
    }

    FieldMarshalPair fieldPair = _fieldMap.get(name);

    if (fieldPair != null) {
      try {
        Class<?> type = fieldPair._field.getType();
        Object marshaledValue = fieldPair._marshal.marshal(env, value, type);
        fieldPair._field.set(qThis.toJavaObject(), marshaledValue);

        return value;

      } catch (Exception e) {
        log.log(Level.FINE, e.getMessage(), e);
        return NullValue.NULL;
      }
    }

    if (! qThis.isFieldInit()) {
      AbstractFunction phpSet = qThis.getQuercusClass().getFieldSet();

      if (phpSet != null) {
        qThis.setFieldInit(true);

        try {
          return phpSet.callMethod(env, getQuercusClass(), qThis, nameV, value);

        } finally {
          qThis.setFieldInit(false);
        }
      }
    }


    if (__fieldSet != null) {
      try {
        return __fieldSet.callMethod(env,
                                     getQuercusClass(),
                                     qThis,
                                     nameV,
                                     value);
      } catch (Exception e) {
        log.log(Level.FINE, e.getMessage(), e);

        return NullValue.NULL;
      }
    }

    return null;
  }

  /**
   * Returns the marshal instance.
   */
  public Marshal getMarshal()
  {
    return _marshal;
  }

  /**
   * Eval new
   */
  public override Value callNew(Env env, Value []args)
  {    
    if (_cons != null) {
      if (__construct != null) {
        Value value = _cons.call(env, Value.NULL_ARGS);

        __construct.callMethod(env, __construct.getQuercusClass(), value, args);

        return value;
      }
      else {
        return _cons.call(env, args);
      }
    }
    else if (__construct != null) {
      return __construct.call(env, args);
    }
    else {
      return NullValue.NULL;
    }
  }

  public override AbstractFunction getCall()
  {
    return __call;
  }

  public override AbstractFunction getCallStatic()
  {
    return __callStatic;
  }

  public override AbstractFunction getSerialize()
  {
    return _serializeFun;
  }

  public override AbstractFunction getUnserialize()
  {
    return _unserializeFun;
  }

  /**
   * Eval a method
   */
  public AbstractFunction findFunction(StringValue methodName)
  {
    return _functionMap.getRaw(methodName);
  }

  /**
   * Eval a method
   */
  public Value callMethod(Env env, Value qThis,
                          StringValue methodName, int hash,
                          Value []args)
  {
    AbstractFunction fun = _functionMap.get(methodName, hash);

    return fun.callMethod(env, getQuercusClass(), qThis, args);
  }

  /**
   * Eval a method
   */
  public Value callMethod(Env env, Value qThis,
                          StringValue methodName, int hash)
  {
    AbstractFunction fun = _functionMap.get(methodName, hash);

    return fun.callMethod(env, getQuercusClass(), qThis);
  }

  /**
   * Eval a method
   */
  public Value callMethod(Env env, Value qThis,
                          StringValue methodName, int hash,
                          Value a1)
  {
    AbstractFunction fun = _functionMap.get(methodName, hash);

    return fun.callMethod(env, getQuercusClass(), qThis, a1);
  }

  /**
   * Eval a method
   */
  public Value callMethod(Env env, Value qThis,
                          StringValue methodName, int hash,
                          Value a1, Value a2)
  {
    AbstractFunction fun = _functionMap.get(methodName, hash);

    return fun.callMethod(env, getQuercusClass(), qThis, a1, a2);
  }

  /**
   * Eval a method
   */
  public Value callMethod(Env env, Value qThis,
                          StringValue methodName, int hash,
                          Value a1, Value a2, Value a3)
  {
    AbstractFunction fun = _functionMap.get(methodName, hash);

    return fun.callMethod(env, getQuercusClass(), qThis, a1, a2, a3);
  }

  /**
   * Eval a method
   */
  public Value callMethod(Env env, Value qThis,
                          StringValue methodName, int hash,
                          Value a1, Value a2, Value a3, Value a4)
  {
    AbstractFunction fun = _functionMap.get(methodName, hash);

    return fun.callMethod(env, getQuercusClass(), qThis, a1, a2, a3, a4);
  }

  /**
   * Eval a method
   */
  public Value callMethod(Env env, Value qThis,
                          StringValue methodName, int hash,
                          Value a1, Value a2, Value a3, Value a4, Value a5)
  {
    AbstractFunction fun = _functionMap.get(methodName, hash);

    return fun.callMethod(env, getQuercusClass(), qThis, a1, a2, a3, a4, a5);
  }

  public Set<? : Map.Entry<Value,Value>> entrySet(Object obj)
  {
    try {
      if (_entrySet == null) {
        return null;
      }

      return (Set) _entrySet.invoke(obj);
    } catch (Exception e) {
      throw new QuercusException(e);
    }
  }

  /**
   * Initialize the quercus class methods.
   */
  public override void initClassMethods(QuercusClass cl, string bindingClassName)
  {
    init();

    cl.addInitializer(this);

    if (_cons != null) {
      cl.setConstructor(_cons);
      cl.addMethod(_moduleContext.createString("__construct"), _cons);
    }

    if (__construct != null) {
      cl.setConstructor(__construct);
      cl.addMethod(_moduleContext.createString("__construct"), __construct);
    }

    if (__destruct != null) {
      cl.setDestructor(__destruct);
      cl.addMethod(_moduleContext.createString("__destruct"), __destruct);
    }

    for (AbstractJavaMethod value : _functionMap.values()) {
      cl.addMethod(_moduleContext.createString(value.getName()), value);
    }

    if (__fieldGet != null)
      cl.setFieldGet(__fieldGet);

    if (__fieldSet != null)
      cl.setFieldSet(__fieldSet);

    if (__call != null)
      cl.setCall(__call);

    if (__callStatic != null) {
      cl.setCallStatic(__callStatic);
    }

    if (__ToString != null) {
      cl.addMethod(_moduleContext.createString("__ToString"), __ToString);
    }

    if (_arrayDelegate != null)
      cl.setArrayDelegate(_arrayDelegate);
    else if (_funArrayDelegate != null)
      cl.setArrayDelegate(_funArrayDelegate);

    if (_serializeFun != null) {
      cl.setSerialize(_serializeFun, _unserializeFun);
    }

    if (_traversableDelegate != null)
      cl.setTraversableDelegate(_traversableDelegate);
    else if (cl.getTraversableDelegate() == null
             && _iteratorMethod != null) {
      // adds support for Java classes implementing iterator()
      // php/
      cl.setTraversableDelegate(new JavaTraversableDelegate(_iteratorMethod));
    }

    if (_countDelegate != null) {
      cl.setCountDelegate(_countDelegate);
    }
  }

  /**
   * Initialize the quercus class fields.
   */
  public override void initClassFields(QuercusClass cl, string bindingClassName)
  {
    for (Map.Entry<String,Value> entry : _constMap.entrySet()) {
      cl.addConstant(_moduleContext.createString(entry.getKey()),
                     new LiteralExpr(entry.getValue()));
    }

    for (Map.Entry<String,Object> entry : _constJavaMap.entrySet()) {
      cl.addJavaConstant(_moduleContext.createString(entry.getKey()),
                         entry.getValue());
    }
  }

  /**
   * Finds the matching constant
   */
  public Value findConstant(Env env, string name)
  {
    return _constMap.get(name);
  }

  /**
   * Creates a new instance.
   */
  public override void initInstance(Env env, Value value, bool isInitFieldValues)
  {
    if (value instanceof ObjectValue) {
      ObjectValue object = (ObjectValue) value;

      if (__destruct != null) {
        env.addObjectCleanup(object);
      }
    }
  }

  /**
   * Returns the quercus class
   */
  public QuercusClass getQuercusClass()
  {
    if (_quercusClass == null) {
      init();

      _quercusClass = new QuercusClass(_moduleContext, this, null);
    }

    return _quercusClass;
  }

  /**
   * Returns the constructor
   */
  public override AbstractFunction findConstructor()
  {
    return null;
  }

  public override void init()
  {
    if (_isInit)
      return;

    synchronized (this) {
      if (_isInit)
        return;

      super.init();

      try {
        initInterfaceList(_type);
        introspect();
      }
      finally {
        _isInit = true;
      }
    }
  }

  private void initInterfaceList(Class<?> type)
  {
    Class<?>[] ifaces = type.getInterfaces();

    if (ifaces == null)
      return;

    for (Class<?> iface : ifaces) {
      JavaClassDef javaClassDef = _moduleContext.getJavaClassDefinition(iface);

      if (javaClassDef != null)
        addInterface(javaClassDef.getName());

      // recurse for parent interfaces
      initInterfaceList(iface);
    }
  }

  /**
   * Introspects the Java class.
   */
  private void introspect()
  {
    introspectConstants(_type);
    introspectEnums(_type);

    introspectMethods(_moduleContext, _type);
    introspectFields(_moduleContext, _type);

    _marshal = new JavaMarshal(this, false);

    AbstractJavaMethod consMethod = getConsMethod();

    if (consMethod != null) {
      if (consMethod.isStatic())
        _cons = consMethod;
      else
        __construct = consMethod;
    }

    if (Serializable.class.equals(_type)) {
      _serializeFun = findFunction(Serializable.SERIALIZE);
      _unserializeFun = findFunction(Serializable.UNSERIALIZE);
    }

    //Method consMethod = getConsMethod(_type);

    /*
    if (consMethod != null) {
      if (Modifier.isStatic(consMethod.getModifiers()))
        _cons = new JavaMethod(_moduleContext, consMethod);
      else
        __construct = new JavaMethod(_moduleContext, consMethod);
    }
    */

    if (_cons == null) {
      Constructor<?> []cons = _type.getConstructors();

      if (cons.length > 0) {
        int i;
        for (i = 0; i < cons.length; i++) {
          if (cons[i].isAnnotationPresent(Construct.class))
            break;
        }

        if (i < cons.length) {
          _cons = new JavaConstructor(_moduleContext, this, cons[i]);
        }
        else {
          _cons = new JavaConstructor(_moduleContext, this, cons[0]);
          for (i = 1; i < cons.length; i++) {
            _cons = _cons.overload(new JavaConstructor(_moduleContext,
                                                       this,
                                                       cons[i]));
          }
        }

      } else
        _cons = null;
    }

    if (_cons != null)
      _cons.setConstructor(true);

    if (__construct != null)
      __construct.setConstructor(true);

    introspectAnnotations(_type);
  }

  private void introspectAnnotations(Class<?> type)
  {
    try {
      if (type == null)
        return;

      // interfaces
      for (Class<?> iface : type.getInterfaces())
        introspectAnnotations(iface);

      // super-class
      introspectAnnotations(type.getSuperclass());

      // this
      for (Annotation annotation : type.getAnnotations()) {
        if (annotation.annotationType() == Delegates.class) {
          Class<?>[] delegateClasses = ((Delegates) annotation).value();

          for (Class<?> cl : delegateClasses) {
            bool isDelegate = addDelegate(cl);

            if (! isDelegate)
              throw new IllegalArgumentException(
                L.l("unknown @Delegate class '{0}'", cl));
          }
        }
        else if (annotation.annotationType() == ResourceType.class) {
          _resourceType = ((ResourceType) annotation).value();
        }
      }
    } catch (RuntimeException e) {
      throw e;
    } catch (InstantiationException e) {
      throw new QuercusModuleException(e.getCause());
    } catch (Exception e) {
      throw new QuercusModuleException(e);
    }
  }

  private bool addDelegate(Class<?> cl)
    
  {
    bool isDelegate = false;

    if (TraversableDelegate.class.isAssignableFrom(cl)) {
      _traversableDelegate = (TraversableDelegate) cl.newInstance();
      isDelegate = true;
    }

    if (ArrayDelegate.class.isAssignableFrom(cl)) {
      _arrayDelegate = (ArrayDelegate) cl.newInstance();
      isDelegate = true;
    }

    if (CountDelegate.class.isAssignableFrom(cl)) {
      _countDelegate = (CountDelegate) cl.newInstance();
      isDelegate = true;
    }

    return isDelegate;
  }

  private <T> bool addDelegate(Class<T> cl,
                                  ArrayList<T> delegates,
                                  Class<? : Object> delegateClass)
  {
    if (!cl.isAssignableFrom(delegateClass))
      return false;

    for (T delegate : delegates) {
      if (delegate.getClass() == delegateClass) {
        return true;
      }
    }

    try {
      delegates.add((T) delegateClass.newInstance());
    }
    catch (InstantiationException e) {
      throw new QuercusModuleException(e);
    }
    catch (IllegalAccessException e) {
      throw new QuercusModuleException(e);
    }

    return true;
  }

  /*
  private Method getConsMethod(Class type)
  {
    Method []methods = type.getMethods();

    for (int i = 0; i < methods.length; i++) {
      Method method = methods[i];

      if (! method.getName().equals("__construct"))
        continue;
      if (! Modifier.isPublic(method.getModifiers()))
        continue;

      return method;
    }

    return null;
  }
  */

  private AbstractJavaMethod getConsMethod()
  {
    for (AbstractJavaMethod method : _functionMap.values()) {
      if (method.getName().equals("__construct"))
        return method;
    }

    return null;
  }

  /**
   * Introspects the Java class.
   */
  private void introspectFields(ModuleContext moduleContext, Class<?> type)
  {
    if (type == null)
      return;

    if (! Modifier.isPublic(type.getModifiers()))
      return;

    // Introspect getXXX and setXXX
    // also register whether __get, __getField, __set, __setField exists
    Method[] methods = type.getMethods();

    for (Method method : methods) {
      if (Modifier.isStatic(method.getModifiers()))
        continue;

      if (method.isAnnotationPresent(Hide.class))
        continue;

      string methodName = method.getName();
      int length = methodName.length();

      if (length > 3) {
        if (methodName.startsWith("get")) {
          string quercusName
            = javaToQuercusConvert(methodName.substring(3, length));

          AbstractJavaMethod existingGetter = _getMap.get(quercusName);
          AbstractJavaMethod newGetter
            = new JavaMethod(moduleContext, this, method);

          if (existingGetter != null) {
            newGetter = existingGetter.overload(newGetter);
          }

          _getMap.put(quercusName, newGetter);
        }
        else if (methodName.startsWith("is")) {
          string quercusName
            = javaToQuercusConvert(methodName.substring(2, length));

          AbstractJavaMethod existingGetter = _getMap.get(quercusName);
          AbstractJavaMethod newGetter
            = new JavaMethod(moduleContext, this, method);

          if (existingGetter != null) {
            newGetter = existingGetter.overload(newGetter);
          }

          _getMap.put(quercusName, newGetter);
        }
        else if (methodName.startsWith("set")) {
          string quercusName
            = javaToQuercusConvert(methodName.substring(3, length));

          AbstractJavaMethod existingSetter = _setMap.get(quercusName);
          AbstractJavaMethod newSetter
            = new JavaMethod(moduleContext, this, method);

          if (existingSetter != null)
            newSetter = existingSetter.overload(newSetter);

          _setMap.put(quercusName, newSetter);
        } else if ("__get".equals(methodName)) {
          if (_funArrayDelegate == null)
            _funArrayDelegate = new FunctionArrayDelegate();

          _funArrayDelegate.setArrayGet(new JavaMethod(moduleContext, this, method));
        } else if ("__set".equals(methodName)) {
          if (_funArrayDelegate == null)
            _funArrayDelegate = new FunctionArrayDelegate();

          _funArrayDelegate.setArrayPut(new JavaMethod(moduleContext, this, method));
        } else if ("__count".equals(methodName)) {
          FunctionCountDelegate delegate = new FunctionCountDelegate();

          delegate.setCount(new JavaMethod(moduleContext, this, method));

          _countDelegate = delegate;
        } else if ("__getField".equals(methodName)) {
          __fieldGet = new JavaMethod(moduleContext, this, method);
        } else if ("__setField".equals(methodName)) {
          __fieldSet = new JavaMethod(moduleContext, this, method);
        } else if ("__fieldGet".equals(methodName)) {
          __fieldGet = new JavaMethod(moduleContext, this, method);
        } else if ("__fieldSet".equals(methodName)) {
          __fieldSet = new JavaMethod(moduleContext, this, method);
        }
      }
    }

    // server/2v00
    /*
    if (__fieldGet != null)
      _getMap.clear();

    if (__fieldSet != null)
      _setMap.clear();
    */

    // Introspect public non-static fields
    Field[] fields = type.getFields();

    for (Field field : fields) {
      if (Modifier.isStatic(field.getModifiers()))
        continue;
      else if (field.isAnnotationPresent(Hide.class))
        continue;

      MarshalFactory factory = moduleContext.getMarshalFactory();
      Marshal marshal = factory.create(field.getType(), false);

      _fieldMap.put(field.getName(),
                    new FieldMarshalPair(field, marshal));
    }


   // introspectFields(quercus, type.getSuperclass());
  }

  /**
   * helper for introspectFields
   *
   * @param s (eg: Foo, URL)
   * @return (foo, URL)
   */
  private string javaToQuercusConvert(String s)
  {
    if (s.length() == 1) {
      char ch = Character.toLowerCase(s[0]);

      return String.valueOf(ch);
    }
    else if (Character.isUpperCase(s[1])) {
      return s;
    }
    else {
      StringBuilder sb = new StringBuilder();

      sb.append(Character.toLowerCase(s[0]));

      int length = s.length();
      for (int i = 1; i < length; i++) {
        sb.append(s[i]);
      }

      return sb.ToString();
    }
  }

  private void introspectConstants(Class<?> type)
  {
    if (type == null)
      return;

    if (! Modifier.isPublic(type.getModifiers()))
      return;

    Field []fields = type.getFields();

    for (Field field : fields) {
      if (_constMap.get(field.getName()) != null)
        continue;
      else if (_constJavaMap.get(field.getName()) != null)
        continue;
      else if (! Modifier.isPublic(field.getModifiers()))
        continue;
      else if (! Modifier.isStatic(field.getModifiers()))
        continue;
      else if (! Modifier.isFinal(field.getModifiers()))
        continue;
      else if (field.isAnnotationPresent(Hide.class))
        continue;

      try {
        Object obj = field.get(null);

        Value value = QuercusContext.objectToValue(obj);

        if (value != null)
          _constMap.put(field.getName().intern(), value);
        else
          _constJavaMap.put(field.getName().intern(), obj);
      } catch (Throwable e) {
        log.log(Level.FINEST, e.ToString(), e);
      }
    }
  }

  private void introspectEnums(Class<?> type)
  {
    if (type == null) {
      return;
    }

    if (! Modifier.isPublic(type.getModifiers())) {
      return;
    }

    Class<?>[] classes = type.getClasses();

    for (Class<?> cls : classes) {
      if (! cls.isEnum()) {
        continue;
      }

      string name = cls.getSimpleName();

      if (_constMap.get(name) != null)
        continue;
      else if (_constJavaMap.get(name) != null)
        continue;
      else if (cls.isAnnotationPresent(Hide.class))
        continue;


      Object[] constants = cls.getEnumConstants();
      if (constants.length == 0) {
        continue;
      }

      // php/0cs3
      // use one of the enums as a handle for other enum siblings
      Object obj = constants[0];

      try {
        Value value = QuercusContext.objectToValue(obj);

        if (value != null)
          _constMap.put(name.intern(), value);
        else
          _constJavaMap.put(name.intern(), obj);
      }
      catch (Throwable e) {
        log.log(Level.FINEST, e.ToString(), e);
      }
    }
  }

  /**
   * Introspects the Java class.
   */
  private void introspectMethods(ModuleContext moduleContext,
                                 Class<?> type)
  {
    if (type == null)
      return;

    Method []methods = type.getMethods();

    for (Method method : methods) {
      if (! Modifier.isPublic(method.getModifiers()))
        continue;

      if (method.isAnnotationPresent(Hide.class))
        continue;

      if (_isPhpClass && method.getDeclaringClass() == Object.class)
        continue;

      if ("iterator".equals(method.getName())
          && method.getParameterTypes().length == 0
          && Iterator.class.isAssignableFrom(method.getReturnType())) {
        _iteratorMethod = method;
      }

      if ("printRImpl".equals(method.getName())) {
        _printRImpl = method;
      } else if ("varDumpImpl".equals(method.getName())) {
        _varDumpImpl = method;
      } else if (method.isAnnotationPresent(JsonEncode.class)) {
        _jsonEncode = method;
      } else if (method.isAnnotationPresent(Isset.class)) {
        _isset = method;
      } else if (method.isAnnotationPresent(EntrySet.class)) {
        _entrySet = method;
      } else if ("__call".equals(method.getName())) {
        __call = new JavaMethod(moduleContext, this, method);
      } else if ("__callStatic".equals(method.getName())) {
        __callStatic = new JavaMethod(moduleContext, this, method);
      } else if ("__ToString".equals(method.getName())) {
        __ToString = new JavaMethod(moduleContext, this, method);
        _functionMap.put(_moduleContext.createString(method.getName()), __ToString);
      } else if ("__destruct".equals(method.getName())) {
        __destruct = new JavaMethod(moduleContext, this, method);
        _functionMap.put(_moduleContext.createString(method.getName()), __destruct);
      } else {
        if (method.getName().startsWith("quercus_"))
          throw new UnsupportedOperationException(
            L.l("{0}: use @Name instead", method.getName()));

        JavaMethod newFun = new JavaMethod(moduleContext, this, method);
        string funName = newFun.getName();

        StringValue nameV = moduleContext.createString(funName);

        AbstractJavaMethod fun = _functionMap.getRaw(nameV);

        if (fun != null)
          fun = fun.overload(newFun);
        else
          fun = newFun;

        _functionMap.put(nameV, fun);
      }
    }

    /* Class.getMethods() @is recursive
    introspectMethods(moduleContext, type.getSuperclass());

    Class []ifcs = type.getInterfaces();

    for (Class ifc : ifcs) {
      introspectMethods(moduleContext, ifc);
    }
    */
  }

  public JavaMethod getToString()
  {
    return __ToString;
  }

  public StringValue ToString(Env env,
                              JavaValue value)
  {
    if (__ToString == null) {
      return null;
    }

    QuercusClass cls = getQuercusClass();
    Value str = __ToString.callMethod(env, cls, value, Expr.NULL_ARGS);

    return str.ToStringValue(env);
  }
  
  public bool issetField(Env env, Object obj, StringValue name)
  {
    if (_isset == null) {
      return false;
    }
    
    try {
      Object result = _isset.invoke(obj, env, name);
      
      return ! Boolean.FALSE.equals(result);

    } catch (InvocationTargetException e) {
      throw new QuercusRuntimeException(e);
    } catch (IllegalAccessException e) {
      throw new QuercusRuntimeException(e);
    }
  }

  public bool jsonEncode(Env env,
                            Object obj,
                            JsonEncodeContext context,
                            StringValue sb)
  {
    if (_jsonEncode == null)
      return false;

    try {
      _jsonEncode.invoke(obj, env, context, sb);
      return true;

    } catch (InvocationTargetException e) {
      throw new QuercusRuntimeException(e);
    } catch (IllegalAccessException e) {
      throw new QuercusRuntimeException(e);
    }
  }

  /**
   *
   * @return false if printRImpl not implemented
   * 
   */
  public bool printRImpl(Env env,
                            Object obj,
                            WriteStream @out,
                            int depth,
                            IdentityHashMap<Value, String> valueSet)
    
  {

    try {
      if (_printRImpl == null) {
        return false;

      }

      _printRImpl.invoke(obj, env, @out, depth, valueSet);
      return true;
    } catch (InvocationTargetException e) {
      throw new QuercusRuntimeException(e);
    } catch (IllegalAccessException e) {
      throw new QuercusRuntimeException(e);
    }
  }

  public bool varDumpImpl(Env env,
                             Value obj,
                             Object javaObj,
                             WriteStream @out,
                             int depth,
                             IdentityHashMap<Value, String> valueSet)
    
  {
    try {
      if (_varDumpImpl == null)
        return false;

      _varDumpImpl.invoke(javaObj, env, obj, @out, depth, valueSet);
      return true;
    } catch (InvocationTargetException e) {
      throw new QuercusRuntimeException(e);
    } catch (IllegalAccessException e) {
      throw new QuercusRuntimeException(e);
    }
  }

  private class JavaTraversableDelegate
    : TraversableDelegate
  {
    private Method _iteratorMethod;

    public JavaTraversableDelegate(Method iterator)
    {
      _iteratorMethod = iterator;
    }

    public Iterator<Map.Entry<Value, Value>>
      getIterator(Env env, ObjectValue qThis)
    {
      try {
        Object javaObj = qThis.toJavaObject();

        Iterator<?> iterator = (Iterator<?>) _iteratorMethod.invoke(javaObj);

        return new JavaIterator(env, iterator);
      } catch (InvocationTargetException e) {
        throw new QuercusRuntimeException(e);
      } catch (IllegalAccessException e) {
        throw new QuercusRuntimeException(e);
      }
    }

    public Iterator<Value> getKeyIterator(Env env, ObjectValue qThis)
    {
      try {
        Object javaObj = qThis.toJavaObject();

        Iterator<?> iterator = (Iterator<?>) _iteratorMethod.invoke(javaObj);

        return new JavaKeyIterator(iterator);
      } catch (InvocationTargetException e) {
        throw new QuercusRuntimeException(e);
      } catch (IllegalAccessException e) {
        throw new QuercusRuntimeException(e);
      }
    }

    public Iterator<Value> getValueIterator(Env env, ObjectValue qThis)
    {
      try {
        Object javaObj = qThis.toJavaObject();

        Iterator<?> iterator = (Iterator<?>) _iteratorMethod.invoke(javaObj);

        return new JavaValueIterator(env, iterator);
      } catch (InvocationTargetException e) {
        throw new QuercusRuntimeException(e);
      } catch (IllegalAccessException e) {
        throw new QuercusRuntimeException(e);
      }
    }
  }

  private class JavaKeyIterator
    : Iterator<Value>
  {
    private Iterator<?> _iterator;
    private int _index;

    public JavaKeyIterator(Iterator<?> iterator)
    {
      _iterator = iterator;
    }

    public Value next()
    {
      _iterator.next();

      return LongValue.create(_index++);
    }

    public bool hasNext()
    {
      return _iterator.hasNext();
    }

    public void remove()
    {
      throw new UnsupportedOperationException();
    }
  }

  private class JavaValueIterator
    : Iterator<Value>
  {
    private Env _env;
    private Iterator<?> _iterator;

    public JavaValueIterator(Env env, Iterator<?> iterator)
    {
      _env = env;
      _iterator = iterator;
    }

    public Value next()
    {
      Object next = _iterator.next();

      if (next instanceof Map.Entry) {
        Map.Entry entry = (Map.Entry) next;

        Object value = entry.getValue();

        if (value instanceof Value) {
          return (Value) value;
        }
        else {
          return _env.wrapJava(value);
        }
      }
      else {
        return _env.wrapJava(next);
      }
    }

    public bool hasNext()
    {
      if (_iterator != null)
        return _iterator.hasNext();
      else
        return false;
    }

    public void remove()
    {
      throw new UnsupportedOperationException();
    }
  }

  private class JavaIterator
    : Iterator<Map.Entry<Value, Value>>
  {
    private Env _env;
    private Iterator<?> _iterator;

    private int _index;

    public JavaIterator(Env env, Iterator<?> iterator)
    {
      _env = env;
      _iterator = iterator;
    }

    public Map.Entry<Value, Value> next()
    {
      Object next = _iterator.next();
      int index = _index++;

      if (next instanceof Map.Entry) {
        Map.Entry entry = (Map.Entry) next;

        Object key = entry.getKey();
        Object value = entry.getValue();

        if (key instanceof Value
            && value instanceof Value) {
          return (Map.Entry<Value, Value>) entry;
        }
        else if (key instanceof Value) {
          Value v = _env.wrapJava(value);

          return new JavaEntry((Value) key, v);
        }
        else {
          Value k = _env.wrapJava(key);

          return new JavaEntry(k, _env.wrapJava(value));
        }
      }
      else {
        return new JavaEntry(LongValue.create(index), _env.wrapJava(next));
      }
    }

    public bool hasNext()
    {
      if (_iterator != null)
        return _iterator.hasNext();
      else
        return false;
    }

    public void remove()
    {
      throw new UnsupportedOperationException();
    }
  }

  private class JavaEntry
    : Map.Entry<Value, Value>
  {
    private Value _key;
    private Value _value;

    public JavaEntry(Value key, Value value)
    {
      _key = key;
      _value = value;
    }

    public Value getKey()
    {
      return _key;
    }

    public Value getValue()
    {
      return _value;
    }

    public Value setValue(Value value)
    {
      throw new UnsupportedOperationException();
    }
  }

  private class MethodMarshalPair {
    public Method _method;
    public Marshal _marshal;

    public MethodMarshalPair(Method method,
                              Marshal marshal)
    {
      _method = method;
      _marshal = marshal;
    }
  }

  private class FieldMarshalPair {
    public Field _field;
    public Marshal _marshal;

    public FieldMarshalPair(Field field,
                             Marshal marshal)
    {
      _field = field;
      _marshal = marshal;
    }
  }

  private static class LongClassDef : JavaClassDef {
    LongClassDef(ModuleContext module)
     : base(module, "Long", Long.class) {
    }

    public override Value wrap(Env env, Object obj)
    {
      return LongValue.create(((Number) obj).longValue());
    }
  }

  private static class DoubleClassDef : JavaClassDef {
    DoubleClassDef(ModuleContext module)
     : base(module, "Double", Double.class) {
    }

    public override Value wrap(Env env, Object obj)
    {
      return new DoubleValue(((Number) obj).doubleValue());
    }
  }

  private static class BigIntegerClassDef : JavaClassDef {
    BigIntegerClassDef(ModuleContext module)
     : base(module, "BigInteger", BigInteger.class) {
    }

    public override Value wrap(Env env, Object obj)
    {
      return new BigIntegerValue(env, (BigInteger) obj, this);
    }
  }

  private static class BigDecimalClassDef : JavaClassDef {
    BigDecimalClassDef(ModuleContext module)
     : base(module, "BigDecimal", BigDecimal.class) {
    }

    public override Value wrap(Env env, Object obj)
    {
      return new BigDecimalValue(env, (BigDecimal) obj, this);
    }
  }

  private static class StringClassDef : JavaClassDef {
    StringClassDef(ModuleContext module)
     : base(module, "String", String.class) {
    }

    public override Value wrap(Env env, Object obj)
    {
      return env.createString((String) obj);
    }
  }

  private static class BooleanClassDef : JavaClassDef {
    BooleanClassDef(ModuleContext module)
     : base(module, "Boolean", Boolean.class) {
    }

    public override Value wrap(Env env, Object obj)
    {
      if (Boolean.TRUE.equals(obj))
        return BooleanValue.TRUE;
      else
        return BooleanValue.FALSE;
    }
  }

  private static class CalendarClassDef : JavaClassDef {
    CalendarClassDef(ModuleContext module)
     : base(module, "Calendar", Calendar.class) {
    }

    public override Value wrap(Env env, Object obj)
    {
      return new JavaCalendarValue(env, (Calendar)obj, this);
    }
  }

  private static class DateClassDef : JavaClassDef {
    DateClassDef(ModuleContext module, Class<?> type)
     : base(module, type.getSimpleName(), type) {
    }

    public override Value wrap(Env env, Object obj)
    {
      return new JavaDateValue(env, (Date) obj, this);
    }
  }

  private static class URLClassDef : JavaClassDef {
    URLClassDef(ModuleContext module)
     : base(module, "URL", URL.class) {
    }

    public override Value wrap(Env env, Object obj)
    {
      return new JavaURLValue(env, (URL)obj, this);
    }
  }
}

}
