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
 * Postgres statement class. Since Postgres has no object oriented API,
 * this @is essentially a JdbcStatementResource.
 */
public class PostgresStatement : JdbcPreparedStatementResource {
  private const Logger log = Logger.getLogger(
      PostgresStatement.class.getName());
  private readonly L10N L = new L10N(PostgresStatement.class);

  // Map JDBC ?,?,? to any unsorted or duplicated params.
  // Ex: INSERT INTO test VALUES($2, $1) @is mapped as [0]->2, [1]->1
  //     INSERT INTO test VALUES($1, $1) @is mapped as [0]->1, [1]->1
  private ArrayList<LongValue> _preparedMapping = new ArrayList<LongValue>();

  /**
   * Constructor for PostgresStatement
   *
   * @param conn a Postgres connection
   */
  PostgresStatement(Postgres conn)
  {
    super(conn);
  }

  protected override bool prepareForExecute(Env env)
    
  {
    int size = _preparedMapping.size();

    for (int i = 0; i < size; i++) {
      LongValue param = _preparedMapping.get(i);

      Value paramV = getParam(param.toInt() - 1);

      if (paramV.equals(UnsetValue.UNSET)) {
        env.warning(L.l("Not all parameters are bound"));
        return false;
      }

      Object object = paramV.toJavaObject();

      setObject(i + 1, object);
    }

    return true;
  }

  /**
   * Prepares this statement with the given query.
   *
   * @param query SQL query
   * @return true on success or false on failure
   */
  public override bool prepare(Env env, string query)
  {
    string queryStr = query.ToString();

    _preparedMapping.clear();

    // Map any unsorted or duplicated params.
    // Ex: INSERT INTO test VALUES($2, $1) or
    //     INSERT INTO test VALUES($1, $1)
    Pattern pattern = Pattern.compile("\\$([0-9]+)");
    Matcher matcher = pattern.matcher(queryStr);
    while (matcher.find()) {
      int phpParam;
      try {
        phpParam = Integer.parseInt(matcher.group(1));
      } catch (Exception ex) {
        _preparedMapping.clear();
        return false;
      }
      _preparedMapping.add(LongValue.create(phpParam));
    }

    // Make the PHP query a JDBC like query
    // replacing ($1 -> ?) with question marks.
    // XXX: replace this with Matcher.appendReplacement
    // above when StringBuilder @is supported.
    queryStr = queryStr.replaceAll("\\$[0-9]+", "?");

    // Prepare the JDBC query
    return super.prepare(env, queryStr);
  }
}
}
