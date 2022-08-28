RemesPath documentation
========================

RemesPath is a JSON query language inspired by [JMESpath](https://jmespath.org/) with such useful features as
* indexing in objects with both dot syntax and square bracket syntax
* boolean indexing
* vectorized arithmetic
* many vectorized functions
* [regular expression](https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference) functions
* recursive search for keys
* SQL-like group_by capabilities

The formal description of the language in pseudo-Backus-Naur form is in ["RemesPath language spec.txt"](/docs/RemesPath%20language%20spec.txt). I'm not 100% sure this is a formally valid language spec, though.

## Indexing and selecting keys ##

1. `@` selects the entirety of an object or array.
2. [Python-style slices](https://stackoverflow.com/questions/509211/understanding-slicing) and indices can be used to select parts of an array. For example:
    * `@[1]` selects the second element
    * `@[-4]` selects the fourth-to-last element
    * `@[:3]` selects the first, second and third elements
    * `@[5::2]` selects every other element of the array starting with the fifth.
3. You can select multiple slices and indices in the same square brackets!
    * `@[1, 5:8, -1]` selects the second, sixth, seventh, eight, and last elements.
4. Dot syntax and square brackets are both valid ways to select keys of an object.
    * `@.bar` and `@[bar]` both select the value corresponding to a single key `bar`.
    * You can select multiple keys from a single object by enclosing all of them in square brackets. You *cannot* follow a dot with square brackets.
        * So `@[foo, bar, baz]` gets the values associated with keys `foo`, `bar`, and `baz`.
    * Backticks (\`) can be used to enquote strings. Thus ``@.`bar` ``and ``@[`bar`]`` are equivalent to `@.bar` and `@[bar]`.
    * The literal backtick character \` can be rendered by an escaped backtick \\\` inside a backtick-enclosed string.
    * Any string that does not *begin* with an underscore or an ASCII letter and *contain* only underscores, ASCII letters, and digits *must* be enclosed in backticks.
        * So `@.a12`, `@._`, `@.a_1`, but ``@[`1_a`]``.
5. Each time you select an index or key, the next index selected from the corresponding value(s).
    1. Consider the array `[[1, 2], [3, 4], [5, 6]]`.
        * `@[:2][1]` selects the second element of each of the first and second elements of that array. So `@[:2][1]` will return `[2, 4]`.
    2. Consider the array of objects `[{"a": 1, "b": ["_"]}, {"a": 2, "b": ["?"]}]`.
        * `@[:].b[0]` will return the first value of the array child of key `b` in each object, so `["_", "?"]`.
        * `@[0][b, a]` will return keys `{"a": 1, "b": ["_"]}`. 
            * *Note that the order of keys in the index is not preserved because objects are inherently unordered.*
6. If every indexer in a chain of indexers returns only one index/key, the query will not return an array or object containing the result; it will only return the result.
    1. Consider again the array `[[1, 2], [3, 4], [5, 6]]`.
        * Query `@[0][1]` returns `2`.
    2. Consider again the array of objects `[{"a": 1, "b": ["_"]}, {"a": 2, "b": ["?"]}]`.
        * Query `@[1].b` returns `["?"]`.
        * Query `@[0].b[0]` returns `"_"`.

## Vectorized operations ##

1. Many operations are *vectorized* in RemesPath. That is, they are applied to every element in an iterable.
    1. Consider the array `[1, 2, 3]`.
        * `2 * @` returns `[2, 4, 6]`.
        * `str(@)` returns `["1", "2", "3"]` because `str` is a vectorized function for converting things to their string representations.
        * @ + @ / 2 returns `[1.5, 3.0, 4.5]`.
        * `@ > @[1]` returns `[false, false, true]`.
    2. Consider the object `{"a": 1, "b": 2, "c": 3}`.
        * `@ ** @` returns `{"a": 1.0, "b": 4.0, "c": 27.0}`
        * `@ & 1` returns `{"a": 1, "b": 0, "c": 1}`.
        * `@ > @.a` returns `{"a": false, "b": true, "c": true}`.

## Binary operators and arithmetic ##

All binary operators in RemesPath are vectorized over iterables.

The binary operators in RemesPath are as follows:

Symbol | Operator                                      | Precedence | Return type
-------|-----------------------------------------------|------------|------------
 `&`   |  bitwise/logical `AND`                        |  0         | int/bool
 `\|`   |  bitwise/logical `OR`                         |  0         | int/bool
 `^`   |  bitwise/logical `XOR`                        |  0         | int/bool
 `=~`  |  value in array/object matches regex          |  1         | bool
 `==`, `!=`, `<`, `>`, `<=`, `>=` | the usual comparison operators | 1 | bool
 `+`   | Addition of numbers, concatenation of strings   |  2         | int/float/string
 `-`   | subtraction                                   |  2         | int/float
 `//`  | floor division                                |  3         | int
 `%`   | modulo                                        |  3         | int/float
 `*`   | multiplication                                |  3         | int/float
 `/`   | division                                      |  3         | int/float
 `**`  | exponentiation                                |  5         | float

As in normal math, the unary minus operator (e.g., `-5`) has lower precedence than exponentation.

All binary operators are [left-associative](https://en.wikipedia.org/wiki/Operator_associativity) (evaluated left-to-right when precedence is tied), except exponentation (`**`), which is right-associative.


## Regular expressions and JSON literals ##

A regular expression can be created in a RemesPath expression by prefixing a \`\` string with the character "g". So ``g`\\s+` ``is the regular expression "\\s+", i.e., at least one whitespace character.

A JSON literal can be created inside a RemesPath expression by prefixing a \`\` string with the character "j". So ``j`[1, 2, 3]` ``creates the JSON array `[1, 2, 3]`.

### Regular expression indexing in objects ###

You can select all the keys of an object that match a regular expression by using a regular expression with the dot or square bracket syntax.

**Examples:**
* Consider the object `{"foo": 1, "bar": 2, "baz": 3}`.
* ``@.g`^b` ``returns `{"bar": 2, "baz": 3}`.
* ``@[g`r$`, foo]`` returns `{"bar": 2, "foo": 1}`.

## Boolean indexing ##

You can select all elements in an iterable that satisfy a condition by applying a boolean index.

A boolean index can be one of the following:
* A single boolean. If it's `false`, an empty array is returned (*it's always an array, even for one-element boolean indices on objects*). If it's true, the whole iterable is returned.
    * Consider the array `[1, 2, 3]`
    * e.g., `@[in(2, @)]` will return `[1, 2, 3]` because `in(2, @)` is `true`.
    * `@[in(4, @)]` will return `[]` because `in(4, @)` is `false`.
* *If the iterable is an **array**, an array of booleans of the same length as the iterable*. An array with all the values for which the boolean index was `true` will be returned.
    * Consider the array `[1, 2, 3]`
    * `@[@ > @[0]]` will return `[2, 3]`.
    * `@[@ ** 2 < 1]` will return `[]`.
    * `@[@[:2] > 0]` will throw a VectorizedArithmeticException, because the boolean index has length 2 and the array has length 3. 
* *If the iterable is an **object**, an object of booleans with exactly the same keys as the iterable.* An object will be returned with all the pairs k: v for which the boolean index's value corresponding to k was `true`.
    * Consider the object `{"a": 1, "b": 2, "c": 3}`
    * `@[@ > @.a]` returns `{"b": 2, "c": 3}`.
    * `@[@[a,b] > 1]` will throw a VectorizedArithmeticException, because the boolean index is `{"a": false, "b": true}`, which does not have exactly the same keys as the object.

## Grouping parentheses ##

Grouping parentheses work exactly the way you expect them to with arithmetic expressions.
* `2 ** 3 / (4 - 5)` evaluates to `8/-1` and thus returns `-8.0`.

Grouping parentheses can also be used to make the query parser treat a single expression as atomic.
* Consider the object `[{"a": [1, 2, 3]}]`.
* The query `@[:].a[@ > @[0]]` returns `[[2, 3]]`. In pseudo-code, this would be:
```
make an array arr
for each object obj in this
    make a subarray subarr
    for each element in obj[a]
       if element > obj[a][0], add element to subarr
    add subarr to arr
return arr
```
* However, we *can't* select the first element of each array by just making the query `@[:].a[@ > @[0]][0]`. This will throw an error.
* That's because the query has already descended to the level of individual elements, and we can't index on the individual elements.
* Instead, we enclose the original query in grouping parentheses: `(@[:].a[@ > @[0]])`.
* Now we can select the first element of each array as follows: `(@[:].a[@ > @[0]])[:][0]`.

## Recursive search ##
* Suppose you have really deep JSON, but all you really want is a certain key in an object.
* For example, consider the JSON `[[[{"a": 1, "b": 2}], [{"a": 3, "b": 4}]]]`.
* You can recursively search for the key "a" in this JSON with *double-dot* syntax `@..a`. This will return `[1, 3]`.
* You can also recursively search for the keys "b" and "a" with the query `@..[b, a]`. This will return `[2, 1, 4, 3]`.

## Functions ##
RemesPath supports a variety of functions, some of which are vectorized and some of which are not.

We'll present the non-vectorized functions separately from the vectorized ones to avoid confusion.

Each subset will be organized in alphabetical order.

### Non-vectorized functions ###

`avg(x: array) -> float`

Finds the arithmetic mean of an array of numbers. `mean` is an alias for this function.

-----
`dict(x: array) -> object`

If x is an array of 2-element subarrays where the first element in each subarray is a string, return an object where each subarray is converted to a key-value pair.

**Example:**
* `dict([["a", 1], ["b", 2]])` returns `{"a": 1, "b": 2}`.

----
`flatten(x: array, depth: int = 1]) -> array`
   
Recursively searches in `x` down to a depth of `depth`, pulling each element of every sub-array at that depth into the final array.

It's easier to understand with some **examples:**
* `flatten([[1, 2], [3, 4]])` returns `[1, 2, 3, 4]`.
* `flatten([1, 2, 3])` returns `[1, 2, 3]`.
* `flatten([1, [2, [3]]])` returns `[1, 2, [3]]`.
* `flatten([1, [2, [3, [4]]]], 3)` returns `[1, 2, 3, 4]`.

----
`group_by(x: array, k: int | str) -> object`

* If `x` is an array of *arrays*:
   * If `k` is *not* an int, throw an error.
   * Return an object where key `str(v)` has an array of sub-arrays `subarr` such that `subarr[k] == v` is `true`.
   * Note that `subarr[k]` might not be a string in these sub-arrays. However, keys in JSON objects must be strings, so the key is the string representation of `subarr[k]` rather than `subarr[k]` itself.
* If `x` is an array of *objects*:
   * If `k` is *not* a string, throw an error.
   * Return an object where key `str(v)` has an array of sub-objects `subobj` such that `subobj[k] == v` is `true`.
   * Note that `subobj[k]` might not be a string in these sub-objects.
* If `x` is an array of anything else, or it has a mixture of arrays an objects, throw an error.

----
`in(elt: anything, itbl: object | array) -> bool`

* If `itbl` is an *array*:
    * if `elt` has a type that is not comparable with an element of `itbl`, throws an error. 
    * returns `true` if `elt` is equal to any element.
* If `itbl` is an *object*:
    * if `elt` is not a string, throws an error.
    * If `elt` is one of the keys in `itbl`, returns `true`.

----
`index(x: array, elt: anything, reverse: bool = false) -> int`

* If `reverse` is `false` (*default*): Returns the index of the *first* element in `x` that is equal to `elt`.
* If `reverse` is `true`: Returns the index of the *last* element in `x` that is equal to `elt`.
* If *no elements in x are equal to elt*, throws an error.

----
`items(x: object) -> array`

Returns an array of 2-item subarrays (the key-value pairs of `x`).

Because objects are not inherently ordered, you may need to sort the key-value pairs by their key or value to get the same result every time.

----
`keys(x: object) -> array`

Returns an array of the keys in `x`.

----
`len(x: object | array) -> int`

Returns the number of key-value pairs in `x` (if an object) or the number of elements in `x` (if an array).

----
`max(x: array) -> float`

Returns a floating-point number equal to the maximum value in an array.

----
`max_by(x: array, k: int | str) -> array | object`

* If `x` is an array of *arrays*:
   * If `k` is not an int, throw an error.
   * Return the subarray `maxarr` such that `maxarr[k] >= subarr[k]` for all sub-arrays `subarr` in `x`.
* If `x` is an array of *objects*:
    * If `k` is not a string, throw an error.
   * Return the subobject `maxobj` such that `maxobj[k] >= subobj[k]` for all sub-objects `subobj` in `x`.

----
`min(x: array) -> float`

Returns a floating-point number equal to the minimum value in an array.

----
`min_by(x: array, k: int | str) -> array | object`

See `max_by`.

----
`quantile(x: array, q: float) -> float`

x must contain only numbers.

q must be between 0 and 1, exclusive.

Returns the q^th quantile of `x`, as a floating-point number.

So `quantile(x, 0.5)` returns the median, `quantile(x, 0.75)` returns the 75th percentile, and so on.

Uses linear interpolation if the index found is not an integer.

For example, suppose that the 60th percentile is at index 6.6, and elements 6 and 7 are 8 and 10.

Then the returned value is `0.6*10 + 0.4*8`, or 9.2.

----
`range(start: int, end: int = null, step: int = 1) -> array[int]`

Returns an array of integers.

* If `end` and `step` are not supplied, return all the integers from 0 to start, excluding start.
   * So `range(3)` returns `[0, 1, 2]`
   * `range(-1)` returns `[]` because -1 is less than 0.
* If `step` is not supplied, return all the integers from `start` to `end`, excluding `end`.
   * `range(3, 5)` returns `[3, 4]`.
   * `range(3, 1)` returns `[]` because 1 is less than 3.
* If all arguments are supplied, return all the integers from `start` to `end`, incrementing by `step` each time.
   * `range(3, 1, -1)` returns `[3, 2]`.
   * `range(0, 6, 3)` returns `[0, 3]`.

----
`s_join(sep: string, x: array) -> string`

Every element of `x` must be a string.

Returns x string-joined with sep (i.e., returns a string that begins with `x[0]` and has `sep` between `x[i - 1]` and `x[i]` for `1 <= i <= len(x)`)

----
`sort_by(x: array, k: string | int, descending: bool = false)`

`x` must be an array of arrays (in which case `k` must be an int) or an array of objects (in which case `k` must be a string).

Returns a new array of subarrays/subobjects `subitbl` such that `subitbl[k]` is sorted ascending.

Analogous to SQL `ORDER BY`.

By default, these sub-iterables are sorted ascending. If `descending` is `true`, they will instead be sorted descending.

----
`sorted(x: array, descending: bool = false)`

`x` must be an array of all strings or all numbers. Either is fine so long as all elements are comparable.

Returns a new array where the elements are sorted ascending. If `descending` is `true`, they're instead sorted descending.

----
`sum(x: array) -> float`

Returns the sum of the elements in x. 

x must contain only numbers. Booleans are fine.

----
`unique(x: array, sorted: bool = false)`

Returns an array of all the unique elements in `x`. 

If `sorted` is true, sorts the array ascending. This will raise an error if not all of x's elements are comparable.

----
`value_counts(x: array)`

Returns an array of two-element subarrays `[k: anything, count: int]` where `count` is the number of elements in `x` equal to `k`.

The order of the sub-arrays is random.

----
`zip(x1: array, x2: array [, x3: array, x4: array, x5: array, x6: array]) -> array`

Because of unfortunate limitations in the C# implementation of RemesPath, there can only be at most *6* arguments to this function.

Returns a new array in which each `i^th` element is an array containing the `i^th` elements of each argument, in the order in which they were passed.

All the argument arrays *must have the same length*.

In other words, it's like the Python `zip` function.

**Example:**
* `zip(["a", "b", "c"], [1, 2, 3])` returns `[["a", 1], ["b", 2], ["c", 3]]`.

### Vectorized functions ###

All of these functions are applied separately to each element in an array or value in an object.

All the vectorized string functions have names beginning with `s_`.

----
`abs(x: number) -> number`

Returns the absolute value of x.

----
`float(x: number) -> number`

Returns a 64-bit floating-point number equal to x.

----
`ifelse(cond: bool, if_true: anything, if_false: anything) -> anything`

Returns `if_true` if `cond` is `true`, otherwise returns `if_false`.

----
`int(x: number) -> int`

* If x is a boolean or integer: returns a 64-bit integer equal to x.
* If x is a float: returns the closest 64-bit integer to x.
   * Note that this is *NOT* the same as the Python `int` function.

----
`is_expr(x: anything) -> bool`

Returns true if x is an array or object.

----
`is_num(x: anything) -> bool`

Returns true if x is a number.

----
`is_str(x: anything) -> bool`

Returns true if x is a string.

----
`isna(x: number) -> bool`

Returns true if x is the floating-point Not-A-Number (represented in some JSON by `NaN`).

Recall that `NaN` is *NOT* in the original JOSN specification.

----
`log(x: number, n: number = e) -> number`

Returns the log base `n` of `x`. If `n` is not specified, returns the natural log of `x`.

----
`log2(x: number) -> number`

Returns the log base 2 of x.

----
`not(x: bool) -> bool`

Logical `NOT`.

----
`round(x: number, sigfigs: int = 0) -> float | int)`

`x` must be an integer or a floating-point number, *not* a boolean.

* If sigfigs is 0: Returns the closest 64-bit integer to `x`.
* If sigfigs > 0: Returns the closest 64-bit floating-point number to `x` rounded to `sigfigs` decimal places.

----
`s_count(x: string, sub: regex | string) -> int`

Returns the number of times substring/regex `sub` occurs in `x`.

----
`s_find(x: string, sub: regex | string) -> array[string]`

Returns an array of all the substrings in `x` that match `sub`.

----
`s_len(x: string) -> int`

The length of string x.

----
`s_lower(x: string) -> string`

The lower-case form of x.

----
`s_mul(x: string, reps: int) -> string`

A string containing `x` repeated `reps` times. E.g., ``s_mul(`abc`, 3)`` returns `"abcabcabc"`.

Basically `x * reps` in Python, except that the binary operator `*` doesn't have that capability in RemesPath.

----
`s_slice(x: string, sli: slice | int) -> string`

`sli` can be an integer or slice with the same Python slice syntax used to index arrays (see above).

Returns the appropriate slice/index of `x`.

----
`s_split(x: string, sep: regex | string) -> array[string]`

If `sep` is a string:
* Returns an array containing all the substrings of `x` that don't contain `sep`, split by the places where `sep` occurs.
   * E.g., ``s_split(`abac`, `a`)`` returns `["", "b", "c"]`

If `sep` is a regex:
* Returns an array containing substrings of `x` where the parts that match `sep` are missing.
   * E.g., ``s_split(`a big bad man`, g`\\s+`)`` returns `["a", "big", "bad", "man"]`.
* However, if `sep` contains any capture groups, the capture groups are included in the array.
   * ``s_split(`a big bad man`, g`(\\s+)`)`` returns `["a", " " "big", " ", "bad", " ", "man"]`.
   * ``s_split(`bob num: 111-222-3333, carol num: 123-456-7890`, g`(\\d{3})-(\\d{3}-\\d{4})`)`` returns `["bob num: ", "111", "222-3333", ", carol num: ", "123", "456-7890", ""]`

----
`s_strip(x: string) -> string`

Strips the whitespace off both ends of x.

----
`s_sub(x: string, to_replace: regex | string, replacement: string) -> string`

Replaces all instances of string/regex `to_replace` in `x` with `replacement`.

See the [C# regular expressions reference on substitions](https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference#substitutions).

----
`s_upper(x: string) -> string`

Returns the upper-case form of x.

----
`str(x: anything) -> string`

Returns the string representation of x.