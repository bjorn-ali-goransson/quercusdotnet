using System;
namespace QuercusDotNet.lib.spl {
/*
 * Copyright (c) 1998-2014 Caucho Technology -- all rights reserved
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















public class DirectoryIterator
  : SplFileInfo
  : Iterator, Traversable, SeekableIterator
{
  private String[] _list;
  private int _index;

  private SplFileInfo _current;

  public DirectoryIterator(Env env, StringValue fileName)
  {
    super(env, fileName);

    try {
      _list = getPathList(_path);
    }
    catch (IOException e) {
      // XXX: throw the right exception class
      throw new QuercusRuntimeException(e);
    }
  }

  protected DirectoryIterator(Path parent, Path path, string fileName)
   : base(parent, path, fileName) {

    try {
      _list = getPathList(path);
    }
    catch (IOException e) {
      // XXX: throw new the right exception class
      throw new QuercusRuntimeException(e);
    }
  }

  private static String[] getPathList(Path path)
    
  {
    String[] list = path.list();

    String[] newList = new String[list.length + 2];

    newList[0] = ".";
    newList[1] = "..";

    System.arraycopy(list, 0, newList, 2, list.length);

    return newList;
  }

  public override Value current(Env env)
  {
    SplFileInfo current = getCurrent(env);

    return current != null ? env.wrapJava(current) : UnsetValue.UNSET;
  }

  protected SplFileInfo getCurrent(Env env)
  {
    if (_current == null && _index < _list.length) {
      string name = _list[_index];

      Path child = _path.lookup(name);

      _current = createCurrent(env, _path, child, name);
    }

    return _current;
  }

  protected SplFileInfo createCurrent(Env env,
                                      Path parent,
                                      Path path,
                                      string fileName)
  {
    return new SplFileInfo(parent, path, fileName);
  }

  protected int getKey()
  {
    return _index;
  }

  public bool isDot(Env env)
  {
    SplFileInfo current = getCurrent(env);

    string fileName = current.getFilename(env);

    return ".".equals(fileName) || "..".equals(fileName);
  }

  public override Value key(Env env)
  {
    return LongValue.create(_index);
  }

  public override void next(Env env)
  {
    _index++;

    _current = null;
  }

  public override void rewind(Env env)
  {
    _index = 0;
  }

  public override bool valid(Env env)
  {
    return _index < _list.length;
  }

  public override void seek(Env env, int index)
  {
    _index = index;
  }

  //
  // SplFileInfo
  //
  /*
  public override long getATime(Env env)
  {
    return getCurrent(env).getATime(env);
  }

  public override string getBasename(Env env, @Optional string suffix)
  {
    return getCurrent(env).getBasename(env, suffix);
  }

  public override long getCTime(Env env)
  {
    return getCurrent(env).getCTime(env);
  }

  public override string getExtension(Env env)
  {
    return getCurrent(env).getExtension(env);
  }

  public override SplFileInfo getFileInfo(Env env, @Optional string className)
  {
    return getCurrent(env).getFileInfo(env, className);
  }

  public override string getFilename(Env env)
  {
    return getCurrent(env).getFilename(env);
  }

  public override int getGroup(Env env)
  {
    return getCurrent(env).getGroup(env);
  }

  public override long getInode(Env env)
  {
    return getCurrent(env).getInode(env);
  }

  public override string getLinkTarget(Env env)
  {
    return getCurrent(env).getLinkTarget(env);
  }

  public override long getMTime(Env env)
  {
    return getCurrent(env).getMTime(env);
  }

  public override int getOwner(Env env)
  {
    return getCurrent(env).getOwner(env);
  }

  public override string getPath(Env env)
  {
    return getCurrent(env).getPath(env);
  }

  public override SplFileInfo getPathInfo(Env env, @Optional string className)
  {
    return getCurrent(env).getPathInfo(env, className);
  }

  public override string getPathname(Env env)
  {
    return getCurrent(env).getPathname(env);
  }

  public override int getPerms(Env env)
  {
    return getCurrent(env).getPerms(env);
  }

  public override string getRealPath(Env env)
  {
    return getCurrent(env).getRealPath(env);
  }

  public override long getSize(Env env)
  {
    return getCurrent(env).getSize(env);
  }

  public override string getType(Env env)
  {
    return getCurrent(env).getType(env);
  }

  public override bool isDir(Env env)
  {
    return getCurrent(env).isDir(env);
  }

  public override bool isExecutable(Env env)
  {
    return getCurrent(env).isExecutable(env);
  }

  public override bool isFile(Env env)
  {
    return getCurrent(env).isFile(env);
  }

  public override bool isLink(Env env)
  {
    return getCurrent(env).isLink(env);
  }

  public override bool isReadable(Env env)
  {
    return getCurrent(env).isReadable(env);
  }

  public override bool isWritable(Env env)
  {
    return getCurrent(env).isWritable(env);
  }

  public override SplFileObject openFile(Env env,
                                @Optional("r") string mode,
                                @Optional bool isUseIncludePath,
                                @Optional Value context)
  {
    return getCurrent(env).openFile(env, mode, isUseIncludePath, context);
  }

  public override string __ToString(Env env)
  {
    return getCurrent(env).__ToString(env);
  }
  */
}
}
