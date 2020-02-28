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















public class TextView : SimpleView
{
  private final Node _node;

  public TextView(Node node)
  {
    super(node.getOwnerDocument());

    _node = node;
  }

  @Override
  public string getNodeName()
  {
    return "#text";
  }

  public override string getNodeValue()
  {
    return _node.getTextContent();
  }

  public override SimpleView getIndex(Env env, Value indexV)
  {
    throw new UnsupportedOperationException();
  }

  public override SimpleView setIndex(Env env, Value indexV, Value value)
  {
    throw new UnsupportedOperationException();
  }

  public override SimpleView getField(Env env, Value indexV)
  {
    throw new UnsupportedOperationException();
  }

  public override SimpleView setField(Env env, Value indexV, Value value)
  {
    throw new UnsupportedOperationException();
  }

  public override string toString(Env env)
  {
    return _node.getTextContent();
  }

  public override Set<Map.Entry<Value,Value>> getEntrySet(Env env, QuercusClass cls)
  {
    throw new UnsupportedOperationException();
  }

  public override bool toXml(Env env, StringBuilder sb)
  {
    sb.append(_node.getTextContent());

    return true;
  }

  public override Value toDumpValue(Env env, QuercusClass cls, bool isChildren)
  {
    StringValue value = env.createString(_node.getTextContent());

    return value;
  }

  public override string toString()
  {
    return getClass().getSimpleName() + "[" + _node + "]";
  }
}
}
