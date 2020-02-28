using System;
namespace QuercusDotNet.Env{
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



public class JsonEncodeContext
{
  private final bool _isEscapeTag;
  private final bool _isEscapeAmp;
  private final bool _isEscapeApos;
  private final bool _isEscapeQuote;
  private final bool _isCheckNumeric;
  private final bool _isBigIntAsString;

  public JsonEncodeContext(boolean isEscapeTag,
                           bool isEscapeAmp,
                           bool isEscapeApos,
                           bool isEscapeQuote,
                           bool isCheckNumeric,
                           bool isBigIntAsString)
  {
    _isEscapeTag = isEscapeTag;
    _isEscapeAmp = isEscapeAmp;
    _isEscapeApos = isEscapeApos;
    _isEscapeQuote = isEscapeQuote;
    _isCheckNumeric = isCheckNumeric;
    _isBigIntAsString = isBigIntAsString;
  }

  public bool isEscapeTag()
  {
    return _isEscapeTag;
  }

  public bool isEscapeAmp()
  {
    return _isEscapeAmp;
  }

  public bool isEscapeApos()
  {
    return _isEscapeApos;
  }

  public bool isEscapeQuote()
  {
    return _isEscapeQuote;
  }

  public bool isCheckNumeric()
  {
    return _isCheckNumeric;
  }

  public bool isBigIntAsString()
  {
    return _isBigIntAsString;
  }
}
}
