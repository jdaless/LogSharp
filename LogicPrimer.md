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

### Implication
You probably haven't seen this one before, it's asymmetric! It's logically equivalent to `(¬A) v B`, and is used to represent conditions. For example, we may want to state that *if* it is night, *then* the sky is black. The sky is black during an eclipse, but the statement is still true. That means if we know it's night time, then we know the sky is black. If the sky isn't black, then we know that it isn't night time. 
* A ⇒ B
* A ⊃ B
* A => B
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
