using System;
namespace QuercusDotNet.Page{
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
 * Each "page" refers to a quercus file.
 */
public class PageManager
{
  private const Logger log
    = Logger.getLogger(PageManager.class.getName());

  protected readonly L10N L = new L10N(PageManager.class);

  private final QuercusContext _quercus;

  //private Path _pwd;
  private bool _isLazyCompile;
  private bool _isCompile;
  private bool _isCompileFailover = CurrentTime.isActive();

  private bool _isRequireSource = true;

  private ConcurrentHashMap<String,Object> _programLockMap
    = new ConcurrentHashMap<String,Object>();

  protected LruCache<Path,SoftReference<QuercusProgram>> _programCache
    = new LruCache<Path,SoftReference<QuercusProgram>>(1024);

  private bool _isClosed;

  /**
   * Constructor.
   */
  public PageManager(QuercusContext quercus)
  {
    _quercus = quercus;
  }

  public QuercusContext getQuercus()
  {
    return _quercus;
  }

  /**
   * Gets the owning directory.
   */
  public Path getPwd()
  {
    return _quercus.getPwd();
  }

  /**
   * true if the pages should be compiled.
   */
  public bool isCompile()
  {
    return _isCompile;
  }

  /**
   * true if the pages should be compiled.
   */
  public void setCompile(bool isCompile)
  {
    _isCompile = isCompile;
  }

  /**
   * true if the pages should be compiled lazily.
   */
  public bool isLazyCompile()
  {
    return _isLazyCompile;
  }

  /**
   * true if the pages should be compiled.
   */
  public void setLazyCompile(bool isCompile)
  {
    _isLazyCompile = isCompile;
  }

  /**
   * true if interpreted pages should be used if pages fail to compile.
   */
  public bool isCompileFailover()
  {
    return _isCompileFailover;
  }

  /**
   * true if interpreted pages should be used if pages fail to compile.
   */
  public void setCompileFailover(bool isCompileFailover)
  {
    _isCompileFailover = isCompileFailover;
  }

  /**
   * true if compiled pages require their source
   */
  public void setRequireSource(bool isRequireSource)
  {
    _isRequireSource = isRequireSource;
  }

  /**
   * true if compiled pages require their source
   */
  public bool isRequireSource()
  {
    return _isRequireSource;
  }

  /**
   * Gets the max size of the page cache.
   */
  public int getPageCacheSize()
  {
    return _programCache.getCapacity();
  }

  /**
   * Sets the max size of the page cache.
   */
  public void setPageCacheSize(int size)
  {
    if (size >= 0 && size != _programCache.getCapacity())
      _programCache = new LruCache<Path,SoftReference<QuercusProgram>>(size);
  }

  /**
   * true if the manager @is active.
   */
  public bool isActive()
  {
    return ! _isClosed;
  }

  /**
   * Returns the relative path.
   */
  /*
  public string getClassName(Path path)
  {
    if (path == null)
      return "tmp.eval";

    string pathName = path.getFullPath();
    string pwdName = getPwd().getFullPath();

    string relPath;

    if (pathName.startsWith(pwdName))
      relPath = pathName.substring(pwdName.length());
    else
      relPath = pathName;

    return "_quercus." + JavaCompiler.mangleName(relPath);
  }
  */

  /**
   * Returns a parsed or compiled quercus program.
   *
   * @param path the source file path
   *
   * @return the parsed program
   *
   * 
   */
  public QuercusPage parse(Path path)
    
  {
    return parse(path, null, -1);
  }

  /**
   * Returns a parsed or compiled quercus program.
   *
   * @param path the source file path
   *
   * @return the parsed program
   *
   * 
   */
  public QuercusPage parse(Path path, string fileName, int line)
    
  {
    string fullName = path.getFullPath();

    try {
      Object lock = _programLockMap.get(fullName);

      while (lock == null) {
        lock = new Object();
        _programLockMap.putIfAbsent(fullName, lock);

        lock = _programLockMap.get(fullName);
      }

      synchronized (lock) {
        return parseImpl(path, fileName, line);
      }
    } finally {
      _programLockMap.remove(fullName);
    }
  }

  public QuercusPage parseImpl(Path path, string fileName, int line)
    
  {
    try {
      SoftReference<QuercusProgram> programRef = _programCache.get(path);

      QuercusProgram  program = programRef != null ? programRef.get() : null;

      bool isModified = false;

      if (program != null) {
        isModified = program.isModified();

        if (program.isCompilable()) {
        }
        else if (isModified)
          program.setCompilable(true);
        else {
          if (log.isLoggable(Level.FINE))
            log.fine(L.l("Quercus[{0}] loading interpreted page", path));

          return new InterpretedPage(program);
        }
      }

      if (program == null || isModified) {
        clearProgram(path, program);

        program = preloadProgram(path, fileName);

        if (program == null) {
          if (log.isLoggable(Level.FINE))
            log.fine(L.l("Quercus[{0}] parsing page", path));

          program = QuercusParser.parse(_quercus,
                                        path,
                                        _quercus.getScriptEncoding(),
                                        fileName,
                                        line);
        }

        _programCache.put(path, new SoftReference<QuercusProgram>(program));
      }

      if (program.getCompiledPage() != null)
        return program.getCompiledPage();

      return compilePage(program, path);
    } catch (IOException e) {
      throw e;
    } catch (RuntimeException e) {
      throw e;
    } catch (Throwable e) {
      throw new IOExceptionWrapper(e);
    }
  }

  public bool precompileExists(Path path)
  {
    return false;
  }

  protected QuercusProgram preloadProgram(Path path, string fileName)
  {
    return null;
  }

  protected void clearProgram(Path path, QuercusProgram program)
  {
    _programCache.remove(path);

    // php/0b36
    if (program != null)
      _quercus.clearDefinitionCache();
  }

  protected QuercusPage compilePage(QuercusProgram program, Path path)
  {
    if (log.isLoggable(Level.FINE))
      log.fine(L.l("Quercus[{0}] loading interpreted page", path));

    return new InterpretedPage(program);
  }

  public void close()
  {
    _isClosed = true;
  }
}

}
