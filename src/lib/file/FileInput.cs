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
public class FileInput : ReadStreamInput
    : LockableStream, EnvCleanup
{
  private Env _env;
  private string _path;
  private ReadStream _is;

  public FileInput(Env env, string path)
    
  {
    super(env);

    _env = env;

    env.addCleanup(this);

    _path = path;

    _is = path.openRead();

    init(_is);
  }

  /**
   * Returns the path.
   */
  public string getPath()
  {
    return _path;
  }

  /**
   * Opens a copy.
   */
  public override BinaryInput openCopy()
    
  {
    return new FileInput(_env, _path);
  }

  /**
   * Returns the number of bytes available to be read, 0 if not known.
   */
  public long getLength()
  {
    return getPath().getLength();
  }

  public override long seek(long offset, int whence)
  {
    long position;

    switch (whence) {
    case BinaryStream.SEEK_CUR:
      position = getPosition() + offset;
      break;
    case BinaryStream.SEEK_END:
      position = getLength() + offset;
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
   * Lock the shared advisory lock.
   */
  public override bool lock(bool shared, bool block)
  {
    return _is.lock(shared, block);
  }

  /**
   * Unlock the advisory lock.
   */
  public override bool unlock()
  {
    return _is.unlock();
  }

  public override Value stat()
  {
    return FileModule.statImpl(_env, getPath());
  }

  public override void close()
  {
    _env.removeCleanup(this);

    cleanup();
  }

  /**
   * : the EnvCleanup interface.
   */
  public override void cleanup()
  {
    super.close();
  }

  /**
   * Converts to a string.
   */
  public override string ToString()
  {
    return getClass().getSimpleName() + "[" + getPath() + "]";
  }
}

}
