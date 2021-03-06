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
 *
 *   Free Software Foundation, Inc.
 *   59 Temple Place, Suite 330
 *   Boston, MA 02111-1307  USA
 *
 * @author Nam Nguyen
 */













/**
 * Input from a compressed stream.
 */
public class ZipEntryInputStream : ReadStreamInput
{
  private readonly L10N L = new L10N(ZipEntryInputStream.class);

  private BinaryInput _in;
  private long _position;

  public ZipEntryInputStream(BinaryInput in, long position)
    
  {
    super(Env.getInstance());

    _in = in;
    _position = position;

    in.setPosition(_position);

    ZipInputStream zipInputStream = new ZipInputStream(in.getInputStream());

    ZipEntry curEntry = zipInputStream.getNextEntry();

    if (curEntry == null)
      throw new IOException(
          L.l("ZipEntry at position {0} not found", _position));

    init(new ReadStream(new VfsStream(zipInputStream, null)));
  }

  /**
   * Opens a copy.
   */
  public BinaryInput openCopy()
    
  {
    return new ZipEntryInputStream(_in.openCopy(), _position);
  }

  public string ToString()
  {
    return "ZipEntryInputStream[]";
  }
}
}
