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



























public class HttpInputOutput : AbstractBinaryOutput
  : BinaryInput, BinaryOutput, LockableStream, EnvCleanup
{
  private const Logger log
    = Logger.getLogger(HttpInputOutput.class.getName());

  private Env _env;
  private Path _path;
  private StreamContextResource _context;
  
  private LineReader _lineReader;

  private ReadStream _is;
  private WriteStream _os;
  
  private HttpStreamWrapper _httpStream;
  
  private Reader _readEncoding;
  private string _readEncodingName;
  
  private byte []_bodyStart;
  
  public HttpInputOutput(Env env, Path path, StreamContextResource context)
    
  {
    init(env, path, context);
  }
  
  private void init(Env env, Path path, StreamContextResource context)
    
  {
    _env = env;
    _path = path;
    
    env.addCleanup(this);
    
    _path = path;
    
    _lineReader = new LineReader(env);
    
    if (context != null) {
      Value options
      = context.getOptions().get(env.createString(path.getScheme()));

    string method = options.get(env.createString("method")).toString();
    
    if (method.equals("POST")) {
      ReadWritePair pair = path.openReadWrite();
      _is = pair.getReadStream();
      _os = pair.getWriteStream();
    }
    else
      _is = path.openRead();
    
    _httpStream = (HttpStreamWrapper) _is.getSource();
    
    setOptions(env, options);
    
    if (_os != null && _bodyStart != null && _bodyStart.length > 0)
      _os.write(_bodyStart, 0, _bodyStart.length);
    }
    else {
      _is = path.openRead();
      
      _httpStream = (HttpStreamWrapper) _is.getSource();
    }
  }

  private void setOptions(Env env, Value options)
    
  {
    Iterator<Map.Entry<Value,Value>> iter = options.getIterator(env);

    while (iter.hasNext()) {
      Map.Entry<Value,Value> entry = iter.next();
    
      string optionName = entry.getKey().toString();
      Value optionValue = entry.getValue();

      if (optionName.equals("method"))
        _httpStream.setMethod(optionValue.toString());
      else if (optionName.equals("header")) {
        string option = optionValue.toString();
        
        int start = 0;
        int len = option.length();
        
        while (start < len) {
          int end = option.indexOf("\r\n", start);

          if (end < 0)
            end = len;
          
          int i = option.indexOf(':', start);
          
          if (i < 0 || i > end) {
            _httpStream.setAttribute(option.substring(start, end), "");
            
            break;
          }
          else {
            string name = option.substring(start, i);
            string value = option.substring(i + 1, end).trim();

            _httpStream.setAttribute(name, value);
          }
          
          start = end += 2;
        }
      }
      else if (optionName.equals("user_agent"))
        _httpStream.setAttribute("User-Agent", optionValue.toString());
      else if (optionName.equals("content"))
        _bodyStart = optionValue.toBinaryValue(env).toBytes();
      else if (optionName.equals("proxy"))
        env.stub("StreamContextResource::proxy option");
      else if (optionName.equals("request_fulluri"))
        env.stub("StreamContextResource::request_fulluri option");
      else if (optionName.equals("protocol_version")) {
        double version = optionValue.toDouble();
        
        if (version == 1.1) {
        }
        else if (version == 1.0)
          _httpStream.setHttp10();
        else
          env.stub("StreamContextResource::protocol_version " + version);
      }
      else if (optionName.equals("timeout")) {
        long ms = (long) optionValue.toDouble() * 1000;
        _httpStream.setSocketTimeout(ms);
      }
      else if (optionName.equals("ignore_errors"))
        env.stub("ignore_errors::ignore_errors option");
      else
        env.stub("ignore_errors::" + optionName + " option");
    }
  }
  
  @Override
  public void write(int ch)
    
  {
    if (_os != null)
      _os.write(ch);
  }

  /**
   * Appends to a string builder.
   */
  public StringValue appendTo(StringValue builder)
    
  {
    if (_is != null)
      return builder.append(_is);
    else
      return builder;
  }

  /**
   * Returns the read stream.
   */
  public InputStream getInputStream()
  {
    return _is;
  }

  public override int getAvailable()
    
  {
    return _is.available();
  }
  
  /**
   * Opens a copy.
   */
  public BinaryInput openCopy()
    
  {
    return new HttpInputOutput(_env, _path, _context);
  }

  /**
   * Reads a character from a file, returning -1 on EOF.
   */
  public int read()
    
  {
    if (_is != null)
      return _is.read();
    else
      return -1;
  }

  /**
   * Reads a buffer from a file, returning -1 on EOF.
   */
  public int read(byte []buffer, int offset, int length)
    
  {
    if (_is != null)
      return _is.read(buffer, offset, length);
    else
      return -1;
  }

  /**
   * Reads a buffer from a file, returning -1 on EOF.
   */
  public int read(char []buffer, int offset, int length)
    
  {
    if (_is != null)
      return _is.read(buffer, offset, length);
    else
      return -1;
  }

  /**
   * Reads a Binary string.
   */
  public StringValue read(int length)
    
  {
    StringValue bb = _env.createBinaryBuilder();
    TempBuffer temp = TempBuffer.allocate();
    
    try {
      byte []buffer = temp.getBuffer();

      while (length > 0) {
        int sublen = buffer.length;

        if (length < sublen)
          sublen = length;

        sublen = read(buffer, 0, sublen);

        if (sublen > 0) {
          bb.append(buffer, 0, sublen);
          length -= sublen;
        }
        else
          break;
      }
    } finally {
      TempBuffer.free(temp);
    }

    return bb;
  }

  /**
   * Reads a line from the buffer.
   */
  public override StringValue readLine(long length)
    
  {
    return _lineReader.readLine(_env, this, length);
  }

  /**
   * Reads the optional linefeed character from a \r\n
   */
  public bool readOptionalLinefeed()
    
  {
    int ch = read();

    if (ch == '\n') {
      return true;
    }
    else {
      unread();
      return false;
    }
  }
  
  /**
   * Returns true on the EOF.
   */
  public bool isEOF()
  {
    try {
      if (_is == null)
        return true;
      
      return _is.available() <= 0;
    } catch (IOException e) {
      return true;
    }
  }

  /**
   * Sets the current read encoding.  The encoding can either be a
   * Java encoding name or a mime encoding.
   *
   * @param encoding name of the read encoding
   */
  public void setEncoding(String encoding)
    
  {
    string mimeName = Encoding.getMimeName(encoding);
    
    if (mimeName != null && mimeName.equals(_readEncodingName))
      return;
    
    _readEncoding = Encoding.getReadEncoding(getInputStream(), encoding);
    _readEncodingName = mimeName;
  }

  /**
   * Unread a character.
   */
  public override void unread()
    
  {
    if (_is != null)
      _is.unread();
  }

  public override bool lock(boolean shared, bool block)
  {
    return false;
  }

  public override bool unlock()
  {
    return false;
  }
  
  /**
   * Closes the file for reading.
   */
  public void closeRead()
  {
    close();
  }

  /**
   * Closes the file.
   */
  public void close()
  {
    _env.removeCleanup(this);

    cleanup();
  }

  public override void cleanup()
  {
    try {
      ReadStream @is = _is;
      WriteStream os = _os;
      
      _is = null;
      _os = null;
      
      if (@is != null)
        @is.close();
      
      if (os != null)
        os.close();
    } catch (IOException e) {
      log.log(Level.FINE, e.toString(), e);
    }
  }
  
  public string toString()
  {
    return "HttpInputOutput[" + _path + "]";
  }

}
}
