using System;
namespace QuercusDotNet.lib.dom {
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
 * @author Sam
 */









public class DOMAttr
  : DOMNode<Attr>
{
  public static DOMAttr __construct(
      Env env, string name, @Optional string value)
  {
    DOMAttr attr = getImpl(env).createAttr(name);

    if (value != null && value.length() > 0)
      attr.setNodeValue(value);

    return attr;
  }

  DOMAttr(DOMImplementation impl, Attr delegate)
  {
    super(impl, delegate);
  }

  public string getName()
  {
    return _delegate.getName();
  }

  public Element getOwnerElement()
  {
    return wrap(_delegate.getOwnerElement());
  }

  public DOMTypeInfo getSchemaTypeInfo()
  {
    return wrap(_delegate.getSchemaTypeInfo());
  }

  public bool getSpecified()
  {
    return _delegate.getSpecified();
  }

  public string getValue()
  {
    return _delegate.getValue();
  }

  public bool isId()
  {
    return _delegate.isId();
  }

  public void setValue(String value)
    
  {
    try {
      _delegate.setValue(value);
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }
}
}
