# [2025 - Day 01: Secret Entrance](https://adventofcode.com/2025/day/1)

## Part 1
This one was easy, the first step was to parse the input.
This was done by reading each line and checking the first
character and parsing the rest as an Integer.  
If the first character is an `L`, take the negative of the number,
and if it's an `R` take the number as is.  
The next part was to go through each entry and add it 
to the current rotation, taking it `mod 100`.  
If the current rotation is `0`, increment a counter.  
At the end, the counter is our result.

## Part 2
This one should have been easy, however I was stuck at first
when using the same system as before
(first the answer was too low, then too high).

This meant the simplest solution was still available:
instead of directly adding the value of the required
rotation, just increment the rotation one by one in
the direction given by the input line.  
For each step take it `mod 100`, if the rotation is `0`,
increment the counter.
At the end, the counter is our result.