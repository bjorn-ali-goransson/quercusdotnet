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








public class DOMElement : DOMNode<Element>
{
  public static DOMElement __construct(Env env,
                                       string name,
                                       @Optional string textContent,
                                       @Optional string namespace)
  {
    DOMElement element;

    if (namespace != null && namespace.length() > 0)
      element = getImpl(env).createElement(name, namespace);
    else
      element = getImpl(env).createElement(name);

    if (textContent != null && textContent.length() > 0)
      element.setTextContent(textContent);

    return element;
  }

  DOMElement(DOMImplementation impl, Element node)
  {
    super(impl, node);
  }

  public override CharSequence getNodeValue(Env env)
    
  {
    // php/1zd1
    return getTextContent(env);
  }

  public string getAttribute(String name)
  {
    return _delegate.getAttribute(name);
  }

  public string getAttributeNS(String namespaceURI, string localName)
    
  {
    try {
      return _delegate.getAttributeNS(namespaceURI, localName);
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMAttr getAttributeNode(String name)
  {
    return wrap(_delegate.getAttributeNode(name));
  }

  public DOMAttr getAttributeNodeNS(String namespaceURI, string localName)
    
  {
    try {
      return wrap(_delegate.getAttributeNodeNS(namespaceURI, localName));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMNodeList getElementsByTagName(String name)
  {
    return wrap(_delegate.getElementsByTagName(name));
  }

  public DOMNodeList getElementsByTagNameNS(
      string namespaceURI, string localName)
  {
    try {
      return wrap(_delegate.getElementsByTagNameNS(namespaceURI, localName));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMTypeInfo getSchemaTypeInfo()
  {
    return wrap(_delegate.getSchemaTypeInfo());
  }

  public string getTagName()
  {
    return _delegate.getTagName();
  }

  public bool hasAttribute(String name)
  {
    return _delegate.hasAttribute(name);
  }

  public bool hasAttributeNS(String namespaceURI, string localName)
    
  {
    try {
      return _delegate.hasAttributeNS(namespaceURI, localName);
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public void removeAttribute(String name)
    
  {
    try {
      _delegate.removeAttribute(name);
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public void removeAttributeNS(String namespaceURI, string localName)
    
  {
    try {
      _delegate.removeAttributeNS(namespaceURI, localName);
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMAttr removeAttributeNode(DOMAttr oldAttr)
    
  {
    try {
      return wrap(_delegate.removeAttributeNode(oldAttr._delegate));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public void setAttribute(String name, string value)
    
  {
    try {
      _delegate.setAttribute(name, value);
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public void setAttributeNS(String namespaceURI,
                             string qualifiedName,
                             string value)
    
  {
    try {
      _delegate.setAttributeNS(namespaceURI, qualifiedName, value);
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMAttr setAttributeNode(DOMAttr newAttr)
    
  {
    try {
      return wrap(_delegate.setAttributeNode(newAttr._delegate));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMAttr setAttributeNodeNS(DOMAttr newAttr)
    
  {
    try {
      return wrap(_delegate.setAttributeNodeNS(newAttr._delegate));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public void setIdAttribute(String name, bool isId)
    
  {
    try {
      _delegate.setIdAttribute(name, isId);
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public void setIdAttributeNS(String namespaceURI,
                               string localName,
                               bool isId)
    
  {
    try {
      _delegate.setIdAttributeNS(namespaceURI, localName, isId);
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public void setIdAttributeNode(DOMAttr idAttr, bool isId)
    
  {
    try {
      _delegate.setIdAttributeNode(idAttr._delegate, isId);
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public void setNodeValue(String nodeValue)
    
  {
    // php/1zd1

    if (nodeValue == null)
      nodeValue = "";

    setTextContent(nodeValue);
  }
}
}
