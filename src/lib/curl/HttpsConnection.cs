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
 * Represents a HttpURLConnection wrapper.
 */
public class HttpsConnection
  : CurlHttpConnection
{
  protected HttpsConnection(URL url,
                            string username,
                            string password)
    
  {
    super(url, username, password);
  }

  public HttpsConnection(URL url,
                         string username,
                         string password,
                         URL proxyURL,
                         string proxyUsername,
                         string proxyPassword,
                         string proxyType)
    
   : base(url, username, password,
          proxyURL, proxyUsername, proxyPassword, proxyType) {
  }
  
  protected override void init(CurlResource curl)
    
  {
    Proxy proxy = getProxy();

    URLConnection conn;
    
    HttpsURLConnection httpsConn = null;

    if (proxy != null)
      conn = getURL().openConnection(proxy);
    else
      conn = getURL().openConnection();
    
    if (conn instanceof HttpsURLConnection) {
      httpsConn = (HttpsURLConnection) conn;
      
      if (! curl.getIsVerifySSLPeer()
          || ! curl.getIsVerifySSLCommonName()
          || ! curl.getIsVerifySSLHostname()) {
        HostnameVerifier hostnameVerifier
          = CurlHostnameVerifier.create(curl.getIsVerifySSLPeer(),
                                        curl.getIsVerifySSLCommonName(),
                                        curl.getIsVerifySSLHostname());
        
        httpsConn.setHostnameVerifier(hostnameVerifier);
      }
    }

    setConnection(conn);
  }
  
  /**
   * Connects to the server.
   */
  /*
  public override void connect(CurlResource curl)
    
            IOException
  {
    try {
      super.connect(curl);
      
      ((HttpsURLConnection)getConnection()).getServerCertificates();
    }
    catch (SSLPeerUnverifiedException e) {
      if (curl.getIsVerifySSLPeer())
        throw e;
    }
  }
  */
}
}
