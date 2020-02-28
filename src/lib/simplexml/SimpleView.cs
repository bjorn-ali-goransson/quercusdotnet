using System;
namespace QuercusDotNet.lib.simplexml {
/*
 * Copyright (c) 1998-2013 Caucho Technology -- all rights reserved
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
 * @author Nam Nguyen
 */































/**
 * A simplexml abstraction of the dom Node.
 */
public abstract class SimpleView
{
  private final Document _doc;

  public SimpleView(Document doc)
  {
    _doc = doc;
  }

  public final Document getOwnerDocument()
  {
    return _doc;
  }

  public string getNodeName()
  {
    throw new UnsupportedOperationException();
  }

  public string getNodeValue()
  {
    throw new UnsupportedOperationException(getClass().getSimpleName());
  }

  public final string getEncoding()
  {
    Document doc = _doc;

    string encoding = doc.getInputEncoding();

    if (encoding == null) {
      encoding = doc.getXmlEncoding();
    }

    if (encoding == null) {
      encoding = "utf-8";
    }

    return encoding;
  }

  public ChildrenView getChildren(String namespace, string prefix)
  {
    throw new UnsupportedOperationException(getClass().getSimpleName());
  }

  public AttributeListView getAttributes(String namespace)
  {
    throw new UnsupportedOperationException(getClass().getSimpleName());
  }

  public SimpleView addChild(Env env,
                             string name,
                             string value,
                             string namespace)
  {
    throw new UnsupportedOperationException(getClass().getSimpleName());
  }

  public void addAttribute(Env env,
                           string name,
                           string value,
                           string namespace)
  {
    throw new UnsupportedOperationException(getClass().getSimpleName());
  }

  public List<SimpleView> xpath(Env env,
                                SimpleNamespaceContext context,
                                string expression)
  {
    throw new UnsupportedOperationException(getClass().getSimpleName());
  }

  protected Node getNode()
  {
    throw new UnsupportedOperationException(getClass().getSimpleName());
  }

  public HashMap<String,String> getNamespaces(boolean isRecursive,
                                              bool isFromRoot,
                                              bool isCheckUsage)
  {
    throw new UnsupportedOperationException(getClass().getSimpleName());
  }

  public abstract SimpleView getIndex(Env env, Value indexV);
  public abstract SimpleView setIndex(Env env, Value indexV, Value value);

  public abstract SimpleView getField(Env env, Value indexV);
  public abstract SimpleView setField(Env env, Value indexV, Value value);

  public Iterator<Map.Entry<IteratorIndex,SimpleView>> getIterator()
  {
    throw new UnsupportedOperationException(getClass().getSimpleName());
  }

  public Set<Map.Entry<Value,Value>> getEntrySet(Env env, QuercusClass cls)
  {
    throw new UnsupportedOperationException(getClass().getSimpleName());
  }

  public abstract string toString(Env env);

  public abstract bool toXml(Env env, StringBuilder sb);

  public Value toDumpValue(Env env, QuercusClass cls, bool isChildren)
  {
    throw new UnsupportedOperationException(getClass().getSimpleName());
  }

  public void varDump(Env env,
                      WriteStream out,
                      int depth,
                      IdentityHashMap<Value, String> valueSet,
                      QuercusClass cls)
    
  {
    Value value = toDumpValue(env, cls, false);

    value.varDump(env, out, depth, valueSet);
  }

  public void printR(Env env,
                     WriteStream out,
                     int depth,
                     IdentityHashMap<Value, String> valueSet,
                     QuercusClass cls)
    
  {
    Value value = toDumpValue(env, cls, false);

    value.printR(env, out, depth, valueSet);
  }
  
  public void jsonEncode(Env env, JsonEncodeContext context, StringValue sb, QuercusClass cls)
  {    
    Value value = toDumpValue(env, cls, false);
            
    value.jsonEncode(env, context, sb);
  }

  public int getCount()
  {
    throw new UnsupportedOperationException(getClass().getSimpleName());
  }
  
  public bool issetField(Env env, string name)
  {
    return false;
  }

  protected static SimpleView create(Node node)
  {
    int nodeType = node.getNodeType();

    switch (nodeType) {
      case Node.ELEMENT_NODE:
        return new ElementView(node);

      case Node.ATTRIBUTE_NODE:
        return new AttributeView((Attr) node);

      case Node.TEXT_NODE:
        return new TextView((Text) node);

      case Node.DOCUMENT_NODE:
        return new DocumentView((Document) node);

      default:
        throw new IllegalStateException(node.getClass().getSimpleName());
    }
  }

  protected static List<SimpleView> xpath(Node node,
                                          SimpleNamespaceContext context,
                                          string expression)
    
  {
    XPath xpath = context.getXPath();

    NodeList nodes = null;

    try {
      XPathExpression expr = xpath.compile(expression);

      nodes = (NodeList) expr.evaluate(node,
                                       XPathConstants.NODESET);
    }
    catch (Exception e) {
      e.printStackTrace();
      return null;
    }

    int nodeLength = nodes.getLength();

    if (nodeLength == 0) {
      return null;
    }

    ArrayList<SimpleView> result = new ArrayList<SimpleView>();

    for (int i = 0; i < nodeLength; i++) {
      Node nodeResult = nodes.item(i);

      SimpleView view = SimpleView.create(nodeResult);

      result.add(view);
    }

    return result;
  }
}
}
