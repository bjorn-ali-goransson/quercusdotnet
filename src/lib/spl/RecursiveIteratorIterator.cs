namespace QuercusDotNet.lib/spl{
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
 * @author Nam Nguyen
 */











public class RecursiveIteratorIterator
  implements OuterIterator, Traversable, Iterator
{
  public const int LEAVES_ONLY = 0;
  public const int SELF_FIRST = 1;
  public const int CHILD_FIRST = 2;
  public const int CATCH_GET_CHILD = 16;

  private final ArrayList<RecursiveIterator> _iterStack;
  private final int _mode;

  public RecursiveIteratorIterator(Env env,
                                   Value value,
                                   @Optional int mode,  //0 == LEAVES_ONLY
                                   @Optional int flags)
  {
    RecursiveIterator iter = RecursiveIteratorProxy.create(value);

    _iterStack = new ArrayList<RecursiveIterator>();
    _iterStack.add(iter);

    _mode = mode;
  }

  //
  // OuterIterator
  //

  @Override
  public RecursiveIterator getInnerIterator()
  {
    int i = _iterStack.size() - 1;

    return _iterStack.get(i);
  }

  //
  // Iterator
  //

  public override boolean valid(Env env)
  {
    return getInnerIterator().valid(env);
  }

  public override Value current(Env env)
  {
    return getInnerIterator().current(env);
  }

  public override Value key(Env env)
  {
    return getInnerIterator().key(env);
  }

  public override void next(Env env)
  {
    RecursiveIterator currentIter = getInnerIterator();

    currentIter.next(env);

    if (! currentIter.valid(env) && _iterStack.size() > 1) {
      _iterStack.remove(_iterStack.size() - 1);

      next(env);

      return;
    }

    if (currentIter.hasChildren(env)) {
      RecursiveIterator recursiveIter = currentIter.getChildren(env);

      _iterStack.add(recursiveIter);
    }
  }

  public override void rewind(Env env)
  {
    for (int i = _iterStack.size() - 1; i > 0; i--) {
      _iterStack.remove(i);
    }

    _iterStack.get(0).rewind(env);
  }

  public override string toString()
  {
    return getClass().getSimpleName() + "[" + _iterStack + "]";
  }

  static class RecursiveIteratorProxy implements RecursiveIterator {
    private final Value _obj;

    private const StringValue HAS_CHILDREN
      = new ConstStringValue("hasChildren");

    private const StringValue GET_CHILDREN
      = new ConstStringValue("getChildren");

    private const StringValue VALID
      = new ConstStringValue("valid");

    private const StringValue CURRENT
      = new ConstStringValue("current");

    private const StringValue KEY
      = new ConstStringValue("key");

    private const StringValue NEXT
      = new ConstStringValue("next");

    private const StringValue REWIND
      = new ConstStringValue("rewind");

    RecursiveIteratorProxy(Value obj)
    {
      _obj = obj;
    }

    public static RecursiveIterator create(Value value)
    {
      Object obj = value.toJavaObject();

      if (obj instanceof RecursiveIterator) {
        return (RecursiveIterator) obj;
      }
      else {
        return new RecursiveIteratorProxy(value);
      }
    }

    public override boolean hasChildren(Env env)
    {
      return _obj.callMethod(env, HAS_CHILDREN).toBoolean();
    }

    public override RecursiveIterator getChildren(Env env)
    {
      Value result = _obj.callMethod(env, GET_CHILDREN);

      return create(result);
    }

    public override boolean valid(Env env)
    {
      return _obj.callMethod(env, VALID).toBoolean();
    }

    public override Value current(Env env)
    {
      return _obj.callMethod(env, CURRENT);
    }

    public override Value key(Env env)
    {
      return _obj.callMethod(env, KEY);
    }

    public override void next(Env env)
    {
      _obj.callMethod(env, NEXT);
    }

    public override void rewind(Env env)
    {
      _obj.callMethod(env, REWIND);
    }
  }
}
}
