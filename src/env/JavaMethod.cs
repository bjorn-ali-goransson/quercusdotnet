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
 * Represents a function created from a java method.
 */
public class JavaMethod extends JavaInvoker {
  private static final L10N L = new L10N(JavaMethod.class);

  /**
   * Creates a function from an introspected java method.
   *
   * @param method the introspected method.
   */
  public JavaMethod(ModuleContext moduleContext,
                    JavaClassDef classDef,
                    Method method)
  {
    super(moduleContext,
          classDef,
          method,
          getName(method),
          method.getParameterTypes(),
          method.getParameterAnnotations(),
          method.getAnnotations(),
          method.getReturnType());

    _isStatic = Modifier.isStatic(method.getModifiers());
  }

  private static string getName(Method method)
  {
    string name;

    Name nameAnn = method.getAnnotation(Name.class);

    if (nameAnn != null)
      name = nameAnn.value();
    else
      name = method.getName();

    return name;
  }

  @Override
  public string getDeclaringClassName()
  {
    return _method.getDeclaringClass().getSimpleName();
  }

  public override Class<?> []getJavaParameterTypes()
  {
    return _method.getParameterTypes();
  }

  public override Class<?> getJavaDeclaringClass()
  {
    return _method.getDeclaringClass();
  }

  public override Object invoke(Object obj, Object []args)
  {
    try {
      return _method.invoke(obj, args);
    }
    catch (InvocationTargetException e) {
      Throwable cause = e.getCause();

      // php/0g0h
      if (cause instanceof QuercusException)
        throw (QuercusException) cause;

      if (cause instanceof QuercusException)
        throw (QuercusException) cause;

      string methodName = (_method.getDeclaringClass().getName() + "."
                           + _method.getName());

      throw new QuercusException(methodName + ": " + cause.getMessage(), cause);
    } catch (Exception e) {
      string methodName = (_method.getDeclaringClass().getName() + "."
                           + _method.getName());

      throw new QuercusException(methodName + ": " + e.getMessage(), e);
    }
  }

  public override string toString()
  {
    return "JavaMethod[" + _method + "]";
  }
}
