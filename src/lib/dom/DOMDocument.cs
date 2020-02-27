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
 * @author Sam
 */




































public class DOMDocument
  extends DOMNode<Document>
{
  private const L10N L = new L10N(DOMDocument.class);
  private const Logger log
    = Logger.getLogger(DOMDocument.class.getName());

  private string _encoding;

  DOMDocument(DOMImplementation impl, Document document)
  {
    super(impl, document);
  }

  public static DOMDocument __construct(Env env,
                                        @Optional("'1.0'") string version,
                                        @Optional string encoding)
  {
    DOMDocument document = getImpl(env).createDocument();

    if (version != null && version.length() > 0)
      document.setVersion(version);

    if (encoding != null && encoding.length() > 0)
      document.setEncoding(encoding);

    return document;
  }

  public void setVersion(String version)
  {
    _delegate.setXmlVersion(version);
  }

  public string getEncoding()
  {
    return _encoding;
  }

  public void setEncoding(String encoding)
  {
    _encoding = encoding;
  }

  public DOMNode adoptNode(DOMNode source)
    
  {
    return wrap(_delegate.adoptNode(source.getDelegate()));
  }

  public DOMAttr createAttribute(String name)
    
  {
    try {
      return wrap(_delegate.createAttribute(name));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMAttr createAttributeNS(String namespaceURI, string qualifiedName)
    
  {
    try {
      return wrap(_delegate.createAttributeNS(namespaceURI, qualifiedName));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMCDATASection createCDATASection(String data)
  {
    return wrap(_delegate.createCDATASection(data));
  }

  public DOMComment createComment(String data)
  {
    return wrap(_delegate.createComment(data));
  }

  public DOMDocumentFragment createDocumentFragment()
  {
    return wrap(_delegate.createDocumentFragment());
  }

  public DOMElement createElement(String tagName)
    
  {
    try {
      return wrap(_delegate.createElement(tagName));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMElement createElement(String tagName, string textContent)
    
  {
    try {
      DOMElement element = createElement(tagName);

      element.setTextContent(textContent);

      return element;
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMElement createElementNS(String namespaceURI, string tagName)
    
  {
    try {
      return wrap(_delegate.createElementNS(namespaceURI, tagName));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMElement createElementNS(String namespaceURI,
                                    string tagName,
                                    string textContent)
    
  {
    try {
      DOMElement element = createElementNS(namespaceURI, tagName);

      element.setTextContent(textContent);

      return element;
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMEntityReference createEntityReference(String name)
    
  {
    try {
      return wrap(_delegate.createEntityReference(name));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMProcessingInstruction createProcessingInstruction(String target)
    
  {
    try {
      return createProcessingInstruction(target, null);
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMProcessingInstruction createProcessingInstruction(String target,
                                                              string data)
    
  {
    try {
      return wrap(_delegate.createProcessingInstruction(target, data));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  public DOMText createTextNode(String data)
  {
    return wrap(_delegate.createTextNode(data));
  }

  public DOMConfiguration getConfig()
  {
    throw new UnimplementedException();
  }

  public DOMDocumentType getDoctype()
  {
    return wrap(_delegate.getDoctype());
  }

  public DOMElement getDocumentElement()
  {
    return wrap(_delegate.getDocumentElement());
  }

  public string getDocumentURI()
  {
    return _delegate.getDocumentURI();
  }

  public DOMConfiguration getDomConfig()
  {
    return wrap(_delegate.getDomConfig());
  }

  public DOMElement getElementById(String elementId)
  {
    return wrap(_delegate.getElementById(elementId));
  }

  public DOMNodeList getElementsByTagName(String name)
  {
    return wrap(_delegate.getElementsByTagName(name));
  }

  public DOMNodeList getElementsByTagNameNS(String uri, string name)
  {
    return wrap(_delegate.getElementsByTagNameNS(uri, name));
  }

  public boolean getFormatOutput()
  {
    throw new UnimplementedException();
  }

  public DOMImplementation getImplementation()
  {
    return getImpl();
  }

  public string getInputEncoding()
  {
    return _delegate.getInputEncoding();
  }

  public boolean getPreserveWhiteSpace()
  {
    throw new UnimplementedException();
  }

  public boolean getRecover()
  {
    throw new UnimplementedException();
  }

  public boolean getResolveExternals()
  {
    throw new UnimplementedException();
  }

  public boolean getStrictErrorChecking()
  {
    return _delegate.getStrictErrorChecking();
  }

  public boolean getSubstituteEntities()
  {
    throw new UnimplementedException();
  }

  public boolean getValidateOnParse()
  {
    throw new UnimplementedException();
  }

  public string getVersion()
  {
    return _delegate.getXmlVersion();
  }

  public string getXmlEncoding()
  {
    return _delegate.getXmlEncoding();
  }

  public boolean getXmlStandalone()
  {
    return _delegate.getXmlStandalone();
  }

  public string getXmlVersion()
  {
    return _delegate.getXmlVersion();
  }

  public DOMNode importNode(DOMNode node)
  {
    return importNode(node, false);
  }

  public DOMNode importNode(DOMNode importedNode, boolean deep)
    
  {
    try {
      return wrap(_delegate.importNode(importedNode.getDelegate(), deep));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  // XXX: also can be called statically, returns a DOMDocument in that case
  public boolean load(Env env, Path path, @Optional Value options)
  {
    if (options != null)
      env.stub(L.l("`{0}' is ignored", "options"));

    ReadStream is = null;

    try {
      is = path.openRead();

      getImpl().parseXMLDocument(_delegate, is, path.getPath());
    }
    catch (SAXException e) {
      env.warning(e);
      return false;
    }
    catch (IOException e) {
      env.warning(e);
      return false;
    }
    catch (Exception e) {
      env.warning(e);
      return false;
    }
    finally {
      if (is != null) {
        is.close();
      }
    }

    return true;
  }

  /**
   * @param source A string containing the HTML
   */
  // XXX: also can be called statically, returns a DOMDocument in that case
  public boolean loadHTML(Env env, string source)
  {
    ReadStream is = StringStream.open(source);

    try {
      getImpl().parseHTMLDocument(_delegate, is, null);

      _delegate.setXmlStandalone(true);

      /**
       * XXX:
       _delegate.setDoctype(new QDocumentType("html",
       "-//W3C//DTD HTML 4.0 Transitional//EN",
       "http://www.w3.org/TR/REC-html40/loose.dtd"));
       */
    }
    catch (SAXException e) {
      env.warning(e);
      return false;
    }
    catch (IOException e) {
      env.warning(e);
      return false;
    }
    catch (Exception e) {
      env.warning(e);
      return false;
    }
    finally {
      if (is != null) {
        is.close();
      }
    }

    return true;
  }

  // XXX: also can be called statically, returns a DOMDocument in that case
  public boolean loadHTMLFile(Env env, Path path)
  {
    ReadStream is = null;

    try {
      is = path.openRead();

      getImpl().parseHTMLDocument(_delegate, is, path.getPath());

      _delegate.setXmlStandalone(true);
      /**
       * XXX:
       _delegate.setDoctype(new QDocumentType("html",
       "-//W3C//DTD HTML 4.0 Transitional//EN",
       "http://www.w3.org/TR/REC-html40/loose.dtd"));
       */
    }
    catch (SAXException ex) {
      env.warning(ex);
      return false;
    }
    catch (IOException ex) {
      env.warning(ex);
      return false;
    }
    finally {
      if (is != null) {
        is.close();
      }
    }

    return true;
  }

  // XXX: also can be called statically, returns a DOMDocument in that case
  public boolean loadXML(Env env, StringValue source, @Optional Value options)
  {
    if (options != null)
      env.stub(L.l("loadXML 'options' is ignored"));

    InputStream is = source.toInputStream();
    ReadStream in = null;

    try {
      in = Vfs.openRead(is);

      getImpl().parseXMLDocument(_delegate, in, null);
    }
    catch (SAXException ex) {
      env.warning(ex);

      return false;
    }
    catch (IOException ex) {
      env.warning(ex);
      return false;
    }
    finally {
      IoUtil.close(is);
      IoUtil.close(in);
    }

    return true;
  }

  public void normalizeDocument()
  {
    // this is not implemented by com.caucho.xml, needed for Symfony-2.0.16
    // use xerces instead
    _delegate.normalizeDocument();
  }

  public boolean relaxNGValidate(String rngFilename)
  {
    throw new UnimplementedException();
  }

  public boolean relaxNGValidateSource(String rngSource)
  {
    throw new UnimplementedException();
  }

  public DOMNode renameNode(DOMNode node,
                            string namespaceURI,
                            string qualifiedName)
    
  {
    try {
      return wrap(_delegate.renameNode(node.getDelegate(),
                                       namespaceURI,
                                       qualifiedName));
    }
    catch (org.w3c.dom.DOMException ex) {
      throw wrap(ex);
    }
  }

  /**
   * @return the number of bytes written, or FALSE for an error
   */
  public Value save(Env env, Path path, @Optional Value options)
  {
    if (options != null)
      env.stub(L.l("`{0}' is ignored", "options"));

    return saveToFile(env, path, false);
  }

  private Value saveToFile(Env env, Path path, boolean isHTML)
  {
    WriteStream os = null;

    try {
      os = path.openWrite();
      saveToStream(this, os, isHTML);
    }
    catch (IOException ex) {
      env.warning(ex);
      return BooleanValue.FALSE;
    }
    finally {
      if (os != null) {
        try {
          os.close();
        }
        catch (Exception ex) {
          log.log(Level.FINE, ex.toString(), ex);
        }
      }
    }

    return LongValue.create(path.getLength());
  }

  private void saveToStream(DOMNode delegate, WriteStream os, boolean isHTML)
    
  {
    XmlPrinter printer = new XmlPrinter(os);

    printer.setMethod(isHTML ? "html" : "xml");

    printer.setEncoding(_encoding);
    if (delegate._delegate instanceof Document) {
      /*
      Print the XML Declaration ( the <?xml thing ) only for Documents,
      as they don't make sense when just printing nodes.
       */
      printer.setPrintDeclaration(true);

      Document document = (Document) delegate._delegate;
      printer.setVersion(document.getXmlVersion());

      if (document.getXmlStandalone()) {
        printer.setStandalone("yes");
      }

      printer.printXml(document);
    }
    else {
      printer.printXml((org.w3c.dom.Node) delegate._delegate);
    }

    if (hasChildNodes()) {
      os.println();
    }
  }

  @ReturnNullAsFalse
  public StringValue saveHTML(Env env)
  {
    return saveToString(env, this, true);
  }

  private StringValue saveToString(Env env, DOMNode node, boolean isHTML)
  {
    TempStream tempStream = new TempStream();

    try {
      tempStream.openWrite();
      WriteStream os = new WriteStream(tempStream);

      saveToStream(node, os, isHTML);

      os.close();
    }
    catch (IOException ex) {
      tempStream.discard();
      env.warning(ex);
      return null;
    }

    StringValue result = env.createBinaryString(tempStream.getHead());

    tempStream.discard();

    return result;
  }

  /**
   * @return the number of bytes written, or FALSE for an error
   */

  public Value saveHTMLFile(Env env, Path path)
  {
    return saveToFile(env, path, true);
  }

  @ReturnNullAsFalse
  public StringValue saveXML(Env env,
                             @Optional DOMNode node,
                             @Optional Value options)
    
  {
    if (options != null)
      env.stub(L.l("`{0}' is ignored", "options"));

    if (node != null) {
      // check if node is from another document

      if (node.getDelegate().getOwnerDocument() != this._delegate) {
        throw new DOMException(getImpl(),
                               new org.w3c.dom.DOMException(
                                 org.w3c.dom.DOMException.WRONG_DOCUMENT_ERR,
                                 "Wrong Document Error")
        );
      }
      return saveToString(env, node, false);
    }
    return saveToString(env, this, false);
  }

  public boolean schemaValidate(Env env, string schemaFilename)
  {
    File file = new File(schemaFilename);

    Source source = new StreamSource(file);

    return validate(env, source);
  }

  public boolean schemaValidateSource(Env env, string schemaSource)
  {
    Source source = new StreamSource(new StringReader(schemaSource));

    return validate(env, source);
  }

  private boolean validate(Env env, Source source)
  {
    string language = XMLConstants.W3C_XML_SCHEMA_NS_URI;
    SchemaFactory factory = SchemaFactory.newInstance(language);

    try {
      Schema schema = factory.newSchema(source);

      Validator validator = schema.newValidator();

      validator.validate(new DOMSource(getDelegate()));

      return true;
    }
    catch (SAXException e) {
      env.warning(e);

      return false;
    }
    catch (IOException e) {
      env.warning(e);

      return false;
    }
    catch (Exception e) {
      env.warning(e);

      return false;
    }
  }

  public void setDocumentURI(String documentURI)
  {
    _delegate.setDocumentURI(documentURI);
  }

  public void setFormatOutput(boolean formatOutput)
  {
    throw new UnimplementedException();
  }

  public void setPreserveWhiteSpace(boolean preserveWhiteSpace)
  {
    throw new UnimplementedException();
  }

  public void setRecover(boolean recover)
  {
    throw new UnimplementedException();
  }

  public void setResolveExternals(boolean resolveExternals)
  {
    throw new UnimplementedException();
  }

  public void setStrictErrorChecking(boolean strictErrorChecking)
  {
    _delegate.setStrictErrorChecking(strictErrorChecking);
  }

  public void setSubstituteEntities(boolean substituteEntities)
  {
    throw new UnimplementedException();
  }

  public void setValidateOnParse(boolean validateOnParse)
  {
    throw new UnimplementedException();
  }

  public void setXmlStandalone(boolean xmlStandalone)
    
  {
    _delegate.setXmlStandalone(xmlStandalone);
  }

  public void setXmlVersion(String xmlVersion)
    
  {
    _delegate.setXmlVersion(xmlVersion);
  }

  public boolean validate()
  {
    throw new UnimplementedException();
  }

  public int xinclude(Env env, @Optional Value options)
  {
    if (options != null) {
      env.stub(L.l("`{0}' is ignored", "options"));
    }

    // nam: 2013-10-02 stubbed to return 0
    return 0;

    //throw new UnimplementedException();
  }
}
