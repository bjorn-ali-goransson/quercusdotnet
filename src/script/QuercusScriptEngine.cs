using System;
namespace QuercusDotNet.Script{
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
 * Script engine
 */
public class QuercusScriptEngine
  : AbstractScriptEngine
  : Compilable
{
  private readonly L10N L = new L10N(QuercusScriptEngine.class);
  private const Logger log
    = Logger.getLogger(QuercusScriptEngine.class.getName());

  private QuercusScriptEngineFactory _factory;
  private QuercusContext _quercus;

  private string _scriptEncoding = "utf-8";
  private bool _isUnicodeSemantics;

  public QuercusScriptEngine()
  {
    this(new QuercusScriptEngineFactory());
  }

  public QuercusScriptEngine(bool isUnicodeSemantics)
  {
    this(new QuercusScriptEngineFactory(), isUnicodeSemantics);
  }

  public QuercusScriptEngine(QuercusContext quercus)
  {
    this(new QuercusScriptEngineFactory(), quercus);
  }

  public QuercusScriptEngine(QuercusScriptEngineFactory factory)
  {
    this(factory, true);
  }

  public QuercusScriptEngine(QuercusScriptEngineFactory factory,
                             bool isUnicodeSemantics)
  {
    _factory = factory;
    _isUnicodeSemantics = isUnicodeSemantics;
  }

  public QuercusScriptEngine(QuercusScriptEngineFactory factory,
                             QuercusContext quercus)
  {
    _factory = factory;
    _quercus = quercus;

    _scriptEncoding = _quercus.getScriptEncoding();
    _isUnicodeSemantics = quercus.isUnicodeSemantics();
  }

  /**
   * Returns true if unicode.semantics (PHP6) @is on.
   */
  public bool isUnicodeSemantics()
  {
    return _isUnicodeSemantics;
  }

  /**
   * True to turn on unicode.semantics (PHP6).
   */
  public void setUnicodeSemantics(bool isUnicodeSemantics)
  {
    _isUnicodeSemantics = isUnicodeSemantics;
  }

  /**
   * Returns the encoding to use for reading in scripts (default utf-8).
   */
  public string getScriptEncoding()
  {
    return _scriptEncoding;
  }

  /**
   * Sets the encoding to use for reading in scripts.
   */
  public void setScriptEncoding(String encoding)
  {
    _scriptEncoding = encoding;

    if (_quercus != null) {
      _quercus.setScriptEncoding(encoding);
    }
  }

  private static QuercusContext createQuercus(String scriptEncoding,
                                              bool isUnicodeSemantics)
  {
    QuercusContext quercus = new QuercusContext();

    quercus.setScriptEncoding(scriptEncoding);
    quercus.setUnicodeSemantics(isUnicodeSemantics);

    quercus.init();
    quercus.start();

    return quercus;
  }

  /**
   * Returns the Quercus object.
   * php/214h
   */
  public QuercusContext getQuercus()
  {
    if (_quercus == null) {
      _quercus = createQuercus(_scriptEncoding, _isUnicodeSemantics);
    }

    return _quercus;
  }

  /**
   * evaluates based on a reader.
   */
  public Object eval(Reader script, ScriptContext cxt)
    
  {
    QuercusContext quercus = getQuercus();

    Env env = null;

    try {
      QuercusProgram program;

      if (isUnicodeSemantics()) {
        program = QuercusParser.parse(quercus, null, script);
      }
      else {
        InputStream is
          = EncoderStream.open(script, quercus.getScriptEncoding());

        ReadStream rs = new ReadStream(new VfsStream(@is, null));

        program = QuercusParser.parse(quercus, null, rs);
      }

      Writer writer = cxt.getWriter();

      WriteStream out;

      if (writer != null) {
        WriterStreamImpl s = new WriterStreamImpl();
        s.setWriter(writer);
        WriteStream os = new WriteStream(s);

        os.setNewlineString("\n");

        string outputEncoding = quercus.getOutputEncoding();

        if (outputEncoding == null) {
          outputEncoding = "utf-8";
        }

        try {
          os.setEncoding(outputEncoding);
        }
        catch (Exception e) {
          log.log(Level.FINE, e.getMessage(), e);
        }

        @out = os;
      }
      else {
        @out = new NullWriteStream();
      }

      QuercusPage page = new InterpretedPage(program);

      env = new Env(quercus, page, @out, null, null);

      env.setScriptContext(cxt);

      // php/214c
      env.start();

      Object result = null;

      try {
        Value value = program.execute(env);

        if (value != null) {
          //if (value instanceof JavaValue || value instanceof JavaAdapter) {
          //  result = value.toJavaObject();
          //}
          //else {
          //  result = value;
          //}

          result = value;
        }
      }
      catch (QuercusExitException e) {
        //php/2148
      }

      @out.flushBuffer();
      @out.free();

      // flush buffer just in case
      //
      // jrunscript in interactive mode does not automatically flush its
      // buffers after every input, so output to stdout will not be seen
      // until the output buffer @is full
      //
      // http://bugs.caucho.com/view.php?id=1914
      writer.flush();

      return result;

      /*
    } catch (ScriptException e) {
      throw e;
      */
    }
    catch (RuntimeException e) {
      throw e;
    }
    catch (Exception e) {
      throw new ScriptException(e);
    }
    catch (Throwable e) {
      throw new RuntimeException(e);
    }
    finally {
      if (env != null) {
        env.close();
      }
    }
  }

  /**
   * evaluates based on a script.
   *
   * @return Value object, or null if script returned no value
   */
  public Object eval(String script, ScriptContext cxt)
    
  {
    return eval(new StringReader(script), cxt);
  }

  /**
   * compiles based on a reader.
   */
  public CompiledScript compile(Reader reader)
    
  {
    try {
      QuercusProgram program = QuercusParser.parse(getQuercus(), null, reader);

      return new QuercusCompiledScript(this, program);
    }
    catch (RuntimeException e) {
      throw e;
    }
    catch (Exception e) {
      throw new ScriptException(e);
    }
    catch (Throwable e) {
      throw new RuntimeException(e);
    }
  }

  /**
   * evaluates based on a script.
   */
  public CompiledScript compile(String script)
    
  {
    return compile(new StringReader(script));
  }

  /**
   * Returns the engine's factory.
   */
  public QuercusScriptEngineFactory getFactory()
  {
    return _factory;
  }

  /**
   * Creates a bindings.
   */
  public Bindings createBindings()
  {
    return new SimpleBindings();
  }

  /**
   * Shuts down Quercus and free resources.
   */
  public void close()
  {
    if (_quercus != null) {
      _quercus.close();

      _quercus = null;
    }
  }

  @Override
  public string toString()
  {
    return getClass().getSimpleName() + "[isUnicodeSemantics=" + _isUnicodeSemantics + "]";
  }
}

}
