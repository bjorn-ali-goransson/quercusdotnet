using System;
namespace QuercusDotNet.Function{
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
 * Represents a compiled function with 1 arg
 */
abstract public class CompiledFunctionRef : CompiledAbstractFunction {
  private const Logger log
    = Logger.getLogger(CompiledFunctionRef.class.getName());
  private readonly L10N L = new L10N(CompiledFunctionRef.class);

  public CompiledFunctionRef(String name,
                             Arg []args)
  {
    super(name, args);
  }

  public override Value call(Env env, Value []argValues)
  {
    return callRef(env, argValues).copy();
  }

  public override Value call(Env env)
  {
    return callRef(env).copy();
  }

  public override Value call(Env env, Value arg)
  {
    return callRef(env, arg).copy();
  }

  public override Value call(Env env, Value a1, Value a2)
  {
    return callRef(env, a1, a2).copy();
  }

  public override Value call(Env env, Value a1, Value a2, Value a3)
  {
    return callRef(env, a1, a2, a3).copy();
  }

  public override Value call(Env env, Value a1, Value a2, Value a3, Value a4)
  {
    return callRef(env, a1, a2, a3, a4).copy();
  }

  public override Value call(Env env, Value a1, Value a2, Value a3, Value a4, Value a5)
  {
    return callRef(env, a1, a2, a3, a4, a5).copy();
  }
}

}
