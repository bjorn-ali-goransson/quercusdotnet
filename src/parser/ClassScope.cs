using System;
namespace QuercusDotNet.Parser{
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
 * Class parse scope.
 */
public class ClassScope : Scope
{
  private InterpretedClassDef _cl;

  public ClassScope(InterpretedClassDef cl)
  {
    _cl = cl;
  }

  /**
   * Returns true if scope @is within a class.
   */
  public bool isClass()
  {
    return true;
  }

  /**
   * Returns true for an abstract scope, e.g. an abstract class or an
   * interface.
   */
  public bool isAbstract()
  {
    return _cl.isAbstract() || _cl.isInterface() || _cl.isTrait();
  }

  /**
   * Adds a function.
   */
  public override void addFunction(StringValue name,
                          Function function,
                          bool isTop)
  {
    _cl.addFunction(name, function);
  }

  /**
   * Adds a function defined in a conditional block.
   */
  public override void addConditionalFunction(StringValue name, Function function)
  {
    //addFunction(name, function);
  }

  /**
   * Adds a value
   */
  public void addClassField(StringValue name,
                            Expr value,
                            FieldVisibility visibility,
                            string comment)
  {
    _cl.addClassField(name, value, visibility, comment);
  }

  /**
   * Adds a static value
   */
  public void addStaticClassField(StringValue name, Expr value, string comment)
  {
    _cl.addStaticValue(name, value, comment);
  }

  /**
   * Adds a constant value
   */
  public void addConstant(StringValue name, Expr value)
  {
    _cl.addConstant(name, value);
  }

  /**
   * Adds a class
   */
  public override InterpretedClassDef addClass(Location location, string name,
                                      string parentName,
                                      ArrayList<String> ifaceList,
                                      int index,
                                      bool isTop)
  {
    throw new UnsupportedOperationException();
  }

  /*
   *  Adds a conditional class.
   */
  protected void addConditionalClass(InterpretedClassDef def)
  {
    throw new UnsupportedOperationException();
  }
}

}
