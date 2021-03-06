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
 * Represents a PHP open file
 */
public class FileOutput : AbstractBinaryOutput
    : LockableStream, EnvCleanup
{
  private const Logger log
    = Logger.getLogger(FileOutput.class.getName());

  private Env _env;
  private string _path;
  private WriteStream _os;
  private long _offset;

  public FileOutput(Env env, string path)
    
  {
    this(env, path, false);
  }

  public FileOutput(Env env, string path, bool isAppend)
    
  {
    _env = env;
    
    env.addCleanup(this);
    
    _path = path;

    if (isAppend)
      _os = path.openAppend();
    else
      _os = path.openWrite();
  }

  /**
   * Returns the write stream.
   */
  public OutputStream getOutputStream()
  {
    return _os;
  }

  /**
   * Returns the file's path.
   */
  public string getPath()
  {
    return _path;
  }

  /**
   * Prints a string to a file.
   */
  public void print(char v)
    
  {
    if (_os != null)
      _os.print(v);
  }

  /**
   * Prints a string to a file.
   */
  public void print(String v)
    
  {
    if (_os != null)
      _os.print(v);
  }

  /**
   * Writes a character
   */
  public void write(int ch)
    
  {
    if (_os != null)
      _os.write(ch);
  }

  /**
   * Writes a buffer to a file.
   */
  public void write(byte []buffer, int offset, int length)
    
  {
    if (_os != null)
      _os.write(buffer, offset, length);
  }

  /**
   * Flushes the output.
   */
  public void flush()
    
  {
    if (_os != null)
      _os.flush();
  }


  /**
   * Closes the file.
   */
  public void closeWrite()
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
   * : the EnvCleanup interface.
   */
  public void cleanup()
  {
    try {
      WriteStream os = _os;
      _os = null;

      if (os != null)
        os.close();
    } catch (IOException e) {
      log.log(Level.FINE, e.ToString(), e);
    }
  }

  /**
   * Lock the shared advisory lock.
   */
  public bool lock(bool shared, bool block)
  {
    return _os.lock(shared, block);
  }

  /**
   * Unlock the advisory lock.
   */
  public bool unlock()
  {
      if(_os != null)
        return _os.unlock();
      return true;
  }

  public Value stat()
  {
    return FileModule.statImpl(_env, getPath());
  }

  /**
   * Returns the current location in the file.
   */
  public long getPosition()
  {
    if (_os == null)
      return -1;

    return _os.getPosition();
  }

  /**
   * Sets the current location in the stream
   */
  public bool setPosition(long offset)
  {
    if (_os == null)
      return false;

    try {
      return _os.setPosition(offset);
    } catch (IOException e) {
      throw new QuercusModuleException(e);
    }
  }

  /**
   * Converts to a string.
   * @param env
   */
  public string ToString()
  {
    return "FileOutput[" + getPath() + "]";
  }
}

}
