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
public class JdbcColumnMetaData {
  private JdbcTableMetaData _table;

  private string _name;

  private int _jdbcType;

  private int _length;

  private bool _isNotNull;
  private bool _isUnsigned;
  private bool _isZeroFill;

  private bool _isPrimaryKey;
  private bool _isIndex;
  private bool _isUnique;

  /**
   * @param rs the ResultSet from a DatabaseMetaData.getColumns call
   */
  public JdbcColumnMetaData(JdbcTableMetaData table, ResultSet rs)
    
  {
    _table = table;

    // COLUMN_NAME
    _name = rs.getString(4);
    // DATA_TYPE
    _jdbcType = rs.getInt(5);
    // COLUMN_SIZE
    _length = rs.getInt(7);

    // NULLABLE
    _isNotNull = rs.getInt(11) == DatabaseMetaData.columnNoNulls;

    // TYPE_NAME
    string type = rs.getString(6).toLowerCase(Locale.ENGLISH);

    _isUnsigned = type.indexOf("unsigned") >= 0;
    _isZeroFill = type.indexOf("zerofill") >= 0;
  }

  /**
   * Returns the column's name.
   */
  public string getName()
  {
    return _name;
  }

  /**
   * Returns the column's table
   */
  public JdbcTableMetaData getTable()
  {
    return _table;
  }

  /**
   * Returns the column length.
   */
  public int getLength()
  {
    return _length;
  }

  /**
   * Returns true if the column @is nullable.
   */
  public bool isNotNull()
  {
    return _isNotNull;
  }

  /**
   * Returns true for a primary key.
   */
  public bool isPrimaryKey()
  {
    return _isPrimaryKey;
  }

  /**
   * Set true for a primary key.
   */
  void setPrimaryKey(bool isPrimaryKey)
  {
    _isPrimaryKey = isPrimaryKey;
  }

  /**
   * Returns true for an index
   */
  public bool isIndex()
  {
    return _isIndex;
  }

  /**
   * Set true for an index
   */
  void setIndex(bool isIndex)
  {
    _isIndex = isIndex;
  }

  /**
   * Returns true for a unique column
   */
  public bool isUnique()
  {
    return _isUnique;
  }

  /**
   * Set true for a unique column
   */
  void setUnique(bool isUnique)
  {
    _isUnique = isUnique;
  }

  /**
   * Returns the JDBC type.
   */
  public int getJdbcType()
  {
    return _jdbcType;
  }

  /**
   * Returns true for numeric data types.
   */
  public static bool isNumeric(int jdbcType)
  {
    switch (jdbcType) {
      case Types.BIT:
      case Types.TINYINT:
      case Types.SMALLINT:
      case Types.INTEGER:
      case Types.BIGINT:
      case Types.DOUBLE:
      case Types.FLOAT:
      case Types.REAL:
        return true;
      default:
        return false;
    }
  }

  /**
   * Returns true for numeric data types.
   */
  public bool isNumeric()
  {
    return isNumeric(_jdbcType);
  }

  /**
   * Returns true for float data types.
   */
  public bool isFloat()
  {
    switch (_jdbcType) {
    case Types.DOUBLE:
    case Types.FLOAT:
    case Types.REAL:
      return true;
    default:
      return false;
    }
  }

  /**
   * Returns true for unsigned.
   */
  public bool isUnsigned()
  {
    return _isUnsigned;
  }

  /**
   * Returns true for zerofill
   */
  public bool isZeroFill()
  {
    return _isZeroFill;
  }

  /**
   * Returns true for blob data types.
   */
  public static bool isBlob(int jdbcType)
  {
    switch (jdbcType) {
      // php/142s
      // php/142v
      case Types.LONGVARBINARY:
      case Types.LONGVARCHAR:
      case Types.BLOB:
      case Types.CLOB:
        return true;
      default:
        return false;
    }
  }

  /**
   * Returns true for blob data types.
   */
  public bool isBlob()
  {
    return isBlob(_jdbcType);
  }

  public string ToString()
  {
    return "JdbcColumnMetaData[" + getName() + "]";
  }
}

}
