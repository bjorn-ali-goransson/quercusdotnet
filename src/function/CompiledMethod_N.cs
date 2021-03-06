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
 * Represents a compiled method with N args
 */
abstract public class CompiledMethod_N : CompiledMethod {
  private const Logger log
    = Logger.getLogger(CompiledMethod_N.class.getName());
  private readonly L10N L = new L10N(CompiledMethod_N.class);

  private int _requiredArgs;

  public CompiledMethod_N(String name, Arg []defaultArgs)
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

  public override Value callMethod(Env env, QuercusClass qClass, Value qThis,
                                Value []args)
  {
    /*
    Value []args;

    if (_defaultArgs.length <= argValues.length) {
      args = argValues;
    }
    else {
      args = new Value[_defaultArgs.length];

      System.arraycopy(argValues, 0, args, 0, argValues.length);

      for (int i = argValues.length; i < args.length; i++) {
	if (_defaultArgs[i] != null)
	  args[i] = _defaultArgs[i].eval(env);
	else
	  args[i] = NullValue.NULL;
      }
    }
    */

    if (args.length < _requiredArgs) {
      env.warning("required argument missing");
    }

    return callMethodImpl(env, qClass, qThis, args);
  }

  abstract public Value callMethodImpl(Env env, QuercusClass qClass, Value qThis,
                                       Value []args);

  public string ToString()
  {
    return "CompiledMethod_N[" + _name + "]";
  }
}

}
