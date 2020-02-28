using System;
namespace QuercusDotNet.lib.dom {
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
 * @author Sam
 */








public class DOMText
  : DOMCharacterData<Text>
{
  public static DOMText __construct(Env env, @Optional string value)
  {
    DOMText text = getImpl(env).createText();

    if (value != null && value.length() > 0)
      text.setNodeValue(value);

    return text;
  }

  DOMText(DOMImplementation impl, Text delegate)
  {
    super(impl, delegate);
  }

  public string getWholeText()
  {
    return _delegate.getWholeText();
  }

  public bool isElementContentWhitespace()
  {
    return _delegate.isElementContentWhitespace();
  }

  public bool isWhitespaceInElementContent()
  {
    return _delegate.isElementContentWhitespace();
  }

  public DOMText replaceWholeText(String content)
    
  {
    try {
      return wrap(_delegate.replaceWholeText(content));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMText splitText(int offset)
    
  {
    try {
      return wrap(_delegate.splitText(offset));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }
}
}
