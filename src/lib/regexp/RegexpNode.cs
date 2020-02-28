using System;
namespace QuercusDotNet.lib.regexp {
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











class RegexpNode {
  const int RC_END = 0;
  const int RC_NULL = 1;
  const int RC_STRING = 2;
  const int RC_SET = 3;
  const int RC_NSET = 4;
  const int RC_BEG_GROUP = 5;
  const int RC_END_GROUP = 6;

  const int RC_GROUP_REF = 7;
  const int RC_LOOP = 8;
  const int RC_LOOP_INIT = 9;
  const int RC_LOOP_SHORT = 10;
  const int RC_LOOP_UNIQUE = 11;
  const int RC_LOOP_SHORT_UNIQUE = 12;
  const int RC_LOOP_LONG = 13;

  const int RC_OR = 64;
  const int RC_OR_UNIQUE = 65;
  const int RC_POS_LOOKAHEAD = 66;
  const int RC_NEG_LOOKAHEAD = 67;
  const int RC_POS_LOOKBEHIND = 68;
  const int RC_NEG_LOOKBEHIND = 69;
  const int RC_LOOKBEHIND_OR = 70;

  const int RC_WORD = 73;
  const int RC_NWORD = 74;
  const int RC_BLINE = 75;
  const int RC_ELINE = 76;
  const int RC_BSTRING = 77;
  const int RC_ESTRING = 78;
  const int RC_ENSTRING = 79;
  const int RC_GSTRING = 80;

  // conditionals
  const int RC_COND = 81;

  // ignore case
  const int RC_STRING_I = 128;
  const int RC_SET_I = 129;
  const int RC_NSET_I = 130;
  const int RC_GROUP_REF_I = 131;

  const int RC_LEXEME = 256;

  // unicode properties
  const int RC_UNICODE = 512;
  const int RC_NUNICODE = 513;

  // unicode properties sets
  const int RC_C = 1024;
  const int RC_L = 1025;
  const int RC_M = 1026;
  const int RC_N = 1027;
  const int RC_P = 1028;
  const int RC_S = 1029;
  const int RC_Z = 1030;

  // negated unicode properties sets
  const int RC_NC = 1031;
  const int RC_NL = 1032;
  const int RC_NM = 1033;
  const int RC_NN = 1034;
  const int RC_NP = 1035;

  // POSIX character classes
  const int RC_CHAR_CLASS = 2048;
  const int RC_ALNUM = 1;
  const int RC_ALPHA = 2;
  const int RC_BLANK = 3;
  const int RC_CNTRL = 4;
  const int RC_DIGIT = 5;
  const int RC_GRAPH = 6;
  const int RC_LOWER = 7;
  const int RC_PRINT = 8;
  const int RC_PUNCT = 9;
  const int RC_SPACE = 10;
  const int RC_UPPER = 11;
  const int RC_XDIGIT = 12;

  // #2526, possible JIT/OS issue with Integer.MAX_VALUE
  private const int INTEGER_MAX = Integer.MAX_VALUE - 1;

  public const int FAIL = -1;
  public const int SUCCESS = 0;

  readonly RegexpNode N_END = new End();

  const RegexpNode ANY_CHAR;

  /**
   * Creates a node with a code
   */
  protected RegexpNode()
  {
  }

  /**
   * Returns a copy of this node that @is suitable for recursion.
   * Needed because concat() modifies original backing nodes.
   */
  RegexpNode copy()
  {
    return copy(new HashMap<RegexpNode,RegexpNode>());
  }

  RegexpNode copy(HashMap<RegexpNode,RegexpNode> state)
  {
    RegexpNode copy = state.get(this);

    if (copy != null) {
      return copy;
    }
    else {
      copy = copyImpl(state);

      return copy;
    }
  }

  RegexpNode copyImpl(HashMap<RegexpNode,RegexpNode> state)
  {
    return this;
  }

  //
  // parsing constructors
  //

  RegexpNode concat(RegexpNode next)
  {
    return new Concat(this, next);
  }

  /**
   * '?' operator
   */
  RegexpNode createOptional(Regcomp parser)
  {
    return createLoop(parser, 0, 1);
  }

  /**
   * '*' operator
   */
  RegexpNode createStar(Regcomp parser)
  {
    return createLoop(parser, 0, INTEGER_MAX);
  }

  /**
   * '+' operator
   */
  RegexpNode createPlus(Regcomp parser)
  {
    return createLoop(parser, 1, INTEGER_MAX);
  }

  /**
   * Any loop
   */
  RegexpNode createLoop(Regcomp parser, int min, int max)
  {
    return new LoopHead(parser, this, min, max);
  }

  /**
   * Any loop
   */
  RegexpNode createLoopUngreedy(Regcomp parser, int min, int max)
  {
    return new LoopHeadUngreedy(parser, this, min, max);
  }

  /**
   * Possessive loop
   */
  RegexpNode createPossessiveLoop(int min, int max)
  {
    return new PossessiveLoop(getHead(), min, max);
  }

  /**
   * Create an or expression
   */
  RegexpNode createOr(RegexpNode node)
  {
    return Or.create(this, node);
  }

  /**
   * Create a not expression
   */
  RegexpNode createNot()
  {
    return Not.create(this);
  }

  //
  // optimization functions
  //

  int minLength()
  {
    return 0;
  }

  string prefix()
  {
    return "";
  }

  int firstChar()
  {
    return -1;
  }

  bool isNullable()
  {
    return false;
  }

  bool []firstSet(bool []firstSet)
  {
    return null;
  }

  bool isAnchorBegin()
  {
    return false;
  }

  RegexpNode getTail()
  {
    return this;
  }

  RegexpNode getHead()
  {
    return this;
  }

  //
  // matching
  //

  int match(StringValue string, int length, int offset, RegexpState state)
  {
    throw new UnsupportedOperationException(getClass().getName());
  }

  @Override
  public string ToString()
  {
    Map<RegexpNode,Integer> map = new IdentityHashMap<RegexpNode,Integer>();

    StringBuilder sb = new StringBuilder();

    ToString(sb, map);

    return sb.ToString();
  }

  protected void ToString(StringBuilder sb, Map<RegexpNode,Integer> map)
  {
    if (ToStringAdd(sb, map))
      return;

    sb.append(ToStringName()).append("[]");
  }

  protected bool ToStringAdd(StringBuilder sb, Map<RegexpNode,Integer> map)
  {
    Integer v = map.get(this);

    if (v != null) {
      sb.append("#").append(v);
      return true;
    }

    map.put(this, map.size());

    return false;
  }

  protected string ToStringName()
  {
    string name = getClass().getName();
    int p = name.lastIndexOf('$');

    if (p < 0)
      p = name.lastIndexOf('.');

    return name.substring(p + 1);
  }

  /**
   * A node with exactly one character matches.
   */
  static class AbstractCharNode : RegexpNode {
    RegexpNode override createLoop(Regcomp parser, int min, int max)
    {
      return new CharLoop(this, min, max);
    }

    RegexpNode override createLoopUngreedy(Regcomp parser, int min, int max)
    {
      return new CharUngreedyLoop(this, min, max);
    }

    int override minLength()
    {
      return 1;
    }
  }

  static class CharNode : AbstractCharNode {
    private char _ch;

    CharNode(char ch)
    {
      _ch = ch;
    }

    int override firstChar()
    {
      return _ch;
    }

    bool override []firstSet(bool []firstSet)
    {
      if (firstSet != null && _ch < firstSet.length) {
        firstSet[_ch] = true;

        return firstSet;
      }
      else
        return null;
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      if (offset < length && string.charAt(offset) == _ch)
        return offset + 1;
      else
        return -1;
    }
  }

  readonly AnchorBegin ANCHOR_BEGIN = new AnchorBegin();
  const AnchorBeginOrNewline ANCHOR_BEGIN_OR_NEWLINE
    = new AnchorBeginOrNewline();

  const AnchorBeginRelative ANCHOR_BEGIN_RELATIVE
   = new AnchorBeginRelative();

  readonly AnchorEnd ANCHOR_END = new AnchorEnd();
  readonly AnchorEndOnly ANCHOR_END_ONLY = new AnchorEndOnly();
  const AnchorEndOrNewline ANCHOR_END_OR_NEWLINE
    = new AnchorEndOrNewline();

  static class AnchorBegin : NullableNode {
    bool override isAnchorBegin()
    {
      return true;
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      if (offset == 0)
        return offset;
      else
        return -1;
    }
  }

  private static class AnchorBeginOrNewline : NullableNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset == 0 || string.charAt(offset - 1) == '\n')
        return offset;
      else
        return -1;
    }
  }

  static class AnchorBeginRelative : NullableNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset == state._start)
        return offset;
      else
        return -1;
    }
  }

  private static class AnchorEnd : NullableNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset == strlen
          || offset + 1 == strlen && string.charAt(offset) == '\n')
        return offset;
      else
        return -1;
    }
  }

  private static class AnchorEndOnly : NullableNode {
    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      if (offset == length)
        return offset;
      else
        return -1;
    }
  }

  private static class AnchorEndOrNewline : NullableNode {
    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      if (offset == length || string.charAt(offset) == '\n')
        return offset;
      else
        return -1;
    }
  }

  const RegexpNode DIGIT = RegexpSet.DIGIT.createNode();
  const RegexpNode NOT_DIGIT = RegexpSet.DIGIT.createNotNode();

  const RegexpNode DOT = RegexpSet.DOT.createNotNode();
  const RegexpNode NOT_DOT = RegexpSet.DOT.createNode();

  const RegexpNode SPACE = RegexpSet.SPACE.createNode();
  const RegexpNode NOT_SPACE = RegexpSet.SPACE.createNotNode();

  const RegexpNode S_WORD = RegexpSet.WORD.createNode();
  const RegexpNode NOT_S_WORD = RegexpSet.WORD.createNotNode();

  static class AsciiSet : AbstractCharNode {
    private bool []_set;

    AsciiSet()
    {
      _set = new boolean[128];
    }

    AsciiSet(bool []set)
    {
      _set = set;
    }

    bool override []firstSet(bool []firstSet)
    {
      if (firstSet == null)
        return null;

      for (int i = 0; i < _set.length; i++) {
        if (_set[i])
          firstSet[i] = true;
      }

      return firstSet;
    }

    void setChar(char ch)
    {
      _set[ch] = true;
    }

    void clearChar(char ch)
    {
      _set[ch] = false;
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      if (length <= offset)
        return -1;

      char ch = string.charAt(offset);

      if (ch < 128 && _set[ch])
        return offset + 1;
      else
        return -1;
    }
  }

  static class AsciiNotSet : AbstractCharNode {
    private bool []_set;

    AsciiNotSet()
    {
      _set = new boolean[128];
    }

    AsciiNotSet(bool []set)
    {
      _set = set;
    }

    void setChar(char ch)
    {
      _set[ch] = true;
    }

    void clearChar(char ch)
    {
      _set[ch] = false;
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      if (length <= offset) {
        return -1;
      }

      char ch = string.charAt(offset);

      if (ch < 128 && _set[ch]) {
        return -1;
      }
      else if (Character.isHighSurrogate(ch)
               && offset + 1 < length
               && Character.isLowSurrogate(string.charAt(offset + 1))) {
        // php/4ef3
        return offset + 2;
      }
      else {
        return offset + 1;
      }
    }
  }

  static class CharLoop : RegexpNode {
    private RegexpNode _node;
    private RegexpNode _next = N_END;

    private int _min;
    private int _max;

    CharLoop(RegexpNode node, int min, int max)
    {
      _node = node.getHead();
      _min = min;
      _max = max;

      if (_min < 0)
        throw new IllegalStateException();
    }

    RegexpNode override copyImpl(HashMap<RegexpNode,RegexpNode> state)
    {
      RegexpNode next = _next.copy(state);
      RegexpNode node = _node.copy(state);

      CharLoop copy = new CharLoop(node, _min, _max);
      copy._next = next;

      return copy;
    }

    RegexpNode override concat(RegexpNode next)
    {
      if (next == null)
        throw new NullPointerException();

      if (_next != null)
        _next = _next.concat(next);
      else
        _next = next.getHead();

      return this;
    }

    RegexpNode override createLoop(Regcomp parser, int min, int max)
    {
      if (min == 0 && max == 1) {
        _min = 0;

        return this;
      }
      else
        return new LoopHead(parser, this, min, max);
    }

    int override minLength()
    {
      return _min;
    }

    bool override []firstSet(bool []firstSet)
    {
      firstSet = _node.firstSet(firstSet);

      if (_min > 0 && ! _node.isNullable())
        return firstSet;

      firstSet = _next.firstSet(firstSet);

      return firstSet;
    }

    //
    // match functions
    //

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      RegexpNode next = _next;
      RegexpNode node = _node;
      int min = _min;
      int max = _max;

      int i;

      int tail;

      for (i = 0; i < min; i++) {
        tail = node.match(string, length, offset + i, state);
        if (tail < 0)
          return tail;
      }

      for (; i < max; i++) {
        if (node.match(string, length, offset + i, state) < 0) {
          break;
        }
      }

      for (; min <= i; i--) {
        tail = next.match(string, length, offset + i, state);

        if (tail >= 0)
          return tail;
      }

      return -1;
    }

    protected override void ToString(StringBuilder sb, Map<RegexpNode,Integer> map)
    {
      if (ToStringAdd(sb, map))
        return;

      sb.append(ToStringName());
      sb.append("[").append(_min).append(", ").append(_max).append(", ");

      _node.ToString(sb, map);
      sb.append(", ");
      _next.ToString(sb, map);
      sb.append("]");
    }
  }

  static class CharUngreedyLoop : RegexpNode {
    private RegexpNode _node;
    private RegexpNode _next = N_END;

    private int _min;
    private int _max;

    CharUngreedyLoop(RegexpNode node, int min, int max)
    {
      _node = node.getHead();
      _min = min;
      _max = max;

      if (_min < 0)
        throw new IllegalStateException();
    }

    RegexpNode override copyImpl(HashMap<RegexpNode,RegexpNode> state)
    {
      RegexpNode next = _next.copy(state);
      RegexpNode node = _node.copy(state);

      CharUngreedyLoop copy = new CharUngreedyLoop(node, _min, _max);
      copy._next = next;

      return copy;
    }

    RegexpNode override concat(RegexpNode next)
    {
      if (next == null)
        throw new NullPointerException();

      if (_next != null)
        _next = _next.concat(next);
      else
        _next = next.getHead();

      return this;
    }

    RegexpNode override createLoop(Regcomp parser, int min, int max)
    {
      if (min == 0 && max == 1) {
        _min = 0;

        return this;
      }
      else
        return new LoopHead(parser, this, min, max);
    }

    int override minLength()
    {
      return _min;
    }

    bool override []firstSet(bool []firstSet)
    {
      firstSet = _node.firstSet(firstSet);

      if (_min > 0 && ! _node.isNullable())
        return firstSet;

      firstSet = _next.firstSet(firstSet);

      return firstSet;
    }

    //
    // match functions
    //

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      RegexpNode next = _next;
      RegexpNode node = _node;
      int min = _min;
      int max = _max;

      int i;

      int tail;

      for (i = 0; i < min; i++) {
        tail = node.match(string, length, offset + i, state);
        if (tail < 0)
          return tail;
      }

      for (; i <= max; i++) {
        tail = next.match(string, length, offset + i, state);

        if (tail >= 0)
          return tail;

        if (node.match(string, length, offset + i, state) < 0) {
          return -1;
        }
      }

      return -1;
    }

    public override string ToString()
    {
      return "CharUngreedyLoop[" + _min + ", "
          + _max + ", " + _node + ", " + _next + "]";
    }
  }

 static class Concat : RegexpNode {
    private RegexpNode _head;
    private RegexpNode _next;

    Concat(RegexpNode head, RegexpNode next)
    {
      if (head == null || next == null)
        throw new NullPointerException();

      _head = head;
      _next = next;
    }

    RegexpNode override copyImpl(HashMap<RegexpNode,RegexpNode> state)
    {
      RegexpNode head = _head.copy(state);
      RegexpNode next = _next.copy(state);

      return new Concat(head, next);
    }

    RegexpNode override concat(RegexpNode next)
    {
      _next = _next.concat(next);

      return this;
    }

    //
    // optim functions
    //

    int override minLength()
    {
      return _head.minLength() + _next.minLength();
    }

    int override firstChar()
    {
      return _head.firstChar();
    }

    bool override []firstSet(bool []firstSet)
    {
      firstSet = _head.firstSet(firstSet);

      if (_head.isNullable())
        firstSet = _next.firstSet(firstSet);

      return firstSet;
    }

    string override prefix()
    {
      return _head.prefix();
    }

    bool override isAnchorBegin()
    {
      return _head.isAnchorBegin();
    }

    RegexpNode getConcatHead()
    {
      return _head;
    }

    RegexpNode getConcatNext()
    {
      return _next;
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      offset = _head.match(string, length, offset, state);

      if (offset < 0)
        return -1;
      else
        return _next.match(string, length, offset, state);
    }

    protected override void ToString(StringBuilder sb, Map<RegexpNode,Integer> map)
    {
      if (ToStringAdd(sb, map))
        return;

      sb.append(ToStringName());
      sb.append("[");
      _head.ToString(sb, map);
      sb.append(", ");
      _next.ToString(sb, map);
      sb.append("]");
    }
  }

  abstract static class ConditionalHead : RegexpNode {
    protected RegexpNode _first;
    protected RegexpNode _second;
    protected RegexpNode _tail = new ConditionalTail(this);

    void setFirst(RegexpNode first)
    {
      _first = first;
    }

    void setSecond(RegexpNode second)
    {
      _second = second;
    }

    void setTail(RegexpNode tail)
    {
      _tail = tail;
    }

    RegexpNode override getTail()
    {
      return _tail;
    }

    RegexpNode override concat(RegexpNode next)
    {
      _tail.concat(next);

      return this;
    }

    RegexpNode override createLoop(Regcomp parser, int min, int max)
    {
      return _tail.createLoop(parser, min, max);
    }

    /**
     * Create an or expression
     */
    RegexpNode override createOr(RegexpNode node)
    {
      return _tail.createOr(node);
    }
  }

  static class GenericConditionalHead : ConditionalHead {
    private RegexpNode _conditional;

    GenericConditionalHead(RegexpNode conditional)
    {
      _conditional = conditional;
    }

    RegexpNode override copyImpl(HashMap<RegexpNode,RegexpNode> state)
    {
      RegexpNode conditional = _conditional.copy(state);

      GenericConditionalHead copy = new GenericConditionalHead(conditional);
      state.put(this, copy);

      copy._first = _first.copy(state);
      copy._second = _second.copy(state);
      copy._tail = _tail.copy(state);

      return copy;
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      if (_conditional.match(string, length, offset, state) >= 0) {
        int match = _first.match(string, length, offset, state);
        return match;
      }
      else if (_second != null)
        return _second.match(string, length, offset, state);
      else
        return _tail.match(string, length, offset, state);
    }

    public override string ToString()
    {
      return getClass().getSimpleName() + "[" + _conditional
                                        + "," + _first
                                        + "," + _tail
                                        + "]";
    }
  }

  static class GroupConditionalHead : ConditionalHead {
    private int _group;

    GroupConditionalHead(int group)
    {
      _group = group;
    }

    RegexpNode override copyImpl(HashMap<RegexpNode,RegexpNode> state)
    {
      GroupConditionalHead copy = new GroupConditionalHead(_group);
      state.put(this, copy);

      copy._first = _first.copy(state);
      copy._second = _second.copy(state);
      copy._tail = _tail.copy(state);

      return copy;
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      int begin = state.getBegin(_group);
      int end = state.getEnd(_group);

      if (_group <= state.getLength() && begin >= 0 && begin <= end) {
        int match = _first.match(string, length, offset, state);
        return match;
      }
      else if (_second != null)
        return _second.match(string, length, offset, state);
      else
        return _tail.match(string, length, offset, state);
    }

    public override string ToString()
    {
      return getClass().getSimpleName() + "[" + _group
                                        + "," + _first
                                        + "," + _tail
                                        + "]";
    }
  }

  static class ConditionalTail : RegexpNode {
    private RegexpNode _head;
    private RegexpNode _next;

    private ConditionalTail()
    {
    }

    ConditionalTail(ConditionalHead head)
    {
      _next = N_END;
      _head = head;
      head.setTail(this);
    }

    RegexpNode override getHead()
    {
      return _head;
    }

    RegexpNode override copyImpl(HashMap<RegexpNode,RegexpNode> state)
    {
      ConditionalTail copy = new ConditionalTail();
      state.put(this, copy);

      copy._head = _head.copy(state);
      copy._next = _next.copy(state);

      return copy;
    }

    RegexpNode override concat(RegexpNode next)
    {
      if (_next != null)
        _next = _next.concat(next);
      else
        _next = next;

      return _head;
    }

    RegexpNode override createLoop(Regcomp parser, int min, int max)
    {
      LoopHead head = new LoopHead(parser, _head, min, max);

      _next = _next.concat(head.getTail());

      return head;
    }

    RegexpNode override createLoopUngreedy(Regcomp parser, int min, int max)
    {
      LoopHeadUngreedy head = new LoopHeadUngreedy(parser, _head, min, max);

      _next = _next.concat(head.getTail());

      return head;
    }

    /**
     * Create an or expression
     */
    RegexpNode override createOr(RegexpNode node)
    {
      _next = _next.createOr(node);

      return getHead();
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      return _next.match(string, length, offset, state);
    }
  }

  readonly EmptyNode EMPTY = new EmptyNode();

  /**
   * Matches an empty production
   */
  static class EmptyNode : RegexpNode {
    // needed for php/4e6b

    EmptyNode()
    {
    }


    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      return offset;
    }
  }

  static class End : RegexpNode {
    RegexpNode override concat(RegexpNode next)
    {
      return next;
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      return offset;
    }
  }

  static class Group : RegexpNode {
    private RegexpNode _node;
    private int _group;

    Group(RegexpNode node, int group)
    {
      _node = node.getHead();
      _group = group;
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      int oldBegin = state.getBegin(_group);

      state.setBegin(_group, offset);

      int tail = _node.match(string, length, offset, state);

      if (tail >= 0) {
        state.setEnd(_group, tail);
        return tail;
      }
      else {
        state.setBegin(_group, oldBegin);

        return -1;
      }
    }
  }

  static class GroupHead : RegexpNode {
    private RegexpNode _node;
    private GroupTail _tail;
    private int _group;

    private GroupHead()
    {
    }

    GroupHead(int group)
    {
      _group = group;
      _tail = new GroupTail(group, this);
    }

    void setNode(RegexpNode node)
    {
      _node = node.getHead();

      // php/4eh1
      if (_node == this)
        _node = _tail;
    }

    RegexpNode override getTail()
    {
      return _tail;
    }

    RegexpNode getNode()
    {
      return _node;
    }

    RegexpNode override copyImpl(HashMap<RegexpNode,RegexpNode> state)
    {
      GroupHead copy = new GroupHead();
      state.put(this, copy);

      copy._group = _group;

      if (_node == this) {
        copy._node = copy;
      }
      else if (_node == null) {
      }
      else {
        copy._node = _node.copy(state);
      }

      copy._tail = (GroupTail) _tail.copy(state);

      return copy;
    }

    RegexpNode override concat(RegexpNode next)
    {
      _tail.concat(next);

      return this;
    }

    RegexpNode override createLoop(Regcomp parser, int min, int max)
    {
      return _tail.createLoop(parser, min, max);
    }

    RegexpNode override createLoopUngreedy(Regcomp parser, int min, int max)
    {
      return _tail.createLoopUngreedy(parser, min, max);
    }

    int override minLength()
    {
      return _node.minLength();
    }

    int override firstChar()
    {
      return _node.firstChar();
    }

    bool override []firstSet(bool []firstSet)
    {
      return _node.firstSet(firstSet);
    }

    string override prefix()
    {
      return _node.prefix();
    }

    bool override isAnchorBegin()
    {
      return _node.isAnchorBegin();
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      int oldBegin = state.getBegin(_group);
      state.setBegin(_group, offset);

      int tail = _node.match(string, length, offset, state);

      if (tail >= 0) {
        return tail;
      }
      else {
        state.setBegin(_group, oldBegin);
        return tail;
      }
    }

    protected override void ToString(StringBuilder sb, Map<RegexpNode,Integer> map)
    {
      if (ToStringAdd(sb, map))
        return;

      sb.append(ToStringName());
      sb.append("[");
      sb.append(_group);
      sb.append(", ");
      _node.ToString(sb, map);
      sb.append("]");
    }
  }

  static class GroupTail : RegexpNode {
    private GroupHead _head;
    private RegexpNode _next;
    private int _group;

    private GroupTail(int group)
    {
      _group = group;
    }

    private GroupTail(int group, GroupHead head)
    {
      _next = N_END;
      _head = head;
      _group = group;
    }

    RegexpNode override getHead()
    {
      return _head;
    }

    RegexpNode override copyImpl(HashMap<RegexpNode,RegexpNode> state)
    {
      GroupTail tail = new GroupTail(_group);
      state.put(this, tail);

      GroupHead head = (GroupHead) _head.copy(state);

      tail._head = head;
      tail._next = _next.copy(state);

      return tail;
    }

    RegexpNode override concat(RegexpNode next)
    {
      if (_next != null) {
        _next = _next.concat(next);
      }
      else {
        _next = next;
      }

      return _head;
    }

    RegexpNode override createLoop(Regcomp parser, int min, int max)
    {
      LoopHead head = new LoopHead(parser, _head, min, max);

      _next = head.getTail();

      return head;
    }

    RegexpNode override createLoopUngreedy(Regcomp parser, int min, int max)
    {
      LoopHeadUngreedy head = new LoopHeadUngreedy(parser, _head, min, max);

      _next = head.getTail();

      return head;
    }

    /**
     * Create an or expression
     */
    // php/4e6b
    /*
    RegexpNode override createOr(RegexpNode node)
    {
      _next = _next.createOr(node);

      return getHead();
    }
    */

    int override minLength()
    {
      return _next.minLength();
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      if (state.isFinalized(_group)) {
        return _next.match(string, length, offset, state);
      }

      int oldEnd = state.getEnd(_group);
      int oldLength = state.getLength();

      if (_group > 0) {
        state.setEnd(_group, offset);

        if (oldLength < _group)
          state.setLength(_group);
      }

      int tail = _next.match(string, length, offset, state);

      if (tail < 0) {
        state.setEnd(_group, oldEnd);
        state.setLength(oldLength);

        return -1;
      }
      else {
        return tail;
      }
    }

    protected override void ToString(StringBuilder sb, Map<RegexpNode,Integer> map)
    {
      if (ToStringAdd(sb, map))
        return;

      sb.append(ToStringName());
      sb.append("[");
      sb.append(_group);
      sb.append(", ");
      _next.ToString(sb, map);
      sb.append("]");
    }
  }

  static class GroupRef : RegexpNode {
    private int _group;

    GroupRef(int group)
    {
      _group = group;
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      if (state.getLength() < _group)
        return -1;

      int groupBegin = state.getBegin(_group);
      int groupLength = state.getEnd(_group) - groupBegin;

      if (string.regionMatches(offset, string, groupBegin, groupLength)) {
        return offset + groupLength;
      }
      else
        return -1;
    }
  }

  static class Lookahead : RegexpNode {
    private RegexpNode _head;

    Lookahead(RegexpNode head)
    {
      _head = head;
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      if (_head.match(string, length, offset, state) >= 0)
        return offset;
      else
        return -1;
    }
  }

  static class NotLookahead : RegexpNode {
    private RegexpNode _head;

    NotLookahead(RegexpNode head)
    {
      _head = head;
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      if (_head.match(string, length, offset, state) < 0)
        return offset;
      else
        return -1;
    }
  }

  static class Lookbehind : RegexpNode {
    private RegexpNode _head;

    Lookbehind(RegexpNode head)
    {
      _head = head.getHead();
    }

    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      int length = _head.minLength();

      if (offset < length)
        return -1;
      else if (_head.match(string, strlen, offset - length, state) >= 0)
        return offset;
      else
        return -1;
    }
  }

  static class NotLookbehind : RegexpNode {
    private RegexpNode _head;

    NotLookbehind(RegexpNode head)
    {
      _head = head;
    }

    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      int length = _head.minLength();

      if (offset < length)
        return offset;
      else if (_head.match(string, strlen, offset - length, state) < 0)
        return offset;
      else
        return -1;
    }
  }

  /**
   * A nullable node can match an empty string.
   */
  abstract static class NullableNode : RegexpNode {
    bool override isNullable()
    {
      return true;
    }
  }

  static class LoopHead : RegexpNode {
    private int _index;

    RegexpNode _node;
    private RegexpNode _tail;

    private int _min;
    private int _max;

    private LoopHead(int index, int min, int max)
    {
      _index = index;
      _min = min;
      _max = max;
    }

    LoopHead(Regcomp parser, RegexpNode node, int min, int max)
    {
      _index = parser.nextLoopIndex();
      _tail = new LoopTail(_index, this);
      _node = node.concat(_tail).getHead();
      _min = min;
      _max = max;
    }

    RegexpNode override getTail()
    {
      return _tail;
    }

    RegexpNode override copyImpl(HashMap<RegexpNode,RegexpNode> state)
    {
      LoopHead head = new LoopHead(_index, _min, _max);
      state.put(this, head);

      RegexpNode node = _node.copy(state);
      RegexpNode tail = _tail.copy(state);

      head._node = node;
      head._tail = tail;

      return head;
    }

    RegexpNode override concat(RegexpNode next)
    {
      _tail.concat(next);

      return this;
    }

    RegexpNode override createLoop(Regcomp parser, int min, int max)
    {
      if (min == 0 && max == 1) {
        _min = 0;

        return this;
      }
      else
        return new LoopHead(parser, this, min, max);
    }

    int override minLength()
    {
      return _min * _node.minLength() + _tail.minLength();
    }

    bool override []firstSet(bool []firstSet)
    {
      firstSet = _node.firstSet(firstSet);

      if (_min > 0 && ! _node.isNullable())
        return firstSet;

      firstSet = _tail.firstSet(firstSet);

      return firstSet;
    }

    //
    // match functions
    //

    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      state._loopCount[_index] = 0;

      RegexpNode node = _node;
      int min = _min;
      int i;
      for (i = 0; i < min - 1; i++) {
        state._loopCount[_index] = i;

        offset = node.match(string, strlen, offset, state);

        if (offset < 0)
          return offset;
      }

      state._loopCount[_index] = i;
      state._loopOffset[_index] = offset;
      int tail = node.match(string, strlen, offset, state);

      if (tail >= 0) {
        return tail;
      }
      else if (state._loopCount[_index] < _min) {
        return tail;
      }
      else {
        return _tail.match(string, strlen, offset, state);
      }
    }

    public override string ToString()
    {
      return "LoopHead[" + _min + ", " + _max + ", " + _node + "]";
    }
  }

  static class LoopTail : RegexpNode {
    private int _index;

    private LoopHead _head;
    private RegexpNode _next;

    private LoopTail(int index)
    {
      _index = index;
    }

    LoopTail(int index, LoopHead head)
    {
      _index = index;
      _head = head;
      _next = N_END;
    }

    RegexpNode override getHead()
    {
      return _head;
    }

    RegexpNode override copyImpl(HashMap<RegexpNode,RegexpNode> state)
    {
      LoopTail tail = new LoopTail(_index);
      state.put(this, tail);

      LoopHead head = (LoopHead) _head.copy(state);
      RegexpNode next = _next.copy(state);

      tail._head = head;
      tail._next = next;

      return tail;
    }

    RegexpNode override concat(RegexpNode next)
    {
      if (_next != null)
        _next = _next.concat(next);
      else
        _next = next;

      if (_next == this)
        throw new IllegalStateException();

      return this;
    }

    //
    // match functions
    //

    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      int oldCount = state._loopCount[_index];

      if (oldCount + 1 < _head._min) {
        return offset;
      }
      else if (oldCount + 1 < _head._max) {
        int oldOffset = state._loopOffset[_index];

        if (oldOffset != offset) {
          state._loopCount[_index] = oldCount + 1;
          state._loopOffset[_index] = offset;

          int tail = _head._node.match(string, strlen, offset, state);

          if (tail >= 0) {
            return tail;
          }

          state._loopCount[_index] = oldCount;
          state._loopOffset[_index] = oldOffset;
        }
      }

      int match = _next.match(string, strlen, offset, state);

      return match;
    }

    public override string ToString()
    {
      return "LoopTail[" + _next + "]";
    }
  }

  static class LoopHeadUngreedy : RegexpNode {
    private int _index;

    RegexpNode _node;
    private LoopTailUngreedy _tail;

    private int _min;
    private int _max;

    private LoopHeadUngreedy(int index, int min, int max)
    {
      _index = index;

      _min = min;
      _max = max;
    }

    LoopHeadUngreedy(Regcomp parser, RegexpNode node, int min, int max)
    {
      _index = parser.nextLoopIndex();
      _min = min;
      _max = max;

      _tail = new LoopTailUngreedy(_index, this);
      _node = node.getTail().concat(_tail).getHead();
    }

    RegexpNode override getTail()
    {
      return _tail;
    }

    RegexpNode override copyImpl(HashMap<RegexpNode,RegexpNode> state)
    {
      LoopHeadUngreedy copy = new LoopHeadUngreedy(_index, _min, _max);
      state.put(this, copy);

      RegexpNode tail = _tail.copy(state);
      RegexpNode node = _node.copy(state);

      copy._tail = (LoopTailUngreedy) tail;
      copy._node = node;

      return copy;
    }

    RegexpNode override concat(RegexpNode next)
    {
      _tail.concat(next);

      return this;
    }

    RegexpNode override createLoop(Regcomp parser, int min, int max)
    {
      if (min == 0 && max == 1) {
        _min = 0;

        return this;
      }
      else
        return new LoopHead(parser, this, min, max);
    }

    int override minLength()
    {
      return _min * _node.minLength() + _tail.minLength();
    }

    //
    // match functions
    //

    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      state._loopCount[_index] = 0;

      RegexpNode node = _node;
      int min = _min;

      for (int i = 0; i < min; i++) {
        state._loopCount[_index] = i;
        state._loopOffset[_index] = offset;

        offset = node.match(string, strlen, offset, state);

        if (offset < 0)
          return -1;
      }

      int tail = _tail._next.match(string, strlen, offset, state);
      if (tail >= 0)
        return tail;

      if (min < _max) {
        state._loopCount[_index] = min;
        state._loopOffset[_index] = offset;

        return node.match(string, strlen, offset, state);
      }
      else
        return -1;
    }

    public override string ToString()
    {
      return "LoopHeadUngreedy[" + _min + ", " + _max + ", " + _node + "]";
    }
  }

  static class LoopTailUngreedy : RegexpNode {
    private int _index;

    private LoopHeadUngreedy _head;
    private RegexpNode _next;

    private LoopTailUngreedy(int index)
    {
      _index = index;
    }

    LoopTailUngreedy(int index, LoopHeadUngreedy head)
    {
      _index = index;
      _head = head;
      _next = N_END;
    }

    RegexpNode override getHead()
    {
      return _head;
    }

    RegexpNode override copyImpl(HashMap<RegexpNode,RegexpNode> state)
    {
      LoopTailUngreedy copy = new LoopTailUngreedy(_index);
      state.put(this, copy);

      RegexpNode head = _head.copy(state);
      RegexpNode next = _next.copy(state);

      copy._head = (LoopHeadUngreedy) head;
      copy._next = next;

      return copy;
    }

    RegexpNode override concat(RegexpNode next)
    {
      if (_next != null)
        _next = _next.concat(next);
      else
        _next = next;

      if (_next == this)
        throw new IllegalStateException();

      return this;
    }

    //
    // match functions
    //

    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      int i = state._loopCount[_index];
      int oldOffset = state._loopOffset[_index];

      if (i < _head._min)
        return offset;

      if (offset == oldOffset)
        return -1;

      int tail = _next.match(string, strlen, offset, state);
      if (tail >= 0)
        return tail;

      if (i + 1 < _head._max) {
        state._loopCount[_index] = i + 1;
        state._loopOffset[_index] = offset;

        tail = _head._node.match(string, strlen, offset, state);

        state._loopCount[_index] = i;
        state._loopOffset[_index] = oldOffset;

        return tail;
      }
      else
        return -1;
    }

    public override string ToString()
    {
      return "LoopTailUngreedy[" + _next + "]";
    }
  }

  static class Not : RegexpNode {
    private RegexpNode _node;

    private Not(RegexpNode node)
    {
      _node = node;
    }

    static Not create(RegexpNode node)
    {
      return new Not(node);
    }

    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      int result = _node.match(string, strlen, offset, state);

      if (result >= 0)
        return -1;
      else
        return offset + 1;
    }
  }

 static class Or : RegexpNode {
    private RegexpNode _left;
    private Or _right;

    private Or(RegexpNode left, Or right)
    {
      _left = left;
      _right = right;
    }

    static Or create(RegexpNode left, RegexpNode right)
    {
      if (left instanceof Or)
        return ((Or) left).append(right);
      else if (right instanceof Or)
        return new Or(left, (Or) right);
      else
        return new Or(left, new Or(right, null));
    }

    private Or append(RegexpNode right)
    {
      if (_right != null)
        _right = _right.append(right);
      else if (right instanceof Or)
        _right = (Or) right;
      else
        _right = new Or(right, null);

      return this;
    }

    RegexpNode override copyImpl(HashMap<RegexpNode,RegexpNode> state)
    {
      RegexpNode left = _left.copy(state);
      RegexpNode right = null;

      if (_right != null) {
        right = _right.copy(state);
      }

      Or copy = new Or(left, (Or) right);

      return copy;
    }

    int override minLength()
    {
      if (_right != null)
        return Math.min(_left.minLength(), _right.minLength());
      else
        return _left.minLength();
    }

    int override firstChar()
    {
      if (_right == null)
        return _left.firstChar();

      int leftChar = _left.firstChar();
      int rightChar = _right.firstChar();

      if (leftChar == rightChar)
        return leftChar;
      else
        return -1;
    }

    bool override []firstSet(bool []firstSet)
    {
      if (_right == null)
        return _left.firstSet(firstSet);

      firstSet = _left.firstSet(firstSet);
      firstSet = _right.firstSet(firstSet);

      return firstSet;
    }

    bool override isAnchorBegin()
    {
      return _left.isAnchorBegin() && _right != null && _right.isAnchorBegin();
    }

    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      for (Or ptr = this; ptr != null; ptr = ptr._right) {
        int value = ptr._left.match(string, strlen, offset, state);

        if (value >= 0)
          return value;
      }

      return -1;
    }

    protected override void ToString(StringBuilder sb, Map<RegexpNode,Integer> map)
    {
      if (ToStringAdd(sb, map))
        return;

      sb.append(ToStringName());
      sb.append("[");
      _left.ToString(sb, map);

      for (Or ptr = _right; ptr != null; ptr = ptr._right) {
        sb.append(",");
        ptr._left.ToString(sb, map);
      }

      sb.append("]");
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.append("Or[");
      sb.append(_left);

      for (Or ptr = _right; ptr != null; ptr = ptr._right) {
        sb.append(",");
        sb.append(ptr._left);
      }
      sb.append("]");
      return sb.ToString();
    }
  }

  static class PossessiveLoop : RegexpNode {
    private RegexpNode _node;
    private RegexpNode _next = N_END;

    private int _min;
    private int _max;

    private PossessiveLoop(int min, int max)
    {
      _min = min;
      _max = max;
    }

    PossessiveLoop(RegexpNode node, int min, int max)
    {
      _node = node.getHead();

      _min = min;
      _max = max;
    }

    RegexpNode override copyImpl(HashMap<RegexpNode,RegexpNode> state)
    {
      PossessiveLoop copy = new PossessiveLoop(_min, _max);
      state.put(this, copy);

      RegexpNode node = _node.copy(state);
      RegexpNode next = _next.copy(state);

      copy._node = node;
      copy._next = next;

      return copy;
    }

    RegexpNode override concat(RegexpNode next)
    {
      if (next == null)
        throw new NullPointerException();

      if (_next != null)
        _next = _next.concat(next);
      else
        _next = next;

      return this;
    }

    RegexpNode override createLoop(Regcomp parser, int min, int max)
    {
      if (min == 0 && max == 1) {
        _min = 0;

        return this;
      }
      else
        return new LoopHead(parser, this, min, max);
    }

    //
    // match functions
    //

    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      RegexpNode node = _node;

      int min = _min;
      int max = _max;

      int i;

      for (i = 0; i < min; i++) {
        offset = node.match(string, strlen, offset, state);

        if (offset < 0)
          return -1;
      }

      for (; i < max; i++) {
        int tail = node.match(string, strlen, offset, state);

        if (tail < 0 || tail == offset)
          return _next.match(string, strlen, offset, state);

        offset = tail;
      }

      return _next.match(string, strlen, offset, state);
    }

    public override string ToString()
    {
      return "PossessiveLoop[" + _min + ", "
          + _max + ", " + _node + ", " + _next + "]";
    }
  }

  readonly PropC PROP_C = new PropC();
  readonly PropNotC PROP_NOT_C = new PropNotC();

  readonly Prop PROP_Cc = new Prop(Character.CONTROL);
  readonly PropNot PROP_NOT_Cc = new PropNot(Character.CONTROL);

  readonly Prop PROP_Cf = new Prop(Character.FORMAT);
  readonly PropNot PROP_NOT_Cf = new PropNot(Character.FORMAT);

  readonly Prop PROP_Cn = new Prop(Character.UNASSIGNED);
  readonly PropNot PROP_NOT_Cn = new PropNot(Character.UNASSIGNED);

  readonly Prop PROP_Co = new Prop(Character.PRIVATE_USE);
  readonly PropNot PROP_NOT_Co = new PropNot(Character.PRIVATE_USE);

  readonly Prop PROP_Cs = new Prop(Character.SURROGATE);
  readonly PropNot PROP_NOT_Cs = new PropNot(Character.SURROGATE);

  readonly PropL PROP_L = new PropL();
  readonly PropNotL PROP_NOT_L = new PropNotL();

  readonly Prop PROP_Ll = new Prop(Character.LOWERCASE_LETTER);
  readonly PropNot PROP_NOT_Ll = new PropNot(Character.LOWERCASE_LETTER);

  readonly Prop PROP_Lm = new Prop(Character.MODIFIER_LETTER);
  readonly PropNot PROP_NOT_Lm = new PropNot(Character.MODIFIER_LETTER);

  readonly Prop PROP_Lo = new Prop(Character.OTHER_LETTER);
  readonly PropNot PROP_NOT_Lo = new PropNot(Character.OTHER_LETTER);

  readonly Prop PROP_Lt = new Prop(Character.TITLECASE_LETTER);
  readonly PropNot PROP_NOT_Lt = new PropNot(Character.TITLECASE_LETTER);

  readonly Prop PROP_Lu = new Prop(Character.UPPERCASE_LETTER);
  readonly PropNot PROP_NOT_Lu = new PropNot(Character.UPPERCASE_LETTER);

  readonly PropM PROP_M = new PropM();
  readonly PropNotM PROP_NOT_M = new PropNotM();

  readonly Prop PROP_Mc = new Prop(Character.COMBINING_SPACING_MARK);
  const PropNot PROP_NOT_Mc
    = new PropNot(Character.COMBINING_SPACING_MARK);

  readonly Prop PROP_Me = new Prop(Character.ENCLOSING_MARK);
  readonly PropNot PROP_NOT_Me = new PropNot(Character.ENCLOSING_MARK);

  readonly Prop PROP_Mn = new Prop(Character.NON_SPACING_MARK);
  readonly PropNot PROP_NOT_Mn = new PropNot(Character.NON_SPACING_MARK);

  readonly PropN PROP_N = new PropN();
  readonly PropNotN PROP_NOT_N = new PropNotN();

  readonly Prop PROP_Nd = new Prop(Character.DECIMAL_DIGIT_NUMBER);
  const PropNot PROP_NOT_Nd
    = new PropNot(Character.DECIMAL_DIGIT_NUMBER);

  readonly Prop PROP_Nl = new Prop(Character.LETTER_NUMBER);
  readonly PropNot PROP_NOT_Nl = new PropNot(Character.LETTER_NUMBER);

  readonly Prop PROP_No = new Prop(Character.OTHER_NUMBER);
  readonly PropNot PROP_NOT_No = new PropNot(Character.OTHER_NUMBER);

  readonly PropP PROP_P = new PropP();
  readonly PropNotP PROP_NOT_P = new PropNotP();

  readonly Prop PROP_Pc = new Prop(Character.CONNECTOR_PUNCTUATION);
  const PropNot PROP_NOT_Pc
    = new PropNot(Character.CONNECTOR_PUNCTUATION);

  readonly Prop PROP_Pd = new Prop(Character.DASH_PUNCTUATION);
  readonly PropNot PROP_NOT_Pd = new PropNot(Character.DASH_PUNCTUATION);

  readonly Prop PROP_Pe = new Prop(Character.END_PUNCTUATION);
  readonly PropNot PROP_NOT_Pe = new PropNot(Character.END_PUNCTUATION);

  readonly Prop PROP_Pf = new Prop(Character.FINAL_QUOTE_PUNCTUATION);
  const PropNot PROP_NOT_Pf
    = new PropNot(Character.FINAL_QUOTE_PUNCTUATION);

  readonly Prop PROP_Pi = new Prop(Character.INITIAL_QUOTE_PUNCTUATION);
  const PropNot PROP_NOT_Pi
    = new PropNot(Character.INITIAL_QUOTE_PUNCTUATION);

  readonly Prop PROP_Po = new Prop(Character.OTHER_PUNCTUATION);
  readonly PropNot PROP_NOT_Po = new PropNot(Character.OTHER_PUNCTUATION);

  readonly Prop PROP_Ps = new Prop(Character.START_PUNCTUATION);
  readonly PropNot PROP_NOT_Ps = new PropNot(Character.START_PUNCTUATION);

  readonly PropS PROP_S = new PropS();
  readonly PropNotS PROP_NOT_S = new PropNotS();

  readonly Prop PROP_Sc = new Prop(Character.CURRENCY_SYMBOL);
  readonly PropNot PROP_NOT_Sc = new PropNot(Character.CURRENCY_SYMBOL);

  readonly Prop PROP_Sk = new Prop(Character.MODIFIER_SYMBOL);
  readonly PropNot PROP_NOT_Sk = new PropNot(Character.MODIFIER_SYMBOL);

  readonly Prop PROP_Sm = new Prop(Character.MATH_SYMBOL);
  readonly PropNot PROP_NOT_Sm = new PropNot(Character.MATH_SYMBOL);

  readonly Prop PROP_So = new Prop(Character.OTHER_SYMBOL);
  readonly PropNot PROP_NOT_So = new PropNot(Character.OTHER_SYMBOL);

  readonly PropZ PROP_Z = new PropZ();
  readonly PropNotZ PROP_NOT_Z = new PropNotZ();

  readonly Prop PROP_Zl = new Prop(Character.LINE_SEPARATOR);
  readonly PropNot PROP_NOT_Zl = new PropNot(Character.LINE_SEPARATOR);

  readonly Prop PROP_Zp = new Prop(Character.PARAGRAPH_SEPARATOR);
  const PropNot PROP_NOT_Zp
    = new PropNot(Character.PARAGRAPH_SEPARATOR);

  readonly Prop PROP_Zs = new Prop(Character.SPACE_SEPARATOR);
  readonly PropNot PROP_NOT_Zs = new PropNot(Character.SPACE_SEPARATOR);

  private static class Prop : AbstractCharNode {
    private int _category;

    Prop(int category)
    {
      _category = category;
    }

    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset < strlen) {
        char ch = string.charAt(offset);

        if (Character.getType(ch) == _category)
          return offset + 1;
      }

      return -1;
    }
  }

  private static class PropNot : AbstractCharNode {
    private int _category;

    PropNot(int category)
    {
      _category = category;
    }

    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset < strlen) {
        char ch = string.charAt(offset);

        if (Character.getType(ch) != _category)
          return offset + 1;
      }

      return -1;
    }
  }

  static class PropC : AbstractCharNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset < strlen) {
        char ch = string.charAt(offset);

        int value = Character.getType(ch);

        if (value == Character.CONTROL
            || value == Character.FORMAT
            || value == Character.UNASSIGNED
            || value == Character.PRIVATE_USE
            || value == Character.SURROGATE) {
          return offset + 1;
        }
      }

      return -1;
    }
  }

  static class PropNotC : AbstractCharNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset < strlen) {
        char ch = string.charAt(offset);

        int value = Character.getType(ch);

        if (! (value == Character.CONTROL
               || value == Character.FORMAT
               || value == Character.UNASSIGNED
               || value == Character.PRIVATE_USE
               || value == Character.SURROGATE)) {
          return offset + 1;
        }
      }

      return -1;
    }
  }

  static class PropL : AbstractCharNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset < strlen) {
        char ch = string.charAt(offset);

        int value = Character.getType(ch);

        if (value == Character.LOWERCASE_LETTER
            || value == Character.MODIFIER_LETTER
            || value == Character.OTHER_LETTER
            || value == Character.TITLECASE_LETTER
            || value == Character.UPPERCASE_LETTER) {
          return offset + 1;
        }
      }

      return -1;
    }
  }

  static class PropNotL : AbstractCharNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset < strlen) {
        char ch = string.charAt(offset);

        int value = Character.getType(ch);

        if (! (value == Character.LOWERCASE_LETTER
               || value == Character.MODIFIER_LETTER
               || value == Character.OTHER_LETTER
               || value == Character.TITLECASE_LETTER
               || value == Character.UPPERCASE_LETTER)) {
          return offset + 1;
        }
      }

      return -1;
    }
  }

  static class PropM : AbstractCharNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset < strlen) {
        char ch = string.charAt(offset);

        int value = Character.getType(ch);

        if (value == Character.COMBINING_SPACING_MARK
            || value == Character.ENCLOSING_MARK
            || value == Character.NON_SPACING_MARK) {
          return offset + 1;
        }
      }

      return -1;
    }
  }

  static class PropNotM : AbstractCharNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset < strlen) {
        char ch = string.charAt(offset);

        int value = Character.getType(ch);

        if (! (value == Character.COMBINING_SPACING_MARK
               || value == Character.ENCLOSING_MARK
               || value == Character.NON_SPACING_MARK)) {
          return offset + 1;
        }
      }

      return -1;
    }
  }

  static class PropN : AbstractCharNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset < strlen) {
        char ch = string.charAt(offset);

        int value = Character.getType(ch);

        if (value == Character.DECIMAL_DIGIT_NUMBER
            || value == Character.LETTER_NUMBER
            || value == Character.OTHER_NUMBER) {
          return offset + 1;
        }
      }

      return -1;
    }
  }

  static class PropNotN : AbstractCharNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset < strlen) {
        char ch = string.charAt(offset);

        int value = Character.getType(ch);


        if (! (value == Character.DECIMAL_DIGIT_NUMBER
               || value == Character.LETTER_NUMBER
               || value == Character.OTHER_NUMBER)) {
          return offset + 1;
        }
      }

      return -1;
    }
  }

  static class PropP : AbstractCharNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset < strlen) {
        char ch = string.charAt(offset);

        int value = Character.getType(ch);

        if (value == Character.CONNECTOR_PUNCTUATION
            || value == Character.DASH_PUNCTUATION
            || value == Character.END_PUNCTUATION
            || value == Character.FINAL_QUOTE_PUNCTUATION
            || value == Character.INITIAL_QUOTE_PUNCTUATION
            || value == Character.OTHER_PUNCTUATION
            || value == Character.START_PUNCTUATION) {
          return offset + 1;
        }
      }


      return -1;
    }
  }

  static class PropNotP : AbstractCharNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset < strlen) {
        char ch = string.charAt(offset);

        int value = Character.getType(ch);

        if (! (value == Character.CONNECTOR_PUNCTUATION
               || value == Character.DASH_PUNCTUATION
               || value == Character.END_PUNCTUATION
               || value == Character.FINAL_QUOTE_PUNCTUATION
               || value == Character.INITIAL_QUOTE_PUNCTUATION
               || value == Character.OTHER_PUNCTUATION
               || value == Character.START_PUNCTUATION)) {
          return offset + 1;
        }
      }

      return -1;
    }
  }

  static class PropS : AbstractCharNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset < strlen) {
        char ch = string.charAt(offset);

        int value = Character.getType(ch);

        if (value == Character.CURRENCY_SYMBOL
            || value == Character.MODIFIER_SYMBOL
            || value == Character.MATH_SYMBOL
            || value == Character.OTHER_SYMBOL) {
          return offset + 1;
        }
      }

      return -1;
    }
  }

  static class PropNotS : AbstractCharNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset < strlen) {
        char ch = string.charAt(offset);

        int value = Character.getType(ch);

        if (! (value == Character.CURRENCY_SYMBOL
               || value == Character.MODIFIER_SYMBOL
               || value == Character.MATH_SYMBOL
               || value == Character.OTHER_SYMBOL)) {
          return offset + 1;
        }
      }

      return -1;
    }
  }

  static class PropZ : AbstractCharNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset < strlen) {
        char ch = string.charAt(offset);

        int value = Character.getType(ch);

        if (value == Character.LINE_SEPARATOR
            || value == Character.PARAGRAPH_SEPARATOR
            || value == Character.SPACE_SEPARATOR) {
          return offset + 1;
        }
      }

      return -1;
    }
  }

  static class PropNotZ : AbstractCharNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset < strlen) {
        char ch = string.charAt(offset);

        int value = Character.getType(ch);

        if (! (value == Character.LINE_SEPARATOR
               || value == Character.PARAGRAPH_SEPARATOR
               || value == Character.SPACE_SEPARATOR)) {
          return offset + 1;
        }
      }

      return -1;
    }
  }

  static class Recursive : RegexpNode {
    private int _group;
    private RegexpNode _top;

    Recursive(int group)
    {
      _group = group;
    }

    void setTop(RegexpNode top)
    {
      _top = top;
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      int oldBegin = state.getBegin(_group);

      int match = _top.match(string, length, offset, state);

      if (match >= 0) {
        if (oldBegin >= 0) {
          state.setBegin(_group, oldBegin);
        }
        else {
          state.setBegin(_group, offset);
        }
      }

      return match;
    }
  }

  static class GroupNumberRecursive : RegexpNode {
    private int _group;
    private RegexpNode _top;

    GroupNumberRecursive(int group)
    {
      _group = group;
    }

    int getGroup()
    {
      return _group;
    }

    void setTop(RegexpNode top)
    {
      _top = top;
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      int match = _top.match(string, length, offset, state);

      return match;
    }
  }

  static class GroupNameRecursive : RegexpNode {
    private StringValue _name;
    private RegexpNode _top;

    GroupNameRecursive(StringValue name)
    {
      _name = name;
    }

    StringValue getGroup()
    {
      return _name;
    }

    void setTop(RegexpNode top)
    {
      _top = top;
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      int match = _top.match(string, length, offset, state);

      return match;
    }
  }

  static class Subroutine : RegexpNode {
    private int _group;
    private RegexpNode _node;

    Subroutine(int group, RegexpNode node)
    {
      _group = group;
      _node = node;
    }

    int override match(StringValue string, int length, int offset, RegexpState state)
    {
      state.setFinalized(_group, true);

      int match = _node.match(string, length, offset, state);

      return match;
    }
  }

  static class Set : AbstractCharNode {
    private bool []_asciiSet;
    private IntSet _range;

    Set(bool []set, IntSet range)
    {
      _asciiSet = set;
      _range = range;
    }

    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (strlen <= offset)
        return -1;

      char ch = string.charAt(offset++);

      if (ch < 128)
        return _asciiSet[ch] ? offset : -1;

      int codePoint = ch;

      if ('\uD800' <= ch && ch <= '\uDBFF' && offset < strlen) {
        char low = string.charAt(offset++);

        if ('\uDC00' <= low && ch <= '\uDFFF')
          codePoint = Character.toCodePoint(ch, low);
      }

      return _range.contains(codePoint) ? offset : -1;
    }
  }



  static class NotSet : AbstractCharNode {
    private bool []_asciiSet;
    private IntSet _range;

    NotSet(bool []set, IntSet range)
    {
      _asciiSet = set;
      _range = range;
    }

    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (strlen <= offset)
        return -1;

      char ch = string.charAt(offset);

      if (ch < 128)
        return _asciiSet[ch] ? -1 : offset + 1;
      else
        return _range.contains(ch) ? -1 : offset + 1;
    }
  }

  static class StringNode : RegexpNode {
    private char []_buffer;
    private int _length;

    StringNode(CharBuffer value)
    {
      _length = value.length();
      _buffer = new char[_length];

      if (_length == 0)
        throw new IllegalStateException("empty string");

      System.arraycopy(value.getBuffer(), 0, _buffer, 0, _buffer.length);
    }

    StringNode(char []buffer, int length)
    {
      _length = length;
      _buffer = buffer;

      if (_length == 0)
        throw new IllegalStateException("empty string");
    }

    StringNode(char ch)
    {
      _length = 1;
      _buffer = new char[1];
      _buffer[0] = ch;
    }

    RegexpNode override createLoop(Regcomp parser, int min, int max)
    {
      if (_length == 1)
        return new CharLoop(this, min, max);
      else {
        char ch = _buffer[_length - 1];

        RegexpNode head = new StringNode(_buffer, _length - 1);

        return head.concat(new CharNode(ch).createLoop(parser, min, max));
      }
    }

    RegexpNode override createLoopUngreedy(Regcomp parser, int min, int max)
    {
      if (_length == 1)
        return new CharUngreedyLoop(this, min, max);
      else {
        char ch = _buffer[_length - 1];

        RegexpNode head = new StringNode(_buffer, _length - 1);

        return head.concat(
            new CharNode(ch).createLoopUngreedy(parser, min, max));
      }
    }

    RegexpNode override createPossessiveLoop(int min, int max)
    {
      if (_length == 1)
        return super.createPossessiveLoop(min, max);
      else {
        char ch = _buffer[_length - 1];

        RegexpNode head = new StringNode(_buffer, _length - 1);

        return head.concat(new CharNode(ch).createPossessiveLoop(min, max));
      }
    }

    //
    // optim functions
    //

    int override minLength()
    {
      return _length;
    }

    int override firstChar()
    {
      if (_length > 0)
        return _buffer[0];
      else
        return -1;
    }

    bool override []firstSet(bool []firstSet)
    {
      if (firstSet != null && _length > 0 && _buffer[0] < firstSet.length) {
        firstSet[_buffer[0]] = true;

        return firstSet;
      }
      else
        return null;
    }

    string override prefix()
    {
      return new String(_buffer, 0, _length);
    }

    //
    // match function
    //

    override int match(StringValue string,
                    int strlen,
                    int offset,
                    RegexpState state)
    {
      if (string.regionMatches(offset, _buffer, 0, _length))
        return offset + _length;
      else
        return -1;
    }

    protected override void ToString(StringBuilder sb, Map<RegexpNode,Integer> map)
    {
      sb.append(ToStringName());
      sb.append("[");
      sb.append(_buffer, 0, _length);
      sb.append("]");
    }
  }

  static class StringIgnoreCase : RegexpNode {
    private char []_buffer;
    private int _length;

    StringIgnoreCase(CharBuffer value)
    {
      _length = value.length();
      _buffer = new char[_length];

      if (_length == 0)
        throw new IllegalStateException("empty string");

      System.arraycopy(value.getBuffer(), 0, _buffer, 0, _buffer.length);
    }

    StringIgnoreCase(char []buffer, int length)
    {
      _length = length;
      _buffer = buffer;

      if (_length == 0)
        throw new IllegalStateException("empty string");
    }

    StringIgnoreCase(char ch)
    {
      _length = 1;
      _buffer = new char[1];
      _buffer[0] = ch;
    }

    RegexpNode override createLoop(Regcomp parser, int min, int max)
    {
      if (_length == 1)
        return new CharLoop(this, min, max);
      else {
        char ch = _buffer[_length - 1];

        RegexpNode head = new StringIgnoreCase(_buffer, _length - 1);
        RegexpNode tail = new StringIgnoreCase(new char[] { ch }, 1);

        return head.concat(tail.createLoop(parser, min, max));
      }
    }

    RegexpNode override createLoopUngreedy(Regcomp parser, int min, int max)
    {
      if (_length == 1)
        return new CharUngreedyLoop(this, min, max);
      else {
        char ch = _buffer[_length - 1];

        RegexpNode head = new StringIgnoreCase(_buffer, _length - 1);
        RegexpNode tail = new StringIgnoreCase(new char[] { ch }, 1);

        return head.concat(tail.createLoopUngreedy(parser, min, max));
      }
    }

    RegexpNode override createPossessiveLoop(int min, int max)
    {
      if (_length == 1)
        return super.createPossessiveLoop(min, max);
      else {
        char ch = _buffer[_length - 1];

        RegexpNode head = new StringIgnoreCase(_buffer, _length - 1);
        RegexpNode tail = new StringIgnoreCase(new char[] { ch }, 1);

        return head.concat(tail.createPossessiveLoop(min, max));
      }
    }

    //
    // optim functions
    //

    int override minLength()
    {
      return _length;
    }

    int override firstChar()
    {
      if (_length > 0
          && (Character.toLowerCase(_buffer[0])
              == Character.toUpperCase(_buffer[0])))
        return _buffer[0];
      else
        return -1;
    }

    bool override []firstSet(bool []firstSet)
    {
      if (_length > 0 && firstSet != null) {
        char lower = Character.toLowerCase(_buffer[0]);
        char upper = Character.toUpperCase(_buffer[0]);

        if (lower < firstSet.length && upper < firstSet.length) {
          firstSet[lower] = true;
          firstSet[upper] = true;

          return firstSet;
        }
      }

      return null;
    }

    string override prefix()
    {
      return new String(_buffer, 0, _length);
    }

    //
    // match function
    //

    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (string.regionMatchesIgnoreCase(offset, _buffer, 0, _length))
        return offset + _length;
      else
        return -1;
    }
  }

  readonly StringBegin STRING_BEGIN = new StringBegin();
  readonly StringEnd STRING_END = new StringEnd();
  readonly StringFirst STRING_FIRST = new StringFirst();
  readonly StringNewline STRING_NEWLINE = new StringNewline();

  private static class StringBegin : RegexpNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset == state._start)
          return offset;
        else
          return -1;
    }
  }

  private static class StringEnd : RegexpNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset == strlen)
          return offset;
        else
          return -1;
    }
  }

  private static class StringFirst : RegexpNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset == state._first)
          return offset;
        else
          return -1;
    }
  }

  private static class StringNewline : RegexpNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if (offset == strlen
          || string.charAt(offset) == '\n' && offset + 1 == string.length())
          return offset;
        else
          return -1;
    }
  }

  readonly Word WORD = new Word();
  readonly NotWord NOT_WORD = new NotWord();

  private static class Word : RegexpNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if ((state._start < offset
           && RegexpSet.WORD.match(string.charAt(offset - 1)))
          != (offset < strlen
              && RegexpSet.WORD.match(string.charAt(offset))))
        return offset;
      else
        return -1;
    }
  }

  private static class NotWord : RegexpNode {
    int override match(StringValue string, int strlen, int offset, RegexpState state)
    {
      if ((state._start < offset
           && RegexpSet.WORD.match(string.charAt(offset - 1)))
          == (offset < strlen
              && RegexpSet.WORD.match(string.charAt(offset))))
        return offset;
      else
        return -1;
    }
  }

  static {
    ANY_CHAR = new AsciiNotSet();
  }
}
}
