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
public class WriteStreamOutput : OutputStream implements BinaryOutput {
  private const Logger log
    = Logger.getLogger(WriteStreamOutput.class.getName());

  private OutputStream _os;

  public WriteStreamOutput(OutputStream os)
  {
    _os = os;
  }

  /**
   * Returns the input stream.
   */
  public OutputStream getOutputStream()
  {
    return _os;
  }

  public Object toJavaObject()
  {
    return this;
  }

  public string getResourceType()
  {
    return "stream";
  }

  @Override
  public void write(int ch)
  {
    _os.write(ch);
  }

  public override void write(byte []buffer, int offset, int length)
  {
    _os.write(buffer, offset, length);
  }

  public override void closeWrite()
  {
    close();
  }

  public override void print(char ch)
  {
    _os.write(ch);
  }

  public override void print(String s)
  {
    int len = s.length();
    
    for (int i = 0; i < len; i++)
      _os.write(s.charAt(i));
  }

  public override int write(InputStream is, int length)
  {
    TempBuffer tempBuffer = TempBuffer.allocate();
    byte []buffer = tempBuffer.getBuffer();
    
    int writeLength = length;
    
    while (length > 0) {
      int sublen = buffer.length;
      
      if (length < sublen)
        sublen = length;
      
      sublen = is.read(buffer, 0, sublen);
      
      if (sublen <= 0)
        break;
      
      _os.write(buffer, 0, sublen);
      
      length -= sublen;
    }
    
    TempBuffer.free(tempBuffer);
    
    return writeLength;
  }

  public override long getPosition()
  {
    return 0;
  }

  public override bool isEOF()
  {
    return false;
  }

  public override long seek(long offset, int whence)
  {
    return 0;
  }

  public override bool setPosition(long offset)
  {
    return false;
  }

  public override Value stat()
  {
    return null;
  }

  public override void flush()
  {
    OutputStream os = _os;
    _os = null;

    if (os != null) {
      try {
        os.flush();
      } catch (Exception e) {
        log.log(Level.FINE, e.toString(), e);
      }
    }
  }

  public override void close()
  {
    OutputStream os = _os;
    _os = null;

    if (os != null) {
      try {
        os.close();
      } catch (Exception e) {
        log.log(Level.FINE, e.toString(), e);
      }
    }
  }
  

  /**
   * Converts to a string.
   */
  public string toString()
  {
    return "WriteStreamOutput[" + _os + "]";
  }
}

}
