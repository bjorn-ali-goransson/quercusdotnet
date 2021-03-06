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
 * Represents a compiled method with 1 arg
 */
abstract public class CompiledMethodRef_1 : CompiledMethodRef {
  private const Logger log
    = Logger.getLogger(CompiledMethodRef_1.class.getName());
  private readonly L10N L = new L10N(CompiledMethodRef_1.class);

  public CompiledMethodRef_1(String name, Arg default_0)
  {
    super(name, new Arg[] {default_0});
  }

  /**
   * Evaluates the method with the given variable arguments.
   */
  public override Value callMethodRef(Env env, QuercusClass qClass, Value qThis,
                             Value []args)
  {
    switch (args.length) {
    case 0:
      return callMethodRef(env, qClass, qThis,
                           _args[0].eval(env));
    case 1:
    default:
      return callMethodRef(env, qClass, qThis,
                           args[0]);
    }
  }

  /**
   * Evaluates the method with the given variable arguments.
   */
  public override Value callMethodRef(Env env, QuercusClass qClass, Value qThis)
  {
    return callMethodRef(env, qClass, qThis,
                         _args[0].eval(env));
  }

  abstract override public Value callMethodRef(Env env, QuercusClass qClass, Value qThis,
                                      Value a1);
}

}
