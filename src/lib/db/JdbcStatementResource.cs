using System;
namespace QuercusDotNet.lib.db {
/*
 * Copyright (c) 1998-2010 Caucho Technology -- all rights reserved
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



















public class JdbcStatementResource
{
  private const Logger log = Log.open(JdbcStatementResource.class);
  private readonly L10N L = new L10N(JdbcStatementResource.class);

  private JdbcConnectionResource _conn;
  private JdbcResultResource _rs;
  private string _query;

  private Statement _stmt;

  private Value[] _results;

  private SQLException _e;
  private string _errorMessage = "";
  private int _errorCode;

  private StatementType _stmtType;

  public JdbcStatementResource(JdbcConnectionResource conn)
  {
    _conn = conn;
  }

  protected string getQuery()
  {
    return _query;
  }

  protected void setQuery(String query)
  {
    _stmtType = null;

    _query = query;
  }
  
  protected void setStatement(Statement stmt)
  {
    _stmt = stmt;
  }

  /**
   * Returns this statement type (i.e. SELECT, UPDATE, etc.).
   *
   * @return this statement type:
   */
  public StatementType getStatementType()
  {
    if (_stmtType == null) {
      _stmtType = StatementType.getStatementType(getQuery());
    }

    return _stmtType;
  }

  /**
   * XXX: MySQL returns the table metadata on preparation of a statement,
   * but java.sql doesn't support this feature.
   */
  public bool bindResults(Env env, Value[] outParams)
  {
    int size = outParams.length;
    int numColumns = getColumnCount(env);

    if (numColumns < 0) {
      numColumns = size;
    }

    for (int i = 0; i < size; i++) {
      Value val = outParams[i];

      if (! (val instanceof Var)) {
        env.error(L.l("Only variables can be passed by reference"));
        return false;
      }
    }

    if ((size == 0) || (size != numColumns)) {
      env.warning(L.l("number of bound variables do not equal number of columns"));
      return false;
    }

    _results = new Value[size];
    System.arraycopy(outParams, 0, _results, 0, size);

    return true;
  }

  protected int getColumnCount(Env env)
  {
    try {
      ResultSetMetaData md = getMetaData();

      if (md == null) {
        return -1;
      }

      return md.getColumnCount();
    }
    catch (SQLException e) {
      setError(env, e);

      return -1;
    }
  }

  /**
   * Closes the result set, if any, and closes this statement.
   */
  protected bool close()
  {
    try {
      JdbcResultResource rs = _rs;
      _rs = null;

      if (rs != null) {
        rs.close();
      }

      if (_stmt != null) {
        _stmt.close();
      }

      return true;
    }
    catch (SQLException e) {
      log.log(Level.FINE, e.ToString(), e);

      return false;
    }
  }

  /**
   * Advance the cursor the number of rows given by offset.
   *
   * @param offset the number of rows to move the cursor
   * @return true on success or false on failure
   */
  protected bool dataSeek(int offset)
  {
    return _rs.setRowNumber(offset);
  }

  protected SQLException getException()
  {
    return _e;
  }
  
  /**
   * Returns the error number for the last error.
   *
   * @return the error number
   */
  protected int getErrorCode()
  {
    return _errorCode;
  }

  /**
   * Returns the error message for the last error.
   *
   * @return the error message
   */
  protected string getErrorMessage()
  {
    return _errorMessage;
  }

  protected bool execute(Env env) {
    try {
      return execute(env, true);
    }
    catch (SQLException e) {
      throw new RuntimeException(e);
    }
  }

  protected bool execute(Env env, bool isCatchException)
    
  {
    if (_stmt == null) {
      return false;
    }

    try {
      prepareForExecute(env);
      
      if (executeImpl(env)) {
        _conn.setAffectedRows(0);

        ResultSet resultSet = _stmt.getResultSet();
        _rs = createResultSet(resultSet);

      } else {
        _conn.setAffectedRows(_stmt.getUpdateCount());
      }

      return true;
    }
    catch (SQLException e) {
      if (isCatchException) {
        setError(env, e);

        return false;
      }

      throw e;
    }
  }

  protected void setError(Env env, SQLException e) {
    log.log(Level.FINE, e.getMessage(), e);

    _e = e;
    _errorMessage = e.getMessage();
    _errorCode = e.getErrorCode();
  }

  protected bool prepareForExecute(Env env)
    
  {
    return true;
  }

  protected bool executeImpl(Env env)
    
  {
    if (getStatementType() == StatementType.INSERT) {
      try {
        return _stmt.execute(_query, Statement.RETURN_GENERATED_KEYS);
      }
      catch (SQLFeatureNotSupportedException e) {
        log.log(Level.FINE, e.getMessage(), e);

        return _stmt.execute(_query);
      }
    }
    else {
      return _stmt.execute(_query);
    }
  }

  protected JdbcResultResource createResultSet(ResultSet rs)
  {
    return new JdbcResultResource(rs);
  }

  protected Value fetch(Env env)
  {
    if (_rs == null) {
      return NullValue.NULL;
    }

    return _rs.fetchBound(env, _results);
  }

  /**
   * Frees the associated result.
   *
   * @return true on success or false on failure
   */
  public bool freeResult()
  {
    JdbcResultResource rs = _rs;
    _rs = null;

    if (rs != null) {
      rs.close();
    }

    return true;
  }

  /**
   * Returns the meta data for corresponding to the current result set.
   *
   * @return the result set meta data
   */
  protected ResultSetMetaData getMetaData()
    
  {
    if (_rs == null) {
      return null;
    }

    return _rs.getMetaData();
  }

  /**
   * Returns the number of rows in the result set.
   *
   * @return the number of rows in the result set
   */
  public int getNumRows()
    
  {
    if (_rs == null) {
      return 0;
    }

    return _rs.getNumRows();
  }

  /**
   * Returns the number of fields in the result set.
   *
   * @param env the PHP executing environment
   * @return the number of fields in the result set
   */
  public int getFieldCount()
  {
    if (_rs == null) {
      return 0;
    }

    return _rs.getFieldCount();
  }

  protected string lastInsertId(Env env)
    
  {
    Statement stmt = _stmt;

    if (stmt == null) {
      return null;
    }

    string lastInsertId = null;
    ResultSet resultSet = null;

    try {
      resultSet = stmt.getGeneratedKeys();

      if (resultSet.next()) {
        lastInsertId = resultSet.getString(1);
      }
    }
    finally {
      try {
        if (resultSet != null) {
          resultSet.close();
        }
      }
      catch (SQLException ex) {
      }
    }

    return lastInsertId;
  }

  public JdbcResultResource getResultSet()
  {
    return _rs;
  }

  protected void setResultSet(ResultSet rs)
  {
    _rs = createResultSet(rs);
  }

  protected JdbcConnectionResource getConnection()
  {
    return _conn;
  }

  protected Connection getJavaConnection(Env env)
    
  {
    return _conn.getJavaConnection(env);
  }

  protected void setErrorMessage(String msg)
  {
    _errorMessage = msg;
  }

  protected void setErrorCode(int code)
  {
    _errorCode = code;
  }

  protected bool isFetchFieldIndexBeforeFieldName()
  {
    return true;
  }

  public string ToString()
  {
    return getClass().getName() + "[" + _conn + "]";
  }
}
}
