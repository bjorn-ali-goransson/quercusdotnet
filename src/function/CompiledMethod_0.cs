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
 * Represents a compiled method with 0 args
 */
abstract public class CompiledMethod_0 : CompiledMethod {
  private const Logger log
    = Logger.getLogger(CompiledMethod_0.class.getName());
  private readonly L10N L = new L10N(CompiledMethod_0.class);

  public CompiledMethod_0(String name)
  {
    super(name, AbstractFunction.NULL_ARGS);
  }

  /**
   * Binds the user's arguments to the actual arguments.
   *
   * @param args the user's arguments
   * @return the user arguments augmented by any defaults
   */
  /*
  public Expr []bindArguments(Env env, Expr fun, Expr []args)
  {
    if (args.length != 0)
      env.warning(L.l("too many arguments"));

    return args;
  }
  */

  public override Value callMethod(Env env,
                          QuercusClass qClass,
                          Value qThis,
                          Value []args)
  {
    return callMethod(env, qClass, qThis);
  }

  abstract override public Value callMethod(Env env,
                                   QuercusClass qClass,
                                   Value qThis);

  public string ToString()
  {
    return "CompiledMethod_0[" + _name + "]";
  }
}

}
