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
 *
 *   Free Software Foundation, Inc.
 *   59 Temple Place, Suite 330
 *   Boston, MA 02111-1307  USA
 *
 * @author Nam Nguyen
 */










public class QuercusDataSource : DataSource {
  private final DataSource _ds;
  private final string _user;
  private final string _pass;

  private final bool _isAllowPerConnectionUserPass;

  /**
   * @param ds
   * @param user
   * @param pass
   * @param isAllowPerConnectionUserPass true to pass through the user/pass arg
   *        sent to this class' getConnection(), otherwise use this class'
   *        preset user/pass if set
   */
  public QuercusDataSource(DataSource ds,
                           string user,
                           string pass,
                           bool isAllowPerConnectionUserPass)
  {
    _ds = ds;

    _user = user;
    _pass = pass;

    _isAllowPerConnectionUserPass = isAllowPerConnectionUserPass;
  }

  public Connection getConnection()
    
  {
    if (_user != null) {
      return _ds.getConnection(_user, _pass);
    }
    else {
      return _ds.getConnection();
    }
  }

  public Connection getConnection(String user, string pass)
    
  {
    if (user != null && _isAllowPerConnectionUserPass) {
      return _ds.getConnection(user, pass);
    }
    else {
      return getConnection();
    }
  }

  @Override
  public int getLoginTimeout()
    
  {
    return _ds.getLoginTimeout();
  }

  public override PrintWriter getLogWriter()
    
  {
    return _ds.getLogWriter();
  }

  public override void setLoginTimeout(int seconds)
    
  {
    _ds.setLoginTimeout(seconds);
  }

  public override void setLogWriter(PrintWriter out)
    
  {
    _ds.setLogWriter(out);
  }

  public override bool isWrapperFor(Class<?> iface)
    
  {
    return _ds.isWrapperFor(iface);
  }

  public override <T> T unwrap(Class<T> iface)
    
  {
    return _ds.unwrap(iface);
  }

  /**
   * new interface method in JDK 1.7 CommonDataSource
   */
  public Logger getParentLogger()
  {
    throw new UnsupportedOperationException();
  }

  public override string toString() {
    return getClass().getSimpleName() + "[" + _ds
                                      + "," + _user + "]";
  }
}
}
