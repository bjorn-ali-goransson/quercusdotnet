using System;
namespace QuercusDotNet.lib.memcache {
/*
 * Copyright (c) 1998-2013 Caucho Technology -- all rights reserved
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







public class Memcached
{
  public const int OPT_COMPRESSION = 0;
  public const int OPT_SERIALIZER = 1;
  public const int SERIALIZER_PHP = 2;
  public const int SERIALIZER_IGBINARY = 3;
  public const int SERIALIZER_JSON = 4;
  public const int OPT_PREFIX_KEY = 5;
  public const int OPT_HASH = 6;
  public const int HASH_DEFAULT = 7;
  public const int HASH_MD5 = 8;
  public const int HASH_CRC = 9;
  public const int HASH_FNV1_64 = 10;
  public const int HASH_FNV1A_64 = 11;
  public const int HASH_FNV1_32 = 12;
  public const int HASH_FNV1A_32 = 13;
  public const int HASH_HSIEH = 14;
  public const int HASH_MURMUR = 15;
  public const int OPT_DISTRIBUTION = 16;
  public const int DISTRIBUTION_MODULA = 17;
  public const int DISTRIBUTION_CONSISTENT = 18;
  public const int OPT_LIBKETAMA_COMPATIBLE = 19;
  public const int OPT_BUFFER_WRITES = 20;
  public const int OPT_BINARY_PROTOCOL = 21;
  public const int OPT_NO_BLOCK = 22;
  public const int OPT_TCP_NODELAY = 23;
  public const int OPT_SOCKET_SEND_SIZE = 24;
  public const int OPT_SOCKET_RECV_SIZE = 25;
  public const int OPT_CONNECT_TIMEOUT = 26;
  public const int OPT_RETRY_TIMEOUT = 27;
  public const int OPT_SEND_TIMEOUT = 28;
  public const int OPT_RECV_TIMEOUT = 29;
  public const int OPT_POLL_TIMEOUT = 30;
  public const int OPT_CACHE_LOOKUPS = 31;
  public const int OPT_SERVER_FAILURE_LIMIT = 32;
  public const int HAVE_IGBINARY = 33;
  public const int HAVE_JSON = 34;
  public const int GET_PRESERVE_ORDER = 35;
  public const int RES_SUCCESS = 36;
  public const int RES_FAILURE = 37;
  public const int RES_HOST_LOOKUP_FAILURE = 38;
  public const int RES_UNKNOWN_READ_FAILURE = 39;
  public const int RES_PROTOCOL_ERROR = 40;
  public const int RES_CLIENT_ERROR = 41;
  public const int RES_SERVER_ERROR = 42;
  public const int RES_WRITE_FAILURE = 43;
  public const int RES_DATA_EXISTS = 44;
  public const int RES_NOTSTORED = 45;
  public const int RES_NOTFOUND = 46;
  public const int RES_PARTIAL_READ = 47;
  public const int RES_SOME_ERRORS = 48;
  public const int RES_NO_SERVERS = 49;
  public const int RES_END = 50;
  public const int RES_ERRNO = 51;
  public const int RES_BUFFERED = 52;
  public const int RES_TIMEOUT = 53;
  public const int RES_BAD_KEY_PROVIDED = 54;
  public const int RES_CONNECTION_SOCKET_CREATE_FAILURE = 55;
  public const int RES_PAYLOAD_FAILURE = 56;

  public bool setOption(Env env, int option, Value value)
  {
    return false;
  }

  public bool setOptions(Env env, ArrayValue array)
  {
    return false;
  }
}
}
