namespace QuercusDotNet {
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


















public class Quercus
  : QuercusContext
{
  private const Logger log
    = Logger.getLogger(Quercus.class.getName());

  private string _fileName;
  private string []_argv;

  public Quercus()
   : base() {
  }

  //
  // command-line main
  //

  public static void main(String []args)
    
  {
    Quercus quercus = new Quercus();

    startMain(args, quercus);
  }

  public static void startMain(String []args, Quercus quercus)
    
  {
    if (! quercus.parseArgs(args)) {
      quercus.printUsage();
      return;
    }

    quercus.init();
    quercus.start();

    if (quercus.getFileName() != null) {
      quercus.execute();
    }
    else {
      InputStream is = System.in;

      ReadStream stream = new ReadStream(new InputStreamStream(is));

      quercus.execute(stream);
    }
  }

  public void printUsage()
  {
    System.out.println("usage: " + getClass().getName() + " [flags] <file> [php-args]");
    System.out.println(" -f            : Explicitly set the script filename.");
    System.out.println(" -d name=value : Sets a php ini value.");
  }

  @Override
  public string getSapiName()
  {
    return "cli";
  }

  public string getFileName()
  {
    return _fileName;
  }

  public void setFileName(String name)
  {
    _fileName = name;
  }

  protected bool parseArgs(String []args)
  {
    ArrayList<String> phpArgList = new ArrayList<String>();

    int i = 0;
    for (; i < args.length; i++) {
      if ("-d".equals(args[i])) {
        string arg = args[i + 1];
        int eqIndex = arg.indexOf('=');

        string name;
        string value;

        if (eqIndex >= 0) {
          name = arg.substring(0, eqIndex);
          value = arg.substring(eqIndex + 1);
        }
        else {
          name = arg;
          value = "";
        }

        i++;
        setIni(name, value);
      }
      else if ("-f".equals(args[i])) {
        _fileName = args[++i];
      }
      else if ("-q".equals(args[i])) {
        // quiet
      }
      else if ("-n".equals(args[i])) {
        // no php-pip
      }
      else if ("--".equals(args[i])) {
        break;
      }
      else if ("-h".equals(args[i])
               || "--help".equals(args[i])) {
        return false;
      }
      else if (args[i].startsWith("-")) {
        System.out.println("unknown option: " + args[i]);
        return false;
      }
      else {
        break;
      }
    }

    for (; i < args.length; i++) {
      phpArgList.add(args[i]);
    }

    _argv = phpArgList.toArray(new String[phpArgList.size()]);

    if (_fileName == null && _argv.length > 0)
      _fileName = _argv[0];

    return true;
  }

  protected String[] getArgv() {
    return _argv;
  }

  public void execute()
    
  {
    Path path = getPwd().lookup(_fileName);

    execute(path);
  }

  public void execute(String code)
    
  {
    Path path = new StringPath(code);

    execute(path);
  }

  public void execute(Path path)
    
  {
    QuercusPage page = parse(path);

    WriteStream os = new WriteStream(StdoutStream.create());

    os.setNewlineString("\n");
    os.setEncoding("iso-8859-1");

    Env env = createEnv(page, os, null, null);
    env.start();

    try {
      env.execute();
    } catch (QuercusDieException e) {
      log.log(Level.FINER, e.toString(), e);
    } catch (QuercusExitException e) {
      log.log(Level.FINER, e.toString(), e);
    } catch (QuercusErrorException e) {
      log.log(Level.FINER, e.toString(), e);
    } finally {
      env.close();

      os.flush();
    }
  }

  public void execute(ReadStream stream)
    
  {
    QuercusPage page = parse(stream);

    WriteStream os = new WriteStream(StdoutStream.create());

    os.setNewlineString("\n");
    os.setEncoding("iso-8859-1");

    Env env = createEnv(page, os, null, null);
    env.start();

    try {
      env.execute();
    } catch (QuercusDieException e) {
      log.log(Level.FINER, e.toString(), e);
    } catch (QuercusExitException e) {
      log.log(Level.FINER, e.toString(), e);
    } catch (QuercusErrorException e) {
      log.log(Level.FINER, e.toString(), e);
    } finally {
      env.close();

      os.flush();
    }
  }
}
}
