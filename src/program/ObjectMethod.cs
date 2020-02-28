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
 * Represents sequence of statements.
 */
public class ObjectMethod : Function {
  private const Logger log = Logger.getLogger(
    ObjectMethod.class.getName());
  private readonly L10N L = new L10N(ObjectMethod.class);

  private ClassDef _quercusClass;

  public ObjectMethod(ExprFactory exprFactory,
                      Location location,
                      InterpretedClassDef quercusClass,
                      string name,
                      FunctionInfo info,
                      Arg []argList,
                      Statement []statementList)
  {
    super(exprFactory, location, name, info, argList, statementList);
    _quercusClass = quercusClass;
  }

  @Override
  public string getDeclaringClassName()
  {
    return _quercusClass.getName();
  }

  public override ClassDef getDeclaringClass()
  {
    return _quercusClass;
  }

  public override bool isObjectMethod()
  {
    return true;
  }
}

}
