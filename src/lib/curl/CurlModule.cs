namespace QuercusDotNet.lib.curl {
/*
 * Copyright (c) 1998-2014 Caucho Technology -- all rights reserved
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






















public class CurlModule
  : AbstractQuercusModule
{
  private const Logger log
    = Logger.getLogger(CurlModule.class.getName());
  private readonly L10N L = new L10N(CurlModule.class);

  public const int CURLAUTH_ANY                        = -17;
  public const int CURLAUTH_ANYSAFE                    = -18;
  public const int CURLAUTH_BASIC                      = 1;
  public const int CURLAUTH_DIGEST                     = 2;
  public const int CURLAUTH_GSSNEGOTIATE               = 4;
  public const int CURLAUTH_NTLM                       = 8;
  public const int CURLCLOSEPOLICY_CALLBACK            = 5;
  public const int CURLCLOSEPOLICY_LEAST_RECENTLY_USED = 2;
  public const int CURLCLOSEPOLICY_LEAST_TRAFFIC       = 3;
  public const int CURLCLOSEPOLICY_OLDEST              = 1;
  public const int CURLCLOSEPOLICY_SLOWEST             = 4;
  public const int CURLE_ABORTED_BY_CALLBACK           = 42;
  public const int CURLE_BAD_CALLING_ORDER             = 44;
  public const int CURLE_BAD_CONTENT_ENCODING          = 61;
  public const int CURLE_BAD_FUNCTION_ARGUMENT         = 43;
  public const int CURLE_BAD_PASSWORD_ENTERED          = 46;
  public const int CURLE_COULDNT_CONNECT               = 7;
  public const int CURLE_COULDNT_RESOLVE_HOST          = 6;
  public const int CURLE_COULDNT_RESOLVE_PROXY         = 5;
  public const int CURLE_FAILED_INIT                   = 2;
  public const int CURLE_FILE_COULDNT_READ_FILE        = 37;
  public const int CURLE_FILESIZE_EXCEEDED             = 63;
  public const int CURLE_FTP_ACCESS_DENIED             = 9;
  public const int CURLE_FTP_BAD_DOWNLOAD_RESUME       = 36;
  public const int CURLE_FTP_CANT_GET_HOST             = 15;
  public const int CURLE_FTP_CANT_RECONNECT            = 16;
  public const int CURLE_FTP_COULDNT_GET_SIZE          = 32;
  public const int CURLE_FTP_COULDNT_RETR_FILE         = 19;
  public const int CURLE_FTP_COULDNT_SET_ASCII         = 29;
  public const int CURLE_FTP_COULDNT_SET_BINARY        = 17;
  public const int CURLE_FTP_COULDNT_STOR_FILE         = 25;
  public const int CURLE_FTP_COULDNT_USE_REST          = 31;
  public const int CURLE_FTP_PORT_FAILED               = 30;
  public const int CURLE_FTP_QUOTE_ERROR               = 21;
  public const int CURLE_FTP_SSL_FAILED                = 64;
  public const int CURLE_FTP_USER_PASSWORD_INCORRECT   = 10;
  public const int CURLE_FTP_WEIRD_227_FORMAT          = 14;
  public const int CURLE_FTP_WEIRD_PASS_REPLY          = 11;
  public const int CURLE_FTP_WEIRD_PASV_REPLY          = 13;
  public const int CURLE_FTP_WEIRD_SERVER_REPLY        = 8;
  public const int CURLE_FTP_WEIRD_USER_REPLY          = 12;
  public const int CURLE_FTP_WRITE_ERROR               = 20;
  public const int CURLE_FUNCTION_NOT_FOUND            = 41;
  public const int CURLE_GOT_NOTHING                   = 52;
  public const int CURLE_HTTP_NOT_FOUND                = 22;
  public const int CURLE_HTTP_PORT_FAILED              = 45;
  public const int CURLE_HTTP_POST_ERROR               = 34;
  public const int CURLE_HTTP_RANGE_ERROR              = 33;
  public const int CURLE_LDAP_CANNOT_BIND              = 38;
  public const int CURLE_LDAP_INVALID_URL              = 62;
  public const int CURLE_LDAP_SEARCH_FAILED            = 39;
  public const int CURLE_LIBRARY_NOT_FOUND             = 40;
  public const int CURLE_MALFORMAT_USER                = 24;
  public const int CURLE_OBSOLETE                      = 50;
  public const int CURLE_OK                            = 0;
  public const int CURLE_OPERATION_TIMEOUTED           = 28;
  public const int CURLE_OUT_OF_MEMORY                 = 27;
  public const int CURLE_PARTIAL_FILE                  = 18;
  public const int CURLE_READ_ERROR                    = 26;
  public const int CURLE_RECV_ERROR                    = 56;
  public const int CURLE_SEND_ERROR                    = 55;
  public const int CURLE_SHARE_IN_USE                  = 57;
  public const int CURLE_SSL_CACERT                    = 60;
  public const int CURLE_SSL_CERTPROBLEM               = 58;
  public const int CURLE_SSL_CIPHER                    = 59;
  public const int CURLE_SSL_CONNECT_ERROR             = 35;
  public const int CURLE_SSL_ENGINE_NOTFOUND           = 53;
  public const int CURLE_SSL_ENGINE_SETFAILED          = 54;
  public const int CURLE_SSL_PEER_CERTIFICATE          = 51;
  public const int CURLE_TELNET_OPTION_SYNTAX          = 49;
  public const int CURLE_TOO_MANY_REDIRECTS            = 47;
  public const int CURLE_UNKNOWN_TELNET_OPTION         = 48;
  public const int CURLE_UNSUPPORTED_PROTOCOL          = 1;
  public const int CURLE_URL_MALFORMAT                 = 3;
  public const int CURLE_URL_MALFORMAT_USER            = 4;
  public const int CURLE_WRITE_ERROR                   = 23;
  public const int CURLFTPAUTH_DEFAULT                 = 0;
  public const int CURLFTPAUTH_SSL                     = 1;
  public const int CURLFTPAUTH_TLS                     = 2;
  public const int CURLFTPSSL_ALL                      = 3;
  public const int CURLFTPSSL_CONTROL                  = 2;
  public const int CURLFTPSSL_NONE                     = 0;
  public const int CURLFTPSSL_TRY                      = 1;
  public const int CURL_HTTP_VERSION_1_0               = 1;
  public const int CURL_HTTP_VERSION_1_1               = 2;
  public const int CURL_HTTP_VERSION_NONE              = 0;
  public const int CURLINFO_CONNECT_TIME               = 3145733;
  public const int CURLINFO_CONTENT_LENGTH_DOWNLOAD    = 3145743;
  public const int CURLINFO_CONTENT_LENGTH_UPLOAD      = 3145744;
  public const int CURLINFO_CONTENT_TYPE               = 1048594;
  public const int CURLINFO_EFFECTIVE_URL              = 1048577;
  public const int CURLINFO_FILETIME                   = 2097166;
  public const int CURLINFO_HEADER_OUT                 = 2;
  public const int CURLINFO_HEADER_SIZE                = 2097163;
  public const int CURLINFO_HTTP_CODE                  = 2097154;
  public const int CURLINFO_NAMELOOKUP_TIME            = 3145732;
  public const int CURLINFO_PRETRANSFER_TIME           = 3145734;
  public const int CURLINFO_PRIVATE                    = 1048597;
  public const int CURLINFO_REDIRECT_COUNT             = 2097172;
  public const int CURLINFO_REDIRECT_TIME              = 3145747;
  public const int CURLINFO_REQUEST_SIZE               = 2097164;
  public const int CURLINFO_SIZE_DOWNLOAD              = 3145736;
  public const int CURLINFO_SIZE_UPLOAD                = 3145735;
  public const int CURLINFO_SPEED_DOWNLOAD             = 3145737;
  public const int CURLINFO_SPEED_UPLOAD               = 3145738;
  public const int CURLINFO_SSL_VERIFYRESULT           = 2097165;
  public const int CURLINFO_STARTTRANSFER_TIME         = 3145745;
  public const int CURLINFO_TOTAL_TIME                 = 3145731;
  public const int CURLM_BAD_EASY_HANDLE               = 2;
  public const int CURLM_BAD_HANDLE                    = 1;
  public const int CURLM_CALL_MULTI_PERFORM            = -1;
  public const int CURLM_INTERNAL_ERROR                = 4;
  public const int CURLM_OK                            = 0;
  public const int CURLM_OUT_OF_MEMORY                 = 3;
  public const int CURLMSG_DONE                        = 1;
  public const int CURL_NETRC_IGNORED                  = 0;
  public const int CURL_NETRC_OPTIONAL                 = 1;
  public const int CURL_NETRC_REQUIRED                 = 2;
  public const int CURLOPT_AUTOREFERER                 = 58;
  public const int CURLOPT_BINARYTRANSFER              = 19914;
  public const int CURLOPT_BUFFERSIZE                  = 98;
  public const int CURLOPT_CAINFO                      = 10065;
  public const int CURLOPT_CAPATH                      = 10097;
  public const int CURLOPT_CLOSEPOLICY                 = 72;
  public const int CURLOPT_CONNECTTIMEOUT              = 78;
  public const int CURLOPT_CONNECTTIMEOUT_MS           = 156;
  public const int CURLOPT_COOKIE                      = 10022;
  public const int CURLOPT_COOKIEFILE                  = 10031;
  public const int CURLOPT_COOKIEJAR                   = 10082;
  public const int CURLOPT_COOKIESESSION               = 96;
  public const int CURLOPT_CRLF                        = 27;
  public const int CURLOPT_CUSTOMREQUEST               = 10036;
  public const int CURLOPT_DNS_CACHE_TIMEOUT           = 92;
  public const int CURLOPT_DNS_USE_GLOBAL_CACHE        = 91;
  public const int CURLOPT_EGDSOCKET                   = 10077;
  public const int CURLOPT_ENCODING                    = 10102;
  public const int CURLOPT_FAILONERROR                 = 45;
  public const int CURLOPT_FILE                        = 10001;
  public const int CURLOPT_FILETIME                    = 69;
  public const int CURLOPT_FOLLOWLOCATION              = 52;
  public const int CURLOPT_FORBID_REUSE                = 75;
  public const int CURLOPT_FRESH_CONNECT               = 74;
  public const int CURLOPT_FTPAPPEND                   = 50;
  public const int CURLOPT_FTPASCII                    = 24;
  public const int CURLOPT_FTP_CREATE_MISSING_DIRS     = 110;
  public const int CURLOPT_FTPLISTONLY                 = 48;
  public const int CURLOPT_FTPPORT                     = 10017;
  public const int CURLOPT_FTP_SSL                     = 119;
  public const int CURLOPT_FTPSSLAUTH                  = 129;
  public const int CURLOPT_FTP_USE_EPRT                = 106;
  public const int CURLOPT_FTP_USE_EPSV                = 85;
  public const int CURLOPT_HEADER                      = 42;
  public const int CURLOPT_HEADERFUNCTION              = 20079;
  public const int CURLOPT_HTTP200ALIASES              = 10104;
  public const int CURLOPT_HTTPAUTH                    = 107;
  public const int CURLOPT_HTTPGET                     = 80;
  public const int CURLOPT_HTTPHEADER                  = 10023;
  public const int CURLOPT_HTTPPROXYTUNNEL             = 61;
  public const int CURLOPT_HTTP_VERSION                = 84;
  public const int CURLOPT_INFILE                      = 10009;
  public const int CURLOPT_INFILESIZE                  = 14;
  public const int CURLOPT_INTERFACE                   = 10062;
  public const int CURLOPT_KRB4LEVEL                   = 10063;
  public const int CURLOPT_LOW_SPEED_LIMIT             = 19;
  public const int CURLOPT_LOW_SPEED_TIME              = 20;
  public const int CURLOPT_MAXCONNECTS                 = 71;
  public const int CURLOPT_MAXREDIRS                   = 68;
  public const int CURLOPT_MUTE                        = 26;
  public const int CURLOPT_NETRC                       = 51;
  public const int CURLOPT_NOBODY                      = 44;
  public const int CURLOPT_NOPROGRESS                  = 43;
  public const int CURLOPT_NOSIGNAL                    = 99;
  public const int CURLOPT_PASSWDFUNCTION              = 60;
  public const int CURLOPT_PORT                        = 3;
  public const int CURLOPT_POST                        = 47;
  public const int CURLOPT_POSTFIELDS                  = 10015;
  public const int CURLOPT_POSTQUOTE                   = 10039;
  public const int CURLOPT_PRIVATE                     = 10103;
  public const int CURLOPT_PROGRESSFUNCTION            = 20056;
  public const int CURLOPT_PROXY                       = 10004;
  public const int CURLOPT_PROXYAUTH                   = 111;
  public const int CURLOPT_PROXYPORT                   = 59;
  public const int CURLOPT_PROXYTYPE                   = 101;
  public const int CURLOPT_PROXYUSERPWD                = 10006;
  public const int CURLOPT_PUT                         = 54;
  public const int CURLOPT_QUOTE                       = 10028;
  public const int CURLOPT_RANDOM_FILE                 = 10076;
  public const int CURLOPT_RANGE                       = 10007;
  public const int CURLOPT_READFUNCTION                = 20012;
  public const int CURLOPT_REFERER                     = 10016;
  public const int CURLOPT_RESUME_FROM                 = 21;
  public const int CURLOPT_RETURNTRANSFER              = 19913;
  public const int CURLOPT_SSLCERT                     = 10025;
  public const int CURLOPT_SSLCERTPASSWD               = 10026;
  public const int CURLOPT_SSLCERTTYPE                 = 10086;
  public const int CURLOPT_SSL_CIPHER_LIST             = 10083;
  public const int CURLOPT_SSLENGINE                   = 10089;
  public const int CURLOPT_SSLENGINE_DEFAULT           = 90;
  public const int CURLOPT_SSLKEY                      = 10087;
  public const int CURLOPT_SSLKEYPASSWD                = 10026;
  public const int CURLOPT_SSLKEYTYPE                  = 10088;
  public const int CURLOPT_SSL_VERIFYHOST              = 81;
  public const int CURLOPT_SSL_VERIFYPEER              = 64;
  public const int CURLOPT_SSLVERSION                  = 32;
  public const int CURLOPT_STDERR                      = 10037;
  public const int CURLOPT_TCP_NODELAY                 = 121;
  public const int CURLOPT_TIMECONDITION               = 33;
  public const int CURLOPT_TIMEOUT                     = 13;
  public const int CURLOPT_TIMEOUT_MS                  = 155;
  public const int CURLOPT_TIMEVALUE                   = 34;
  public const int CURLOPT_TRANSFERTEXT                = 53;
  public const int CURLOPT_UNRESTRICTED_AUTH           = 105;
  public const int CURLOPT_UPLOAD                      = 46;
  public const int CURLOPT_URL                         = 10002;
  public const int CURLOPT_USERAGENT                   = 10018;
  public const int CURLOPT_USERPWD                     = 10005;
  public const int CURLOPT_VERBOSE                     = 41;
  public const int CURLOPT_WRITEFUNCTION               = 20011;
  public const int CURLOPT_WRITEHEADER                 = 10029;
  public const int CURLPROXY_HTTP                      = 0;
  public const int CURLPROXY_SOCKS5                    = 5;
  public const int CURL_TIMECOND_IFMODSINCE            = 1;
  public const int CURL_TIMECOND_IFUNMODSINCE          = 2;
  public const int CURL_TIMECOND_LASTMOD               = 3;
  public const int CURL_VERSION_IPV6                   = 1;
  public const int CURL_VERSION_KERBEROS4              = 2;
  public const int CURL_VERSION_LIBZ                   = 8;
  public const int CURLVERSION_NOW                     = 3;
  public const int CURL_VERSION_SSL                    = 4;

  public string []getLoadedExtensions()
  {
    return new String[] { "curl" };
  }

  /**
   * Closes this cURL object.
   *
   * @param env
   * @param curl
   */
  public static void curl_close(Env env,
                                @NotNull CurlResource curl)
  {
    if (curl == null)
      return;

    curl.close();
  }

  /**
   * Returns a copy of this resource.
   *
   * @param env
   * @param curl
   */
  @ReturnNullAsFalse
  public static CurlResource curl_copy_handle(Env env,
                                              @NotNull CurlResource curl)
  {
    if (curl == null)
      return null;

    return curl.clone();
  }

  /**
   * Returns the error code from the last operation.
   *
   * @param env
   * @param curl
   */
  public static Value curl_errno(Env env,
                                 @NotNull CurlResource curl)
  {
    if (curl == null)
      return BooleanValue.FALSE;

    return LongValue.create(curl.getErrorCode());
  }

  /**
   * Returns the error string from the last operation.
   *
   * @param env
   * @param curl
   */
  public static Value curl_error(Env env,
                                 @NotNull CurlResource curl)
  {
    if (curl == null)
      return BooleanValue.FALSE;

    return env.createString(curl.getError());
  }

  /**
   * @param env
   * @param curl
   */
  public static Value curl_exec(Env env,
                                @NotNull CurlResource curl)
  {
    if (curl == null) {
      return BooleanValue.FALSE;
    }

    return curl.execute(env, true);
  }

  /**
   * Returns information about the last request.
   *
   * @param env
   * @param curl
   * @param option type of information to return
   */
  public static Value curl_getinfo(Env env,
                                   @NotNull CurlResource curl,
                                   @Optional Value option)
  {
//    if (option instanceof DefaultValue)
//     return curl.getAllInfo();

    if (curl == null)
      return BooleanValue.FALSE;

    if (option.isDefault()) {
      ArrayValue array = new ArrayValueImpl();

      putInfo(env, curl, array, "url", CURLINFO_EFFECTIVE_URL);
      putInfo(env, curl, array, "http_code", CURLINFO_HTTP_CODE);
      putInfo(env, curl, array, "header_size", CURLINFO_HEADER_SIZE);
      putInfo(env, curl, array, "request_size", CURLINFO_REQUEST_SIZE);
      putInfo(env, curl, array, "filetime", CURLINFO_FILETIME);
      putInfo(env, curl, array,
              "ssl_verify_result", CURLINFO_SSL_VERIFYRESULT);
      putInfo(env, curl, array, "redirect_count", CURLINFO_REDIRECT_COUNT);
      putInfo(env, curl, array, "total_time", CURLINFO_TOTAL_TIME);
      putInfo(env, curl, array, "namelookup_time", CURLINFO_NAMELOOKUP_TIME);
      putInfo(env, curl, array, "connect_time", CURLINFO_CONNECT_TIME);
      putInfo(env, curl, array, "pretransfer_time", CURLINFO_PRETRANSFER_TIME);
      putInfo(env, curl, array, "size_upload", CURLINFO_SIZE_UPLOAD);
      putInfo(env, curl, array, "size_download", CURLINFO_SIZE_DOWNLOAD);
      putInfo(env, curl, array, "speed_download", CURLINFO_SPEED_DOWNLOAD);
      putInfo(env, curl, array, "speed_upload", CURLINFO_SPEED_UPLOAD);
      putInfo(env, curl, array,
              "download_content_length", CURLINFO_CONTENT_LENGTH_DOWNLOAD);
      putInfo(env, curl, array,
              "upload_content_length", CURLINFO_CONTENT_LENGTH_UPLOAD);
      putInfo(env, curl, array,
              "starttransfer_time", CURLINFO_STARTTRANSFER_TIME);
      putInfo(env, curl, array, "redirect_time", CURLINFO_REDIRECT_TIME);

      return array;
    }

    return getInfo(env, curl, option.toInt());
  }

  private static void putInfo(Env env,
                              CurlResource curl,
                              ArrayValue array,
                              string name,
                              int option)
  {
    array.put(env.createString(name), getInfo(env, curl, option));
  }

  private static Value getInfo(Env env,
                               CurlResource curl,
                               int option)
  {
    switch (option) {
      case CURLINFO_EFFECTIVE_URL:
        return env.createString(curl.getURL());
      case CURLINFO_HTTP_CODE:
        return LongValue.create(curl.getResponseCode());
      case CURLINFO_HEADER_SIZE:
        if (curl.getHeader() != null)
          return LongValue.create(curl.getHeader().length());
        else
          return LongValue.ZERO;
      case CURLINFO_REQUEST_SIZE:
        break;
      case CURLINFO_FILETIME:
        break;
      case CURLINFO_SSL_VERIFYRESULT:
        break;
      case CURLINFO_REDIRECT_COUNT:
        break;
      case CURLINFO_TOTAL_TIME:
        break;
      case CURLINFO_NAMELOOKUP_TIME:
        break;
      case CURLINFO_CONNECT_TIME:
        break;
      case CURLINFO_PRETRANSFER_TIME:
        break;
      case CURLINFO_SIZE_UPLOAD:
        break;
      case CURLINFO_SIZE_DOWNLOAD:
        break;
      case CURLINFO_SPEED_DOWNLOAD:
        break;
      case CURLINFO_SPEED_UPLOAD:
        break;
      case CURLINFO_CONTENT_LENGTH_DOWNLOAD:
        return LongValue.create(curl.getContentLength());
      case CURLINFO_CONTENT_LENGTH_UPLOAD:
        break;
      case CURLINFO_STARTTRANSFER_TIME:
        break;
      case CURLINFO_REDIRECT_TIME:
        break;

      case CURLINFO_HEADER_OUT:
        return curl.getHeader();
      case CURLINFO_CONTENT_TYPE:
        string type = curl.getContentType();

        if (type == null)
          return NullValue.NULL;

        return env.createString(type);
      default:
        env.warning(L.l("Unknown CURL getinfo option"));
    }

    return NullValue.NULL;
  }

  /**
   * Returns a cURL handle.
   *
   * @param env
   * @param url
   */
  public static CurlResource curl_init(Env env,
                                       @Optional string url)
  {
    CurlResource curl = new CurlResource();

    if (url != null) {
      setURL(curl, url);
    }

    return curl;
  }

  /**
   * Sets the url and extracts username/password from url.
   * Format: [protocol://]?[username:password@]?host
   */
  private static void setURL(CurlResource curl, string url)
  {
    int atSignIndex = url.indexOf('@');

    if (atSignIndex < 0) {
      curl.setURL(url);
      return;
    }

    int j = url.indexOf("://");
    if (j < 0) {
      curl.setURL("http://" + url);
      return;
    }

    int slashIndex = url.indexOf('/', j + 3);
    if (0 < slashIndex && slashIndex < atSignIndex) {
      curl.setURL(url);
      return;
    }

    j += 3;

    string protocol = url.substring(0, j);
    int colonIndex = url.indexOf(':', j);

    if (colonIndex < 0 || colonIndex > atSignIndex) {
      curl.setURL(url);
      return;
    }

    curl.setUsername(url.substring(j, colonIndex++));
    curl.setPassword(url.substring(colonIndex, atSignIndex++));
    curl.setURL(protocol + url.substring(atSignIndex));
  }

  /**
   *  int curl_multi_add_handle ( resource $mh , resource $ch )
   */
  public static int curl_multi_add_handle(Env env,
                                          CurlMultiResource multi,
                                          CurlResource curl)
  {
    multi.addCurl(curl);

    return 0;
  }

  /**
   * void curl_multi_close ( resource $mh )
   */
  public static void curl_multi_close(Env env,
                                      CurlMultiResource multi)
  {
    for (CurlResource curl : multi.getCurlList()) {
      curl_close(env, curl);
    }
  }

  /**
   * int curl_multi_exec ( resource $mh , int &$still_running )
   */
  public static int curl_multi_exec(Env env,
                                    CurlMultiResource multi,
                                    @Reference Value stillRunning)
  {
    return multi.execute(env, stillRunning);
  }

  /**
   * string curl_multi_getcontent ( resource $ch )
   */
  public static Value curl_multi_getcontent(Env env,
                                            CurlResource curl)
  {
    StringValue data = curl.getReturnTransfer();

    if (data != null) {
      return data;
    }
    else {
      return NullValue.NULL;
    }
  }

  /**
   * XXX: not documented by PHP
   *
   * @param env
   * @param curls
   */
  public static Value curl_multi_info_read(Env env,
                                           CurlMultiResource multi,
                                           @Optional @Reference Value msgsInQueue)
  {
    return multi.readInfo(env, msgsInQueue);
  }

  /**
   * resource curl_multi_init ( void )
   */
  public static CurlMultiResource curl_multi_init(Env env)
  {
    return new CurlMultiResource();
  }

  /**
   * int curl_multi_remove_handle ( resource $mh , resource $ch )
   */
  public static int curl_multi_remove_handle(Env env,
                                             CurlMultiResource multi,
                                             CurlResource curl)
  {
    multi.removeCurl(curl);

    return 0;
  }

  /**
   * int curl_multi_select ( resource $mh [, float $timeout = 1.0 ] )
   */
  public static int curl_multi_select(Env env,
                                      CurlMultiResource multi,
                                      @Optional Value timeout)
  {
    return 0;
  }

  /**
   * Sets an array of options.
   *
   * @param env
   * @param curl
   * @param options
   */
  public static BooleanValue curl_setopt_array(Env env,
                                               @NotNull CurlResource curl,
                                               ArrayValue options)
  {
    if (curl == null)
      return BooleanValue.FALSE;

    for (Map.Entry<Value,Value> entry : options.entrySet()) {
      if (! setOption(env, curl, entry.getKey().toInt(), entry.getValue()))
        return BooleanValue.FALSE;
    }

    return BooleanValue.TRUE;
  }

  /**
   * Sets a cURL option.
   *
   * @param env
   * @param curl
   * @param option
   * @param value
   *
   * @return true if successful
   */
  public static BooleanValue curl_setopt(Env env,
                                         @NotNull CurlResource curl,
                                         int option,
                                         Value value)
  {
    if (curl == null)
      return BooleanValue.FALSE;

    if (setOption(env, curl, option, value))
      return BooleanValue.TRUE;
    else
      return BooleanValue.FALSE;
  }

  private static bool setOption(Env env,
                                   CurlResource curl,
                                   int option,
                                   Value value)
  {
    int i;

    switch (option) {
      //
      // booleans
      //
      case CURLOPT_AUTOREFERER:
        //XXX
        break;
      case CURLOPT_COOKIESESSION:
        curl.setCookie(null);
        break;
      case CURLOPT_FAILONERROR:
        curl.setFailOnError(value.toBoolean());
        break;
      case CURLOPT_FOLLOWLOCATION:
        curl.setIsFollowingRedirects(value.toBoolean());
        break;
      case CURLOPT_FRESH_CONNECT:
        // caching handled by Java
        break;
      case CURLOPT_HEADER:
        curl.setIsReturningHeader(value.toBoolean());
        break;
      case CURLOPT_HTTPGET:
        curl.setRequestMethod("GET");
        break;
      case CURLOPT_HTTPPROXYTUNNEL:
        curl.setIsProxying(value.toBoolean());
        break;
      case CURLOPT_MUTE:
        curl.setIsVerbose(! value.toBoolean());
        break;
      case CURLOPT_NETRC:
        //username:password file
        //XXX
        break;
      case CURLOPT_NOBODY:
        curl.setIsReturningBody(false);
        break;
      case CURLOPT_NOPROGRESS:
        //XXX
        break;
      case CURLOPT_POST:
        curl.setRequestMethod("POST");
        break;
      case CURLOPT_PUT:
        curl.setRequestMethod("PUT");
        break;
      case CURLOPT_RETURNTRANSFER:
        curl.setIsReturningData(value.toBoolean());
        break;
      case CURLOPT_UNRESTRICTED_AUTH:
        //XXX
        break;
      case CURLOPT_SSL_VERIFYPEER:
        curl.setIsVerifySSLPeer(value.toBoolean());
        break;
      case CURLOPT_UPLOAD:
        if (value.toBoolean()) {
          // CURLOPT_UPLOAD implies PUT but not necessarily
          //curl.setRequestMethod("PUT");
        }
        break;
      case CURLOPT_VERBOSE:
        curl.setIsVerbose(value.toBoolean());
        break;

      //
      // ints
      //
      case CURLOPT_BUFFERSIZE:
        //XXX
        break;
      case CURLOPT_CONNECTTIMEOUT:
        curl.setConnectTimeout(value.toInt() * 1000);
        break;
      case CURLOPT_HTTP_VERSION:
        if (value.toInt() == CURL_HTTP_VERSION_1_0) {
          env.stub("cURL HTTP/1.0 not specifically supported");
        }
        break;
      case CURLOPT_HTTPAUTH:
        // get authentication method from server instead
/*
        int method = value.toInt();

        if ((method & CURLAUTH_BASIC) == CURLAUTH_BASIC)
          curl.setAuthenticationMethod(CURLAUTH_BASIC);
        else if ((method & CURLAUTH_DIGEST) == CURLAUTH_DIGEST)
          curl.setAuthenticationMethod(CURLAUTH_DIGEST);
        else
          env.stub("cURL Http authentication method not supported");
*/
        break;
      case CURLOPT_INFILESIZE:
        curl.setUploadFileSize(value.toLong());
        break;
      case CURLOPT_LOW_SPEED_LIMIT:
        //XXX
        break;
      case CURLOPT_LOW_SPEED_TIME:
        //XXX
        break;
      case CURLOPT_MAXCONNECTS:
        //XXX
        break;
      case CURLOPT_MAXREDIRS:
        //XXX
        break;
      case CURLOPT_PORT:
        curl.setPort(value.toInt());
        break;
      case CURLOPT_PROXYAUTH:
        //XXX
        break;
      case CURLOPT_PROXYPORT:
        curl.setProxyPort(value.toInt());
        break;
      case CURLOPT_PROXYTYPE:
        switch (value.toInt()) {
          case CURLPROXY_HTTP:
            curl.setProxyType("HTTP");
            break;
          case CURLPROXY_SOCKS5:
            curl.setProxyType("SOCKS");
            break;
          default:
            env.warning(L.l("unknown curl proxy type"));
        }
        break;
      case CURLOPT_SSL_VERIFYHOST:
        i = value.toInt();
        switch (i) {
          case 0:
            curl.setIsVerifySSLCommonName(false);
            curl.setIsVerifySSLHostname(false);
            break;
          case 1:
            curl.setIsVerifySSLCommonName(true);
            curl.setIsVerifySSLHostname(false);
            break;
          case 2:
            curl.setIsVerifySSLCommonName(true);
            curl.setIsVerifySSLHostname(true);
            break;
          default:
            env.warning(L.l("unknown ssl verify host option '{0}", i));
        }
        break;
      case CURLOPT_TIMECONDITION:
        switch (value.toInt()) {
          case CURL_TIMECOND_IFMODSINCE:
            curl.setIfModifiedSince(true);
            break;
          case CURL_TIMECOND_IFUNMODSINCE:
            curl.setIfModifiedSince(false);
            break;
          default:
            env.warning(L.l("invalid CURLOPT_TIMECONDITION option"));
        }
        break;
      case CURLOPT_TIMEOUT:
        curl.setReadTimeout(value.toInt() * 1000);
        break;
      case CURLOPT_TIMEVALUE:
        long time = value.toLong() * 1000L;
        string format = "%a, %d %b %Y %H:%M:%S %Z";

        string date = QDate.formatGMT(time, format);

        curl.setModifiedTime(date);
        break;

      //
      // strings
      //
      case CURLOPT_CAINFO:
        env.notice(L.l("custom certificates not supported"));
        break;

      case CURLOPT_COOKIE:
        curl.setCookie(value.toString());
        break;
      case CURLOPT_COOKIEFILE:
        // XXX: Netscape cookie format support
        ReadStream in = null;

        try {
          Path path = env.getPwd().lookup(value.toString());

          if (path.exists()) {
            in = path.openRead();

            StringBuilder sb = new StringBuilder();

            int ch;
            while ((ch = in.read()) >= 0) {
              sb.append((char)ch);
            }

            curl.setCookie(sb.toString());
          }
        }
        catch (IOException e) {
          throw new QuercusModuleException(e);
        }
        finally {
          if (in != null)
            in.close();
        }
        break;
      case CURLOPT_COOKIEJAR:
        //XXX: Netscape cookie file format
        curl.setCookieFilename(value.toString());
        break;
      case CURLOPT_CUSTOMREQUEST:
        curl.setRequestMethod(value.toString());
        break;
      case CURLOPT_ENCODING:
        string encoding = value.toString();
        if (encoding.length() == 0)
          encoding = "gzip, deflate, identity";
        curl.setRequestProperty("Accept-Encoding", encoding);
        break;
      case CURLOPT_POSTFIELDS:
        curl.setRequestMethod("POST");
        curl.setPostBody(value);
        break;
      case CURLOPT_PROXY:
        curl.setIsProxying(true);
        curl.setProxyURL(value.toString());
        break;
      case CURLOPT_PROXYUSERPWD:
        string proxyUserPwd = value.toString();
        i = proxyUserPwd.indexOf(':');

        if (i >= 0)
          curl.setProxyUsername(proxyUserPwd.substring(0, i));

        curl.setProxyPassword(proxyUserPwd.substring(i + 1));
        break;
      case CURLOPT_RANGE:
        curl.setRequestProperty("Range", "bytes=" + value.toString());
        break;
      case CURLOPT_REFERER:
        curl.setRequestProperty("Referer", value.toString());
        break;
      case CURLOPT_URL:
        setURL(curl, value.toString());
        //curl.setURL(value.toString());
        break;
      case CURLOPT_USERAGENT:
        curl.setRequestProperty("User-Agent", value.toString());
        break;
      case CURLOPT_USERPWD:
        string userpwd = value.toString();
        i = userpwd.indexOf(':');

        if (i >= 0)
          curl.setUsername(userpwd.substring(0, i));

        curl.setPassword(userpwd.substring(i + 1));
        break;

      //
      // arrays
      //
      case CURLOPT_HTTP200ALIASES:
        //XXX: nonstandard HTTP replies like "FOO HTTP/1.1 OK"
        break;
      case CURLOPT_HTTPHEADER:
        ArrayValue array = value.toArrayValue(env);

        for (Map.Entry<Value,Value> entry : array.entrySet()) {
          string header = entry.getValue().toString();

          string name = header;
          string body = "";

          i = header.indexOf(':');

          if (i >= 0) {
            name = header.substring(0, i).trim();
            body = header.substring(i + 1).trim();
          }

          curl.setRequestProperty(name, body);
        }
        break;

      //
      // fopen stream resources
      //
      case CURLOPT_FILE:
        Object outputFile = value.toJavaObject();

        if (outputFile instanceof BinaryOutput)
          curl.setOutputFile((BinaryOutput) outputFile);
        break;
      case CURLOPT_INFILE:
        Object uploadFile = value.toJavaObject();

        if (uploadFile instanceof BinaryInput)
          curl.setUploadFile((BinaryInput) uploadFile);
        break;
      case CURLOPT_STDERR:
        //XXX
        break;
      case CURLOPT_WRITEHEADER:
        Object outputHeaderFile = value.toJavaObject();

        if (outputHeaderFile instanceof BinaryOutput)
          curl.setOutputHeaderFile((BinaryOutput)outputHeaderFile);
        break;

      //
      // callback functions
      //
      case CURLOPT_HEADERFUNCTION:
        curl.setHeaderCallback(value.toCallable(env, false));
        break;
      case CURLOPT_PASSWDFUNCTION:
        curl.setPasswordCallback(value.toCallable(env, false));
        break;
      case CURLOPT_READFUNCTION:
        curl.setReadCallback(value.toCallable(env, false));
        break;
      case CURLOPT_WRITEFUNCTION:
        curl.setWriteCallback(value.toCallable(env, false));
        break;

      default:
        env.warning(L.l("CURL option '{0}' unknown or unimplemented",
                        option));

        log.fine(L.l("CURL option '{0}' unknown or unimplemented",
                     option));
        return false;
    }

    return true;
  }

  /**
   * Returns the version of this cURL implementation.
   *
   * @param env
   * @param version
   */
  public static ArrayValue curl_version(Env env,
                                        @Optional Value age)
  {
    ArrayValue array = new ArrayValueImpl();

    // dummy values
    array.put(env, "version_number", 462848);
    array.put(env, "age", 2);
    array.put(env, "features", 540);
    array.put(env, "ssl_version_number", 0);
    array.put(env, "version", "7.16.0");
    array.put(env, "host", "i386-pc-java");
    array.put(env, "ssl_version", " OpenSSL/0.9.8g");
    array.put(env, "libz_version", "1.2.3");

    // supported protocols
    ArrayValue protocols = new ArrayValueImpl();
    protocols.put(env.createString("http"));
    protocols.put(env.createString("https"));

    array.put(env.createString("protocols"), protocols);

    return array;
  }

}
}
