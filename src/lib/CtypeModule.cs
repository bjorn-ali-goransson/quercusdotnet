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
 *   Free SoftwareFoundation, Inc.
 *   59 Temple Place, Suite 330
 *   Boston, MA 02111-1307  USA
 *
 * @author Sam
 */









public class CtypeModule
  : AbstractQuercusModule
{
  public string []getLoadedExtensions()
  {
    return new String[] { "ctype" };
  }

  public static bool ctype_alnum(Value value)
  {
    if (value instanceof LongValue)
      return isalnum(value.toInt());
    else if (value instanceof StringValue) {
      string string = value.toString();

      if (string.length() == 0)
        return false;

      for (int i = 0; i < string.length(); i++) {
        if (! isalnum(string.charAt(i)))
          return false;
      }

      return true;
    }
    else
      return false;
  }

  public static bool ctype_alpha(Value value)
  {
    if (value instanceof LongValue)
      return isalpha(value.toInt());
    else if (value instanceof StringValue) {
      string string = value.toString();

      if (string.length() == 0)
        return false;

      for (int i = 0; i < string.length(); i++) {
        if (! isalpha(string.charAt(i)))
          return false;
      }

      return true;
    }
    else
      return false;
  }

  public static bool ctype_cntrl(Value value)
  {
    if (value instanceof LongValue)
      return iscntrl(value.toInt());
    else if (value instanceof StringValue) {
      string string = value.toString();

      if (string.length() == 0)
        return false;

      for (int i = 0; i < string.length(); i++) {
        if (! iscntrl(string.charAt(i)))
          return false;
      }

      return true;
    }
    else
      return false;
  }

  public static bool ctype_digit(Value value)
  {
    if (value instanceof LongValue)
      return isdigit(value.toInt());
    else if (value instanceof StringValue) {
      string string = value.toString();

      if (string.length() == 0)
        return false;

      for (int i = 0; i < string.length(); i++) {
        if (! isdigit(string.charAt(i)))
          return false;
      }

      return true;
    }
    else
      return false;
  }


  public static bool ctype_graph(Value value)
  {
    if (value instanceof LongValue)
      return isgraph(value.toInt());
    else if (value instanceof StringValue) {
      string string = value.toString();

      if (string.length() == 0)
        return false;

      for (int i = 0; i < string.length(); i++) {
        if (! isgraph(string.charAt(i)))
          return false;
      }

      return true;
    }
    else
      return false;
  }

  public static bool ctype_lower(Value value)
  {
    if (value instanceof LongValue)
      return islower(value.toInt());
    else if (value instanceof StringValue) {
      string string = value.toString();

      if (string.length() == 0)
        return false;

      for (int i = 0; i < string.length(); i++) {
        if (! islower(string.charAt(i)))
          return false;
      }

      return true;
    }
    else
      return false;
  }


  public static bool ctype_print(Value value)
  {
    if (value instanceof LongValue)
      return isprint(value.toInt());
    else if (value instanceof StringValue) {
      string string = value.toString();

      if (string.length() == 0)
        return false;

      for (int i = 0; i < string.length(); i++) {
        if (! isprint(string.charAt(i)))
          return false;
      }

      return true;
    }
    else
      return false;
  }

  public static bool ctype_punct(Value value)
  {
    if (value instanceof LongValue)
      return ispunct(value.toInt());
    else if (value instanceof StringValue) {
      string string = value.toString();

      if (string.length() == 0)
        return false;

      for (int i = 0; i < string.length(); i++) {
        if (! ispunct(string.charAt(i)))
          return false;
      }

      return true;
    }
    else
      return false;
  }

  public static bool ctype_space(Value value)
  {
    if (value instanceof LongValue)
      return isspace(value.toInt());
    else if (value instanceof StringValue) {
      string string = value.toString();

      if (string.length() == 0)
        return false;

      for (int i = 0; i < string.length(); i++) {
        if (! isspace(string.charAt(i)))
          return false;
      }

      return true;
    }
    else
      return false;
  }

  public static bool ctype_upper(Value value)
  {
    if (value instanceof LongValue)
      return isupper(value.toInt());
    else if (value instanceof StringValue) {
      string string = value.toString();

      if (string.length() == 0)
        return false;

      for (int i = 0; i < string.length(); i++) {
        if (! isupper(string.charAt(i)))
          return false;
      }

      return true;
    }
    else
      return false;
  }

  public static bool ctype_xdigit(Value value)
  {
    if (value instanceof LongValue)
      return isxdigit(value.toInt());
    else if (value instanceof StringValue) {
      string string = value.toString();

      if (string.length() == 0)
        return false;

      for (int i = 0; i < string.length(); i++) {
        if (! isxdigit(string.charAt(i)))
          return false;
      }

      return true;
    }
    else
      return false;
  }

  public static bool isalnum(int ch)
  {
    return ('a' <= ch && ch <= 'z'
            || 'A' <= ch && ch <= 'Z'
            || '0' <= ch && ch <= '9');
  }

  public static bool isalpha(int ch)
  {
    return ('a' <= ch && ch <= 'z'
            || 'A' <= ch && ch <= 'Z');
  }

  public static bool iscntrl(int ch)
  {
    return (0 <= ch && ch <= 31 || ch == 127);
  }

  public static bool isdigit(int ch)
  {
    return ('0' <= ch && ch <= '9');
  }

  public static bool isgraph(int ch)
  {
    return ('!' <= ch && ch <= '~');
  }

  public static bool islower(int ch)
  {
    return ('a' <= ch && ch <= 'z');
  }

  public static bool isprint(int ch)
  {
    return (' ' <= ch && ch <= '~');
  }

  public static bool ispunct(int ch)
  {
    return isprint(ch) && !isspace(ch) && !isalnum(ch);
  }

  public static bool isspace(int ch)
  {
    return ch == ' ' || 9 <= ch && ch <= 13;
  }

  public static bool isupper(int ch)
  {
    return ('A' <= ch && ch <= 'Z');
  }

  public static bool isxdigit(int ch)
  {
    return ((ch >= '0' && ch <= '9')
            || (ch >= 'a' && ch <= 'f')
            || (ch >= 'A' && ch <= 'F'));
  }
}
}
