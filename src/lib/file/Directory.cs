using System;
namespace QuercusDotNet.lib.file {
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











@ResourceType("stream")
public class Directory
{
  public Directory handle;
  public string path;

  private string []_list;
  private int _index;

  protected Directory()
  {
    handle = this;
  }

  protected Directory(Env env, Path dir)
    
  {
    handle = this;

    this.path = dir.ToString();

    _list = dir.list();
  }

  public void open(Env env)
  {
  }

  public Value read(Env env)
  {
    if (_index < _list.length)
      return env.createString(_list[_index++]);
    else
      return BooleanValue.FALSE;
  }

  public void rewind(Env env)
  {
    _index = 0;
  }

  public void close(Env env)
  {
  }

  public void cleanup()
  {
    close(Env.getInstance());
  }

  public string ToString()
  {
    return getClass().getSimpleName() + "[" + path + "]";
  }
}
}
