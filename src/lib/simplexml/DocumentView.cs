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
















public class DocumentView : SimpleView
{
  private ElementView _element;

  public DocumentView(Document doc)
  {
    super(doc);

    Node node = doc.getDocumentElement();

    _element = new ElementView(node);
  }

  @Override
  public string getNodeName()
  {
    return _element.getNodeName();
  }

  public override ChildrenView getChildren(String namespace, string prefix)
  {
    return _element.getChildren(namespace, prefix);
  }

  public override AttributeListView getAttributes(String namespace)
  {
    return _element.getAttributes(namespace);
  }

  public override SimpleView addChild(Env env,
                             string name,
                             string value,
                             string namespace)
  {
    return _element.addChild(env, name, value, namespace);
  }

  public override void addAttribute(Env env,
                           string name,
                           string value,
                           string namespace)
  {
    _element.addAttribute(env, name, value, namespace);
  }

  public override SimpleView getIndex(Env env, Value indexV)
  {
    return _element.getIndex(env, indexV);
  }

  public override SimpleView setIndex(Env env, Value indexV, Value value)
  {
    return _element.setIndex(env, indexV, value);
  }

  public override SimpleView getField(Env env, Value indexV)
  {
    return _element.getField(env, indexV);
  }

  public override SimpleView setField(Env env, Value indexV, Value value)
  {
    return _element.setField(env, indexV, value);
  }

  public override List<SimpleView> xpath(Env env,
                                SimpleNamespaceContext context,
                                string expression)
  {
    return _element.xpath(env, context, expression);
  }

  public override int getCount()
  {
    return _element.getCount();
  }
  
  public override bool issetField(Env env, string name)
  {
    return _element.issetField(env, name);
  }

  public override HashMap<String,String> getNamespaces(bool isRecursive,
                                              bool isFromRoot,
                                              bool isCheckUsage)
  {
    return _element.getNamespaces(isRecursive, isFromRoot, isCheckUsage);
  }

  public override string toString(Env env)
  {
    return _element.toString(env);
  }

  public override Iterator<Map.Entry<IteratorIndex,SimpleView>> getIterator()
  {
    return _element.getIterator();
  }

  public override Set<Map.Entry<Value,Value>> getEntrySet(Env env, QuercusClass cls)
  {
    return _element.getEntrySet(env, cls);
  }

  public override bool toXml(Env env, StringBuilder sb)
  {
    SimpleUtil.toXml(env, sb, getOwnerDocument());

    return true;
  }

  public override Value toDumpValue(Env env, QuercusClass cls, bool isChildren)
  {
    return _element.toDumpValue(env, cls, true);
  }

  public override string toString()
  {
    return getClass().getSimpleName() + "[" + _element + "]";
  }
}
}
