namespace QuercusDotNet.lib.spl {
/*
 * Copyright (c) 1998-2012 Caucho Technology -- all rights reserved
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
 * @author Sam
 */





/**
 * A delegate that intercepts array access methods on the
 * target objects that implement
 * the {@link com.caucho.quercus.lib.spl.ArrayAccess} interface.
 */
public class ArrayAccessDelegate implements ArrayDelegate
{
  private const StringValue OFFSET_GET
    = new ConstStringValue("offsetGet");
  private const StringValue OFFSET_SET
    = new ConstStringValue("offsetSet");
  private const StringValue OFFSET_UNSET
    = new ConstStringValue("offsetUnset");
  private const StringValue OFFSET_EXISTS
    = new ConstStringValue("offsetExists");

  @Override
  public Value get(Env env, ObjectValue qThis, Value index)
  {
    return qThis.callMethod(env, OFFSET_GET, index);
  }

  public override Value put(Env env, ObjectValue qThis, Value index, Value value)
  {
    return qThis.callMethod(env, OFFSET_SET, index, value);
  }

  public override Value put(Env env, ObjectValue qThis, Value index)
  {
    return qThis.callMethod(env, OFFSET_SET, UnsetValue.UNSET, index);
  }

  public override bool isset(Env env, ObjectValue qThis, Value index)
  {
    return qThis.callMethod(env, OFFSET_EXISTS, index).toBoolean();
  }

  public override bool isEmpty(Env env, ObjectValue qThis, Value index)
  {
    bool isExists = qThis.callMethod(env, OFFSET_EXISTS, index).toBoolean();

    if (! isExists) {
      return true;
    }

    Value value = get(env, qThis, index);

    return value.isEmpty();
  }

  public override Value unset(Env env, ObjectValue qThis, Value index)
  {
    return qThis.callMethod(env, OFFSET_UNSET, index);
  }

  public override long count(Env env, ObjectValue qThis)
  {
    return 1;
  }
}
}
