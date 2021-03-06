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
 * Interface for a Quercus binary input stream
 */
public interface BinaryInput : BinaryStream {
  /**
   * Returns an InputStream to the input.
   */
  public InputStream getInputStream();

  /**
   * Opens a new copy.
   */
  public BinaryInput openCopy()
    

  /**
   * @return
   */
  public int getAvailable()
    

  /**
   * Reads the next byte, returning -1 on eof.
   */
  public int read()
    

  /**
   * Unreads the last byte.
   */
  public void unread()
    

  /**
   * Reads into a buffer, returning -1 on eof.
   */
  public int read(byte []buffer, int offset, int length)
    

  /**
   * Reads a Binary string.
   */
  public StringValue read(int length)
    

  /**
   * Reads the optional linefeed character from a \r\n
   */
  public bool readOptionalLinefeed()
    

  /**
   * Reads a line from the buffer.
   */
  public StringValue readLine(long length)
    

  /**
   * Appends to a string builder.
   */
  public StringValue appendTo(StringValue builder)
    

  /**
   * Returns the current location in the stream
   */
  public long getPosition();

  /**
   * Sets the current location in the stream
   */
  public bool setPosition(long offset);

  /**
   * Closes the stream.
   */
  public void close();

  /**
   * Closes the stream for reading
   */
  public void closeRead();
}

}
