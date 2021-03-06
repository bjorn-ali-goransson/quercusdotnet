using System;
namespace QuercusDotNet.Program{
/*
 * Copyright (c) 1998-2014 Caucho Technology -- all rights reserved
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
 * Represents an interpreted PHP class definition.
 */
public class InterpretedClassDef : ClassDef
  : InstanceInitializer
{
  protected bool _isAbstract;
  protected bool _isInterface;
  protected bool _isTrait;
  protected bool _isFinal;

  protected bool _hasPublicMethods;
  protected bool _hasProtectedMethods;
  protected bool _hasPrivateMethods;

  // true if defined in the top scope of a page
  private bool _isTopScope;

  protected LinkedHashMap<StringValue,AbstractFunction> _functionMap
    = new LinkedHashMap<StringValue,AbstractFunction>();

  protected LinkedHashMap<StringValue,ClassField> _fieldMap
    = new LinkedHashMap<StringValue,ClassField>();

  protected LinkedHashMap<StringValue,StaticFieldEntry> _staticFieldMap
    = new LinkedHashMap<StringValue,StaticFieldEntry>();

  protected HashMap<StringValue,Expr> _constMap
    = new HashMap<StringValue,Expr>();

  protected AbstractFunction _constructor;
  protected AbstractFunction _destructor;
  protected AbstractFunction _getField;
  protected AbstractFunction _setField;
  protected AbstractFunction _isset;
  protected AbstractFunction _unset;
  protected AbstractFunction _call;
  protected AbstractFunction _callStatic;

  protected AbstractFunction _serializeFun;
  protected AbstractFunction _unserializeFun;

  protected AbstractFunction _invoke;
  protected AbstractFunction _ToString;

  protected int _parseIndex;

  protected string _comment;

  public InterpretedClassDef(Location location,
                             string name,
                             string parentName,
                             string []ifaceList,
                             int index)
  {
    this(location,
          name, parentName, ifaceList, ClassDef.NULL_STRING_ARRAY, index);
  }

  public InterpretedClassDef(Location location,
                             string name,
                             string parentName,
                             string []ifaceList,
                             string []traitList,
                             int index)
  {
    super(location, name, parentName, ifaceList, traitList);

    _parseIndex = index;
  }

  public InterpretedClassDef(String name,
                             string parentName,
                             string []ifaceList,
                             string []traitList)
  {
    this(null, name, parentName, ifaceList, traitList, 0);
  }

  public InterpretedClassDef(String name,
                             string parentName,
                             string []ifaceList)
  {
    this(null, name, parentName, ifaceList, ClassDef.NULL_STRING_ARRAY, 0);
  }

  /**
   * true for an abstract class.
   */
  public void setAbstract(bool isAbstract)
  {
    _isAbstract = isAbstract;
  }

  /**
   * True for an abstract class.
   */
  public override bool isAbstract()
  {
    return _isAbstract;
  }

  /**
   * true for an interface class.
   */
  public void setInterface(bool isInterface)
  {
    _isInterface = isInterface;
  }

  /**
   * True for an interface class.
   */
  public override bool isInterface()
  {
    return _isInterface;
  }

  /**
   * true for an trait class.
   */
  public void setTrait(bool isTrait)
  {
    _isTrait = isTrait;
  }

  /**
   * True for an trait class.
   */
  public override bool isTrait()
  {
    return _isTrait;
  }

  /**
   * True for a class.
   */
  public void setFinal(bool isFinal)
  {
    _isFinal = isFinal;
  }

  /**
   * Returns true for a class.
   */
  public bool isFinal()
  {
    return _isFinal;
  }

  /**
   * Returns true if class has public methods.
   */
  public bool hasPublicMethods()
  {
    return _hasPublicMethods;
  }

  /**
   * Returns true if class has protected or private methods.
   */
  public bool hasProtectedMethods()
  {
    return _hasProtectedMethods;
  }

  /**
   * Returns true if the class has private methods.
   */
  public bool hasPrivateMethods()
  {
    return _hasPrivateMethods;
  }

  /**
   * True if defined at the top-level scope
   */
  public bool isTopScope()
  {
    return _isTopScope;
  }

  /**
   * True if defined at the top-level scope
   */
  public void setTopScope(bool isTopScope)
  {
    _isTopScope = isTopScope;
  }

  /**
   * Unique name to use for compilation.
   */
  public string getCompilationName()
  {
    string name = getName();
    name = name.replace("__", "___");
    name = name.replace("\\", "__");

    return name + "_" + _parseIndex;
  }

  /**
   * Unique instance name for the compiled class.
   */
  public string getCompilationInstanceName()
  {
    return "q_cl_" + getCompilationName();
  }

  /**
   * Initialize the quercus class methods.
   */
  public override void initClassMethods(QuercusClass cl, string bindingClassName)
  {
    cl.addInitializer(this);

    if (_constructor != null) {
      cl.setConstructor(_constructor);

      // php/093o
      //if (_functionMap.get("__construct") == null) {
      //  cl.addMethod("__construct", _constructor);
      //}
    }

    if (_destructor != null) {
      cl.setDestructor(_destructor);

      // XXX: make sure we need to do this (test case), also look at ProClassDef
      cl.addMethod(cl.getModuleContext().createString("__destruct"), _destructor);
    }

    if (_getField != null)
      cl.setFieldGet(_getField);

    if (_setField != null)
      cl.setFieldSet(_setField);

    if (_call != null) {
      cl.setCall(_call);
    }

    if (_callStatic != null) {
      cl.setCallStatic(_callStatic);
    }

    if (_invoke != null)
      cl.setInvoke(_invoke);

    if (_ToString != null)
      cl.setToString(_ToString);

    if (_isset != null)
      cl.setIsset(_isset);

    if (_unset != null)
      cl.setUnset(_unset);

    if (_serializeFun != null) {
      cl.setSerialize(_serializeFun, _unserializeFun);
    }

    for (Map.Entry<StringValue,AbstractFunction> entry : _functionMap.entrySet()) {
      StringValue funName = entry.getKey();
      AbstractFunction fun = entry.getValue();

      if (fun.isTraitMethod()) {
        cl.addTraitMethod(bindingClassName, funName, fun);
      }
      else {
        cl.addMethod(funName, fun);
      }
    }
  }

  /**
   * Initialize the quercus class fields.
   */
  public override void initClassFields(QuercusClass cl, string declaringClassName)
  {
    if (isTrait()) {
      for (Map.Entry<StringValue,ClassField> entry : _fieldMap.entrySet()) {
        ClassField field = entry.getValue();

        cl.addTraitField(field);
      }
    }
    else {
      for (Map.Entry<StringValue,ClassField> entry : _fieldMap.entrySet()) {
        ClassField field = entry.getValue();

        cl.addField(field);
      }
    }

    if (isTrait()) {
      for (Map.Entry<StringValue, StaticFieldEntry> entry : _staticFieldMap.entrySet()) {
        StaticFieldEntry field = entry.getValue();

        cl.addStaticTraitFieldExpr(declaringClassName,
                                   entry.getKey(), field.getValue());
      }
    }
    else {
      string className = getName();
      for (Map.Entry<StringValue, StaticFieldEntry> entry : _staticFieldMap.entrySet()) {
        StaticFieldEntry field = entry.getValue();

        cl.addStaticFieldExpr(className, entry.getKey(), field.getValue());
      }
    }

    for (Map.Entry<StringValue, Expr> entry : _constMap.entrySet()) {
      cl.addConstant(entry.getKey(), entry.getValue());
    }
  }

  /**
   * Sets the constructor.
   */
  public void setConstructor(AbstractFunction fun)
  {
    _constructor = fun;
  }

  public override AbstractFunction getCall()
  {
    return _call;
  }

  public override AbstractFunction getCallStatic()
  {
    return _callStatic;
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
   * Adds a function.
   */
  public void addFunction(StringValue name, Function fun)
  {
    _functionMap.put(name, fun);

    if (fun.isPublic()) {
      _hasPublicMethods = true;
    }

    if (fun.isProtected()) {
      _hasProtectedMethods = true;
    }

    if (fun.isPrivate()) {
      _hasPrivateMethods = true;
    }

    if (name.equalsString("__construct")) {
      _constructor = fun;
    }
    else if (name.equalsString("__destruct")) {
      _destructor = fun;
    }
    else if (name.equalsString("__get")) {
      _getField = fun;
    }
    else if (name.equalsString("__set")) {
      _setField = fun;
    }
    else if (name.equalsString("__call")) {
      _call = fun;
    }
    else if (name.equalsString("__callStatic")) {
      _callStatic = fun;
    }
    else if (name.equalsString("__invoke")) {
      _invoke = fun;
    }
    else if (name.equalsString("__ToString")) {
      _ToString = fun;
    }
    else if (name.equalsString("__isset")) {
      _isset = fun;
    }
    else if (name.equalsString("__unset")) {
      _unset = fun;
    }
    else if (name.equalsStringIgnoreCase(getName()) && _constructor == null) {
      _constructor = fun;
    }
    else if (name.equalsString("serialize") && isA(null, "Serializable")) {
      _serializeFun = fun;
    }
    else if (name.equalsString("unserialize") && isA(null, "Serializable")) {
      _unserializeFun = fun;
    }
  }

  /**
   * Adds a static value.
   */
  public void addStaticValue(Value name, Expr value)
  {
    _staticFieldMap.put(name.ToStringValue(), new StaticFieldEntry(value));
  }

  /**
   * Adds a static value.
   */
  public void addStaticValue(Value name, Expr value, string comment)
  {
    _staticFieldMap.put(name.ToStringValue(), new StaticFieldEntry(value, comment));
  }

  /**
   * Adds a const value.
   */
  public void addConstant(StringValue name, Expr value)
  {
    _constMap.put(name, value);
  }

  /**
   * Return a const value.
   */
  public Expr findConstant(String name)
  {
    return _constMap.get(name);
  }

  /**
   * Adds a value.
   */
  public void addClassField(StringValue name,
                            Expr value,
                            FieldVisibility visibility,
                            string comment)
  {
    ClassField field = new ClassField(name,
                                      getName(),
                                      value,
                                      visibility,
                                      comment,
                                      isTrait());

    _fieldMap.put(name, field);
  }

  /**
   * Adds a value.
   */
  public ClassField getClassField(StringValue name)
  {
    ClassField field = _fieldMap.get(name);

    return field;
  }

  /**
   * Return true for a declared field.
   */
  public bool isDeclaredField(StringValue name)
  {
    return _fieldMap.get(name) != null;
  }

  /**
   * Initialize the class.
   */
  public void init(Env env)
  {
    QuercusClass qClass = env.getClass(getName());

    for (Map.Entry<StringValue,StaticFieldEntry> entry : _staticFieldMap.entrySet()) {
      StringValue name = entry.getKey();

      StaticFieldEntry field = entry.getValue();

      Var var = qClass.getStaticFieldVar(env, name);

      var.set(field.getValue().eval(env).copy());
    }
  }

  /**
   * Initialize the fields
   */
  public override void initInstance(Env env, Value obj, bool isInitFieldValues)
  {
    ObjectValue object = (ObjectValue) obj;

    for (Map.Entry<StringValue,ClassField> entry : _fieldMap.entrySet()) {
      ClassField field = entry.getValue();

      object.initField(env, field, isInitFieldValues);
    }

    if (_destructor != null) {
      env.addObjectCleanup(object);
    }
  }

  /**
   * Returns the constructor
   */
  public AbstractFunction findConstructor()
  {
    return _constructor;
  }

  /**
   * Sets the documentation for this class.
   */
  public void setComment(String comment)
  {
    _comment = comment;
  }

  /**
   * Returns the documentation for this class.
   */
  public override string getComment()
  {
    return _comment;
  }

  /**
   * Returns the comment for the specified field.
   */
  public override string getFieldComment(StringValue name)
  {
    ClassField field = _fieldMap.get(name);

    if (field != null)
      return field.getComment();
    else
      return null;
  }

  /**
   * Returns the comment for the specified field.
   */
  public override string getStaticFieldComment(StringValue name)
  {
    StaticFieldEntry field = _staticFieldMap.get(name);

    if (field != null)
      return field.getComment();
    else
      return null;
  }

  public override Set<Map.Entry<StringValue,ClassField>> fieldSet()
  {
    return _fieldMap.entrySet();
  }

  public override ClassField getField(StringValue name)
  {
    return _fieldMap.get(name);
  }

  public override Set<Map.Entry<StringValue, StaticFieldEntry>> staticFieldSet()
  {
    return _staticFieldMap.entrySet();
  }

  public override Set<Map.Entry<StringValue, AbstractFunction>> functionSet()
  {
    return _functionMap.entrySet();
  }

  public AbstractFunction getFunction(StringValue name)
  {
    return _functionMap.get(name);
  }

  public HashMap<StringValue,AbstractFunction> getFunctionMap()
  {
    return _functionMap;
  }
}

}
