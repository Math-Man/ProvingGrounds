# Math Grapher

A small script set for drawing mathematical graphs with different rendering methods.


The functions follow classic declaration convention where every item of the list corresponds to a power of input (x)
example: {1, 2, 3} y = 1*x^0 + 2*x^1 + 3*x^3 ...


The static function allows for writing a conventional function with time scale, this is located under GraphBuilder.cs MathFunction:Value() line 79
as default its a simple sine function var "sm = Mathf.Sin(Mathf.PI * (x/resolution + (t)));"


the grapher.cs is an old implementation I decided to keep.


[![IMAGE ALT TEXT HERE](https://github.com/Math-Man/UnityProvingGrounds/blob/main/Assets/Prototypes/MathGraph/gifs/a1.gif)](https://github.com/Math-Man/UnityProvingGrounds/blob/main/Assets/Prototypes/MathGraph/gifs/a1.gif)
