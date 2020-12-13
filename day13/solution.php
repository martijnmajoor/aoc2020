<?php
// TIL about the Chinese Remainder Theorem
// https://www.reddit.com/r/adventofcode/comments/kc60ri/2020_day_13_can_anyone_give_me_a_hint_for_part_2/gfnnfm3/

$f = "example.txt";

echo "part one: ".partOne($f);
echo "\npart two: ".partTwo($f);

function partOne($file) {
    $lines = file($file);
    $buses = array_flip(
        array_filter(
            explode(',', $lines[1]),
            function($line) {
                return $line != 'x';
            }
        )
    );
    array_walk(
        $buses,
        function(&$departure, $id) use ($lines) {
            $departure = $id - ($lines[0] % $id);
        }
    );
    asort($buses);
 
    $id = array_key_first($buses);
    return $id * $buses[$id];
}
function partTwo($file) {
    $lines = file($file);
    $buses = explode(',', $lines[1]);
    $t = 0;
    $step = $buses[0];
    
    for($i = 1; $i < count($buses); $i++) {
        if($buses[$i] == 'x') {
            continue;
        }

        $mul = 1;
        while(true) {
            $passed = $step * $mul;
            $next = $t + ($step * $mul) + $i;
            
            if($next % $buses[$i] == 0) {
                $t += $passed;
                break;
            }
            
            $mul++;
        }

        $step = $step * $buses[$i];
    }
    
    return $t;
}