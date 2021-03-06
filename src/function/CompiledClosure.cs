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
 * @author Nam Nguyen
 */









/**
 * Represents a compiled closure.
 */
abstract public class CompiledClosure : Closure {
  public CompiledClosure(String name, Value qThis)
  {
    super(name, qThis);
  }

  abstract public Value call(Env env, Value []args);

  public override Value callRef(Env env, Value []argValues)
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

  public abstract static class CompiledClosure_0 : CompiledClosure {
    public CompiledClosure_0(String name, Value qThis)
     : base(name, qThis) {
    }

    public override Value call(Env env, Value []argValues)
    {
      return call(env);
    }

    abstract public Value call(Env env);
  }

  public abstract static class CompiledClosure_1 : CompiledClosure {
    private Expr _default0;

    public CompiledClosure_1(String name, Value qThis, Expr default0)
     : base(name, qThis) {

      _default0 = default0;
    }

    public override Value call(Env env, Value []argValues)
    {
      switch (argValues.length) {
        case 0:
          return call(env,
                      _default0.eval(env));
        case 1:
        default:
          return call(env,
                      argValues[0]);
      }
    }

    public override Value call(Env env)
    {
      return call(env, _default0.eval(env));
    }

    abstract public Value call(Env env, Value a1);
  }

  public abstract static class CompiledClosure_2 : CompiledClosure {
    private Expr _default0;
    private Expr _default1;

    public CompiledClosure_2(String name, Value qThis,
                             Expr default0, Expr default1)
     : base(name, qThis) {

      _default0 = default0;
      _default1 = default1;
    }

    public override Value call(Env env, Value []argValues)
    {
      switch (argValues.length) {
        case 0:
          return call(env,
                      _default0.eval(env),
                      _default1.eval(env));
        case 1:
          return call(env,
                      argValues[0],
                      _default1.eval(env));
        case 2:
        default:
          return call(env,
                      argValues[0],
                      argValues[1]);
      }
    }

    public override Value call(Env env)
    {
      return call(env, _default0.eval(env), _default1.eval(env));
    }

    public override Value call(Env env, Value a1)
    {
      return call(env, a1, _default1.eval(env));
    }

    abstract public Value call(Env env, Value a1, Value a2);
  }

  public abstract static class CompiledClosure_3 : CompiledClosure {
    private Expr _default0;
    private Expr _default1;
    private Expr _default2;

    public CompiledClosure_3(String name, Value qThis,
                             Expr default0, Expr default1, Expr default2)
     : base(name, qThis) {

      _default0 = default0;
      _default1 = default1;
      _default2 = default2;
    }

    public override Value call(Env env, Value []argValues)
    {
      switch (argValues.length) {
        case 0:
          return call(env,
                      _default0.eval(env),
                      _default1.eval(env),
                      _default2.eval(env));
        case 1:
          return call(env,
                      argValues[0],
                      _default1.eval(env),
                      _default2.eval(env));
        case 2:
          return call(env,
                      argValues[0],
                      argValues[1],
                      _default2.eval(env));
        case 3:
        default:
          return call(env,
                      argValues[0],
                      argValues[1],
                      argValues[2]);
      }
    }

    public override Value call(Env env)
    {
      return call(env,
                  _default0.eval(env),
                  _default1.eval(env),
                  _default2.eval(env));
    }

    public override Value call(Env env, Value a1)
    {
      return call(env,
                  a1,
                  _default1.eval(env),
                  _default2.eval(env));
    }

    public override Value call(Env env, Value a1, Value a2)
    {
      return call(env,
                  a1,
                  a2,
                  _default2.eval(env));
    }

    abstract public Value call(Env env, Value a1, Value a2, Value a3);
  }

  public abstract static class CompiledClosure_4 : CompiledClosure {
    private Expr _default0;
    private Expr _default1;
    private Expr _default2;
    private Expr _default3;

    public CompiledClosure_4(String name, Value qThis,
                             Expr default0, Expr default1,
                             Expr default2, Expr default3)
     : base(name, qThis) {

      _default0 = default0;
      _default1 = default1;
      _default2 = default2;
      _default3 = default3;
    }

    public override Value call(Env env, Value []argValues)
    {
      switch (argValues.length) {
        case 0:
          return call(env,
                      _default0.eval(env),
                      _default1.eval(env),
                      _default2.eval(env),
                      _default3.eval(env));
        case 1:
          return call(env,
                      argValues[0],
                      _default1.eval(env),
                      _default2.eval(env),
                      _default3.eval(env));
        case 2:
          return call(env,
                      argValues[0],
                      argValues[1],
                      _default2.eval(env),
                      _default3.eval(env));
        case 3:
          return call(env,
                      argValues[0],
                      argValues[1],
                      argValues[2],
                      _default3.eval(env));
        case 4:
        default:
          return call(env,
                      argValues[0],
                      argValues[1],
                      argValues[2],
                      argValues[3]);
      }
    }

    public override Value call(Env env)
    {
      return call(env,
                  _default0.eval(env),
                  _default1.eval(env),
                  _default2.eval(env),
                  _default3.eval(env));
    }

    public override Value call(Env env, Value a1)
    {
      return call(env,
                  a1,
                  _default1.eval(env),
                  _default2.eval(env),
                  _default3.eval(env));
    }

    public override Value call(Env env, Value a1, Value a2)
    {
      return call(env,
                  a1,
                  a2,
                  _default2.eval(env),
                  _default3.eval(env));
    }

    public override Value call(Env env, Value a1, Value a2, Value a3)
    {
      return call(env,
                  a1,
                  a2,
                  a3,
                  _default3.eval(env));
    }

    abstract public Value call(Env env, Value a1, Value a2, Value a3, Value a4);
  }

  public abstract static class CompiledClosure_5 : CompiledClosure {
    private Expr _default0;
    private Expr _default1;
    private Expr _default2;
    private Expr _default3;
    private Expr _default4;

    public CompiledClosure_5(String name, Value qThis,
                             Expr default0, Expr default1,
                             Expr default2, Expr default3,
                             Expr default4)
     : base(name, qThis) {

      _default0 = default0;
      _default1 = default1;
      _default2 = default2;
      _default3 = default3;
      _default4 = default4;
    }

    public override Value call(Env env, Value []argValues)
    {
      switch (argValues.length) {
        case 0:
          return call(env,
                      _default0.eval(env),
                      _default1.eval(env),
                      _default2.eval(env),
                      _default3.eval(env),
                      _default4.eval(env));
        case 1:
          return call(env,
                      argValues[0],
                      _default1.eval(env),
                      _default2.eval(env),
                      _default3.eval(env),
                      _default4.eval(env));
        case 2:
          return call(env,
                      argValues[0],
                      argValues[1],
                      _default2.eval(env),
                      _default3.eval(env),
                      _default4.eval(env));
        case 3:
          return call(env,
                      argValues[0],
                      argValues[1],
                      argValues[2],
                      _default3.eval(env),
                      _default4.eval(env));
        case 4:
          return call(env,
                      argValues[0],
                      argValues[1],
                      argValues[2],
                      argValues[3],
                      _default4.eval(env));
        case 5:
        default:
          return call(env,
                      argValues[0],
                      argValues[1],
                      argValues[2],
                      argValues[3],
                      argValues[4]);
      }
    }

    public override Value call(Env env)
    {
      return call(env,
                  _default0.eval(env),
                  _default1.eval(env),
                  _default2.eval(env),
                  _default3.eval(env),
                  _default4.eval(env));
    }

    public override Value call(Env env, Value a1)
    {
      return call(env,
                  a1,
                  _default1.eval(env),
                  _default2.eval(env),
                  _default3.eval(env),
                  _default4.eval(env));
    }

    public override Value call(Env env, Value a1, Value a2)
    {
      return call(env,
                  a1,
                  a2,
                  _default2.eval(env),
                  _default3.eval(env),
                  _default4.eval(env));
    }

    public override Value call(Env env, Value a1, Value a2, Value a3)
    {
      return call(env,
                  a1,
                  a2,
                  a3,
                  _default3.eval(env),
                  _default4.eval(env));
    }

    public override Value call(Env env, Value a1, Value a2, Value a3, Value a4)
    {
      return call(env,
                  a1,
                  a2,
                  a3,
                  a4,
                  _default4.eval(env));
    }

    abstract public Value call(Env env, Value a1, Value a2, Value a3, Value a4,
                               Value a5);
  }

  public abstract static class CompiledClosure_N : CompiledClosure {
    private Expr []_defaultArgs;
    private int _requiredArgs;

    public CompiledClosure_N(String name, Value qThis, Expr []defaultArgs)
     : base(name, qThis) {
      _defaultArgs = defaultArgs;

      int requiredArgs = 0;

      for (int i = 0; i < _defaultArgs.length; i++) {
        if (_defaultArgs[i] == ParamRequiredExpr.REQUIRED) {
          requiredArgs++;
        }
        else {
          break;
        }
      }

      _requiredArgs = requiredArgs;
    }

    public override Value call(Env env, Value []argValues)
    {
      if (argValues.length < _requiredArgs) {
        env.warning("required argument missing");
      }

      return callImpl(env, argValues);
    }

    abstract public Value callImpl(Env env, Value []args);
  }
}

}
