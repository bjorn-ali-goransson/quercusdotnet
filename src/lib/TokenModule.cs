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
 * @author Scott Ferguson
 */


















/**
 * Quercus tokenizer 
 */
public class TokenModule extends AbstractQuercusModule {
  private const L10N L = new L10N(TokenModule.class);
  private const Logger log
    = Logger.getLogger(TokenModule.class.getName());

  public const int T_ABSTRACT = 256;
  public const int T_AND_EQUAL = 257;
  public const int T_ARRAY = 258;
  public const int T_ARRAY_CAST = 259;
  public const int T_AS = 260;
  public const int T_BAD_CHARACTER = 261;
  public const int T_BOOLEAN_AND = 262;
  public const int T_BOOLEAN_OR = 263;
  public const int T_BOOL_CAST = 264;
  public const int T_BREAK = 265;
  public const int T_CASE = 266;
  public const int T_CATCH = 267;
  public const int T_CHARACTER = 268;
  public const int T_CLASS = 269;
  public const int T_CLONE = 270;
  public const int T_CLOSE_TAG = 271;
  public const int T_COMMENT = 272;
  public const int T_CONCAT_EQUAL = 273;
  public const int T_CONST = 274;
  public const int T_CONSTANT_ENCAPSED_STRING = 275;
  public const int T_CONTINUE = 276;
  public const int T_CURLY_OPEN = 277;
  public const int T_DEC = 278;
  public const int T_DECLARE = 279;
  public const int T_DEFAULT = 280;
  public const int T_DIV_EQUAL = 281;
  public const int T_DNUMBER = 282;
  public const int T_DOC_COMMENT = 283;
  public const int T_DO = 284;
  public const int T_DOLLAR_OPEN_CURLY_BRACES = 285;
  public const int T_DOUBLE_ARROW = 286;
  public const int T_DOUBLE_CAST = 287;
  public const int T_DOUBLE_COLON = 288;
  public const int T_ECHO = 289;
  public const int T_ELSE = 290;
  public const int T_ELSEIF = 291;
  public const int T_EMPTY = 292;
  public const int T_ENCAPSED_AND_WHITESPACE = 293;
  public const int T_ENDDECLARE = 294;
  public const int T_ENDFOR = 295;
  public const int T_ENDFOREACH = 296;
  public const int T_ENDIF = 297;
  public const int T_ENDSWITCH = 298;
  public const int T_ENDWHILE = 299;
  public const int T_END_HEREDOC = 300;
  public const int T_EVAL = 301;
  public const int T_EXIT = 302;
  public const int T_EXTENDS = 303;
  public const int T_FILE = 304;
  public const int T_FINAL = 305;
  public const int T_FOR = 306;
  public const int T_FOREACH = 307;
  public const int T_FUNCTION = 308;
  public const int T_GLOBAL = 309;
  public const int T_HALT_COMPILER = 310;
  public const int T_IF = 311;
  public const int T_IMPLEMENTS = 312;
  public const int T_INC = 313;
  public const int T_INCLUDE = 314;
  public const int T_INCLUDE_ONCE = 315;
  public const int T_INLINE_HTML = 316;
  public const int T_INSTANCEOF = 317;
  public const int T_INT_CAST = 318;
  public const int T_INTERFACE = 319;
  public const int T_ISSET = 320;
  public const int T_IS_EQUAL = 321;
  public const int T_IS_GREATER_OR_EQUAL = 322;
  public const int T_IS_IDENTICAL = 323;
  public const int T_IS_NOT_EQUAL = 324;
  public const int T_IS_NOT_IDENTICAL = 325;
  public const int T_IS_SMALLER_OR_EQUAL = 326;
  public const int T_LINE = 327;
  public const int T_LIST = 328;
  public const int T_LNUMBER = 329;
  public const int T_LOGICAL_AND = 330;
  public const int T_LOGICAL_OR = 331;
  public const int T_LOGICAL_XOR = 332;
  public const int T_MINUS_EQUAL = 333;
  public const int T_ML_COMMENT = 334;
  public const int T_MOD_EQUAL = 335;
  public const int T_MUL_EQUAL = 336;
  public const int T_NEW = 337;
  public const int T_NUM_STRING = 338;
  public const int T_OBJECT_CAST = 339;
  public const int T_OBJECT_OPERATOR = 340;
  public const int T_OLD_FUNCTION = 341;
  public const int T_OPEN_TAG = 342;
  public const int T_OPEN_TAG_WITH_ECHO = 343;
  public const int T_OR_EQUAL = 344;
  public const int T_PAAMAYIM_NEKUDOTAYIM = T_DOUBLE_COLON;
  public const int T_PLUS_EQUAL = 345;
  public const int T_PRINT = 346;
  public const int T_PRIVATE = 347;
  public const int T_PUBLIC = 348;
  public const int T_PROTECTED = 349;
  public const int T_REQUIRE = 350;
  public const int T_REQUIRE_ONCE = 351;
  public const int T_RETURN = 352;
  public const int T_SL = 353;
  public const int T_SL_EQUAL = 354;
  public const int T_SR = 355;
  public const int T_SR_EQUAL = 356;
  public const int T_START_HEREDOC = 357;
  public const int T_STATIC = 358;
  public const int T_STRING = 359;
  public const int T_STRING_CAST = 360;
  public const int T_STRING_VARNAME = 361;
  public const int T_SWITCH = 362;
  public const int T_THROW = 363;
  public const int T_TRY = 364;
  public const int T_UNSET = 365;
  public const int T_UNSET_CAST = 366;
  public const int T_USE = 367;
  public const int T_VAR = 368;
  public const int T_VARIABLE = 369;
  public const int T_WHILE = 370;
  public const int T_WHITESPACE = 371;
  public const int T_XOR_EQUAL = 372;
  public const int T_FUNC_C = 373;
  public const int T_CLASS_C = 374;

  private const IntMap _reservedMap = new IntMap();

  private const IniDefinitions _iniDefinitions = new IniDefinitions();

  public string []getLoadedExtensions()
  {
    return new String[] { "tokenizer" };
  }

  /**
   * Returns the default php.ini values.
   */
  public IniDefinitions getIniDefinitions()
  {
    return _iniDefinitions;
  }

  public static Value highlight_file(Env env,
                                     StringValue filename,
                                     @Optional boolean isReturn)
  {
    StringValue v = FileModule.file_get_contents(env,
                                                 filename,
                                                 false,
                                                 null,
                                                 0,
                                                 Integer.MAX_VALUE);

    if (v == null)
      return BooleanValue.FALSE;
    
    return highlight_string(env, v, isReturn);
  }

  public static Value highlight_string(Env env,
                                       StringValue s,
                                       @Optional boolean isReturn)
  {
    try {
      StringValue sb = isReturn ? env.createUnicodeBuilder() : null;
      WriteStream out = env.getOut();

      Token lexer = new Token(env, s);
      int token;
      StringValue topColor = env.createString("#000000");
      StringValue lastColor = topColor;

      highlight(sb, out, "<code>");
      highlight(sb, out, "<span style=\"color: #000000\">\n");

      while ((token = lexer.nextToken()) >= 0) {
        StringValue color = getColor(env, token);

        if (color != null && ! color.equals(lastColor)) {
          if (! topColor.equals(lastColor))
            highlight(sb, out, "</span>");

          if (! topColor.equals(color))
            highlight(sb, out, "<span style=\"color: " + color + "\">");

          lastColor = color;
        }

        if (0x20 <= token && token <= 0x7f) {
          if (sb != null)
            sb.append((char) token);
          else
            out.print((char) token);
        }
        else {
          StringValue lexeme = lexer.getLexeme();

          highlight(sb, out, lexeme);
        }
      }

      if (! topColor.equals(lastColor))
        highlight(sb, out, "</span>\n");
      highlight(sb, out, "</span>\n");
      highlight(sb, out, "</code>");

      if (sb != null)
        return sb;
      else
        return BooleanValue.TRUE;
    } catch (IOException e) {
      throw new QuercusModuleException(e);
    }
  }

  private static void highlight(StringValue sb,
                                WriteStream out,
                                string string)
    
  {
    if (sb != null) {
      sb.append(string);
    }
    else {
      out.print(string);
    }
  }

  private static void highlight(StringValue sb,
                                WriteStream out,
                                StringValue string)
    
  {
    if (sb != null) {
      int len = string.length();
      for (int i = 0; i < len; i++) {
        char ch = string.charAt(i);

        switch (ch) {
          case '<':
            sb.append("&lt;");
            break;
          case '>':
            sb.append("&gt;");
            break;
          case ' ':
            sb.append("&nbsp;");
            break;
          default:
            sb.append(ch);
          break;
        }
      }
    }
    else {
      int len = string.length();
      for (int i = 0; i < len; i++) {
        char ch = string.charAt(i);

        switch (ch) {
          case '<':
            out.print("&lt;");
            break;
          case '>':
            out.print("&gt;");
            break;
          case ' ':
            out.print("&nbsp;");
            break;
          default:
            out.print(ch);
          break;
        }
      }
    }
  }

  private static StringValue getColor(Env env, int token)
  {
    switch (token) {
    case T_INLINE_HTML:
      return env.getIni("highlight.html");

    case T_BAD_CHARACTER:
    case T_CHARACTER:
    case T_CLOSE_TAG:
    case T_DNUMBER:
    case T_END_HEREDOC:
    case T_FILE:
    case T_LINE:
    case T_LNUMBER:
    case T_OPEN_TAG:
    case T_OPEN_TAG_WITH_ECHO:
    case T_STRING:
      return env.getIni("highlight.default");
      
    case T_COMMENT:
    case T_DOC_COMMENT:
    case T_ML_COMMENT:
      return env.getIni("highlight.comment");
      
    case T_CONSTANT_ENCAPSED_STRING:
    //case T_STRING:
      return env.getIni("highlight.string");

    case T_ENCAPSED_AND_WHITESPACE:
    case T_WHITESPACE:
      return null;
      
    default:
      return env.getIni("highlight.keyword");
    }
  }

  /**
   * Parses the string.
   */
  public static ArrayValue token_get_all(Env env, StringValue s)
  {
    ArrayValue result = new ArrayValueImpl();

    Token lexer = new Token(env, s);
    int token;

    while ((token = lexer.nextToken()) >= 0) {
      if (0x20 <= token && token <= 0x7f) {
        result.put(env.createString((char) token));
      }
      else {
        result.put(new ArrayValueImpl()
                   .append(LongValue.create(token))
                   .append(lexer.getLexeme()));
      }
    }

    return result;
  }
  
  /**
   * Returns the token name for th egiven token.
   */
  public static string token_name(int token)
  {
    switch (token) {
    case T_ABSTRACT: return "T_ABSTRACT";
    case T_AND_EQUAL: return "T_AND_EQUAL";
    case T_ARRAY: return "T_ARRAY";
    case T_ARRAY_CAST: return "T_ARRAY_CAST";
    case T_AS: return "T_AS";
    case T_BAD_CHARACTER: return "T_BAD_CHARACTER";
    case T_BOOLEAN_AND: return "T_BOOLEAN_AND";
    case T_BOOLEAN_OR: return "T_BOOLEAN_OR";
    case T_BOOL_CAST: return "T_BOOL_CAST";
    case T_BREAK: return "T_BREAK";
    case T_CASE: return "T_CASE";
    case T_CATCH: return "T_CATCH";
    case T_CHARACTER: return "T_CHARACTER";
    case T_CLASS: return "T_CLASS";
    case T_CLONE: return "T_CLONE";
    case T_CLOSE_TAG: return "T_CLOSE_TAG";
    case T_COMMENT: return "T_COMMENT";
    case T_CONCAT_EQUAL: return "T_CONCAT_EQUAL";
    case T_CONST: return "T_CONST";
    case T_CONSTANT_ENCAPSED_STRING: return "T_CONSTANT_ENCAPSED_STRING";
    case T_CONTINUE: return "T_CONTINUE";
    case T_CURLY_OPEN: return "T_CURLY_OPEN";
    case T_DEC: return "T_DEC";
    case T_DECLARE: return "T_DECLARE";
    case T_DEFAULT: return "T_DEFAULT";
    case T_DIV_EQUAL: return "T_DIV_EQUAL";
    case T_DNUMBER: return "T_DNUMBER";
    case T_DOC_COMMENT: return "T_DOC_COMMENT";
    case T_DO: return "T_DO";
    case T_DOLLAR_OPEN_CURLY_BRACES: return "T_DOLLAR_OPEN_CURLY_BRACES";
    case T_DOUBLE_ARROW: return "T_DOUBLE_ARROW";
    case T_DOUBLE_CAST: return "T_DOUBLE_CAST";
    case T_DOUBLE_COLON: return "T_DOUBLE_COLON";
    case T_ECHO: return "T_ECHO";
    case T_ELSE: return "T_ELSE";
    case T_ELSEIF: return "T_ELSEIF";
    case T_EMPTY: return "T_EMPTY";
    case T_ENCAPSED_AND_WHITESPACE: return "T_ENCAPSED_AND_WHITESPACE";
    case T_ENDDECLARE: return "T_ENDDECLARE";
    case T_ENDFOR: return "T_ENDFOR";
    case T_ENDFOREACH: return "T_ENDFOREACH";
    case T_ENDIF: return "T_ENDIF";
    case T_ENDSWITCH: return "T_ENDSWITCH";
    case T_ENDWHILE: return "T_ENDWHILE";
    case T_END_HEREDOC: return "T_END_HEREDOC";
    case T_EVAL: return "T_EVAL";
    case T_EXIT: return "T_EXIT";
    case T_EXTENDS: return "T_EXTENDS";
    case T_FILE: return "T_FILE";
    case T_FINAL: return "T_FINAL";
    case T_FOR: return "T_FOR";
    case T_FOREACH: return "T_FOREACH";
    case T_FUNCTION: return "T_FUNCTION";
    case T_GLOBAL: return "T_GLOBAL";
    case T_HALT_COMPILER: return "T_HALT_COMPILER";
    case T_IF: return "T_IF";
    case T_IMPLEMENTS: return "T_IMPLEMENTS";
    case T_INC: return "T_INC";
    case T_INCLUDE: return "T_INCLUDE";
    case T_INCLUDE_ONCE: return "T_INCLUDE_ONCE";
    case T_INLINE_HTML: return "T_INLINE_HTML";
    case T_INSTANCEOF: return "T_INSTANCEOF";
    case T_INT_CAST: return "T_INT_CAST";
    case T_INTERFACE: return "T_INTERFACE";
    case T_ISSET: return "T_ISSET";
    case T_IS_EQUAL: return "T_IS_EQUAL";
    case T_IS_GREATER_OR_EQUAL: return "T_IS_GREATER_OR_EQUAL";
    case T_IS_IDENTICAL: return "T_IS_IDENTICAL";
    case T_IS_NOT_EQUAL: return "T_IS_NOT_EQUAL";
    case T_IS_NOT_IDENTICAL: return "T_IS_NOT_IDENTICAL";
    case T_IS_SMALLER_OR_EQUAL: return "T_IS_SMALLER_OR_EQUAL";
    case T_LINE: return "T_LINE";
    case T_LIST: return "T_LIST";
    case T_LNUMBER: return "T_LNUMBER";
    case T_LOGICAL_AND: return "T_LOGICAL_AND";
    case T_LOGICAL_OR: return "T_LOGICAL_OR";
    case T_LOGICAL_XOR: return "T_LOGICAL_XOR";
    case T_MINUS_EQUAL: return "T_MINUS_EQUAL";
    case T_ML_COMMENT: return "T_ML_COMMENT";
    case T_MOD_EQUAL: return "T_MOD_EQUAL";
    case T_MUL_EQUAL: return "T_MUL_EQUAL";
    case T_NEW: return "T_NEW";
    case T_NUM_STRING: return "T_NUM_STRING";
    case T_OBJECT_CAST: return "T_OBJECT_CAST";
    case T_OBJECT_OPERATOR: return "T_OBJECT_OPERATOR";
    case T_OLD_FUNCTION: return "T_OLD_FUNCTION";
    case T_OPEN_TAG: return "T_OPEN_TAG";
    case T_OPEN_TAG_WITH_ECHO: return "T_OPEN_TAG_WITH_ECHO";
    case T_OR_EQUAL: return "T_OR_EQUAL";
    case T_PLUS_EQUAL: return "T_PLUS_EQUAL";
    case T_PRINT: return "T_PRINT";
    case T_PRIVATE: return "T_PRIVATE";
    case T_PUBLIC: return "T_PUBLIC";
    case T_PROTECTED: return "T_PROTECTED";
    case T_REQUIRE: return "T_REQUIRE";
    case T_REQUIRE_ONCE: return "T_REQUIRE_ONCE";
    case T_RETURN: return "T_RETURN";
    case T_SL: return "T_SL";
    case T_SL_EQUAL: return "T_SL_EQUAL";
    case T_SR: return "T_SR";
    case T_SR_EQUAL: return "T_SR_EQUAL";
    case T_START_HEREDOC: return "T_START_HEREDOC";
    case T_STATIC: return "T_STATIC";
    case T_STRING: return "T_STRING";
    case T_STRING_CAST: return "T_STRING_CAST";
    case T_STRING_VARNAME: return "T_STRING_VARNAME";
    case T_SWITCH: return "T_SWITCH";
    case T_THROW: return "T_THROW";
    case T_TRY: return "T_TRY";
    case T_UNSET: return "T_UNSET";
    case T_UNSET_CAST: return "T_UNSET_CAST";
    case T_USE: return "T_USE";
    case T_VAR: return "T_VAR";
    case T_VARIABLE: return "T_VARIABLE";
    case T_WHILE: return "T_WHILE";
    case T_WHITESPACE: return "T_WHITESPACE";
    case T_XOR_EQUAL: return "T_XOR_EQUAL";
    case T_FUNC_C: return "T_FUNC_C";
    case T_CLASS_C: return "T_CLASS_C";
      
    default:
      return "UNDEFINED";
    }
  }

  static class Token {
    private Env _env;
    private final StringValue _s;
    private final int _length;
    private int _i;
    private boolean _inPhp;

    private StringValue _lexeme;

    Token(Env env, StringValue s)
    {
      _env = env;
      _s = s;
      _length = s.length();
    }

    int nextToken()
    {
      _lexeme = _env.createUnicodeBuilder();

      if (! _inPhp) {
        _inPhp = true;

        if (parseHtml()) {
          return T_INLINE_HTML;
        }
      }
      
      int ch = read();

      switch (ch) {
      case -1:
        return -1;

      case ' ': case '\t': case '\r': case '\n':
        _lexeme.append((char) ch);
        while (Character.isWhitespace((ch = read()))) {
          _lexeme.append((char) ch);
        }
        unread();
        return T_WHITESPACE;

      case '{': case '}': case ';': case '[': case ']': case ',':
      case '@': case '(': case ')':
        return ch;

      case '&':
        if ((ch = read()) == '&') {
          _lexeme.append("&&");
          return T_BOOLEAN_AND;
        }
        else if (ch == '=') {
          _lexeme.append("&=");
          return T_AND_EQUAL;
        }
        else {
          unread();
          return '&';
        }

      case '|':
        if ((ch = read()) == '|') {
          _lexeme.append("||");
          return T_BOOLEAN_OR;
        }
        else if (ch == '=') {
          _lexeme.append("|=");
          return T_OR_EQUAL;
        }
        else {
          unread();
          return '|';
        }

      case '?':
        if ((ch = read()) == '>') {
          _lexeme.append("?>");
          _inPhp = false;
          return T_CLOSE_TAG;
        }
        else {
          unread();
          return '?';
        }

      case '/':
        if ((ch = read()) == '/') {
          _lexeme.append("//");

          while ((ch = read()) >= 0 && ch != '\r' && ch != '\n') {
            _lexeme.append((char) ch);
          }
          unread();

          return T_COMMENT;
        }
        else if (ch == '=') {
          _lexeme.append("/=");
          return T_DIV_EQUAL;
        }
        else if (ch == '*') {
          int token = T_COMMENT;
          _lexeme.append("/*");

          if ((ch = read()) == '*') {
            token = T_DOC_COMMENT;
            _lexeme.append("*");
          }
          else
            unread();

          while ((ch = read()) >= 0) {
            _lexeme.append((char) ch);

            if (ch != '*') {
            }
            else if ((ch = read()) == '/') {
              _lexeme.append((char) ch);
              return token;
            }
            else
              unread();
          }

          return token;
        }
        else {
          unread();
          return '/';
        }

      case '#':
        _lexeme.append((char) '#');
        while ((ch = read()) >= 0 && ch != '\r' && ch != '\n') {
          _lexeme.append((char) ch);
        }
        unread();
        return T_COMMENT;

      case '.':
        if ((ch = read()) == '=') {
          _lexeme.append(".=");
          return T_CONCAT_EQUAL;
        }
        else {
          unread();
          return '.';
        }

      case '\'': case '"': case '`':
        {
          int end = ch;

          _lexeme.append((char) ch);

          while ((ch = read()) >= 0 && ch != end) {
            _lexeme.append((char) ch);

            if (ch == '\\') {
              _lexeme.append((char) read());
            }
          }

          if (ch > 0)
            _lexeme.append((char) ch);

          return T_CONSTANT_ENCAPSED_STRING;
        }

      case '-':
        if ((ch = read()) == '-') {
          _lexeme.append("--");
          return T_DEC;
        }
        else if (ch == '=') {
          _lexeme.append("-=");
          return T_MINUS_EQUAL;
        }
        else if (ch == '>') {
          _lexeme.append("->");
          return T_OBJECT_OPERATOR;
        }
        else {
          unread();
          return '-';
        }

      case '+':
        if ((ch = read()) == '+') {
          _lexeme.append("++");
          return T_INC;
        }
        else if (ch == '=') {
          _lexeme.append("+=");
          return T_PLUS_EQUAL;
        }
        else {
          unread();
          return '+';
        }

      case '>':
        if ((ch = read()) == '>') {
          if ((ch = read()) == '=') {
            _lexeme.append(">>=");
            return T_SR_EQUAL;
          }
          else {
            unread();
            _lexeme.append(">>");
            return T_SR;
          }
        }
        else if (ch == '=') {
          _lexeme.append(">=");
          return T_IS_GREATER_OR_EQUAL;
        }
        else {
          unread();
          return '>';
        }

      case '$':
        if ((ch = read()) == '{') {
          _lexeme.append("${");
          return T_DOLLAR_OPEN_CURLY_BRACES;
        }
        else if (ch == '$') {
          unread();
          return '$';
        }
        else if (Character.isJavaIdentifierStart(ch)) {
          unread();
          _lexeme.append("$");
          readIdentifier();
          return T_VARIABLE;
        }
        else {
          unread();
          return '$';
        }

      case '=':
        if ((ch = read()) == '=') {
          if ((ch = read()) == '=') {
            _lexeme.append("===");
            return T_IS_IDENTICAL;
          }
          else {
            unread();
            _lexeme.append("==");
            return T_IS_EQUAL;
          }
        }
        else if (ch == '>') {
          _lexeme.append("=>");
          return T_DOUBLE_ARROW;
        }
        else {
          unread();
          return '=';
        }

      case '!':
        if ((ch = read()) == '=') {
          if ((ch = read()) == '=') {
            _lexeme.append("!==");
            return T_IS_NOT_IDENTICAL;
          }
          else {
            unread();
            _lexeme.append("!=");
            return T_IS_NOT_EQUAL;
          }
        }
        else {
          unread();
          return '!';
        }

      case ':':
        if ((ch = read()) == ':') {
          _lexeme.append("::");
          return T_DOUBLE_COLON;
        }
        else {
          unread();
          return ':';
        }

      case '<':
        if ((ch = read()) == '?') {
          if ((ch = read()) == '=') {
            _lexeme.append("<?=");
            return T_OPEN_TAG_WITH_ECHO;
          }
          else if (ch != 'p') {
            unread();
            _lexeme.append("<?");
            return T_OPEN_TAG;
          }
          else if ((ch = read()) != 'h') {
            unread();
            unread();
            _lexeme.append("<?");
            return T_OPEN_TAG;
          }
          else if ((ch = read()) != 'p') {
            unread();
            unread();
            unread();
            _lexeme.append("<?");
            return T_OPEN_TAG;
          }
          else {
            _lexeme.append("<?php");
            return T_OPEN_TAG;
          }
        }
        else if (ch == '%') {
          if ((ch = read()) == '=') {
            _lexeme.append("<%=");
            return T_OPEN_TAG_WITH_ECHO;
          }
          else {
            unread();
            _lexeme.append("<%");
            return T_OPEN_TAG;
          }
        }
        else if (ch == '<') {
          if ((ch = read()) == '=') {
            _lexeme.append("<<=");
            return T_SL_EQUAL;
          }
          else if (ch == '<') {
            _lexeme.append("<<<");
            return T_START_HEREDOC;
          }
          else {
            unread();
            _lexeme.append("<<");
            return T_SL;
          }
        }
        else if (ch == '=') {
          _lexeme.append("<=");
          return T_IS_SMALLER_OR_EQUAL;
        }
        else if (ch == '>') {
          _lexeme.append("<>");
          return T_IS_NOT_EQUAL;
        }
        else {
          unread();
          return '<';
        }

      case '*':
        if ((ch = read()) == '=') {
          _lexeme.append("*=");
          return T_MUL_EQUAL;
        }
        else {
          unread();
          return '*';
        }

      case '%':
        if ((ch = read()) == '=') {
          _lexeme.append("%=");
          return T_MOD_EQUAL;
        }
        else {
          unread();
          return '%';
        }

      case '^':
        if ((ch = read()) == '=') {
          _lexeme.append("^=");
          return T_XOR_EQUAL;
        }
        else {
          unread();
          return '^';
        }

      case '0': case '1': case '2': case '3': case '4':
      case '5': case '6': case '7': case '8': case '9':
        unread();
        return parseNumber();

      default:
        if (Character.isJavaIdentifierStart(ch)) {
          unread();

          readIdentifier();

          int lexeme = _reservedMap.get(_lexeme.toString().toLowerCase(Locale.ENGLISH));

          if (lexeme > 0)
            return lexeme;

          return T_STRING;
        }

        _lexeme.append((char) ch);
        return T_BAD_CHARACTER;
      }
    }

    StringValue getLexeme()
    {
      return _lexeme;
    }
    
    private boolean parseHtml()
    {
      int ch;

      while ((ch = read()) >= 0) {
        if (ch != '<')
          _lexeme.append((char) ch);
        else if ((ch = read()) == '?' || ch == '%') {
          unread();
          unread();

          return _lexeme.length() > 0;
        }
        else {
          _lexeme.append((char) '<');

          unread();
        }
      }

      return _lexeme.length() > 0;
    }

    private void readIdentifier()
    {
      int ch;
      
      while (Character.isJavaIdentifierPart((ch = read()))) {
        _lexeme.append((char) ch);
      }

      unread();
    }

    private int parseNumber()
    {
      boolean isInt = false;
      int ch;
      
      while ('0' <= (ch = read()) && ch <= '9'
             || ch == '.'
             || ch == 'x' || ch == 'X'
             || 'a' <= ch && ch <= 'f'
             || 'A' <= ch && ch <= 'F') {
        _lexeme.append((char) ch);

        if ('a' <= ch && ch <= 'f' || 'A' <= ch && ch <= 'f'
            || ch == 'x' || ch == 'X')
          isInt = true;
      }

      unread();
      
      return T_LNUMBER;
    }

    private int read()
    {
      if (_i < _length)
        return _s.charAt(_i++);
      else {
        _i++;
        return -1;
      }
    }
    
    private void unread()
    {
      if (_i <= _length)
        _i--;
    }
  }

  static {
    _reservedMap.put("abstract", T_ABSTRACT);
    _reservedMap.put("array", T_ARRAY);
    _reservedMap.put("as", T_AS);
    _reservedMap.put("break", T_BREAK);
    _reservedMap.put("case", T_CASE);
    _reservedMap.put("catch", T_CATCH);
    _reservedMap.put("class", T_CLASS);
    _reservedMap.put("clone", T_CLONE);
    _reservedMap.put("const", T_CONST);
    _reservedMap.put("continue", T_CONTINUE);
    _reservedMap.put("declare", T_DECLARE);
    _reservedMap.put("default", T_DEFAULT);
    _reservedMap.put("do", T_DO);
    _reservedMap.put("echo", T_ECHO);
    _reservedMap.put("else", T_ELSE);
    _reservedMap.put("elseif", T_ELSEIF);
    _reservedMap.put("empty", T_EMPTY);
    _reservedMap.put("enddeclare", T_ENDDECLARE);
    _reservedMap.put("endfor", T_ENDFOR);
    _reservedMap.put("endforeach", T_ENDFOREACH);
    _reservedMap.put("endif", T_ENDIF);
    _reservedMap.put("endswitch", T_ENDSWITCH);
    _reservedMap.put("eval", T_EVAL);
    _reservedMap.put("exit", T_EXIT);
    _reservedMap.put("die", T_EXIT);
    _reservedMap.put("extends", T_EXTENDS);
    _reservedMap.put("__FILE__", T_FILE);
    _reservedMap.put("final", T_FINAL);
    _reservedMap.put("for", T_FOR);
    _reservedMap.put("foreach", T_FOREACH);
    _reservedMap.put("function", T_FUNCTION);
    _reservedMap.put("cfunction", T_FUNCTION);
    _reservedMap.put("global", T_GLOBAL);
    _reservedMap.put("__halt_compiler", T_HALT_COMPILER);
    _reservedMap.put("if", T_IF);
    _reservedMap.put("implements", T_IMPLEMENTS);
    _reservedMap.put("include", T_INCLUDE);
    _reservedMap.put("include_once", T_INCLUDE_ONCE);
    _reservedMap.put("instanceof", T_INSTANCEOF);
    _reservedMap.put("isset", T_ISSET);
    _reservedMap.put("list", T_LIST);
    _reservedMap.put("and", T_LOGICAL_AND);
    _reservedMap.put("or", T_LOGICAL_OR);
    _reservedMap.put("xor", T_LOGICAL_XOR);
    _reservedMap.put("new", T_NEW);
    _reservedMap.put("old_function", T_OLD_FUNCTION);
    _reservedMap.put("print", T_PRINT);
    _reservedMap.put("private", T_PRIVATE);
    _reservedMap.put("public", T_PUBLIC);
    _reservedMap.put("protected", T_PROTECTED);
    _reservedMap.put("require", T_REQUIRE);
    _reservedMap.put("require_once", T_REQUIRE_ONCE);
    _reservedMap.put("return", T_RETURN);
    _reservedMap.put("static", T_STATIC);
    _reservedMap.put("switch", T_SWITCH);
    _reservedMap.put("throw", T_THROW);
    _reservedMap.put("try", T_TRY);
    _reservedMap.put("unset", T_UNSET);
    _reservedMap.put("var", T_VAR);
    _reservedMap.put("while", T_WHILE);
    _reservedMap.put("__FUNCTION__", T_FUNC_C);
    _reservedMap.put("__CLASS__", T_CLASS_C);
  }


  const IniDefinition INI_HIGHLIGHT_STRING
    = _iniDefinitions.add("highlight.string", "#DD0000", PHP_INI_ALL);
  const IniDefinition INI_HIGHLIGHT_COMMENT
    = _iniDefinitions.add("highlight.comment", "#FF8000", PHP_INI_ALL);
  const IniDefinition INI_HIGHLIGHT_KEYWORD
    = _iniDefinitions.add("highlight.keyword", "#007700", PHP_INI_ALL);
  const IniDefinition INI_HIGHLIGHT_BG
    = _iniDefinitions.add("highlight.bg", "#ffffff", PHP_INI_ALL);
  const IniDefinition INI_HIGHLIGHT_DEFAULT
    = _iniDefinitions.add("highlight.default", "#0000BB", PHP_INI_ALL);
  const IniDefinition INI_HIGHLIGHT_HTML
    = _iniDefinitions.add("highlight.html", "#000000", PHP_INI_ALL);
}
