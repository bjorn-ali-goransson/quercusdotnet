using System;
namespace QuercusDotNet.Function{
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
 * @author Scott Ferguson
 */








/**
 * Represents a compiled function
 */
@SuppressWarnings("serial")
abstract public class CompiledFunction : CompiledAbstractFunction {

  public CompiledFunction(String name,
                          Arg []args)
  {
    super(name, args);
  }

  @Override
  public Value callRef(Env env, Value []argValues)
  {
    return call(env, argValues).copyReturn();
  }

  public override Value callRef(Env env)
  {
    return call(env).copyReturn();
  }

  public override Value callRef(Env env, Value a1)
  {
    return call(env, a1).copyReturn();
  }

  public override Value callRef(Env env, Value a1, Value a2)
  {
    return call(env, a1, a2).copyReturn();
  }

  public override Value callRef(Env env, Value a1, Value a2, Value a3)
  {
    return call(env, a1, a2, a3).copyReturn();
  }

  public override Value callRef(Env env, Value a1, Value a2, Value a3, Value a4)
  {
    return call(env, a1, a2, a3, a4).copyReturn();
  }

  public override Value callRef(Env env, Value a1, Value a2,
                       Value a3, Value a4, Value a5)
  {
    return call(env, a1, a2, a3, a4, a5).copyReturn();
  }


  //
  // special methods
  //

  public override Value callMethod(Env env,
                          StringValue methodName, int hash,
                          Value []args)
  {
    if (methodName.equalsString("__invoke")) {
      return call(env, args);
    }
    else {
      return super.callMethod(env, methodName, hash, args);
    }
  }
}

}
