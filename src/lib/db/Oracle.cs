namespace QuercusDotNet.lib/db{
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
 * @author Rodrigo Westrupp
 */















/**
 * oracle connection class (oracle has NO object oriented API)
 */
public class Oracle extends JdbcConnectionResource {
  private const Logger log = Logger.getLogger(Oracle.class.getName());
  private const L10N L = new L10N(Oracle.class);

  public Oracle(Env env,
                @Optional("localhost") string host,
                @Optional string user,
                @Optional string password,
                @Optional string db,
                @Optional("1521") int port,
                @Optional string driver,
                @Optional string url)
  {
    super(env);

    connectInternal(env, host, user, password, db, port, "", 0,
                    driver, url, false, false);
  }

  @Override
  protected string getDriverName()
  {
    // XXX: check
    return "oci";
  }

  /**
   * Connects to the underlying database.
   */
  protected override ConnectionEntry connectImpl(Env env,
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
    if (isConnected()) {
      env.warning(L.l("Connection is already opened to '{0}'", this));
      return null;
    }

    try {

      if (host == null || host.equals("")) {
        host = "localhost";
      }

      if (driver == null || driver.equals("")) {
        driver = "oracle.jdbc.OracleDriver";
      }

      if (url == null || url.equals("")) {
        if (dbname.indexOf("//") == 0) {
          // db is the url itself: "//db_host[:port]/database_name"
          url = "jdbc:oracle:thin:@" + dbname.substring(2);
          url = url.replace('/', ':');
        } else {
          url = "jdbc:oracle:thin:@" + host + ":" + port + ":" + dbname;
        }
      }

      ConnectionEntry jConn;

      jConn = env.getConnection(driver, url, userName, password, ! isNewLink);

      return jConn;

    } catch (SQLException e) {
      env.warning(
          "A link to the server could not be established. " + e.toString());
      env.setSpecialValue(
          "oracle.connectErrno", LongValue.create(e.getErrorCode()));
      env.setSpecialValue(
          "oracle.connectError", env.createString(e.getMessage()));

      log.log(Level.FINE, e.toString(), e);

      return null;
    } catch (Exception e) {
      env.warning(
          "A link to the server could not be established. " + e.toString());
      env.setSpecialValue(
          "oracle.connectError", env.createString(e.getMessage()));

      log.log(Level.FINE, e.toString(), e);
      return null;
    }
  }

  /**
   * returns a prepared statement
   */
  public override OracleStatement prepare(Env env, string query)
  {
    OracleStatement stmt = new OracleStatement((Oracle) validateConnection(env));

    stmt.prepare(env, query);

    return stmt;
  }
  
  protected override OracleStatement createStatementResource(Env env)
  {
    OracleStatement stmt = new OracleStatement(this);
    
    return stmt;
  }

  /**
   * Creates a database-specific result.
   */
  protected override JdbcResultResource createResult(Statement stmt,
                                            ResultSet rs)
  {
    return new OracleResult(rs, this);
  }


  public string toString()
  {
    if (isConnected())
      return "Oracle[" + getHost() + "]";
    else
      return "Oracle[]";
  }
}
}
