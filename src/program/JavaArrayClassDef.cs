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
public class JavaArrayClassDef : JavaClassDef {
  public JavaArrayClassDef(ModuleContext moduleContext,
                           string name,
                           Class type)
  {
    super(moduleContext, name, type);
  }
  
  public JavaArrayClassDef(ModuleContext moduleContext,
                           string name,
                           Class type,
                           string extension)
   : base(moduleContext, name, type, extension) {
  }

  public override bool isArray()
  {
    return true;
  }

  public override Value wrap(Env env, Object obj)
  {
    if (! _isInit)
      init();
    
    ArrayValueImpl arrayValueImpl = new ArrayValueImpl();

    // XXX: needs to go into constructor
    Class componentClass = getType().getComponentType();

    MarshalFactory factory = getModuleContext().getMarshalFactory();
    Marshal componentClassMarshal = factory.create(componentClass);

    int length = Array.getLength(obj);
      
    for (int i = 0; i < length; i++) {
      Object component = Array.get(obj, i);
      
      arrayValueImpl.put(componentClassMarshal.unmarshal(env, component));
    }

    return arrayValueImpl;
  }
}

}
