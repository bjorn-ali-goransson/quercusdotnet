namespace QuercusDotNet.lib.reflection {
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









public class ReflectionException : QuercusLanguageException
{
  private Location _location;
  private string _message;

  public ReflectionException(Env env)
  {
    super(env);

    _location = env.getLocation();
  }

  public ReflectionException(Env env, string msg)
   : base(env) {

    _location = env.getLocation();

    _message = msg;
  }

  public Location getLocation(Env env)
  {
    return _location;
  }

  public string getMessage()
  {
    return _message;
  }

  public string getMessage(Env env)
  {
    return getMessage();
  }
}
}
