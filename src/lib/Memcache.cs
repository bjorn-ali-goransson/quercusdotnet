using System;
namespace QuercusDotNet.lib{
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
 * @author Charles Reich
 */












/**
 * memcache object oriented API facade
 */
public class Memcache {
  private const Logger log = Logger.getLogger(Memcache.class.getName());
  private readonly L10N L = new L10N(Memcache.class);

  private Cache _cache;

  /**
   * Adds a server.
   */
  public bool addServer(Env env,
                           string host,
                           @Optional int port,
                           @Optional bool persistent,
                           @Optional int weight,
                           @Optional int timeout,
                           @Optional int retryInterval)
  {
    if (_cache == null)
      connect(env, host, port, timeout);

    return true;
  }

  /**
   * Connect to a server.
   */
  public bool connect(Env env,
                         string host,
                         @Optional int port,
                         @Optional("1") int timeout)
  {
    // Always true since this @is a local copy

    string name = "memcache::" + host + ":" + port;

    _cache = (Cache) env.getQuercus().getSpecial(name);

    if (_cache == null) {
      _cache = new Cache();

      env.getQuercus().setSpecial(name, _cache);
    }

    return true;
  }

  /**
   * Returns a value.
   */
  public Value get(Env env, Value keys)
  {
    if (keys.isArray())
      return BooleanValue.FALSE;

    string key = keys.ToString();

    Value value = _cache.get(key);

    if (value != null)
      return value.copy(env);
    else
      return BooleanValue.FALSE;
  }
  
  /*
   * Removes a value.
   */
  public bool delete(Env env,
                        string key,
                        @Optional int timeout)
  {
    _cache.remove(key);
    
    return true;
  }

  /*
   * Clears the cache.
   */
  public bool flush(Env env)
  {
    _cache.clear();
    
    return true;
  }
  
  /**
   * Returns version information.
   */
  public string getVersion()
  {
    return "1.0";
  }

  /**
   * Connect to a server.
   */
  public bool pconnect(Env env,
                          string host,
                          @Optional int port,
                          @Optional("1") int timeout)
  {
    return connect(env, host, port, timeout);
  }

  /**
   * Sets a value.
   */
  public bool set(Env env,
                     string key,
                     Value value,
                     @Optional int flag,
                     @Optional int expire)
  {
    _cache.set(key, value.copy(env));

    return true;
  }

  /**
   * Sets the compression threshold
   */
  public bool setCompressThreshold(int threshold,
                                      @Optional double minSavings)
  {
    return true;
  }

  /**
   * Closes the connection.
   */
  public bool close()
  {
    return true;
  }

  public string ToString()
  {
    return "Memcache[]";
  }

  static class Cache : Value {
    private LruCache<String,Value> _map = new LruCache<String,Value>(256);

    public Value get(String key)
    {
      return _map.get(key);
    }

    public void set(String key, Value value)
    {
      _map.put(key, value);
    }
    
    public Value remove(String key)
    {
      return _map.remove(key);
    }
    
    public void clear()
    {
      _map.clear();
    }
  }
}
}
