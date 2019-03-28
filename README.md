# LogSharp
Logical Programming in .NET

Still definitely a work in progress, but the goal is to have a .NET Standard library to have Prolog-like functionality.

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

Prolog pulled from [here](http://www.cs.toronto.edu/~sheila/384/w11/simple-prolog-examples.html) and C# code is part of the unit tests.

### Relationships

#### Prolog In
```prolog
likes(mary,food).
likes(mary,wine).
likes(john,wine).
likes(john,mary).
```
#### Prolog Console
```prolog
| ?- likes(mary,food). 
 yes.
| ?- likes(john,wine). 
 yes.
| ?- likes(john,food). 
 no.
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
