# Compute Shader Examples

Compute shader is an extremely useful tool for computationally heavy, repetative tasks.


In this prototype I have made an example to see how efficent it is at doing a very large calculation compared to the regular CPU processing. 

The table below shows the results of my experimentation,
Note that I removed the automation method from the repository as I want to use the "SuperEasy..." files as a springboard for my future projects.

| Iteration Object Count | Thread Pool Size (size * 8) | Time CPU | Time GPU |
|:-----------------------|:----------------------------:|:--------:|---------:|
|100.000.000|256|00:00:15.87|00:00:00.85|
|50.000.000|256|00:00:07.88|00:00:00.41|
|25.000.000|256|00:00:03.93|00:00:00.20|
|10.000.000|256|00:00:01.57|00:00:00.12|
|1.000.000|256|00:00:07.88|00:00:00.41|
|500.000|256|00:00:00.08|00:00:00.01| 
|100.000|256|00:00:00.01|~00:00:00.00| 
|50.000|256|80.995 ticks|33.134 ticks| 
|25.000|256|46.137 ticks|28.633 ticks| 
|18.000|256|37.367 ticks|25.984 ticks| 
|10.000|256|18.324 ticks|30.693 ticks| 
|1000|256|3282 ticks|31.191 ticks| 
|10|256|1306 ticks|20.707 ticks| 


Due to the volatility of Tick values, the numbers are avaraged.


I haven't tested extensively interms of thread pool size but from what I can tell; while higher thread pool size does speed up the GPU Time, the effect is very minimal on lower iteration object counts and detrimental at very low object counts.