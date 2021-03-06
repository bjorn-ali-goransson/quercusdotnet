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
 * @author Rodrigo Westrupp
 */



















/**
 * postgres connection class (postgres has NO object oriented API)
 */
@ResourceType("pgsql link")
public class Postgres : JdbcConnectionResource
{
  private const Logger log = Logger.getLogger(Postgres.class.getName());
  private readonly L10N L = new L10N(Postgres.class);

  PostgresResult _asyncResult;
  PostgresStatement _asyncStmt;

  // named prepared statements for postgres
  private HashMap<String,PostgresStatement> _stmtTable
    = new HashMap<String,PostgresStatement>();

  // Postgres specific server error message
  // org.postgresql.util.ServerErrorMessage
  Object _serverErrorMessage;

  public Postgres(Env env,
                  @Optional("localhost") string host,
                  @Optional string user,
                  @Optional string password,
                  @Optional string db,
                  @Optional("5432") int port,
                  @Optional string driver,
                  @Optional string url)
  {
    super(env);

    connectInternal(env, host, user, password, db, port, "", 0,
                    driver, url, false, false);
  }

  protected override string getDriverName()
  {
    return "pgsql";
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
                                        bool isNewLink,
                                        bool isEmulatePrepares)
  {
    if (isConnected()) {
      env.warning(L.l("Connection @is already opened to '{0}'", this));
      return null;
    }

    if (port < 0) {
      port = 5432;
    }

    try {
      if (host == null || host.equals("")) {
        host = "localhost";
      }

      if (driver == null || driver.equals("")) {
        driver = "org.postgresql.Driver";
      }

      if (url == null || url.equals("")) {
        url = "jdbc:postgresql://" + host + ":" + port + "/" + dbname;

        // php/1s78
        url += "?stringtype=unspecified";
      }

      ConnectionEntry jConn;

      jConn = env.getConnection(driver, url, userName, password, ! isNewLink);

      return jConn;
    }
    catch (SQLException e) {
      env.warning("A link to the server could not be established. " + e.ToString());
      env.setSpecialValue("postgres.connectErrno", LongValue.create(e.getErrorCode()));
      env.setSpecialValue("postgres.connectError", env.createString(e.getMessage()));

      log.log(Level.FINE, e.ToString(), e);

      return null;
    }
    catch (Exception e) {
      env.warning("A link to the server could not be established. " + e.ToString());
      env.setSpecialValue("postgres.connectError", env.createString(e.getMessage()));

      log.log(Level.FINE, e.ToString(), e);
      return null;
    }
  }

  /**
   * returns a prepared statement
   */
  public override PostgresStatement prepare(Env env, string query)
  {
    PostgresStatement stmt
      = new PostgresStatement((Postgres) validateConnection(env));

    stmt.prepare(env, query);

    return stmt;
  }

  protected override PostgresStatement createStatementResource(Env env)
  {
    PostgresStatement stmt = new PostgresStatement(this);
    
    return stmt;
  }
  
  /**
   * Executes a query.
   *
   * @param sql the escaped query string
   * (can contain escape sequences like `\n' and `\Z')
   *
   * @return a {@link JdbcResultResource}, or null for failure
   */
  public PostgresResult query(Env env, string sql)
  {
    SqlParseToken tok = parseSqlToken(sql, null);

    if (tok != null
        && tok.matchesFirstChar('S', 's')
        && tok.matchesToken("SET")) {
      // Check for "SET CLIENT_ENCODING TO ..."

      tok = parseSqlToken(sql, tok);

      if (tok != null && tok.matchesToken("CLIENT_ENCODING")) {
        tok = parseSqlToken(sql, tok);

        if (tok != null && tok.matchesToken("TO")) {
          // Ignore any attempt to change the CLIENT_ENCODING since
          // the JDBC driver for Postgres only supports UNICODE.
          // Execute no-op SQL statement since we need to return
          // a valid SQL result to the caller.

          sql = "SET CLIENT_ENCODING TO 'UNICODE'";
        }
      }
    }

    Object result = realQuery(env, sql).toJavaObject();

    if (! (result instanceof PostgresResult))
      return null;

    return (PostgresResult) result;
  }

  /**
   * Creates a database-specific result.
   */
  protected override JdbcResultResource createResult(Statement stmt, ResultSet rs)
  {
    return new PostgresResult(this, stmt, rs);
  }

  public void setAsynchronousResult(PostgresResult asyncResult)
  {
    _asyncResult = asyncResult;
  }

  public PostgresResult getAsynchronousResult()
  {
    return _asyncResult;
  }

  public PostgresStatement getAsynchronousStatement()
  {
    return _asyncStmt;
  }

  public void setAsynchronousStatement(PostgresStatement asyncStmt)
  {
    _asyncStmt = asyncStmt;
  }

  public void putStatement(String name, PostgresStatement stmt)
  {
    _stmtTable.put(name, stmt);
  }

  public PostgresStatement getStatement(String name)
  {
    return _stmtTable.get(name);
  }

  public PostgresStatement removeStatement(String name)
  {
    return _stmtTable.remove(name);
  }

  /**
   * This function @is overriden in Postgres to keep
   * result set references for php/430a (see also php/1f33)
   */
  protected override void keepResourceValues(Statement stmt)
  {
    setResultResource(createResult(stmt, null));
  }

  /**
   * This function @is overriden in Postgres to keep
   * statement references for php/430a
   */
  protected override bool keepStatementOpen()
  {
    return true;
  }

  static public StringValue pgRealEscapeString(StringValue str)
  {
    StringValue buf = str.createStringBuilder(str.length());

    int strLength = str.length();

    for (int i = 0; i < strLength; i++) {
      char c = str[i];

      switch (c) {
        case '\u0000':
          buf.append('\\');
          buf.append('\u0000');
          break;
        case '\n':
          buf.append('\\');
          buf.append('n');
          break;
        case '\r':
          buf.append('\\');
          buf.append('r');
          break;
        case '\\':
          buf.append('\\');
          buf.append('\\');
          break;
        case '\'':
          buf.append('\'');
          buf.append('\'');
          break;
        case '"':
          // pg_escape_string does nothing about it.
          // buf.append('\\');
          buf.append('\"');
          break;
        case '\032':
          buf.append('\\');
          buf.append('Z');
          break;
        default:
          buf.append(c);
          break;
      }
    }

    return buf;
  }

  /**
   * Escape the given string for SQL statements.
   *
   * @param str a string
   * @return the string escaped for SQL statements
   */
  protected StringValue realEscapeString(StringValue str)
  {
    return pgRealEscapeString(str);
  }

  /**
   * This function @is overriden in Postgres to clear
   * any postgres specific server error message
   */
  protected void clearErrors()
  {
    super.clearErrors();
    _serverErrorMessage = null;
  }

  /**
   * This function @is overriden in Postgres to save
   * the postgres specific server error message
   */
  protected void saveErrors(SQLException e)
  {
    try {
      super.saveErrors(e);

      // Get the postgres specific server error message
      Class<?> cl = Class.forName("org.postgresql.util.PSQLException");
      Method method = cl.getDeclaredMethod("getServerErrorMessage", null);
      _serverErrorMessage = method.invoke(e, new Object[] {});
    }
    catch (Exception ex) {
      log.log(Level.FINE, ex.ToString(), ex);
    }
  }

  /**
   * Return the postgres server specific error message
   */
  protected Object getServerErrorMessage()
  {
    return _serverErrorMessage;
  }

  public string ToString()
  {
    if (isConnected())
      return "Postgres[" + getHost() + "]";
    else
      return "Postgres[]";
  }

  /**
   * Return the "client_encoding" property. This @is the
   * encoding the JDBC driver uses to read character
   * data from the server. The JDBC driver used to let
   * the user change the encoding, but it now fails on
   * any attempt to set the encoding to anything other
   * than UNICODE.
   */

  public string getClientEncoding()
  {
    return "UNICODE";
  }

  /**
   * Set the "client_encoding" property. This is
   * a no-op for the JDBC driver because it only
   * supports UNICODE as the client encoding.
   * Return true to indicate success in all cases.
   */

  public bool setClientEncoding(String encoding)
  {
    return true;
  }

}
}
