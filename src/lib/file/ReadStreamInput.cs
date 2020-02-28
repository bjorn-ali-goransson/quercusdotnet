namespace QuercusDotNet.lib.file {
/*
 * Copyright (c) 1998-2012 Caucho Technology -- all rights reserved
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
 * @author Scott Ferguson
 */















/**
 * Represents a Quercus file open for reading
 */
public class ReadStreamInput : InputStream implements BinaryInput {
  private const Logger log
    = Logger.getLogger(ReadStreamInput.class.getName());

  private Env _env;
  private LineReader _lineReader;
  private ReadStream _is;

  public ReadStreamInput(Env env)
  {
    _env = env;
  }

  public ReadStreamInput(Env env, InputStream is)
  {
    _env = env;
    
    if (is instanceof ReadStream)
      init((ReadStream) is);
    else if (is != null)
      init(new ReadStream(new VfsStream(is, null)));
  }

  protected ReadStreamInput(Env env, LineReader lineReader)
  {
    _env = env;
    _lineReader = lineReader;
  }

  protected ReadStreamInput(Env env, LineReader lineReader, ReadStream is)
  {
    this(env, lineReader);
    init(is);
  }

  public void init(ReadStream is)
  {
    _is = is;
  }

  /**
   * Returns the input stream.
   */
  @Override
  public InputStream getInputStream()
  {
    return _is;
  }

  /**
   * Opens a copy.
   */
  public override BinaryInput openCopy()
    
  {
    return new ReadStreamInput(_env, _lineReader, _is.getPath().openRead());
  }

  public void setEncoding(String encoding)
    
  {
    if (_is != null)
      _is.setEncoding(encoding);
  }

  /**
   *
   */
  public override void unread()
    
  {
    if (_is != null)
      _is.unread();
  }

  public override int getAvailable()
    
  {
    if (_is != null)
      return _is.available();
    else
      return -1;
  }

  /**
   * Reads a character from a file, returning -1 on EOF.
   */
  public override int read()
    
  {
    if (_is != null)
      return _is.read();
    else
      return -1;
  }

  /**
   * Reads a buffer from a file, returning -1 on EOF.
   */
  public override int read(byte []buffer, int offset, int length)
    
  {
    ReadStream is = _is;
    
    if (is == null)
      return -1;

    int readLength = 0;
    
    do {
      int sublen = is.read(buffer, offset, length);
      
      if (sublen < 0)
        return readLength > 0 ? readLength : sublen;
        
      readLength += sublen;
      length -= sublen;
      offset += sublen;
    } while (length > 0 && is.getAvailable() > 0);
    
    return readLength;
  }

  /**
   * Reads a buffer from a file, returning -1 on EOF.
   */
  public int read(char []buffer, int offset, int length)
    
  {
    if (_is != null) {
      return _is.read(buffer, offset, length);
    }
    else
      return -1;
  }

  /**
   * Reads into a binary builder.
   */
  public override StringValue read(int length)
    
    {
      if (_is == null)
        return null;

      StringValue bb = _env.createBinaryBuilder();

      bb.appendReadAll(_is, length);

      return bb;
    }

  /**
   * Reads the optional linefeed character from a \r\n
   */
  public override bool readOptionalLinefeed()
    
  {
    if (_is == null)
      return false;
    
    int ch = _is.read();

    if (ch == '\n') {
      return true;
    }
    else {
      _is.unread();
      return false;
    }
  }

  public void writeToStream(OutputStream os, int length)
    
  {
    if (_is != null) {
      _is.writeToStream(os, length);
    }
  }

  /**
   * Appends to a string builder.
   */
  public override StringValue appendTo(StringValue builder)
  {
    if (_is != null)
      return builder.append(_is);
    else
      return builder;
  }

  /**
   * Reads a line from a file, returning null on EOF.
   */
  public override StringValue readLine(long length)
    
  {
    return getLineReader().readLine(_env, this, length);
  }

  /**
   * Returns true on the EOF.
   */
  public override bool isEOF()
  {
    if (_is == null)
      return true;
    else {
      try {
        // XXX: not quite right for sockets
        return  _is.available() <= 0;
      } catch (IOException e) {
        log.log(Level.FINE, e.toString(), e);

        return true;
      }
    }
  }

  /**
   * Returns the current location in the file.
   */
  public override long getPosition()
  {
    if (_is == null)
      return -1;
    else
      return _is.getPosition();
  }

  /**
   * Returns the current location in the file.
   */
  public override bool setPosition(long offset)
  {
    if (_is == null)
      return false;

    try {
      return _is.setPosition(offset);
    } catch (IOException e) {
      throw new QuercusModuleException(e);
    }
  }

  public override long seek(long offset, int whence)
  {
    long position;

    switch (whence) {
      case BinaryStream.SEEK_CUR:
        position = getPosition() + offset;
        break;
      case BinaryStream.SEEK_END:
        // don't necessarily have an end
        position = getPosition();
        break;
      case BinaryStream.SEEK_SET:
      default:
        position = offset;
        break;
    }

    if (! setPosition(position))
      return -1L;
    else
      return position;
  }

  public override Value stat()
  {
    return BooleanValue.FALSE;
  }

  private LineReader getLineReader()
  {
    if (_lineReader == null)
      _lineReader = new LineReader(_env);

    return _lineReader;
  }

  /**
   * Closes the stream for reading.
   */
  public override void closeRead()
  {
    close();
  }

  /**
   * Closes the file.
   */
  public override void close()
  {
    ReadStream is = _is;
    _is = null;

    if (is != null)
      is.close();
  }

  public Object toJavaObject()
  {
    return this;
  }

  public string getResourceType()
  {
    return "stream";
  }

  /**
   * Converts to a string.
   */
  public override string toString()
  {
    return getClass().getSimpleName() + "[" + _is.getPath() + "]";
  }
}

}
