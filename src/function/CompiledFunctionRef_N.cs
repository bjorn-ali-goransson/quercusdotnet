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
 * Represents a compiled function with N args
 */
abstract public class CompiledFunctionRef_N extends CompiledFunctionRef {
  private const Logger log
    = Logger.getLogger(CompiledFunctionRef_N.class.getName());
  private const L10N L = new L10N(CompiledFunctionRef_N.class);

  private final int _requiredArgs;

  public CompiledFunctionRef_N(String name, Arg []defaultArgs)
  {
    super(name, defaultArgs);

    int requiredArgs = 0;

    for (int i = 0; i < defaultArgs.length; i++) {
      if (defaultArgs[i].isRequired()) {
        requiredArgs++;
      }
      else {
        break;
      }
    }

    _requiredArgs = requiredArgs;
  }

  /**
   * Binds the user's arguments to the actual arguments.
   *
   * @param args the user's arguments
   * @return the user arguments augmented by any defaults
   */
  public Expr []bindArguments(Env env, Expr fun, Expr []args)
  {
    return args;
  }

  public final Value callRef(Env env, Value []argValues)
  {
    /*
    Value []args = argValues;

    if (_defaultArgs.length != argValues.length) {
      int len = _defaultArgs.length;

      if (len < argValues.length)
        len = argValues.length;

      args = new Value[len];

      System.arraycopy(argValues, 0, args, 0, argValues.length);

      for (int i = argValues.length; i < _defaultArgs.length; i++) {
        args[i] = _defaultArgs[i].eval(env);
      }
    }
    */

    if (argValues.length < _requiredArgs) {
      env.warning("required argument missing");
    }

    return callRefImpl(env, argValues);
  }

  abstract public Value callRefImpl(Env env, Value []args);

  public string toString()
  {
    return "CompiledFunction_N[" + _name + "]";
  }
}

}
