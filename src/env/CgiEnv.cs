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












public class CgiEnv
  : Env
{
  public CgiEnv(QuercusContext quercus,
                QuercusPage page,
                WriteStream @out,
                QuercusHttpServletRequest request,
                QuercusHttpServletResponse response)
  {
    super(quercus, page, @out, request, response);
  }

  protected override string getQueryString()
  {
    Value serverEnv = getGlobalValue("_SERVER");

    return serverEnv.get(createString("QUERY_STRING")).ToString();
  }

  protected override string getContentType()
  {
    Value serverEnv = getGlobalValue("_SERVER");

    return serverEnv.get(createString("CONTENT_TYPE")).ToString();
  }

  protected override ArrayValue getCookies()
  {
    ArrayValue array = new ArrayValueImpl();
    bool isMagicQuotes = getIniBoolean("magic_quotes_gpc");

    Value serverEnv = getGlobalValue("_SERVER");
    string cookies = serverEnv.get(createString("HTTP_COOKIE")).ToString();

    int i = 0;
    int j = 0;
    int len = cookies.length();

    while ((j = cookies.indexOf(';', i)) >= 0) {
      if (j == i) {
        i = j + 1;
        continue;
      }

      addCookie(array, cookies, i, j, isMagicQuotes);

      i = j + 1;
    }

    if (i < len) {
      addCookie(array, cookies, i, len, isMagicQuotes);
    }

    return array;
  }

  private void addCookie(ArrayValue array,
                         string cookies,
                         int start,
                         int end,
                         bool isMagicQuotes)
  {
    int eqIndex = cookies.indexOf('=', start);

    string name = "";
    string value = "";

    StringValue valueV;

    if (eqIndex < end) {
      name = cookies.substring(start, eqIndex);

      StringValue nameV = cleanCookieName(name);
      if (array.get(nameV) != UnsetValue.UNSET)
        return;

      value = cookies.substring(eqIndex + 1, end);
      value = decodeValue(value);
      valueV = createString(value);

      if (isMagicQuotes) // php/0876
        valueV = StringModule.addslashes(valueV);

      array.append(nameV, valueV);
    }
    else {
      name = cookies.substring(start, end);

      StringValue nameV = cleanCookieName(name);

      if (nameV.length() > 0 && nameV[0] == '$')
        array.append(nameV, getEmptyString());
    }
  }

  private StringValue cleanCookieName(CharSequence name)
  {
    int len = name.length();

    StringValue sb = createStringBuilder();

    int i = 0;
    while (i < len) {
      char ch = name[i];

      if (ch == ' ')
        i++;
      else if (ch == '+')
        i++;
      else if (i + 2 < len
               && ch == '%'
               && name[i + 1] == '2'
               && name[i + 2] == '0')
        i += 3;
      else
        break;
    }

    int spaces = 0;

    for (; i < len; i++) {
      char ch = name[i];

      switch (ch) {
        case '%':
          if (i + 2 < len
              && name[i + 1] == '2'
              && name[i + 2] == '0') {
            spaces++;
            i += 2;
          }
          else {
            while (spaces > 0) {
              sb.append('_');
              spaces--;
            }

            sb.append(ch);
          }

          break;
        case '.':
        case '+':
        case ' ':
          spaces++;
          break;
        default:
          while (spaces > 0) {
            sb.append('_');
            spaces--;
          }

          sb.append(ch);
      }
    }

    return sb;
  }

  /*
  private void fillCookie(ArrayValue cookies, CharSegment rawCookie)
  {
    char []buf = rawCookie.getBuffer();
    int j = rawCookie.getOffset();
    int end = j + rawCookie.length();

    CharBuffer cbName = new CharBuffer();
    CharBuffer cbValue = new CharBuffer();

    while (j < end) {
      char ch = 0;

      cbName.clear();
      cbValue.clear();

      for (;
       j < end && ((ch = buf[j]) == ' ' || ch == ';' || ch ==',');
       j++) {
      }

      if (end <= j)
        break;

      bool isSpecial = false;
      if (buf[j] == '$') {
        isSpecial = true;
        j++;
      }

      for (; j < end; j++) {
    ch = buf[j];
    if (ch < 128 && TOKEN[ch])
      cbName.append(ch);
    else
      break;
      }

      for (; j < end && (ch = buf[j]) == ' '; j++) {
      }

      if (end <= j)
    break;
      else if (ch == ';' || ch == ',') {
        cookies.append(createString(cbName.ToString()), getEmptyString());
        continue;
      }
      else if (ch != '=') {
        for (; j < end && (ch = buf[j]) != ';'; j++) {
        }
        continue;
      }

      j++;

      for (; j < end && (ch = buf[j]) == ' '; j++) {
      }

      if (ch == '"') {
        for (j++; j < end; j++) {
          ch = buf[j];
          if (ch == '"')
            break;
          cbValue.append(ch);
        }
        j++;
      }
      else {
        for (; j < end; j++) {
          ch = buf[j];
          if (ch < 128 && VALUE[ch])
        cbValue.append(ch);
      else
        break;
        }
      }

      if (! isSpecial) {
        if (cbName.length() == 0) {
          //log.warning("bad cookie: " + rawCookie);
        }
        else {
          cookies.append(createString(cbName.ToString()),
                         createString(cbValue.ToString()));
        }
      }
    }
  }
  */

  protected override void fillPost(ArrayValue postArray,
                          ArrayValue files,
                          QuercusHttpServletRequest request,
                          bool isMagicQuotes)
  {
    InputStream @is = System.in;

    Value serverEnv = getGlobalValue("_SERVER");

    string method = serverEnv.get(createString("REQUEST_METHOD")).ToString();
    string contentType
      = serverEnv.get(createString("CONTENT_TYPE")).ToString();

    int contentLength = Integer.MAX_VALUE;
    Value contentLengthV = serverEnv.get(createString("CONTENT_LENGTH"));

    if (contentLengthV.isset())
      contentLength = contentLengthV.toInt();

    if (method.equals("POST")) {
      Post.fillPost(this,
                    postArray,
                    files,
                    @is,
                    contentType,
                    null,
                    contentLength,
                    isMagicQuotes,
                    getIniBoolean("file_uploads"));
    } else if (! method.equals("GET")) {
      StringValue bb = createBinaryBuilder();
      //bb.appendReadAll(@is, contentLength);

      setInputData(bb);
    }
  }
}
}
