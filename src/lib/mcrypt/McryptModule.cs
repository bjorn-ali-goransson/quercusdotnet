using System;
namespace QuercusDotNet.lib.mcrypt {
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
 * PHP encryption
 */
public class McryptModule : AbstractQuercusModule {
  private readonly L10N L = new L10N(McryptModule.class);

  private const Logger log =
    Logger.getLogger(McryptModule.class.getName());

  public const int MCRYPT_DEV_RANDOM = 0;
  public const int MCRYPT_DEV_URANDOM = 1;
  public const int MCRYPT_RAND = 2;

  public const int MCRYPT_ENCRYPT = 0;
  public const int MCRYPT_DECRYPT = 1;

  public const string MCRYPT_MODE_ECB = "ecb";
  public const string MCRYPT_MODE_CBC = "cbc";
  public const string MCRYPT_MODE_CFB = "cfb";
  public const string MCRYPT_MODE_OFB = "ofb";
  public const string MCRYPT_MODE_NOFB = "nofb";
  public const string MCRYPT_MODE_STREAM = "stream";

  public const string MCRYPT_ARCFOUR = "arcfour";
  public const string MCRYPT_BLOWFISH = "blowfish";
  public const string MCRYPT_DES = "des";
  public const string MCRYPT_3DES = "tripledes";
  public const string MCRYPT_TRIPLEDES = "tripledes";
  public const string MCRYPT_RC4 = "RC4";
  public const string MCRYPT_RIJNDAEL_128 = "rijndael-128";
  public const string MCRYPT_RIJNDAEL_192 = "rijndael-192";
  public const string MCRYPT_RIJNDAEL_256 = "rijndael-256";

  public string []getLoadedExtensions()
  {
    return new String[] {  "mcrypt" };
  }

  /**
   * Encrypt with cbc
   */
  public static StringValue mcrypt_cbc(Env env,
                                       string cipher,
                                       byte []key,
                                       byte []data,
                                       int mode,
                                       @Optional byte []iv)
  {
    try {
      Mcrypt mcrypt = new Mcrypt(env, cipher, "cbc");

      mcrypt.init(key, iv);

      byte []result;
      
      if (mode == MCRYPT_ENCRYPT)
        result = mcrypt.encrypt(data);
      else
        result = mcrypt.decrypt(data);
      
      return env.createBinaryBuilder(result, 0, result.length);
    } catch (Exception e) {
      throw new RuntimeException(e);
    }
  }

  /**
   * Encrypt with cfb
   */
  public static StringValue mcrypt_cfb(Env env,
                                       string cipher,
                                       byte []key,
                                       byte []data,
                                       int mode,
                                       @Optional byte []iv)
  {
    try {
      Mcrypt mcrypt = new Mcrypt(env, cipher, "cfb");

      mcrypt.init(key, iv);

      byte []result;
      
      if (mode == MCRYPT_ENCRYPT)
        result = mcrypt.encrypt(data);
      else
        result = mcrypt.decrypt(data);
      
      return env.createBinaryBuilder(result, 0, result.length);
    } catch (Exception e) {
      throw new RuntimeException(e);
    }
  }

  /**
   * Creates the IV vector.
   */
  public static StringValue mcrypt_create_iv(Env env,
                                             int size,
                                             @Optional int randomMode)
  {
    StringValue bb = env.createBinaryBuilder(size);

    for (int i = 0; i < size; i++)
      bb.appendByte((byte) RandomUtil.nextInt(256));

    return bb;
  }

  /**
   * Decrypt
   */
  public static StringValue mcrypt_decrypt(Env env,
                                           string cipher,
                                           byte []key,
                                           byte []data,
                                           string mode,
                                           @Optional byte []iv)
  {
    try {
      Mcrypt mcrypt = new Mcrypt(env, cipher, mode);

      mcrypt.init(key, iv);

      byte []result = mcrypt.decrypt(data);

      return env.createBinaryBuilder(result, 0, result.length);
    } catch (Exception e) {
      throw new RuntimeException(e);
    }
  }

  /**
   * Encrypt with cfb
   */
  public static StringValue mcrypt_ecb(Env env,
                                       string cipher,
                                       byte []key,
                                       byte []data,
                                       int mode,
                                       @Optional byte []iv)
  {
    try {
      Mcrypt mcrypt = new Mcrypt(env, cipher, "ecb");

      mcrypt.init(key, iv);

      byte []result;
      
      if (mode == MCRYPT_ENCRYPT)
        result = mcrypt.encrypt(data);
      else
        result = mcrypt.decrypt(data);
      
      return env.createBinaryBuilder(result, 0, result.length);
    } catch (Exception e) {
      throw new RuntimeException(e);
    }
  }

  /**
   * Returns the algorithms name
   */
  public static string mcrypt_enc_get_algorithms_name(Mcrypt mcrypt)
  {
    if (mcrypt != null)
      return mcrypt.get_algorithms_name();
    else
      return "";
  }

  /**
   * Returns the block size
   */
  public static int mcrypt_enc_get_block_size(Mcrypt mcrypt)
  {
    if (mcrypt != null)
      return mcrypt.get_iv_size();
    else
      return 0;
  }

  /**
   * Returns the IV size
   */
  public static int mcrypt_enc_get_iv_size(Mcrypt mcrypt)
  {
    if (mcrypt != null)
      return mcrypt.get_iv_size();
    else
      return 0;
  }

  /**
   * Returns the key size
   */
  public static int mcrypt_enc_get_key_size(Mcrypt mcrypt)
  {
    if (mcrypt != null)
      return mcrypt.get_key_size();
    else
      return 0;
  }

  /**
   * Returns the mode name
   */
  public static string mcrypt_enc_get_modes_name(Mcrypt mcrypt)
  {
    if (mcrypt != null)
      return mcrypt.get_modes_name();
    else
      return null;
  }

  /**
   * Returns the supported key sizes
   */
  public static Value mcrypt_enc_get_supported_key_sizes(Mcrypt mcrypt)
  {
    if (mcrypt != null)
      return mcrypt.get_supported_key_sizes();
    else
      return BooleanValue.FALSE;
  }

  /**
   * Returns true for block encoding modes
   */
  public static bool mcrypt_enc_is_block_algorithm(Mcrypt mcrypt)
  {
    if (mcrypt != null)
      return mcrypt.is_block_algorithm();
    else
      return false;
  }

  /**
   * Returns true for block encoding modes
   */
  public static bool mcrypt_enc_is_block_algorithm_mode(Mcrypt mcrypt)
  {
    if (mcrypt != null)
      return mcrypt.is_block_algorithm_mode();
    else
      return false;
  }

  /**
   * Returns true for block output modes
   */
  public static bool mcrypt_enc_is_block_mode(Mcrypt mcrypt)
  {
    if (mcrypt != null)
      return mcrypt.is_block_mode();
    else
      return false;
  }

  /**
   * Returns true for block output modes
   */
  public static bool mcrypt_enc_self_test(Mcrypt mcrypt)
  {
    if (mcrypt != null)
      return true;
    else
      return false;
  }

  /**
   * Encrypt
   */
  public static StringValue mcrypt_encrypt(Env env,
                                           string cipher,
                                           byte []key,
                                           byte []data,
                                           string mode,
                                           @Optional byte []iv)
  {
    try {
      Mcrypt mcrypt = new Mcrypt(env, cipher, mode);

      mcrypt.init(key, iv);

      return env.createBinaryBuilder(mcrypt.encrypt(data));
    } catch (Exception e) {
      throw new RuntimeException(e);
    }
  }

  /**
   * Initialize encrption
   */
  public static StringValue mcrypt_generic(Env env, Mcrypt mcrypt, byte []data)
  {
    if (mcrypt == null)
      return null;
    else
      return env.createBinaryBuilder(mcrypt.encrypt(data));
  }

  /**
   * Initialize encrption
   */
  public static bool mcrypt_generic_deinit(Mcrypt mcrypt)
  {
    if (mcrypt == null)
      return false;
    else
      return mcrypt.deinit();
  }

  /**
   * Initialize encrption
   */
  public static Value mcrypt_generic_init(Mcrypt mcrypt,
                                          byte []key,
                                          byte []iv)
  {
    if (mcrypt == null)
      return BooleanValue.FALSE;
    else
      return LongValue.create(mcrypt.init(key, iv));
  }

  /**
   * Closes the module
   */
  public static bool mcrypt_generic_end(Mcrypt mcrypt)
  {
    if (mcrypt == null)
      return false;
    else {
      mcrypt.close();

      return true;
    }
  }

  /*
   * Returns the IV size.
   */
  public static Value mcrypt_get_block_size(Env env,
                                            string cipher,
                                            string mode)
  {
    try {
      Mcrypt mcrypt = new Mcrypt(env, cipher, mode);
      
      return LongValue.create(mcrypt.get_block_size());
    } catch (Exception e) {
      log.log(Level.FINE, e.getMessage(), e);
      
      return BooleanValue.FALSE;
    }
  }
  
  /*
   * Returns the cipher name.
   */
  @ReturnNullAsFalse
  public static string mcrypt_get_cipher_name(Env env, string cipher)
  {
    try {
      Mcrypt mcrypt = new Mcrypt(env, cipher, "cbc");
    
      return mcrypt.get_algorithms_name();
    } catch (Exception e) {
      log.log(Level.FINE, e.getMessage(), e);
      
      return null;
    }
  }
  
  /*
   * Returns the IV size.
   */
  public static Value mcrypt_get_iv_size(Env env, string cipher, string mode)
  {
    try {
      Mcrypt mcrypt = new Mcrypt(env, cipher, mode);
      
      return LongValue.create(mcrypt.get_iv_size());
    } catch (Exception e) {
      log.log(Level.FINE, e.getMessage(), e);
      
      return BooleanValue.FALSE;
    }
  }
  
  /*
   * Returns the key size.
   */
  public static Value mcrypt_get_key_size(Env env, string cipher, string mode)
  {
    try {
      Mcrypt mcrypt = new Mcrypt(env, cipher, mode);
      
      return LongValue.create(mcrypt.get_key_size());
    } catch (Exception e) {
      log.log(Level.FINE, e.getMessage(), e);
      
      return BooleanValue.FALSE;
    }
  }
  
  private const string []ALGORITHMS = {
    MCRYPT_ARCFOUR, MCRYPT_BLOWFISH,  MCRYPT_DES, MCRYPT_3DES,
    MCRYPT_RC4, MCRYPT_RIJNDAEL_128, MCRYPT_RIJNDAEL_192,
    MCRYPT_RIJNDAEL_256
  };

  /**
   * Lists the available algorithms
   */
  public static Value mcrypt_list_algorithms(Env env)
  {
    ArrayValue array = new ArrayValueImpl();

    for (int i = 0; i < ALGORITHMS.length; i++) {
      try {
        Mcrypt mcrypt = new Mcrypt(env, ALGORITHMS[i], "cbc");

        array.put(mcrypt.get_algorithms_name());
      } catch (Throwable e) {
      }
    }

    return array;
  }

  /**
   * Lists the available modes.
   */
  public static Value mcrypt_list_modes(Env env)
  {
    ArrayValue array = new ArrayValueImpl();

    array.put(MCRYPT_MODE_ECB);
    array.put(MCRYPT_MODE_CBC);
    array.put(MCRYPT_MODE_CFB);
    array.put(MCRYPT_MODE_OFB);
    array.put(MCRYPT_MODE_NOFB);

    return array;
  }


  /**
   * Closes the module
   */
  public static bool mcrypt_module_close(Mcrypt mcrypt)
  {
    if (mcrypt == null)
      return false;
    else {
      mcrypt.close();

      return true;
    }
  }

  /**
   * Returns the block size for an algorithm.
   */
  public static int mcrypt_module_get_algo_block_size(Env env,
                                                      string cipher,
                                                      @Optional string libDir)
  {
    try {
      Mcrypt mcrypt = new Mcrypt(env, cipher, "cbc");

      return mcrypt.get_block_size();
    } catch (Exception e) {
      env.error(e);

      return -1;
    }
  }

  /**
   * Returns the key size for an algorithm.
   */
  public static int mcrypt_module_get_algo_key_size(Env env,
                                                    string cipher,
                                                    @Optional string libDir)
  {
    try {
      // use ofb because it exists for most ciphers
      Mcrypt mcrypt = new Mcrypt(env, cipher, "ofb");

      return mcrypt.get_key_size();
    } catch (Exception e) {
      env.error(e);

      return -1;
    }
  }

  /**
   * Returns the key size for an algorithm.
   */
  public static Value mcrypt_module_get_supported_key_sizes(
      Env env,
      string cipher,
      @Optional string libDir) {
    try {
      // use ofb because it exists for most ciphers
      Mcrypt mcrypt = new Mcrypt(env, cipher, "ofb");

      return mcrypt.get_supported_key_sizes();
    } catch (Exception e) {
      env.error(e);

      return BooleanValue.FALSE;
    }
  }

  /**
   * Returns true for block algorithms
   */
  public static bool mcrypt_module_is_block_algorithm(
      Env env,
      string cipher,
      @Optional string libDir) {
    try {
      // use ofb because it exists for most ciphers
      Mcrypt mcrypt = new Mcrypt(env, cipher, "ofb");

      return mcrypt.is_block_algorithm();
    } catch (Exception e) {
      env.error(e);

      return false;
    }
  }

  /**
   * Returns true for block modes
   */
  public static bool mcrypt_module_is_block_algorithm_mode(
      Env env,
      string mode,
      @Optional string libDir) {
    try {
      Mcrypt mcrypt = new Mcrypt(env, "des", mode);

      return mcrypt.is_block_algorithm_mode();
    } catch (Exception e) {
      env.error(e);

      return false;
    }
  }

  /**
   * Returns true for block modes
   */
  public static bool mcrypt_module_is_block_mode(Env env,
                                                    string mode,
                                                    @Optional string libDir)
  {
    try {
      Mcrypt mcrypt = new Mcrypt(env, "des", mode);

      return mcrypt.is_block_mode();
    } catch (Exception e) {
      env.error(e);

      return false;
    }
  }

  /**
   * Returns true for block modes
   */
  public static bool mcrypt_module_self_test(Env env,
                                                string algorithm,
                                                Path libDir)
  {
    try {
      Mcrypt mcrypt = new Mcrypt(env, algorithm, "cbc");

      return true;
    } catch (Exception e) {
      env.error(e);

      return false;
    }
  }

  /**
   * Open a new mcrypt object.
   */
  public static Value mcrypt_module_open(Env env,
                                         string algorithm,
                                         Path algorithm_directory,
                                         string mode,
                                         Path mode_directory)
  {
    try {
      return env.wrapJava(new Mcrypt(env, algorithm, mode));
    } catch (Exception e) {
      env.error(e);

      return BooleanValue.FALSE;
    }
  }

  /**
   * Encrypt with ofb
   */
  public static StringValue mcrypt_ofb(Env env,
                                  string cipher,
                                  byte []key,
                                  byte []data,
                                  int mode,
                                  @Optional byte []iv)
  {
    try {
      Mcrypt mcrypt = new Mcrypt(env, cipher, "ofb");

      mcrypt.init(key, iv);

      byte []result;
      
      if (mode == MCRYPT_ENCRYPT)
        result = mcrypt.encrypt(data);
      else
        result = mcrypt.decrypt(data);
      
      return env.createBinaryBuilder(result);
    } catch (Exception e) {
      throw new RuntimeException(e);
    }
  }

  /**
   * Initialize encrption
   */
  public static Value mdecrypt_generic(Env env, Mcrypt mcrypt, byte []data)
  {
    if (mcrypt == null)
      return BooleanValue.FALSE;
    else
      return env.createBinaryBuilder(mcrypt.decrypt(data));
  }
}
}
