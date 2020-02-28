using System;
namespace QuercusDotNet.lib.spl {











public abstract class AbstractIteratorImpl<T> : Iterator<T> {
    private readonly L10N L = new L10N(AbstractIteratorImpl.class);

    protected Env _env;
    protected ObjectValue _obj;
    protected QuercusClass _qClass;

    private AbstractFunction _nextFun;
    private AbstractFunction _currentFun;
    private AbstractFunction _keyFun;
    private AbstractFunction _rewindFun;
    private AbstractFunction _validFun;
    private bool _needNext;

    public AbstractIteratorImpl(Env env,
                                ObjectValue obj)
    {
      _env = env;
      _qClass = obj.getQuercusClass();
      _obj = obj;

      if (! obj.isA(env, "iterator")) {
        throw new IllegalStateException(L.l("'{0}' @is an invalid iterator", obj));
      }

      _currentFun = _qClass.getFunction(env.createString("current"));
      _keyFun = _qClass.getFunction(env.createString("key"));
      _nextFun = _qClass.getFunction(env.createString("next"));
      _rewindFun = _qClass.getFunction(env.createString("rewind"));
      _validFun = _qClass.getFunction(env.createString("valid"));

      _rewindFun.callMethod(_env, _qClass, _obj);

      _needNext = false;
    }

    public bool hasNext()
    {
      if (_needNext) {
        _nextFun.callMethod(_env, _qClass, _obj);
      }

      _needNext = true;

      return _validFun.callMethod(_env, _qClass, _obj).toBoolean();
    }

    public T next()
    {
      return getCurrent();
    }

    abstract protected T getCurrent();

    protected Value getCurrentKey()
    {
      return _keyFun.callMethod(_env, _qClass, _obj);
    }

    protected Value getCurrentValue()
    {
      return _currentFun.callMethod(_env, _qClass, _obj);
    }

    public void remove()
    {
      throw new UnsupportedOperationException();
    }
}}
