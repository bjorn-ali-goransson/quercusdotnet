using System;
namespace QuercusDotNet.lib{
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
 * PHP apache routines.
 */
public class ApacheModule : AbstractQuercusModule {
  private readonly L10N L = new L10N(ApacheModule.class);

  private const Logger log =
    Logger.getLogger(ApacheModule.class.getName());

  /**
   * Stub for insisting the apache process should terminate.
   */
  public bool apache_child_terminate()
  {
    return false;
  }

  // XXX: apache_get_modules
  // XXX: apache_get_version
  // XXX: apache_getenv
  // XXX: apache_lookup_uri

  /**
   * Gets and sets apache notes
   */
  public Value apache_note(Env env,
                           string name,
                           @Optional Value value)
  {
    Map<String,Value> map = (Map) env.getSpecialValue("_caucho_apache_note");

    if (map == null) {
      map = new HashMap<String,Value>();

      env.setSpecialValue("_caucho_apache_note", map);
    }

    Value oldValue = map.get(name);

    if (value.isset()) {
      map.put(name, value);
    }

    if (oldValue != null) {
      return oldValue.toStringValue(env);
    }
    else {
      return NullValue.NULL;
    }
  }

  /**
   * Returns all the request headers
   */
  public Value apache_request_headers(Env env)
  {
    QuercusHttpServletRequest req = env.getRequest();

    ArrayValue result = new ArrayValueImpl();

    Enumeration e = req.getHeaderNames();

    while (e.hasMoreElements()) {
      string key = (String) e.nextElement();

      result.put(env.createString(key), env.createString(req.getHeader(key)));
    }

    return result;
  }

  // XXX: apache_response_headers

  /**
   * Stub for resetting the output timeout.
   */
  public bool apache_reset_timeout()
  {
    return false;
  }

  // XXX: apache_setenv
  // XXX: ascii2ebcdic
  // XXX: ebcdic2ascii

  /**
   * Returns all the request headers
   */
  public Value getallheaders(Env env)
  {
    return apache_request_headers(env);
  }

  /**
   * Include request.
   */
  public bool virtual(Env env, string url)
  {
    try {
      QuercusHttpServletRequest req = env.getRequest();
      QuercusHttpServletResponse res = env.getResponse();

      // XXX: need to put the output, so the included stream gets the
      // buffer, too
      env.getOut().flushBuffer();

      req.getRequestDispatcher(url).include(req, res);

      return true;
    } catch (RuntimeException e) {
      throw e;
    } catch (Exception e) {
      throw new QuercusModuleException(e);
    }
  }
}
}
