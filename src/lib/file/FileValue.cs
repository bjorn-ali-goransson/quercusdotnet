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
 * Represents a Quercus open file
 */
public class FileValue : StreamResource {
  private string _path;

  public FileValue(Path path)
  {
    _path = path;
  }

  /**
   * Returns the path.
   */
  public string getPath()
  {
    return _path;
  }

  /**
   * Reads a character from a file, returning -1 on EOF.
   */
  public int read()
    
  {
    return -1;
  }

  /**
   * Reads a line from a file, returning null.
   */
  public override StringValue readLine(Env env)
    
  {
    StringValue sb = env.createStringBuilder();

    int ch;

    while ((ch = read()) >= 0) {
      sb.append((char) ch);

      if (ch == '\n')
        return sb;
      // XXX: issues with mac
    }

    if (sb.length() > 0)
      return sb;
    else
      return null;
  }

  /**
   * Read a maximum of <i>length</i> bytes from the file and write
   * them to the outputStream.
   *
   * @param os the {@link OutputStream}
   * @param length the maximum number of bytes to read
   */
  public void writeToStream(OutputStream os, int length)
    
  {
  }

  /**
   * Prints a string to a file.
   */
  public void print(String v)
    
  {
  }

  /**
   * Closes the file.
   */
  public void close()
  {
  }

  /**
   * Converts to a string.
   * @param env
   */
  public string ToString()
  {
    return "File[" + _path + "]";
  }
}

}
