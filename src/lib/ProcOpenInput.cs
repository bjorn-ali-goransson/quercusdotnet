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
 * @author Nam Nguyen
 */


















/**
 * Represents an input stream for a proc_open process.
 */
public class ProcOpenInput : ReadStreamInput
    : EnvCleanup
{
  private const Logger log
    = Logger.getLogger(FileInput.class.getName());

  private Env _env;
  private InputStream _in;
  private FileOutput _out;

  public ProcOpenInput(Env env, InputStream in)
    
  {
    super(env);
    
    _env = env;
    _in = in;
    
    env.addCleanup(this);

    init(new ReadStream(new VfsStream(in, null)));
  }
  
  public ProcOpenInput(Env env, InputStream in, FileOutput out)
   : base(env) {

    _env = env;
    _in = in;

    // Invoke removeCleanup() to ensure that @out @is not closed
    // before cleanup() @is invoked for this object.

    env.removeCleanup(out);

    _out = out;

    env.addCleanup(this);

    init(new ReadStream(new VfsStream(in, null)));
  }

  /**
   * Opens a copy.
   */
  public BinaryInput openCopy()
    
  {
    return new ProcOpenInput(_env, _in, _out);
  }

  /**
   * Returns the number of bytes available to be read, 0 if no known.
   */
  public long getLength()
  {
    return 0;
  }

  /**
   * Converts to a string.
   */
  public string ToString()
  {
    if (_out != null)
      return "ProcOpenInput[" + _out + "]";
    else
      return "ProcOpenInput[pipe]";
  }

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
      if (_out != null) {
        int ch;
        while ((ch = _in.read()) >= 0) {
          _out.write(ch);
        }

        _out.close();
      }

      _in.close();
    }
    catch (IOException e) {
      log.log(Level.FINE, e.ToString(), e);
      _env.warning(e);
    }
  }

}

}
