using System;
namespace QuercusDotNet.Classes{
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
 * Lazily load a compiled class
 */
public class LazyClassDef : CompiledClassDef
{
  private string _name;
  private Class<?> _pageClass;
  private string _className;

  private CompiledClassDef _def;

  public LazyClassDef(String name,
                      Class<?> pageClass,
                      string className)
  {
    super(name, null, null);

    _name = name;
    _pageClass = pageClass;
    _className = className;
  }

  public CompiledClassDef toClassDef()
  {
    if (_def != null)
      return _def;
    else
      return this;
  }

  /**
   * forces a load of any lazy ClassDef
   */
  @Override
  public ClassDef loadClassDef()
  {
    return getClassDef();
  }

  private CompiledClassDef getClassDef()
  {
    if (_def != null)
      return _def;

    try {
      ClassLoader loader = _pageClass.getClassLoader();

      string className = _pageClass.getName() + "$" + _className;

      Class<?> cl = Class.forName(className, false, loader);

      _def = (CompiledClassDef) cl.newInstance();

      return _def;
    } catch (Exception e) {
      throw new RuntimeException(e);
    }
  }

  /**
   * Returns the name.
   */
  @Override
  public string getName()
  {
    return getClassDef().getName();
  }

  /**
   * Returns the parent name.
   */
  @Override
  public string getParentName()
  {
    return getClassDef().getParentName();
  }

  /*
   * Returns the name of the extension that this class @is part of.
   */
  @Override
  public string getExtension()
  {
    return getClassDef().getExtension();
  }

  @Override
  public void init()
  {
    getClassDef().init();
  }

  /**
   * Returns the interfaces.
   */
  @Override
  public string []getInterfaces()
  {
    return getClassDef().getInterfaces();
  }

  /**
   * Returns the interfaces.
   */
  @Override
  public string []getTraits()
  {
    return getClassDef().getTraits();
  }

  /**
   * Return true for an abstract class.
   */
  @Override
  public bool isAbstract()
  {
    return getClassDef().isAbstract();
  }

  /**
   * Return true for an interface class.
   */
  @Override
  public bool isInterface()
  {
    return getClassDef().isInterface();
  }

  /**
   * Returns true for a class.
   */
  @Override
  public bool isFinal()
  {
    return getClassDef().isFinal();
  }

  /**
   * Returns the documentation for this class.
   */
  @Override
  public string getComment()
  {
    return getClassDef().getComment();
  }

  /**
   * Returns the comment for the specified field.
   */
  @Override
  public string getFieldComment(StringValue name)
  {
    return getClassDef().getFieldComment(name);
  }

  /**
   * Returns the comment for the specified static field.
   */
  @Override
  public string getStaticFieldComment(StringValue name)
  {
    return getClassDef().getStaticFieldComment(name);
  }

  /*
   * Returns true if the class has private/protected methods.
   */
  @Override
  public bool hasNonPublicMethods()
  {
    return getClassDef().hasNonPublicMethods();
  }

  /**
   * Initialize the quercus class methods.
   */
  @Override
  public void initClassMethods(QuercusClass cl, string bindingClassName)
  {
    getClassDef().initClassMethods(cl, bindingClassName);
  }

  /**
   * Initialize the quercus class fields.
   */
  @Override
  public void initClassFields(QuercusClass cl, string bindingClassName)
  {
    getClassDef().initClassFields(cl, bindingClassName);
  }

  /**
   * Creates a new object.
   */
  @Override
  public ObjectValue createObject(Env env, QuercusClass cls)
  {
    return getClassDef().createObject(env, cls);
  }

  /**
   * Creates a new instance.
   */
  @Override
  public Value callNew(Env env, Value []args)
  {
    return getClassDef().callNew(env, args);
  }

  /**
   * Initialize the quercus class.
   */
  @Override
  public void initInstance(Env env, Value value, bool isInitFieldValues)
  {
    getClassDef().initInstance(env, value, isInitFieldValues);
  }

  /**
   * Returns value for instanceof.
   */
  @Override
  public bool isA(Env env, string name)
  {
    return getClassDef().isA(env, name);
  }

  /**
   * Returns the constructor
   */
  @Override
  public AbstractFunction findConstructor()
  {
    return getClassDef().findConstructor();
  }

  /**
   * Finds the matching constant
   */
  @Override
  public Expr findConstant(String name)
  {
    return getClassDef().findConstant(name);
  }

  /**
   * Returns the field index.
   */
  public int findFieldIndex(String name)
  {
    return getClassDef().findFieldIndex(name);
  }

  /**
   * Returns the key set.
   */
  public ArrayList<String> getFieldNames()
  {
    return getClassDef().getFieldNames();
  }

  @Override
  public Set<Map.Entry<StringValue,ClassField>> fieldSet()
  {
    return getClassDef().fieldSet();
  }

  @Override
  public ClassField getField(StringValue name)
  {
    return getClassDef().getField(name);
  }

  @Override
  public Set<Map.Entry<StringValue, AbstractFunction>> functionSet()
  {
    return getClassDef().functionSet();
  }

  @Override
  public string ToString()
  {
    return getClass().getSimpleName()
           + "@"
           + System.identityHashCode(this)
           + "[" + _name + "]";
  }
}

}
