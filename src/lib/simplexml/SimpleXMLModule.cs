using System;
namespace QuercusDotNet.lib.simplexml {
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
 * @author Charles Reich
 */





















/**
 * PHP SimpleXML
 */
public class SimpleXMLModule
  : AbstractQuercusModule
{
  private const Logger log
    = Logger.getLogger(SimpleXMLModule.class.getName());
  private readonly L10N L = new L10N(SimpleXMLModule.class);

  public string []getLoadedExtensions()
  {
    return new String[] { "SimpleXML" };
  }

  public Value simplexml_load_string(Env env,
                                     StringValue data,
                                     @Optional string className,
                                     @Optional int options,
                                     @Optional Value namespaceV,
                                     @Optional bool isPrefix)
  {
    if (data.length() == 0) {
      return BooleanValue.FALSE;
    }

    if (className == null || className.length() == 0)
      className = "SimpleXMLElement";

    QuercusClass cls = env.getClass(className);

    return SimpleXMLElement.create(env, cls,
                                   data, options, false,
                                   namespaceV, isPrefix);
  }

  public Value simplexml_load_file(Env env,
                                   @NotNull StringValue file,
                                   @Optional string className,
                                   @Optional int options,
                                   @Optional Value namespaceV,
                                   @Optional bool isPrefix)
  {
    if (className == null || className.length() == 0) {
      className = "SimpleXMLElement";
    }

    QuercusClass cls = env.getClass(className);

    return SimpleXMLElement.create(env, cls,
                                   file, options, true,
                                   namespaceV, isPrefix);
  }

  public SimpleXMLElement simplexml_import_dom(Env env)
  {
    // XXX: DOMNode needs to be able to export partial documents
    throw new UnimplementedException("simplexml_import_dom");
  }
}
}
