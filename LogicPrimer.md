# Quick and Dirty Guide to Formal Logic For Programmers

## Atoms/Predicates
Atoms are the simplest statements possible. For example, if `P` represents *Jan 1 1970 is a Thursday*, we would say that `P` is **true**.

Predicates give us a little more power, since they have arguments like functions do. For example, we could say that `T(x)` represents *x is a Thursday*. If `a` is *Jan 1 1970* we would say that `T(a)` is **true**.

The arity of a predicate is how many arguments it takes. Unary predicates take one argument, like `T(x)` does. These are usually adjectives that apply to an object or not, or groups that may or may not contain the object. Binary predicates represent relations. For example, let's say `B(x,y)` represents *x is bigger than y*. This predicate can't be expressed in any number of unary predicates, we need the arity of 2. Higher arities work the same way. 

## Connectives
This part should be familiar, connectives are just boolean logic. Each one is represented by a truth table. 
### Conjunction
Conjunction is true only when both conjuncts are true. 
* A & B
* A ^ B
* A * B
* A and B
<table>
  <tr>
    <td colspan=2 rowspan=2></td>
    <td colspan="2">B</td>
  </tr>
  <tr>
    <td>T</td>
    <td>F</td>
  </tr>
  <tr>
    <td rowspan="2">A</td>
    <td>T</td>
    <td>T</td>
    <td>F</td>
  </tr>
  <tr>
    <td>F</td>
    <td>F</td>
    <td>F</td>
  </tr>
</table>

### Disjunction
Disjunction is true if either disjunct is true. 
* A | B
* A v B
* A + B
* A or B
<table>
  <tr>
    <td colspan=2 rowspan=2></td>
    <td colspan="2">B</td>
  </tr>
  <tr>
    <td>T</td>
    <td>F</td>
  </tr>
  <tr>
    <td rowspan="2">A</td>
    <td>T</td>
    <td>T</td>
    <td>T</td>
  </tr>
  <tr>
    <td>F</td>
    <td>T</td>
    <td>F</td>
  </tr>
</table>

### Negation
Negation is the opposite of its argument. 
* !A
* ¬A
* -A
* ~A
* not A
<table>
  <tr>
    <td colspan="2">A</td>
  </tr>
  <tr>
    <td>T</td>
    <td>F</td>
  </tr>
  <tr>
    <td>F</td>
    <td>T</td>
  </tr>
</table>

### Material Implication
You probably haven't seen this one before, it's asymmetric! It's logically equivalent to `(¬A) v B`, and is used to represent conditions. For example, we may want to state that *if* it is night, *then* the sky is black. The sky is black during an eclipse, but the statement is still true. That means if we know it's night time, then we know the sky is black. If the sky isn't black, then we know that it isn't night time.
* A ⇒ B
* A ⊃ B
* A => B
* A implies B
<table>
  <tr>
    <td colspan=2 rowspan=2></td>
    <td colspan="2">B</td>
  </tr>
  <tr>
    <td>T</td>
    <td>F</td>
  </tr>
  <tr>
    <td rowspan="2">A</td>
    <td>T</td>
    <td>T</td>
    <td>F</td>
  </tr>
  <tr>
    <td>F</td>
    <td>T</td>
    <td>T</td>
  </tr>
</table>

A logical implication is a material implication that is true no matter how the variables in the statement are satisfied. The logical implication `P ^ Q => P` is always true, but the material implication `Q => P` is only true when it satisies the above truth table. The symbols are interchangeable which is confusing. 

### Material Equivalence
This one is self explanatory, it represents a two way material implication. `A <=> B` is logically equivalent to `(A => B) ^ (B => A)`. A logical equivalence is a two way logical implication in the same way!
* A ⇔ B
* A ≡ B
* A <=> B
* A iff B
<table>
  <tr>
    <td colspan=2 rowspan=2></td>
    <td colspan="2">B</td>
  </tr>
  <tr>
    <td>T</td>
    <td>F</td>
  </tr>
  <tr>
    <td rowspan="2">A</td>
    <td>T</td>
    <td>T</td>
    <td>F</td>
  </tr>
  <tr>
    <td>F</td>
    <td>F</td>
    <td>T</td>
  </tr>
</table>

## Arguments
The real power of formal logic comes from chaining together statements to form arguments. Arguments are cool because if you accept the premises you have to accept the conclusion. For example:
```
p1  Socrates is a man
p2  All men are mortal
------------------------
∴   Socrates is mortal
```
is the classic example. You can't argue with the conclusion itself, you must attack the premises. This is because the argument is *valid*: the *conjunction* of the premises *implies* the conclusion, literally
```
(p1 ^ p2) => ∴
```
A valid argument is *sound* when the premises are accepted to be true. 

### Proofs
The neat part of this is proving validity. There are certain rules for doing proofs, just like in arithmetic, to show that an argument is valid. For example, the associative property works the same for conjunction and disjunction like for addition and multiplication. 
```
(p ^ q) ^ r <=> p ^ (q ^ r)
(p v q) v r <=> p v (q v r)
(p ^ q) v r </> p ^ (q v r)
```
You can construct truth tables to see that these are true for any `p`, `q`, or `r`.

These are called *rules of replacement* since you can replace one statement with the other. There are also *rules of inference* that can only be replaced one way. For example
```
p ^ q => p
```
You can *infer* `p` from `p ^ q` but not the other way around. Disjunction works the opposite way:
```
p => p v q
```

## LogSharp

The `Fact`s and `Rule`s added to a `World` represent premises and queries represent possible conclusions. A query is true iff it is a consequence of the premises. This is where the power of logical programming comes in, it is all state and no functions. There are no moving parts, but state can force other state. For example, [this](https://www.cpp.edu/~jrfisher/www/prolog_tutorial/2_8.html) prolog example shows a complete change calculator that only uses *one rule* added to the world. 

LogSharp is an attempt to bring this power into a context where it can be used. 