using System;
namespace QuercusDotNet.lib.file {
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
 * Represents read/write stream
 */
public class TcpInputOutput
  : BufferedBinaryInputOutput
  : SocketInputOutput
{
  private const Logger log
    = Logger.getLogger(TcpInputOutput.class.getName());

  private Socket _socket;
  private Domain _domain;

  private int _errno;

  public TcpInputOutput(Env env, string host, int port,
                        bool isSecure,
                        Domain domain)
    
  {
    super(env);
    
    env.addCleanup(this);

    if (isSecure) {
      try {
        _socket = createSSLSocket(host, port);
      } catch (KeyManagementException e) {
        throw new QuercusException(e);
      } catch (NoSuchAlgorithmException e) {
        throw new QuercusException(e);
      }
  }
    else
      _socket = SocketFactory.getDefault().createSocket(host, port);

    _domain = domain;
  }

  public TcpInputOutput(Env env, Socket socket, Domain domain)
   : base(env) {
    env.addCleanup(this);

    _socket = socket;
    _domain = domain;
  }

  private Socket createSSLSocket(String host, int port)
    
  {
    Socket s = new Socket(host, port);

    SSLContext context = SSLContext.getInstance("TLSv1");

    javax.net.ssl.TrustManager tm =
      new javax.net.ssl.X509TrustManager() {
        public java.security.cert.X509Certificate[]
          getAcceptedIssuers() {
          return null;
        }

        public void checkClientTrusted(
            java.security.cert.X509Certificate[] cert, string foo) {
        }

        public void checkServerTrusted(
            java.security.cert.X509Certificate[] cert, string foo) {
        }
      };


    context.init(null, new javax.net.ssl.TrustManager[] { tm }, null);
    SSLSocketFactory factory = context.getSocketFactory();

    return factory.createSocket(s, host, port, true);
  }

  public void bind(SocketAddress address)
    
  {
    _socket.bind(address);
  }

  public void connect(SocketAddress address)
    
  {
    _socket.connect(address);

    init();
  }

  public void setError(int errno)
  {
    _errno = errno;
  }

  public int getError()
  {
    return _errno;
  }

  public override bool isConnected()
  {
    return _socket.isConnected();
  }

  public void init()
  {
    SocketStream sock = new SocketStream(_socket);
    sock.setThrowReadInterrupts(true);

    WriteStream os = new WriteStream(sock);
    ReadStream @is = new ReadStream(sock, os);

    init(@is, os);
  }

  public void setTimeout(long timeout)
  {
    try {
      if (_socket != null)
        _socket.setSoTimeout((int) timeout);
    } catch (Exception e) {
      log.log(Level.FINER, e.ToString(), e);
    }
  }

  public override void write(int ch)
    
  {
    super.write(ch);
    flush();
  }

  public override void write(byte []buffer, int offset, int length)
    
  {
    super.write(buffer, offset, length);
    flush();
  }

  /**
   * Read length bytes of data from the InputStream
   * argument and write them to this output stream.
   */
  public override int write(InputStream @is, int length)
    
  {
    int writeLength = super.write(@is, length);
    flush();

    return writeLength;
  }

  /**
   * : the EnvCleanup interface.
   */
  public void cleanup()
  {
    Socket s = _socket;
    _socket = null;

    try {
      if (s != null)
        s.close();
    } catch (IOException e) {
      log.log(Level.FINE, e.ToString(), e);
    }
  }

  public string ToString()
  {
    if (_socket != null)
      return "TcpInputOutput[" + _socket.getInetAddress()
          + "," + _socket.getPort() + "]";
    else
      return "TcpInputOutput[closed]";
  }
}

}
