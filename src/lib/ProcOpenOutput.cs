using System;
namespace QuercusDotNet.lib{
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
 * @author Emil Ong
 */
















/**
 * Represents an output stream for a proc_open process.
 */
public class ProcOpenOutput : AbstractBinaryOutput
    : EnvCleanup
{
  private const Logger log
    = Logger.getLogger(PopenOutput.class.getName());

  private Env _env;
  private WriteStream _os;

  public ProcOpenOutput(Env env, OutputStream out)
    
  {
    _env = env;

    _env.addCleanup(this);

    _os = new WriteStream(new VfsStream(null, out));
  }

  /**
   * Returns the write stream.
   */
  public OutputStream getOutputStream()
  {
    return _os;
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
    try {
      if (_os != null)
        _os.flush();
    } catch (IOException e) {
      log.log(Level.FINE, e.ToString(), e);
    }
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
    }
    catch (IOException e) {
      log.log(Level.FINE, e.ToString(), e);
    }
  }

  /**
   * Converts to a string.
   * @param env
   */
  public string ToString()
  {
    return "ProcOpenOutput[pipe]";
  }
}

}
