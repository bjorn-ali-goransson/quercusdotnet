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



















public class ChildrenView : SimpleView
{
  private SimpleView _parent;

  private ArrayList<SimpleView> _childList;
  private ArrayList<AttributeView> _attrList;

  public ChildrenView(SimpleView parent,
                      ArrayList<SimpleView> childList,
                      ArrayList<AttributeView> attrList)
  {
    super(parent.getOwnerDocument());

    _parent = parent;

    _childList = childList;
    _attrList = attrList;
  }

  public override string getNodeName()
  {
    if (_childList.size() > 0) {
      return _childList.get(0).getNodeName();
    }
    else {
      return null;
    }
  }

  public override ChildrenView getChildren(String namespace, string prefix)
  {
    if (_childList.size() > 0) {
      return _childList.get(0).getChildren(namespace, prefix);
    }
    else {
      return null;
    }
  }

  public override AttributeListView getAttributes(String namespace)
  {
    if (_childList.size() > 0) {
      return _childList.get(0).getAttributes(namespace);
    }
    else {
      return null;
    }
  }

  public override SimpleView addChild(Env env,
                             string name,
                             string value,
                             string namespace)
  {
    if (_childList.size() > 0) {
      return _childList.get(0).addChild(env, name, value, namespace);
    }
    else {
      return null;
    }
  }

  public override HashMap<String,String> getNamespaces(bool isRecursive,
                                              bool isFromRoot,
                                              bool isCheckUsage)
  {
    if (_childList.size() > 0) {
      return _childList.get(0).getNamespaces(isRecursive, isFromRoot, isCheckUsage);
    }
    else {
      return null;
    }
  }

  public override SimpleView getIndex(Env env, Value indexV)
  {
    if (indexV.isString()) {
      if (_childList.size() > 0) {
        return _childList.get(0).getIndex(env, indexV);
      }
      else {
        return null;
      }
    }
    else {
      int index = indexV.toInt();

      if (index < _childList.size()) {
        return _childList.get(index);
      }
      else {
        return null;
      }
    }
  }

  public override SimpleView setIndex(Env env, Value indexV, Value value)
  {
    throw new UnsupportedOperationException();
  }

  public override SimpleView getField(Env env, Value indexV)
  {
    string nodeName = indexV.ToStringValue(env).ToString();

    ArrayList<SimpleView> childList = new ArrayList<SimpleView>();

    for (SimpleView view : _childList) {
      string childName = view.getNodeName();

      if (nodeName.equals(childName)) {
        childList.add(view);
      }
    }

    ArrayList<AttributeView> attrList = new ArrayList<AttributeView>();

    SelectedView view
      = new SelectedView(this, nodeName, childList, attrList);

    return view;
  }

  public override SimpleView setField(Env env, Value indexV, Value value)
  {
    if (_childList.size() > 0) {
      SimpleView firstChild = _childList.get(0);

      return firstChild.setField(env, indexV, value);
    }
    else {
      return null;
    }
  }

  public override int getCount()
  {
    return _childList.size();
  }
  
  public override bool issetField(Env env, string name)
  {
    for (SimpleView child : _childList) {
      if (child.getNodeName().equals(name)) {
        return true;
      }
    }
    
    return false;
  }

  public override List<SimpleView> xpath(Env env,
                                SimpleNamespaceContext context,
                                string expression)
  {
    if (_childList.size() > 0) {
      SimpleView firstChild = _childList.get(0);

      return firstChild.xpath(env, context, expression);
    }
    else {
      return null;
    }
  }

  public override string ToString(Env env)
  {
    if (_childList.size() > 0) {
      SimpleView firstChild = _childList.get(0);

      return firstChild.ToString(env);
    }
    else {
      return "";
    }
  }

  public override Iterator<Map.Entry<IteratorIndex,SimpleView>> getIterator()
  {
    LinkedHashMap<IteratorIndex,SimpleView> map
      = new LinkedHashMap<IteratorIndex,SimpleView>();

    for (int i = 0; i < _childList.size(); i++) {
      SimpleView view = _childList.get(i);

      map.put(IteratorIndex.create(view.getNodeName()), view);
    }

    return map.entrySet().iterator();
  }

  public override Set<Map.Entry<Value,Value>> getEntrySet(Env env, QuercusClass cls)
  {
    throw new UnsupportedOperationException();
  }

  public override bool toXml(Env env, StringBuilder sb)
  {
    if (_childList.size() > 0) {
      SimpleView firstChild = _childList.get(0);

      firstChild.toXml(env, sb);

      return true;
    }
    else {
      return false;
    }
  }

  public override Value toDumpValue(Env env, QuercusClass cls, bool isChildren)
  {
    int childSize = _childList.size();

    ObjectValue obj = env.createObject();
    obj.setClassName(cls.getName());

    if (childSize > 0) {
      for (int i = 0; i < childSize; i++) {
        SimpleView child = _childList.get(i);

        Value childValue = child.toDumpValue(env, cls, false);

        StringValue nodeName = env.createString(child.getNodeName());

        obj.putField(env, nodeName, childValue);

        /*
        Value existing = obj.getField(env, nodeName);

        if (existing == UnsetValue.UNSET) {
          obj.putField(env, nodeName, childValue);
        }
        else if (existing.isArray()) {
          existing.toArrayValue(env).append(childValue);
        }
        else {
          ArrayValue array = new ArrayValueImpl();

          array.append(existing);
          array.append(childValue);

          obj.putField(env, nodeName, array);
        }
        */
      }
    }
    else if (_attrList.size() > 0) {
      ArrayValue array = new ArrayValueImpl();

      for (AttributeView view : _attrList) {
        StringValue attrName = env.createString(view.getNodeName());
        StringValue attrValue = env.createString(view.getNodeValue());

        array.append(attrName, attrValue);
      }

      obj.putField(env, env.createString("@attributes"), array);
    }

    return obj;
  }

  public override string ToString()
  {
    int size = _childList.size();
    SimpleView firstChild = null;

    if (size > 0) {
      firstChild = _childList.get(0);
    }

    return getClass().getSimpleName() + "[size=" + size + ",first=" + firstChild + ",parent=" + _parent + "]";
  }
}
}
