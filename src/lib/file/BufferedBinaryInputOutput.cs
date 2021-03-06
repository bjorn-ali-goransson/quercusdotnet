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
 * @author Scott Ferguson
 */














/**
 * Represents a Quercus file open for reading
 */
abstract public class BufferedBinaryInputOutput
  : AbstractBinaryInputOutput
{
  private const Logger log
    = Logger.getLogger(BufferedBinaryInputOutput.class.getName());

  private ReadStream _is;
  private WriteStream _os;

  protected BufferedBinaryInputOutput(Env env)
  {
    super(env);
  }

  public void init(ReadStream @is, WriteStream os)
  {
    super.init(@is, os);
    
    _is = is;
    _os = os;
  }

  //
  // read methods
  //

  public void setEncoding(String encoding)
    
  {
    if (_is != null)
      _is.setEncoding(encoding);
  }

  /**
   * Unread the last byte.
   */
  public void unread()
    
  {
    if (_is != null) {
      _is.unread();
      _isEOF = false;
    }
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
  public int read()
    
  {
    try {
      if (_is != null) {
        int c = _is.read();

        if (c < 0)
          _isEOF = true;

        return c;
      }
      else
        return -1;
    } catch (IOException e) {
      _isTimeout = true;
      _isEOF = true;

      log.log(Level.FINER, e.ToString(), e);

      return -1;
    }
  }

  /**
   * Reads a buffer from a file, returning -1 on EOF.
   */
  public int read(char []buffer, int offset, int length)
    
  {
    try {
      if (_is != null) {
        int c = _is.read(buffer, offset, length);

        if (c == -1)
          _isEOF = true;
        else
          _isEOF = false;

        return c;
      }
      else
        return -1;
    } catch (IOException e) {
      _isTimeout = true;
      _isEOF = true;

      log.log(Level.FINER, e.ToString(), e);

      return -1;
    }
  }

  public void writeToStream(OutputStream os, int length)
    
  {
    try {
      if (_is != null) {
        _is.writeToStream(os, length);
      }
    } catch (IOException e) {
      _isTimeout = true;
      _isEOF = true;

      log.log(Level.FINER, e.ToString(), e);
    }
  }

  /**
   * Reads a line from a file, returning null on EOF.
   */
  public StringValue readLine(long length)
    
  {
    try {
      StringValue line = _lineReader.readLine(_env, this, length);

      return line;
    } catch (IOException e) {
      _isTimeout = true;
      _isEOF = true;

      log.log(Level.FINER, e.ToString(), e);

      return _env.getEmptyString();
    }
  }

  /**
   * Returns the current location in the file.
   */
  public long getPosition()
  {
    if (_is != null)
      return _is.getPosition();
    else
      return -1;
  }

  /**
   * Sets the current location in the file.
   */
  public bool setPosition(long offset)
  {
    if (_is == null)
      return false;

    _isEOF = false;

    try {
      return _is.setPosition(offset);
    } catch (IOException e) {
      throw new QuercusModuleException(e);
    }
  }

  public Value stat()
  {
    return BooleanValue.FALSE;
  }

  /**
   * Converts to a string.
   */
  public string ToString()
  {
    if (_is != null)
      return "BufferedBinaryInputOutput[" + _is.getPath() + "]";
    else
      return "BufferedBinaryInputOutput[closed]";
  }
}

}
