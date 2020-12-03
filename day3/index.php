<?php
echo "part one: ".partOne();
echo "\npart two: ".partTwo();

function partOne() {
    return slopes(3, 1);
}
function partTwo() {
    return slopes(1, 1) * slopes(3, 1) * slopes(5, 1) * slopes(7, 1) * slopes(1, 2);
}
function slopes($left, $down) {
    $lines = file("input.txt");
    $trees = 0;
    for($idx = $down; $idx < count($lines); $idx += $down) {
        $line = $lines[$idx];

        if(substr($line, ($idx * $left) % strlen(trim($line)), 1) == "#") {
            $trees++;
        }
    }
    return $trees;
}