using System;
namespace QuercusDotNet.lib.pdf {
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
 * PHP PDF routines.
 */
public class PDFModule : AbstractQuercusModule {
  private readonly L10N L = new L10N(PDFModule.class);

  private const Logger log =
    Logger.getLogger(PDFModule.class.getName());

  /**
   * Returns true for the mysql extension.
   */
  public string []getLoadedExtensions()
  {
    return new String[] { "pdf" };
  }

  /**
   * Activates a created element.
   */
  public static bool pdf_activate_item(Env env,
                                          @NotNull PDF pdf,
                                          int id)
  {
    env.stub("pdf_activate_item");

    return false;
  }

  /**
   * Adds an annotation
   */
  public static bool pdf_add_annotation(Env env,
                                          @NotNull PDF pdf)
  {
    env.stub("pdf_add_annotation");

    return false;
  }

  /**
   * Adds an bookmarkannotation
   */
  public static bool pdf_add_bookmark(Env env,
                                         @NotNull PDF pdf)
  {
    env.stub("pdf_add_bookmark");

    return false;
  }

  /**
   * Adds an launchlink
   */
  public static bool pdf_add_launchlink(Env env,
                                           @NotNull PDF pdf,
                                           double llx,
                                           double lly,
                                           double urx,
                                           double ury,
                                           string filename)
  {
    env.stub("pdf_add_launchlink");

    return false;
  }

  /**
   * Adds a locallink
   */
  public static bool pdf_add_locallink(Env env,
                                          @NotNull PDF pdf,
                                          double llx,
                                          double lly,
                                          double urx,
                                          double ury,
                                          int page,
                                          string dest)
  {
    env.stub("pdf_add_locallink");

    return false;
  }

  /**
   * Creates a named destination
   */
  public static bool pdf_add_nameddest(Env env,
                                          @NotNull PDF pdf,
                                          string name,
                                          @Optional string optlist)
  {
    env.stub("pdf_add_nameddest");

    return false;
  }

  /**
   * Creates an annotation
   */
  public static bool pdf_add_note(Env env,
                                     @NotNull PDF pdf,
                                     double llx,
                                     double lly,
                                     double urx,
                                     double ury,
                                     string contents,
                                     string title,
                                     string icon,
                                     int open)
  {
    env.stub("pdf_add_note");

    return false;
  }

  /**
   * Creates an outline
   */
  public static bool pdf_add_outline(Env env,
                                        @NotNull PDF pdf,
                                        string name,
                                        @Optional string optlist)
  {
    env.stub("pdf_add_outline");

    return false;
  }

  /**
   * Creates a file link annotation
   */
  public static bool pdf_add_pdflink(Env env,
                                        @NotNull PDF pdf,
                                        double llx,
                                        double lly,
                                        double urx,
                                        double ury,
                                        string filename,
                                        int page,
                                        string dest)
  {
    env.stub("pdf_add_pdflink");

    return false;
  }

  /**
   * Adds a thumbnail
   */
  public static bool pdf_add_thumbnail(Env env,
                                          @NotNull PDF pdf,
                                          @NotNull PDFImage image)
  {
    env.stub("pdf_add_thumbnail");

    return false;
  }

  /**
   * Adds a web link
   */
  public static bool pdf_add_weblink(Env env,
                                        @NotNull PDF pdf,
                                        double llx,
                                        double lly,
                                        double urx,
                                        double ury,
                                        string url)
  {
    env.stub("pdf_add_weblink");

    return false;
  }

  /**
   * Creates a counterclockwise arc
   */
  public static bool pdf_arc(@NotNull PDF pdf,
                                double x1, double y1,
                                double r, double a, double b)
  {
    if (pdf == null)
      return false;
      
    return pdf.arc(x1, y1, r, a, b);
  }

  /**
   * Creates a clockwise arc
   */
  public static bool pdf_arcn(@NotNull PDF pdf,
                                double x1, double y1,
                                double r, double a, double b)
  {
    if (pdf == null)
      return false;
    
    return pdf.arcn(x1, y1, r, a, b);
  }

  /**
   * Adds a file attachment
   */
  public static bool pdf_attach_file(Env env,
                                        @NotNull PDF pdf,
                                        double llx,
                                        double lly,
                                        double urx,
                                        double ury,
                                        string filename,
                                        string description,
                                        string author,
                                        string mimetype,
                                        string icon)
  {
    env.stub("pdf_attach_file");
    
    return false;
  }

  /**
   * Starts the document.
   */
  public static bool pdf_begin_document(@NotNull PDF pdf,
                                           @Optional string fileName,
                                           @Optional string optList)
  {
    try {
      if (pdf == null)
        return false;
    
      return pdf.begin_document(fileName, optList);
    } catch (IOException e) {
      throw new QuercusModuleException(e);
    }
  }

  /**
   * Starts a font definition
   */
  public static bool pdf_begin_font(Env env,
                                       @NotNull PDF pdf,
                                       string fileName,
                                       double a,
                                       double b,
                                       double c,
                                       double d,
                                       double e,
                                       double f,
                                       @Optional string optList)
  {
    env.stub("pdf_begin_font");
    
    return false;
  }

  /**
   * Starts a glyph definition
   */
  public static bool pdf_begin_glyph(Env env,
                                        @NotNull PDF pdf,
                                        string glyphname,
                                        double wx,
                                        double llx,
                                        double lly,
                                        double urx,
                                        double ury)
  {
    env.stub("pdf_begin_glyph");
    
    return false;
  }

  /**
   * Starts a structure element
   */
  public static bool pdf_begin_item(Env env,
                                       @NotNull PDF pdf,
                                       string tag,
                                       string optlist)
  {
    env.stub("pdf_begin_item");
    
    return false;
  }

  /**
   * Starts a pdf layer
   */
  public static bool pdf_begin_layer(Env env,
                                        @NotNull PDF pdf,
                                        int layer)
  {
    env.stub("pdf_begin_layer");
    
    return false;
  }

  /**
   * Starts the page.
   */
  public static bool pdf_begin_page_ext(@NotNull PDF pdf,
                                           double w, double h,
                                           @Optional string optlist)
  {
    try {
      if (pdf == null)
        return false;
    
      return pdf.begin_page_ext(w, h, optlist);
    } catch (IOException e) {
      throw new QuercusModuleException(e);
    }
  }

  /**
   * Starts the page.
   */
  public static bool pdf_begin_page(@NotNull PDF pdf,
                                       double w, double h)
  {
    try {
      if (pdf == null)
        return false;
    
      return pdf.begin_page(w, h);
    } catch (IOException e) {
      throw new QuercusModuleException(e);
    }
  }

  /**
   * Starts a pattern
   */
  public static bool pdf_begin_pattern(Env env,
                                          @NotNull PDF pdf,
                                          double w,
                                          double h,
                                          double xStep,
                                          double yStep,
                                          int paintType)
  {
    env.stub("pdf_begin_pattern");
    
    return false;
  }

  /**
   * Starts a template
   */
  public static bool pdf_begin_template(Env env,
                                          @NotNull PDF pdf,
                                          double w,
                                          double h)
  {
    env.stub("pdf_begin_template");
    
    return false;
  }

  /**
   * Draws a circle
   */
  public static bool pdf_circle(@NotNull PDF pdf,
                                   double x,
                                   double y,
                                   double r)
  {
    if (pdf == null)
      return false;
    
    return pdf.circle(x, y, r);
  }

  /**
   * Clips the path.
   */
  public static bool pdf_clip(@NotNull PDF pdf)
  {
    if (pdf == null)
      return false;
    
    return pdf.clip();
  }

  /**
   * Closes an image
   */
  public static bool pdf_close_image(Env env,
                                        @NotNull PDF pdf,
                                        PDFImage image)
  {
    env.stub("pdf_close_image");
    
    return false;
  }

  /**
   * Closes a page
   */
  public static bool pdf_close_pdi_page(Env env,
                                       @NotNull PDF pdf,
                                       int page)
  {
    env.stub("pdf_close_pdi_page");
    
    return false;
  }

  /**
   * Closes a document
   */
  public static bool pdf_close_pdi(Env env,
                                      @NotNull PDF pdf,
                                      int doc)
  {
    env.stub("pdf_close_pdi");
    
    return false;
  }

  /**
   * Closes the pdf document.
   */
  public static bool pdf_close(@NotNull PDF pdf)
  {
    try {
      if (pdf == null)
        return false;
    
      return pdf.close();
    } catch (IOException e) {
      throw new QuercusModuleException(e);
    }
  }

  /**
   * Closes the path, fill, and stroke it.
   */
  public static bool pdf_closepath_fill_stroke(@NotNull PDF pdf)
  {
    if (pdf == null)
      return false;
    
    return pdf.closepath_fill_stroke();
  }

  /**
   * Closes the path and stroke it.
   */
  public static bool pdf_closepath_stroke(@NotNull PDF pdf)
  {
    if (pdf == null)
      return false;
    
    return pdf.closepath_stroke();
  }

  /**
   * Closes the path.
   */
  public static bool pdf_closepath(@NotNull PDF pdf)
  {
    if (pdf == null)
      return false;
    
    return pdf.closepath();
  }

  /**
   * Concatenates a transformation matrix
   */
  public static bool pdf_concat(@NotNull PDF pdf,
                                   double a,
                                   double b,
                                   double c,
                                   double d,
                                   double e,
                                   double f)
  {
    if (pdf == null)
      return false;
    
    return pdf.concat(a, b, c, d, e, f);
  }

  /**
   * Continues text at the next line.
   */
  public static bool pdf_continue_text(@NotNull PDF pdf,
                                          string text)
  {
    if (pdf == null)
      return false;
    
    return pdf.continue_text(text);
  }

  /**
   * Creates an action.
   */
  public static bool pdf_create_action(Env env,
                                          @NotNull PDF pdf,
                                          string type,
                                          @Optional string optList)
  {
    env.stub("pdf_create_action");
    
    return false;
  }

  /**
   * Creates a rectangular annotation
   */
  public static bool pdf_create_annotation(Env env,
                                              @NotNull PDF pdf,
                                              double llx,
                                              double lly,
                                              double urx,
                                              double ury,
                                              string type,
                                              @Optional string optList)
  {
    env.stub("pdf_create_annotation");
    
    return false;
  }

  /**
   * Creates a bookmark
   */
  public static bool pdf_create_bookmark(Env env,
                                            @NotNull PDF pdf,
                                            string text,
                                            @Optional string optList)
  {
    env.stub("pdf_create_bookmark");
    
    return false;
  }

  /**
   * Creates a form field.
   */
  public static bool pdf_create_field(Env env,
                                         @NotNull PDF pdf,
                                         double llx,
                                         double lly,
                                         double urx,
                                         double ury,
                                         string name,
                                         string type,
                                         @Optional string optList)
  {
    env.stub("pdf_create_field");
    
    return false;
  }

  /**
   * Creates a form field group.
   */
  public static bool pdf_create_fieldgroup(Env env,
                                              @NotNull PDF pdf,
                                              string name,
                                              @Optional string optList)
  {
    env.stub("pdf_create_fieldgroup");
    
    return false;
  }

  /**
   * Creates a graphics state
   */
  public static bool pdf_create_gstate(Env env,
                                          @NotNull PDF pdf,
                                          @Optional string optList)
  {
    env.stub("pdf_create_gstate");
    
    return false;
  }

  /**
   * Creates a virtual file
   */
  public static bool pdf_create_pvf(Env env,
                                       @NotNull PDF pdf,
                                       string filename,
                                       string data,
                                       @Optional string optList)
  {
    env.stub("pdf_create_pvf");
    
    return false;
  }

  /**
   * Creates a textflow object
   */
  public static bool pdf_create_textflow(Env env,
                                            @NotNull PDF pdf,
                                            string text,
                                            @Optional string optList)
  {
    env.stub("pdf_create_textflow");
    
    return false;
  }

  /**
   * Draws a bezier curve
   */
  public static bool pdf_curveto(@NotNull PDF pdf,
                                    double x1,
                                    double y1,
                                    double x2,
                                    double y2,
                                    double x3,
                                    double y3)
  {
    if (pdf == null)
      return false;
    
    return pdf.curveto(x1, y1, x2, y2, x3, y3);
  }

  /**
   * Creates a layer
   */
  public static bool pdf_define_layer(Env env,
                                         @NotNull PDF pdf,
                                         string name,
                                         @Optional string optList)
  {
    env.stub("pdf_define_layer");
    
    return false;
  }

  /**
   * Delete a virtual file
   */
  public static bool pdf_delete_pvf(Env env,
                                       @NotNull PDF pdf,
                                       string name)
  {
    env.stub("pdf_delete_pvf");
    
    return false;
  }

  /**
   * Delete a textflow object
   */
  public static bool pdf_delete_textflow(Env env,
                                            @NotNull PDF pdf,
                                            int textflow)
  {
    env.stub("pdf_delete_textflow");
    
    return false;
  }

  /**
   * Delete the pdf object.
   */
  public static bool pdf_delete(@NotNull PDF pdf)
  {
    try {
      if (pdf == null)
        return false;
    
      return pdf.delete();
    } catch (IOException e) {
      throw new QuercusModuleException(e);
    }
  }

  /**
   * Adds a glyph to a custom encoding.
   */
  public static bool pdf_encoding_set_char(Env env,
                                              @NotNull PDF pdf,
                                              string encoding,
                                              int slow,
                                              string glyphname,
                                              int uv)
  {
    env.stub("pdf_encoding_set_char");

    return false;
  }

  /**
   * Completes the document.
   */
  public static bool pdf_end_document(@NotNull PDF pdf,
                                         @Optional string optlist)
  {
    try {
      if (pdf == null)
        return false;
    
      return pdf.end_document(optlist);
    } catch (IOException e) {
      throw new QuercusModuleException(e);
    }
  }

  /**
   * Completes a font definition
   */
  public static bool pdf_end_font(Env env,
                                     @NotNull PDF pdf)
  {
    env.stub("pdf_end_font");
    
    return false;
  }

  /**
   * Completes a glyph definition
   */
  public static bool pdf_end_glyph(Env env,
                                     @NotNull PDF pdf)
  {
    env.stub("pdf_end_glyph");
    
    return false;
  }

  /**
   * Completes a structure element.
   */
  public static bool pdf_end_item(Env env,
                                     @NotNull PDF pdf,
                                     int id)
  {
    env.stub("pdf_end_item");
    
    return false;
  }

  /**
   * Completes a layer
   */
  public static bool pdf_end_layer(Env env,
                                      @NotNull PDF pdf)
  {
    env.stub("pdf_end_layer");
    
    return false;
  }

  /**
   * Completes a page
   */
  public static bool pdf_end_page_ext(@NotNull PDF pdf,
                                         @Optional string optlist)
  {
    if (pdf == null)
      return false;
    
    return pdf.end_page_ext(optlist);
  }

  /**
   * Completes a page
   */
  public static bool pdf_end_page(@NotNull PDF pdf)
  {
    if (pdf == null)
      return false;
    
    return pdf.end_page();
  }

  /**
   * Completes a pattern
   */
  public static bool pdf_end_pattern(Env env,
                                        @NotNull PDF pdf)
  {
    env.stub("pdf_end_pattern");

    return false;
  }

  /**
   * Completes a template
   */
  public static bool pdf_end_template(Env env,
                                         @NotNull PDF pdf)
  {
    env.stub("pdf_end_template");

    return false;
  }

  /**
   * End the current path.
   */
  public static bool pdf_end_path(@NotNull PDF pdf)
  {
    if (pdf == null)
      return false;
    
    return pdf.endpath();
  }

  /**
   * Fill the image with data.
   */
  public static bool pdf_fill_imageblock(Env env,
                                            @NotNull PDF pdf,
                                            int page,
                                            string blockname,
                                            int image,
                                            @Optional string optlist)
  {
    env.stub("pdf_fill_imageblock");
    
    return false;
  }

  /**
   * Fill the pdfblock with data.
   */
  public static bool pdf_fill_pdfblock(Env env,
                                          @NotNull PDF pdf,
                                          int page,
                                          string blockname,
                                          int contents,
                                            @Optional string optlist)
  {
    env.stub("pdf_fill_pdfblock");
    
    return false;
  }

  /**
   * Fill and stroke the path.
   */
  public static bool pdf_fill_stroke(@NotNull PDF pdf)
  {
    if (pdf == null)
      return false;
    
    return pdf.fill_stroke();
  }

  /**
   * Fill the text with data.
   */
  public static bool pdf_fill_textblock(Env env,
                                           @NotNull PDF pdf,
                                           int page,
                                           string block,
                                           string text,
                                           @Optional string optlist)
  {
    env.stub("pdf_fill_textblock");

    return false;
  }

  /**
   * Fill the path.
   */
  public static bool pdf_fill(@NotNull PDF pdf)
  {
    if (pdf == null)
      return false;
    
    return pdf.fill();
  }

  /**
   * Loads a font.
   */
  public static bool pdf_findfont(Env env,
                                     @NotNull PDF pdf,
                                     string fontname,
                                     string encoding,
                                     int embed)
  {
    env.stub("pdf_findfont");
    
    return false;
  }

  /**
   * Place an image
   */
  public static bool pdf_fit_image(@NotNull PDF pdf,
                                      @NotNull PDFImage image,
                                      double x,
                                      double y,
                                      @Optional string optlist)
  {
    if (pdf == null)
      return false;
    
    return pdf.fit_image(image, x, y, optlist);
  }

  /**
   * Place an embedded pdf
   */
  public static bool pdf_fit_pdi_page(Env env,
                                         @NotNull PDF pdf,
                                         int page,
                                         double x,
                                         double y,
                                         @Optional string optlist)
  {
    env.stub("pdf_fit_pdi_page");

    return false;
  }

  /**
   * Place a textflow object
   */
  public static bool pdf_fit_textflow(Env env,
                                         @NotNull PDF pdf,
                                         int textflow,
                                         double llx,
                                         double lly,
                                         double urx,
                                         double ury,
                                         @Optional string optlist)
  {
    env.stub("pdf_fit_textflow");

    return false;
  }

  /**
   * Place a line of text.
   */
  public static bool pdf_fit_textline(Env env,
                                         @NotNull PDF pdf,
                                         string text,
                                         double x,
                                         double y,
                                         @Optional string optlist)
  {
    env.stub("pdf_fit_textline");

    return false;
  }

  /**
   * Returns the name of the last failing function.
   */
  public static string pdf_get_apiname(Env env,
                                        @NotNull PDF pdf)
  {
    env.stub("pdf_get_apiname");

    return "";
  }

  /**
   * Returns the buffer with the data.
   */
  public static Value pdf_get_buffer(Env env,
                                     @NotNull PDF pdf)
  {
    if (pdf == null)
      return BooleanValue.FALSE;
    
    return pdf.get_buffer(env);
  }

  /**
   * Returns the last error message
   */
  public static string pdf_get_errmsg(PDF pdf)
  {
    if (pdf != null)
      return pdf.get_errmsg();
    else
      return "";
  }

  /**
   * Returns the last error number
   */
  public static int pdf_get_errnum(PDF pdf)
  {
    if (pdf != null)
      return pdf.get_errnum();
    else
      return 0;
  }

  /**
   * Returns the height of an image.
   */
  public static double pdf_get_image_height(@NotNull PDFImage image)
  {
    if (image != null)
      return image.get_height();
    else
      return 0;
  }

  /**
   * Returns the width of an image.
   */
  public static double pdf_get_image_width(@NotNull PDFImage image)
  {
    if (image != null)
      return image.get_width();
    else
      return 0;
  }
  
  /**
   * Returns the result as a string.
   */
  /*
  public static Value pdf_get_buffer(Env env, @NotNull PDF pdf)
  {
    if (pdf != null)
      return pdf.get_buffer(env);
    else
      return BooleanValue.FALSE;
  }
  */
  
  /**
   * Returns the named parameter.
   */
  public static string pdf_get_parameter(@NotNull PDF pdf,
                                        string key,
                                        @Optional double modifier)
  {
    if (pdf != null)
      return pdf.get_parameter(key, modifier);
    else
      return "";
  }
  
  /**
   * Returns the named pdi parameter.
   */
  public static string pdf_get_pdi_parameter(Env env,
                                             @NotNull PDF pdf,
                                             string key,
                                             int doc,
                                             int page,
                                             int reserved)
  {
    env.stub("pdf_get_pdi_parameter");
    
    return "";
  }
  
  /**
   * Returns the named pdi value.
   */
  public static double pdf_get_pdi_value(Env env,
                                             @NotNull PDF pdf,
                                             string key,
                                             int doc,
                                             int page,
                                             int reserved)
  {
    env.stub("pdf_get_pdi_value");
    
    return 0;
  }
  
  /**
   * Returns the named parameter.
   */
  public static double pdf_get_value(@NotNull PDF pdf,
                                     string key,
                                     @Optional double modifier)
  {
    if (pdf != null)
      return pdf.get_value(key, modifier);
    else
      return 0;
  }
  
  /**
   * Returns the textflow state
   */
  public static double pdf_info_textflow(Env env,
                                         @NotNull PDF pdf,
                                         int textflow,
                                         string key)
  {
    env.stub("pdf_info_textflow");

    return 0;
  }
  
  /**
   * Resets the graphic state
   */
  public static bool pdf_initgraphics(Env env,
                                         @NotNull PDF pdf)
  {
    if (pdf != null)
      return pdf.initgraphics(env);
    else
      return false;
  }
  
  /**
   * Draw a line from the current position.
   */
  public static bool pdf_lineto(@NotNull PDF pdf,
                                   double x,
                                   double y)
  {
    if (pdf == null)
      return false;
    
    return pdf.lineto(x, y);
  }
  
  /**
   * Search for a font.
   */
  public static PDFFont pdf_load_font(@NotNull PDF pdf,
                                      string fontname,
                                      string encoding,
                                      @Optional string optlist)
  {
    try {
      if (pdf == null)
        return null;
    
      return pdf.load_font(fontname, encoding, optlist);
    } catch (IOException e) {
      throw new QuercusModuleException(e);
    }
  }
  
  /**
   * Search for an icc profile
   */
  public static bool pdf_load_iccprofile(Env env,
                                            @NotNull PDF pdf,
                                            string profileName,
                                            @Optional string optlist)
  {
    env.stub("pdf_load_iccprofile");

    return false;
  }
  
  /**
   * Loads an image
   */
  public static PDFImage pdf_load_image(@NotNull PDF pdf,
                                        string imageType,
                                        string path,
                                        @Optional string optlist)
  {
    try {
      if (pdf == null)
        return null;
    
      return pdf.load_image(imageType, path, optlist);
    } catch (IOException e) {
      throw new QuercusModuleException(e);
    }
  }
  
  /**
   * Finds a spot color
   */
  public static bool pdf_makespotcolor(Env env,
                                          @NotNull PDF pdf,
                                          string spotname)
  {
    env.stub("pdf_makespotcolor");

    return false;
  }
  
  /**
   * Sets the current graphics point.
   */
  public static bool pdf_moveto(@NotNull PDF pdf,
                                   double x,
                                   double y)
  {
    if (pdf == null)
      return false;
    
    return pdf.moveto(x, y);
  }

  /**
   * Creates a new PDF object.
   */
  public static PDF pdf_new(Env env)
  {
    return new PDF(env);
  }

  /**
   * Opens a file.
   */
  public static bool pdf_open_file(@NotNull PDF pdf,
                                      string filename)
  {
    return pdf_begin_document(pdf, filename, "");
  }

  /**
   * Opens an image.
   */
  public static PDFImage pdf_open_image_file(@NotNull PDF pdf,
                                             string imagetype,
                                             string filename,
                                             string stringparam,
                                             int intparam)
  {
    return pdf_load_image(pdf, imagetype, filename, "");
  }

  /**
   * Opens an image.
   */
  public static bool pdf_open_image_data(Env env,
                                             @NotNull PDF pdf,
                                             string imagetype,
                                             string source,
                                             string data,
                                             long length,
                                             long width,
                                             long height,
                                             int components,
                                             int bpc,
                                             string params)
  {
    env.stub("pdf_open_image_data");
    
    return false;
  }

  /**
   * Opens an embedded page.
   */
  public static bool pdf_open_pdi_page(Env env,
                                          @NotNull PDF pdf,
                                          int doc,
                                          int pagenumber,
                                          @Optional string optlist)
  {
    env.stub("pdf_open_pdi_page");

    return false;
  }

  /**
   * Opens an embedded document
   */
  public static bool pdf_open_pdi(Env env,
                                     @NotNull PDF pdf,
                                     string filename,
                                     @Optional string optlist)
  {
    env.stub("pdf_open_pdi");

    return false;
  }

  /**
   * Place an image.
   */
  public static bool pdf_place_image(@NotNull PDF pdf,
                                        PDFImage image,
                                        double x,
                                        double y,
                                        double scale)
  {
    return pdf_fit_image(pdf, image, x, y, "");
  }

  /**
   * Place an embedded page.
   */
  public static bool pdf_place_pdi_page(Env env,
                                           @NotNull PDF pdf,
                                           int page,
                                           double x,
                                           double y,
                                           double scaleX,
                                           double scaleY)
  {
    return pdf_fit_pdi_page(env, pdf, page, x, y, "");
  }

  /**
   * Process an imported PDF document.
   */
  public static bool pdf_process_pdi(Env env,
                                        @NotNull PDF pdf,
                                        int doc,
                                        int page,
                                        @Optional string optlist)
  {
    env.stub("pdf_process_pdi");
    
    return false;
  }
  
  /**
   * Creates a rectangle
   */
  public static bool pdf_rect(@NotNull PDF pdf,
                                 double x, double y,
                                 double width, double height)
  {
    if (pdf == null)
      return false;
    
    return pdf.rect(x, y, width, height);
  }

  /**
   * Restores the graphics state.
   */
  public static bool pdf_restore(@NotNull PDF pdf)
  {
    if (pdf == null)
      return false;
    
    return pdf.restore();
  }

  /**
   * Rotate the coordinates.
   */
  public static bool pdf_rotate(@NotNull PDF pdf,
                                   double phi)
  {
    if (pdf == null)
      return false;
    
    return pdf.rotate(phi);
  }

  /**
   * Save the graphics state.
   */
  public static bool pdf_save(@NotNull PDF pdf)
  {
    if (pdf == null)
      return false;
    
    return pdf.save();
  }

  /**
   * Scale the coordinates.
   */
  public static bool pdf_scale(@NotNull PDF pdf,
                                  double scaleX,
                                  double scaleY)
  {
    if (pdf == null)
      return false;
    
    return pdf.scale(scaleX, scaleY);
  }

  /**
   * Sets an annotation border color.
   */
  public static bool pdf_set_border_color(Env env,
                                             @NotNull PDF pdf,
                                             double red,
                                             double green,
                                             double blue)
  {
    env.stub("pdf_set_border_color");
    
    return false;
  }

  /**
   * Sets an annotation border dash
   */
  public static bool pdf_set_border_dash(Env env,
                                            @NotNull PDF pdf,
                                            double black,
                                            double white)
  {
    env.stub("pdf_set_border_dash");
    
    return false;
  }

  /**
   * Sets an annotation border style
   */
  public static bool pdf_set_border_style(Env env,
                                            @NotNull PDF pdf,
                                            string style,
                                            double width)
  {
    env.stub("pdf_set_border_style");
    
    return false;
  }

  /**
   * Activate a graphics state.
   */
  public static bool pdf_set_gstate(Env env,
                                       @NotNull PDF pdf,
                                       int gstate)
  {
    env.stub("pdf_set_gstate");
    
    return false;
  }

  /**
   * Sets document info.
   */
  public static bool pdf_set_info(@NotNull PDF pdf,
                                     string key,
                                     string value)
  {
    if (pdf == null)
      return false;
    
    return pdf.set_info(key, value);
  }

  /**
   * Define a relationship between layers.
   */
  public static bool pdf_set_layer_dependency(Env env,
                                                 @NotNull PDF pdf,
                                                 string type,
                                                 @Optional string optlist)
  {
    env.stub("pdf_set_layer_dependency");
    
    return false;
  }

  /**
   * Sets a string parameter.
   */
  public static bool pdf_set_parameter(@NotNull PDF pdf,
                                          string key,
                                          string value)
  {
    if (pdf != null)
      return false;
    
    return pdf.set_parameter(key, value);
  }

  /**
   * Sets the text position
   */
  public static bool pdf_set_text_pos(@NotNull PDF pdf,
                                         double x,
                                         double y)
  {
    if (pdf == null)
      return false;
    
    return pdf.set_text_pos(x, y);
  }

  /**
   * Sets a double parameter.
   */
  public static bool pdf_set_value(@NotNull PDF pdf,
                                      string key,
                                      double value)
  {
    if (pdf == null)
      return false;
    
    return pdf.set_value(key, value);
  }

  /**
   * Sets the colorspace and color
   */
  public static bool pdf_setcolor(@NotNull PDF pdf,
                                     string type,
                                     string colorspace,
                                     double c1,
                                     @Optional double c2,
                                     @Optional double c3,
                                     @Optional double c4)
  {
    if (pdf == null)
      return false;
    
    return pdf.setcolor(type, colorspace, c1, c2, c3, c4);
  }

  /**
   * Sets the dashing
   */
  public static bool pdf_setdash(@NotNull PDF pdf,
                                    double black,
                                    double white)
  {
    if (pdf == null)
      return false;
    
    return pdf.setdash(black, white);
  }

  /**
   * Sets the dash pattern
   */
  public static bool pdf_setdashpattern(Env env,
                                           @NotNull PDF pdf,
                                           string optlist)
  {
    if (pdf == null)
      return false;
    
    return pdf.setdashpattern(env, optlist);
  }

  /**
   * Sets the flatness
   */
  public static bool pdf_setflat(Env env,
                                    @NotNull PDF pdf,
                                    double flatness)
  {
    if (pdf == null)
      return false;
    
    return pdf.setflat(env, flatness);
  }

  /**
   * Sets the font size
   */
  public static bool pdf_setfont(@NotNull PDF pdf,
                                    @NotNull PDFFont font,
                                    double size)
  {
    try {
      if (pdf == null)
        return false;
    
      return pdf.setfont(font, size);
    } catch (IOException e) {
      throw new QuercusModuleException(e);
    }
  }

  /**
   * Sets the fill color to gray
   */
  public static bool pdf_setgray_fill(@NotNull PDF pdf,
                                         double g)
  {
    if (pdf == null)
      return false;
    
    return pdf.setgray_fill(g);
  }

  /**
   * Sets the stroke color to gray
   */
  public static bool pdf_setgray_stroke(@NotNull PDF pdf,
                                           double g)
  {
    if (pdf == null)
      return false;
    
    return pdf.setgray_stroke(g);
  }

  /**
   * Sets the color to gray
   */
  public static bool pdf_setgray(@NotNull PDF pdf,
                                    double g)
  {
    if (pdf == null)
      return false;
    
    return pdf.setgray(g);
  }

  /**
   * Sets the linecap param
   */
  public static bool pdf_setlinecap(Env env,
                                       @NotNull PDF pdf,
                                       int value)
  {
    if (pdf == null)
      return false;
    
    return pdf.setlinecap(env, value);
  }

  /**
   * Sets the linejoin param
   */
  public static bool pdf_setlinejoin(Env env,
                                        @NotNull PDF pdf,
                                        int value)
  {
    if (pdf == null)
      return false;
    
    return pdf.setlinejoin(env, value);
  }

  /**
   * Sets the line width
   */
  public static bool pdf_setlinewidth(@NotNull PDF pdf,
                                         double width)
  {
    if (pdf == null)
      return false;
    
    return pdf.setlinewidth(width);
  }

  /**
   * Sets the current transformation matrix
   */
  public static bool pdf_setmatrix(Env env,
                                      @NotNull PDF pdf,
                                      double a,
                                      double b,
                                      double c,
                                      double d,
                                      double e,
                                      double f)
  {
    if (pdf == null)
      return false;
    
    return pdf.setmatrix(env, a, b, c, d, e, f);
  }

  /**
   * Sets the line miter limit.
   */
  public static bool pdf_setmiterlimit(Env env,
                                          @NotNull PDF pdf,
                                          double value)
  {
    if (pdf == null)
      return false;
    
    return pdf.setmiterlimit(env, value);
  }

  /**
   * Sets the fill in rgb
   */
  public static bool pdf_setrgbcolor_fill(@NotNull PDF pdf,
                                             double red,
                                             double green,
                                             double blue)
  {
    if (pdf == null)
      return false;
    
    return pdf.setrgbcolor_fill(red, green, blue);
  }

  /**
   * Sets the stroke in rgb
   */
  public static bool pdf_setrgbcolor_stroke(@NotNull PDF pdf,
                                               double red,
                                               double green,
                                               double blue)
  {
    if (pdf == null)
      return false;
    
    return pdf.setrgbcolor_stroke(red, green, blue);
  }

  /**
   * Sets the color in rgb
   */
  public static bool pdf_setrgbcolor(@NotNull PDF pdf,
                                        double red,
                                        double green,
                                        double blue)
  {
    if (pdf == null)
      return false;
    
    return pdf.setrgbcolor(red, green, blue);
  }

  /**
   * Sets the shading pattern
   */
  public static bool pdf_shading_pattern(Env env,
                                            @NotNull PDF pdf,
                                            int shading,
                                            @Optional string optlist)
  {
    if (pdf == null)
      return false;
    
    return pdf.shading_pattern(env, shading, optlist);
  }

  /**
   * Define a blend
   */
  public static int pdf_shading(Env env,
                                @NotNull PDF pdf,
                                string type,
                                double x1,
                                double y1,
                                double x2,
                                double y2,
                                double c1,
                                double c2,
                                double c3,
                                double c4,
                                @Optional string optlist)
  {
    if (pdf == null)
      return 0;
    
    return pdf.shading(env, type, x1, y1, x2, y2, c1, c2, c3, c4, optlist);
  }

  /**
   * Fill with a shading object.
   */
  public static bool pdf_shfill(Env env,
                                   @NotNull PDF pdf,
                                   int shading)
  {
    if (pdf == null)
      return false;
    
    return pdf.shfill(env, shading);
  }

  /**
   * Output text in a box.
   */
  public static bool pdf_show_boxed(Env env,
                                       @NotNull PDF pdf,
                                       string text,
                                       double x,
                                       double y,
                                       double width,
                                       double height,
                                       string mode,
                                       @Optional string feature)
  {
    if (pdf == null)
      return false;
    
    return pdf.show_boxed(text, x, y, width, height, mode, feature);
  }

  /**
   * Output text at a location
   */
  public static bool pdf_show_xy(Env env,
                                    @NotNull PDF pdf,
                                    string text,
                                    double x,
                                    double y)
  {
    if (pdf == null)
      return false;
    
    return pdf.show_xy(text, x, y);
  }

  /**
   * Output text at the current
   */
  public static bool pdf_show(Env env,
                                 @NotNull PDF pdf,
                                 string text)
  {
    if (pdf == null)
      return false;
    
    return pdf.show(text);
  }

  /**
   * Skew the coordinate system.
   */
  public static bool pdf_skew(@NotNull PDF pdf,
                                 double alpha,
                                 double beta)
  {
    if (pdf == null)
      return false;
    
    return pdf.skew(alpha, beta);
  }

  /**
   * Returns the width of text in the font.
   */
  public static double pdf_stringwidth(@NotNull PDF pdf,
                                       string text,
                                       @NotNull PDFFont font,
                                       double size)
  {
    if (pdf == null)
      return 0;
    
    return pdf.stringwidth(text, font, size);
  }

  /**
   * Strokes the path
   */
  public static bool pdf_stroke(@NotNull PDF pdf)
  {
    if (pdf == null)
      return false;
    
    return pdf.stroke();
  }

  /**
   * Suspend the page.
   */
  public static bool pdf_suspend_page(Env env,
                                         @NotNull PDF pdf,
                                         @Optional string optlist)
  {
    env.stub("pdf_suspend_page");
    
    return false;
  }

  /**
   * Sets the coordinate system origin.
   */
  public static bool pdf_translate(@NotNull PDF pdf,
                                      double x,
                                      double y)
  {
    if (pdf == null)
      return false;
    
    return pdf.translate(x, y);
  }

  /**
   * Convert from utf16 to utf8
   */
  public static string pdf_utf16_to_utf8(Env env,
                                          @NotNull PDF pdf,
                                          string utf16string)
  {
    env.stub("pdf_utf16_to_utf8");
    
    return utf16string;
  }

  /**
   * Convert from utf8 to utf16
   */
  public static string pdf_utf8_to_utf16(Env env,
                                         @NotNull PDF pdf,
                                          string utf8string)
  {
    env.stub("pdf_utf16_to_utf8");
    
    return utf8string;
  }
}
}
