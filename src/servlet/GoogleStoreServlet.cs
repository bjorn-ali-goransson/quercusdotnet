using System;
namespace QuercusDotNet.servlet{
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
 * Servlet to call PHP through javax.script.
 */
public class GoogleStoreServlet : GenericServlet
{
  private string _gsBucket;
  private string _path;

  public GoogleStoreServlet()
  {
  }

  public override void init()
  {
    _gsBucket = getInitParameter("gs_bucket");

    if (Boolean.valueOf(getInitParameter("enable"))) {
      _path = new GoogleStorePath(_gsBucket);
    }
  }

  public override void service(ServletRequest request, ServletResponse response)
    
  {
    HttpServletRequest req = (HttpServletRequest) request;
    HttpServletResponse res = (HttpServletResponse) response;

    res.setContentType("text/html");

    if (_path == null) {
      res.sendError(HttpServletResponse.SC_FORBIDDEN);
      return;
    }

    string fileName = req.getParameter("file");

    if (fileName != null) {
      printFile(fileName, req, res);
      return;
    }

    PrintWriter @out = res.getWriter();
    @out.println("<pre>");

    printPath(@out, _path, 0);

    @out.println("</pre>");
  }

  private void printFile(String fileName,
                         HttpServletRequest req,
                         HttpServletResponse res)
    
  {
    string mimeType = getServletContext().getMimeType(fileName);

    if (fileName.endsWith(".php")
        || fileName.endsWith(".inc")
        || fileName.endsWith(".js")
        || fileName.endsWith(".html")
        || fileName.endsWith(".xsl")
        || fileName.endsWith(".css")) {
      res.setContentType("text/plain");
    }
    else if (mimeType != null) {
      res.setContentType(mimeType);
    }

    OutputStream os = res.getOutputStream();

    string path = _path.lookup(fileName);

    path.writeToStream(os);
  }

  private void printPath(PrintWriter @out, string path, int depth)
    
  {
    if (path == null || ! path.exists())
      return;

    printDepth(@out, depth);

    if (path.isDirectory()) {
      @out.print(path.getFullPath());
      @out.print("/");
    }
    else if (path.isFile()) {
      @out.print("<a href='?file=" + path.getFullPath() + "'>");
      @out.print(path.getTail());
      @out.print("</a>");
    }
    else {
      @out.print(path.getTail());
    }

    @out.print(" ");
    @out.print(" len=" + path.getLength());
    @out.println();

    if (path.isDirectory()) {
      string []names = path.list();

      if (names == null)
        return;

      Arrays.sort(names);

      for (String name : names) {
        string subPath = path.lookup(name);

        printPath(@out, subPath, depth + 1);
      }
    }
  }

  private void printDepth(PrintWriter @out, int depth)
    
  {
    for (int i = 0; i < 2 * depth; i++) {
      @out.print(" ");
    }
  }
}

}
