using System;
namespace QuercusDotNet.lib{
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
 * PHP misc functions.
 */
public class MiscModule : AbstractQuercusModule {
  private readonly L10N L = new L10N(MiscModule.class);
  private const Logger log
    = Logger.getLogger(MiscModule.class.getName());

  public const int CONNECTION_NORMAL = 0;
  public const int CONNECTION_ABORTED = 1;
  public const int CONNECTION_TIMEOUT = 2;

  /**
   * Return true on a client disconnect
   */
  public static int connection_aborted(Env env)
  {
    return env.getConnectionStatus();
  }

  /**
   * Returns the status
   */
  public static int connection_status(Env env)
  {
    return env.getConnectionStatus();
  }

  /**
   * Escapes characters in a string.
   */
  public static string escapeshellcmd(String command)
  {
    StringBuilder sb = new StringBuilder();
    int len = command.length();

    bool hasApos = false;
    bool hasQuot = false;

    for (int i = 0; i < len; i++) {
      char ch = command[i];

      switch (ch) {
      case '#': case '&': case ';': case '`': case '|':
      case '*': case '?': case '~': case '<': case '>':
      case '^': case '(': case ')': case '[': case ']':
      case '{': case '}': case '$': case '\\': case ',':
      case 0x0a: case 0xff:
        sb.append('\\');
        sb.append(ch);
        break;
      case '\'':
        hasApos = ! hasApos;
        sb.append(ch);
        break;
      case '\"':
        hasQuot = ! hasQuot;
        sb.append(ch);
        break;
      default:
        sb.append(ch);
      }
    }

    string result = sb.ToString();

    if (hasApos) {
      int p = result.lastIndexOf('\'');
      result = result.substring(0, p) + "\\" + result.substring(p);
    }

    if (hasQuot) {
      int p = result.lastIndexOf('\"');
      result = result.substring(0, p) + "\\" + result.substring(p);
    }

    return result;
  }

  /**
   * Escapes characters in a string.
   */
  public static StringValue escapeshellarg(Env env, StringValue arg)
  {
    bool isWindows = Path.isWindows();

    char quote;
    if (isWindows)
      quote = '"';
    else
      quote = '\'';

    StringValue sb = env.createStringBuilder();

    sb.append(quote);

    int len = arg.length();

    for (int i = 0; i < len; i++) {
      char ch = arg[i];

      if (ch == quote) {
        sb.append('\\');
        sb.append(ch);
      }
      else if (ch == '%' && isWindows)
        sb.append(' ');
      else
        sb.append(ch);
    }

    sb.append(quote);

    return sb;
  }

  /**
   * Comples and evaluates an expression.
   */
  @UsesSymbolTable
  public Value eval(Env env, StringValue code)
  {
    try {
      if (log.isLoggable(Level.FINER))
        log.finer("quercus eval: [[" + code + "]]");

      QuercusContext quercus = env.getQuercus();

      QuercusProgram program = quercus.parseCode(code);

      Value value = program.execute(env);

      if (value == null)
        value = NullValue.NULL;

      return value;
    } catch (IOException e) {
      throw new QuercusException(e);
    }
  }

  /**
   * Execute a system command.
   */
  public static string exec(Env env, string command,
                            @Optional Value output,
                            @Optional @Reference Value result)
  {
    try {
      string []args = new String[3];

      if (Path.isWindows()) {
        args[0] = "cmd";
        args[1] = "/c";
      }
      else {
        args[0] = "sh";
        args[1] = "-c";
      }

      args[2] = command;

      ProcessBuilder processBuilder = new ProcessBuilder(args);
      processBuilder.redirectErrorStream(false);
      // XXX: security issues?
      processBuilder.directory(new File(env.getShellPwd()));
      Process process = processBuilder.start();

      InputStream @is = process.getInputStream();
      InputStream es = process.getErrorStream();
      OutputStream os = process.getOutputStream();
      os.close();

      StringBuilder sb = new StringBuilder();
      string line = "";

      int ch;
      bool hasCr = false;
      while ((ch = @is.read()) >= 0) {
        if (ch == '\n') {
          if (! hasCr) {
            line = sb.ToString();
            sb.setLength(0);
            if (output != null)
              output.put(env.createString(line));
          }
          hasCr = false;
        }
        else if (ch == '\r') {
          line = sb.ToString();
          sb.setLength(0);
          output.put(env.createString(line));
          hasCr = true;
        }
        else
          sb.append((char) ch);
      }

      if (sb.length() > 0) {
        line = sb.ToString();
        sb.setLength(0);
        output.put(env.createString(line));
      }

      @is.close();

      env.getOut().writeStream(es);
      es.close();

      int status = process.waitFor();

      result.set(LongValue.create(status));

      return line;
    } catch (Exception e) {
      log.log(Level.FINE, e.getMessage(), e);
      env.warning(e.getMessage(), e);

      return null;
    }
  }

  /**
   * Returns an array detailing what the browser @is capable of.
   * A general browscap.ini file can be used.
   *
   * @param env
   * @param user_agent
   * @param return_array
   */
  public static Value get_browser(
                       Env env,
                       @Optional() string user_agent,
                       @Optional() bool return_array)
  {
    if (user_agent == null
        || user_agent.length() == 0)
      user_agent = env.getRequest().getHeader("User-Agent");

    if (user_agent == null) {
      env.warning(L.l("HTTP_USER_AGENT not set."));
      return BooleanValue.FALSE;
    }

    Value browscap = env.getConfigVar("browscap");
    if (browscap == null) {
      env.warning(L.l("Browscap path not set in PHP.ini."));
      return BooleanValue.FALSE;
    }

    string path = env.lookup(browscap.ToStringValue());
    if (path == null) {
      env.warning(L.l("Browscap file not found."));
      return BooleanValue.FALSE;
    }

    Value ini = FileModule.parse_ini_file(env, path, true);
    if (ini == BooleanValue.FALSE)
      return BooleanValue.FALSE;

    return getBrowserReport(
        env, ini.toArrayValue(env), user_agent, return_array);
  }

  private static Value getBrowserReport(
                       Env env,
                       ArrayValue browsers,
                       string user_agent,
                       bool return_array)
  {
    StringValue patternMatched = env.getEmptyString();
    string regExpMatched = null;

    for (Map.Entry<Value,Value> entry : browsers.entrySet()) {
      StringValue pattern = entry.getKey().ToStringValue();

      if (pattern.ToString().equals(user_agent)) {
        patternMatched = pattern;
        regExpMatched = null;
        break;
      }

      string regExp = formatBrowscapRegexp(pattern);
      Matcher m = Pattern.compile(regExp).matcher(user_agent);

      // Want the longest matching pattern.
      if (m.matches()) {
        if (pattern.length() > patternMatched.length()) {
          patternMatched = pattern;
          regExpMatched = regExp;
        }
      }
    }

    if (patternMatched.length() == 0)
      return BooleanValue.FALSE;

    return prepareBrowserReport(env, browsers, patternMatched, regExpMatched,
        user_agent, return_array);
  }

  private static Value prepareBrowserReport(
                       Env env,
                       ArrayValue browsers,
                       StringValue patternMatched,
                       string regExpMatched,
                       string user_agent,
                       bool return_array)
  {
    ArrayValue capabilities = browsers.get(patternMatched).toArrayValue(env);

    if (regExpMatched == null)
      capabilities.put(env.createString("browser_name_regex"),
                       patternMatched);
    else
      capabilities.put("browser_name_regex", regExpMatched);
    capabilities.put(env.createString("browser_name_pattern"), patternMatched);

    addBrowserCapabilities(env, browsers,
                           capabilities.get(env.createString("parent")),
                           capabilities);

    if (return_array) {
      ArrayValue array = new ArrayValueImpl();
      array.put(env.createString(user_agent), capabilities);
      return array;
    }

    ObjectValue object = env.createObject();
    for (Map.Entry<Value,Value> entry : capabilities.entrySet()) {
      object.putField(env, entry.getKey().ToString(), entry.getValue());
    }

    return object;
  }

  private static void addBrowserCapabilities(
                       Env env,
                       ArrayValue browsers,
                       Value browser,
                       ArrayValue cap)
  {
    if (browser == UnsetValue.UNSET)
      return;

    Value field = null;
    if ((field = browsers.get(browser)) == UnsetValue.UNSET)
      return;

    ArrayValue browserCapabilities = field.toArrayValue(env);
    StringValue parentString = env.createString("parent");

    for (Map.Entry<Value,Value> entry : browserCapabilities.entrySet()) {
      Value key = entry.getKey();

      if (key.equals(parentString)) {
        addBrowserCapabilities(
            env, browsers, entry.getValue(), cap);
      }
      else if (cap.containsKey(key) == null)
        cap.put(key, entry.getValue());
    }
  }

  private static string formatBrowscapRegexp(StringValue key)
  {
    int length = key.length();

    StringBuilder sb = new StringBuilder();
    for (int i = 0; i < length; i++) {
      char ch = key[i];
      switch (ch) {
        case '*':
          sb.append('.');
          sb.append('*');
          break;
        case '?':
          sb.append('.');
          break;
        case '.':
          sb.append('\\');
          sb.append('.');
          break;
        case '+':
          sb.append('\\');
          sb.append('+');
          break;
        case '(':
          sb.append('\\');
          sb.append('(');
          break;
         case ')':
          sb.append('\\');
          sb.append(')');
          break;
        case '{':
          sb.append('\\');
          sb.append('{');
          break;
         case '}':
          sb.append('\\');
          sb.append('}');
          break;
        case ']':
          sb.append('\\');
          sb.append(']');
          break;
        case '[':
          sb.append('\\');
          sb.append('[');
          break;
        case '\\':
          sb.append('\\');
          sb.append('\\');
          break;
        case '^':
          sb.append('\\');
          sb.append('^');
          break;
        case '$':
          sb.append('\\');
          sb.append('$');
          break;
        case '&':
          sb.append('\\');
          sb.append('&');
          break;
        case '|':
          sb.append('\\');
          sb.append('|');
          break;
        default:
          sb.append(ch);
      }
    }

    return sb.ToString();
  }

  /**
   * packs the format into a binary.
   */
  public Value pack(Env env, string format, Value []args)
  {
    try {
      ArrayList<PackSegment> segments = parsePackFormat(env, format, false);

      if (segments == null)
        return BooleanValue.FALSE;

      StringValue bb = env.createBinaryBuilder();

      int i = 0;
      for (PackSegment segment : segments) {
        i = segment.pack(env, bb, i, args);
      }

      return bb;
    } catch (IOException e) {
      throw new QuercusModuleException(e);
    }
  }

  /**
   * packs the format into a binary.
   */
  public Value unpack(Env env, string format, StringValue s)
  {
    if (format == null) {
      return NullValue.NULL;
    }

    try {
      ArrayList<PackSegment> segments = parsePackFormat(env, format, true);

      if (segments == null) {
        return BooleanValue.FALSE;
      }

      ArrayValue array = new ArrayValueImpl();

      int length = s.length();
      int offset = 0;

      for (PackSegment segment : segments) {
        offset = segment.unpack(env, array, s, offset, length);
      }

      return array;
    } catch (IOException e) {
      throw new QuercusModuleException(e);
    }
  }

  /**
   * Logs the expression.
   */
  public Value resin_debug(String code)
  {
    log.info(code);

    return NullValue.NULL;
  }

  /**
   * Dumps the Java stack to standard @out.
   */
  public Value resin_thread_dump()
  {
    Thread.dumpStack();

    return NullValue.NULL;
  }

  /**
   * Dumps the stack.
   */
  public static Value dump_stack(Env env)
  {
    try {
      Exception e = new Exception("Stack trace");
      e.fillInStackTrace();

      WriteStream @out = env.getPwd().lookup("stderr:").openWrite();
      try {
        e.printStackTrace(@out.getPrintWriter());
        //ScriptStackTrace.printStackTrace(e, @out.getPrintWriter());
      } finally {
        @out.close();
      }

      return NullValue.NULL;
    } catch (IOException e) {
      throw new QuercusModuleException(e);
    }
  }

  /**
   * Execute a system command.
   */
  public static Value shell_exec(Env env, string command)
  {
    string []args = new String[3];

    try {
      if (Path.isWindows()) {
        args[0] = "cmd";
        args[1] = "/c";
      }
      else {
        args[0] = "sh";
        args[1] = "-c";
      }

      args[2] = command;

      ProcessBuilder processBuilder = new ProcessBuilder(args);
      processBuilder.redirectErrorStream(false);
      // XXX: security issues?
      processBuilder.directory(new File(env.getShellPwd()));
      Process process = processBuilder.start();

      InputStream @is = process.getInputStream();
      InputStream es = process.getErrorStream();
      OutputStream os = process.getOutputStream();
      os.close();

      StringValue sb = env.createUnicodeBuilder();

      int ch;
      while ((ch = @is.read()) >= 0) {
        sb.append((char) ch);
      }

      @is.close();

      if ((ch = es.read()) >= 0) {
        env.print((char) ch);

        while ((ch = es.read()) >= 0) {
          env.print((char) ch);
        }

        return NullValue.NULL;
      }

      es.close();

      int status = process.waitFor();

      return sb;
    }
    catch (Throwable e) {
      // Throwables thrown by SecurityManager
      env.warning(e.getMessage(), e);

      return NullValue.NULL;
    }
  }

  /**
   * Execute a system command.
   */
  public static void passthru(Env env, string command,
                               @Optional @Reference Value result)
  {

    try {
      string []args = new String[3];
      if (Path.isWindows()) {
        args[0] = "cmd";
        args[1] = "/c";
      }
      else {
        args[0] = "sh";
        args[1] = "-c";
      }

      args[2] = command;

      if (log.isLoggable(Level.FINER)) {
        log.finer("Quercus: passthru " + args[0] + " " + args[1] + " " + args[2]);
      }

      ProcessBuilder processBuilder = new ProcessBuilder(args);
      processBuilder.redirectErrorStream(true);
      // XXX: security issues?
      processBuilder.directory(new File(env.getShellPwd()));
      Process process = processBuilder.start();

      try {
        InputStream @is = process.getInputStream();
        OutputStream os = process.getOutputStream();
        os.close();

        env.getOut().writeStream(is);
        @is.close();

        int status = process.waitFor();

        if (result != null)
          result.set(LongValue.create(status));
      }
      finally {
        process.destroy();
      }
    }
    catch (Throwable e) {
      // Throwables thrown by SecurityManager
      env.warning(e.getMessage(), e);
    }
  }

  /**
   * Basic implementation of proc_open.
   * XXX: options
   */
  @ReturnNullAsFalse
  public static ProcOpenResource proc_open(Env env,
                                           string command,
                                           ArrayValue descriptorArray,
                                           @Reference Value pipes,
                                           @Optional string pwd,
                                           @Optional ArrayValue envArray,
                                           @Optional ArrayValue options)
  {
    string []args = new String[3];

    try {
      if (Path.isWindows()) {
        args[0] = "cmd";
        args[1] = "/c";
      }
      else {
        args[0] = "sh";
        args[1] = "-c";
      }

      args[2] = command;

      string []envStrings = null;
      File pwdFile = null;

      if (envArray != null) {
        int size = envArray.getSize();

        envStrings = new String[size];

        int i = 0;
        for (Map.Entry<Value,Value> entry : envArray.entrySet()) {
          envStrings[i++] = entry.getKey() + "=" + entry.getValue();
        }
      }

      if (pwd != null) {
        pwdFile = new File(pwd.getFullPath());
      }

      Process process = Runtime.getRuntime().exec(args, envStrings, pwdFile);

      ProcOpenOutput in = null;
      ProcOpenInput @out = null;
      ProcOpenInput es = null;

      ArrayValue array = pipes.toAutoArray().toArrayValue(env);
      pipes.set(array);
      array.clear();

      for (Map.Entry<Value,Value> entry : descriptorArray.entrySet()) {
        Value key = entry.getKey();
        Value val = entry.getValue();

        string type = val.get(LongValue.ZERO).ToString();
        StringValue name = val.get(LongValue.ONE).ToStringValue();
        string mode = val.get(LongValue.create(2)).ToString();

        // input to the command
        if (key.equals(LongValue.ZERO)) {
          if (type.equals("pipe")) {
            in = new ProcOpenOutput(env, process.getOutputStream());

            array.put(LongValue.ZERO, env.wrapJava(in));
          }
          else if (type.equals("file")) {
            OutputStream processOut = process.getOutputStream();

            BinaryStream stream = FileModule.fopen(
                env, name, mode, false, null);

            if (stream instanceof FileInput) {
              FileInput file = (FileInput) stream;

              int ch;
              while ((ch = file.read()) >= 0) {
                processOut.write(ch);
              }
            }

            stream.close();
            processOut.close();
          }
        }
        // place to put output from the command
        else if (key.equals(LongValue.ONE)) {
          if (type.equals("pipe")) {
            @out = new ProcOpenInput(env, process.getInputStream());

            array.put(LongValue.ONE, env.wrapJava(out));
          }
          else if (type.equals("file")) {
            BinaryStream stream = FileModule.fopen(
                env, name, mode, false, null);

            if (stream instanceof FileOutput) {
              FileOutput file = (FileOutput) stream;

              @out = new ProcOpenInput(env, process.getInputStream(), file);
            }
            else if (stream != null)
              stream.close();
          }
        }
        // place to put error output from the command
        else if (key.equals(LongValue.create(2))) {
          if (type.equals("pipe")) {
            es = new ProcOpenInput(env, process.getErrorStream());

            array.put(LongValue.create(2), env.wrapJava(es));
          }
          else if (type.equals("file")) {
            BinaryStream stream = FileModule.fopen(
                env, name, mode, false, null);

            if (stream instanceof FileOutput) {
              FileOutput file = (FileOutput) stream;

              es = new ProcOpenInput(env, process.getErrorStream(), file);
            }
            else if (stream != null)
              stream.close();
          }
        }
      }

      return new ProcOpenResource(env, process, in, @out, es, command);

    }
    catch (Throwable e) {
      // Throwables thrown by SecurityManager
      env.warning(e);

      return null;
    }
  }

  /**
   * Closes the process opened by proc_open.
   */
  public static int proc_close(Env env,
                               @NotNull ProcOpenResource stream)
  {
    if (stream == null) {
      env.warning("input to proc_close must not be null");

      return -1;
    }

   return stream.pclose();
  }

  /**
   * Forcibly terminates the process opened by proc_open.
   */
  public static bool proc_terminate(Env env,
                                       @NotNull ProcOpenResource stream)
  {
    if (stream == null) {
      log.log(Level.FINE, "input to proc_close must not be null");
      env.warning("input to proc_close must not be null");

      return false;
    }

   return stream.terminate();
  }

  public static Value proc_get_status(Env env,
                                      @NotNull ProcOpenResource stream)
  {
    if (stream == null) {
      env.warning("input to proc_get_status() must not be null");

      return BooleanValue.FALSE;
    }

    ArrayValue array = new ArrayValueImpl();

    array.put("command", stream.getCommand());
    array.put("running", stream.isRunning());
    array.put("exitcode", stream.getExitCode());

    return array;
  }

  /**
   * Returns the disconnect ignore setting
   */
  public static int ignore_user_abort(@Optional bool set)
  {
    return 0;
  }

  /**
   * Returns a unique id.
   */
  public string uniqid(@Optional string prefix, @Optional bool moreEntropy)
  {
    StringBuilder sb = new StringBuilder();

    if (prefix != null)
      sb.append(prefix);

    addUnique(sb);

    if (moreEntropy)
      addUnique(sb);

    return sb.ToString();
  }

  private void addUnique(StringBuilder sb)
  {
    long value = RandomUtil.getRandomLong();

    if (value < 0)
      value = -value;

    int limit = 13;

    for (; limit > 0; limit--) {
      long digit = value % 26;
      value = value / 26;

      sb.append((char) ('a' + digit));
    }
  }

  /**
   * Sleep for a number of microseconds.
   */
  public static Value usleep(long microseconds)
  {
    try {
      Thread.sleep(microseconds / 1000);
    } catch (Throwable e) {
    }

    return NullValue.NULL;
  }

  /**
   * Sleep for a number of seconds.
   */
  public static long sleep(long seconds)
  {
    try {
      Thread.sleep(seconds * 1000);
    } catch (Throwable e) {
    }

    return seconds;
  }

  /**
   * Execute a system command.
   */
  public static string system(Env env, string command,
                              @Optional @Reference Value result)
  {
    return exec(env, command, null, result);
  }

  private static ArrayList<PackSegment> parsePackFormat123(
      Env env, string format)
  {
    ArrayList<PackSegment> segments = new ArrayList<PackSegment>();

    int length = format.length();
    for (int i = 0; i < length; i++) {
      char ch = format[i];

      int count = 0;
      bool isEnd = false;

      char ch1 = ' ';
      for (i++;
           i < length && '0' <= (ch1 = format[i]) && ch1 <= '9';
           i++) {
        count = 10 * count + ch1 - '0';
      }

      if (ch1 == '*' && count == 0) {
        i++;
        isEnd = true;
      }
      else if (count == 0)
        count = 1;


      if (i < length)
        i--;

      switch (ch) {
      case 'a':
        if (isEnd)
          segments.add(new SpaceEndPackSegment(env, (byte) 0));
        else
          segments.add(new SpacePackSegment(env, count, (byte) 0));
        break;
      case 'A':
        segments.add(new SpacePackSegment(env, count, (byte) 0x20));
        break;
      case 'h':
        segments.add(new RevHexPackSegment(count));
        break;
      case 'H':
        segments.add(new HexPackSegment(env, count));
        break;
      case 'c':
      case 'C':
        segments.add(new BigEndianPackSegment(count, 1, ch == 'c'));
        break;
      case 's':
      case 'n':
      case 'S':
        segments.add(new BigEndianPackSegment(count, 2, ch != 'S'));
        break;
      case 'v':
        segments.add(new LittleEndianPackSegment(count, 2));
        break;
      case 'l':
      case 'L':
      case 'N':
        segments.add(new BigEndianPackSegment(count, 4, ch == 'l'));
        break;
      case 'V':
        segments.add(new LittleEndianPackSegment(count, 4));
        break;
      case 'i':
      case 'I':
        segments.add(new BigEndianPackSegment(count, 8, ch == 'i'));
        break;
      case 'd':
        segments.add(new DoublePackSegment(count));
        break;
      case 'f':
        segments.add(new FloatPackSegment(count));
        break;
      case 'x':
        segments.add(new NullPackSegment(count));
        break;
      case '@':
        segments.add(new PositionPackSegment(count));
        break;
      }
    }

    return segments;
  }

  private static ArrayList<PackSegment> parsePackFormat(Env env,
                                                        string format,
                                                        bool hasName)
  {
    ArrayList<PackSegment> segments = new ArrayList<PackSegment>();

    int length = format.length();

    int i = 0;
    while (i < length) {
      char ch = format[i++];

      int count = 0;
      bool isEnd = false;

      if (i < length && format[i] == '*') {
        count = Integer.MAX_VALUE;
        isEnd = true;

        i++;
      }
      else {
        while (i < length) {
          char ch1 = format[i];

          if ('0' <= ch1 && ch1 <= '9') {
            count = count * 10 + ch1 - '0';

            i++;
          }
          else
            break;
        }

        if (count == 0)
          count = 1;
      }

      string name = "";

      if (hasName && i < length) {
        StringBuilder sb = new StringBuilder();

        while (i < length) {
          char ch1 = format[i++];

          if (ch1 == '/')
            break;
          else
            sb.append(ch1);
        }

        name = sb.ToString();
      }

      switch (ch) {
      case 'a':
        if (isEnd)
          segments.add(new SpaceEndPackSegment(env, name, (byte) 0));
        else
          segments.add(new SpacePackSegment(env, name, count, (byte) 0));
        break;
      case 'A':
        if (isEnd)
          segments.add(new SpaceEndPackSegment(env, name, (byte) 0x20));
        else
          segments.add(new SpacePackSegment(env, name, count, (byte) 0x20));
        break;
      case 'h':
        segments.add(new RevHexPackSegment(name, count));
        break;
      case 'H':
        segments.add(new HexPackSegment(env, name, count));
        break;
      case 'c':
        segments.add(new BigEndianPackSegment(name, count, 1, true));
        break;
      case 'C':
        segments.add(new BigEndianPackSegment(name, count, 1, false));
        break;
      case 's':
        segments.add(new BigEndianPackSegment(name, count, 2, true));
        break;
      case 'S':
      case 'n':
        segments.add(new BigEndianPackSegment(name, count, 2, false));
        break;
      case 'v':
        segments.add(new LittleEndianPackSegment(name, count, 2));
        break;
      case 'l':
        segments.add(new BigEndianPackSegment(name, count, 4, true));
        break;
      case 'L':
      case 'N':
        segments.add(new BigEndianPackSegment(name, count, 4, false));
        break;
      case 'V':
        segments.add(new LittleEndianPackSegment(name, count, 4));
        break;
      case 'i':
      case 'I':
        segments.add(new BigEndianPackSegment(name, count, 8, false));
        break;
      case 'd':
        segments.add(new DoublePackSegment(name, count));
        break;
      case 'f':
        segments.add(new FloatPackSegment(name, count));
        break;
      case 'x':
        segments.add(new NullPackSegment(name, count));
        break;
      case '@':
        segments.add(new PositionPackSegment(name, count));
        break;
      default:
        env.warning(L.l("invalid format '{0}'", String.valueOf(ch)));
        return null;
      }
    }

    return segments;
  }

  abstract static class PackSegment {
    abstract public int pack(Env env, StringValue bb,
                              int i, Value []args)
      

    abstract public int unpack(Env env,
                               ArrayValue array,
                               StringValue s,
                               int offset,
                               int strLen)
      
  }

  static class SpacePackSegment : PackSegment {
    private StringValue _name;
    private int _length;
    private byte _pad;

    SpacePackSegment(Env env, int length, byte pad)
    {
      this(env, "", length, pad);
    }

    SpacePackSegment(Env env, string name, int length, byte pad)
    {
      _name = env.createString(name);
      _length = length;
      _pad = pad;
    }

    public override int pack(Env env, StringValue bb, int i, Value []args)
      
    {
      Value arg;

      if (i < args.length) {
        arg = args[i];
        i++;
      }
      else {
        env.warning(L.l("a: not enough arguments"));

        return i;
      }

      InputStream @is = arg.toInputStream();

      int length = _length;

      for (int j = 0; j < length; j++) {
        int ch = @is.read();

        if (ch >= 0)
          bb.appendByte(ch);
        else if (length == Integer.MAX_VALUE)
          return i;
        else
          bb.appendByte(_pad);
      }

      return i;
    }

    public override int unpack(Env env, ArrayValue result,
                      StringValue s, int offset, int strLen)
    {
      if (strLen - offset < _length)
        return offset;

      StringValue bb = env.createBinaryBuilder();

      int j = offset;

      for (int i = 0; i < _length; i++) {
        char ch = s[offset++];

        if (ch != _pad) {
          if (j + 1 != offset)
            bb.append(s, j, offset);
          else
            bb.append(ch);

          j = offset;
        }
      }

      if (_name.length() == 0)
        result.put(env.createString('1'), bb);
      else
        result.put(_name, bb);

      return offset;
    }
  }

  static class SpaceEndPackSegment : PackSegment
  {
    private StringValue _name;
    private byte _pad;

    SpaceEndPackSegment(Env env, byte pad)
    {
      this(env, "", pad);
    }

    SpaceEndPackSegment(Env env, string name, byte pad)
    {
      _name = env.createString(name);
      _pad = pad;
    }

    public override int pack(Env env, StringValue bb, int i, Value []args)
      
    {
      Value arg;

      if (i < args.length) {
        arg = args[i];
        i++;
      }
      else {
        env.warning(L.l("a: not enough arguments"));

        return i;
      }

      StringValue s = arg.ToStringValue(env);

      bb.append(s);

      return i;
    }

    public override int unpack(Env env, ArrayValue result,
                      StringValue s, int offset, int strLen)
    {
      StringValue bb = env.createBinaryBuilder();

      int j = offset;

      while (offset < strLen) {
        char ch = s[offset++];

        if (ch != _pad) {
          if (j + 1 != offset)
            bb.append(s, j, offset);
          else
            bb.append(ch);

          j = offset;
        }
      }

      if (_name.length() == 0)
        result.put(env.createString('1'), bb);
      else
        result.put(_name, bb);

      return offset;
    }
  }

  static class HexPackSegment : PackSegment {
    private Value _name;
    private int _length;

    HexPackSegment(Env env, int length)
    {
      this(env, "", length);
    }

    HexPackSegment(Env env, string name, int length)
    {
      if (name.length() != 0) {
        _name = env.createString(name);
      }
      else {
        _name = LongValue.ONE;
      }

      _length = length;
    }

    public override int pack(Env env, StringValue bb, int i, Value []args)
      
    {
      Value arg;

      if (i < args.length) {
        arg = args[i];
        i++;
      }
      else {
        env.warning(L.l("a: not enough arguments"));

        return i;
      }

      StringValue s = arg.ToStringValue();

      int strlen = s.length();

      if (_length == Integer.MAX_VALUE) {
      }
      else if (strlen < _length) {
        env.warning(L.l("not enough characters in hex string"));

        return i;
      }
      else if (_length < strlen)
        strlen = _length;

      int tail = strlen / 2;
      for (int j = 0; j < tail; j++) {
        int d = 0;

        char ch = s[2 * j];

        d += 16 * hexToDigit(env, ch);

        ch = s[2 * j + 1];

        d += hexToDigit(env, ch);

        bb.appendByte(d);
      }

      if ((strlen & 1) == 1) {
        int d = 16 * hexToDigit(env, s[strlen - 1]);

        bb.appendByte(d);
      }

      return i;
    }

    public override int unpack(Env env, ArrayValue result,
                      StringValue s, int offset, int strLen)
    {
      int len = _length;
      int maxLen = (strLen - offset) * 2;

      len = Math.min(len, maxLen);

      if (len == 0) {
        return offset;
      }

      StringValue sb = env.createStringBuilder();
      while (offset < strLen) {
        char ch = s[offset++];

        sb.append(digitToHex(ch >> 4));

        if (--len <= 0) {
          break;
        }

        sb.append(digitToHex(ch));

        if (--len <= 0) {
          break;
        }
      }

      result.put(_name, sb);

      return offset;
    }
  }

  static class RevHexPackSegment : PackSegment {
    private StringValue _name;
    private int _length;

    RevHexPackSegment(int length)
    {
      this("", length);
    }

    RevHexPackSegment(String name, int length)
    {
      _name = new StringBuilderValue(name);
      _length = length;
    }

    public override int pack(Env env, StringValue bb, int i, Value []args)
      
    {
      Value arg;

      if (i < args.length) {
        arg = args[i];
        i++;
      }
      else {
        env.warning("a: not enough arguments");

        return i;
      }

      StringValue s = arg.ToStringValue();

      int strlen = s.length();

      if (_length == Integer.MAX_VALUE) {
      }
      else if (strlen < _length) {
        env.warning("not enough characters in hex string");

        return i;
      }
      else if (_length < strlen)
        strlen = _length;

      int tail = strlen / 2;
      for (int j = 0; j < tail; j++) {
        int d = 0;

        char ch = s[2 * j];

        d += hexToDigit(env, ch);

        ch = s[2 * j + 1];

        d += 16 * hexToDigit(env, ch);

        bb.appendByte(d);
      }

      if ((strlen & 1) == 1) {
        int d = hexToDigit(env, s[strlen - 1]);

        bb.appendByte(d);
      }

      return i;
    }

    public override int unpack(Env env, ArrayValue result,
                      StringValue s, int offset, int strLen)
    {
      if (offset + (long) (_length / 2 - 1) >= strLen)
        return offset;

      StringValue sb = env.createStringBuilder();
      for (int i = _length / 2 - 1; i >= 0; i--) {
        char ch = s[offset++];

        sb.append(digitToHex(ch));
        sb.append(digitToHex(ch >> 4));
      }

      result.put(_name, sb);

      return offset;
    }
  }

  static class BigEndianPackSegment : PackSegment {
    private string _name;
    private int _length;
    private int _bytes;
    private bool _isSigned;

    BigEndianPackSegment(int length, int bytes, bool isSigned)
    {
      _name = "";
      _length = length;
      _bytes = bytes;
      _isSigned = isSigned;
    }

    BigEndianPackSegment(String name, int length, int bytes, bool isSigned)
    {
      _name = name;
      _length = length;
      _bytes = bytes;
      _isSigned = isSigned;
    }

    public override int pack(Env env, StringValue bb, int i, Value []args)
      
    {
      for (int j = 0; j < _length; j++) {
        Value arg;

        if (i < args.length) {
          arg = args[i];
          i++;
        }
        else if (_length == Integer.MAX_VALUE)
          return i;
        else {
          env.warning("a: not enough arguments");

          return i;
        }

        long v = arg.toLong();

        for (int k = _bytes - 1; k >= 0; k--) {
          bb.appendByte((int) (v >> (8 * k)));
        }
      }

      return i;
    }

    public override int unpack(Env env, ArrayValue result,
                      StringValue s, int offset, int strLen)
    {
      outer:
      for (int j = 0; j < _length; j++) {
        Value key;

        // XXX: check key type with unicode semantics

        if (_name.length() == 0)
          key = LongValue.create(j + 1);
        else if (_length == 1)
          key = env.createString(_name);
        else {
          StringValue sb = env.createStringBuilder();
          sb.append(_name);
          sb.append(j + 1);

          key = sb;
        }

        long v = 0;

        for (int k = 0; k < _bytes; k++) {
          if (strLen <= offset)
            break outer;

          char ch = s[offset++];

          long d = ch & 0xff;

          v = (v << 8) + d;
        }

        if (_isSigned) {
          switch (_bytes) {
          case 1:
            v = (byte) v;
            break;
          case 2:
            v = (short) v;
            break;
          case 4:
            v = (int) v;
            break;
          }
        }

        result.put(key, LongValue.create(v));
      }

      return offset;
    }
  }

  static class LittleEndianPackSegment : PackSegment {
    private string _name;
    private int _length;
    private int _bytes;

    LittleEndianPackSegment(int length, int bytes)
    {
      _name = "";
      _length = length;
      _bytes = bytes;
    }

    LittleEndianPackSegment(String name, int length, int bytes)
    {
      _name = name;
      _length = length;
      _bytes = bytes;
    }

    public override int pack(Env env, StringValue bb, int i, Value []args)
      
    {
      for (int j = 0; j < _length; j++) {
        Value arg;

        if (i < args.length) {
          arg = args[i];
          i++;
        }
        else if (_length == Integer.MAX_VALUE)
          return i;
        else {
          env.warning("a: not enough arguments");

          return i;
        }

        long v = arg.toLong();

        for (int k = 0; k < _bytes; k++) {
          bb.appendByte((int) (v >> (8 * k)));
        }
      }

      return i;
    }

    public override int unpack(Env env, ArrayValue result,
                      StringValue s, int offset, int strLen)
    {
      outer:
      for (int j = 0; j < _length; j++) {
        Value key;

        if (_name.length() == 0)
          key = LongValue.create(j + 1);
        else if (_length == 1)
          key = env.createString(_name);
        else {
          StringValue sb = env.createStringBuilder();
          sb.append(_name);
          sb.append(j + 1);

          key = sb;
        }

        long v = 0;

        for (int k = 0; k < _bytes; k++) {
          if (offset >= strLen)
            break outer;

          char ch = s[offset++];

          long d = ch & 0xff;

          v |= d << 8 * k;
        }

        result.put(key, LongValue.create(v));
      }

      return offset;
    }
  }

  static class DoublePackSegment : PackSegment {
    private string _name;
    private int _length;

    DoublePackSegment(int length)
    {
      this("", length);
    }

    DoublePackSegment(String name, int length)
    {
      _name = name;
      _length = length;
    }

    public override int pack(Env env, StringValue bb, int i, Value []args)
      
    {
      for (int j = 0; j < _length; j++) {
        Value arg;

        if (i < args.length) {
          arg = args[i];
          i++;
        }
        else if (_length == Integer.MAX_VALUE)
          return i;
        else {
          env.warning("a: not enough arguments");

          return i;
        }

        double d = arg.toDouble();
        long v = Double.doubleToLongBits(d);

        for (int k = 7; k >= 0; k--) {
          bb.appendByte((int) (v >> (8 * k) & 0xff));
        }
      }

      return i;
    }

    public override int unpack(Env env, ArrayValue result,
                      StringValue s, int offset, int strLen)
    {
      outer:
      for (int j = 0; j < _length; j++) {
        Value key;

        if (_name.length() == 0)
          key = LongValue.create(j + 1);
        else if (_length == 1)
          key = env.createString(_name);
        else {
          StringValue sb = env.createBinaryBuilder();
          sb.append(_name);
          sb.append(j + 1);

          key = sb;
        }

        long v = 0;

        for (int k = 0; k < 8; k++) {
          if (offset >= strLen)
            break outer;

          char ch = s[offset++];

          long d = ch & 0xff;

          v = 256 * v + d;
        }

        result.put(key, new DoubleValue(Double.longBitsToDouble(v)));
      }

      return offset;
    }
  }

  static class FloatPackSegment : PackSegment {
    private string _name;
    private int _length;

    FloatPackSegment(int length)
    {
      this("", length);
    }

    FloatPackSegment(String name, int length)
    {
      _name = name;
      _length = length;
    }

    public override int pack(Env env, StringValue bb, int i, Value []args)
      
    {
      for (int j = 0; j < _length; j++) {
        Value arg;

        if (i < args.length) {
          arg = args[i];
          i++;
        }
        else if (_length == Integer.MAX_VALUE)
          return i;
        else {
          env.warning("a: not enough arguments");

          return i;
        }

        double d = arg.toDouble();
        int v = Float.floatToIntBits((float) d);

        for (int k = 3; k >= 0; k--) {
          bb.appendByte((int) (v >> (8 * k) & 0xff));
        }
      }

      return i;
    }

    public override int unpack(Env env, ArrayValue result,
                      StringValue s, int offset, int strLen)
    {
      outer:
      for (int j = 0; j < _length; j++) {
        Value key;

        if (_name.length() == 0)
          key = LongValue.create(j + 1);
        else if (_length == 1)
          key = env.createString(_name);
        else {
          StringValue sb = env.createBinaryBuilder();
          sb.append(_name);
          sb.append(j + 1);

          key = sb;
        }

        int v = 0;

        for (int k = 0; k < 4; k++) {
          if (offset >= strLen)
            break outer;

          char ch = s[offset++];

          int d = ch & 0xff;

          v = 256 * v + d;
        }

        result.put(key, new DoubleValue(Float.intBitsToFloat(v)));
      }

      return offset;
    }
  }

  static class NullPackSegment : PackSegment {
    private string _name;
    private int _length;

    NullPackSegment(int length)
    {
      this("", length);
    }

    NullPackSegment(String name, int length)
    {
      _name = name;

      if (length == Integer.MAX_VALUE)
        length = 0;

      _length = length;
    }

    public override int pack(Env env, StringValue bb, int i, Value []args)
      
    {
      for (int j = 0; j < _length; j++) {
        bb.appendByte(0);
      }

      return i;
    }

    public override int unpack(Env env, ArrayValue result,
                      StringValue s, int offset, int strLen)
    {
      return (int) Math.min(offset + (long) _length, strLen);
    }
  }

  static class PositionPackSegment : PackSegment {
    private int _length;

    PositionPackSegment(int length)
    {
      this("", length);
    }

    PositionPackSegment(String name, int length)
    {
      if (length == Integer.MAX_VALUE)
        length = 0;

      _length = length;
    }

    public override int pack(Env env, StringValue bb, int i, Value []args)
      
    {
      while (bb.length() < _length) {
        bb.appendByte(0);
      }

      return i;
    }

    public override int unpack(Env env, ArrayValue result,
                      StringValue s, int offset, int strLen)
    {
      throw new UnsupportedOperationException("'@' skip to position");
    }
  }

  static int hexToDigit(Env env, char ch)
  {
    if ('0' <= ch && ch <= '9')
      return (ch - '0');
    else if ('a' <= ch && ch <= 'f')
      return (ch - 'a' + 10);
    else if ('A' <= ch && ch <= 'F')
      return (ch - 'A' + 10);
    else {
      env.warning("pack: non hex digit: " + (char) ch);

      return 0;
    }
  }

  static char digitToHex(int d)
  {
    d &= 0xf;

    if (d < 10)
      return (char) ('0' + d);
    else
      return (char) ('a' + d - 10);
  }
}
}
