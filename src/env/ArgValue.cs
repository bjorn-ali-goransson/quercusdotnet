using System;
namespace QuercusDotNet.Env{
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
 * Represents an array-get argument which might be a call to a reference.
 *
 * foo($a[0]), where is not known if foo is defined as foo($a) or foo(&amp;$a)
 */
abstract public class ArgValue : Value
{
  @Override
  public Value toValue()
  {
    return toLocalValue();
  }

  public override ArrayValue toArrayValue(Env env)
  {
    return toLocalValue().toArrayValue(env);
  }

  public override StringValue toStringValue()
  {
    return toLocalValue().toStringValue();
  }

  public override StringValue toStringValue(Env env)
  {
    return toLocalValue().toStringValue(env);
  }

  public override char toChar()
  {
    return toLocalValue().toChar();
  }

  public override string toJavaString()
  {
    return toLocalValue().toJavaString();
  }

  public override Object toJavaObject()
  {
    return toLocalValue().toJavaObject();
  }

  public override bool toBoolean()
  {
    return toLocalValue().toBoolean();
  }

  public override double toDouble()
  {
    return toLocalValue().toDouble();
  }

  public override long toLong()
  {
    return toLocalValue().toLong();
  }

  public override ArrayValue toArray()
  {
    return toLocalValue().toArray();
  }

  public override Value toAutoObject(Env env)
  {
    return toLocalValue().toObject(env);
  }

  public override InputStream toInputStream()
  {
    return toLocalValue().toInputStream();
  }

  protected override void varDumpImpl(Env env,
                             WriteStream out,
                             int depth,
                             IdentityHashMap<Value, String> valueSet)
    
  {
    toLocalValue().varDumpImpl(env, out, depth, valueSet);
  }

  protected override void printRImpl(Env env,
                            WriteStream out,
                            int depth,
                            IdentityHashMap<Value, String> valueSet)
    
  {
    toLocalValue().printRImpl(env, out, depth, valueSet);
  }
}

}
