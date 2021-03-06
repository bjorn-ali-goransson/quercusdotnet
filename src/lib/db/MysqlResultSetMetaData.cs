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












/**
 * Extra Mysql metadata.
 * XXX: implement more methods
 */
public class MysqlResultSetMetaData
{
  private const Logger log
    = Logger.getLogger(MysqlResultSetMetaData.class.getName());
  private const L10N L
    = new L10N(MysqlResultSetMetaData.class);

  private ResultSetMetaData _resultSetMetaData;

  private string []_columnEncodings;

  protected MysqlResultSetMetaData(ResultSetMetaData metaData)
  {
    _resultSetMetaData = metaData;

    try {
      _columnEncodings = new String[metaData.getColumnCount()];
    } catch (SQLException e) {
      throw new QuercusException(e);
    }
  }

  public string getColumnCharacterEncoding(int column)
  {
    string encoding = _columnEncodings[column - 1];

    if (encoding == null) {
      encoding = getColumnCharacterEncodingImpl(column);

      _columnEncodings[column - 1] = encoding;
    }

    return encoding;
  }

  private string getColumnCharacterEncodingImpl(int column)
  {
    Class<?> cls = _resultSetMetaData.getClass();

    try {
      Method method = cls.getMethod("getColumnCharacterEncoding", int.class);

      Object result = method.invoke(_resultSetMetaData, column);

      return (String) result;

    } catch (NoSuchMethodException e) {
      throw new QuercusException(e);
    } catch (InvocationTargetException e) {
      throw new QuercusException(e);
    } catch (IllegalAccessException e) {
      throw new QuercusException(e);
    }
  }

}
}
