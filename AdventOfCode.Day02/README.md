# [2025 - Day 02: Gift Shop](https://adventofcode.com/2025/day/2)

## Part 1
After reading the input, I thought of a smart way to try and 
solve it, however bruteforce wins in the end...  
In the end the easiest solution was to loop through each number
and check if it is a valid one by taking `10` to the power
of half the number of digits. And then checking if the
integer division and mod give the same result.  
If it is, add it to the list, and sum the result to get
the final answer.


## Part 2
By using the same bruteforce technique as before, loop through
each number.  
Then check for each possible division of the number of digits
if the first part repeated `n` times is equal to the complete
number.  
If it is, add it to the list, and sum the result to get
the final answer.