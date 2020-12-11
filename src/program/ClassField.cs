using System;
namespace QuercusDotNet.Program{
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
 * @author Nam Nguyen
 */









public class ClassField
{
  private StringValue _name;
  private StringValue _canonicalName;
  private string _declaringClassName;

  private Expr _initValue;
  private bool _isTraitField;

  private string _comment;

  public ClassField(StringValue name,
                    StringValue canonicalName,
                    string declaringClassName,
                    Expr initValue,
                    string comment,
                    bool isTraitField)
  {
    _name = name;
    _canonicalName = canonicalName;
    _declaringClassName = declaringClassName;

    _initValue = initValue;

    _comment = comment;

    _isTraitField = isTraitField;
  }

  public ClassField(StringValue name,
                    string declaringClassName,
                    Expr initValue,
                    FieldVisibility visibility,
                    string comment,
                    bool isTraitField)
  {
    _name = name;
    _declaringClassName = declaringClassName;

    _initValue = initValue;

    _comment = comment;

    _isTraitField = isTraitField;

    if (visibility.isProtected()) {
      StringValue sb = name.createStringBuilder();

      _canonicalName = createProtectedCanonicalName(sb, name);
    }
    else if (visibility.isPrivate()) {
      StringValue sb = name.createStringBuilder();

      _canonicalName = createPrivateCanonicalName(sb, name, declaringClassName);
    }
    else {
      _canonicalName = name;
    }
  }

  public static StringValue getOrdinaryName(StringValue canonicalName)
  {
    int p = canonicalName.lastIndexOf('\u0000');

    if (p >= 0) {
      return canonicalName.substring(p + 1);
    }
    else {
      return canonicalName;
    }
  }

  public static StringValue getCanonicalName(Env env,
                                             string classContext,
                                             StringValue name)
  {
    ClassDef classDef = env.findClassDef(classContext);
    ClassField entry = classDef.getField(name);

    if (entry != null) {
      return entry.getCanonicalName();
    }

    return name;
  }

  public static StringValue createProtectedCanonicalName(StringValue sb,
                                                         StringValue name)
  {
    sb.append('\u0000');
    sb.append('*');
    sb.append('\u0000');
    sb.append(name);

    return sb;
  }

  public static StringValue createPrivateCanonicalName(StringValue name,
                                                       string declaringClass)
  {
    return createPrivateCanonicalName(name.createStringBuilder(), name, declaringClass);
  }

  public static StringValue createPrivateCanonicalName(StringValue sb,
                                                       StringValue name,
                                                       string declaringClass)
  {
    sb.append('\u0000');
    sb.append(declaringClass);
    sb.append('\u0000');

    sb.append(name);

    return sb;
  }

  public static bool isPublic(StringValue canonicalName)
  {
    int p = canonicalName.lastIndexOf('\u0000');

    return p < 0;
  }

  public static bool isPrivate(StringValue canonicalName)
  {
    return canonicalName.length() > 3
           && canonicalName[0] == '\u0000'
           && canonicalName[1] != '*';
  }

  public static bool isProtected(StringValue canonicalName)
  {
    return canonicalName.length() > 3
           && canonicalName[0] == '\u0000'
           && canonicalName[1] == '*'
           && canonicalName[2] == '\u0000';
  }

  public static StringValue getDeclaringClass(StringValue sb,
                                              StringValue canonicalName)
  {
    if (canonicalName[0] != '\u0000') {
      return sb;
    }

    int len = canonicalName.length();
    for (int i = 1; i < len; i++) {
      char ch = canonicalName[i];

      if (ch == '\u0000') {
        break;
      }
      else {
        sb.append(ch);
      }
    }

    return sb;
  }

  public string getDeclaringClassName()
  {
    return _declaringClassName;
  }

  public StringValue getName()
  {
    return _name;
  }

  public StringValue getCanonicalName()
  {
    return _canonicalName;
  }

  public Expr getInitExpr()
  {
    return _initValue;
  }

  public void setInitExpr(Expr initValue)
  {
    _initValue = initValue;
  }

  public Value evalInitExpr(Env env)
  {
    return _initValue.eval(env).copy();
  }

  public bool isPublic()
  {
    return ! isProtected() && ! isPrivate();
  }

  public bool isProtected()
  {
    return isProtected(_canonicalName);
  }

  public bool isPrivate()
  {
    return isPrivate(_canonicalName);
  }

  public bool isTraitField()
  {
    return _isTraitField;
  }

  public string getComment()
  {
    return _comment;
  }

  public override string ToString()
  {
    string access = "";

    if (isPrivate()) {
      access = "private:";
    }
    else if (isProtected()) {
      access = "protected:";
    }

    return getClass().getSimpleName() + "[" + _declaringClassName + ":" + access + _name + "]";
  }
}
}
