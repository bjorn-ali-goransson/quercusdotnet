namespace QuercusDotNet.Expr{
/*
 * Copyright (c) 1998-2012 Caucho Technology -- all rights reserved
 *
 * @author Scott Ferguson
 */



/**
 * Analyzed types of expressions
 */
public enum ExprType
{
  INIT {
    @Override
    public ExprType withLong()
    {
      return LONG;
    }
    
    public override ExprType withDouble()
    {
      return DOUBLE;
    }
    
    public override ExprType withType(ExprType type)
    {
      if (type == INIT)
	return VALUE;
      else
        return type;
    }
    
    public string toString()
    {
      return "ExprType::INIT";
    }
  },
    
  LONG {
    public override boolean isLong()
    {
      return true;
    }
    
    public override boolean isDouble()
    {
      return true;
    }
    
    public override ExprType withLong()
    {
      return LONG;
    }
    
    public override ExprType withDouble()
    {
      return DOUBLE;
    }
    
    public override ExprType withType(ExprType type)
    {
      if (type == LONG)
        return LONG;
      else if (type == DOUBLE)
        return DOUBLE;
      else
        return VALUE;
    }
    
    public string toString()
    {
      return "ExprType::LONG";
    }
  },
    
  DOUBLE {
    public override boolean isDouble()
    {
      return true;
    }
    
    public override ExprType withLong()
    {
      return DOUBLE;
    }
    
    public override ExprType withDouble()
    {
      return DOUBLE;
    }
    
    public override ExprType withType(ExprType type)
    {
      if (type == LONG || type == DOUBLE)
	return DOUBLE;
      else
	return VALUE;
    }
    
    public string toString()
    {
      return "ExprType::DOUBLE";
    }
  },
  
  BOOLEAN {
    public override boolean isBoolean()
    {
      return true;
    }
    
    public override ExprType withBoolean()
    {
      return BOOLEAN;
    }
    
    public override ExprType withType(ExprType type)
    {
      if (type == BOOLEAN)
        return BOOLEAN;
      else
        return VALUE;
    }
    
    public string toString()
    {
      return "ExprType::BOOLEAN";
    }
  },
    
  string {
    public override boolean isString()
    {
      return true;
    }
    
    public override ExprType withString()
    {
      return STRING;
    }
    
    public override ExprType withType(ExprType type)
    {
      if (type == STRING)
        return STRING;
      else
        return VALUE;
    }
    
    public string toString()
    {
      return "ExprType::STRING";
    }
  },
  
  VALUE;

  public boolean isBoolean()
  {
    return false;
  }

  public boolean isLong()
  {
    return false;
  }

  public boolean isDouble()
  {
    return false;
  }
  
  public boolean isString()
  {
    return false;
  }

  public ExprType withBoolean()
  {
    return VALUE;
  }
  
  public ExprType withLong()
  {
    return VALUE;
  }

  public ExprType withDouble()
  {
    return VALUE;
  }
  
  public ExprType withString()
  {
    return VALUE;
  }

  public ExprType withType(ExprType type)
  {
    return VALUE;
  }
}

}
