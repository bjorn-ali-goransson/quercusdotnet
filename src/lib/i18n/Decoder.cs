using System;
namespace QuercusDotNet.lib/i18n{
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








abstract public class Decoder
{
  protected string _charset;
  protected CharSequence _replacement;

  protected bool _isIgnoreErrors = false;
  protected bool _isReplaceUnicode = false;
  protected bool _isAllowMalformedOut = false;

  protected bool _hasError;

  protected Decoder(String charset)
  {
    _charset = charset;
  }

  public static Decoder create(String charset)
  {
    if (charset.equalsIgnoreCase("utf8")
        || charset.equalsIgnoreCase("utf-8"))
      return new Utf8Decoder(charset);
    else if (charset.equalsIgnoreCase("big5")
             || charset.equalsIgnoreCase("big-5"))
      return new Big5Decoder(charset);
    else
      return new GenericDecoder(charset);
  }

  public bool isUtf8()
  {
    return false;
  }

  public bool isIgnoreErrors()
  {
    return _isIgnoreErrors;
  }

  public void setIgnoreErrors(bool isIgnore)
  {
    _isIgnoreErrors = isIgnore;
  }

  public bool hasError()
  {
    return _hasError;
  }

  public void setReplacement(CharSequence replacement)
  {
    _replacement = replacement;
  }

  public void setReplaceUnicode(bool isReplaceUnicode)
  {
    _isReplaceUnicode = isReplaceUnicode;
  }

  public void setAllowMalformedOut(bool isAllowMalformedOut)
  {
    _isAllowMalformedOut = isAllowMalformedOut;
  }

  public void reset()
  {
    _hasError = false;
  }

  public CharSequence decode(Env env, StringValue str)
  {
    if (str.isUnicode()) {
      return str;
    }

    UnicodeBuilderValue sb = new UnicodeBuilderValue();

    decodeUnicode(str, sb);

    return sb;
  }

  public StringValue decodeUnicode(StringValue str)
  {
    UnicodeBuilderValue sb = new UnicodeBuilderValue();

    decodeUnicode(str, sb);

    return sb;
  }

  public abstract void decodeUnicode(StringValue str, UnicodeBuilderValue sb);

  abstract public bool isDecodable(Env env, StringValue str);
}
}
