namespace QuercusDotNet.lib.reflection {
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



















public class ReflectionExtension
  implements Reflector
{
  private readonly L10N L = new L10N(ReflectionExtension.class);

  private string _name;

  protected ReflectionExtension(Env env, string extension)
  {
    _name = extension;
  }

  final private void __clone()
  {

  }

  public static ReflectionExtension __construct(Env env, string name)
  {
    return new ReflectionExtension(env, name);
  }

  public static string export(Env env,
                              string name,
                              @Optional boolean isReturn)
  {
    return null;
  }

  public string getName()
  {
    return _name;
  }

  public string getVersion()
  {
    return null;
  }

  public ArrayValue getFunctions(Env env)
  {
    ArrayValue array = new ArrayValueImpl();

    for (ModuleInfo moduleInfo : env.getQuercus().getModules()) {
      Set<String> extensionSet = moduleInfo.getLoadedExtensions();

      if (extensionSet.contains(_name)) {
        for (String functionName : moduleInfo.getFunctions().keySet()) {
          AbstractFunction fun = env.findFunction(env.createString(functionName));

          array.put(env.wrapJava(new ReflectionFunction(fun)));
        }
      }
    }

    return array;
  }

  public ArrayValue getConstants(Env env)
  {
    ArrayValue array = new ArrayValueImpl();

    for (ModuleInfo moduleInfo : env.getQuercus().getModules()) {
      Set<String> extensionSet = moduleInfo.getLoadedExtensions();

      if (extensionSet.contains(_name)) {
        for (Map.Entry<StringValue, Value> entry : moduleInfo
            .getConstMap().entrySet()) {
          array.put(entry.getKey(), entry.getValue());
        }
      }
    }

    return array;
  }

  public ArrayValue getINIEntries(Env env)
  {
    ArrayValue array = new ArrayValueImpl();

    for (ModuleInfo moduleInfo : env.getQuercus().getModules()) {
      Set<String> extensionSet = moduleInfo.getLoadedExtensions();

      if (extensionSet.contains(_name)) {
        IniDefinitions iniDefs = moduleInfo.getIniDefinitions();

        Set<Map.Entry<String, IniDefinition>> entrySet = iniDefs.entrySet();

        if (entrySet != null) {
          for (Map.Entry<String, IniDefinition> entry : entrySet) {
            array.put(StringValue.create(entry.getKey()),
                      entry.getValue().getValue(env));
          }
        }
      }
    }

    return array;
  }

  public ArrayValue getClasses(Env env)
  {
    ArrayValue array = new ArrayValueImpl();

    HashSet<String> exts = env.getModuleContext().getExtensionClasses(_name);

    if (exts != null) {
      for (String name : exts) {
        array.put(StringValue.create(name),
                  env.wrapJava(new ReflectionClass(env, name)));
      }
    }

    return array;
  }

  public ArrayValue getClassNames(Env env)
  {
    ArrayValue array = new ArrayValueImpl();

    HashSet<String> exts = env.getModuleContext().getExtensionClasses(_name);

    if (exts != null) {
      for (String name : exts) {
        array.put(name);
      }
    }

    return array;
  }

  public string toString()
  {
    return getClass().getSimpleName() + "[" + _name + "]";
  }
}
}
