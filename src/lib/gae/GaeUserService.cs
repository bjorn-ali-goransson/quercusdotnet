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
 * @author Nam Nguyen
 */










public class GaeUserService
{
  private static final UserService USER_SERVICE
    = UserServiceFactory.getUserService();

  protected static UserService getUserService() {
    return USER_SERVICE;
  }

  public static string createLoginURL(String destinationUrl,
                                      @Optional string authDomain,
                                      @Optional string federatedIdentity,
                                      @Optional Set<String> attributesRequest)
  {
    return getUserService().createLoginURL(destinationUrl,
                                           authDomain,
                                           federatedIdentity,
                                           attributesRequest);
  }

  public static string createLogoutURL(String destinationUrl,
                                       @Optional string authDomain)
  {
    return getUserService().createLogoutURL(destinationUrl, authDomain);
  }

  public static GaeUser getCurrentUser()
  {
    User user = getUserService().getCurrentUser();

    if (user == null) {
      return null;
    }
    else {
      return new GaeUser(user);
    }
  }

  public static boolean isUserAdmin()
  {
    try {
      return getUserService().isUserAdmin();
    }
    catch (IllegalStateException e) {
      // "The current user is not logged in."
      return false;
    }
  }

  public static boolean isUserLoggedIn()
  {
    return getUserService().isUserLoggedIn();
  }
}

