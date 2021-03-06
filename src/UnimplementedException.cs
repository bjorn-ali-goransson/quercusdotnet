using System;
namespace QuercusDotNet {
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
 *   Free SoftwareFoundation, Inc.
 *   59 Temple Place, Suite 330
 *   Boston, MA 02111-1307  USA
 *
 * @author Sam
 */





public class UnimplementedException
  : UnsupportedOperationException
{
  private readonly L10N L = new L10N(UnimplementedException.class);

  private const string MESSAGE
    = "{0} has not been implemented. "
      + "A more recent version of Quercus may be available "
      + "at http://www.caucho.com/download "
      + "Requests for unimplemented features can be "
      + "entered in the bugtracking system at http://bugs.caucho.com";

  public UnimplementedException()
  {
    super(createMessage(null));
  }

  public UnimplementedException(String functionality)
   : base(createMessage(functionality)) {
  }

  public UnimplementedException(String functionality, Throwable cause)
   : base(createMessage(functionality), cause) {
  }

  public UnimplementedException(Throwable cause)
   : base(createMessage(null), cause) {
  }

  private static string createMessage(String functionality)
  {
    if (functionality == null)
      functionality = "This functionality";
    else
      functionality = "`" + functionality + "'";

    return L.l(MESSAGE, functionality);
  }
}
}
