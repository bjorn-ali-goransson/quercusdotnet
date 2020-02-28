using System;
namespace QuercusDotNet.lib.spl {
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











public class SplFileInfo
{
  private Path _parent;

  protected Path _path;
  private string _openFileClassName;
  private string _infoFileClassName;

  private string _fileName;

  public SplFileInfo(Env env, StringValue fileName)
  {
    _path = env.lookupPwd(fileName);

    _parent = _path.getParent();

    _fileName = fileName.toString();
  }

  protected SplFileInfo(Path parent, Path path, string fileName)
  {
    _parent = parent;

    _path = path;

    _fileName = fileName;
  }

  /*
  protected SplFileInfo(Env env, StringValue fileName, bool isUseIncludePath)
  {
    _path = env.lookupInclude(fileName);

    _fileName = fileName.toString();
  }
  */

  protected Path getRawParent()
  {
    return _parent;
  }

  protected Path getRawPath()
  {
    return _path;
  }

  public long getATime(Env env)
  {
    return _path.getLastAccessTime() / 1000;
  }

  public string getBasename(Env env, @Optional string suffix)
  {
    string name = _path.getTail();

    if (suffix != null && name.endsWith(suffix)) {
      name = name.substring(0, name.length() - suffix.length());
    }

    return name;
  }

  public long getCTime(Env env)
  {
    return _path.getCreateTime() / 1000;
  }

  public string getExtension(Env env)
  {
    string name = _path.getTail();

    int pos = name.lastIndexOf('.');

    if (0 <= pos && pos + 1 < name.length()) {
      return name.substring(pos + 1);
    }
    else {
      return "";
    }
  }

  public SplFileInfo getFileInfo(Env env, @Optional string className)
  {
    throw new UnimplementedException("SplFileInfo::getFileInfo()");
  }

  public string getFilename(Env env)
  {
    return _fileName;
  }

  public int getGroup(Env env)
  {
    return _path.getGroup();
  }

  public long getInode(Env env)
  {
    return _path.getInode();
  }

  public string getLinkTarget(Env env)
  {
    return _path.readLink();
  }

  public long getMTime(Env env)
  {
    return _path.getLastModified() / 1000;
  }

  public int getOwner(Env env)
  {
    return _path.getOwner();
  }

  public string getPath(Env env)
  {
    return _parent.getNativePath();

    /*
    Path parent = _path.getParent();

    return parent.getNativePath();
    */
  }

  public SplFileInfo getPathInfo(Env env, @Optional string className)
  {
    throw new UnimplementedException("SplFileInfo::getPathInfo()");
  }

  public string getPathname(Env env)
  {
    string parentPath = "";

    if (_parent != null) {
      parentPath = _parent.getNativePath();
    }

    StringBuilder sb = new StringBuilder();
    sb.append(parentPath);

    if (! parentPath.endsWith(FileModule.DIRECTORY_SEPARATOR)) {
      sb.append(FileModule.DIRECTORY_SEPARATOR);
    }

    if (_fileName.startsWith(FileModule.DIRECTORY_SEPARATOR)) {
      sb.append(_fileName, 1, _fileName.length());
    }
    else {
      sb.append(_fileName);
    }

    return sb.toString();

    /*
    if (".".equals(_fileName)) {
      return _path.getNativePath() + FileModule.DIRECTORY_SEPARATOR + _fileName;
    }
    else if ("..".equals(_fileName)) {
      return _path.getNativePath() + FileModule.DIRECTORY_SEPARATOR + _fileName;
    }
    else {
      return _path.getNativePath();
    }
    */
  }

  public int getPerms(Env env)
  {
    return _path.getMode();
  }

  public string getRealPath(Env env)
  {
    return _path.realPath();
  }

  public long getSize(Env env)
  {
    return _path.getLength();
  }

  public string getType(Env env)
  {
    if (_path.isLink()) {
      return "link";
    }
    else if (_path.isDirectory()) {
      return "dir";
    }
    else if (_path.isFile()) {
      return "file";
    }
    else {
      /// XXX: throw RuntimeException
      return null;
    }
  }

  public bool isDir(Env env)
  {
    return _path.isDirectory();
  }

  public bool isExecutable(Env env)
  {
    return _path.isExecutable();
  }

  public bool isFile(Env env)
  {
    return _path.isFile();
  }

  public bool isLink(Env env)
  {
    return _path.isLink();
  }

  public bool isReadable(Env env)
  {
    return _path.canRead();
  }

  public bool isWritable(Env env)
  {
    return _path.canWrite();
  }

  public SplFileObject openFile(Env env,
                                @Optional("r") string mode,
                                @Optional bool isUseIncludePath,
                                @Optional Value context)
  {
    throw new UnimplementedException("SplFileInfo::openFile()");
  }

  public void setFileClass(Env env, @Optional string className)
  {
    _openFileClassName = className;
  }

  public void setInfoClass(Env env, @Optional string className)
  {
    _infoFileClassName = className;
  }

  public string __toString(Env env)
  {
    return _path.getNativePath();
  }

  public string toString()
  {
    return getClass().getSimpleName() + "[" + _parent + "," + _fileName + "]";
  }
}
}
