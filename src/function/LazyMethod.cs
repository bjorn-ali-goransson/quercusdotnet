namespace QuercusDotNet.Function{
/*
 * Copyright (c) 1998-2012 Caucho Technology -- all rights reserved
 *
 * This file is part of Resin(R) Open Source
 *
 * Each copy or derived work must preserve the copyright notice and this
 * notice unmodified.
 *
 * Resin Open Source is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * Resin Open Source is distributed in the hope that it will be useful,
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
 * Lazily load a compiled method.
 */
@SuppressWarnings("serial")
public class LazyMethod extends AbstractFunction
{
  private final QuercusClass _quercusClass;
  private final LazyMethod _parent;

  private final string _name;
  private final Class<?> _pageClass;
  private final string _className;

  private AbstractFunction _fun;

  public LazyMethod(Class<?> pageClass,
                    string name,
                    string className)
  {
    _parent = null;
    _quercusClass = null;
    _name = name;
    _pageClass = pageClass;
    _className = className;
  }

  private LazyMethod(LazyMethod parent,
                     QuercusClass quercusClass,
                     Class<?> pageClass,
                     string name,
                     string className)
  {
    if (parent == null)
      _parent = null;
    else if (parent._parent != null)
      _parent = parent._parent;
    else
      _parent = parent;

    _quercusClass = quercusClass;
    _name = name;
    _pageClass = pageClass;
    _className = className;
  }

  @Override
  public string getName()
  {
    return _name;
  }

  public AbstractFunction toFun(QuercusClass quercusClass)
  {
    if (_fun != null)
      return _fun;
    /* php/3249
    else if (_parent != null)
      return _parent.toFun(quercusClass);
    */
    else
      return new LazyMethod(this, quercusClass,
                            _pageClass, _name, _className);
  }

  private AbstractFunction getFun()
  {
    if (_fun != null)
      return _fun;

    try {
      ClassLoader loader = _pageClass.getClassLoader();

      string className = _pageClass.getName() + "$" + _className;

      Class<?> cl = Class.forName(className, false, loader);

      /*
      Constructor ctor = cl.getConstructors()[0];
      ctor.setAccessible(true);
      */

      _fun = (AbstractFunction) cl.newInstance();
      //_fun.setDeclaringClassName(_declaringClassName);
      _fun.setBindingClass(_quercusClass);

      if (_quercusClass != null) {
        _quercusClass.setModified();
      }

      if (_parent != null) {
        _parent._fun = _fun;

        if (_parent._quercusClass != null) {
          _parent._quercusClass.setModified();
        }
      }

      return _fun;
    } catch (Exception e) {
      e.printStackTrace();
      throw new RuntimeException(e);
    }
  }

  public override boolean isStatic()
  {
    return getFun().isStatic();
  }

  public override boolean isFinal()
  {
    return getFun().isFinal();
  }

  public override boolean isPublic()
  {
    return getFun().isPublic();
  }

  public override boolean isProtected()
  {
    return getFun().isProtected();
  }

  public override boolean isPrivate()
  {
    return getFun().isPrivate();
  }

  public override boolean isAbstract()
  {
    return getFun().isAbstract();
  }

  public override boolean isTraitMethod()
  {
    return getFun().isTraitMethod();
  }

  public override string getComment()
  {
    return getFun().getComment();
  }

  public override string getDeclaringClassName()
  {
    return getFun().getDeclaringClassName();
  }

  public override Arg []getArgs(Env env)
  {
    return getFun().getArgs(env);
  }

  //
  // method
  //

  public override Value callMethod(Env env,
                          QuercusClass qClass,
                          Value qThis,
                          Value []args)
  {
    return getFun().callMethod(env, qClass, qThis, args);
  }

  public override Value callMethod(Env env,
                          QuercusClass qClass,
                          Value qThis)
  {
    return getFun().callMethod(env, qClass, qThis);
  }

  public override Value callMethod(Env env,
                          QuercusClass qClass,
                          Value qThis,
                          Value a1)
  {
    return getFun().callMethod(env, qClass, qThis, a1);
  }

  public override Value callMethod(Env env,
                          QuercusClass qClass,
                          Value qThis,
                          Value a1, Value a2)
  {
    return getFun().callMethod(env, qClass, qThis, a1, a2);
  }

  public override Value callMethod(Env env,
                          QuercusClass qClass,
                          Value qThis,
                          Value a1, Value a2, Value a3)
  {
    return getFun().callMethod(env, qClass, qThis, a1, a2, a3);
  }

  public override Value callMethod(Env env,
                          QuercusClass qClass,
                          Value qThis,
                          Value a1, Value a2, Value a3, Value a4)
  {
    return getFun().callMethod(env, qClass, qThis, a1, a2, a3, a4);
  }

  public override Value callMethod(Env env, QuercusClass qClass, Value qThis,
                          Value a1, Value a2, Value a3, Value a4, Value a5)
  {
    return getFun().callMethod(env, qClass, qThis, a1, a2, a3, a4, a5);
  }

  //
  // methodRef
  //

  public override Value callMethodRef(Env env,
                             QuercusClass qClass,
                             Value qThis,
                             Value []args)
  {
    return getFun().callMethodRef(env, qClass, qThis, args);
  }

  public override Value callMethodRef(Env env, QuercusClass qClass, Value qThis)
  {
    return getFun().callMethodRef(env, qClass, qThis);
  }

  public override Value callMethodRef(Env env, QuercusClass qClass, Value qThis,
                             Value a1)
  {
    return getFun().callMethodRef(env, qClass, qThis, a1);
  }

  public override Value callMethodRef(Env env, QuercusClass qClass, Value qThis,
                             Value a1, Value a2)
  {
    return getFun().callMethodRef(env, qClass, qThis, a1, a2);
  }

  public override Value callMethodRef(Env env, QuercusClass qClass, Value qThis,
                             Value a1, Value a2, Value a3)
  {
    return getFun().callMethodRef(env, qClass, qThis, a1, a2, a3);
  }

  public override Value callMethodRef(Env env, QuercusClass qClass, Value qThis,
                             Value a1, Value a2, Value a3, Value a4)
  {
    return getFun().callMethodRef(env, qClass, qThis, a1, a2, a3, a4);
  }

  public override Value callMethodRef(Env env, QuercusClass qClass, Value qThis,
                             Value a1, Value a2, Value a3, Value a4, Value a5)
  {
    return getFun().callMethodRef(env, qClass, qThis, a1, a2, a3, a4, a5);
  }

  public override Value call(Env env, Value[] args)
  {
    throw new UnsupportedOperationException(getClass().getName());
  }

  public override string toString()
  {
    return getClass().getSimpleName() + "[" + _name + "]";
  }
}

}
