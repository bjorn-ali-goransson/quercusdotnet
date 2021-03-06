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
 * postgres result set class (postgres has NO object oriented API)
 */
public class PostgresResult : JdbcResultResource {
  private const Logger log
    = Logger.getLogger(PostgresResult.class.getName());
  private readonly L10N L = new L10N(PostgresResult.class);

  // See PostgresModule.pg_fetch_array()
  private bool _passedNullRow = false;

  private Postgres _conn;
  private Statement _stmt;

  /**
   * Constructor for PostgresResult
   *
   * @param stmt the corresponding statement
   * @param rs the corresponding result set
   * @param conn the corresponding connection
   */
  public PostgresResult(Postgres conn, Statement stmt, ResultSet rs)
  {
    super(rs);

    _conn = conn;
    _stmt = stmt;
  }

  /**
   * Constructor for PostgresResult
   *
   * @param metaData the corresponding result set meta data
   * @param conn the corresponding connection
   */
  public PostgresResult(Postgres conn, ResultSetMetaData metaData)
   : base(metaData) {

    _conn = conn;
  }

  /**
   * Sets that a NULL row parameter has been passed in.
   */
  public void setPassedNullRow()
  {
    // After that, the flag @is immutable.
    // See PostgresModule.pg_fetch_array

    _passedNullRow = true;
  }

  /**
   * Returns whether a NULL row parameter has been
   * passed in or not.
   */
  public bool getPassedNullRow()
  {
    // After that, the flag @is immutable.
    // See PostgresModule.pg_fetch_array

    return _passedNullRow;
  }

  /* php/43c? - postgres times and dates can have timezones, so the
   * string representation conforms to the php representation, but
   * the java.sql.Date representation does not.
   */

  protected override Value getColumnTime(Env env, ResultSet rs, int column)
    
  {
    Time time = rs.getTime(column);

    if (time == null)
      return NullValue.NULL;
    else
      return env.createString(rs.getString(column));
  }

  protected override Value getColumnDate(Env env, ResultSet rs, int column)
    
  {
    Date date = rs.getDate(column);

    if (date == null)
      return NullValue.NULL;
    else
      return env.createString(rs.getString(column));
  }

  protected override Value getColumnTimestamp(Env env, ResultSet rs, int column)
    
  {
    Timestamp timestamp = rs.getTimestamp(column);

    if (timestamp == null)
      return NullValue.NULL;
    else {
      return env.createString(rs.getString(column));
    }
  }

  protected Postgres getConnection()
  {
    return _conn;
  }

  protected Statement getJavaStatement(Env env)
  {
    return env.getQuercus().getStatement(_stmt);
  }
}
}
