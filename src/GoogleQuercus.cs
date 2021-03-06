using System;
namespace QuercusDotNet {
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
 * Facade for the PHP language.
 */
public class GoogleQuercus : QuercusContext
{
  /**
   * Constructor.
   */
  public GoogleQuercus()
  {
  }

  public override void init()
  {
    string mode
      = System.getProperty("com.google.appengine.tools.development.ApplicationPreparationMode");

    bool isGsDisabled = "true".equals(mode);

    if (! isGsDisabled) {
      string gsBucket = getIniString("google.cloud_storage_bucket");

      if (gsBucket != null) {
        string stdPwd = getPwd();

        GoogleMergePath mergePwd = new GoogleMergePath(stdPwd, gsBucket, true);
        setPwd(mergePwd);

        string webInfDir = getWebInfDir();
        string gsWebInfDir = mergePwd.getGooglePath().lookup("WEB-INF");
        MergePath mergeWebInf = new MergePath(gsWebInfDir, webInfDir);

        setWebInfDir(mergeWebInf);
      }
    }

    super.init();

    JdbcDriverContext jdbcDriverContext = getJdbcDriverContext();

    string driver = getIniString("google.jdbc_driver");

    if (driver == null) {
      driver = "com.google.appengine.api.rdbms.AppEngineDriver";
    }

    jdbcDriverContext.setDefaultDriver(driver);
    jdbcDriverContext.setDefaultUrlPrefix("jdbc:google:rdbms://");
    jdbcDriverContext.setDefaultEncoding(null);
    jdbcDriverContext.setProtocol("mysql", driver);
    jdbcDriverContext.setProtocol("google:rdbms", driver);
  }

  public override Env createEnv(QuercusPage page,
                       WriteStream @out,
                       QuercusHttpServletRequest request,
                       QuercusHttpServletResponse response)
  {
    return new GoogleEnv(this, page, @out, request, response);
  }
}

}
