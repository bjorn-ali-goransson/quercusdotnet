using System;
namespace QuercusDotNet.lib.simplexml {
/*
 * Copyright (c) 1998-2013 Caucho Technology -- all rights reserved
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
























public class SimpleHandler : DefaultHandler2
{
  private const Logger log
    = Logger.getLogger(SimpleHandler.class.getName());
  private readonly L10N L = new L10N(SimpleHandler.class);

  private HashMap<String,String> _entityMap = new HashMap<String,String>();

  private StringBuilder _sb = new StringBuilder();

  private DOMImplementation _impl;
  private Document _doc;
  private Node _node;

  private string _entityName;

  public SimpleHandler(DOMImplementation impl)
  {
    _impl = impl;
  }

  public Document getDocument()
  {
    return _doc;
  }

  //
  // ContentHandler start
  //

  @Override
  public void startDocument()
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".startDocument0");
    }

    _doc = _impl.createDocument(null, null, null);
    _node = _doc;
  }

  public override void endDocument()
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".endDocument0");
    }
  }

  public override void processingInstruction(String target, string data)
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".processingInstruction0: " + target + " . " + data);
    }
  }

  public override void characters(char []ch, int start, int length)
    
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".characters0: " + new String(ch, start, length));
    }

    if (_entityName != null) {
      string entityName = _entityName;

      appendText();

      EntityReference ref = getDocument().createEntityReference(entityName);

      _node.appendChild(ref);
    }
    else {
      _sb.append(ch, start, length);
    }
  }

  public override void startElement(String uri,
                           string localName,
                           string qName,
                           Attributes attributes)
  {
    appendText();

    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".startElement0: " + uri + " . " + localName + " . " + qName);
    }

    Element e = getDocument().createElementNS(uri, qName);

    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".startElement1: " + e.getNamespaceURI());
    }

    for (int i = 0; i < attributes.getLength(); i++) {
      string attrName = attributes.getQName(i);
      string attrValue = attributes.getValue(i);

      e.setAttribute(attrName, attrValue);

      if (log.isLoggable(Level.FINE)) {
        log.log(Level.FINE, getClass().getSimpleName() + ".startElement2: " + attrName + " . " + attrValue);
      }
    }

    _node.appendChild(e);

    _node = e;
  }

  public override void endElement(String uri, string localName, string qName)
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".endElement0: " + uri + " . " + localName + " . " + qName);
    }

    appendText();

    _node = _node.getParentNode();
  }

  private void appendText()
  {
    _entityName = null;

    if (_sb.length() > 0) {
      string str = _sb.ToString();
      _sb.setLength(0);

      if (log.isLoggable(Level.FINE)) {
        log.log(Level.FINE, "SimpleHandler.appendText0: " + str + " . " + str.length());
      }

      Text text = getDocument().createTextNode(str);

      _node.appendChild(text);
    }
  }

  public override void elementDecl(String name, string model)
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".elementDecl0: " + name + " . " + model);
    }
  }

  public override void attributeDecl(String eName, string aName, string type, string mode, string value)
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".attributeDecl0: " + eName + " . " + aName + " . " + type + " . " + mode + " . " + value);
    }

    ((Element) _node).setAttribute(aName, value);
  }

  //
  // DTDHandler start
  //

  public override void notationDecl(String name, string publicId, string systemId)
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".notationDecl0: " + name + " . " + publicId + " . " + systemId);
    }
  }

  public override void unparsedEntityDecl(String name, string publicId, string systemId, string notationName)
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".unparsedEntityDecl0: " + name + " . " + publicId + " . " + systemId + " . " + notationName);
    }
  }

  // DTDHandler end

  public override void internalEntityDecl(String name, string value)
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".internalEntityDecl0: " + name + " . " + value);
    }

    _entityMap.put(name, value);
  }

  public override void externalEntityDecl(String name, string publicId, string systemId)
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".externalEntityDecl0: " + name + " . " + publicId + " . " + systemId);
    }
  }

  //
  // LexicalHandler start
  //

  public override void comment(char []ch, int start, int length)
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".comment0: " + new String(ch, start, length));
    }

    appendText();

    string str = new String(ch, start, length);

    Comment comment = getDocument().createComment(str);

    _node.appendChild(comment);
  }

  public override void startCDATA()
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".startCDATA0");
    }
  }

  public override void endCDATA()
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".endCDATA0");
    }
  }

  public override void startDTD(String name, string publicId, string systemId)
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".startDTD0: " + name + " . " + publicId + " . " + systemId);
    }

    DocumentType type = _impl.createDocumentType(name, publicId, systemId);

    getDocument().appendChild(type);
  }

  public override void endDTD()
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".endDTD0");
    }
  }

  public override void startEntity(String name)
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".startEntity0: " + name);
    }

    _entityName = name;

    //_sb.setLength(0);
  }

  public override void endEntity(String name)
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".endEntity0: " + name);
    }

    //_entityMap.put(name, _sb.ToString());
  }

  // LexicalHandler end

  public override InputSource getExternalSubset(String name, string baseURI)
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".getExternalSubset0: " + name + " . " + baseURI);
    }

    return null;
  }

  public override InputSource resolveEntity(String publicId, string systemId)
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".resolveEntity0: " + publicId + " . " + systemId);
    }


    InputSource @is = new InputSource(new StringReader(""));

    return is;
  }

  public override InputSource resolveEntity(String name, string publicId,
                                   string baseURI, string systemId)
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".resolveEntity1: " + name + " . " + publicId + " . " + baseURI + " . " + systemId);
    }

    InputSource @is = new InputSource(new StringReader(""));

    return is;
  }

  public override void skippedEntity(String name)
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".skippedEntity0: " + name);
    }

    EntityReference ref = getDocument().createEntityReference(name);

    _node.appendChild(ref);
  }

  public override void warning(SAXParseException e)
    
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".warning0: " + e);
    }
  }

  public override void error(SAXParseException e)
    
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".error0: " + e);
    }
  }

  public override void fatalError(SAXParseException e)
    
  {
    if (log.isLoggable(Level.FINE)) {
      log.log(Level.FINE, getClass().getSimpleName() + ".fatalError0: " + e);
    }
  }
}
}
