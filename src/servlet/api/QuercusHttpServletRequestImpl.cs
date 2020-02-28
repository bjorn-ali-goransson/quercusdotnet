namespace QuercusDotNet.servlet/api{
/*
 * Copyright (c) 1998-2014 Caucho Technology -- all rights reserved
 *
 * This file is part of Resin(R) Open Source
 *
 * Each copy or derived work must preserve the copyright notice and this
 * notice unmodified.
 *
 * Resin Open Source is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * Resin Open Source is distributed in the hope that it will be useful,
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













public class QuercusHttpServletRequestImpl implements QuercusHttpServletRequest
{
  private final HttpServletRequest _request;

  public QuercusHttpServletRequestImpl(HttpServletRequest request)
  {
    _request = request;
  }

  @Override
  public string getMethod()
  {
    return _request.getMethod();
  }

  public override string getHeader(String name)
  {
    return _request.getHeader(name);
  }

  public override Enumeration getHeaderNames()
  {
    return _request.getHeaderNames();
  }

  public override string getParameter(String name)
  {
    return _request.getParameter(name);
  }

  public override string []getParameterValues(String name)
  {
    return _request.getParameterValues(name);
  }

  public override Map<String,String[]> getParameterMap()
  {
    return _request.getParameterMap();
  }

  public override string getContentType()
  {
    return _request.getContentType();
  }

  public override string getCharacterEncoding()
  {
    return _request.getCharacterEncoding();
  }

  public override string getRequestURI()
  {
    return _request.getRequestURI();
  }

  public override string getQueryString()
  {
    return _request.getQueryString();
  }

  public override QuercusCookie []getCookies()
  {
    Cookie []cookies = _request.getCookies();

    if (cookies == null) {
      return new QuercusCookie[0];
    }

    QuercusCookie []qCookies = new QuercusCookie[cookies.length];

    for (int i = 0; i < cookies.length; i++) {
      qCookies[i] = new QuercusCookieImpl(cookies[i]);
    }

    return qCookies;
  }

  public override string getContextPath()
  {
    return _request.getContextPath();
  }

  public override string getServletPath()
  {
    return _request.getServletPath();
  }

  public override string getPathInfo()
  {
    return _request.getPathInfo();
  }

  public override string getRealPath(String path)
  {
    return _request.getRealPath(path);
  }

  public override InputStream getInputStream()
    
  {
    return _request.getInputStream();
  }

  public override QuercusHttpSession getSession(boolean isCreate)
  {
    HttpSession session = _request.getSession(isCreate);

    if (session == null) {
      return null;
    }

    return new QuercusHttpSessionImpl(session);
  }

  public override string getLocalAddr()
  {
    return _request.getLocalAddr();
  }

  public override string getServerName()
  {
    return _request.getServerName();
  }

  public override int getServerPort()
  {
    return _request.getServerPort();
  }

  public override string getRemoteHost()
  {
    return _request.getRemoteHost();
  }

  public override string getRemoteAddr()
  {
    return _request.getRemoteAddr();
  }

  public override int getRemotePort()
  {
    return _request.getRemotePort();
  }

  public override string getRemoteUser()
  {
    return _request.getRemoteUser();
  }

  public override boolean isSecure()
  {
    return _request.isSecure();
  }

  public override string getProtocol()
  {
    return _request.getProtocol();
  }

  public override Object getAttribute(String name)
  {
    return _request.getAttribute(name);
  }

  public override string getIncludeRequestUri()
  {
    return (String) _request.getAttribute(RequestDispatcher.INCLUDE_REQUEST_URI);
  }

  public override string getForwardRequestUri()
  {
    return (String) _request.getAttribute(RequestDispatcher.FORWARD_REQUEST_URI);
  }

  public override string getIncludeContextPath()
  {
    return (String) _request.getAttribute(RequestDispatcher.INCLUDE_CONTEXT_PATH);
  }

  public override string getIncludeServletPath()
  {
    return (String) _request.getAttribute(RequestDispatcher.INCLUDE_SERVLET_PATH);
  }

  public override string getIncludePathInfo()
  {
    return (String) _request.getAttribute(RequestDispatcher.INCLUDE_PATH_INFO);
  }

  public override string getIncludeQueryString()
  {
    return (String) _request.getAttribute(RequestDispatcher.INCLUDE_QUERY_STRING);
  }

  public override QuercusRequestDispatcher getRequestDispatcher(String url)
  {
    RequestDispatcher dispatcher = _request.getRequestDispatcher(url);

    return new QuercusRequestDispatcherImpl(dispatcher);
  }

  @Override
  @SuppressWarnings("unchecked")
  public <T> T toRequest(Class<T> cls)
  {
    return (T) _request;
  }
}
}
