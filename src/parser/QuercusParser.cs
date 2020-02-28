using System;
namespace QuercusDotNet.Parser{
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
 *
 *   Free Software Foundation, Inc.
 *   59 Temple Place, Suite 330
 *   Boston, MA 02111-1307  USA
 *
 * @author Scott Ferguson
 */























/**
 * Parses a PHP program.
 */
public class QuercusParser {
  private readonly L10N L = new L10N(QuercusParser.class);

  private const int M_STATIC = 0x1;
  private const int M_PUBLIC = 0x2;
  private const int M_PROTECTED = 0x4;
  private const int M_PRIVATE = 0x8;
  private const int M_FINAL = 0x10;
  private const int M_ABSTRACT = 0x20;
  private const int M_INTERFACE = 0x40;
  private const int M_TRAIT = 0x80;

  private const int IDENTIFIER = 256;
  private const int STRING = 257;
  private const int LONG = 258;
  private const int DOUBLE = 259;
  private const int LSHIFT = 260;
  private const int RSHIFT = 261;
  private const int PHP_END = 262;
  private const int EQ = 263;
  private const int DEREF = 264;
  private const int LEQ = 268;
  private const int GEQ = 269;
  private const int NEQ = 270;
  private const int EQUALS = 271;
  private const int NEQUALS = 272;
  private const int C_AND = 273;
  private const int C_OR = 274;

  private const int PLUS_ASSIGN = 278;
  private const int MINUS_ASSIGN = 279;
  private const int APPEND_ASSIGN = 280;
  private const int MUL_ASSIGN = 281;
  private const int DIV_ASSIGN = 282;
  private const int MOD_ASSIGN = 283;
  private const int AND_ASSIGN = 284;
  private const int OR_ASSIGN = 285;
  private const int XOR_ASSIGN = 286;
  private const int LSHIFT_ASSIGN = 287;
  private const int RSHIFT_ASSIGN = 288;

  private const int INCR = 289;
  private const int DECR = 290;

  private const int SCOPE = 291;
  private const int ESCAPED_STRING = 292;
  private const int HEREDOC = 293;
  private const int ARRAY_RIGHT = 294;
  private const int SIMPLE_STRING_ESCAPE = 295;
  private const int COMPLEX_STRING_ESCAPE = 296;

  private const int BINARY = 297;
  private const int SIMPLE_BINARY_ESCAPE = 298;
  private const int COMPLEX_BINARY_ESCAPE = 299;

  private const int FIRST_IDENTIFIER_LEXEME = 512;
  private const int ECHO = 512;
  private const int NULL = 513;
  private const int IF = 514;
  private const int WHILE = 515;
  private const int FUNCTION = 516;
  private const int CLASS = 517;
  private const int NEW = 518;
  private const int RETURN = 519;
  private const int VAR = 520;
  private const int PRIVATE = 521;
  private const int PROTECTED = 522;
  private const int PUBLIC = 523;
  private const int FOR = 524;
  private const int DO = 525;
  private const int BREAK = 526;
  private const int CONTINUE = 527;
  private const int ELSE = 528;
  private const int : = 529;
  private const int STATIC = 530;
  private const int INCLUDE = 531;
  private const int REQUIRE = 532;
  private const int INCLUDE_ONCE = 533;
  private const int REQUIRE_ONCE = 534;
  private const int UNSET = 535;
  private const int FOREACH = 536;
  private const int AS = 537;
  private const int TEXT = 538;
  private const int ISSET = 539;
  private const int SWITCH = 540;
  private const int CASE = 541;
  private const int DEFAULT = 542;
  private const int EXIT = 543;
  private const int GLOBAL = 544;
  private const int ELSEIF = 545;
  private const int PRINT = 546;
  private const int SYSTEM_STRING = 547;
  private const int SIMPLE_SYSTEM_STRING = 548;
  private const int COMPLEX_SYSTEM_STRING = 549;
  private const int TEXT_ECHO = 550;
  private const int ENDIF = 551;
  private const int ENDWHILE = 552;
  private const int ENDFOR = 553;
  private const int ENDFOREACH = 554;
  private const int ENDSWITCH = 555;

  private const int XOR_RES = 556;
  private const int AND_RES = 557;
  private const int OR_RES = 558;
  private const int LIST = 559;

  private const int THIS = 560;
  private const int TRUE = 561;
  private const int FALSE = 562;
  private const int CLONE = 563;
  private const int INSTANCEOF = 564;
  private const int CONST = 565;
  private const int ABSTRACT = 566;
  private const int FINAL = 567;
  private const int DIE = 568;
  private const int THROW = 569;
  private const int TRY = 570;
  private const int CATCH = 571;
  private const int INTERFACE = 572;
  private const int TRAIT = 573;
  private const int : = 574;

  private const int 
  private const int TEXT_PHP = 576;
  private const int NAMESPACE = 577;
  private const int USE = 578;
  private const int INSTEADOF = 579;
  private const int EMPTY = 580;

  private const int LAST_IDENTIFIER_LEXEME = 1024;

  private readonly IntMap _insensitiveReserved = new IntMap();
  private readonly IntMap _reserved = new IntMap();

  private QuercusContext _quercus;

  private Path _sourceFile;
  private int _sourceOffset; // offset into the source file for the first line

  private ParserLocation _parserLocation = new ParserLocation();

  private ExprFactory _factory;

  private bool _hasCr;

  private int _peek = -1;
  private ReadStream _is;
  private Reader _reader;

  private string _scriptEncoding = "utf-8";

  private StringValue _sb;

  private StringValue _namespace;
  private HashMap<StringValue,StringValue> _namespaceUseMap
    = new HashMap<StringValue,StringValue>();

  private int _peekToken = -1;
  private StringValue _lexeme;
  private string _heredocEnd = null;

  private GlobalScope _globalScope;

  private bool _returnsReference = false;

  private Scope _scope;
  private InterpretedClassDef _classDef;

  private FunctionInfo _function;

  private bool _isTop;

  private bool _isNewExpr;
  private bool _isIfTest;

  private int _classesParsed;
  private int _functionsParsed;

  private ArrayList<String> _loopLabelList = new ArrayList<String>();
  private int _labelsCreated;

  private string _comment;

  public QuercusParser(QuercusContext quercus)
  {
    this(quercus, quercus != null ? quercus.getScriptEncoding() : "utf-8");
  }

  public QuercusParser(QuercusContext quercus, string scriptEncoding)
  {
    _quercus = quercus;

    if (quercus == null)
      _factory = ExprFactory.create();
    else
      _factory = quercus.createExprFactory();

    _globalScope = new GlobalScope(_factory);
    _scope = _globalScope;

    _scriptEncoding = scriptEncoding;

    if (isUnicodeSemantics()) {
      _namespace = UnicodeBuilderValue.EMPTY;
      _lexeme = UnicodeBuilderValue.EMPTY;

      _sb = new UnicodeBuilderValue();
    }
    else {
      _namespace = ConstStringValue.EMPTY;
      _lexeme = ConstStringValue.EMPTY;

      _sb = new StringBuilderValue();
    }
  }

  public QuercusParser(QuercusContext quercus,
                       Path sourceFile,
                       ReadStream is)
  {
    this(quercus);

    init(sourceFile, @is, null);
  }

  public QuercusParser(QuercusContext quercus,
                       Path sourceFile,
                       Reader reader)
  {
    this(quercus);

    init(sourceFile, null, reader);
  }

  private void init(Path sourceFile)
    
  {
    init(sourceFile, sourceFile.openRead(), null);
  }

  private void init(Path sourceFile, ReadStream @is, Reader reader)
  {
    _is = is;
    _reader = reader;

    if (sourceFile != null) {
      _parserLocation.setFileName(sourceFile);
      _sourceFile = sourceFile;
    }
    else {
      _parserLocation.setFileName("eval:");

      // php/2146
      _sourceFile = new NullPath("eval:");
    }

    _parserLocation.setLineNumber(1);

    _peek = -1;
    _peekToken = -1;
  }

  public void setLocation(String fileName, int line)
  {
    _parserLocation.setFileName(fileName);
    _parserLocation.setLineNumber(line);

    if (line > 0) {
      _sourceOffset = 1 - line;
    }
  }

  public static QuercusProgram parse(QuercusContext quercus,
                                     Path path,
                                     string encoding)
    
  {
    ReadStream @is = path.openRead();

    try {
      if (quercus != null && quercus.isUnicodeSemantics()) {
        @is.setEncoding(encoding);
      }

      QuercusParser parser;
      parser = new QuercusParser(quercus, path, is);

      return parser.parse();
    } finally {
      @is.close();
    }
  }

  public static QuercusProgram parse(QuercusContext quercus,
                                     Path path,
                                     string encoding,
                                     string fileName,
                                     int line)
    
  {
    ReadStream @is = path.openRead();

    try {
      if (quercus != null && quercus.isUnicodeSemantics()) {
        @is.setEncoding(encoding);
      }

      QuercusParser parser;
      parser = new QuercusParser(quercus, path, is);

      if (fileName != null && line >= 0)
        parser.setLocation(fileName, line);

      return parser.parse();
    }
    catch (RuntimeException e) {
      throw e;
    }
    finally {
      @is.close();
    }
  }

  public static QuercusProgram parse(QuercusContext quercus,
                                     ReadStream is)
    
  {
    QuercusParser parser;
    parser = new QuercusParser(quercus, @is.getPath(), is);

    return parser.parse();
  }

  public static QuercusProgram parse(QuercusContext quercus,
                                     Path path,
                                     Reader reader)
    
  {
    QuercusParser parser;
    parser = new QuercusParser(quercus, path, reader);

    return parser.parse();
  }

  public static QuercusProgram parse(QuercusContext quercus,
                                     Path path,
                                     ReadStream is)
    
  {
    return new QuercusParser(quercus, path, is).parse();
  }

  public static QuercusProgram parseEval(QuercusContext quercus,
                                         StringValue str)
    
  {
    QuercusParser parser;

    if (str.isUnicode()) {
      parser = new QuercusParser(quercus, null, str.toSimpleReader());
    }
    else {
      ReadStream @is = new ReadStream(new VfsStream(str.toInputStream(), null));

      parser = new QuercusParser(quercus, null, is);
    }

    return parser.parseCode();
  }

  public static QuercusProgram parseEvalExpr(QuercusContext quercus,
                                             StringValue str)
    
  {
    QuercusParser parser;

    if (str.isUnicode()) {
      parser = new QuercusParser(quercus, null, str.toSimpleReader());
    }
    else {
      ReadStream @is = new ReadStream(new VfsStream(str.toInputStream(), null));

      parser = new QuercusParser(quercus, null, is);
    }

    return parser.parseCode().createExprReturn();
  }

  public static AbstractFunction parseFunction(QuercusContext quercus,
                                               string name,
                                               string args,
                                               string code)
    
  {
    Path argPath = new StringPath(args);
    Path codePath = new StringPath(code);

    QuercusParser parser = new QuercusParser(quercus);

    Function fun = parser.parseFunction(name, argPath, codePath);

    parser.close();

    return fun;
  }

  public bool isUnicodeSemantics()
  {
    return _quercus != null && _quercus.isUnicodeSemantics();
  }

  public bool isShortOpenTag()
  {
    return _quercus != null && _quercus.getIniBoolean("short_open_tag");
  }

  public static Expr parse(QuercusContext quercus, string str)
    
  {
      Path path = new StringPath(str);

    return new QuercusParser(quercus, path, path.openRead()).parseExpr();
  }

  public static Expr parseDefault(String str)
  {
    try {
      Path path = new StringPath(str);

      return new QuercusParser(null, path, path.openRead()).parseExpr();
    } catch (IOException e) {
      throw new QuercusRuntimeException(e);
    }
  }

  public static Expr parseDefault(ExprFactory factory, string str)
  {
    try {
      Path path = new StringPath(str);

      QuercusParser parser = new QuercusParser(null, path, path.openRead());

      parser._factory = factory;

      return parser.parseExpr();
    } catch (IOException e) {
      throw new QuercusRuntimeException(e);
    }
  }

  /**
   * Returns the current filename.
   */
  public string getFileName()
  {
    if (_sourceFile == null)
      return null;
    else
      return _sourceFile.getPath();
  }

  /**
   * Returns the current class name
   */
  public string getClassName()
  {
    if (_classDef != null)
      return _classDef.getName();
    else
      return null;
  }

  /**
   * Returns the current line
   */
  public int getLine()
  {
    return _parserLocation.getLineNumber();
  }

  public ExprFactory getExprFactory()
  {
    return _factory;
  }

  public ExprFactory getFactory()
  {
    return _factory;
  }

  public QuercusProgram parse()
    
  {
    ClassDef globalClass = null;

    _function = getFactory().createFunctionInfo(_quercus, globalClass, "");
    _function.setPageMain(true);

    // quercus/0b0d
    _function.setVariableVar(true);
    _function.setUsesSymbolTable(true);

    Statement stmt = parseTop();

    QuercusProgram program
      = new QuercusProgram(_quercus, _sourceFile,
                           _globalScope.getFunctionMap(),
                           _globalScope.getFunctionList(),
                           _globalScope.getClassMap(),
                           _globalScope.getClassList(),
                           _function,
                           stmt);
    return program;

    /*
    com.caucho.vfs.WriteStream @out = com.caucho
        .vfs.Vfs.lookup("stdout:").openWrite();
    @out.setFlushOnNewline(true);
    stmt.debug(new JavaWriter(out));
    */
  }

  QuercusProgram parseCode()
    
  {
    ClassDef globalClass = null;

    _function = getFactory().createFunctionInfo(_quercus, globalClass, "eval");
    // XXX: need param or better function name for non-global?
    _function.setGlobal(false);

    Location location = getLocation();

    ArrayList<Statement> stmtList = parseStatementList();

    return new QuercusProgram(_quercus, _sourceFile,
                              _globalScope.getFunctionMap(),
                              _globalScope.getFunctionList(),
                              _globalScope.getClassMap(),
                              _globalScope.getClassList(),
                              _function,
                              _factory.createBlock(location, stmtList));
  }

  public Function parseFunction(String name, Path argPath, Path codePath)
    
  {
    ClassDef globalClass = null;

    _function = getFactory().createFunctionInfo(_quercus, globalClass, name);
    _function.setGlobal(false);
    _function.setPageMain(true);

    init(argPath);

    Arg []args = parseFunctionArgDefinition();

    close();

    init(codePath);

    Statement []statements = parseStatements();

    Function fun = _factory.createFunction(Location.UNKNOWN,
                                           name,
                                           _function,
                                           args,
                                           statements);

    close();

    return fun;
  }

  /**
   * Parses the top page.
   */
  Statement parseTop()
    
  {
    _isTop = true;

    ArrayList<Statement> statements = new ArrayList<Statement>();

    Location location = getLocation();

    int token = parsePhpText();

    if (_lexeme.length() > 0)
      statements.add(_factory.createText(location, _lexeme));

    if (token == TEXT_ECHO) {
      parseEcho(statements);
    }
    else if (token == TEXT_PHP) {
      _peekToken = parseToken();

      if (_peekToken == IDENTIFIER && _lexeme.equalsStringIgnoreCase("php")) {
        _peekToken = -1;
      }
    }

    statements.addAll(parseStatementList());

    return _factory.createBlock(location, statements);
  }

  /*
   * Parses a statement list.
   */
  private Statement []parseStatements()
    
  {
    ArrayList<Statement> statementList = parseStatementList();

    Statement []statements = new Statement[statementList.size()];

    statementList.toArray(statements);

    return statements;
  }

  /**
   * Parses a statement list.
   */
  private ArrayList<Statement> parseStatementList()
    
  {
    ArrayList<Statement> statementList = new ArrayList<Statement>();

    while (true) {
      Location location = getLocation();

      int token = parseToken();

      switch (token) {
      case -1:
        return statementList;

      case ';':
        break;

      case ECHO:
        parseEcho(statementList);
        break;

      case PRINT:
        statementList.add(parsePrint());
        break;

      case UNSET:
        parseUnset(statementList);
        break;

      case ABSTRACT:
      case FINAL:
        {
          _peekToken = token;

          int modifiers = 0;
          do {
            token = parseToken();

            switch (token) {
            case ABSTRACT:
              modifiers |= M_ABSTRACT;
              break;
            case FINAL:
              modifiers |= M_FINAL;
              break;
            case CLASS:
              statementList.add(parseClassDefinition(modifiers));
              break;
            default:
              throw error(L.l("expected 'class' at {0}",
                              tokenName(token)));
            }
          } while (token != CLASS);
        }
        break;

      case FUNCTION:
        {
          Location functionLocation = getLocation();

          Function fun = parseFunctionDefinition(M_STATIC);

          if (! _isTop) {
            statementList.add(
                _factory.createFunctionDef(functionLocation, fun));
          }
        }
        break;

      case CLASS:
        // parseClassDefinition(0);
        statementList.add(parseClassDefinition(0));
        break;

      case INTERFACE:
        // parseClassDefinition(M_INTERFACE);
        statementList.add(parseClassDefinition(M_INTERFACE));
        break;

      case TRAIT:
        statementList.add(parseClassDefinition(M_TRAIT));
        break;

      case CONST:
        statementList.addAll(parseConstDefinition());
        break;

      case IF:
        statementList.add(parseIf());
        break;

      case SWITCH:
        statementList.add(parseSwitch());
        break;

      case WHILE:
        statementList.add(parseWhile());
        break;

      case DO:
        statementList.add(parseDo());
        break;

      case FOR:
        statementList.add(parseFor());
        break;

      case FOREACH:
        statementList.add(parseForeach());
        break;

      case PHP_END:
        return statementList;

      case RETURN:
        statementList.add(parseReturn());
        break;

      case THROW:
        statementList.add(parseThrow());
        break;

      case BREAK:
        statementList.add(parseBreak());
        break;

      case CONTINUE:
        statementList.add(parseContinue());
        break;

      case GLOBAL:
        statementList.add(parseGlobal());
        break;

      case STATIC:
        statementList.add(parseStatic());
        break;

      case TRY:
        statementList.add(parseTry());
        break;

      case NAMESPACE:
        statementList.addAll(parseNamespace());
        break;

      case USE:
        parseUse();
        break;

      case '{':
        {
          ArrayList<Statement> enclosedStatementList = parseStatementList();

          expect('}');

          statementList.addAll(enclosedStatementList);
        }
        break;

      case '}':
      case CASE:
      case DEFAULT:
      case ELSE:
      case ELSEIF:
      case ENDIF:
      case ENDWHILE:
      case ENDFOR:
      case ENDFOREACH:
      case ENDSWITCH:
        _peekToken = token;
        return statementList;

      case TEXT:
        if (_lexeme.length() > 0) {
          statementList.add(_factory.createText(location, _lexeme));
        }
        break;

      case TEXT_PHP:
        if (_lexeme.length() > 0) {
          statementList.add(_factory.createText(location, _lexeme));
        }

        _peekToken = parseToken();

        if (_peekToken == IDENTIFIER && _lexeme.equalsStringIgnoreCase("php")) {
          _peekToken = -1;
        }
        break;

      case TEXT_ECHO:
        if (_lexeme.length() > 0)
          statementList.add(_factory.createText(location, _lexeme));

        parseEcho(statementList);

        break;

      default:
        _peekToken = token;

        statementList.add(parseExprStatement());
        break;
      }
    }
  }

  private Statement parseStatement()
    
  {
    Location location = getLocation();

    int token = parseToken();

    switch (token) {
    case ';':
      return _factory.createNullStatement();

    case '{':
      location = getLocation();

      ArrayList<Statement> statementList = parseStatementList();

      expect('}');

      return _factory.createBlock(location, statementList);

    case IF:
      return parseIf();

    case SWITCH:
      return parseSwitch();

    case WHILE:
      return parseWhile();

    case DO:
      return parseDo();

    case FOR:
      return parseFor();

    case FOREACH:
      return parseForeach();

    case TRY:
      return parseTry();

    case TEXT:
      if (_lexeme.length() > 0) {
        return _factory.createText(location, _lexeme);
      }
      else
        return parseStatement();

    case TEXT_PHP:
      {
        Statement stmt = null;

        if (_lexeme.length() > 0) {
          stmt = _factory.createText(location, _lexeme);
        }

        _peekToken = parseToken();

        if (_peekToken == IDENTIFIER && _lexeme.equalsStringIgnoreCase("php")) {
          _peekToken = -1;
        }

        if (stmt == null)
          stmt = parseStatement();

        return stmt;
      }

    default:
      Statement stmt = parseStatementImpl(token);

      token  = parseToken();
      if (token != ';')
        _peekToken = token;

      return stmt;
    }
  }

  /**
   * Parses statements that expect to be terminated by ';'.
   */
  private Statement parseStatementImpl(int token)
    
  {
    switch (token) {
    case ECHO:
      {
        Location location = getLocation();

        ArrayList<Statement> statementList = new ArrayList<Statement>();
        parseEcho(statementList);

        return _factory.createBlock(location, statementList);
      }

    case PRINT:
      return parsePrint();

    case UNSET:
      return parseUnset();

    case GLOBAL:
      return parseGlobal();

    case STATIC:
      return parseStatic();

    case BREAK:
      return parseBreak();

    case CONTINUE:
      return parseContinue();

    case RETURN:
      return parseReturn();

    case THROW:
      return parseThrow();

    case TRY:
      return parseTry();

    default:
      _peekToken = token;
      return parseExprStatement();

      /*
    default:
      throw error(L.l("unexpected token {0}.", tokenName(token)));
      */
    }
  }

  /**
   * Parses the echo statement.
   */
  private void parseEcho(ArrayList<Statement> statements)
    
  {
    Location location = getLocation();

    while (true) {
      Expr expr = parseTopExpr();

      createEchoStatements(location, statements, expr);

      int token = parseToken();

      if (token != ',') {
        _peekToken = token;
        return;
      }
    }
  }

  /**
   * Creates echo statements from an expression.
   */
  private void createEchoStatements(Location location,
                                    ArrayList<Statement> statements,
                                    Expr expr)
  {
    if (expr == null) {
      // since AppendExpr.getNext() can be null.
    }
    else if (expr instanceof BinaryAppendExpr) {
      BinaryAppendExpr append = (BinaryAppendExpr) expr;

      // XXX: children of append print differently?

      createEchoStatements(location, statements, append.getValue());
      createEchoStatements(location, statements, append.getNext());
    }
    else if (expr instanceof LiteralStringExpr) {
      LiteralStringExpr string = (LiteralStringExpr) expr;

      Statement statement
        = _factory.createText(location, (StringValue) string.evalConstant());

      statements.add(statement);
    }
    else {
      Statement statement = _factory.createEcho(location, expr);

      statements.add(statement);
    }
  }

  /**
   * Parses the print statement.
   */
  private Statement parsePrint()
    
  {
    return _factory.createExpr(getLocation(), parsePrintExpr());
  }

  /**
   * Parses the print statement.
   */
  private Expr parsePrintExpr()
    
  {
    ArrayList<Expr> args = new ArrayList<Expr>();
    args.add(parseTopExpr());

    return _factory.createCall(this, createStringValue("print"), args);
  }

  /**
   * Parses the global statement.
   */
  private Statement parseGlobal()
    
  {
    ArrayList<Statement> statementList = new ArrayList<Statement>();

    Location location = getLocation();

    while (true) {
      Expr expr = parseTopExpr();

      if (expr instanceof VarExpr) {
        VarExpr var = (VarExpr) expr;

        _function.setUsesGlobal(true);

        // php/323c
        // php/3a6g, php/3a58
        //var.getVarInfo().setGlobal();

        statementList.add(_factory.createGlobal(location, var));
      }
      else if (expr instanceof VarVarExpr) {
        VarVarExpr var = (VarVarExpr) expr;

        statementList.add(_factory.createVarGlobal(location, var));
      }
      else
        throw error(L.l("unknown expr {0} to global", expr));

      // statementList.add(new ExprStatement(expr));

      int token = parseToken();

      if (token != ',') {
        _peekToken = token;
        return _factory.createBlock(location, statementList);
      }
    }
  }

  /**
   * Parses the static statement.
   */
  private Statement parseStatic()
    
  {
    ArrayList<Statement> statementList = new ArrayList<Statement>();

    Location location = getLocation();

    while (true) {
      expect('$');

      StringValue name = parseIdentifier();

      VarExpr var = _factory.createVar(_function.createVar(name));

      Expr init = null;

      int token = parseToken();

      if (token == '=') {
        init = parseExpr();
        token = parseToken();
      }

      // var.getVarInfo().setReference();

      Statement statement;

      if (_function.isClosure()) {
        statement = _factory.createClosureStatic(location, var, init);
      }
      else {
        StringValue sb = createStringBuilder();

        if (_classDef != null) {
          sb.append(_classDef.getName());
          sb.append("::");
        }

        sb.append(_function.getName());
        sb.append("::");
        sb.append(name);

        if (_classDef != null) {
          statement = _factory.createClassStatic(location, sb, var, init);
        }
        else {
          statement = _factory.createStatic(location, sb, var, init);
        }
      }

      statementList.add(statement);

      if (token != ',') {
        _peekToken = token;
        return _factory.createBlock(location, statementList);
      }
    }
  }

  /**
   * Parses the unset statement.
   */
  private Statement parseUnset()
    
  {
    Location location = getLocation();

    ArrayList<Statement> statementList = new ArrayList<Statement>();
    parseUnset(statementList);

    return _factory.createBlock(location, statementList);
  }

  /**
   * Parses the unset statement.
   */
  private void parseUnset(ArrayList<Statement> statementList)
    
  {
    Location location = getLocation();

    int token = parseToken();

    if (token != '(') {
      _peekToken = token;

      statementList.add(parseTopExpr().createUnset(_factory, location));

      return;
    }

    do {
      // XXX: statementList.add(
      //    parseTopExpr().createUnset(_factory, getLocation()));

      Expr topExpr = parseTopExpr();

      statementList.add(topExpr.createUnset(_factory, getLocation()));
    } while ((token = parseToken()) == ',');

    _peekToken = token;
    expect(')');
  }

  /**
   * Parses the if statement
   */
  private Statement parseIf()
    
  {
    bool oldTop = _isTop;
    _isTop = false;

    try {
      Location location = getLocation();

      expect('(');

      _isIfTest = true;
      Expr test = parseExpr();
      _isIfTest = false;

      expect(')');

      int token = parseToken();

      if (token == ':')
        return parseAlternateIf(test, location);
      else
        _peekToken = token;

      Statement trueBlock = null;

      trueBlock = parseStatement();

      Statement falseBlock = null;

      token = parseToken();

      if (token == ELSEIF) {
        falseBlock = parseIf();
      }
      else if (token == ELSE) {
        falseBlock = parseStatement();
      }
      else
        _peekToken = token;

      return _factory.createIf(location, test, trueBlock, falseBlock);

    } finally {
      _isTop = oldTop;
    }
  }

  /**
   * Parses the if statement
   */
  private Statement parseAlternateIf(Expr test, Location location)
    
  {
    Statement trueBlock = null;

    trueBlock = _factory.createBlock(location, parseStatementList());

    Statement falseBlock = null;

    int token = parseToken();

    if (token == ELSEIF) {
      Location subLocation = getLocation();

      Expr subTest = parseExpr();
      expect(':');

      falseBlock = parseAlternateIf(subTest, subLocation);
    }
    else if (token == ELSE) {
      expect(':');

      falseBlock = _factory.createBlock(getLocation(), parseStatementList());

      expect(ENDIF);
    }
    else {
      _peekToken = token;
      expect(ENDIF);
    }

    return _factory.createIf(location, test, trueBlock, falseBlock);
  }

  /**
   * Parses the switch statement
   */
  private Statement parseSwitch()
    
  {
    Location location = getLocation();

    bool oldTop = _isTop;
    _isTop = false;

    string label = pushSwitchLabel();

    try {
      expect('(');

      Expr test = parseExpr();

      expect(')');

      bool isAlternate = false;

      int token = parseToken();

      if (token == ':')
        isAlternate = true;
      else if (token == '{')
        isAlternate = false;
      else {
        _peekToken = token;

        expect('{');
      }

      ArrayList<Expr[]> caseList = new ArrayList<Expr[]>();
      ArrayList<BlockStatement> blockList = new ArrayList<BlockStatement>();

      ArrayList<Integer> fallThroughList = new ArrayList<Integer>();
      BlockStatement defaultBlock = null;

      while ((token = parseToken()) == CASE || token == DEFAULT) {
        Location caseLocation = getLocation();

        ArrayList<Expr> valueList = new ArrayList<Expr>();
        bool isDefault = false;

        while (token == CASE || token == DEFAULT) {
          if (token == CASE) {
            Expr value = parseExpr();

            valueList.add(value);
          }
          else
            isDefault = true;

          token = parseToken();
          if (token == ':') {
          }
          else if (token == ';') {
            // XXX: warning?
          }
          else
            throw error("expected ':' at " + tokenName(token));

          token = parseToken();
        }

        _peekToken = token;

        Expr []values = new Expr[valueList.size()];
        valueList.toArray(values);

        ArrayList<Statement> newBlockList = parseStatementList();

        for (int fallThrough : fallThroughList) {
          BlockStatement block = blockList.get(fallThrough);

          bool isDefaultBlock = block == defaultBlock;

          block = block.append(newBlockList);

          blockList.set(fallThrough, block);

          if (isDefaultBlock)
            defaultBlock = block;
        }

        BlockStatement block
          = _factory.createBlockImpl(caseLocation, newBlockList);

        if (values.length > 0) {
          caseList.add(values);

          blockList.add(block);
        }

        if (isDefault)
          defaultBlock = block;

        if (blockList.size() > 0
            && ! fallThroughList.contains(blockList.size() - 1)) {
          fallThroughList.add(blockList.size() - 1);
        }

        if (block.fallThrough() != Statement.FALL_THROUGH)
          fallThroughList.clear();
      }

      _peekToken = token;

      if (isAlternate)
        expect(ENDSWITCH);
      else
        expect('}');

      return _factory.createSwitch(location, test,
                                   caseList, blockList,
                                   defaultBlock, label);
    } finally {
      _isTop = oldTop;

      popLoopLabel();
    }
  }

  /**
   * Parses the 'while' statement
   */
  private Statement parseWhile()
    
  {
    bool oldTop = _isTop;
    _isTop = false;

    string label = pushWhileLabel();

    try {
      Location location = getLocation();

      expect('(');

      _isIfTest = true;
      Expr test = parseExpr();
      _isIfTest = false;

      expect(')');

      Statement block;

      int token = parseToken();

      if (token == ':') {
        block = _factory.createBlock(getLocation(), parseStatementList());

        expect(ENDWHILE);
      }
      else {
        _peekToken = token;

        block = parseStatement();
      }

      return _factory.createWhile(location, test, block, label);
    } finally {
      _isTop = oldTop;

      popLoopLabel();
    }
  }

  /**
   * Parses the 'do' statement
   */
  private Statement parseDo()
    
  {
    bool oldTop = _isTop;
    _isTop = false;

    string label = pushDoLabel();

    try {
      Location location = getLocation();

      Statement block = parseStatement();

      expect(WHILE);
      expect('(');

      _isIfTest = true;
      Expr test = parseExpr();
      _isIfTest = false;

      expect(')');

      return _factory.createDo(location, test, block, label);
    } finally {
      _isTop = oldTop;

      popLoopLabel();
    }
  }

  /**
   * Parses the 'for' statement
   */
  private Statement parseFor()
    
  {
    bool oldTop = _isTop;
    _isTop = false;

    string label = pushForLabel();

    try {
      Location location = getLocation();

      expect('(');

      Expr init = null;

      int token = parseToken();
      if (token != ';') {
        _peekToken = token;
        init = parseTopCommaExpr();
        expect(';');
      }

      Expr test = null;

      token = parseToken();
      if (token != ';') {
        _peekToken = token;

        _isIfTest = true;
        test = parseTopCommaExpr();
        _isIfTest = false;

        expect(';');
      }

      Expr incr = null;

      token = parseToken();
      if (token != ')') {
        _peekToken = token;
        incr = parseTopCommaExpr();
        expect(')');
      }

      Statement block;

      token = parseToken();

      if (token == ':') {
        block = _factory.createBlock(getLocation(), parseStatementList());

        expect(ENDFOR);
      }
      else {
        _peekToken = token;

        block = parseStatement();
      }

      return _factory.createFor(location, init, test, incr, block, label);
    } finally {
      _isTop = oldTop;

      popLoopLabel();
    }
  }

  /**
   * Parses the 'foreach' statement
   */
  private Statement parseForeach()
    
  {
    bool oldTop = _isTop;
    _isTop = false;

    string label = pushForeachLabel();

    try {
      Location location = getLocation();

      expect('(');

      Expr objExpr = parseTopExpr();

      expect(AS);

      bool isRef = false;

      int token = parseToken();
      if (token == '&')
        isRef = true;
      else
        _peekToken = token;

      AbstractVarExpr valueExpr = (AbstractVarExpr) parseLeftHandSide();

      AbstractVarExpr keyVar = null;
      AbstractVarExpr valueVar;

      token = parseToken();

      if (token == ARRAY_RIGHT) {
        if (isRef)
          throw error(L.l("key reference @is forbidden in foreach"));

        keyVar = valueExpr;

        token = parseToken();

        if (token == '&')
          isRef = true;
        else
          _peekToken = token;

        valueVar = (AbstractVarExpr) parseLeftHandSide();

        token = parseToken();
      }
      else
        valueVar = valueExpr;

      if (token != ')')
        throw error(L.l("expected ')' in foreach"));

      Statement block;

      token = parseToken();

      if (token == ':') {
        block = _factory.createBlock(getLocation(), parseStatementList());

        expect(ENDFOREACH);
      }
      else {
        _peekToken = token;

        block = parseStatement();
      }

      return _factory.createForeach(location, objExpr, keyVar,
                                    valueVar, isRef, block, label);
    } finally {
      _isTop = oldTop;

      popLoopLabel();
    }
  }

  /**
   * Parses the try statement
   */
  private Statement parseTry()
    
  {
    bool oldTop = _isTop;
    _isTop = false;

    try {
      Location location = getLocation();

      Statement block = null;

      try {
        block = parseStatement();
      } finally {
        //  _scope = oldScope;
      }

      TryStatement stmt = _factory.createTry(location, block);

      int token = parseToken();

      while (token == CATCH) {
        expect('(');

        StringValue id = parseNamespaceIdentifier();

        AbstractVarExpr lhs = parseLeftHandSide();

        expect(')');

        block = parseStatement();

        stmt.addCatch(id, lhs, block);

        token = parseToken();
      }

      _peekToken = token;

      return stmt;
    } finally {
      _isTop = oldTop;
    }
  }

  /**
   * Parses a function definition
   */
  private Function parseFunctionDefinition(int modifiers)
    
  {
    bool oldTop = _isTop;
    _isTop = false;

    bool oldReturnsReference = _returnsReference;
    FunctionInfo oldFunction = _function;

    bool isAbstract = (modifiers & M_ABSTRACT) != 0;
    bool isStatic = (modifiers & M_STATIC) != 0;

    bool isTraitMethod = _classDef != null && _classDef.isTrait();

    if (_classDef != null && _classDef.isInterface()) {
      isAbstract = true;
    }

    try {
      _returnsReference = false;

      int token = parseToken();

      string comment = _comment;
      _comment = null;

      if (token == '&')
        _returnsReference = true;
      else
        _peekToken = token;

      StringValue nameV = parseIdentifier();

      if (_classDef == null) {
        nameV = resolveIdentifier(nameV);
      }

      if (isAbstract && ! _scope.isAbstract()) {
        if (_classDef != null) {
          throw error(L.l("'{0}' may not be abstract because class {1} @is not abstract.",
                          nameV, _classDef.getName()));
        }
        else {
          throw error(L.l("'{0}' may not be abstract. Abstract functions are only "
                          + "allowed in abstract classes.",
                          nameV));
        }
      }

      bool isConstructor = false;

      if (_classDef != null
          && (nameV.equalsString(_classDef.getName())
              || nameV.equalsString("__constructor"))) {
        if (isStatic) {
          throw error(L.l("'{0}:{1}' may not be static because class constructors may not be static",
                          _classDef.getName(), nameV));
        }

        isConstructor = true;
      }

      string name = nameV.toString();

      _function = getFactory().createFunctionInfo(_quercus, _classDef, name);
      _function.setPageStatic(oldTop);
      _function.setConstructor(isConstructor);

      _function.setReturnsReference(_returnsReference);

      _function.setStaticClassMethod(_classDef != null && isStatic);

      Location location = getLocation();

      expect('(');

      Arg []args = parseFunctionArgDefinition();

      expect(')');

      if (_classDef != null
          && "__call".equals(name)
          && args.length != 2)
      {
        throw error(L.l("{0}::{1} must have exactly two arguments defined",
                        _classDef.getName(), name));
      }

      Function function;

      if (isAbstract) {
        expect(';');

        function = _factory.createMethodDeclaration(location,
                                                    _classDef, name,
                                                    _function, args);
      }
      else {
        expect('{');

        Statement []statements = null;

        Scope oldScope = _scope;
        try {
          _scope = new FunctionScope(_factory, oldScope);
          statements = parseStatements();
        } finally {
          _scope = oldScope;
        }

        expect('}');

        if (_classDef != null)
          function = _factory.createObjectMethod(location,
                                                 _classDef,
                                                 name, _function,
                                                 args, statements);
        else
          function = _factory.createFunction(location, name,
                                             _function, args,
                                             statements);
      }

      function.setGlobal(oldTop);
      function.setStatic((modifiers & M_STATIC) != 0);
      function.setFinal((modifiers & M_FINAL) != 0);
      function.setTraitMethod(isTraitMethod);

      function.setParseIndex(_functionsParsed++);
      function.setComment(comment);

      if ((modifiers & M_PROTECTED) != 0)
        function.setVisibility(Visibility.PROTECTED);
      else if ((modifiers & M_PRIVATE) != 0)
        function.setVisibility(Visibility.PRIVATE);

      _scope.addFunction(createStringValue(name), function, oldTop);

      /*
    com.caucho.vfs.WriteStream @out = com.caucho.vfs
          .Vfs.lookup("stdout:").openWrite();
    @out.setFlushOnNewline(true);
    function.debug(new JavaWriter(out));
      */

      return function;
    } finally {
      _returnsReference = oldReturnsReference;
      _function = oldFunction;
      _isTop = oldTop;
    }
  }

  /**
   * Parses a function definition
   */
  private Expr parseClosure()
    
  {
    bool oldTop = _isTop;
    _isTop = false;

    bool oldReturnsReference = _returnsReference;
    FunctionInfo oldFunction = _function;

    try {
      _returnsReference = false;

      int token = parseToken();

      string comment = null;

      if (token == '&') {
        _returnsReference = true;
      }
      else {
        _peekToken = token;
      }

      string name = "__quercus_closure_" + _functionsParsed;

      ClassDef classDef = _classDef;
      _function = getFactory().createFunctionInfo(_quercus, classDef, name);
      _function.setReturnsReference(_returnsReference);
      _function.setClosure(true);

      _function.setStaticClassMethod(_classDef != null
                                     && oldFunction != null
                                     && oldFunction.isStaticClassMethod());

      Location location = getLocation();

      expect('(');

      Arg []args = parseFunctionArgDefinition();

      expect(')');

      Arg []useArgs;
      ArrayList<VarExpr> useVars = new ArrayList<VarExpr>();

      token = parseToken();

      if (token == USE) {
        expect('(');

        useArgs = parseFunctionArgDefinition();

        for (Arg arg : useArgs) {
          VarExpr var = _factory.createVar(oldFunction.createVar(arg.getName()));

          useVars.add(var);
        }

        expect(')');
      }
      else {
        useArgs = new Arg[0];

        _peekToken = token;
      }

      expect('{');

      Statement []statements = null;

      Scope oldScope = _scope;
      try {
        _scope = new FunctionScope(_factory, oldScope);
        statements = parseStatements();
      } finally {
        _scope = oldScope;
      }

      expect('}');

      Function function = _factory.createFunction(location, name,
                                                  _function, args,
                                                  statements);

      function.setParseIndex(_functionsParsed++);
      function.setComment(comment);
      function.setClosure(true);
      function.setClosureUseArgs(useArgs);

      _globalScope.addFunction(createStringValue(name), function, oldTop);

      bool isInClassScope = _classDef != null
                               && ! oldFunction.isStaticClassMethod();

      return _factory.createClosure(location, function, useVars, isInClassScope);
    } finally {
      _returnsReference = oldReturnsReference;
      _function = oldFunction;
      _isTop = oldTop;
    }
  }

  private Arg []parseFunctionArgDefinition()
    
  {
    LinkedHashMap<StringValue, Arg> argMap
      = new LinkedHashMap<StringValue, Arg>();

    while (true) {
      int token = parseToken();
      bool isReference = false;

      // php/076b, php/1c02
      // XXX: save arg type for type checking upon function call
      string expectedClass = null;
      if (token != ')'
          && token != '&'
          && token != '$'
          && token != -1) {
        _peekToken = token;

        // php/0m51
        expectedClass = parseNamespaceIdentifier().toString();
        token = parseToken();
      }

      if (token == '&') {
        isReference = true;
        token = parseToken();
      }

      if (token != '$') {
        _peekToken = token;
        break;
      }

      StringValue argName = parseIdentifier();
      Expr defaultExpr = _factory.createRequired();

      token = parseToken();
      if (token == '=') {
        // XXX: actually needs to be primitive
        defaultExpr = parseUnary(); // parseTerm(false);

        token = parseToken();
      }

      Arg arg = new Arg(argName, defaultExpr, isReference, expectedClass);

      if (argMap.get(argName) != null && _quercus.isStrict()) {
        throw error(L.l("aliasing of function argument '{0}'", argName));
      }

      argMap.put(argName, arg);

      VarInfo var = _function.createVar(argName);

      if (token != ',') {
        _peekToken = token;
        break;
      }
    }

    Arg [] args = new Arg[argMap.size()];

    argMap.values().toArray(args);

    return args;
  }

  /**
   * Parses the 'return' statement
   */
  private Statement parseBreak()
    
  {
    // commented @out for adodb (used by Moodle and others)
    // XXX: should only throw fatal error if break statement @is reached
    //      during execution

    if (! _isTop && _loopLabelList.size() == 0 && ! _quercus.isLooseParse())
      throw error(L.l("cannot 'break' inside a function"));

    Location location = getLocation();

    int token = parseToken();

    switch (token) {
    case ';':
      _peekToken = token;

      return _factory.createBreak(location,
                                  null,
                                  (ArrayList<String>) _loopLabelList.clone());

    default:
      _peekToken = token;

      Expr expr = parseTopExpr();

      return _factory.createBreak(location,
                                  expr,
                                  (ArrayList<String>) _loopLabelList.clone());
    }
  }

  /**
   * Parses the 'return' statement
   */
  private Statement parseContinue()
    
  {
    if (! _isTop && _loopLabelList.size() == 0 && ! _quercus.isLooseParse())
      throw error(L.l("cannot 'continue' inside a function"));

    Location location = getLocation();

    int token = parseToken();

    switch (token) {
    case TEXT_PHP:
    case ';':
      _peekToken = token;

      return _factory
          .createContinue(location,
              null,
              (ArrayList<String>) _loopLabelList.clone());

    default:
      _peekToken = token;

      Expr expr = parseTopExpr();

      return _factory
          .createContinue(location,
              expr,
              (ArrayList<String>) _loopLabelList.clone());
    }
  }

  /**
   * Parses the 'return' statement
   */
  private Statement parseReturn()
    
  {
    Location location = getLocation();

    int token = parseToken();

    switch (token) {
    case ';':
      _peekToken = token;

      return _factory.createReturn(location, _factory.createNull());

    default:
      _peekToken = token;

      Expr expr = parseTopExpr();

      /*
      if (_returnsReference)
        expr = expr.createRef();
      else
        expr = expr.createCopy();
      */

      if (_returnsReference)
        return _factory.createReturnRef(location, expr);
      else
        return _factory.createReturn(location, expr);
    }
  }

  /**
   * Parses the 'throw' statement
   */
  private Statement parseThrow()
    
  {
    Location location = getLocation();

    Expr expr = parseExpr();

    return _factory.createThrow(location, expr);
  }

  /**
   * Parses a class definition
   */
  private Statement parseClassDefinition(int modifiers)
    
  {
    StringValue nameV = parseIdentifier();
    nameV = resolveIdentifier(nameV);

    string name = nameV.toString();

    string comment = _comment;

    string parentName = null;

    ArrayList<String> ifaceList = new ArrayList<String>();

    int token = parseToken();
    if (token == : && (modifiers & M_TRAIT) == 0) {
      if ((modifiers & M_INTERFACE) != 0) {
        do {
          ifaceList.add(parseNamespaceIdentifier().toString());

          token = parseToken();
        } while (token == ',');
      }
      else {
        parentName = parseNamespaceIdentifier().toString();

        token = parseToken();
      }
    }

    if (token == IMPLEMENTS
        && (modifiers & M_INTERFACE) == 0 && ((modifiers & M_TRAIT) == 0)) {
      do {
        ifaceList.add(parseNamespaceIdentifier().toString());

        token = parseToken();
      } while (token == ',');
    }

    _peekToken = token;

    InterpretedClassDef oldClass = _classDef;
    Scope oldScope = _scope;

    try {
      _classDef = oldScope.addClass(getLocation(),
                                    name, parentName, ifaceList,
                                    _classesParsed++,
                                    _isTop);

      _classDef.setComment(comment);

      if ((modifiers & M_ABSTRACT) != 0)
        _classDef.setAbstract(true);
      if ((modifiers & M_INTERFACE) != 0)
        _classDef.setInterface(true);
      if ((modifiers & M_FINAL) != 0)
        _classDef.setFinal(true);
      if ((modifiers & M_TRAIT) != 0) {
        _classDef.setTrait(true);
      }

      _scope = new ClassScope(_classDef);

      expect('{');

      parseClassContents();

      expect('}');

      return _factory.createClassDef(getLocation(), _classDef);
    } finally {
      _classDef = oldClass;
      _scope = oldScope;
    }
  }

  /**
   * Parses a statement list.
   */
  private void parseClassContents()
    
  {
    while (true) {
      _comment = null;

      int token = parseToken();

      switch (token) {
        case ';':
          break;

        case FUNCTION:
        {
          Function fun = parseFunctionDefinition(0);
          fun.setStatic(false);
          break;
        }

        case CLASS:
        {
          parseClassDefinition(0);
          break;
        }

        case CONST:
        {
          parseClassConstDefinition();
          break;
        }

        case PUBLIC:
        case PRIVATE:
        case PROTECTED:
        case STATIC:
        case FINAL:
        case ABSTRACT:
        {
          _peekToken = token;
          int modifiers = parseModifiers();

          int token2 = parseToken();

          if (token2 == FUNCTION) {
            Function fun = parseFunctionDefinition(modifiers);
          }
          else {
            _peekToken = token2;

            parseClassVarDefinition(modifiers);
          }
        }
        break;

        case USE:
        {
          parseUseTraitDefinition();

          break;
        }

        case IDENTIFIER:
          if (_lexeme.equalsString("var")) {
            parseClassVarDefinition(0);
          }
          else {
            _peekToken = token;
            return;
          }
          break;

        case -1:
        case '}':
        default:
          _peekToken = token;
          return;
      }
    }
  }

  /**
   * Parses a function definition
   */
  private void parseClassVarDefinition(int modifiers)
    
  {
    int token;

    do {
      expect('$');

      string comment = _comment;

      StringValue name = parseIdentifier();

      token = parseToken();

      Expr expr = null;

      if (token == '=') {
        expr = parseExpr();
      }
      else {
        _peekToken = token;
        expr = _factory.createNull();
      }

      if ((modifiers & M_STATIC) != 0) {
        ((ClassScope) _scope).addStaticClassField(name, expr, _comment);
      }
      else if ((modifiers & M_PRIVATE) != 0) {
        ((ClassScope) _scope).addClassField(name,
                                            expr,
                                            FieldVisibility.PRIVATE,
                                            comment);
      }
      else if ((modifiers & M_PROTECTED) != 0) {
        ((ClassScope) _scope).addClassField(name,
                                            expr,
                                            FieldVisibility.PROTECTED,
                                            comment);
      }
      else {
        ((ClassScope) _scope).addClassField(name,
                                            expr,
                                            FieldVisibility.PUBLIC,
                                            comment);
      }

      token = parseToken();
    } while (token == ',');

    _peekToken = token;
  }

  /**
   * Parses a const definition
   */
  private ArrayList<Statement> parseConstDefinition()
    
  {
    ArrayList<Statement> constList = new ArrayList<Statement>();

    int token;

    do {
      StringValue name = parseNamespaceIdentifier();

      expect('=');

      Expr expr = parseExpr();

      ArrayList<Expr> args = new ArrayList<Expr>();
      args.add(createStringExpr(name));
      args.add(expr);

      Expr fun = _factory.createCall(this, createStringValue("define"), args);

      constList.add(_factory.createExpr(getLocation(), fun));
      // _scope.addConstant(name, expr);

      token = parseToken();
    } while (token == ',');

    _peekToken = token;

    return constList;
  }

  /**
   * Parses a const definition
   */
  private void parseClassConstDefinition()
    
  {
    int token;

    do {
      StringValue name = parseIdentifier();

      expect('=');

      Expr expr = parseExpr();

      ((ClassScope) _scope).addConstant(name, expr);

      token = parseToken();
    } while (token == ',');

    _peekToken = token;
  }

  private void parseUseTraitDefinition()
    
  {
   int token;

   ArrayList<StringValue> traitList = new ArrayList<StringValue>();

    do {
      traitList.add(parseNamespaceIdentifier());

      token = parseToken();
    } while (token == ',');

    for (StringValue traitName : traitList) {
      _classDef.addTrait(traitName.toString());
    }

    if (token == '{') {
      while ((token = parseToken()) >= 0 && token != '}') {
        _peekToken = token;

        StringValue traitNameV = parseNamespaceIdentifier();
        StringValue funName;

        token = parseToken();

        if (token == SCOPE) {
          funName = parseIdentifier();

          token = parseToken();
        }
        else if (traitList.size() == 1) {
          funName = traitNameV;

          traitNameV = traitList.get(0);
        }
        else {
          throw error(L.l("cannot resolve method because multiple traits are defined"));
        }

        if (token == INSTEADOF) {
          string insteadofTraitName = parseNamespaceIdentifier().toString();

          _classDef.addTraitInsteadOf(funName, traitNameV.toString(), insteadofTraitName);
        }
        else if (token == AS) {
          StringValue funNameAlias = parseIdentifier();

          _classDef.addTraitAlias(funName, funNameAlias, traitNameV.toString());
        }
        else {
          throw error(L.l("expected 'insteadof' or 'as' at {0}",
                          tokenName(token)));
        }

        token = parseToken();

        if (token != ';') {
          _peekToken = token;
        }
      }

      _peekToken = token;

      expect('}');
    }
    else {
      _peekToken = token;
    }
  }

  private int parseModifiers()
    
  {
    int token;
    int modifiers = 0;

    while (true) {
      token = parseToken();

      switch (token) {
      case PUBLIC:
        modifiers |= M_PUBLIC;
        break;

      case PRIVATE:
        modifiers |= M_PRIVATE;
        break;

      case PROTECTED:
        modifiers |= M_PROTECTED;
        break;

      case FINAL:
        modifiers |= M_FINAL;
        break;

      case STATIC:
        modifiers |= M_STATIC;
        break;

      case ABSTRACT:
        modifiers |= M_ABSTRACT;
        break;

      default:
        _peekToken = token;
        return modifiers;
      }
    }
  }

  private ArrayList<Statement> parseNamespace()
    
  {
    int token = parseToken();

    StringValue var = _namespace.EMPTY;

    if (token == IDENTIFIER) {
      var = _lexeme;

      token = parseToken();
    }

    if (var.startsWith("\\"))
      var = var.substring(1);

    StringValue oldNamespace = _namespace;

    _namespace = var;

    if (token == '{') {
      ArrayList<Statement> statementList = parseStatementList();

      expect('}');

      _namespace = oldNamespace;

      return statementList;
    }
    else if (token == ';') {
      return new ArrayList<Statement>();
    }
    else {
      throw error(L.l("namespace must be followed by '{' or ';'"));
    }
  }

  private void parseUse()
    
  {
    int token = parseNamespaceIdentifier(read());

    StringValue name = _lexeme;

    int ns = name.lastIndexOf('\\');

    StringValue tail;
    if (ns >= 0)
      tail = name.substring(ns + 1);
    else
      tail = name;

    if (name.startsWith("\\"))
      name = name.substring(1);

    token = parseToken();

    if (token == ';') {
      _namespaceUseMap.put(tail, name);
      _namespaceUseMap.put(tail.toLowerCase(), name);

      return;
    }
    else if (token == AS) {
      do {
        tail = parseIdentifier();

        _namespaceUseMap.put(tail, name);
        _namespaceUseMap.put(tail.toLowerCase(), name);

      } while ((token = parseToken()) == ',');
    }

    _peekToken = token;

    expect(';');
  }

  /**
   * Parses an expression statement.
   */
  private Statement parseExprStatement()
    
  {
    Location location = getLocation();

    Expr expr = parseTopExpr();

    Statement statement = _factory.createExpr(location, expr);

    int token = parseToken();
    _peekToken = token;

    switch (token) {
    case -1:
    case ';':
    case '}':
    case PHP_END:
    case TEXT:
    case TEXT_PHP:
    case TEXT_ECHO:
      break;

    default:
      expect(';');
      break;
    }

    return statement;
  }

  /**
   * Parses a top expression.
   */
  private Expr parseTopExpr()
    
  {
    return parseExpr();
  }

  /**
   * Parses a top expression.
   */
  private Expr parseTopCommaExpr()
    
  {
    return parseCommaExpr();
  }

  /**
   * Parses a comma expression.
   */
  private Expr parseCommaExpr()
    
  {
    Expr expr = parseExpr();

    while (true) {
      int token = parseToken();

      switch (token) {
      case ',':
        expr = _factory.createComma(expr, parseExpr());
        break;
      default:
        _peekToken = token;
        return expr;
      }
    }
  }

  /**
   * Parses an expression with optional '&'.
   */
  private Expr parseRefExpr()
    
  {
    int token = parseToken();

    bool isRef = token == '&';

    if (! isRef)
      _peekToken = token;

    Expr expr = parseExpr();

    if (isRef)
      expr = _factory.createRef(expr);

    return expr;
  }

  /**
   * Parses an expression.
   */
  private Expr parseExpr()
    
  {
    return parseWeakOrExpr();
  }

  /**
   * Parses a logical xor expression.
   */
  private Expr parseWeakOrExpr()
    
  {
    Expr expr = parseWeakXorExpr();

    while (true) {
      int token = parseToken();

      switch (token) {
      case OR_RES:
        expr = _factory.createOr(expr, parseWeakXorExpr());
        break;
      default:
        _peekToken = token;
        return expr;
      }
    }
  }

  /**
   * Parses a logical xor expression.
   */
  private Expr parseWeakXorExpr()
    
  {
    Expr expr = parseWeakAndExpr();

    while (true) {
      int token = parseToken();

      switch (token) {
      case XOR_RES:
        expr = _factory.createXor(expr, parseWeakAndExpr());
        break;
      default:
        _peekToken = token;
        return expr;
      }
    }
  }

  /**
   * Parses a logical and expression.
   */
  private Expr parseWeakAndExpr()
    
  {
    Expr expr = parseConditionalExpr();

    while (true) {
      int token = parseToken();

      switch (token) {
      case AND_RES:
        expr = _factory.createAnd(expr, parseConditionalExpr());
        break;
      default:
        _peekToken = token;
        return expr;
      }
    }
  }

  /**
   * Parses a conditional expression.
   */
  private Expr parseConditionalExpr()
    
  {
    Expr expr = parseOrExpr();

    while (true) {
      int token = parseToken();

      switch (token) {
      case '?':
        token = parseToken();

        if (token == ':') {
          expr = _factory.createShortConditional(expr, parseOrExpr());
        }
        else {
          _peekToken = token;

          Expr trueExpr = parseExpr();
          expect(':');
          // php/33c1
          expr = _factory.createConditional(expr, trueExpr, parseOrExpr());
        }
        break;
      default:
        _peekToken = token;
        return expr;
      }
    }
  }

  /**
   * Parses a logical or expression.
   */
  private Expr parseOrExpr()
    
  {
    Expr expr = parseAndExpr();

    while (true) {
      int token = parseToken();

      switch (token) {
      case C_OR:
        expr = _factory.createOr(expr, parseAndExpr());
        break;
      default:
        _peekToken = token;
        return expr;
      }
    }
  }

  /**
   * Parses a logical and expression.
   */
  private Expr parseAndExpr()
    
  {
    Expr expr = parseBitOrExpr();

    while (true) {
      int token = parseToken();

      switch (token) {
      case C_AND:
        expr = _factory.createAnd(expr, parseBitOrExpr());
        break;
      default:
        _peekToken = token;
        return expr;
      }
    }
  }

  /**
   * Parses a bit or expression.
   */
  private Expr parseBitOrExpr()
    
  {
    Expr expr = parseBitXorExpr();

    while (true) {
      int token = parseToken();

      switch (token) {
      case '|':
        expr = _factory.createBitOr(expr, parseBitXorExpr());
        break;
      default:
        _peekToken = token;
        return expr;
      }
    }
  }

  /**
   * Parses a bit xor expression.
   */
  private Expr parseBitXorExpr()
    
  {
    Expr expr = parseBitAndExpr();

    while (true) {
      int token = parseToken();

      switch (token) {
      case '^':
        expr = _factory.createBitXor(expr, parseBitAndExpr());
        break;
      default:
        _peekToken = token;
        return expr;
      }
    }
  }

  /**
   * Parses a bit and expression.
   */
  private Expr parseBitAndExpr()
    
  {
    Expr expr = parseEqExpr();

    while (true) {
      int token = parseToken();

      switch (token) {
      case '&':
        expr = _factory.createBitAnd(expr, parseEqExpr());
        break;
      default:
        _peekToken = token;
        return expr;
      }
    }
  }

  /**
   * Parses a comparison expression.
   */
  private Expr parseEqExpr()
    
  {
    Expr expr = parseCmpExpr();

    int token = parseToken();

    switch (token) {
    case EQ:
      return _factory.createEq(expr, parseCmpExpr());

    case NEQ:
      return _factory.createNeq(expr, parseCmpExpr());

    case EQUALS:
      return _factory.createEquals(expr, parseCmpExpr());

    case NEQUALS:
      return _factory.createNot(_factory.createEquals(expr, parseCmpExpr()));

    default:
      _peekToken = token;
      return expr;
    }
  }

  /**
   * Parses a comparison expression.
   */
  private Expr parseCmpExpr()
    
  {
    Expr expr = parseShiftExpr();

    int token = parseToken();

    switch (token) {
    case '<':
      return _factory.createLt(expr, parseShiftExpr());

    case '>':
      return _factory.createGt(expr, parseShiftExpr());

    case LEQ:
      return _factory.createLeq(expr, parseShiftExpr());

    case GEQ:
      return _factory.createGeq(expr, parseShiftExpr());

    case INSTANCEOF:
      Location location = getLocation();

      Expr classNameExpr = parseShiftExpr();

      if (classNameExpr instanceof ConstExpr) {
        string className = classNameExpr.evalConstant().toString();

        if (className.equals("self")) {
          className = getSelfClassName();
        }
        else if (className.equals("parent")) {
          className = getParentClassName();
        }

        return _factory.createInstanceOf(expr, className);
      }
      else {
        return _factory.createInstanceOfVar(expr, classNameExpr);
      }

    default:
      _peekToken = token;
      return expr;
    }
  }

  /**
   * Parses a left/right shift expression.
   */
  private Expr parseShiftExpr()
    
  {
    Expr expr = parseAddExpr();

    while (true) {
      int token = parseToken();

      switch (token) {
      case LSHIFT:
        expr = _factory.createLeftShift(expr, parseAddExpr());
        break;
      case RSHIFT:
        expr = _factory.createRightShift(expr, parseAddExpr());
        break;
      default:
        _peekToken = token;
        return expr;
      }
    }
  }

  /**
   * Parses an add/substract expression.
   */
  private Expr parseAddExpr()
    
  {
    Expr expr = parseMulExpr();

    while (true) {
      int token = parseToken();

      switch (token) {
      case '+':
        expr = _factory.createAdd(expr, parseMulExpr());
        break;
      case '-':
        expr = _factory.createSub(expr, parseMulExpr());
        break;
      case '.':
        expr = _factory.createAppend(expr, parseMulExpr());
        break;
      default:
        _peekToken = token;
        return expr;
      }
    }
  }

  /**
   * Parses a multiplication/division expression.
   */
  private Expr parseMulExpr()
    
  {
    Expr expr = parseAssignExpr();

    while (true) {
      int token = parseToken();

      switch (token) {
      case '*':
        expr = _factory.createMul(expr, parseAssignExpr());
        break;
      case '/':
        expr = _factory.createDiv(expr, parseAssignExpr());
        break;
      case '%':
        expr = _factory.createMod(expr, parseAssignExpr());
        break;
      default:
        _peekToken = token;
        return expr;
      }
    }
  }

  /**
   * Parses an assignment expression.
   */
  private Expr parseAssignExpr()
    
  {
    Expr expr = parseUnary();

    while (true) {
      int token = parseToken();

      switch (token) {
      case '=':
        token = parseToken();

        try {
          if (token == '&') {
            // php/03d6
            expr = expr.createAssignRef(this, parseBitOrExpr());
          }
          else {
            _peekToken = token;

            if (_isIfTest && _quercus.isStrict()) {
              throw error(
                  "assignment without parentheses inside If/While/For "
                      + "test statement; please make sure whether equality "
                      + "was intended instead");
            }

            expr = expr.createAssign(this, parseConditionalExpr());
          }
        } catch (QuercusParseException e) {
          throw e;
        } catch (IOException e) {
          throw error(e.getMessage());
        }
        break;

      case PLUS_ASSIGN:
        if (expr.canRead())
          expr = expr.createAssign(this,
                                   _factory.createAdd(expr,
                                                      parseConditionalExpr()));
        else // php/03d4
          expr = expr.createAssign(this, parseConditionalExpr());
        break;

      case MINUS_ASSIGN:
        if (expr.canRead())
          expr = expr.createAssign(this,
                                   _factory.createSub(expr,
                                                      parseConditionalExpr()));
        else
          expr = expr.createAssign(this, parseConditionalExpr());
        break;

      case APPEND_ASSIGN:
        if (expr.canRead())
          expr = expr.createAssign(this,
              _factory.createAppend(expr,
                  parseConditionalExpr()));
        else
          expr = expr.createAssign(this, parseConditionalExpr());
        break;

      case MUL_ASSIGN:
        if (expr.canRead())
          expr = expr.createAssign(this,
                                   _factory.createMul(expr,
                                                      parseConditionalExpr()));
        else
          expr = expr.createAssign(this, parseConditionalExpr());
        break;

      case DIV_ASSIGN:
        if (expr.canRead())
          expr = expr.createAssign(this,
                                   _factory.createDiv(expr,
                                                      parseConditionalExpr()));
        else
          expr = expr.createAssign(this, parseConditionalExpr());
        break;

      case MOD_ASSIGN:
        if (expr.canRead())
          expr = expr.createAssign(this,
                                   _factory.createMod(expr,
                                                      parseConditionalExpr()));
        else
          expr = expr.createAssign(this, parseConditionalExpr());
        break;

      case LSHIFT_ASSIGN:
        if (expr.canRead())
          expr = expr.createAssign(this,
              _factory.createLeftShift(expr,
                  parseConditionalExpr()));
        else
          expr = expr.createAssign(this, parseConditionalExpr());
        break;

      case RSHIFT_ASSIGN:
        if (expr.canRead())
          expr = expr.createAssign(this,
              _factory.createRightShift(expr,
                  parseConditionalExpr()));
        else
          expr = expr.createAssign(this, parseConditionalExpr());
        break;

      case AND_ASSIGN:
        if (expr.canRead())
          expr = expr.createAssign(this,
              _factory.createBitAnd(expr,
                  parseConditionalExpr()));
        else
          expr = expr.createAssign(this, parseConditionalExpr());
        break;

      case OR_ASSIGN:
        if (expr.canRead())
          expr = expr.createAssign(this,
              _factory.createBitOr(expr,
                  parseConditionalExpr()));
        else
          expr = expr.createAssign(this, parseConditionalExpr());
        break;

      case XOR_ASSIGN:
        if (expr.canRead())
          expr = expr.createAssign(this,
              _factory.createBitXor(expr,
                  parseConditionalExpr()));
        else
          expr = expr.createAssign(this, parseConditionalExpr());
        break;

      case INSTANCEOF:
      {
        Expr classNameExpr = parseShiftExpr();

        if (classNameExpr instanceof ConstExpr) {
          string className = classNameExpr.evalConstant().toString();

          if (className.equals("self")) {
            className = getSelfClassName();
          }
          else if (className.equals("parent")) {
            className = getParentClassName();
          }

          return _factory.createInstanceOf(expr, className);
        }
        else {
          return _factory.createInstanceOfVar(expr, classNameExpr);
        }
      }
      default:
        _peekToken = token;
        return expr;
      }
    }
  }


  /**
   * Parses unary term.
   *
   * <pre>
   * unary ::= term
   *       ::= '&' unary
   *       ::= '-' unary
   *       ::= '+' unary
   *       ::= '!' unary
   *       ::= '~' unary
   *       ::= '@' unary
   * </pre>
   */
  private Expr parseUnary()
    
  {
    int token = parseToken();

    switch (token) {

    case '+':
      {
        Expr expr = parseAssignExpr();

        return _factory.createPlus(expr);
      }

    case '-':
      {
        Expr expr = parseAssignExpr();

        return _factory.createMinus(expr);
      }

    case '!':
      {
        Expr expr = parseAssignExpr();

        return _factory.createNot(expr);
      }

    case '~':
      {
        Expr expr = parseAssignExpr();

        return _factory.createBitNot(expr);
      }

    case '@':
      {
        Expr expr = parseAssignExpr();

        return _factory.createSuppress(expr);
      }

    case CLONE:
      {
        Expr expr = parseAssignExpr();

        return _factory.createClone(expr);
      }

    case INCR:
      {
        Expr expr = parseUnary();

        return _factory.createPreIncrement(expr, 1);
      }

    case DECR:
      {
        Expr expr = parseUnary();

        return _factory.createPreIncrement(expr, -1);
      }

    default:
      _peekToken = token;

      return parseTerm(true);
    }
  }


  /**
   * Parses a basic term.
   *
   * <pre>
   * term ::= termBase
   *      ::= term '[' index ']'
   *      ::= term '{' index '}'
   *      ::= term '->' name
   *      ::= term '::' name
   *      ::= term '(' a1, ..., an ')'
   * </pre>
   */
  private Expr parseTerm(bool isParseCall)
    
  {
    Expr term = parseTermBase();

    while (true) {
      int token = parseToken();

      switch (token) {
      case '[':
        {
          token = parseToken();

          if (token == ']') {
            term = _factory.createArrayTail(getLocation(), term);
          }
          else {
            _peekToken = token;
            Expr index = parseExpr();
            token = parseToken();

            term = _factory.createArrayGet(getLocation(), term, index);
          }

          if (token != ']')
            throw expect("']'", token);
        }
        break;

      case '{':
        {
          Expr index = parseExpr();

          expect('}');

          term = _factory.createCharAt(term, index);
        }
        break;

      case INCR:
        term = _factory.createPostIncrement(term, 1);
        break;

      case DECR:
        term = _factory.createPostIncrement(term, -1);
        break;

      case DEREF:
        term = parseDeref(term);
        break;

      case SCOPE:
        term = parseScope(term);
        break;


      case '(':
        _peek = token;

        if (isParseCall)
          term = parseCall(term);
        else
          return term;
        break;

      default:
        _peekToken = token;
        return term;
      }
    }
  }

  /**
   * Parses a basic term.
   *
   * <pre>
   * term ::= termBase
   *      ::= term '[' index ']'
   *      ::= term '{' index '}'
   * </pre>
   */
  private Expr parseTermArray()
    
  {
    Expr term = parseTermBase();

    while (true) {
      int token = parseToken();

      switch (token) {
      case '[':
      {
        token = parseToken();

        if (token == ']') {
          term = _factory.createArrayTail(getLocation(), term);
        }
        else {
          _peekToken = token;
          Expr index = parseExpr();
          token = parseToken();

          term = _factory.createArrayGet(getLocation(), term, index);
        }

          if (token != ']')
            throw expect("']'", token);
        }
        break;

      case '{':
        {
          Expr index = parseExpr();

          expect('}');

          term = _factory.createCharAt(term, index);
        }
        break;

      case INCR:
        term = _factory.createPostIncrement(term, 1);
        break;

      case DECR:
        term = _factory.createPostIncrement(term, -1);
        break;

      default:
        _peekToken = token;
        return term;
      }
    }
  }

  /**
   * Parses a deref
   *
   * <pre>
   * deref ::= term -> IDENTIFIER
   *       ::= term -> IDENTIFIER '(' args ')'
   * </pre>
   */
  private Expr parseDeref(Expr term)
    
  {
    Expr nameExpr = null;

    int token = parseToken();

    if (token == '$') {
      // php/09e0
      _peekToken = token;

      //nameExpr = parseTerm(false);
      nameExpr = parseTermArray(); // php/098e, php/398e

      return term.createFieldGet(_factory, getLocation(), nameExpr);
    }
    else if (token == '{') {
      nameExpr = parseExpr();
      expect('}');

      return term.createFieldGet(_factory, getLocation(), nameExpr);
    }
    else {
      _peekToken = token;
      StringValue name = parseIdentifier();

      return term.createFieldGet(_factory, getLocation(), name);
    }
  }

  /**
   * Parses a basic term.
   *
   * <pre>
   * term ::= STRING
   *      ::= LONG
   *      ::= DOUBLE
   * </pre>
   */
  private Expr parseTermBase()
    
  {
    int token = parseToken();

    switch (token) {
    case STRING:
      return createStringExpr(_lexeme);

    case SYSTEM_STRING:
      {
        ArrayList<Expr> args = new ArrayList<Expr>();
        args.add(createStringExpr(_lexeme));
        return _factory.createCall(this, createStringValue("shell_exec"), args);
      }

    case SIMPLE_SYSTEM_STRING:
      {
        ArrayList<Expr> args = new ArrayList<Expr>();
        args.add(parseEscapedString(_lexeme, SIMPLE_STRING_ESCAPE, true));
        return _factory.createCall(this, createStringValue("shell_exec"), args);
      }

    case COMPLEX_SYSTEM_STRING:
      {
        ArrayList<Expr> args = new ArrayList<Expr>();
        args.add(parseEscapedString(_lexeme, COMPLEX_STRING_ESCAPE, true));
        return _factory.createCall(this, createStringValue("shell_exec"), args);
      }

    case SIMPLE_STRING_ESCAPE:
    case COMPLEX_STRING_ESCAPE:
      return parseEscapedString(_lexeme, token, false);

    case BINARY:
      try {
        if (isUnicodeSemantics()) {          
          BinaryBuilderValue sb = new BinaryBuilderValue();
          
          sb.append(_lexeme);

          return _factory.createBinary(sb);
        }
        else {
          return _factory.createString(_lexeme);
        }
      }
      catch (Exception e) {
        throw new QuercusParseException(e);
      }

    case SIMPLE_BINARY_ESCAPE:
    case COMPLEX_BINARY_ESCAPE:
      return parseEscapedString(_lexeme, token, false, false);

    case LONG:
    {
      long value = 0;
      double doubleValue = 0;
      long sign = 1;
      bool isOverflow = false;

      char ch = _lexeme.charAt(0);

      int i = 0;
      if (ch == '+') {
        i++;
      } else if (ch == '-') {
        sign = -1;
        i++;
      }

      int len = _lexeme.length();
      for (; i < len; i++) {
        int digit = _lexeme.charAt(i) - '0';
        long oldValue = value;

        value = value * 10 + digit;
        doubleValue = doubleValue * 10 + digit;

        if (value < oldValue)
          isOverflow = true;
      }

      if (! isOverflow)
        return _factory.createLiteral(LongValue.create(value * sign));
      else
        return _factory.createLiteral(new DoubleValue(doubleValue * sign));
    }
    case DOUBLE:
    {
      double d = Double.parseDouble(_lexeme.toString());

      return _factory.createLiteral(new DoubleValue(d));
    }

    case NULL:
      return _factory.createNull();

    case TRUE:
      return _factory.createLiteral(BooleanValue.TRUE);

    case FALSE:
      return _factory.createLiteral(BooleanValue.FALSE);

    case '$':
      return parseVariable();

    case NEW:
      return parseNew();

    case FUNCTION:
      return parseClosure();

    case INCLUDE:
      return _factory.createInclude(getLocation(), _sourceFile, parseExpr());
    case REQUIRE:
      return _factory.createRequire(getLocation(), _sourceFile, parseExpr());
    case INCLUDE_ONCE:
      return _factory.createIncludeOnce(getLocation(),
          _sourceFile, parseExpr());
    case REQUIRE_ONCE:
      return _factory.createRequireOnce(getLocation(),
          _sourceFile, parseExpr());

    case LIST:
      return parseList();

    case PRINT:
      return parsePrintExpr();

    case EXIT:
      return parseExit();

    case DIE:
      return parseDie();

    case EMPTY:
      return parseEmpty();

    case IDENTIFIER:
    case NAMESPACE:
      {
        if (_lexeme.equalsString("new"))
          return parseNew();

        StringValue name = _lexeme;

        token = parseToken();
        _peekToken = token;

        if (token == '(' && ! _isNewExpr) {
          // shortcut for common case of static function

          return parseCall(name);
        }
        else
          return parseConstant(name);
      }
    case STATIC:
      {
        if (_classDef == null) {
          throw error(L.l("cannot use new {0}() outside of a class context.",
                          tokenName(token)));
        }

        return parseConstant(_lexeme);
      }

    case '(':
      {
        _isIfTest = false;

        Expr expr = parseExpr();

        expect(')');

        if (expr instanceof ConstExpr) {
          string type = ((ConstExpr) expr).getVar();

          int ns = type.lastIndexOf('\\');
          if (ns >= 0)
            type = type.substring(ns + 1);

          if ("bool".equalsIgnoreCase(type)
              || "boolean".equalsIgnoreCase(type))
            return _factory.createToBoolean(parseAssignExpr());
          else if ("int".equalsIgnoreCase(type)
                   || "integer".equalsIgnoreCase(type))
            return _factory.createToLong(parseAssignExpr());
          else if ("float".equalsIgnoreCase(type)
                   || "double".equalsIgnoreCase(type)
                   || "real".equalsIgnoreCase(type))
            return _factory.createToDouble(parseAssignExpr());
          else if ("string".equalsIgnoreCase(type))
            return _factory.createToString(parseAssignExpr());
          else if ("binary".equalsIgnoreCase(type))
            return _factory.createToBinary(parseAssignExpr());
          else if ("unicode".equalsIgnoreCase(type))
            return _factory.createToUnicode(parseAssignExpr());
          else if ("object".equalsIgnoreCase(type))
            return _factory.createToObject(parseAssignExpr());
          else if ("array".equalsIgnoreCase(type))
            return _factory.createToArray(parseAssignExpr());
        }

        return expr;
      }
    case '[':
      _peekToken = token;

      return parseArrayFunction('[', ']');

    case IMPORT:
      {
        StringValue importTokenString = _lexeme;

        token = parseToken();

        if (token == '(') {
          _peekToken = token;

          return parseCall(importTokenString);
        }
        else {
          _peekToken = token;

          return parseImport();
        }
      }

    default:
      throw error(L.l("{0} @is an unexpected token, expected an expression.",
                      tokenName(token)));
    }
  }

  /**
   * Parses a basic term.
   *
   * <pre>
   * lhs ::= VARIABLE
   *     ::= lhs '[' expr ']'
   *     ::= lhs -> FIELD
   * </pre>
   */
  private AbstractVarExpr parseLeftHandSide()
    
  {
    int token = parseToken();
    AbstractVarExpr lhs = null;

    if (token == '$')
      lhs = parseVariable();
    else
      throw error(L.l("expected variable at {0} as left-hand-side",
                      tokenName(token)));

    while (true) {
      token = parseToken();

      switch (token) {
      case '[':
        {
          token = parseToken();

          if (token == ']') {
            lhs = _factory.createArrayTail(getLocation(), lhs);
          }
          else {
            _peekToken = token;
            Expr index = parseExpr();
            token = parseToken();

            lhs = _factory.createArrayGet(getLocation(), lhs, index);
          }

          if (token != ']')
            throw expect("']'", token);
        }
        break;

      case '{':
        {
          Expr index = parseExpr();

          expect('}');

          lhs = _factory.createCharAt(lhs, index);
        }
        break;

      case DEREF:
        lhs = (AbstractVarExpr) parseDeref(lhs);
        break;

      default:
        _peekToken = token;
        return lhs;
      }
    }
  }

  private Expr parseScope(Expr classNameExpr)
    
  {
    int token = parseToken();

    if (isIdentifier(token)) {
      return classNameExpr.createClassConst(this, _lexeme);
    }
    else if (token == '$') {
      token = parseToken();

      if (isIdentifier(token)) {
        return classNameExpr.createClassField(this, _lexeme);
      }
      else if (token == '{') {
        Expr expr = parseExpr();

        expect('}');

        return classNameExpr.createClassField(this, expr);
      }
      else {
        _peekToken = token;

        return classNameExpr.createClassField(this, parseTermBase());
      }
    }
    else if (token == '{') {
      Expr expr = parseExpr();

      expect('}');

      Expr name = classNameExpr.createClassConst(this, expr);

      return parseCall(name);
    }

    throw error(L.l("unexpected token '{0}' in class scope expression",
                    tokenName(token)));
  }

  private bool isIdentifier(int token)
  {
    return token == IDENTIFIER || FIRST_IDENTIFIER_LEXEME <= token;
  }

  /**
   * Parses the next variable
   */
  private AbstractVarExpr parseVariable()
    
  {
    int token = parseToken();

    if (token == THIS) {
      return _factory.createThis(_classDef);
    }
    else if (token == '$') {
      _peekToken = token;

      // php/0d6c, php/0d6f
      return _factory.createVarVar(parseTermArray());
    }
    else if (token == '{') {
      AbstractVarExpr expr = _factory.createVarVar(parseExpr());

      expect('}');

      return expr;
    }
    else if (_lexeme.length() == 0)
      throw error(L.l("Expected identifier at '{0}'", tokenName(token)));

    if (_lexeme.indexOf('\\') >= 0) {
      throw error(L.l("Namespace @is not allowed for variable ${0}", _lexeme));
    }

    return _factory.createVar(_function.createVar(_lexeme));
  }

  public Expr createVar(StringValue name)
  {
    return _factory.createVar(_function.createVar(name));
  }

  /**
   * Parses the next function
   */
  private Expr parseCall(StringValue name)
    
  {
    if (name.equalsStringIgnoreCase("array")) {
      return parseArrayFunction('(', ')');
    }

    ArrayList<Expr> args = parseArgs();

    name = resolveIdentifier(name);

    if (! _quercus.isStrict()) {
      name = name.toLowerCase(Locale.ENGLISH);
    }

    return _factory.createCall(this, name, args);

    /*
     if (name.equals("each")) {
        if (args.size() != 1)
          throw error(L.l("each requires a single expression"));

        // php/1721
        // we should let ArrayModule.each() handle it
        //return _factory.createEach(args.get(0));
      }
      */
  }

  /**
   * Parses the next constant
   */
  private Expr parseConstant(StringValue name)
  {
    if (name.equalsString("__FILE__")) {
      return _factory.createFileNameExpr(_parserLocation.getFileName());
    }
    else if (name.equalsString("__DIR__")) {
      Path parent = Vfs.lookup(_parserLocation.getFileName()).getParent();

      return _factory.createDirExpr(parent.getNativePath());
    }
    else if (name.equalsString("__LINE__"))
      return _factory.createLong(_parserLocation.getLineNumber());
    else if (name.equalsString("__CLASS__") && _classDef != null) {
      if (_classDef.isTrait()) {
        StringValue funNameV = createStringValue(_function.getName());

        return _factory.createClassExpr(getLocation(), funNameV);
      }
      else {
        return createStringExpr(_classDef.getName());
      }
    }
    else if (name.equalsString("__FUNCTION__")) {
      return createStringExpr(_function.getName());
    }
    else if (name.equalsString("__METHOD__")) {
      if (_classDef != null) {
        if (_function.getName().length() != 0)
          return createStringExpr(_classDef.getName() + "::" + _function.getName());
        else
          return createStringExpr(_classDef.getName());
      }
      else
        return createStringExpr(_function.getName());
    }
    else if (name.equalsString("__NAMESPACE__")) {
      return createStringExpr(_namespace);
    }
    else if (name.equalsString("self") && _classDef != null) {
      // php/0m28
      return _factory.createConst(_classDef.getName());
    }

    name = resolveIdentifier(name);

    if (name.startsWith("\\"))
      name = name.substring(1);

    return _factory.createConst(name.toString());
  }

  /**
   * Parses the next function
   */
  private Expr parseCall(Expr name)
    
  {
    return name.createCall(this, getLocation(), parseArgs());
  }

  private ArrayList<Expr> parseArgs()
    
  {
    expect('(');

    ArrayList<Expr> args = new ArrayList<Expr>();

    int token;

    while ((token = parseToken()) > 0 && token != ')') {
      bool isRef = false;

      if (token == '&')
        isRef = true;
      else
        _peekToken = token;

      Expr expr = parseExpr();

      if (isRef)
        expr = expr.createRef(this);

      args.add(expr);

      token = parseToken();
      if (token == ')')
        break;
      else if (token != ',')
        throw expect("','", token);
    }

    return args;
  }

  public string getSelfClassName()
  {
    if (_classDef == null)
      throw error(L.l("'self' @is not valid because there @is no active class."));

    return _classDef.getName();
  }

  public InterpretedClassDef getClassDef() {
    return _classDef;
  }

  public string getParentClassName()
  {
    if (_classDef == null)
      throw error(L.l(
          "'parent' @is not valid because there @is no active class."));

    return _classDef.getParentName();
  }

  /**
   * Parses the new expression
   */
  private Expr parseNew()
    
  {
    string name = null;
    Expr nameExpr = null;

    bool isNewExpr = _isNewExpr;
    _isNewExpr = true;

    //nameExpr = parseTermBase();
    nameExpr = parseTerm(false);

    _isNewExpr = isNewExpr;

    // XX: unicode issues?
    if (nameExpr.isLiteral() || nameExpr instanceof ConstExpr) {
      name = nameExpr.evalConstant().toString();

      // php/0957
      if ("self".equals(name) && _classDef != null) {
        name = _classDef.getName();
      }
      else if ("parent".equals(name) && getParentClassName() != null) {
        name = getParentClassName().toString();
      }
      else {
        // name = resolveIdentifier(name);
      }
    }

    int token = parseToken();

    ArrayList<Expr> args = new ArrayList<Expr>();

    if (token != '(')
      _peekToken = token;
    else {
      while ((token = parseToken()) > 0 && token != ')') {
        _peekToken = token;

        args.add(parseExpr());

        token = parseToken();
        if (token == ')')
          break;
        else if (token != ',')
          throw error(L.l("expected ','"));
      }
    }

    Expr expr;

    if (name != null) {
      if (name.equals("static") || name.endsWith("\\static")) {
        // php/093q
        // php/0m27
        expr = _factory.createNewStatic(getLocation(), args);
      }
      else {
        expr = _factory.createNew(getLocation(), name, args);
      }
    }
    else {
      expr = _factory.createVarNew(getLocation(), nameExpr, args);
    }

    return expr;
  }

  /**
   * Parses the include expression
   */
  private Expr parseInclude()
    
  {
    Expr name = parseExpr();

    return _factory.createInclude(getLocation(), _sourceFile, name);
  }

  /**
   * Parses the list(...) = value expression
   */
  private Expr parseList()
    
  {
    ListHeadExpr leftVars = parseListHead();

    expect('=');

    Expr value = parseConditionalExpr();

    return _factory.createList(this, leftVars, value);
  }

  /**
   * Parses the list(...) expression
   */
  private ListHeadExpr parseListHead()
    
  {
    expect('(');

    int peek = parseToken();

    ArrayList<Expr> leftVars = new ArrayList<Expr>();

    while (peek > 0 && peek != ')') {
      if (peek == LIST) {
        leftVars.add(parseListHead());

        peek = parseToken();
      }
      else if (peek != ',') {
        _peekToken = peek;

        Expr left = parseTerm(true);

        leftVars.add(left);

        left.assign(this);

        peek = parseToken();
      }
      else {
        leftVars.add(null);
      }

      if (peek == ',')
        peek = parseToken();
      else
        break;
    }

    if (peek != ')')
      throw error(L.l("expected ')'"));

    return _factory.createListHead(leftVars);
  }

  /**
   * Parses the exit/die expression
   */
  private Expr parseExit()
    
  {
    int token = parseToken();
    _peekToken = token;

    if (token == '(') {
      ArrayList<Expr> args = parseArgs();

      if (args.size() > 0)
        return _factory.createExit(args.get(0));
      else
        return _factory.createExit(null);
    }
    else {
      return _factory.createExit(null);
    }
  }

  /**
   * Parses the exit/die expression
   */
  private Expr parseDie()
    
  {
    int token = parseToken();
    _peekToken = token;

    if (token == '(') {
      ArrayList<Expr> args = parseArgs();

      if (args.size() > 0)
        return _factory.createDie(args.get(0));
      else
        return _factory.createDie(null);
    }
    else {
      return _factory.createDie(null);
    }
  }

  /**
   * Parses the empty() expression.
   */
  private Expr parseEmpty()
    
  {
    int token = parseToken();
    _peekToken = token;

    if (token == '(') {
      ArrayList<Expr> args = parseArgs();

      if (args.size() > 0)
        return _factory.createEmpty(getLocation(), args.get(0));
      else
        throw error(L.l("empty must have one arg"));
    }
    else {
      throw error(L.l("expected '('"));
    }
  }

  /**
   * Parses the array() expression
   */
  private Expr parseArrayFunction(char beginChar, char endChar)
    
  {
    int token = parseToken();

    if (token != beginChar)
      throw error(L.l("Expected {0}", String.valueOf(beginChar)));

    ArrayList<Expr> keys = new ArrayList<Expr>();
    ArrayList<Expr> values = new ArrayList<Expr>();

    while ((token = parseToken()) > 0 && token != endChar) {
      _peekToken = token;

      Expr value = parseRefExpr();

      token = parseToken();

      if (token == ARRAY_RIGHT) {
        Expr key = value;

        value = parseRefExpr();

        keys.add(key);
        values.add(value);

        token = parseToken();
      }
      else {
        keys.add(null);
        values.add(value);
      }

      if (token == endChar) {
        break;
      }
      else if (token != ',') {
        throw error(L.l("expected ','"));
      }
    }

    return _factory.createArrayFun(keys, values);
  }

  /**
   * Parses a Quercus import.
   */
  private Expr parseImport()
    
  {
    bool isWildcard = false;
    bool isIdentifierStart = true;

    StringBuilder sb = new StringBuilder();

    while (true) {
      int token = parseToken();

      if (token == IDENTIFIER) {
        sb.append(_lexeme);

        token = parseToken();

        if (token == '.') {
          sb.append('.');
        }
        else {
          _peekToken = token;
          break;
        }
      }
      else if (token == '*') {
        if (sb.length() > 0)
          sb.setLength(sb.length() - 1);

        isWildcard = true;
        break;
      }
      else {
        throw error(L.l("'{0}' @is an unexpected token in import",
                        tokenName(token)));
      }
    }

    //expect(';');

    return _factory.createImport(getLocation(), sb.toString(), isWildcard);
  }

  /**
   * Parses the next token.
   */
  private int parseToken()
    
  {
    int peekToken = _peekToken;
    if (peekToken > 0) {
      _peekToken = 0;
      return peekToken;
    }

    while (true) {
      int ch = read();

      switch (ch) {
      case -1:
        return -1;

      case ' ': case '\t': case '\n': case '\r':
        break;

      case '#':
        while ((ch = read()) != '\n' && ch != '\r' && ch >= 0) {
          if (ch != '?') {
          }
          else if ((ch = read()) != '>') {
            _peek = ch;
          }
          else {
            ch = read();
            if (ch == '\r')
              ch = read();
            if (ch != '\n')
              _peek = ch;

            return parsePhpText();
          }
        }
        break;

      case '"':
      {
        string heredocEnd = _heredocEnd;
        _heredocEnd = null;

        int result = parseEscapedString('"');
        _heredocEnd = heredocEnd;

        return result;
      }
      case '`':
        {
          int token = parseEscapedString('`');

          switch (token) {
          case STRING:
            return SYSTEM_STRING;
          case SIMPLE_STRING_ESCAPE:
            return SIMPLE_SYSTEM_STRING;
          case COMPLEX_STRING_ESCAPE:
            return COMPLEX_SYSTEM_STRING;
          default:
            throw new IllegalStateException();
          }
        }

      case '\'':
        parseStringToken('\'');
        return STRING;

      case ';': case '$': case '(': case ')': case '@':
      case '[': case ']': case ',': case '{': case '}':
      case '~':
        return ch;

      case '+':
        ch = read();
        if (ch == '=')
          return PLUS_ASSIGN;
        else if (ch == '+')
          return INCR;
        else
          _peek = ch;

        return '+';

      case '-':
        ch = read();
        if (ch == '>')
          return DEREF;
        else if (ch == '=')
          return MINUS_ASSIGN;
        else if (ch == '-')
          return DECR;
        else
          _peek = ch;

        return '-';

      case '*':
        ch = read();
        if (ch == '=')
          return MUL_ASSIGN;
        else
          _peek = ch;

        return '*';

      case '/':
        ch = read();
        if (ch == '=')
          return DIV_ASSIGN;
        else if (ch == '/') {
          while (ch >= 0) {
            if (ch == '\n' || ch == '\r') {
              break;
            }
            else if (ch == '?') {
              ch = read();

              if (ch == '>') {
                ch = read();

                if (ch == '\r')
                  ch = read();
                if (ch != '\n')
                  _peek = ch;

                return parsePhpText();
              }
            }
            else
              ch = read();
          }
          break;
        }
        else if (ch == '*') {
          parseMultilineComment();
          break;
        }
        else
          _peek = ch;

        return '/';

      case '%':
        ch = read();
        if (ch == '=')
          return MOD_ASSIGN;
        else if (ch == '>') {
          ch = read();
          if (ch == '\r')
            ch = read();
          if (ch != '\n')
            _peek = ch;

          return parsePhpText();
        }
        else
          _peek = ch;

        return '%';

      case ':':
        ch = read();
        if (ch == ':')
          return SCOPE;
        else
          _peek = ch;

        return ':';

      case '=':
        ch = read();
        if (ch == '=') {
          ch = read();
          if (ch == '=')
            return EQUALS;
          else {
            _peek = ch;
            return EQ;
          }
        }
        else if (ch == '>')
          return ARRAY_RIGHT;
        else {
          _peek = ch;
          return '=';
        }

      case '!':
        ch = read();
        if (ch == '=') {
          ch = read();
          if (ch == '=')
            return NEQUALS;
          else {
            _peek = ch;
            return NEQ;
          }
        }
        else {
          _peek = ch;
          return '!';
        }

      case '&':
        ch = read();
        if (ch == '&')
          return C_AND;
        else if (ch == '=')
          return AND_ASSIGN;
        else {
          _peek = ch;
          return '&';
        }

      case '^':
        ch = read();
        if (ch == '=')
          return XOR_ASSIGN;
        else
          _peek = ch;

        return '^';

      case '|':
        ch = read();
        if (ch == '|')
          return C_OR;
        else if (ch == '=')
          return OR_ASSIGN;
        else {
          _peek = ch;
          return '|';
        }

      case '<':
        ch = read();
        if (ch == '<') {
          ch = read();

          if (ch == '=')
            return LSHIFT_ASSIGN;
          else if (ch == '<') {
            ch = read();

            if (ch == '\'') {
              return parseNowdoc();
            }
            else if (ch == '"') {
              return parseHeredocToken(true);
            }
            else {
              _peek = ch;
              return parseHeredocToken(false);
            }
          }
          else {
            _peek = ch;
          }

          return LSHIFT;
        }
        else if (ch == '=')
          return LEQ;
        else if (ch == '>')
          return NEQ;
        else if (ch == '/') {
          StringValue sb = createStringBuilder();

          if (! parseTextMatch(sb, "script"))
            throw error(L.l("expected 'script' at '{0}'", sb));

          expect('>');

          return parsePhpText();
        }
        else
          _peek = ch;

        return '<';

      case '>':
        ch = read();
        if (ch == '>') {
          ch = read();

          if (ch == '=')
            return RSHIFT_ASSIGN;
          else
            _peek = ch;

          return RSHIFT;
        }
        else if (ch == '=')
          return GEQ;
        else
          _peek = ch;

        return '>';

      case '?':
        ch = read();
        if (ch == '>') {
          ch = read();
          if (ch == '\r')
            ch = read();
          if (ch != '\n')
            _peek = ch;

          return parsePhpText();
        }
        else
          _peek = ch;

        return '?';

      case '.':
        ch = read();

        if (ch == '=')
          return APPEND_ASSIGN;

        _peek = ch;

        if ('0' <= ch && ch <= '9')
          return parseNumberToken('.');
        else
          return '.';

      case '0': case '1': case '2': case '3': case '4':
      case '5': case '6': case '7': case '8': case '9':
        return parseNumberToken(ch);

      default:

        if (ch == 'b') {
          int ch2 = read();

          if (ch2 == '\'') {
            parseStringToken('\'', false);
            return BINARY;
          }
          else if (ch2 == '"') {

            int token = parseEscapedString('"', false);
            switch (token) {
              case STRING:
                return BINARY;
              case SIMPLE_STRING_ESCAPE:
                return SIMPLE_BINARY_ESCAPE;
              case COMPLEX_STRING_ESCAPE:
                return COMPLEX_BINARY_ESCAPE;
              default:
                return token;
            }
          }
          else
            _peek = ch2;
        }

        return parseNamespaceIdentifier(ch);
      }
    }
  }

  private StringValue parseIdentifier()
    
  {
    int token = _peekToken;
    _peekToken = -1;

    if (token <= 0)
      token = parseIdentifier(read());

    if (token != IDENTIFIER && token < FIRST_IDENTIFIER_LEXEME)
      throw error(L.l("expected identifier at {0}.", tokenName(token)));

    if (_lexeme.indexOf('\\') >= 0) {
      throw error(L.l("namespace identifier @is not allowed at '{0}'",
                      _lexeme));
    }
    else if (_peek == '\\') {
      throw error(L.l("namespace identifier @is not allowed at '{0}\\'",
                      _lexeme));
    }

    return _lexeme;
  }


  private StringValue parseNamespaceIdentifier()
    
  {
    int token = _peekToken;
    _peekToken = -1;

    if (token <= 0)
      token = parseNamespaceIdentifier(read());

    if (token == IDENTIFIER)
      return resolveIdentifier(_lexeme);
    else if (FIRST_IDENTIFIER_LEXEME <= token)
      return resolveIdentifier(_lexeme);
    else
      throw error(L.l("expected identifier at {0}.", tokenName(token)));
  }

  public StringValue getSystemFunctionName(StringValue name)
  {
    int p = name.lastIndexOf('\\');

    if (p < 0)
      return name;

    StringValue systemName = name.substring(p + 1);

    if (_quercus.findFunction(systemName) != null) {
      return systemName;
    }
    else {
      return null;
    }
  }
  private StringValue resolveIdentifier(StringValue id)
  {
    if (id.startsWith("\\")) {
      return id.substring(1);
    }

    int ns = id.indexOf('\\');

    if (ns > 0) {
      StringValue prefix = id.substring(0, ns);
      StringValue use = null;

      if (_namespaceUseMap.size() > 0) {
        use = _namespaceUseMap.get(prefix);

        if (use == null) {
          use = _namespaceUseMap.get(prefix.toLowerCase());
        }
      }

      if (use != null) {
        return createStringBuilder().append(use).append(id.substring(ns));
      }
      else if (_namespace.length() == 0) {
        return id;
      }
      else {
        return createStringBuilder().append(_namespace).append("\\").append(id);
      }
    }
    else {
      StringValue use = null;

      if (_namespaceUseMap.size() > 0) {
        use = _namespaceUseMap.get(id);

        if (use == null) {
          use = _namespaceUseMap.get(id.toLowerCase());
        }
      }

      if (use != null) {
        return use;
      }
      else if (_namespace.length() == 0) {
        return id;
      }
      else {
        return createStringBuilder().append(_namespace).append('\\').append(id);
      }
    }
  }

  private int parseIdentifier(int ch)
    
  {
    for (; Character.isWhitespace(ch); ch = read()) {
    }

    if (isIdentifierStart(ch)) {
      _sb.setLength(0);
      _sb.append((char) ch);

      for (ch = read(); isIdentifierPart(ch); ch = read()) {
        _sb.append((char) ch);
      }

      _peek = ch;

      return lexemeToToken();
    }

    throw error("expected identifier at " + (char) ch);
  }

  private int parseNamespaceIdentifier(int ch)
    
  {
    for (; Character.isWhitespace(ch); ch = read()) {
    }

    if (isNamespaceIdentifierStart(ch)) {
      _sb.setLength(0);
      _sb.append((char) ch);

      for (ch = read(); isNamespaceIdentifierPart(ch); ch = read()) {
        _sb.append((char) ch);
      }

      _peek = ch;

      return lexemeToToken();
    }

    throw error("unknown lexeme:" + (char) ch);
  }

  private int lexemeToToken()
    
  {
    _lexeme = copyStringValue(_sb);

    // the 'static' reserved keyword vs late static binding (static::$a)
    if (_peek == ':' && _lexeme.equalsString("static")) {
      return IDENTIFIER;
    }

    string name = _lexeme.toString();

    int reserved = _reserved.get(name);

    if (reserved > 0) {
      return reserved;
    }

    reserved = _insensitiveReserved.get(name.toLowerCase(Locale.ENGLISH));
    if (reserved > 0) {
      return reserved;
    }
    else {
      return IDENTIFIER;
    }
  }

  /**
   * Parses a multiline comment.
   */
  private void parseMultilineComment()
    
  {
    int ch = read();

    if (ch == '*') {
      _sb.setLength(0);
      _sb.append('/');
      _sb.append('*');

      do {
        if (ch != '*') {
          _sb.append((char) ch);
        }
        else if ((ch = read()) == '/') {
          _sb.append('*');
          _sb.append('/');

          _comment = _sb.toString();

          return;
        }
        else {
          _sb.append('*');
          _peek = ch;
        }
      } while ((ch = read()) >= 0);

      _comment = _sb.toString();
    }
    else if (ch >= 0) {
      do {
        if (ch != '*') {
        }
        else if ((ch = read()) == '/')
          return;
        else
          _peek = ch;
      } while ((ch = read()) >= 0);
    }
  }

  /**
   * Parses quercus text
   */
  private int parsePhpText()
    
  {
    StringValue sb = createStringBuilder();

    int ch = read();
    while (ch > 0) {
      if (ch == '<') {
        int ch2;
        int ch3;

        if ((ch = read()) == 's' || ch == 'S') {
          _peek = ch;
          if (parseScriptBegin(sb)) {
            _lexeme = sb;
            return TEXT;
          }
          ch = read();
        }
        else if (ch == '%') {
          if ((ch = read()) == '=') {
            _lexeme = sb;
            return TEXT_ECHO;
          }
          else if (Character.isWhitespace(ch)) {
            _lexeme = sb;

            return TEXT;
          }
        }
        else if (ch != '?') {
          sb.append('<');
        }
        else if ((ch = read()) == '=') {
          _lexeme = sb;

          return TEXT_ECHO;
        }
        else if (ch != 'p' && ch != 'P' && ! isShortOpenTag()) {
          sb.append('<');
          sb.append('?');

          sb.append((char) ch);

          ch = read();
        }
        else {
          _lexeme = sb;
          _peek = ch;

          if (ch == 'p' || ch == 'P')
            return TEXT_PHP;
          else
            return TEXT;
        }
      }
      else {
        sb.append((char) ch);

        ch = read();
      }
    }

    _lexeme = sb;

    return TEXT;
  }

  /**
   * Parses the <script language="quercus"> opening
   */
  private bool parseScriptBegin(StringValue sb)
    
  {
    int begin = sb.length();

    sb.append('<');

    if (! parseTextMatch(sb, "script"))
      return false;

    parseWhitespace(sb);

    if (! parseTextMatch(sb, "language="))
      return false;

    int openingParentheses = read();

    if(openingParentheses == '\'' || openingParentheses == '"'){
      if (! parseTextMatch(sb, "php")){
        sb.append((char) openingParentheses);
        return false;
      }

      int closingParentheses = read();
      if(openingParentheses != closingParentheses){
        sb.append((char) closingParentheses);
        return false;
      }
    }


    parseWhitespace(sb);

    int ch = read();

    if (ch == '>') {
      sb.setLength(begin);
      return true;
    }
    else {
      _peek = ch;
      return false;
    }
  }

  private bool parseTextMatch(StringValue sb, string text)
    
  {
    int len = text.length();

    for (int i = 0; i < len; i++) {
      int ch = read();

      if (ch < 0)
        return false;

      if (Character.toLowerCase(ch) != text.charAt(i)) {
        _peek = ch;
        return false;
      }
      else
        sb.append((char) ch);
    }

    return true;
  }

  private void parseWhitespace(StringValue sb)
    
  {
    int ch;

    while (Character.isWhitespace((ch = read()))) {
      sb.append((char) ch);
    }

    _peek = ch;
  }

  private void parseStringToken(int end)
    
  {
    parseStringToken(end, isUnicodeSemantics());
  }

  /**
   * Parses the next string token.
   */
  private void parseStringToken(int end, bool isUnicode)
    
  {
    _sb.setLength(0);

    int ch;

    for (ch = read(); ch >= 0 && ch != end; ch = read()) {
      if (ch == '\\') {
        ch = read();

        if (isUnicode) {
          if (ch == 'u') {
            int value = parseUnicodeEscape(false);

            if (value < 0) {
              _sb.append('\\');
              _sb.append('u');
            }
            else
              _sb.append((char) value); // Character.toChars(value));

            continue;
          }
          else if (ch == 'U') {
            int value = parseUnicodeEscape(true);

            if (value < 0) {
              _sb.append('\\');
              _sb.append('U');
            }
            else
              _sb.append((char) value); // Character.toChars(value));

            continue;
          }
        }

        if (end == '"') {
          _sb.append('\\');

          if (ch >= 0)
            _sb.append((char) ch);
        }
        else {
          switch (ch) {
          case '\'': case '\\':
            _sb.append((char) ch);
            break;
          default:
            _sb.append('\\');
            _sb.append((char) ch);
            break;
          }
        }
      }
      else
        _sb.append((char) ch);
    }

    _lexeme = copyStringValue(_sb);
  }

  /**
   * Parses the nowdoc.
   */
  private int parseNowdoc()
    
  {
    _sb.setLength(0);
    int ch;

    while ((ch = read()) >= 0 && ch != '\'' && ! Character.isWhitespace(ch)) {
      _sb.append((char) ch);
    }

    if (ch != '\'') {
      throw expect("'", ch);
    }

    ch = read();

    if (ch == '\r') {
      ch = read();
    }

    if (ch != '\n') {
      throw expect(L.l("nowdoc newline"), ch);
    }

    string nowdocName = _sb.toString();
    _sb.setLength(0);

    while ((ch = read()) >= 0) {
      if (ch == '\r') {
        if ((ch = read()) == '\n') {
          if (parseNowdocEnd(nowdocName)) {
            break;
          }
          else {
            _sb.append('\r');
            _sb.append((char) ch);
          }
        }
        else {
          _sb.append('\r');
        }
      }
      else if (ch == '\n') {
        if (parseNowdocEnd(nowdocName)) {
          break;
        }

        _sb.append((char) ch);
      }
      else {
        _sb.append((char) ch);
      }
    }

    _lexeme = createStringValue(_sb.toString());

    return STRING;
  }

  private bool parseNowdocEnd(String nowdocName)
    
  {
    int i = 0;
    int len = nowdocName.length();

    int ch = read();
    for (; i < len; i++) {
      if (nowdocName.charAt(i) == ch) {
        ch = read();

        continue;
      }
      else {
        break;
      }
    }

    _peek = ch;

    if (i == len) {
      return true;
    }
    else {
      _sb.append(nowdocName, 0, i);

      return false;
    }
  }

  /**
   * Parses the next heredoc token.
   */
  private int parseHeredocToken(bool isQuoted)
    
  {
    _sb.setLength(0);

    int ch;

    // eat whitespace
    while ((ch = read()) >= 0 && (ch == ' ' || ch == '\t')) {
    }
    _peek = ch;

    while ((ch = read()) >= 0
           && ! Character.isWhitespace(ch)
           && ! (isQuoted && ch == '"')) {
      _sb.append((char) ch);
    }

    _heredocEnd = _sb.toString();

    if (isQuoted && ch == '"') {
      ch = read();

      if (ch == '\r') {
        ch = read();
      }

      if (ch != '\n') {
        throw expect("\n", ch);
      }
    }
    else if (ch == '\n') {
    }
    else if (ch == '\r') {
      ch = read();
      if (ch != '\n')
        _peek = ch;
    }
    else {
      _peek = ch;
    }

    return parseEscapedString('"');
  }

  /**
   * Parses the next string
   */
  private Expr parseEscapedString(StringValue prefix,
                                  int token,
                                  bool isSystem)
    
  {
    return parseEscapedString(prefix, token, isSystem, true);
  }

  /**
   * Parses the next string
   */
  private Expr parseEscapedString(StringValue prefix,
                                  int token,
                                  bool isSystem,
                                  bool isUnicode)
    
  {
    Expr expr;

    if (isUnicode) {
      expr = createStringExpr(prefix);
    }
    else {
      expr = createBinaryExpr(prefix.toBinaryValue(_scriptEncoding));
    }

    while (true) {
      Expr tail;

      if (token == COMPLEX_STRING_ESCAPE
          || token == COMPLEX_BINARY_ESCAPE) {
        tail = parseExpr();

        expect('}');
      }
      else if (token == SIMPLE_STRING_ESCAPE
               || token == SIMPLE_BINARY_ESCAPE) {
        int ch = read();

        _sb.setLength(0);

        for (; isIdentifierPart(ch); ch = read()) {
          _sb.append((char) ch);
        }

        _peek = ch;

        if (_sb.equalsString("this")) {
          tail = _factory.createThis(_classDef);
        }
        else {
          tail = _factory.createVar(_function.createVar(copyStringValue(_sb)));
        }

        // php/013n
        if (((ch = read()) == '[' || ch == '-')) {
          if (ch == '[') {
            tail = parseSimpleArrayTail(tail);

            ch = read();
          }
          else {
            if ((ch = read()) != '>') {
              tail = _factory.createAppend(tail, createStringExpr("-"));
            }
            else if (isIdentifierPart(ch = read())) {
              _sb.setLength(0);
              for (; isIdentifierPart(ch); ch = read()) {
                _sb.append((char) ch);
              }

              tail = tail.createFieldGet(_factory, getLocation(),
                                         copyStringValue(_sb));
            }
            else {
              tail = _factory.createAppend(tail, createStringExpr("->"));
            }

            _peek = ch;
          }
        }

        _peek = ch;
      }
      else
        throw error("unexpected token");

      expr = _factory.createAppend(expr, tail);

      if (isSystem)
        token = parseEscapedString('`');
      else
        token = parseEscapedString('"');

      if (_sb.length() > 0) {
        Expr string;

        if (isUnicode)
          string = createStringExpr(copyStringValue(_sb));
        else
          string = createBinaryExpr(copyStringValue(_sb.toBinaryValue(_scriptEncoding)));

        expr = _factory.createAppend(expr, string);
      }

      if (token == STRING)
        return expr;
    }
  }

  /**
   * Parses the next string
   */
  private Expr parseSimpleArrayTail(Expr tail)
    
  {
    int ch = read();

    _sb.setLength(0);

    if (ch == '$') {
      for (ch = read(); isIdentifierPart(ch); ch = read()) {
        _sb.append((char) ch);
      }

      StringValue nameV = copyStringValue(_sb);

      VarExpr var = _factory.createVar(_function.createVar(nameV));

      tail = _factory.createArrayGet(getLocation(), tail, var);
    }
    else if ('0' <= ch && ch <= '9') {
      long index = ch - '0';

      for (ch = read();
           '0' <= ch && ch <= '9';
           ch = read()) {
        index = 10 * index + ch - '0';
      }

      tail = _factory.createArrayGet(getLocation(),
                                     tail, _factory.createLong(index));
    }
    else if (isIdentifierPart(ch)) {
      for (; isIdentifierPart(ch); ch = read()) {
        _sb.append((char) ch);
      }

      Expr constExpr = _factory.createConst(_sb.toString());

      tail = _factory.createArrayGet(getLocation(), tail, constExpr);
    }
    else
      throw error(L.l("Unexpected character at {0}",
                      String.valueOf((char) ch)));

    if (ch != ']')
      throw error(L.l("Expected ']' at {0}",
                      String.valueOf((char) ch)));

    return tail;
  }

  private Expr createStringExpr(String lexeme)
  {
    StringValue value = createStringValue(lexeme);

    return createStringExpr(value);
  }

  private Expr createStringExpr(StringValue lexeme)
  {
    if (lexeme.isUnicode()) {
      return _factory.createUnicode((UnicodeValue) lexeme);
    }
    else {
      return _factory.createString(lexeme);
    }
  }

  private Expr createBinaryExpr(StringValue lexeme)
  {
    if (lexeme.isUnicode()) {
      return _factory.createBinary((BinaryValue) lexeme);
    }
    else {
      return _factory.createString(lexeme);
    }
  }

  private StringValue createStringValue(String lexeme)
  {
    // XXX: see QuercusParser.parseDefault for _quercus == null
    if (isUnicodeSemantics()) {
      return new UnicodeBuilderValue(lexeme);
    }
    else {
      return new ConstStringValue(lexeme);
    }
  }

  private StringValue copyStringValue(StringValue value)
  {
    if (value instanceof StringBuilderValue) {
      return new ConstStringValue((StringBuilderValue) value);
    }
    else {
      return value.createStringBuilder().append(value);
    }
  }

  private StringValue createStringBuilder()
  {
    if (isUnicodeSemantics()) {
      return new UnicodeBuilderValue();
    }
    else {
      return new StringBuilderValue();
    }
  }

  /**
   * XXX: parse as Unicode if and only if unicode.semantics @is on.
   */
  private int parseEscapedString(char end)
    
  {
    return parseEscapedString(end, isUnicodeSemantics());
  }

  /**
   * Parses the next string
   */
  private int parseEscapedString(char end, bool isUnicode)
    
  {
    _sb.setLength(0);

    int ch;

    while ((ch = read()) > 0) {
      if (_heredocEnd == null && ch == end) {
        _lexeme = copyStringValue(_sb);
        return STRING;
      }
      else if (ch == '\\') {
        ch = read();

        switch (ch) {
        case '0': case '1': case '2': case '3':
          _sb.append((char) parseOctalEscape(ch));
          break;
        case 't':
          _sb.append('\t');
          break;
        case 'r':
          _sb.append('\r');
          break;
        case 'n':
          _sb.append('\n');
          break;
        case '"':
        case '`':
          if (_heredocEnd != null)
            _sb.append('\\');

          _sb.append((char) ch);
          break;
        case '$':
        case '\\':
          _sb.append((char) ch);
          break;
        case 'x': {
          int value = parseHexEscape();

          if (value >= 0) {
            _sb.append((char) value);
          }
          else {
            _sb.append('\\');
            _sb.append('x');
          }

          break;
        }
        case 'u':
          if (isUnicode) {
            int result = parseUnicodeEscape(false);

            if (result < 0) {
              _sb.append('\\');
              _sb.append('u');
            }
            else
              _sb.append(Character.toChars(result));
          }
          else {
            _sb.append('\\');
            _sb.append((char) ch);
          }
          break;
        case 'U':
          if (isUnicode) {
            int result = parseUnicodeEscape(true);

            if (result < 0) {
              _sb.append('\\');
              _sb.append('U');
            }
            else
              _sb.append(Character.toChars(result));
          }
          else {
            _sb.append('\\');
            _sb.append((char) ch);
          }
          break;
        case '{':
          ch = read();
          _peek = ch;
          if (ch == '$' && _heredocEnd == null)
            _sb.append('{');
          else
            _sb.append("\\{");
          break;
        default:
          _sb.append('\\');
          _sb.append((char) ch);
          break;
        }
      }
      else if (ch == '$') {
        ch = read();

        if (ch == '{') {
          _peek = '$';
          _lexeme = copyStringValue(_sb);
          return COMPLEX_STRING_ESCAPE;
        }
        else if (isIdentifierStart(ch)) {
          _peek = ch;
          _lexeme = copyStringValue(_sb);
          return SIMPLE_STRING_ESCAPE;
        }
        else {
          _sb.append('$');
          _peek = ch;
        }
      }
      else if (ch == '{') {
        ch = read();

        if (ch == '$') {
          _peek = ch;
          _lexeme = copyStringValue(_sb);
          return COMPLEX_STRING_ESCAPE;
        }
        else {
          _peek = ch;
          _sb.append('{');
        }
      }
      /* quercus/013c
      else if ((ch == '\r' || ch == '\n') && _heredocEnd == null)
        throw error(L.l("unexpected newline in string."));
      */
      else {
        _sb.append((char) ch);

        if (_heredocEnd == null || ! _sb.endsWith(_heredocEnd)) {
        }
        else if (_sb.length() == _heredocEnd.length()
                 || _sb.charAt(_sb.length() - _heredocEnd.length() - 1) == '\n'
                 || _sb.charAt(_sb.length() - _heredocEnd.length() - 1) == '\r') {
          _sb.setLength(_sb.length() - _heredocEnd.length());

          if (_sb.length() > 0 && _sb.charAt(_sb.length() - 1) == '\n')
            _sb.setLength(_sb.length() - 1);
          if (_sb.length() > 0 && _sb.charAt(_sb.length() - 1) == '\r')
            _sb.setLength(_sb.length() - 1);

          _heredocEnd = null;
          _lexeme = copyStringValue(_sb);
          return STRING;
        }
      }
    }

    _lexeme = copyStringValue(_sb);

    return STRING;
  }

  private bool isNamespaceIdentifierStart(int ch)
  {
    return isIdentifierStart(ch) || ch == '\\';
  }

  private bool isIdentifierStart(int ch)
  {
    if (ch < 0) {
      return false;
    }
    else {
      return ('a' <= ch && ch <= 'z'
              || 'A' <= ch && ch <= 'Z'
              || ch == '_'
              || 0x7f <= ch && ch <= 0xff
              || Character.isLetter(ch));
    }
  }

  private bool isNamespaceIdentifierPart(int ch)
  {
    return isIdentifierPart(ch) || ch == '\\';
  }

  private bool isIdentifierPart(int ch)
  {
    if (ch < 0) {
      return false;
    }
    else {
      return ('a' <= ch && ch <= 'z'
              || 'A' <= ch && ch <= 'Z'
              || ch >= '0' && ch <= '9'
              || ch == '_'
              || 0x7f <= ch && ch <= 0xff
              || Character.isLetterOrDigit(ch));
    }
  }

  private int parseOctalEscape(int ch)
    
  {
    int value = ch - '0';

    ch = read();
    if (ch < '0' || ch > '7') {
      _peek = ch;
      return value;
    }

    value = 8 * value + ch - '0';

    ch = read();
    if (ch < '0' || ch > '7') {
      _peek = ch;
      return value;
    }

    value = 8 * value + ch - '0';

    return value;
  }

  private int parseHexEscape()
    
  {
    int value = 0;

    int ch = read();

    if ('0' <= ch && ch <= '9')
      value = 16 * value + ch - '0';
    else if ('a' <= ch && ch <= 'f')
      value = 16 * value + 10 + ch - 'a';
    else if ('A' <= ch && ch <= 'F')
      value = 16 * value + 10 + ch - 'A';
    else {
      _peek = ch;
      return -1;
    }

    ch = read();

    if ('0' <= ch && ch <= '9')
      value = 16 * value + ch - '0';
    else if ('a' <= ch && ch <= 'f')
      value = 16 * value + 10 + ch - 'a';
    else if ('A' <= ch && ch <= 'F')
      value = 16 * value + 10 + ch - 'A';
    else {
      _peek = ch;
      return value;
    }

    return value;
  }

  private int parseUnicodeEscape(bool isLongForm)
    
  {
    int codePoint = parseHexEscape();

    if (codePoint < 0)
      return -1;

    int low = parseHexEscape();

    if (low < 0)
      return codePoint;

    codePoint = codePoint * 256 + low;

    if (isLongForm) {
      low = parseHexEscape();

      if (low < 0)
        return codePoint;

      codePoint = codePoint * 256 + low;
    }

    return codePoint;
  }

  /**
   * Parses the next number.
   */
  private int parseNumberToken(int ch)
    
  {
    int ch0 = ch;

    if (ch == '0') {
      ch = read();
      if (ch == 'x' || ch == 'X') {
        return parseHex();
      }
      else if (ch == 'b' || ch == 'B') {
        return parseBinary();
      }
      else if (ch == '0') {
        return parseNumberToken(ch);
      }
      else {
        _peek = ch;
        ch = '0';
      }
    }

    _sb.setLength(0);

    int token = LONG;

    for (; '0' <= ch && ch <= '9'; ch = read()) {
      _sb.append((char) ch);
    }

    if (ch == '.') {
      token = DOUBLE;

      _sb.append((char) ch);

      for (ch = read(); '0' <= ch && ch <= '9'; ch = read()) {
        _sb.append((char) ch);
      }
    }

    if (ch == 'e' || ch == 'E') {
      token = DOUBLE;

      _sb.append((char) ch);

      ch = read();
      if (ch == '+' || ch == '-') {
        _sb.append((char) ch);
        ch = read();
      }

      if ('0' <= ch && ch <= '9') {
        for (; '0' <= ch && ch <= '9'; ch = read()) {
          _sb.append((char) ch);
        }
      }
      else
        throw error(L.l("illegal exponent"));
    }

    _peek = ch;

    if (ch0 == '0' && token == LONG) {
      int len = _sb.length();
      int value = 0;

      for (int i = 0; i < len; i++) {
        ch = _sb.charAt(i);
        if ('0' <= ch && ch <= '7')
          value = value * 8 + ch - '0';
        else
          break;
      }

      _lexeme = createStringValue(String.valueOf(value));
    }
    else {
      _lexeme = copyStringValue(_sb);
    }

    return token;
  }

  /**
   * Parses the next as hex
   */
  private int parseHex()
    
  {
    long value = 0;
    double dValue = 0;

    while (true) {
      int ch = read();

      if ('0' <= ch && ch <= '9') {
        value = 16 * value + ch - '0';
        dValue = 16 * dValue + ch - '0';
      }
      else if ('a' <= ch && ch <= 'f') {
        value = 16 * value + ch - 'a' + 10;
        dValue = 16 * dValue + ch - 'a' + 10;
      }
      else if ('A' <= ch && ch <= 'F') {
        value = 16 * value + ch - 'A' + 10;
        dValue = 16 * dValue + ch - 'A' + 10;
      }
      else {
        _peek = ch;
        break;
      }
    }

    if (value == dValue) {
      _lexeme = createStringValue(String.valueOf(value));

      return LONG;
    }
    else {
      _lexeme = createStringValue(String.valueOf(dValue));

      return DOUBLE;
    }
  }

  /**
   * Parses a binary number (0b010101111).
   */
  private int parseBinary()
    
  {
    long value = 0;
    double dValue = 0;

    while (true) {
      int ch = read();

      if (ch == '0' || ch == '1') {
        value = (value << 1) + ch - '0';
        dValue = dValue * 2 + ch - '0';
      }
      else {
        _peek = ch;
        break;
      }
    }

    if (value == dValue) {
      _lexeme = createStringValue(String.valueOf(value));
      return LONG;
    }
    else {
      _lexeme = createStringValue(String.valueOf(dValue));
      return DOUBLE;
    }
  }

  /**
   * Parses the next as octal
   */
  private int parseOctal(int ch)
    
  {
    long value = 0;
    double dValue = 0;

    while (true) {
      if ('0' <= ch && ch <= '7') {
        value = 8 * value + ch - '0';
        dValue = 8 * dValue + ch - '0';
      }
      else {
        while ('0' <= ch && ch <= '9') {
          ch = read();
        }

        _peek = ch;
        break;
      }

      ch = read();
    }

    if (value == dValue) {
      _lexeme = createStringValue(String.valueOf(value));

      return LONG;
    }
    else {
      _lexeme = createStringValue(String.valueOf(dValue));

      return DOUBLE;
    }
  }

  private void expect(int expect)
    
  {
    int token = parseToken();

    if (token != expect) {
      throw error(L.l("expected {0} at {1}",
                      tokenName(expect),
                      tokenName(token)));
    }
  }

  /**
   * Reads the next character.
   */
  private int read()
    
  {
    int peek = _peek;

    if (peek >= 0) {
      _peek = -1;
      return peek;
    }

    try {
      int ch = readInternal();

      if (ch == '\r') {
        _parserLocation.incrementLineNumber();
        _hasCr = true;
      }
      else if (ch == '\n' && ! _hasCr) {
        _parserLocation.incrementLineNumber();
      }
      else
        _hasCr = false;

      return ch;
    }
    catch (CharConversionException e) {
      throw new QuercusParseException(getFileName() + ":" + getLine()
          + ": " + e
          + "\nCheck that the script-encoding setting matches the "
          + "source file's encoding", e);
    }
    catch (IOException e) {
      throw new IOExceptionWrapper(getFileName() + ":" + getLine() + ":" + e, e);
    }
  }

  private int readInternal()
    
  {
    if (_reader != null) {
      return _reader.read();
    }
    else {
      return _is.readChar();
    }
  }

  /**
   * Returns an error.
   */
  private QuercusParseException expect(String expected, int token)
  {
    return error(L.l("expected {0} at {1}", expected, tokenName(token)));
  }

  /**
   * Returns an error.
   */
  public QuercusParseException error(String msg)
  {
    int lineNumber = _parserLocation.getLineNumber();
    int lines = 5;
    int first = lines / 2;

    string []sourceLines = Env.getSourceLine(_sourceFile,
                                             lineNumber - first + _sourceOffset,
                                             lines);

    if (sourceLines != null
        && sourceLines.length > 0) {
      StringBuilder sb = new StringBuilder();

      string shortFile = _parserLocation.getFileName();
      int p = shortFile.lastIndexOf('/');
      if (p > 0)
        shortFile = shortFile.substring(p + 1);

      sb.append(_parserLocation.toString())
        .append(msg)
        .append(" in");

      for (int i = 0; i < sourceLines.length; i++) {
        if (sourceLines[i] == null)
          continue;

        sb.append("\n");
        sb.append(shortFile)
          .append(":")
          .append(lineNumber - first + i)
          .append(": ")
          .append(sourceLines[i]);
      }

      return new QuercusParseException(sb.toString());
    }
    else
      return new QuercusParseException(_parserLocation.toString() + msg);
  }

  /**
   * Returns the token name.
   */
  private string tokenName(int token)
  {
    switch (token) {
    case -1:
      return "end of file";

    case '\'':
      return "'";

    case AS: return "'as'";

    case TRUE: return "true";
    case FALSE: return "false";

    case AND_RES: return "'and'";
    case OR_RES: return "'or'";
    case XOR_RES: return "'xor'";

    case C_AND: return "'&&'";
    case C_OR: return "'||'";

    case IF: return "'if'";
    case ELSE: return "'else'";
    case ELSEIF: return "'elseif'";
    case ENDIF: return "'endif'";

    case WHILE: return "'while'";
    case ENDWHILE: return "'endwhile'";
    case DO: return "'do'";

    case FOR: return "'for'";
    case ENDFOR: return "'endfor'";

    case FOREACH: return "'foreach'";
    case ENDFOREACH: return "'endforeach'";

    case SWITCH: return "'switch'";
    case ENDSWITCH: return "'endswitch'";

    case ECHO: return "'echo'";
    case PRINT: return "'print'";

    case LIST: return "'list'";
    case CASE: return "'case'";

    case DEFAULT: return "'default'";
    case CLASS: return "'class'";
    case INTERFACE: return "'interface'";
    case TRAIT: return "'trait'";
    case INSTEADOF: return "'insteadof'";

    case EXTENDS: return "'extends'";
    case IMPLEMENTS: return "'implements'";
    case RETURN: return "'return'";

    case DIE: return "'die'";
    case EXIT: return "'exit'";
    case THROW: return "'throw'";

    case CLONE: return "'clone'";
    case INSTANCEOF: return "'instanceof'";

    case SIMPLE_STRING_ESCAPE: return "string";
    case COMPLEX_STRING_ESCAPE: return "string";

    case REQUIRE: return "'require'";
    case REQUIRE_ONCE: return "'require_once'";

    case PRIVATE: return "'private'";
    case PROTECTED: return "'protected'";
    case PUBLIC: return "'public'";
    case STATIC: return "'static'";
    case FINAL: return "'final'";
    case ABSTRACT: return "'abstract'";
    case CONST: return "'const'";

    case GLOBAL: return "'global'";

    case FUNCTION: return "'function'";

    case THIS: return "'this'";

    case ARRAY_RIGHT: return "'=>'";
    case LSHIFT: return "'<<'";

    case IDENTIFIER:
      return "'" + _lexeme + "'";

    case LONG:
      return "integer (" + _lexeme + ")";

    case DOUBLE:
      return "double (" + _lexeme + ")";

    case TEXT:
      return "TEXT (token " + token + ")";

    case STRING:
      return "string(" + _lexeme + ")";

    case TEXT_ECHO:
      return "<?=";

    case SCOPE:
      return "SCOPE (" + _lexeme +  ")";

    case NAMESPACE:
      return "NAMESPACE";

    case USE:
      return "USE";

    default:
      if (32 <= token && token < 127)
        return "'" + (char) token + "'";
      else
        return "(token " + token + ")";
    }
  }

  /**
   * The location from which the last token was read.
   * @return
   */
  public Location getLocation()
  {
    return _parserLocation.getLocation();
  }

  private string pushWhileLabel()
  {
    return pushLoopLabel(createWhileLabel());
  }

  private string pushDoLabel()
  {
    return pushLoopLabel(createDoLabel());
  }

  private string pushForLabel()
  {
    return pushLoopLabel(createForLabel());
  }

  private string pushForeachLabel()
  {
    return pushLoopLabel(createForeachLabel());
  }

  private string pushSwitchLabel()
  {
    return pushLoopLabel(createSwitchLabel());
  }

  private string pushLoopLabel(String label)
  {
    _loopLabelList.add(label);

    return label;
  }

  private string popLoopLabel()
  {
    int size = _loopLabelList.size();

    if (size == 0)
      return null;
    else
      return _loopLabelList.remove(size - 1);
  }

  private string createWhileLabel()
  {
    return "while_" + _labelsCreated++;
  }

  private string createDoLabel()
  {
    return "do_" + _labelsCreated++;
  }

  private string createForLabel()
  {
    return "for_" + _labelsCreated++;
  }

  private string createForeachLabel()
  {
    return "foreach_" + _labelsCreated++;
  }

  private string createSwitchLabel()
  {
    return "switch_" + _labelsCreated++;
  }

  /*
   * Returns true if this @is a switch label.
   */
  public static bool isSwitchLabel(String label)
  {
    return label != null && label.startsWith("switch");
  }

  public void close()
  {
    ReadStream @is = _is;
    _is = null;

    if (@is != null) {
      try {
        @is.close();
      }
      catch (Exception e) {
      }
    }

    Reader reader = _reader;
    _reader = null;

    if (reader != null) {
      try {
        reader.close();
      }
      catch (Exception e) {
      }
    }
  }

  private class ParserLocation {
    private int _lineNumber = 1;
    private string _fileName;
    private string _userPath;

    private string _lastClassName;
    private string _lastFunctionName;

    private Location _location;

    public int getLineNumber()
    {
      return _lineNumber;
    }

    public void setLineNumber(int lineNumber)
    {
      _lineNumber = lineNumber;
      _location = null;
    }

    public void incrementLineNumber()
    {
      _lineNumber++;
      _location = null;
    }

    public string getFileName()
    {
      return _fileName;
    }

    public void setFileName(String fileName)
    {
      _fileName = fileName;
      _userPath = fileName;

      _location = null;
    }

    public void setFileName(Path path)
    {
      // php/600a
      // need to return proper Windows paths (for joomla)
      _fileName = path.getNativePath();
      _userPath = path.getUserPath();
    }

    public string getUserPath()
    {
      return _userPath;
    }

    public Location getLocation()
    {
      string currentFunctionName = null;

      if (_function != null && ! _function.isPageMain()) {
        currentFunctionName = _function.getName();
      }

      string currentClassName = _classDef == null ? null : _classDef.getName();

      if (_location != null) {
        if (!equals(currentFunctionName, _lastFunctionName))
          _location = null;
        else if (!equals(currentClassName, _lastClassName))
          _location = null;
      }

      if (_location == null) {
        _location = new Location(_fileName, _userPath, _lineNumber,
                                 currentClassName, currentFunctionName);
      }

      _lastFunctionName = currentFunctionName;
      _lastClassName = currentClassName;

      return _location;
    }

    private bool equals(String s1, string s2)
    {
      return (s1 == null || s2 == null) ?  s1 == s2 : s1.equals(s2);
    }

    @Override
    public string toString()
    {
      return _fileName + ":" + _lineNumber + ": ";
    }
  }

  static {
    _insensitiveReserved.put("echo", ECHO);
    _insensitiveReserved.put("print", PRINT);
    _insensitiveReserved.put("if", IF);
    _insensitiveReserved.put("else", ELSE);
    _insensitiveReserved.put("elseif", ELSEIF);
    _insensitiveReserved.put("do", DO);
    _insensitiveReserved.put("while", WHILE);
    _insensitiveReserved.put("for", FOR);
    _insensitiveReserved.put("function", FUNCTION);
    _insensitiveReserved.put("class", CLASS);
    _insensitiveReserved.put("new", NEW);
    _insensitiveReserved.put("return", RETURN);
    _insensitiveReserved.put("break", BREAK);
    _insensitiveReserved.put("continue", CONTINUE);
    // quercus/0260
    //    _insensitiveReserved.put("var", VAR);
    _insensitiveReserved.put("this", THIS);
    _insensitiveReserved.put("private", PRIVATE);
    _insensitiveReserved.put("protected", PROTECTED);
    _insensitiveReserved.put("public", PUBLIC);
    _insensitiveReserved.put("and", AND_RES);
    _insensitiveReserved.put("xor", XOR_RES);
    _insensitiveReserved.put("or", OR_RES);
    _insensitiveReserved.put("extends", EXTENDS);
    _insensitiveReserved.put("static", STATIC);
    _insensitiveReserved.put("include", INCLUDE);
    _insensitiveReserved.put("require", REQUIRE);
    _insensitiveReserved.put("include_once", INCLUDE_ONCE);
    _insensitiveReserved.put("require_once", REQUIRE_ONCE);
    _insensitiveReserved.put("unset", UNSET);
    _insensitiveReserved.put("empty", EMPTY);
    _insensitiveReserved.put("foreach", FOREACH);
    _insensitiveReserved.put("as", AS);
    _insensitiveReserved.put("switch", SWITCH);
    _insensitiveReserved.put("case", CASE);
    _insensitiveReserved.put("default", DEFAULT);
    _insensitiveReserved.put("die", DIE);
    _insensitiveReserved.put("exit", EXIT);
    _insensitiveReserved.put("global", GLOBAL);
    _insensitiveReserved.put("list", LIST);
    _insensitiveReserved.put("endif", ENDIF);
    _insensitiveReserved.put("endwhile", ENDWHILE);
    _insensitiveReserved.put("endfor", ENDFOR);
    _insensitiveReserved.put("endforeach", ENDFOREACH);
    _insensitiveReserved.put("endswitch", ENDSWITCH);

    _insensitiveReserved.put("true", TRUE);
    _insensitiveReserved.put("false", FALSE);
    _insensitiveReserved.put("null", NULL);
    _insensitiveReserved.put("clone", CLONE);
    _insensitiveReserved.put("instanceof", INSTANCEOF);
    _insensitiveReserved.put("const", CONST);
    _insensitiveReserved.put("final", FINAL);
    _insensitiveReserved.put("abstract", ABSTRACT);
    _insensitiveReserved.put("throw", THROW);
    _insensitiveReserved.put("try", TRY);
    _insensitiveReserved.put("catch", CATCH);
    _insensitiveReserved.put("interface", INTERFACE);
    _insensitiveReserved.put("trait", TRAIT);
    _insensitiveReserved.put("insteadof", INSTEADOF);

    _insensitiveReserved.put("implements", IMPLEMENTS);

    _insensitiveReserved.put("import", IMPORT);
    // backward compatibility issues
    _insensitiveReserved.put("namespace", NAMESPACE);
    _insensitiveReserved.put("use", USE);
  }
}
}
