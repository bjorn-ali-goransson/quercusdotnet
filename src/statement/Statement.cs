using System;
namespace QuercusDotNet.Statement{
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
 * Represents a PHP statement
 */
abstract public class Statement {
  private const Logger log = Logger.getLogger(Statement.class.getName());

  public const int FALL_THROUGH = 0;
  public const int BREAK_FALL_THROUGH = 0x1;
  public const int RETURN = 0x2;

  private Location _location;

  private Statement _parent;
  private string _loopLabel;

  protected Statement()
  {
    _location = Location.UNKNOWN;
  }

  protected Statement(Location location)
  {
    _location = location;
  }

  public Location getLocation()
  {
    return _location;
  }

  public bool isLoop()
  {
    return false;
  }

  public Statement getParent()
  {
    return _parent;
  }

  public void setParent(Statement parent)
  {
    _parent = parent;
  }

  abstract public Value execute(Env env);

  /**
   * Returns true if the statement can fallthrough.
   */
  public int fallThrough()
  {
    return FALL_THROUGH;
  }

  protected void rethrow(Throwable t)
    
  {
    rethrow(t, Throwable.class);
  }

  protected <E : Throwable> void rethrow(Throwable t, Class<E> cl)
    
  {
    E typedT;

    if (! cl.isAssignableFrom(t.getClass())) {
      try {
        typedT = cl.newInstance();
        typedT.initCause(t);
      }
      catch (InstantiationException e) {
        log.log(Level.WARNING, t.ToString(), t);
        throw new RuntimeException(e);
      }
      catch (IllegalAccessException e) {
        log.log(Level.WARNING, t.ToString(), t);
        throw new RuntimeException(e);
      }
    }
    else
      typedT = (E) t;

    Throwable rootCause = t;

    // guard against circular cause
    IdentityHashMap<Throwable, Boolean> causes
      = new IdentityHashMap<Throwable, Boolean>();

    causes.put(rootCause, Boolean.TRUE);

    while (rootCause.getCause() != null) {
      Throwable cause = rootCause.getCause();

      if (causes.containsKey(cause))
        break;

      causes.put(cause, Boolean.TRUE);

      rootCause = cause;
    }

    if (!(rootCause instanceof QuercusExecutionException)) {
      string rootCauseName = rootCause.getClass().getName();
      string rootCauseMessage = rootCause.getMessage();

      StringBuilder quercusExMessage = new StringBuilder();

      quercusExMessage.append(rootCauseName);

      if (rootCauseMessage != null && rootCauseMessage.length() > 0) {
        quercusExMessage.append(" ");
        quercusExMessage.append(rootCauseMessage);
      }

      QuercusExecutionException quercusEx
        = new QuercusExecutionException(quercusExMessage.ToString());

      StackTraceElement[] quercusExStackTrace = quercusEx.getStackTrace();
      StackTraceElement[] rootCauseStackTrace = rootCause.getStackTrace();

      int quercusExIndex = quercusExStackTrace.length - 1;
      int rootCauseIndex = rootCauseStackTrace.length - 1;

      while (rootCauseIndex >= 0 &&  quercusExIndex >= 0) {
        StackTraceElement
          rootCauseElement
          = rootCauseStackTrace[rootCauseIndex];
        StackTraceElement
          quercusExElement
          = quercusExStackTrace[quercusExIndex];

        if (! quercusExElement.equals(rootCauseElement))
          break;

        rootCauseIndex--;
        quercusExIndex--;
      }

      int len = rootCauseIndex + 1;

      StackTraceElement[] trimmedElements = new StackTraceElement[len];
      System.arraycopy(rootCauseStackTrace, 0, trimmedElements, 0, len);

      quercusEx.setStackTrace(trimmedElements);
      try {
        rootCause.initCause(quercusEx);
        rootCause = quercusEx;
      }
      catch (IllegalStateException ex) {
        // XXX: guard against reported bug that could not be reproduced
        log.log(Level.FINE, ex.ToString(), ex);
      }
    }

    string className = _location.getClassName();
    string functionName = _location.getFunctionName();
    string fileName = _location.getFileName();
    int lineNumber = _location.getLineNumber();

    if (className == null)
      className = "";

    if (functionName == null)
      functionName = "";

    StackTraceElement[] existingElements = rootCause.getStackTrace();
    int len = existingElements.length;
    StackTraceElement lastElement;

    if (len > 1)
      lastElement = existingElements[len - 1];
    else
      lastElement = null;

    // early return if function and class are same as last one
    if (lastElement != null
        && (functionName.equals(lastElement.getMethodName()))
        && (className.equals(lastElement.getClassName())))
    {
      throw typedT;
    }

    StackTraceElement[] elements = new StackTraceElement[len + 1];

    System.arraycopy(existingElements, 0, elements, 0, len);

    elements[len] = new StackTraceElement(className,
                                          functionName,
                                          fileName,
                                          lineNumber);

    rootCause.setStackTrace(elements);

    throw typedT;
  }

  public string ToString()
  {
    return getClass().getSimpleName() + "[]";
  }
}

}
