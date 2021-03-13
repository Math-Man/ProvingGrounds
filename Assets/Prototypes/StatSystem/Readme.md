This is a multifaceted number cruncher and a very basic stats system combined.

This is something I am currently using in my my projects where I have to keep a set of static numbers that should be easy to change and adjust.


[![IMAGE ALT TEXT HERE](/Images/Interface.png)](/Images/Interface.png)

Through code several different modifiers of a stat can be modified these are: Base Value, Percentage Increase, Percantage Multiplier, Flat added, and Flat Multiplier.

Mathematical function used to calculate the individual stats is as follows:

(((Base) * Increase) + (FlatAdded * FlatMultiplier)) * Multiplier

Which is then turnacated depending on the min/max value.

Through this dynamic grid interface any value can be added and manipulated through the client per object basis.

Albeit not all objects are necceserily need the same set of stats calculation is solely dependant on object references.
