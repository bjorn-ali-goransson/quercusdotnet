using System;
namespace QuercusDotNet.lib.simplexml {



































public abstract class SimpleXMLNode
{
  private const Logger log
    = Logger.getLogger(SimpleXMLNode.class.getName());

  private readonly L10N L = new L10N(SimpleXMLNode.class);

  protected QuercusClass _cls;
  protected SimpleView _view;

  private SimpleNamespaceContext _xpathNamespaceContext;

  public SimpleXMLNode(QuercusClass cls, SimpleView view)
  {
    _cls = cls;
    _view = view;
  }

  /**
   * public string getName()
   */
  @Name("getName")
  public string simplexml_getName()
  {
    return _view.getNodeName();
  }

  /**
   * public string __ToString()
   */
  public StringValue __ToString(Env env)
  {
    string str = _view.ToString(env);
    
    StringValue sb = env.createStringBuilder();
        
    if (sb.isUnicode()) {
      sb.append(str);
    }
    else {
      string encoding = _view.getEncoding();
      byte[] bytes;
      
      try {
        bytes = str.getBytes(encoding);
      }
      catch (UnsupportedEncodingException e) {
        throw new QuercusException(e);
      }
      
      sb.appendBytes(bytes, 0, bytes.length);
    }
    
    return sb;
  }

  /**
   * Implementation for getting the fields of this class.
   * i.e. <code>$a->foo</code>
   */
  public Value __getField(Env env, StringValue name)
  {
    SimpleView view = _view.getField(env, name);

    if (view == null) {
      return NullValue.NULL;
    }

    SimpleXMLElement e = new SimpleXMLElement(_cls, view);

    return e.wrapJava(env);
  }

  /**
   * Implementation for setting the fields of this class.
   * i.e. <code>$a->foo = "hello"</code>
   */
  public void __setField(Env env, StringValue name, Value value)
  {
    SimpleView view = _view.setField(env, name, value);
  }

  /**
   * Implementation for getting the indices of this class.
   * i.e. <code>$a->foo[0]</code>
   */
  public Value __get(Env env, Value indexV)
  {
    SimpleView view = _view.getIndex(env, indexV);

    if (view == null) {
      return NullValue.NULL;
    }

    SimpleXMLElement e = new SimpleXMLElement(_cls, view);

    return e.wrapJava(env);
  }

  /**
   * Implementation for setting the indices of this class.
   * i.e. <code>$a->foo[0] = "hello"</code>
   */
  public void __set(Env env, StringValue nameV, StringValue value)
  {
    _view.setIndex(env, nameV, value);
  }

  public int __count(Env env)
  {
    return _view.getCount();
  }

  /**
   * public SimpleXMLElement addChild( string $name [, string $value [, string $namespace ]] )
   */
  public SimpleXMLElement addChild(Env env,
                                   StringValue nameV,
                                   @Optional StringValue valueV,
                                   @Optional string namespace)
  {
    string name;
    string value;
    
    string encoding = _view.getEncoding();
    
    try {
      name = nameV.ToString(encoding);
      value = valueV.ToString(encoding);
    }
    catch (UnsupportedEncodingException e) {
      env.warning(e);
      
      return null;
    }
    
    SimpleView view = _view.addChild(env, name, value, namespace);

    SimpleXMLElement e = new SimpleXMLElement(_cls, view);

    return e;
  }

  /**
   * public void SimpleXMLElement::addAttribute ( string $name [, string $value [, string $namespace ]] )
   */
  public void addAttribute(Env env,
                           StringValue nameV,
                           @Optional StringValue valueV,
                           @Optional string namespace)
  {
    string name;
    string value;
    
    string encoding = _view.getEncoding();
    
    try {
      name = nameV.ToString(encoding);
      value = valueV.ToString(encoding);
    }
    catch (UnsupportedEncodingException e) {
      env.warning(e);
      
      return;
    }
    
    if (namespace != null) {
      if (namespace.length() == 0) {
        namespace = null;
      }
      else if (name.indexOf(':') <= 0) {
        env.warning(L.l("Adding attributes with namespaces requires attribute name with a prefix"));

        return;
      }
    }

    _view.addAttribute(env, name, value, namespace);
  }

  /**
   * public mixed SimpleXMLElement::asXML([ string $filename ])
   */
  public Value asXML(Env env, @Optional Value filename)
  {
    StringBuilder sb = new StringBuilder();
    if (! _view.toXml(env, sb)) {
      return BooleanValue.FALSE;
    }

    string encoding = _view.getEncoding();
    
    if (filename.isDefault()) {
      StringValue value = env.createStringBuilder();

      if (env.isUnicodeSemantics()) {
        value.append(sb.ToString());
      }
      else {
        byte []bytes;

        try {
          bytes = sb.ToString().getBytes(encoding);
        }
        catch (UnsupportedEncodingException e) {
          log.log(Level.FINE, e.getMessage(), e);
          env.warning(e);

          return BooleanValue.FALSE;
        }

        value.append(bytes);
      }

      return value;
    }
    else {
      string path = env.lookupPwd(filename);

      OutputStream os = null;

      try {
        os = path.openWrite();

        byte []bytes = sb.ToString().getBytes(encoding);
        os.write(bytes);

        return BooleanValue.TRUE;
      }
      catch (IOException e) {
        log.log(Level.FINE, e.getMessage(), e);
        env.warning(e);

        return BooleanValue.FALSE;
      }
      finally {
        if (os != null) {
          IoUtil.close(os);
        }
      }
    }
  }

  /**
   * public SimpleXMLElement SimpleXMLElement::attributes([ string $ns = NULL [, bool $is_prefix = false ]])
   */
  public Value attributes(Env env,
                          @Optional Value namespaceV,
                          @Optional bool isPrefix)
  {
    string namespace = null;
    if (! namespaceV.isNull()) {
      namespace = namespaceV.ToString();

      if (namespace != null && namespace.length() == 0) {
        namespace = null;
      }
    }

    AttributeListView view = _view.getAttributes(namespace);

    SimpleXMLElement e = new SimpleXMLElement(getQuercusClass(), view);

    return e.wrapJava(env);
  }

  /**
   * public SimpleXMLElement SimpleXMLElement::children([ string $ns [, bool $is_prefix = false ]])
   */
  public Value children(Env env,
                        @Optional Value namespaceV,
                        @Optional bool isPrefix)
  {
    string namespace = null;
    string prefix = null;

    if (! namespaceV.isNull()) {
      if (isPrefix) {
        prefix = namespaceV.ToString();

        if (prefix != null && prefix.length() == 0) {
          prefix = null;
        }
      }
      else {
        namespace = namespaceV.ToString();

        if (namespace != null && namespace.length() == 0) {
          namespace = null;
        }
      }
    }

    ChildrenView view = _view.getChildren(namespace, prefix);

    SimpleXMLElement e = new SimpleXMLElement(_cls, view);

    return e.wrapJava(env);
  }

  /**
   * public array SimpleXMLElement::getNamespaces ([ bool $recursive = false ] )
   */
  public ArrayValue getNamespaces(Env env, @Optional bool isRecursive)
  {
    ArrayValue array = new ArrayValueImpl();

    HashMap<String,String> usedMap = _view.getNamespaces(isRecursive, false, true);

    for (Map.Entry<String,String> entry : usedMap.entrySet()) {
      StringValue key = env.createString(entry.getKey());
      StringValue value = env.createString(entry.getValue());

      array.append(key, value);
    }

    return array;
  }

  /**
    * public array SimpleXMLElement::getDocNamespaces ([ bool $recursive = false [, bool $from_root = true ]] )
    */
  public ArrayValue getDocNamespaces(Env env,
                                     @Optional bool isRecursive,
                                     @Optional bool isFromRoot)
  {
    ArrayValue array = new ArrayValueImpl();

    HashMap<String,String> usedMap = _view.getNamespaces(isRecursive, isFromRoot, false);

    for (Map.Entry<String,String> entry : usedMap.entrySet()) {
      StringValue key = env.createString(entry.getKey());
      StringValue value = env.createString(entry.getValue());

      array.append(key, value);
    }

    return array;
  }

  /**
   *  public bool SimpleXMLElement::registerXPathNamespace ( string $prefix , string $ns )
   */
  public bool registerXPathNamespace(Env env, string prefix, string ns)
  {
    if (_xpathNamespaceContext == null) {
      _xpathNamespaceContext = new SimpleNamespaceContext();
    }

    _xpathNamespaceContext.addPrefix(prefix, ns);

    return true;
  }

  /**
   *  public array SimpleXMLElement::xpath( string $path )
   */
  public Value xpath(Env env, string expression)
  {
    if (_xpathNamespaceContext == null) {
      _xpathNamespaceContext = new SimpleNamespaceContext();
    }

    List<SimpleView> viewList = _view.xpath(env, _xpathNamespaceContext, expression);

    if (viewList == null) {
      return NullValue.NULL;
    }

    ArrayValue array = new ArrayValueImpl();

    for (SimpleView view : viewList) {
      SimpleXMLElement e = new SimpleXMLElement(_cls, view);

      Value value = e.wrapJava(env);

      array.append(value);
    }

    return array;
  }
  
  @JsonEncode
  public void jsonEncode(Env env, JsonEncodeContext context, StringValue sb)
  {    
    _view.jsonEncode(env, context, sb, _cls);
  }

  protected QuercusClass getQuercusClass()
  {
    return _cls;
  }

  protected Value wrapJava(Env env)
  {
    if (! "SimpleXMLElement".equals(_cls.getName())) {
      return new ObjectExtJavaValue(env, _cls, this, _cls.getJavaClassDef());
    }
    else {
      return new JavaValue(env, this, _cls.getJavaClassDef());
    }
  }
}
}
