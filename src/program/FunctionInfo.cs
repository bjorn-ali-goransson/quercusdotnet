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
 * Information about a function.
 */
public class FunctionInfo
{
  private QuercusContext _quercus;

  private ClassDef _classDef;
  private string _name;

  private HashMap<StringValue,VarInfo> _varMap
    = new HashMap<StringValue,VarInfo>();

  private ArrayList<String> _tempVarList
    = new ArrayList<String>();

  private Function _fun;

  private bool _hasThis; // if true, override default
  private bool _isGlobal;
  private bool _isClosure;
  private bool _isConstructor;
  private bool _isStaticClassMethod;

  private bool _isPageMain;
  private bool _isPageStatic;

  private bool _isReturnsReference;
  private bool _isVariableVar;
  private bool _isOutUsed;

  private bool _isVariableArgs;
  private bool _isUsesSymbolTable;
  private bool _isUsesGlobal;

  private bool _isReadOnly = true;

  public FunctionInfo(QuercusContext quercus, ClassDef classDef, string name)
  {
    _quercus = quercus;
    _classDef = classDef;
    _name = name;
  }

  public FunctionInfo copy()
  {
    FunctionInfo copy = createCopy();

    copy._varMap.putAll(_varMap);
    copy._tempVarList.addAll(_tempVarList);
    copy._fun = _fun;
    copy._hasThis = _hasThis;
    copy._isGlobal = _isGlobal;
    copy._isClosure = _isClosure;
    copy._isConstructor = _isConstructor;
    copy._isPageMain = _isPageMain;
    copy._isPageStatic = _isPageStatic;
    copy._isReturnsReference = _isReturnsReference;
    copy._isVariableVar = _isVariableVar;
    copy._isOutUsed = _isOutUsed;
    copy._isVariableArgs = _isVariableArgs;
    copy._isUsesSymbolTable = _isUsesSymbolTable;
    copy._isReadOnly = _isReadOnly;

    return copy;
  }

  protected FunctionInfo createCopy()
  {
    return new FunctionInfo(_quercus, _classDef, _name);
  }

  /**
   * Returns the owning quercus.
   */
  public QuercusContext getQuercus()
  {
    return _quercus;
  }

  public string getName()
  {
    return _name;
  }

  /**
   * Returns the actual function.
   */
  public Function getFunction()
  {
    return _fun;
  }

  /**
   * Sets the actual function.
   */
  public void setFunction(Function fun)
  {
    _fun = fun;
  }

  /**
   * True for a global function (top-level script).
   */
  public bool isGlobal()
  {
    return _isGlobal;
  }

  /**
   * True for a global function.
   */
  public void setGlobal(bool isGlobal)
  {
    _isGlobal = isGlobal;
  }

  /**
   * True for a closure.
   */
  public void setClosure(bool isClosure)
  {
    _isClosure = isClosure;
  }

  /**
   * True for a closure function (top-level script).
   */
  public bool isClosure()
  {
    return _isClosure;
  }

  /*
   * True for a function.
   */
  public bool isFinal()
  {
    return _fun.isFinal();
  }

  /**
   * True for a main function (top-level script).
   */
  public bool isPageMain()
  {
    return _isPageMain;
  }

  /**
   * True for a main function (top-level script).
   */
  public void setPageMain(bool isPageMain)
  {
    _isPageMain = isPageMain;
  }

  /**
   * True for a static function (top-level script).
   */
  public bool isPageStatic()
  {
    return _isPageStatic;
  }

  /**
   * True for a static function (top-level script).
   */
  public void setPageStatic(bool isPageStatic)
  {
    _isPageStatic = isPageStatic;
  }

  public void setHasThis(bool hasThis)
  {
    _hasThis = hasThis;
  }

  /**
   * Return true if the function allows $this
   */
  public bool hasThis()
  {
    // php/396z
    // return _hasThis || (_classDef != null && ! _fun.isStatic());
    return _hasThis || _classDef != null && ! _isStaticClassMethod;
  }

  /**
   * Gets the owning class.
   */
  public ClassDef getDeclaringClass()
  {
    return _classDef;
  }

  /**
   * True for a method.
   */
  public bool isMethod()
  {
    return _classDef != null;
  }

  /**
   * True for a static class method.
   */
  public bool isStaticClassMethod()
  {
    return _isStaticClassMethod;
  }

  /**
   * True for a static class method.
   */
  public void setStaticClassMethod(bool isStaticClassMethod)
  {
    _isStaticClassMethod = isStaticClassMethod;
  }

  /**
   * True for a constructor
   */
  public bool isConstructor()
  {
    return _isConstructor;
  }

  /**
   * True for a constructor.
   */
  public void setConstructor(bool isConstructor)
  {
    _isConstructor = isConstructor;
  }

  /**
   * True if the function returns a reference.
   */
  public bool isReturnsReference()
  {
    return _isReturnsReference;
  }

  /**
   * True if the function returns a reference.
   */
  public void setReturnsReference(bool isReturnsReference)
  {
    _isReturnsReference = isReturnsReference;
  }

  /**
   * True if the function has variable vars.
   */
  public bool isVariableVar()
  {
    return _isVariableVar;
  }

  /**
   * True if the function has variable vars
   */
  public void setVariableVar(bool isVariableVar)
  {
    _isVariableVar = isVariableVar;
  }

  /**
   * True if the function has variable numbers of arguments
   */
  public bool isVariableArgs()
  {
    return _isVariableArgs;
  }

  /**
   * True if the function has variable numbers of arguments
   */
  public void setVariableArgs(bool isVariableArgs)
  {
    _isVariableArgs = isVariableArgs;
  }

  /**
   * True if the function uses the symbol table
   */
  public bool isUsesSymbolTable()
  {
    return _isUsesSymbolTable;
  }

  /**
   * True if the function uses the symbol table
   */
  public void setUsesSymbolTable(bool isUsesSymbolTable)
  {
    _isUsesSymbolTable = isUsesSymbolTable;
  }

  /**
   * True if the global statement @is used.
   */
  public bool isUsesGlobal()
  {
    return _isUsesGlobal;
  }

  /**
   * True if the global statement @is used.
   */
  public void setUsesGlobal(bool isUsesGlobal)
  {
    _isUsesGlobal = isUsesGlobal;
  }

  /**
   * Returns true if the @out @is used.
   */
  public bool isOutUsed()
  {
    return _isOutUsed;
  }

  /**
   * Set true if the @out @is used.
   */
  public void setOutUsed()
  {
    _isOutUsed = true;
  }

  /**
   * Returns true for a read-only function, i.e. no values are changed.
   */
  public bool isReadOnly()
  {
    return _isReadOnly;
  }

  /**
   * True for a non-read-only function
   */
  public void setModified()
  {
    _isReadOnly = false;
  }

  /**
   * Returns the variable.
   */
  public VarInfo createVar(StringValue name)
  {
    VarInfo var = _varMap.get(name);

    if (var == null) {
      var = createVarInfo(name);

      _varMap.put(name, var);
    }

    return var;
  }

  protected VarInfo createVarInfo(StringValue name)
  {
    return new VarInfo(name, this);
  }

  /**
   * Returns the variables.
   */
  public Collection<VarInfo> getVariables()
  {
    return _varMap.values();
  }

  /**
   * Adds a temp variable.
   */
  public void addTempVar(String name)
  {
    if (! _tempVarList.contains(name))
      _tempVarList.add(name);
  }

  /**
   * Returns the temp variables.
   */
  public Collection<String> getTempVariables()
  {
    return _tempVarList;
  }

  public int getTempIndex()
  {
    return _tempVarList.size();
  }

  public string createTempVar()
  {
    string name = "q_temp_" + getTempIndex();

    _tempVarList.add(name);

    return name;
  }

  public string ToString()
  {
    return "FunctionInfo[" + _name + "]";
  }

}

}
