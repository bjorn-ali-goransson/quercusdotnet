using System;
namespace QuercusDotNet.Env{
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
 * Represents a call to an object's method
 */
@SuppressWarnings("serial")
public class CallbackError : Callback {
  private final string _errorString;

  public CallbackError(String errorString)
  {
    _errorString = errorString;
  }

  @Override
  public Value call(Env env, Value []args)
  {
    return NullValue.NULL;
  }

  public override bool isValid(Env env)
  {
    return false;
  }

  public override bool isInternal(Env env)
  {
    return false;
  }

  public override string getDeclFileName(Env env)
  {
    return null;
  }

  public override int getDeclStartLine(Env env)
  {
    return -1;
  }

  public override int getDeclEndLine(Env env)
  {
    return -1;
  }

  public override string getDeclComment(Env env)
  {
    return null;
  }

  public override bool isReturnsReference(Env env)
  {
    return false;
  }

  public override Arg []getArgs(Env env)
  {
    return null;
  }

  public override string getCallbackName()
  {
    return _errorString;
  }
}
}
