<?php
$lines = file("example.txt");

echo "part one: ".array_sum(array_map(function($line) { return calc($line); }, $lines));
echo "\npart two: ".array_sum(array_map(function($line) { return calc($line, true); }, $lines));

function calc($problem, $advanced = false) {
    while(str_contains($problem, '(')) {
        $problem = preg_replace_callback(
            "/\(([^()]+)\)/",
            function($matches) use($advanced) {
                return applyRules($matches[1], $advanced);
            },
            $problem
        );
    }
    return applyRules($problem, $advanced);
}
function applyRules($problem, $advanced) {
    return $advanced ? precedence($problem) : equal($problem);
}

function precedence($problem) {
    return eval('return '.preg_replace("/(\d+ \+ \d+( \+ \d+){0,})/", "($1)", $problem).';');
}
function equal($problem) {
    $count = 0;
    $repl = preg_replace("/(\d+)/", "$1)", $problem, -1, $count);
    return eval('return '.str_repeat("(", $count).$repl.';');
}