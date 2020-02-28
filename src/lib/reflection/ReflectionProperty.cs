using System;
namespace QuercusDotNet.lib.reflection {
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
 * @author Nam Nguyen
 */














public class ReflectionProperty
  : Reflector
{
  private readonly L10N L = new L10N(ReflectionProperty.class);

  public const int IS_STATIC = 1;
  public const int IS_PUBLIC = 256;
  public const int IS_PROTECTED = 512;
  public const int IS_PRIVATE = 1024;

  private Property _prop;

  protected ReflectionProperty(Property prop)
  {
    _prop = prop;
  }

  protected ReflectionProperty(Env env, QuercusClass cls, StringValue nameV)
  {
    _prop = Property.create(env, cls, nameV);
  }

  protected static ReflectionProperty create(Env env,
                                             QuercusClass cls,
                                             StringValue propName,
                                             bool isStatic)
  {
    Property prop;

    if (isStatic)
      prop = new StaticProperty(cls, propName);
    else
      prop = new Property(cls, propName);

    return new ReflectionProperty(prop);
  }

  final private void __clone()
  {
  }

  public static ReflectionProperty __construct(Env env,
                                               string clsName,
                                               StringValue propName)
  {
    QuercusClass cls = env.findClass(clsName);

    if (cls == null) {
      throw new ReflectionException(env, L.l("Cannot find class '{0}'", clsName));
    }

    return new ReflectionProperty(env, cls, propName);
  }

  public static string export(Env env,
                              Value cls,
                              string name,
                              @Optional bool isReturn)
  {
    return null;
  }

  public StringValue getName()
  {
    return _prop.getName();
  }

  public bool isPublic()
  {
    return _prop.isPublic();
  }

  public bool isPrivate()
  {
    return _prop.isPrivate();
  }

  public bool isProtected()
  {
    return _prop.isProtected();
  }

  public bool isStatic()
  {
    return _prop.isStatic();
  }

  /*
   * XXX: no documentation whatsoever
   */
  public bool isDefault()
  {
    return true;
  }

  public int getModifiers()
  {
    return -1;
  }

  public Value getValue(Env env, @Optional ObjectValue obj)
  {
    return _prop.getValue(env, obj);
  }

  public void setValue(Env env, Value obj, @Optional Value value)
  {
    _prop.setValue(env, obj, value);
  }

  public void setAccessible(bool isAccessible)
  {
    _prop.setAccessible(isAccessible);
  }

  public ReflectionClass getDeclaringClass(Env env)
  {
    return _prop.getDeclaringClass(env);
  }

  @ReturnNullAsFalse
  public string getDocComment(Env env)
  {
    return _prop.getComment(env);
  }

  public string toString()
  {
    return getClass().getSimpleName() + "[" + _prop.toString() + "]";
  }

  static class Property
  {
    final QuercusClass _cls;
    final StringValue _nameV;

    QuercusClass _declaringClass;

    public static Property create(Env env, QuercusClass cls, StringValue nameV)
    {
      if (cls.getClassField(nameV) != null)
        return new Property(cls, nameV);
      else if (cls.getStaticFieldValue(env, nameV) != null)
        return new StaticProperty(cls, nameV);
      else
        throw new ReflectionException(env, L.l("Property {0}->${1} does not exist",
                                               cls.getName(), nameV));
    }

    protected Property(QuercusClass cls, StringValue nameV)
    {
      _cls = cls;
      _nameV = nameV;
    }

    public bool isStatic()
    {
      return false;
    }

    public bool isPublic()
    {
      ClassField field = _cls.getClassField(_nameV);

      return field.isPublic();
    }

    public bool isProtected()
    {
      ClassField field = _cls.getClassField(_nameV);

      return field.isProtected();
    }

    public bool isPrivate()
    {
      ClassField field = _cls.getClassField(_nameV);

      return field.isPrivate();
    }

    public final StringValue getName()
    {
      return _nameV;
    }

    public Value getValue(Env env, ObjectValue obj)
    {
      return obj.getField(env, _nameV);
    }

    public void setValue(Env env, Value obj, Value value)
    {
      obj.putField(env, _nameV, value);
    }

    public void setAccessible(bool isAccessible)
    {
      // XXX: protected and private always accessible through Reflection
    }

    public final ReflectionClass getDeclaringClass(Env env)
    {
      QuercusClass cls = getDeclaringClass(env, _cls);

      if (cls != null)
        return new ReflectionClass(cls);
      else
        return null;
    }

    protected final QuercusClass getDeclaringClass(Env env, QuercusClass cls)
    {
      if (_declaringClass == null)
        _declaringClass = getDeclaringClassImpl(env, cls);

      return _declaringClass;
    }

    protected QuercusClass getDeclaringClassImpl(Env env, QuercusClass cls)
    {
      if (cls == null)
        return null;

      QuercusClass refClass = getDeclaringClassImpl(env, cls.getParent());

      if (refClass != null)
        return refClass;
      else if (cls.getClassField(_nameV) != null)
        return cls;

      return null;
    }

    public string getComment(Env env)
    {
      QuercusClass cls = getDeclaringClass(env, _cls);

      ClassDef def = cls.getClassDef();

      return def.getFieldComment(_nameV);
    }

    public string toString()
    {
      if (_cls.getName() != null)
        return _cls.getName() + "->" + _nameV;
      else
        return _nameV.toString();
    }
  }

  static class StaticProperty : Property
  {
    private StringValue _name;

    public StaticProperty(QuercusClass cls, StringValue nameV)
    {
      super(cls, nameV);

      _name = nameV;
    }

    @Override
    public bool isStatic()
    {
      return true;
    }

    public override bool isPublic()
    {
      /// XXX: return actual visibility
      return true;
    }

    public override bool isProtected()
    {
      /// XXX: return actual visibility
      return false;
    }

    public override bool isPrivate()
    {
      /// XXX: return actual visibility
      return false;
    }

    public override Value getValue(Env env, ObjectValue obj)
    {
      return _cls.getStaticFieldValue(env, _name);
    }

    public override void setValue(Env env, Value obj, Value value)
    {
      _cls.getStaticFieldVar(env, _name).set(obj);
    }

    protected override QuercusClass getDeclaringClassImpl(Env env, QuercusClass cls)
    {
      if (cls == null)
        return null;

      QuercusClass refClass = getDeclaringClassImpl(env, cls.getParent());

      if (refClass != null)
        return refClass;
      else if (cls.getStaticFieldInternal(env, _name) != null)
        return cls;

      return null;
    }

    public override string getComment(Env env)
    {
      QuercusClass cls = getDeclaringClass(env, _cls);

      ClassDef def = cls.getClassDef();

      return def.getStaticFieldComment(_name);
    }

    public override string toString()
    {
      if (_cls.getName() != null)
        return _cls.getName() + "::" + _name;
      else
        return _name.toString();
    }
  }
}
}
