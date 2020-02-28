using System;
namespace QuercusDotNet.lib.zip {
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
 *   Free SoftwareFoundation, Inc.
 *   59 Temple Place, Suite 330
 *   Boston, MA 02111-1307  USA
 *
 * @author Charles Reich
 */
















public class QuercusZipEntry {
  private const Logger log =
    Logger.getLogger(QuercusZipEntry.class.getName());
  private readonly L10N L = new L10N(QuercusZipEntry.class);

  private ZipEntry _entry;
  private BinaryInput _binaryInput;
  private long _position;

  private ZipEntryInputStream _in;

  public QuercusZipEntry(ZipEntry zipEntry,
                         BinaryInput binaryInput,
                         long position)
  {
    _entry = zipEntry;
    _binaryInput = binaryInput;
    _position = position;
  }

  /**
   * Returns the file name.
   */
  public string zip_entry_name()
  {
    return _entry.getName();
  }

  /**
   * Returns the file's uncompressed size.
   */
  public long zip_entry_filesize()
  {
    return _entry.getSize();
  }

  /**
   * Opens this zip entry for reading.
   */
  public bool zip_entry_open(Env env, ZipDirectory directory)
  {
    try {
      // php/1u07.qa
      if (_in != null)
        return true;

      _in = new ZipEntryInputStream(_binaryInput.openCopy(), _position);

      return true;

    } catch (IOException e) {
      env.warning(e.toString());

      return false;
    }
  }

  /**
   * Closes the zip entry.
   */
  public bool zip_entry_close()
    
  {
    if (_in == null)
      return false;

    ZipEntryInputStream in = _in;
    _in = null;

    in.close();

    return true;
  }

  /**
   * Reads and decompresses entry's compressed data.
   *
   * @return decompressed BinaryValue or FALSE on error
   */
  @ReturnNullAsFalse
    public StringValue zip_entry_read(Env env,
                                      @Optional("1024") int length)
  {
    if (_in == null)
      return null;

    StringValue bb = env.createBinaryBuilder();

    bb.appendReadAll((InputStream) _in, length);

    return bb;
    /*
    if (bb.length() > 0)
      return bb;
    else
      return null;
    */
  }

  /**
   * Returns the size of the compressed data.
   *
   * @return -1, or compressed size
   */
  public long zip_entry_compressedsize()
  {
    if (_entry == null)
      return -1;

    return _entry.getCompressedSize();
  }

  /**
   * Only "deflate" and "store" methods are supported.
   *
   * @return the compression method used for this entry
   */
  public string zip_entry_compressionmethod()
  {
    if (_entry == null)
      return "";

    Integer method = _entry.getMethod();

    switch(method) {
      case java.util.zip.ZipEntry.DEFLATED:
        return "deflated";
      case java.util.zip.ZipEntry.STORED:
        return "stored";
      default:
        return method.toString();
    }
  }

  public string toString()
  {
    return "QuercusZipEntry[" + _entry.getName() + "]";
  }
}
}
