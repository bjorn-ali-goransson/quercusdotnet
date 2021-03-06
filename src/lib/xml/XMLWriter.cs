using System;
namespace QuercusDotNet.lib.xml {
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
 * XMLWriter
 */
public class XMLWriter {
  private const Logger log
    = Logger.getLogger(XMLWriter.class.getName());
  private readonly L10N L = new L10N(XMLWriter.class);

  private readonly XMLWriterStream NULL_STREAM = new XMLWriterStream();

  private XMLWriterStream _s = NULL_STREAM;

  private WriterState _state = WriterState.TOP;

  private HashMap<StringValue,StringValue> _nsMap
    = new HashMap<StringValue,StringValue>();

  private ArrayList<StringValue> _elementStack
    = new ArrayList<StringValue>();

  private ArrayList<ArrayList<StringValue>> _nsStack
    = new ArrayList<ArrayList<StringValue>>();

  /**
   * Default constructor
   */
  public XMLWriter()
  {
  }

  /**
   * Flushes the output and returns the result.
   */
  public Value flush()
  {
    return _s.flush();
  }

  /**
   * Opens the writer for a memory target
   */
  public bool openMemory(Env env)
  {
    StringValue s = env.createUnicodeBuilder();
    
    _s = new MemoryXMLWriterStream(s);

    _nsMap.clear();
    _elementStack.clear();
    _nsStack.clear();
    
    _state = WriterState.TOP;
    
    return true;
  }

  /**
   * Opens the writer for a uri target
   */
  public bool openURI(Env env, string path)
  {
    try {
      WriteStream @out = path.openWrite();

      _s = new PathXMLWriterStream(out);

      _nsMap.clear();
      _elementStack.clear();
      _nsStack.clear();
      
      _state = WriterState.TOP;
    } catch (IOException e) {
      log.log(Level.WARNING, e.ToString(), e);
    }
    
    return true;
  }

  /**
   * Returns the memory result
   */
  public Value outputMemory()
  {
    return flush();
  }

  /**
   * Ends an attribute
   */
  public bool endAttribute()
  {
    if (_state != WriterState.ATTRIBUTE)
      return false;
    
    _s.append("\"");
    _state = WriterState.ELEMENT_HEADER;

    return true;
  }

  /**
   * Starts a CData section
   */
  public bool endCData()
  {
    if (_state != WriterState.CDATA)
      return false;

    _state = WriterState.ELEMENT_BODY;

    _s.append("]]>");

    return true;
  }

  /**
   * Starts a comment section
   */
  public bool endComment()
  {
    if (_state != WriterState.COMMENT)
      return false;

    _state = WriterState.ELEMENT_BODY;

    _s.append("-->");

    return true;
  }

  /**
   * Ends a pi section
   */
  public bool endPI()
  {
    if (_state != WriterState.PI)
      return false;

    _state = WriterState.ELEMENT_BODY;

    _s.append("?>");

    return true;
  }

  /**
   * Ends the document
   */
  public bool endDocument()
  {
    return true;
  }

  /**
   * Ends a DTD attribute list
   */
  public bool endDTDAttlist()
  {
    return true;
  }

  /**
   * Ends a DTD element list
   */
  public bool endDTDElement()
  {
    return true;
  }

  /**
   * Ends a DTD entity
   */
  public bool endDTDEntity()
  {
    return true;
  }

  /**
   * Ends a DTD
   */
  public bool endDTD()
  {
    return true;
  }

  /**
   * Ends an element
   */
  public bool endElement(Env env)
  {
    if (_state == WriterState.ATTRIBUTE)
      endAttribute();
    
    if (_elementStack.size() == 0)
      return false;
    else if (_state == WriterState.ELEMENT_HEADER) {
      popElement();

      _s.append("/>");

      return true;
    }
    else if (_state == WriterState.ELEMENT_BODY) {
      StringValue name = popElement();

      _s.append("</").append(env, name).append(">");
      
      return true;
    }
    else
      return false;
  }

  /**
   * Ends an element
   */
  public bool endElementNS(Env env)
  {
    return endElement(env);
  }

  /**
   * Ends an element
   */
  public bool fullEndElement(Env env)
  {
    if (_state == WriterState.ATTRIBUTE)
      endAttribute();
    
    if (_elementStack.size() == 0)
      return false;
    
    if (_state == WriterState.ELEMENT_HEADER) {
      _s.append(">");
    }
    else if (_state != WriterState.ELEMENT_BODY)
      return false;
    
    StringValue name = popElement();

    _s.append("</").append(env, name).append(">");

    return true;
  }

  /**
   * enables indentation
   */
  public bool setIndent(bool isIndent)
  {
    return false;
  }

  /**
   * sets the indentation string
   */
  public bool setIndentString(StringValue value)
  {
    return false;
  }

  /**
   * Starts an attribute
   */
  public bool startAttribute(Env env, StringValue name)
  {
    if (_state != WriterState.ELEMENT_HEADER)
      return false;
    
    _s.append(" ").append(env, name).append("=\"");

    _state = WriterState.ATTRIBUTE;

    return true;
  }

  /**
   * Starts an attribute with a namespace
   */
  public bool startAttributeNS(Env env,
                                  StringValue prefix,
                                  StringValue name,
                                  StringValue uri)
  {
    if (_state != WriterState.ELEMENT_HEADER)
      return false;

    pushNamespace(env, prefix, uri);
    
    _s.append(" ").append(env, prefix).append(":").append(env,name);
    _s.append("=\"");

    _state = WriterState.ATTRIBUTE;

    return true;
  }

  /**
   * Starts a CData section
   */
  public bool startCData()
  {
    startContent();

    _state = WriterState.CDATA;

    _s.append("<![CDATA[");

    return true;
  }

  /**
   * Starts a comment section
   */
  public bool startComment()
  {
    startContent();

    _state = WriterState.COMMENT;

    _s.append("<!--");

    return true;
  }

  /**
   * Starts the document
   */
  public bool startDocument(Env env,
                               @Optional StringValue version,
                               @Optional StringValue encoding,
                               @Optional StringValue standalone)
  {
    _s.append("<?xml");
    
    if (version == null || version.length() == 0)
      _s.append(" version=\"1.0\"");
    else
      _s.append(" version=\"").append(env, version).append("\"");
    
    if (encoding != null && encoding.length() != 0)
      _s.append(" encoding=\"").append(env, encoding).append("\"");
    
    if (standalone != null && standalone.length() != 0)
      _s.append(" standalone=\"").append(env, standalone).append("\"");

    _s.append("?>\n");

    return true;
  }

  /**
   * Starts a DTD attribute list
   */
  public bool startDTDAttlist(StringValue name)
  {
    return true;
  }

  /**
   * Starts a DTD element list
   */
  public bool startDTDElement(StringValue name)
  {
    return true;
  }

  /**
   * Starts a DTD entity
   */
  public bool startDTDEntity(StringValue name)
  {
    return true;
  }

  /**
   * Starts a DTD
   */
  public bool startDTD(StringValue name,
                          @Optional StringValue publicId,
                          @Optional StringValue systemId)
  {
    return true;
  }

  /**
   * Starts an element
   */
  public bool startElement(Env env, StringValue name)
  {
    if (_state == WriterState.ELEMENT_HEADER) {
      _s.append(">");
    }

    _s.append("<").append(env, name);

    _elementStack.add(name);
    _nsStack.add(null);

    _state = WriterState.ELEMENT_HEADER;

    return true;
  }

  /**
   * Starts a namespaced element
   */
  public bool startElementNS(Env env,
                                StringValue prefix,
                                StringValue name,
                                StringValue uri)
  {
    startContent();

    _s.append("<").append(env, prefix).append(":").append(env, name);

    StringValue endName = prefix.createStringBuilder();
    endName.append(prefix).append(":").append(name);
    
    _elementStack.add(endName);
    
    _nsStack.add(null);
    pushNamespace(env, prefix, uri);

    _state = WriterState.ELEMENT_HEADER;

    return true;
  }

  /**
   * Starts a processing instruction section
   */
  public bool startPI(Env env, StringValue target)
  {
    startContent();

    _state = WriterState.PI;

    _s.append("<?").append(env, target).append(" ");

    return true;
  }

  /**
   * Writes text
   */
  public bool text(Env env, StringValue text)
  {
    if (_state == WriterState.ELEMENT_HEADER)
      startContent();

    if (_state == WriterState.ELEMENT_BODY || _state == WriterState.TOP) {
      int len = text.length();

      for (int i = 0; i < len; i++) {
        char ch = text[i];

        switch (ch) {
        case '<':
          _s.append("&lt;");
          break;
        case '>':
          _s.append("&gt;");
          break;
        case '&':
          _s.append("&amp;");
          break;
        case '"':
          _s.append("&quot;");
          break;
        default:
          _s.append(ch);
          break;
        }
      }
    }
    else
      _s.append(env, text);

    return true;
  }

  /**
   * Writes a complete attribute
   */
  public bool writeAttribute(Env env, StringValue name,
                                StringValue value)
  {
    startAttribute(env, name);
    text(env, value);
    endAttribute();

    return true;
  }

  /**
   * Writes a complete attribute
   */
  public bool writeAttributeNS(Env env,
                                  StringValue prefix,
                                  StringValue name,
                                  StringValue uri,
                                  StringValue value)
  {
    startAttributeNS(env, prefix, name, uri);
    text(env, value);
    endAttribute();

    return true;
  }

  /**
   * Writes a complete cdata
   */
  public bool writeCData(Env env, StringValue value)
  {
    startCData();
    text(env, value);
    endCData();

    return true;
  }

  /**
   * Writes a complete comment
   */
  public bool writeComment(Env env, StringValue value)
  {
    startComment();
    text(env, value);
    endComment();

    return true;
  }

  /**
   * Writes a DTD attribute list
   */
  public bool writeDTDAttlist(Env env,
                                 StringValue name,
                                 StringValue content)
  {
    startDTDAttlist(name);
    text(env, content);
    endDTDAttlist();

    return true;
  }

  /**
   * Writes a DTD element
   */
  public bool writeDTDElement(Env env,
                                 StringValue name,
                                 StringValue content)
  {
    startDTDElement(name);
    text(env, content);
    endDTDElement();

    return true;
  }

  /**
   * Writes a DTD entity
   */
  public bool writeDTDEntity(Env env,
                                StringValue name,
                                StringValue content)
  {
    startDTDEntity(name);
    text(env, content);
    endDTDEntity();

    return true;
  }

  /**
   * Writes a DTD
   */
  public bool writeDTD(Env env,
                          StringValue name,
                          @Optional StringValue publicId,
                          @Optional StringValue systemId,
                          @Optional StringValue subset)
  {
    startDTD(name, publicId, systemId);
    text(env, subset);
    endDTDEntity();

    return true;
  }

  /**
   * Writes a complete element
   */
  public bool writeElement(Env env,
                              StringValue name,
                              @Optional StringValue content)
  {
    startElement(env, name);

    if (content != null && content.length() > 0)
      text(env, content);
    
    endElement(env);

    return true;
  }

  /**
   * Writes a complete element
   */
  public bool writeElementNS(Env env,
                                StringValue prefix,
                                StringValue name,
                                StringValue uri,
                                @Optional StringValue content)
  {
    startElementNS(env, prefix, name, uri);

    if (content != null && content.length() > 0)
      text(env, content);

    endElement(env);

    return true;
  }

  /**
   * Writes a pi
   */
  public bool writePI(Env env, StringValue name, StringValue value)
  {
    startPI(env, name);
    text(env, value);
    endPI();

    return true;
  }

  /**
   * Writes raw text
   */
  public bool writeRaw(Env env, StringValue value)
  {
    _s.append(env, value);

    return true;
  }

  private void startContent()
  {
    if (_state == WriterState.ATTRIBUTE) {
      _s.append("\">");
      
      _state = WriterState.ELEMENT_BODY;
    }
    else if (_state == WriterState.ELEMENT_HEADER) {
      _s.append(">");

      _state = WriterState.ELEMENT_BODY;
    }
  }

  private void pushNamespace(Env env, StringValue prefix, StringValue uri)
  {
    if (! uri.equals(_nsMap.get(prefix))) {
      _s.append(" ");
      
      if (prefix.length() == 0)
        _s.append("xmlns");
      else
        _s.append("xmlns:").append(env, prefix);

      _s.append("=\"").append(env, uri).append("\"");

      ArrayList<StringValue> stack = _nsStack.get(_nsStack.size() - 1);
      
      if (stack == null) {
        stack = new ArrayList<StringValue>();
        _nsStack.set(_nsStack.size() - 1, stack);
      }

      stack.add(prefix);

      _nsMap.put(prefix, uri);
    }
  }

  private StringValue popElement()
  {
    if (_elementStack.size() > 1)
      _state = WriterState.ELEMENT_BODY;
    else
      _state = WriterState.TOP;
    
    if (_elementStack.size() > 0) {
      StringValue name = _elementStack.remove(_elementStack.size() - 1);
      
      ArrayList<StringValue> prefixList = _nsStack.remove(_nsStack.size() - 1);

      if (prefixList != null) {
        for (StringValue prefix : prefixList) {
          _nsMap.remove(prefix);
        }
      }

      return name;
    }

    return null;
  }

  public override string ToString()
  {
    return getClass().getSimpleName() + "[]";
  }

  static enum WriterState {
    TOP,
    ELEMENT_HEADER,
    ATTRIBUTE,
    COMMENT,
    CDATA,
    ELEMENT_BODY,
    PI;
  }
      

  static class XMLWriterStream {
    XMLWriterStream append(char v) { return this; }
    
    XMLWriterStream append(Env env, StringValue v) { return this; }
    
    XMLWriterStream append(String v) { return this; }

    Value flush() { return NullValue.NULL; }
  }

  static class MemoryXMLWriterStream : XMLWriterStream {
    private StringValue _v;

    MemoryXMLWriterStream(StringValue v)
    {
      _v = v;
    }
    
    XMLWriterStream override append(char v)
    {
      _v.append(v);

      return this;
    }
    
    XMLWriterStream override append(Env env, StringValue v)
    {
      _v.append(v);

      return this;
    }
    
    XMLWriterStream override append(String text)
    {
      _v.append(text);

      return this;
    }

    Value override flush()
    {
      return _v;
    }
  }

  static class PathXMLWriterStream : XMLWriterStream {
    private WriteStream _out;

    PathXMLWriterStream(WriteStream out)
    {
      _out = out;
    }
    
    XMLWriterStream override append(char v)
    {
      try {
        _out.print(v);
      } catch (IOException e) {
        log.log(Level.WARNING, e.ToString(), e);
      }

      return this;
    }

    XMLWriterStream override append(Env env, StringValue v)
    {
      v.print(env, _out);

      return this;
    }
    
    XMLWriterStream override append(String text)
    {
      try {
        _out.print(text);
      } catch (IOException e) {
        log.log(Level.WARNING, e.ToString(), e);
      }

      return this;
    }

    Value override flush()
    {
      try {
        _out.close();
      } catch (IOException e) {
        log.log(Level.WARNING, e.ToString(), e);
      }
      
      return LongValue.create(1);
    }
  }
}
}
