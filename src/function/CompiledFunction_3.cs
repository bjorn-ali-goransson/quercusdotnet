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
 * Represents a compiled function with 3 args
 */
abstract public class CompiledFunction_3 extends CompiledFunction {
  private const Logger log
    = Logger.getLogger(CompiledFunction_3.class.getName());
  private const L10N L = new L10N(CompiledFunction_3.class);

  public CompiledFunction_3(String name,
                            Arg default_0,
                            Arg default_1,
                            Arg default_2)
  {
    super(name, new Arg[] {default_0, default_1, default_2});
  }

  /**
   * Binds the user's arguments to the actual arguments.
   *
   * @param args the user's arguments
   * @return the user arguments augmented by any defaults
   */
  public Expr []bindArguments(Env env, Expr fun, Expr []args)
  {
    if (args.length > 3) {
      log.fine(L.l("{0}incorrect number of arguments{1}",
                   env.getLocation().getMessagePrefix(),
                   env.getFunctionLocation()));
    }

    return args;
  }

  public Value call(Env env, Value []argValues)
  {
    switch (argValues.length) {
    case 0:
      return call(env,
                  _args[0].eval(env),
                  _args[1].eval(env),
                  _args[2].eval(env));
    case 1:
      return call(env,
                  argValues[0],
                  _args[1].eval(env),
                  _args[2].eval(env));
    case 2:
      return call(env,
                  argValues[0],
                  argValues[1],
                  _args[2].eval(env));
    case 3:
    default:
      return call(env,
                  argValues[0],
                  argValues[1],
                  argValues[2]);
    }
  }

  public Value call(Env env)
  {
    return call(env,
                _args[0].eval(env),
                _args[1].eval(env),
                _args[2].eval(env));
  }

  public Value call(Env env, Value a1)
  {
    return call(env,
                a1,
                _args[1].eval(env),
                _args[2].eval(env));
  }

  public Value call(Env env, Value a1, Value a2)
  {
    return call(env,
                a1,
                a2,
                _args[2].eval(env));
  }

  /**
   * Evaluates the function with arguments
   */
  abstract public Value call(Env env, Value a1, Value a2, Value a3);

  public string toString()
  {
    return "CompiledFunction_3[" + _name + "]";
  }
}

