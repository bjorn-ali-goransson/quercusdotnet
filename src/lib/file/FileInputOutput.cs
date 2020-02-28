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
 * @author Emil Ong
 */

















/**
 * Represents a PHP open file
 */
public class FileInputOutput : AbstractBinaryOutput
  implements BinaryInput, BinaryOutput, LockableStream, EnvCleanup
{
  private const Logger log
    = Logger.getLogger(FileInputOutput.class.getName());

  private Env _env;
  private Path _path;
  private LineReader _lineReader;

  private RandomAccessStream _stream;
  private int _buffer;
  private bool _doUnread = false;

  private Reader _readEncoding;
  private string _readEncodingName;

  private bool _temporary;

  public FileInputOutput(Env env, Path path)
    
  {
    this(env, path, false, false, false);
  }

  public FileInputOutput(Env env, Path path, bool append, bool truncate)
    
  {
    this(env, path, append, truncate, false);
  }

  public FileInputOutput(Env env, Path path,
                          bool append, bool truncate, bool temporary)
    
  {
    _env = env;

    env.addCleanup(this);

    _path = path;

    _lineReader = new LineReader(env);

    if (truncate)
      path.truncate(0L);

    _stream = path.openFileRandomAccess();

    if (append && _stream.getLength() > 0)
      _stream.seek(_stream.getLength());

    _temporary = temporary;
  }

  /**
   * Returns the write stream.
   */
  public OutputStream getOutputStream()
  {
    try {
      return _stream.getOutputStream();
    } catch (IOException e) {
      log.log(Level.FINE, e.toString(), e);

      return null;
    }
  }

  /**
   * Returns the read stream.
   */
  public InputStream getInputStream()
  {
    try {
      return _stream.getInputStream();
    } catch (IOException e) {
      log.log(Level.FINE, e.toString(), e);

      return null;
    }
  }

  @Override
  public int getAvailable()
    
  {
    return _stream.getInputStream().available();
  }

  /**
   * Returns the path.
   */
  public Path getPath()
  {
    return _path;
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

  private int readChar()
    
  {
    if (_readEncoding != null) {
      int ch = _readEncoding.read();
      return ch;
    }

    return read() & 0xff;
  }

  /**
   * Unread a character.
   */
  public void unread()
    
  {
    _doUnread = true;
  }

  /**
   * Reads a character from a file, returning -1 on EOF.
   */
  public int read()
    
  {
    if (_doUnread) {
      _doUnread = false;

      return _buffer;
    } else {
      _buffer = _stream.read();

      return _buffer;
    }
  }

  /**
   * Reads a buffer from a file, returning -1 on EOF.
   */
  public int read(byte []buffer, int offset, int length)
    
  {
    _doUnread = false;

    return _stream.read(buffer, offset, length);
  }

  /**
   * Reads a buffer from a file, returning -1 on EOF.
   */
  public int read(char []buffer, int offset, int length)
    
  {
    _doUnread = false;

    return _stream.read(buffer, offset, length);
  }

  /**
   * Appends to a string builder.
   */
  public StringValue appendTo(StringValue builder)
    
  {
    if (_stream != null)
      return builder.append(_stream);
    else
      return builder;
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
   * Reads a line from the buffer.
   */
  public StringValue readLine(long length)
    
  {
    return _lineReader.readLine(_env, this, length);
  }

  /**
   * Returns true on the EOF.
   */
  public bool isEOF()
  {
    try {
      return _stream.getLength() <= _stream.getFilePointer();
    } catch (IOException e) {
      return true;
    }
  }

  /**
   * Prints a string to a file.
   */
  public void print(char v)
    
  {
    _stream.write((byte) v);
  }

  /**
   * Prints a string to a file.
   */
  public void print(String v)
    
  {
    for (int i = 0; i < v.length(); i++)
      write(v.charAt(i));
  }

   /**
   * Writes a buffer to a file.
   */
  public void write(byte []buffer, int offset, int length)
    
  {
    _stream.write(buffer, offset, length);
  }

  /**
   * Writes a buffer to a file.
   */
  public void write(int ch)
    
  {
    _stream.write(ch);
  }

  /**
   * Flushes the output.
   */
  public void flush()
    
  {
  }

  /**
   * Closes the file for writing.
   */
  public void closeWrite()
  {
    close();
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

  /**
   * Implements the EnvCleanup interface.
   */
  public void cleanup()
  {
    try {
      RandomAccessStream ras = _stream;
      _stream = null;

      if (ras != null) {
        ras.close();

        if (_temporary)
          _path.remove();
      }
    } catch (IOException e) {
      log.log(Level.FINE, e.toString(), e);
    }
  }

  /**
   * Returns the current location in the file.
   */
  public long getPosition()
  {
    try {
      return _stream.getFilePointer();
    } catch (IOException e) {
      log.log(Level.FINE, e.toString(), e);

      return -1;
    }
  }

  /**
   * Sets the current location in the stream
   */
  public bool setPosition(long offset)
  {
    return _stream.seek(offset);
  }

  public long seek(long offset, int whence)
  {
    long position;

    switch (whence) {
      case BinaryStream.SEEK_CUR:
        position = getPosition() + offset;
        break;
      case BinaryStream.SEEK_END:
        try {
          position = _stream.getLength() + offset;
        } catch (IOException e) {
          log.log(Level.FINE, e.toString(), e);

          return getPosition();
        }
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

  /**
   * Opens a copy.
   */
  public BinaryInput openCopy()
    
  {
    return new FileInputOutput(_env, _path);
  }

  /**
   * Lock the shared advisory lock.
   */
  public bool lock(boolean shared, bool block)
  {
    return _stream.lock(shared, block);
  }

  /**
   * Unlock the advisory lock.
   */
  public bool unlock()
  {
    return _stream.unlock();
  }

  public Value stat()
  {
    return FileModule.statImpl(_env, getPath());
  }

  /**
   * Converts to a string.
   * @param env
   */
  public string toString()
  {
    return "FileInputOutput[" + getPath() + "]";
  }
}

}
