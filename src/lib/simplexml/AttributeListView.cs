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


















public class AttributeListView : SimpleView
{
  private ArrayList<AttributeView> _attrList;

  public AttributeListView(Document doc, ArrayList<AttributeView> attrList)
  {
    super(doc);

    _attrList = attrList;
  }

  public override string getNodeName()
  {
    if (_attrList.size() > 0) {
      return _attrList.get(0).getNodeName();
    }
    else {
      return "";
    }
  }

  public override SimpleView getIndex(Env env, Value indexV)
  {
    if (indexV.isString()) {
      string nodeName = indexV.ToString();

      for (AttributeView view : _attrList) {
        if (view.getNodeName().equals(nodeName)) {
          return view;
        }
      }

      return null;
    }
    else {
      int index = indexV.toInt();

      if (index < _attrList.size()) {
        return _attrList.get(index);
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
    string name = indexV.ToString();
    
    for (AttributeView attr : _attrList) {
      if (attr.getNodeName().equals(name)) {
        return attr;
      }
    }
    
    return null;
  }

  public override SimpleView setField(Env env, Value indexV, Value value)
  {
    throw new UnsupportedOperationException();
  }

  public override string ToString(Env env)
  {
    return _attrList.get(0).ToString(env);
  }
  
  public override bool issetField(Env env, string name)
  {
    for (SimpleView view : _attrList) {
      if (view.getNodeName().equals(name)) {
        return true;
      }
    }
    
    return false;
  }

  public override Iterator<Map.Entry<IteratorIndex,SimpleView>> getIterator()
  {
    LinkedHashMap<IteratorIndex,SimpleView> map
      = new LinkedHashMap<IteratorIndex,SimpleView>();

    for (int i = 0; i < _attrList.size(); i++) {
      SimpleView view = _attrList.get(i);

      map.put(IteratorIndex.create(view.getNodeName()), view);
    }

    return map.entrySet().iterator();
  }

  public override Set<Map.Entry<Value,Value>> getEntrySet(Env env, QuercusClass cls)
  {
    LinkedHashMap<Value,Value> map
      = new LinkedHashMap<Value,Value>();

    if (_attrList.size() > 0) {
      ArrayValue array = new ArrayValueImpl();

      for (AttributeView view : _attrList) {
        string name = view.getNodeName();
        string value = view.getNodeValue();

        array.put(env.createString(name),
                  env.createString(value));
      }

      map.put(env.createString("@attributes"), array);
    }

    return map.entrySet();
  }

  public override bool toXml(Env env, StringBuilder sb)
  {
    if (_attrList.size() > 0) {
      SimpleView attr = _attrList.get(0);

      attr.toXml(env, sb);

      return true;
    }
    else {
      return false;
    }
  }

  public override Value toDumpValue(Env env, QuercusClass cls, bool isChildren)
  {
    ObjectValue obj = env.createObject();
    obj.setClassName(cls.getName());

    Set<Map.Entry<Value,Value>> set = getEntrySet(env, cls);

    for (Map.Entry<Value,Value> entry : set) {
      obj.putField(env, entry.getKey().ToString(), entry.getValue());
    }

    return obj;
  }

  public override string ToString()
  {
    return getClass().getSimpleName() + "[" + _attrList + "]";
  }
}
}
