using System;
namespace QuercusDotNet.lib.curl {
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
 * Represents a POST Http request.
 */
public class HttpPostRequest
  : CurlHttpRequest
{
  private PostBody _body;

  public HttpPostRequest(CurlResource curlResource)
  {
    super(curlResource);
  }

  /**
   * Initializes the connection.
   */
  protected bool init(Env env)
    
  {
    if (! super.init(env)) {
      return false;
    }

    _body = PostBody.create(env, getCurlResource());

    if (_body == null) {
      return false;
    }

    CurlHttpConnection conn = getHttpConnection();

    if (conn.getRequestProperty("Content-Type") == null) {
      conn.setRequestProperty("Content-Type",
                              _body.getContentType());
    }

    if (conn.getRequestProperty("Content-Length") == null) {
      long contentLength = _body.getContentLength();

      if (contentLength >= 0) {
        conn.setRequestProperty("Content-Length",
                                String.valueOf(contentLength));
      }
      else if (false && _body.isChunked()) {
        conn.setRequestProperty("Transfer-Encoding", "chunked");
      }

    }

    conn.setDoOutput(true);

    return true;
  }

  /**
   * Transfer data to the server.
   */
  protected void transfer(Env env)
    
  {
    super.transfer(env);

    CurlHttpConnection conn = getHttpConnection();
    OutputStream @out = conn.getOutputStream();

    //out = new TestOutputStream(out);

    _body.writeTo(env, out);
  }

  /*
  static class TestOutputStream : OutputStream
  {
    OutputStream _out;
    FileOutputStream _ps;

    TestOutputStream(OutputStream out)
      
    {
      _out = out;

      _ps = new FileOutputStream("c:/out.txt");
    }

    public void close()
      
    {
      _out.close();
      _ps.close();
    }

    public void flush()
      
    {
      _out.flush();
      _ps.close();
    }

    public void write(int b)
      
    {
      _out.write(b);
      _ps.write(b);
    }

    public void write(byte b[])
      
    {
      _out.write(b);
      _ps.write(b);
    }

    public void write(byte b[], int offset, int len)
      
    {
      _out.write(b, offset, len);
      _ps.write(b, offset, len);
    }
  }
  */

}
}
