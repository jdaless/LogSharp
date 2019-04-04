# LogSharp
Logical Programming in .NET

Still definitely a work in progress, but the goal is to have a .NET Standard library to have Prolog-like functionality. LogSharp will be more formal-logic-y than Prolog though, since instead of defining rules like functions, usage involves defining implications. 

## Setup

Do you have dotnet? [Get it.](https://dotnet.microsoft.com/download)


```
$ git clone https://github.com/jdaless/LogSharp.git
$ cd LogSharp/
$ dotnet build
$ dotnet test
```

A lot of the tests will fail, they're just there to measure progress. 

## Example

Prolog pulled and C# code is part of the unit tests.

### Relationships

From [here](http://www.cs.toronto.edu/~sheila/384/w11/simple-prolog-examples.html) 

#### Prolog In
```prolog
likes(mary,food).
likes(mary,wine).
likes(john,wine).
likes(john,mary).
```
#### Prolog Console
```prolog
?- likes(mary,food). 
true.
?- likes(john,wine). 
true.
?- likes(john,food). 
false.
```
 
#### C# In 
```cs
World w = new World();
Rule likes = new Rule();
w.Add(likes["mary", "food"]);
w.Add(likes["mary", "wine"]);
w.Add(likes["john", "wine"]);
w.Add(likes["john", "mary"]);
Console.WriteLine(w.Query(likes["mary","food"]);
Console.WriteLine(w.Query(likes["john","wine"]);
Console.WriteLine(w.Query(likes["john","food"]);
```
#### C# Console
```
True
True
False
```
### Abraham

From [here](http://cg.huminf.aau.dk/Module_II/1063.html)

#### Prolog in
```prolog
son(ishmael, abraham, mother_is_slave).
son(isaac, abraham, mother_is_free).
patriarch(abraham).
freeborn(P,S) :- patriarch(P), 
                 son(S,P,F),
                 eq(F,mother_is_free).
```

#### Prolog Console
```prolog
?- freeborn(X,Y).
X = abraham,
Y = isaac.
```

#### C# In
```cs
World w = new World();
Rule son = new Rule();
Rule patriarch = new Rule();
Rule freeborn = new Rule();
w.Add(son["ishmael", "abraham", "mother_is_slave"]);
w.Add(son["isaac", "abraham", "mother_is_free"]);
w.Add(patriarch["abraham"]);
using(var p = new Variable())
using(var s = new Variable())
using(var f = new Variable())
{
    w.Add(freeborn[p,s] < 
        patriarch[p] ^ 
        son[s,p,f] ^ 
        Rule.Equality[f, "mother_is_free"]);
}
using(var x = new Variable())
using(var y = new Variable())
{
    w.Query(freeborn[x,y]);
    Console.WriteLine(x.OfType<string>().Single());
    Console.WriteLine(y.OfType<string>().Single());
}
```
#### C# Console
```cs
abraham
isaac
```