using System;
namespace QuercusDotNet.lib.db {
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
 *   Free SoftwareFoundation, Inc.
 *   59 Temple Place, Suite 330
 *   Boston, MA 02111-1307  USA
 *
 * @author Sam
 */








public class PDOException
  : QuercusLanguageException
{
  private string _code;
  private string _message;

  private Location _location;

  public PDOException(Env env, string code, string message)
  {
    super(env);

    _code = code;
    _message = "SQLSTATE[" + code + "]: " + message;

    _location = env.getLocation();
  }

  public string getCode()
  {
    return _code;
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

  /**
   * Converts the exception to a Value.
   */
  @Override
  public Value toValue(Env env)
  {
    Value e = env.createException("PDOException", _code, _message);

    return e;
  }
}
}
