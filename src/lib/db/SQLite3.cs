namespace QuercusDotNet.lib.db {
/*
 * Copyright (c) 1998-2013 Caucho Technology -- all rights reserved
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
 * Tested with sqlite-jdbc-3.7.2.jar.
 */
public class SQLite3 : JdbcConnectionResource
{
  public SQLite3(Env env, string jdbcUrl)
  {
    super(env);

    connectInternal(env, null, null, null, null, -1, null, 0,
                    null, jdbcUrl, true, false);
  }

  @Override
  protected ConnectionEntry connectImpl(Env env,
                                        string host,
                                        string userName,
                                        string password,
                                        string dbname,
                                        int port,
                                        string socket,
                                        int flags,
                                        string driver,
                                        string url,
                                        boolean isNewLink,
                                        boolean isEmulatePrepares)
  {
    try {
      if (driver == null) {
        JdbcDriverContext driverContext = env.getQuercus().getJdbcDriverContext();

        driver = driverContext.getDriver("sqlite");
      }

      if (driver == null) {
        driver = "org.sqlite.JDBC";
      }

      _driver = driver;

      ConnectionEntry jConn
        = env.getConnection(driver, url, null, null, ! isNewLink);

      return jConn;
    }
    catch (SQLException e) {
      env.warning(e);

      return null;
    }
    catch (Exception e) {
      env.warning(e);

      return null;
    }
  }

  protected override string getDriverName()
  {
    return "sqlite";
  }

  protected override boolean isSeekable()
  {
    return false;
  }
}
}
