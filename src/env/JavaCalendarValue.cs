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
 * @author Scott Ferguson
 */








/**
 * Represents a Quercus java Calendar value.
 */
public class JavaCalendarValue : JavaValue {
  private const Logger log
    = Logger.getLogger(JavaCalendarValue.class.getName());
  
  private Calendar _calendar;
  
  public JavaCalendarValue(Env env, Calendar calendar, JavaClassDef def)
  {
    super(env, calendar, def);
    _calendar = calendar;
  }

  /**
   * Converts to a long.
   */
  public override long toLong()
  {
    return _calendar.getTimeInMillis();
  }
  
  /**
   * Converts to a Java Calendar.
   */
  public override Calendar toJavaCalendar()
  {
    return _calendar;
  }
  
  public string ToString()
  {
    return _calendar.getTime().ToString();
  }
}
}
