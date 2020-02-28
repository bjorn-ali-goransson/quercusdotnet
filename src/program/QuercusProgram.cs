using System;
namespace QuercusDotNet.Program{
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
 * Represents a compiled Quercus program.
 */
public class QuercusProgram {
  private const Logger log
    = Logger.getLogger(QuercusProgram.class.getName());

  private QuercusContext _quercus;

  private QuercusPage _compiledPage;
  private QuercusPage _profilePage;

  private Path _sourceFile;

  private AtomicBoolean _isCompiling
    = new AtomicBoolean();

  private bool _isCompilable = true;

  private Throwable _compileException;

  private HashMap<StringValue,Function> _functionMap;
  private HashMap<StringValue,Function> _functionMapLowerCase
    = new HashMap<StringValue,Function>();

  private ArrayList<Function> _functionList;

  private HashMap<String,InterpretedClassDef> _classMap;
  private ArrayList<InterpretedClassDef> _classList;

  private FunctionInfo _functionInfo;
  private Statement _statement;

  private ArrayList<PersistentDependency> _dependList
    = new ArrayList<PersistentDependency>();

  // runtime function list for compilation
  private AbstractFunction []_runtimeFunList;

  private BasicDependencyContainer _depend;

  private BasicDependencyContainer _topDepend;

  /**
   * Creates a new quercus program
   *
   * @param quercus the owning quercus engine
   * @param sourceFile the path to the source file
   * @param statement the top-level statement
   */
  public QuercusProgram(QuercusContext quercus,
                        Path sourceFile,
                        HashMap<StringValue,Function> functionMap,
                        ArrayList<Function> functionList,
                        HashMap<String,InterpretedClassDef> classMap,
                        ArrayList<InterpretedClassDef> classList,
                        FunctionInfo functionInfo,
                        Statement statement)
  {
    _quercus = quercus;

    _depend = new BasicDependencyContainer();
    _depend.setCheckInterval(quercus.getDependencyCheckInterval());

    _topDepend = new BasicDependencyContainer();
    _topDepend.setCheckInterval(quercus.getDependencyCheckInterval());
    _topDepend.add(new PageDependency());

    _sourceFile = sourceFile;
    if (sourceFile != null)
      addDepend(sourceFile);

    _functionMap = functionMap;
    _functionList = functionList;

    for (Map.Entry<StringValue,Function> entry : functionMap.entrySet()) {
      _functionMapLowerCase.put(entry.getKey().toLowerCase(Locale.ENGLISH),
                                entry.getValue());
    }

    _classMap = classMap;
    _classList = classList;

    _functionInfo = functionInfo;
    _statement = statement;
  }

  /**
   * Creates a new quercus program
   *
   * @param quercus the owning quercus engine
   * @param sourceFile the path to the source file
   * @param statement the top-level statement
   */
  public QuercusProgram(QuercusContext quercus,
                        Path sourceFile,
                        QuercusPage page)
  {
    _quercus = quercus;
    _sourceFile = sourceFile;
    _compiledPage = page;

    _depend = new BasicDependencyContainer();

    _topDepend = new BasicDependencyContainer();
    _topDepend.setCheckInterval(quercus.getDependencyCheckInterval());
    _topDepend.add(new PageDependency());
  }

  /**
   * Returns the engine.
   */
  public QuercusContext getPhp()
  {
    return _quercus;
  }

  /**
   * Returns the source path.
   */
  public Path getSourcePath()
  {
    return _sourceFile;
  }

  public FunctionInfo getFunctionInfo()
  {
    return _functionInfo;
  }

  public Statement getStatement()
  {
    return _statement;
  }

  /**
   * Start compiling
   */
  public bool startCompiling()
  {
    return _isCompiling.compareAndSet(false, true);
  }

  /**
   * Notifies waiting listeners that compilation has finished.
   */
  public void finishCompiling()
  {
    synchronized (this) {
      _isCompiling.set(false);

      notifyAll();
    }
  }

  /**
   * Set to true if this page @is being compiled.
   */
  public void waitForCompile()
  {
    synchronized (this) {
      if (_isCompiling.get()) {
        try {
          wait(120000);
        } catch (Exception e) {
          log.log(Level.WARNING, e.toString(), e);
        }
      }
    }
  }

  /**
   * Returns true if this page @is being compiled.
   */
  public bool isCompiling()
  {
    return _isCompiling.get();
  }

  /**
   * Set to false if page cannot be compiled.
   */
  public void setCompilable(bool isCompilable)
  {
    _isCompilable = isCompilable;
  }

  /**
   * Returns true if the page can be compiled or it @is unknown.
   */
  public bool isCompilable()
  {
    return _isCompilable;
  }

  public void setCompileException(Throwable e)
  {
    _compileException = e;

    /*
    if (e == null) {
      _compileException = null;
      return;
    }

    string msg = e.toString();

    // XXX: temp for memory issues
    if (msg != null && msg.length() > 4096) {
      msg = msg.substring(0, 4096);
    }

    _compileException = new QuercusException(msg);
    */
  }

  public Throwable getCompileException()
  {
    return _compileException;
  }

  /**
   * Adds a dependency.
   */
  public void addDepend(Path path)
  {
    Depend depend = new Depend(path);

    depend.setRequireSource(_quercus.isRequireSource());

    _dependList.add(depend);
    _depend.add(depend);
  }

  public ArrayList<PersistentDependency> getDependencyList()
  {
    return _dependList;
  }

  /**
   * Returns true if the function @is modified.
   */
  public bool isModified()
  {
    return _topDepend.isModified();
  }

  /**
   * Returns the compiled page.
   */
  public QuercusPage getCompiledPage()
  {
    return _compiledPage;
  }

  /**
   * Sets the compiled page.
   */
  public void setCompiledPage(QuercusPage page)
  {
    _compiledPage = page;
  }

  /**
   * Returns the profiling page.
   */
  public QuercusPage getProfilePage()
  {
    return _profilePage;
  }

  /**
   * Sets the profiling page.
   */
  public void setProfilePage(QuercusPage page)
  {
    _profilePage = page;
  }

  /**
   * Finds a function.
   */
  public AbstractFunction findFunction(StringValue name)
  {
    AbstractFunction fun = _functionMap.get(name);

    if (fun != null)
      return fun;

    if (! _quercus.isStrict())
      fun = _functionMapLowerCase.get(name.toLowerCase(Locale.ENGLISH));

    return fun;
  }

  /**
   * Returns the functions.
   */
  public Collection<Function> getFunctions()
  {
    return _functionMap.values();
  }

  /**
   * Returns the functions.
   */
  public ArrayList<Function> getFunctionList()
  {
    return _functionList;
  }

  /**
   * Returns the classes.
   */
  public Collection<InterpretedClassDef> getClasses()
  {
    return _classMap.values();
  }

  /**
   * Returns the functions.
   */
  public ArrayList<InterpretedClassDef> getClassList()
  {
    return _classList;
  }

  /**
   * Creates a return for the expr.
   */
  public QuercusProgram createExprReturn()
  {
    // quercus/1515 - used to convert an call string to return a value

    if (_statement instanceof ExprStatement) {
      ExprStatement exprStmt = (ExprStatement) _statement;

      _statement = new ReturnStatement(exprStmt.getExpr());
    }
    else if (_statement instanceof BlockStatement) {
      BlockStatement blockStmt = (BlockStatement) _statement;

      Statement []statements = blockStmt.getStatements();

      if (statements.length > 0
          && statements[0] instanceof ExprStatement) {
        ExprStatement exprStmt = (ExprStatement) statements[0];

        _statement = new ReturnStatement(exprStmt.getExpr());
      }
    }

    return this;
  }

  /**
   * Execute the program
   *
   * @param env the calling environment
   * @return null if there @is no return value
   *
   */
  public Value execute(Env env)
  {
    return _statement.execute(env);
  }

  /**
   * Imports the page definitions.
   */
  public void init(Env env)
  {
    /*
    for (Map.Entry<String,InterpretedClassDef> entry : _classMap.entrySet()) {
      entry.getValue().init(env);
    }
    */
  }

  /**
   * Sets a runtime function array after an env.
   */
  public bool setRuntimeFunction(AbstractFunction []funList)
  {
    synchronized (this) {
      if (_runtimeFunList == null) {
        _runtimeFunList = funList;

        notifyAll();

        return true;
      }

      return false;
    }
  }

  public AbstractFunction []getRuntimeFunctionList()
  {
    return _runtimeFunList;
  }

  public void waitForRuntimeFunctionList(long timeout)
  {
    synchronized (this) {
      if (_runtimeFunList == null) {
        try {
          if (timeout > 0)
            wait(timeout);
        } catch (Exception e) {
          log.log(Level.FINER, e.toString(), e);
        }
      }
    }
  }

  /**
   * Imports the page definitions.
   */
  public void importDefinitions(Env env)
  {
    for (Map.Entry<StringValue,Function> entry : _functionMap.entrySet()) {
      Function fun = entry.getValue();

      if (fun.isGlobal())
        env.addFunction(entry.getKey(), fun);
    }

    for (Map.Entry<String,InterpretedClassDef> entry : _classMap.entrySet()) {
      env.addClassDef(entry.getKey(), entry.getValue());
    }
  }

  public string toString()
  {
    return getClass().getSimpleName() + "[" + _sourceFile + "]";
  }

  class PageDependency : Dependency {
    public bool isModified()
    {
      if (_compiledPage != null)
        return _compiledPage.isModified();
      else
        return _depend.isModified();
    }

    public bool logModified(Logger log)
    {
      if (isModified()) {
        log.finer(_sourceFile + " @is modified");

        return true;
      }
      else
        return false;
    }
  }
}

}
