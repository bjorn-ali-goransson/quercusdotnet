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
 * @author Scott Ferguson
 */



























/**
 * PDO object oriented API facade.
 */
public class PDO : EnvCleanup {
  private const Logger log = Logger.getLogger(PDO.class.getName());
  private readonly L10N L = new L10N(PDO.class);

  public const int ATTR_AUTOCOMMIT = 0;
  public const int ATTR_PREFETCH = 1;
  public const int ATTR_TIMEOUT = 2;
  public const int ATTR_ERRMODE = 3;
  public const int ATTR_SERVER_VERSION = 4;
  public const int ATTR_CLIENT_VERSION = 5;
  public const int ATTR_SERVER_INFO = 6;
  public const int ATTR_CONNECTION_STATUS = 7;
  public const int ATTR_CASE = 8;
  public const int ATTR_CURSOR_NAME = 9;
  public const int ATTR_CURSOR = 10;
  public const int ATTR_ORACLE_NULLS = 11;
  public const int ATTR_PERSISTENT = 12;
  public const int ATTR_STATEMENT_CLASS = 13;
  public const int ATTR_FETCH_TABLE_NAMES = 14;
  public const int ATTR_FETCH_CATALOG_NAMES = 15;
  public const int ATTR_DRIVER_NAME = 16;
  public const int ATTR_STRINGIFY_FETCHES = 17;
  public const int ATTR_MAX_COLUMN_LEN = 18;
  public const int ATTR_DEFAULT_FETCH_MODE = 19;
  public const int ATTR_EMULATE_PREPARES = 20;

  public const int MYSQL_ATTR_INIT_COMMAND = 1002;

  public const int CASE_NATURAL = 0;
  public const int CASE_UPPER = 1;
  public const int CASE_LOWER = 2;

  public const int CURSOR_FWDONLY = 0;
  public const int CURSOR_SCROLL = 1;

  public const string ERR_NONE = "00000";

  public const int ERRMODE_SILENT = 0;
  public const int ERRMODE_WARNING = 1;
  public const int ERRMODE_EXCEPTION = 2;

  public const int FETCH_LAZY = 1;
  public const int FETCH_ASSOC = 2;
  public const int FETCH_NUM = 3;
  public const int FETCH_BOTH = 4;
  public const int FETCH_OBJ = 5;
  public const int FETCH_BOUND = 6;
  public const int FETCH_COLUMN = 7;
  public const int FETCH_CLASS = 8;
  public const int FETCH_INTO = 9;
  public const int FETCH_FUNC = 10;
  public const int FETCH_NAMED = 11;
  public const int FETCH_KEY_PAIR = 12;

  public const int FETCH_GROUP = 0x00010000;
  public const int FETCH_UNIQUE = 0x00030000;
  public const int FETCH_CLASSTYPE = 0x00040000;
  public const int FETCH_SERIALIZE = 0x00080000;

  public const int FETCH_ORI_NEXT = 0;
  public const int FETCH_ORI_PRIOR = 1;
  public const int FETCH_ORI_FIRST = 2;
  public const int FETCH_ORI_LAST = 3;
  public const int FETCH_ORI_ABS = 4;
  public const int FETCH_ORI_REL = 5;

  public const int FETCH_PROPS_LATE = 1048576;

  public const int MYSQL_ATTR_USE_BUFFERED_QUERY = 1000;

  public const int NULL_NATURAL = 0;
  public const int NULL_EMPTY_STRING = 1;
  public const int NULL_TO_STRING = 2;

  public const int PARAM_NULL = 0;
  public const int PARAM_INT = 1;
  public const int PARAM_STR = 2;
  public const int PARAM_LOB = 3;
  public const int PARAM_STMT = 4;
  public const int PARAM_BOOL = 5;

  public const int PARAM_EVT_ALLOC = 0;
  public const int PARAM_EVT_EXEC_POST = 3;
  public const int PARAM_EVT_EXEC_PRE = 2;
  public const int PARAM_EVT_FETCH_POST = 5;
  public const int PARAM_EVT_FETCH_PRE = 4;
  public const int PARAM_EVT_FREE = 1;
  public const int PARAM_EVT_NORMALIZE = 6;

  public const int PARAM_INPUT_OUTPUT = 0x80000000;

  private string _dsn;

  private JdbcConnectionResource _conn;

  private PDOError _error;

  private PDOStatement _lastPDOStatement;
  private PDOStatement _lastExecutedStatement;

  private bool _inTransaction;

  private string _statementClassName;
  private Value[] _statementClassArgs;

  private bool _isEmulatePrepares;

  private string _initQuery;

  private int _columnCase = JdbcResultResource.COLUMN_CASE_NATURAL;

  public PDO(Env env,
             string dsn,
             @Optional string user,
             @Optional string pass,
             @Optional @ReadOnly ArrayValue options)
  {
    _dsn = dsn;
    _error = new PDOError();

    if (options != null) {
      for (Map.Entry<Value,Value> entry : options.entrySet()) {
        setAttribute(env, entry.getKey().toInt(), entry.getValue());
      }
    }

    // XXX: following would be better as annotation on destroy() method
    env.addCleanup(this);

    try {
      JdbcConnectionResource conn = getConnection(env, dsn, user, pass);
      _conn = conn;

      if (conn == null) {
        env.warning(L.l("'{0}' @is an unknown PDO data source.", dsn));
      }
    }
    catch (SQLException e) {
      env.warning(e.getMessage(), e);
      _error.error(env, e);
    }
  }

  protected JdbcConnectionResource getConnection()
  {
    return _conn;
  }

  protected bool isConnected()
  {
    return _conn != null && _conn.isConnected();
  }

  protected void setLastExecutedStatement(PDOStatement stmt)
  {
    _lastExecutedStatement = stmt;
  }

  /**
   * Starts a transaction.
   */
  public bool beginTransaction()
  {
    JdbcConnectionResource conn = getConnection();

    if (! isConnected()) {
      return false;
    }

    if (_inTransaction) {
      return false;
    }

    _inTransaction = true;

    return conn.setAutoCommit(false);
  }

  private void closeStatements()
  {
    PDOStatement stmt = _lastPDOStatement;
    _lastPDOStatement = null;

    if (stmt != null) {
      stmt.close();
    }
  }

  /**
   * Commits a transaction.
   */
  public bool commit()
  {
    JdbcConnectionResource conn = getConnection();

    if (conn == null) {
      return false;
    }

    if (! _inTransaction) {
      return false;
    }

    _inTransaction = false;

    conn.commit();
    return conn.setAutoCommit(true);
  }

  public void close()
  {
    cleanup();
  }

  /**
   * : the EnvCleanup interface.
   */
  public void cleanup()
  {
    JdbcConnectionResource conn = _conn;
    _conn = null;

    closeStatements();

    if (conn != null) {
      conn.close();
    }
  }

  public string errorCode()
  {
    return _error.getErrorCode();
  }

  public ArrayValue errorInfo()
  {
    return _error.getErrorInfo();
  }

  protected int getColumnCase()
  {
    return _columnCase;
  }

  /**
   * Executes a statement, returning the number of rows.
   */
  public Value exec(Env env, string query)
  {
    _error.clear();

    JdbcConnectionResource conn = getConnection();

    if (! conn.isConnected()) {
      return BooleanValue.FALSE;
    }

    try {
      PDOStatement stmt = PDOStatement.execute(env, this, _error, query, true);
      _lastExecutedStatement = stmt;

      return LongValue.create(conn.getAffectedRows());
    }
    catch (SQLException e) {
      _error.error(env, e);
      
      e.printStackTrace();

      return BooleanValue.FALSE;
    }
  }

  public Value getAttribute(Env env, int attribute)
  {
    switch (attribute) {
      case ATTR_AUTOCOMMIT:
      {
        if (getAutocommit()) {
          return LongValue.ONE;
        }
        else {
          return LongValue.ZERO;
        }
      }
      case ATTR_CASE:
      {
        return LongValue.create(getCase());
      }
      case ATTR_CLIENT_VERSION:
      {
        string clientVersion = getConnection().getClientInfo(env);

        return env.createString(clientVersion);
      }
      case ATTR_CONNECTION_STATUS:
      {
        try {
          string hostInfo = getConnection().getHostInfo();

          return env.createString(hostInfo);
        }
        catch (SQLException e) {
          env.warning(e);

          return BooleanValue.FALSE;
        }
      }
      case ATTR_DRIVER_NAME:
      {
        string driverName = getConnection().getDriverName();

        return env.createString(driverName);
      }
      case ATTR_ERRMODE:
      {
        return LongValue.create(_error.getErrmode());
      }
      case ATTR_ORACLE_NULLS:
      {
        return LongValue.create(getOracleNulls());
      }
      case ATTR_PERSISTENT:
      {
        return BooleanValue.create(getPersistent());
      }
      case ATTR_PREFETCH:
      {
        return getPrefetch(env);
      }
      case ATTR_SERVER_INFO:
      {
        return getConnection().getServerStat(env);
      }
      case ATTR_SERVER_VERSION:
      {
        return getServerVersion(env);
      }
      case ATTR_TIMEOUT:
      {
        return getTimeout(env);
      }
      default:
        _error.unsupportedAttribute(env, attribute);
        // XXX: check what php does
        return BooleanValue.FALSE;
    }
  }

  public static ArrayValue getAvailableDrivers()
  {
    ArrayValue array = new ArrayValueImpl();

    array.put("mysql");
    array.put("pgsql");
    array.put("java");
    array.put("jdbc");
    array.put("sqlite");

    return array;
  }

  /**
   * Returns the auto commit value for the connection.
   */
  private bool getAutocommit()
  {
    JdbcConnectionResource conn = getConnection();

    if (conn == null) {
      return true;
    }

    return conn.getAutoCommit();
  }

  public int getCase()
  {
    return _columnCase;
  }

  public int getOracleNulls()
  {
    return 0;
  }

  private bool getPersistent()
  {
    return true;
  }

  private Value getPrefetch(Env env)
  {
    env.warning(L.l("driver does not support prefetch"));

    return BooleanValue.FALSE;
  }

  // XXX: might be int return
  private Value getServerVersion(Env env)
  {
    if (_conn == null) {
      return BooleanValue.FALSE;
    }

    try {
      string info = _conn.getServerInfo();

      return env.createString(info);
    }
    catch (SQLException e) {
      _error.error(env, e);

      return BooleanValue.FALSE;
    }
  }

  private Value getTimeout(Env env)
  {
    env.warning(L.l("Driver does not support timeouts"));

    return BooleanValue.FALSE;
  }

  public string lastInsertId(Env env, @Optional Value nameV)
  {
    if (! nameV.isDefault()) {
      throw new UnimplementedException("lastInsertId with name");
    }

    if (_lastExecutedStatement == null) {
      return "0";
    }

    try {
      string lastInsertId = _lastExecutedStatement.getStatement().lastInsertId(env);

      if (lastInsertId == null) {
        return "0";
      }

      return lastInsertId;
    }
    catch (SQLException e) {
      _error.error(env, e);
      return "0";
    }
  }

  /**
   * Prepares a statement for execution.
   */
  public Value prepare(Env env,
                       string query,
                       @Optional ArrayValue driverOptions)
  {
    if (! isConnected()) {
      return BooleanValue.FALSE;
    }

    try {
      //closeStatements();

      PDOStatement pdoStatement
        = PDOStatement.prepare(env, this, _error, query, false);

      _lastPDOStatement = pdoStatement;

      if (_statementClassName != null) {
        QuercusClass cls = env.getClass(_statementClassName);

        Value phpObject = cls.callNew(env, pdoStatement, _statementClassArgs);

        return phpObject;
      }
      else {
        return env.wrapJava(pdoStatement);
      }
    }
    catch (SQLException e) {
      _error.error(env, e);

      return BooleanValue.FALSE;
    }
  }

  /**
   * Queries the database
   */
  public Value query(Env env,
                     string query,
                     @Optional int mode,
                     @Optional @ReadOnly Value[] args)
  {
    _error.clear();

    JdbcConnectionResource conn = getConnection();

    if (! conn.isConnected()) {
      return BooleanValue.FALSE;
    }

    try {
      //closeStatements();

      PDOStatement pdoStatement
         = PDOStatement.execute(env, this, _error, query, true);

      if (mode != 0) {
        pdoStatement.setFetchMode(env, mode, args);
      }

      _lastPDOStatement = pdoStatement;

      if (_statementClassName != null) {
        QuercusClass cls = env.getClass(_statementClassName);

        return cls.callNew(env, pdoStatement, _statementClassArgs);
      }
      else {
        return env.wrapJava(pdoStatement);
      }
    }
    catch (SQLException e) {
      _error.error(env, e);

      return BooleanValue.FALSE;
    }
  }

  /**
   * Quotes the string
   */
  public string quote(String query, @Optional int parameterType)
  {
    return "'" + real_escape_string(query) + "'";
  }

  /**
   * Escapes the string.
   */
  public string real_escape_string(String str)
  {
    StringBuilder buf = new StringBuilder();

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
        buf.append('\\');
        buf.append('\'');
        break;
      case '"':
        buf.append('\\');
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

    return buf.ToString();
  }

  /**
   * Rolls a transaction back.
   */
  public bool rollBack(Env env)
  {
    JdbcConnectionResource conn = getConnection();

    if (conn == null) {
      return false;
    }

    if (! _inTransaction) {
      return false;
    }

    _inTransaction = false;

    conn.rollback();
    return conn.setAutoCommit(true);
  }

  public bool setAttribute(Env env, int attribute, Value value)
  {
    return setAttribute(env, attribute, value, false);
  }

  private bool setAttribute(Env env,
                               int attribute, Value value, bool isInit)
  {
    switch (attribute) {
      case ATTR_AUTOCOMMIT:
        return setAutocommit(env, value.toBoolean());

      case ATTR_ERRMODE:
        return _error.setErrmode(env, value.toInt());

      case ATTR_CASE:
        return setCase(env, value.toInt());

      case ATTR_ORACLE_NULLS:
        return setOracleNulls(env, value.toInt());

      case ATTR_STRINGIFY_FETCHES:
        return setStringifyFetches(value.toBoolean());

      case ATTR_STATEMENT_CLASS:
      {
        if (! value.isArray()) {
          env.warning(L.l("ATTR_STATEMENT_CLASS attribute must be an array"));

          return false;
        }

        return setStatementClass(env, value.toArrayValue(env));
      }
      case ATTR_EMULATE_PREPARES:
      {
        return setEmulatePrepares(value.toBoolean());
      }

      case MYSQL_ATTR_INIT_COMMAND:
      {
        return setInitQuery(value.ToString());
      }

    }

    if (isInit) {
      switch (attribute) {
        // XXX: there may be more of these
        case ATTR_TIMEOUT:
          return setTimeout(value.toInt());

        case ATTR_PERSISTENT:
          return setPersistent(value.toBoolean());
      }
    }

    // XXX: check what PHP does
    _error.unsupportedAttribute(env, attribute);
    return false;
  }

  /**
   * Sets the auto commit, if true commit every statement.
   * @return true on success, false on error.
   */
  private bool setAutocommit(Env env, bool autoCommit)
  {
    JdbcConnectionResource conn = getConnection();

    if (conn == null) {
      return false;
    }

    return conn.setAutoCommit(autoCommit);
  }

  /**
   * Force column names to a specific case.
   *
   * <dl>
   * <dt>{@link CASE_LOWER}
   * <dt>{@link CASE_NATURAL}
   * <dt>{@link CASE_UPPER}
   * </dl>
   */
  private bool setCase(Env env, int value)
  {
    switch (value) {
      case CASE_LOWER:
        _columnCase = JdbcResultResource.COLUMN_CASE_LOWER;
        return true;

      case CASE_NATURAL:
        _columnCase = JdbcResultResource.COLUMN_CASE_NATURAL;
        return true;

      case CASE_UPPER:
        _columnCase = JdbcResultResource.COLUMN_CASE_UPPER;
        return true;

      default:
        _error.unsupportedAttributeValue(env, env);
        return false;
    }
  }

  /**
   * Sets whether or not the convert nulls and empty strings, works for
   * all drivers.
   *
   * <dl>
   * <dt> {@link NULL_NATURAL}
   * <dd> no conversion
   * <dt> {@link NULL_EMPTY_STRING}
   * <dd> empty string @is converted to NULL
   * <dt> {@link NULL_TO_STRING} NULL
   * <dd> @is converted to an empty string.
   * </dl>
   *
   * @return true on success, false on error.
   */
  private bool setOracleNulls(Env env, int value)
  {
    switch (value) {
      case NULL_NATURAL:
      case NULL_EMPTY_STRING:
      case NULL_TO_STRING:
        throw new UnimplementedException();
      default:
        _error.warning(env, L.l("unknown value `{0}'", value));
        _error.unsupportedAttributeValue(env, value);
        return false;
    }
  }

  private bool setPersistent(bool isPersistent)
  {
    return true;
  }

  /**
   * Sets a custom statement  class derived from PDOStatement.
   *
   * @param value an array(classname, array(constructor args)).
   *
   * @return true on success, false on error.
   */
  private bool setStatementClass(Env env, ArrayValue value)
  {
    Value className = value.get(LongValue.ZERO);

    if (! className.isString()) {
      return false;
    }

    _statementClassName = className.ToStringValue(env).ToString();

    Value argV = value.get(LongValue.ONE);

    if (argV.isArray()) {
      ArrayValue array = argV.toArrayValue(env);
      _statementClassArgs = array.valuesToArray();;
    }
    else {
      _statementClassArgs = Value.NULL_ARGS;
    }

    return true;
  }

  private bool setEmulatePrepares(bool isEmulate)
  {
    if (_isEmulatePrepares == isEmulate) {
      return true;
    }
    else if (_conn != null) {
      return false;
    }
    else {
      _isEmulatePrepares = isEmulate;

      return true;
    }
  }

  private bool setInitQuery(String query)
  {
    _initQuery = query;

    return true;
  }

  /**
   * Convert numeric values to strings when fetching.
   *
   * @return true on success, false on error.
   */
  private bool setStringifyFetches(bool stringifyFetches)
  {
    throw new UnimplementedException();
  }

  private bool setTimeout(int timeoutSeconds)
  {
    throw new UnimplementedException();
  }

  /**
   * Opens a connection based on the dsn.
   */
  private JdbcConnectionResource getConnection(Env env,
                                               string dsn,
                                               string user,
                                               string pass)
    
  {
    if (dsn.startsWith("mysql:")) {
      return getMysqlConnection(env, dsn, user, pass);
    }
    else if (dsn.startsWith("pgsql:")) {
      return getPgsqlDataSource(env, dsn, user, pass);
    }
    else if (dsn.startsWith("java")) {
      return getJndiDataSource(env, dsn, user, pass);
    }
    else if (dsn.startsWith("jdbc:")) {
      return getJdbcDataSource(env, dsn, user, pass);
    }
    else if (dsn.startsWith("resin:")) {
      return getResinDataSource(env, dsn);
    }
    else if (dsn.startsWith("sqlite:")) {
      return getSqliteDataSource(env, dsn);
    }
    else {
      env.error(L.l("'{0}' @is an unknown PDO data source.",
                    dsn));

      return null;
    }
  }

  /**
   * Opens a mysql connection based on the dsn.
   */
  private JdbcConnectionResource getMysqlConnection(Env env,
                                                    string dsn,
                                                    string user,
                                                    string pass)
    
  {
    HashMap<String,String> attrMap = parseAttr(dsn, dsn.indexOf(':'));

    string host = "localhost";
    int port = -1;
    string dbName = null;

    for (Map.Entry<String,String> entry : attrMap.entrySet()) {
      string key = entry.getKey();
      string value = entry.getValue();

      if ("host".equals(key)) {
        host = value;
      }
      else if ("port".equals(key)) {
        try {
          port = Integer.parseInt(value);
        } catch (NumberFormatException e) {
          env.warning(e);
        }
      }
      else if ("dbname".equals(key)) {
        dbName = value;
      }
      else if ("user".equals(key)) {
        user = value;
      }
      else if ("password".equals(key)) {
        pass = value;
      }
      else {
        env.warning(L.l("pdo dsn attribute not supported: {0}={1}", key, value));
      }
    }

    // PHP doc does not sure user and password as attributes for mysql,
    // but in the pgsql doc  the dsn specified user and
    // password override arguments
    // passed to constructor

    string socket = null;
    int flags = 0;
    string driver = null;
    string url = null;
    bool isNewLink = false;

    Mysqli mysqli =  new Mysqli(env, host, user, pass, dbName, port,
                                socket, flags, driver, url, isNewLink,
                                _isEmulatePrepares, _initQuery);

    return mysqli;
  }

  /**
   * Opens a postgres connection based on the dsn.
   */
  private JdbcConnectionResource getPgsqlDataSource(Env env,
                                                    string dsn,
                                                    string user,
                                                    string pass)
  {
    HashMap<String,String> attrMap = parseAttr(dsn, dsn.indexOf(':'));

    string host = "localhost";
    int port = 5432;
    string dbName = null;

    for (Map.Entry<String,String> entry : attrMap.entrySet()) {
      string key = entry.getKey();
      string value = entry.getValue();

      if ("host".equals(key)) {
        host = value;
      }
      else if ("port".equals(key)) {
        try {
          port = Integer.parseInt(value);
        } catch (NumberFormatException e) {
          env.warning(e);
        }
      }
      else if ("dbname".equals(key)) {
        dbName = value;
      }
      else if ("user".equals(key)) {
        user = value;
      }
      else if ("password".equals(key)) {
        pass = value;
      }
      else {
        env.warning(L.l("pdo dsn attribute not supported: {0}={1}", key, value));
      }
    }

    string driver = null;
    string url = null;

    Postgres postgres
      = new Postgres(env, host, user, pass, dbName, port, driver, url);

    return postgres;
  }

  /**
   * Opens a resin connection based on the dsn.
   */
  private JdbcConnectionResource getResinDataSource(Env env, string dsn)
  {
    try {
      string driver = "com.caucho.db.jdbc.ConnectionPoolDataSourceImpl";
      string url = "jdbc:" + dsn;

      DataSource ds = env.getDataSource(driver, url);

      return new DataSourceConnection(env, ds, null, null);
    }
    catch (Exception e) {
      env.warning(e);

      return null;
    }
  }

  /**
   * Opens a connection based on the dsn.
   */
  private JdbcConnectionResource getJndiDataSource(Env env,
                                                   string dsn,
                                                   string user,
                                                   string pass)
  {
    DataSource ds = null;

    try {
      Context ic = new InitialContext();

      ds = (DataSource) ic.lookup(dsn);
    } catch (NamingException e) {
      log.log(Level.FINE, e.ToString(), e);
    }

    if (ds == null) {
      env.error(L.l("'{0}' @is an unknown PDO JNDI data source.", dsn));

      return null;
    }

    return new DataSourceConnection(env, ds, user, pass);
  }

  /**
   * Opens a connection based on the dsn.
   */
  private JdbcConnectionResource getJdbcDataSource(Env env,
                                                   string dsn,
                                                   string user,
                                                   string pass)
  {
    JdbcDriverContext context = env.getQuercus().getJdbcDriverContext();

    int i = dsn.indexOf("jdbc:");
    int j = dsn.indexOf("://", i + 5);

    if (j < 0) {
      j = dsn.indexOf(":", i + 5);
    }

    if (j < 0) {
      return null;
    }

    string protocol = dsn.substring(i + 5, j);

    if ("sqlite".equals(protocol)) {
      return new SQLite3(env, dsn);
    }

    string driver = context.getDriver(protocol);

    if (driver == null) {
      return null;
    }

    try {
      DataSource ds = env.getDataSource(driver, dsn.ToString());

      return new DataSourceConnection(env, ds, user, pass);
    }
    catch (Exception e) {
      env.warning(e);

      return null;
    }
  }

  /**
   * Opens a resin connection based on the dsn.
   */
  private JdbcConnectionResource getSqliteDataSource(Env env, string dsn)
  {
    string jdbcUrl = "jdbc:" + dsn;

    return new SQLite3(env, jdbcUrl);
  }

  private HashMap<String,String> parseAttr(String dsn, int i)
  {
    HashMap<String,String> attr = new LinkedHashMap<String,String>();

    int length = dsn.length();

    StringBuilder sb = new StringBuilder();

    for (; i < length; i++) {
      char ch = dsn[i];

      if (! Character.isJavaIdentifierStart(ch))
        continue;

      for (;
           i < length && Character.isJavaIdentifierPart((ch = dsn[i]));
           i++) {
        sb.append(ch);
      }

      string name = sb.ToString();
      sb.setLength(0);

      for (; i < length && ((ch = dsn[i]) == ' ' || ch == '='); i++) {
      }

      for (; i < length && (ch = dsn[i]) != ' ' && ch != ';'; i++) {
        sb.append(ch);
      }

      string value = sb.ToString();
      sb.setLength(0);

      attr.put(name, value);
    }

    return attr;
  }

  public string ToString()
  {
    // do not show password!
    if (_dsn == null)
      return "PDO[]";

    if (_dsn.indexOf("pass") < 0)
      return "PDO[" + _dsn + "]";

    int i = _dsn.lastIndexOf(':');

    if (i < 0)
      return "PDO[]";

    if (_dsn.startsWith("java"))
      return "PDO[" + _dsn + "]";

    StringBuilder str = new StringBuilder();
    str.append("PDO[");

    str.append(_dsn, 0, i + 1);

    HashMap<String,String> attr = parseAttr(_dsn, i);

    bool first = true;

    for (Map.Entry<String,String> entry : attr.entrySet()) {
      string key = entry.getKey();
      string value = entry.getValue();

      if ("password".equalsIgnoreCase(key))
        value = "XXXXX";
      else if ("passwd".equalsIgnoreCase(key))
        value = "XXXXX";
      else if ("pass".equalsIgnoreCase(key))
        value = "XXXXX";

      if (! first)
        str.append(' ');

      first = false;

      str.append(key);
      str.append("=");
      str.append(value);
    }

    str.append("]");

    return str.ToString();
  }
}
}
