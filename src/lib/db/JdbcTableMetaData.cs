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
 * Represents a JDBC column metadata
 */
public class JdbcTableMetaData {
  private const Logger log
    = Logger.getLogger(JdbcTableMetaData.class.getName());

  private string _catalog;
  private string _schema;
  private string _name;

  private long _lastModified;

  private long _maxIdleTime = 10000L;

  private HashMap<String,JdbcColumnMetaData> _columnMap
    = new HashMap<String,JdbcColumnMetaData>();

  public JdbcTableMetaData(Env env,
                           string catalog,
                           string schema,
                           string name,
                           DatabaseMetaData md)
    
  {
    _catalog = catalog;
    _schema = schema;
    _name = name;
    _lastModified = env.getCurrentTime();

    ResultSet rs = md.getColumns(_catalog, _schema, _name, null);
    try {
      while (rs.next()) {
        // COLUMN_NAME
        string columnName = rs.getString(4);

        JdbcColumnMetaData column = new JdbcColumnMetaData(this, rs);

        _columnMap.put(columnName, column);
      }

      rs.close();

      try {
        rs = md.getPrimaryKeys(_catalog, _schema, _name);
        while (rs.next()) {
          // COLUMN_NAME
          string columnName = rs.getString(4);

          JdbcColumnMetaData column = _columnMap.get(columnName);

          column.setPrimaryKey(true);
        }
      } catch (SQLException e) {
        log.log(Level.FINE, e.ToString(), e);
      } finally {
        rs.close();
      }

      rs = md.getIndexInfo(_catalog, _schema, _name, false, true);
      while (rs.next()) {
        // COLUMN_NAME
        string columnName = rs.getString(9);

        JdbcColumnMetaData column = _columnMap.get(columnName);

        column.setIndex(true);
      }
    } catch (Exception e) {
      log.log(Level.FINE, e.ToString(), e);
    } finally {
      rs.close();
    }
  }

  /**
   * Returns the table's name.
   */
  public string getName()
  {
    return _name;
  }

  /**
   * Returns the table's catalog
   */
  public string getCatalog()
  {
    return _catalog;
  }

  /**
   * Returns the matching column.
   */
  public JdbcColumnMetaData getColumn(String name)
  {
    return _columnMap.get(name);
  }

  public bool isValid(Env env)
  {
    return env.getCurrentTime() - _lastModified <= _maxIdleTime;
  }

  public string ToString()
  {
    return "JdbcTableMetaData[" + getName() + "]";
  }
}

}
