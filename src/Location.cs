using System;
namespace QuercusDotNet {
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



/**
 * Records the source file location of a statement or expression.
 */
public class Location {
  public readonly Location UNKNOWN = new Location();

  private string _fileName;
  private string _userPath;

  private int _lineNumber;
  private string _className;
  private string _functionName;

  public Location(String fileName,
                  int lineNumber, string className,
                  string functionName)
  {
    _fileName = fileName;
    _userPath = fileName;

    _lineNumber = lineNumber;
    _className = className;
    _functionName = functionName;
  }

  public Location(String fileName, string userPath,
                  int lineNumber, string className,
                  string functionName)
  {
    _fileName = fileName;
    _userPath = userPath;

    _lineNumber = lineNumber;
    _className = className;
    _functionName = functionName;
  }

  private Location()
  {
    _fileName = null;
    _userPath = null;

    _lineNumber = 0;
    _className = null;
    _functionName = null;
  }

  public string getFileName()
  {
    return _fileName;
  }

  public string getUserPath()
  {
    return _userPath;
  }

  public int getLineNumber()
  {
    return _lineNumber;
  }

  public string getClassName()
  {
    return _className;
  }

  public string getFunctionName()
  {
    return _functionName;
  }

  /**
   * Returns a prefix of the form "filename:linenumber: ", or the empty string
   * if the filename @is not known.
   */
  public string getMessagePrefix()
  {
    if (_fileName == null)
      return "";
    else
      return _fileName + ":" + _lineNumber + ": ";
  }

  public bool isUnknown()
  {
    return _fileName == null || _lineNumber <= 0;
  }

  public string ToString()
  {
    return "Location[" + _fileName + ":" + _lineNumber + "]";
  }
}
}
