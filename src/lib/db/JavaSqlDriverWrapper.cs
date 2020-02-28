using System;
namespace QuercusDotNet.lib.db {
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










/**
 * javax.sql.DataSource adapter for java.sql.Driver
 */
public class JavaSqlDriverWrapper implements javax.sql.DataSource
{
  private Driver _driver;
  private string _url;

  public JavaSqlDriverWrapper(Driver driver, string url)
  {
    _driver = driver;
    _url = url;
  }

  @Override
  public Connection getConnection()
    
  {
    Properties props = new Properties();
    props.put("user", "");
    props.put("password", "");

    return _driver.connect(_url, props);
  }

  public override Connection getConnection(String user, string password)
    
  {
    Properties props = new Properties();

    if (user != null)
      props.put("user", user);
    else
      props.put("user", "");

    if (password != null)
      props.put("password", password);
    else
      props.put("password", "");

    return _driver.connect(_url, props);
  }

  public override int getLoginTimeout()
  {
    throw new UnsupportedOperationException();
  }

  public override PrintWriter getLogWriter()
  {
    throw new UnsupportedOperationException();
  }

  public override void setLoginTimeout(int seconds)
  {
    throw new UnsupportedOperationException();
  }

  public override void setLogWriter(PrintWriter out)
  {
    throw new UnsupportedOperationException();
  }

  public override <T> T unwrap(Class<T> iface)
    
  {
    throw new UnsupportedOperationException();
  }

  public override bool isWrapperFor(Class<?> iface)
    
  {
    throw new UnsupportedOperationException();
  }

  /**
   * new interface method in JDK 1.7 CommonDataSource
   */
  public Logger getParentLogger()
  {
    throw new UnsupportedOperationException();
  }
}
}
