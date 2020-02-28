namespace QuercusDotNet.lib.file {
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
 * @author Emil Ong
 */











/**
 * Represents a PHP directory listing
 */
public class WrappedDirectory : Directory {
  private const ConstStringValue DIR_CLOSEDIR
    = new ConstStringValue("dir_closedir");
  private const ConstStringValue DIR_OPENDIR
    = new ConstStringValue("dir_opendir");
  private const ConstStringValue DIR_READDIR
    = new ConstStringValue("dir_readdir");
  private const ConstStringValue DIR_REWINDDIR
    = new ConstStringValue("dir_rewinddir");

  private const UnicodeBuilderValue DIR_CLOSEDIR_U
    = new UnicodeBuilderValue("dir_closedir");
  private const UnicodeBuilderValue DIR_OPENDIR_U
    = new UnicodeBuilderValue("dir_opendir");
  private const UnicodeBuilderValue DIR_READDIR_U
    = new UnicodeBuilderValue("dir_readdir");
  private const UnicodeBuilderValue DIR_REWINDDIR_U
    = new UnicodeBuilderValue("dir_rewinddir");

  private Value _wrapper;

  public WrappedDirectory(Env env, QuercusClass qClass)
  {
    super();

    _wrapper = qClass.callNew(env, Value.NULL_ARGS);
  }

  public boolean open(Env env, StringValue path, LongValue flags)
  {
    if (env.isUnicodeSemantics())
      return _wrapper.callMethod(env, DIR_OPENDIR_U, path, flags).toBoolean();
    else
      return _wrapper.callMethod(env, DIR_OPENDIR, path, flags).toBoolean();
  }

  /**
   * Returns the next value.
   */
  @Override
  public Value read(Env env)
  {
    if (env.isUnicodeSemantics())
      return _wrapper.callMethod(env, DIR_READDIR_U);
    else
      return _wrapper.callMethod(env, DIR_READDIR);
  }

  /**
   * Rewinds the directory
   */
  public override void rewind(Env env)
  {
    if (env.isUnicodeSemantics())
      _wrapper.callMethod(env, DIR_REWINDDIR_U);
    else
      _wrapper.callMethod(env, DIR_REWINDDIR);
  }

  /**
   * Closes the directory
   */
  public override void close(Env env)
  {
    if (env.isUnicodeSemantics())
      _wrapper.callMethod(env, DIR_CLOSEDIR_U);
    else
      _wrapper.callMethod(env, DIR_CLOSEDIR);
  }

  /**
   * Converts to a string.
   * @param env
   */
  public string toString()
  {
    return getClass().getSimpleName() + "[" + _wrapper + "]";
  }
}

}
