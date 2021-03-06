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









public class DOMNode<T : Node>
  : DOMWrapper<T>
{
  protected DOMNode(DOMImplementation impl, T delegate)
  {
    super(impl, delegate);
  }

  Node getDelegate()
  {
    return _delegate;
  }

  public DOMNode appendChild(DOMNode newChild)
    
  {
    try {
      return wrap(_delegate.appendChild(newChild.getDelegate()));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMNode cloneNode(bool deep)
  {
    return wrap(_delegate.cloneNode(deep));
  }

  public short compareDocumentPosition(DOMNode other)
    
  {
    try {
      return _delegate.compareDocumentPosition(other.getDelegate());
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMNamedNodeMap getAttributes()
  {
    return wrap(_delegate.getAttributes());
  }

  public string getBaseURI()
  {
    return _delegate.getBaseURI();
  }

  public DOMNodeList getChildNodes()
  {
    return wrap(_delegate.getChildNodes());
  }

  public Object getFeature(String feature, string version)
  {
    return _delegate.getFeature(feature, version);
  }

  public DOMNode getFirstChild()
  {
    return wrap(_delegate.getFirstChild());
  }

  public DOMNode getLastChild()
  {
    return wrap(_delegate.getLastChild());
  }

  public string getLocalName()
  {
    return _delegate.getLocalName();
  }

  public string getNamespaceURI()
  {
    return _delegate.getNamespaceURI();
  }

  public DOMNode getNextSibling()
  {
    return wrap(_delegate.getNextSibling());
  }

  public string getNodeName()
  {
    return _delegate.getNodeName();
  }

  public short getNodeType()
  {
    return _delegate.getNodeType();
  }

  public CharSequence getNodeValue(Env env)
    
  {
    try {
      string value = _delegate.getNodeValue();

      return convertToUtf8(env, value);
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMDocument getOwnerDocument()
  {
    return wrap(_delegate.getOwnerDocument());
  }

  public DOMNode getParentNode()
  {
    return wrap(_delegate.getParentNode());
  }

  public string getPrefix()
  {
    return _delegate.getPrefix();
  }

  public DOMNode getPreviousSibling()
  {
    return wrap(_delegate.getPreviousSibling());
  }

  public CharSequence getTextContent(Env env)
    
  {
    try {
      string value = _delegate.getTextContent();

      return convertToUtf8(env, value);
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  private CharSequence convertToUtf8(Env env, string value)
  {
    if (env.isUnicodeSemantics()) {
      return value;
    }
    else {
      int len = value.length();

      bool isUtf16 = false;

      for (int i = 0; i < len; i++) {
        char ch = value[i];

        if (0x00 <= ch && ch <= 0xff) {
        }
        else {
          isUtf16 = true;
          break;
        }
      }

      if (isUtf16) {
        // XXX: for mediawiki-1.20.2 install text,
        //      not right but will have to do until we redo DOM
        Encoder encoder = Encoder.create("utf-8");

        StringValue sb = env.createBinaryBuilder();

        encoder.encode(sb, value);

        return sb;
      }
      else {
        return value;
      }
    }
  }

  public Object getUserData(String key)
  {
    return _delegate.getUserData(key);
  }

  public bool hasAttributes()
  {
    return _delegate.hasAttributes();
  }

  public bool hasChildNodes()
  {
    return _delegate.hasChildNodes();
  }

  public DOMNode insertBefore(DOMNode newChild, DOMNode refChild)
    
  {
    try {
      return wrap(_delegate.insertBefore(
          newChild.getDelegate(), refChild.getDelegate()));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public bool isDefaultNamespace(String namespaceURI)
  {
    return _delegate.isDefaultNamespace(namespaceURI);
  }

  public bool isEqualNode(DOMNode arg)
  {
    return _delegate.isEqualNode(arg.getDelegate());
  }

  public bool isSameNode(DOMNode other)
  {
    return _delegate.isSameNode(other.getDelegate());
  }

  public bool isSupported(String feature, string version)
  {
    return _delegate.isSupported(feature, version);
  }

  public string lookupNamespaceURI(String prefix)
  {
    return _delegate.lookupNamespaceURI(prefix);
  }

  public string lookupPrefix(String namespaceURI)
  {
    return _delegate.lookupPrefix(namespaceURI);
  }

  public void normalize()
  {
    _delegate.normalize();
  }

  public DOMNode removeChild(DOMNode oldChild)
    
  {
    try {
      return wrap(_delegate.removeChild(oldChild.getDelegate()));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMNode replaceChild(DOMNode newChild, DOMNode oldChild)
    
  {
    try {
      return wrap(_delegate.replaceChild(
          newChild.getDelegate(), oldChild.getDelegate()));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public void setNodeValue(String nodeValue)
    
  {
    try {
      _delegate.setNodeValue(nodeValue);
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public void setPrefix(String prefix)
    
  {
    try {
      _delegate.setPrefix(prefix);
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public void setTextContent(String textContent)
    
  {
    try {
      _delegate.setTextContent(textContent);
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public Object setUserData(String key, Object data)
  {
    return _delegate.setUserData(key, data, null);
  }

  public string ToString()
  {
    return getClass().getSimpleName();
  }
}
}
