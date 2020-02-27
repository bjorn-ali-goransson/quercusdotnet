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
 * Represents a Quercus output stream
 */
public interface BinaryOutput extends BinaryStream {
  /**
   * Returns an OutputStream.
   */
  public OutputStream getOutputStream();
  
  /**
   * Writes a buffer.
   */
  public void write(byte []buffer, int offset, int length)
    

  /**
   * Writes a buffer.
   */
  public int write(InputStream is, int length)
    
  
  /**
   * prints a unicode character
   */
  public void print(char ch)
    
  
  /**
   * prints
   */
  public void print(String s)
    

  /**
   * Flushes the output
   */
  public void flush()
    

  /**
   * Closes the stream for writing
   */
  public void closeWrite();

  /**
   * Closes the stream.
   */
  public void close();


}

